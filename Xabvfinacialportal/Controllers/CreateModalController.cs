using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
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
        public ActionResult CreateMultiModal(string purpose)
        {
            var user = userHelper.GetUser();
            switch (purpose)
                {
                    case "bankaccount":
                        return PartialView("_CreateBankAccount");
                    case "budget":
                        return PartialView("_CreateBudget");
                    case "budgetitem":
                    ViewBag.BudgetId = new SelectList(user.Budgets, "Id", "BudgetName");
                    return PartialView("_CreateBudgetItem");
            }

            return PartialView("_CreateBankAccount");

        }

        //[HttpPost]
        //public ActionResult CreateBankAccount()
        //{

        //}

        //[HttpPost]
        //public ActionResult CreateBudget()
        //{

        //}

        //[HttpPost]
        //public ActionResult CreateBudgetItem()
        //{

        //}

    }
}