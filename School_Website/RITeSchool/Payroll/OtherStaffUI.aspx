<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="OtherStaffUI.aspx.cs" Inherits="OtherStaffUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<%@ Register Src="~/UserControls/UserBasicDetails.ascx" TagName="UserBasicDetailsUC" TagPrefix="UserBasicDetailsUC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>               
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                    vertical-align: top">
                    <tr>
                        <td id="MainDataTable" align="center">
                            <!-- Data Insert Here -->
                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 77%">
                                                    <asp:Panel ID="pnlErrorMsg" runat="server" Width="96%">
                                                        <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="Red"
                                                            Height="20px" Width="100%" CssClass="ClsMdtStar"></asp:Label>
                                                    </asp:Panel>
                                                </td>
                                                <td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
                                                    <span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                                        ValidationGroup="Save" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trname">
                                    <td colspan="1" class="ClsTextNormal" align="center">
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                        <!-- User InfoTable starts here -->
                                        <table id="tblUsername" runat="server" border="0" cellpadding="1" cellspacing="2"
                                            style="width: 575px; margin-left: 19px;">
                                            <tr >
                                                <td align="left" class="ClsBorderLight" style="width: 20%">
                                                    <asp:Label CssClass = "ClsLabel" ID="lblName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                    <asp:Label CssClass = "LblSmlGray floatR" ID="lblFirstName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, FirstName%>"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
                                                    <asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        Width="186px"></asp:TextBox>*
                                                    
                                                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName"
                                                        Display="None" ErrorMessage= "<%$ Resources:LocalizedResources, FirstNameValidation%>" ValidationGroup="Save" ></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 20%;">
                                                     <asp:Label CssClass = "LblSmlGray floatR" ID="Label1" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MiddleInitial %>"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="50" onblur="formatName(this)"
                                                        Width="186px"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 20%">
                                                   <asp:Label CssClass = "LblSmlGray floatR" ID="lblLastName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LastName%>"></asp:Label>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" CssClass="MidTxtBox" onblur="formatName(this)"></asp:TextBox>
                                                    *<asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName"
                                                        Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValLastNameBlank%>" ValidationGroup="Save"> </asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 200px;">
                                                      <asp:Label CssClass = "ClsLabel" ID="lblAddress" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Address%>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                 </td>
                                                 <td align="left" class="ClsMdtStar">
                                                     <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                                    Height="72px" Width="240px"></asp:TextBox>*
                                                    
                                                    <asp:CustomValidator ID="cstValAddress" runat="server" 
                                                    ClientValidationFunction="validateAddress" CssClass="ClsMdtStar" 
                                                    Display="None" EnableClientScript="true" ErrorMessage="Error msg" 
                                                    ValidationGroup="Save" Visible="true"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="ClsBorderlight" align="center" style="width: 200px;">
                                                    <asp:Label CssClass = "ClsLabel" ID="lblDateBirth" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left" valign="top" style="width: 31%">
                                                    <asp:TextBox ID="txtDOB" runat="server" CssClass="SmlTxtBox" MaxLength="11"></asp:TextBox>
                                                    <rjs:PopCalendar ID="cFromDate" runat="server" Control="txtDOB" Format="dd MMM yyyy" Culture = "en"
                                                        ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage= "<%$ Resources:LocalizedResources, DateErrorMsg%>"
                                                        ControlFocusOnError="True" To-Today="true" ValidationGroup="Save" />
                                                        <asp:CustomValidator ID="cst_DOB" runat="server" ControlToValidate="txtDOB" ClientValidationFunction="DOBValidation"
                                                      ValidationGroup="Save" Display="None" CssClass="ClsLabel"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 200px;">
                                                    <asp:Label CssClass = "ClsLabel" ID="lblMobileNumber" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MobileNumber %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left" class="ClsMdtStar" style="width: 31%">
                                                    <asp:TextBox ID="txtMobileNo" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                        ondrop="event.returnValue=false" />
                                                    *<asp:RequiredFieldValidator ID="reqvalMobileNo" runat="server" ControlToValidate="txtMobileNo"
                                                        Display="None" ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, MobileNumberBlank %>"></asp:RequiredFieldValidator>
                                                    <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                                                        Visible="true" ErrorMessage= "<%$ Resources:LocalizedResources, MobileDigit %>"  EnableClientScript="true"
                                                        ClientValidationFunction="MobileNumberValidation" ValidationGroup="Save"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 42%">
                                            <asp:Label CssClass = "ClsLabel" ID="lblEmergencyNumber" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, EmergencyContact %>"></asp:Label>
                                             <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                        <asp:TextBox ID="txtEmergencyNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                            ondrop="event.returnValue=false" />&nbsp;*
                                       <asp:RequiredFieldValidator ID="reqEmergencyNo" runat="server" ControlToValidate="txtEmergencyNo"
                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valEmergencyContactNo %>" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                        
                                    </td>
                                </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 200px;">                                                    
                                                     <asp:Label CssClass = "ClsLabel" ID="lblEmail" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Email %>"></asp:Label>
                                             <span class="ClsLabel colonPadding">:</span>
                                                    <asp:RegularExpressionValidator ID="regValEmail" runat="server" ControlToValidate="txtEmail"
                                                        Display="None" ValidationGroup="Save" ErrorMessage= "<%$ Resources:LocalizedResources, valEmailID %>"
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                </td>
                                                <td align="left" style="width: 31%">
                                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="ExLrgTxtBox" MaxLength="50" Width="239px"></asp:TextBox>
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 200px;">
                                              <asp:Label CssClass = "ClsLabel" ID="lblDesignation" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Designation %>"></asp:Label>
                                             <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left" style="width: 31%">
                                                    <asp:DropDownList ID="cmbDesignation" runat="server" CssClass="LrgCombo">
                                                    </asp:DropDownList>
                                                    <span class="ClsMdtStar">*
                                                        <asp:CompareValidator ID="cmpDesignation" runat="server" ControlToValidate="cmbDesignation"
                                                            Display="None" ErrorMessage="<%$ Resources:LocalizedResources, valDesignation %>"  Operator="NotEqual"
                                                            Type="Integer" ValueToCompare="0" ValidationGroup="Save"></asp:CompareValidator>
                                                    </span>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 42%">
                                                    <asp:Label CssClass = "ClsLabel" ID="lblUserName" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UserName %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td id="Td1" align="left" style="color: red;">
                                                    <asp:TextBox ID="txtUserName" runat="server" MaxLength="20" 
                                                        CssClass="ExLrgTxtBox" ></asp:TextBox>
                                                        <span class="ClsMdtStar">*</span>
                                                    <asp:RegularExpressionValidator ID="regUsername" runat="server" ValidationGroup="Save" 
                                                         ErrorMessage= "<%$ Resources:LocalizedResources, ValUserName%>"  style="font-size:9pt" 
                                                        ValidationExpression="[A-Za-z0-9_.]+"  
                                                        ControlToValidate="txtUserName"> <span class="ClsMdtStar"></span> </asp:RegularExpressionValidator>
                                                    <asp:RequiredFieldValidator ID="reqUserName" runat="server" 
                                                        ControlToValidate="txtUserName" Display="Dynamic" style="font-size:9pt"></asp:RequiredFieldValidator> 
                                          
                                       
                                                    <asp:CustomValidator ID="cst_UserName" runat="server" ClientValidationFunction="UserNameValidation"
                                                        Display="None" ValidateEmptyText="True" CssClass="ClsMdtStar"
                                                        ValidationGroup="Save" ></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 42%">
                                                    <asp:Label CssClass = "ClsLabel" ID="lblPassword" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Password %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtPasswd" runat="server" TextMode="Password" MaxLength="15" CssClass="ExLrgTxtBox"></asp:TextBox>
                                                    <span class="ClsMdtStar">*
                                                        <asp:CustomValidator ID="cstValPassword" runat="server" ClientValidationFunction="PasswordValidation"
                                                            Display="None" ValidateEmptyText="True" ValidationGroup="Save"></asp:CustomValidator>                                               
                                                    </span>
                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderLight" style="width: 42%">
                                                    <asp:Label CssClass = "ClsLabel" ID="Label2" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ConfirmPassword %>"></asp:Label>
                                                     <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td class="ClsMdtStar" align="left">
                                                    <asp:TextBox ID="txtConfirmPasswd" runat="server" TextMode="Password" MaxLength="15"
                                                        CssClass="ExLrgTxtBox"></asp:TextBox>
                                                    *<asp:CustomValidator ID="cstValConfirmPassword" runat="server" ClientValidationFunction="ComparePasswordValidation"
                                                            Display="None" ValidateEmptyText="True" ValidationGroup="Save"></asp:CustomValidator>                                        
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 42%">
                                                    &nbsp;
                                                </td>
                                                <td class="ClsMdtStar" align="left">
                                                <span class="LblSmlGray">                                        
                                                     <asp:Label  ID="lblNotePass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswod %>"></asp:Label>
                                                        <br />
                                                         <asp:Label  ID="lblNoteConfirmPass" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NoteForPasswordCombination %>"></asp:Label>
                                                       </span>
                                                </td>
                                            </tr>
                                            <tr id="trSendSMS" runat="server">
                                                <td class="ClsBorderLight">
                                                    <asp:Label CssClass = "ClsLabel" ID="Label3" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SendSMS %>"></asp:Label>
                                                    <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkSendSMS" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" style="width: 200px;"  class="ClsBorderlight">
                                            <asp:Label CssClass = "ClsLabel" ID="lblPhoto" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Photo %>"></asp:Label>
                                             <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td>
                                                    <div class="ClsBorderlight" style="width: 112px; vertical-align: middle">                                                       
                                                        <img id="imgPhoto" alt="image"  runat="server" height="151" width="119"/>                                             
                                                    </div>                                                        
                                                </td>
                                            </tr>                                            
                                            <tr>
                                                <td class="ClsBorderlight" style="width: 200px;">
                                               <asp:Label CssClass = "ClsLabel" ID="lblUploadPhoto" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Upload_CapturePhoto %>"></asp:Label>
                                             <span class="ClsLabel colonPadding">:</span>
                                                </td>
                                                <td>
												<table>
									<tr>
									<td>
                                                    <asp:FileUpload ID="UploadPhoto" runat="server" />
                                                    <asp:CustomValidator ID="CustPhoto" Display="None" runat="server" ClientValidationFunction="ValidatePhoto"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat %>"  ValidationGroup="Save" ControlToValidate="UploadPhoto"
                                                        CssClass="LblErrorMsg"></asp:CustomValidator>
														</td>
											<td>
														<img id="ImgWebCam"  title= "<%$ Resources:LocalizedResources, CapturePhoto%>" runat="server" style="cursor:pointer;" src="../images/WebCam.png" />
														</td>
											</tr>
											</table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="2">
                                                    <span class="LblSmlGray"><asp:Label ID="lblUploadImage" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageForOtherStaff%>"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                                    <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageSize%>"></asp:Label></span>
                                                </td>
                                            </tr>
                                            <tr>
                                            <td colspan="2">
                                            <UserBasicDetailsUC:UserBasicDetailsUC ID="ucUserBasicDetails" runat="server"/>
                                             <asp:HiddenField ID="hidBasicDetailUserId" runat="server" Value="" />                                             
                                            </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources,Save%>" runat="server" CssClass="ClsBtn" BorderWidth="1px" disable-page="true"
                                                        CausesValidation="true" OnClick="btnSave_Click" ValidationGroup="Save" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources,Cancel%>" CssClass="ClsBtn" BorderWidth="1px"
                                                        CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- User InfoTable ListView -->
                    <tr>
                        <td align="center">
                            <table align="center">
                                <tr align="center">
                                    <td align="left">
                                         <asp:Label ID="Label6" runat="server" class="ClsLabel" Text="User Type"></asp:Label>
                                         <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left">
                                         <asp:DropDownList ID="ddlUserType" runat="server" AutoPostBack="true" Width="132px" OnSelectedIndexChanged="ddlUserType_SelectedIndexChanged">                                                                                        
                                         </asp:DropDownList>
                                    </td>
                                </tr>
                                   <tr align="center">
                                                                        <td align="left" class="ClsBorderlight">
                                                                        <asp:Label ID="Label4" runat="server" class="ClsLabel" Text="Name"></asp:Label>
                                                                         <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtName" TabIndex="1" runat="server" MaxLength="50" CssClass="MidTxtBox"  autocomplete="off"></asp:TextBox>&nbsp;
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" TabIndex="2" CssClass="ClsBtnMid remove-margin-top"
                                                                                OnClick="btnSearch_Click" CausesValidation="false"/>
                                                                        </td>
                                                                    </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trPagerOtherStaff" runat="server">
                        <td align="center">
                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="2" PagedControlID="lstvwOtherStaff">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, To%>" />
                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, OutOf%>" />
                                            <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                CssClass="LblNrmlB" />
                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text= "<%$ Resources:LocalizedResources, Records%>" />
                                            <br />
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="800px" cellpadding="1" cellspacing="2">
                                <tr>
                                    <td style="width: 800px" align="center">
                                        <asp:ListView ID="lstvwOtherStaff" runat="server" OnDataBound="lstvwOtherStaff_DataBound"
                                            DataKeyNames="OtherStaffId,UserId" OnItemDataBound="lstvwOtherStaff_ItemDataBound" OnItemCommand="lstvwOtherStaff_ItemCommand"
                                            OnSorting="lstvwOtherStaff_Sorting" DataSourceID="ObjDSOtherStaff">
                                            <LayoutTemplate>
                                                <table width="100%" runat="server" id="tblStaffInfo" style="color: #333333" cellpadding="0"
                                                    cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" width="30%" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnSortName" runat="server" CommandName="Sort" CommandArgument="Name" Text = "<%$ Resources:LocalizedResources, Name%>"
                                                                CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                            </th>
                                                        <th align="left" width="17%" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnDesignation" runat="server" CommandName="Sort" CommandArgument="DesignationId" Text = "<%$ Resources:LocalizedResources, Designation%>"
                                                                CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                        </th>
                                                        <th align="left" width="22%" style="padding-left: 9px;">
                                                            <asp:LinkButton ID="lnkBtnMobileNo" runat="server" CommandName="Sort" CommandArgument="MobileNo" Text = "<%$ Resources:LocalizedResources, MobileNumber%>"
                                                                CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                        </th>
                                                        <th class="paddingLR" align="center">
                                                            <asp:Label ID="lblhoto" runat="server" Text="<%$ Resources:LocalizedResources, Photo  %>" />
                                                        </th>
                                                        <th align="center" width="125px">
                                                              <asp:Label ID="lblEdit" runat="server" Text="<%$ Resources:LocalizedResources, Edit %>" />
                                                        </th>
                                                        <th align="center" width="125px">
                                                              <asp:Label ID="lblDelete" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>" />
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="ClsBorderPager" id="trDataPager">
                                                        <td colspan="6">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwOtherStaff"
                                                                PageSize="20">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="MessageLabel" Text= "<%$ Resources:LocalizedResources, SelectPage%>" runat="server" CssClass="LblNrmlB" />
                                                                                           <span class="colonPadding">:</span>
                                                                                        <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                        </asp:DropDownList>
                                                                                    </td>
                                                                                    <td align="right" class="LblNormal">
                                                                                        <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        <asp:LinkButton ID="lnkbtnOtherName" runat="server" CommandName="Sort" CommandArgument="Name"  Text='<%# Eval("Name") %>'
                                                               Visible="false"  CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingLR" align="center" width="5%">
                                                        <asp:Image ID="imgPhotoUpload" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESTAFF"
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
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                        <asp:LinkButton ID="lnkbtnOtherName" runat="server" CommandName="Sort" CommandArgument="Name"  Text='<%# Eval("Name") %>'
                                                               Visible="false"  CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </td>
                                                    <td class="paddingL" align="left">
                                                        <asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("Designation") %>'></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%# Eval("MobileNo") %>'></asp:Label>
                                                    </td>
                                                    <td class="paddingLR" align="center" width="5%">
                                                        <asp:Image ID="imgPhotoUpload" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATESTAFF"
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
                                </tr>
                                <tr>
                                    <td align="center" >
                                        &nbsp;
                                        <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtn" BorderWidth="1px"
                                            CausesValidation="False" UseSubmitBehavior="false" />
                                            <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>" CssClass="ClsBtn" BorderWidth="1px" CausesValidation="false" UseSubmitBehavior="false" />
                                           
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ObjectDataSource TypeName="BusinessLogic.OtherStaffBL" EnablePaging="True" ID="ObjDSOtherStaff"
                                runat="server" SelectMethod="GetAll" SortParameterName="sortExpression" SelectCountMethod="CountTotalOtherStaff"
                                EnableCaching="False">
                                <SelectParameters>
                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />                                    
                                    <asp:Parameter Name="sortExpression" Type="String" />
                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                   <asp:ControlParameter Name="asFilter" ControlID="txtName" Type="String"  PropertyName="Text" />
                                      <%-- <asp:ControlParameter Name="asFilter" ControlID="hidFilter" Type="String"  />--%>
                                                           
                                    <asp:ControlParameter Name="asUserType" ControlID="ddlUserType" propertyname="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                             <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidMode" runat="server" Value="NEW" />
                            <asp:HiddenField ID="hidOtherStaffID" runat="server" />
                            <asp:HiddenField ID="hidUserID" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                            <asp:HiddenField ID="hidFilePath" runat="server" />
                            <asp:HiddenField ID="hidRetirementAge" runat="server" Value="0"/>
                            <asp:HiddenField ID="hidRetAge" runat="server" />
                            <asp:HiddenField ID="hidUserBasicDetails" runat="server" Value="N"/>
							<asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />

                            <asp:HiddenField ID = "hidInvalidFileFormat" runat = "server" />
                            <asp:HiddenField ID = "hidCultureInfo" runat = "server" />

                             <asp:HiddenField ID = "hidDateOfBirthFutureDate" runat = "server" />
                            <asp:HiddenField ID = "hidMobileDigit" runat = "server" />
                             <asp:HiddenField ID = "hidMobileNoVal" runat = "server" />
                             <asp:HiddenField ID = "hidAreYouSureYouWantToDeleteThisRecords" runat = "server" />

                             <asp:HiddenField ID = "hidAddressBlank" runat = "server" />
                             <asp:HiddenField ID = "hidvalLegthOfAddress" runat = "server" />
                             <asp:HiddenField ID = "hidvalAgeLength" runat = "server" />
                              <asp:HiddenField ID = "hidyears" runat = "server" />
                             <asp:HiddenField ID = "hidShouldBeLessThan" runat = "server" />
                             <asp:HiddenField ID = "hidbtnvalue" runat = "server" />

                             <asp:HiddenField ID = "hidvalConfirmPassword" runat = "server" />                     
                             <asp:HiddenField ID = "hidValPasswordLengh" runat = "server" />
                             <asp:HiddenField ID = "hidValForPassword" runat = "server" />
                             <asp:HiddenField ID = "hidValUserNameBlank" runat = "server" />
                             <asp:HiddenField ID = "hidvalUserNameLength" runat = "server" />
                             <asp:HiddenField ID = "hidNoteForPasswordCombination" runat = "server" />
                             <asp:HiddenField ID = "hidvalBlankConfirmPassword" runat = "server" />                             
                             <asp:HiddenField ID = "hidPassword" runat = "server" />
                             <asp:HiddenField ID= "hidUserRoleid" runat="server" />
                              <asp:HiddenField ID="hidQueryString" runat="server"/>                            
                            <asp:CustomValidator ID="cstBirthDate" runat="server" Display="none" EnableClientScript="true"
                                ClientValidationFunction="ValidateBirthDate" ErrorMessage= "<%$ Resources:LocalizedResources, DateOfBirthFutureDate%>"></asp:CustomValidator>
                     </td>
                    </tr>
                </table>
        </tr>
    </table>

    <script type="text/javascript" language="javascript">
        _clientcst_LblErrMsg = "<%=this.lblErrorMsg.ClientID %>"
        _clientcstbtnSave = "<%=this.btnSave.ClientID%>"
        _clientcstbtnCancel = "<%=this.btnCancel.ClientID%>"
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID%>"
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clienttxtDOB = "<%=this.txtDOB.ClientID %>"
        _clientcstBirthDate = "<%=this.cstBirthDate.ClientID%>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        _clientUploadPhoto = "<%=this.UploadPhoto.ClientID%>"
        _ClientCustPhoto = "<%=this.CustPhoto.ClientID %>"
        _ClienttxtAddress = "<%=this.txtAddress.ClientID %>"
        _clientcstValAddress = "<%=this.cstValAddress.ClientID %>"
        _clienthidMode = "<%=this.hidMode.ClientID %>"
        _clienthidUserBasicDetails = "<%=this.hidUserBasicDetails.ClientID %>"
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _clienthidRetirementAge = "<%=this.hidRetirementAge.ClientID %>" 
        _clientcal_DOB = "<%=this.txtDOB.ClientID %>";
        _clientcst_DOB = "<%=this.cst_DOB.ClientID %>";
        _clienthidRetAge = "<%=this.hidRetAge.ClientID %>"
        _clienttxtUserName = "<%=this.txtUserName.ClientID %>"
        _clienttxtConfirmPasswd = "<%=this.txtConfirmPasswd.ClientID %>"
        _clienttxtPasswdId = "<%=this.txtPasswd.ClientID %>";

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
            var RetConfigAge = $get(_clienthidRetAge).value;
            var yearDiff = thisYear - parseInt(DOBYear);

            if (parseInt(RetConfigAge) > 0) {
               if (sDate < RetDate) {

                   document.getElementById(_clientcst_DOB).errormessage = document.getElementById("<%=this.hidShouldBeLessThan.ClientID %>").value + RetConfigAge + document.getElementById("<%=this.hidyears.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            if (parseInt(yearDiff) < 18) {

                document.getElementById(_clientcst_DOB).errormessage = document.getElementById("<%=this.hidvalAgeLength.ClientID %>").value;
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
        function ConfirmDelete() {
              var bResult = true
              if (!window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteThisRecords.ClientID %>").value)) {
                bResult = false
            }
            return bResult
        }
        function ValidateBirthDate(source, args) {
            ResetUpdateLbl()
            var bIsValid = true
            if (document.getElementById(_clienttxtDOB).value != "") {
                var serverDate = document.getElementById(_clientServerDate).value
                dtStartDate = new Date(convertdate(document.getElementById(_clienttxtDOB).value))
                var today = new Date(serverDate)
                if (today < dtStartDate) {
                    document.getElementById(_clientcstBirthDate).errormessage =
document.getElementById("<%=this.hidDateOfBirthFutureDate.ClientID %>").value;
                    bIsValid = false
                }
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobileNo.ClientID %>"
        function MobileNumberValidation(oSrc, args) {

            ResetUpdateLbl()
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)
            document.getElementById(_clientcst_MobileNumber).errormessage = ""
            if (sMobileNumber.length > 0 && sMobileNumber.length < 10) {
                document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileDigit.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileNoVal.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function ResetUpdateLbl() {
            if (document.getElementById(_clientlblUpdateSucess) != null) {
                document.getElementById(_clientlblUpdateSucess).style.display = "none"
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

           function OpenWebcamPopup(sQueryString) {
           	window.open('../Common/WebcamPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=620,height=530');
           	return true;
        }


           function UpdateHiddenField() {
           	$get(_clienthidIsPhotoCaptured).value = "Y";
        }

        function UserNameValidation(oSrc, args) {            
            var sEmail = document.getElementById(_clienttxtUserName).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);
            if (sEmail.length == 0) {

                oSrc.errormessage = document.getElementById("<%=this.hidValUserNameBlank.ClientID %>").value;
                oSrc.errormessage = document.getElementById("<%=this.hidValUserNameBlank.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            if (sEmail.length < 6) {
                oSrc.errormessage = document.getElementById("<%=this.hidvalUserNameLength.ClientID %>").value;
                oSrc.errormessage = document.getElementById("<%=this.hidvalUserNameLength.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
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
                oSrc.errormessage = document.getElementById("<%=this.hidValForPassword.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (sPassword.length < 6) {
                oSrc.errormessage = document.getElementById("<%=this.hidValPasswordLengh.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            else if (!passed) {
                oSrc.errormessage = document.getElementById("<%=this.hidNoteForPasswordCombination.ClientID %>").value;
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

            if (sConfirmPassword.trim() == "") {               
                oSrc.errormessage = document.getElementById("<%=this.hidvalBlankConfirmPassword.ClientID %>").value;
                args.IsValid = false
                return true
            }
            else if (sPassword != sConfirmPassword) {
                oSrc.errormessage = document.getElementById("<%=this.hidvalConfirmPassword.ClientID %>").value;
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }

    </script>

</asp:Content>
