/**********************************************************************
* Description:  TBD
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: TBD
*
* $Workfile: FakeDataContext.cs $
* $Revision: 1 $
* $Header: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeDataContext.cs   1   2009-12-22 14:27:57-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev.Tests/Arena.Custom.Cccev.BaptismScheduler.Tests/Fakes/FakeDataContext.cs $
*  
*  Revision: 1   Date: 2009-12-22 21:27:57Z   User: JasonO 
*  Updating tests to reflect code changes. 
**********************************************************************/

using System.Data.Linq;
using Arena.Custom.Cccev.DataUtils;

namespace Arena.Custom.Cccev.BaptismScheduler.Tests.Fakes
{
    public class FakeDataContext : DataContext
    {
        public FakeDataContext() : base(Constants.NULL_STRING)
        {
            
        }
    }
}
