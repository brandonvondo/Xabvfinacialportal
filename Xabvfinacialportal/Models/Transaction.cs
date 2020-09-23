using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;

namespace Xabvfinacialportal.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        // Parents
        [Display(Name = "Bank Account")]
        public int AccountId { get; set; }
        public virtual BankAccount Account { get; set; }
        public int? BudgetItemId { get; set; }
        public virtual BudgetItem BudgetItem { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Properties
        public TransactionType TransactionType { get; set; }
        public DateTime Created { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; } // What did we spend the money on "gas" "food"
        [Display(Name = "Delete Transaction")]
        public bool IsDeleted { get; set; }

        // Constructor
        public Transaction()
        {
            Created = DateTime.Now;
            UserId = HttpContext.Current.User.Identity.GetUserId();
            IsDeleted = false;
        }


    }
}