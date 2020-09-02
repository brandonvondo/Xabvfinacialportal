﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Models
{
    public class Budget
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int Id { get; set; }

        // Parents
        public int HouseHoldId { get; set; }
        public virtual Houshold Household { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        // Actual Properties
        public DateTime Created { get; set; }
        [Display(Name = "Name")]
        public string BudgetName { get; set; }
        [Display(Name = "Current Amount")]
        public decimal CurrentAmount { get; set; }
        [NotMapped]
        [Display(Name = "Target Amount")]
        public decimal TargetAmount // What the user expects to spend in a category for a time period
        {
            get
            {
                var target = db.BudgetItems.Where(bI => bI.BudgetId == Id ).Count();
                return target != 0 ? db.BudgetItems.Where(bI => bI.BudgetId == Id ).Sum(s => s.TargetAmount ) : 0
            }
        }

        // Children
        public virtual ICollection<BudgetItem> Items { get; set; }

        // Constructor
        public Budget()
        {
            Items = new HashSet<BudgetItem>();
            Created = DateTime.Now;
            UserId = HttpContext.Current.User.Identity.GetUserId();
        }
    }
}