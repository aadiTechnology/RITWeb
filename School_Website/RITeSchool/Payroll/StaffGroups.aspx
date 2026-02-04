<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StaffGroups.aspx.cs" Inherits="StaffGroups"
    ValidateRequest="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummStaffGroups" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
            <tr align="center" id="trErrorMessage" runat="server" visible="false">
                <td align="center">
                    <table align="center">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                     EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <div id="Div1" style="width: 50%; overflow: auto;">
                        <asp:ListView ID="lstvwStaffGroups" runat="server" DataKeyNames="StaffGroupsId,OriginalStaffGroupsId,SchoolId"
                            OnItemDataBound="lstvwStaffGroups_ItemDataBound">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="30px">
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAllsStaffGroups()" />
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Staff Group
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="trItem" runat="server" class="ClsGridRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtStaffGroup" runat="server" MaxLength="50" Text='<%#Eval("StaffGroupsName") %>'></asp:TextBox>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtStaffGroup" runat="server" MaxLength="50" Text='<%#Eval("StaffGroupsName") %>'></asp:TextBox>
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                        <asp:CustomValidator ID="cstvalStaffGroups" runat="server" ClientValidationFunction="ValidateStaffGroups"
                            SetFocusOnError="True" Display="None" ErrorMessage="Selected staff group should not be blank."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalDuplicateValue" runat="server" ClientValidationFunction="DuplicateValue"
                            SetFocusOnError="True" Display="None" ErrorMessage="You have entered duplicate value for selected staff group."></asp:CustomValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px;">
                    &nbsp;
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        OnClick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="false" UseSubmitBehavior="false" />
                    <asp:HiddenField ID="hidSaveCount" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientSaveId = "<%=this.BtnSave.ClientID %>"
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
        _clientlstvwStaffGroups = "<%=this.lstvwStaffGroups.ClientID %>"
        _ClientcstStaffGroups = "<%=this.cstvalStaffGroups.ClientID %>"
        _ClientvalSummStaffGroups = "<%=this.valSummStaffGroups.ClientID %>"
        _ClientcstvalDuplicateValue = "<%=this.cstvalDuplicateValue.ClientID %>"
        _clienthidSaveCount = "<%=this.hidSaveCount.ClientID %>"
        _clienttrErrorMessage = "<%=this.trErrorMessage.ClientID %>"
        function DisableButtons(objBtn) {
            document.getElementById(_clientSaveId).disabled = true
            document.getElementById(_clientbtnCancelId).disabled = true
            __doPostBack(objBtn.name, '')
        }
        function CheckSelectedGroups(objBtn) {
            var errMsg = document.getElementById(_clienttrErrorMessage)
            if (errMsg != null)
                errMsg.style.display = "none"
            var bResult = true
            if (CheckSelection(_clientlstvwStaffGroups, 'ChkSelect')) {          
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                if (bResult)
                    bResult = ConfirmDelete()
                if (bResult) {
                    document.getElementById(_clientSaveId).disabled = true
                    document.getElementById(_clientbtnCancelId).disabled = true
                    __doPostBack(objBtn.name, '')
                } 
            }
            else {
                $get(_ClientvalSummStaffGroups).style.display = "none"
                alert("At least one staff group should be selected.")
                bResult = false
            }
            return bResult
        }
        function CheckAllUncheckAllsStaffGroups() {
            var checkAll = document.getElementById("ctl00_MainBody_lstvwStaffGroups_ChkSelectAll").checked
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            else
                chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
                else
                    chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            } 
        }
        function ValidateStaffGroups(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtStaffGroup = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_txtStaffGroup")
                    if (txtStaffGroup.value.trim() == "")
                        sMessage = true
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMessage == true) {
                $get(_ClientcstStaffGroups).errormessage = "Selected staff group should not be blank."
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function DuplicateValue(oSrc, args) {
            if (DuplicateText(document, _clientlstvwStaffGroups, "_ChkSelect", "_txtStaffGroup")) {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            } 
        }
        function ConfirmDelete() {
            var bResult = true
            var sSvaeCount = $get(_clienthidSaveCount).value
            var iCount = 0
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    iCount = iCount + 1
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwStaffGroups + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (parseInt(sSvaeCount) > iCount) {
                if (!window.confirm('Are you sure, you want to delete currently unchecked staff groups?'))
                    bResult = false
            }
            return bResult
        }
    </script>
</asp:Content>

