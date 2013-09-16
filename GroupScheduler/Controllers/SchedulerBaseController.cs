using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupScheduler.Infrastructure.Database.Classes;

namespace GroupScheduler.Controllers
{
    public class SchedulerBaseController : Controller
    {

        public SchedulerBaseController(ISchedulerContext schedulerContext)
        {
            this.SchedulerContext = schedulerContext;
        }

        protected ISchedulerContext SchedulerContext { get; private set; }
    }
}
