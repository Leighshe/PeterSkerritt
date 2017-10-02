using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class ReportingController : Controller
    {
        [Authorize(Roles = "Administrator")]
        // GET: ManageReportGroups
        public ActionResult ManageReportGroups()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: ChangeReport
        public ActionResult ChangeReport()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: SevenDayReport
        public ActionResult SevenDayReport()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: EmailAddressExport
        public ActionResult EmailAddressExport()
        {
            return View();
        }
    }
}