using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupScheduler.Models
{
    public class AddNewMemberModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Member is Admin?")]
        public bool IsAdmin { get; set; }

        public int GroupId { get; set; }
    }
}