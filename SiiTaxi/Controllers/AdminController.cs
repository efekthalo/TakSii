using System.Web.Mvc;
using SiiTaxi.Models;

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
            return View(new TaxiViewModel());
        }
    }
}
