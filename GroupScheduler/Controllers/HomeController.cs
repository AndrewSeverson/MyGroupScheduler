using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Data.Stores;
using GroupScheduler.Infrastructure.Database.Classes;
using GroupScheduler.Models;

namespace GroupScheduler.Controllers
{
    public class HomeController : SchedulerBaseController
    {
        private readonly GroupDb groupDb;

        public HomeController(ISchedulerContext schedulerContext, GroupDb groupDb)
            : base(schedulerContext)
        {
            this.groupDb = groupDb;
        }

        public ActionResult Index()
        {
            HomeModel model = new HomeModel
                {
                    CurrentUser = SchedulerContext.User,
                };
            if (model.CurrentUser != null)
            {
                model.UserGroups = groupDb.GetUsersGroupMemberships(model.CurrentUser.UserId);
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetAllPublicGroups(string search)
        {
            List<Group> groups = groupDb.GetAllPublicGroups(search);
            var results = groups.Select(g => new
            {
                name = g.Name + (g.Description == null ? "" : " - " + g.Description),
                id = g.GroupId
            }).ToList();
            return this.Json(results);
        }

    }
}
