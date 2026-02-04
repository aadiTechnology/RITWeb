<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="EarningsAndDeductions.aspx.cs" Inherits="EarningsAndDeductions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="98%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummary" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
            <tr id="trMessage" runat="server" align="center">
                <td align="center" colspan="3">
                    <table align="center">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red" EnableViewState="false"></asp:Label>
                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Font-Bold="True" ForeColor="Blue"
                                    EnableViewState="False" Visible="false"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr style="height: 10px">
                <td>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <asp:Label ID="lblEarnings" runat="server" Text="Earnings" EnableViewState="False"
                        Font-Bold="True"></asp:Label>
                </td>
                <td width="10px">
                </td>
                <td align="center">
                    <asp:Label ID="Label1" runat="server" Text="Deductions" EnableViewState="false" Font-Bold="True"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td align="center" valign="top">
                    <div id="divEarnings" runat="server" style="width: 400px; height: 390px; overflow: scroll;"
                        class="GridBorder">
                        <asp:ListView ID="lstvwEarnings" runat="server" DataKeyNames="EarningsDeductionsId,OriginalEarningsDeductionsId,SchoolId,IsAttendanceDependent,IsBasic"
                            OnItemDataBound="lstvwEarnings_ItemDataBound">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="30px">
                                            <asp:CheckBox ID="ChkAll" runat="server" onclick="CheckAllUncheckAllsEarningDeductions(_clientlstvwEarningsChkAll,_clientlstvwEarnings,'_ChkSelect')" />
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Earning
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Short Name
                                        </th>
                                        <th align="center" width="200px">
                                            Is Attendance Dependent?
                                        </th>
                                        <th align="center" width="200px">
                                            Include In Salary Difference
                                        </th>
                                        <th align="center" width="100px">
                                            Formula/Range
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsName" runat="server" MaxLength="50" Text='<%#Eval("EarningsDeductionsName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsShortName" runat="server" MaxLength="30" Text='<%#Eval("ShortName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="ChkIsAttendanceDependent" runat="server" Width="200px" />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIncludeInSalaryDifference" runat="server" Width="200px" Checked='<%#Convert.ToBoolean(Eval("IncludeInSalaryDifference")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkbtnEditFormula" runat="server" Text="Add Formula" CommandName="FORMULA"
                                            Width="100px" ToolTip="Formula" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsName" runat="server" MaxLength="50" Text='<%#Eval("EarningsDeductionsName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsShortName" runat="server" MaxLength="30" Text='<%#Eval("ShortName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="ChkIsAttendanceDependent" runat="server" Width="200px" />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIncludeInSalaryDifference" runat="server" Width="200px" Checked='<%#Convert.ToBoolean(Eval("IncludeInSalaryDifference")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkbtnEditFormula" runat="server" Text="Add Formula" CommandName="FORMULA"
                                            ToolTip="Formula" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </div>
                    <asp:CustomValidator ID="cstvalEarnings" runat="server" ClientValidationFunction="ValidateEarningsDeductions"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstvalDeductions" runat="server" ClientValidationFunction="ValidateEarningsDeductions"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstDuplicateTextEarnings" runat="server" ClientValidationFunction="DuplicateValue"
                        SetFocusOnError="True" Display="None" ErrorMessage="You have entered duplicate earnings or its short name."></asp:CustomValidator>
                    <asp:CustomValidator ID="cstDuplicateTextDeduction" runat="server" ClientValidationFunction="DuplicateValue"
                        SetFocusOnError="True" Display="None" ErrorMessage="You have entered duplicate deduction or its short name."></asp:CustomValidator>
                    <asp:CustomValidator ID="cstCheckDuplicateInEarningAndDeductionGrid" runat="server"
                        ClientValidationFunction="IsDuplicateInGrid" SetFocusOnError="True" Display="None"
                        ErrorMessage="You have entered same name for Earning and Deduction."></asp:CustomValidator>
                </td>
                <td width="10px">
                </td>
                <td align="center" valign="top">
                    <div id="divDeductions" runat="server" style="width: 400px; height: 390px; overflow: scroll;"
                        class="GridBorder">
                        <asp:ListView ID="lstvwDeductions" runat="server" DataKeyNames="EarningsDeductionsId,OriginalEarningsDeductionsId,SchoolId,IsAttendanceDependent,IsBasic"
                            OnItemDataBound="lstvwDeductions_ItemDataBound">
                            <LayoutTemplate>
                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                    cellspacing="1" class="GridBorder">
                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                        <th align="center" width="30px">
                                            <asp:CheckBox ID="ChkAll" runat="server" onclick="CheckAllUncheckAllsEarningDeductions(_clientlstvwDeductionsChkAll ,_clientlstvwDeductions,'_ChkSelect')" />
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Deduction
                                        </th>
                                        <th align="left" style="padding-left: 10px;">
                                            Short Name
                                        </th>
                                        <th align="center" width="200px">
                                            Is Attendance Dependent?
                                        </th>
                                        <th align="center" width="200px">
                                            Include In Salary Difference
                                        </th>
                                        <th align="center">
                                            Formula/Range
                                        </th>
                                    </tr>
                                    <tr id="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsName" runat="server" MaxLength="50" Text='<%#Eval("EarningsDeductionsName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsShortName" runat="server" MaxLength="30" Text='<%#Eval("ShortName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="ChkIsAttendanceDependent" runat="server" Width="200px" />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIncludeInSalaryDifference" runat="server" Width="200px" Checked='<%#Convert.ToBoolean(Eval("IncludeInSalaryDifference")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkbtnEditFormula" runat="server" Text="Add Formula" CommandName="FORMULA"
                                            Width="100px" ToolTip="Formula" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                    <td align="center">
                                        <asp:CheckBox ID="ChkSelect" runat="server" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsName" runat="server" MaxLength="50" Text='<%#Eval("EarningsDeductionsName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEarningsDeductionsShortName" runat="server" MaxLength="30" Text='<%#Eval("ShortName") %>' onkeypress="return blockNonAlphabates (this, event);" onpaste="event.returnValue=false"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="ChkIsAttendanceDependent" runat="server" Width="200px" />
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIncludeInSalaryDifference" runat="server" Width="200px" Checked='<%#Convert.ToBoolean(Eval("IncludeInSalaryDifference")) %>' />
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="lnkbtnEditFormula" runat="server" Text="Add Formula" CommandName="FORMULA"
                                            ToolTip="Formula" />
                                    </td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:ListView>
                    </div>
                </td>
            </tr>
            <tr style="height: 20px;">
                <td colspan="3">
                </td>
            </tr>
            <tr align="center">
                <td align="center" colspan="3">
                    <asp:Button ID="BtnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        UseSubmitBehavior="false" OnClick="BtnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="False" UseSubmitBehavior="false" />                    
                    <asp:HiddenField ID="hidSaveWithDependancy" runat="server" Value="N" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
        _clientSaveId = "<%=this.BtnSave.ClientID %>"
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>"
        _clientlstvwEarnings = "<%=this.lstvwEarnings.ClientID %>"
        _clientlstvwDeductions = "<%=this.lstvwDeductions.ClientID %>"
        _ClientcstvalEarnings = "<%=this.cstvalEarnings.ClientID %>"
        _ClientcstvalDeductions = "<%=this.cstvalDeductions.ClientID %>"
        _ClientcstDuplicateTextEarnings = "<%=this.cstDuplicateTextEarnings.ClientID %>"
        _ClientcstDuplicateTextDeduction = "<%=this.cstDuplicateTextDeduction.ClientID %>"
        _clienthidSaveWithDependancy = "<%=this.hidSaveWithDependancy.ClientID %>"
        _clientlstvwDeductionsChkAll = _clientlstvwDeductions + '_ChkAll';
        _clientlstvwEarningsChkAll = _clientlstvwEarnings + '_ChkAll';
        _clienttrMessage = "<%=this.trMessage.ClientID %>"

        function DisableButtons(objBtn) {
            document.getElementById(_clientSaveId).disabled = true
            document.getElementById(_clientbtnCancelId).disabled = true
            __doPostBack(objBtn.name, '')
        }

        function CheckSelectedEarningDeductions() {
            var bResult = true
            if (IsAtleastOneChecked(_clientlstvwEarnings, 'ChkSelect') &&
                                IsAtleastOneChecked(_clientlstvwDeductions, 'ChkSelect')) {
                bResult = true
                if (typeof (Page_ClientValidate) == 'function') {
                    bResult = Page_ClientValidate()
                }
                if (ConfirmDelete(_clientlstvwEarnings) ||
                                ConfirmDelete(_clientlstvwDeductions)) {
                    if (!window.confirm('Are you sure you want to delete currently unchecked earnings and deductions?'))
                        bResult = false
                    else
                        bResult = true
                }
                else { }
                if (bResult) {
                    document.getElementById(_clientSaveId).disabled = true
                    document.getElementById(_clientbtnCancelId).disabled = true
                }
            }
            else {
                alert("At least one deduction should be added.")
                if (document.getElementById(_clienttrMessage) != null)
                    document.getElementById(_clienttrMessage).style.display = "none"
                bResult = false
            }
            return bResult
        }

        function IsAtleastOneChecked(listview, itemName) {            
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(listview + "_ctrl" + iRowCount +"_"+ itemName)
            else
                chk = document.getElementById(listview + "_ctrl" + iRowCount + "_" + itemName)
            while (chk != null) {
                if (chk.checked)
                    return true;
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + "_" + itemName)
                else
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + "_" + itemName)
            }
            return false;
        }

        function CheckAllUncheckAllsEarningDeductions(HeaderCheckboxe, listview, itemName) {
            var checkAll = document.getElementById(HeaderCheckboxe).checked;
            var chk
            var iRowCount = 0
            if (iRowCount < 10)
                chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
            else
                chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
            while (chk != null) {
                if (chk.disabled == false)
                    chk.checked = checkAll
                iRowCount = iRowCount + 1
                if (iRowCount < 10)
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
                else
                    chk = document.getElementById(listview + "_ctrl" + iRowCount + itemName)
            }
        }

        function VisibleFormulaLink(listview, RowId) {
            var listviewName
            if (listview == "Earning")
                listviewName = _clientlstvwEarnings
            else
                listviewName = _clientlstvwDeductions
            var chkSelect = document.getElementById(listviewName + "_ctrl" + RowId + "_ChkSelect")
            if (chkSelect.checked == true) {
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_lnkbtnEditFormula") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_lnkbtnEditFormula").disabled = false
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_ChkIsAttendanceDependent") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_ChkIsAttendanceDependent").disabled = false
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_chkIncludeInSalaryDifference") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_chkIncludeInSalaryDifference").disabled = false
            }
            else {
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_lnkbtnEditFormula") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_lnkbtnEditFormula").disabled = true
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_ChkIsAttendanceDependent") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_ChkIsAttendanceDependent").disabled = true
                if (document.getElementById(listviewName + "_ctrl" + RowId + "_chkIncludeInSalaryDifference") != null)
                    document.getElementById(listviewName + "_ctrl" + RowId + "_chkIncludeInSalaryDifference").disabled = true
            }
        }

        function OpenFormulaPopup(sEncryptedString, RowId, listview) {
            var listviewName
            if (listview == "Earning")
                listviewName = _clientlstvwEarnings
            else
                listviewName = _clientlstvwDeductions
            var editLink = document.getElementById(listviewName + "_ctrl" + RowId + "_lnkbtnEditFormula")
            if (editLink.disabled != true) {
                window.open('EarningAndDeductionFormula.aspx?' +
                            sEncryptedString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=650')
            }
            return false
        }

        function ValidateEarningsDeductions(oSrc, args) {
            var chk
            var sMessage = false
            var iRowCount = 0
            if (oSrc.id == _ClientcstvalEarnings)
                listview = _clientlstvwEarnings
            else
                listview = _clientlstvwDeductions
            chk = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtEarnings = document.getElementById(listview + "_ctrl" + iRowCount + "_txtEarningsDeductionsName")
                    txtShortName = document.getElementById(listview + "_ctrl" + iRowCount + "_txtEarningsDeductionsShortName")
                    if (txtEarnings.value.trim() == "" || txtShortName.value.trim() == "")
                        sMessage = true
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkSelect")
            }
            if (sMessage == true) {
                if (listview == _clientlstvwEarnings)
                    $get(_ClientcstvalEarnings).errormessage = "Selected earning and short name should not be blank."
                else
                    $get(_ClientcstvalDeductions).errormessage = "Selected deduction and short name should not be blank."
                if (document.getElementById(_clienttrMessage) != null)
                    document.getElementById(_clienttrMessage).style.display = "none"
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function DuplicateValue(oSrc, args) {
            if (oSrc.id == _ClientcstDuplicateTextEarnings) {
                listview = _clientlstvwEarnings
                if (DuplicateText(document, listview, "_ChkSelect", "_txtEarningsDeductionsName")) {
                    if (document.getElementById(_clienttrMessage) != null)
                        document.getElementById(_clienttrMessage).style.display = "none"
                    args.IsValid = false
                    return true
                }
                else {
                    if (DuplicateText(document, listview, "_ChkSelect", "_txtEarningsDeductionsShortName")) {
                        if (document.getElementById(_clienttrMessage) != null)
                            document.getElementById(_clienttrMessage).style.display = "none"
                        args.IsValid = false
                        return true
                    }
                    else {
                        args.IsValid = true
                        return false
                    }
                }
            }
            else {
                listview = _clientlstvwDeductions
                if (DuplicateText(document, listview, "_ChkSelect", "_txtEarningsDeductionsName")) {
                    if (document.getElementById(_clienttrMessage) != null)
                        document.getElementById(_clienttrMessage).style.display = "none"
                    args.IsValid = false
                    return true
                }
                else {
                    if (DuplicateText(document, listview, "_ChkSelect", "_txtEarningsDeductionsShortName")) {
                        if (document.getElementById(_clienttrMessage) != null)
                            document.getElementById(_clienttrMessage).style.display = "none"
                        args.IsValid = false
                        return true
                    }
                    else {
                        args.IsValid = true
                        return false
                    }
                }
            }
        }

        function IsDuplicateInGrid(oSrc, args) {
            var chk
            var sMessage = false
            var iRowCountEarnings = 0
            chk = document.getElementById(_clientlstvwEarnings + "_ctrl" + iRowCountEarnings + "_ChkSelect")
            while (chk != null) {
                if (chk.checked == true) {
                    txtEarnings = document.getElementById(_clientlstvwEarnings + "_ctrl" + iRowCountEarnings + "_txtEarningsDeductionsName")
                    txtEarningsShortName = document.getElementById(_clientlstvwEarnings + "_ctrl" + iRowCountEarnings + "_txtEarningsDeductionsShortName")
                    txtEarningsText = txtEarnings.value
                    txtEarningsShortNameText = txtEarningsShortName.value
                    var iRowCountDeduction = 0
                    var chkDeduction
                    chkDeduction = document.getElementById(_clientlstvwDeductions + "_ctrl" + iRowCountDeduction + "_ChkSelect")
                    while (chkDeduction != null) {
                        if (chkDeduction.checked == true) {
                            txtDeduction = document.getElementById(_clientlstvwDeductions + "_ctrl" + iRowCountDeduction + "_txtEarningsDeductionsName")
                            txtDeductionShortName = document.getElementById(_clientlstvwDeductions + "_ctrl" + iRowCountDeduction + "_txtEarningsDeductionsShortName")
                            txtDeductionText = txtDeduction.value
                            txtDeductionShortNameText = txtDeductionShortName.value
                            if (txtEarningsText.toUpperCase() == txtDeductionText.toUpperCase() || txtEarningsText.toUpperCase() == txtDeductionShortNameText.toUpperCase() ||
                                txtEarningsShortNameText.toUpperCase() == txtDeductionText.toUpperCase() || txtEarningsShortNameText.toUpperCase() == txtDeductionShortNameText.toUpperCase())
                                sMessage = true
                        }
                        iRowCountDeduction = iRowCountDeduction + 1
                        chkDeduction = document.getElementById(_clientlstvwDeductions + "_ctrl" + iRowCountDeduction + "_ChkSelect")
                    }
                }
                iRowCountEarnings = iRowCountEarnings + 1
                chk = document.getElementById(_clientlstvwEarnings + "_ctrl" + iRowCountEarnings + "_ChkSelect")
            }
            if (sMessage == true) {
                document.getElementById(_clienttrMessage).style.display = "none"
                args.IsValid = false
                return true
            }
            else {
                document.getElementById(_clienttrMessage).style.display = "none"
                args.IsValid = true
                return false
            }
        }

        function ConfirmDelete(listview) {
            var sDeleteMessage = false
            var sInsertMessage = false
            var bResult = true
            var sMsg = ""
            var chk
            var ChkIsAttendanceDependent
            var iRowCount = 0
            chk = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkSelect")
            ChkIsAttendanceDependent = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkIsAttendanceDependent")
            while (chk != null) {
                if (chk.checked != true && ChkIsAttendanceDependent != null) {
                    sDeleteMessage = true
                    break
                }
                iRowCount = iRowCount + 1
                chk = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkSelect")
                ChkIsAttendanceDependent = document.getElementById(listview + "_ctrl" + iRowCount + "_ChkIsAttendanceDependent")
            }
            return sDeleteMessage
        }
        function OnGridKeyUp(obj, e) {
            UpDownKeyPress(obj.id, e);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
