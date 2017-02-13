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
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(EncodedResponse))
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

            Taxi taxi = new Taxi
            {
                From = przejazdFrom,
                To = przejazdTo,
                Time = parsedTime,
                Owner = peopleModel.UpdateEntityBy<People>("Email", owner).PeopleId,
                Approver = approver
            };

            try
            {
                taxiModel.UpdateEntity(taxi);

                if (adds != null)
                {
                    foreach (var add in adds)
                    {
                        var other = new People { Name = add, Email = "" };
                        var taxiPeople = new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdateEntityBy<People>("Name", other).PeopleId };
                        taxiPeopleModel.AddEntity(taxiPeople);
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
            return View(new TaxiViewModel().GetEntityByKey(id));
        }

        [HttpPost]
        public ActionResult Include(int id, string name, string phone, string email, TaxiViewModel taxiModel, PeopleViewModel peopleModel, TaxiPeopleViewModel taxiPeopleModel)
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(EncodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return View(new TaxiViewModel().GetEntityByKey(id));
            }
            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(new TaxiViewModel().GetEntityByKey(id));
            }

            try
            {
                var other = new People { Name = name, Email = email, Phone = phone };
                var taxiPeople = new TaxiPeople { TaxiId = id, PeopleId = peopleModel.UpdateEntityBy<People>("Name", other).PeopleId };
                taxiPeopleModel.AddEntity(taxiPeople);
            }
            catch
            {
                TempData["errorMessage"] = Messages.DatabaseError;
                return View(new TaxiViewModel().GetEntityByKey(id));
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
            var taxi = new TaxiViewModel().GetEntityByKey(id);
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
                return View(new TaxiViewModel().GetEntityByKey(id));
            }
            TempData["successMessage"] = Messages.ConfirmSucceed;
            return RedirectToAction("Index", "Taxi") ;
        }
    }
}
