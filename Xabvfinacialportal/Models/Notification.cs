using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        // Parents
        public int? HouseHoldId { get; set; }
        public virtual Household Household { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        // Properties
        public string Subject { get; set; }
        public string Body { get; set; }
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
        public bool IsRead { get; set; }

        // Constructor
        public Notification()
        {
            Created = DateTime.Now;
        }
    }
}