using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Data.Stores
{
    public class GroupMember : User
    {
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
    }
}