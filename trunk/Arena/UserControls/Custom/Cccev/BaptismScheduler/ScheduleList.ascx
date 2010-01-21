<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ScheduleList.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler.ScheduleList" %>
<asp:UpdatePanel ID="upScheduleList" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Panel id="pnlSchedules" runat="server">
            <h3>Baptism Schedules</h3>
            <Arena:DataGrid ID="dgSchedules" runat="server">
                <Columns>
                    <asp:BoundColumn HeaderText="ID" DataField="ScheduleID" />
                    <asp:BoundColumn HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundColumn HeaderText="Campus" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundColumn HeaderText="Description" HeaderStyle-HorizontalAlign="Left" 
                        ItemStyle-HorizontalAlign="Left" ItemStyle-Wrap="true" />
                    <asp:ButtonColumn CommandName="EditItem" ButtonType="LinkButton" Text="<img src='images/edit.gif' border='0' />"
					    ItemStyle-Wrap="False" ItemStyle-VerticalAlign="Top" />
                </Columns>
            </Arena:DataGrid>
        </asp:Panel>

        <asp:Panel ID="pnlEditSchedule" runat="server" Visible="false">
            <h3>Add/Edit Schedule</h3>
            <asp:Literal ID="lErrors" runat="server" Visible="false" />
            <input type="hidden" id="ihScheduleID" runat="server" />
            <p class="field">
                <label for="<%= tbName.ClientID %>" class="formLabel">Name</label>
                <asp:TextBox ID="tbName" runat="server" CssClass="formItem" />
            </p>
            <p class="field">
                <label for="<%= tbDescription.ClientID %>" class="formLabel">Description</label>
                <asp:TextBox ID="tbDescription" runat="server" CssClass="formItem" TextMode="MultiLine" />
            </p>
            <p class="field">
                <label for="<%= ddlCampus.ClientID %>" class="formLabel">Campus</label>
                <asp:DropDownList ID="ddlCampus" runat="server" CssClass="formItem" />
            </p>
            <asp:Button ID="btnSave" runat="server" class="formItem" Text="Save" />
            <asp:Button ID="btnCancel" runat="server" class="formItem" Text="Cancel" />
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>