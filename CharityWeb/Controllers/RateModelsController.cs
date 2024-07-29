using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CharityWeb.Models;

namespace CharityWeb.Controllers
{
    public class RateModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RateModels
        public ActionResult Index()
        {
            double averageRating = db.Rate.Average(f => f.Rating);
            ViewData["AverageRating"] = averageRating;
            return View(db.Rate.ToList());
        }

        // GET: RateModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateModels rateModels = db.Rate.Find(id);
            if (rateModels == null)
            {
                return HttpNotFound();
            }
            return View(rateModels);
        }

        // GET: RateModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RateModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        /*        [HttpPost]
                [ValidateAntiForgeryToken]
                public ActionResult Create([Bind(Include = "Id,Rating,Feedback,SubmittedUser")] RateModels rateModels)
                {
                    if (ModelState.IsValid)
                    {
                        db.Rate.Add(rateModels);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                    return View(rateModels);
                }*/
        [HttpPost]
        public string CreateFeedback(int Rating, string Feedback, string SubmittedUser)
        {
            var feeddback = new RateModels
            {
                Rating = Rating,
                Feedback = Feedback,
                SubmittedUser = SubmittedUser
            };
            if (ModelState.IsValid)
            {
                db.Rate.Add(feeddback);
                db.SaveChanges();
                return "success";
            }

            return "error";
        }

        // GET: RateModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateModels rateModels = db.Rate.Find(id);
            if (rateModels == null)
            {
                return HttpNotFound();
            }
            return View(rateModels);
        }

        // POST: RateModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rating,Feedback,SubmittedUser")] RateModels rateModels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rateModels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(rateModels);
        }

        // GET: RateModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RateModels rateModels = db.Rate.Find(id);
            if (rateModels == null)
            {
                return HttpNotFound();
            }
            return View(rateModels);
        }

        // POST: RateModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RateModels rateModels = db.Rate.Find(id);
            db.Rate.Remove(rateModels);
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
