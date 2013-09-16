using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Data.Stores
{
    public class GroupNewsComment
    {
        public int Id { get; set; }

        public int GroupNewsId { get; set; }

        public string Text { get; set; }

        public User Commenter { get; set; }
    }
}