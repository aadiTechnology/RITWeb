<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="LateMarkConfigurationUI.aspx.cs" Inherits="LateMarkConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr align="center" id="trValSummary" runat="server">
                <td align="center">
                    <asp:ValidationSummary ID="valSummary" CssClass="LblErrorMsg" ShowSummary="true"
                        runat="server" />
                </td>
            </tr>
            <tr id="trErrorMessage" runat="server" align="center">
                <td align="center" colspan="3">
                    <table align="center">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorMessage" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                    EnableViewState="False"></asp:Label>
                                <asp:Label ID="lblMessage" runat="server" CssClass="ClsLabel" Font-Bold="True" Text=""
                                    ForeColor="Blue" EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr><td style="height: 28px">&nbsp;&nbsp;</td></tr>--%>
                    </table>
                </td>
            </tr>
            <tr>
                <td id="tdLateMarkConfig" runat="server" align="center" colspan="2">
                    <asp:ListView ID="lstvwLateMarkConfiguration" runat="server" DataKeyNames="LateMarkConfigurationId,Is_Deleted"
                        OnItemDataBound="lstvwLateMarkConfiguration_ItemDataBound">
                        <LayoutTemplate>
                            <table width="60%" runat="server" id="tblLateMarkConfig" style="color: #333333" cellpadding="0"
                                cellspacing="1" class="GridBorder">
                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                    <th align="center" width="30px">
                                    </th>
                                    <th align="center" style="padding-left: 10px;">
                                        Sr. No
                                    </th>
                                    <th align="left" style="padding-left: 10px;">
                                        Late Mark Count
                                    </th>
                                    <th align="left" style="padding-left: 10px;">
                                        Considered Leaves
                                    </th>
                                    <th align="center" style="padding-left: 10px">
                                        Sort Order
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="Tr21" runat="server" class="ClsGridRow">
                                <td align="center">
                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblRowNo" runat="server" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:TextBox ID="txtLateMarkCount" runat="server" MaxLength="2" Text='<%#Eval("LateMarkCount") %>'
                                        onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" 
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:TextBox ID="txtConsideredLeaves" runat="server" MaxLength="4" Text='<%#Eval("ConsideredLeaves") %>'
                                        onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                </td>
                                <td align="center" style="padding-left: 10px;">
                                    <asp:DropDownList ID="cmbSortOrder" runat="server" AppendDataBoundItems="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                <td align="center">
                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblRowNo" runat="server" EnableViewState="false"></asp:Label>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:TextBox ID="txtLateMarkCount" runat="server" MaxLength="2" Text='<%#Eval("LateMarkCount") %>'
                                        onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:TextBox ID="txtConsideredLeaves" runat="server" MaxLength="4" Text='<%#Eval("ConsideredLeaves") %>'
                                        onchange="Validate(this)" onblur="extractNumber(this,1,false);" ondrop="event.returnValue=false"
                                        onkeypress="return blockNonNumbers(this, event, true, false);" onkeyup="extractNumber(this,1,false);"
                                        onpaste="event.returnValue=false"></asp:TextBox>
                                </td>
                                <td align="center" style="padding-left: 10px;">
                                    <asp:DropDownList ID="cmbSortOrder" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td id="tdlstvwLeaveSortOrder" runat="server" align="center" colspan="2">
                    <asp:ListView runat="server" ID="lstvwLeaveSortOrder" DataKeyNames="StaffLeaveSortOrderId,LeaveId"
                        OnItemDataBound="lstvwLeaveSortOrder_ItemDataBound">
                        <LayoutTemplate>
                            <table width="30%" runat="server" id="tblStaffLeaveSortOrder" style="color: #333333"
                                cellpadding="0" cellspacing="1" class="GridBorder">
                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                    <th align="left" style="padding-left: 10px;">
                                        Staff Leave
                                    </th>
                                    <th align="center" style="padding-left: 10px">
                                        Sort Order
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                <td style="padding-left: 10px;">
                                    <asp:Label ID="lblStaffLeaves" runat="server" Text='<%#Eval("ShortName") %>' EnableViewState="false"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="cmbStaffLeaveSortOrder" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                <td style="padding-left: 10px;">
                                    <asp:Label ID="lblStaffLeaves" runat="server" Text='<%#Eval("ShortName") %>' EnableViewState="false"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="cmbStaffLeaveSortOrder" runat="server">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnSave" Text="Save" runat="server" CssClass="ClsBtn" BorderWidth="1px" 
                        OnClick="btnSave_Click" EnableTheming="True" />
                    &nbsp;<asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="ClsBtn" BorderWidth="1px"
                        CausesValidation="False" UseSubmitBehavior="False" />
                    <asp:HiddenField ID="hidIsConfigured" runat="server" Value="N" />
                    <asp:HiddenField ID="hidLeaveSortOrderSaveCount" runat="server" />
                    <asp:HiddenField ID="hidLateNarkConfigSaveCount" runat="server" />
                    <asp:HiddenField ID="hidConsideredLeaves" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:CustomValidator ID="cstLateMarkCount" runat="server" ClientValidationFunction="ValidateLateMarkCount"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstDuplicateSortOrder" runat="server" ClientValidationFunction="ValidateDuplicateSortOrder"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                </td>
                <asp:CustomValidator ID="cstSortOrder" runat="server" ClientValidationFunction="ValidateSortOrder"
                    SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                <td>
                    <asp:CustomValidator ID="cstStaffLeaveSortOrder" runat="server" ClientValidationFunction="ValidateStaffLeaveSortOrder"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                </td>
            </tr>
        </table>
    </div>

    <script type="text/javascript" language="javascript">
        _clientcstSortOrder = "<%=this.cstSortOrder.ClientID %>";
        _clientcstDuplicateSortOrder = "<%=this.cstDuplicateSortOrder.ClientID %>";
        _clientcstLateMarkCount = "<%=this.cstLateMarkCount.ClientID %>";
        _clientcstLateMarkCount = "<%=this.cstLateMarkCount.ClientID %>";
        _clientcstSaffLeaveSortOrder = "<%=this.cstStaffLeaveSortOrder.ClientID %>";
        _clientSaveId = "<%=this.btnSave.ClientID %>";
        _clientbtnCancelId = "<%=this.btnCancel.ClientID %>";
        _clientlstvwLateMarkConfiguration = "<%=this.lstvwLateMarkConfiguration.ClientID %>";
        _clientlstvwLeaveSortOrder = "<%=this.lstvwLeaveSortOrder.ClientID %>";        
        _ClientlblMessage = "<%=this.lblMessage.ClientID %>";
        _ClientvalSummStaffGroups = "<%=this.valSummary.ClientID %>"
        _ClienthidLateMarkConfigSaveCount = "<%=this.hidLateNarkConfigSaveCount.ClientID %>";
        _ClienthidLeaveSortOrderSaveCount = "<%=this.hidLeaveSortOrderSaveCount.ClientID %>";
        _ClienthidConsideredLeaves = "<%=this.hidConsideredLeaves.ClientID %>";

        function GetValue(txt) {
            document.getElementById(_ClienthidConsideredLeaves).value = txt.value;
        }
        function Validate(textbox, MaxVal) {
            var iRowCount = 0;
            var sValue = textbox.value
            var iValue = parseInt(sValue);
            if (sValue == "") {
                textbox.value = document.getElementById(_ClienthidConsideredLeaves).value;
                textbox.focus();
            }
            else {
                var floatValue = parseFloat(textbox.value);
                var intValue = parseInt(textbox.value);
                intValue = parseFloat(intValue)
                var difference = parseFloat((floatValue * 10) % 10);
                if (difference != 5 && difference != 0) {
                    if (difference > 5)
                        difference = intValue + 1;
                    else
                        difference = intValue + 0.5;
                    textbox.value = difference;
                }
            }
        }



        function DisableButtons(objBtn) {
            if (document.getElementById(_clientSaveId) != null)
                document.getElementById(_clientSaveId).disabled = true;
            document.getElementById(_clientbtnCancelId).disabled = true;
            __doPostBack(objBtn.name, '');
        }

        function CheckSelectedGroups(objBtn) {

            //******** At least one CheckBox should be selected.**********/


            if (CheckSelection(_clientlstvwLateMarkConfiguration, '_ChkSelect')) {
                bResult = true;

                if (typeof (Page_ClientValidate) == 'function')
                    bResult = Page_ClientValidate();
            }
            else {
                $get(_ClientvalSummStaffGroups).style.display = "none"
                alert("At least one Late Mark Configuration should be selected.")
                bResult = false;
            }


            return bResult;
        }

        function ValidateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sortOrders = "";
            var notSelected = true;
            var isDuplicate = false;
            var sCount = "";
            var sCnt = "";
            chk = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + iRowCount + "_ChkSelect");
            cmb = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + iRowCount + "_cmbSortOrder");
            document.getElementById(_clientcstSortOrder).errormessage = "";
            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value == "0") {
                        notSelected = false;
                        if (sCount != "")
                            sCount = sCount + ", " + (iRowCount + 1);
                        else
                            sCount = (iRowCount + 1);                        
                    }
                    else {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);                            
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }
                }
                else {
                    cmb.value = "0";
                }
                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount) + "_ChkSelect")
                cmb = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }
            if (!notSelected) {
                document.getElementById(_clientcstSortOrder).errormessage = "Late Mark sort order should be select for row : " + (sCount) + ".";
                document.getElementById(_clientcstSortOrder).innerHTML = "Late Mark sort order should be select for row : " + (sCount) + ".";
                args.IsValid = false;

            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }

        function ValidateStaffLeaveSortOrder(oSrc, args) {

            var iRowCount = 0;
            var sortOrders = "";
            var hidLeaveSortOrderSaveCount = document.getElementById(_ClienthidLeaveSortOrderSaveCount).value;
            var isDuplicate = false;

            cmb = document.getElementById(_clientlstvwLeaveSortOrder + "_ctrl" + iRowCount + "_cmbStaffLeaveSortOrder");

            while (hidLeaveSortOrderSaveCount > 0) {
                if (sortOrders.match("," + cmb.value + ",") != null) {
                    isDuplicate = true;                    
                    break;
                }
                else {
                    if (cmb.value != "9999")
                        sortOrders = sortOrders + "," + cmb.value + ",";
                }
                hidLeaveSortOrderSaveCount = hidLeaveSortOrderSaveCount - 1;
                iRowCount = iRowCount + 1;
                cmb = document.getElementById(_clientlstvwLeaveSortOrder + "_ctrl" + iRowCount + "_cmbStaffLeaveSortOrder");

            }
            if (isDuplicate) {
                document.getElementById(_clientcstSaffLeaveSortOrder).errormessage = "Staff leave sort order should not be duplicate. ";
                document.getElementById(_clientcstSaffLeaveSortOrder).innerHTML = "Staff leave sort order should not be duplicate.";
                args.IsValid = false;
                return true;

                args.IsValid = true;
                return false;
            }
            if (sortOrders == "") {
                document.getElementById(_clientcstSaffLeaveSortOrder).errormessage = "At least one Staff leave sort order should be selected. ";
                document.getElementById(_clientcstSaffLeaveSortOrder).innerHTML = "At least one Staff leave sort order should be selected.";
                args.IsValid = false;

            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }
        function ValidateLateMarkCount(oSrc, args) {

            var sCount = "";
            //********Validate LateMarkCount if chechbox is checked LateMarlCount should not be Zero**********/
            if (bResult) {
                var iRowCount = document.getElementById(_ClienthidLateMarkConfigSaveCount).value;
                while (iRowCount) {
                    var chkSelect = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount - 1) + "_ChkSelect");
                    if (chkSelect.checked == true) {
                        var iLateMarkCountValue = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount - 1) + "_txtLateMarkCount");
                        if (iLateMarkCountValue.value == 0) {
                            if (sCount != "")
                                sCount = (iRowCount) + ", " + sCount;
                            else
                                sCount = (iRowCount);
                            $get(_clientcstLateMarkCount).errormessage = "Late Mark Count should not be zero for row : " + sCount + ".";
                            document.getElementById(_clientcstLateMarkCount).innerHTML = "Late Mark Count should not be zero for row : " + sCount + ".";
                            args.IsValid = false;
                        }
                    }
                    iRowCount = iRowCount - 1;
                }
                if (args.IsValid == false)
                    return true;

                if (args.IsValid == true)
                    return false;

            }

        }
        function ValidateDuplicateSortOrder(oSrc, args) {
            var iRowCount = 0;
            var sortOrders = "";
            var isDuplicate = false;

            var sCnt = "";
            chk = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + iRowCount + "_ChkSelect");
            cmb = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + iRowCount + "_cmbSortOrder");

            while (chk != null) {
                if (chk.checked == true) {
                    if (cmb.value != 0) {
                        if (sortOrders.match("," + cmb.value + ",") != null) {
                            isDuplicate = true;
                            if (sCnt != "")
                                sCnt = sCnt + ", " + (iRowCount + 1);
                            else
                                sCnt = (iRowCount + 1);                            
                        }
                        else {
                            if (cmb.value != "9999")
                                sortOrders = sortOrders + "," + cmb.value + ",";
                        }
                    }

                }

                iRowCount = iRowCount + 1;
                chk = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount) + "_ChkSelect")
                cmb = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + (iRowCount) + "_cmbSortOrder");
            }
            if (isDuplicate) {
                document.getElementById(_clientcstDuplicateSortOrder).errormessage = "Late Mark sort order should not be duplicate for row : " + (sCnt) + ".";
                document.getElementById(_clientcstDuplicateSortOrder).innerHTML = "Late Mark sort order should not be duplicate for row : " + (sCnt) + ".";
                args.IsValid = false;
            }
            if (args.IsValid == false)
                return true;

            if (args.IsValid == true)
                return false;
        }

        function ResetFields(rowIndex) {
            var chk = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + rowIndex + "_ChkSelect");
            var leaveCount = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + rowIndex + "_txtLateMarkCount");
            var consideredLeaves = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + rowIndex + "_txtConsideredLeaves");
            var cmb = document.getElementById(_clientlstvwLateMarkConfiguration + "_ctrl" + rowIndex + "_cmbSortOrder");

            if (chk.checked == false) {
                leaveCount.value = 0;
                consideredLeaves.value = 0;
                cmb.value = 0;
            }
        }
        function OnGridKeyUpNumber(obj, decimalPlaces, allowNegative, e) {
            extractNumber(obj, decimalPlaces, allowNegative);
            UpDownKeyPress(obj.id, e);
        }

     
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
