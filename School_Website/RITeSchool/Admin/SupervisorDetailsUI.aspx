<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="SupervisorDetailsUI.aspx.cs" Inherits="SupervisorDetailsUI" ValidateRequest="false" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UserBasicDetails.ascx" TagName="UserBasicDetails"
    TagPrefix="UserBasicDetailsUC" %>
      <%@ Register Src="~/UserControls/EmployeeBasicDetails.ascx" TagName="EmployeeBasicDetails"
    TagPrefix="EmployeeBasicDetailsUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" width="100%" style="height: 100%;">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td width="80%">
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" CssClass="LblErrorMsg"
                                                EnableViewState="false"></asp:Label></asp:Panel>
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
                            <asp:UpdatePanel ID="UpdatePanel12" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                        ShowSummary="true" ValidationGroup="Save" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgBtnCancel" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="imgBtnSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" id="tdMessage" runat="server">
                            <asp:UpdatePanel ID="UpdatePanel11" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                        Font-Bold="true" Visible="False" EnableViewState="False" CssClass="ClsLabel"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="imgBtnCancel" EventName="Click" />
                                    <asp:PostBackTrigger ControlID="imgBtnSubmit" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr style="width: 100%; height: 100%;">
                        <td align="left">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr align="center" valign="top" runat="server" id="tr1">
                                    <td align="center">
                                        <table align="center" width="100%">
                                        <%--<cc1:CollapsablePanel ID="colpnlBasicDetails" runat="server" TitleText="<%$ Resources:LocalizedResources, BasicDetails%>"
                                            TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                            CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                            Collapsed="False" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">--%>
                                            <tr>
                                                <td class="CollapsedTitle" style="padding-left:15px;">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, BasicDetails%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height:10px;"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="1" class="ClsTextNormal" align="center">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <!-- User InfoTable starts here -->
                                                            <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                                                style="width: 80%;">
                                                                <tr>
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblName" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                        <asp:Label CssClass="LblSmlGray floatR" ID="lblFirstName" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, FirstName%> "></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar" style="width: 50%">
                                                                        <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" TabIndex="0">
                                                                        </asp:DropDownList>
                                                                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                                                            onblur="formatName(this)" Width="186px"></asp:TextBox>
                                                                        *&nbsp;
                                                                        <asp:RequiredFieldValidator ID="rqdFirstName" runat="server" ControlToValidate="txtFirstName"
                                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, FirstNameValidation%>"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">
                                                                        <asp:Label CssClass="LblSmlGray floatR" ID="Label1" runat="server" EnableViewState="False"
                                                                            Text="Middle Name Initial "></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="50"
                                                                            onblur="formatName(this)" Width="186px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">
                                                                        <asp:Label CssClass="LblSmlGray floatR" ID="lblLastName" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, LastName%>"></asp:Label>
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="MidTxtBox"
                                                                            onblur="formatName(this)"></asp:TextBox>
                                                                        *<asp:RequiredFieldValidator ID="rqdLastName" runat="server" ValidationGroup="Save"
                                                                            ControlToValidate="txtLastName" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValLastNameBlank%>"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblAddress" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, Address%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span> &nbsp;
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                                            Height="72px" Width="240px"></asp:TextBox>
                                                                        <span style="color: red">* </span>
                                                                        <asp:CustomValidator ID="cstValAddress" runat="server" ClientValidationFunction="validateAddress"
                                                                            CssClass="ClsMdtStar" Display="None" EnableClientScript="true" ErrorMessage="Error msg"
                                                                            ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                 <tr  style="display:none;">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="Label10" runat="server" EnableViewState="False"
                                                                            Text="Present Address"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span> &nbsp;
                                                                    </td>
                                                                    <td align="left" class="ClsMdtStar">
                                                                        <asp:TextBox ID="txtpresentAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                                            Height="72px" Width="240px"></asp:TextBox>
                                                                        
                                                                    </td>
                                                                </tr>

                                                                <tr>
                                                                 <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">                                                        
                                                 <span class="ClsLabel"><asp:Label ID="Label20" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, City %>">
                                                  </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" class="ClsMdtStar">
                                                    <asp:TextBox ID="txtLocalCity" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                                                    <%--<span class="ClsMdtStar">*
                                                        &nbsp; </span>--%>
                                                </td>
                                                                </tr>
                                                              
                                                                  <tr>
                                              
                                                <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">                                                
                                                 <span class="ClsLabel"><asp:Label ID="Label21" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, State %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" class="ClsMdtStar">
                                                    <asp:TextBox ID="txtState" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                                                    <%--<span class="ClsMdtStar">*
                                                    </span>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                             
                                                <td align="left" class="ClsBorderLight" style="width: 42%; padding-right:5px;">                                                   
                                                 <span class="ClsLabel"><asp:Label ID="Label22" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Pincode %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" class="ClsMdtStar">
                                                    <asp:TextBox ID="txtLocalPincode" CssClass="MidTxtBox" runat="server" MaxLength="6"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <%--<span class="ClsMdtStar">*   --%>                                                 
                                                        <asp:CustomValidator ID="cst_LocalPin" runat="server" ClientValidationFunction="PinCodeValidation" ControlToValidate="txtLocalPincode"
                                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                                    <%--</span>--%>
                                                </td>
                                            </tr>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight" align="center" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblDateBirth" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" valign="top">
                                                                        <asp:TextBox ID="txtDOB" runat="server" CssClass="SmlTxtBox" MaxLength="11"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtDOB" Culture="en" Format="dd MMM yyyy"
                                                                            ShowErrorMessage="false" To-Today="true" ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, DateErrorMsg%>"
                                                                            ValidationGroup="Save" />
                                                                        <asp:CustomValidator ID="cstBirthDate" runat="server" Display="none" EnableClientScript="true"
                                                                            ClientValidationFunction="ValidateBirthDate" ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthFutureDate%>"
                                                                            ValidationGroup="Save"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="cst_DOB" runat="server" ControlToValidate="txtDOB" ClientValidationFunction="DOBValidation"
                                                                            ValidationGroup="Save" Display="None" CssClass="ClsLabel"></asp:CustomValidator>
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
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                            ondrop="event.returnValue=false" />
                                                                        *<asp:RequiredFieldValidator ID="reqtxtMobileNo" runat="server" ControlToValidate="txtMobileNo"
                                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, MobileNumberBlank%>"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                                            Visible="true" ErrorMessage="<%$ Resources:LocalizedResources, MobileDigit%>"
                                                                            EnableClientScript="true" ClientValidationFunction="MobileNumberValidation" ValidationGroup="Save"></asp:CustomValidator>
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
                                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                            ondrop="event.returnValue=false" />&nbsp;*
                                                                        <asp:RequiredFieldValidator ID="reqEmergencyNo" runat="server" ControlToValidate="txtEmergencyNo"
                                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valEmergencyContactNo%>"
                                                                            ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trMail">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%;">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblEmail" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, Email %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                        <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                                                            ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True" ValidationGroup="Save"></asp:CustomValidator>
                                                                    </td>
                                                                    <td align="left" style="width: 50%;">
                                                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                                                                        <span class="ClsMdtStar">*</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%;">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblDesignation" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, Designation %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left" style="width: 50%;">
                                                                        <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="LrgCombo" OnSelectedIndexChanged="cmbDesignation_SelectedIndexChanged"
                                                                            AutoPostBack="true">
                                                                        </asp:DropDownList>
                                                                        <span class="ClsMdtStar">*
                                                                            <asp:CompareValidator ID="cmpDesignation" runat="server" ControlToValidate="cmbDesignation"
                                                                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valDesignation%>"
                                                                                Operator="NotEqual" Type="Integer" ValueToCompare="0" ValidationGroup="Save"></asp:CompareValidator>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="tracademic">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblAcadamicYearChange" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, AcadamicYearChange %>"></asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkAcademicApplicable" runat="server" Checked="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trchkCanApproveRequisitions" visible="true">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblApproveRequisition" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserApproveRequisition %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td id="Td22" align="left">
                                                                        <asp:CheckBox ID="chkCanApproveRequisitions" runat="server" Checked="True" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trchkCanCraeteGenerelRequisition" visible="true">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblCreateRequisition" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserCreateRequisition %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td id="Td2" align="left">
                                                                        <asp:CheckBox ID="chkCanCreateGeneralRequisition" runat="server" Checked="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trchkCanSanctionLeave" visible="true">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblSanctionLeave" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserSanctionLeave %>"></asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td id="Td3" align="left">
                                                                        <asp:CheckBox ID="chkCanSanctionLeave" runat="server" Checked="false" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trPublishorUnpublish" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblPublishExam" Height="16px" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, CanUserPublishExam %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkPublishorUnpublishExam" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow0" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblFinantialYearChange" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, FinancialYearChange %>"></asp:Label>
                                                                        <span class="colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkFinancialYearChangeApplicable" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow1" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblApproveVoucher" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserApproveVoucher %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCanApproveVoucher" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow2" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblUserCreateVoucher" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserCreateVoucher %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCanCreateVoucher" runat="server" OnClick="CreateVoucherOnChange(this);" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow3" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblSelfApproveVoucher" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelfApproveVoucher %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCanSelfApprove" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow4" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblDeleteVoucher" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserDeleteVoucher %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCanDeleteVoucher" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trAccountsRow5" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblEditOldFinantialYear" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, CanUserEditOldFinancialYear %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkCanEditOldFinancialYear" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trInternalUser" runat="server">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblIsInternalUser" Height="16px" runat="server"
                                                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, IsInternalUser %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkInternalUser" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSMSAllow">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label ID="lblShowAllSentSMS" runat="server" CssClass="ClsLabel" Text="Allow to view all sent sms?"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkShowAllSentSMS" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trusername">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblUserName" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, UserName %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td id="Td1" align="left" style="color: red;">
                                                                        <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                                        <span class="ClsMdtStar">*</span>
                                                                        <asp:RegularExpressionValidator ID="regUsername" runat="server" ValidationGroup="Save"
                                                                            ErrorMessage="<%$ Resources:LocalizedResources, ValUserName%>" Style="font-size: 9pt"
                                                                            ValidationExpression="[A-Za-z0-9_.]+" ControlToValidate="txtUserName"> <span class="ClsMdtStar"></span></asp:RegularExpressionValidator>
                                                                        <asp:RequiredFieldValidator ID="reqUserName" runat="server" ControlToValidate="txtUserName"
                                                                            Display="Dynamic" Style="font-size: 9pt"></asp:RequiredFieldValidator>
                                                                        <asp:CustomValidator ID="cst_UserName" runat="server" ClientValidationFunction="UserNameValidation"
                                                                            Display="None" ControlToValidate="txtUserName" ValidateEmptyText="True" CssClass="ClsMdtStar"
                                                                            ValidationGroup="Save"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trPassword">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="lblPassword" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, Password %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtPasswd" runat="server" type="Password" MaxLength="15" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                                        <span class="ClsMdtStar">*
                                                                            <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                                                ControlToValidate="txtPasswd" Display="None" ValidateEmptyText="True" ValidationGroup="Save"></asp:CustomValidator>
                                                                            <%-- <asp:RequiredFieldValidator ID="reqPasswd" runat="server" ControlToValidate="txtPasswd"
                                            Display="None" ErrorMessage="Password should not be blank." ValidationGroup="Save"></asp:RequiredFieldValidator>--%>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trConfirmPassword">
                                                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                                                        <asp:Label CssClass="ClsLabel" ID="Label2" runat="server" EnableViewState="False"
                                                                            Text="<%$ Resources:LocalizedResources, ConfirmPassword %>"></asp:Label>
                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                    </td>
                                                                    <td class="ClsMdtStar" align="left">
                                                                        <asp:TextBox ID="txtConfirmPasswd" runat="server" type="Password" MaxLength="15"
                                                                            CssClass="ExLrgTxtBox"></asp:TextBox>
                                                                        *<asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                                            ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True"
                                                                            ValidationGroup="Save"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trNotePassword">
                                                                    <td style="width: 42%">
                                                                        &nbsp;
                                                                    </td>
                                                                    <td class="ClsMdtStar" align="left">
                                                                        <span class="LblSmlGray">
                                                                            <asp:Label ID="lblNotePass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswod %>"></asp:Label>
                                                                            <br />
                                                                            <asp:Label ID="Label3" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswordCombination %>"></asp:Label>
                                                                        </span>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trSMS">
                                                                    <td class="ClsBorderLight">
                                                                        <asp:Label CssClass="ClsLabel" ID="Label4" runat="server" EnableViewState="False"
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
                                                                                        ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="UploadPhoto"
                                                                                        ValidationGroup="Save" CssClass="LblErrorMsg"></asp:CustomValidator>
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
                                                                <tr style="height:10px">
                                                                    <td colspan="2">
                                                                    </td>
                                                                </tr>
                                                                <tr id="trLicenseAttachment" runat="server">
                                                                    <td align="left" class="ClsBorderLight">
                                                                        <asp:Label ID="lblLicenseAttachment" CssClass="ClsLabel" runat="server" Text="Upload Driving License Copy :"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:FileUpload ID="flDocument" runat="server" />                                                                        
                                                                        <asp:ImageButton ID="btnFile" runat="server" CausesValidation="false" Visible="false"
                                                                         ToolTip="Update" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trLicenseAttachmentNote" runat="server">
                                                                    <td>
                                                                    </td>
                                                                    <td align="left">
                                                                        <span class="lblSmlGray">(Attachment supports files of types - .BMP, .JPG, .JPEG, .PDF,
                                                                            .PNG upto 5 MB.)</span>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trExpiryDate" runat="server">
                                                                    <td align="left" class="ClsBorderLight">
                                                                        <asp:Label ID="lblExpiryDate" CssClass="ClsLabel" runat="server" Text="Driving License Expiry Date :"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtLicenseExpiryDate" runat="server" CssClass="SmlCombo" 
                                                                            AutoPostBack="true"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="CalExpiryDate" runat="server" Control="txtLicenseExpiryDate" Format="dd MMM yyyy"
                                                                          ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid Start date."
                                                                          From-Today="True" />
                                                                          <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" ClientValidationFunction="ValidateLicenseExpiryDateForPast" ValidationGroup="Save" Display="None"></asp:CustomValidator>
                                                                        <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" ClientValidationFunction="ValidateLicenseExpiryDate" ValidationGroup="Save" Display="None"></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trLicenseRenew" runat="server">
                                                                    <td align="left" class="ClsBorderLight">
                                                                        <asp:Label ID="Label11" CssClass="ClsLabel" runat="server" Text="Renew Driving License? :"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkRenew" runat="server" />
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDriverBatch" runat="server">
                                                                    <td align="left" class="ClsBorderLight">
                                                                        <asp:Label ID="lblDriverBatch" CssClass="ClsLabel" runat="server" Text="Driver Batch :"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtDriverBatch" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height:10px">
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr runat="server" id="trUserBasicControl">
                                                                    <td colspan="2">
                                                                        <UserBasicDetailsUC:UserBasicDetails ID="ucUserBasicDetails" runat="server" />
                                                                        <asp:HiddenField ID="hidBasicDetailUserId" runat="server" Value="" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                            <td colspan="4">
                                             <EmployeeBasicDetailsUC:EmployeeBasicDetails ID="ucEmployeeBasicDetails" runat="server"/>
                                                     <asp:HiddenField ID="hidEmpDetailsuc" runat="server" Value="" />
                                                     </td>
                                                     </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                         <Triggers>
                                                            <asp:PostBackTrigger ControlID="imgBtnSubmit" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                         </table>
                                        <%--</cc1:CollapsablePanel>--%>
                                    </td>
                                </tr>
                                <tr style="width: 100%; height: 100%;">
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr align="center" valign="top" runat="server" id="trCollapsSubjectmenu">
                                                <td align="center">
                                                    <cc1:CollapsablePanel ID="colpnlSubjectSchedule" runat="server" TitleText="<%$ Resources:LocalizedResources, Menu%>"
                                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                                        Collapsed="True" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center" valign="top" class="ClsHilightBG">
                                                                            <asp:Label ID="lblSchoolConfig" runat="server" Font-Bold="True" CssClass="ClsHilightText"
                                                                                Text="<%$ Resources:LocalizedResources, SchoolMenus %>" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                        <td align="center" class="ClsHilightBG">
                                                                            <asp:Label ID="lblOtherMenus" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, Reports%>"
                                                                                CssClass="ClsHilightText" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="td-vertical-align-top">
                                                                            <div>
                                                                                <asp:GridView CssClass="GridBorder" ID="grdAccessConfiguration" runat="server" AutoGenerateColumns="False"
                                                                                    CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None" PageSize="2000"
                                                                                    Width="100%">
                                                                                    <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                                    <Columns>
                                                                                        <asp:BoundField DataField="ConfigChk" HeaderText="" HtmlEncode="false">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="Configure_Name" HeaderText="<%$ Resources:LocalizedResources, ScreenName%>"
                                                                                            SortExpression="Configure_Name" HtmlEncode="false">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                        </asp:BoundField>
                                                                                        <asp:BoundField DataField="CanEdit" HeaderText="<%$ Resources:LocalizedResources, CanEdit%>"
                                                                                            HtmlEncode="false">
                                                                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                            <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
                                                                                        </asp:BoundField>
                                                                                    </Columns>
                                                                                    <RowStyle CssClass="ClsGridRow" />
                                                                                    <PagerStyle CssClass="ClsNwGridPaging" Font-Bold="True" Font-Names="Arial" Font-Size="Small"
                                                                                        Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                                                                    <HeaderStyle CssClass="ClsGridHeader" />
                                                                                    <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </td>
                                                                        <td valign="top" class="td-vertical-align-top">
                                                                            <div id="Div1" style="width: 100%; overflow: auto;">
                                                                                <asp:ListView ID="lstvwReportFolders" runat="server" DataKeyNames="Report_Folder_Id , HasAccess"
                                                                                    OnItemDataBound="lstvwReportFolders_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="center" width="30px">
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    <asp:Label ID="lblReportName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ReportName%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="right" style="padding-left: 10px;">
                                                                                                    <asp:Label ID="lblHasFullAccess" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, HasFullAccess%>"></asp:Label>
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
                                                                                            <td class="paddingL" colspan="2">
                                                                                                <asp:Label ID="lblReportFolder" runat="server" Text='<%#Eval("Report_Folder_Name") %>'
                                                                                                    Font-Bold="true" CssClass="ClsLabel"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trReports" runat="server" class="ClsGridRow">
                                                                                            <td>
                                                                                            </td>
                                                                                            <td id="tdReports" runat="server" colspan="2">
                                                                                                <asp:ListView ID="lstvwReports" runat="server" DataKeyNames="Report_Id,HasAccess,HasFullAccess,IsViewAvailable"
                                                                                                    OnItemDataBound="lstvwReports_ItemDataBound">
                                                                                                    <LayoutTemplate>
                                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                            cellspacing="1">
                                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </LayoutTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                                                            <td class="paddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    CssClass="ClsLabel" />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                    <AlternatingItemTemplate>
                                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                                            <td class="paddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    CssClass="ClsLabel" />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </AlternatingItemTemplate>
                                                                                                </asp:ListView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                                            </td>
                                                                                            <td class="paddingL" colspan="2">
                                                                                                <asp:Label ID="lblReportFolder" runat="server" Text='<%#Eval("Report_Folder_Name") %>'
                                                                                                    Font-Bold="true" CssClass="ClsLabel"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trReports" runat="server" class="ClsGridRow">
                                                                                            <td>
                                                                                            </td>
                                                                                            <td id="tdReports" runat="server" colspan="2">
                                                                                                <asp:ListView ID="lstvwReports" runat="server" DataKeyNames="Report_Id,HasAccess,HasFullAccess,IsViewAvailable"
                                                                                                    OnItemDataBound="lstvwReports_ItemDataBound">
                                                                                                    <LayoutTemplate>
                                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                            cellspacing="1">
                                                                                                            <tr id="itemPlaceholder" runat="server">
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </LayoutTemplate>
                                                                                                    <ItemTemplate>
                                                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                                                            <td class="paddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    CssClass="ClsLabel" />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </ItemTemplate>
                                                                                                    <AlternatingItemTemplate>
                                                                                                        <tr id="trAltItem" runat="server" class="ClsGridRow">
                                                                                                            <td class="paddingL">
                                                                                                                <asp:CheckBox ID="chkReportName" runat="server" Text='<%#Eval("Report_Display_Name") %>'
                                                                                                                    CssClass="ClsLabel" />
                                                                                                            </td>
                                                                                                            <td align="center" style="width: 100px;">
                                                                                                                <asp:CheckBox ID="chkHasFullAccess" runat="server" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </AlternatingItemTemplate>
                                                                                                </asp:ListView>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                           <Triggers>
                                                                <asp:PostBackTrigger ControlID="imgBtnSubmit" />
                                                                <asp:AsyncPostBackTrigger ControlID="imgBtnCancel" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </cc1:CollapsablePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="1" style="padding-top: 5px">
                                                    <asp:UpdatePanel ID="UpdatePanel14" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Button CssClass="ClsBtn" ID="imgBtnSubmit" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                                                BorderWidth="1px" OnClick="imgBtnSubmit_Click" disable-page="true" ValidationGroup="Save">
                                                            </asp:Button>&nbsp;<asp:Button CssClass="ClsBtn" ID="imgBtnCancel" CausesValidation="false"
                                                                runat="server" Text="<%$ Resources:LocalizedResources, Clear%>" BorderWidth="1px"
                                                                OnClick="imgBtnCancel_Click"></asp:Button>
                                                                <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                                                        </ContentTemplate>                                                       
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr align="center" valign="top" runat="server" id="tr2">
                                                <td align="center">
                                                    <cc1:CollapsablePanel ID="colpnlAdditionalDetails" runat="server" TitleText="<%$ Resources:LocalizedResources, AdditionalFieldsDetail%>"
                                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                                        Collapsed="true" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                                        <table width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                     <asp:UpdatePanel ID="UpdatePanel18" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ValidationSummary ID="ValSumAdditionalError" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                                                                ShowSummary="true" ValidationGroup="ValidateAdditional" />
                                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateAdditionalFields"
                                                                                CssClass="ClsMdtStar" Display="None" ValidationGroup="ValidateAdditional"></asp:CustomValidator>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>   
                                                            <tr>
                                                                <td colspan="1" class="ClsTextNormal" align="center">
                                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <table border="0" cellpadding="1" align="center" style="margin:0px auto;" cellspacing="2" style="width: 80%;">                                                                                                                                                           
                                                                                <tr>
                                                                                    <td align="center" colspan="2">
                                                                                        <asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Conditional">
                                                                                            <ContentTemplate>
                                                                                                <asp:Label ID="lblAdditionalMessage" runat="server" ForeColor="Blue" Font-Size="12px"
                                                                                                    Font-Bold="true" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                                                                            </ContentTemplate>
                                                                                            <Triggers>
                                                                                                <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                                            </Triggers>
                                                                                        </asp:UpdatePanel>
                                                                                    </td>
                                                                                </tr>                                                                                
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight" style="width:200px;">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblBloodgroup" runat="server" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, BloodGroup %>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:DropDownList ID="cmbBloodGroup" runat="server" CssClass="MidCombo" AutoPostBack="false">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblMartialStatus" runat="server" EnableViewState="False"
                                                                                            Text=" Marital Status"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:DropDownList ID="cmbMartialStatus" runat="server" CssClass="MidCombo" AutoPostBack="false">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblReligion" runat="server" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, Religion %>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:DropDownList ID="cmbReligion" runat="server" CssClass="MidCombo" AutoPostBack="false">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblCategory" runat="server" EnableViewState="False"
                                                                                            Text="<%$ Resources:LocalizedResources, Category %>"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="MidCombo" AutoPostBack="false">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblCast" runat="server" Text="Cast"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="txtCast" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" class="ClsBorderLight">
                                                                                        <asp:Label CssClass="ClsLabel" ID="lblAadharNumber" runat="server" EnableViewState="False"
                                                                                            Text="Aadhar Number"></asp:Label>
                                                                                        <span class="ClsLabel colonPadding">:</span>
                                                                                    </td>
                                                                                    <td align="left">
                                                                                        <asp:TextBox ID="txtAadharNumber" runat="server" CssClass="MidTxtBox" MaxLength="12"
                                                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                        onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                                        ondrop="event.returnValue=false"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center" colspan="2" style="padding-top: 5px">
                                                                                        <asp:Button CssClass="ClsBtn" ID="btnSave" runat="server" Text="Save" BorderWidth="1px"
                                                                                            disable-page="true" OnClick="btnSave_Click" ValidationGroup="ValidateAdditional">
                                                                                        </asp:Button>
                                                                                        <asp:Button CssClass="ClsBtn" ID="btnClear" CausesValidation="false" runat="server"
                                                                                            Text="<%$ Resources:LocalizedResources, Clear%>" BorderWidth="1px" OnClick="btnClear_Click">
                                                                                        </asp:Button>                                                                                        
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </cc1:CollapsablePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr align="center" valign="top" runat="server" id="tr3">
                                                <td align="center">
                                                    <cc1:CollapsablePanel ID="colpnlEducationalDetails" runat="server" TitleText="<%$ Resources:LocalizedResources, EducationalQualificationDetails%>"
                                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                                        Collapsed="true" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%" align="center">
                                                                    <tr>
                                                                        <td>
                                                                             <asp:UpdatePanel ID="UpdatePanel19" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                         <asp:ValidationSummary ID="valSumEducationalDetails" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                                                                                   ShowSummary="true" ValidationGroup="ValidateEducation" />
                                                                                          <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateEducationalFields"
                                                                                              CssClass="ClsMdtStar" Display="None" ValidationGroup="ValidateEducation"></asp:CustomValidator>
                                                                                    </ContentTemplate>
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="lstvwEducationalDetails" EventName="ItemCommand" />
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnEducationSave" EventName="Click" />
                                                                                        <asp:AsyncPostBackTrigger ControlID="btnEclear" EventName="Click" />
                                                                                    </Triggers>
                                                                                </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr align="center">
                                                                        <td align="center" colspan="2">
                                                                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <asp:Label ID="lblEducationMessage" ForeColor="Blue" Font-Size="12px" Font-Bold="true"
                                                                                        runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>
                                                                                </ContentTemplate>
                                                                                <Triggers>
                                                                                    <asp:AsyncPostBackTrigger ControlID="lstvwEducationalDetails" EventName="ItemCommand" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnEducationSave" EventName="Click" />
                                                                                    <asp:AsyncPostBackTrigger ControlID="btnEclear" EventName="Click" />
                                                                                </Triggers>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>                                                                    
                                                                    <tr>
                                                                        <td align="center">
                                                                            <table width="70%">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <span class="ClsLabel">
                                                                                            <asp:Label ID="Label33" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Qualification %>"></asp:Label>
                                                                                            <span class="colonPadding">:</span></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 16%;">
                                                                                        <asp:DropDownList ID="cmbQualification" runat="server" CssClass="MidTxtBox">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td style="width: 132px">
                                                                                        <span class="ClsLabel">
                                                                                            <asp:Label ID="Label333" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Specialization %>"></asp:Label>
                                                                                            <span class="colonPadding">:</span></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 13%;">
                                                                                        <asp:TextBox ID="txtSpecialization" CssClass="SmlTxtBox" runat="server" MaxLength="100" />
                                                                                    </td>
                                                                                    <td align="left" style="width: 50%;">
                                                                                        <span class="ClsLabel">
                                                                                            <asp:Label ID="Label35" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, YearOfPassing %>"></asp:Label>
                                                                                            <span class="colonPadding">:</span></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 23%;">
                                                                                        <asp:TextBox ID="txtYearOfPassing" CssClass="SmlTxtBox" runat="server" MaxLength="4"
                                                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                            onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                                            ondrop="event.returnValue=false" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" style="width: 20%;">
                                                                                        <span class="ClsLabel">
                                                                                            <asp:Label ID="Label36" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ClassGrade %>"></asp:Label>
                                                                                            <span class="colonPadding">:</span></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 25%;">
                                                                                        <asp:DropDownList ID="cmbPassingClass" runat="server" CssClass="LrgTxtBox">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="left" style="width: 15%;">
                                                                                        <span class="ClsLabel">
                                                                                            <asp:Label ID="Label37" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, University %>"></asp:Label>
                                                                                            <span class="colonPadding">:</span></span>
                                                                                    </td>
                                                                                    <td align="left" style="width: 23%;">
                                                                                        <asp:TextBox ID="txtPassingUniversity" runat="server" MaxLength="100" CssClass="MidTxtBox"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" colspan="4" style="padding-top: 5px">
                                                                            <asp:Button CssClass="ClsBtn" ID="btnEducationSave" runat="server" Text="Save" BorderWidth="1px"
                                                                                disable-page="true" ValidationGroup="ValidateEducation" OnClick="btnEducationSave_Click"
                                                                                CausesValidation="true"></asp:Button>
                                                                            <asp:Button CssClass="ClsBtn" ID="btnEclear" CausesValidation="false" runat="server"
                                                                                Text="<%$ Resources:LocalizedResources, Clear%>" BorderWidth="1px" OnClick="btnEclear_Click">
                                                                            </asp:Button>
                                                                            <asp:HiddenField ID="hidEducationId" runat="server" Value="0" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwEducationalDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnEducationSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnEclear" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <table align="center">
                                                            <tr align="center">
                                                                <td align="center">
                                                                    <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:ListView ID="lstvwEducationalDetails" runat="server" DataKeyNames="EducationId"
                                                                                OnItemCommand="lstvwEducationalDetails_ItemCommand" OnItemDataBound="lstvwEducationalDetails_ItemDataBound"
                                                                                OnItemDeleting="lstvwEducationalDetails_ItemDeleting" OnItemEditing="lstvwEducationalDetails_ItemEditing"
                                                                                OnSelectedIndexChanged="lstvwEducationalDetails_SelectedIndexChanged">
                                                                                <LayoutTemplate>
                                                                                    <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                        <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                                                            <th align="left" class="clsLabelgrd">
                                                                                                <span><b>Qualification</b></span>
                                                                                            </th>
                                                                                            <th align="center" width="150px" class="clsLabelgrd">
                                                                                                <span><b>Specialization</b></span>
                                                                                            </th>
                                                                                            <th align="center" class="clsLabelgrd" width="150px">
                                                                                                <span><b>Year Of Passing</b></span>
                                                                                            </th>
                                                                                            <th align="right" class="clsLabelgrd" width="100px" style="padding-right: 5px;">
                                                                                                <span><b>Class</b></span>
                                                                                            </th>
                                                                                            <th align="center" class="clsLabelgrd" width="150px">
                                                                                                <span><b>University</b></span>
                                                                                            </th>
                                                                                            <th width="40px" align="center" class="clsLabelgrd">
                                                                                                <asp:Label ID="lblEdit" runat="server" Text="Edit" ToolTip="<%$ Resources:LocalizedResources, Edit%>"> </asp:Label>
                                                                                            </th>
                                                                                            <th width="40px" align="center" class="clsLabelgrd">
                                                                                                <asp:Label ID="lblDelete" runat="server" Text="Delete" ToolTip="<%$ Resources:LocalizedResources, Delete%>"> </asp:Label>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblStanderd" runat="server" CssClass="ClsLabel" Text='<%#Eval("Qualification") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("Specialization") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("YearOfPassing") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                                                                padding-right: 5px;" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblDOBMin" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("University") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("Qualification") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("Specialization") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("YearOfPassing") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblAmount" runat="server" CssClass="ClsLabel" Style="float: inherit;
                                                                                                padding-right: 5px;" Text='<%#Eval("ClassName") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblDOBMin" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                                                                Text='<%#Eval("University") %>'></asp:Label>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="lstvwEducationalDetails" EventName="ItemCommand" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnEducationSave" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnEclear" EventName="Click" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </cc1:CollapsablePanel>
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
                            <table cellpadding="0" cellspacing="0">
                                <tr align="center" valign="top" runat="server" id="tr4">
                                    <td align="center">
                                        <cc1:CollapsablePanel ID="colpnlWorkExperienceDetails" runat="server" TitleText="<%$ Resources:LocalizedResources, WorkExperienceDetails%>"
                                            TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                            CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                            Collapsed="true" SlideSpeed="25" Height="100%" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                            <table width="100%" align="center">
                                                <tr>
                                                    <td>
                                                        <asp:UpdatePanel ID="UpdatePanel20" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ValidationSummary ID="valSumExperianceDetails" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                                                   ShowSummary="true" ValidationGroup="ValidateExperiance" />
                                                                <asp:CustomValidator ID="CustomValidator3" runat="server" ClientValidationFunction="ValidateExperianceFields"
                                                                   CssClass="ClsMdtStar" Display="None" ValidationGroup="ValidateExperiance"></asp:CustomValidator>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnExpClear" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr align="center">
                                                    <td id="tdAchievemenMessage" runat="server" align="center" colspan="2">
                                                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblSuccessMsg" ForeColor="Blue" Font-Size="12px" Font-Bold="true" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"></asp:Label>                                                                
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnExpClear" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>                                               
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="60%">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label25" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Achievements %>">
                                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                                        </td>
                                                                        <td align="left" colspan="3" style="height: 41px">
                                                                            <asp:TextBox ID="txtAchivements" runat="server" TextMode="MultiLine" Width="578px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 20%;">
                                                                        </td>
                                                                        <td>
                                                                            <span class="LblSmlGray">(<asp:Label ID="Label26" runat="server" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Year %>">
                                                                            </asp:Label>)</span> <span class="LblSmlGray">(<asp:Label ID="Label27" runat="server"
                                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, Months %>">
                                                                            </asp:Label>)</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 20%;">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label28" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PastExperience %>">
                                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                                        </td>
                                                                        <td align="left" style="width: 25%;">
                                                                            <asp:TextBox ID="txtExpYears" MaxLength="2" CssClass="ExSmlTxtBox" runat="server"
                                                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                                ondrop="event.returnValue=false" Text="00" />
                                                                            <asp:TextBox ID="txtExpMonths" CssClass="ExSmlTxtBox" runat="server" MaxLength="2"
                                                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                                onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                                                ondrop="event.returnValue=false" Text="00" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 20%;">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label29" runat="server" EnableViewState="False" Text="Organization Name">
                                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                                        </td>
                                                                        <td align="left" style="width: 23%;" colspan="3">
                                                                            <asp:TextBox ID="txtSchoolname" runat="server" MaxLength="100" CssClass="MidTxtBox"
                                                                                Width="578px" CausesValidation="true"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 20%;">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label30" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoinedDate %>">
                                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                                        </td>
                                                                        <td align="left" style="width: 25%;">
                                                                            <asp:TextBox ID="txtjoinedDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="calender_JoinDate" runat="server" Culture="en-US" Control="txtjoinedDate"
                                                                                To-Today="true" Enabled="true" ShowErrorMessage="false" Format="dd MMM yyyy"
                                                                                To-Message="<%$ Resources:LocalizedResources, JoinedDateValidation %>" From-Message="<%$ Resources:LocalizedResources, JoinedDateValidation %>"
                                                                                ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, JoinedDateValidation %>" />
                                                                        </td>
                                                                        <td align="left" style="width: 15%;">
                                                                            <span class="ClsLabel">
                                                                                <asp:Label ID="Label31" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeftDate %>">
                                                                                        <span class="colonPadding"> :</span>
                                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                                        </td>
                                                                        <td align="left" style="width: 25%;">
                                                                            <asp:TextBox ID="txtLeftDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="calender_LeftDate" runat="server" Control="txtLeftDate" To-Today="true"
                                                                                ValidationGroup="valGrpExpDetails" Culture="en-US" ShowErrorMessage="false" Format="dd MMM yyyy"
                                                                                To-Message="<%$ Resources:LocalizedResources, LeftDateValidation %>" From-Message="<%$ Resources:LocalizedResources, LeftDateValidation %>"
                                                                                ShowWeekend="True" Enabled="true" InvalidDateMessage="<%$ Resources:LocalizedResources, LeftDateValidation %>" />
                                                                        </td>
                                                                    </tr>
                                                                     <tr>
                                          
                                                    <td style="width: 132px">
                                                    <span class="ClsLabel"><asp:Label ID="Label58" runat="server" EnableViewState="False" Text="Designation"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                    </td>
                                                     <td align="left" style="width: 13%;">
                                                     <asp:TextBox ID="txtDesignation" CssClass="MidTxtBox" runat="server" MaxLength="100" style="width:220px;"
                                                                />
                                                                </td>
                                                                 <td align="left" style="width: 15%;">
                                                        <span class="ClsLabel"><asp:Label ID="Label59" runat="server" EnableViewState="False" Text="Last Salary"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                         </td>
                                                         <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtLastSalary" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                            </tr>
                                             <tr>
                                             
                                                               
                                                  <td align="left" style="width: 15%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label60" runat="server" EnableViewState="False" Text="Duration"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                  </td>
                                                  <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtDuration" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                                           <td align="left" style="width: 15%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label61" runat="server" EnableViewState="False" Text="Job Description"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                  </td>
                                                  <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtJobDescription" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                              
                                                         </td>
                                                         </tr>
                                                         <tr>
                                                        
                                                          <td align="left" style="width: 15%;">
                                                            <span class="ClsLabel"><asp:Label ID="Label62" runat="server" EnableViewState="False" Text="Reason For Leaving"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                          </td>
                                                           <td align="left" style="width: 23%;">
                                                            <asp:TextBox ID="txtReasonForLeaving" CssClass="MidTxtBox" runat="server" style="width:220px;" ></asp:TextBox>
                                                           </td>

                                            </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnExpClear" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                        <table>
                                                            <tr>
                                                                <td align="center" colspan="4" style="padding-top: 5px">
                                                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                                        <ContentTemplate>
                                                                            <asp:Button CssClass="ClsBtn" ID="btnSaveExperiance" runat="server" Text="Save" BorderWidth="1px"
                                                                                disable-page="true" ValidationGroup="ValidateExperiance" OnClick="BtnSaveExperiance_Click"
                                                                                CausesValidation="true"></asp:Button>
                                                                            <asp:Button CssClass="ClsBtn" ID="btnExpClear" CausesValidation="false" runat="server"
                                                                                Text="<%$ Resources:LocalizedResources, Clear%>" BorderWidth="1px" OnClick="btnExpClear_Click">
                                                                            </asp:Button>
                                                                            <asp:HiddenField ID="hidExperienceDetailsId" runat="server" Value="0" />
                                                                        </ContentTemplate>
                                                                        <Triggers>
                                                                            <asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />
                                                                            <asp:AsyncPostBackTrigger ControlID="btnExpClear" EventName="Click" />
                                                                        </Triggers>
                                                                    </asp:UpdatePanel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <table width="60%" align="center">
                                                <tr align="center">
                                                    <td align="center" colspan="4">
                                                        <asp:UpdatePanel ID="UpdatePanel7" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:ListView ID="lstvwExpDetails" runat="server" DataKeyNames="ExperianceId" OnItemDataBound="lstvwExpDetails_ItemDataBound"
                                                                    OnItemCommand="lstvwExpDetails_ItemCommand" OnItemDeleting="lstvwExpDetails_ItemDeleting"
                                                                    OnItemEditing="lstvwExpDetails_ItemEditing" OnSelectedIndexChanged="lstvwExpDetails_SelectedIndexChanged">
                                                                    <LayoutTemplate>
                                                                        <table align="center" width="100%" runat="server" id="tblExperienceInfo" style="color: #333333"
                                                                            cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" style="width: 40%;">
                                                                                    <asp:Label ID="Label25" runat="server" EnableViewState="False" Text="Organasation Name">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                <th align="center" style="width: 100px;">
                                                                                    <asp:Label ID="Label26" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoinedDate %>">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                <th align="center" style="width: 100px;">
                                                                                    <asp:Label ID="LabelLeftDateText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeftDate %>">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                 <th align="center" style="width: 100px;">
                                                                                    <asp:Label ID="Label6" runat="server" EnableViewState="False" Text="Designation">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                 <th align="center" style="width: 100px;">
                                                                                    <asp:Label ID="Label7" runat="server" EnableViewState="False" Text="Duration">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                <th align="center" style="width: 50px;">
                                                                                    <asp:Label ID="LabelEditText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Edit %>">
                                                                                    </asp:Label>
                                                                                </th>
                                                                                <th align="center" style="width: 50px;">
                                                                                    <asp:Label ID="LabelDeleteText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Delete %>">
                                                                                    </asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr runat="server" id="itemPlaceholder">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                            <td align="left">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("OrganisationName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="lblJoinDate" runat="server" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="lblLeftDate" runat="server" Text='<%#Eval("LeftDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("PreviousDesignation") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("Duration") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 50px;">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center" style="width: 50px;">
                                                                                <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                    ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                            <td align="left">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%#Eval("OrganisationName") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 250px;">
                                                                                <asp:Label ID="lblJoinDate" runat="server" Text='<%#Eval("JoiningDate") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 250px;">
                                                                                <asp:Label ID="lblLeftDate" runat="server" Text='<%#Eval("LeftDate") %>'></asp:Label>
                                                                            </td>
                                                                             <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="Label8" runat="server" Text='<%#Eval("PreviousDesignation") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 100px;">
                                                                                <asp:Label ID="Label9" runat="server" Text='<%#Eval("Duration") %>'></asp:Label>
                                                                            </td>
                                                                            <td align="center" style="width: 50px;">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                    ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center" style="width: 50px;">
                                                                                <asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
                                                                                    runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </AlternatingItemTemplate>
                                                                </asp:ListView>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />                                                                
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </cc1:CollapsablePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <table>
            <tr align="center">
                <asp:Button CssClass="ClsBtn" ID="btnBack" CausesValidation="false" runat="server"
                    Text="Back" BorderWidth="1px" OnClick="btnBack_Click" />
            </tr>
            <%--<tr>
                <td>
                    <asp:UpdatePanel ID="UpdatePanel17" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                        </ContentTemplate>
                        <Triggers>                            
                            <asp:AsyncPostBackTrigger ControlID="imgBtnSubmit" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnEducationSave" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnSaveExperiance" EventName="Click" />                            
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>
        </table>
        <asp:HiddenField ID="hidSupervisorId" runat="server" />
        <asp:HiddenField ID="hidIsConfig" runat="server" />
        <asp:HiddenField ID="hidScreenAccess" runat="server" />
        <asp:HiddenField ID="hidCountSchoolConfig" runat="server" />
        <asp:HiddenField ID="hidCountOtherNode" runat="server" />
        <asp:HiddenField ID="hidServerDate" runat="server" />
        <asp:HiddenField ID="hidFilePath" runat="server" />
        <asp:HiddenField ID="hidRetirementAge" runat="server" Value="0" />
        <asp:HiddenField ID="hidRetAge" runat="server" />
        <asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidAgeValidationCondition" runat="server" />
        <asp:HiddenField ID="hidInvalidFileFormat" runat="server" />
        <asp:HiddenField ID="hidDateOfBirthFutureDate" runat="server" />
        <asp:HiddenField ID="hidAddressBlank" runat="server" />
        <asp:HiddenField ID="hidvalLegthOfAddress" runat="server" />
        <asp:HiddenField ID="hidMobileNoVal" runat="server" />
        <asp:HiddenField ID="hidMobileDigit" runat="server" />
        <asp:HiddenField ID="hidValUserNameBlank" runat="server" />
        <asp:HiddenField ID="hidvalUserNameLength" runat="server" />
        <asp:HiddenField ID="hidvalConfirmPassword" runat="server" />
        <asp:HiddenField ID="hidNoteForPasswordCombination" runat="server" />
        <asp:HiddenField ID="hidValPasswordLengh" runat="server" />
        <asp:HiddenField ID="hidValForPassword" runat="server" />
        <asp:HiddenField ID="hidEmailShouldNotBlank" runat="server" />
        <asp:HiddenField ID="hidEmailValidation" runat="server" />
        <asp:HiddenField ID="hidAgeShouldBeLessThan" runat="server" />
        <asp:HiddenField ID="hidyears" runat="server" />
        <asp:HiddenField ID="hidPassword" runat="server" />
        <asp:HiddenField ID="hidOtherStaff" runat="server" />
        <asp:HiddenField ID="hidTransportStaff" runat="server" />
        <asp:HiddenField ID="hidMode" runat="server" Value="NEW" />
        <asp:HiddenField ID="hidTransportStaffID" runat="server" Value = "0" />
        <asp:HiddenField ID="hidOtherStaffID" runat="server" />
        <asp:HiddenField ID="hidQualificationId" runat="server" />
        <asp:HiddenField ID="hidSelectedIndex" runat="server" />
        <asp:HiddenField ID="hidbtnAddText" runat="server" />
        <asp:HiddenField ID="hidbtnAddDetailsText" runat="server" />
        <asp:HiddenField ID="hidSlectedExpIndex" runat="server" />
        <asp:HiddenField ID="hidJoinDate" runat="server" />
        <asp:HiddenField ID="hidLeftDate" runat="server" />
        <asp:HiddenField ID="hidSchoolName" runat="server" />
        <asp:HiddenField ID="hidLocalPincodeShouldNotBlank" runat="server" />
        <asp:HiddenField ID="hidLocalPincodeValidation" runat="server" />
        <asp:HiddenField ID="hidFileUpload" runat="server" />
        <asp:HiddenField ID="hidTransportStaffFields" runat="server" Value="0" />
        <asp:HiddenField ID="hidLicensceExpDate" runat="server" Value="" />
        <asp:HiddenField ID="hidCurrentDate" runat="server" Value="" />
        <!-- Data Insert End Here -->
    </table>
    <script type="text/javascript" lang = "javascript">
        _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>";
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _clienttxtPasswdId = "<%=this.txtPasswd.ClientID %>";
        _clientcstValPasswdId = "<%=this.cstValPassword.ClientID %>";
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
        _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>"
        _clienttxtUserName = "<%=this.txtUserName.ClientID %>";
        _clientcst_UserNameId = "<%=this.cst_UserName.ClientID %>";
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clientcstbtnSubmit = "<%=this.imgBtnSubmit.ClientID%>";
        _clientcstBtnCancel = "<%=this.imgBtnCancel.ClientID%>";
        _clienthidCountSchoolConfigId = "<%=this.hidCountSchoolConfig.ClientID%>";
        _clienthidCountOtherNodeId = "<%=this.hidCountOtherNode.ClientID%>";
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID%>";
        _clienttxtDOB = "<%=this.txtDOB.ClientID %>";
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clientcstBirthDate = "<%=this.cstBirthDate.ClientID%>";
        _clientlstvwReportFolders = "<%=this.lstvwReportFolders.ClientID %>";
        _clientUploadPhoto = "<%=this.UploadPhoto.ClientID%>";
        _ClientCustPhoto = "<%=this.CustPhoto.ClientID %>";
        _clientchkCanSelfApprove = "<%= this.chkCanSelfApprove.ClientID %>";
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _clienthidRetirementAge = "<%=this.hidRetirementAge.ClientID %>"
        _clientcal_DOB = "<%=this.txtDOB.ClientID %>";
        _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
        _clienthidRetAge = "<%=this.hidRetAge.ClientID %>";
        _clientcmbBloodGroup = "<%=this.cmbBloodGroup.ClientID %>";
        _clientcmbMartialStatus = "<%=this.cmbMartialStatus.ClientID %>";
        _clientcmbReligion = "<%=this.cmbReligion.ClientID %>";
        _clientcmbCategory = "<%=this.cmbCategory.ClientID %>";
        _clienttxtCast = "<%=this.txtCast.ClientID %>";
        _clienttxtAadharNumber = "<%=this.txtAadharNumber.ClientID %>";
        _clientcmbQualification = "<%=this.cmbQualification.ClientID %>";
        _clienttxtSpecialization = "<%=this.txtSpecialization.ClientID %>";
        _clienttxtYearOfPassing = "<%=this.txtYearOfPassing.ClientID %>";
        _clientcmbPassingClass = "<%=this.cmbPassingClass.ClientID %>";
        _clienttxtPassingUniversity = "<%=this.txtPassingUniversity.ClientID %>";
        _clienttxtAchivements = "<%=this.txtAchivements.ClientID %>";
        _clienttxtExpYears = "<%=this.txtExpYears.ClientID %>";
        _clienttxtExpMonths = "<%=this.txtExpMonths.ClientID %>";
        _clienttxtSchoolname = "<%=this.txtSchoolname.ClientID %>";
        _clienttxtLeftDate = "<%=this.txtLeftDate.ClientID %>";
        _clienttxtjoinedDate = "<%=this.txtjoinedDate.ClientID %>";
        _clienthidUserId = "<%=this.hidUserId.ClientID %>";
        _clienttxtLocalPincode = "<%=this.txtLocalPincode.ClientID %>";
        _clientcst_LocalPin = "<%=this.cst_LocalPin.ClientID %>";


        //This function is used to validate pin code.
        function PinCodeValidation(oSrc, args) {
        
            var pinCode = document.getElementById(_clienttxtLocalPincode).value;
            pinCode = stripLeadingTrailingBlanks(pinCode);

            if (pinCode != '') {
                if (pinCode.length != 6) {

                    document.getElementById(_clientcst_LocalPin).errormessage = "Pin code should be of 6 digits"
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        } 
        //This function is used to validate DOB.
        function DOBValidation(oSrc, args) {

            var oDOBObj;
            var RetirementAge = $get(_clienthidRetirementAge).value;
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).innerText = "";
                document.getElementById(_clientlblUpdateSucess).innerHTML = "";
            }
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
                oSrc.errormessage = document.getElementById("<%=this.hidAgeValidationCondition.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;

            return false;
        }


        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientcstbtnSubmit)) {
                var isPageValid = true;

                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate();
                }

            }

        }


        function ValidateBirthDate(source, args) {
            var bIsValid = true;

            if (document.getElementById(_clienttxtDOB).value != "") {
                var serverDate = document.getElementById(_clientServerDate).value;
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtDOB).value));
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    document.getElementById(_clientcstBirthDate).errormessage =
                                    document.getElementById("<%=this.hidDateOfBirthFutureDate.ClientID %>").value
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CountCheckedNode(iNodeLength, stvwName) {
            var iIsCheked = 0;
            for (iChildNode = 0; iChildNode < iNodeLength; iChildNode++) {
                var tvNodeId;
                if (iChildNode < 10) {
                    tvNodeId = stvwName + iChildNode + "CheckBox";
                }
                else if (iChildNode >= 10) {
                    tvNodeId = stvwName + iChildNode + "CheckBox";
                }

                if (document.getElementById(tvNodeId) != null) {
                    if (document.getElementById(tvNodeId).checked == true) {
                        iIsCheked++;
                    }
                }
            }
            return iIsCheked;
        }

        function ResetErrorMsgLbl() {
            if (document.getElementById(_clientcst_LblErrMsg) != null) {
                document.getElementById(_clientcst_LblErrMsg).innerText = " ";
                document.getElementById(_clientcst_LblErrMsg).innerHTML = " ";
            }
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).innerText = " ";
                document.getElementById(_clientlblUpdateSucess).innerHTML = " ";
            }
        }


        var blanks = " \t\n\r";  // Ek whitespace chars
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

        function UserNameValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtUserName).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (sEmail.length == 0) {

                oSrc.errormessage = document.getElementById("<%=this.hidValUserNameBlank.ClientID %>").value;
                document.getElementById(_clientcst_UserNameId).errormessage = document.getElementById("<%=this.hidValUserNameBlank.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            if (sEmail.length < 6) {
                oSrc.errormessage = document.getElementById("<%=this.hidvalUserNameLength.ClientID %>").value;
                document.getElementById(_clientcst_UserNameId).errormessage = document.getElementById("<%=this.hidvalUserNameLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }

        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>";

        function MobileNumberValidation(oSrc, args) {

            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
            document.getElementById(_clientcst_MobileNumber).errormessage = "";
            if (sMobileNumber.length == 0) {

            }

            else if (sMobileNumber.length < 10) {
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

        function clickButton(e) {

            var evt = e ? e : window.event;


            if (evt.keyCode == 13) {

                return false;
            }
        }
    
    </script>
    <script lang="javascript" type="text/javascript">
        _ClienttxtAddress = "<%=this.txtAddress.ClientID %>"
        _clientcstValAddress = "<%=this.cstValAddress.ClientID %>"
        _clientcmbDesignation = "<%=this.cmbDesignation.ClientID %>"
        _clientchkCanDeleteVoucher = "<%=this.chkCanDeleteVoucher.ClientID %>"
        _clientchkFinancialYearChangeApplicable = "<%=this.chkFinancialYearChangeApplicable.ClientID %>"
        _chkCanEditOldFinancialYear = "<%=this.chkCanEditOldFinancialYear.ClientID %>";

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

        function ValidateAdditionalFields(oSrc, args) {            
            var BloodGroup = document.getElementById(_clientcmbBloodGroup).value;
            var MaritialStatus = document.getElementById(_clientcmbMartialStatus).value;
            var Religion = document.getElementById(_clientcmbReligion).value;
            var Category = document.getElementById(_clientcmbCategory).value;
            var Castt = document.getElementById(_clienttxtCast).value;
            var AadharCardNo = document.getElementById(_clienttxtAadharNumber).value;
            var UserId = document.getElementById(_clienthidUserId).value;

            if (UserId != 0) {
                if (BloodGroup == 0 && MaritialStatus == 0 && Religion == 0 && Category == 0 && Castt == "" && AadharCardNo == "") {
                    oSrc.errormessage = "At least one Additional details should be entered.";
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                oSrc.errormessage = "Basic details of user should be added first.";
                args.IsValid = false
                return true
            }
        }

        function ValidateEducationalFields(oSrc, args) {            
            var Qualification = document.getElementById(_clientcmbQualification).value;
            var Specialization = document.getElementById(_clienttxtSpecialization).value;
            var YearOfPassing = document.getElementById(_clienttxtYearOfPassing).value;
            var PassingClass = document.getElementById(_clientcmbPassingClass).value;
            var PassingUniver = document.getElementById(_clienttxtPassingUniversity).value;
            var UserId = document.getElementById(_clienthidUserId).value;

            if (UserId != 0) {
                if (Qualification == 0 && Specialization == "" && YearOfPassing == "" && PassingClass == 0 && PassingUniver == "") {
                    oSrc.errormessage = "At least one Educational qualification details should be entered.";
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                oSrc.errormessage = "Basic details of user should be added first.";
                args.IsValid = false
                return true
            }
        }

        function ValidateExperianceFields(oSrc, args) {            
            var Achievement = document.getElementById(_clienttxtAchivements).value;
            var PastExperi1 = document.getElementById(_clienttxtExpYears).value;
            var PastExperi2 = document.getElementById(_clienttxtExpMonths).value;
            var Organization = document.getElementById(_clienttxtSchoolname).value;
            var JoinedDate = document.getElementById(_clienttxtjoinedDate).value;
            var LeftDate = document.getElementById(_clienttxtLeftDate).value;
            var UserId = document.getElementById(_clienthidUserId).value;

            if (UserId != 0) {
                if (Achievement == "" && (PastExperi1 == 00 || PastExperi2 == 00) && Organization == "" && JoinedDate == "" && LeftDate == "") {
                    oSrc.errormessage = "At least one Work Experience details should be entered.";
                    args.IsValid = false
                    return true
                }
                else if (JoinedDate == "" && LeftDate == "") {
                    oSrc.errormessage = "Joined date & Left date of Work Experience details should be entered.";
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else {
                oSrc.errormessage = "Basic details of user should be added first.";
                args.IsValid = false
                return true
            }
        }


        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                //check if nxt sibling is not null & is an element node
                if (nxtSibling && nxtSibling.nodeType == 1) {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }
        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }
        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;
            if (parentNodeTable) {
                var checkUncheckSwitch;
                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        return; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = false;
                }
                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }

        }
        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) {
                    //check if the child node is an element node
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            //return true;
            return false;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

        function CheckAllChild(parent, evt, chkName) {
            var check = false;
            if (parent.checked)
                check = true;
            var childChkBoxes = document.getElementsByName(chkName);
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
                if (childChkBoxes[i].id.match("EchkAccess") != null) {
                    childChkBoxes[i].disabled = !check;
                    childChkBoxes[i].checked = false;
                }
            }
        }

        function checkUnCheckEdit(parent, chkName) {
            document.getElementById(chkName).checked = parent.checked;
            document.getElementById(chkName).disabled = !parent.checked;
        }

        function CalculateAccess() {
            var sXml = "<ScreenAccess>"
            var sIds = ''
            var sAbsentIds = ''
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var allChecked = false;
            var totalChecked = 0;

            for (j = 0; j < boxLength; j++) {
                if (checks[j].type == 'checkbox' && checks[j].id.match("chkAccess_") != null && checks[j].id.charAt(0) != "E") {
                    var EditAccessChk = document.getElementById("E" + checks[j].id);
                    sXml = sXml + '<Screen id="' + checks[j].id.split('_')[1] + '" IsDeleted="' + (checks[j].checked ? 'N' : 'Y') + '" CanEdit="' + (EditAccessChk.checked ? 'Y' : 'N') + '"></Screen>';
                }
            }
            sXml = sXml + "</ScreenAccess>";
            $get("<%=hidScreenAccess.ClientID %>").value = sXml;

            return false;

        }

        function SelectUnSelectChilds(row, chkSelect) {
            var iRowIndex = 0;
            var isChecked = chkSelect.checked;
            var reportName = document.getElementById(_clientlstvwReportFolders + "_ctrl" + row + "_lstvwReports_ctrl" + iRowIndex + "_chkReportName")
            while (reportName != null) {
                reportName.checked = isChecked;
                iRowIndex++;
                reportName = document.getElementById(_clientlstvwReportFolders + "_ctrl" + row + "_lstvwReports_ctrl" + iRowIndex + "_chkReportName")
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

        function UpdateHiddenField() {
            $get(_clienthidIsPhotoCaptured).value = "Y";
        }
        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?');
        }

        function ValidateFile(oSrc, args) {
            if ($get("<%=this.flDocument.ClientID %>") != null) {
                var fl = $get("<%=this.flDocument.ClientID %>").value;
                var uploadedFile = $get("<%=this.hidFileUpload.ClientID %>").value
                var isRenew = $get('<%=this.chkRenew.ClientID %>').checked

                if (isRenew && fl == '') {
                    oSrc.errormessage = "Please upload Driver License if need to renew it.";
                    args.IsValid = false;
                    return true;
                }
                else if (fl != "") {
                    var file = $get("<%=this.flDocument.ClientID %>")
                    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                                  fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF"
                                )) {
                        oSrc.errormessage = "Please select valid file type to upload Driver License.";
                        args.IsValid = false;
                        return true;
                    }
                    else if (file.files[0].size >= 5242880) {
                        oSrc.errormessage = "File size should be less than 5 MB."
                        args.IsValid = false
                        return true
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function OpenFile(file) {
            window.open(file, '_blank')
            return false;
        }

        function ValidateLicenseExpiryDate(oSrc, args) {
            if ($get('<%=this.chkRenew.ClientID %>') != null) {
                var isRenew = $get('<%=this.chkRenew.ClientID %>').checked

                if (isRenew) {
                    var expDate = $get('<%=this.txtLicenseExpiryDate.ClientID %>').value
                    var oldExpDate = $get('<%=this.hidLicensceExpDate.ClientID %>').value

                    var dtExpDate;
                    if (document.all)
                        dtExpDate = new Date(expDate.replace('-', ' '));
                    else
                        dtExpDate = new Date(convertdate(expDate));

                    var dtOldExpDate;
                    if (document.all)
                        dtOldExpDate = new Date(oldExpDate.replace('-', ' '));
                    else
                        dtOldExpDate = new Date(convertdate(oldExpDate));

                    if (dtExpDate <= dtOldExpDate) {
                        oSrc.errormessage = 'In renew case, new Driving License expiry date should be greater than current Driving License expiry date.'
                        args.IsValid = false;
                        return true;
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateLicenseExpiryDateForPast(oSrc, args) {
        
            if ($get('<%=this.txtLicenseExpiryDate.ClientID %>') != null) {

                var expDate = $get('<%=this.txtLicenseExpiryDate.ClientID %>').value
                var serverDate = $get('<%=this.hidCurrentDate.ClientID %>').value

                var dtExpDate;
                if (document.all)
                    dtExpDate = new Date(expDate.replace('-', ' '));
                else
                    dtExpDate = new Date(convertdate(expDate));

                var dtServerDate;
                if (document.all)
                    dtServerDate = new Date(serverDate.replace('-', ' '));
                else
                    dtServerDate = new Date(convertdate(serverDate));
                
                if (dtExpDate <= dtServerDate) {
                    oSrc.errormessage = 'Driving License expiry date should not be in past.'
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true
            return false;
        }

    </script>

</asp:Content>
