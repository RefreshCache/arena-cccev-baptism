/**********************************************************************
* Description:  Fake data repository to simulate ORM
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: FakeScheduleRepository.cs $
* $Revision: 7 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeScheduleRepository.cs   7   2009-12-22 14:28:03-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeScheduleRepository.cs $
*  
*  Revision: 7   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 6   Date: 2009-12-22 20:56:23Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-22 20:55:34Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-22 20:55:14Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 20:53:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes
{
    public class FakeScheduleRepository : IScheduleRepository
    {
        private static int count;
        private static readonly List<Schedule> schedules = new List<Schedule>();

        public FakeScheduleRepository()
        {
            if (schedules.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    Schedule schedule = TestFactories.GetSchedule();
                    schedule.ScheduleID = i + 1;
                    schedules.Add(schedule);
                    count++;
                }
            }
        }

        // ReSharper disable UnusedParameter.Local
        public FakeScheduleRepository(DataContext dataContext) : this() { }
        // ReSharper restore UnusedParameter.Local

        public IEnumerable<Schedule> GetAllSchedules()
        {
            return schedules;
        }

        public Schedule GetSchedule(int id)
        {
            return schedules.SingleOrDefault(s => s.ScheduleID == id);
        }

        public void Add(Schedule schedule)
        {
            count++;
            schedule.ScheduleID = count;
            schedules.Add(schedule);
        }

        public void Delete(Schedule schedule)
        {
            schedules.Remove(schedule);
        }

        public void Save()
        {
            var itemRepository = new FakeScheduleItemRepository();
            var dateRepository = new FakeBlackoutDateRepository();
            SyncAddedItems(itemRepository);
            SyncDeletedItems(itemRepository);
            SyncAddedBlackout(dateRepository);
            SyncDeletedBlackout(dateRepository);
        }

        public bool Exists(int id)
        {
            return schedules.Any(s => s.ScheduleID == id);
        }

        private static void SyncAddedItems(FakeScheduleItemRepository itemRepository)
        {
            foreach (var schedule in schedules)
            {
                foreach (var item in schedule.ScheduleItems)
                {
                    if (item.ScheduleItemID == 0 && !itemRepository.Exists(item.ScheduleItemID))
                    {
                        itemRepository.AddScheduleItem(item);
                    }
                }
            }
        }

        private static void SyncDeletedItems(FakeScheduleItemRepository itemRepository)
        {
            List<ScheduleItem> orphanedItems = new List<ScheduleItem>();

            foreach (var item in itemRepository.Items)
            {
                bool exists = false;

                foreach (var schedule in schedules)
                {
                    if (schedule.ScheduleItems.Contains(item))
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    orphanedItems.Add(item);
                }
            }

            foreach (var item in orphanedItems)
            {
                itemRepository.Delete(item);
            }
        }

        private static void SyncAddedBlackout(FakeBlackoutDateRepository dateRepository)
        {
            foreach (var schedule in schedules)
            {
                foreach (var date in schedule.BlackoutDates)
                {
                    if (date.BlackoutDateID == 0 && !dateRepository.Exists(date.BlackoutDateID))
                    {
                        dateRepository.AddBlackoutDate(date);
                    }
                }
            }
        }

        private static void SyncDeletedBlackout(FakeBlackoutDateRepository dateRepository)
        {
            List<BlackoutDate> orphanedItems = new List<BlackoutDate>();

            foreach (var date in dateRepository.Dates)
            {
                bool exists = false;

                foreach (var schedule in schedules)
                {
                    if (schedule.BlackoutDates.Contains(date))
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    orphanedItems.Add(date);
                }
            }

            foreach (var date in orphanedItems)
            {
                dateRepository.Delete(date);
            }
        }
    }
}
