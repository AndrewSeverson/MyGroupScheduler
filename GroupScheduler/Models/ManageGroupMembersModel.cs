using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Data.Stores;

namespace GroupScheduler.Models
{
    public class ManageGroupMembersModel
    {
        public Group Group { get; set; }
        public bool UserIsOwner { get; set; }
    }
}