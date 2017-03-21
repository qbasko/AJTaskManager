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
    public class UserInfoController : Controller
    {
        // GET: UserInfo
        public ActionResult Index()
        {
            return View();

        }

        // GET: UserInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();

        }

        // GET: UserInfo/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: UserInfo/Create
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

        // GET: UserInfo/Edit/5
        public ActionResult Edit(User user)
        {
            UserModel userModel = new UserModel();
            userModel.Email = user.Email;
            userModel.UserName = user.UserName;
            userModel.LastName = user.LastName;
            return View(userModel);
        }

        // POST: UserInfo/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(User user, UserModel userModel)
        {
            try
            {
                user.UserName = userModel.UserName;
                user.LastName = userModel.LastName;

                string accessToken = Session["MicrosoftAccessToken"] as string;
                var userService = new UserService(accessToken);
                await userService.Update(user);
                return RedirectToAction("Index", "Settings");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();

        }

        // POST: UserInfo/Delete/5
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
    }
}
