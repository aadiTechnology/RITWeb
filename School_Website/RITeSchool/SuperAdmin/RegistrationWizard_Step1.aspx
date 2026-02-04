<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    CodeFile="RegistrationWizard_Step1.aspx.cs" Inherits="RegistrationWizard_Step1" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" style="width: 100%;">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center">
                <!-- Data Insert Here -->
                <table style="width: 97%;">
                    <tr runat="server" id="trHeading">
                        <td align="left" colspan="5">
                            <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                <tr>
                                    <td>                                        
                                             <span style="font-weight:bold;border-width:0px" class="MainTitleHead">School Registration</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" ValidationGroup="Save" />
                            <asp:ValidationSummary ID="ValSumIcard" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                ShowSummary="true" ValidationGroup="ICard" />
                            <asp:HiddenField ID="hidSMSSenderName" runat="server" />
                            <asp:HiddenField ID="hidBackUrl" runat="server" />
                            <asp:Label CssClass="LblErrorMsg" ID="lblErrorMsg" runat="server" EnableViewState="false"></asp:Label>
                        </td>
                        <td align="left" colspan="1">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="padding-left: 5px; padding-top: 15px;" class="ClsBtmBorderGray">
                          <asp:Label ID = "lblSchoolInformation" runat = "server" CssClass  = "ClsLblLgnd" Font-Bold  ="true" EnableViewState = "false" Text = "<%$ Resources:LocalizedResources, SchoolInformation%>"></asp:Label>
                        </td>
                        <td align="right" class="ClsBtmBorderGray" colspan="2" style="padding-right: 30px"
                            valign="bottom">
                                <span class="ClsMdtStar">*</span> 
                                <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table width="92%" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="left" style="width: 146px" class="ClsBorderLight">
                        </td>
                        <td align="left" style="width: 90%;">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                             <asp:Label ID="lblSchoolName" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SchoolName%>"
                                                                EnableViewState="False"></asp:Label>
                        <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtSchoolName" runat="server" MaxLength="70" CssClass="LrgTxtBox"
                                Width="400px" TabIndex="1"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">*<asp:RequiredFieldValidator ID="reqFldValSchoolName"
                                runat="server" ControlToValidate="txtSchoolName" Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, SchoolNameShouldNotBlank%>"
                                CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                             <asp:Label ID="lblRegNo" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, RegistrationNumber%>"
                                                                EnableViewState="False"></asp:Label>
                                                                 <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtRegNo" runat="server" MaxLength="48" CssClass="LrgTxtBox" Width="400px"
                                TabIndex="2"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">*<asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                runat="server" ControlToValidate="txtRegNo" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, RegNumberBlank%>" 
                                CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                             <asp:Label ID="lblAddress1" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, Address1%>"
                                                                EnableViewState="False"></asp:Label>
                                                                 <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtAddress1" runat="server" MaxLength="300" CssClass="LrgTxtBox"
                                Width="400px" TabIndex="3"></asp:TextBox>
                            <span style="color: red" enableviewstate="false" class="ClsMdtStar">*
                                <asp:RequiredFieldValidator ID="reqFldValActualAddress" runat="server" ControlToValidate="txtAddress1"
                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, Address1NotBlank%>" CssClass="LblErrorMsg" 
                                    ValidationGroup="Save"></asp:RequiredFieldValidator></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <asp:Label ID="lblAddress2" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, Address2%>"
                                                                EnableViewState="False"></asp:Label>    
                                                                      <span class="ClsLabel colonPadding">:</span>                            
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtAddress2" runat="server" MaxLength="300" CssClass="LrgTxtBox"
                                Width="400px" TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <asp:Label ID="lblCity" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, City%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="LrgTxtBox" TabIndex="5"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">*<asp:RequiredFieldValidator ID="reqFldValCity"
                                runat="server" ControlToValidate="txtCity" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, CityBlank%>" 
                                CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator></span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <asp:Label ID="lblState" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, State%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtState" runat="server" MaxLength="50" CssClass="LrgTxtBox" TabIndex="6"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">*</span>
                            <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="txtState"
                                Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, StateShouldNotBlank%>"  CssClass="LblErrorMsg"  
                                ValidationGroup="Save" Height="16px"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                       <asp:Label ID="lblPIN" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, Pincode%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtPIN" CssClass="LrgTxtBox" runat="server" MaxLength="6" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers(this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="7" />
                            <span style="color: red;" enableviewstate="false" class="ClsMdtStar">*
                                <asp:RequiredFieldValidator ID="rqd_Pin" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, PinBlank%>"    
                                    ControlToValidate="txtPIN" Display="None" CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage="<%$ Resources:LocalizedResources, PinCodeDigit%>"  
                                    ID="cst_PIN" runat="server" ControlToValidate="txtPIN" ClientValidationFunction="PinCodeValidation"
                                    ValidationGroup="Save"></asp:CustomValidator>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblPhone1" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, PhoneNumber1%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtCPhone" runat="server" CssClass="LrgTxtBox" MaxLength="15"
                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" TabIndex="8"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rqd_CPhone" runat="server" ErrorMessage= "<%$ Resources:LocalizedResources, PhoneNo1Blank%>"  
                                ControlToValidate="txtCPhone" Display="None" CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            <span style="color: red" class="ClsMdtStar">*</span>
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                              <asp:Label ID="lblPhone2" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, PhoneNumber2%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtPhoneNo2" runat="server" CssClass="LrgTxtBox" MaxLength="15"
                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" TabIndex="8"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <asp:Label ID="lblEmailAddress" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, EmailAddress%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="LrgTxtBox" MaxLength="100" TabIndex="9"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="reg_Email" runat="server" ControlToValidate="txtEmail"
                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, EmailValidation%>"  
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" CssClass="ClsLabel"
                                ValidationGroup="Save"></asp:RegularExpressionValidator>
                        </td>
                    </tr>

                      <tr id="trFeedback" runat="server">
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblEmailAddressForFeedBack" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, EmailAddressForFeedback %>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtEmails" runat="server" CssClass="LrgTxtBox" 
                                MaxLength="200"  TabIndex="9" TextMode="MultiLine"></asp:TextBox>
                            <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage= "<%$ Resources:LocalizedResources, EnterValidEmailAddress %>"   
                             ID="cstValidateEmail" runat="server" ControlToValidate="txtEmails" ClientValidationFunction="ValidateEmailAddress"
                             ValidationGroup="Save"></asp:CustomValidator>
                             <asp:Label ID="lblEmailAddresses" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, JoinEmailAddress%>"
                                                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>

                     <tr id="trCareer" runat="server">
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                             <asp:Label ID="lblEmailAddressForCareer" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, CareerEmailAddress%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtCareerEmails" runat="server" CssClass="LrgTxtBox" 
                                MaxLength="200"  TabIndex="10" TextMode="MultiLine"></asp:TextBox>
                            <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage= "<%$ Resources:LocalizedResources, EnterValidEmailAddress %>"  
                             ID="cstValidateCareerEmails" runat="server" ControlToValidate="txtCareerEmails" ClientValidationFunction="ValidateCareerEmailAddress"
                             ValidationGroup="Save"></asp:CustomValidator>
                             <asp:Label ID="lblEmailAddresses1" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, JoinEmailAddress%>"
                                                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trForgotPasswordEmail" runat="server">
                        <td align="left" class="ClsBorderLight" style="width: 39%">
                            <asp:Label ID="lblEmailAddressForForgetPass" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, EmailAddressForForgetPassword%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtForgotPassword" runat="server" CssClass="LrgTxtBox" 
                                MaxLength="200"  TabIndex="10" TextMode="MultiLine"></asp:TextBox>
                            <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage= "<%$ Resources:LocalizedResources, EnterValidEmailAddress %>"  
                             ID="cstForgotPassword" runat="server" ControlToValidate="txtForgotPassword" ClientValidationFunction="ValidateForgotPasswordEmailAddress"
                             ValidationGroup="Save"></asp:CustomValidator>
                              <asp:Label ID="Label1" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, JoinEmailAddress%>"
                                                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblFax" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, Fax %>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtFaxNo" runat="server" MaxLength="8" CssClass="LrgTxtBox" onblur="extractNumber(this,0,false);"
                                onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers(this, event, false, false);"
                                onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                            <asp:Label ID="lblWebsite" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, Website %>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" style="color: red;">
                            <asp:TextBox ID="txtWebSite" runat="server" MaxLength="100" CssClass="LrgTxtBox" TabIndex="11"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="Reg_Expr_ValidContent" runat="server" Display="None"
                                ControlToValidate="txtWebSite" ErrorMessage= "<%$ Resources:LocalizedResources, EnterValidWebSite %>" ValidationExpression="([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?"
                                CssClass="ClsLabel" ValidationGroup="Save"> </asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                <asp:Label ID="lblUpload" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, UploadLogo%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1" style="height: 151px">
                            <img id="imgPhoto" alt="image"  runat="server" height="151" width="119"/> 
                            <span style="color: red" class="ClsMdtStar">*</span> <br />
                            <asp:FileUpload ID="UploadLogo" runat="server" TabIndex="12" />
                            
                             <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateLogo"
                                ErrorMessage="<%$ Resources:LocalizedResources, ValBITMapFileFormat%>" CssClass="TxtNormal"
                                ValidationGroup="Save"></asp:CustomValidator>
                           <asp:Label ID="lblBMPFileType" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, BMPFileType%>"
                                                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                             <asp:Label ID="lblSchoolScience" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SchoolSince%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="calSinceDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"
                                TabIndex="13"></asp:TextBox>
                            <rjs:PopCalendar ID="cSinceDate" runat="server" Control="calSinceDate" Format="dd MMM yyyy" Culture = "en"
                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage= "<%$ Resources:LocalizedResources, ValSinceDate%>" />
                            <span style="color: red" enableviewstate="false" class="ClsMdtStar">*</span>&nbsp;
                             <asp:Label ID="lblFoundationDate" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, FoundationDate%>"
                                                                EnableViewState="False"></asp:Label>
                            <asp:CustomValidator ID="cstFoundationDate" runat="server" CssClass="ClsMdtStar"
                                Display="None" EnableClientScript="true" Visible="true" ClientValidationFunction="validateDate"
                                ErrorMessage="<%$ Resources:LocalizedResources, SchoolSinceDate%>"   ValidationGroup="Save"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblSchoolOrganization" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SchoolOrganization%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtSchoolOrgn" runat="server" CssClass="LrgTxtBox" MaxLength="200"
                                Width="400px" TabIndex="14"></asp:TextBox>
                            <span style="color: red" enableviewstate="false" class="ClsMdtStar">*<asp:RequiredFieldValidator
                                ID="reqValOrg" runat="server" ControlToValidate="txtSchoolOrgn" CssClass="LblErrorMsg"
                                Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SchoolOrganizationBlank%>"   ValidationGroup="Save"></asp:RequiredFieldValidator></span>
                        </td>
                    </tr>
                      <tr id="trSMSSender" runat="server">
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblSmsSenderName" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SMSSenderName%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtSMSSenderName" runat="server" MaxLength="8" CssClass="LrgTxtBox"
                                TabIndex="15"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">*<asp:RequiredFieldValidator ID="reqSMSSenderName"
                                runat="server" ControlToValidate="txtSMSSenderName" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValSMSSenderNameBlank%>"   
                                CssClass="LblErrorMsg" ValidationGroup="Save"></asp:RequiredFieldValidator></span>
                        </td>
                    </tr>                    
                    <tr id="trAccount" runat="server" visible="false">
                        <td align="left" class="ClsBorderLight" style="width: 146px">
                           <asp:Label ID="lblAccountNo" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, AccountNo%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" colspan="1">
                            <asp:TextBox ID="txtAccountNo" runat="server" MaxLength="15" CssClass="LrgTxtBox"
                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false"
                                ondrop="event.returnValue=false" TabIndex="16"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">* </span>
                            <asp:RequiredFieldValidator ID="reqAccountNo" runat="server" CssClass="ClsMdtStar"
                                ErrorMessage="<%$ Resources:LocalizedResources, ValAccountNoBlank%>"   Display="None" ControlToValidate="txtAccountNo"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                           <asp:Label ID="lblProfessionalTax" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, ProfessionalTaxRegNo%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtPTRegCertificateNo" runat="server" MaxLength="20" CssClass="LrgTxtBox"
                                TabIndex="17"></asp:TextBox>
                            <span style="color: red" class="ClsMdtStar">* </span>
                            <asp:RequiredFieldValidator ID="reqValPTRegCertificateNo" runat="server" ControlToValidate="txtPTRegCertificateNo"
                                ErrorMessage="<%$ Resources:LocalizedResources, ProTaxRegCertificateNoBlank%>"  
                                ValidationGroup="Save" Display="None"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                           <asp:Label ID="lblSchoolRecgnitionNoPri" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SchoolRecognitionNo_Pri%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtRecognitionNoPri" runat="server" MaxLength="50" CssClass="LrgTxtBox"
                                TabIndex="18"></asp:TextBox>                            
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                             <asp:Label ID="lblSchoolRegNoSec" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SchoolRecognitionNo_Sec%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtRecognitionNoSec" runat="server" MaxLength="50" CssClass="LrgTxtBox"
                                TabIndex="19"></asp:TextBox>                            
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                           <asp:Label ID="lblIndexNo" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, IndexNo%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtIndexNo" runat="server" MaxLength="20" CssClass="LrgTxtBox"
                                TabIndex="20"></asp:TextBox>                               
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                          <asp:Label ID="lblPanNo" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, PanNo%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtPanNo" runat="server" MaxLength="20" CssClass="LrgTxtBox"  onkeypress="return PreventSpecialChars(event);" 
                                TabIndex="21"></asp:TextBox>                              
                        </td>
                    </tr>
                     <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                            <asp:Label ID="lblTanNumber" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, TanNumber%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtTanNo" runat="server" MaxLength="20" CssClass="LrgTxtBox"  onkeypress="return PreventSpecialChars(event);" 
                                TabIndex="22"></asp:TextBox>                                                       
                        </td>
                    </tr>

                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                            <asp:Label ID="lblGSTIN" runat="server" CssClass="ClsLabel" Text= "GSTIN"
                                                             EnableViewState="false"></asp:Label>
                                                                    <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtGSTIN" runat="server" MaxLength="50" CssClass="LrgTxtBox" onkeypress="return PreventSpecialChars(event);"
                                TabIndex="23"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                            <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text= "UDISE Number"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtUDISENumber" runat="server" MaxLength="30" CssClass="LrgTxtBox"  onkeypress="return PreventSpecialChars(event);" 
                                TabIndex="23"></asp:TextBox>                                                       
                        </td>
                    </tr>
                      <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                            <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text= "Lattitude"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtLattitude" runat="server" MaxLength="100" CssClass="LrgTxtBox"  
                                TabIndex="24"></asp:TextBox>                                                       
                        </td>
                    </tr>
                      <tr>
                        <td align="left" class="ClsBorderlight" style="width: 39%">
                            <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text= "Longitude"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                        </td>
                        <td align="left" class="ClsMdtStar" style="width: 61%">
                            <asp:TextBox ID="txtLongitude" runat="server" MaxLength="100" CssClass="LrgTxtBox"  
                                TabIndex="25"></asp:TextBox>                                                       
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkSchoolAccountDetails" runat="server" Text= "<%$ Resources:LocalizedResources, SchoolBankDetails%>" CssClass="ClsLabel"></asp:LinkButton>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="height: 25px; width: 146px;">
                            &nbsp;
                        </td>
                        <td colspan="1" style="height: 25px">
                            <span class="LblSmlGray">For Reliance India Mobile & TataIndicom CDMA (Mobile and Fixed
                                Wireless Phones - FWP [Land Line]) You can use only a Valid Private GSM Mobile Number
                                (in 98 series only) as the Sender</span>
                        </td>
                    </tr>--%>
                    <tr runat="server" id="trTerms">
                        <td style="width: 146px">
                        </td>
                        <td align="Left" colspan="3">
                            <asp:CheckBox ID="chkTermsAndConditions" runat="server" CssClass="LblSmlRslt" Text= "<%$ Resources:LocalizedResources, TermsAndCondition%>"
                                TabIndex="23" />
                            <asp:CustomValidator ID="cstCheckBox" Display="None" runat="server" EnableClientScript="true"
                                ClientValidationFunction="CheckboxValidation" ErrorMessage="<%$ Resources:LocalizedResources, AcceptTermAndCondition%>"    
                                CssClass="LblErrorMsg" ValidationGroup="Save" Height="16px" Width="77px"></asp:CustomValidator>
                            <asp:HyperLink ID="lnkTermsandConditions" runat="server" CssClass="LblSmlGray"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2" style="height: 28px">
                            <asp:Button ID="imgBtnSubmit" CssClass="ClsBtn" runat="server" Text= "<%$ Resources:LocalizedResources, Save%>" BorderWidth="1px"
                                OnClick="imgBtnSubmit_Click" UseSubmitBehavior="false" ValidationGroup="Save"
                                TabIndex="24" />
                            &nbsp;<asp:Button ID="imgBtnCancel" CssClass="ClsBtn" runat="server" CausesValidation="False"
                                Text= "<%$ Resources:LocalizedResources, Cancel%>" BorderWidth="1px" OnClick="imgBtnCancel_Click" UseSubmitBehavior="false"
                                TabIndex="25" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hidFilePath" runat="server" />
                            <asp:HiddenField ID="hidSignPath" runat="server" />
                            <asp:HiddenField ID="hidICardPath" runat="server" />
                            <asp:HiddenField ID ="hidCultureInfo" runat = "server" />
                            <asp:HiddenField ID  = "hidInvalidFileFormat" runat = "server" />
                            <asp:HiddenField ID = "hidValBITMapFileFormat" runat = "server" />
                            <asp:HiddenField ID ="hidSchoolLogoBlank" runat = "server" />
                            <asp:HiddenField ID = "hidCareerEmailAddLength" runat = "server" />
                            <asp:HiddenField ID = "hidValSinceDate" runat = "server" />
                            <asp:HiddenField ID = "hidPinCodeDigit" runat = "server" />
                            <asp:HiddenField ID = "hidSchoolSinceDateFutureDate" runat = "server" />
                            <asp:HiddenField ID = "hidValEmailAddForFeedbackLength" runat  ="server" />
                            <asp:HiddenField ID = "hidValFeedBackEmailAdd" runat = "server" />
                            <asp:HiddenField ID = "hidValEmailAddValid" runat = "server" />
                            <asp:HiddenField ID = "hidAreDuplicated" runat = "server" />
                            <asp:HiddenField ID = "hidvalCareerEmailAddress" runat ="server" />
                            <asp:HiddenField ID = "hidvalForgotPassEmails" runat ="server" />
                            <asp:HiddenField ID = "hidForgotPasswordEmailLength" runat ="server" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: white;" id="Td1" colspan="2">
                            <cc1:CollapsablePanel ID="colpnlPhotoGallery" runat="server" TitleText= "<%$ Resources:LocalizedResources, ICardInformation%>"
                                TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
                                TitleStyle-Height="25px" Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle"
                                TabIndex="26">
                                <table>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                            <asp:Label ID="lblICardAddress" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, IcardAddress%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1">
                                            <asp:TextBox ID="txtAddress" runat="server" MaxLength="300" CssClass="LrgTxtBox"
                                                Width="500px" TabIndex="27"></asp:TextBox>
                                            <span style="color: red" enableviewstate="false" class="ClsMdtStar">*
                                                <asp:RequiredFieldValidator ID="reqFldValAddress" runat="server" ControlToValidate="txtAddress"
                                                    Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ICardAddressBlank%>" CssClass="LblErrorMsg"
                                                    ValidationGroup="ICard"></asp:RequiredFieldValidator></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 146px">
                                            <asp:Label ID="lblUploadPrinciplesSign" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, UploadPrinciplesSign%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 30px;">
                                             <img id="imgSign" alt="image"  runat="server" height="30" width="77"/>                                          
                                            <br />
                                            <asp:FileUpload ID="UploadSign" runat="server" TabIndex="28" />
                                            <asp:CustomValidator ID="CustValidateSign" Display="None" runat="server" ClientValidationFunction="ValidateSignLogo"
                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>"    ControlToValidate="UploadSign" CssClass="LblErrorMsg"
                                                ValidationGroup="ICard"></asp:CustomValidator>
                                            <asp:Label ID="lblNoteForFileType1" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, NoteForFileType1%>"
                                                                EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderLight" style="width: 188px">
                                             <asp:Label ID="lblUploadLogo" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, UploadLogoForICard%>"
                                                                EnableViewState="False"></asp:Label>
                                                                      <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="1" style="height: 50px;">
                                             <img id="imgLogoICard" alt="image"  runat="server" height="50" width="72"/>  
                                            <br />
                                            <asp:FileUpload ID="UploadICard" runat="server" TabIndex="29" />
                                            <asp:CustomValidator ID="CustICardLogo" Display="None" runat="server" ClientValidationFunction="ValidateICardLogo"
                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="UploadICard" CssClass="LblErrorMsg"
                                                ValidationGroup="ICard"></asp:CustomValidator>
                                                  <%--<asp:Label ID="lblFileType" runat="server" CssClass="LblSmlGray" Text= "<%$ Resources:LocalizedResources, NoteForFileType%>"
                                                                EnableViewState="False"></asp:Label>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2" style="height: 28px">
                                            <asp:Button ID="btnSave" ValidationGroup="ICard" CssClass="ClsBtn" runat="server" TabIndex="30"
                                                Text= "<%$ Resources:LocalizedResources, Save%>" BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </cc1:CollapsablePanel>
                        </td>
                    </tr>
                </table>
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        _clientCstPIN = "<%=this.cst_PIN.ClientID %>"
        _clientPinCode = "<%=this.txtPIN.ClientID %>"
        _clientCheckBox = "<%=this.chkTermsAndConditions.ClientID%>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID%>"
        _clientFoundationDate = "<%=this.calSinceDate.ClientID%>"
        _clientcstFoundationDate = "<%=this.cstFoundationDate.ClientID%>"                
        _clientcstbtnSubmit = "<%=this.imgBtnSubmit.ClientID%>"
        _clientimgBtnCancel = "<%=this.imgBtnCancel.ClientID%>"
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>"
        _clientFileUploadLogo = "<%=this.UploadLogo.ClientID%>"
        _clientCustValidateSign = "<%=this.CustValidateSign.ClientID %>"
        _clientUploadSign = "<%=this.UploadSign.ClientID%>"
        _clientCustICardLogo = "<%=this.CustICardLogo.ClientID %>"
        _clientUploadICard = "<%=this.UploadICard.ClientID%>"
        _sClientlblMessageId = "<%=this.lblErrorMsg.ClientID%>"
        _clienthidFilePath = "<%=this.hidFilePath.ClientID %>"
        _clientcstValidateEmail = "<%=this.cstValidateEmail.ClientID %>"
        _clienttxtEmails = "<%=this.txtEmails.ClientID %>"
        _clientcstValidateCareerEmail = "<%=this.cstValidateCareerEmails.ClientID %>"
        _clienttxtCareerEmails = "<%=this.txtCareerEmails.ClientID %>"
        _clienttxtForgotPassword = "<%=this.txtForgotPassword.ClientID %>"
        _clientcstForgotPassword = "<%=this.cstForgotPassword.ClientID %>"   

        fnChkboxClick();

        // the function that will enable or disable the next button
        // according to the terms & conditions checkbox.
        function fnChkboxClick() {
            var btnNext = $get(_clientcstbtnSubmit);
        	if (btnNext.value.indexOf('Next') > -1) {
        		var chkboxTerms = $get(_clientCheckBox);
        		if (chkboxTerms) {
        			btnNext.disabled = !chkboxTerms.checked;
        		}
        	}
        }
        
        function VisibleSuccessMsg() {
            if (document.getElementById(_sClientlblMessageId) != undefined) {
                document.getElementById(_sClientlblMessageId).style.display = "none"
            }
        }
        function DisableButtons(ObjBtn) {
            VisibleSuccessMsg()
            if (ObjBtn == document.getElementById(_clientcstbtnSubmit)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientcstbtnSubmit).disabled = true
                    if (document.getElementById(_clientimgBtnCancel))
                    document.getElementById(_clientimgBtnCancel).disabled = true
                }
            }
            else if (ObjBtn == document.getElementById(_clientimgBtnCancel)) {
                document.getElementById(_clientcstbtnSubmit).disabled = true
                document.getElementById(_clientimgBtnCancel).disabled = true
            }
        }

        function validateDate(source, args) {

            var txtDOL = document.getElementById(_clientFoundationDate).value
            var bIsValid = true
            if (txtDOL == "") {
                bIsValid = false
            }
            if (txtDOL != "") {
                var re = /\b(0?[1-9]|[12][0-9]|3[01])[\-\/]([AFMJSOND][a-z][a-z])[\-\/]((19|20)\d{2})/;
                var matchArray = re.exec(txtDOL);
                if (matchArray == null) {
                    bIsValid = false
                    document.getElementById(_clientcstFoundationDate).errormessage =
                       document.getElementById("<%=this.hidValSinceDate.ClientID %>").value
                }

                if (matchArray != null) {
                    var oCurrDate = new Date()
                    var dtFDt
                    if (document.all)
                        dtFDt = new Date(txtDOL.replace('-', ' '))
                    else
                        dtFDt = new Date(convertdate(txtDOL))
                    var dtCurrDate = oCurrDate
                    if (dtCurrDate < dtFDt) {
                        bIsValid = false
                        document.getElementById(_clientcstFoundationDate).errormessage = document.getElementById("<%=this.hidSchoolSinceDateFutureDate.ClientID %>").value
                    }
                }
            }
            args.IsValid = bIsValid
            return !bIsValid
        }

        function EnableDisableButton(obj) {
            if (obj.checked) {
                document.getElementById(_clientButton).disabled = false
                document.getElementById(_clientButton).CausesValidation = true
            }
            else
                document.getElementById(_clientButton).disabled = true
        }

        function CheckboxValidation(oSrc, args) {
            if (document.getElementById(_clientCheckBox)) {
                if (!document.getElementById(_clientCheckBox).checked) {
                    args.IsValid = false
                    return true
                }
            }
            else {
                args.IsValid = true
                return false
            }
            args.IsValid = true
            return false
        }

        function PinCodeValidation(oSrc, args) {
            var sPIN = document.getElementById(_clientPinCode).value
            if (sPIN.length == 0) {
                document.getElementById(_clientPinCode).value = ""
                args.IsValid = true
                return false
            }
            if (sPIN.length != 6) {
                document.getElementById(_clientCstPIN).ErrorMessage = document.getElementById("<%=this.hidPinCodeDigit.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateEmailAddress(oSrc, args) {            
            var email = (document.getElementById(_clienttxtEmails).value).trim();
            var ArrEmail,txtEmail='';
            var sEmail;
            var sEmail1;
            var sMsg="";
            var sMsg1 = "";
            if (email.trim() != "") {
                ArrEmail = email.split(',');

                for (var j = 0; j < ArrEmail.length; j++) {

                    sEmail1 = ArrEmail[j];
                    if (sEmail1 != "")
                        txtEmail = txtEmail + sEmail1 + ",";

                    if (sEmail1.trim() != "," && sEmail1.trim() != "" && !isEmail(sEmail1.trim())) {
                        sMsg = sMsg + sEmail1 + ", ";
                    }

                    for (var i = 0; i < ArrEmail.length; i++) {
                        sEmail = ArrEmail[i];
                        if (j != i && ArrEmail[j].trim() !="" && sEmail.trim()!="" && ArrEmail[j].trim() == sEmail.trim()) {
                            sMsg1 = sMsg1 + sEmail + ", ";
                        }
                    } 
                }
            }

            if (email.length > 200) {
                document.getElementById(_clientcstValidateEmail).errormessage = document.getElementById("<%=this.hidValEmailAddForFeedbackLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcstValidateEmail).errormessage = document.getElementById("<%=this.hidValFeedBackEmailAdd.ClientID %>").value + "(" + sMsg.trim() + ")" + document.getElementById("<%=this.hidValEmailAddValid.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sMsg1 != "") {
                sMsg1 = sMsg1.substring(0, sMsg1.length - 2);
                var names = sMsg1.split(',');
                var sMsg2 = ArrNoDupe(names);
                if (sMsg2.length > 0) {
                    sMsg1 = sMsg2.toString();
                    document.getElementById(_clientcstValidateEmail).errormessage = document.getElementById("<%=this.hidValFeedBackEmailAdd.ClientID %>").value + "(" + sMsg1.trim() + ") " + document.getElementById("<%=this.hidAreDuplicated.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }            
            else {
                txtEmail = txtEmail.substring(0, txtEmail.length - 1);
                document.getElementById(_clienttxtEmails).value = txtEmail;
            }
            
            args.IsValid = true;
            return false;            
        }

        function ValidateCareerEmailAddress(oSrc, args) {
            var email = (document.getElementById(_clienttxtCareerEmails).value).trim();
            var ArrEmail, txtEmail = '';
            var sEmail;
            var sEmail1;
            var sMsg = "";
            var sMsg1 = "";
            if (email.trim() != "") {
                ArrEmail = email.split(',');

                for (var j = 0; j < ArrEmail.length; j++) {

                    sEmail1 = ArrEmail[j];
                    if (sEmail1 != "")
                        txtEmail = txtEmail + sEmail1 + ",";

                    if (sEmail1.trim() != "," && sEmail1.trim() != "" && !isEmail(sEmail1.trim())) {
                        sMsg = sMsg + sEmail1 + ", ";
                    }

                    for (var i = 0; i < ArrEmail.length; i++) {
                        sEmail = ArrEmail[i];
                        if (j != i && ArrEmail[j].trim() != "" && sEmail.trim() != "" && ArrEmail[j].trim() == sEmail.trim()) {
                            sMsg1 = sMsg1 + sEmail + ", ";
                        }
                    }
                }
            }

            if (email.length > 200) {
                document.getElementById(_clientcstValidateCareerEmail).errormessage = document.getElementById("<%=this.hidCareerEmailAddLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                document.getElementById(_clientcstValidateCareerEmail).errormessage = document.getElementById("<%=this.hidvalCareerEmailAddress.ClientID %>").value + "(" + sMsg.trim() + ") " + document.getElementById("<%=this.hidValEmailAddValid.ClientID %>").value; ;
                args.IsValid = false;
                return true;
            }
            else if (sMsg1 != "") {
                sMsg1 = sMsg1.substring(0, sMsg1.length - 2);
                var names = sMsg1.split(',');
                var sMsg2 = ArrNoDupe(names);
                if (sMsg2.length > 0) {
                    sMsg1 = sMsg2.toString();
                    document.getElementById(_clientcstValidateCareerEmail).errormessage = document.getElementById("<%=this.hidvalCareerEmailAddress.ClientID %>").value + "(" + sMsg1.trim() + ")" + document.getElementById("<%=this.hidAreDuplicated.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            else {
                txtEmail = txtEmail.substring(0, txtEmail.length - 1);
                document.getElementById(_clienttxtCareerEmails).value = txtEmail;
            }

            args.IsValid = true;
            return false;
        }

        function ArrNoDupe(origArr) {
            var newArr = [],
                origLen = origArr.length,
                 found,
                    x, y;
            for (x = 0; x < origLen; x++) {
                found = undefined;
                for (y = 0; y < newArr.length; y++) {
                    if (origArr[x].trim() === newArr[y].trim()) {
                        found = true;
                        break;
                    }
                }
                if (!found) newArr.push(origArr[x]);
            }
            return newArr;  
        }

        function clearError() {
            document.getElementById(_clientlblErrorMsg).innerText = ""
        }

        function clickButton(e) {
            var evt = e ? e : window.event
            if (evt.keyCode == 13) {
                return false
            }
        }

        function trimAll(sString) {
            while (sString.substring(0, 1) == ' ') {
                sString = sString.substring(1, sString.length)
            }
            while (sString.substring(sString.length - 1, sString.length) == ' ') {
                sString = sString.substring(0, sString.length - 1)
            }
            while (sString.charCodeAt(sString.length - 1) == 10 || sString.charCodeAt(sString.length - 1) == 13) {
                sString = sString.substring(0, sString.length - 1)
            }
            return sString
        }

        function ValidateLogo(aSrc, args) {
            var myImage = new Image()
            myImage.src = document.getElementById(_clientFileUploadLogo).value
            var iWidth = myImage.width
            var iHeight = myImage.height
            
            if (document.getElementById(_clienthidFilePath).value == "" && document.getElementById(_clientFileUploadLogo).value == '') {
                document.getElementById(_clientCstValidateLogo).errormessage = document.getElementById("<%=this.hidSchoolLogoBlank.ClientID %>").value;
                args.IsValid = false
                return true
            }
            
            if (document.getElementById(_clientFileUploadLogo).value == '') {
                args.IsValid = true
                return false
            }
            else if (!CheckFileType(myImage.src)) {
            document.getElementById(_clientCstValidateLogo).errormessage = document.getElementById("<%=this.hidValBITMapFileFormat.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckFileType(sFileName) {
            var bIsValid = true
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {
                    bIsValid = true
                }
                else {
                    bIsValid = false
                }
            }
            return bIsValid
        }

        function ValidateSignLogo(aSrc, args) {
            var myImage = new Image()
            myImage.src = document.getElementById(_clientUploadSign).value
            var iWidth = myImage.width
            var iHeight = myImage.height
            if (CheckFileTypeSign(myImage.src)) { }
            else {
                document.getElementById(_clientCustValidateSign).errormessage = document.getElementById("<%=this.hidInvalidFileFormat.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function ValidateICardLogo(aSrc, args) {
            var myImage = new Image()
            myImage.src = document.getElementById(_clientUploadICard).value
            var iWidth = myImage.width
            var iHeight = myImage.height
            if (CheckFileTypeSign(myImage.src)) { }
            else {
                document.getElementById(_clientCustICardLogo).errormessage = document.getElementById("<%=this.hidInvalidFileFormat.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

        function CheckFileTypeSign(sFileName) {
            var bIsValid
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG"
|| sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {
                    
                    
                    bIsValid = true
                }
                else {
                    bIsValid = false
                }
            }
            else {
                bIsValid = false
            }
            return bIsValid
        }

        function PreventSpecialChars(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || (k >= 48 && k <= 57) || k == 0 || k == 9);
        }

        function ValidateForgotPasswordEmailAddress(oSrc, args) {
            var email = ($get(_clienttxtForgotPassword).value).trim();
            var ArrEmail, txtEmail = '';
            var sEmail;
            var sEmail1;
            var sMsg = "";
            var sMsg1 = "";
            if (email.trim() != "") {
                ArrEmail = email.split(',');

                for (var j = 0; j < ArrEmail.length; j++) {

                    sEmail1 = ArrEmail[j];
                    if (sEmail1 != "")
                        txtEmail = txtEmail + sEmail1 + ",";

                    if (sEmail1.trim() != "," && sEmail1.trim() != "" && !isEmail(sEmail1.trim())) {
                        sMsg = sMsg + sEmail1 + ", ";
                    }

                    for (var i = 0; i < ArrEmail.length; i++) {
                        sEmail = ArrEmail[i];
                        if (j != i && ArrEmail[j].trim() != "" && sEmail.trim() != "" && ArrEmail[j].trim() == sEmail.trim()) {
                            sMsg1 = sMsg1 + sEmail + ", ";
                        }
                    }
                }
            }

            if (email.length > 200) {
                $get(_clientcstForgotPassword).errormessage = $get("<%=this.hidForgotPasswordEmailLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sMsg != "") {
                sMsg = sMsg.substring(0, sMsg.length - 2);
                $get(_clientcstForgotPassword).errormessage = $get("<%=this.hidvalForgotPassEmails.ClientID %>").value + "(" + sMsg.trim() + ")" + $get("<%=this.hidValEmailAddValid.ClientID %>").value; 
                args.IsValid = false;
                return true;
            }
            else if (sMsg1 != "") {
                sMsg1 = sMsg1.substring(0, sMsg1.length - 2);
                var names = sMsg1.split(',');
                var sMsg2 = ArrNoDupe(names);
                if (sMsg2.length > 0) {
                    sMsg1 = sMsg2.toString();
                    $get(_clientcstForgotPassword).errormessage = $get("<%=this.hidvalForgotPassEmails.ClientID %>").value + "(" + sMsg1.trim() + ")" + $get("<%=this.hidAreDuplicated.ClientID %>").value; 
                    args.IsValid = false;
                    return true;
                }
            }
            else {
                txtEmail = txtEmail.substring(0, txtEmail.length - 1);
                $get(_clienttxtForgotPassword).value = txtEmail;
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
