/**********************************************************************
* Description:  Defines application logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/10/2009
*
* $Workfile: ScheduleController.cs $
* $Revision: 12 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Application/ScheduleController.cs   12   2010-01-11 14:40:25-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Application/ScheduleController.cs $
*  
*  Revision: 12   Date: 2010-01-11 21:40:25Z   User: JasonO 
*  
*  Revision: 11   Date: 2010-01-04 22:24:35Z   User: JasonO 
*  
*  Revision: 10   Date: 2010-01-04 22:18:18Z   User: JasonO 
*  
*  Revision: 9   Date: 2009-12-30 17:35:42Z   User: JasonO 
*  
*  Revision: 8   Date: 2009-12-29 17:50:35Z   User: JasonO 
*  
*  Revision: 7   Date: 2009-12-28 15:14:52Z   User: JasonO 
*  Improving validation logic 
*  
*  Revision: 6   Date: 2009-12-23 20:52:24Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Application;
using Arena.Custom.Cccev.FrameworkUtils.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Application
{
    public class ScheduleController : CentralControllerBase
    {
        private const string BLL_SESSION_PREFIX = "Cccev.Bsch.Schedule";

        public void CreateSchedule(Lookup campus, string name, string description, string userID)
        {
            Schedule schedule = new Schedule
            {
                Campus = campus,
                Name = name,
                Description = description
            };

            ScheduleBll bll = new ScheduleBll();
            bll.Entity = schedule;
            bll.SaveSchedule(userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public void UpdateSchedule(int scheduleID, Lookup campus, string name, string description, string userID)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, scheduleID);
            Schedule schedule = bll.Entity;
            schedule.Campus = campus;
            schedule.Name = name;
            schedule.Description = description;
            bll.SaveSchedule(userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, scheduleID, bll);
        }

        public Schedule GetSchedule(int id)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, id);
            return bll.Entity;
        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var scheduleRepository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            return scheduleRepository.GetAllSchedules();
        }

        public void DeleteSchedule(Schedule schedule)
        {
            int id = schedule.ScheduleID;
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, id);
            bll.RemoveSchedule();
            RemoveObjectFromCache(BLL_SESSION_PREFIX, id);
        }

        public void CreateScheduleItem(Schedule schedule, DateTime date, Person person, List<Baptizer> baptizers, 
            Person approvedBy, bool isConfirmed, string userID)
        {
            ScheduleItem scheduleItem = new ScheduleItem
            {
                ScheduleItemDate = date,
                ScheduleID = schedule.ScheduleID,
                Person = person,
                ApprovedBy = approvedBy,
                IsConfirmed = isConfirmed
            };

            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.CreateScheduleItem(scheduleItem, baptizers, userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public IEnumerable<ScheduleItem> GetScheduleItemsByDateRange(Schedule schedule, DateRange dateRange)
        {
            return (from i in schedule.ScheduleItems
                    where i.ScheduleItemDate.Date >= dateRange.Start.Date &&
                          i.ScheduleItemDate.Date <= dateRange.End.Date
                    orderby i.ScheduleItemDate ascending
                    select i).ToList();
        }

        public void UpdateScheduleItem(Schedule schedule, int scheduleItemID, DateTime date, Person person,
            List<Baptizer> baptizers, Person approvedBy, bool isConfirmed, string userID)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.UpdateScheduleItem(scheduleItemID, date, person, baptizers, approvedBy, isConfirmed, userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public void DeleteScheduleItem(Schedule schedule, int scheduleItemID)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.RemoveScheduleItem(scheduleItemID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public void CreateBlackoutDate(Schedule schedule, string description, DateTime date, string userID)
        {
            BlackoutDate blackoutDate = new BlackoutDate
            {
                ScheduleID = schedule.ScheduleID,
                Description = description,
                Date = date
            };

            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.CreateBlackoutDate(blackoutDate, userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public void UpdateBlackoutDate(Schedule schedule, int blackoutDateID, string description, DateTime date, string userID)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.UpdateBlackoutDate(blackoutDateID, description, date, userID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }

        public void DeleteBlackoutDate(Schedule schedule, int blackoutDateID)
        {
            var bll = GetCachedObject<ScheduleBll>(BLL_SESSION_PREFIX, schedule.ScheduleID);
            bll.RemoveBlackoutDate(blackoutDateID);
            SaveObjectToCache(BLL_SESSION_PREFIX, schedule.ScheduleID, bll);
        }
    }
}
