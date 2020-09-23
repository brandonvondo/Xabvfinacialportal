using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xabvfinacialportal.Helpers;
using Xabvfinacialportal.Models;
using Xabvfinacialportal.ViewModels;

namespace Xabvfinacialportal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserHelper userHelper = new UserHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserProfile(string displayname, string section)
        {
            if (displayname != null)
            {
                var model = userHelper.GetUserByDisplayName(displayname);

                if (model != null)
                {
                    if (section != null)
                    {
                        ViewBag.Shown = section.ToString();
                    }

                    return View(model);
                }
                   
            }

            return RedirectToAction("Error");
        }

        [HttpPost]
        public ContentResult ProfilePost(xEditableVM xdt)
        {
            var n = xdt.Name;
            var user = userHelper.GetUser();
            string msg = "Oops something didn't go quite right.";
            switch(n)
            {
                case "firstname":
                    userHelper.ChangeFirstName(xdt.Value);
                    msg = "First Name has been updated to ";
                    break;
                case "lastname":
                    userHelper.ChangeLastName(xdt.Value);
                    msg = "Last Name has been updated to";
                    break;
            }
            return Content(msg);
        }

        [HttpPost]
        public async Task<ContentResult> EmailPost(xEditableVM xdt)
        {
            var userId = userHelper.GetUserId();
            // get user object from the storage
            var user = await userManager.FindByIdAsync(userId);

            // change username and email
            user.UserName = xdt.Value;
            user.Email = xdt.Value;

            // Persiste the changes
            await userManager.UpdateAsync(user);

            string msg = "Email has been updated to";

            return Content(msg);
        }

        [HttpGet]
        public ActionResult NotifLoader()
        {
            var user = userHelper.GetUser();
            if (user.Notifications.Count > 0)
            {
                ViewBag.Notifs = user.Notifications.ToList();
            }
            else
            {
                ViewBag.Notifs = null;
            }
            return PartialView("_NotifLoader");
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}