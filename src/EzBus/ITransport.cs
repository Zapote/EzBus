using System;
using System.Collections.Generic;
using System.Text;

namespace EzBus
{
    public interface ITransport
    {
        IHost Host { get; }
    }
}
