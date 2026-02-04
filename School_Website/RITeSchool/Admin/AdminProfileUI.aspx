<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true"
    CodeFile="AdminProfileUI.aspx.cs" Inherits="AdminProfileUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Src="~/UserControls/UserBasicDetails.ascx" TagName="UserBasicDetails"
    TagPrefix="UserBasicDetailsUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" style="width: 97%;">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td width="80%">
                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Panel ID="pnlErrorMsg" runat="server">
                                                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Width="100%"
                                                        CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                                                </asp:Panel>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                    <td align="right" class="ClsTextNormal" style="padding-right: 10px; height: 19px;">
                                        <span class="ClsMdtStar">*</span>
                                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="1">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ValidationGroup="Save" ShowSummary="true" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="1">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        Visible="false" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnSave"/>
                                    <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" class="ClsTextNormal" align="center">
                            <!-- User InfoTable starts here -->
                            <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                style="width: 68%;">
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblName" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                            <asp:Label CssClass="LblSmlGray floatR" ID="lblFirstName" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, FirstName %> "></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar" style="width: 50%">
                                                            <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" TabIndex="1">
                                                            </asp:DropDownList>
                                                            <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Width="186px" onblur="formatName(this)"
                                                                CssClass="MidTxtBox" TabIndex="2"></asp:TextBox>
                                                            *&nbsp;
                                                            <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName"
                                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, FirstNameValidation%>"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="None" ClientValidationFunction="DuplicateName" ValidationGroup="Save"></asp:CustomValidator>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="LblSmlGray floatR" ID="Label4" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, MiddleInitial %> "></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="1"
                                                                onblur="formatName(this)" Width="50px" TabIndex="3"></asp:TextBox>
                                                           <%-- *<asp:RequiredFieldValidator ID="reqMiddleName" runat="server" ControlToValidate="txtMiddleName"
                                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValMiddleInitial%>"></asp:RequiredFieldValidator> --%>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="LblSmlGray floatR" ID="lblLastName" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, LastName%>"></asp:Label>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" onblur="formatName(this)"
                                                                CssClass="MidTxtBox" TabIndex="4"></asp:TextBox>
                                                            *<asp:RequiredFieldValidator ID="rqdLastName" runat="server" ControlToValidate="txtLastName"
                                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValLastNameBlank%>"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 200px;">
                                                            <asp:Label CssClass="ClsLabel" ID="lblAddress" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Address%>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                                TabIndex="5" Height="72px" Width="240px"></asp:TextBox>
                                                            <span style="color: red">* </span>
                                                            <asp:CustomValidator ID="cstValAddress" runat="server" ClientValidationFunction="validateAddress"
                                                                CssClass="ClsMdtStar" ValidationGroup="Save" Display="None" EnableClientScript="true"
                                                                ErrorMessage="Error msg" Visible="true"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" align="center" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblDateBirth" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:TextBox ID="txtDOB" runat="server" CssClass="SmlTxtBox" MaxLength="11" TabIndex="6"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cBirthDate" runat="server" Control="txtDOB" Format="dd MMM yyyy"
                                                                Culture="en" ShowWeekend="True" Enabled="true" ShowErrorMessage="false" To-Today="true"
                                                                InvalidDateMessage="<%$ Resources:LocalizedResources, ValidBirthDateErrorMsg%>"
                                                                ControlFocusOnError="True" />
                                                            <asp:CustomValidator ID="cstBirthDate" runat="server" Display="none" EnableClientScript="true"
                                                                ValidationGroup="Save" ClientValidationFunction="ValidateBirthDate" ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthFutureDate%>"></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cst_DOB" runat="server" ControlToValidate="txtDOB" ClientValidationFunction="DOBValidation"
                                                                Display="None" CssClass="ClsLabel" ValidationGroup="Save"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblMobileNumber" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, MobileNumber %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                ondrop="event.returnValue=false" TabIndex="7" />&nbsp;*
                                                            <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                                ValidationGroup="Save" Visible="true" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblEmergencyNumber" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, EmergencyContact %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" class="ClsMdtStar">
                                                            <asp:TextBox ID="txtEmergencyNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                                TabIndex="8" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                ondrop="event.returnValue=false" />&nbsp;*
                                                            <asp:RequiredFieldValidator ID="reqEmergencyNo" runat="server" ControlToValidate="txtEmergencyNo"
                                                                ValidationGroup="Save" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valEmergencyContactNo%>"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%; height: 24px">
                                                            <asp:Label CssClass="ClsLabel" ID="lblEmail" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Email %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                            <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                                                ValidationGroup="Save" ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                        </td>
                                                        <td align="left" style="width: 72%; height: 24px;">
                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50" TabIndex="9"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%; height: 24px">
                                                            <asp:Label CssClass="ClsLabel" ID="lblDesignation" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Designation %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left" style="width: 72%; height: 24px;">
                                                            <asp:DropDownList ID="cmbDesignations" runat="server" CssClass="MidCombo" Style="width: 190px;"
                                                                TabIndex="10">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqDesignations" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DesignationShouldBeSelected%>"
                                                                ValidationGroup="Save" ControlToValidate="cmbDesignations" InitialValue="0" CssClass="ClsMdtStar"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trchkCanApproveRequisitions" visible="true">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblApproveRequisition" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserApproveRequisition %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td id="Td22" runat="server" align="left">
                                                            <asp:CheckBox ID="chkCanApproveRequisitions" runat="server" TabIndex="11" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trPublishorUnpublish" runat="server">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblPublishExam" Height="16px" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, CanUserPublishExam %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkPublishorUnpublishExam" runat="server" TabIndex="19" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trchkCanCraeteGenerelRequisition" visible="true">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblCreateRequisition" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserCreateRequisition %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td id="Td2" runat="server" align="left">
                                                            <asp:CheckBox ID="chkCanCreateGeneralRequisition" runat="server" Checked="false"
                                                                TabIndex="12" />
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trchkCanSanctionLeave" visible="true">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblSanctionLeave" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserSanctionLeave %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td id="Td3" align="left">
                                                            <asp:CheckBox ID="chkCanSanctionLeave" runat="server" Checked="false" TabIndex="13" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAccountsRow1" runat="server">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblApproveVoucher" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserApproveVoucher %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCanApproveVoucher" TabIndex="14" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAccountsRow2" runat="server">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblUserCreateVoucher" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserCreateVoucher %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCanCreateVoucher" TabIndex="15" runat="server" OnClick="CreateVoucherOnChange(this);" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAccountsRow3" runat="server">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="Label1" Height="16px" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, UserSelfApproveVoucher %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCanSelfApprove" TabIndex="16" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAccountsRow4" runat="server" visible="false">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%" visible="false">
                                                            <asp:Label CssClass="ClsLabel" ID="lblDeleteVoucher" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserDeleteVoucher %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCanDeleteVoucher" TabIndex="17" runat="server" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trAccountsRow5" runat="server" visible="false">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%" visible="false">
                                                            <asp:Label CssClass="ClsLabel" ID="lblEditOldFinantialYear" Height="16px" runat="server"
                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, CanUserEditOldFinancialYear %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkCanEditOldFinancialYear" TabIndex="18" runat="server" Visible="False" />
                                                        </td>
                                                    </tr>
                                                    <tr id="Tr1" runat="server">
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblIsShowAllSentSMS" Height="16px" runat="server"
                                                                EnableViewState="False" Text="Allow to view all sent sms?"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkShowAllSentSMS" TabIndex="18" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblUserName" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, UserName %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td id="Td1" align="left" style="color: red;">
                                                            <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"
                                                                TabIndex="19"></asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                            <asp:RegularExpressionValidator ID="regUsername" runat="server" ValidationGroup="Save"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, UserNameValidation%>" Style="font-size: 9pt"
                                                                ValidationExpression="[A-Za-z0-9_.]+" ControlToValidate="txtUserName"> <span class="ClsMdtStar"></span></asp:RegularExpressionValidator>
                                                            <asp:RequiredFieldValidator ID="reqUserName" runat="server" ControlToValidate="txtUserName"
                                                                Display="Dynamic" Style="font-size: 9pt"></asp:RequiredFieldValidator>
                                                            <asp:CustomValidator ID="cst_UserName" runat="server" ClientValidationFunction="UserNameValidation"
                                                                ValidationGroup="Save" Display="None"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="lblPassword" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Password %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" MaxLength="15" CssClass="ExLrgTxtBox"
                                                                TabIndex="20"></asp:TextBox>
                                                            <span class="ClsMdtStar">*
                                                                <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                                    ValidationGroup="Save" ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderLight" style="width: 42%">
                                                            <asp:Label CssClass="ClsLabel" ID="Label2" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, ConfirmPassword %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td class="ClsMdtStar" align="left">
                                                            <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"
                                                                CssClass="ExLrgTxtBox" TabIndex="21"></asp:TextBox>
                                                            *<asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                                ValidationGroup="Save" ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 42%">
                                                            &nbsp;
                                                        </td>
                                                        <td class="ClsMdtStar" align="left">
                                                            <span class="LblSmlGray">
                                                                <asp:Label ID="lblNotePass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswod %>"></asp:Label>
                                                                <br />
                                                                <asp:Label ID="lblNoteConfirmPass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswordCombination %>"></asp:Label>
                                                            </span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight">
                                                            <asp:Label CssClass="ClsLabel" ID="Label3" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, SendSMS %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chkSendSMS" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="1" style="width: 25%;" class="ClsBorderlight">
                                                            <asp:Label CssClass="ClsLabel" ID="lblPhoto" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Photo %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <div class="ClsBorderlight" style="width: 112px; vertical-align: middle">
                                                                <img id="imgPhoto" alt="image" runat="server" height="151" width="119" />
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight">
                                                            <asp:Label CssClass="ClsLabel" ID="lblUploadPhoto" runat="server" EnableViewState="False"
                                                                Text="<%$ Resources:LocalizedResources, Upload_CapturePhoto %>"></asp:Label>
                                                            <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:FileUpload ID="UploadPhoto" runat="server" />
                                                                        <asp:CustomValidator ID="CustPhoto" Display="None" runat="server" ClientValidationFunction="ValidatePhoto"
                                                                            ValidationGroup="Save" TabIndex="22" ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>"
                                                                            ControlToValidate="UploadPhoto" CssClass="LblErrorMsg"></asp:CustomValidator>
                                                                    </td>
                                                                    <td>
                                                                        <img id="ImgWebCam" title="<%$ Resources:LocalizedResources, CapturePhoto%>" runat="server"
                                                                            style="cursor: pointer;" src="../images/WebCam.png" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" colspan="2">
                                                            <span class="LblSmlGray">
                                                                <asp:Label ID="lblUploadImage" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageForOtherStaff%>"></asp:Label>
                                                                <br />
                                                                <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                                                <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label></span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <UserBasicDetailsUC:UserBasicDetails ID="ucUserBasicDetails" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="BtnSave"  />
                                                <asp:AsyncPostBackTrigger ControlID="BtnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 42%; height: 32px;">
                                    </td>
                                    <td align="left" colspan="1" style="height: 32px">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                    BorderWidth="1px" ValidationGroup="Save" OnClick="btnSave_Click" TabIndex="24">
                                                </asp:Button>&nbsp;
                                                <asp:Button CssClass="ClsBtn" ID="BtnCancel" CausesValidation="false" runat="server"
                                                    Text="<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px" TabIndex="25"
                                                    OnClick="BtnCancel_Click"></asp:Button>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave"/>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td style="width: 0; white-space: nowrap;">
                                                    <span class="ClsLblLgnd">Legend :</span>
                                                </td>
                                                <td style="width: 0;">
                                                    <span style="display: inline-block; background-color: LightBlue; border: 1px solid black;
                                                        height: 20px; width: 20px; float: left"></span>
                                                </td>
                                                <td align="left">
                                                    <span class="ClsLabel" style="font-weight:bold;">Active Admin</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:ListView ID="lstvwAdminDetails" runat="server" DataKeyNames="UserId,DesignationId"
                                                    OnItemDataBound="lstvwAdminDetails_ItemDataBound" OnItemCommand="lstvwAdminDetails_ItemCommand"
                                                    OnSorting="lstvwAdminDetails_Sorting">
                                                    <LayoutTemplate>
                                                        <table align="center" width="100%" runat="server" id="tblStopInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="paddingL">
                                                                    <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="Name"
                                                                        CausesValidation="false" ForeColor="Black" Text="<%$ Resources:LocalizedResources, Name%>"> </asp:LinkButton>
                                                                </th>
                                                                <th align="left" style="padding-left: 9px;" width="175px">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Designation%>"></asp:Label>
                                                                </th>
                                                                <th align="center" width="100px">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                </th>
                                                                <th align="center" width="125px">
                                                                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, MobileNumber%>"></asp:Label>
                                                                </th>
                                                                <th align="center" width="50px">
                                                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, Edit%>"></asp:Label>
                                                                </th>
                                                                <th align="center" width="50px">
                                                                    <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Delete%>"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trRow" runat="server" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidUserId" runat="server" Value='<%# Eval("UserId") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" style="padding-right: 5px;">
                                                                <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand" ToolTip="<%$ Resources:LocalizedResources, Edit%>"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trRow" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                                                <asp:HiddenField ID="hidUserId" runat="server" Value='<%# Eval("UserId") %>' />
                                                            </td>
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                            </td>
                                                            <td align="center" style="padding-right: 5px;">
                                                                <asp:Label ID="lblDOB" runat="server" Text='<%# Eval("DOB") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label5" runat="server" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand" ToolTip="<%$ Resources:LocalizedResources, Edit%>"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand" ToolTip="<%$ Resources:LocalizedResources, Delete%>"
                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                                <asp:HiddenField ID="hidSortDirection" runat="server" Value="" />
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="btnSave" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <asp:HiddenField ID="hidServerDate" runat="server" />
                                        <asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidRetirementAge" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidRetAge" runat="server" />
                                    </td>
                                </tr>
                            </table>
                            <!-- User InfoTable end here -->
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidFilePath" runat="server" />
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnSave"/>
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwAdminDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                <asp:HiddenField ID="hidAgeShouldBeLessThan" runat="server" />
                <asp:HiddenField ID="hidyears" runat="server" />
                <asp:HiddenField ID="hidAgeValidationCondition" runat="server" />
                <asp:HiddenField ID="hidInvalidFileFormat" runat="server" />
                <asp:HiddenField ID="hidDateOfBirthFutureDate" runat="server" />
                <asp:HiddenField ID="hidvalLegthOfAddress" runat="server" />
                <asp:HiddenField ID="hidAddressBlank" runat="server" />
                <asp:HiddenField ID="hidEmailShouldNotBlank" runat="server" />
                <asp:HiddenField ID="hidEmailValidation" runat="server" />
                <asp:HiddenField ID="hidValForPassword" runat="server" />
                <asp:HiddenField ID="hidValPasswordLengh" runat="server" />
                <asp:HiddenField ID="hidNoteForPasswordCombination" runat="server" />
                <asp:HiddenField ID="hidvalConfirmPassword" runat="server" />
                <asp:HiddenField ID="hidValUserNameBlank" runat="server" />
                <asp:HiddenField ID="hidvalUserNameLength" runat="server" />
                <asp:HiddenField ID="hidMobileNoVal" runat="server" />
                <asp:HiddenField ID="hidMobileDigit" runat="server" />
                <asp:HiddenField ID="hidMobileNumberBlank" runat="server" />
                <asp:HiddenField ID="hidconfirmDelete" runat="server" Value="" />
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">

        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>";
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _clienttxtPasswdId = "<%=this.txtPasswd.ClientID %>";
        _clientcstValPasswdId = "<%=this.cstValPassword.ClientID %>";
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>";
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>";
        _clienttxtUserName = "<%=this.txtUserName.ClientID %>";
        _clientcst_UserNameId = "<%=this.cst_UserName.ClientID %>";
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clientbtnSave = "<%=this.btnSave.ClientID %>";
        _clientbtnCancel = "<%=this.BtnCancel.ClientID %>";
        _clienttxtDOB = "<%=this.txtDOB.ClientID %>";
        _clientcstBirthDate = "<%=this.cstBirthDate.ClientID%>";
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
        _clientUploadPhoto = "<%=this.UploadPhoto.ClientID%>";
        _ClientCustPhoto = "<%=this.CustPhoto.ClientID %>";
        _ClienttxtAddress = "<%=this.txtAddress.ClientID %>";
        _clientcstValAddress = "<%=this.cstValAddress.ClientID %>";
        _clientchkCanSelfApprove = "<%= this.chkCanSelfApprove.ClientID %>";
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _clienthidRetirementAge = "<%=this.hidRetirementAge.ClientID %>"
        _clientcal_DOB = "<%=this.txtDOB.ClientID %>";
        _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
        _clienthidRetAge = "<%=this.hidRetAge.ClientID %>"
        _clientlstvwAdminDetails = "<%=this.lstvwAdminDetails.ClientID %>"

        _clienttxtFirstName = "<%=this.txtFirstName.ClientID %>"
        _clienttxtMiddleName = "<%=this.txtMiddleName.ClientID %>"
        _clienttxtLastName = "<%=this.txtLastName.ClientID %>"
        _clienttxtDOB = "<%=this.txtDOB.ClientID %>"
        _clienthidUserId = "<%=this.hidUserId.ClientID %>"

        //This function is used to validate DOB.
        function DOBValidation(oSrc, args) {
            var oDOBObj;
            var RetirementAge = $get(_clienthidRetirementAge).value;
            oDOBObj = document.getElementById(_clientcal_DOB).value;
            var sDate;

            if (document.all)
                sDate = new Date(oDOBObj.replace('-', ' '));
            else
                sDate = new Date(convertdate(oDOBObj));

            var RetDate;
            if (document.all)
                RetDate = new Date(RetirementAge.replace('-', ' '));
            else
                RetDate = new Date(convertdate(RetirementAge));

            var today = new Date();
            var DOBYear = parseInt(sDate.getFullYear());
            var thisYear = parseInt(today.getFullYear());

            var yearDiff = thisYear - parseInt(DOBYear);
            var RetConfigAge = $get(_clienthidRetAge).value;

            if (parseInt(RetConfigAge) > 0) {
                if (sDate < RetDate) {

                    document.getElementById(_clientcst_DOB).errormessage = document.getElementById("<%=this.hidAgeShouldBeLessThan.ClientID %>").value + RetConfigAge + document.getElementById("<%=this.hidyears.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            if (parseInt(yearDiff) < 18) {

                document.getElementById(_clientcst_DOB).errormessage = document.getElementById("<%=this.hidAgeValidationCondition.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;

            return false;
        }


        function validateAddress(source, args) {
            var txtAddress = document.getElementById(_ClienttxtAddress).value;
            var bIsValid = true;

            if (txtAddress.trim() != "") {
                if (txtAddress.length > 150) {
                    bIsValid = false;
                    document.getElementById(_clientcstValAddress).errormessage =
                  document.getElementById("<%=this.hidvalLegthOfAddress.ClientID %>").value;
                }
            }
            else {

                bIsValid = false;
                document.getElementById(_clientcstValAddress).errormessage =
                  document.getElementById("<%=this.hidAddressBlank.ClientID %>").value;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }
        //This function is used to disable buttons.
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnSave)) {
                var isPageValid = true;
                if (typeof (Page_ClientValidate) == 'function')
                    isPageValid = Page_ClientValidate();

            }
            ResetErrorMsgLbl();
        }

        var blanks = " \t\n\r";  // Ek whitespace chars
        //This function is used to validate Email address.
        function EmailValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtEmailId).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);

            if (isEmpty(sEmail)) {
                document.getElementById(_clientcstValEmailId).errormessage = document.getElementById("<%=this.hidEmailShouldNotBlank.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else {
                // If email is not blank then validate for valid email address.
                if (!isEmail(sEmail)) {
                    document.getElementById(_clientcstValEmailId).errormessage = document.getElementById("<%=this.hidEmailValidation.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        //This function is used to validate password.
        function PasswordValidation(oSrc, args) {

            var sPassword = document.getElementById(_clienttxtPasswdId).value;
            var password = sPassword;
//            var passed = validatePassword(password, {
//                length: [6, Infinity],
//                alpha: 1,
//                numeric: 1,
//                special: 1
//            });
            var passed = CheckPassword(sPassword)
            if (sPassword == "") {
                document.getElementById(_clientcstValPasswdId).errormessage = document.getElementById("<%=this.hidValForPassword.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sPassword.length < 6) {
                document.getElementById(_clientcstValPasswdId).errormessage = document.getElementById("<%=this.hidValPasswordLengh.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (!passed) {
                document.getElementById(_clientcstValPasswdId).errormessage = document.getElementById("<%=this.hidNoteForPasswordCombination.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function CheckPassword(inputtxt) {
            var decimal = /^(?=.*\d)(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{6,15}$/;
            if (inputtxt.match(decimal)) {
                return true;
            }
            else {
                return false;
            }
        }   
        //This function is used to validate whether password and confirm password are same or not.
        function ComparePasswordValidation(oSrc, args) {

            var sConfirmPassword = document.getElementById(_clienttxtConfirmPasswd).value
            var sPassword = document.getElementById(_clienttxtPasswdId).value
            if (sPassword != sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = document.getElementById("<%=this.hidvalConfirmPassword.ClientID %>").value;
                args.IsValid = false
                return true

            }
            args.IsValid = true
            return false
        }

        //This function is used to validate user name.
        function UserNameValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtUserName).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (isEmpty(sEmail)) {
                document.getElementById(_clientcst_UserNameId).errormessage = document.getElementById("<%=this.hidValUserNameBlank.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            if (sEmail.length < 6) {
                document.getElementById(_clientcst_UserNameId).errormessage = document.getElementById("<%=this.hidvalUserNameLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>";
        //This function is used to validate mobile No.
        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
            document.getElementById(_clientcst_MobileNumber).errormessage = "";
            if (sMobileNumber.length == 0) {
                document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileNumberBlank.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            if (sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileDigit.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileNoVal.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        //This function is used to validate birth date.
        function ValidateBirthDate(source, args) {
            var bIsValid = true;

            if (document.getElementById(_clienttxtDOB).value != "") {
                var serverDate = document.getElementById(_clientServerDate).value;
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtDOB).value));
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    document.getElementById(_clientcstBirthDate).errormessage =
                                     document.getElementById("<%=this.hidDateOfBirthFutureDate.ClientID %>").value;
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ResetErrorMsgLbl() {
            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).style.display = "none";
            }
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none";
            }
        }
        function ValidatePhoto(aSrc, args) {
            var sImage = new Image();
            aSrc.errormessage = "";
            sImage.src = document.getElementById(_clientUploadPhoto).value;
            var iWidth = sImage.width
            var iHeight = sImage.height
            if (sImage.src != "") {
                if (!CheckFileType(sImage.src)) {
                    aSrc.errormessage = document.getElementById("<%=this.hidInvalidFileFormat.ClientID %>").value;
                    document.getElementById(_ClientCustPhoto).errormessage = document.getElementById("<%=this.hidInvalidFileFormat.ClientID %>").value;
                }
            }
            if (aSrc.errormessage == "") {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }
        //This function is used to check file type.
        function CheckFileType(sFileName) {
            var bIsValid;
            var sFileType = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase();
            if (sFileType == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG")
                bIsValid = true;
            else
                bIsValid = false;
            return bIsValid
        }

        // This function enables disables the CanSelfApprove checkbox when CanApproveVoucher checkbox is checked/unchecked
        function CreateVoucherOnChange(src) {
            var chkCanSelfApprove = $get(_clientchkCanSelfApprove);
            if (chkCanSelfApprove)
                chkCanSelfApprove.disabled = !src.checked;
            if (!src.checked)
                chkCanSelfApprove.checked = false;
        }

        function OpenWebcamPopup(sQueryString) {
            window.open('../Common/WebcamNewPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400');
            return true;
        }

        function UpdateHiddenField(Char) {
            $get(_clienthidIsPhotoCaptured).value = "Y";
        }

        function ConfirmDelete() {
            return confirm($get("<%=this.hidconfirmDelete.ClientID %>").value)
        }

        function DuplicateName(oSrc, args) {            
            var iIndex = 0;

            var isFound = false;
            var name = document.getElementById(_clientlstvwAdminDetails + "_ctrl" + iIndex + "_lblName")
            while (name != null) {

                var dob = document.getElementById(_clientlstvwAdminDetails + "_ctrl" + iIndex + "_lblDOB").innerHTML
                var userId = $get(_clientlstvwAdminDetails + "_ctrl" + iIndex + "_hidUserId").value
                var fullName = $get(_clienttxtFirstName).value.trim() + " " + $get(_clienttxtMiddleName).value.trim() + " " + $get(_clienttxtLastName).value.trim()

                //if (fullName == name.innerHTML && $get(_clienttxtDOB).value.trim() == dob && parseInt(userId) != parseInt($get(_clienthidUserId).value)) {
                if (fullName == name.innerHTML && parseInt(userId) != parseInt($get(_clienthidUserId).value)) {
                    isFound = true;
                    break;
                }
                iIndex++;
                name = document.getElementById(_clientlstvwAdminDetails + "_ctrl" + iIndex + "_lblName")
            }

            if (isFound) {
                oSrc.errormessage = "Name should not be duplicate."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
