﻿/**********************************************************************
* Description:  List/Edit functionality for Baptism Schedules
* Created By:   Jason Offutt @ Central Christian Church of the East Valley
* Date Created: 12/15/2009
*
* $Workfile: ScheduleList.ascx.cs $
* $Revision: 3 $
* $Header: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/ScheduleList.ascx.cs   3   2010-01-04 15:30:26-07:00   JasonO $
*
* $Log: /trunk/Arena/UserControls/Custom/Cccev/BaptismScheduler/ScheduleList.ascx.cs $
*  
*  Revision: 3   Date: 2010-01-04 22:30:26Z   User: JasonO 
*  
*  Revision: 2   Date: 2009-12-28 15:15:47Z   User: JasonO 
*  
*  Revision: 1   Date: 2009-12-23 20:53:17Z   User: JasonO 
**********************************************************************/

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Arena.Core;
using Arena.Custom.Cccev.BaptismScheduler.Application;
using Arena.Custom.Cccev.BaptismScheduler.Entities;
using Arena.Custom.Cccev.DataUtils;
using Arena.Custom.Cccev.FrameworkUtils.UI;
using Arena.Portal;
using Arena.Security;

namespace ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler
{
    public partial class ScheduleList : PortalControl
    {
        [ListFromSqlSetting("Campus", "Campus where schedule items will be assigned.", true, "",
            "SELECT lookup_type_id, lookup_type_name FROM core_lookup_type")]
        public string CampusLookupTypeSetting { get { return Setting("CampusLookupType", "", true); } }

        [PageSetting("Schedule Items Page", "Page that contains ScheduleItemList module.", true)]
        public string ScheduleItemPageSetting { get { return Setting("ScheduleItemPage", "", true); } }

        private ScheduleController scheduleController;
        private bool editEnabled;

        protected void Page_Init(object sender, EventArgs e)
        {
            dgSchedules.ItemCommand += dgSchedules_ItemCommand;
            dgSchedules.DeleteCommand += dgSchedules_DeleteCommand;
            dgSchedules.ItemDataBound += dgSchedules_ItemDataBound;
            dgSchedules.AddItem += dgSchedules_AddItem;
            dgSchedules.ReBind += DataGridHelper.EmptyEvent;
            btnCancel.Click += btnCancel_Click;
            btnSave.Click += btnSave_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            scheduleController = new ScheduleController();
            editEnabled = CurrentModule.Permissions.Allowed(OperationType.Edit, CurrentUser);
            BasePage.AddCssLink(Page, "~/UserControls/Custom/Cccev/BaptismScheduler/css/BaptismScheduler.css");

            if (!Page.IsPostBack)
            {
                ShowView();
                BindDropDownList();
            }
        }

        private void dgSchedules_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            ListItemType itemType = e.Item.ItemType;
            Schedule schedule = (Schedule) e.Item.DataItem;

            if (itemType == ListItemType.Item || 
                itemType == ListItemType.AlternatingItem || 
                itemType == ListItemType.SelectedItem)
            {
                e.Item.Cells[1].Text = string.Format("<a href=\"default.aspx?page={0}&schedule={1}\">{2}</a>",
                    ScheduleItemPageSetting, schedule.ScheduleID, Server.HtmlEncode(schedule.Name));
                e.Item.Cells[2].Text = Server.HtmlEncode(schedule.Campus.Value);
                e.Item.Cells[3].Text = Server.HtmlEncode(schedule.Description);
            }
        }

        private void dgSchedules_DeleteCommand(object source, DataGridCommandEventArgs e)
        {
            int scheduleID = int.Parse(e.Item.Cells[0].Text);
            Schedule schedule = scheduleController.GetSchedule(scheduleID);
            scheduleController.DeleteSchedule(schedule);
            ShowView();
        }

        private void dgSchedules_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "EditItem")
            {
                int scheduleID = int.Parse(e.Item.Cells[0].Text);
                Schedule schedule = scheduleController.GetSchedule(scheduleID);
                ClearErrors();
                ShowEdit(schedule);
            }
        }

        private void dgSchedules_AddItem(object sender, EventArgs e)
        {
            ClearErrors();
            ShowEdit(null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ihScheduleID.Value == Constants.NULL_STRING)
                {
                    scheduleController.CreateSchedule(new Lookup(int.Parse(ddlCampus.SelectedValue)),
                        tbName.Text.Trim(), tbDescription.Text.Trim(), CurrentUser.Identity.Name);
                }
                else
                {
                    scheduleController.UpdateSchedule(int.Parse(ihScheduleID.Value), new Lookup(int.Parse(ddlCampus.SelectedValue)),
                        tbName.Text.Trim(), tbDescription.Text.Trim(), CurrentUser.Identity.Name);
                }

                ClearFields();
                ClearErrors();
                ShowView();
            }
            catch (ValidationException ex)
            {
                ShowErrors(ex.Errors);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ShowView();
        }

        private void ShowView()
        {
            pnlSchedules.Visible = true;
            pnlEditSchedule.Visible = false;
            BindSchedules();
        }

        private void ShowEdit(Schedule schedule)
        {
            pnlSchedules.Visible = false;
            pnlEditSchedule.Visible = true;

            if (schedule != null)
            {
                ihScheduleID.Value = schedule.ScheduleID.ToString();
                ddlCampus.SelectedValue = schedule.Campus.LookupID.ToString();
                tbName.Text = schedule.Name;
                tbDescription.Text = schedule.Description;
                return;
            }
            
            ClearFields();
        }

        private void ClearFields()
        {
            ihScheduleID.Value = Constants.NULL_STRING;
            ddlCampus.SelectedIndex = 0;
            tbName.Text = Constants.NULL_STRING;
            tbDescription.Text = Constants.NULL_STRING;
        }

        private void BindSchedules()
        {
            dgSchedules.AddEnabled = editEnabled;
            dgSchedules.AddImageUrl = "~/images/add_activity.gif";
            dgSchedules.BulkUpdateEnabled = false;
            dgSchedules.DeleteEnabled = editEnabled;
            dgSchedules.EditEnabled = false;
            dgSchedules.ExportEnabled = true;
            dgSchedules.MailEnabled = true;
            dgSchedules.MergeEnabled = false;
            dgSchedules.MoveEnabled = false;
            dgSchedules.PersonMergeEnabled = false;
            dgSchedules.SMSEnabled = false;
            dgSchedules.DataSource = scheduleController.GetAllSchedules();
            dgSchedules.DataBind();
        }

        private void BindDropDownList()
        {
            LookupType campuses = new LookupType(int.Parse(CampusLookupTypeSetting));
            campuses.Values.LoadDropDownList(ddlCampus);
        }

        private void ShowErrors(IEnumerable<string> errors)
        {
            lErrors.Text = ExceptionHelper.GetErrorList(errors);
            lErrors.Visible = true;
        }

        private void ClearErrors()
        {
            lErrors.Text = Constants.NULL_STRING;
            lErrors.Visible = false;
        }
    }
}