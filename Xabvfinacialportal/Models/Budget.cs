using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Xabvfinacialportal.Extensions;

namespace Xabvfinacialportal.Models
{
    public class Budget
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int Id { get; set; }

        // Parents
        public int HouseHoldId { get; set; }
        public virtual Household Household { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Actual Properties
        public DateTime Created { get; set; }
        [NotMapped]
        [Display(Name = "Created")]
        public string CreatedString 
        {
            get
            {
                string dateString = Created.ToString("MMM dd, yyyy");
                return dateString;
            }
        }
        [Display(Name = "Name")]
        public string BudgetName { get; set; }
        [NotMapped]
        [Display(Name = "Current Amount")]
        public decimal CurrentAmount // What the user expects to spend in a category for a time period
        {
            get
            {
                var target = db.BudgetItems.Where(bI => bI.BudgetId == Id).Count();
                return target != 0 ? db.BudgetItems.Where(bI => bI.BudgetId == Id).Sum(s => s.CurrentAmount) : 0;
            }
        }
        [NotMapped]
        [Display(Name = "Target Amount")]
        public decimal TargetAmount // What the user expects to spend in a category for a time period
        {
            get
            {
                var target = db.BudgetItems.Where(bI => bI.BudgetId == Id ).Count();
                return target != 0 ? db.BudgetItems.Where(bI => bI.BudgetId == Id).Sum(s => s.TargetAmount) : 0;
            }
        }

        // Children
        public virtual ICollection<BudgetItem> Items { get; set; }

        // Constructor
        public Budget(string name)
        {
            BudgetName = name;
            Items = new HashSet<BudgetItem>();
            Created = DateTime.Now;
            UserId = HttpContext.Current.User.Identity.GetUserId();
            HouseHoldId = (int)HttpContext.Current.User.Identity.GetHouseholdId();
        }

        public Budget()
        {
            Items = new HashSet<BudgetItem>();
            Created = DateTime.Now;
            UserId = HttpContext.Current.User.Identity.GetUserId();
            HouseHoldId = (int)HttpContext.Current.User.Identity.GetHouseholdId();
        }
    }
}