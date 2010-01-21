/**********************************************************************
* Description:  Abstracts comparison of Baptizer objects to avoid duplication
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/17/2009
*
* $Workfile: BaptizerComparer.cs $
* $Revision: 1 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Util/BaptizerComparer.cs   1   2009-12-17 14:46:35-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Util/BaptizerComparer.cs $
*  
*  Revision: 1   Date: 2009-12-17 21:46:35Z   User: JasonO 
**********************************************************************/

using System.Collections.Generic;
using Arena.Custom.Cccev.BaptismScheduler.Entities;

namespace Arena.Custom.Cccev.BaptismScheduler.Util
{
    public class BaptizerComparer : IEqualityComparer<Baptizer>
    {
        public bool Equals(Baptizer x, Baptizer y)
        {
            if ((x.ScheduleItemID == y.ScheduleItemID) &&
                (x.Person.PersonID == y.Person.PersonID))
            {
                return true;
            }

            return false;
        }

        public int GetHashCode(Baptizer obj)
        {
            return obj.GetHashCode();
        }
    }
}
