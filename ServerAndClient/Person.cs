using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    public class Person
    {
        private readonly TcpClient client;
        private readonly string userName;
        private NetworkStream stream;

        public Person(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
            StrartThread();
            /*his.userName = GetUsername(userName);*/
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
            string message = String.Empty;
            message = ProtocolInOut(message);
            Console.WriteLine(/*userName + ":" +*/ message);
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

        //public string GetUsername(string Username)
        //{
        //    var a = Read();
        //    Username = a.ToString();
        //    return Username = string.Concat(Username.TakeWhile((c) => c != ':'));
        //}
    }
}
