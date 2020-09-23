using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.ViewModels
{
    public class ConfigureHouseVM
    {
        public ICollection<BankAccountVM> BankAccounts { get; set; }
        public ICollection<BudgetVM> Budgets { get; set; }
    }

    public class BankAccountVM
    {
        public string Name { get; set; }
        public int Type { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal WarningBalance { get; set; }
    }

    public class BudgetVM
    {
        public string Name { get; set; }
        public ICollection<ItemVM> Items { get; set; }
    }

    public class ItemVM
    {
        public string Name { get; set; }
        public decimal TargetValue { get; set; }
    }
}