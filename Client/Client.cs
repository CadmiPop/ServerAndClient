using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommonClasses;

namespace Client
{
    class Client
    {
        private TcpClient client;
        private string userName;
        private ChatStream stream;

        public Client()
        {           
            ConnectClient();
            this.stream = new ChatStream(client.GetStream());
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
            Console.WriteLine("-----You are now connected to Server!!!-----");
        }

        public void StrartThread()
        {
            Thread clientThread = new Thread(() => Send());
            clientThread.Start();
            Read();
        }

        public void Send()
        {
            while (true)
            {
                string msg = Console.ReadLine();
                if (msg.Contains("exit") || msg.Length > 255)
                {
                    stream.Send(new Message(userName + ":<!exit!>"));
                    Disconnect();
                    return;
                }
                ClearLastLine();
                stream.Send(new Message(userName + ": " + msg));
            }
        }

        public void Read()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(stream.Message());
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