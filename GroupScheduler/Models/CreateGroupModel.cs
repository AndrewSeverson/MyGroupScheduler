using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Data;
using GroupScheduler.Classes;

namespace GroupScheduler.Models
{
    public class CreateGroupModel
    {
        [CanBeNull]
        public User User { get; set; }

        [Required]
        [Display(Name = "Group Name")]
        [StringLength(100)]
        public string Group { get; set; }

        [Required]
        [Display(Name = "Public Group")]
        public bool PublicGroup { get; set; }

        [Display(Name = "Short Group Description")]
        [StringLength(100)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}