/**********************************************************************
* Description:  Ecapsulates data access logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: ArenaBlackoutDateRepository.cs $
* $Revision: 2 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaBlackoutDateRepository.cs   2   2009-12-22 11:07:09-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/ArenaBlackoutDateRepository.cs $
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
    public class ArenaBlackoutDateRepository : IBlackoutDateRepository
    {
        private readonly ArenaDataContext db;

        public ArenaBlackoutDateRepository() : this(new ArenaDataContext(ArenaDataContext.CONNECTION_STRING)) { }

        public ArenaBlackoutDateRepository(DataContext dataContext)
        {
            db = dataContext as ArenaDataContext;
        }

        public IEnumerable<BlackoutDate> GetAllBlackoutDates()
        {
            return (from b in db.GetTable<BlackoutDate>()
                    select b).ToList();
        }

        public IEnumerable<BlackoutDate> GetUpcomingBlackoutDates()
        {
            return (from b in db.GetTable<BlackoutDate>()
                    where b.Date >= DateTime.Now
                    select b).ToList();
        }

        public IEnumerable<BlackoutDate> GetBlackoutDatesByDate(DateTime date)
        {
            return (from b in db.GetTable<BlackoutDate>()
                    where b.Date.Date == date.Date
                    select b).ToList();
        }

        public BlackoutDate GetBlackoutDate(int id)
        {
            return db.GetTable<BlackoutDate>().SingleOrDefault(b => b.BlackoutDateID == id);
        }

        public void Delete(BlackoutDate blackoutDate)
        {
            db.GetTable<BlackoutDate>().DeleteOnSubmit(blackoutDate);
        }

        public void Save()
        {
            db.SubmitChanges();
        }

        public bool Exists(int id)
        {
            return db.GetTable<BlackoutDate>().Any(b => b.BlackoutDateID == id);
        }
    }
}
