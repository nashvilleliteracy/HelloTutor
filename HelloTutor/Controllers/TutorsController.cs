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
        var HelloTutor = new HelloTutorEntities();
        tutor = HelloTutor.Tutors.Add(tutor);
        HelloTutor.SaveChanges();
        
      }

      return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Update([DataSourceRequest] DataSourceRequest request, Tutor tutor)
    {
      if (tutor != null && ModelState.IsValid)
      {
        //productService.Update(product);
      }

      return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult EditingInline_Destroy([DataSourceRequest] DataSourceRequest request, Tutor tutor)
    {
      if (tutor != null)
      {
        //productService.Destroy(product);
      }

      return Json(new[] { tutor }.ToDataSourceResult(request, ModelState));
    }

  }
}