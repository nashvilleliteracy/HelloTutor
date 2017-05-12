using HelloTutor.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloTutor.Controllers
{
    public class RolesController : Controller
    {
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Roles_Read([DataSourceRequest]DataSourceRequest request)
        {
            var HelloTutor = new HelloTutorEntities();
            IQueryable<Role> roles = HelloTutor.Roles;
            //DataSourceResult result = roles.ToDataSourceResult(request);
            DataSourceResult result = roles.ToDataSourceResult(request, role => new RoleViewModel
            {
                RoleGuid = role.GUID,
                RoleID = role.Id,
                RoleName = role.Name,
                RoleDescription = role.Description
            });
            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, RoleViewModel rvm)
        {
            if (rvm != null &&
                !(string.IsNullOrWhiteSpace(rvm.RoleName) &&
                  string.IsNullOrWhiteSpace(rvm.RoleDescription)
                 )
            )
            {
                using (var HelloTutor = new HelloTutorEntities())
                {
                    var role = new Role()
                    {
                        Name = rvm.RoleName,
                        Description = rvm.RoleDescription,
                        GUID = Guid.NewGuid()
                    };
                    //Added Guid Generator since empty guid is causing default NewId() in db to fail leaving guid value as empty value.
                    HelloTutor.Roles.Add(role);
                    HelloTutor.SaveChanges();
                    //forcing modelstate.Isvalid true here to force proper refresh of grid
                    //Not sure what I am doing wrong but caused expected behavior
                    //quick way to get working until we really need to fix
                    rvm.RoleID = role.Id;
                    rvm.RoleGuid = role.GUID;
                    ModelState.Clear();
                }

            }

            return Json(new[] { rvm }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, RoleViewModel rvm)
        {
            if (rvm != null && ModelState.IsValid)
            {
                var role = new Role()
                {
                    Name = rvm.RoleName,
                    Description = rvm.RoleDescription,
                    GUID = rvm.RoleGuid,
                    Id = rvm.RoleID
                   
                };
                //productService.Update(product);
                using (var HelloTutor = new HelloTutorEntities())
                {
                    HelloTutor.Roles.Attach(role);
                    HelloTutor.SaveChanges();
                }
            }

            return Json(new[] { rvm }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, RoleViewModel rvm)
        {
            if (rvm != null)
            {
                using (var HelloTutor = new HelloTutorEntities())
                {
                    var role = new Role()
                    {
                        Name = rvm.RoleName,
                        Description = rvm.RoleDescription,
                        GUID = rvm.RoleGuid,
                        Id = rvm.RoleID

                    };

                    //Remove all associated classes
                    foreach (var rc in role.Classes)
                    {
                        HelloTutor.Classes.Remove(rc);
                    }

                    //Remove all associated roles
                    foreach (var tr in role.TutorsRoles)
                    {
                        HelloTutor.TutorsRoles.Remove(tr);
                    }
                    //Remove Tutor
                    HelloTutor.Roles.Attach(role);
                    HelloTutor.Roles.Remove(role);
                    HelloTutor.SaveChanges();
                }
            }

            return Json(new[] { rvm }.ToDataSourceResult(request, ModelState));
        }
    }
}