using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Models
{
    public class Household
    {
        public int Id { get; set; }

        // Properties
        [Display(Name = "Name")]
        public string HouseholdName { get; set; }
        public string Greeting { get; set; }
        public DateTime Created { get; set; }
        [Display(Name = "Delete Household")]
        public bool IsDeleted { get; set; }

        // Children
        public virtual ICollection<ApplicationUser> Members { get; set; }
        public virtual ICollection<BankAccount> BankAccounts { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }

        // Constructor
        public Houshold()
        {
            Members = new HashSet<ApplicationUser>();
            BankAccounts = new HashSet<BankAccount>();
            Invitations = new HashSet<Invitation>();
            Notifications = new HashSet<Notification>();
            Budgets = new HashSet<Budget>();
            Created = DateTime.Now;
        }

    }
}