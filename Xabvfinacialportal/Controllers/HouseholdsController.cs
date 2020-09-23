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

        // POST: Leave Household       user hhId and bank account hhId need to null
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

        // Post: Leave Household as head
        [Authorize(Roles = "Head")]
        public async Task<ActionResult> LeaveAsyncHOH(FormCollection form)
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            var role = roleHelper.ListUserRole(user.Id);
            string newHeadId = form["NewHead"];
            if (newHeadId == "none")
            {
                user.Household.IsDeleted = true;
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

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
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
