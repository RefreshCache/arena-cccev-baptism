/**********************************************************************
* Description:  Fake data repository to simulate ORM
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: FakeBlackoutDateRepository.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeBlackoutDateRepository.cs   5   2009-12-22 14:28:03-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeBlackoutDateRepository.cs $
*  
*  Revision: 5   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 4   Date: 2009-12-22 20:55:14Z   User: JasonO 
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
    public class FakeBlackoutDateRepository : IBlackoutDateRepository
    {
        private static int count;
        private static readonly List<BlackoutDate> blackoutDates = new List<BlackoutDate>();

        public List<BlackoutDate> Dates { get { return blackoutDates; } }

        public FakeBlackoutDateRepository()
        {
            if (blackoutDates.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    BlackoutDate blackoutDate = TestFactories.GetBlackoutDate();
                    blackoutDate.BlackoutDateID = i + 1;
                    blackoutDates.Add(blackoutDate);
                    count++;
                }
            }
        }

        // ReSharper disable UnusedParameter.Local
        public FakeBlackoutDateRepository(DataContext dataContext) : this() { }
        // ReSharper restore UnusedParameter.Local

        public IEnumerable<BlackoutDate> GetAllBlackoutDates()
        {
            return blackoutDates;
        }

        public IEnumerable<BlackoutDate> GetUpcomingBlackoutDates()
        {
            return blackoutDates.Where(d => d.Date >= DateTime.Now);
        }

        public IEnumerable<BlackoutDate> GetBlackoutDatesByDate(DateTime date)
        {
            return blackoutDates.Where(d => d.Date.Date == date.Date);
        }

        public BlackoutDate GetBlackoutDate(int id)
        {
            return blackoutDates.SingleOrDefault(d => d.BlackoutDateID == id);
        }

        public void AddBlackoutDate(BlackoutDate date)
        {
            count++;
            date.BlackoutDateID = count;
            blackoutDates.Add(date);
        }

        public void Delete(BlackoutDate date)
        {
            blackoutDates.Remove(date);
        }

        public void Save()
        {
        }

        public bool Exists(int id)
        {
            return blackoutDates.Any(d => d.BlackoutDateID == id);
        }
    }
}
