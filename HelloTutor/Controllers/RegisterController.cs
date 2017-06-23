using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloTutor.Models;
using System.Data.Entity.Validation;

namespace HelloTutor.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        
        public ActionResult Index()
        {
            RoleViewModel rvm = new RoleViewModel();

            IEnumerable<SelectListItem> rolesdata = rvm.GetRolesForListBox();
            ViewBag.Roles = rolesdata;

            return View();
        }

        [HttpPost]
        public ActionResult Index(TutorViewModel newtutor)
        {
            HelloTutorEntities db = new HelloTutorEntities();
            Tutor tutor = new Tutor();
            tutor.Comments = newtutor.Comments;
            tutor.Email = newtutor.EmailAddress;
            tutor.FirstName = newtutor.FirstName;
            tutor.LastName = newtutor.LastName;
            tutor.PhoneNumber = newtutor.PhoneNumber;
            tutor.GUID = Guid.NewGuid();
            tutor.DateTimeCreated = DateTime.Now;
            db.Tutors.Add(tutor);

            try
            {
                db.SaveChanges();
            }
            
        catch (DbEntityValidationException ex)
        {
            // Retrieve the error messages as a list of strings.
            var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

            // Join the list to a single string.
            var fullErrorMessage = string.Join("; ", errorMessages);

            // Combine the original exception message with the new one.
            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
    
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        }



    // roles
    TutorsRole tr = new TutorsRole();
            tr.GUID = Guid.NewGuid();
            tr.RoleId = Convert.ToInt32(newtutor.RoleID);
            tr.TutorId = tutor.Id;
            db.TutorsRoles.Add(tr);
            db.SaveChanges();

            // classes
            TutorsClass tc = new TutorsClass();
            tc.GUID = Guid.NewGuid();
            tc.ClassId = Convert.ToInt32(newtutor.TrainingSessionID);
            tc.TutorId = tutor.Id;
            db.TutorsClasses.Add(tc);
            db.SaveChanges();


            return View("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}