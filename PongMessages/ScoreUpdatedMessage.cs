using MyNetworkLibrary;
using System;

namespace PongMessages
{
    [Serializable]
    public class ScoreUpdatedMessage : Message
    {
        public int LeftPlayer;
        public int RightPlayer;
    }
}