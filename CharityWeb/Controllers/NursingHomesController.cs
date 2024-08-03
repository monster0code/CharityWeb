using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CharityWeb.Models;
using OfficeOpenXml;
using PagedList;

namespace CharityWeb.Controllers
{
    public class NursingHomesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: NursingHomes
        [Authorize(Roles = "Admin")]
        public ActionResult Index(int? page)
        {
            var nursingHomes = db.NursingHomes.OrderBy(a => a.Name).ToList();

            int pageSize = 10; // 每页显示的数据量
            int pageNumber = (page ?? 1); // 当前页码
            return View(nursingHomes.OrderBy(n => n.Name).ToPagedList(pageNumber, pageSize));
        }
        // GET: NursingHomes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingHome nursingHome = db.NursingHomes.Find(id);
            if (nursingHome == null)
            {
                return HttpNotFound();
            }
            return View(nursingHome);
        }

        // GET: NursingHomes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NursingHomes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Location,Price,ImageUrl,Info")] NursingHome nursingHome)
        {
            if (ModelState.IsValid)
            {
                db.NursingHomes.Add(nursingHome);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nursingHome);
        }

        // GET: NursingHomes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingHome nursingHome = db.NursingHomes.Find(id);
            if (nursingHome == null)
            {
                return HttpNotFound();
            }
            return View(nursingHome);
        }

        // POST: NursingHomes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Location,Price,ImageUrl,Info")] NursingHome nursingHome)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nursingHome).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nursingHome);
        }

        // GET: NursingHomes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NursingHome nursingHome = db.NursingHomes.Find(id);
            if (nursingHome == null)
            {
                return HttpNotFound();
            }
            return View(nursingHome);
        }

        // POST: NursingHomes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NursingHome nursingHome = db.NursingHomes.Find(id);
            db.NursingHomes.Remove(nursingHome);
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

        /// <summary>
        /// ExportToExcel
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportToExcel()
        {
            var nursingHomes = db.NursingHomes.ToList(); // 获取所有数据

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("NursingHomes");

                // 添加表头
                worksheet.Cells[1, 1].Value = "Name";
                worksheet.Cells[1, 2].Value = "Location";
                worksheet.Cells[1, 3].Value = "Price";
                worksheet.Cells[1, 4].Value = "ImageUrl";
                worksheet.Cells[1, 5].Value = "Info";

                // 添加数据
                for (int i = 0; i < nursingHomes.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = nursingHomes[i].Name;
                    worksheet.Cells[i + 2, 2].Value = nursingHomes[i].Location;
                    worksheet.Cells[i + 2, 3].Value = nursingHomes[i].Price;
                    worksheet.Cells[i + 2, 4].Value = nursingHomes[i].ImageUrl;
                    worksheet.Cells[i + 2, 5].Value = nursingHomes[i].Info;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                var content = stream.ToArray();
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "NursingHomes.xlsx");
            }
        }
    }
}
