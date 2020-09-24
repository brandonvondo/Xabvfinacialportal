using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Enums;

namespace Xabvfinacialportal.ViewModels
{
    public class CreateBankAccVM
    {
            public string AccountName { get; set; }
            public AccountType AccountType { get; set; }
            public string StartingBalance { get; set; }
            public string WarningBalance { get; set; }
    }
}