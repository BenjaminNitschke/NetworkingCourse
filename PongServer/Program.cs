using MyNetworkLibrary;
using PongMessages;
using System;
using System.Threading;

namespace PongServer
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("PongServer started on port " + Settings.Port);
            server = new Server();
            server.ClientConnected += endPoint =>
            {
                if (server.clients.Count == 2)
                    StartGame();
            };
            server.ClientMessageReceived += PaddlePositionMessageReceived;
            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }

        private static void PaddlePositionMessageReceived(Client client, Message message)
        {
            var paddleMessage = (PaddleMessage)message;
            client.PaddleY = paddleMessage.Y;
            foreach (var other in server.clients)
                if (other != client)
                    other.Send(message);
        }

        private static void StartGame()
        {
            Console.WriteLine("2 players joined, starting game!");
            server.clients[LeftPlayerIndex].Send(new StartWithPaddleMessage { Left = true });
            server.clients[RightPlayerIndex].Send(new StartWithPaddleMessage { Left = false });
            new Timer(HandleBall, null, 0, 1000 / Fps);
        }

        const int LeftPlayerIndex = 0;
        const int RightPlayerIndex = 1;
        const int Fps = 30;

        private static void HandleBall(object unused)
        {
            ballX += ballVelocityX;
            ballY += ballVelocityY;
            if (ballY < -0.9f || ballY > 0.9f)
                ballVelocityY = -ballVelocityY;
            if (IsColliding(ballX, ballY, Constants.BallWidth, Constants.BallHeight,
                Constants.RightPaddleX, server.clients[RightPlayerIndex].PaddleY,
                Constants.PaddleWidth, Constants.PaddleHeight))
                ballVelocityX = -Math.Abs(ballVelocityX);
            if (IsColliding(ballX, ballY, Constants.BallWidth, Constants.BallHeight,
                Constants.LeftPaddleX, server.clients[LeftPlayerIndex].PaddleY,
                Constants.PaddleWidth, Constants.PaddleHeight))
                ballVelocityX = Math.Abs(ballVelocityX);
            if (ballX < -1)
            {
                server.clients[RightPlayerIndex].Points++;
                ResetBall();
            }
            else if (ballX > 1)
            {
                server.clients[LeftPlayerIndex].Points++;
                ResetBall();
            }
            else
                SendToAllPlayers(new BallMessage { X = ballX, Y = ballY });
        }

        static float ballX = 0.0f;
        static float ballY = 0.0f;
        static float ballVelocityX = Constants.DefaultBallVelocityX;
        static float ballVelocityY = Constants.DefaultBallVelocityY;

        public static bool IsColliding(float x, float y, float width, float height,
            float otherX, float otherY, float otherWidth, float otherHeight)
        {
            var left = x - width / 2;
            var right = x + width / 2;
            var top = y - height / 2;
            var bottom = y + height / 2;
            var otherLeft = otherX - otherWidth / 2;
            var otherRight = otherX + otherWidth / 2;
            var otherTop = otherY - otherHeight / 2;
            var otherBottom = otherY + otherHeight / 2;
            return right > otherLeft &&
                bottom > otherTop &&
                left < otherRight &&
                top < otherBottom;
        }

        private static void ResetBall()
        {
            SendToAllPlayers(new ScoreUpdatedMessage
            {
                LeftPlayer = server.clients[LeftPlayerIndex].Points,
                RightPlayer = server.clients[RightPlayerIndex].Points
            });
            ballX = 0;
            ballY = 0;
            var random = new Random((int)DateTime.Now.Ticks);
            ballVelocityX = (random.Next(2) == 0 ? -1 : +1) * Constants.DefaultBallVelocityX;
            ballVelocityY = (random.Next(2) == 0 ? -1 : +1) * Constants.DefaultBallVelocityY;
        }

        private static void SendToAllPlayers(Message message)
        {
            foreach (var client in server.clients)
                client.Send(message);
        }

        private static Server server;
    }
}
