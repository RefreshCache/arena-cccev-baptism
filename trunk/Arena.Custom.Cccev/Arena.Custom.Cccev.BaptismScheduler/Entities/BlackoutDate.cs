/**********************************************************************
* Description:  Defines domain logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: BlackoutDate.cs $
* $Revision: 3 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/BlackoutDate.cs   3   2009-12-29 11:23:50-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/BlackoutDate.cs $
*  
*  Revision: 3   Date: 2009-12-29 18:23:50Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-28 15:14:52Z   User: JasonO 
*  Improving validation logic 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Entity;

namespace Arena.Custom.Cccev.BaptismScheduler.Entities
{
    [Table(Name = "cust_cccev_bsch_blackout_date")]
    public class BlackoutDate : CentralObjectBase
    {
        private readonly List<string> errors = new List<string>();

        [Column(Name = "blackout_date_id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int BlackoutDateID { get; set; }

        [Column(Name="schedule_id")]
        public int ScheduleID { get; set; }

        [Column(Name = "description")]
        public string Description { get; set; }

        [Column(Name = "date")]
        public DateTime Date { get; set; }

        [Column(Name = "created_by")]
        public override string CreatedBy { get; set; }

        [Column(Name = "date_created")]
        public override DateTime DateCreated { get; set; }

        [Column(Name = "modified_by")]
        public override string ModifiedBy { get; set; }

        [Column(Name = "date_modified")]
        public override DateTime DateModified { get; set; }

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
            if (BlackoutDateID <= Constants.ZERO)
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

            if (Description == Constants.NULL_STRING)
            {
                errors.Add("Please enter a valid 'Description'.");
            }

            if (Date == Constants.NULL_DATE || Date == DateTime.MinValue)
            {
                errors.Add("Please enter a valid 'Date'.");
            }

            if (ScheduleID <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Schedule'.");
            }

            if (errors.Count > Constants.ZERO)
            {
                throw new ValidationException(errors);
            }

            return true;
        }
    }
}
