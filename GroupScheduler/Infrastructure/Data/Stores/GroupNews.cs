using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Data.Stores
{
    public class GroupNews
    {
        public int Id { get; set; }

        public string Subject { get; set; }

        public string Text { get; set; }

        public int GroupId { get; set; }

        public User Poster { get; set; }

        public DateTime PostedDate { get; set; }

        public string PostedDateString
        {
            get { return PostedDate.ToShortDateString() + " " + PostedDate.ToShortTimeString(); }
        }

        public List<GroupNewsComment> Comments { get; set; } 
    }
}