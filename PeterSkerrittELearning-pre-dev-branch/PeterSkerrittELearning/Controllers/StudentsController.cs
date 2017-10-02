using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class StudentsController : Controller
    {
        [Authorize(Roles = "Administrator")]
        // GET: AddStudent
        public ActionResult AddStudent()
        {
            return View();
        }

        [Authorize(Roles = "Administrator")]
        // GET: SearchStudents
        public ActionResult SearchStudents()
        {
            return View();
        }
    }
}