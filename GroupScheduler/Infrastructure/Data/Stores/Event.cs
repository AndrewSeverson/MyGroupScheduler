using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupScheduler.Infrastructure.Data.Stores
{
    public class Event
    {
        public int EventId { get; set; }

        public string EventName { get; set; }

        public Group Group { get; set; }

        public DateTime EventDateTime { get; set; }

        public string EventDateTimeString { get { return EventDateTime.ToShortDateString() + " " + EventDateTime.ToShortTimeString(); } }

        public DateTime EventEndDateTime { get; set; }

        public string EventEndDateTimeString { get { return EventEndDateTime.ToShortDateString() + " " + EventEndDateTime.ToShortTimeString(); } }

        public EventRepeatType EventRepeatType { get; set; }

        public string EventDescription { get; set; }
    }
}