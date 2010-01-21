/**********************************************************************
* Description:  Edit Baptism Blackout Date
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/17/2009
*
* $Workfile: EditBlackoutDate.ascx.cs $
* $Revision: 5 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/EditBlackoutDate.ascx.cs   5   2010-01-06 10:12:44-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/EditBlackoutDate.ascx.cs $
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
using Arena.Custom.Cccev.BaptismScheduler.Application;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.UI;
using Arena.Portal;

namespace ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler
{
    public partial class EditBlackoutDate : PortalControl
    {
        [PageSetting("Schedule Item List Page", "Page containing 'ScheduleItemList' module.", true)]
        public string ScheduleItemListPageSetting { get { return Setting("ScheduleItemListPage", "", true); } }

        private ScheduleController scheduleController;
        private Schedule schedule;
        private BlackoutDate blackoutDate;

        protected void Page_Init(object sender, EventArgs e)
        {
            btnSave.Click += btnSave_Click;
            btnDelete.Click += btnDelete_Click;
            btnCancel.Click += btnCancel_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            scheduleController = new ScheduleController();
            GetSchedule();
            btnDelete.Visible = blackoutDate != null;

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
            scheduleController.DeleteBlackoutDate(schedule, blackoutDate.BlackoutDateID);
            Response.Redirect(string.Format("~/default.aspx?page={0}&schedule={1}",
                ScheduleItemListPageSetting, schedule.ScheduleID));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ClearStatus();
                DateTime date = dtbDate.Text.Trim() != Constants.NULL_STRING ?
                    DateTime.Parse(dtbDate.Text) : Constants.NULL_DATE;
                int blackoutDateID;

                if (blackoutDate == null)
                {
                    scheduleController.CreateBlackoutDate(schedule, tbDescription.Text.Trim(),
                        date, CurrentUser.Identity.Name);
                    blackoutDateID = schedule.BlackoutDates.First(b => b.Date.Date == date.Date).BlackoutDateID;
                    blackoutDate = schedule.BlackoutDates.SingleOrDefault(b => b.BlackoutDateID == blackoutDateID);
                }
                else
                {
                    scheduleController.UpdateBlackoutDate(schedule, blackoutDate.BlackoutDateID,
                        tbDescription.Text.Trim(), date, CurrentUser.Identity.Name);
                    blackoutDateID = blackoutDate.BlackoutDateID;
                }

                ihBlackoutDateID.Value = blackoutDateID.ToString();
                ShowSuccess();
            }
            catch (ValidationException ex)
            {
                ShowErrors(ex.Errors);
            }
        }

        private void GetSchedule()
        {
            string scheduleID = Request.QueryString.Get("schedule");
            string blackoutDateID = Request.QueryString.Get("blackoutdate");

            if (scheduleID == null)
            {
                return;
            }

            schedule = scheduleController.GetSchedule(int.Parse(scheduleID));

            if (blackoutDateID == null)
            {
                blackoutDateID = ihBlackoutDateID.Value.Trim() != Constants.NULL_STRING ? ihBlackoutDateID.Value : null;
            }

            if (blackoutDateID != null)
            {
                blackoutDate = schedule.BlackoutDates.SingleOrDefault(b => b.BlackoutDateID == int.Parse(blackoutDateID));
            }
        }

        private void ShowView()
        {
            if (blackoutDate != null)
            {
                dtbDate.Text = blackoutDate.Date.ToShortDateString();
                tbDescription.Text = blackoutDate.Description;
            }
            else if (Request.QueryString.Get("date") != null)
            {
                DateTime date = DateTime.Parse(Server.UrlDecode(Request.QueryString.Get("date")));
                dtbDate.Text = date.ToShortDateString();
            }
        }

        private void ShowSuccess()
        {
            StringBuilder html = new StringBuilder();
            html.AppendLine("<div class=\"success\">");
            html.AppendLine(string.Format("<span class=\"success\">{0} has been blacked out!</span>", 
                blackoutDate.Date.ToString("MMMM d yyyy")));
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
    }
}