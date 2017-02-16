using System.Linq;

namespace SiiTaxi.Models
{
    public sealed class PeopleViewModel : AbstractViewModel
    {
        public IQueryable<People> Approvers;

        public IQueryable<People> People;

        public PeopleViewModel()
        {
            Context = new SiiTaxiEntities();
            People = Get<People>();
            Approvers = People.Where(x => x.Approvers.IsApprover);
        }
    }
}