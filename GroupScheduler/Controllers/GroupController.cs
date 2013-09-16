using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GroupScheduler.Infrastructure.Data.Stores;
using GroupScheduler.Infrastructure.Database.Classes;
using GroupScheduler.Models;
using GroupScheduler.Classes;

namespace GroupScheduler.Controllers
{
    public class GroupController : SchedulerBaseController
    {
        private readonly GroupDb groupDb;
        private readonly ISchedulerUserService schedulerUserService;
        private readonly SchedulerDb schedulerDb;
        //
        // GET: /Group/

        public GroupController(ISchedulerContext schedulerContext, GroupDb groupDb, ISchedulerUserService schedulerUserService, SchedulerDb schedulerDb)
            : base(schedulerContext)
        {
            this.groupDb = groupDb;
            this.schedulerUserService = schedulerUserService;
            this.schedulerDb = schedulerDb;
        }

        public ActionResult ViewGroup(int groupId)
        {
            // grab the group being viewed
            Group group = groupDb.GetGroupInformation(groupId);
            // grab dictionary of group members with the group members userId as the key
            Dictionary<int, GroupMember> members = groupDb.GetGroupMembersForGroup(groupId);
            // the user might not be logged in, so if they are not, we will give them a fake id
            int userId = SchedulerContext.User == null ? -1 : SchedulerContext.User.UserId;
            // if the user has access to the page or if the page is public, allow them to view, otherwise redirect to error page
            if (group.IsPublic || members.ContainsKey(userId))
            {
                group.Members = new List<GroupMember>(members.Values);
                ViewGroupModel model = new ViewGroupModel
                {
                    CurrentUser = SchedulerContext.User,
                    Group = group,
                    IsAdmin = false,
                    IsMember = false,
                    GroupNews = groupDb.GetAllGroupNews(groupId)
                };
                // if a user is currently logged in, check if they are a member of the group
                if (model.CurrentUser != null)
                {
                    model.IsMember = members.ContainsKey(SchedulerContext.User.UserId);
                    // if the user is a member, check if they are an admin
                    if (model.IsMember)
                    {
                        GroupMember user = new GroupMember();
                        members.TryGetValue(model.CurrentUser.UserId, out user);
                        model.IsAdmin = user.IsAdmin;
                    }
                }
                return this.View(model);
            }
            // return error page here
            return this.RedirectToAction("Index", "Home");
        }

