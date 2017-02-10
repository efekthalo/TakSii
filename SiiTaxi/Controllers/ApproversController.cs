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
        public ActionResult Index(string name, string phone, string email, string emailAlt, TaxiViewModel taxiModel, PeopleViewModel peopleModel)
        {
            var person = new People()
            {
                Name = name,
                Phone = phone,
                Email = email,
                AltEmail = emailAlt,
                IsApprover = true
            };

            peopleModel.UpdatePeopleByName(person);

            return RedirectToAction("Index", "Approvers");
        }


        [HttpPost]
        public ActionResult Delete(int id, PeopleViewModel peopleModel)
        {
            peopleModel.Delete(id);

            return RedirectToAction("Index", "Approvers");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            return View(new PeopleViewModel().GetEntityByKey(id));
        }


        [HttpPost]
        public ActionResult Update(int id, string name, string email, string emailAlt, string phone, PeopleViewModel peopleModel)
        {
            var person = peopleModel.GetEntityByKey(id);
            person.Name = name;
            person.Email = email;
            person.AltEmail = emailAlt;
            person.Phone = phone;

            peopleModel.UpdatePeopleByKey(person);

            return RedirectToAction("Index", "Approvers");
        }

        public ActionResult Update(int id)
        {
            return View(new PeopleViewModel().GetEntityByKey(id));
        }
    }
}
