using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Common.Logging;
using GifinIt.Infrastucture.Data;
using GifinIt.Infrastucture.Database;
using GroupScheduler.Classes;
using MySql.Data.MySqlClient;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public class AccountDb : DBConnect, IAccountDb
    {
        private readonly ILog logger;

        public AccountDb(ILog logger)
        {
            this.logger = logger;
        }

        public int RegisterNewUser([NotNull]User user, [NotNull]byte[] hashPassword, [NotNull]byte[] salt)
        {
            const string queryText = @"  INSERT INTO 
                                        mysql_60747_scheduler.user
                                    (
                                        Email,
                                        DisplayName,
                                        Password,
                                        Salt
                                    )
                                    Values
                                    (
                                        @Email,
                                        @DisplayName,
                                        @Password,
                                        @Salt
                                    );
                                    SELECT LAST_INSERT_ID() AS 'Id';
                                ";
            try
            {
                dynamic id = this.RunQuery<dynamic>(queryText, new
                {
                    user.Email,
                    user.DisplayName,
                    Password = hashPassword,
                    Salt = salt
                }).FirstOrDefault();
                return (int)id.Id;
            }
            catch (MySqlException e)
            {
                if (e.Message.StartsWith("Duplicate"))
                {
                    // The email address provided is already in use
                    return -1;
                }
                //TODO : log here
                throw;
            }
        }
    }
}