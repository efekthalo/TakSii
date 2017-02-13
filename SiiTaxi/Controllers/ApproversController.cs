using System.Web.Mvc;
using SiiTaxi.Models;

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
            var person = new People()
            {
                Name = name,
                Phone = phone,
                Email = email,
                IsApprover = true
            };

            peopleModel.UpdateApproverByEmail(person);

            return RedirectToAction("Index", "Approvers");
        }


        [HttpPost]
        public ActionResult Delete(int id, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityByKey(id);
            person.IsApprover = false;
            peopleModel.UpdateApproverByEmail(person);

            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(new PeopleViewModel().GetEntityByKey(id));
        }


        [HttpPost]
        public ActionResult Update(int id, string name, string email, string phone, PeopleViewModel peopleModel)
        {
            var approver = peopleModel.GetEntityByKey(id);
            approver.Name = name;
            approver.Email = email;
            approver.Phone = phone;

            peopleModel.UpdateApproverByEmail(approver);

            return RedirectToAction("Index", "Approvers");
        }

        public ActionResult Update(int id)
        {
            return View(new PeopleViewModel().GetEntityByKey(id));
        }
    }
}
