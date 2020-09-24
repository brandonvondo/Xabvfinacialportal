using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.Helpers
{
    public class HouseholdHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Household GetUserHousehold()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var id = user.HouseholdId;
            var household = db.Households.Find(id);
            return household;
        }

        public Household GetHouseholdById(int id)
        {
            var household = db.Households.Find(id);
            return household;
        }

        public bool CanTransfer(int householdId)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var accounts = db.BankAccounts.Where(b => b.HouseholdId == householdId && b.UserId != userId).ToList();
            if (accounts.Count > 0)
            {
                return true;
            }

            return false;
        }

        public bool CanLeave(int householdId)
        {
            var household = db.Households.Where(h => h.Id == householdId).FirstOrDefault();
            if (household.Members.Count > 1)
            {
                return false;
            }
            else return true;
        }

    }
}