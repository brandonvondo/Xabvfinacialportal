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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserHelper userHelper = new UserHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account).Include(t => t.BudgetItem).Include(t => t.User);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        [HttpGet]
        public ActionResult CreateModal(int id, string purpose)
        {
            var user = userHelper.GetUser();
            var list = userHelper.GetUserItems(user);
            var model = new TransactionModalVM();
            model.Purpose = purpose;
            if (id == 0)
            {
                switch(purpose)
                {

                    case "withdraw":
                        ViewBag.AccountId = new SelectList(user.Accounts, "Id", "AccountName");
                        ViewBag.BudgetItemId = new SelectList(list, "Id", "ItemName");
                        return PartialView("_TransactionModalWithdrawNeedId", model);
                    case "deposit":
                        ViewBag.AccountId = new SelectList(user.Accounts, "Id", "AccountName");
                        return PartialView("_TransactionModalDepositNeedId", model);
                    case "transfer":
                        ViewBag.AccountId = new SelectList(user.Household.BankAccounts.Where(b => b.UserId != user.Id), "Id", "AccountName");
                        ViewBag.TransferId = new SelectList(db.BankAccounts.Where(b => b.UserId == user.Id), "Id", "AccountName");
                        return PartialView("_TransactionModalTransferNeedId", model);
                }
            }
            model.AccountId = id;
            switch (purpose)
            {
                case "withdraw":
                    ViewBag.BudgetItemId = new SelectList(list, "Id", "ItemName");
                    return PartialView("_TransactionModalWithdraw", model);
                case "deposit":
                    return PartialView("_TransactionModalDeposit", model);
            }

            ViewBag.TransferId = new SelectList(db.BankAccounts.Where(b => b.UserId == user.Id), "Id", "AccountName");
            return PartialView("_TransactionModalTransfer", model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionModalVM form)
        {
            var user = userHelper.GetUser();
            BudgetItem budgetItem = db.BudgetItems.Find(form.BudgetItemId);
            BankAccount account = db.BankAccounts.Find(form.AccountId);
            Transaction transaction = new Transaction();
            transaction.AccountId = form.AccountId;
            transaction.BudgetItemId = form.BudgetItemId;
            transaction.TransactionType = form.TransactionType;
            transaction.Amount = decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency);
            transaction.Memo = form.Memo;
            switch (form.Purpose)
            {
                case "withdraw":
                    budgetItem.CurrentAmount = (budgetItem.CurrentAmount + decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency));
                    account.CurrentBalance = (account.CurrentBalance - decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency));
                    db.Entry(account).State = EntityState.Modified;
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                    break;
                case "deposit":
                    account.CurrentBalance = (account.CurrentBalance + decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency));
                    db.Entry(account).State = EntityState.Modified;
                    db.Transactions.Add(transaction);
                    db.SaveChanges();
                    break;
                case "transfer":
                    BankAccount account2 = db.BankAccounts.Where(b => b.Id == form.TransferId).FirstOrDefault();
                    transaction.Memo = $"Transfer from {user.FullName}";
                    Transaction transaction2 = new Transaction();
                    transaction2.AccountId = (int)form.TransferId;
                    transaction2.BudgetItemId = form.BudgetItemId;
                    transaction2.TransactionType = form.TransactionType;
                    transaction2.Amount = decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency);
                    transaction2.Memo = form.Memo;
                    account.CurrentBalance = (account.CurrentBalance + decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency));
                    account2.CurrentBalance = (account2.CurrentBalance - decimal.Parse(form.Amount, System.Globalization.NumberStyles.Currency));
                    db.Entry(account).State = EntityState.Modified;
                    db.Entry(account2).State = EntityState.Modified;
                    db.Transactions.Add(transaction);
                    db.Transactions.Add(transaction2);
                    db.SaveChanges();
                    break;
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "UserId", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "ItemName", transaction.BudgetItemId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,BudgetItemId,UserId,TransactionType,Created,Amount,Memo,IsDeleted")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountId = new SelectList(db.BankAccounts, "Id", "UserId", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "ItemName", transaction.BudgetItemId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", transaction.UserId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
