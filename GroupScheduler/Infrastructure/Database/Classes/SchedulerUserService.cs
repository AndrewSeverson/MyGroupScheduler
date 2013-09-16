using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using GifinIt.Infrastucture.Data;
using GifinIt.Infrastucture.Database;
using GroupScheduler.Classes;
using MySql.Data.MySqlClient;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public class SchedulerUserService : DBConnect, ISchedulerUserService
    {
        public const string SCHEDULER_USER_SESSION_KEY = "SchedulerUser";

        private readonly IAccountDb accountDb;
        private readonly HttpContextBase context;
        [CanBeNull]
        private readonly string email;

        public SchedulerUserService(IAccountDb accountDb, HttpContextBase context)
        {
            this.accountDb = accountDb;
            this.context = context;

            if (this.context.Session != null)
            {
                User user = this.context.Session[SCHEDULER_USER_SESSION_KEY] as User;
                if (user != null && user.Email != null)
                {
                    this.email = user.Email;
                }
            }
        }

        [CanBeNull]
        public User GetCurrentSchedulerUser()
        {
            if (string.IsNullOrEmpty(this.email))
            {
                return null;
            }

            User user = this.GetCurrentSchedulerUserFromSession();

            if (user == null)
            {
                //portalUser = this.GetUser(this.username);
                user = new User();
                if (user != null)
                {
                    this.SetCurrentSchedulerUser(user);
                }
            }
            
            return user;
        }

        [CanBeNull]
        private User GetCurrentSchedulerUserFromSession()
        {
            return this.context.Session[SCHEDULER_USER_SESSION_KEY] as User;
        }

        public void SetCurrentSchedulerUser(User schedulerUser)
        {
            this.context.Session[SCHEDULER_USER_SESSION_KEY] = schedulerUser;
        }

        [CanBeNull]
        public dynamic GetSchedulerUserLogInInformation([NotNull] string providedEmail){
            const string queryText = @"SELECT
                                        UserId,
                                        DisplayName,
                                        Password,
                                        Salt
                                    FROM
                                        mysql_60747_scheduler.user
                                    WHERE
                                        Email = @Email;
                                    ";
            try
            {
                return this.RunQuery<dynamic>(queryText, new
                {
                    Email = providedEmail
                }).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public User GetUser(string email)
        {
            const string queryText = @"SELECT
                                        UserId,
                                        DisplayName,
                                        Email
                                    FROM
                                        mysql_60747_scheduler.user
                                    WHERE
                                        Email = @Email;
                                    ";
            try
            {
                return this.RunQuery<User>(queryText, new
                {
                    Email = email
                }).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public void SetAuthenticationCookie(string email, bool remember)
        {
            FormsAuthentication.SetAuthCookie(email, remember);
        }

        public void Logout()
        {
            this.context.Session.Abandon();
            FormsAuthentication.SignOut();
        }
    }
}