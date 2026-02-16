<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ApplyLeaveUI.aspx.cs" Inherits="ApplyLeaveUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table style="width: 100%;" cellpadding="0" cellspacing="1">
        <tr id="trMandatory" runat="server">
            <td align="right" colspan="6">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left" valign="top">
                <asp:ValidationSummary ID="valSumError" runat="server" HeaderText="Please correct following errors."
                    CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" />
                <asp:ValidationSummary ID="ValSumApprove" runat="server" HeaderText="Please correct following errors."
                    CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" ValidationGroup="approve" />
                <asp:ValidationSummary ID="valSumUpdateLeaveRecord" runat="server" HeaderText="Please correct following errors."
                    CssClass="ClsMdtStar" ShowMessageBox="false" ShowSummary="true" ValidationGroup="UpdateLeaveRecord" />
                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Leave dates should not overlap on another leave day's." Display="None" OnServerValidate="DateOverlapping_Validate"></asp:CustomValidator>
                <asp:CustomValidator ID="custValDateValidate" runat="server" ErrorMessage="" ValidationGroup="UpdateLeaveRecord" Display="None" OnServerValidate="Date_Validate" Enabled="false"></asp:CustomValidator>
                <asp:CustomValidator ID="cstValidateDates" runat="server" Display="None" ClientValidationFunction="ValidateEndDate" EnableClientScript="true" />
            </td>
        </tr>
        <tr>
            <td align="center" id="tdMessage" runat="server" colspan="2">
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                    Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                <asp:Label ID="lblError" runat="server" EnableViewState="false" ForeColor="Red" CssClass="ClsTextNormal"
                    Style="display: block; margin: 5px 0;" />
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="75%">
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="150px">
                                <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="Staff Name : "></asp:Label>
                            </td>
                            <td colspan="4" class="ClsHilightBGB">
                                <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight" width="150px">
                                <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" Text="Start Date : "></asp:Label>
                            </td>
                            <td align="left" width="250px">
                                <asp:TextBox ID="txtStartDate" TabIndex="2" runat="server" ViewStateMode="Enabled"
                                    Enabled="true" MaxLength="12" CssClass="SmlTxtBox" Format="dd MMM yyyy"></asp:TextBox>
                                <rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="Enabled" Control="txtStartDate"
                                    Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" />
                                <span style="color: #ff0000">*</span>
                            </td>
                            <td width="10px">
                            </td>
                            <td align="left" class="ClsBorderlight">
                                <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" Text="End Date : "></asp:Label>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtEndDate" TabIndex="3" runat="server" ViewStateMode="Enabled"
                                    Enabled="true" Format="dd MMM yyyy" MaxLength="12" CssClass="SmlTxtBox"></asp:TextBox>
                                <rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="Enabled" Control="txtEndDate"
                                    Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" />
                                <span style="color: #ff0000">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderlight" id="td1" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="LeaveID" runat="server" CssClass="ClsLabel" Text="Leave Type : "></asp:Label>
                            </td>
                            <td align="left" id="td2" runat="server" viewstatemode="Enabled">
                                <asp:DropDownList ID="ddlleavetype" runat="server" CssClass="SmlCombo" Enabled="true">
                                </asp:DropDownList>
                                <span style="color: #ff0000">*</span>
                                <%--<asp:RequiredFieldValidator ID="reqvalLeavetype" runat="server" Display="None" ControlToValidate="ddlleavetype"
                                    CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="Please select Leave Type"></asp:RequiredFieldValidator>--%>
                            </td>
                            <td>
                            </td>
                            <td align="left" class="ClsBorderlight" id="td3" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text="Total Days : "></asp:Label>
                            </td>
                            <td align="left" id="td4" runat="server" viewstatemode="Enabled">
                                <asp:TextBox ID="txtTotalDays" TabIndex="3" runat="server" ViewStateMode="Enabled"
                                    Enabled="true" MaxLength="4" CssClass="SmlTxtBox" onblur="extractNumber(this,1,false);"
                                    onkeyup="extractNumber(this,1,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                                <span style="color: #ff0000">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None"
                                    ControlToValidate="txtTotalDays" CssClass="ClsMdtStar" ErrorMessage="Total Days should not be blank."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr id="trChargeHandover" runat="server" visible="false">
                            <td align="left" class="ClsBorderlight" id="tdHandoverto" runat="server" viewstatemode="Enabled">
                                <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text="Charge Handover To : "></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlUserName" runat="server" CssClass="ExLrgCombo" Enabled="true"
                                    Width="180px">
                                </asp:DropDownList>
                                <span style="color: #ff0000">*</span>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" Enabled="false"
                                    ControlToValidate="ddlUserName" CssClass="ClsMdtStar" InitialValue="0" ErrorMessage="'Charge Handover To' should be selected."></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="middle" class="ClsBorderlight">
                                <span class="ClsLabel">Attachment :</span>
                            </td>
                            <td align="left">
                            <asp:FileUpload ID="fuDocumentPhoto" runat="server" ViewStateMode="Enabled" />
                                <asp:ImageButton ID="btnView" runat="server"  ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" 
                                    Visible="false" ToolTip="View Attachment" />
                            <asp:CustomValidator ID="cvFileUpload" runat="server" ControlToValidate="fuDocumentPhoto" ErrorMessage="Attachment file should be of type : BMP, JPG, JPEG, PDF, PNG." ClientValidationFunction="ValidateFileUpload"
                                OnServerValidate="cvFileUpload_ServerValidate" Display="none" ForeColor="Red"></asp:CustomValidator>
                              </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span class="LblSmlGray">(Attachment supports files of types - .BMP, .JPG, .JPEG, .PDF,
                                    .PNG upto 5 MB.)</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" class="ClsBorderLight">
                                <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text="Description"
                                    Enabled="false"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td colspan="4" align="left">
                                <asp:TextBox ID="txtDescription" CssClass="LrgTxtBox" MaxLength="300" Width="96%"
                                    Enabled="true" Height="100px" runat="server" TextMode="MultiLine"></asp:TextBox>
                                <span style="color: #ff0000">*</span>
                                <asp:HiddenField ID="hidPaymentId" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="left">
                                <table width="100%">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="width: 100px; background-color: #ffffc4;">
                                            <asp:Label ID="Label4" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 1 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <asp:Label ID="lblLeaveBalance" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                            <asp:Label ID="Label6" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note 2 :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px">
                                            <asp:Label ID="Label7" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="If leave start date or end date is across the month then system will update leave for only days those are in upcoming salary publish month."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="center">
                                <asp:Button ID="btnSubmit" CssClass="ClsBtn" runat="server" Text="Submit" UseSubmitBehavior="false"
                                    OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Clear" CssClass="ClsBtn" OnClick="btnCancel_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                <asp:HiddenField ID="hidQueryString" runat="server" />
                                <asp:HiddenField ID="hidConfigId" runat="server" Value="0" />
                                <asp:HiddenField ID="hidCategoryId" runat="server" Value="0" />
                            </td>
                        </tr>
                        <tr id="trSeparator" runat="server">
                            <td align="center" id="a" runat="server" colspan="5">
                                <hr style="width: 100%; border-width: 2px;" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <table width="100%" id="tblRemark" runat="server" align="center" style="text-align: center;
                                    margin: 0px auto;">
                                    <tr style="text-align: center; margin: 0px auto;" align="center" id="trRemark" runat="server">
                                        <td align="left" class="ClsBorderLight" style="width: 150px">
                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Remark"></asp:Label>
                                            <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtRemark" CssClass="LrgTxtBox" MaxLength="300" Width="96%" Height="100px"
                                                runat="server" TextMode="MultiLine"></asp:TextBox>
                                            <span style="color: #ff0000">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:Button ID="btnApprove" CssClass="ClsBtn" runat="server" Text="Approve" ValidationGroup="approve"
                                                OnClick="btnApprove_Click" />
                                            <asp:Button ID="btnReject" CssClass="ClsBtn" runat="server" Text="Reject" OnClick="btnReject_Click"
                                                ValidationGroup="approve" />
                                            <asp:Button ID="btnUpdateLeaveRecord" CssClass="ClsBtn" runat="server" Text="Update Leave" Visible="false"
                                                onclick="btnUpdateLeaveRecord_Click" />
                                            <asp:Button ID="btnFinalApprove" CssClass="ClsBtn" runat="server" Text="Final Approve" ValidationGroup="approve" OnClick="btnFinalApprove_Click" Visible="false"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="5">
                                <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" Text="Back" CausesValidation="false" />
                                <asp:HiddenField ID="hidUserLeaveDetails" runat="server" Value="" />
                                <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr style="text-align: center; margin: 0px auto;" align="center">
                <td align="center" style="text-align: center; margin: 0px auto;">
                    <table width="80%" id="tblRemark" runat="server" align="center" style="text-align:center; margin:0px auto;">
                        <tr style="text-align:center; margin:0px auto;" align="center">
                            <td align="left" class="ClsBorderLight" style="width: 13%">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Remark"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtRemark" CssClass="LrgTxtBox" MaxLength="300" Width="80%" Height="50px"
                                    runat="server" TextMode="MultiLine"></asp:TextBox>
                                <span style="color: #ff0000">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnApprove" CssClass="ClsBtn" runat="server" Text="Approve" ValidationGroup="approve"
                                    OnClick="btnApprove_Click" />
                                <asp:Button ID="btnReject" CssClass="ClsBtn" runat="server" Text="Reject" OnClick="btnReject_Click"
                                    ValidationGroup="approve" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <%--<tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnBack" CssClass="ClsBtn" runat="server" Text="Back" CausesValidation="false" />
                </td>
            </tr>--%>
        </table>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Start Date should not be blank."
            Display="None" ControlToValidate="txtStartDate"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="End Date should not be blank."
            Display="None" ControlToValidate="txtEndDate"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Description should not blank."
            Display="None" ControlToValidate="txtDescription"></asp:RequiredFieldValidator>
        <asp:RequiredFieldValidator ID="ReqRemark" runat="server" ErrorMessage="Remark should not blank."
            Display="None" ValidationGroup="approve" ControlToValidate="txtRemark"></asp:RequiredFieldValidator>
        <%--  <asp:CustomValidator ID="cstStartDateValidator"
							 runat="server"
							 Display="None"
							 ClientValidationFunction="ValidateStartDate"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="cstEndDateValidator"
							 runat="server"
							 Display="None"
							 ClientValidationFunction="ValidateEndDate"
							 EnableClientScript="true" />--%>
        <asp:CustomValidator ID="cstDateValidator" runat="server" Display="None" ClientValidationFunction="ValidateDates"
            EnableClientScript="true" />
        <asp:CustomValidator ID="cstValidateLeave" runat="server" Display="None" ClientValidationFunction="ValidateUSerLeaves"
            EnableClientScript="true" />
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
            Display="None" ControlToValidate="txtDescription" ErrorMessage="Length of Description should not exceed 1000 characters."
            CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ViewStateMode="Enabled"
            ValidationGroup="approve" Display="None" ControlToValidate="txtRemark" ErrorMessage="Length of Remark should not exceed 1000 characters."
            CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
    </div>
    <script type="text/javascript" language="javascript">

        var _clientddlleavetype = "<%=this.ddlleavetype.ClientID %>";
        var _clienttxtTotalDays = "<%=this.txtTotalDays.ClientID %>";
        var _clienttxtDescription = "<%=this.txtDescription.ClientID %>";
        var _clienttxtStartDate = '<%= this.txtStartDate.ClientID %>';

        var _clienttxtEndDate = '<%= this.txtEndDate.ClientID %>';

        var _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        var _clientlblError = "<%=this.lblError.ClientID %>";
        var _clientvalSumError = "<%=this.valSumError.ClientID %>";
        var _clientddlUserName = '<%= this.ddlUserName.ClientID %>';

        var _clientbtnSubmit = "<%=this.btnSubmit.ClientID %>";
        var _clientbtnCancel = '<%= this.btnCancel.ClientID %>';
        var _clienthidUserLeaveDetails = "<%=this.hidUserLeaveDetails.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);

        function EndReqHandler(sender, args) {

        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }
        function ClearMessages() {
            $get(_clientlblMessage).innerText = "";
            $get(_clientlblMessage).innerHTML = "";
            $get(_clientlblError).innerText = "";
            $get(_clientlblError).innerHTML = "";
        }

        function ClearControls() {
            ClearMessages();
            $get(_clientvalSumError).style.display = "none";
            $get(_clientddlleavetype).value = "0";
            $get(_clienttxtTotalDays).value = "";
            $get(_clienttxtDescription).value = ""
            $get(_clienttxtEndDate).value = "";
            $get(_clienttxtStartDate).value = "";
            $get(_clientddlUserName).value = 0;

            //            var btnSubmit = $get(_clientbtnSubmit);
            //            btnSubmit.value = "Submit";

            return true;
        }

        function ValidateDates(src, args) {        
            var totaldays = document.getElementById(_clienttxtTotalDays).value

            var dtFrom = document.getElementById(_clienttxtStartDate).value;
            var dtFromDate
            if (document.all)
                dtFromDate = new Date(dtFrom.replace('-', ' '));
            else
                dtFromDate = new Date(convertdate(dtFrom));

            var dtTo = document.getElementById(_clienttxtEndDate).value;
            var dtToDate
            if (document.all)
                dtToDate = new Date(dtTo.replace('-', ' '));
            else
                dtToDate = new Date(convertdate(dtTo));

            var diffTime = Math.abs(dtFromDate - dtToDate);

            var DateDiff = Math.ceil(diffTime / (1000 * 60 * 60 * 24));

            DateDiff = DateDiff + 1;

            if (parseFloat(DateDiff) < parseFloat(totaldays) || parseFloat(DateDiff) > parseFloat(totaldays)) {

                var adjDays = parseFloat(totaldays) + 0.5
                var adjDays1 = parseFloat(totaldays) + 1.0

                if (parseFloat(DateDiff) != parseFloat(adjDays) && parseFloat(DateDiff) != parseFloat(adjDays1)) {
                    src.errormessage = 'Total days should match with given date range.'
                    args.IsValid = false;
                    return true
                }
            }

            args.IsValid = true;
            return false
        }

        function ValidateUSerLeaves(oSrc, args) {
            var UserLeave = eval('[' + document.getElementById(_clienthidUserLeaveDetails).value + ']')[0];
            var LeaveId = $get(_clientddlleavetype).value;
            var leaveCount = $get(_clienttxtTotalDays).value;

            if (LeaveId == '0') {
                oSrc.errormessage = 'Leave Type should be selected.'
                args.IsValid = false;
                return true
            }
            else if (UserLeave != "" && LeaveId != '0' && leaveCount != '') {
                for (var k = 0; k < UserLeave.length; k++) {
                    var LeaveDetails = UserLeave[k];
                    if (LeaveDetails.LeaveId == LeaveId && LeaveDetails.IsUnpaid == false && LeaveDetails.AllowZeroBalance == false && parseFloat(LeaveDetails.Balance) < parseFloat(leaveCount)) {
                        oSrc.errormessage = 'Total days should be less than or equal to actual leave balance.'
                        args.IsValid = false;
                        return true
                    }
                }
            }

            args.IsValid = true;
            return false
        }
        function ValidateEndDate(oSrc, args) {
            var StartDate
            var EndDate
            if (document.all) {
                StartDate = new Date((document.getElementById(_clienttxtStartDate).value).replace('-', ' '))
                EndDate = new Date((document.getElementById(_clienttxtEndDate).value).replace('-', ' '))
            }
            else {
                StartDate = new Date(convertdate(document.getElementById(_clienttxtStartDate).value))
                EndDate = new Date(convertdate(document.getElementById(_clienttxtEndDate).value))
            }
            if (StartDate > EndDate) {
                oSrc.errormessage = "End Date should be greater than or equal to Start Date."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function ValidateFileUpload(sender, args) {
            var fileUpload = document.getElementById('<%= fuDocumentPhoto.ClientID %>');

            // Non-mandatory: valid if no file selected
            if (fileUpload.value === "") {
                args.IsValid = true;
                return;
            }

            var allowedExtensions = /\.(bmp|jpg|jpeg|pdf|png)$/i;
            var fileName = fileUpload.value;

            if (!allowedExtensions.test(fileName)) {
                args.IsValid = false;
                return;
            }

            var fileSize = fileUpload.files[0].size; // bytes
            var maxSize = 5 * 1024 * 1024; // 5 MB

            if (fileSize > maxSize) {
                sender.errormessage = 'Atttachment size should not be more than 5 MB.';
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
