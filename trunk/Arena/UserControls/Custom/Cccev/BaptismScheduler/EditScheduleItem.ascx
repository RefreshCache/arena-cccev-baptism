<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EditScheduleItem.ascx.cs" Inherits="ArenaWeb.UserControls.Custom.Cccev.BaptismScheduler.EditScheduleItem" %>

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

    function openSearchWindow()
    {
        var tPos = window.screenTop + 100;
        var lPos = window.screenLeft + 100;
        $('input#ihPersonListID').val('<%= ihPersonList.ClientID %>');
        $('input#ihRefreshButtonID').val('<%= bRefresh.ClientID %>');
        var searchWindow = window.open('Default.aspx?page=<%= PopupSearchWindowSetting %>', 'Search', 'height=400,width=600,resizable=yes,scrollbars=yes,toolbar=no,location=no,directories=no,status=no,menubar=no,top=' + tPos + ',left=' + lPos);
        searchWindow.focus();
    }
</script>

<div class="edit">
    <asp:UpdatePanel ID="upScheduleItems" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <input type="hidden" id="ihPersonList" runat="server" name="ihPersonList" />
            <input type="hidden" id="ihSearchType" runat="server" name="ihSearchType" />
            <button ID="bRefresh" class="refresh" runat="server" onserverclick="bRefresh_Click">Refresh</button>
        
            <asp:Panel ID="pnlEditItem" runat="server">
                <h3>Add/Edit Baptism Schedule Item</h3>
                <input type="hidden" id="ihScheduleItemID" runat="server" />
                <asp:Literal ID="lStatus" runat="server" Visible="false" />
                <p class="field">
                    <label for="<%= dtbDate.ClientID %>" class="formLabel">Date</label>
                    <Arena:DateTextBox ID="dtbDate" runat="server" CssClass="formItem date" />
                    <Arena:DateTextBox ID="dtbTime" runat="server" CssClass="formItem date" Format="Time" />
                </p>
                <div class="field">
                    <label class="formLabel">Person Being Baptized</label>
                    <div class="personPicker">
                        <input type="hidden" id="ihPersonID" runat="server" name="ihPersonID" />
                        <asp:Label ID="lblPerson" runat="server" CssClass="person formItem" />
                        <div class="personSearchButtons">
                            <input type="image" id="lbFindPerson" class="findPerson" src="/Arena/UserControls/Custom/Cccev/BaptismScheduler/images/search.png" onclick="openSearchWindow(); return false;" />
                            <input type="image" id="lbRemovePerson" runat="server" class="removePerson" src="/Arena/UserControls/Custom/Cccev/BaptismScheduler/images/delete.png" />
                        </div>
                    </div>
                </div>
                <div class="field">
                    <label class="formLabel">Baptizers</label>
                    <div class="personPicker">
                        <input type="hidden" id="ihBaptizers" runat="server" name="ihBaptizers" />
                        <asp:Repeater ID="rptrBaptizers" runat="server">
                            <HeaderTemplate>
                                <ul class="baptizers">
                            </HeaderTemplate>
                            <ItemTemplate>
                                <li class="baptizer">
                                    <asp:HiddenField ID="ihBaptizerPersonID" runat="server" Value='<%# Eval("PersonID") %>' />
                                    <span class="formItem"><%# Eval("FullName") %></span>
                                    <asp:ImageButton ID="ibDeleteBaptizer" runat="server" CommandName="Delete" CssClass="formItem" 
                                        ImageUrl="~/UserControls/Custom/Cccev/BaptismScheduler/images/delete.png" />
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                        </asp:Repeater>
                        <div class="personSearchButtons">
                            <input type="image" id="lbFindBaptizer" class="findPerson" src="/Arena/UserControls/Custom/Cccev/BaptismScheduler/images/search.png" onclick="openSearchWindow(); return false;" />
                        </div>
                    </div>
                </div>
                <div class="field">
                    <label class="formLabel">Approved By</label>
                    <div class="personPicker">
                        <input type="hidden" id="ihApproverID" runat="server" name="ihApproverID" />
                        <asp:Label ID="lblApprover" runat="server" CssClass="person formItem" />
                        <div class="personSearchButtons">
                            <input type="image" id="lbFindApprover" class="findPerson" src="/Arena/UserControls/Custom/Cccev/BaptismScheduler/images/search.png" onclick="openSearchWindow(); return false;" />
                            <input type="image" id="lbRemoveApprover" runat="server" class="removePerson" src="/Arena/UserControls/Custom/Cccev/BaptismScheduler/images/delete.png" />
                        </div>
                    </div>
                </div>
                <p class="field">
                    <label class="formLabel">Confirmed</label>
                    <asp:CheckBox ID="cbConfirmed" runat="server" CssClass="formItem" />
                </p>
                <asp:Button ID="btnSave" runat="server" CssClass="formItem" Text="Save" />
                <asp:Button ID="btnDelete" runat="server" CssClass="formItem delete" Text="Delete" />
                <asp:Button ID="btnCancel" runat="server" CssClass="formItem" Text="Cancel" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>