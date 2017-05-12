using HelloTutor;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HelloTutor.Controllers
{
    public class TutorsController : Controller
    {

        // GET: Tutors
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Tutors_Read([DataSourceRequest]DataSourceRequest request)
        {
            var HelloTutor = new HelloTutorEntities();
            IQueryable<Tutor> tutors = HelloTutor.Tutors;
            DataSourceResult result = tutors.ToDataSourceResult(request);
            return Json(result);

        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, Tutor tutor)
        {
            if (tutor != null &&
                !(string.IsNullOrWhiteSpace(tutor.LastName) &&
                  string.IsNullOrWhiteSpace(tutor.FirstName) &&
                  string.IsNullOrWhiteSpace(tutor.Email) &&
                  string.IsNullOrWhiteSpace(tutor.PhoneNumber)
                 )
            )
            {
                using (var HelloTutor = new HelloTutorEntities())
                {
                    //Added Guid Generator since empty guid is causing default NewId() in db to fail leaving guid value as empty value.
                    tutor.GUID = Guid.NewGuid();
                    HelloTutor.Tutors.Add(tutor);
                    HelloTutor.SaveChanges();
                    //forcing modelstate.Isvalid true here to force proper refresh of grid
                    //Not sure what I am doing wrong but caused expected behavior
                    //quick way to get working until we really need to fix
                    ModelState.Clear();
                }

            }

            return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Tutor tutor)
        {
            if (tutor != null && ModelState.IsValid)
            {
                //productService.Update(product);
                using (var HelloTutor = new HelloTutorEntities())
                {
                    HelloTutor.Tutors.Attach(tutor);
                    HelloTutor.Entry(tutor).State = EntityState.Modified;
                    HelloTutor.SaveChanges();
                }
            }

            return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Tutor tutor)
        {
            if (tutor != null)
            {
                using (var HelloTutor = new HelloTutorEntities())
                {

                    //Remove all associated classes
                    foreach (var tc in tutor.TutorsClasses)
                    {
                        HelloTutor.TutorsClasses.Remove(tc);
                    }

                    //Remove all associated roles
                    foreach (var tr in tutor.TutorsRoles)
                    {
                        HelloTutor.TutorsRoles.Remove(tr);
                    }
                    //Remove Tutor
                    HelloTutor.Tutors.Attach(tutor);
                    HelloTutor.Tutors.Remove(tutor);
                    HelloTutor.SaveChanges();
                }
            }

            return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
        }

    }
}