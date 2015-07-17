using MyNetworkLibrary;
using System;

namespace PongMessages
{
    [Serializable]
    public class BallMessage : Message
    {
        public float X;
        public float Y;
    }
}