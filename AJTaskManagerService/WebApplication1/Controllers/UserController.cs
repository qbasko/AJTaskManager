using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            
            return View();
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            //var obj = this.RouteData.Values;
            UserModel userModel = new UserModel();
            userModel.Email = Session["MicrosoftEmail"] as string;
            userModel.UserName = Session["FirstName"] as string;
            userModel.LastName = Session["LastName"] as string;

            return View(userModel);
        }

        // POST: User/Create
        [HttpPost]
        public async Task<ActionResult> Create(UserModel userModel)
        {
            DTO.User user = new User();

            user.Id = Guid.NewGuid().ToString();
            user.UserName = userModel.UserName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;

            UserService userService = new UserService(Session["MicrosoftAccessToken"] as string);

            
            try
            {
                // TODO: Add insert logic here
                await userService.InsertUser(user, Session["UserId"]as string);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: User/Edit/5
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

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
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
