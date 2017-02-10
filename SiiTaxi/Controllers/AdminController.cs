using System.Web.Mvc;
using SiiTaxi.Models;
using System.Linq;

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

        public ActionResult Approvers(PeopleViewModel peopleModel)
        {
            var approvers = peopleModel.Get().Where(x => x.IsApprover == true);
            return View(approvers);
        }
    }
}
