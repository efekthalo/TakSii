using System;
using System.Collections.Generic;
using System.Linq;
using SiiTaxi.Email;

namespace SiiTaxi.Models
{
    public class TaxiViewModel
    {
        private readonly SiiTaxiEntities _context;

        public IQueryable<Taxi> Taxis;

        public TaxiViewModel()
        {
            _context = new SiiTaxiEntities();
            DateInput = DateTime.Now.Date;
            Taxis = Get();
        }

        public TaxiViewModel(DateTime date)
        {
            _context = new SiiTaxiEntities();
            DateInput = date;
            Taxis = Get(date);
        }

        public TaxiViewModel(int id)
        {
            _context = new SiiTaxiEntities();

            var template = new ConfirmTemplate();
            template.ConfirmationString = "AAAAAAAAAAA";
            var body = template.TransformText();

            var client = new Emailer("adam.guja@gmail.com", "adam.guja@gmail.com", body);
            client.SendEmail();
        }

        public DateTime DateInput { get; set; }

        public IQueryable<Taxi> Get()
        {
            var list = _context.Taxi;
            return list == null ? new List<Taxi>().AsQueryable() : list;
        }

        public IQueryable<Taxi> Get(DateTime date)
        {
            var list =
                Get().Where(x => (x.Time.Year == date.Year) && (x.Time.Month == date.Month) && (x.Time.Day == date.Day));
            return list == null ? new List<Taxi>().AsQueryable() : list;
        }

        public Taxi GetEntityByKey(int key)
        {
            return _context.Taxi.Find(key);
        }

        public Taxi UpdateEntity(Taxi update)
        {
            var entity = GetEntityByKey(update.TaxiId);
            if (entity == null)
            {
                _context.Taxi.Add(update);
            }
            else
            {
                update.TaxiId = entity.TaxiId;
                entity = update;
            }

            _context.SaveChanges();
            return entity;
        }

        public void Delete(Taxi delete)
        {
            var taxi = GetEntityByKey(delete.TaxiId);
            _context.Taxi.Remove(taxi);
            _context.SaveChanges();
        }
    }
}