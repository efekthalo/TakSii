using SiiTaxi.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        [HttpPost]
        public ActionResult New(string ownerName, string ownerPhone, string time, string ownerEmail, string ownerAltEmail, string przejazdFrom, string przejazdTo, List<string> adds, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
        {
            DateTime parsedTime;
            DateTime.TryParse(time, out parsedTime);

            People owner = new People { Name = ownerName, Email = ownerEmail, AltEmail = ownerAltEmail, Phone = ownerPhone };
            owner = peopleModel.UpdatePeopleByEmail(owner);

            Taxi taxi = new Taxi
            {
                From = przejazdFrom,
                To = przejazdTo,
                Time = parsedTime,
                People = owner
            };

            taxiModel.UpdateEntity(taxi);

            if (adds != null)
            {
                foreach (var add in adds)
                {
                    var other = new People { Name = add, Email = "", AltEmail = "" };
                    taxi.TaxiPeople.Add(new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdatePeopleByName(other).PeopleId });
                }

                taxiModel.UpdateEntity(taxi);
            }

            return View();
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        public ActionResult Include()
        {
            return View();
        }

        public ActionResult Index(DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
        }
    }
}
