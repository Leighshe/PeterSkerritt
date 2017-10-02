using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class CourseMaterialController : Controller
    {
        [Authorize(Roles = "Administrator")]
        // GET: UploadMaterial
        public ActionResult UploadMaterial()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: ManageMaterial
        public ActionResult ManageMaterial()
        {
            return View();
        }
    }
}