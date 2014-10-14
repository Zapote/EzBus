using EzBus.Core;
using EzBus.Samples.Messages;
using System.Web.Mvc;

namespace EzBus.Samples.WebClient.Controllers
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