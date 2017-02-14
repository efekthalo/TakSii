using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Taxi()
        {
            return View(new TaxiViewModel());
        }

        public ActionResult Approvers(PeopleViewModel peopleModel)
        {
            return View(new PeopleViewModel());
        }


        [HttpPost]
        public ActionResult SendCode(int id, string code, TaxiViewModel taxiModel)
        {
            try
            {
                taxiModel.SendCode(id, code);
            }
            catch
            {
                TempData["errorMessage"] = Messages.SendCodeFailed;
                return View(new TaxiViewModel());
            }
            TempData["successMessage"] = Messages.SendCodeSucceed;
            return RedirectToAction("Taxi", "Admin");
        }
    }
}
