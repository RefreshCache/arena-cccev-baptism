/**********************************************************************
* Description:  Unit Tests to define domain functionality
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: BlackoutDateTests.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/BlackoutDateTests.cs   5   2010-01-04 16:07:10-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/BlackoutDateTests.cs $
*  
*  Revision: 5   Date: 2010-01-04 23:07:10Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-29 18:28:20Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 2   Date: 2009-12-22 20:56:23Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using System.Linq;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Util;
using NUnit.Framework;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests
{
    [TestFixture]
    public class BlackoutDateTests
    {
        [Test]
        public void Validation_Failure()
        {
            BlackoutDate blackoutDate = new BlackoutDate
            {
                Date = Constants.NULL_DATE,
                Description = Constants.NULL_STRING,
                ScheduleID = 0
            };

            try
            {
                bool result = blackoutDate.IsValid;
                Assert.Fail(string.Format("Validation should have failed, not returned '{0}'", result));
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual(ex.Errors.Count, 3);
                Assert.IsTrue(ex.Errors[0].Contains("Description"));
                Assert.IsTrue(ex.Errors[1].Contains("Date"));
                Assert.IsTrue(ex.Errors[2].Contains("Schedule"));
            }
        }

        [Test]
        public void Validation_Success()
        {
            BlackoutDate blackoutDate = TestFactories.GetBlackoutDate();
            bool result = blackoutDate.IsValid;
            Assert.IsTrue(result);
            Assert.AreEqual(blackoutDate.Errors.Count, 0);
        }

        [Test]
        public void Load_BlackoutDate_List_From_RepositoryFactory()
        {
            string key = KeyHelper.GetKey<IBlackoutDateRepository>();
            var repository = RepositoryFactory.GetRepository<IBlackoutDateRepository>(key);
            Assert.IsInstanceOf(typeof(FakeBlackoutDateRepository), repository);

            var blackoutDates = repository.GetAllBlackoutDates();
            Assert.GreaterOrEqual(blackoutDates.Count(), 3);
        }
    }
}
