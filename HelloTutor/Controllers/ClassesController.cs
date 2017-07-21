using HelloTutor.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BootstrapTable;

namespace HelloTutor.Controllers
{
    public class ClassesController : Controller
    {
        // GET: Roles
        [Authorize(Roles = "Admin")]
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

        public Boolean _removeTutorFromClass(int ClassID, int TutorID)
        {
            HelloTutorEntities db = new HelloTutorEntities();
            TutorsClass tc = (from t in db.TutorsClasses where t.ClassId == ClassID && t.TutorId == TutorID select t).FirstOrDefault();

            if(tc != null)
            {
                db.TutorsClasses.Remove(tc);
                db.SaveChanges();
                return true;
            }

            return false;
        }

        // get Tutors to Classes
        [Authorize(Roles = "Admin")]
        public ActionResult GetTutorsToClasses()
        {
            var rvm = new RoleViewModel();
            ViewData["roles"] = rvm.GetRolesForListBox();

            return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult ViewClassRegistrations(Int32 ID)
        {
            HelloTutorEntities db = new HelloTutorEntities();
            // get the class name
            Class thisClass = (from c in db.Classes where c.Id == ID select c).FirstOrDefault();

            String sql = "Select * from Tutors where Id in (select TutorID from TutorsClasses where ClassID = " + ID + ") order by LastName, FirstName";

            List<Tutor> tutorList = db.Database.SqlQuery<Tutor>(sql).ToList();

            ViewData["ClassName"] = thisClass.Name;
            ViewData["tutors"] = tutorList;
            ViewBag.ClassID = thisClass.Id;

            return View();
        }

        public ActionResult tutors_Classes_Read([DataSourceRequest]DataSourceRequest request)
        {
            TutorsClassesViewModel tcv = new TutorsClassesViewModel();

            DataSourceResult result = tcv.GetTutorsClasses().ToDataSourceResult(request);

            return Json(result);
        }


        private List<ClassesViewModel> getClasses()
        {
            var db = new HelloTutorEntities();
            List<ClassesViewModel> classes = (from c in db.Classes where c.IsDeleted == false select new ClassesViewModel
            {
                ClassName = c.Name,
                ClassDescription = c.Description,
                MaxEnrollment = c.MaxEnrollment,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                ClassGuid = c.GUID,
                RoleID = c.Role,
                ClassID = c.Id
            }).ToList();

            return classes;
        }

        public JsonResult _getClassesPaged(int offset, int limit, string search, string sort, string order)
        {
            var classes = getClasses();
            var model = new
            {
                total = classes.Count(),
                rows = classes.Skip((offset / limit) * limit).Take(limit),
            };
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Classes_Read([DataSourceRequest]DataSourceRequest request)
        {
            var db = new HelloTutorEntities();
            IQueryable<HelloTutor.Class> classes = (from c in db.Classes where c.IsDeleted == false select c);
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, ClassesViewModel cvm)
        {
            if (cvm != null)
            {
                using (var db = new HelloTutorEntities())
                {
                    String sql = "update Classes set IsDeleted = 1 where Id = " + cvm.ClassID;
                    db.Database.ExecuteSqlCommand(sql);
                }
            }

            return Json(new[] { cvm }.ToDataSourceResult(request, ModelState));
        }

    }
}