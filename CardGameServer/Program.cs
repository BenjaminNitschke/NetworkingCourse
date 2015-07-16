using CardGameMessages;
using MyNetworkLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("CardGameServer started on port " + Settings.Port);
            server = new Server();
            server.ClientConnected += endPoint =>
                Console.WriteLine("Client connected: " + endPoint);
            server.ClientDisconnected += endPoint =>
                Console.WriteLine("Client disconnected: " + endPoint);
            server.ClientMessageReceived += MessageReceived;
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        static Server server;

        private static void MessageReceived(Client client, Message message)
        {
            if (message is ChatMessage)
                RelayChatToAllClients(client, message as ChatMessage);
            else if (message is PlayCardMessage)
                HandlePlayCard(client, message as PlayCardMessage);
            else if (message is PlayerNameMessage)
                HandlePlayerName(client, message as PlayerNameMessage);
        }

        private static void RelayChatToAllClients(Client client, ChatMessage chatMessage)
        {
            var text = new ChatMessage { Text = client.PlayerName + ": " + chatMessage.Text };
            Console.WriteLine(text.Text);
            foreach (var other in server.clients)
                if (other != client)
                    other.Send(text);
        }

        private static void HandlePlayCard(Client client, PlayCardMessage playCardMessage)
        {
            client.Points += playCardMessage.CardValue;
            foreach (var player in server.clients)
                if (player.Points == 0)
                    return;
            Client playerWithHighestPoints = null;
            foreach (var player in server.clients)
                if (playerWithHighestPoints == null || playerWithHighestPoints.Points < player.Points)
                    playerWithHighestPoints = player;
            if (playerWithHighestPoints.Points < 100)
                return;
            Console.Write("Game over, player " + playerWithHighestPoints.PlayerName + " has won with " + playerWithHighestPoints.Points);
            foreach (var player in server.clients)
                player.Send(new GameOverMessage
                {
                    Winner = playerWithHighestPoints.PlayerName,
                    Points = playerWithHighestPoints.Points
                });
        }

        private static void HandlePlayerName(Client client, PlayerNameMessage playerNameMessage)
        {
            client.PlayerName = playerNameMessage.PlayerName;
        }
    }
}