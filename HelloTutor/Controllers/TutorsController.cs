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
using HelloTutor.Models;

namespace HelloTutor.Controllers
{
    public class TutorsController : Controller
    {

        // GET: Tutors
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Tutors_Read([DataSourceRequest]DataSourceRequest request)
        {
            var db = new HelloTutorEntities();
            List<TutorViewModel> tutors = (from t in db.Tutors select new TutorViewModel {
                    TutorID = t.Id,
                    TutorGuid = t.GUID,
                    Comments = t.Comments,
                    EmailAddress = t.Email,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    PhoneNumber = t.PhoneNumber,
                    DateTimeCreated = t.DateTimeCreated                        
            }).ToList();
            DataSourceResult result = tutors.ToDataSourceResult(request);
            return Json(result);

        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, TutorViewModel tutor)
        {
            if (tutor != null && ModelState.IsValid)
            {
                //productService.Update(product);
                using (var db = new HelloTutorEntities())
                {
                    Tutor thisTutor = (from t in db.Tutors where t.Id == tutor.TutorID select t).FirstOrDefault();
                    if(thisTutor != null)
                    {
                        thisTutor.GUID = tutor.TutorGuid;
                        thisTutor.Id = tutor.TutorID;
                        thisTutor.Email = tutor.EmailAddress;
                        thisTutor.LastName = tutor.LastName;
                        thisTutor.FirstName = tutor.FirstName;
                        thisTutor.Comments = tutor.Comments;
                        thisTutor.PhoneNumber = tutor.PhoneNumber;

                        db.SaveChanges();
                    }
                }
            }

            return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
        }

        [Authorize(Roles = "Admin")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, TutorViewModel tutor)
        {
            if (tutor != null)
            {
                using (var db = new HelloTutorEntities())
                {

                    //Remove all associated classes
                    var classes = (from c in db.TutorsClasses where c.TutorId == tutor.TutorID select c).ToList();
                    
                    foreach (var item in classes)
                    {
                        db.TutorsClasses.Remove(item);
                    }

                    //Remove all associated roles
                    var roles = (from c in db.TutorsRoles where c.TutorId == tutor.TutorID select c).ToList();

                    foreach (var tr in roles)
                    {
                        db.TutorsRoles.Remove(tr);
                    }

                    //Remove Tutor
                    Tutor thisTutor = new Tutor() { Id = tutor.TutorID };
                    db.Tutors.Attach(thisTutor);
                    db.Tutors.Remove(thisTutor);
                    db.SaveChanges();
                }
            }

            return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
        }

    }
}