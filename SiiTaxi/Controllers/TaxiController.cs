using SiiTaxi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using SiiTaxi.Providers;
using System.Linq;
using SiiTaxi.Email;

namespace SiiTaxi.Controllers
{
    public class TaxiController : Controller
    {
        private readonly SiiTaxiEntities _context = new SiiTaxiEntities();

        [HttpPost]
        public ActionResult New(string ownerName, string ownerPhone, string time,
            string ownerEmail, string przejazdFrom, string przejazdTo,
            List<string> adds, int approver)
        {
            var isBigTaxi = Request.Form["IsBigTaxi"] == "on";
            var order = Request.Form["order"] == "on";
            TempData["formData"] = Request.Form;

            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(encodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return View(new PeopleViewModel());
            }
            if (!Validators.IsEmailValid(ownerEmail, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(new PeopleViewModel());
            }
            if (!Validators.IsPhoneValid(ownerPhone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return View(new PeopleViewModel());
            }

            DateTime parsedTime;
            DateTime.TryParseExact(time, "dd/MM/yyyy HH:mm", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedTime);

            if (parsedTime < DateTime.Now)
            {
                TempData["errorMessage"] = Messages.NotValidDate;
                return View(new PeopleViewModel());
            }

            var owner = _context.People.FirstOrDefault(x => x.Email == ownerEmail);
            if (owner != null)
            {
                TryUpdateModel(owner);
            }
            else
            {
                owner = _context.People.Add(new People() { Name = ownerName, Phone = ownerPhone, Email = ownerEmail });
            }

            var taxi = new Taxi
            {
                From = przejazdFrom,
                To = przejazdTo,
                Time = parsedTime,
                Owner = owner.PeopleId,
                Approver = approver,
                IsBigTaxi = isBigTaxi,
                Order = !order,
                ConfirmCode = Guid.NewGuid().ToString()
            };

            try
            {
                taxi = _context.Taxi.Add(taxi);

                if (adds != null)
                {
                    foreach (var add in adds.Distinct())
                    {
                        if (add == ownerEmail)
                        {
                            continue;
                        }

                        var other = _context.People.FirstOrDefault(x => x.Email == add);
                        if (other != null)
                        {
                            TryUpdateModel(other);
                        }
                        else
                        {
                            other = _context.People.Add(new People() { Name = "", Email = add });
                        }

                        var taxiPeople = new TaxiPeople
                        {
                            TaxiId = taxi.TaxiId,
                            PeopleId = other.PeopleId,
                            ConfirmCode = Guid.NewGuid().ToString(),
                            IsConfirmed = true
                        };

                        _context.TaxiPeople.Add(taxiPeople);
                    }
                }

                SendConfirmEmail(taxi);
            }
            catch
            {
                TempData["errorMessage"] = Messages.DatabaseError;
                return View(new PeopleViewModel());
            }

            TempData["successMessage"] = Messages.AddNewTaxiSuccess;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpGet]
        public ActionResult New()
        {
            return View();
        }

        public ActionResult Join(int id)
        {
            var maxInTaxi = 3;
            TempData["formData"] = Request.Form;

            var taxi = _context.Taxi.Find(id);
            if (taxi != null)
            {
                if (taxi.Time < DateTime.Now)
                {
                    TempData["errorMessage"] = Messages.TaxiExpired;
                    return RedirectToAction("Index", "Taxi");
                }

                if (taxi.IsBigTaxi)
                    maxInTaxi = 6;

                if (taxi.TaxiPeople.Count <= maxInTaxi)
                    return View(taxi);

                TempData["errorMessage"] = Messages.TaxiFull;
                return RedirectToAction("Index", "Taxi");
            }

            TempData["errorMessage"] = Messages.TaxiNotFound;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpPost]
        public ActionResult Join(int id, string name, string phone, string email)
        {
            var maxInTaxi = 3;

            var taxi = _context.Taxi.Find(id);
            if (taxi == null)
            {
                TempData["errorMessage"] = Messages.TaxiNotFound;
                return RedirectToAction("Index", "Taxi");
            }

            if (taxi.IsBigTaxi)
                maxInTaxi = 6;

            if (taxi.Time < DateTime.Now)
            {
                TempData["errorMessage"] = Messages.TaxiExpired;
                return RedirectToAction("Index", "Taxi");
            }

            TempData["formData"] = Request.Form;
            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(encodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return View(taxi);
            }
            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(taxi);
            }
            if (!Validators.IsPhoneValid(phone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return View(taxi);
            }
            if (taxi.TaxiPeople.Count >= maxInTaxi)
            {
                TempData["errorMessage"] = Messages.TaxiFull;
                return RedirectToAction("Index", "Taxi");
            }
            if (email == taxi.People.Email || taxi.TaxiPeople.Any(x => x.People.Name == name))
            {
                TempData["errorMessage"] = Messages.JoinedAlready;
                return RedirectToAction("Index", "Taxi");
            }

            try
            {
                var other = _context.People.FirstOrDefault(x => x.Email == email);
                if (other != null)
                {
                    TryUpdateModel(other);
                }
                else
                {
                    other = _context.People.Add(new People() { Name = "", Phone = phone, Email = email });
                }

                var taxiPeople = new TaxiPeople { TaxiId = id, PeopleId = other.PeopleId, ConfirmCode = Guid.NewGuid().ToString() };
                taxiPeople = _context.TaxiPeople.Add(taxiPeople);

                SendJoinEmail(taxiPeople);
            }
            catch
            {
                TempData["errorMessage"] = Messages.DatabaseError;
                return View(taxi);
            }

            TempData["successMessage"] = Messages.IncludeTaxiSuccess;
            return RedirectToAction("Index", "Taxi");
        }

        public ActionResult Index(DateTime? date = null)
        {
            var localDate = (date == null || date < DateTime.Now ? DateTime.Now : (DateTime)date);
            IQueryable<Taxi> taxis = _context.Taxi.Where(x => (x.Time.Year == localDate.Year) && (x.Time.Month == localDate.Month) && (x.Time.Day == localDate.Day));
            return View(taxis);
        }

        [HttpGet]
        public ActionResult Confirm(int id, string code)
        {
            var taxi = new TaxiViewModel().GetEntityBy<Taxi>("TaxiId", id);
            if (taxi != null && taxi.ConfirmCode == code)
            {
                if (taxi.Time < DateTime.Now)
                {
                    TempData["errorMessage"] = Messages.TaxiExpired;
                    return RedirectToAction("Index", "Taxi");
                }
                if (taxi.IsConfirmed)
                {
                    TempData["errorMessage"] = Messages.TaxiConfirmed;
                    return RedirectToAction("Index", "Taxi");
                }
                TempData["code"] = code;
                return View(taxi);
            }
            TempData["errorMessage"] = Messages.TaxiNotFound;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpPost]
        public ActionResult Confirm(int id, string code)
        {
            try
            {
                taxiModel.Confirm(id, code);
            }
            catch
            {
                TempData["errorMessage"] = Messages.ConfirmFailed;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }
            TempData["successMessage"] = Messages.ConfirmSucceed;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpGet]
        public ActionResult ConfirmJoin(int id, string code, TaxiViewModel taxiModel)
        {
            var taxiPeople = new TaxiViewModel().GetEntityBy<TaxiPeople>("Id", id);
            if (taxiPeople != null && taxiPeople.ConfirmCode == code)
            {
                if (taxiPeople.IsConfirmed)
                {
                    TempData["errorMessage"] = Messages.TaxiConfirmed;
                    return RedirectToAction("Index", "Taxi");
                }
                TempData["code"] = code;
                return View(taxiPeople);
            }
            TempData["errorMessage"] = Messages.TaxiNotFound;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpPost]
        public ActionResult ConfirmJoin(int id, string code)
        {
            try
            {
                new TaxiViewModel().ConfirmJoin(id, code);
            }
            catch
            {
                TempData["errorMessage"] = Messages.ConfirmFailed;
                return View(new TaxiViewModel().GetEntityBy<TaxiPeople>("Id", id));
            }
            TempData["successMessage"] = Messages.ConfirmSucceed;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpPost]
        public ActionResult BugReport(string name, string description)
        {
            var encodedResponse = Request.Form["g-Recaptcha-Response"];
            if (!Validators.IsCaptchaValid(encodedResponse))
            {
                TempData["errorMessage"] = Messages.NotValidCaptcha;
                return RedirectToAction("Index", "Taxi");
            }

            if (name != null && description != null)
            {
                var body = $"<p>Zgłaszający: {name}</p><p>Opis błędu: {description}</p>";
                var client = new Emailer("taksii.test@gmail.com", "taksii.test@gmail.com", body, "Zgłoszenie błędu TakSii");
                client.SendEmail();

                TempData["successMessage"] = Messages.BugReported;
                return RedirectToAction("Index", "Taxi");
            }

            TempData["successMessage"] = Messages.BugFailed;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpGet]
        public ActionResult Remove(int id, string code)
        {
            var taxi = new TaxiViewModel().GetEntityBy<Taxi>("TaxiId", id);
            if (taxi != null)
            {
                if (taxi.IsOrdered)
                {
                    TempData["errorMessage"] = Messages.TaxiOrdered;
                    return RedirectToAction("Index", "Taxi");
                }

                TempData["code"] = code;
                return View(taxi);
            }

            TempData["errorMessage"] = Messages.TaxiNotFound;
            return RedirectToAction("Index", "Taxi");
        }

        [HttpPost]
        public ActionResult Remove(int id, string code)
        {
            try
            {
                taxiModel.Remove(id, code);
            }
            catch
            {
                TempData["errorMessage"] = Messages.RemoveFailed;
                return View(taxiModel.GetEntityBy<Taxi>("TaxiId", id));
            }

            TempData["successMessage"] = Messages.RemoveSucceed;
            return RedirectToAction("Index", "Taxi");
        }

        public bool SendConfirmEmail(Taxi taxi)
        {
            var template = new ConfirmTemplate
            {
                Taxi = taxi
            };

            var body = template.TransformText();
            var owner = taxi.People.Email;
            var approver = taxi.Approvers.People.Email;
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
            if (entity.TaxiId != null)
            {
                var template = new ConfirmJoinTemplate
                {
                    ConfirmationString = entity.ConfirmCode,
                    Id = entity.Id
                };

                var body = template.TransformText();
                var joiner = entity.People.Email;
                var owner = entity.Taxi.People.Email;
                if (joiner != null && owner != null)
                {
                    var client = new Emailer("taksii.test@gmail.com", joiner, body, "Potwierdzenie dołączenia - TakSii", owner);
                    client.SendEmail();
                    return true;
                }
            }

            return false;
        }
    }
}
