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

    public ActionResult Roles_Read([DataSourceRequest]DataSourceRequest request)
    {
        var HiTutor = new HelloTutorEntities();
        IQueryable<Role> role = HiTutor.Roles;
        DataSourceResult result = role.ToDataSourceResult(request);
        return Json(result);
    }
  }
}