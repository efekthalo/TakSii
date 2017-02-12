using System.Linq;

namespace SiiTaxi.Models
{
    public class TaxiPeopleViewModel
    {
        private readonly SiiTaxiEntities _context;
        public IQueryable<TaxiPeople> TaxiPeople;

        public TaxiPeopleViewModel()
        {
            _context = new SiiTaxiEntities();
            TaxiPeople = _context.TaxiPeople;
        }

        internal TaxiPeople GetEntityByKey(int key)
        {
            return _context.TaxiPeople.Find(key);
        }

        internal TaxiPeople AddEntity(TaxiPeople update)
        {
            var entity = _context.TaxiPeople.Add(update);
            _context.SaveChanges();

            return entity;
        }

        internal void Delete(TaxiPeople delete)
        {
            var taxi = GetEntityByKey(delete.Id);
            _context.TaxiPeople.Remove(taxi);
            _context.SaveChanges();
        }
    }
}