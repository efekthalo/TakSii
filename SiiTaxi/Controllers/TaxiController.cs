using SiiTaxi.Models;
using System.Linq;
using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        public ActionResult Index()
        {
            return View(Get());
        }

        public ActionResult New()
        {
            return View();
        }
        public ActionResult Include()
        {
            return View();
        }

        private readonly SiiTaxiEntities _context;
        public TaxiController()
        {
            _context = new SiiTaxiEntities();
        }

        public IQueryable<Taxi> Get()
        {
            return _context.Taxi;
        }

        protected Taxi GetEntityByKey(string key)
        {
            return _context.Taxi.Find(key);
        }

        protected Taxi UpdateEntity(string key, Taxi update)
        {
            var entity = GetEntityByKey(key);
            if (entity == null)
            {
                _context.Taxi.Add(update);
            }
            else
            {
                entity = update;
            }
            _context.SaveChanges();
            return update;
        }

        public void Delete(string key)
        {
            var customer = _context.Taxi.Find(key);
            _context.Taxi.Remove(customer);
            _context.SaveChanges();
        }
    }
}
