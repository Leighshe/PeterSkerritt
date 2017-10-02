using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PeterSkerrittELearning.Controllers
{
    public class QuestionsController : Controller
    {
        [Authorize(Roles = "Administrator")]
        // GET: Questions
        public ActionResult ManageQuestions()
        {
            return View();
        }
    }
}