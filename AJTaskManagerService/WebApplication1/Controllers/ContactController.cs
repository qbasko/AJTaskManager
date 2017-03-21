using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.MobileServices;
using WebApplication1.Models;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using WebApplication1.DTO;

namespace WebApplication1.Controllers
{
    public class ContactController : Controller
    {
       // private ContactContext db = new ContactContext();


        private static MobileServiceClient MobileService = new MobileServiceClient("https://ajtaskmanagerservice.azure-mobile.net/", "FLdMQrDpZuOTXNyGOuKsNvtOOmgOuO96");



        private Task<bool> EnsureLogin()
        {
            if (MobileService.CurrentUser == null)
            {
                try
                {
                    var accessToken = Session["MicrosoftAccessToken"] as string;
                    var token = new JObject();
                    token.Add("authenticationToken", accessToken);
                    return MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount, token).ContinueWith<bool>(t =>
                    {
                        if (t.Exception != null)
                        {
                            return true;
                        }
                        else
                        {
                            System.Diagnostics.Trace.WriteLine("Error logging in: " + t.Exception);
                            return false;
                        }
                    });
                }
                catch (Exception e) 
                { }
                return null;
            }

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            tcs.SetResult(true);
            return tcs.Task;
        }



        // GET: /Contact/
        public async Task<ActionResult> Index()
        {
            if (!await EnsureLogin())
            {
                return this.RedirectToAction("Index", "Home");
            }

            try
            {
                var list = await MobileService.GetTable<UserDomain>().ToListAsync();
            }
            catch (Exception ex)
            {
                
            }
            


            var list2 = await MobileService.GetTable<Contact>().ToListAsync();
            return View(list2);
        }

        // GET: /Contact/Details/5
        public async Task<ActionResult> Details(int id = 0)
        {
            if (!await EnsureLogin())
            {
                return this.RedirectToAction("Index", "Home");
            }

            var contacts = await MobileService.GetTable<Contact>().Where(c => c.Id == id).ToListAsync();
            if (contacts.Count == 0)
            {
                return HttpNotFound();
            }
            return View(contacts[0]);
        }

        // GET: /Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (!await EnsureLogin())
                {
                    return RedirectToAction("Index", "Home");
                }

                var table = MobileService.GetTable<Contact>();
                await table.InsertAsync(contact);
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: /Contact/Edit/5
        public async Task<ActionResult> Edit(int id = 0)
        {
            if (!await EnsureLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var contacts = await MobileService.GetTable<Contact>().Where(c => c.Id == id).ToListAsync();
            if (contacts.Count == 0)
            {
                return HttpNotFound();
            }
            return View(contacts[0]);
        }

        // POST: /Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                if (!await EnsureLogin())
                {
                    return RedirectToAction("Index", "Home");
                }

                await MobileService.GetTable<Contact>().UpdateAsync(contact);
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: /Contact/Delete/5
        public async Task<ActionResult> Delete(int id = 0)
        {
            if (!await EnsureLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            var contacts = await MobileService.GetTable<Contact>().Where(c => c.Id == id).ToListAsync();
            if (contacts.Count == 0)
            {
                return HttpNotFound();
            }

            return View(contacts[0]);
        }

        // POST: /Contact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!await EnsureLogin())
            {
                return RedirectToAction("Index", "Home");
            }

            await MobileService.GetTable<Contact>().DeleteAsync(new Contact { Id = id });
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                MobileService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
