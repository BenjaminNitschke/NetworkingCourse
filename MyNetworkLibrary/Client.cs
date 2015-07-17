using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyNetworkLibrary
{
    public class Client
    {
        public Client(string hostname)
            : this(new TcpClient(hostname, Settings.Port)) {}

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            serializer = new MessageSerializer();
            new Thread(DataReceived).Start(tcpClient);
        }

        private TcpClient tcpClient;
        private MessageSerializer serializer;

        private void DataReceived(object clientObject)
        {
            var client = (TcpClient)clientObject;
            var data = new byte[1000];
            while (client.Connected)
            {
                var stream = client.GetStream();
                int receivedBytes = 0;
                try
                {
                    receivedBytes = stream.Read(data, 0, data.Length);
                }
                catch
                {
                    // Connection lost
                    break;
                }
                byte[] receivedData = new byte[receivedBytes];
                Array.Copy(data, receivedData, receivedData.Length);
                if (Received != null)
                    Received(serializer.Deserialize(receivedData));
            }
            if (Disconnected != null)
                Disconnected();
        }

        public event Action<Message> Received;
        public event Action Disconnected;
        public string PlayerName
        {
            get
            {
                if (playerName != null)
                    return playerName;
                return EndPoint.ToString();
            }
            set { playerName = value; }
        }
        private string playerName;
        public int Points;
        public float PaddleY;

        public void Send(Message message)
        {
            tcpClient.Client.Send(serializer.Serialize(message));
        }

        public EndPoint EndPoint
        {
            get { return tcpClient.Client.RemoteEndPoint; }
        }
    }
}