using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;
using System.Linq;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class ApproversController : Controller
    {
        private readonly SiiTaxiEntities _context = new SiiTaxiEntities();

        [HttpGet]
        [ActionName("Index")]
        public ActionResult IndexGet()
        {
            IQueryable<Approvers> approvers = from a in _context.Approvers
                                              where a.IsApprover
                                              select a;
            return View(approvers);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexPost(string name, string phone, string email)
        {
            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return RedirectToAction("Index", "Approvers");
            }
            if (!Validators.IsPhoneValid(phone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return RedirectToAction("Index", "Approvers");
            }

            try
            {
                var approver = _context.People.FirstOrDefault(x => x.Email == email);
                if (approver != null)
                {
                    TryUpdateModel(approver);
                }
                else
                {
                    approver = _context.People.Add(new People() { Name = name, Phone = phone, Email = email });
                }

                if (approver.Approvers == null)
                {
                    _context.Approvers.Add(new Approvers() { PeopleId = approver.PeopleId, IsApprover = true });
                }
                else
                {
                    approver.Approvers.IsApprover = true;
                }

                _context.SaveChanges();
            }
            catch
            {
                TempData["errorMessage"] = Messages.AddNewApproverFailed;
                return RedirectToAction("Index", "Approvers");
            }

            TempData["successMessage"] = Messages.AddNewApproverSucceed;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {
            var person = _context.People.Find(id);
            if (person != null)
            {
                person.Approvers.IsApprover = false;
                _context.SaveChanges();

                return RedirectToAction("Index", "Approvers");
            }

            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            var person = _context.People.Find(id);
            if (person?.Approvers != null)
            {
                if (person.Approvers.IsApprover)
                {
                    return View(person);
                }
            }

            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpPost]
        [ActionName("Update")]
        public ActionResult UpdatePost(int id, string name, string email, string phone)
        {
            var person = _context.People.Find(id);

            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(person);
            }
            if (!Validators.IsPhoneValid(phone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return View(person);
            }

            try
            {
                if (person?.Approvers != null)
                {
                    TryUpdateModel(person);
                    _context.SaveChanges();

                    TempData["successMessage"] = Messages.UpdateApproverSucceed;
                    return RedirectToAction("Index", "Approvers");
                }

                TempData["errorMessage"] = Messages.ApproverNotFound;
                return View(person);
            }
            catch
            {
                TempData["errorMessage"] = Messages.UpdateApproverFailed;
                return View(person);
            }
        }

        [HttpGet]
        [ActionName("Update")]
        public ActionResult UpdateGet(int id)
        {
            var person = _context.People.Find(id);
            if (person?.Approvers != null)
            {
                if (person.Approvers.IsApprover)
                {
                    return View(person);
                }
            }

            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }
    }
}
