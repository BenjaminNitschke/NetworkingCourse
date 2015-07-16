using MyNetworkLibrary;
using System;

namespace CardGameMessages
{
    [Serializable]
    public class PlayCardMessage : Message
    {
        public int CardValue;
    }
}