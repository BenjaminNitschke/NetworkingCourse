using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNetworkLibrary
{
    public interface Message {}

    [Serializable]
    public class ChatMessage : Message
    {
        public string Text;
    }
}
