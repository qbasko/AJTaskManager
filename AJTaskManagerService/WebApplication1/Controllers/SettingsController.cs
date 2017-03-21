using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public async Task<ActionResult> Index()
        {
            string accessToken = Session["MicrosoftAccessToken"] as string;

            if (String.IsNullOrWhiteSpace(accessToken))
                return RedirectToAction("Login", "Account");

            string userId = Session["UserId"] as string;
            var userService = new UserService(accessToken);
            var user = await userService.GetUser(userId, UserDomainEnum.Microsoft);
            ViewBag.User = user;
            return View();
        }

        // GET: Settings/Details/5
        public ActionResult Details(int id)
        {
            return View();

        }

        // GET: Settings/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: Settings/Create
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

        // GET: Settings/Edit/5
        public ActionResult Edit(int id)
        {
            return View();

        }

        // POST: Settings/Edit/5
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

        // GET: Settings/Delete/5
        public ActionResult Delete(int id)
        {
            return View();

        }

        // POST: Settings/Delete/5
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
    }
}
