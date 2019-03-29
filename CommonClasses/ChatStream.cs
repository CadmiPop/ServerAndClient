using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
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

        public void BeginReadMessage(Action<Message> messageReceived, Action<Exception> onReadError)
        {
            BeginReadMessageLength(length =>
            {
                var buffer = new byte[length];
                BeginReadMessageChunk(messageReceived, onReadError, buffer,0);

            }, onReadError);

        }

        private void BeginReadMessageChunk(Action<Message> messageReceived, Action<Exception> onReadError, byte[] buffer, int offset)
        {
            stream.BeginRead(buffer, offset, buffer.Length- offset, r =>
            {
                try
                {
                    var totalRead = stream.EndRead(r) + offset;
                    if (totalRead == buffer.Length)
                    {
                        messageReceived(new Message(buffer));
                    }
                    else
                    {
                        BeginReadMessageChunk(messageReceived, onReadError, buffer, totalRead);
                    }                 
                }
                catch (Exception e)
                {                   
                    onReadError(e);
                }
            }, null);
        }

        private void BeginReadMessageLength(Action<int> onLengthReceived, Action<Exception> onReadError)
        {
            var buffer = new byte[1];
            stream.BeginRead(buffer, 0, 1, r =>
            {
                try
                {
                    var read = stream.EndRead(r);
                    onLengthReceived(buffer[0]);
                }
                catch (Exception e)
                {
                    onReadError?.Invoke(e);
                }
            }, null);
        }


        public void Send(Message message, Action messageSent, Action<Exception> onSendError)
        {
            var length = BitConverter.GetBytes(message.ToByteArray().Length);
            stream.BeginWrite(length, 0,1, r =>
            {
                try
                {
                    stream.EndWrite(r);
                    SendMessage(message, messageSent, onSendError);
                }
                catch (Exception e)
                {
                    onSendError(e);
                    Console.WriteLine("Server Disconnected");
                    return;
                }
            },null);           
        }

        private void SendMessage(Message message, Action messageSent, Action<Exception> onSendError)
        {
            var buffer = message.ToByteArray();
            stream.BeginWrite(buffer, 0, buffer.Length, r =>
            {
                try
                {
                    stream.EndWrite(r);
                    messageSent?.Invoke();
                }
                catch (Exception e)
                {
                    onSendError(e);                    
                }
            }, null);
        }

        public void Close()
        {
            stream.Flush();
            stream.Dispose();
        }
    }
}
