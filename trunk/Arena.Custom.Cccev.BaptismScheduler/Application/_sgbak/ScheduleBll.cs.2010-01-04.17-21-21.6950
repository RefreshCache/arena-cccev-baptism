/**********************************************************************
* Description:  Business object to manage entity hierarchy and data context
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/28/2009
*
* $Workfile: ScheduleBll.cs $
* $Revision: 4 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Application/ScheduleBll.cs   4   2010-01-04 15:24:35-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Application/ScheduleBll.cs $
*  
*  Revision: 4   Date: 2010-01-04 22:24:35Z   User: JasonO 
*  
*  Revision: 3   Date: 2010-01-04 22:20:33Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-30 15:35:05Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-29 17:50:32Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Data;
using Arena.Custom.Cccev.FrameworkUtils.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Application
{
    public sealed class ScheduleBll : CentralBusinessObject<Schedule, DataContext>
    {
        private IScheduleRepository scheduleRepository;
        private IBlackoutDateRepository blackoutDateRepository;
        private IScheduleItemRepository scheduleItemRepository;
        private IBaptizerRepository baptizerRepository;

        public override Schedule Entity { get; set; }
        public override DataContext Context { get; set; }

        public ScheduleBll()
        {
            RegisterContext();
            Entity = new Schedule();
        }

        public ScheduleBll(int id)
        {
            RegisterContext();
            Entity = scheduleRepository.GetSchedule(id);
        }

        public ScheduleBll(Schedule schedule)
        {
            RegisterContext();
            RegisterEntity(schedule);
        }

        public void SaveSchedule(string userID)
        {
            Entity.Update(userID);

            if (Entity.IsValid)
            {
                if (Entity.ScheduleID <= Constants.ZERO)
                {
                    scheduleRepository.Add(Entity);
                }

                scheduleRepository.Save();
            }
        }

        public void RemoveSchedule()
        {
            scheduleRepository.Delete(Entity);
            scheduleRepository.Save();
        }

        public void CreateScheduleItem(ScheduleItem scheduleItem, List<Baptizer> baptizers, string userID)
        {
            scheduleItem.Update(userID);

            if (scheduleItem.IsValid)
            {
                Entity.ScheduleItems.Add(scheduleItem);
                scheduleRepository.Save();
                SyncBaptizers(scheduleItem, baptizers);
                scheduleRepository.Save();
            }
        }

        public void UpdateScheduleItem(int id, DateTime date, Person person,
            List<Baptizer> baptizers, Person approvedBy, bool isConfirmed, string userID)
        {
            var scheduleItem = Entity.ScheduleItems.SingleOrDefault(i => i.ScheduleItemID == id);

            if (scheduleItem != null)
            {
                scheduleItem.ScheduleItemDate = date;
                scheduleItem.Person = person;
                scheduleItem.ApprovedBy = approvedBy;
                scheduleItem.IsConfirmed = isConfirmed;
                scheduleItem.Update(userID);
                SyncBaptizers(scheduleItem, baptizers);

                if (scheduleItem.IsValid)
                {
                    scheduleRepository.Save();
                }
            }
        }

        public void RemoveScheduleItem(int id)
        {
            var scheduleItem = Entity.ScheduleItems.SingleOrDefault(i => i.ScheduleItemID == id);

            if (scheduleItem != null)
            {
                Entity.ScheduleItems.Remove(scheduleItem);
                scheduleItemRepository.Delete(scheduleItem);
                scheduleRepository.Save();
            }
        }

        public void CreateBlackoutDate(BlackoutDate blackoutDate, string userID)
        {
            blackoutDate.Update(userID);

            if (blackoutDate.IsValid)
            {
                Entity.BlackoutDates.Add(blackoutDate);
                scheduleRepository.Save();
            }
        }
        
        public void UpdateBlackoutDate(int id, string description, DateTime date, string userID)
        {
            var blackoutDate = Entity.BlackoutDates.SingleOrDefault(b => b.BlackoutDateID == id);

            if (blackoutDate != null)
            {
                blackoutDate.Description = description;
                blackoutDate.Date = date;
                blackoutDate.Update(userID);

                if (blackoutDate.IsValid)
                {
                    scheduleRepository.Save();
                }
            }
        }
        
        public void RemoveBlackoutDate(int id)
        {
            var blackoutDate = Entity.BlackoutDates.SingleOrDefault(b => b.BlackoutDateID == id);

            if (blackoutDate != null)
            {
                Entity.BlackoutDates.Remove(blackoutDate);
                blackoutDateRepository.Delete(blackoutDate);
                scheduleRepository.Save();
            }
        }

        private void SyncBaptizers(ScheduleItem scheduleItem, IEnumerable<Baptizer> baptizers)
        {
            var baptizersToRemove = scheduleItem.SyncBaptizers(baptizers);

            foreach (var baptizer in baptizersToRemove)
            {
                scheduleItem.Baptizers.Remove(baptizer);
                baptizerRepository.Delete(baptizer);
            }
        }

        private void RegisterContext()
        {
            if (Context == null)
            {
                Context = RepositoryHelper.GetDataContext();
            }

            scheduleRepository = RepositoryFactory.GetRepository<IScheduleRepository>(
                KeyHelper.GetKey<IScheduleRepository>(), Context);
            blackoutDateRepository = RepositoryFactory.GetRepository<IBlackoutDateRepository>(
                KeyHelper.GetKey<IBlackoutDateRepository>(), Context);
            scheduleItemRepository = RepositoryFactory.GetRepository<IScheduleItemRepository>(
                KeyHelper.GetKey<IScheduleItemRepository>(), Context);
            baptizerRepository = RepositoryFactory.GetRepository<IBaptizerRepository>(
                KeyHelper.GetKey<IBaptizerRepository>(), Context);
        }

        private void RegisterEntity(Schedule schedule)
        {
            Entity = schedule;

            try
            {
                Context.GetTable<Schedule>().Attach(Entity, true);
                Context.GetTable<ScheduleItem>().AttachAll(Entity.ScheduleItems, true);
                Context.GetTable<BlackoutDate>().AttachAll(Entity.BlackoutDates, true);
                var baptizerTable = Context.GetTable<Baptizer>();

                foreach (var scheduleItem in Entity.ScheduleItems)
                {
                    baptizerTable.AttachAll(scheduleItem.Baptizers, true);
                }
            }
            catch (InvalidOperationException) { }
        }
    }
}