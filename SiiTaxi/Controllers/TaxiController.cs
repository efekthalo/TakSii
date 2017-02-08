using SiiTaxi.Models;
using System;
using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View(new TaxiViewModel(DateTime.Now));
        //}
        [HttpPost]
        public ActionResult New(string ownerName)
        {

            Taxi taxi = new Taxi
            {
                From = ModelState["ownerName"].Value,
                Other = ModelState["ownerName"].Value,
                Owner = ModelState["ownerName"].Value,
                Time = ModelState["ownerName"].Value,
                To = ModelState["ownerName"].Value
            };

            var x = ModelState["ownerName"].Value;
            return View();
        }

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
