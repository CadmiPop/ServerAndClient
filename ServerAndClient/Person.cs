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

        private readonly Action messageSent;

        public Person(TcpClient client)
        {
            this.client = client;
            stream = new ChatStream(client.GetStream());
        }

        public event Action<Message> NewMessage;

        public event Action<Person> PersonDisconnected;

        public void Engage()
        {
            BeginReadMessage();                                
        }      

        private void Leave()
        {
            Console.WriteLine(userName + " Disconnected!!!");
            DisconnectPerson();
        }

        internal void Send(Message message)
        {
            stream.Send(message, messageSent, e => DisconnectPerson());
        }

        private void DisconnectPerson()
        {
            PersonDisconnected?.Invoke(this);
        }

        private void DispachMessage(Message message)
        {           
            NewMessage?.Invoke(message);
        }

        public void BeginReadMessage()
        {
            stream.BeginReadMessage(m =>
            {
                SaveUserNameFromMessage(m);
                if (m.ToString().Contains("<!exit!>"))
                {
                    Leave();
                    return;
                }
                Print(m);
                DispachMessage(m);
                BeginReadMessage();
            }, e => DisconnectPerson());
        }

        private void SaveUserNameFromMessage(Message m)
        {
            if (userName == null)
            {
                userName = m.UserName;
            }
        }

        public void Print(Message message)
        {
            Console.WriteLine(message);
        }
    }
}
