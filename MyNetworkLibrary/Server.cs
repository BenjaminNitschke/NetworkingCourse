using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace MyNetworkLibrary
{
    public class Server
    {
        public Server()
	    {
            listener = new TcpListener(IPAddress.Any, Settings.Port);
            listener.Start();
            new Thread(ListenForClients).Start();
	    }

        private void ListenForClients()
        {
            while (true)
            {
                var client = new Client(listener.AcceptTcpClient());
                clients.Add(client);
                if (ClientConnected != null)
                    ClientConnected(client.EndPoint);
                client.Disconnected += () =>
                {
                    if (ClientDisconnected != null)
                        ClientDisconnected(client.EndPoint);
                };
                client.Received += message => ClientMessageReceived(client, message);
            }
        }

        private TcpListener listener;
        public event Action<EndPoint> ClientConnected;
        public event Action<EndPoint> ClientDisconnected;
        public event Action<Client, Message> ClientMessageReceived;
        public readonly List<Client> clients = new List<Client>();
    }
}