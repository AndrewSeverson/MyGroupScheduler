using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GifinIt.Infrastucture.Data;
using GifinIt.Infrastucture.Database;
using GroupScheduler.Classes;
using GroupScheduler.Infrastructure.Data.Stores;
using MySql.Data.MySqlClient;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public class GroupDb : DBConnect
    {

        public int CreateNewGroup([NotNull]Group group, User groupCreator)
        {
            DateTime currentDate = DateTime.Now;
            const string queryText = @"  INSERT INTO 
                                        mysql_60747_scheduler.group
                                    (
                                        Name,
                                        CreationDate,
                                        IsPublic,
                                        Description,
                                        Owner
                                    )
                                    Values
                                    (
                                        @Name,
                                        @CreationDate,
                                        @IsPublic,
                                        @Description,
                                        @MemberId
                                    );

                                    INSERT INTO 
                                        mysql_60747_scheduler.groupmembers
                                    (
                                        MemberId,
                                        GroupId,
                                        IsAdmin
                                    )
                                    Values
                                    (
                                        @MemberId,
                                        LAST_INSERT_ID(),
                                        1
                                    );
                                    SELECT LAST_INSERT_ID() AS GroupId; 
                                ";
            try
            {
                dynamic id = this.RunQuery<dynamic>(queryText, new
                {
                    group.Name,
                    CreationDate = currentDate,
                    group.IsPublic,
                    MemberId = groupCreator.UserId,
                    group.Description
                }).FirstOrDefault();
                return (int)id.GroupId;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public Dictionary<int, GroupMember> GetGroupMembersForGroup([NotNull]int groupId)
        {
            const string queryText = @"  SELECT
                                            MemberId AS UserId,
                                            IsAdmin,
                                            u.DisplayName,
                                            u.Email
                                        FROM
                                            mysql_60747_scheduler.groupmembers gm INNER JOIN
                                            mysql_60747_scheduler.user u ON u.UserId = gm.MemberId
                                        WHERE
                                            gm.GroupId = @GroupId;
                                    ";
            try
            {
                return this.RunQuery<GroupMember>(queryText, new
                {
                   GroupId = groupId
                }).ToDictionary(m => m.UserId);
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public Group GetGroupInformation([NotNull] int groupId)
        {
            const string queryText = @"  SELECT
                                            Name,
                                            GroupId,
                                            IsPublic,
                                            Description,
                                            CreationDate,
                                            Owner AS OwnerId
                                        FROM
                                            mysql_60747_scheduler.group
                                        WHERE
                                            GroupId = @GroupId;
                               ";
            try
            {
                return this.RunQuery<Group>(queryText, new
                    {
                        GroupId = groupId
                    }).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public List<Group> GetUsersGroupMemberships(int userId)
        {
            const string queryText = @"  SELECT
	                                        g.GroupId,
	                                        g.Name,
	                                        CASE WHEN g.Description IS NULL THEN '' ELSE g.Description END AS Description
                                        FROM
	                                        mysql_60747_scheduler.group g
                                        where
	                                        @UserId IN (
			                                        SELECT 
				                                        MemberId 
			                                        FROM 
				                                        mysql_60747_scheduler.groupmembers gm 
			                                        WHERE 
				                                        gm.GroupId = g.GroupId
		                                        )
                                    ";
            try
            {
                return this.RunQuery<Group>(queryText, new
                {
                    UserId = userId
                }).ToList();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public List<GroupNews> GetAllGroupNews(int groupId)
        {
             const string queryText = @"  SELECT
	                                        Id,
	                                        Subject,
	                                        Text,
                                            PostedDate,
                                            u.UserId,
                                            u.DisplayName
                                        FROM
	                                        mysql_60747_scheduler.groupnews INNER JOIN
                                            mysql_60747_scheduler.user u ON u.UserId = PostedByUserId
                                        where
	                                        GroupId = @GroupId
                                    ";
            try
            {
                IEnumerable<dynamic> list = this.RunQuery<dynamic>(queryText, new
                    {
                        GroupId = groupId
                    }).ToList();
                return
                    list.Select(
                        n =>
                        new GroupNews
                            {
                                Id = n.Id,
                                Subject = n.Subject,
                                Text = n.Text,
                                PostedDate = n.PostedDate,
                                Poster = new User
                                    {
                                        UserId = n.UserId, 
                                        DisplayName = n.DisplayName
                                    }
                            }).ToList();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        
        } 

        public List<Group> GetAllPublicGroups(string search)
        {
            const string queryText = @"  SELECT
	                                        GroupId,
	                                        Name,
	                                        Description
                                        FROM
	                                        mysql_60747_scheduler.group
                                        where
	                                        IsPublic = 1
                                        AND
                                            Name LIKE @Search '%';
                                    ";
            try
            {
                return this.RunQuery<Group>(queryText, new
                    {
                        Search = search
                    }).ToList();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        } 

        public bool InsertGroupMember(int groupId, int userId, bool isAdmin)
        {
            const string queryText = @"  
                                        INSERT INTO
                                            mysql_60747_scheduler.groupmembers
                                        (
                                            MemberId,
                                            GroupId,
                                            IsAdmin
                                        )
                                        VALUES
                                        (
                                            @MemberId,
                                            @GroupId,
                                            @IsAdmin
                                        );
                                    ";
            try
            {
                return this.ExecuteQuery(queryText, new
                    {
                        MemberId = userId,
                        GroupId = groupId,
                        IsAdmin = isAdmin
                    }) > 0;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public bool RemoveMemberFromGroup(int userId, int groupId)
        {
            const string queryText = @"  
                                        DELETE FROM mysql_60747_scheduler.groupmembers
                                        WHERE
                                            MemberId = @MemberId
                                            AND
                                            GroupId = @GroupId;
                                    ";
            try
            {
                return this.ExecuteQuery(queryText, new
                    {
                        MemberId = userId,
                        GroupId = groupId
                    }) > 0;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public int InsertGroupNews(GroupNews news)
        {
            const string queryText = @"  
                                        INSERT INTO
                                            mysql_60747_scheduler.groupnews
                                        (
                                            GroupId,
                                            Subject,
                                            Text,
                                            PostedByUserId,
                                            PostedDate
                                        )
                                        VALUES
                                        (
                                            @GroupId,
                                            @Subject,
                                            @Text,
                                            @PostedByUserId,
                                            NOW()
                                        );
                                        SELECT LAST_INSERT_ID() AS GroupNewsId; 
                                    ";
            try
            {
                dynamic id = this.RunQuery<dynamic>(queryText, new
                {
                    news.GroupId,
                    news.Subject,
                    news.Text,
                    PostedByUserId = news.Poster.UserId
                }).FirstOrDefault();
                return (int)id.GroupNewsId;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public bool UpdateMemberRole(GroupMember updatedMember, int groupId)
        {
            const string queryText = @"  
                                        UPDATE mysql_60747_scheduler.groupmembers
                                        SET
                                            IsAdmin = @IsAdmin
                                        WHERE
                                            MemberId = @UserId
                                            AND
                                            GroupId = @GroupId;
                                    ";
            try
            {
                return this.ExecuteQuery(queryText, new
                {
                    updatedMember.IsAdmin,
                    updatedMember.UserId,
                    GroupId = groupId
                }) > 0;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }
    }
}