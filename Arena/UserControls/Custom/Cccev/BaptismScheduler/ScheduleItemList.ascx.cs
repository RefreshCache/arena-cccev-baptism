/**********************************************************************
* Description:  List Baptism Schedule Items
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/15/2009
*
* $Workfile: ScheduleItemList.ascx.cs $
* $Revision: 5 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/ScheduleItemList.ascx.cs   5   2010-01-06 14:58:48-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/ScheduleItemList.ascx.cs $
*  
*  Revision: 5   Date: 2010-01-06 21:58:48Z   User: JasonO 
*  
*  Revision: 4   Date: 2010-01-06 17:12:44Z   User: JasonO 
*  
*  Revision: 3   Date: 2010-01-04 22:30:26Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-28 15:15:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-23 20:53:17Z   User: JasonO 
**********************************************************************/

using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Application;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.DataUtils;
using Arena.Portal;
using Arena.Security;

namespace ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler
{
    public partial class ScheduleItemList : PortalControl
    {
        [PageSetting("Blackout Date Edit Page", "Page that contains EditBlackoutDate module.", true)]
        public string EditBlackoutDatePageSetting { get { return Setting("EditBlackoutDatePage", "", true); } }

        [PageSetting("Schedule Item Edit Page", "Page that contains EditScheduleItem module.", true)]
        public string EditScheduleItemPageSetting { get { return Setting("EditScheduleItemPage", "", true); } }

        [TextSetting("Printable Report Path", "Relative path to reporting services report.", false)]
        public string ReportPathSetting { get { return Setting("ReportPath", "/Arena/BaptismScheduler/BaptismScheduleItems", false); } }

        private ScheduleController scheduleController;
        protected bool editEnabled;
        protected Schedule schedule;
        protected DateRange dates;

        protected void Page_Init(object sender, EventArgs e)
        {
            scheduleController = new ScheduleController();
            editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);

            if (!Page.IsPostBack)
            {
                InitCalendar();
                BindSchedules();
            }

            InitEvents();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetSchedule();
            SetDates();

            if (!Page.IsPostBack)
            {
                BasePage.AddCssLink(Page, "~/UserControls/Custom/Cccev/BaptismScheduler/css/BaptismScheduler.css");
                lbReport.Visible = ReportPathSetting.Trim() != Constants.NULL_STRING;
                BuildList();
            }
        }

