using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloTutor.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

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

    public ActionResult Classes_Read([DataSourceRequest]DataSourceRequest request)
    {
      var HiTutor = new HelloTutorEntities();
      IQueryable<Class> clss = HiTutor.Classes;
      DataSourceResult result = clss.ToDataSourceResult(request);
      return Json(result);

    }

  }
}