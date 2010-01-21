/**********************************************************************
* Description:  Defines contract for data repository
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: IScheduleRepository.cs $
* $Revision: 1 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IScheduleRepository.cs   1   2009-12-14 16:56:27-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Data/IScheduleRepository.cs $
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System.Collections.Generic;
using Arena.Custom.Cccev.BaptismScheduler.Entities;

namespace Arena.Custom.Cccev.BaptismScheduler.Data
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetAllSchedules();
        Schedule GetSchedule(int id);
        void Add(Schedule schedule);
        void Delete(Schedule schedule);
        void Save();
        bool Exists(int id);
    }
}
