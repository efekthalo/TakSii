using System;
using System.Collections.Generic;
using System.Linq;

namespace SiiTaxi.Models
{
    public class PeopleViewModel
    {
        private readonly SiiTaxiEntities _context;

        public IQueryable<People> People;

        public IQueryable<People> Approvers;

        public PeopleViewModel()
        {
            _context = new SiiTaxiEntities();
            People = Get();
            Approvers = People.Where(x => x.IsApprover);
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

        public People GetEntityByEmail(string email)
        {
            return _context.People.FirstOrDefault(x => x.Email == email);
        }

        public People GetEntityByName(string name)
        {
            return _context.People.FirstOrDefault(x => x.Name == name);
        }

        public People UpdatePeopleByKey(People update)
        {
            var entity = GetEntityByKey(update.PeopleId);
            return UpdatePeople(entity, update);
        }

        internal People UpdatePeopleByEmail(People update)
        {
            var entity = GetEntityByEmail(update.Email);
            return UpdatePeople(entity, update);
        }

        internal People UpdatePeopleByName(People update)
        {
            var entity = GetEntityByName(update.Name);
            return UpdatePeople(entity, update);
        }

        internal People UpdatePeople(People entity, People update)
        {
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
            var people = _context.People.Find(key);
            _context.People.Remove(people);
            _context.SaveChanges();
        }
    }
}