﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloTutor.Models;

namespace HelloTutor.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(TutorViewModel newtutor)
        {

            return View("Success");
        }

        public ActionResult Success()
        {
            return View();
        }
    }
}