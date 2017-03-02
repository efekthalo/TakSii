using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;
using System;
using System.Configuration;
using SiiTaxi.Email;
using System.Linq;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly SiiTaxiEntities _context = new SiiTaxiEntities();

        public ActionResult Taxi(DateTime? date)
        {
            IQueryable<Taxi> taxis;
            if (date != null)
            {
                taxis =
                    _context.Taxi.Where(
                        x =>
                            (x.Time.Year == ((DateTime)date).Year) &&
                            (x.Time.Month == ((DateTime)date).Month) &&
                            (x.Time.Day == ((DateTime)date).Day));
            }
            else
            {
                taxis = _context.Taxi;
            }
            return View(taxis);
        }

        [HttpPost]
        public ActionResult SendCode(int id, string code, string action)
        {
            try
            {
                var taxi = _context.Taxi.Find(id);
                if (taxi != null && taxi.IsOrdered)
                {
                    // taxi has already been ordered, no need to send the code
                    TempData["errorMessage"] = Messages.TaxiOrdered;
                    return RedirectToAction("Taxi", "Admin");
                }

                // action is used for sending different emails
                SendCode(taxi, code, action);
            }
            catch
            {
                TempData["errorMessage"] = Messages.SendCodeFailed;
                return RedirectToAction("Taxi", "Admin");
            }

            TempData["successMessage"] = Messages.SendCodeSucceed;
            return RedirectToAction("Taxi", "Admin");
        }

        public void SendCode(Taxi taxi, string code, string action)
        {
            if (taxi.IsConfirmed)
            {
                string body = "";

                // different emails when owner needs code only or taxi ordered
                switch (action)
                {
                    case "Send":
                        var codeTemplate = new SendCodeTemplate
                        {
                            TaxiFrom = taxi.From,
                            TaxiTo = taxi.To,
                            TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                            TaxiCodeString = code
                        };
                        body = codeTemplate.TransformText();
                        break;

                    case "Order":
                        var orderTemplate = new SendCodeAndOrderedTemplate
                        {
                            TaxiFrom = taxi.From,
                            TaxiTo = taxi.To,
                            TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                            TaxiCodeString = code
                        };
                        body = orderTemplate.TransformText();
                        break;
                }

                var client = new Emailer(ConfigurationManager.AppSettings["adminEmail"], taxi.People.Email, body, "Kod na taksówke - TakSii", ConfigurationManager.AppSettings["adminEmail"]);
                if (client.SendEmail())
                {
                    // update taxi code and mark as ordered after the email has been sent
                    taxi.TaxiCode = code;
                    taxi.IsOrdered = true;

                    TryUpdateModel(taxi);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Failed to send email.");
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
