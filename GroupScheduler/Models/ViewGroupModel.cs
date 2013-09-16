using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Data.Stores;

namespace GroupScheduler.Models
{
    public class ViewGroupModel
    {
        public User CurrentUser { get; set; }

        public Group Group { get; set; }

        public bool IsAdmin { get; set; }

        public bool IsMember { get; set; }

        public List<GroupNews> GroupNews { get; set; } 

    }
}