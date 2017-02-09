using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Taxi()
        {
            return View();
        }

        public ActionResult Approvers()
        {
            return View();
        }
    }
}
