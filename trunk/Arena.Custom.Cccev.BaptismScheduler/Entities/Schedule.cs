/**********************************************************************
* Description:  Defines domain logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: Schedule.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/Schedule.cs   5   2010-01-04 15:24:35-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/Schedule.cs $
*  
*  Revision: 5   Date: 2010-01-04 22:24:35Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-29 18:23:50Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-28 15:14:52Z   User: JasonO 
*  Improving validation logic 
*  
*  Revision: 2   Date: 2009-12-22 18:07:09Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Data;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Entity;
using Arena.Custom.Cccev.FrameworkUtils.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Entities
{
    [Table(Name = "cust_cccev_bsch_schedule")]
    public class Schedule : CentralObjectBase
    {
        private Lookup campus;
        private readonly List<string> errors = new List<string>();
        private readonly EntitySet<ScheduleItem> scheduleItems = new EntitySet<ScheduleItem>();
        private readonly EntitySet<BlackoutDate> blackoutDates = new EntitySet<BlackoutDate>();

        [Column(Name = "schedule_id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ScheduleID { get; set; }
        
        [Column(Name = "campus_luid")]
        private int CampusLuid { get; set; }

        public Lookup Campus
        {
            get
            {
                if (CampusLuid > Constants.ZERO)
                {
                    if (campus == null || campus.LookupID != CampusLuid)
                    {
                        return new Lookup(CampusLuid);
                    }

                    return campus;
                }

                return null;
            } 
            
            set
            {
                CampusLuid = value != null ? value.LookupID : Constants.ZERO;
                campus = value;
            }
        }

        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "created_by")]
        public override string CreatedBy { get; set; }

        [Column(Name = "date_created")]
        public override DateTime DateCreated { get; set; }

        [Column(Name = "modified_by")]
        public override string ModifiedBy { get; set; }

        [Column(Name = "date_modified")]
        public override DateTime DateModified { get; set; }

        [Association(Storage = "scheduleItems", OtherKey = "ScheduleID")]
        public EntitySet<ScheduleItem> ScheduleItems
        {
            get { return scheduleItems; }
            set { scheduleItems.Assign(value); }
        }

        [Association(Storage = "blackoutDates", OtherKey = "ScheduleID")]
        public EntitySet<BlackoutDate> BlackoutDates
        {
            get { return blackoutDates; }
            set { blackoutDates.Assign(value); }
        }

        public override bool IsValid
        {
            get { return Validate(); }
        }

        public List<string> Errors
        {
            get { return errors; }
        }

        internal void Update(string userID)
        {
            if (ScheduleID <= Constants.ZERO)
            {
                CreatedBy = userID;
                DateCreated = DateTime.Now;
            }

            ModifiedBy = userID;
            DateModified = DateTime.Now;
        }

        private bool Validate()
        {
            errors.Clear();

            if (Name == Constants.NULL_STRING)
            {
                errors.Add("Please enter a valid 'Name'.");
            }

            if (Description == Constants.NULL_STRING)
            {
                errors.Add("Please enter a valid 'Description'.");
            }

            if (CampusLuid <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Campus'.");
            }

            if (errors.Count > Constants.ZERO)
            {
                throw new ValidationException(errors);
            }

            return true;
        }

        public static bool CheckDates(int scheduleID, DateTime date)
        {
            string key = KeyHelper.GetKey<IScheduleRepository>();
            IScheduleRepository repository = RepositoryFactory.GetRepository<IScheduleRepository>(key);
            Schedule schedule = repository.GetSchedule(scheduleID);

            if (schedule != null)
            {
                return !schedule.BlackoutDates.Any(b => b.Date.Date == date.Date);
            }

            throw new ApplicationException("Schedule not found.");
        }
    }
}
