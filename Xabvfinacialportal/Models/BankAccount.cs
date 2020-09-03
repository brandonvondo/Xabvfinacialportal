using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;

namespace Xabvfinacialportal.Models
{
    public class BankAccount
    {
        public int Id { get; set; }

        // Parents
        public int HouseholdId { get; set; }
        public string UserId { get; set; }
        public virtual Household Household { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Bank Account Properties
        [Display(Name = "Bank Account Name")] //Display Name
        public string AccountName { get; set; } //System Name
        public DateTime Created { get; set; }
        [Display(Name = "Created")]
        public string CreatedString { get; internal set; }
        [Display(Name = "Starting Balance")]
        public decimal StartingBalance { get; internal set; }
        [Display(Name = "Current Balance")]
        public decimal CurrentBalance { get; set; }
        [Display(Name = "Warning Balance")]
        public decimal WarningBalance { get; set; }
        [Display(Name = "Delete Account")]
        public bool IsDeleted { get; set; }

        // Children
        public virtual ICollection<Transaction> Transactions { get; set; }
        public AccountType AccountType { get; set;}

        // Constructor
        public BankAccount(decimal startingBalance, decimal warningBalance, string accountName)
        {
            Transactions = new HashSet<Transaction>();
            StartingBalance = startingBalance;
            CurrentBalance = startingBalance;
            WarningBalance = warningBalance;
            Created = DateTime.Now;
            UserId = HttpContext.Current.User.Identity.GetUserId();
            CreatedString = DateTime.Now.ToString();
            AccountName = accountName;
            
        }

        public BankAccount()
        {
            StartingBalance = -1;
        }
    }
}