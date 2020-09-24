using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.ViewModels
{
    public class TransactionModalVM
    {
        public string Purpose { get; set; }
        public int AccountId { get; set; }
        public int? TransferId { get; set; }
        public int? BudgetItemId { get; set; }
        [Display(Name = "Transaction Type")]
        public TransactionType TransactionType { get; set; }
        public string Amount { get; set; }
        public string Memo { get; set; } // What did we spend the money on "gas" "food"
    }
}