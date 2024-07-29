using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CharityWeb.Models;
using Microsoft.AspNet.Identity;

namespace CharityWeb.Controllers
{
    public class NursingAppointmentModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NursingAppointmentModels
        public ActionResult Index()
        {
            var currentUser = User.Identity.GetUserName(); // 获取当前登录用户的用户名
            var userAppointments = db.NursingAppointment.Where(a => a.UserName == currentUser).ToList(); // 过滤当前用户的预约
            return View(userAppointments);
        }

        // GET: NursingAppointmentModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingAppointmentModels nursingAppointmentModels = db.NursingAppointment.Find(id);
            if (nursingAppointmentModels == null)
            {
                return HttpNotFound();
            }
            return View(nursingAppointmentModels);
        }

        // GET: NursingAppointmentModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NursingAppointmentModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
/*        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserName,HomeId,YourDate")] NursingAppointmentModels nursingAppointmentModels)
        {
            if (ModelState.IsValid)
            {
                db.NursingAppointment.Add(nursingAppointmentModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nursingAppointmentModels);
        }*/
        [HttpPost]
        public String CreateAppointment(string UserName, int HomeId, string Date)
        {
            // 打印接收到的数据
            Debug.WriteLine("Received data:");
            Debug.WriteLine($"UserName: {UserName}");
            Debug.WriteLine($"HomeId: {HomeId}");
            Debug.WriteLine($"Date: {Date}");
            var appointment = new NursingAppointmentModels
            {
                UserName = UserName,
                HomeId = HomeId,
                YourDate = DateTime.Parse(Date)
            };

            if (ModelState.IsValid)
            {
                db.NursingAppointment.Add(appointment);
                db.SaveChanges();
                return "Success";
            }
            return "Database Unavailable.";

        }

        // 获取当前用户对特定活动的预约日期列表
        public JsonResult GetUserAppointments(int activityId)
        {
            var currentUser = User.Identity.GetUserName(); // 获取当前登录用户的用户名

            // 根据活动 ID 和用户名查询已预约的日期列表
            // 先获取所有预约日期
            var reservedDates = db.NursingAppointment
                .Where(a => a.HomeId == activityId && a.UserName == currentUser)
                .Select(a => a.YourDate)
                .ToList();

            // 将日期格式化为字符串
            var formattedDates = reservedDates.Select(d => d.ToString("yyyy-MM-dd")).ToList();

            return Json(formattedDates, JsonRequestBehavior.AllowGet);
        }

        // GET: NursingAppointmentModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingAppointmentModels nursingAppointmentModels = db.NursingAppointment.Find(id);
            if (nursingAppointmentModels == null)
            {
                return HttpNotFound();
            }
            return View(nursingAppointmentModels);
        }

        // POST: NursingAppointmentModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserName,HomeId,YourDate")] NursingAppointmentModels nursingAppointmentModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nursingAppointmentModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nursingAppointmentModels);
        }


        // GET: NursingAppointmentModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingAppointmentModels nursingAppointmentModels = db.NursingAppointment.Find(id);
            if (nursingAppointmentModels == null)
            {
                return HttpNotFound();
            }
            return View(nursingAppointmentModels);
        }

        // POST: NursingAppointmentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NursingAppointmentModels nursingAppointmentModels = db.NursingAppointment.Find(id);
            db.NursingAppointment.Remove(nursingAppointmentModels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
