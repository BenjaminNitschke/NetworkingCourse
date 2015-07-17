using MyNetworkLibrary;
using System;

namespace PongMessages
{
    [Serializable]
    public class StartWithPaddleMessage : Message
    {
        public bool Left;
    }
}