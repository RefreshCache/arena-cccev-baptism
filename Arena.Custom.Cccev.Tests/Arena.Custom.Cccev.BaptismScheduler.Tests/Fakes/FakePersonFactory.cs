/**********************************************************************
* Description:  Fake person factory to abstract ArenaContext.Current.Person
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: FakePersonFactory.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakePersonFactory.cs   5   2010-01-11 14:40:40-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakePersonFactory.cs $
*  
*  Revision: 5   Date: 2010-01-11 21:40:40Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-22 21:28:03Z   User: JasonO 
*  Updating tests to reflect code changes. 
*  
*  Revision: 3   Date: 2009-12-22 20:55:14Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-15 00:11:18Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:57:19Z   User: JasonO 
**********************************************************************/

using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Tests.Util;
using Arena.Custom.Cccev.FrameworkUtils.Entity;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes
{
    class FakePersonFactory : IPersonFactory
    {
        public Person GetCurrentPerson()
        {
            return TestFactories.GetDefaultPerson();
        }
    }
}
