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

    }
}