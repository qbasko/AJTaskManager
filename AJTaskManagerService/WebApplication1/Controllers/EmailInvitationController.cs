using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Common;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EmailInvitationController : Controller
    {
        // GET: EmailNotification
        public ActionResult Index()
        {
            return View();

        }

        // GET: EmailNotification/Details/5
        public ActionResult Details(int id)
        {
            return View();

        }

        // GET: EmailNotification/Create
        public ActionResult Create()
        {
            return View();

        }

        // POST: EmailNotification/Create
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

        // GET: EmailNotification/Edit/5
        public async  Task<ActionResult> Edit(User user)
        {
            var accessToken = Session["MicrosftAccessToken"] as string;
            UserService userSevice =new UserService(accessToken);
            var userInfo = await userSevice.GetUser(Session["UserId"] as string, UserDomainEnum.Microsoft);
            
            EmailInvitationModel emailModel = new EmailInvitationModel();
            emailModel.Message =
                "Hello!"+"\n\n"+ "User "+userInfo.FullName+" ("+userInfo.Email+")"+ " would like to invite you to join AJTaskManager service :) You can download app to your mobile phone with Windows Phone OS or you can use it via internet browser. Just go to: http://ajtaskmanagerservice.azure.net/" +"\n\n"+"AJTaskManger Team";

            return View(emailModel);
        }

        // POST: EmailNotification/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(User user, EmailInvitationModel emailInvitationModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(emailInvitationModel.ToEmail));
                    message.From = new MailAddress("ajtaskmanager@outlook.com");
                    message.Subject = "Invitation to AJTaskManager";
                    message.Body = String.Format(body, user.FullName, user.Email, emailInvitationModel.Message);
                    message.IsBodyHtml = true;

                    using (var smtp=new SmtpClient())
                    {
                        var credential = new NetworkCredential
                        {
                            UserName = Properties.Settings.Default.EmailFrom,
                            Password = Properties.Settings.Default.EmailUserPass
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp-mail.outlook.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(message);
                    }
                }
                return RedirectToAction("Index", "Settings");
            }
            catch
            {
                return View();
            }

        }

        // GET: EmailNotification/Delete/5
        public ActionResult Delete(int id)
        {
            return View();

        }

        // POST: EmailNotification/Delete/5
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
