using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index(string fromDatePie, string toDatePie, string fromDateWorks, string toDateWorks)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            if (String.IsNullOrWhiteSpace(accessToken))
                return RedirectToAction("Login", "Account");

            const string dateFormatString = "yyyy-MM-dd";
            ViewBag.FromDatePie = !String.IsNullOrWhiteSpace(fromDatePie) ? fromDatePie : DateTime.Today.AddDays(-7).ToString(dateFormatString);
            ViewBag.ToDatePie = !String.IsNullOrWhiteSpace(toDatePie) ? toDatePie : DateTime.Today.AddDays(1).ToString(dateFormatString);
            ViewBag.FromDateWorks = !String.IsNullOrWhiteSpace(fromDateWorks) ? fromDateWorks : DateTime.Today.AddDays(-7).ToString(dateFormatString);
            ViewBag.ToDateWorks = !String.IsNullOrWhiteSpace(toDateWorks) ? toDateWorks : DateTime.Today.AddDays(1).ToString(dateFormatString);

            return View();

        }

        // GET: Reports/Details/5
        public ActionResult Details(int id)
        {
            return View();

        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Reports/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int id)
        {
            return View();

        }

        // POST: Reports/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int id)
        {
            return View();

        }

        // POST: Reports/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> GetPieChart(string fromDatePie, string toDatePie)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var taskSubItemService = new TaskSubitemService(accessToken);
            UserService userService = new UserService(accessToken);
            var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            DateTime fromDate = DateTime.Parse(fromDatePie);
            DateTime toDate = DateTime.Parse(toDatePie);
            var taskSubitemsCompleted = await taskSubItemService.GetCompletedTaskSubitems(userId, fromDate, toDate);
            var taskSubitemsNotCompleted = await taskSubItemService.GetNotCompletedTaskSubitems(userId, fromDate, toDate);
            
            var key = new Chart(width: 1280, height: 720)
               .AddSeries(
                   chartType: "pie",
                   legend: "Completed/NotCompleted",
                   xValue: new[] { "Completed: " + taskSubitemsCompleted.Count, "Not completed: " + taskSubitemsNotCompleted.Count },
                   yValues: new[] { taskSubitemsCompleted.Count, taskSubitemsNotCompleted.Count })
               .Write();

            return null;
        }

        #region SampleCharts
        public ActionResult GetChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "pie",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" })
                .Write();
            return null;
        }

        public ActionResult GetRainfallChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "pie",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" })
                .Write();
            return null;
        }

        public ActionResult GetBarChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "bar",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" }).AddSeries(
                    chartType: "bar",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "30", "10", "20", "40", "50" })
                .Write();
            return null;
        }
        public ActionResult GetColumnChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "column",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" }).AddSeries(
                    chartType: "column",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                     yValues: new[] { "30", "10", "20", "40", "50" })
                .Write();
            return null;
        }

        public ActionResult GetAreaChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "area",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" }).AddSeries(
                    chartType: "area",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                     yValues: new[] { "30", "10", "20", "40", "50" })
                .Write();
            return null;
        }

        public ActionResult GetLineChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "line",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" }).AddSeries(
                    chartType: "line",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                     yValues: new[] { "30", "10", "20", "40", "50" })
                .Write();
            return null;
        }

        public ActionResult GetStockChart()
        {
            var key = new Chart(width: 1280, height: 720)
                .AddSeries(
                    chartType: "stock",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                    yValues: new[] { "20", "20", "40", "10", "10" }).AddSeries(
                    chartType: "stock",
                    legend: "Rainfall",
                    xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
                     yValues: new[] { "30", "10", "20", "40", "50" })
                .Write();
            return null;
        } 
        #endregion

    }
}
