using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Data;
using GroupScheduler.Classes;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public interface IAccountDb
    {
        int RegisterNewUser([NotNull]User user, [NotNull]byte[] hashPassword, [NotNull]byte[] salt);
    }
}