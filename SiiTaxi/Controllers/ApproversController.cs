using System;
using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class ApproversController : Controller
    {
        public ActionResult Index(PeopleViewModel peopleModel)
        {
            return View(peopleModel);
        }

        [HttpPost]
        public ActionResult Index(string name, string phone, string email, PeopleViewModel peopleModel)
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
                var entity = peopleModel.GetEntityBy<People>("Email", email);
                if (entity != null)
                {
                    entity.Name = name;
                    entity.Phone = phone;
                }
                else
                {
                    entity = new People()
                    {
                        Name = name,
                        Phone = phone,
                        Email = email
                    };
                }

                entity = peopleModel.UpdateEntityBy("Email", entity);

                if (entity.Approvers == null)
                {
                    var approver = new Approvers()
                    {
                        PeopleId = entity.PeopleId,
                        IsApprover = true
                    };

                    peopleModel.UpdateEntity(null, approver);
                }
                else
                {
                    entity.Approvers.IsApprover = true;
                    peopleModel.UpdateEntityBy("Email", entity);
                }
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
        public ActionResult DeletePost(int id, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityBy<People>("PeopleId", id);
            if (person != null)
            {
                person.Approvers.IsApprover = false;
                peopleModel.UpdateEntityBy("PeopleId", person);

                return RedirectToAction("Index", "Approvers");
            }
            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        public ActionResult Delete(int id, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityBy<People>("PeopleId", id);
            if (person != null && person.Approvers.IsApprover)
            {
                return View(person);
            }
            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpPost]
        public ActionResult Update(int id, string name, string email, string phone, PeopleViewModel peopleModel)
        {
            if (!Validators.IsEmailValid(email, true))
            {
                TempData["errorMessage"] = Messages.NotValidCompanyEmail;
                return View(peopleModel.GetEntityBy<People>("PeopleId", id));
            }
            if (!Validators.IsPhoneValid(phone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return View(peopleModel.GetEntityBy<People>("PeopleId", id));
            }

            try
            {
                var entity = peopleModel.GetEntityBy<People>("Email", email);
                if (entity != null && entity.Approvers.IsApprover)
                {
                    entity.Name = name;
                    entity.Phone = phone;
                    peopleModel.UpdateEntityBy("Email", entity);

                    TempData["successMessage"] = Messages.UpdateApproverSucceed;
                    return RedirectToAction("Index", "Approvers");
                }
                else
                {
                    TempData["errorMessage"] = Messages.UpdateApproverFailed;
                    return View(peopleModel.GetEntityBy<People>("PeopleId", id));
                }                
            }
            catch
            {
                TempData["errorMessage"] = Messages.UpdateApproverFailed;
                return View(peopleModel.GetEntityBy<People>("PeopleId", id));
            }

            TempData["failedMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        public ActionResult Update(int id, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityBy<People>("PeopleId", id);
            if (person != null && person.Approvers.IsApprover)
            {
                return View(person);
            }
            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }
    }
}
