using NUnit.Framework;
using System;

namespace MyNetworkLibrary
{
    public class MessageSerializerTests
    {
        [SetUp]
        public void CreateSerializer()
        {
            serializer = new MessageSerializer();
        }
        private MessageSerializer serializer;

        [Test]
        public void SerializeAndDeserializeEmptyMessage()
        {
            var data = serializer.Serialize(new EmptyMessage());
            Assert.That(data, Is.Not.Empty);
            var message = serializer.Deserialize(data);
            Assert.That(message, Is.InstanceOf<EmptyMessage>());
        }

        [Serializable]
        public class EmptyMessage : Message {}

        [Test]
        public void SerializeAndDeserializeChatMessage()
        {
            var data = serializer.Serialize(new ChatMessage { Text = "hi there" });
            var message = (ChatMessage)serializer.Deserialize(data);
            Assert.That(message.Text, Is.EqualTo("hi there"));
        }
    }
}
