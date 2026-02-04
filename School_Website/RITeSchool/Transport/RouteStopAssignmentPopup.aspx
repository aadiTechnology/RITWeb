<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="RouteStopAssignmentPopup.aspx.cs" Inherits="RouteStopAssignmentPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table style="width: 100%;" cellpadding="0" cellspacing="1">
        <tr>
            <td align="left" colspan="3">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="height: 20px">
                            <span style="font-weight: bold; padding-right: 5px;">Transport Assignment </span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trMandatory" runat="server">
            <td align="right" colspan="6">
                <span class="ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                    ShowMessageBox="false" ShowSummary="true" />
            </td>
        </tr>
        <tr id="trError">
            <td align="center">
                <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                    ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                <asp:Label ID="lblError" CssClass="LblErrorMsg" runat="server" EnableViewState="False"
                    Width="100%" Style="text-align: left; padding-left: 20px" Visible="false"></asp:Label>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td align="left" class="ClsBorderlight">
                            <asp:Label CssClass="ClsLabel" ID="lblUserName" runat="server" Text="User Name : "></asp:Label>
                        </td>
                        <td class="ClsHilightBGB" align="left">
                            <asp:Label ID="lblName" runat="server" class="LblNrmlB" Style="border-width: 0px;
                                font-weight: bold;" Text=" Name"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%">
         <asp:UpdatePanel ID="upnl" runat="server">
            <ContentTemplate>
        <tr>
            <td width="50%">
                <table align="left">
                    <tr>
                        <td class="ClsBorderlight" align="center" colspan="2">
                            <asp:Label CssClass="ClsLabel" ID="lblPickup" runat="server" Text="Pickup Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <asp:Label CssClass="ClsLabel" ID="lblPickupRoute" runat="server" Text="Route :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight">
                            <asp:DropDownList ID="cmbRoute" runat="server" CssClass="MidCombo" AutoPostBack="true" 
                                OnSelectedIndexChanged="cmbRoute_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Pick Up Route should be selected." Display="None" ControlToValidate="cmbRoute" ValueToCompare="0" Type="Integer" Operator="NotEqual"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <asp:Label CssClass="ClsLabel" ID="lblPickupStop" runat="server" Text="Stop :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space:nowrap">
                            <asp:DropDownList ID="cmbStop" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                OnSelectedIndexChanged="cmbStop_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span id="StopMandatory" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstValPickupStop" runat="server" ClientValidationFunction="ValidatePickupStop"
                                Display="None" ErrorMessage="Pickup Stop should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <asp:Label CssClass="ClsLabel" ID="lblPickupShift" runat="server" Text="Shift :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space:nowrap">
                            <asp:DropDownList ID="cmbShift" runat="server" CssClass="MidCombo" 
                                AutoPostBack="True" onselectedindexchanged="cmbShift_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span id="ShiftMandatory" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstvalPickupShift" runat="server" ClientValidationFunction="ValidatePickupShift"
                                Display="None" ErrorMessage="Pickup Shift should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" valign="top" style="white-space: nowrap">
                            <asp:Label CssClass="ClsLabel" ID="lblPickupvehicle" runat="server" Text="Vehicle Number :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space:nowrap">
                            <asp:DropDownList ID="cmbVehicle" CssClass="MidCombo" runat="server">
                            </asp:DropDownList>
                            <span id="VehicleMandatory" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstvalPickupVehicle" runat="server" ClientValidationFunction="ValidatePickupVehicle"
                                Display="None" ErrorMessage="Pickup Vehicle should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </td>
            <td colspan="5" align="center">
                <hr style="width: 3px; height: 125px; background-color: Silver" align="center" />
            </td>
            <td width="50%">
                <table align="left" id="tblDrop" runat="server">
                    <tr>
                        <td class="ClsBorderlight" align="center" colspan="2">
                            <asp:Label CssClass="ClsLabel" ID="lblDrop" runat="server" Text="Drop Details"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr id="trDropRoute" runat="server">
                        <td class="ClsBorderlight">
                            <%--<span class="ClsLabel">Route :</span>--%>
                            <asp:Label ID="lblDropRoute" runat="server" CssClass="ClsLabel" Text="Route :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight">
                            <asp:DropDownList ID="cmbDropRoute" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                OnSelectedIndexChanged="cmbDropRoute_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="trDropStop" runat="server">
                        <td class="ClsBorderlight">
                            <span class="ClsLabel">Stop :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space: nowrap">
                            <asp:DropDownList ID="cmbDropStop" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                OnSelectedIndexChanged="cmbDropStop_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span id="StopMandatory1" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstvalDropStop" runat="server" ClientValidationFunction="ValidateDropStop"
                                Display="None" ErrorMessage="Drop Stop should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr id="trDropShift" runat="server">
                        <td class="ClsBorderlight">
                            <asp:Label CssClass="ClsLabel" ID="lblDropShift" runat="server" Text="Shift :"></asp:Label>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space: nowrap">
                            <asp:DropDownList ID="cmbDropShift" runat="server" CssClass="MidCombo" AutoPostBack="True"
                                OnSelectedIndexChanged="cmbDropShift_SelectedIndexChanged">
                            </asp:DropDownList>
                            <span id="ShiftMandatory1" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstvalDropShift" runat="server" ClientValidationFunction="ValidateDropShift"
                                Display="None" ErrorMessage="Drop Shift should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="white-space: nowrap">
                            <span class="ClsLabel">Vehicle Number :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="white-space: nowrap">
                            <asp:DropDownList ID="cmbDropVehile" CssClass="MidCombo" runat="server">
                            </asp:DropDownList>
                            <span id="VehicleMandatory1" runat="server" class="ClsMdtStar" >*</span>
                            <asp:CustomValidator ID="cstvalDropVehicle" runat="server" ClientValidationFunction="ValidateDropVehicle"
                                Display="None" ErrorMessage="Drop Vehicle should be selected.">
                            </asp:CustomValidator>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
         </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <table width="100%">
         <asp:UpdatePanel ID="upnl1" runat="server">
            <ContentTemplate>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td class="ClsBorderlight" align="left" colspan="4">
                            <asp:CheckBox AutoPostBack="True" ID="chkIncludeAll" Text="Check if drop route stop is same as of pickup route stop."
                                runat="server" Visible="true" Checked="false" Enabled="false" OnCheckedChanged="chkIncludeAll_CheckedChanged" />
                        </td>
                    </tr>
                    <tr class="height20">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" align="left" colspan="1">
                            <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Transport Facility Period :" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight" style="white-space: nowrap">
                            <asp:Label CssClass="ClsLabel" ID="Label2" runat="server" Text="Start Date :"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start Date should not be blank." />
                            <span class="ClsMdtStar">* </span>
                            <asp:CustomValidator ID="cstEffectiveDate" runat="server" Display="none" EnableClientScript="true"
                                ClientValidationFunction="ValidateEffectiveDate" ErrorMessage="Start Date should not be blank."></asp:CustomValidator>
                        </td>
                        <td class="ClsBorderlight" style="white-space: nowrap">
                            <asp:Label CssClass="ClsLabel" ID="Label1" runat="server" Text="End Date :"></asp:Label>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEndDate" CssClass="SmlTxtBox" runat="server" TabIndex="1"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="none" EnableClientScript="true"
                                ClientValidationFunction="ValidateEndDate" ErrorMessage="End Date should not be less than Start Date."></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <span class="LblNrmlB ClsBorderlight" style="background-color: #ffffc4;">Note :</span>
                            <span class="ClsBorderlight" style="font-family: Verdana; font-size: 8pt; border: 100%;">
                                Transport charges will be applicable from selected transport facility period start Date at the time of saving transport details.</span>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:HiddenField ID="hidUserId" runat="server" />
                <asp:HiddenField ID="hidUserName" runat="server" />
                <asp:HiddenField ID="hidTransportDetailsPickupId" runat="server" />
                <asp:HiddenField ID="hidTransportDetailsDropId" runat="server" />
                <asp:HiddenField ID="hidQueryString" runat="server" />
                <asp:HiddenField ID="hidAcYearStartDate" runat="server" />
                <asp:HiddenField ID="hidAcYearEndDate" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="3">
                <asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" TabIndex="9"
                    disable-page="true" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" Text="Delete" CssClass="ClsBtn" runat="server" TabIndex="10" OnClick="btnDelete_Click" />
                <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" UseSubmitBehavior="false"
                    Text="<%$ Resources:LocalizedResources, Close%>" CausesValidation="False" />
            </td>
        </tr>
         </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <script type="text/javascript" language="javascript">
        _clientcmbRoute = "<%=this.cmbRoute.ClientID %>";
        _clientcmbStop = "<%=this.cmbStop.ClientID %>";
        _clientcmbShift = "<%=this.cmbShift.ClientID %>";
        _clientcmbVehicle = "<%=this.cmbVehicle.ClientID %>";
        _clientcmbDropRoute = "<%=this.cmbDropRoute.ClientID %>";
        _clientcmbDropStop = "<%=this.cmbDropStop.ClientID %>";
        _clientcmbDropShift = "<%=this.cmbDropShift.ClientID %>";
        _clientcmbDropVehile = "<%=this.cmbDropVehile.ClientID %>";
        _clienttxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>";
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
        _clienthidAcYearStartDate = "<%=this.hidAcYearStartDate.ClientID %>";
        _clienthidAcYearEndDate = "<%=this.hidAcYearEndDate.ClientID %>";
        _clienttxtEndDate = "<%=this.txtEndDate.ClientID %>"


        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ValidatePickupStop(aSrc, args) {
            if ($get(_clientcmbStop).value == 0 && $get(_clientcmbRoute).value != 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidatePickupShift(aSrc, args) {
            if ($get(_clientcmbShift).value == 0 && $get(_clientcmbRoute).value != 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidatePickupVehicle(aSrc, args) {
            if ($get(_clientcmbVehicle).value == 0 && $get(_clientcmbRoute).value != 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidateDropStop(aSrc, args) {
            if ($get(_clientcmbDropRoute).value != 0 && $get(_clientcmbDropStop).value == 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidateDropShift(aSrc, args) {
            if ($get(_clientcmbDropRoute).value != 0 && $get(_clientcmbDropShift).value == 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidateDropVehicle(aSrc, args) {
            if ($get(_clientcmbDropRoute).value != 0 && $get(_clientcmbDropVehile).value == 0)
                args.IsValid = false;
            else {
                args.IsValid = true;
                return false;
            }
            return false;
        }

        function ValidateEffectiveDate(source, args) {
            var bIsValid = true;
            var StartDate = $get(_clienthidAcYearStartDate).value;
            var EndDate = $get(_clienthidAcYearEndDate).value;
            var dtPaymentDate = $get(_clienttxtPaymentDate);
            dtPaymentDate.value = dtPaymentDate.value.trim();

            if (dtPaymentDate.value == "") {
                source.errormessage = "Start Date should not be blank.";
                bIsValid = false;
            }
            else if (dtPaymentDate.value != "" && !validateDate(dtPaymentDate)) {
                source.errormessage = "Start Date should be in valid format.";
                bIsValid = false;
            }
            else if (getDate(StartDate) > getDate(dtPaymentDate.value) || getDate(EndDate) < getDate(dtPaymentDate.value)) {
                source.errormessage = "Start Date should be within current academic year ( " + StartDate + " To " + EndDate + " ).";
                bIsValid = false;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateEndDate(source, args) {
            var bIsValid = true;
            var StartDate = $get(_clienthidAcYearStartDate).value;
            var EndDate = $get(_clienthidAcYearEndDate).value;
            var dtEndDate = $get(_clienttxtEndDate);
            dtEndDate.value = dtEndDate.value.trim();

            var dtStartDate = $get(_clienttxtPaymentDate);
            dtStartDate.value = dtStartDate.value.trim();

            if (dtEndDate.value != "") {
                if (!validateDate(dtEndDate)) {
                    source.errormessage = "End Date should be in valid format.";
                    bIsValid = false;
                }
                else if (getDate(StartDate) > getDate(dtEndDate.value) || getDate(EndDate) < getDate(dtEndDate.value)) {
                    source.errormessage = "End Date should be within current academic year ( " + StartDate + " To " + EndDate + " ).";
                    bIsValid = false;
                }
                else if (dtStartDate.value.trim() != "" && getDate(dtStartDate.value) > getDate(dtEndDate.value)) {
                    source.errormessage = "Start Date should not be less than End Date.";
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function validateDate(txtDueDate) {
            var isValid = true;
            if (document.all) {
                if (isNaN(new Date(convertdate(txtDueDate.value).replace(/-/g, ' '))))
                    isValid = false;
            }
            else {
                if (isNaN(new Date(convertdate(txtDueDate.value).replace('-', ' '))))
                    isValid = false;
            }
            return isValid;
        }

        function getDate(obj) {
            var strDate = obj.replace('-', ' ').replace('-', ' ');
            return new Date(strDate);
        }

        function CloseWindow() {
            window.opener.location = window.opener.location.pathname + "?" + $get(_clienthidQueryString).value;
            window.opener.focus();
            window.close();
        }

    </script>
</asp:Content>
