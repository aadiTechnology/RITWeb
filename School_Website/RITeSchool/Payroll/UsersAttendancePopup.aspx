<%@ Page Title="" Language="C#" MasterPageFile="../MasterPages/PopupMaster.master"
    EnableEventValidation="false" AutoEventWireup="true" CodeFile="UsersAttendancePopup.aspx.cs"
    Inherits="UsersAttendancePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td valign="top">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
                                <tr>
                                    <td>
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="height: 5px;">
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 20px; width: 99%;" class="ClsGrayMainTitle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <span style="font-weight: bold">Set Full Attendance</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="60%">
                                            <tr style="height: 5px;">
                                                <td colspan="5">
                                                </td>
                                            </tr>
                                            <tr class="ClsBorderlight">
                                                <td align="left" style="height: 19px; width:12%" class="ClsBorderlight">
                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Font-Bold = "true">Month :</asp:Label>
                                                </td>
                                                <td align="left" style="height: 19px; width:25%" class="ClsBorderlight">
                                                    <asp:Label ID="lblSalaryMonth" runat="server" CssClass="ClsLabel" EnableViewState="true"></asp:Label>
                                                </td>
                                                <td align="right" style="height: 19px; width: 2%">
                                                </td>
                                                <td align="left" style="height: 19px; width:18%" class="ClsBorderlight">
                                                    <asp:Label ID="lblStaffGroup" runat="server" CssClass="ClsLabel" Font-Bold="true">Staff Group :</asp:Label>
                                                </td>
                                                <td align="left" style="height: 19px; width:25%">
                                                    <asp:DropDownList ID="cmbStaffGroup" runat="server" Width="150px" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbStaffGroup_SelectedIndexChanged" 
                                                        Font-Names="Arial" Font-Size="9pt">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr style="height: 5px;">
                                                <td colspan="5">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblMessage" runat="server" Text="Full attendance is set successfully!!!"
                                            Visible="false" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <div id="divContainer" class="GridBorder" runat="server" visible="false" style="width: 100%;
                                            height: 370px; overflow: scroll">
                                            <asp:ListView ID="lstvwUsers" runat="server" DataKeyNames="StaffAttendanceId,UserId"
                                                OnItemDataBound="lstvwUsers_ItemDataBound">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                            <th align="center" width="30px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAllUncheckAllsStaffGroups()" />
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;" width="40%">
                                                                Staff Name
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;" width="26%">
                                                                Designation
                                                            </th>
                                                            <th align="left" style="padding-left: 10px;" width="30%">
                                                                Used Leaves
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblStffName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblUsedLeaves" runat="server" CssClass="ClsLabel" Text="-"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblStffName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Name") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsLabel" Text='<%#Eval("Designation") %>'></asp:Label>
                                                        </td>
                                                        <td class="paddingL">
                                                            <asp:Label ID="lblUsedLeaves" runat="server" CssClass="ClsLabel" Text="-"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                            <asp:HiddenField ID="hidStaffGroup" runat="server" Value="0" />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table width="100%" id="tblNote" runat="server">
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note1 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="lblNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Staff already having full attendance for selected month are not displayed in the list."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight " style="width: 5%; background-color: #ffffc4;">
                                                    <asp:Label ID="Label2" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note2 :"
                                                        CssClass="LblNrmlB"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
                                                    <asp:Label ID="Label3" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="Attendance cannot be set to month for which salary has been published."></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trNoRecordFound" runat="server" visible="false" class="LblNoRecord" width="100%">
                                    <td align="center">
                                        <asp:Label ID="lblNoRecordFound" runat="server" Text="No record found."></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="20%">
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" Text="Save" OnClick="btnSave_Click" />
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStaffGroup" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td align="center">
                                <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" Text="Close" OnClick="btnClose_Click"
                                    CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                    <asp:HiddenField ID="hidIsStaticOutput" runat="server" Value="N" />
                    <asp:HiddenField ID="hidFilter" runat="server" Value="" />                    
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientlstvwUsers = "<%=this.lstvwUsers.ClientID %>";
        _clientcmbStaffGroup = "<%=this.cmbStaffGroup.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";


        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            DisableControls(true)
        }
        function beginRequestHandler(sender, args) {
            DisableControls(false)
        }

        function DisableControls(flag) {            
            if (document.getElementById(_clientcmbStaffGroup)!=null)
                document.getElementById(_clientcmbStaffGroup).disabled = !flag;

            if (document.getElementById(_clientbtnSave)!=null)
                document.getElementById(_clientbtnSave).disabled = !flag;

            if (document.getElementById(_clientbtnClose)!=null)
                document.getElementById(_clientbtnClose).disabled = !flag;
        }

        function CheckSelectedGroups() {
            var bResult = true
            if (CheckSelection(_clientlstvwUsers, '_chkSelect')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                
                bResult = CheckStaffGroupSelection();
            }
            else {
                alert("At least one staff should be selected.")
                bResult = false
            }
            return bResult
        }

        function CheckStaffGroupSelection() {            
            var chk
            var iRowCount = 0
            var found = false;
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
            else
                chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                if (!chk.checked) {
                    found = true;
                    break;
                }
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
                else
                    chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
                iRowCount++;
            }
            if (!found)
                return confirm('Are you sure you want to set full attendance for all users?')
            return true;
        }

        function CheckAllUncheckAllsStaffGroups() {
            var checkAll = document.getElementById(_clientlstvwUsers + "_chkSelectAll").checked
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
            else
                chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
                else
                    chk = document.getElementById(_clientlstvwUsers + "_ctrl" + iRowCount + "_chkSelect")
            }
        }
    </script>
</asp:Content>
