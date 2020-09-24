using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.Helpers
{
    public class BankAccountHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public BankAccount GetBankAccountById(int id)
        {
            var account = db.BankAccounts.Where(b => b.Id == id).FirstOrDefault();
            return account;
        }

        public string GetBankAccountNameById(int id)
        {
            var account = db.BankAccounts.Where(b => b.Id == id).FirstOrDefault();
            if (account != null)
            {
                return account.AccountName.ToString();
            }

            return "Default Account";
        }

        public string TransactionBudgetItemName(int id)
        {
            var transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return "N/A";
            }
            var bId = transaction.BudgetItemId;
            var budgetitem = db.BudgetItems.Where(b => b.Id == bId).FirstOrDefault();
            return budgetitem.ItemName;
        }
    }
}