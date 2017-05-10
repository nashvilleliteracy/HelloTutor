using HelloTutor;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
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
      using (var HiTutor = new HelloTutorEntities())
      {
        IQueryable<Tutor> tutors = HiTutor.Tutors;
        DataSourceResult result = tutors.ToDataSourceResult(request);
        return Json(result);
      }

    }
  }
}