using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    public class Protocol
    {
        private NetworkStream stream;
        private byte[] buffer;
        private int length;
        private string message;

        public Protocol(NetworkStream stream)
        {
            this.stream = stream;
            buffer = new byte[1];           
        }

        public string Manipulation()
        {
            MemoryStream ms = new MemoryStream();
            stream.Read(buffer, 0, 1);
            length = buffer[0];
            while (ms.Length != length)
            {
                ms.Write(buffer, 0, stream.Read(buffer, 0, buffer.Length));
            }
            return message = Encoding.ASCII.GetString(ms.ToArray());
        } 
    }
}
