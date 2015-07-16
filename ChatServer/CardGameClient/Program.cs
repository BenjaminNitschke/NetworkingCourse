using CardGameMessages;
using MyNetworkLibrary;
using System;
using System.Text;

namespace ChatClient
{
    class Program
    {
        static void Main()
        {
#if DEBUG
            const string hostname = "localhost";
#else
            Console.Write("Enter ChatServer Ip: ");
            var hostname = Console.ReadLine();
#endif
            var server = new Client(hostname);
            server.Received += MessageReceived;
            Console.Write("Please enter your player name: ");
            var playerName = Console.ReadLine();
            server.Send(new PlayerNameMessage { PlayerName = playerName });
            while (true)
            {
                var random = new Random((int)DateTime.Now.Ticks);
                var card = new PlayCardMessage { CardValue = random.Next(5, 50) };
                Console.WriteLine("Press enter to play your card (Value=" + card.CardValue + ")");
                Console.ReadLine();
                server.Send(card);
            }
        }

        private static void MessageReceived(Message message)
        {
            if (message is ChatMessage)
                Console.WriteLine(((ChatMessage)message).Text);
            else if (message is GameOverMessage)
                Console.WriteLine("Game Over, Player " + ((GameOverMessage)message).Winner +
                    " has won with Points=" + ((GameOverMessage)message).Points);
        }
    }
}