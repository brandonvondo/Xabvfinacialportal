using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Xabvfinacialportal.Extensions;
using Xabvfinacialportal.Helpers;
using Xabvfinacialportal.Models;
using Xabvfinacialportal.ViewModels;

namespace Xabvfinacialportal.Controllers
{
    [Authorize]
    public class CreateModalController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();

        [HttpGet]
        public ActionResult CreateMultiModal(int id)
        {
            var user = userHelper.GetUser();
            switch (id)
                {
                    case 1:
                        return PartialView("_CreateBankAccount");
                    case 2:
                        return PartialView("_CreateBudget");
                    case 3:
                    ViewBag.BudgetId = new SelectList(user.Budgets, "Id", "BudgetName");
                    return PartialView("_CreateBudgetItem");
            }

            return PartialView("_CreateBankAccount");

        }

        [HttpPost]
        public ActionResult CreateBankAccount(CreateBankAccVM acc)
        {
            var start = decimal.Parse(acc.StartingBalance, System.Globalization.NumberStyles.Currency);
            var warn = decimal.Parse(acc.WarningBalance, System.Globalization.NumberStyles.Currency);
            BankAccount bankAccount = new BankAccount(start, warn, acc.AccountName);
            bankAccount.AccountType = acc.AccountType;
            bankAccount.HouseholdId = (int)User.Identity.GetHouseholdId();
            db.BankAccounts.Add(bankAccount);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateBudget(CreateBudgetVM bdgt)
        {
            Budget budget = new Budget(bdgt.Name);
            budget.HouseHoldId = (int)User.Identity.GetHouseholdId();
            db.Budgets.Add(budget);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult CreateBudgetItem(CreateItemVM itm)
        {
            var target = decimal.Parse(itm.TargetValue, System.Globalization.NumberStyles.Currency);
            BudgetItem item = new BudgetItem(target, itm.Name, itm.BudgetId);
            db.BudgetItems.Add(item);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}