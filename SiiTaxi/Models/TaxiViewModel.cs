using SiiTaxi.Email;
using System;
using System.Linq;

namespace SiiTaxi.Models
{
    public sealed class TaxiViewModel : AbstractViewModel
    {
        public IQueryable<Taxi> Taxis;

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

        public DateTime DateInput { get; set; }

        public void Confirm(int id, string confirm)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);
            if (taxi.ConfirmCode == confirm)
            {
                taxi.IsConfirmed = true;
                Context.SaveChanges();
                SendNotification(taxi);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private void SendNotification(Taxi taxi)
        {
            var codeTemplate = new SendNotificationTemplate
            {
                TaxiFrom = taxi.From,
                TaxiTo = taxi.To,
                TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
            };
            var body = codeTemplate.TransformText();

            var client = new Emailer("taksii.test@gmail.com", "aguja@pl.sii.eu", body, "Nowa potwierdzona taksówka - TakSii");
            client.SendEmail();
        }

        public void Remove(int id, string confirm)
        {
            var taxi = GetEntityBy<Taxi>("TaxiId", id);
            var joiner = taxi.TaxiPeople.FirstOrDefault(x => x.ConfirmCode == confirm);

            if (taxi.ConfirmCode == confirm)
            {
                SendRemoveToJoiners(taxi);
                Delete(taxi);
            }
            else if (joiner != null)
            {
                SendRemoveToOwner(taxi, joiner);
                Delete(joiner);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void SendCode(Taxi taxi, string code, string action)
        {
            if (taxi.IsConfirmed)
            {
                string body = "";
                switch (action)
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

                var client = new Emailer("taksii.test@gmail.com", taxi.People.Email, body, "Kod na taksówke - TakSii");
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

        public bool SendConfirmEmail(Taxi taxi)
        {
            var code = Guid.NewGuid().ToString();
            taxi.ConfirmCode = code;
            UpdateEntityBy("TaxiId", taxi);

            var template = new ConfirmTemplate
            {
                Taxi = taxi
            };

            var body = template.TransformText();
            var owner = GetEntityBy<People>("PeopleId", taxi.Owner).Email;
            var approver = GetEntityBy<People>("PeopleId", taxi.Approver).Email;
            if (owner != null && approver != null)
            {
                var client = new Emailer("taksii.test@gmail.com", owner, body, "Potwierdzenie taksówki - TakSii", approver);
                client.SendEmail();
                return true;
            }

            return false;
        }

        public bool SendJoinEmail(TaxiPeople entity)
        {
            var code = Guid.NewGuid().ToString();
            entity.ConfirmCode = code;
            UpdateEntityBy("Id", entity);

            if (entity.TaxiId != null)
            {
                var template = new ConfirmJoinTemplate
                {
                    ConfirmationString = code,
                    Id = entity.Id
                };

                var body = template.TransformText();
                var joiner = GetEntityBy<People>("PeopleId", entity.PeopleId).Email;
                var owner = GetEntityBy<People>("PeopleId", entity.Taxi.Owner).Email;
                if (joiner != null && owner != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", joiner, body, "Potwierdzenie dołączenia - TakSii", owner);
                    client.SendEmail();
                    return true;
                }
            }

            return false;
        }

        public void SendRemoveToJoiners(Taxi entity)
        {
            foreach (var taxiPeople in entity.TaxiPeople)
            {
                var codeTemplate = new SendRemoveToJoinersTemplate
                {
                    TaxiFrom = entity.From,
                    TaxiTo = entity.To,
                    TaxiTime = entity.Time.ToString("HH:mm dd/MM/yyyy")
                };
                var body = codeTemplate.TransformText();

                var client = new Emailer("taksii.test@gmail.com", taxiPeople.People.Email, body, "Taksówka została odwołana - TakSii");
                client.SendEmail();
            }
        }

        public void ConfirmJoin(int id, string confirm)
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

        private void SendRemoveToOwner(Taxi taxi, TaxiPeople joiner)
        {
            var codeTemplate = new SendRemoveToOwnerTemplate
            {
                TaxiFrom = taxi.From,
                TaxiTo = taxi.To,
                TaxiTime = taxi.Time.ToString("HH:mm dd/MM/yyyy"),
                Joiner = joiner
            };
            var body = codeTemplate.TransformText();

            var client = new Emailer("taksii.test@gmail.com", taxi.People.Email, body, "Wypisano " + joiner.People.Email + " z taksówki - TakSii");
            client.SendEmail();
        }
    }
}