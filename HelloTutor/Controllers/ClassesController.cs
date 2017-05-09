using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloTutor.Models;

namespace HelloTutor.Controllers
{
    public class ClassesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult _getClassesByRoleId(int RoleID)
        {
            ClassesViewModel rvm = new ClassesViewModel();

            var classdata = rvm.GetClassesForList(RoleID);

            return Json(classdata, JsonRequestBehavior.AllowGet);
        }
    }
}