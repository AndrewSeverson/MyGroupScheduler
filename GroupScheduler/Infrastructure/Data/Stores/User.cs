using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupScheduler.Classes
{
    [Serializable]
    public class User
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string DisplayName { get; set; }
    }
}