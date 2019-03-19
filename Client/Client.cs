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
            Read();
        }

        private void Engage()
        {
            Send();
        }

        public void Send()
        {
            while (true)
            {
                string msg = Console.ReadLine();
                if (msg == "exit" || msg.Length > 255)
                {
                    Disconnect();
                    return;
                }
                ClearLastLine();
                var message = new Message(userName + ": " + msg);
                stream.Write(BitConverter.GetBytes(message.ToByteArray().Length), 0, 1);
                stream.Write(message.ToByteArray(), 0, message.ToString().Length);
                stream.Flush();
            }
        }

        public void Read()
        {
            while (true)
            {
                try
                {
                    string message = "test";
                    message = new Protocol(stream).Manipulation();
                    Console.WriteLine(message);
                }
                catch (Exception)
                {
                    return;
                }
            } 
        }

        private void Disconnect()
        {
            stream.Close();
            client.Close();
        }

        public static void ClearLastLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}