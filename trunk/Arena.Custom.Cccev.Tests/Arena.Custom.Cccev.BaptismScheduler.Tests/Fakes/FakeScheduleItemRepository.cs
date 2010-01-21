/**********************************************************************
* Description:  Fake data repository to simulate ORM
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: FakeScheduleItemRepository.cs $
* $Revision: 6 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeScheduleItemRepository.cs   6   2009-12-22 14:28:03-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeScheduleItemRepository.cs $
*  
*  Revision: 6   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 5   Date: 2009-12-22 20:55:34Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-22 20:55:14Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 20:53:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes
{
    class FakeScheduleItemRepository : IScheduleItemRepository
    {
        private static int count;
        private static readonly List<ScheduleItem> items = new List<ScheduleItem>();

        public List<ScheduleItem> Items { get { return items; } }

        public FakeScheduleItemRepository()
        {
            if (items.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    ScheduleItem item = TestFactories.GetScheduleItem();
                    item.ScheduleItemID = i + 1;
                    items.Add(item);
                    count++;
                }
            }
        }

        // ReSharper disable UnusedParameter.Local
        public FakeScheduleItemRepository(DataContext dataContext) : this() { }
        // ReSharper restore UnusedParameter.Local

        public IEnumerable<ScheduleItem> GetAllScheduleItems()
        {
            return items;
        }

        public IEnumerable<ScheduleItem> GetScheduleItemsByDate(DateTime time)
        {
            return items.Where(i => i.ScheduleItemDate.Date == time.Date);
        }

        public void AddScheduleItem(ScheduleItem item)
        {
            count++;
            item.ScheduleItemID = count;
            items.Add(item);
        }

        public void Delete(ScheduleItem item)
        {
            items.Remove(item);
        }

        public ScheduleItem GetScheduleItem(int id)
        {
            return items.SingleOrDefault(i => i.ScheduleItemID == id);
        }

        public void Save()
        {
            var baptizerRepository = new FakeBaptizerRepository();
            SyncAddedBaptizers(baptizerRepository);
            SyncDeletedBaptizers(baptizerRepository);
        }

        public bool Exists(int id)
        {
            return items.Any(i => i.ScheduleItemID == id);
        }

        private static void SyncAddedBaptizers(FakeBaptizerRepository baptizerRepository)
        {
            foreach (var item in items)
            {
                foreach (var baptizer in item.Baptizers)
                {
                    if (baptizer.BaptizerID == 0 && !baptizerRepository.Exists(baptizer.BaptizerID))
                    {
                        baptizerRepository.AddBaptizer(baptizer);
                    }
                }
            }
        }

        private static void SyncDeletedBaptizers(FakeBaptizerRepository baptizerRepository)
        {
            List<Baptizer> orphanedBaptizers = new List<Baptizer>();

            foreach (var baptizer in baptizerRepository.Baptizers)
            {
                bool exists = false;

                foreach (var item in items)
                {
                    if (item.Baptizers.Contains(baptizer))
                    {
                        exists = true;
                    }
                }

                if (!exists)
                {
                    orphanedBaptizers.Add(baptizer);
                }
            }

            foreach(var baptizer in orphanedBaptizers)
            {
                baptizerRepository.Delete(baptizer);
            }
        }
    }
}
