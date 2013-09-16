using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Database;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public class SchedulerContext: DBConnect, ISchedulerContext
    {
        public SchedulerContext(ISchedulerUserService schedulerUserService)
        {
            this.User = schedulerUserService.GetCurrentSchedulerUser();
        }

        public User User { get; private set; }
    }
}