<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScheduleItemList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler.ScheduleItemList" %>

<asp:ScriptManagerProxy ID="smpScripts" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/include/scripts/jquery.1.3.2.min.js" />
        <asp:ScriptReference Path="~/UserControls/Custom/Cccev/BaptismScheduler/js/baptismCalendar.js" />
    </Scripts>
</asp:ScriptManagerProxy>

<script type="text/javascript">
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function()
    {
        initBaptismEvents();
        initBaptismCalendar();
    });
</script>

<asp:UpdatePanel ID="upScheduleItems" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Panel ID="pnlSchedules" runat="server" Visible="false">
            <p class="field">
                <label for="<%= ddlSchedules.ClientID %>" class="formLabel">Schedules</label>
                <asp:DropDownList ID="ddlSchedules" runat="server" class="formItem" AutoPostBack="true" />
            </p>
        </asp:Panel>
    
        <asp:Panel ID="pnlControls" runat="server" CssClass="sub-nav">
            <asp:LinkButton ID="lbAddItem" runat="server" CssClass="sub-nav">+ Add Baptism</asp:LinkButton>
            <asp:LinkButton ID="lbAddBlackout" runat="server" CssClass="sub-nav">Black Out A Day</asp:LinkButton>
            <asp:LinkButton ID="lbReport" runat="server" CssClass="sub-nav">Print Report</asp:LinkButton>
        </asp:Panel>
        
        <div id="calendar">
            <h3>Calendar</h3>
            <asp:Calendar ID="calSchedule" runat="server" DayNameFormat="FirstLetter" SelectionMode="Day" BorderColor="#999999"
                TitleStyle-BackColor="#e5e5e5" NextPrevStyle-ForeColor="#333333" FirstDayOfWeek="Monday" Width="200" CssClass="calendar">
                <DayStyle CssClass="calendar-day" />
                <TodayDayStyle CssClass="calendar-today" />
                <SelectedDayStyle CssClass="calendar-selected" />
                <OtherMonthDayStyle CssClass="calendar-last-month" ForeColor="#999999" />
                <DayHeaderStyle CssClass="calendar-day-header" />
                <NextPrevStyle CssClass="calendar-next-prev" ForeColor="#777777" />
                <TitleStyle CssClass="calendar-title" />
            </asp:Calendar>
        </div>
        
        <div id="content">
            <asp:Panel ID="pnlItems" runat="server">
                <asp:PlaceHolder ID="phItems" runat="server" />
            </asp:Panel>            
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
