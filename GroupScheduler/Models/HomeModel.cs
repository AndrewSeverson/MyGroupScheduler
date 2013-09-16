using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Data.Stores;

namespace GroupScheduler.Models
{
    public class HomeModel
    {
        public User CurrentUser { get; set; }

        public List<Group> UserGroups { get; set; } 
    }
}