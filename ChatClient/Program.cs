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
            server.Received += message => Console.WriteLine(((ChatMessage)message).Text);
            while (true)
            {
                Console.WriteLine("Enter text:");
                var text = Console.ReadLine();
                server.Send(new ChatMessage { Text = text });
            }
        }
    }
}