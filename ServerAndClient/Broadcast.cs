using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Broadcast
    {
        private List<TcpClient> connectedClients;
        private byte[] msg;

        public Broadcast(byte[] msg)
        {
            this.msg = msg;
        }

        public void BroadcastData()
        {
            foreach (var clientOnline in connectedClients)
            {
                NetworkStream nS = clientOnline.GetStream();
                nS.Write(BitConverter.GetBytes(msg.Length), 0, 1);
                nS.Write(msg, 0, msg.Length);
                nS.Flush();
            }
        }
    }
}
