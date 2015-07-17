using MyNetworkLibrary;
using System;

namespace CardGameMessages
{
    [Serializable]
    public class GameOverMessage : Message
    {
        public string Winner;
        public int Points;
    }
}
