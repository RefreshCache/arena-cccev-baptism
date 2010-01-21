/**********************************************************************
* Description:  Ecapsulates data access logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: ArenaScheduleItemRepository.cs $
* $Revision: 2 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaScheduleItemRepository.cs   2   2009-12-22 11:07:09-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaScheduleItemRepository.cs $
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.FrameworkUtils.Data;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public class ArenaScheduleItemRepository : IScheduleItemRepository
    {
        private readonly ArenaDataContext db;

        public ArenaScheduleItemRepository() : this(new ArenaDataContext(ArenaDataContext.CONNECTION_STRING)) { }

        public ArenaScheduleItemRepository(DataContext dataContext)
        {
            db = dataContext as ArenaDataContext;
        }

        public IEnumerable<ScheduleItem> GetAllScheduleItems()
        {
            return (from i in db.GetTable<ScheduleItem>()
                    select i).ToList();
        }

        public IEnumerable<ScheduleItem> GetScheduleItemsByDate(DateTime date)
        {
            return (from i in db.GetTable<ScheduleItem>()
                    where i.ScheduleItemDate.Date == date.Date
                    select i).ToList();
        }

        public ScheduleItem GetScheduleItem(int id)
        {
            return db.GetTable<ScheduleItem>().SingleOrDefault(i => i.ScheduleItemID == id);
        }

        public void Delete(ScheduleItem scheduleItem)
        {
            db.GetTable<ScheduleItem>().DeleteOnSubmit(scheduleItem);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public bool Exists(int id)
        {
            return db.GetTable<ScheduleItem>().Any(i => i.ScheduleItemID == id);
        }
    }
}
