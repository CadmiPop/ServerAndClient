using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommonClasses;

namespace Server
{
    public class Person
    {
        private readonly TcpClient client;
        private string userName;
        private NetworkStream stream;

        public Person(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
            StrartThread();
        }

        public event Action<Message> NewMessage;

        public event Action<Person> PersonDisconnected;

        public void StrartThread()
        {
            var clientThread = new Thread(() => Engage());
            clientThread.Start();
        }

        public void Send(Message message)
        {
            stream.Write(BitConverter.GetBytes(message.ToByteArray().Length), 0, 1);
            stream.Write(message.ToByteArray(), 0, message.ToString().Length);
            stream.Flush();
        }

        private void Engage()
        {
            while (true)
            {
                try
                {
                    var message = Read();
                    DispachMessage(message);
                }
                catch (IOException)
                {
                    Console.WriteLine(userName + " Disconnected!!!");
                    DisconnectPerson(this);                   
                    return;
                }
            }
        }

        private void DisconnectPerson(Person person)
        {
            PersonDisconnected?.Invoke(person);
        }

        private void DispachMessage(Message message)
        {           
            NewMessage?.Invoke(message);
        }

        public Message Read()
        {
            string message = new Protocol(stream).Message();
            if (userName == null)
            {
                userName = string.Concat(message.TakeWhile((c) => c != ':'));
            }
            Console.WriteLine(message);
            return new Message(message);
        }
    }
}
