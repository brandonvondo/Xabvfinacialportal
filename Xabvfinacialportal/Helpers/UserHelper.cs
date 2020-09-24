using Xabvfinacialportal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Ajax.Utilities;
using System.Web.Helpers;
using Newtonsoft.Json;

namespace Xabvfinacialportal.Helpers

{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));


        public void ChangeLastName(string lastName)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            user.LastName = lastName;
            db.SaveChanges();
        }

        public void ChangeFirstName(string firstName)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            user.FirstName = firstName;
            db.SaveChanges();
        }

        public string GetAvatarPath()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            return user.AvatarPath;
        }

        public string GetUserId()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return userId;
        }

        public ApplicationUser GetUser()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            return user;
        }

        public ApplicationUser GetUserById(string userId)
        {
            var user = db.Users.Find(userId);
            return user;
        }

        public ApplicationUser GetUserByDisplayName(string displayName)
        {
            var user = db.Users.Where(u => u.DisplayName == displayName).FirstOrDefault();
            return user;
        }

        public string GetFullName()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }
        public string GetFullName(string userId)
        {
            var user = db.Users.Find(userId);
            var firstName = user.FirstName;
            var lastName = user.LastName;
            return firstName + " " + lastName;
        }

        public string LastNameFirst()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            return $"{user.LastName}, ${user.FirstName}";
        }

        public ICollection<BudgetItem> GetUserItems(ApplicationUser user)
        {
            var list = user.Budgets.ToList();
            List<BudgetItem> itemList = new List<BudgetItem>();

            foreach (var budget in list)
            {
                if (budget.UserId == user.Id)
                {
                    foreach(var item in budget.Items.ToList())
                    {
                        itemList.Add(item);
                    }
                }
            }

            return itemList;
        }

        public List<decimal> BudgetItemGraphData(int id)
        {
            var budget = db.Budgets.Where(b => b.Id == id).FirstOrDefault();
            var items = budget.Items.ToList();
            List<decimal> targetList = new List<decimal>();
            foreach(var item in items)
            {
                targetList.Add(item.TargetAmount);
            }

            return targetList;
        }

        public List<string> BudgetItemGraphLables(int id)
        {
            var budget = db.Budgets.Where(b => b.Id == id).FirstOrDefault();
            var items = budget.Items.ToList();
            List<string> labelList = new List<string>();
            foreach (var item in items)
            {
                labelList.Add(item.ItemName);
            }

            return labelList;
        }
    }
}
