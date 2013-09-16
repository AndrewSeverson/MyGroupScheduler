using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Data;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Data.Stores
{
    [Serializable]
    public class Group
    {
        public int GroupId { get; set; }

        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public bool IsPublic { get; set; }

        public List<GroupMember> Members { get; set; }

        public string Description { get; set; }

        public int OwnerId { get; set; }
    }
}