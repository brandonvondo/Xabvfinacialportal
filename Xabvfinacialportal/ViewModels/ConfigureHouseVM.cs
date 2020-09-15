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
        public int? HouseholdId { get; set; }
        public decimal StartingBalance { get; set; }
        public decimal WarningBalance { get; set; }
        public string AccountName { get; set; }
        public AccountType AccountType { get; set; }
        public Budget Budget { get; set; }
        public BudgetItem BudgetItem { get; set; }
    }
}