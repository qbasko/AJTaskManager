using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Management;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Services;
using WebGrease.Css.Ast.Animation;

namespace WebApplication1.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        public async Task<ActionResult> Index(string fromDatePie, string toDatePie, string fromDateWorks, string toDateWorks, string groupId, string fromDateUserWorks, string toDateUserWorks)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;

            if (String.IsNullOrWhiteSpace(accessToken))
                return RedirectToAction("Login", "Account");

            const string dateFormatString = "yyyy-MM-dd";
            ViewBag.FromDatePie = !String.IsNullOrWhiteSpace(fromDatePie) ? fromDatePie : DateTime.Today.AddDays(-7).ToString(dateFormatString);
            ViewBag.ToDatePie = !String.IsNullOrWhiteSpace(toDatePie) ? toDatePie : DateTime.Today.AddDays(1).ToString(dateFormatString);
            ViewBag.FromDateWorks = !String.IsNullOrWhiteSpace(fromDateWorks) ? fromDateWorks : DateTime.Today.AddDays(-7).ToString(dateFormatString);
            ViewBag.ToDateWorks = !String.IsNullOrWhiteSpace(toDateWorks) ? toDateWorks : DateTime.Today.AddDays(1).ToString(dateFormatString);
            ViewBag.FromDateUserWorks = !String.IsNullOrWhiteSpace(fromDateUserWorks) ? fromDateUserWorks : DateTime.Today.AddDays(-7).ToString(dateFormatString);
            ViewBag.ToDateUserWorks = !String.IsNullOrWhiteSpace(toDateUserWorks) ? toDateUserWorks : DateTime.Today.AddDays(1).ToString(dateFormatString);

            UserGroupService userGroupService = new UserGroupService(accessToken);

            var usrGroup = await userGroupService.GetGroupForUser(Session["UserId"] as string);
            var defaultGroup = usrGroup.SingleOrDefault(g => g.GroupName.Contains("Default group for user"));
            var selectedGroupId = String.IsNullOrWhiteSpace(groupId) ? defaultGroup.Id : groupId;


            ViewBag.GroupID = new SelectList(usrGroup, "Id", "GroupNameTruncated", selectedGroupId);

            ViewBag.SelectedGroupID = selectedGroupId;

            return View();
        }

        // GET: Statistic/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Statistic/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statistic/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Statistic/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistic/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Statistic/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

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

            int width = 600;
            int height = 50;
            string chartTitle = "You don not have any tasks in this period of time";

            if (taskSubitemsCompleted.Any() || taskSubitemsNotCompleted.Any())
            {
                width = 1024;
                height = 700;
                chartTitle = "Your completed and not completed tasks";
            }

            var key = new Chart(width: width, height: height)
                .AddSeries(
                    chartType: "pie",
                    legend: "Completed/NotCompleted",
                    xValue:
                        new[]
                            {
                                "Completed: " + taskSubitemsCompleted.Count,
                                "Not completed: " + taskSubitemsNotCompleted.Count
                            },
                    yValues: new[] { taskSubitemsCompleted.Count, taskSubitemsNotCompleted.Count }).AddTitle(chartTitle)
                .Write();
            return null;

        }

        public async Task<ActionResult> GetWorksLineChart(string fromDateWorks, string toDateWorks, string groupId)
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;
            var taskSubitemWorkService = new TaskSubitemWorkService(accessToken);
            UserService userService = new UserService(accessToken);
            var userId = await userService.GetUserId(Session["UserId"] as string, UserDomainEnum.Microsoft);
            DateTime fromDate = DateTime.Parse(fromDateWorks);
            DateTime toDate = DateTime.Parse(toDateWorks);
            var userWorks = await taskSubitemWorkService.GetGroupTaskSubitemWorksForUser(userId, groupId, fromDate, toDate);
            var groupWorks = await taskSubitemWorkService.GetGroupTaskSubitemWorks(groupId, fromDate, toDate);

            var userWorksGroupByDay = userWorks.GroupBy(w => w.EndDateTime.Value.Date);
            var groupWorksGroupByDay = groupWorks.GroupBy(w => w.EndDateTime.Value.Date);

            Dictionary<DateTime, double> valuesForGroup = new Dictionary<DateTime, double>();
            foreach (var item in groupWorksGroupByDay)
            {
                TimeSpan totalTimeSpan = new TimeSpan();
                foreach (TaskSubitemWork work in item)
                {
                    totalTimeSpan += work.EndDateTime.Value - work.StartDateTime;
                }
                valuesForGroup.Add(item.Key, totalTimeSpan.Minutes);
            }

            Dictionary<DateTime, double> valuesForUser = new Dictionary<DateTime, double>();
            foreach (var item in userWorksGroupByDay)
            {
                TimeSpan totalTimeSpan = new TimeSpan();
                foreach (TaskSubitemWork work in item)
                {
                    totalTimeSpan += work.EndDateTime.Value - work.StartDateTime;
                }
                valuesForUser.Add(item.Key, totalTimeSpan.Minutes);
            }

            var groupDictDt = valuesForGroup.OrderBy(i => i.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            var userDictDt = valuesForUser.OrderBy(i => i.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var g in groupDictDt)
            {
                if (!userDictDt.ContainsKey(g.Key))
                    userDictDt.Add(g.Key, 0);
            }

            var groupDict = groupDictDt.OrderBy(i => i.Key).ToDictionary(pair => pair.Key.ToString("dd-MM-yyyy"), pair => pair.Value);
            var userDict = userDictDt.OrderBy(i => i.Key).ToDictionary(pair => pair.Key.ToString("dd-MM-yyyy"), pair => pair.Value);

            int width = 600;
            int height = 50;
            string chartTitle = "In this period nobody spend time on the tasks in this group.";

            if (groupDict.Any() || userDict.Any())
            {
                width = 1024;
                height = 700;
                chartTitle = "Minutes spent on task subitems";
            }
            var key = new Chart(width: width, height: height)
               .AddSeries(
                   chartType: "column",
                   legend: "group",
                   xValue: groupDict.Keys,
                   yValues: groupDict.Values)
               .AddSeries(
                   chartType: "column",
                   legend: "user",
                   xValue: userDict.Keys,
                   yValues: userDict.Values).AddTitle(chartTitle)
               .Write();
            return null;
        }

        public async Task<ActionResult> GetUserTimeSpendOnTask(string fromDateUserWorks, string toDateUserWorks)
        {
            var accessToken = Session["MicrosoftAccessToken"] as string;
            var externalUserId = Session["UserId"] as string;
            DateTime fromDateUsr = DateTime.Parse(fromDateUserWorks);
            DateTime toDateUsr = DateTime.Parse(toDateUserWorks);
            TaskSubitemWorkService taskSubitemWorkService = new TaskSubitemWorkService(accessToken);
            UserService userService = new UserService(accessToken);
            var userWorks =
                await
                    taskSubitemWorkService.GetUserTaskSubitemWorks(
                        await userService.GetUserId(externalUserId, UserDomainEnum.Microsoft), fromDateUsr, toDateUsr);
            var userWorksGroupByDay = userWorks.GroupBy(w => w.EndDateTime.Value.Date);
            Dictionary<DateTime, double> valuesForUser = new Dictionary<DateTime, double>();
            foreach (var item in userWorksGroupByDay)
            {
                TimeSpan totalTimeSpan = new TimeSpan();
                foreach (TaskSubitemWork work in item)
                {
                    totalTimeSpan += work.EndDateTime.Value - work.StartDateTime;
                }
                valuesForUser.Add(item.Key, totalTimeSpan.Minutes);
            }

            var userDict = valuesForUser.OrderBy(i => i.Key).ToDictionary(pair => pair.Key.ToString("dd-MM-yyyy"), pair => pair.Value);

            int width = 600;
            int height = 50;
            string chartTitle = "In this period you did not spend time on the tasks.";
            if (userDict.Any())
            {
                width = 1024;
                height = 700;
                chartTitle = "Your time spent on task subitems";
            }
            var key = new Chart(width: width, height: height)
                .AddSeries(
                    chartType: "column",
                    legend: "group",
                    xValue: userDict.Keys,
                    yValues: userDict.Values).AddTitle(chartTitle).Write();

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

        public async Task<SelectList> SetGroupForStatstic(string accessToken)
        {
            UserGroupService userGroupService = new UserGroupService(accessToken);

            var usrGroup = await userGroupService.GetGroupForUser(Session["UserId"] as string);
            var defaultGroup = usrGroup.SingleOrDefault(g => g.GroupName.Contains("Default group for user"));

            //ViewBag.GroupId = new SelectList(usrGroup, "Id", "GroupNameTruncated", defaultGroup);

            return new SelectList(usrGroup, "Id", "GroupNameTruncated", defaultGroup);

        }
    }
}
