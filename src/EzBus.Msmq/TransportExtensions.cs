using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBus.Msmq
{
    public static class TransportExtensions
    {
        public static void UseMsmq(this ITransport transport)
        {
            transport.Host.Start();
        }
    }
}
