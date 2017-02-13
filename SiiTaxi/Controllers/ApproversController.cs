using System.Web.Mvc;
using SiiTaxi.Models;

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
        public ActionResult Index(string name, string phone, string email, string emailAlt, PeopleViewModel peopleModel)
        {
            var person = new People()
            {
                Name = name,
                Phone = phone,
                Email = email,
                AltEmail = emailAlt,
                IsApprover = true
            };

            peopleModel.UpdateApproverByEmail(person);

            return RedirectToAction("Index", "Approvers");
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(int id, PeopleViewModel peopleModel)
        {
            peopleModel.Delete<People>("PeopleId", id);

            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        public ActionResult Delete(int id, PeopleViewModel peopleModel)
        {
            return View(peopleModel.GetEntityBy<People>("PeopleId", id));
        }

        [HttpPost]
        public ActionResult Update(int id, string name, string email, string emailAlt, string phone, PeopleViewModel peopleModel)
        {
            var approver = peopleModel.GetEntityBy<People>("PeopleId", id);
            approver.Name = name;
            approver.Email = email;
            approver.AltEmail = emailAlt;
            approver.Phone = phone;

            peopleModel.UpdateApproverByEmail(approver);

            return RedirectToAction("Index", "Approvers");
        }

        public ActionResult Update(int id, PeopleViewModel peopleModel)
        {
            return View(peopleModel.GetEntityBy<People>("PeopleId", id));
        }
    }
}
