/**********************************************************************
* Description:  Unit Tests to define application functionality
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/14/2009
*
* $Workfile: ScheduleControllerTests.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/ScheduleControllerTests.cs   5   2010-01-04 16:07:10-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/ScheduleControllerTests.cs $
*  
*  Revision: 5   Date: 2010-01-04 23:07:10Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-29 18:28:20Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-22 20:56:23Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-17 21:45:27Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Application;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Util;
using NUnit.Framework;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests
{
    [TestFixture]
    public class ScheduleControllerTests
    {
        public Schedule CreateSchedule()
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            ScheduleController controller = new ScheduleController();
            controller.CreateSchedule(TestFactories.GetDefaultLookup(), "Test Schedule", "This is a test", "test user");
            return repository.GetAllSchedules().Last();
        }

        [Test]
        public void Schedule_Create()
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            ScheduleController controller = new ScheduleController();
            controller.CreateSchedule(TestFactories.GetDefaultLookup(), "Test Schedule", "This is a test", "test user");
            var schedules = repository.GetAllSchedules();
            Assert.Greater(schedules.Count(), 3);

            Schedule schedule = repository.GetAllSchedules().Last();
            Assert.Greater(schedule.ScheduleID, 0);
            Assert.AreEqual(schedule.CreatedBy, "test user");
            Assert.AreEqual(schedule.ModifiedBy, "test user");
        }

        [Test]
        public void Schedule_Read()
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            Schedule schedule = CreateSchedule();
            int scheduleID = schedule.ScheduleID;
            var newSchedule = repository.GetSchedule(scheduleID);
            Assert.AreSame(schedule, newSchedule);
        }

        [Test]
        public void Schedule_Update()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            schedule.Name = "new name";
            controller.UpdateSchedule(schedule.ScheduleID, TestFactories.GetDefaultLookup(), schedule.Name, schedule.Description, "new user");
            Assert.AreNotEqual(schedule.Name, "Test Schedule");
            Assert.AreNotEqual(schedule.CreatedBy, schedule.ModifiedBy);
            Assert.AreEqual(schedule.ModifiedBy, "new user");
        }

        [Test]
        public void Schedule_Delete()
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            int scheduleID = schedule.ScheduleID;
            controller.DeleteSchedule(schedule);
            Assert.IsFalse(repository.Exists(scheduleID));
        }

        [Test]
        public void Schedule_Item_Create()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateScheduleItem(schedule, DateTime.Now, TestFactories.GetDefaultPerson(), new List<Baptizer>(), 
                TestFactories.GetDefaultPerson(), true, "test user");

            Assert.Greater(schedule.ScheduleItems.Count, 0);
            Assert.AreEqual(schedule.ScheduleItems.First().CreatedBy, "test user");
            Assert.Greater(schedule.ScheduleItems.First().ScheduleItemID, 0);
        }

        [Test]
        public void Schedule_Item_Read()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateScheduleItem(schedule, DateTime.Now, TestFactories.GetDefaultPerson(), new List<Baptizer>(),
                TestFactories.GetDefaultPerson(), true, "test user");

            ScheduleItem item = schedule.ScheduleItems.First();
            string key = KeyHelper.GetKey<IScheduleItemRepository>();
            ScheduleItem newItem = RepositoryFactory.GetRepository<IScheduleItemRepository>(key).GetScheduleItem(item.ScheduleItemID);
            Assert.AreSame(item, newItem);
        }

        [Test]
        public void Schedule_Item_Update()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateScheduleItem(schedule, DateTime.Now, TestFactories.GetDefaultPerson(), new List<Baptizer>(),
                TestFactories.GetDefaultPerson(), true, "test user");

            ScheduleItem item = schedule.ScheduleItems.First();
            item.ScheduleItemDate = DateTime.Now.AddDays(1);
            controller.UpdateScheduleItem(schedule, item.ScheduleItemID, item.ScheduleItemDate, item.Person, 
                item.Baptizers.ToList(), item.ApprovedBy, true, "other user");

            Assert.AreNotEqual(item.ScheduleItemDate, DateTime.Now);
            Assert.AreNotEqual(item.CreatedBy, item.ModifiedBy);
            Assert.AreEqual(item.ModifiedBy, "other user");
        }

        [Test]
        public void Schedule_Item_Delete()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateScheduleItem(schedule, DateTime.Now, TestFactories.GetDefaultPerson(), new List<Baptizer>(),
                TestFactories.GetDefaultPerson(), true, "test user");

            ScheduleItem item = schedule.ScheduleItems.First();
            int itemID = item.ScheduleItemID;
            controller.DeleteScheduleItem(schedule, item.ScheduleItemID);
            string key = KeyHelper.GetKey<IScheduleItemRepository>();
            Assert.IsFalse(RepositoryFactory.GetRepository<IScheduleItemRepository>(key).Exists(itemID));
        }

        [Test]
        public void Schedule_Item_Baptizer_Sync()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();

            Baptizer b1 = new Baptizer
            {
                Person = new Person { PersonID = 3, FirstName = "Jimmy", LastName = "Jams" }
            };

            Baptizer b2 = new Baptizer
            {
                Person = new Person { PersonID = 4, FirstName = "James", LastName = "Jams" }
            };

            List<Baptizer> baptizers = new List<Baptizer>
            {
                TestFactories.GetBaptizer(),
                b1
            };

            controller.CreateScheduleItem(schedule, DateTime.Now, TestFactories.GetDefaultPerson(), baptizers,
                TestFactories.GetDefaultPerson(), true, "test user");
            ScheduleItem item = schedule.ScheduleItems.First();
            baptizers = new List<Baptizer>
            {
                TestFactories.GetBaptizer(),
                b2
            };

            controller.UpdateScheduleItem(schedule, item.ScheduleItemID, DateTime.Now, TestFactories.GetDefaultPerson(), baptizers,
                TestFactories.GetDefaultPerson(), true, "test user");

            Assert.AreEqual(item.Baptizers.Count, 2);
            Assert.IsTrue(item.Baptizers.Any(b => b.Person.PersonID == 2));
            Assert.IsTrue(item.Baptizers.Any(b => b.Person.PersonID == 4));
            Assert.IsFalse(item.Baptizers.Any(b => b.Person.PersonID == 3));
        }

        [Test]
        public void Blackout_Date_Create()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateBlackoutDate(schedule, "Test blackout date", DateTime.Now, "test user");

            Assert.Greater(schedule.BlackoutDates.Count, 0);
            Assert.AreEqual(schedule.BlackoutDates.First().CreatedBy, "test user");
            Assert.Greater(schedule.BlackoutDates.First().BlackoutDateID, 0);
        }

        [Test]
        public void Blackout_Date_Read()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateBlackoutDate(schedule, "Test blackout date", DateTime.Now, "test user");

            BlackoutDate blackoutDate = schedule.BlackoutDates.First();
            string key = KeyHelper.GetKey<IBlackoutDateRepository>();
            BlackoutDate newBlackoutDate = RepositoryFactory.GetRepository<IBlackoutDateRepository>(key).GetBlackoutDate(blackoutDate.BlackoutDateID);
            Assert.AreSame(blackoutDate, newBlackoutDate);
        }

        [Test]
        public void Blackout_Date_Update()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateBlackoutDate(schedule, "Test blackout date", DateTime.Now, "test user");

            BlackoutDate blackoutDate = schedule.BlackoutDates.First();
            blackoutDate.Date = DateTime.Now.AddDays(1);
            controller.UpdateBlackoutDate(schedule, blackoutDate.BlackoutDateID, blackoutDate.Description, 
                blackoutDate.Date, "other user");

            Assert.AreNotEqual(blackoutDate.Date, DateTime.Now);
            Assert.AreNotEqual(blackoutDate.CreatedBy, blackoutDate.ModifiedBy);
            Assert.AreEqual(blackoutDate.ModifiedBy, "other user");
        }

        [Test]
        public void Blackout_Date_Delete()
        {
            ScheduleController controller = new ScheduleController();
            Schedule schedule = CreateSchedule();
            controller.CreateBlackoutDate(schedule, "Test blackout date", DateTime.Now, "test user");

            BlackoutDate blackoutDate = schedule.BlackoutDates.First();
            int blackoutDateID = blackoutDate.BlackoutDateID;
            controller.DeleteBlackoutDate(schedule, blackoutDateID);
            string key = KeyHelper.GetKey<IBlackoutDateRepository>();
            Assert.IsFalse(RepositoryFactory.GetRepository<IBlackoutDateRepository>(key).Exists(blackoutDateID));
        }

        [Test]
        public void Adding_ScheduleItem_On_Blacked_Out_Date_Should_Fail()
        {
            DateTime blackedOutDate = DateTime.Now.AddDays(3);
            string key = KeyHelper.GetKey<IScheduleRepository>();
            var repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            ScheduleController controller = new ScheduleController();
            controller.CreateSchedule(TestFactories.GetDefaultLookup(), "Test Schedule", "This is a test", "test user");

            Schedule schedule = repository.GetAllSchedules().Last();
            controller.CreateBlackoutDate(schedule, "Test blackout date", blackedOutDate, "test user");

            try
            {
                controller.CreateScheduleItem(schedule, blackedOutDate, TestFactories.GetDefaultPerson(), new List<Baptizer>(),
                    TestFactories.GetDefaultPerson(), true, "test user");
                Assert.Fail("controller.CreateScheduleItem() should throw an exception.");
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Errors[0].Contains("Unable to schedule a baptism on a Black Out Date."));
                Assert.AreEqual(ex.Errors.Count, 1);
            }
        }
    }
}
