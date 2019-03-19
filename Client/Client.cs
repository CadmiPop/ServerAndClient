using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Client
{
    class Client
    {
        private TcpClient client;
        private string userName;
        private NetworkStream stream;

        public Client()
        {           
            ConnectClient();
            this.stream = client.GetStream();           
            StrartThread();
            
        }

        private void GetUserName()
        {
            Console.WriteLine("Enter Username:");
            userName = Console.ReadLine();
        }

        private void ConnectClient()
        {
            client = new TcpClient();
            client.Connect(IPAddress.Parse("127.0.0.1"), 5000);
            GetUserName();
            Console.WriteLine(userName + " connected to Server!!");
        }

        public void StrartThread()
        {
            Thread clientThread = new Thread(() => Engage());
            clientThread.Start();
            while (true)
            {
                Read();
            }
        }

        private void Engage()
        {
            while (true)
            {             
                Send();
            }
        }

        public void Send()
        {
            string msg = Console.ReadLine();
            var message = new Message(userName + ": " + msg);
            stream.Write(BitConverter.GetBytes(message.ToByteArray().Length), 0, 1);
            stream.Write(message.ToByteArray(), 0, message.ToString().Length);
            stream.Flush();
        }

        public Message Read()
        {
            string message = String.Empty;
            message = ProtocolInOut(message);
            Console.WriteLine(message);
            return new Message(message);
        }

        public string ProtocolInOut(string message)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1];
            stream.Read(buffer, 0, 1);
            int length = buffer[0];

            while (ms.Length != length)
            {
                ms.Write(buffer, 0, stream.Read(buffer, 0, buffer.Length));
            }

            return message = Encoding.ASCII.GetString(ms.ToArray());
        }
    }
}