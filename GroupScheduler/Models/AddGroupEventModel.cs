using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GroupScheduler.Infrastructure.Data.Stores;

namespace GroupScheduler.Models
{
    public class AddGroupEventModel
    {
        public int GroupId { get; set; }

        [Required]
        [Display(Name = "Name of Event")]
        [StringLength(150)]
        public string EventName { get; set; }

        [Display(Name = "Event Details and Description")]
        [StringLength(2000)]
        [DataType(DataType.MultilineText)]
        public string EventDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date and Time of Event")]
        public DateTime EventDateTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "How long will the event last")]
        public DateTime EventDurration { get; set; }

        [DataType(DataType.Date)]
        public DateTime EventEndDateTime { get; set; }

        [Required]
        public List<EventRepeatType> EventRepeatTypes { get; set; } 
    }
}