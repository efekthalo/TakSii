using System.Web.Mvc;
using SiiTaxi.Models;
using SiiTaxi.Providers;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class ApproversController : Controller
    {
        public ActionResult Index()
        {
            return View(new PeopleViewModel());
        }

        [HttpPost]
        public ActionResult Index(string name, string phone, string email, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
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
                var person = new People()
                {
                    Name = name,
                    Phone = phone,
                    Email = email,
                    IsApprover = true
                };
                peopleModel.UpdateApproverByEmail(person);
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
        public ActionResult Delete(int id, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityByKey(id);
            if (person != null)
            {
                person.IsApprover = false;
                peopleModel.UpdateApproverByEmail(person);

                return RedirectToAction("Index", "Approvers");
            }
            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var person = new PeopleViewModel().GetEntityByKey(id);
            if (person != null)
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
                return View(new PeopleViewModel().GetEntityByKey(id));
            }
            if (!Validators.IsPhoneValid(phone))
            {
                TempData["errorMessage"] = Messages.NotValidPhone;
                return View(new PeopleViewModel().GetEntityByKey(id));
            }

            try
            {
                var approver = peopleModel.GetEntityByKey(id);
                approver.Name = name;
                approver.Email = email;
                approver.Phone = phone;

                peopleModel.UpdateApproverByEmail(approver);
            }
            catch
            {
                TempData["errorMessage"] = Messages.UpdateApproverFailed;
                return View(new PeopleViewModel().GetEntityByKey(id));
            }

            TempData["successMessage"] = Messages.UpdateApproverSucceed;
            return RedirectToAction("Index", "Approvers");
        }

        public ActionResult Update(int id)
        {
            var person = new PeopleViewModel().GetEntityByKey(id);
            if (person != null)
            {
                return View(person);
            }
            TempData["errorMessage"] = Messages.ApproverNotFound;
            return RedirectToAction("Index", "Approvers");
        }
    }
}
