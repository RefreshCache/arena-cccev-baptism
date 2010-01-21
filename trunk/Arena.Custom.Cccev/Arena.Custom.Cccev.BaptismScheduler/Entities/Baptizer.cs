/**********************************************************************
* Description:  Defines domain logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/9/2009
*
* $Workfile: Baptizer.cs $
* $Revision: 5 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/Baptizer.cs   5   2009-12-29 11:23:50-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/Baptizer.cs $
*  
*  Revision: 5   Date: 2009-12-29 18:23:50Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-29 17:50:35Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-28 15:14:52Z   User: JasonO 
*  Improving validation logic 
*  
*  Revision: 2   Date: 2009-12-23 20:52:24Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using Arena.Core;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Entity;

namespace Arena.Custom.Cccev.BaptismScheduler.Entities
{
    [Table(Name = "cust_cccev_bsch_baptizer")]
    public class Baptizer : CentralObjectBase
    {
        private Person person;
        private readonly List<string> errors = new List<string>();

        [Column(Name = "baptizer_id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int BaptizerID { get; set; }

        [Column(Name = "schedule_item_id")]
        public int ScheduleItemID { get; set; }

        [Column(Name = "person_id")]
        private int PersonID { get; set; }

        public Person Person
        {
            get
            {
                if (PersonID > Constants.ZERO)
                {
                    if (person == null || person.PersonID != PersonID)
                    {
                        return new Person(PersonID);
                    }

                    return person;
                }

                return null;
            }

            set
            {
                PersonID = value != null ? value.PersonID : Constants.ZERO;
                person = value;
            }
        }

        public override bool IsValid
        {
            get { return Validate(); }
        }

        public List<string> Errors
        {
            get { return errors; }
        }

        private bool Validate()
        {
            errors.Clear();

            if (ScheduleItemID <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Schedule Item'.");
            }

            if (PersonID <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Person'.");
            }

            if (errors.Count > Constants.ZERO)
            {
                throw new ValidationException(errors);
            }

            return true;
        }

        #region Unsupported Properties

        /// <summary>
        /// Not supported. This field is not tied to the database.
        /// </summary>
        public override string CreatedBy { get; set; }

        /// <summary>
        /// Not supported. This field is not tied to the database.
        /// </summary>
        public override DateTime DateCreated { get; set; }

        /// <summary>
        /// Not supported. This field is not tied to the database.
        /// </summary>
        public override string ModifiedBy { get; set; }

        /// <summary>
        /// Not supported. This field is not tied to the database.
        /// </summary>
        public override DateTime DateModified { get; set; }

        #endregion
    }
}
