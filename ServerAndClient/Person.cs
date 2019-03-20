using CommonClasses;
using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    public class Person
    {
        private readonly TcpClient client;
        private string userName;
        private ChatStream stream;

        public Person(TcpClient client)
        {
            this.client = client;
            stream = new ChatStream(client.GetStream());
            StrartThread();
        }

        public event Action<Message> NewMessage;

        public event Action<Person> PersonDisconnected;

        public void StrartThread()
        {
            Thread clientThread = new Thread(() => Engage());
            clientThread.Start();
        }


        private void Engage()
        {
            while (true)
            {
                try
                {
                    Message message = Read();
                    if (message.ToString().Contains("<!exit!>"))
                    {
                        Leave();
                        return;
                    }
                    Console.WriteLine(message);
                    DispachMessage(message);
                }
                catch (IOException)
                {
                    Leave();
                    return;
                }
            }
        }

        private void Leave()
        {
            Console.WriteLine(userName + " Disconnected!!!");
            DisconnectPerson(this);
        }

        internal void Send(Message message)
        {
            stream.Send(message);
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
            Message message = stream.Message();
            if (userName == null)
            {
                userName = message.UserName;
            }
            return message;
        }
    }
}
