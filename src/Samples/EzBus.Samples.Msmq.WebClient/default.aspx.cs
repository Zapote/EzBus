using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EzBus.Samples.Messages.Commands;

namespace EzBus.Samples.Msmq.WebClient
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var command = new CreateOrder();
            Bus.Send(command);
            Bus.Send(new PlaceOrder(command.OrderId));
        }
    }
}