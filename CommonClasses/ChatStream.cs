using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace CommonClasses
{
    public class ChatStream
    {
        private NetworkStream stream;

        public ChatStream(NetworkStream stream)
        {
            this.stream = stream;
        }

        public Message Message()
        {
            var length = ReadMessageLength();
            var buffer = new byte[length];
            var read = 0;
            while (read != length)
            {
                read += stream.Read(buffer, read, buffer.Length - read);
            }
            return new Message(buffer);
        }

        private int ReadMessageLength()
        {
            var buffer = new byte[1];
            stream.Read(buffer, 0, 1);
            return buffer[0];
        }

        public void Send(Message message)
        {
            try
            {
                stream.Write(BitConverter.GetBytes(message.ToByteArray().Length), 0, 1);
                stream.Write(message.ToByteArray(), 0, message.ToString().Length);
                stream.Flush();
            }
            catch (IOException)
            {
                Console.WriteLine("Server Disconnected");
                return;
            }
        }

        public void Close()
        {
            stream.Flush();
            stream.Close();
        }
    }
}
