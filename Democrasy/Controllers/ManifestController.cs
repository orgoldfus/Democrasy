using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemocrasyModel;
using DemocrasyBackOffice.Manifesto;
using System.Net;

namespace Democrasy.Controllers
{
    public class ManifestController : Controller
    {
        ManifestServices service;

        public ManifestController()
        {
            service = new ManifestServices();
        }

        [HttpGet]
        public ActionResult GetTopRanked(int numOfRecords, int skip = 0)
        {
            int MAX = 12;

            numOfRecords = numOfRecords > MAX ? MAX : numOfRecords;
            var result = service.GetTopManifests(numOfRecords, skip);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNewest(int numOfRecords, int skip = 0)
        {
            int MAX = 12;

            numOfRecords = numOfRecords > MAX ? MAX : numOfRecords;
            var result = service.GetNewManifests(numOfRecords, skip);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Create(string text, string author)
        {
            var result = service.CreateManifest(text, author);

            return Redirect("/Home/New");
        }

        [HttpPost]
        public bool Upvote(string id)
        {
            var result = service.UpvoteManifest(id);

            return result;
        }

        [HttpPost]
        public bool Downvote(string id)
        {
            var result = service.DownvoteManifest(id);

            return result;
        }
    }
}