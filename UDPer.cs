using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;

namespace UDPer
{
    class UDPer
    {
        const int PORT_NUMBER = 15000;

        Thread t = null;
        public void Start()
        {
            if (t != null)
            {
                throw new Exception("Already started, stop first");
            }
            Console.WriteLine("Started listening");
            StartListening();
        }
        public void Stop()
        {
            try
            {
                udp.Close();
                Console.WriteLine("Stopped listening");
            }
            catch { /* don't care */ }
        }

        private readonly UdpClient udp = new UdpClient(PORT_NUMBER);
        IAsyncResult ar_ = null;

        private void StartListening()
        {
            ar_ =  udp.BeginReceive(Receive, new object());
        }
        private void Receive(IAsyncResult ar)
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, PORT_NUMBER);
            byte[] bytes = udp.EndReceive(ar, ref ip);
            string message = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("From {0} received: {1} ", ip.Address.ToString(), message);
            StartListening();
        }
        public void Send(string message)
        {
            UdpClient client = new UdpClient();
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("255.255.255.255"), PORT_NUMBER);
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            client.Send(bytes, bytes.Length, ip);
            client.Close();
            Console.WriteLine("Sent: {0} ", message);
        }
    }

    
    
    class Program
    {
        static void Main(string[] args)
        {
            UDPer udp = new UDPer();
            udp.Start();

            ConsoleKeyInfo cki;
            do
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    switch (cki.KeyChar)
                    {
                        case 's':
                            udp.Send(new Random().Next().ToString());
                            break;
                        case 'x':
                            udp.Stop();
                            return;
                    }
                }
                Thread.Sleep(10);
            } while (true);
        }
    }
}
