/**********************************************************************
* Description:  Edit Baptism Schedule Item
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/17/2009
*
* $Workfile: EditScheduleItem.ascx.cs $
* $Revision: 6 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/EditScheduleItem.ascx.cs   6   2010-01-11 15:34:33-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/EditScheduleItem.ascx.cs $
*  
*  Revision: 6   Date: 2010-01-11 22:34:33Z   User: JasonO 
*  
*  Revision: 5   Date: 2010-01-06 17:12:44Z   User: JasonO 
*  
*  Revision: 4   Date: 2010-01-04 22:30:26Z   User: JasonO 
*  
*  Revision: 3   Date: 2009-12-29 18:03:04Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-28 15:15:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-23 20:53:17Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Application;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.Application;
using Arena.Custom.Cccev.FrameworkUtils.UI;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler
{
    public partial class EditScheduleItem : PortalControl
    {
        [PageSetting("Person Popup Search Page", "The page that is used for the popup search.", false, 13)]
        public string PopupSearchWindowSetting { get { return Setting("PopupSearchWindow", "13", false); } }

        [PageSetting("Schedule Item List Page", "Page containing 'ScheduleItemList' module.", true)]
        public string ScheduleItemListPageSetting { get { return Setting("ScheduleItemListPage", "", true); } }

        private ScheduleController scheduleController;
        private Schedule schedule;
        private ScheduleItem scheduleItem;

        protected void Page_Init(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            btnCancel.Click += btnCancel_Click;
            btnDelete.Click += btnDelete_Click;
            rptrBaptizers.ItemCommand += rptrBaptizers_ItemCommand;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            scheduleController = new ScheduleController();
            GetSchedule();
            btnDelete.Visible = scheduleItem != null;

            if (!Page.IsPostBack)
            {
                BasePage.AddCssLink(Page, "~/UserControls/Custom/Cccev/BaptismScheduler/css/BaptismScheduler.css");
                ShowView();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/default.aspx?page={0}&schedule={1}",
                ScheduleItemListPageSetting, schedule.ScheduleID));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            scheduleController.DeleteScheduleItem(schedule, scheduleItem.ScheduleItemID);
            Response.Redirect(string.Format("~/default.aspx?page={0}&schedule={1}",
                ScheduleItemListPageSetting, schedule.ScheduleID));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatus();
                DateTime date = GetDate();
                Person person = LoadPerson(ihPersonID.Value);
                Person approver = LoadPerson(ihApproverID.Value);
                int scheduleItemID;

                if (scheduleItem == null)
                {
                    scheduleController.CreateScheduleItem(schedule, date, person, GetBaptizers(),
                        approver, cbConfirmed.Checked, CurrentUser.Identity.Name);
                    scheduleItemID = schedule.ScheduleItems.First(i => i.Person.FullName == person.FullName).ScheduleItemID;
                    scheduleItem = schedule.ScheduleItems.SingleOrDefault(i => i.ScheduleItemID == scheduleItemID);
                }
                else
                {
                    scheduleController.UpdateScheduleItem(schedule, scheduleItem.ScheduleItemID, date,
                        person, GetBaptizers(), approver, cbConfirmed.Checked, CurrentUser.Identity.Name);
                    scheduleItemID = scheduleItem.ScheduleItemID;
                }

                if (approver == null)
                {
                    lblApprover.Text = Constants.NULL_STRING;
                }

                ihPersonList.Value = Constants.NULL_STRING;
                ihSearchType.Value = Constants.NULL_STRING;
                ihScheduleItemID.Value = scheduleItemID.ToString();
                ShowSuccess();
            }
            catch (ValidationException ex)
            {
                ShowErrors(ex.Errors);
            }
        }

        protected void bRefresh_Click(object sender, EventArgs e)
        {
            switch (ihSearchType.Value.ToLower().Trim())
            {
                case "person":
                    PopulatePerson();
                    break;
                case "approver":
                    PopulateApprover();
                    break;
                case "baptizer":
                    PopulateBaptizers();
                    break;
                default:
                    break;
            }
        }

        private void rptrBaptizers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                List<string> ids = ihBaptizers.Value.Split(new[] { ',' }).ToList();
                string idToRemove = ((HiddenField)e.Item.FindControl("ihBaptizerPersonID")).Value;
                ids.Remove(idToRemove);
                ihBaptizers.Value = string.Join(",", ids.ToArray());

                List<Person> people = new List<Person>();

                foreach (var id in ids)
                {
                    people.Add(PersonController.GetPerson(int.Parse(id)));
                }

                BindBaptizers(people);
            }
        }

        private void GetSchedule()
        {
            string scheduleID = Request.QueryString.Get("schedule");
            string scheduleItemID = Request.QueryString.Get("scheduleitem");

            if (scheduleID == null)
            {
                return;
            }

            schedule = scheduleController.GetSchedule(int.Parse(scheduleID));

            if (scheduleItemID == null)
            {
                scheduleItemID = ihScheduleItemID.Value.Trim() != Constants.NULL_STRING ? ihScheduleItemID.Value : null;
            }

            if (scheduleItemID != null)
            {
                scheduleItem = schedule.ScheduleItems.SingleOrDefault(i => i.ScheduleItemID == int.Parse(scheduleItemID));
            }
        }

        private void ShowView()
        {
            if (scheduleItem == null)
            {
                if (Request.QueryString.Get("date") != null)
                {
                    DateTime date = DateTime.Parse(Server.UrlDecode(Request.QueryString.Get("date")));
                    dtbDate.Text = date.ToShortDateString();
                }

                dtbTime.Text = "12:00 AM";
                lbRemovePerson.Visible = false;
                lbRemoveApprover.Visible = false;
                return;
            }

            dtbDate.Text = scheduleItem.ScheduleItemDate.ToShortDateString();
            dtbTime.Text = scheduleItem.ScheduleItemDate.ToShortTimeString();
            ihPersonID.Value = scheduleItem.Person.PersonID.ToString();
            lblPerson.Text = scheduleItem.Person.FullName;
            lbRemovePerson.Visible = ihPersonID.Value.Trim() != Constants.NULL_STRING;
            ihApproverID.Value = scheduleItem.ApprovedBy != null ? scheduleItem.ApprovedBy.PersonID.ToString() : Constants.NULL_STRING;
            lblApprover.Text = scheduleItem.ApprovedBy != null ?
                string.Format("{0},<br /> <span class=\"smallText\">{1}</span>", 
                scheduleItem.ApprovedBy.FullName, scheduleItem.DateApproved.ToShortDateString()) : 
                Constants.NULL_STRING;
            lbRemoveApprover.Visible = ihApproverID.Value.Trim() != Constants.NULL_STRING;
            BindBaptizers((from p in scheduleItem.Baptizers
                           select p.Person).ToList());
            cbConfirmed.Checked = scheduleItem.IsConfirmed;
        }

        private void PopulatePerson()
        {
            string[] ids = ihPersonList.Value.Split(new[] { ',' });
            int id = int.Parse(ids.First());
            ihPersonID.Value = id.ToString();
            lblPerson.Text = PersonController.GetPerson(id).FullName;
            lbRemovePerson.Visible = id != Constants.NULL_INT;
            ihPersonList.Value = Constants.NULL_STRING;
            ihSearchType.Value = Constants.NULL_STRING;
        }

        private void PopulateApprover()
        {
            string[] ids = ihPersonList.Value.Split(new[] { ',' });
            int id = int.Parse(ids.First());
            ihApproverID.Value = id.ToString();
            lblApprover.Text = PersonController.GetPerson(id).FullName;
            lbRemoveApprover.Visible = id != Constants.NULL_INT;
            ihPersonList.Value = Constants.NULL_STRING;
            ihSearchType.Value = Constants.NULL_STRING;
        }

        private void PopulateBaptizers()
        {
            if (ihBaptizers.Value.Trim() != Constants.NULL_STRING)
            {
                ihBaptizers.Value += ",";
            }

            ihBaptizers.Value += ihPersonList.Value;
            string[] ids = ihBaptizers.Value.Split(new[] { ',' });
            List<Person> people = new List<Person>();

            foreach (var id in ids)
            {
                if (id != Constants.NULL_STRING)
                {
                    people.Add(PersonController.GetPerson(int.Parse(id)));
                }
            }

            BindBaptizers(people);
            ihPersonList.Value = Constants.NULL_STRING;
        }

        private void BindBaptizers(IEnumerable<Person> people)
        {
            StringBuilder ids = new StringBuilder();

            foreach (var person in people)
            {
                if (ids.Length > Constants.ZERO)
                {
                    ids.Append(",");
                }

                ids.Append(person.PersonID);
            }

            ihBaptizers.Value = ids.ToString();
            rptrBaptizers.DataSource = people;
            rptrBaptizers.DataBind();
        }

        private List<Baptizer> GetBaptizers()
        {
            if (ihBaptizers.Value.Trim() == Constants.NULL_STRING)
            {
                return new List<Baptizer>();
            }

            List<Baptizer> baptizers = new List<Baptizer>();
            string[] ids = ihBaptizers.Value.Split(new[] { ',' });

            foreach (var id in ids)
            {
                baptizers.Add(new Baptizer
                {
                    ScheduleItemID = scheduleItem != null ? scheduleItem.ScheduleItemID : Constants.ZERO,
                    Person = PersonController.GetPerson(int.Parse(id))
                });
            }

            return baptizers;
        }

        private void ShowSuccess()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"success\">");
            html.AppendLine(string.Format("<span class=\"success\">{0}'s baptism has been saved!</span>",
                scheduleItem.Person.FullName));
            html.AppendLine(string.Format("<a href=\"default.aspx?page={0}&schedule={1}\" class=\"smallText\">Back To List</a>",
                ScheduleItemListPageSetting, schedule.ScheduleID));
            html.AppendLine("</div>");
            lStatus.Text = html.ToString();
            lStatus.Visible = true;
        }

        private void ShowErrors(IEnumerable<string> errors)
        {
            lStatus.Text = ExceptionHelper.GetErrorList(errors);
            lStatus.Visible = true;
        }

        private void ClearStatus()
        {
            lStatus.Text = Constants.NULL_STRING;
            lStatus.Visible = false;
        }

        private static Person LoadPerson(string id)
        {
            return id.Trim() != Constants.NULL_STRING ?
                PersonController.GetPerson(int.Parse(id)) : null;
        }

        private DateTime GetDate()
        {
            DateTime date = Constants.NULL_DATE;

            if (dtbDate.Text.Trim() != Constants.NULL_STRING &&
                dtbTime.Text.Trim() != Constants.NULL_STRING)
            {
                date = DateTime.Parse(string.Format("{0} {1}", dtbDate.Text, dtbTime.Text));
            }

            return date;
        }
    }
}