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

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, Class class_)
    {

      if (class_ != null &&
          !(string.IsNullOrWhiteSpace(class_.Name) &&
            string.IsNullOrWhiteSpace(class_.Description) &&
            string.IsNullOrWhiteSpace(class_.MaxEnrollment.ToString()) &&
            string.IsNullOrWhiteSpace(class_.Role.ToString()) &&
            class_.EndDate == null 

           )
      )
      {
        using (var HelloTutor = new HelloTutorEntities())
        {
          //Added Guid Generator since empty guid is causing default NewId() in db to fail leaving guid value as empty value.
          class_.GUID = Guid.NewGuid();
          HelloTutor.Classes.Add(class_);
          HelloTutor.SaveChanges();
          //forcing modelstate.Isvalid true here to force proper refresh of grid
          //Not sure what I am doing wrong but caused expected behavior
          //quick way to get working until we really need to fix
          ModelState.Clear();
        }

      }

      return Json(new[] { class_ }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Class class_)
    {
      if (class_ != null && ModelState.IsValid)
      {
        //productService.Update(product);
        using (var HelloTutor = new HelloTutorEntities())
        {
          HelloTutor.Classes.Attach(class_);
          HelloTutor.SaveChanges();
        }
      }

      return Json(new[] { class_ }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Class class_)
    {
      if (class_ != null)
      {
        using (var HelloTutor = new HelloTutorEntities())
        {

          //Remove all associated roles
          foreach (var tc in class_.TutorsClasses)
          {
            HelloTutor.TutorsClasses.Remove(tc);
          }
          //Remove Tutor
          HelloTutor.Classes.Attach(class_);
          HelloTutor.Classes.Remove(class_);
          HelloTutor.SaveChanges();
        }
      }

      return Json(new[] { class_ }.ToDataSourceResult(request, ModelState));
    }

  }
}