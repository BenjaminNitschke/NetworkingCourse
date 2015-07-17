using MyNetworkLibrary;
using System;

namespace CardGameMessages
{
    [Serializable]
    public class PlayerNameMessage : Message
    {
        public string PlayerName;
    }
}