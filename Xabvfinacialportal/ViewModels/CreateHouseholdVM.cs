using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xabvfinacialportal.ViewModels
{
    public class CreateHouseholdVM
    {
        [Required]
        [StringLength(20, ErrorMessage = "Please keep the household name between 4 and 20 characters.", MinimumLength = 4)]
        public string HouseHoldName { get; set; }
    }
}