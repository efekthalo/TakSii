using SiiTaxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        [HttpPost]
        public ActionResult New(string ownerName, string time, string ownerEmail, string OwnerAltEmail, string przejazdFrom, string przejazdTo, List<string> adds, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
        {
            DateTime parsedTime;
            DateTime.TryParse(time, out parsedTime);

            People owner = peopleModel.UpdatePeople(ownerName, ownerEmail, OwnerAltEmail);
            
            Taxi taxi = new Taxi
            {
                From = przejazdFrom,
                Owner = owner.PeopleId,
                Time = parsedTime,
                To = przejazdTo
            };

            taxiModel.UpdateEntity(-1, taxi);

            foreach (var add in adds)
            {
                taxi.TaxiPeople.Add(new TaxiPeople { TaxiId = taxi.TaxiId, PeopleId = peopleModel.UpdatePeople(add, "", "").PeopleId });
            }

            taxiModel.UpdateEntity(-1, taxi);

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
