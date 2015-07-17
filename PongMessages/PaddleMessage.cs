using MyNetworkLibrary;
using System;

namespace PongMessages
{
    [Serializable]
    public class PaddleMessage : Message
    {
        public float Y;
    }
}