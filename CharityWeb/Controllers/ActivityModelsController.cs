using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CharityWeb.Models;
using PagedList;

namespace CharityWeb.Controllers
{
    public class ActivityModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ActivityModels
        /*        public ActionResult Index(int? page)
                {
                    *//*return View(db.Activities.ToList());*//*
                    var activities = db.Activities.OrderBy(a => a.Name).ToList(); // 获取数据并排序
                    int pageSize = 2; // 每页显示的数据量
                    int pageNumber = (page ?? 1); // 当前页码
                    return View(activities.ToPagedList(pageNumber, pageSize));
                }*/

        public ActionResult Index(string searchString, int? page)
        {
            var activity = from n in db.Activities
                               select n;

            if (!String.IsNullOrEmpty(searchString))
            {
                activity = activity.Where(n => n.Name.Contains(searchString));
            }

            int pageSize = 10; // 每页显示的数据量
            int pageNumber = (page ?? 1); // 当前页码

            ViewBag.CurrentFilter = searchString; // 保存当前的搜索词以便在视图中使用

            return View(activity.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
        }

        // GET: ActivityModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModels activityModels = db.Activities.Find(id);
            if (activityModels == null)
            {
                return HttpNotFound();
            }
            return View(activityModels);
        }

        // GET: ActivityModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivityModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Location,Time,ImageUrl,Info")] ActivityModels activityModels)
        {
            if (ModelState.IsValid)
            {
                db.Activities.Add(activityModels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activityModels);
        }

        // GET: ActivityModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModels activityModels = db.Activities.Find(id);
            if (activityModels == null)
            {
                return HttpNotFound();
            }
            return View(activityModels);
        }

        // POST: ActivityModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Location,Time,ImageUrl,Info")] ActivityModels activityModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activityModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activityModels);
        }

        // GET: ActivityModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModels activityModels = db.Activities.Find(id);
            if (activityModels == null)
            {
                return HttpNotFound();
            }
            return View(activityModels);
        }

        // POST: ActivityModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityModels activityModels = db.Activities.Find(id);
            db.Activities.Remove(activityModels);
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
