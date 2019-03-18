using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Server
{
    public class Message
    {       
        private readonly string message;
        
        public Message(string message)
        {
            this.message = message;            
        }

        public Message(byte[] message)
            : this(Encoding.UTF8.GetString(message))
        {
        }

        public byte[] ToByteArray() =>
              Encoding.UTF8.GetBytes(message);

        public override string ToString() => message;

    }
}
