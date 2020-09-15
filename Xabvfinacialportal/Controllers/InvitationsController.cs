using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xabvfinacialportal.Extensions;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Head")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(String email)
        {
            if (User.Identity.GetHouseholdId() > 0 && User.IsInRole("Head") && email != null)
            {
                Invitation invitation = new Invitation((int)User.Identity.GetHouseholdId());
                invitation.Code = Guid.NewGuid();
                invitation.RecipientEmail = email;
                db.Invitations.Add(invitation);
                db.SaveChanges();

                await invitation.SendInvitation();

                return RedirectToAction("Index","Home");
                
            }

            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
