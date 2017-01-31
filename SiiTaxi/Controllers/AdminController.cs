using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiiTaxi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
