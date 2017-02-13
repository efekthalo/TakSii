using SiiTaxi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using SiiTaxi.Providers;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        [HttpPost]
        public ActionResult New(string ownerName, string ownerPhone, string time, string ownerEmail, string ownerAltEmail, string przejazdFrom, string przejazdTo, List<string> adds, int approver, TaxiViewModel taxiModel, PeopleViewModel peopleModel, TaxiPeopleViewModel taxiPeopleModel)
        {
            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(encodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return View(new PeopleViewModel());
            }

            if(!Validators.IsEmailValid(ownerEmail, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(new PeopleViewModel());
            }
            if (!Validators.IsEmailValid(ownerAltEmail))
            {
                TempData["errorMessage"] = Messages.NotValidAltEmail;
                return View(new PeopleViewModel());
            }

            DateTime parsedTime;
            DateTime.TryParseExact(time, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedTime);

            if(parsedTime < DateTime.Now)
            {
                TempData["errorMessage"] = Messages.NotValidDate;
                return View(new PeopleViewModel());
            }

            var owner = new People
            {
                Name = ownerName,
                Email = ownerEmail,
                AltEmail = ownerAltEmail,
                Phone = ownerPhone
            };

            var taxi = new Taxi
            {
                From = przejazdFrom,
                To = przejazdTo,
                Time = parsedTime,
                Owner = peopleModel.UpdateEntityBy("Email", owner).PeopleId,
                Approver = approver
            };

            try
            {
                taxiModel.UpdateEntity(null, taxi);

                if (adds != null)
                {
                    foreach (var add in adds)
                    {
                        var other = new People { Name = add, Email = "" };
                        var taxiPeople = new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdateEntityBy("Name", other).PeopleId };
                        taxiPeopleModel.UpdateEntity(null, taxiPeople);
                    }
                }
            }
            catch
            {
                TempData["errorMessage"] = Messages.DatabaseError;
                return View(new PeopleViewModel());
            }

            TempData["successMessage"] = Messages.AddNewTaxiSuccess;
            return View(new PeopleViewModel());
        }

        [HttpGet]
        public ActionResult New()
        {
            return View(new PeopleViewModel());
        }

        public ActionResult Include(int id)
        {
            return View(new TaxiViewModel().GetEntityBy<Taxi>("TaxiId", id));
        }

        [HttpPost]
        public ActionResult Include(int id, string name, string phone, string email, TaxiViewModel taxiModel, PeopleViewModel peopleModel, TaxiPeopleViewModel taxiPeopleModel)
        {
            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(encodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }
            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }

            try
            {
                var other = new People { Name = name, Email = email, Phone = phone };
                var taxiPeople = new TaxiPeople { TaxiId = id, PeopleId = peopleModel.UpdateEntityBy("Name", other).PeopleId };
                taxiPeopleModel.UpdateEntity(null, taxiPeople);
            }
            catch
            {
                TempData["errorMessage"] = Messages.DatabaseError;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }

            TempData["successMessage"] = Messages.IncludeTaxiSuccess;
            return RedirectToAction("Index", "Taxi");
        }

        public ActionResult Index(DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
        }

        [HttpGet]
        public ActionResult Confirm(int id, string code)
        {
            var taxi = new TaxiViewModel().GetEntityBy<Taxi>("Taxi", id);
            if(taxi != null && taxi.Confirm == code)
            {
                if (taxi.IsConfirmed)
                {
                    TempData["errorMessage"] = Messages.TaxiConfirmed;
                    return RedirectToAction("Index", "Taxi");
                }
                TempData["code"] = code;
                return View(taxi);
            }
            TempData["errorMessage"] = Messages.TaxiNotExist;
            return RedirectToAction("Index", "Taxi");
  
        }

        [HttpPost]
        public ActionResult Confirm(int id, string code, TaxiViewModel taxiModel)
        {
            try
            {
                taxiModel.ConfirmTaxi(id, code);
            }
            catch
            {
                TempData["errorMessage"] = Messages.ConfirmFailed;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }
            TempData["successMessage"] = Messages.ConfirmSucceed;
            return RedirectToAction("Index", "Taxi") ;
        }
    }
}
