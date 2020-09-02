using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.Models
{
    public class Notification
    {
        public int Id { get; set; }

        // Parents
        public int HouseHoldId { get; set; }
        public virtual Household Household { get; set; }
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        // Properties
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime Created { get; set; }
        public bool IsRead { get; set; }

        // Constructor
        public Notification()
        {
            Created = DateTime.Now;
        }
    }
}