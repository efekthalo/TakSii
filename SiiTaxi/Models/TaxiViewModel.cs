using System;
using System.Linq;
using SiiTaxi.Email;

namespace SiiTaxi.Models
{
    public class TaxiViewModel
    {
        private readonly SiiTaxiEntities _context;
        public IQueryable<Taxi> Taxis;

        public DateTime DateInput { get; set; }

        public TaxiViewModel(SiiTaxiEntities context = null)
        {
            _context = context == null ? new SiiTaxiEntities() : context;
            DateInput = DateTime.Now.Date;
            Taxis = _context.Taxi;
        }

        public TaxiViewModel(DateTime date, SiiTaxiEntities context = null)
        {
            _context = context == null ? new SiiTaxiEntities() : context;
            DateInput = date;
            Taxis = _context.Taxi.Where(x => (x.Time.Year == date.Year) && (x.Time.Month == date.Month) && (x.Time.Day == date.Day));
        }

        internal Taxi GetEntityByKey(int key)
        {
            return _context.Taxi.Find(key);
        }

        internal Taxi UpdateEntity(Taxi update)
        {
            var entity = GetEntityByKey(update.TaxiId);
            if (entity == null)
            {
                string code = Guid.NewGuid().ToString();
                update.Confirm = code;
                entity = _context.Taxi.Add(update);
                _context.SaveChanges();

                var template = new ConfirmTemplate();
                template.ConfirmationString = code;
                template.TaxiId = entity.TaxiId;
                var body = template.TransformText();

                var client = new Emailer("taksii.test@gmail.com", _context.People.Find(entity.Owner).Email, _context.People.Find(entity.Approver).Email, body);
                client.SendEmail();
            }
            else
            {
                //update.TaxiId = entity.TaxiId;
                //entity = update;
                _context.Entry(entity).CurrentValues.SetValues(update);
                _context.SaveChanges();
            }

            return entity;
        }

        internal void Delete(Taxi delete)
        {
            var taxi = GetEntityByKey(delete.TaxiId);
            _context.Taxi.Remove(taxi);
            _context.SaveChanges();
        }

        internal void ConfirmTaxi(int id, string confirm)
        {
            var taxi = GetEntityByKey(id);

            if (taxi.Confirm == confirm)
            {
                taxi.IsConfirmed = true;
                _context.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
