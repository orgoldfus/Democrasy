using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Democrasy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Democrasy - Top Rated Manifests";

            return View();
        }

        public ActionResult New()
        {
            ViewBag.Title = "Democrasy - Newest Manifests";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Title = "About";

            return View();
        }
    }
}
