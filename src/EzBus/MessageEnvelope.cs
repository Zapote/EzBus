using System;
using System.Collections.Generic;
using System.IO;

namespace EzBus
{
    public class MessageEnvelope
    {
        private readonly IList<MessageHeader> headers = new List<MessageHeader>();

        public MessageEnvelope(Stream bodyStream, Type messageType)
        {
            MessageType = messageType;
            BodyStream = bodyStream;
        }

        public Stream BodyStream { get; private set; }
        public Type MessageType { get; private set; }

        public IEnumerable<MessageHeader> Headers
        {
            get { return headers; }
        }

        public void AddHeader(string name, string value)
        {
            headers.Add(new MessageHeader(name, value));
        }
    }
}