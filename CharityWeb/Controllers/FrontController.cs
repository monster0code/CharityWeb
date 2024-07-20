using CharityWeb.DAL;
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
            NursingHomeDAL dal = new NursingHomeDAL();
            var nursingHomes = dal.GetAllNursingHomes();
            return View(nursingHomes);
        }
        public ActionResult Place()
        {
            NursingHomeDAL dal = new NursingHomeDAL();
            var nursingHomes = dal.GetAllNursingHomes();
            return View(nursingHomes);
        }
        public ActionResult Activitylist()
        {
            ActivityDAL dal = new ActivityDAL();
            var activities = dal.GetAllActivity();
            return View(activities);
        }
        public ActionResult Detail(int id)
        {
            NursingHomeDAL dal = new NursingHomeDAL();
            var nursingHome = dal.GetNursingHomeById(id);
            if (nursingHome == null)
            {
                return HttpNotFound();
            }
            return View(nursingHome);
        }
        public ActionResult Activity(int id)
        {
            ActivityDAL dal = new ActivityDAL();
            var activity = dal.GetActivityById(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }
    }
}