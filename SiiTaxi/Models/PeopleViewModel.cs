using System;
using System.Collections.Generic;
using System.Linq;

namespace SiiTaxi.Models
{
    public class PeopleViewModel
    {
        private readonly SiiTaxiEntities _context;

        public IQueryable<People> People;

        public PeopleViewModel()
        {
            _context = new SiiTaxiEntities();
            People = Get();
        }

        public IQueryable<People> Get()
        {
            var list = _context.People;
            return list == null ? new List<People>().AsQueryable() : list;
        }

        public People GetEntityByKey(int key)
        {
            return _context.People.Find(key);
        }

        public People GetEntityByName(string name)
        {
            return _context.People.FirstOrDefault(x => x.Name == name);
        }

        public People UpdateEntity(int key, People update)
        {
            var entity = GetEntityByKey(key);
            if (entity == null)
            {
                _context.People.Add(update);
            }
            else
            {
                entity = update;
            }
            _context.SaveChanges();
            return update;
        }

        internal People UpdatePeople(string add, string email, string altEmail)
        {
            var update = new People { Name = add, Email = email, AltEmail = altEmail };

            var entity = GetEntityByName(add);
            if (entity == null)
            {
                entity = _context.People.Add(update);
            }
            else
            {
                update.PeopleId = entity.PeopleId;
                entity = update;
            }
            _context.SaveChanges();
            return entity;
        }

        public void Delete(int key)
        {
            var customer = _context.People.Find(key);
            _context.People.Remove(customer);
            _context.SaveChanges();
        }
    }
}