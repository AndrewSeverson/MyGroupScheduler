using System.Collections.Generic;
using System.Linq;
using GifinIt.Infrastucture.Database;
using GroupScheduler.Infrastructure.Data.Stores;
using MySql.Data.MySqlClient;

namespace GroupScheduler.Infrastructure.Database.Classes
{
    public class SchedulerDb : DBConnect
    {
        public bool InsertGroupEvent(Event ev)
        {
            const string queryText = @"  
                                        INSERT INTO
                                            mysql_60747_scheduler.event
                                        (
                                            EventName,
                                            EventDateTime,
                                            EventEndDateTime,
                                            EventDescription,
                                            RepeatTypeId
                                        )
                                        VALUES
                                        (
                                            @EventName,
                                            @EventDateTime,
                                            @EventEndDateTime,
                                            @EventDescription,
                                            @RepeatTypeId
                                        );
                                    ";
            try
            {
                return this.ExecuteQuery(queryText, new
                    {
                        ev.EventName,
                        ev.EventDateTime,
                        ev.EventEndDateTime,
                        ev.EventDescription,
                        ev.EventRepeatType.RepeatTypeId 
                    }) > 0;
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public List<EventRepeatType> GetAllEventRepeatType()
        {
            const string queryText = @"  SELECT
                                            RepeatTypeId,
                                            RepeatTypeName
                                        FROM
                                            mysql_60747_scheduler.eventrepeattype;
                                    ";
            try
            {
                return this.RunQuery<EventRepeatType>(queryText).ToList();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }

        public List<Event> GetAllGroupEvents(int groupId)
        {
            const string queryText = @" SELECT 
	                                        e.EventId,
	                                        e.EventName,
	                                        e.EventDateTime,
	                                        e.EventEndDateTime,
	                                        e.EventDescription,
	                                        r.RepeatTypeId,
	                                        r.RepeatTypeName
                                        FROM 
	                                        mysql_60747_scheduler.event e INNER JOIN
	                                        mysql_60747_scheduler.eventrepeattype r on e.RepeatTypeId = r.RepeatTypeId
                                        WHERE
	                                        e.GroupId = @GroupId
                                    ";
            try
            {
                IEnumerable<dynamic> list = this.RunQuery<dynamic>(queryText, new{
                    GroupId = groupId
                });
                return list.Select(e => new Event
                    {
                        EventId = e.EventId,
                        EventName = e.EventName,
                        EventDateTime = e.EventDateTime,
                        EventEndDateTime = e.EventEndDateTime,
                        EventDescription = e.EventDescription,
                        EventRepeatType = new EventRepeatType{
                            RepeatTypeId = e.RepeatTypeId,
                            RepeatTypeName = e.RepeatTypeName
                        }
                    }).ToList();
            }
            catch (MySqlException e)
            {
                //TODO : log here
                throw;
            }
        }
        }
    }

