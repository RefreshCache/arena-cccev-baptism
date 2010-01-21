/**********************************************************************
* Description:  Defines contract for data repository
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: IScheduleItemRepository.cs $
* $Revision: 2 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IScheduleItemRepository.cs   2   2009-12-22 11:07:09-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IScheduleItemRepository.cs $
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using Arena.Custom.Cccev.BaptismScheduler.Entities;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public interface IScheduleItemRepository
    {
        IEnumerable<ScheduleItem> GetAllScheduleItems();
        IEnumerable<ScheduleItem> GetScheduleItemsByDate(DateTime date);
        ScheduleItem GetScheduleItem(int id);
        void Delete(ScheduleItem scheduleItem);
        void Save();
        bool Exists(int id);
    }
}
