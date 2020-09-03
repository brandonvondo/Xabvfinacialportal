namespace Xabvfinacialportal.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Xabvfinacialportal.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Xabvfinacialportal.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Xabvfinacialportal.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Head"))
            {
                roleManager.Create(new IdentityRole { Name = "Head" });
            }

            if (!context.Roles.Any(r => r.Name == "Member"))
            {
                roleManager.Create(new IdentityRole { Name = "Member" });
            }

            if (!context.Roles.Any(r => r.Name == "New User"))
            {
                roleManager.Create(new IdentityRole { Name = "New User" });
            }

            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));
            if (!context.Users.Any(u => u.Email == "xabv@mailinator.com"))
            {
                userManager.Create(new ApplicationUser()
                {
                    Email = "xabv@mailinator.com",
                    UserName = "xabv@mailinator.com",
                    FirstName = "Anya",
                    LastName = "Vondo"
                }, "135790bjvdd1shoe");

                var userID = userManager.FindByEmail("xabv@mailinator.com").Id;

                userManager.AddToRole(userID, "Admin");
            }
        }
    }
}