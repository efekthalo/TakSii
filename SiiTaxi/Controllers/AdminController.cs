using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;
using System;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Taxi(System.DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
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
                return RedirectToAction("Taxi", "Admin");
            }
            TempData["successMessage"] = Messages.SendCodeSucceed;
            return RedirectToAction("Taxi", "Admin");
        }
    }
}
