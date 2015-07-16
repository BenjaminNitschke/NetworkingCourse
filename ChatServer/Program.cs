using MyNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("ChatServer started on port " + Settings.Port);
            server = new Server();
            server.ClientConnected += endPoint =>
                Console.WriteLine("Client connected: " + endPoint);
            server.ClientDisconnected += endPoint =>
                Console.WriteLine("Client disconnected: " + endPoint);
            server.ClientMessageReceived += ChatReceived;
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        static Server server;
        
        private static void ChatReceived(Client client, Message message)
        {
            var text = new ChatMessage
            {
                Text = "Client " + client.EndPoint + ": " + ((ChatMessage)message).Text
            };
            Console.WriteLine(text.Text);
            foreach (var other in server.clients)
                if (other != client)
                    other.Send(text);
        }
    }
}