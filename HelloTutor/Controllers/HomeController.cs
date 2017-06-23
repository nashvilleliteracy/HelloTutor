using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloTutor.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Redirect("Register/Index");
            //return Redirect("Account/Login");
        }
    }
}