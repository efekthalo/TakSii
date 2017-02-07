using SiiTaxi.Models;
using System;
using System.Web.Mvc;
using SiiTaxi.ViewModels;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View(new TaxiViewModel(DateTime.Now));
        //}

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(TaxiOrderViewModel taxiOrderViewModel)
        {
            return RedirectToAction("Index", "Taxi");
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
