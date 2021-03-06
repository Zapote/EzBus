﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EzBus
{
    public class BasicMessage
    {
        private readonly List<MessageHeader> headers = new List<MessageHeader>();

        public BasicMessage(Stream bodyStream)
        {
            BodyStream = bodyStream;
        }

        public Stream BodyStream { get; private set; }

        public IEnumerable<MessageHeader> Headers => headers;

        public void AddHeader(string name, string value)
        {
            var h = headers.FirstOrDefault(x => x.Name == name);
            if (h == null) h = new MessageHeader(name, value);
            h.ChangeValue(value);
            headers.Add(h);
        }

        public void AddHeader(params MessageHeader[] headerparams)
        {
            headers.AddRange(headerparams);
        }

        public string GetHeader(string name)
        {
            var header = headers.FirstOrDefault(x => x.Name == name);
            return header == null ? string.Empty : header.Value;
        }
    }
}