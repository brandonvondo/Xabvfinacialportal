using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        // Parents
        public int HouseholdId { get; set; }
        public virtual Household Household { get; set; }

        // Properties
        public bool IsValid { get; set; }
        public DateTime Created { get; set; }
        public int TTL { get; internal set; } //Time to Live
        //if(DateTime.Now > Created.AddDays(TTL)){IsValid = false}
        [Display(Name = "Recipient Email")]
        public string RecipientEmail { get; set; }
        public Guid Code { get; set; }

        // Constructor
        public Invitation()
        {
            Created = DateTime.Now;
            IsValid = true;
            TTL = 7;
        }
    }
}