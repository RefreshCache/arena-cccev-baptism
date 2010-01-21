/**********************************************************************
* Description:  Defines contract for data repository
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: IBaptizerRepository.cs $
* $Revision: 3 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IBaptizerRepository.cs   3   2009-12-23 13:52:24-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IBaptizerRepository.cs $
*  
*  Revision: 3   Date: 2009-12-23 20:52:24Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using Arena.Custom.Cccev.BaptismScheduler.Entities;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public interface IBaptizerRepository
    {
        Baptizer GetBaptizer(int id);
        bool Exists(int id);
        void Delete(Baptizer baptizer);
        void Save();
    }
}
