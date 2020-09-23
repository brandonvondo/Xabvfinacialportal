using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;

namespace Xabvfinacialportal.ViewModels
{
    public class TransactionModalVM
    {
        public int? BudgetItemId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Memo { get; set; } // What did we spend the money on "gas" "food"
    }
}