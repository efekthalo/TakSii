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

        public TaxiViewModel()
        {
            _context = new SiiTaxiEntities();
            DateInput = DateTime.Now.Date;
            Taxis = _context.Taxi;
        }

        public TaxiViewModel(DateTime date)
        {
            _context = new SiiTaxiEntities();
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

                var client = new Emailer("taksii.test@gmail.com", _context.People.Find(entity.Owner).Email, body, "Potwierdzenie TakSii", _context.People.Find(entity.Approver).Email);
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

        internal void SendCode(int id, string code)
        {
            var taxi = GetEntityByKey(id);

            if (taxi.IsConfirmed)
            {
                var template = new SendCodeTemplate();
                template.TaxiFrom = taxi.From;
                template.TaxiTo = taxi.To;
                template.TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy");
                template.TaxiCodeString = code;
                var body = template.TransformText();

                var client = new Emailer("taksii.test@gmail.com", taxi.People.Email, body, "Kod TaxSii");
                client.SendEmail();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
