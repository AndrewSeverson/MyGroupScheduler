using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GroupScheduler.Infrastructure.Data
{
    public static class Enums
    {
        // this enum corresponds with the type of repeat schedule event it is
        enum ScheduleEventType
        {
            Single = 1,
            Week,
            TwoWeek,
            Month,
            Year
        }
    }
}