using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult DefaultView()
        {
            return View();
        }

        public ActionResult AddTodoList()
        {
            return RedirectToAction("Create", "AddTodoList");
        }

        public ActionResult Lists()
        {
            return RedirectToAction("Index", "AddTodoList");
        }

        public ActionResult AccessDeniedList()
        {
            return RedirectToAction("Index", "AddTodoList");

        }

    }
}