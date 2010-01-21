/**********************************************************************
* Description:  Unit Tests to define domain functionality
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: BaptizerTests.cs $
* $Revision: 3 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/BaptizerTests.cs   3   2010-01-04 16:07:10-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/BaptizerTests.cs $
*  
*  Revision: 3   Date: 2010-01-04 23:07:10Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-29 18:28:20Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;
using Arena.Custom.Cccev.DataUtils;
using NUnit.Framework;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests
{
    [TestFixture]
    public class BaptizerTests
    {
        [Test]
        public void Validation_Failure()
        {
            Baptizer baptizer = new Baptizer
            {
                ScheduleItemID = Constants.NULL_INT,
                Person = null
            };

            try
            {
                bool result = baptizer.IsValid;
                Assert.Fail(string.Format("Validation should have failed, not returned '{0}'", result));
            }
            catch (ValidationException ex)
            {
                Assert.AreEqual(ex.Errors.Count, 2);
                Assert.IsTrue(ex.Errors[0].Contains("Schedule Item"));
                Assert.IsTrue(ex.Errors[1].Contains("Person"));
            }
        }

        [Test]
        public void Validation_Success()
        {
            Baptizer baptizer = TestFactories.GetBaptizer();
            bool result = baptizer.IsValid;
            Assert.IsTrue(result);
            Assert.AreEqual(baptizer.Errors.Count, 0);
        }
    }
}
