using MyNetworkLibrary;
using PongMessages;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicsEngine
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            server = new Client("localhost");
            server.Received += ServerMessageReceived;
            GraphicsAPI = API.OpenGL;
            Init();
            //graphics.Init3D();
            CreateSprites();
            while (true)
            {
                Application.DoEvents();
                if (form.IsDisposed)
                    break;
                graphics.Render();
                //graphics.Draw3DCube();
                HandleInput();
                DrawSprites();
                graphics.Present();
                if (title != null)
                    form.Text = title;
                title = null;
            }
            graphics.Dispose();
        }

        static Client server;

        private static void ServerMessageReceived(MyNetworkLibrary.Message message)
        {
            if (message is StartWithPaddleMessage)
                amILeftPaddle = ((StartWithPaddleMessage)message).Left;
            else if (message is PaddleMessage)
                SetOtherPaddlePosition(((PaddleMessage)message).Y);
            else if (message is BallMessage)
                SetBallPosition((BallMessage)message);
            else if (message is ScoreUpdatedMessage)
                UpdateTitleWithScore((ScoreUpdatedMessage)message);
        }

        private static void SetBallPosition(BallMessage ballMessage)
        {
            ballX = ballMessage.X;
            ballY = ballMessage.Y;
        }

        private static void UpdateTitleWithScore(ScoreUpdatedMessage score)
        {
            title = "Pong Score: " + score.LeftPlayer + " - " + score.RightPlayer;
        }
        static string title;

        private static void SetOtherPaddlePosition(float y)
        {
            if (amILeftPaddle)
                rightPaddleY = y;
            else
                leftPaddleY = y;
        }

        public static API GraphicsAPI;

        static void Init()
        {
            var common = new Common();
            form = new GraphicsEngineForm();
            form.Text = "Pong";
            form.Show();
            graphics = (Graphics)new OpenGLGraphics(form, common);
        }

        static GraphicsEngineForm form;
        static Graphics graphics;

        private static void CreateSprites()
        {
            background = new Sprite(new Texture("Background.jpg"), 2.0f, 2.0f);
            ball = new Sprite(new Texture("Ball.png"), Constants.BallWidth, Constants.BallHeight);
            var paddleTexture = new Texture("Paddle.png");
            leftPaddle = new Sprite(paddleTexture, Constants.PaddleWidth, Constants.PaddleHeight);
            rightPaddle = new Sprite(paddleTexture, Constants.PaddleWidth, Constants.PaddleHeight);
        }

        static Sprite background;
        static Sprite ball;
        static bool amILeftPaddle;
        static Sprite leftPaddle;
        static float leftPaddleY = 0;
        static Sprite rightPaddle;
        static float rightPaddleY = 0;

        private static void HandleInput()
        {
            if (amILeftPaddle)
            {
                if (form.DownPressed && leftPaddleY > -0.8f)
                {
                    leftPaddleY -= Constants.PaddleSpeed;
                    server.Send(new PaddleMessage { Y = leftPaddleY });
                }
                if (form.UpPressed && leftPaddleY < 0.8f)
                {
                    leftPaddleY += Constants.PaddleSpeed;
                    server.Send(new PaddleMessage { Y = leftPaddleY });
                }
            }
            else
            {
                if (form.DownPressed && rightPaddleY > -0.8f)
                {
                    rightPaddleY -= Constants.PaddleSpeed;
                    server.Send(new PaddleMessage { Y = rightPaddleY });
                }
                if (form.UpPressed && rightPaddleY < 0.8f)
                {
                    rightPaddleY += Constants.PaddleSpeed;
                    server.Send(new PaddleMessage { Y = rightPaddleY });
                }
            }
        }

        static float ballX = 0;
        static float ballY = 0;

        private static void DrawSprites()
        {
            background.Draw();
            ball.Draw(ballX, ballY);
            leftPaddle.Draw(Constants.LeftPaddleX, leftPaddleY);
            rightPaddle.Draw(Constants.RightPaddleX, rightPaddleY);
        }
        
#if UNUSED
        public static void ToggleAPIAndRestart()
        {
            GraphicsAPI = GraphicsAPI == API.OpenGL
                ? API.OpenGL4
                : API.OpenGL;
            form.Close();
            Init();
        }
#endif
    }
}