using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;
using System;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Taxi(DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
        }

        [HttpPost]
        public ActionResult SendCode(int id, string code, string action, TaxiViewModel taxiModel)
        {
            try
            {
                var taxi = taxiModel.GetEntityBy<Taxi>("TaxiId", id);
                if (taxi.IsOrdered)
                {
                    TempData["errorMessage"] = Messages.TaxiOrdered;
                    return RedirectToAction("Taxi", "Admin");
                }

                taxiModel.SendCode(taxi, code, action);
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
