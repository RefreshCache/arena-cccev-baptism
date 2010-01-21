<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditBlackoutDate.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler.EditBlackoutDate" %>

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
    });
</script>

<div class="edit">
    <asp:UpdatePanel ID="upBlackoutDates" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Panel ID="pnlEditBlackout" runat="server">
                <h3>Add/Edit Blackout Date</h3>
                <input type="hidden" id="ihBlackoutDateID" runat="server" />
                <asp:Literal ID="lStatus" runat="server" Visible="false" />
                <p class="field">
                    <label for="<%= dtbDate.ClientID %>" class="formLabel">Blackout Date</label>
                    <Arena:DateTextBox ID="dtbDate" runat="server" CssClass="formItem" />
                </p>
                <p class="field">
                    <label for="<%= tbDescription.ClientID %>" class="formLabel">Description</label>
                    <asp:TextBox ID="tbDescription" runat="server" CssClass="formItem" TextMode="MultiLine" />
                </p>
                <asp:Button ID="btnSave" runat="server" CssClass="formItem" Text="Save" />
                <asp:Button ID="btnDelete" runat="server" CssClass="formItem delete" Text="Delete" />
                <asp:Button ID="btnCancel" runat="server" CssClass="formItem" Text="Cancel" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>