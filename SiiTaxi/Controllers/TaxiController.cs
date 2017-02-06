using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SiiTaxi.Models;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        //public ActionResult Index()
        //{
        //    return View(new TaxiViewModel(DateTime.Now));
        //}

        private TaxiContext db = new TaxiContext();

        public ActionResult New()
        {
            return View();
        }

        // GET: Taxis/Details/5
        public ActionResult Include(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Taxi");
            }
            Taxi taxi = db.Taxis.Find(id);
            if (taxi == null)
            {
                return HttpNotFound();
            }
            return View(taxi);
        }

        public ActionResult Index(DateTime? date = null)
        {

            return date != null ? IndexDate((DateTime)date) : IndexDate(DateTime.Now);

        }

        private ActionResult IndexDate(DateTime date)
        {
            return View(db.Taxis.Where(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day).ToList());
        }
    }
}
