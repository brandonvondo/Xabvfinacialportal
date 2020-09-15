using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Xabvfinacialportal.Models;

namespace Xabvfinacialportal.Helpers
{

    public class RoleHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private UserHelper userHelper = new UserHelper();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }

        public string ListUserRole(string userId)
        {
            return userManager.GetRoles(userId).FirstOrDefault();
        }

        public string ListUserRole()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            return userManager.GetRoles(userId).FirstOrDefault();
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public void UpdateUserRole(string userId, string roleName)
        {
            var user = db.Users.Find(userId);
            var roleOld = ListUserRole(userId);
            if (roleOld != null)
            {
                userManager.RemoveFromRole(userId, roleOld);
            }
            userManager.AddToRole(userId, roleName);
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ApplicationUser FindHouseholdHead(ICollection<ApplicationUser> list)
        {
            var userDefault = userHelper.GetUser();
            foreach (var user in list)
            {
                if (IsUserInRole(user.Id, "Head"))
                    return user;
            }
            return userDefault;
        }

        public ICollection<ApplicationUser> FindHouseholdMembers(ICollection<ApplicationUser> list)
        {
            var resultList = new List<ApplicationUser>();
            foreach (var user in list)
            {
                if (IsUserInRole(user.Id, "Member"))
                    resultList.Add(user);
            }
            return resultList;
        }

        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                    resultList.Add(user);
            }
            return resultList;
        }

        public SelectList SelectListRoles()
        {
            return new SelectList(db.Roles, "Name", "Name");
        }
    }

}