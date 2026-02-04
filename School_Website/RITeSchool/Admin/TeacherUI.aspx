<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeacherUI.aspx.cs" Inherits="TeacherUI"
    EnableEventValidation="False" MasterPageFile="../MasterPages/MasterPage.master" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Src="~/UserControls/UserBasicDetails.ascx" TagName="UserBasicDetails"
    TagPrefix="UserBasicDetailsUC" %>
    <%@ Register Src="~/UserControls/EmployeeBasicDetails.ascx" TagName="EmployeeBasicDetails"
    TagPrefix="EmployeeBasicDetailsUC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">

    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <!-- Data Insert Here -->
                <table border="0" cellpadding="0" cellspacing="2" style="width: 97%;">
                    <tr>
                        <td align="left" colspan="4" valign="top">
                            <div runat="server">
                                <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                        Visible="False"></asp:Label>
                                    <div id="divRIMsg" runat="server" visible="false" enableviewstate="false">
                                        <asp:Label ID="lblErrorMsgForStd" Style="text-align: left" runat="server" Font-Bold="False"
                                            Text="Label" ForeColor="Red" Height="20px" Width="100%" EnableViewState="False"
                                            CssClass="LblErrorMsg" Visible="False"> </asp:Label></div>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" style="">
                            <asp:Panel ID="Panel1" runat="server" Width="96%">
                                <asp:Label ID="lblChkDuplicate" Style="text-align: left" runat="server" ForeColor="Red"
                                    EnableViewState="false" Height="20px" Width="100%" CssClass="ClsMdtStar" Visible="false"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="3" valign="top" style="height: 49px">
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel"/>
                            <asp:ValidationSummary ID="valAddEduDetails" runat="server" ValidationGroup="valGrpAddEduDetails"
                                CssClass="ClsLabel" />
                            <asp:ValidationSummary ID="valsumExpDetails" runat="server" CssClass="ClsLabel" ValidationGroup="valGrpExpDetails" />
                            <asp:ValidationSummary ID="valsumUserDetails" runat="server" CssClass="ClsLabel" ValidationGroup="Save" />
                        </td>
                        <td align="right" valign="top">
                            <span style="color: red; font-family: Arial" class="ClsMdtStar"><span>* </span>
                                        <asp:Label ID="lblMandatoryFields" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>" ForeColor="Red"></asp:Label>
                                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="4" valign="top">
                            <asp:Wizard ID="wizard_TeacherInfo" Width="100%" runat="server" DisplaySideBar="False"
                                ActiveStepIndex="0" OnActiveStepChanged="wizard_TeacherInfo_ActiveStepChanged"
                                OnFinishButtonClick="wizard_TeacherInfo_FinishButtonClick" DisplayCancelButton="True"
                                OnCancelButtonClick="wizard_TeacherInfo_CancelButtonClick" Height="376px">
                                <WizardSteps>
                                    <asp:WizardStep ID="WizardStep1" runat="server" Title="Step 1" StepType="Start">
                                        <table border="0" width="100%">
                                            <tr>
                                                <td align="left" colspan="4" style="height: 19px" class="ClsBtmBorderGray">                                                    
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold" >
                                                        <asp:Label ID="lblTeacherDetailText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, PersonalInformation %>"></asp:Label><span class="colonPadding"> :</span>
                                            </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height: 5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%">
                                                </td>
                                                <td align="left" style="width: 32%">
                                                    <span class="LblSmlGray" id="lblSalutation"><asp:Label ID="Label48" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, Salutation %>"></asp:Label></span>
                                                    <span class="LblSmlGray" id="lblFirstName">( <asp:Label ID="Label1" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, FirstName %>"></asp:Label> )</span>                                                  
                                                </td>
                                                <td align="left" style="width: 19%; padding-left: 7px;">
                                                          <span class="LblSmlGray" id="lblMiddleInitial">( <asp:Label ID="Label2" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MiddleName %>"></asp:Label> )</span>
                                                </td>
                                                <td align="left" style="width: 23%">
                                                <span class="LblSmlGray" id="lblLastName">( <asp:Label ID="Label3" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, LastName %>"></asp:Label> )</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%">
                                                 <span class="ClsLabel" id="lblTeacherName1">
                                                 <asp:Label ID="lblTeacherNameText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                        </asp:Label><span class="colonPadding"> :</span></span>                                                  
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 32%">
                                                    <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="MidTxtBox" onblur="formatName(this)"></asp:TextBox>
                                                    &nbsp;*
                                                </td>
                                                <td align="left" style="width: 23%; padding-left: 7px;">
                                                    <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="50" onblur="formatName(this)"></asp:TextBox>
                                                </td>
                                                <td align="left" style="width: 23%">
                                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" onblur="formatName(this)" CssClass="MidTxtBox"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                  <span class="ClsLabel" id="lblDesignation"><asp:Label ID="lblDesignationText" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, Designation %>">
                                        </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 32%;" class="ClsMdtStar">
                                                    <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="MidCombo">
                                                    </asp:DropDownList> &nbsp;*
                                                </td>
                                                <td align="left" style="width: 6%;">
                                                 <span class="ClsLabel" id="lblDateofBirth"><asp:Label ID="Label4" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, DateOfBirth %>">
                                        </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 32%;">
                                                    <asp:TextBox ID="cal_DOB" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calendar_DOB" runat="server" Culture="en" Control="cal_DOB" Format="dd MMM yyyy"
                                                        To-Message="<%$ Resources:LocalizedResources, ValidBirthDateErrorMsg %>" From-Message="<%$ Resources:LocalizedResources, ValidBirthDateErrorMsg %>"
                                                        To-Today="true" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, ValidBirthDateErrorMsg %>" />                                                 
                                                    
                                                   <span class="ClsMdtStar">&nbsp;*</span></td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                   <span class="ClsLabel" id="lblPhoneNumber"> <asp:Label ID="Label15" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PhoneNumber %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                                </td>
                                                <td align="left" style="width: 32%;">
                                                    <asp:TextBox ID="txtPhoneNumber" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar"></span>
                                                </td>
                                                <td align="left" style="width: 19%;">
                                                <span class="ClsLabel" id="lblMobileNumber">
                                                <asp:Label ID="Label17" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MobileNumber %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:TextBox ID="txtreadonly" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        ReadOnly="True" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        Width="40px" Text="91" onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar">
                                                        <asp:TextBox ID="txtMobileNumber" CssClass="SmlTxtBox" runat="server" MaxLength="10"
                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" />
                                                        <span class="ClsMdtStar">&nbsp;*</span> </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                 <span class="ClsLabel" id="lblNationality"><asp:Label ID="Label5" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Nationality %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                                </td>
                                                <td align="left" style="width: 32%;">
                                                    <asp:TextBox ID="txtNationality" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                    <span class="ClsMdtStar">*  </span>
                                                </td>
                                                <td align="left" style="width: 19%;">
                                                 <span class="ClsLabel" id="lblReligion"><asp:Label ID="Label6" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Religion %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                                </td>
                                                <td align="left" style="width: 27%;">
                                                    <span class="ClsMdtStar">
                                                        <asp:DropDownList ID="cmbReligion" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                        &nbsp;*</span></td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 19%;">
                                                <span class="ClsLabel" id="lblCasteSubCaste"><asp:Label ID="Label9" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, CasteAndSubCaste %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:TextBox ID="txtCasteSubCaste" runat="server" CssClass="MidTxtBox" MaxLength="100"></asp:TextBox>
                                                </td>
                                                <td align="left" style="width: 6%;">
                                                <span class="ClsLabel" id="lblCategory"><asp:Label ID="Label8" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Category %>">
                                                </asp:Label><span class="colonPadding"> :</span>
                                                </span>
                                                </td>
                                                <td align="left" style="width: 32%;">                                                   
                                                        <asp:DropDownList ID="cmbCategory" runat="server" CssClass="MidCombo">
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">* </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 19%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label7" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, EmergencyContact %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:TextBox ID="txtEmergencyNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar">*</span>
                                                </td>

                                              
                                                <td id="tdTeacherForClass" runat="server" align="left" visible = "false" style="width: 19%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label49" runat="server" EnableViewState="False" Text="Associated Standard Category">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                               <td id="tdTeacherComboForClass" runat="server" align="left" visible = "false" style="width: 32%;">                                                   
                                                        <asp:DropDownList ID="cmbTeachingForClass" runat="server" CssClass="MidCombo">
                                                        <asp:ListItem Value = "0" Text="-- Select --"></asp:ListItem>
                                                        <asp:ListItem Value = "1" Text="Pre-Primary"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Primary"></asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="ClsMdtStar">* </span>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:RequiredFieldValidator ID="req_FirstName" runat="server" ControlToValidate="txtFirstName"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, FirstNameShouldNotBeBlank %>" CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cmp_valDesignation" runat="server" ControlToValidate="cmbDesignation"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DesignationShouldBeSelected %>" Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:CustomValidator ID="cst_Designation" runat="server" ControlToValidate="cmbDesignation"
                                            ClientValidationFunction="CheckIfPrincipalAlreadyExists" ErrorMessage="<%$ Resources:LocalizedResources, PrincipalCondition %>"
                                            Display="None" CssClass="ClsLabel"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="req_PhoneNumber" runat="server" ControlToValidate="txtPhoneNumber"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, PhoneNoShouldNotBlank %>" Visible="False"
                                            CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="req_MobileNumber" runat="server" ControlToValidate="txtMobileNumber"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, MobileNumberBlank %>" CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:CustomValidator ID="cst_MobileNumber" runat="server" ControlToValidate="txtMobileNumber"
                                            Display="None" ClientValidationFunction="MobileNumberValidation" CssClass="ClsLabel"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cst_DOB" runat="server" ControlToValidate="cal_DOB" ClientValidationFunction="DOBValidation"
                                            Display="None" CssClass="ClsLabel"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="req_DateOfBirth" runat="server" ControlToValidate="cal_DOB"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthValidation %>" CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="reqEmergencyNo" runat="server" CssClass="ClsLabel"
                                            ControlToValidate="txtEmergencyNo" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, EmergencyContactShouldNotBlank %>"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cmpValDOB" runat="server" ControlToValidate="cal_DOB" Display="None"
                                            ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthValidation1 %>" Type="Date" Operator="LessThan"
                                            Visible="False" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="req_Nationality" runat="server" ControlToValidate="txtNationality"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, NationalityShouldNotBlank %>" CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cmp_ValReligion" runat="server" ControlToValidate="cmbReligion"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ReligionShouldBeSelected %>" Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:RequiredFieldValidator ID="req_CasteSubCaste" runat="server" ControlToValidate="txtCasteSubCaste"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, CasteAndSubCasteShouldNotBlank %>" Visible="False"
                                            CssClass="ClsLabel"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator ID="cmp_ValCategory" runat="server" ControlToValidate="cmbCategory"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, CategoryShouldBeSelected %>" Operator="NotEqual"
                                            ValueToCompare="0" CssClass="ClsLabel"></asp:CompareValidator>
                                        <asp:CustomValidator ID="cst_StandardCategory" runat="server" ControlToValidate="cmbTeachingForClass"
                                            ClientValidationFunction="CheckStandardCategory" ErrorMessage="Associated standard category should be selected."
                                            Display="None" CssClass="ClsLabel"></asp:CustomValidator>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep2" runat="server" Title="Step 2">
                                        <table>
                                            <tr>
                                                <td width="150px">
                                                    <span class="ClsLblLgnd"><asp:Label ID="Label10" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td width="6px" align="right">
                                                </td>
                                                <td class="ClsHilightBGB" width="234px">
                                                    <asp:Label ID="lblTeacherNameStep2" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td colspan="4" class="ClsBtmBorderGray">                                                    
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold" ><asp:Label ID="Label11" runat="server" CssClass="ClsLblLgnd" Font-Bold="True" Text="<%$ Resources:LocalizedResources, LocalAddress %>"
                                                            Width="164px" EnableViewState="false"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;" valign="top">
                                                 <span class="ClsLabel"><asp:Label ID="Label18" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Address %>">
                                                  </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" rowspan="4" style="width: 35%" class="ClsMdtStar" valign="top">
                                                    <asp:TextBox ID="txtLocalAddress" runat="server" MaxLength="150" CssClass="ExLrgTxtBox"
                                                        Height="75px" TextMode="MultiLine"></asp:TextBox>
                                                    *&nbsp;
                                                    <asp:RegularExpressionValidator ID="Reg_Vali_Loc_Add" runat="server" Display="None"
                                                        ControlToValidate="txtLocalAddress" ErrorMessage="<%$ Resources:LocalizedResources, LocalAddressLengthValidation %>"
                                                        ValidationExpression="^[\s\S]{0,200}$" ValidationGroup="Save"> </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="req_LocalAddr" runat="server" ControlToValidate="txtLocalAddress"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, LocalAddressShouldNotBlank %>" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" style="width: 18%;">                                                        
                                                 <span class="ClsLabel"><asp:Label ID="Label20" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, City %>">
                                                  </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 24%;">
                                                    <asp:TextBox ID="txtLocalCity" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*<asp:RequiredFieldValidator ID="req_LocalCity" runat="server" ValidationGroup="Save"
                                                        ControlToValidate="txtLocalCity" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, LocalCityShouldNotBlank %>"></asp:RequiredFieldValidator>
                                                        &nbsp; </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                </td>
                                                <td align="left" style="width: 18%;">                                                
                                                 <span class="ClsLabel"><asp:Label ID="Label21" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, State %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 24%;">
                                                    <asp:TextBox ID="txtState" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*<asp:RequiredFieldValidator ID="reqState" runat="server" ValidationGroup="Save"
                                                        ControlToValidate="txtState" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, StateShouldNotBlank %>"></asp:RequiredFieldValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                </td>
                                                <td align="left" style="width: 18%;">                                                   
                                                 <span class="ClsLabel"><asp:Label ID="Label22" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Pincode %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:TextBox ID="txtLocalPincode" CssClass="MidTxtBox" runat="server" MaxLength="6"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    <span class="ClsMdtStar">*
                                                        <asp:CustomValidator ID="cst_LocalPin" runat="server" ClientValidationFunction="PinCodeValidation"
                                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                                    </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                </td>
                                                <td align="left" style="width: 18%;">                                                    
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" style="height: 19px" class="ClsBtmBorderGray">                                                  
                                                        <span class="ClsLblLgnd" style="width:178px;font:Bold" ><asp:Label ID="Label12" runat="server" Font-Bold="True" CssClass="ClsLblLgnd" Text="<%$ Resources:LocalizedResources, PermanentAddress %>"
                                                            EnableViewState="false"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%">
                                                </td>
                                                <td align="left" colspan="3">
                                                    <asp:CheckBox ID="chkAddress" runat="server" Width="338px" Text="<%$ Resources:LocalizedResources, IsPermanentAddrSmeLocalAddr %>"
                                                        CssClass="LabelLft" />                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%">                                                
                                                 <span class="ClsLabel"><asp:Label ID="Label13" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Address %>">
                                                  </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" rowspan="3" style="width: 32%" valign="top">
                                                    <asp:TextBox ID="txtPerAddress" runat="server" CssClass="ExLrgTxtBox" Height="75px"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="Regu_Vali_PerAdd" runat="server" Display="None"
                                                        ControlToValidate="txtPerAddress" ErrorMessage="<%$ Resources:LocalizedResources, PermanentAddressLengthCondition %>"
                                                        ValidationExpression="^[\s\S]{0,200}$"> </asp:RegularExpressionValidator>
                                                </td>
                                                <td style="width: 6%;" align="left">
                                                 <span class="ClsLabel"><asp:Label ID="Label14" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, City %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 32%">
                                                    <asp:TextBox ID="txtPerCity" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>                                                   
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 6%;" align="left">
                                                </td>
                                                <td align="left" style="width: 6%;">
                                                 <span class="ClsLabel"><asp:Label ID="Label16" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, State %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 32%">
                                                    <asp:TextBox ID="txtPerState" runat="server" CssClass="MidCombo"> </asp:TextBox>
                                                    <span style="color: red"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 6%;">
                                                </td>
                                                <td style="height: 16px; width: 18%;" align="left">
                                                 <span class="ClsLabel"><asp:Label ID="Label19" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Pincode %>">
                                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td style="height: 16px; width: 23%;" align="left">
                                                    <asp:TextBox ID="txtPerPinCode" CssClass="MidTxtBox" runat="server" MaxLength="6"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 13%;">
                                                </td>
                                                <td align="left" colspan="2" style="width: 76px;">
                                                </td>
                                                <td align="left" colspan="1" style="width: 119px;">
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep3" runat="server" Title="Step 3">
                                        <table border="0" style="width: 100%; height: 100%;">
                                            <tr>
                                                <td align="left">
                                                    <span class="ClsLblLgnd"><asp:Label ID="Label23" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td class="ClsHilightBGB" width="200px">
                                                    <asp:Label ID="lblTeacherNameStep3" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 15%;">                                                   
                                                        <span class="ClsLblLgnd" style="width:141px;" ><asp:Label ID="Label34" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, DateOfRetirement %>">
                                            </asp:Label><span class="colonPadding"> :</span>
                                            </span>
                                                </td>
                                                <td align="left" class="ClsHilightBGB" style="width: 23%;">
                                                    <asp:Label ID="lblDateofRetirement" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" class="ClsBtmBorderGray" style="height: 19px" colspan="4">                                                    
                                                        <span class="ClsLblLgnd" style="width:15%;font:Bold">
                                                        <asp:Label ID="lblEmployeeDetails" runat="server" Text="<%$ Resources:LocalizedResources, EmployeeDetails %>" Width="164px" EnableViewState="false"></asp:Label>
                                                        </span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px">
                                                </td>
                                            </tr>
                                              <tr>
                                                <td colspan="2">
                                                    <UserBasicDetailsUC:UserBasicDetails ID="ucUserBasicDetails" runat="server"/>
                                                     <asp:HiddenField ID="hidUserIdUC" runat="server" Value="" />
                                                </td>
                                            </tr>
                                            <tr id="trSPSTeacherType" runat="server" visible="false">
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td align="left" style="width: 320px; height: 41px; white-space:nowrap;" valign="top" class="ClsBorderlight">
                                                                 <span class="ClsLabel"><asp:Label ID="Label50" runat="server" EnableViewState="False"
                                                                Text="Type">
                                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                             </td>
                                                             <td>
                                                                <asp:DropDownList ID="cmbType" runat="server" AutoPostBack="true" ViewStateMode="Enabled" CssClass="SmlTxtBox">
                                                                    <asp:ListItem Value = "0" Text="-- All --"></asp:ListItem>
                                                                    <asp:ListItem Value = "1" Text="Boarding"></asp:ListItem>
                                                                    <asp:ListItem Value = "2" Text="DayBoarding"></asp:ListItem>
                                                                 </asp:DropDownList>
                                                             </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" style="height: 19px" class="ClsBtmBorderGray">                                                   
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold" >
                                                        <asp:Label ID="Label24" runat="server" Text="<%$ Resources:LocalizedResources, ExperienceDetails %>" Width="164px" EnableViewState="false"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 5px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:CustomValidator ID="cstJoinResignDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        Visible="true" ErrorMessage="" EnableClientScript="true"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%; height: 41px;" valign="top">
                                                 <span class="ClsLabel"><asp:Label ID="Label25" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Achievements %>">
                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" colspan="3" style="height: 41px">
                                                    <asp:TextBox ID="txtAchivements" runat="server" TextMode="MultiLine" Width="578px"></asp:TextBox>
                                                    <asp:RegularExpressionValidator ID="Regu_Vali_Achiv" runat="server" Display="None"
                                                        ControlToValidate="txtAchivements" ErrorMessage="<%$ Resources:LocalizedResources, AchievementLengthCondition %>"
                                                        ValidationExpression="^[\s\S]{0,300}$"> </asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%">
                                                </td>
                                                <td align="left" style="width: 25%">                                                    
                                                 <span class="LblSmlGray">(<asp:Label ID="Label26" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Year %>">
                                            </asp:Label>)</span>
                                                 <span class="LblSmlGray">(<asp:Label ID="Label27" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, Months %>">
                                            </asp:Label>)</span>
                                                </td>
                                                <td align="left" style="width: 15%">                                                    
                                                </td>
                                                <td align="left" style="width: 23%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label28" runat="server" EnableViewState="False"
                                                Text="<%$ Resources:LocalizedResources, PastExperience %>">
                                            </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:TextBox ID="txtExpYears" MaxLength="2" CssClass="ExSmlTxtBox" runat="server"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" Text="00" />
                                                    <asp:TextBox ID="txtExpMonths" CssClass="ExSmlTxtBox" runat="server" MaxLength="2"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" Text="00" />
                                                </td>
                                                <td align="left" style="width: 15%;">
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label29" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolName %>">
                                                    </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 23%;" colspan="3">
                                                    <asp:TextBox ID="txtSchoolname" runat="server" MaxLength="100" CssClass="MidTxtBox"
                                                        Width="578px" CausesValidation="true"></asp:TextBox>
                                                    <span class="ClsMdtStar">*</span>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSchoolname"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, SchoolNameShouldNotBlank %>" ValidationGroup="valGrpExpDetails"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label30" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoinedDate %>">
                                                    </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:TextBox ID="txtjoinedDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calender_JoinDate" runat="server" Culture="en-US" Control="txtjoinedDate" To-Today="true"
                                                        Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
                                                        To-Message="<%$ Resources:LocalizedResources, JoinedDateValidation %>" From-Message="<%$ Resources:LocalizedResources, JoinedDateValidation %>"
                                                        ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, JoinedDateValidation %>" />
                                                    <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ControlToValidate="txtjoinedDate"
                                                        ID="ReqJoinDate" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, JoinedDateShouldNotBlank %>"
                                                        ValidationGroup="valGrpExpDetails" Display="None"></asp:RequiredFieldValidator>
                                                </td>
                                                <td align="left" style="width: 15%;">                                                    
                                                    <span class="ClsLabel"><asp:Label ID="Label31" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeftDate %>">
                                                    <span class="colonPadding"> :</span>
                                                    </asp:Label></span>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:TextBox ID="txtLeftDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
                                                    <rjs:PopCalendar ID="calender_LeftDate" runat="server" Control="txtLeftDate" To-Today="true"
                                                        ValidationGroup="valGrpExpDetails" Culture="en-US"  ShowErrorMessage="false" Format="dd MMM yyyy"
                                                        To-Message="<%$ Resources:LocalizedResources, LeftDateValidation %>" From-Message="<%$ Resources:LocalizedResources, LeftDateValidation %>"
                                                        ShowWeekend="True" Enabled="true" InvalidDateMessage="<%$ Resources:LocalizedResources, LeftDateValidation %>" />
                                                    <span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ControlToValidate="txtLeftDate"
                                                        ID="RequiredFieldValidator3" runat="server" ValidationGroup="valGrpExpDetails"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, LeftDateShouldNotBlank %>" Display="None"></asp:RequiredFieldValidator>
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







                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                </td>
                                                <td align="left" style="" colspan="2">
                                                    <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, AddDetails %>" CssClass="ClsBtnMid" BorderStyle="Solid"
                                                        BorderWidth="1px" OnClick="btnAdd_Click" ValidationGroup="valGrpExpDetails" CausesValidation="true" />
                                                    <asp:Button ID="btnCancelDetails" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                        CausesValidation="False" CssClass="ClsBtnSml" Text="<%$ Resources:LocalizedResources, Cancel %>" OnClick="btnCancelDetails_Click" />
                                                </td>
                                            </tr>
                                            <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                                Visible="true" ErrorMessage="<%$ Resources:LocalizedResources, LeftDateValidation1 %>" ClientValidationFunction="ValidateControls"
                                                ValidationGroup="valGrpExpDetails"></asp:CustomValidator>

                                              <%--   <asp:ListView ID="lstvwExpDetails" runat="server" DataKeyNames="SchoolName,JoiningDate,leftDate"
                                                        OnItemDataBound="lstvwExpDetails_ItemDataBound" OnItemCommand="lstvwExpDetails_ItemCommand">--%>
                                               
                                            <tr>
                                                <td align="center" colspan="4">
                                                     <asp:ListView ID="lstvwExpDetails" runat="server" DataKeyNames="SchoolName,JoiningDate,leftDate,PreviousDesignation,Last_Salary,Job_Description,Reason_for_Leaving,DurationDays"
                                                        OnItemDataBound="lstvwExpDetails_ItemDataBound" OnItemCommand="lstvwExpDetails_ItemCommand">
                                                        <LayoutTemplate>
                                                            <table align="center" width="75%" runat="server" id="tblExperienceInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" width="40%">
                                                                       <asp:Label ID="Label25" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolName %>">
                                                    </asp:Label>
                                                                    </th>
                                                                    <th align="center">
                                                                        <asp:Label ID="Label26" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, JoinedDate %>">
                                                    </asp:Label>
                                                                    </th>
                                                                    <th align="center">
                                                                        <asp:Label ID="LabelLeftDateText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeftDate %>">
                                                    </asp:Label>
                                                                    </th>
                                                                    <th align="center">
                                                                        <asp:Label ID="LabelEditText" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Edit %>">
                                                    </asp:Label>
                                                                    </th>
                                                                    <th align="center">
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
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Modify"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
                                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="Modify"
                                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:ImageButton ID="imgBtnDelete" CommandName="REMOVE" CausesValidation="false"
                                                                        runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                    </asp:ListView>
                                                </td>
                                                 <asp:HiddenField ID="hidPreviousDesignation" runat="server" />
                                                      <asp:HiddenField ID="hidLast_Salary" runat="server" />
                                                           <asp:HiddenField ID="hidJob_Description" runat="server" />
                                                                <asp:HiddenField ID="hidReason_for_Leaving" runat="server" />
                                                                     <asp:HiddenField ID="hidDurationDays" runat="server" />
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:CompareValidator ID="cmp_ExpYears" runat="server" ControlToValidate="txtExpYears"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, PastExperienceValidation %>"
                                                        Operator="LessThanEqual" Type="Integer" ValueToCompare="60"></asp:CompareValidator>
                                                    <asp:CompareValidator ID="cmp_ExpInMonths" runat="server" ControlToValidate="txtExpMonths"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, PastExperienceValidation1 %>"
                                                        Operator="LessThan" Type="Integer" ValueToCompare="12"></asp:CompareValidator>
                                                </td>
                                                <td align="left" style="width: 15%;">
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="4" class="ClsBtmBorderGray">                                                   
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold" >
                                                        <asp:Label ID="Label32" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, EducationalInformation %>">
                                                    </asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="height: 5px;" colspan="4">
                                                </td>
                                            </tr>

                                            <tr>
                                            <td style="height: 5px;" colspan="4">
                                            <table width ="100%">
                                                <tr>
                                                    <td align="left">                                                
                                                        <span class="ClsLabel"><asp:Label ID="Label33" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Qualification %>"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                </td>
                                                       <td align="left" style="padding-left : 259px;">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel2">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbQualification" runat="server" CssClass="MidTxtBox">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*<asp:CompareValidator ID="cmpValQualification" runat="server"
                                                                ControlToValidate="cmbQualification" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, QualificationShouldBeSelected %>"
                                                                Operator="NotEqual" ValueToCompare="0" ValidationGroup="valGrpAddEduDetails"></asp:CompareValidator>
                                                            </span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                    <td style="width: 132px">
                                                    <span class="ClsLabel"><asp:Label ID="Label333" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Specialization %>"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                    </td>
                                                    
                                                       <td align="left" style="width: 13%;">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel22">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtSpecialization" CssClass="SmlTxtBox" runat="server" MaxLength="100"
                                                                />
                                                            
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                   
                                                </td>
                                                    
                                                      <td align="left" style="width: 15%;">
                                                        <span class="ClsLabel"><asp:Label ID="Label35" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, YearOfPassing %>"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                </td>
                                                   <td align="left" style="width: 23%;">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel1">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtYearOfPassing" CssClass="SmlTxtBox" runat="server" MaxLength="4"
                                                                onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                ondrop="event.returnValue=false" />
                                                            <span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                    <asp:CustomValidator ID="cst_YearOfPassing" runat="server" ValidationGroup="valGrpAddEduDetails"
                                                        ClientValidationFunction="YearValidation" ControlToValidate="txtYearOfPassing"
                                                        Display="None"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtYearOfPassing"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, YearOfPassingShouldNotBlank %>" ValidationGroup="valGrpAddEduDetails"></asp:RequiredFieldValidator>
                                                </td>
                                                </tr>
                                            </table>
                                            </td>
                                            </tr>

                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                    <span class="ClsLabel"><asp:Label ID="Label36" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ClassGrade %>"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 25%;">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel6">
                                                        <ContentTemplate>
                                                            <asp:DropDownList ID="cmbPassingClass" runat="server" CssClass="LrgTxtBox">
                                                            </asp:DropDownList>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" style="width: 15%;">
                                                         <span class="ClsLabel"><asp:Label ID="Label37" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, University %>"></asp:Label>
                                                         <span class="colonPadding"> :</span></span>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel3">
                                                        <ContentTemplate>
                                                            <asp:TextBox ID="txtPassingUniversity" runat="server" MaxLength="100" CssClass="MidTxtBox"></asp:TextBox>
                                                            <span class="ClsMdtStar"><span style="color: #ff0000">*&nbsp;</span> </span>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                    <asp:CustomValidator ID="cstValGrid" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, EducationalInformationValidation %>"
                                                        ClientValidationFunction="validateGridData" ValidationGroup="Save"></asp:CustomValidator>
                                                </td>
                                                <td align="left" style="" colspan="2">
                                                    <asp:UpdatePanel ID="upnlButtons" runat="server">
                                                        <ContentTemplate>
                                                            <asp:Button ID="btnAddDetails" runat="server" Text="<%$ Resources:LocalizedResources, AddDetails %>" OnClick="btnAddDetails_Click"
                                                                CssClass="ClsBtnMid" BorderStyle="Solid" ValidationGroup="valGrpAddEduDetails"
                                                                BorderWidth="1px" />
                                                            <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
                                                                CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="grdvwEducationDetails" EventName="RowCommand" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                    <asp:RequiredFieldValidator ID="req_PassingUniversity" runat="server" ControlToValidate="txtPassingUniversity"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, UniversityShouldNotBlank %>" ValidationGroup="valGrpAddEduDetails"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                </td>
                                                <td align="left" colspan="2" style="">
                                                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
                                                        ID="UpdatePanel5">
                                                        <ContentTemplate>
                                                            <asp:Label ID="lblDuplicateDetails" runat="server" Visible="False" ForeColor="Red"
                                                                CssClass="ClsLabel" EnableViewState="False" Text="<%$ Resources:LocalizedResources, DuplicateEducationalInformation %>"></asp:Label>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 20%;">
                                                </td>
                                                <td align="left" colspan="2" style="">
                                                </td>
                                                <td align="left" style="width: 23%;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                    <asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Always" runat="server"
                                                        ID="uPnl">
                                                        <ContentTemplate>
                                                            <asp:GridView CssClass="GridBorder" ID="grdvwEducationDetails" runat="server" Width="75%"
                                                                Height="90%" AutoGenerateColumns="False" PageSize="20" CellPadding="0" CellSpacing="1"
                                                                ForeColor="#333333" GridLines="None" OnRowCommand="grdvwEducationDetails_RowCommand"
                                                                OnRowDataBound="grdvwEducationDetails_RowDataBound" DataKeyNames="Qualification_Id">
                                                                <PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
                                                                </PagerStyle>
                                                                <PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
                                                                    FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
                                                                <Columns>
                                                                    <asp:BoundField DataField="Qualification_Name" HeaderText="<%$ Resources:LocalizedResources, Qualification %>" SortExpression="Qualification_Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>

                                                                    <asp:BoundField DataField="Specialization" HeaderText="<%$ Resources:LocalizedResources, Specialization %>" SortExpression="Specialization">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                    </asp:BoundField>
                                                                   
                                                                    <asp:BoundField DataField="Year_Of_Passing" HeaderText="<%$ Resources:LocalizedResources, YearOfPassing %>" SortExpression="Year_Of_Passing">
                                                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="center" VerticalAlign="Middle" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Passing_University" HeaderText="<%$ Resources:LocalizedResources, University %>" SortExpression="Passing_University">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="Class_Name" HeaderText="<%$ Resources:LocalizedResources, ClassGrade %>" SortExpression="Class_Name">
                                                                        <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" />
                                                                        <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" />
                                                                    </asp:BoundField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Edit %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnEditQualification" runat="server" CausesValidation="false"
                                                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="EDIT_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Delete %>">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnDeleteEducationalDetails" runat="server" CausesValidation="false"
                                                                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="DELETE_ROW" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <RowStyle CssClass="ClsGridRow" />
                                                                <HeaderStyle CssClass="ClsGridHeader" />
                                                                <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                            </asp:GridView>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="4">
                                                </td>
                                            </tr>
                                            <tr>
                                            <td colspan="4">
                                             <EmployeeBasicDetailsUC:EmployeeBasicDetails ID="ucEmployeeBasicDetails" runat="server"/>
                                                     <asp:HiddenField ID="hidEmpDetailsuc" runat="server" Value="" />
                                                     </td>
                                                     </tr>
                                        </table>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep4" runat="server" Title="Step 4">
                                        <table>
                                            <tr>
                                                <td width="61px" align="right">
                                                </td>
                                                <td width="100px">
                                                    <span class="ClsLblLgnd"><asp:Label ID="Label38" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, TeacherName %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                </td>
                                                <td class="ClsHilightBGB" width="234px">
                                                    <asp:Label ID="lblTeacherName" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" style="width: 100%;">
                                            <tr>
                                                <td align="center" width="50%">
                                                    <table width="70%">
                                                        <tr>
                                                            <td align="left" class="ClsBtmBorderGray">                                                                
                                                                    <span class="ClsLblLgnd" style="width:200px;font:Bold" ><asp:Label ID="Label39" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectStandards %>">
                                                </asp:Label></span>
                                                                <span style="color: red; font-family: Arial"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="height: 5px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" width="90%" class="ClsGridBG">
                                                                <div id="divStandards" runat="server" align="center" style="width: 100%; overflow: auto;
                                                                    height: 175pt;">
                                                                    <asp:GridView ID="grdStandards" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Standard_Id,Original_Standard_Id,Teacher_Id,Teacher_Standard_Id"
                                                                        PageSize="30" Width="100%" OnRowDataBound="grdStandards_RowDataBound" AllowPaging="false" CssClass="GridBorder" style="border-spacing: 0px !important;">
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                            NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <input id="ChkAllDel1" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientgrdStandards,this,'ChkBoxStdSelect', false)"
                                                                                        type="checkbox" />
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="30px" CssClass="paddingLSML" HorizontalAlign="Left" />
                                                                                <HeaderStyle Width="30px" CssClass="paddingLSML" HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ChkBoxStdSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Standard_Name" HeaderText="<%$ Resources:LocalizedResources, StandardName %>" SortExpression="Standard_Name">
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="90%" CssClass="paddingLSML" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                </div>
                                                                <asp:CustomValidator ID="cst_valStandards"  ValidationGroup="Save" runat="server" Display="None" ClientValidationFunction="ConfirmValidationForStandard"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="center" width="50%">
                                                    <table style="width: 80%;">
                                                        <tr>
                                                            <td align="left" class="ClsBtmBorderGray">                                                                
                                                                    <span class="ClsLblLgnd" style="font:Bold" >
                                                                    <asp:Label ID="Label40" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectSubjects %>">
                                                </asp:Label></span>
                                                                <span style="color: red; font-family: Arial"></span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="height: 5px">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" width="90%" class="ClsGridBG">
                                                                <div id="divSubjects" runat="server" align="center" style="width: 100%; overflow: auto;
                                                                    height: 175pt;">
                                                                    <asp:GridView ID="grdSubjects" runat="server" AutoGenerateColumns="False" CellPadding="0"
                                                                        CellSpacing="1" ForeColor="#333333" GridLines="None" DataKeyNames="Subject_Id,Original_Subject_Id,Teacher_Id,Teacher_Subject_Id"
                                                                        PageSize="100" Width="100%" OnRowDataBound="grdSubjects_RowDataBound" AllowPaging="false" CssClass="GridBorder" style="border-spacing: 0px !important;">
                                                                        <HeaderStyle CssClass="ClsGridHeader" />
                                                                        <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
                                                                            NextPageText="Next" Position="TopAndBottom" PreviousPageText="Previous" />
                                                                        <EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
                                                                        <AlternatingRowStyle CssClass="ClsGridAltRow" />
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <input id="ChkAllDel" runat="server" onclick="CheckAllOrUncheckAllGridItems(document,_clientgrdSubjects,this,'ChkBoxDivSelect', false)"
                                                                                        type="checkbox" />
                                                                                </HeaderTemplate>
                                                                                <ItemStyle Width="30px" CssClass="paddingLSML" HorizontalAlign="Left" />
                                                                                <HeaderStyle Width="30px" CssClass="paddingLSML" HorizontalAlign="Left" />
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="ChkBoxDivSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="Subject_Name" HeaderText="<%$ Resources:LocalizedResources, SubjectName %>" SortExpression="Subject_Name">
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="90%" CssClass="paddingLSML" />
                                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
                                                                            </asp:BoundField>
                                                                        </Columns>
                                                                        <RowStyle CssClass="ClsGridRow" />
                                                                        <PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
                                                                    </asp:GridView>
                                                                </div>
                                                                <asp:CustomValidator ID="cst_valSelectSubject" runat="server" Display="None" ValidationGroup="Save" ClientValidationFunction="ConfirmValidation"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:WizardStep>
                                    <asp:WizardStep ID="WizardStep5" runat="server" StepType="Finish" Title="Step 5">
                                        <table border="0" style="width: 100%;">
                                        <tr>
                                        <td>
                                        <asp:Label ID="lblErrorWizard" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                        Visible="False"> </asp:Label>
                                        </td>
                                        </tr>
                                            <tr>
                                                <td align="left" class="ClsBtmBorderGray" style="height: 19px">                                                   
                                                        <span class="ClsLblLgnd" style="width:200px;font:Bold">
                                                        <asp:Label ID="Label41" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserInformation %>">
                                                </asp:Label></span></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsTextNormal">
                                                    <!-- User InfoTable starts here -->
                                                    <table id="tblUserInfo" runat="server" border="0" style="width: 100%;">
                                                        <tr runat="server" id="Tr6">
                                                            <td colspan="4" align="left" style="width: 15%; height: 5px" id="Td19" runat="server">
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr1" runat="server">
                                                            <td align="left" style="width: 15%;" id="Td1" runat="server">
                                                                <span class="ClsLabel"><asp:Label ID="Label42" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Email %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                            </td>
                                                            <td align="left" style="width: 25%;" runat="server">
                                                                <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"></asp:TextBox>                                                               
                                                                <span id="spMandatoryField" runat="server" class="ClsMdtStar">*</span>
                                                                <asp:CustomValidator ID="cstValEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="Save"
                                                                    ClientValidationFunction="EmailValidation" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                            </td>
                                                            <td align="left" style="width: 15%;" id="Td3" runat="server">
                                                                    <span class="ClsLabel"><asp:Label ID="Label43" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserName %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                            </td>
                                                            <td id="CellCustomDept" align="left" style="color: red; width: 25%;" runat="server">
                                                                <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                                <span class="ClsMdtStar">*</span>
                                                                 <asp:RegularExpressionValidator ID="regUsername" runat="server" ValidationGroup="Save" 
                                             ErrorMessage="<%$ Resources:LocalizedResources, UserNameValidation %>" style="font-size:9pt" 
                                            ValidationExpression="[A-Za-z0-9_.]+"  
                                            ControlToValidate="txtUserName"> <span class="ClsMdtStar"></span> </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="reqUserName" runat="server" 
                                            ControlToValidate="txtUserName" Display="Dynamic" style="font-size:9pt"></asp:RequiredFieldValidator> 

                                                                <asp:CustomValidator ID="cst_UserName" runat="server" ClientValidationFunction="UserNameValidation" ValidationGroup="Save"
                                                                    Display="None"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr2" runat="server">
                                                            <td align="left" id="Td4" runat="server">
                                                                    <span class="ClsLabel"><asp:Label ID="Label44" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Password %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                            </td>
                                                            <td align="left" id="Td5" runat="server">
                                                                <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" MaxLength="15" CssClass="ExLrgTxtBox"  ></asp:TextBox>
                                                                 <span class="ClsMdtStar">* </span>
                                                                <span class="ClsMdtStar">
                                                                    <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation" ValidationGroup="Save"
                                                                        ControlToValidate="txtPasswd" ValidateEmptyText="True" Display="None"></asp:CustomValidator>                                                                   
                                                                        
                                                                </span>
                                                            </td>
                                                            <td align="left" id="Td6" runat="server">
                                                                    <span class="ClsLabel"><asp:Label ID="Label45" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ConfirmPassword %>">
                                                </asp:Label><span class="colonPadding"> :</span></span>
                                                            </td>
                                                            <td align="left" id="Td7" runat="server" style="width: 25%; color: red">
                                                                <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"  
                                                                    CssClass="ExLrgTxtBox"></asp:TextBox>
                                                              <span class="ClsMdtStar">* </span>                                                               
                                                                 <asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                                        ControlToValidate="txtConfirmPasswd" Display="None" ValidateEmptyText="True"  ValidationGroup="Save"></asp:CustomValidator>
                                                            </td>
                                                        </tr>
                                                        <tr id="Tr3" runat="server">
                                                            <td>
                                                            </td>
                                                            <td align="left" colspan="3" class="LblSmlGray " valign="top">
                                                                <asp:Label ID="Label46" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PasswordCondition %>">
                                                </asp:Label><br />
                                                               <asp:Label ID="Label47" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PasswordCondition1 %>">
                                                </asp:Label> 
                                                            </td>
                                                        </tr>
                                                        <tr id="trModuleRow1" runat="server">
                                                            <td>
                                                            </td>
                                                            <td id="tdchkCanApproveRequisitions" runat="server" align="right">
                                                                <asp:CheckBox ID="chkCanApproveRequisitions" runat="server" Checked="false" CssClass="ClsLabel"
                                                                    Text="<%$ Resources:LocalizedResources, CanUserApproveRequisitions %>" TextAlign="Right" />
                                                            </td>
                                                            <td id="tdchkCanApproveVoucher" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkCanApproveVoucher" runat="server" CssClass="ClsLabel" TextAlign="Right"
                                                                    Text="<%$ Resources:LocalizedResources, CanUserApproveVouchers %>" />
                                                            </td>
                                                        </tr>
                                                        <tr id="trModuleRow2" runat="server">
                                                            <td>
                                                            </td>
                                                            <td id="tdchkCanCraeteGenerelRequisition" runat="server" align="right">
                                                                <asp:CheckBox ID="chkCanCreateGeneralRequisition" runat="server" Checked="false"
                                                                    CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, CanUserCreateGeneralRequisitions %>" TextAlign="Right" />
                                                            </td>
                                                            <td id="tdchkCanCreateVoucher" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkCanCreateVoucher" runat="server" CssClass="ClsLabel" TextAlign="Right"
                                                                    Text="<%$ Resources:LocalizedResources, CanUserCreateVouchers %>" OnClick="CreateVoucherOnChange(this);" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="right">
                                                                <asp:CheckBox ID="chkCanSanctionLeave" runat="server" Checked="false" CssClass="ClsLabel"
                                                                    Text="<%$ Resources:LocalizedResources, CanUserSanctionLeave %>" TextAlign="Right" />
                                                            </td>
                                                            <td id="tdchkCanSelfApprove" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkCanSelfApprove" runat="server" CssClass="ClsLabel" TextAlign="Right"
                                                                    Text="<%$ Resources:LocalizedResources, CanUserSelf_ApproveVoucher %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                            </td>
                                                            <td align="right">
                                                                &nbsp;</td>
                                                            <td id="td2" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkShowAllSentSMS" runat="server" CssClass="ClsLabel" TextAlign="Right"
                                                                    Text="Allow to view all sent SMS?" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                            </td>
                                                            <td id="tdCanDeleteVoucher" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkCanDeleteVoucher" runat="server" CssClass="ClsLabel" TextAlign="Right"  visible="false" 
                                                                    Text="<%$ Resources:LocalizedResources, CanUserDeleteVoucher %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td >
                                                                	
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkInternalUser" Text="<%$ Resources:LocalizedResources, IsInternalUser %>" runat="server"  CssClass="ClsLabel" />
                                                            </td>
                                                            <td id="tdCanEditFinYear" runat="server" align="right" colspan="2">
                                                                <asp:CheckBox ID="chkCanEditOldFinancialYear" runat="server" CssClass="ClsLabel" TextAlign="Right" visible="false" 
                                                                    Text="<%$ Resources:LocalizedResources, CanUserEditOldFinancialYear %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                        <td></td>
                                                             <td id="tdPublishorUnpublish"  runat="server" >
                                                                <asp:CheckBox ID="chkPublishorUnpublishExam"  runat="server" CssClass="ClsLabel" TextAlign="Right" 
                                                                    Text="<%$ Resources:LocalizedResources, CanUserPublish_UnPublishExam %>"  />
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkSendSMS" runat="server" Text="<%$ Resources:LocalizedResources, SendSMS %>" CssClass="ClsLabel" />
                                                            </td>
                                                        </tr>
                                                    
                                                    </table>
                                                    <!-- User InfoTable end here -->
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:WizardStep>
                                </WizardSteps>
                                <FinishNavigationTemplate>
                                    <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False"  CommandName="MovePrevious" OnClientClick="TakePassward()"
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Previous %>" />
                                    <asp:Button ID="FinishButton" runat="server" CommandName="MoveComplete" CssClass="ClsBtnMid" ValidationGroup="Save"
                                        Text="<%$ Resources:LocalizedResources, Finish %>" />
                                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                                </FinishNavigationTemplate>
                                <StartNavigationTemplate>
                                    <asp:Button ID="StartNextButton" runat="server" CausesValidation="True" CommandName="MoveNext" 
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Next %>" />&nbsp;
                                        <asp:Button ID="btnStepSave" runat="server" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Finish %>" OnClick="btnStepSave_Click"  CausesValidation="true"  />
                                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                                </StartNavigationTemplate>
                                <StepNavigationTemplate>
                                    <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious" 
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Previous %>" />
                                    <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" CssClass="ClsBtnMid" ValidationGroup="Save" 
                                        Text="<%$ Resources:LocalizedResources, Next %>" />
                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Save %>" OnClick="btnSave_Click" ValidationGroup="Save" 
                                        Visible="false" />
                                        <asp:Button ID="btnStepSave" runat="server" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Finish %>" OnClick="btnStepSave_Click" ValidationGroup="Save" CausesValidation="true" />
                                    <asp:Button ID="CancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                                        CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Cancel %>" />
                                </StepNavigationTemplate>
                                <StepStyle ForeColor="#333333" CssClass="td-vertical-align-top" />
                                <SideBarStyle BackColor="#507CD1" VerticalAlign="Top" />
                                <NavigationButtonStyle CssClass="ClsBtnMid" />
                                <SideBarButtonStyle BackColor="#507CD1" Font-Names="Verdana" ForeColor="White" />
                                <HeaderStyle BackColor="#284E98" BorderColor="#EFF3FB" BorderStyle="Solid" BorderWidth="2px"
                                    Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
                            </asp:Wizard>
                        </td>
                    </tr>
                </table>
                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                    <ContentTemplate>
                        <asp:HiddenField ID="hidQualificationId" runat="server" />
                        <asp:HiddenField ID="hidSelectedIndex" runat="server" />
                        <asp:HiddenField ID="hidSlectedExpIndex" runat="server" />
                        <asp:HiddenField ID="hidUserId" runat="server" />
                        <asp:HiddenField ID="hidTeacherId" runat="server" />
                        <asp:HiddenField ID="hidPassword" runat="server" />
                        <asp:HiddenField ID="hidHeadMFlag" runat="server" />
                        <asp:HiddenField ID="HidBackUrl" runat="server" />
                        <asp:HiddenField ID="hidDesginationId" runat="server" />
                        <asp:HiddenField ID="hidHeadMasterDesgnID" runat="server" />
                        <asp:HiddenField ID="hidIsConfig" runat="server" />
                        <asp:HiddenField ID="hidStep" runat="server" />
                        <asp:HiddenField ID="hidExperienceDetailsId" runat="server" />
                        <asp:HiddenField ID="hidSchoolName" runat="server" />
                        <asp:HiddenField ID="hidJoinDate" runat="server" />
                        <asp:HiddenField ID="hidLeftDate" runat="server" />
                        <asp:HiddenField ID="hidMode" runat="server" />
                        <asp:HiddenField ID="hidRetirementAge" runat="server" Value="0"/>
						<asp:HiddenField ID="hidIndex" runat="server" />
                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                        <asp:HiddenField ID="hidRetAge" runat="server" />
                        <asp:HiddenField ID="hidCultureInfo" runat="server" />
                        <asp:HiddenField ID="hidEmailNotBlankMessage" runat="server" />
                        <asp:HiddenField ID="hidEmailShouldBeValidFormat" runat="server" />
                        <asp:HiddenField ID="hidUserNameShouldNotBlank" runat="server" />
                        <asp:HiddenField ID="hidUserNameShouldBeOfMinSixChar" runat="server" />
                        <asp:HiddenField ID="hidLocalPincodeShouldNotBlank" runat="server" />
                        <asp:HiddenField ID="hidLocalPincodeValidation" runat="server" />
                        <asp:HiddenField ID="hidYearOfPassingShouldNotBlank" runat="server" />
                        <asp:HiddenField ID="hidYearOfPassingValidation" runat="server" />
                        <asp:HiddenField ID="hidYearOfPassingInvalid" runat="server" />
                        <asp:HiddenField ID="hidYearOfPassingValidation1" runat="server" />
                        <asp:HiddenField ID="hidDateOfBirthFutureDate" runat="server" />
                        <asp:HiddenField ID="hidAgeShouldBeLessThan" runat="server" />
                        <asp:HiddenField ID="hidYears" runat="server" />                        
                        <asp:HiddenField ID="hidAgeValidationCondition" runat="server" />
                        <asp:HiddenField ID="hidSubjectValidation" runat="server" />
                        <asp:HiddenField ID="hidStandardCondition" runat="server" />
                        <asp:HiddenField ID="hidMobileNumberValidation1" runat="server" />
                        <asp:HiddenField ID="hidMobileDigit" runat="server" />
                        <asp:HiddenField ID="hidNewConfirmSamePwdErrorMsg" runat="server" />
                        <asp:HiddenField ID="hidPasswordCondition1" runat="server" />
                        <asp:HiddenField ID="hidPasswordConditionErrorMsg" runat="server" />
                        <asp:HiddenField ID="hidPasswordShouldNotBlank" runat="server" />
                        <asp:HiddenField ID="hidAreYouSureDeleteEducationalDetails" runat="server" />
                        <asp:HiddenField ID="hidAreYouSureDeleteExperienceDetails" runat="server" />
                        <asp:HiddenField ID="hidLeftDateJoinedDateValidation" runat="server" />
                        <asp:HiddenField ID="hidbtnAddText" runat="server" />
                        <asp:HiddenField ID="hidbtnAddDetailsText" runat="server" />
                        <asp:HiddenField ID="hidPasswordOfTeacher" runat="server" />
                        <asp:HiddenField ID="hidVisibalStatusCombo" runat="server" />
                        <asp:HiddenField ID="hidIsPPSNSchool" runat="server" Value="N" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <!-- Data Insert End Here -->
            </td>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">

    _clientGridId = "<%=this.grdStandards.ClientID %>";    
    _clienttxtLocalAddressId = "<%=this.txtLocalAddress.ClientID %>";
    _clienttxtPerAddressId = "<%=this.txtPerAddress.ClientID %>";
    _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
    _clientcstValEmailId = "<%=this.cstValEmail.ClientID %>";
    _clienttxtUserName = "<%=this.txtUserName.ClientID %>";
    _clientcst_UserNameId = "<%=this.cst_UserName.ClientID %>";
    _clienttxtLocalPincode = "<%=this.txtLocalPincode.ClientID %>";
    _clientcst_LocalPin = "<%=this.cst_LocalPin.ClientID %>";
    _clienttxtYearOfPassing = "<%=this.txtYearOfPassing.ClientID %>";
    _clientcst_YearOfPassing = "<%=this.cst_YearOfPassing.ClientID %>";
    _clientcal_DOB = "<%=this.cal_DOB.ClientID %>";
    _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
    _client_AddDetails= "<%=this.btnAddDetails.ClientID %>";
    _client_Add = "<%=this.btnAdd .ClientID %>";
    _clientgrdvwEducationDetailsID = "<%=this.grdvwEducationDetails.ClientID %>";
    _clientgrdSubjects = "<%=this.grdSubjects.ClientID %>";
    _clientcst_valSelectSubject = "<%=this.cst_valSelectSubject.ClientID %>";
    _clientgrdStandards = "<%=this.grdStandards.ClientID %>";
    _clientcst_valStandards = "<%=this.cst_valStandards.ClientID %>";
    _clientchkAddress = "<%=this.chkAddress.ClientID %>";
    _clienttxtLocalCity = "<%=this.txtLocalCity.ClientID %>";
    _clienttxtState = "<%=this.txtState.ClientID %>";
    _clienttxtPerCity = "<%=this.txtPerCity.ClientID %>";
    _clienttxtPerPinCode = "<%=this.txtPerPinCode.ClientID %>";
    _clientcmbPerState = "<%=this.txtPerState.ClientID %>";
    _clienttxtPasswd = "<%=this.txtPasswd.ClientID %>";
    _clientcstValPassword = "<%=this.cstValPassword.ClientID %>";
    _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
    _clientcstValConfirmPassword = "<%=this.cstValConfirmPassword.ClientID %>"
    _clienttxtMobileNumber = "<%=this.txtMobileNumber.ClientID %>";
    _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
    _clientlbl_lblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
    _clientFinishButton = "<%=this.wizard_TeacherInfo.FindControl("FinishNavigationTemplateContainerID").FindControl("FinishButton").ClientID %>";
    _clientCancelButton = "<%=this.wizard_TeacherInfo.FindControl("FinishNavigationTemplateContainerID").FindControl("CancelButton").ClientID %>";
     _clienthidHeadMFlag ="<%=this.hidHeadMFlag.ClientID %>"
    _clientcmbDesingnation ="<% = this.cmbDesignation.ClientID %>"
     _client_LblErrMsg ="<%=this.lblErrorMsgForStd.ClientID %>"
     _clienthidDesginationId ="<%=this.hidDesginationId.ClientID %>"
     _clienthidHeadMasterDesgnID ="<%=this.hidHeadMasterDesgnID.ClientID %>" 
     _clientcstFormId = "<%=this.cstForm.ClientID %>"
     _clienttxtJoinDate = "<%=this.txtjoinedDate.ClientID %>"
     _clienttxtEndDate = "<%=this.txtLeftDate.ClientID %>"
     _clienttxtSchoolName = "<%=this.txtSchoolname.ClientID %>"
     _clientcstJoinResineDate = "<%=this.cstJoinResignDate.ClientID%>"
     _clientlblChkDuplicate = "<%=this.lblChkDuplicate.ClientID %>"
     _clientbtnCancelDetails = "<%=this.btnCancelDetails.ClientID %>"
    _clientchkCanSelfApprove = "<%= this.chkCanSelfApprove.ClientID %>";
    _clientNewPassward='<%=this.hidPassword.ClientID %>'
    _clienthidRetirementAge ="<%=this.hidRetirementAge.ClientID %>" 
    _clienthidRetAge ="<%=this.hidRetAge.ClientID %>" 
    _clientcmbTeachingForClass = "<%=this.cmbTeachingForClass.ClientID %>"
    _clientcst_StandardCategory = "<%=this.cst_StandardCategory.ClientID %>"
    _clienthidVisibalStatusCombo= "<%=this.hidVisibalStatusCombo.ClientID %>"
    _clienthidIsPPSNSchool = "<%=this.hidIsPPSNSchool.ClientID %>"

   
     

    //This function is used to check whetjer principal is already present ot not.
    function CheckIfPrincipalAlreadyExists(oSrc,args)
    {
       var sFlagVal = document.getElementById(_clienthidHeadMFlag).value;
       var DesignationIndex = document.getElementById(_clientcmbDesingnation).value;
       var DesignationID = parseInt(document.getElementById(_clienthidDesginationId).value);
       var HeadMasterDesgID = parseInt(document.getElementById(_clienthidHeadMasterDesgnID).value);
       if(DesignationID == HeadMasterDesgID)
           sFlagVal="N" ;
       
       if(sFlagVal =="Y")
       {       
           if(parseInt(DesignationIndex) == HeadMasterDesgID)
           {
                args.IsValid =false;
                return true;
           }
       }
        args.IsValid =true ;  
        return false;   
   }
    
   function CheckStandardCategory(oSrc,args)
    {    
        var Selectedvalue = document.getElementById(_clientcmbTeachingForClass).value;
        var IsVisibelStatusCombo = document.getElementById(_clienthidVisibalStatusCombo).value;

        if(IsVisibelStatusCombo == 1)
        {
            if(Selectedvalue == 0)    
            {
                 args.IsValid = false;
                 return true;
            }
            else
            {
                args.IsValid = true ;  
                return false;   
            }
        }
        else
        {
             args.IsValid = true ;  
             return false;  
        }
            
    }

    //This function is used to disable controls.
    function disableControls()
    {
        var isPageValid = true ;
                      
           if (typeof(Page_ClientValidate) == 'function')
                isPageValid = Page_ClientValidate(); 
           
           var finishButton =  document.getElementById(_clientFinishButton);
           var CancelButton =  document.getElementById(_clientCancelButton);
           var AddDetailsButton =  document.getElementById(_client_AddDetails);
           var AddButton=document.getElementById(_client_Add);
           
        if(isPageValid)
        {    
            if(finishButton!=null)
                finishButton.disabled=true;            
            if(CancelButton!=null)
            CancelButton.disabled=true;        
             if(AddDetailsButton!=null)
            AddDetailsButton.disabled=true; 
            if(AddButton!=null)
            AddButton.disabled=true;          
             __doPostBack(document.getElementById(_clientFinishButton).name,'');           
        }
    }
    
   //This function is used to reset error messages.
   function ResetErrorMsgLbl() 
   {
     if(document.getElementById(_clientlbl_lblErrorMsg)!=null)
     {
        document.getElementById(_clientlbl_lblErrorMsg).innerText =" ";
        document.getElementById(_clientlbl_lblErrorMsg).innerHTML =" ";
     }         
     if(document.getElementById(_client_LblErrMsg)!=null)
     {
        document.getElementById(_client_LblErrMsg).innerText =" ";
        document.getElementById(_client_LblErrMsg).innerHTML =" ";
     }
   } 
   
 var blanks = " \t\n\r";  // Ek whitespace chars
//This function is used to validate Email address.
function EmailValidation(oSrc, args)
{  
    var sEmail=document.getElementById(_clienttxtEmailId).value;
    var IsPPSNSchool = document.getElementById(_clienthidIsPPSNSchool).value;
    sEmail=stripLeadingTrailingBlanks(sEmail);
    if(IsPPSNSchool == "Y")
    {
        // If email is not blank then validate for valid email address.
        if(sEmail != "")
        {
	        if (!isEmail(sEmail))
	        {
	           document.getElementById(_clientcstValEmailId).errormessage=document.getElementById("<%=hidEmailShouldBeValidFormat.ClientID%>").value;
               args.IsValid =false ;
               return true;	       
	        }
        }
    }
    else
    {
        if (isEmpty(sEmail))
	    {
           document.getElementById(_clientcstValEmailId).errormessage=document.getElementById("<%=hidEmailNotBlankMessage.ClientID%>").value;
           args.IsValid =false ;
           return true;         
	    }
	    else 
	    {
	    // If email is not blank then validate for valid email address.
	        if (!isEmail(sEmail))
	        {
	           document.getElementById(_clientcstValEmailId).errormessage=document.getElementById("<%=hidEmailShouldBeValidFormat.ClientID%>").value;
               args.IsValid =false ;
               return true;	       
	        }
	    }
    }
	args.IsValid =true ;
    return false;	
}
//This function is used to validate user name.
function UserNameValidation(oSrc, args)
{

    ResetErrorMsgLbl();

    var   sEmail=document.getElementById(_clienttxtUserName).value;
    sEmail=stripLeadingTrailingBlanks(sEmail);
    if (isEmpty(sEmail))
    {
           document.getElementById(_clientcst_UserNameId).errormessage=document.getElementById("<%=hidUserNameShouldNotBlank.ClientID%>").value;
           args.IsValid =false ;
           return true;         
    }

    if(sEmail.length < 6)
    {     
      document.getElementById(_clientcst_UserNameId).errormessage=document.getElementById("<%=hidUserNameShouldBeOfMinSixChar.ClientID%>").value;
      args.IsValid =false ;
      return true; 
    }
    args.IsValid =true ;
           return false;	
}

//This function is used to validate pin code.
function PinCodeValidation(oSrc, args)
{
  var sMobileNumber = document.getElementById(_clienttxtLocalPincode).value;
   sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
  if(sMobileNumber.length==0)
  {
      document.getElementById(_clienttxtLocalPincode).value = "";
      document.getElementById(_clientcst_LocalPin).errormessage=document.getElementById("<%=hidLocalPincodeShouldNotBlank.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
  
  if(sMobileNumber.length != 6)
  {     
      document.getElementById(_clientcst_LocalPin).errormessage=document.getElementById("<%=hidLocalPincodeValidation.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
   args.IsValid =true ;
   return false;   
} 
//This function is used to validate year.
function YearValidation(oSrc, args)
{ 
  var sYear = document.getElementById(_clienttxtYearOfPassing).value;
  sYear = stripLeadingTrailingBlanks(sYear);
  var today = new Date();
  var thisYear =  parseInt(today.getFullYear());
  var yearDiff = thisYear - parseInt(sYear);
  var RetirementAge = $get(_clienthidRetirementAge).value; 

  if(sYear.length==0)
  {
      document.getElementById(_clienttxtYearOfPassing).value = "";
      document.getElementById(_clientcst_YearOfPassing).errormessage=document.getElementById("<%=hidYearOfPassingShouldNotBlank.ClientID%>").value;
      args.IsValid =false ;
      args.IsValid =false ;
      return true; 
  }
  if(sYear.length != 4)
  {     
      document.getElementById(_clientcst_YearOfPassing).errormessage=document.getElementById("<%=hidYearOfPassingValidation.ClientID%>").value;
      args.IsValid =false ;
      args.IsValid =false ;
      return true; 
  }
  if(yearDiff > RetirementAge)
  {
    document.getElementById(_clientcst_YearOfPassing).errormessage=document.getElementById("<%=hidYearOfPassingInvalid.ClientID%>").value;
      args.IsValid =false ;
    args.IsValid =false ;
    return true; 
  }
  if(parseInt(yearDiff) < 0)
  {
    document.getElementById(_clientcst_YearOfPassing).errormessage=document.getElementById("<%=hidYearOfPassingValidation1.ClientID%>").value;
      args.IsValid =false ;
    args.IsValid =false ;
    return true; 
  }
   args.IsValid =true ;
   return false;   
} 

//This function is used to change date format.
function getDateString(obj)
{
    
     var strDate = obj.getDate() + "-"; 
     var strMonth =parseInt( obj.getMonth())+ 1; 
         strMonth = getMonthName(strMonth);
         strDate = strDate+ strMonth + "-";  
         strDate = strDate+ obj.getFullYear() ;
    return strDate;
}

//This function is used to get month name.
function getMonthName(month)
{
    switch(month)
    {
        case 1:
            return "Jan";
        break;
        
        case 2:
             return "Feb";
        break;
        
        case 3:
             return "March";
        break;
       
        case 4:
             return "April";
        break;
        
        case 5:
             return "May";
        break;
       
        case 6:
             return "June";
        break;
       
        case 7:
             return "july";
        break;
        
        case 8:
            return "Aug";
        break;
       
        case 9:
            return "Sep";
        break;
       
        case 10:
         return "Oct";
        break;
      
        case 11:
            return "Nov";
        break;
       
        case 12:
         return "Dec";
        break;
    }
}

//This function is used to validate DOB.
function DOBValidation(oSrc, args)
{   
    var oDOBObj;
    var RetirementAge = $get(_clienthidRetirementAge).value; 
    oDOBObj = document.getElementById(_clientcal_DOB).value;    
    var sDate ;   

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
  var thisYear =  parseInt(today.getFullYear());
  
  var yearDiff = thisYear - parseInt(DOBYear);
  
  var sMinYear = parseInt(thisYear) - 60;
  var sMaxYear = parseInt(thisYear) - 18;
  var RetConfigAge = $get(_clienthidRetAge).value;

  if(today <= sDate )
  {    
      document.getElementById(_clientcst_DOB).errormessage=document.getElementById("<%=hidDateOfBirthFutureDate.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
 
  if(parseInt(RetConfigAge) > 0)
  {  
      if (sDate < RetDate) 
      {
        
        document.getElementById(_clientcst_DOB).errormessage=document.getElementById("<%=hidAgeShouldBeLessThan.ClientID%>").value + " " + RetConfigAge + " "+document.getElementById("<%=hidYears.ClientID%>").value;
        args.IsValid =false ;
        return true; 
      }
  }
  if(parseInt(yearDiff) < 18)
  {
  
    document.getElementById(_clientcst_DOB).errormessage=document.getElementById("<%=hidAgeValidationCondition.ClientID%>").value;
    args.IsValid =false ;
    return true; 
  }
   args.IsValid =true ;   
   
      return false;   
} 

//This function is used to validate educational details.
function validateGridData(oSrc, args)
{
  var grdViewElement = document.getElementById(_clientgrdvwEducationDetailsID) 
   if (null == grdViewElement)
    {  
       args.IsValid = false;
        return true;
    }
     else
     {
        if(grdViewElement.rows.length  > 0)
        {
            args.IsValid = true;
            return false;
        }
        else
        {
            args.IsValid = false;
            return true;
        }
     }        
}

//This function is used to display message if no one subject is selected.
function ConfirmAction(iPageCount, sActionName)
{
    var bResult = true;
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document,_clientgrdSubjects,'ChkBoxDivSelect',sActionName,'false', false, 'false'))
        bResult= true;
    else
     bResult =false;
    return bResult;
} 

function ConfirmValidation(oSrc, args)
{                                         
   var sActionName = document.getElementById("<%=hidSubjectValidation.ClientID%>").value;
   var IsTrue =  ConfirmAction(1,sActionName) 
    if(IsTrue == false)
    {
        document.getElementById(_clientcst_valSelectSubject).errormessage = document.getElementById("<%=hidSubjectValidation.ClientID%>").value;
        args.IsValid =false ;
        return true;
     }
     
    args.IsValid =true ;
    return false;    
 }
 
 //This function is used to display message if no one standard is se;lected.
function ConfirmActionForStandard(iPageCount, sActionName)
{
    var bResult = true;
    if (CheckIfAtleastOneCheckboxInGridIsSelected(document,_clientgrdStandards,'ChkBoxStdSelect',sActionName,'false', false, 'false'))
        bResult= true;
    else
        bResult =false;
    return bResult;
} 

 //This function is used to validate standard.
 function ConfirmValidationForStandard(oSrc, args)
 {
   var sActionName = document.getElementById("<%=hidStandardCondition.ClientID%>").value;   
   var IsTrue = ConfirmActionForStandard(1,sActionName) 
    if(IsTrue == false)
    {
        document.getElementById(_clientcst_valStandards).errormessage = document.getElementById("<%=hidStandardCondition.ClientID%>").value;
        args.IsValid =false ;
        return true;
     }
     
    args.IsValid =true ;
    return false;    
 }
 //This function is used to fill permanant address.
function FillPermanentAddress()
{ 
   if(document.getElementById(_clientchkAddress) != null)
   {
     var  sLocalAddr= document.getElementById(_clienttxtLocalAddressId).value;
     var  sLocalCity = document.getElementById(_clienttxtLocalCity).value;
     var  sLocalPin = document.getElementById(_clienttxtLocalPincode).value;  
     var sLocalState = document.getElementById(_clienttxtState).value;  
      if(document.getElementById(_clientchkAddress).checked == true)
      {      
        document.getElementById(_clienttxtPerAddressId).value = sLocalAddr;        
        document.getElementById(_clienttxtPerCity).value = sLocalCity;
        document.getElementById(_clienttxtPerPinCode).value = sLocalPin;
        document.getElementById(_clientcmbPerState).value = sLocalState;
        document.getElementById(_clienttxtPerAddressId).disabled = true;
        document.getElementById(_clienttxtPerCity).disabled = true; 
        document.getElementById(_clienttxtPerPinCode).disabled = true;
        document.getElementById(_clientcmbPerState).disabled =  true;
      }
      else
      {
        document.getElementById(_clienttxtPerAddressId).disabled = false;
        document.getElementById(_clienttxtPerCity).disabled = false; 
        document.getElementById(_clienttxtPerPinCode).disabled = false;
        document.getElementById(_clientcmbPerState).disabled =  false;
      }
   }
}
    
//This function is used to validate password.
function PasswordValidation(oSrc, args)
{
  var sPassword = document.getElementById(_clienttxtPasswd).value;        
   var password = sPassword;
         //   var passed = validatePassword(password, {
           //     length: [6, Infinity],                
             //   alpha: 1,
               // numeric: 1,
                //special: 1
            //});
            var passed = CheckPassword(sPassword)
  if(sPassword=="")
  {     
      document.getElementById(_clientcstValPassword).errormessage=document.getElementById("<%=hidPasswordShouldNotBlank.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
  else if(sPassword.length < 6)
  {     
      document.getElementById(_clientcstValPassword).errormessage=document.getElementById("<%=hidPasswordConditionErrorMsg.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
  else if (!passed) {
      document.getElementById(_clientcstValPassword).errormessage = document.getElementById("<%=hidPasswordCondition1.ClientID%>").value;
      args.IsValid = false;
      return true;
 }

   args.IsValid =true ;
      return false;   
} 
    function CheckPassword(inputtxt) {
            var decimal =   /^(?=.*\d)(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9])(?!.*\s).{6,15}$/;  
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
            var sPassword = document.getElementById(_clienttxtPasswd).value
            if (sPassword!=sConfirmPassword) {
                document.getElementById(_clientcstValConfirmPassword).errormessage = document.getElementById("<%=hidNewConfirmSamePwdErrorMsg.ClientID%>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

//This function is used to validate mobile number.
function MobileNumberValidation(oSrc, args)
{
  var sMobileNumber = document.getElementById(_clienttxtMobileNumber).value;
   sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber);
   document.getElementById(_clientcst_MobileNumber).errormessage = "";
  if(sMobileNumber.length < 10)
  {     
      document.getElementById(_clientcst_MobileNumber).errormessage=document.getElementById("<%=hidMobileDigit.ClientID%>").value;
      args.IsValid =false ;
      return true; 
  }
  else if (sMobileNumber.substring(0, 1) == '0') {
            document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=hidMobileNumberValidation1.ClientID%>").value;
                args.IsValid = false;
                return true;
                }
   args.IsValid =true ;
      return false;   
} 

//This function is used to display confirmation message to delete educational details.
function ConfirmDelete()
{
    var bResult = true;
    if (!window.confirm(document.getElementById("<%=hidAreYouSureDeleteEducationalDetails.ClientID%>").value) )
     bResult= false;
    return bResult;
}
 function DeleteExpDetails() {
            var bResult = true
            if (!window.confirm(document.getElementById("<%=hidAreYouSureDeleteExperienceDetails.ClientID%>").value)) {
                bResult = false
            }
            return bResult
        }
        
function ValidateControls(oSrc, args) {
	var JoinDate
	var LeftDate
	
	if (document.all) {
		JoinDate = new Date((document.getElementById(_clienttxtJoinDate).value).replace('-', ' '))
		LeftDate = new Date((document.getElementById(_clienttxtEndDate).value).replace('-', ' '))
	}
    else {
		JoinDate = new Date(document.getElementById(_clienttxtJoinDate).value.replace(/-/g,' '))
		LeftDate = new Date(document.getElementById(_clienttxtEndDate).value.replace(/-/g,' '))
	}
	if (JoinDate > LeftDate) {
		oSrc.errormessage = document.getElementById("<%=hidLeftDateJoinedDateValidation.ClientID%>").value;
		args.IsValid = false
		return true
	}
}

  function ResetUpdateLbl() {
          
            if(document.getElementById(_clientlblChkDuplicate) != null) {
                document.getElementById(_clientlblChkDuplicate).style.display = "none"
                document.getElementById(_clientlblChkDuplicate).innerHTML = "" 
                }
        }

        // This function enables disables the CanSelfApprove checkbox when CanApproveVoucher checkbox is checked/unchecked
        function CreateVoucherOnChange(src) {
        	var chkCanSelfApprove = $get(_clientchkCanSelfApprove);
        	if (chkCanSelfApprove)
        		chkCanSelfApprove.disabled = !src.checked;
        	if (!src.checked)
        		chkCanSelfApprove.checked = false;
        }

            //This function is used to take password password.
function TakePassward()
{
 
 document.getElementById(_clientNewPassward).value= document.getElementById(_clienttxtPasswd).value;        
  
} 
    </script>
    <script language="javascript" type="text/javascript"">

        FillPermanentAddress();

    </script>


</asp:Content>
