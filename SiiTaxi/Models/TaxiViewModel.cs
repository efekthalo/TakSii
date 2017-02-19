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
            Context = new SiiTaxiEntities(true);
            DateInput = DateTime.Now.Date;
            Taxis = Get<Taxi>();
        }

        public TaxiViewModel(DateTime date)
        {
            Context = new SiiTaxiEntities(true);
            DateInput = date;
            Taxis = Get<Taxi>().Where(x => (x.Time.Year == date.Year) && (x.Time.Month == date.Month) && (x.Time.Day == date.Day));
        }

        public bool SendConfirmEmail(int id)
        {
            var entity = GetEntityBy<Taxi>("TaxiId", id);
            if (entity != null)
            {
                var code = Guid.NewGuid().ToString();
                entity.ConfirmCode = code;
                UpdateEntityBy("TaxiId", entity);

                var template = new ConfirmTemplate
                {
                    ConfirmationString = code,
                    TaxiId = entity.TaxiId
                };

                var body = template.TransformText();
                var owner = entity.People.Email;
                var approver = entity.Approvers.People.Email;
                if (owner != null && approver != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", owner, body, "Potwierdzenie TakSii", approver);
                    client.SendEmail();
                    return true;
                }
            }

            return false;
        }

        internal void ConfirmTaxi(int id, string confirm)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);
            if (taxi.ConfirmCode == confirm)
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
            var taxi = GetEntityBy<Taxi>("TaxiId", id);
            taxi.TaxiCode = code;
            UpdateEntityBy("TaxiId", taxi);

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

        internal void ConfirmJoin(int id, string confirm)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);
            if (taxi.ConfirmCode == confirm)
            {
                taxi.IsConfirmed = true;
                Context.SaveChanges();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public bool SendJoinEmail(int id)
        {
            var entity = GetEntityBy<TaxiPeople>("Id", id);
            if (entity != null)
            {
                var code = Guid.NewGuid().ToString();
                entity.ConfirmCode = code;
                UpdateEntityBy("Id", entity);

                if (entity.TaxiId != null)
                {
                    var template = new ConfirmJoinTemplate
                    {
                        ConfirmationString = code,
                        TaxiId = (int) entity.TaxiId
                    };

                    var body = template.TransformText();
                    var joiner = entity.People.Email;
                    var owner = entity.Taxi.People.Email;
                    if (joiner != null && owner != null)
                    {
                        var client = new Emailer("taksii.test@gmail.com", joiner, body, "Potwierdzenie TakSii", owner);
                        client.SendEmail();
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
