using HelloTutor.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace HelloTutor.Controllers
{
    public class ClassesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            var rvm = new RoleViewModel();
            ViewData["roles"] = rvm.GetRolesForListBox();

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
            IQueryable<HelloTutor.Class> classes = HiTutor.Classes;
            DataSourceResult result = classes.ToDataSourceResult(request, class_ => new ClassesViewModel
            {
                ClassName = class_.Name,
                ClassDescription = class_.Description,
                MaxEnrollment = class_.MaxEnrollment,
                StartDate = class_.StartDate,
                EndDate = class_.EndDate,
                ClassGuid = class_.GUID,
                RoleID = class_.Role,
                ClassID = class_.Id
            });

            //DataSourceResult result = clss.ToDataSourceResult(request);
            return Json(result);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, ClassesViewModel cvm)
        {

            if (cvm != null &&
                !(string.IsNullOrWhiteSpace(cvm.ClassName) &&
                  string.IsNullOrWhiteSpace(cvm.ClassDescription) &&
                  string.IsNullOrWhiteSpace(cvm.MaxEnrollment.ToString()) &&
                  string.IsNullOrWhiteSpace(cvm.RoleID.ToString()) &&
                  cvm.EndDate == null &&
                  cvm.StartDate == null
                 )
            )
            {
                using (var HelloTutor = new HelloTutorEntities())
                {
                    var class_ = new Class()
                    {
                        Name = cvm.ClassName,
                        Description = cvm.ClassDescription,
                        Role = cvm.RoleID,
                        MaxEnrollment = cvm.MaxEnrollment,
                        StartDate = cvm.StartDate,
                        EndDate = cvm.EndDate,
                        GUID = Guid.NewGuid()
                    };

                    HelloTutor.Classes.Add(class_);
                    HelloTutor.SaveChanges();
                    //forcing modelstate.Isvalid true here to force proper refresh of grid
                    //Not sure what I am doing wrong but caused expected behavior
                    //quick way to get working until we really need to fix
                    cvm.ClassGuid = class_.GUID;
                    cvm.ClassID = class_.Id;
                    ModelState.Clear();
                }

            }

            return Json(new[] { cvm }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, ClassesViewModel cvm)
        {
            if (cvm != null && ModelState.IsValid)
            {
                var class_ = new Class()
                {
                    Name = cvm.ClassName,
                    Description = cvm.ClassDescription,
                    Role = cvm.RoleID,
                    MaxEnrollment = cvm.MaxEnrollment,
                    StartDate = cvm.StartDate,
                    EndDate = cvm.EndDate,
                    GUID = cvm.ClassGuid,
                    Id = cvm.ClassID
                };
                //productService.Update(product);
                using (var HelloTutor = new HelloTutorEntities())
                {
                    HelloTutor.Classes.Attach(class_);
                    HelloTutor.Entry(class_).State = EntityState.Modified;
                    HelloTutor.SaveChanges();
                }
            }

            return Json(new[] { cvm }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ClassesViewModel cvm)
        {
            if (cvm != null)
            {
                using (var HelloTutor = new HelloTutorEntities())
                {

                    var class_ = new Class()
                    {
                        Name = cvm.ClassName,
                        Description = cvm.ClassDescription,
                        Role = cvm.RoleID,
                        MaxEnrollment = cvm.MaxEnrollment,
                        StartDate = cvm.StartDate,
                        EndDate = cvm.EndDate,
                        GUID = cvm.ClassGuid,
                        Id = cvm.ClassID
                    };

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

            return Json(new[] { cvm }.ToDataSourceResult(request, ModelState));
        }

    }
}