        private void lbAddItem_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/default.aspx?page={0}&schedule={1}&date={2}",
                EditScheduleItemPageSetting, schedule.ScheduleID, 
                Server.UrlEncode(calSchedule.SelectedDate.ToShortDateString())));
        }

        private void lbAddBlackout_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/default.aspx?page={0}&schedule={1}&date={2}",
                EditBlackoutDatePageSetting, schedule.ScheduleID,
                Server.UrlEncode(calSchedule.SelectedDate.ToShortDateString())));
        }

        private void ddlSchedules_SelectedIndexChanged(object sender, EventArgs e)
        {
            schedule = scheduleController.GetSchedule(int.Parse(ddlSchedules.SelectedValue));
            BuildList();
        }

        private void calSchedule_SelectionChanged(object sender, EventArgs e)
        {
            SetDates();
            BuildList();
        }

        private void calSchedule_DayRender(object sender, DayRenderEventArgs e)
        {
            DateTime day = e.Day.Date;

            if (schedule.ScheduleItems.Any(i => i.ScheduleItemDate.Date == day.Date))
            {
                e.Cell.Style.Add("font-weight", "bold");
            }

            if (schedule.BlackoutDates.Any(b => b.Date.Date == day.Date))
            {
                e.Cell.Style.Add("background-color", "#ffcfcf");
            }

        }

        private void calSchedule_VisibleMonthChanged(object sender, MonthChangedEventArgs e)
        {
            BuildList();
        }

        private void lbReport_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/ReportPDFViewer.aspx?Report={0}&StartDate={1}&EndDate={2}&ScheduleID={3}",
                ReportPathSetting, Server.UrlEncode(dates.Start.ToShortDateString()), 
                Server.UrlEncode(dates.End.ToShortDateString()), schedule.ScheduleID));
        }

        private void GetSchedule()
        {
            string scheduleID = Request.QueryString.Get("schedule");

            if (scheduleID == null)
            {
                pnlSchedules.Visible = true;

                if (ddlSchedules.SelectedValue.Trim() != Constants.NULL_STRING)
                {
                    scheduleID = ddlSchedules.SelectedValue;
                }
            }

            schedule = scheduleID != null ? scheduleController.GetSchedule(int.Parse(scheduleID)) : new Schedule { Name = "New Schedule" };
        }

        private void InitEvents()
        {
            ddlSchedules.SelectedIndexChanged += ddlSchedules_SelectedIndexChanged;
            calSchedule.SelectionChanged += calSchedule_SelectionChanged;
            calSchedule.DayRender += calSchedule_DayRender;
            calSchedule.VisibleMonthChanged += calSchedule_VisibleMonthChanged;
            lbAddBlackout.Click += lbAddBlackout_Click;
            lbAddItem.Click += lbAddItem_Click;
            lbReport.Click += lbReport_Click;
        }

        private void BuildList()
        {
            if (schedule == null || !pnlItems.Visible)
            {
                return;
            }

            phItems.Controls.Clear();
            phItems.Controls.Add(new LiteralControl(string.Format("<h3>{0}:&nbsp; {1} - {2}</h3>",
                Server.HtmlEncode(schedule.Name), dates.Start.ToString("MMMM d"), dates.End.ToString("MMMM d"))));
            BuildBlackoutDate();
            var scheduleItems = scheduleController.GetScheduleItemsByDateRange(schedule, dates);

            if (scheduleItems.Count() > Constants.ZERO)
            {
                DateTime current = DateTime.MinValue;

                foreach (var i in scheduleItems)
                {
                    if (current != i.ScheduleItemDate)
                    {
                        current = i.ScheduleItemDate;
                        BuildItemListHeader(current);
                    }

                    BuildListItem(i);
                }

                return;
            }
            
            phItems.Controls.Add(new LiteralControl("<h4 class=\"date-header\">No baptisms scheduled for the selected week!</h4>"));
        }

        private void BuildItemListHeader(DateTime date)
        {
            phItems.Controls.Add(new LiteralControl("<div class=\"sub-header\">"));
            phItems.Controls.Add(new LiteralControl(
                string.Format("<h4 class=\"date-header\">Service: {0} - {1} {2}</h4>",
                date.ToShortTimeString(), date.DayOfWeek, date.ToString("MM/d"))));
            phItems.Controls.Add(new LiteralControl("<span class=\"date-sub-header\">Attendee</span>"));
            phItems.Controls.Add(new LiteralControl("<span class=\"date-sub-header\">Baptized By</span>"));
            phItems.Controls.Add(new LiteralControl("<span class=\"date-sub-header\">Phone Number</span>"));
            phItems.Controls.Add(new LiteralControl("<span class=\"date-sub-header\">Approved By</span>"));
            phItems.Controls.Add(new LiteralControl("<span class=\"date-sub-header\">Confirmed</span>"));
            phItems.Controls.Add(new LiteralControl("</div>"));
        }

        private void BuildListItem(ScheduleItem scheduleItem)
        {
            phItems.Controls.Add(new LiteralControl("<div class=\"item\">"));
            phItems.Controls.Add(new LiteralControl(string.Format("<span class=\"item-field\">{0}</span>", 
                scheduleItem.Person.FullName)));
            phItems.Controls.Add(new LiteralControl("<span class=\"item-field\">"));
            phItems.Controls.Add(new LiteralControl("<ul>"));

            foreach (var b in scheduleItem.Baptizers)
            {
                phItems.Controls.Add(new LiteralControl(string.Format("<li>{0}</li>", b.Person.FullName)));
            }

            phItems.Controls.Add(new LiteralControl("</ul>"));
            phItems.Controls.Add(new LiteralControl("</span>"));
            phItems.Controls.Add(new LiteralControl(string.Format("<span class=\"item-field\">{0}</span>",
                scheduleItem.Person.Phones.FindByType(SystemLookup.PhoneType_Home))));
            phItems.Controls.Add(new LiteralControl(string.Format("<span class=\"item-field\">{0}</span>",
                scheduleItem.ApprovedBy != null ? scheduleItem.ApprovedBy.FullName : Constants.NULL_STRING)));
            phItems.Controls.Add(new LiteralControl("<span class=\"item-field\">"));

            CheckBox cb = new CheckBox
            {
                ID = string.Format("cbConfirmed_{0}", scheduleItem.ScheduleItemID), 
                Checked = scheduleItem.IsConfirmed, 
                Enabled = false
            };

            phItems.Controls.Add(cb);
            ImageButton ibEdit = new ImageButton
            {
                ID = string.Format("ibEdit_{0}", 
                scheduleItem.ScheduleItemID), 
                ImageUrl = "~/images/edit.gif", Visible = editEnabled
            };

            ibEdit.Attributes.Add("onclick", GetJavaScriptRedirect(EditScheduleItemPageSetting, 
                "scheduleitem", scheduleItem.ScheduleItemID));
            phItems.Controls.Add(ibEdit);
            phItems.Controls.Add(new LiteralControl("</div>"));
        }

        private void BuildBlackoutDate()
        {
            BlackoutDate blackoutDate = schedule.BlackoutDates.SingleOrDefault(b => b.Date.Date == calSchedule.SelectedDate.Date);

            if (blackoutDate != null)
            {
                phItems.Controls.Add(new LiteralControl("<div class=\"errors\">"));
                phItems.Controls.Add(new LiteralControl(string.Format("<h4 class=\"errorText\">{0} has been blacked out!</h4>",
                    blackoutDate.Date.ToLongDateString())));
                phItems.Controls.Add(new LiteralControl(string.Format("<span class=\"errorCaption\">&quot;{0}&quot;</span>", 
                    Server.HtmlEncode(blackoutDate.Description))));
                LinkButton lbEdit = new LinkButton();
                phItems.Controls.Add(lbEdit);
                lbEdit.ID = string.Format("lbEditBlackoutDate_{0}", blackoutDate.BlackoutDateID);
                lbEdit.Text = "Edit";
                lbEdit.CssClass = "smallText edit-blackout";
                lbEdit.Attributes.Add("onclick", GetJavaScriptRedirect(EditBlackoutDatePageSetting, 
                    "blackoutdate", blackoutDate.BlackoutDateID));
                phItems.Controls.Add(new LiteralControl("</div>"));
            }
        }

        private void BindSchedules()
        {
            var schedules = scheduleController.GetAllSchedules();

            foreach (var s in schedules)
            {
                ddlSchedules.Items.Add(new ListItem(s.Name, s.ScheduleID.ToString()));
            }
        }

        private void InitCalendar()
        {
            if (calSchedule.VisibleDate == DateTime.MinValue)
            {
                calSchedule.VisibleDate = DateTime.Now;
            }

            if (calSchedule.SelectedDate == DateTime.MinValue)
            {
                calSchedule.SelectedDate = DateTime.Now;
            }
        }

        private void SetDates()
        {
            if (dates == null)
            {
                dates = new DateRange();
            }

            dates.Start = DateUtils.GetWeekStartDate(calSchedule.SelectedDate);
            dates.End = dates.Start.AddDays(6);
        }

        private string GetJavaScriptRedirect(string pageID, string objectKey, int id)
        {
            return string.Format("window.location = '{0}'; return false;", GetUrl(pageID, objectKey, id));
        }

        private string GetUrl(string pageID, string objectKey, int id)
        {
            return string.Format("default.aspx?page={0}&schedule={1}&{2}={3}",
                pageID, schedule.ScheduleID, objectKey, id);
        }
    }
}