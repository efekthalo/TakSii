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

        public bool SendConfirmEmail(int id)
        {
            var entity = GetEntityBy<Taxi>("TaxiId", id);
            if (entity != null)
            {
                var code = Guid.NewGuid().ToString();
                entity.Confirm = code;
                UpdateEntityBy("TaxiId", entity);

                var template = new ConfirmTemplate
                {
                    ConfirmationString = code,
                    TaxiId = entity.TaxiId
                };

                var body = template.TransformText();
                var people = GetEntityBy<People>("PeopleId", entity.Owner);
                var approver = GetEntityBy<People>("PeopleId", entity.Approver);
                if (people != null && approver != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", people.Email, body, "Potwierdzenie TakSii", approver.Email);
                    client.SendEmail();
                    return true;
                }
            }

            return false;
        }

        public void ConfirmTaxi(int id, string confirm)
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

        public void SendCode(int id, string code)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);

            if (taxi.IsConfirmed)
            {
                var template = new SendCodeTemplate
                {
                    TaxiFrom = taxi.From,
                    TaxiTo = taxi.To,
                    TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                    TaxiCodeString = code
                };
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
