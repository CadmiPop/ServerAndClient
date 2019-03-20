using System.Linq;
using System.Text;

namespace CommonClasses
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

        public byte[] ToByteArray()
        {
            return Encoding.UTF8.GetBytes(message);
        }

        public override string ToString()
        {
            return message;
        }

        public string UserName => string.Concat(message.TakeWhile((c) => c != ':'));
    }
}
