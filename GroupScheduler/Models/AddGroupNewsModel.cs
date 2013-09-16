using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroupScheduler.Models
{
    public class AddGroupNewsModel
    {
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "News Subject")]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "News Text")]
        [StringLength(8000)]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }
}