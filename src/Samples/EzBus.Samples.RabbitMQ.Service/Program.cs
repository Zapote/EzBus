using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EzBus.Samples.Messages;

namespace EzBus.Samples.RabbitMQ.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            Bus.Start();
        }
    }
}
