/**********************************************************************
* Description:  Defines domain logic for Baptism Scheduler
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/8/2009
*
* $Workfile: ScheduleItem.cs $
* $Revision: 7 $
* $Header: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/ScheduleItem.cs   7   2010-01-11 14:40:25-07:00   JasonO $
*
* $Log: /trunk/Arena.Custom.Cccev/Arena.Custom.Cccev.BaptismScheduler/Entities/ScheduleItem.cs $
*  
*  Revision: 7   Date: 2010-01-11 21:40:25Z   User: JasonO 
*  
*  Revision: 6   Date: 2009-12-29 18:23:50Z   User: JasonO 
*  
*  Revision: 5   Date: 2009-12-29 17:50:35Z   User: JasonO 
*  
*  Revision: 4   Date: 2009-12-28 15:14:52Z   User: JasonO 
*  Improving validation logic 
*  
*  Revision: 3   Date: 2009-12-23 20:52:24Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-17 21:46:36Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-14 23:56:27Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Util;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Entity;
using Arena.Custom.Cccev.FrameworkUtils.Util;

namespace Arena.Custom.Cccev.BaptismScheduler.Entities
{
    [Table(Name = "cust_cccev_bsch_schedule_item")]
    public class ScheduleItem : CentralObjectBase
    {
        private Person person;
        private Person approvedBy;
        private Person confirmedBy;
        private readonly EntitySet<Baptizer> baptizers = new EntitySet<Baptizer>();
        private readonly List<string> errors = new List<string>();

        [Column(Name = "schedule_item_id", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ScheduleItemID { get; set; }

        [Column(Name = "schedule_item_date")]
        public DateTime ScheduleItemDate { get; set; }

        [Column(Name = "schedule_id")]
        public int ScheduleID { get; set; }

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

        [Association(Storage = "baptizers", OtherKey = "ScheduleItemID")]
        public EntitySet<Baptizer> Baptizers
        {
            get { return baptizers; }
            set { baptizers.Assign(value); }
        }

        [Column(Name = "approved_by")]
        private int? ApprovedByID { get; set; }

        public Person ApprovedBy
        {
            get
            {
                if (ApprovedByID.HasValue)
                {
                    if (approvedBy == null || approvedBy.PersonID != ApprovedByID)
                    {
                        return new Person(ApprovedByID.Value);
                    }

                    return approvedBy;
                }

                return null;
            }

            set
            {
                if (value != null)
                {
                    ApprovedByID = value.PersonID;
                }
                else
                {
                    ApprovedByID = null;
                }

                approvedBy = value;
            }
        }

        [Column(Name = "date_approved")]
        public DateTime DateApproved { get; private set; }

        [Column(Name = "is_confirmed")]
        public bool IsConfirmed { get; set; }

        [Column(Name = "confirmed_by")]
        private int? ConfirmedByID { get; set; }

        public Person ConfirmedBy
        {
            get
            {
                if (ConfirmedByID.HasValue)
                {
                    if (confirmedBy == null || confirmedBy.PersonID != ConfirmedByID)
                    {
                        return new Person(ConfirmedByID.Value);
                    }

                    return confirmedBy;
                }

                return null;
            }
            
            private set
            {
                if (value != null)
                {
                    ConfirmedByID = value.PersonID;
                }
                else
                {
                    ConfirmedByID = null;
                }

                confirmedBy = value;
            }
        }

        [Column(Name = "date_confirmed")]
        public DateTime DateConfirmed { get; private set; }

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
            get { return errors;}
        }

        internal void Update(string userID)
        {
            if (ScheduleItemID <= Constants.ZERO)
            {
                CreatedBy = userID;
                DateCreated = DateTime.Now;
            }

            ModifiedBy = userID;
            DateModified = DateTime.Now;
            SyncConfirmed();
            SyncApproved();
        }

        /// <summary>
        /// Parses through internal list of Baptizers and compares them to 
        /// the list passed in.  It adds missing Baptizers to the internal
        /// list and returns a list of Baptizer to remove that are in the
        /// internal list, but not in the list of Baptizers passed in.
        /// </summary>
        /// <param name="list">The new master List of Baptizers</param>
        /// <returns>List of Baptizers to be deleted</returns>
        internal List<Baptizer> SyncBaptizers(IEnumerable<Baptizer> list)
        {
            IEqualityComparer<Baptizer> comparer = new BaptizerComparer();
            var baptizersToRemove = (from b in baptizers
                                     where !list.Any(l => l.Person.PersonID == b.Person.PersonID)
                                     select b).ToList();

            foreach (var b in list)
            {
                Baptizer baptizer = b;

                if (!baptizers.Any(bap => comparer.Equals(bap, baptizer)))
                {
                    if (baptizer.ScheduleItemID <= Constants.ZERO)
                    {
                        baptizer.ScheduleItemID = ScheduleItemID;
                    }

                    baptizers.Add(baptizer);
                }
            }

            return baptizersToRemove;
        }

        private bool Validate()
        {
            errors.Clear();

            if (PersonID <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Person'.");
            }

            if (ScheduleItemDate == Constants.NULL_DATE)
            {
                errors.Add("Please enter a valid 'Schedule Date'.");
            }

            if (ApprovedByID.HasValue && DateApproved == Constants.NULL_DATE)
            {
                errors.Add("Please enter a valid 'Approval Date'.");
            }

            if (IsConfirmed && ConfirmedByID.HasValue && DateConfirmed == Constants.NULL_DATE)
            {
                errors.Add("Please enter a valid 'Confirmation Date'.");
            }

            if (ScheduleID <= Constants.ZERO)
            {
                errors.Add("Please enter a valid 'Schedule'.");
            }

            CheckDates();

            if (errors.Count > Constants.ZERO)
            {
                throw new ValidationException(errors);
            }

            return true;
        }

        private void SyncApproved()
        {
            bool hasPerson = ApprovedByID.HasValue;
            bool hasDate = (DateApproved != Constants.NULL_DATE && DateApproved != DateTime.MinValue);

            if (hasPerson && !hasDate)
            {
                DateApproved = DateTime.Now;
            }
            else if (!hasPerson)
            {
                DateApproved = Constants.NULL_DATE;
            }
        }

        private void SyncConfirmed()
        {
            if (IsConfirmed && !ConfirmedByID.HasValue)
            {
                ConfirmedBy = PersonUtils.GetCurrentPerson();
                DateConfirmed = DateTime.Now;
            }
            else if (!IsConfirmed)
            {
                ConfirmedBy = null;
                DateConfirmed = Constants.NULL_DATE;
            }
        }

        private void CheckDates()
        {
            try
            {
                if (!Schedule.CheckDates(ScheduleID, ScheduleItemDate))
                {
                    errors.Add("Unable to schedule a baptism on a Black Out Date.");
                }
            }
            catch (ApplicationException ex)
            {
                errors.Add(ex.Message);
            }
        }
    }
}
