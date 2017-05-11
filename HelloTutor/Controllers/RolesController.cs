using HelloTutor.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
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

    [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
    public ActionResult Roles_Read([DataSourceRequest]DataSourceRequest request)
    {
      var HelloTutor = new HelloTutorEntities();
      IQueryable<Role> roles = HelloTutor.Roles;
      DataSourceResult result = roles.ToDataSourceResult(request);
      return Json(result);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Create([DataSourceRequest] DataSourceRequest request, Role role)
    {
      if (role != null &&
          !(string.IsNullOrWhiteSpace(role.Name) &&
            string.IsNullOrWhiteSpace(role.Description_)
           )
      )
      {
        using (var HelloTutor = new HelloTutorEntities())
        {
          //Added Guid Generator since empty guid is causing default NewId() in db to fail leaving guid value as empty value.
          role.GUID = Guid.NewGuid();
          HelloTutor.Roles.Add(role);
          HelloTutor.SaveChanges();
          //forcing modelstate.Isvalid true here to force proper refresh of grid
          //Not sure what I am doing wrong but caused expected behavior
          //quick way to get working until we really need to fix
          ModelState.Clear();
        }

      }

      return Json(new[] { role }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Role role)
    {
      if (role != null && ModelState.IsValid)
      {
        //productService.Update(product);
        using (var HelloTutor = new HelloTutorEntities())
        {
          HelloTutor.Roles.Attach(role);
          HelloTutor.SaveChanges();
        }
      }

      return Json(new[] { role }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Role role)
    {
      if (role != null)
      {
        using (var HelloTutor = new HelloTutorEntities())
        {

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

      return Json(new[] { role }.ToDataSourceResult(request, ModelState));
    }
  }
}