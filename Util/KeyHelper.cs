/**********************************************************************
* Description:  Abstracts retrieval of data access repository from application
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: KeyHelper.cs $
* $Revision: 1 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Util/KeyHelper.cs   1   2010-01-04 15:13:15-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Util/KeyHelper.cs $
*  
*  Revision: 1   Date: 2010-01-04 22:13:15Z   User: JasonO 
*  Abstracting instantiation logic to framework utils. 
*  
*  Revision: 4   Date: 2009-12-30 17:35:42Z   User: JasonO 
*  
**********************************************************************/

using System;

namespace Arena.Custom.Cccev.BaptismScheduler.Util
{
    /// <summary>
    /// Class to manage configuration keys for domain-level objects.
    /// </summary>
    public class KeyHelper
    {
        private const string SCHEDULE_ITEM_KEY = "Cccev.Bsch.ScheduleItemRepository";
        private const string SCHEDULE_KEY = "Cccev.Bsch.ScheduleRepository";
        private const string BLACKOUT_DATE_KEY = "Cccev.Bsch.BlackoutDateRepository";
        private const string BAPTIZER_KEY = "Cccev.Bsch.BaptizerRepository";

        /// <summary>
        /// Returns a key given the type of object to instantiate.
        /// </summary>
        /// <typeparam name="T">Object type of the requested key</typeparam>
        /// <returns>Configuration key of corresponding object type</returns>
        public static string GetKey<T>()
        {
            Type type = typeof(T);
            string typeName = type.FullName;

            switch (typeName.Substring(typeName.LastIndexOf(".") + 1))
            {
                case "IBaptizerRepository":
                    return BAPTIZER_KEY;
                case "IBlackoutDateRepository":
                    return BLACKOUT_DATE_KEY;
                case "IScheduleItemRepository":
                    return SCHEDULE_ITEM_KEY;
                case "IScheduleRepository":
                    return SCHEDULE_KEY;
                default:
                    return null;
            }
        }
    }
}
