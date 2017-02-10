using SiiTaxi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SiiTaxi.Providers;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        [HttpPost]
        public ActionResult New(string ownerName, string ownerPhone, string time, string ownerEmail, string ownerAltEmail, string przejazdFrom, string przejazdTo, List<string> adds, int approver, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptcha.Validate(EncodedResponse) == "True" ? true : false);

            if (!IsCaptchaValid)
            {
                return RedirectToAction("Index", "Taxi");
            }


            DateTime parsedTime;
            DateTime.TryParseExact(time, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedTime);

            var owner = new People { Name = ownerName, Email = ownerEmail, AltEmail = ownerAltEmail, Phone = ownerPhone };

            Taxi taxi = new Taxi
            {
                From = przejazdFrom,
                To = przejazdTo,
                Time = parsedTime,
                Owner = peopleModel.UpdatePeopleByEmail(owner).PeopleId,
                Approver = approver
            };

            taxiModel.UpdateEntity(taxi);

            if (adds != null)
            {
                foreach (var add in adds)
                {
                    var other = new People { Name = add, Email = "" };
                    taxi.TaxiPeople.Add(new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdatePeopleByName(other).PeopleId });
                }

                taxiModel.UpdateEntity(taxi);
            }

            return View();
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
        public ActionResult Include(int id, string name, string phone, string email, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
        {
            string EncodedResponse = Request.Form["g-Recaptcha-Response"];
            bool IsCaptchaValid = (ReCaptcha.Validate(EncodedResponse) == "True" ? true : false);

            if (!IsCaptchaValid)
            {
                return RedirectToAction("Index", "Taxi");
            }


            var taxi = new TaxiViewModel().GetEntityByKey(id);

            var person = new People()
            {
                Name = name,
                Phone = phone,
                Email = email
            };

            taxi.TaxiPeople.Add(new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdatePeopleByName(person).PeopleId });

            taxiModel.UpdateEntity(taxi);

            return RedirectToAction("Index", "Taxi");
        }

        public ActionResult Index(DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
        }

        public ActionResult Confirm(int id, string code)
        {
            var viewModel = new TaxiViewModel();
            viewModel.ConfirmTaxi(id, code);
            return View();
        }
    }
}
