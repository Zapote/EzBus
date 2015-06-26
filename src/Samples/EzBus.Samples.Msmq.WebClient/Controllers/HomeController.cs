using System.Web.Mvc;
using EzBus.Samples.Messages;

namespace EzBus.Samples.Msmq.WebClient.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            Bus.Send(new SayHello("f")
            {

            });
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}