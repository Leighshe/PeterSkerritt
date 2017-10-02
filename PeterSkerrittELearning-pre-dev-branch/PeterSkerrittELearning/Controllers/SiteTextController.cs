using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class SiteTextController : Controller
    {
        [Authorize(Roles = "Administrator")]
        // GET: ManageText
        public ActionResult ManageText()
        {
            return View();
        }
    }
}