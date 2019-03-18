using System;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;

namespace MessageFacts
{
    public class MessageFacts
    {
        [Fact]
        public void Test1()
        {
            string str = "salut";
            var a = new Message();
            Assert.Equal(a.MessageType,str);
        }
    }
}
