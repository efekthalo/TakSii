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
            Context = new SiiTaxiEntities();
            People = Get<People>();
            Approvers = People.Where(x => x.IsApprover);
        }

        public People UpdateApproverByEmail(People update)
        {
            var entity = GetEntityBy<People>("Email", update.Email);
            return UpdateApprover(entity, update);
        }

        internal People UpdatePeople(People entity, People update)
        {
            if (entity == null)
            {
                entity = _context.People.Add(update);
            }
            else
            {
                entity.Name = update.Name;
                entity.Email = update.Email;
                entity.Phone = update.Phone;
            }

            _context.SaveChanges();
            return entity;
        }

        internal People UpdateApprover(People entity, People update)
        {
            if (entity == null)
            {
                entity = Context.People.Add(update);
            }
            else
            {
                entity.Name = update.Name;
                entity.Email = update.Email;
                entity.Phone = update.Phone;
                entity.IsApprover = update.IsApprover;
            }

            Context.SaveChanges();
            return entity;
        }
    }
}