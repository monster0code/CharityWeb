using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CharityWeb.Models;
using Microsoft.AspNet.Identity;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.IO;

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

        [Authorize(Roles = "Admin")]
        public ActionResult AdminIndex()
        {
            return View(db.NursingAppointment.ToList());
        }
        [HttpPost]
        public async Task<string> SendBluck(List<string> selectedUsers)
        {
            // Logic to handle selected users
            // Example: Send selected users to backend processing
            foreach (var user in selectedUsers)
            {
                Debug.WriteLine("Selected UserName: " + user); // Print to console

                string subject = "Appointment notification email";
                string body = $"I hope this email finds you well. This message serves as a reminder regarding your upcoming appointment.<br/>" +
                              $"Please ensure your presence at the scheduled time. If there are any changes or if you need to reschedule, please contact us as soon as possible.<br/>" +
                              $"Contact：admin@student.monash.edu<br/>" +
                              $"Please contact us for more information.";


                // 发送邮件
                await SendGridEmail(user, subject, body);
            }

            // Return success or error response as needed
            return "success";
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
        public async Task<string> CreateAppointment(string UserName, int HomeId, string Date)
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

            // 查询对应的 NursingHome
            var nursingHome = db.NursingHomes.FirstOrDefault(n => n.ID == HomeId);
            Debug.WriteLine(nursingHome.Name);

            if (ModelState.IsValid)
            {
                db.NursingAppointment.Add(appointment);
                db.SaveChanges();

                // 构建邮件内容
                string subject = "Appointment success confirmation email";
                string body = $"You have made a successful reservation {nursingHome.Name}，detail：<br/>" +
                              $"Location：{nursingHome.Location}<br/>" +
                              $"<img src=\"{nursingHome.ImageUrl}\" alt=\"Nursing Home Image\"><br/>" +
                              $"Your Date：{Date}<br/>" +
                              $"Contact：admin@student.monash.edu<br/>" +
                              $"Please contact us for more information.";
                

                // 发送邮件
                await SendGridEmail(UserName, subject, body);

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

        // 获取当前用户对特定活动的预约日期列表
        public JsonResult GetUserCalendarAppointments(int activityId)
        {
            var currentUser = User.Identity.GetUserName(); // 获取当前登录用户的用户名

            // 根据活动 ID 和用户名查询已预约的日期列表
            var reservedDates = db.NursingAppointment
                .Where(a => a.HomeId == activityId && a.UserName == currentUser)
                .OrderBy(a => a.YourDate)  // 按日期排序
                .Select(a => a.YourDate)
                .ToList();

            // 将日期格式化为 FullCalendar 所需的事件格式
            var events = reservedDates.Select(d => new
            {
                title = "My Appointment",
                start = d.ToString("yyyy-MM-dd"), // FullCalendar 需要 ISO 格式
                allDay = false // 如果是全天事件
            }).ToList();

            return Json(events, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateAppointmentDate(int id, DateTime newDate, DateTime oldDate)
        {
            var appointment = db.NursingAppointment.SingleOrDefault(a => a.HomeId == id && a.YourDate == oldDate);
            Debug.WriteLine(appointment);
            if (appointment == null)
            {
                return Json(new { success = false, message = "Appointment not found." });
            }

            // 更新日期
            appointment.YourDate = newDate;

            try
            {
                db.SaveChanges();  // 保存更改到数据库
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // 捕捉任何可能的错误并返回失败消息
                return Json(new { success = false, message = ex.Message });
            }
        }



        // send-email
        private async Task SendEmail(string recipient, string subject, string body)
        {
            Debug.WriteLine("send...");
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("persist.1015.you@gmail.com");
                message.To.Add(new MailAddress(recipient));
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true; // 设置为 true 如果邮件正文是HTML格式
                /* Attachment newAttach = new Attachment(Server.MapPath("~/Content/static/pic/nursing-home-vector-20430179.jpg"));
                 message.Attachments.Add(newAttach);*/

                /*SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Send(message);*/
                using (SmtpClient client = new SmtpClient())
                {
                    client.EnableSsl = true;
                    await client.SendMailAsync(message);
                }
                Debug.WriteLine("send successfully");
            }
            catch (Exception ex)
            {
                // 处理邮件发送异常
                ViewBag.Error = $"发送邮件时出错: {ex.Message}";
            }
        }

        public Task SendGridEmail(string email, string subject, string htmlMessage)
        {
            Debug.WriteLine("sending...");
            var apiKey = "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";
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("persist.1015.you@gmail.com", "Notification");
            var to = new EmailAddress(email);
            var message = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);

            // 添加本地图片作为附件
            string imagePath = "E:\\Models\\Semester2\\vsNet\\CharityWeb\\CharityWeb\\Content\\static\\pic\\nursing-home-vector-20430179.jpg"; // 替换为你的本地图片路径
            string fileName = "ElderlyCareHub.jpg";
            byte[] fileBytes = System.IO.File.ReadAllBytes(imagePath);
            var file = Convert.ToBase64String(fileBytes);
            message.AddAttachment(fileName, file, "image/jpeg", "attachment");

            return client.SendEmailAsync(message);
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
