using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xabvfinacialportal.Extensions;
using Xabvfinacialportal.Helpers;
using Xabvfinacialportal.Models;
using Xabvfinacialportal.ViewModels;

namespace Xabvfinacialportal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private RoleHelper roleHelper = new RoleHelper();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateHouseholdVM housename)
        {
            if (ModelState.IsValid)
            {
                Household household = new Household();
                household.HouseholdName = housename.HouseHoldName;
                db.Households.Add(household);
                db.SaveChanges();

                var user = db.Users.Find(User.Identity.GetUserId());

                user.HouseholdId = household.Id;

                foreach (var account in user.Accounts.ToList())
                {
                    account.HouseholdId = household.Id;
                    db.Entry(account).State = EntityState.Modified;
                }

                db.SaveChanges();
                roleHelper.UpdateUserRole(user.Id, "Head");
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);

                return RedirectToAction("ConfigureHouse");
            }

            return View(housename);
        }

        // GET: ConfigureHousehold after creation
        [Authorize(Roles = "Head")]
        [HttpGet]
        public ActionResult ConfigureHouse()
        {
            var model = new ConfigureHouseVM();
            var HouseholdId = User.Identity.GetHouseholdId();
            if (HouseholdId == null)
            {
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ConfigureHouse(ConfigureHouseVM houseSetup)
        {
            foreach(var account in houseSetup.BankAccounts)
            {
                BankAccount bankAccount = new BankAccount(account.StartingBalance, account.WarningBalance, account.Name);
                db.BankAccounts.Add(bankAccount);
                db.SaveChanges();
            }
            foreach(var b in houseSetup.Budgets)
            {
                Budget budget = new Budget(b.Name);
                db.Budgets.Add(budget);
                db.SaveChanges();
                var budgetId = budget.Id;
                foreach(var item in b.Items)
                {
                    BudgetItem budgetItem = new BudgetItem(item.TargetValue, item.Name, budgetId);
                    db.BudgetItems.Add(budgetItem);
                    db.SaveChanges();
                }
            }
            var myJson = "/Home/Index";
            return Json(myJson);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdName,Greeting,Created,IsDeleted")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // POST: Leave Household  user hhId and bank account hhId need to null
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Member")]
        public async Task<ActionResult> LeaveAsyncHH()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var role = roleHelper.ListUserRole(user.Id);
            user.HouseholdId = null;

            foreach (var account in user.Accounts)
            {
                account.HouseholdId = null;
            }

            db.SaveChanges();

            roleHelper.UpdateUserRole(user.Id, "New User");
            await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "Head")]
        public ActionResult LeaveAsyncHOH()
        {
            var hhId = (int)User.Identity.GetHouseholdId();
            var house = db.Households.Where(h => h.Id == hhId).FirstOrDefault();
            if (householdHelper.CanLeave(hhId))
            {
                return PartialView("_HeadNoMemLeave");
            }

            ViewBag.MemberListId = new SelectList(house.Members.Where(m => m.Id != User.Identity.GetUserId()), "Id", "FullName");
            return PartialView("_HeadMemLeave");
        }


        // Post: Leave Household as head
        [HttpPost]
        [Authorize(Roles = "Head")]
        public async Task<ActionResult> LeaveAsyncHOH(LeaveHHHeadVM leave)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var role = roleHelper.ListUserRole(user.Id);
            var household = db.Households.Where(h => h.Id == user.HouseholdId).FirstOrDefault();
            string newHeadId = leave.MemberListId;
            if (newHeadId == null)
            {
                household.IsDeleted = true;
                user.HouseholdId = null;

                foreach (var account in user.Accounts)
                {
                    account.HouseholdId = null;
                }

                db.SaveChanges();

                roleHelper.UpdateUserRole(user.Id, "New User");
                await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);
                return RedirectToAction("Index", "Home");
            }
            var newHead = db.Users.Find(newHeadId);
            
            user.HouseholdId = null;


            foreach (var account in user.Accounts)
            {
                account.HouseholdId = null;
            }

            db.SaveChanges();
            roleHelper.UpdateUserRole(user.Id, "New User");
            roleHelper.UpdateUserRole(newHeadId, "Head");
            

            await AuthorizeExtensions.RefreshAuthentication(HttpContext, user);

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
