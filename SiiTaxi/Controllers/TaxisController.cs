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
    public class TaxisController : Controller
    {
        private TaxiContext db = new TaxiContext();

        // GET: Taxis
        public ActionResult Index()
        {
            return View(db.Taxis.ToList());
        }

        // GET: Taxis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxi taxi = db.Taxis.Find(id);
            if (taxi == null)
            {
                return HttpNotFound();
            }
            return View(taxi);
        }

        // GET: Taxis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Taxis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TaxiID,From,To,Date,OwnerID,ApproverID,ownerConfirmed,approverConfirmed")] Taxi taxi)
        {
            if (ModelState.IsValid)
            {
                db.Taxis.Add(taxi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(taxi);
        }

        // GET: Taxis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxi taxi = db.Taxis.Find(id);
            if (taxi == null)
            {
                return HttpNotFound();
            }
            return View(taxi);
        }

        // POST: Taxis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TaxiID,From,To,Date,OwnerID,ApproverID,ownerConfirmed,approverConfirmed")] Taxi taxi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taxi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taxi);
        }

        // GET: Taxis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taxi taxi = db.Taxis.Find(id);
            if (taxi == null)
            {
                return HttpNotFound();
            }
            return View(taxi);
        }

        // POST: Taxis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taxi taxi = db.Taxis.Find(id);
            db.Taxis.Remove(taxi);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
