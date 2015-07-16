using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MyNetworkLibrary
{
    public class MessageSerializer
    {
        public MessageSerializer()
        {
            formatter = new BinaryFormatter();
        }
        private BinaryFormatter formatter;

        public byte[] Serialize(Message message)
        {
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, message);
            return stream.ToArray();
        }

        public Message Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            return (Message)formatter.Deserialize(stream);
        }

    }
}