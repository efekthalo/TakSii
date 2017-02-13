using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

namespace SiiTaxi.Models
{
    public class PeopleViewModel : AbstractViewModel
    {
        public IQueryable<People> Approvers;

        public IQueryable<People> People;

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

         public People UpdateApproverByEmail(People update)
        {
            var entity = GetEntityBy<People>("Email", update.Email);
            return UpdateApprover(entity, update);
        }



        private People UpdateApprover(People entity, People update)
        {
            if (entity == null)
            {
                entity = _context.People.Add(update);
            }
            else
            {
                entity.Name = update.Name;
                entity.Email = update.Email;
                entity.AltEmail = update.AltEmail;
                entity.Phone = update.Phone;
                entity.IsApprover = update.IsApprover;
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