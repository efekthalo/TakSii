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
                var owner = GetEntityBy<People>("PeopleId", entity.Owner).Email;
                var approver = GetEntityBy<People>("PeopleId", entity.Approver).Email;
                if (owner != null && approver != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", owner, body, "Potwierdzenie TakSii", approver);
                    client.SendEmail();
                    return true;
                }
            }

            return false;
        }

        internal void SendCode(int id, string code, string action)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);

            if (taxi.IsConfirmed)
            {
                string body = "";
                switch(action)
                {
                    case "Send":
                        var codeTemplate = new SendCodeTemplate
                        {
                            TaxiFrom = taxi.From,
                            TaxiTo = taxi.To,
                            TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                            TaxiCodeString = code
                        };
                        body = codeTemplate.TransformText();
                        break;
                    case "Order":
                        var orderTemplate = new SendCodeAndOrderedTemplate
                        {
                            TaxiFrom = taxi.From,
                            TaxiTo = taxi.To,
                            TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                            TaxiCodeString = code
                        };
                        body = orderTemplate.TransformText();
                        break;
                }
                
                var client = new Emailer("taksii.test@gmail.com", taxi.People.Email, body, "Kod TaxSii");
                client.SendEmail();

                taxi.TaxiCode = code;
                taxi.IsOrdered = true;
                UpdateEntityBy("TaxiId", taxi);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        internal void Confirm(int id, string confirm)
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

        internal void ConfirmJoin(int id, string confirm)
        {
            var taxiPeople = GetEntityBy<TaxiPeople>("Id", id);
            if (taxiPeople.ConfirmCode == confirm)
            {
                taxiPeople.IsConfirmed = true;
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
                        Id = (int) entity.Id
                    };

                    var body = template.TransformText();
                    var joiner = GetEntityBy<People>("PeopleId", entity.PeopleId).Email;
                    var owner = GetEntityBy<People>("PeopleId", entity.Taxi.Owner).Email;
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
