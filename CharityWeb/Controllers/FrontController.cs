using CharityWeb.DAL;
using GenerativeAI.Models;
using MarkdownSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost] // 根据前端发送请求的方法（POST 或者 GET）
        public async Task<ActionResult> AIprocessAsync(string text)
        {
            // 在这里处理业务逻辑，可以调用服务层方法等
            Debug.WriteLine(text);
            var apiKey = "AIzaSyDFHfRkIV9d7aMH1rBESlox25jrKmZtwWg";

            var model = new GenerativeModel(apiKey);
            //or var model = new GeminiProModel(apiKey)

            var res = await model.GenerateContentAsync(text);
            Markdown markdown = new Markdown();
            string htmlContent = markdown.Transform(res);
            Debug.WriteLine(htmlContent);
            ViewBag.HtmlContent = htmlContent;
            // 返回JSON数据给前端
            return Json(new { htmlContent });
        }
        public ActionResult AI()
        {
            return File("~/Views/Front/ai.html", "text/html");
        }
    }
}