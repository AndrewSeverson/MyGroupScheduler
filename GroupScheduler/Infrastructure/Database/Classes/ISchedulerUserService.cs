using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Data;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public interface ISchedulerUserService
    {
        [CanBeNull]
        User GetCurrentSchedulerUser();

        void SetCurrentSchedulerUser([NotNull] User schedulerUser);

        void SetAuthenticationCookie(string email, bool remember);

        void Logout();

        [CanBeNull]
        dynamic GetSchedulerUserLogInInformation([NotNull]string email);
    }
}