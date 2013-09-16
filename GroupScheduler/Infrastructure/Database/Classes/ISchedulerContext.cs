using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public interface ISchedulerContext
    {
        User User { get; }
    }
}