        public ActionResult CreateGroup()
        {
            // We want to pass in the current user so we can check if anyone is logged in or not.
            // If the user is not logged in, we can provide them a link to log in or register
            CreateGroupModel model = new CreateGroupModel {User = SchedulerContext.User};
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateGroup(CreateGroupModel model)
        {
            if (ModelState.IsValid)
            {
                Group newGroup = new Group
                    {
                        Name = model.Group,
                        IsPublic = model.PublicGroup,
                        Description = model.Description
                    };
                int groupId = groupDb.CreateNewGroup(newGroup, this.SchedulerContext.User);
                return this.RedirectToAction("ViewGroup", new {groupId});
            }
            else
            {
                this.ModelState.AddModelError("", "There was an error in your information. Please provide valid information.");
                return View(model);
            }
        }

        #region Add Group Event

        public ActionResult AddGroupEvent(int groupId)
        {
            // if they are not logged in, return to home
            if (SchedulerContext.User == null)
            {
                return this.RedirectToAction("ViewGroup", new { groupId });
            }
            AddGroupEventModel model = new AddGroupEventModel
                {
                    GroupId = groupId,
                    EventRepeatTypes = schedulerDb.GetAllEventRepeatType()
                };
            return this.View(model);
        }

        #endregion 

        #region Group News

        public ActionResult AddGroupNews(int groupId)
        {
            // if they are not logged in, return to home
            if (SchedulerContext.User == null)
            {
                return this.RedirectToAction("ViewGroup", new{groupId});
            }
            // grab groups memebers for this group
            Dictionary<int, GroupMember> members = groupDb.GetGroupMembersForGroup(groupId);
            // check if the current user is an admin and should be able to add a group news
            GroupMember member = new GroupMember();
            if (members.TryGetValue(SchedulerContext.User.UserId, out member))
            {
                // this person is a member of the group, now check and see if they are an admin
                if (!member.IsAdmin)
                {
                    // this person should be be here, return them to the group
                    return this.RedirectToAction("ViewGroup", "Group", new { groupId });
                }
            }
            else
            {
                // this person is not a member so return them to the home page
                return this.RedirectToAction("Index", "Home");
            }
            AddGroupNewsModel model = new AddGroupNewsModel
            {
                GroupId = groupId
            };
            return this.View(model);
        }

        [HttpPost]
        public ActionResult AddGroupNews(AddGroupNewsModel model)
        {
            if (!ModelState.IsValid)
            {
                this.ModelState.AddModelError("", "There was an error in your information. Please provide valid information.");
            }
            GroupNews news = new GroupNews
            {
                Poster = new User { UserId = this.SchedulerContext.User.UserId },
                Subject = model.Subject,
                Text = model.Text,
                GroupId = model.GroupId
            };
            groupDb.InsertGroupNews(news);
            return RedirectToAction("ViewGroup", "Group", new { groupId = model.GroupId });
        }

        #endregion

        #region Manage Group Members Page

        public ActionResult ManageGroupMembers(int groupId)
        {
            // check if they are an admin
            if (!userIsAdminOfGroup(groupId))
            {
                return this.RedirectToAction("ViewGroup", new {groupId});
            }
            //grab the group information
            ManageGroupMembersModel model = new ManageGroupMembersModel
                {
                    Group = groupDb.GetGroupInformation(groupId)
                };
            // grab the dictionary of group members
            Dictionary<int, GroupMember> members = groupDb.GetGroupMembersForGroup(groupId);
            // changes member who created the group to the owner
            members[model.Group.OwnerId].IsOwner = true;
            // if the current user is owner, pass that into model
            model.UserIsOwner = members[SchedulerContext.User.UserId].IsOwner;
            // make it a list
            model.Group.Members = members.Select(m => m.Value).ToList();
            return this.View(model);
        }

        [HttpPost]
        public JsonResult UpdateMemberRole(GroupMember updatedMember, int groupId)
        {
            // if the user is not an admin, the should not be able to do anything
            if (!userIsAdminOfGroup(groupId))
            {
                return this.Json(false);
            }
            return this.Json(groupDb.UpdateMemberRole(updatedMember, groupId));
        }

        public ActionResult AddNewMember(int groupId)
        {
            // check if they are an admin
            if (!userIsAdminOfGroup(groupId))
            {
                return this.RedirectToAction("ViewGroup", new { groupId });
            }
            return this.View(new AddNewMemberModel());
        }

        [HttpPost]
        public JsonResult RemoveMemberFromGroup(int userId, int groupId)
        {
            // if the user is not an admin, the should not be able to do anything
            if (!userIsAdminOfGroup(groupId))
            {
                return this.Json(false);
            }
            return this.Json(groupDb.RemoveMemberFromGroup(userId, groupId));
        }

        [HttpPost]
        public ActionResult AddNewMember(AddNewMemberModel model)
        {
            // check if they are an admin
            if (!userIsAdminOfGroup(model.GroupId))
            {
                return this.RedirectToAction("ViewGroup", new { model.GroupId });
            }
            // check if the provided email is actually a user
            dynamic userInfo = schedulerUserService.GetSchedulerUserLogInInformation(model.Email);
            if (userInfo == null)
            {
                // user does not exist
                this.ModelState.AddModelError("", "The email you provided is not associated with any user accounts. Please make sure the email is linked to a user.");
                return this.View(model);
            }
            // grab groups memebers for this group
            Dictionary<int, GroupMember> members = groupDb.GetGroupMembersForGroup(model.GroupId);
            // make sure this provided user is not already a member in the group
            GroupMember member = new GroupMember();
            if (members.ContainsKey(userInfo.UserId))
            {
                // user already in the group as a member
                this.ModelState.AddModelError("", "This user is already a member of the group.");
                return this.View(model);
            }
            if (groupDb.InsertGroupMember(model.GroupId, userInfo.UserId, model.IsAdmin))
            {
                return this.RedirectToAction("ManageGroupMembers", new {groupId = model.GroupId});
            }
            else
            {
                // error inserting this user
                this.ModelState.AddModelError("", "There was an error inserting this user. Please refresh the page and try again.");
                return this.View(model);
            }
        }
        #endregion

        #region Private Methods

        private bool userIsAdminOfGroup(int groupId)
        {
            // if they are not logged in, cannot be an admin
            if (SchedulerContext.User == null)
            {
                return false;
            }
            // grab groups memebers for this group
            Dictionary<int, GroupMember> members = groupDb.GetGroupMembersForGroup(groupId);
            // check if the current user is an admin
            GroupMember member = new GroupMember();
            if (members.TryGetValue(SchedulerContext.User.UserId, out member))
            {
                // this person is a member of the group, now check and see if they are an admin
                if (!member.IsAdmin)
                {
                    // this person is a member but not an admin
                    return false;
                }
            }
            else
            {
                // this person is not a member so return false
                return false;
            }
            // is an admin
            return true;
        }

        #endregion

    }
}
