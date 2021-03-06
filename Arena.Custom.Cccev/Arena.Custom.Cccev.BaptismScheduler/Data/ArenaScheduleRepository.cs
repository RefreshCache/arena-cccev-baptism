﻿/**********************************************************************
* Description:  Ecapsulates data access logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: ArenaScheduleRepository.cs $
* $Revision: 3 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaScheduleRepository.cs   3   2009-12-29 10:50:35-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaScheduleRepository.cs $
*  
*  Revision: 3   Date: 2009-12-29 17:50:35Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.FrameworkUtils.Data;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public class ArenaScheduleRepository : IScheduleRepository
    {
        private readonly ArenaDataContext db;

        public ArenaScheduleRepository() : this(new ArenaDataContext(ArenaDataContext.CONNECTION_STRING)) { }

        public ArenaScheduleRepository(DataContext dataContext)
        {
            db = dataContext as ArenaDataContext;
        }

        public IEnumerable<Schedule> GetAllSchedules()
        {
            return (from s in db.GetTable<Schedule>()
                    select s).ToList();
        }

        public Schedule GetSchedule(int id)
        {
            return db.GetTable<Schedule>().SingleOrDefault(s => s.ScheduleID == id);
        }

        public void Add(Schedule schedule)
        {
            db.GetTable<Schedule>().InsertOnSubmit(schedule);
        }

        public void Delete(Schedule schedule)
        {
            db.GetTable<Schedule>().DeleteOnSubmit(schedule);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public bool Exists(int id)
        {
            return db.GetTable<Schedule>().Any(s => s.ScheduleID == id);
        }
    }
}
