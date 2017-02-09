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

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Include(int id)
        {
            return View(new TaxiViewModel(id));
        }

        public ActionResult Index(DateTime? date = null)
        {
            return View(new TaxiViewModel(date ?? DateTime.Now));
        }
    }
}
