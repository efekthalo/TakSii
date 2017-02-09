using SiiTaxi.Email;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SiiTaxi.Models
{
    public class TaxiViewModel
    {
        private readonly SiiTaxiEntities _context;

        public DateTime DateInput { get; set; }

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

        public TaxiViewModel(int id, string confirmString)
        {
            _context = new SiiTaxiEntities();

            ConfirmTemplate template = new ConfirmTemplate();
            template.ConfirmationString = confirmString;
            var body = template.TransformText();

            Emailer client = new Emailer("adam.guja@gmail.com", "adam.guja@gmail.com", body);
            client.SendEmail();
        }

        public IQueryable<Taxi> Get()
        {
            var list = _context.Taxi;
            return list == null ? new List<Taxi>().AsQueryable() : list;
        }

        public IQueryable<Taxi> Get(DateTime date)
        {
            var list = Get().Where(x => x.Time.Year == date.Year && x.Time.Month == date.Month && x.Time.Day == date.Day);
            return list == null ? new List<Taxi>().AsQueryable() : list;
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