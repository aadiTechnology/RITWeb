<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ConfigureLeaveTypesUI.aspx.cs" Inherits="ConfigureLeaveTypesUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummLeaves" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" width="100%">
                        <tr>
                            <td align="right" width="50%" id="trErrorMessage" runat="server" visible="false">
                                <div style="float: right;">
                                    <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsLabel" ForeColor="Red"
                                        EnableViewState="False"></asp:Label>
                                </div>
                            </td>
                            <td align="right">
                                <table id="tblBasicLeaves" runat="server">
                                    <tr>
                                        <td align="right" style="height: 25px" class="ClsGreenBG">
                                            <asp:LinkButton ID="lnkUserLeaves" runat="server" Text="Basic Leave Configuration" CssClass="SubTitle"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <table align="center">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Font-Bold="True" Text=""
                                    ForeColor="Blue" EnableViewState="False" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <div id="Div1" style="width: 80%; overflow: auto;">
                        <asp:ListView ID="lstvwStaffLeaves" runat="server" DataKeyNames="LeaveId,OriginalLeaveId,SchoolId,CanAccumulate,IsUnpaidLeave,CurrentYearAccumulated,AccumulateLeaves,ExcludeFromDeduction,AllowZeroBalance,IsODApplicable"
                            OnItemDataBound="lstvwStaffLeaves_ItemDataBound">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="10px">
                                            <asp:CheckBox ID="ChkSelectAll" runat="server" onclick="CheckAllUncheckAlls()" />
                                        </th>
                                        <th align="left" style="padding-left: 5px; width: 200px">
                                            Leave Type
                                        </th>
                                        <th align="left" style="padding-left: 5px; width: 200px">
                                            Short Name
                                        </th>                                        
                                        <th align="left" style="padding-left: 5px;">
                                            Minimum Balance
                                        </th>
                                        <th align="center">
                                            Color
                                        </th>
                                        <th align="center" style="padding-left: 5px;">
                                            Exclude From Deduction
                                        </th>
                                        <th align="center" style="padding-left: 5px;">
                                            Allow Zero Balance?
                                        </th>
                                        <th align="center" style="padding-left: 5px;">
                                            Is Unpaid Leave?
                                        </th>
                                        <th align="center" style="padding-left: 5px;">
                                            Consider On Duty?
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
                                        <asp:TextBox Width="100px" ID="txtLeave" runat="server" MaxLength="50" Text='<%#Eval("LeaveName") %>'
                                            onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtShortName" Width="100px" runat="server" MaxLength="50" Text='<%#Eval("ShortName") %>'
                                            onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>                                   
                                    <td align="left" class="paddingL">
                                        <asp:TextBox CssClass="SmlTxtBox" ID="txtMinimumBalance" runat="server" Text='<%#Eval("MinimumBalance") %>'
                                            onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"
                                            onpaste="event.returnValue=false" MaxLength="5"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="cmbColorCode" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkApplicabletostaffholiday" runat="server" />
                                        <asp:HiddenField ID="hidIsUnpaidLeave" runat="server" Value='<%#Eval("IsUnpaidLeave") %>' />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkAllowZeroBalance" runat="server" />                                        
                                    </td>
                                     <td align="center">
                                        <asp:CheckBox ID="ChkIsUnpaidLeave" runat="server" />                                        
                                    </td>
                                     <td align="center">
                                        <asp:CheckBox ID="ChkConsiderOnDuty" runat="server"/>                                        
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox Width="100px" ID="txtLeave" runat="server" MaxLength="50" Text='<%#Eval("LeaveName") %>'
                                            onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td class="paddingL">
                                        <asp:TextBox ID="txtShortName" Width="100px" runat="server" MaxLength="50" Text='<%#Eval("ShortName") %>'
                                            onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>                                    
                                    <td align="left" class="paddingL">
                                        <asp:TextBox CssClass="SmlTxtBox" ID="txtMinimumBalance" runat="server" Text='<%#Eval("MinimumBalance") %>'
                                            onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                            onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"
                                            onpaste="event.returnValue=false" MaxLength="4"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="cmbColorCode" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkApplicabletostaffholiday" runat="server" />
                                        <asp:HiddenField ID="hidIsUnpaidLeave" runat="server" Value='<%#Eval("IsUnpaidLeave") %>' />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkAllowZeroBalance" runat="server" />                                        
                                    </td>
                                     <td align="center">
                                        <asp:CheckBox ID="ChkIsUnpaidLeave" runat="server" />   
                                                                     
                                    </td>
                                     <td align="center">
                                        <asp:CheckBox ID="ChkConsiderOnDuty" runat="server" />                                        
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                        <asp:CustomValidator ID="cstvalLeaves" runat="server" ClientValidationFunction="ValidateLeaves"
                            SetFocusOnError="True" Display="None" ErrorMessage="Leave Type should not be blank for selected leave type."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstValShortName" runat="server" ClientValidationFunction="ValidateShortName"
                            SetFocusOnError="True" Display="None" ErrorMessage="Short Name should not be blank for selected leave type."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalDuplicateText" runat="server" ClientValidationFunction="DuplicateValue"
                            SetFocusOnError="True" Display="None" ErrorMessage="Selected leaves or its short name should not be duplicate."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalAccumulateLeave" runat="server" ClientValidationFunction="ValidateAccumulateLeaves"
                            SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalMinimumLeaves" runat="server" ClientValidationFunction="ValidateMinimumLeaves"
                            SetFocusOnError="True" Display="None" ErrorMessage="Minimum Balance should not be blank for selected leave type."></asp:CustomValidator>
                        <asp:CustomValidator ID="cstvalDuplicateColor" runat="server" ClientValidationFunction="DuplicateColor"
                            SetFocusOnError="True" Display="None" ErrorMessage="Selected colors should not be duplicate."></asp:CustomValidator>
                        <asp:HiddenField ID="hidTxtAccumulate" runat="server" />
                        <asp:HiddenField ID="hidSavedCount" runat="server" />
                        <asp:HiddenField ID="hidTxtValue" runat="server" />
                    </div>
                </td>
            </tr>            
            <tr align="center">
                <td align="center">
                    <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" 
                        OnClick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="false" UseSubmitBehavior="false" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">

        _clientSaveId = "<%=this.BtnSave.ClientID %>"
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
        _clientLstvwStaffLeaves = "<%=this.lstvwStaffLeaves.ClientID %>"
        _ClientCstvalLeaves = "<%=this.cstvalLeaves.ClientID %>"
        _ClientValSummLeaves = "<%=this.valSummLeaves.ClientID %>"
        _ClientCstvalDuplicateText = "<%=this.cstvalDuplicateText.ClientID %>"
        _ClientcstvalAccumulateLeave = "<%=this.cstvalAccumulateLeave.ClientID %>"
        _clienttrErrorMessage = "<%=this.lblErrorMessage.ClientID %>"
        _clientMessage = "<%=this.lblMessage.ClientID %>" 
        _clienthidSaveCount = "<%=this.hidSavedCount.ClientID %>"
        _clienthidTxtValue = "<%=this.hidTxtValue.ClientID %>"

        function CheckSelectedLeaves(objBtn) {
            var bResult = true
            var errMsg = document.getElementById(_clienttrErrorMessage)
            if (errMsg != null)
                errMsg.style.display = "none"

            if (IsAtleastOneLeaveSelected()) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
            }
            else {
                $get(_ClientValSummLeaves).style.display = "none"
                alert("At least one paid leave should be selected.")
                bResult = false
            }
            return bResult
        }

        function IsAtleastOneLeaveSelected() {
            var chk, isFound = false
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                var leaveId = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
                var isUnpaid = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_hidIsUnpaidLeave").value;
                if (chk.checked && isUnpaid == "False") {
                    isFound = true;
                    break;
                }

                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }
            return isFound;
        }

        function CheckAllUncheckAlls() {
            var checkAll = document.getElementById(_clientLstvwStaffLeaves + "_ChkSelectAll").checked
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            else
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.disabled != true)
                    chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
                else
                    chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function ValidateLeaves(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtStaffLeave = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_txtLeave")
                    if (txtStaffLeave.value.trim() == "")
                        sMessage = true
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMessage == true) {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateShortName(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtShortName = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_txtShortName")
                    if (txtShortName.value.trim() == "")
                        sMessage = true
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMessage == true) {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateMinimumLeaves(aSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtMinimumBalance = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_txtMinimumBalance")
                    if (txtMinimumBalance.value.trim() == "")
                        sMessage = true
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMessage == true) {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateAccumulateLeaves(aSrc, args) {
            args.IsValid = true
            return false
        }
        function DuplicateValue(oSrc, args) {
            if (DuplicateText(document, _clientLstvwStaffLeaves, "_ChkSelect", "_txtLeave")) {
                args.IsValid = false;
                return true
            }
            else {
                if (DuplicateText(document, _clientLstvwStaffLeaves, "_ChkSelect", "_txtShortName")) {
                    args.IsValid = false
                    return true

                }
                else {
                    args.IsValid = true
                    return false
                }
            }
        }

        function DuplicateColor(oSrc, args) {
        	if (DuplicateText(document, _clientLstvwStaffLeaves, "_ChkSelect", "_cmbColorCode")) {
        		if ($get("<%=this.lblMessage.ClientID %>") != null)
        			$get("<%=this.lblMessage.ClientID %>").innerHTML = "";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        
        function ConfirmInsert() {
            var bResult = true
            var sSvaeCount = $get(_clienthidSaveCount).value
            var iCount = 0
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true)
                    iCount = iCount + 1
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientLstvwStaffLeaves + "_ctrl" + iRowCount + "_ChkSelect")
            }

            if (parseInt(sSvaeCount) < iCount) {
                if (!window.confirm('Once you add a new leave type, you cannot delete it. Are you sure you want to add new leave type(s)?'))
                    bResult = false;
            }
            return bResult
        }

        

        function GetValue(txt) {
            document.getElementById(_clienthidTxtValue).value = txt.value
        }

        function Validate(textbox) {
            var iDaysOfYear = 366
            var sMarks = textbox.value
            var iMarks = parseInt(sMarks)
            if (sMarks == "" || iMarks > iDaysOfYear) {
                textbox.value = document.getElementById(_clienthidTxtValue).value
                textbox.focus()
            }
            else {
                var floatValue = parseFloat(textbox.value)
                var intValue = parseInt(textbox.value)
                intValue = parseFloat(intValue)
                var difference = parseFloat((floatValue * 10) % 10)
                if (difference != 5 && difference != 0) {
                    if (difference > 5)
                        difference = intValue + 1
                    else
                        difference = intValue + 0.5
                    textbox.value = difference
                }
            }
        }


        function Validate(textbox) {
            var floatValue = parseFloat(textbox.value)
            var intValue = parseInt(textbox.value)
            intValue = parseFloat(intValue)
            var difference = parseFloat((floatValue * 10) % 10)

            if ((intValue + "").length > 3)
                textbox.value = "";
            else if (difference != 5 && difference != 0) {
                if (difference > 5)
                    difference = intValue + 1
                else
                    difference = intValue + 0.5
                textbox.value = difference
            }
        }

        function SetColorPayPeriod(obj) {
            if (obj.value != '0') {
                obj.style.backgroundColor = obj.value;
            }
        }

        function OpenPopup() {            
            window.open('BasicLeaveConfigPopup.aspx?', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=650').focus();            
            return false;            
        }

    </script>
</asp:Content>
