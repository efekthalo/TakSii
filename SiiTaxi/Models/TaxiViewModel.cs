using System;
using System.Linq;
using SiiTaxi.Email;

namespace SiiTaxi.Models
{
    public sealed class TaxiViewModel : AbstractViewModel
    {
        public IQueryable<Taxi> Taxis;

        public DateTime DateInput { get; set; }

        public TaxiViewModel()
        {
            Context = new SiiTaxiEntities();
            DateInput = DateTime.Now.Date;
            Taxis = Get<Taxi>();
        }

        public TaxiViewModel(DateTime date)
        {
            Context = new SiiTaxiEntities();
            DateInput = date;
            Taxis = Get<Taxi>().Where(x => (x.Time.Year == date.Year) && (x.Time.Month == date.Month) && (x.Time.Day == date.Day));
        }

        public Taxi UpdateEntity(Taxi update)
        {
            var entity = GetEntityBy<Taxi>("TaxiId", update.TaxiId);
            if (entity == null)
            {
                var code = Guid.NewGuid().ToString();
                update.Confirm = code;
                entity = Context.Taxi.Add(update);
                Context.SaveChanges();

                var template = new ConfirmTemplate
                {
                    ConfirmationString = code,
                    TaxiId = entity.TaxiId
                };
                var body = template.TransformText();
                var people = Context.People.Find(entity.Owner);
                var approver = Context.People.Find(entity.Approver);
                if (people != null && approver != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", people.Email, body, "Potwierdzenie TakSii", approver.Email);
                    client.SendEmail();
                }
            }
            else
            {
                //update.TaxiId = entity.TaxiId;
                //entity = update;
                Context.Entry(entity).CurrentValues.SetValues(update);
                Context.SaveChanges();
            }

            return entity;
        }

        internal void ConfirmTaxi(int id, string confirm)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);

            if (taxi.Confirm == confirm)
            {
                taxi.IsConfirmed = true;
                Context.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal void SendCode(int id, string code)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId",id);

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
