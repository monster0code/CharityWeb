using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CharityWeb.Controllers
{
    public class FrontController : Controller
    {
        // GET: Front
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Place()
        {
            return View();
        }
        public ActionResult Activity()
        {
            return View();
        }
        public ActionResult Detail()
        {
            return View();
        }
    }
}