using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Group
    {
        private readonly List<Person> persons;

        public Group(List<Person> group)
        {
            this.persons = group;
        }

        public void Add(Person person)
        {
            person.NewMessage += Broadcast;
            person.PersonDisconnected += Remove;
            persons.Add(person);
        }

        public void Remove(Person person)
        {
            person.NewMessage -= Broadcast;
            person.PersonDisconnected -= Remove;
            persons.Remove(person);
        }

        public void Broadcast(Message message)
        {
            foreach (var person in persons)
            {
                person.Send(message);
            }
        }

        
    }
}
