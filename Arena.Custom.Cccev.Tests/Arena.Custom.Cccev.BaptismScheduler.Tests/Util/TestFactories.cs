using System;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Entities;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Util
{
    public static class TestFactories
    {
        public static Person GetDefaultPerson()
        {
            return new Person
                       {
                           PersonID = 2,
                           FirstName = "Jon",
                           LastName = "Doe"
                       };
        }

        public static Lookup GetDefaultLookup()
        {
            return new Lookup
                       {
                           LookupID = 9,
                           LookupTypeID = 3,
                           Value = "Sunday"
                       };
        }

        public static ScheduleItem GetScheduleItem()
        {
            return new ScheduleItem
                       {
                           ScheduleItemDate = DateTime.Now,
                           Person = GetDefaultPerson(),
                           ApprovedBy = GetDefaultPerson(),
                           IsConfirmed = true,
                           ScheduleID = 1
                       };
        }

        public static ScheduleItem GetScheduleItem(int scheduleID)
        {
            return new ScheduleItem
                       {
                           ScheduleItemDate = DateTime.Now,
                           Person = GetDefaultPerson(),
                           ApprovedBy = GetDefaultPerson(),
                           IsConfirmed = true,
                           ScheduleID = scheduleID
                       };
        }

        public static Schedule GetSchedule()
        {
            return new Schedule
                       {
                           Name = "My Schedule",
                           Description = "This is a schedule",
                           Campus = GetDefaultLookup()
                       };
        }

        public static BlackoutDate GetBlackoutDate()
        {
            return new BlackoutDate
                       {
                           Description = "This is a blackout date",
                           Date = DateTime.Now,
                           ScheduleID = 1
                       };
        }

        public static BlackoutDate GetBlackoutDate(int id)
        {
            return new BlackoutDate
                       {
                           Description = "This is a blackout date",
                           Date = DateTime.Now,
                           ScheduleID = id
                       };
        }

        public static Baptizer GetBaptizer()
        {
            return new Baptizer
                       {
                           ScheduleItemID = 1,
                           Person = GetDefaultPerson()
                       };
        }
    }
}
