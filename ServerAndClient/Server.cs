using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Server
    {
        private TcpListener chatServer;
        private Group group = new Group(new List<Person>());

        public Server()
        {
            chatServer = new TcpListener(IPAddress.Any, 5000);
            chatServer.Start();
            AcceptClients();
        }

        public void AcceptClients()
        {
            Console.WriteLine("-----Server Started!!!-----");
            while (true)
            {
                var client = new Person(chatServer.AcceptTcpClient());
                group.Add(client);
                Console.WriteLine("Client Connected!!");
            } 
        }
    }
}
