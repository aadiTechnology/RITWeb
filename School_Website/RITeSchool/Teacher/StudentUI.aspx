<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="StudentUI.aspx.cs"
    Inherits="StudentUI" MasterPageFile="../MasterPages/PopupMaster.master"  %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table id="tblTop" runat="server" style="width: 100%;" cellspacing="1" cellpadding="0"
        border="0">
        <tbody>
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table id="tblStudentInfo" runat="server" style="width: 95%;" border="0" cellpadding="0"
                        cellspacing="0">
                        <tbody>
                            <tr id="trPrecondition" runat="server" visible="false">
                                <td>
                                    <div runat="server" id="div1">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4" class="ClsBtmBorderGray" style="height: 57px">
                                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                        <tbody>
                                            <tr>
                                                <td style="height: 20px;" colspan="1">
                                                    <table class="ClsGrayMainTitle" style="padding-right: 5px" cellspacing="0" cellpadding="0"
                                                        border="0">
                                                        <tr>
                                                            <td style="height: 20px" class="MainTitleHead">
                                                                <asp:Label CssClass="MainTitleHead" ID="lblTitle" runat="server" EnableViewState="False"
                                                                    Font-Bold="true" Text="<%$ Resources:LocalizedResources,EnterStudentDetails%>"></asp:Label>
                                                                <span class="colonPadding">:</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="float: right; height: 10px" align="right">
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>  
                                    <asp:Label ID="lblStandard" Font-Bold="True" Text="" BorderWidth="0px" Font-Size="10"
                                                    runat="server"></asp:Label>    
                                    <div style="float:right">
                                    <asp:Label ID="lblConfirmedBy" Visible = "false" Font-Bold="True" Text="" BorderWidth="0px" Font-Size="10"
                                                    runat="server"></asp:Label> 
                                    </div>                                                                                     
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4" valign="top" height="10%">
                                    <div style="float: right; text-align: right; vertical-align: middle">
                                        <span class="ClsMdtStar">*</span>
                                        <asp:Label ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False"
                                            Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                    </div>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4" valign="top" height="10%">
                                    <asp:Panel ID="pnlErrorMsg" runat="server" Visible="true">
                                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr align="center">
                                <td colspan="3">
                                    <blink>
                                                <asp:Label ID="lblLeaveMessage" runat="server" CssClass="ErrHeadNew" EnableViewState="true" Visible="false"></asp:Label>
                                                </blink>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" style="width: 50%" valign="top">
                                    <table style="width: 100%;" border="0">
                                        <tr id="trNewAdd" runat="server" visible="false">
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblNewAdmission" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, NewAdmission%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkNewAddmission" runat="server" />
                                            </td>
                                        </tr>                                                                                
                                        <tr id="trRTE" runat="server">
                                            <td align="left" colspan="1" style="width: 25%;" class="ClsBorderlight">
                                                <asp:Label CssClass="LblNrmlB" Font-Bold="true" ID="lblRTE" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, IsRTEApplicable%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsRTEApplicable" runat="server" />
                                            </td>
                                        </tr>
                                         
                                        <tr id="trRTECatrgory" runat="server" style="visibility: hidden; display: none;">
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <span class="ClsLabel" id="lblRTECategory">RTE Category :</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:DropDownList ID="cmbRTECategory" runat="server" CssClass="MidCombo" Width="170px" >
                                                </asp:DropDownList>
                                                <span style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="cstcmbRTECategory" runat="server" Display="None" ErrorMessage=""
                                                    SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateRTECategory">
                                                </asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trRTEFormNo" runat= "server" style="visibility: hidden; display: none;">
                                        <td align="left" colspan="1" style="width: 25%;" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Label62" runat="server" EnableViewState="False"
                                                    Text="RTE Application Form No."></asp:Label>
                                                <span class="colonPadding">:</span></td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtRTEApplicationForm" runat="server" CssClass="MidTxtBox" MaxLength="20"></asp:TextBox>
                                                </td>
                                        </tr>

                                        <tr id="trAmount" runat= "server" style="visibility: hidden; display: none;">
                                        <td align="left" colspan="1" style="width: 25%;" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Label66" runat="server" EnableViewState="False"
                                                    Text="Annual Income"></asp:Label>
                                                <span class="colonPadding">:</span></td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtAnnualIncome" runat="server" CssClass="MidTxtBox" MaxLength="20" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                <asp:CustomValidator ID="CustomValidator7" runat="server" Display="None" ErrorMessage=""
                                                    SetFocusOnError="True" ValidateEmptyText="True" CssClass="ClsMdtStar" ClientValidationFunction="ValidateRTEIncome">
                                                </asp:CustomValidator>                                                    
                                                </td>
                                        </tr>
                                        <tr id="trRTENote" runat="server">
                                            <td align="left" class="ClsBorderlight " style="width: 20px; background-color: #ffffc4;">
                                                <asp:Label CssClass="LblNrmlB" Font-Bold="true" ID="lblNote" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Note%>"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 5px;">
                                                <span class="LblSmlGray" style="border-width: 0px">
                                                    <asp:Label ID="lblRTEStudent" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, RTEFeeStudent%>"></asp:Label></span>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span1" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, UserName%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:TextBox ID="txtLoginId" runat="server" MaxLength="20" CssClass="MidTxtBox" Enabled="false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" colspan="2">
                                                <asp:CheckBox ID="chkSendSMS" runat="server" Text="<%$ Resources:LocalizedResources, SendSMS%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblFormNumber" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, FormNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtFormNo" runat="server" CssClass="MidTxtBox" MaxLength="15" onchange="HideMessage()"></asp:TextBox>
                                                <span id="spnFormNo" runat="server" style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="reqFieldValidatorFormNo" runat="server" Display="None" ErrorMessage="Form Number should not be blank."
                                                    CssClass="ClsMdtStar" ClientValidationFunction="checkFormNo"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trStudentId" runat="server" visibile="false">
                                                <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="lblStudentID" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, StudentID%>"></asp:Label>                                               
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtStudentID" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                </td>
                                          </tr>
                                          <tr id="trGRNumber" runat="server" visibile="false">
                                           <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="lblGRNumber" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, GRNumber%>"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtGRNumber" runat="server" CssClass="MidTxtBox" MaxLength="30"></asp:TextBox>
                                                </td>
                                          </tr>
                                          <tr id="trPPRegNo" runat="server" visible="false">
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Label57" runat="server" EnableViewState="False"
                                                    Text="PP Registration Number"></asp:Label>                                                
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtPrePrimaryRegNo" runat="server" CssClass="MidTxtBox" MaxLength="20" ReadOnly="true" BackColor="lightGray"></asp:TextBox>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" style="width: 25%" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblRegNo" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, RegistrationNumber%>"></asp:Label>
                                                <a>
                                                    <img id="imgPrefixes" runat="server" width="20" height="20" src="../images/InfoIcon.jpg" /></a>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtRegNo" runat="server" CssClass="MidTxtBox" MaxLength="20" onchange="HideMessage()"></asp:TextBox>
                                                <span id="spnRegNo" runat="server" style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="reqFieldValidatorRegNo" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, RegNumberBlank%>"
                                                    CssClass="ClsMdtStar" ClientValidationFunction="checkRegNo"></asp:CustomValidator>
                                            </td>
                                        </tr>

                                        
                                       
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblAdmissionDate" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, AdmissionDate%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtcalAdmissionDate" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <rjs:PopCalendar ID="calAdmissionDate" runat="server" Control="txtcalAdmissionDate"
                                                    ShowErrorMessage="false" InvalidDateMessage="Please select valid admission date."
                                                    Format="dd MMM yyyy" ShowWeekend="True" />
                                                <span style="color: #ff0000">*</span>
                                                <asp:CustomValidator ID="cstAdmissionDate" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ControlToValidate="txtcalAdmissionDate" Visible="true" ErrorMessage="<%$ Resources:LocalizedResources, DateOfAdmission%>"
                                                    EnableClientScript="true" ClientValidationFunction="checkAdmissionDate"></asp:CustomValidator>
                                                <asp:RequiredFieldValidator ID="reqFieldValidateAdmissionDate" Display="None" runat="server"
                                                    ErrorMessage="<%$ Resources:LocalizedResources, ValAdmissionDate%>" CssClass="ClsMdtStar"
                                                    ControlToValidate="txtcalAdmissionDate"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="lbljoiningdate" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, JoiningDate%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtJoiningDate" runat="server" AutoPostBack="True" CssClass="SmlCombo"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                                    ID="calJoingDate" runat="server" Control="txtJoiningDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid joining date." Format="dd MMM yyyy" ShowWeekend="True"
                                                    RequiredDate="True" RequiredDateMessage="Joining date should not be blank." From-Control="txtcalAdmissionDate"
                                                    From-Date="" From-Message="Joining date should be greater than or equal to date of admission." />
                                                <asp:CustomValidator ID="custJoining" Display="None" runat="server" CssClass="ClsMdtStar"
                                                    ControlToValidate="txtJoiningDate" Visible="true" ErrorMessage="" EnableClientScript="true"
                                                    ClientValidationFunction="IsValidJoiningDate"></asp:CustomValidator>
                                                &nbsp;<span style="color: #ff0000">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Span2" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, StudentRollNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtRollNumber" CssClass="SmlTxtBox" runat="server" MaxLength="3"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                                <span id="spnRollMandatory" runat="server" style="color: #ff0000">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" rowspan="1">
                                                <span class="LblSmlGray floatR colonPadding">:</span>
                                                <asp:Label CssClass="LblSmlGray floatR" ID="Span3" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, FirstName%>"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="MidTxtBox" MaxLength="50"
                                                    onblur="formatName(this)"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center" colspan="2" rowspan="1" style="width: 50%" valign="top">
                                    <table style="width: 100%;" border="0">
                                        <tr>
                                            <td align="left" colspan="1" style="width: 25%;" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblPhoto" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources,Photo%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%" rowspan="6" valign="middle">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <div class="ClsBorderlight" style="vertical-align: middle">
                                                                <img id="imgPhoto" alt="image" runat="server" height="151" width="119" />
                                                            </div>
                                                        </td>
                                                        <td align="right" valign="top">
                                                            <asp:Button ID="btnRemovePhoto" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, RemovePhoto%>"
                                                                UseSubmitBehavior="false" Width="95px" OnClick="btnRemovePhoto_Click" Enabled="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span9" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Upload_CapturePhoto%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:FileUpload ID="FileUploadLogo" runat="server" />
                                                            <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateLogo"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="FileUploadLogo"
                                                                CssClass="LblErrorMsg"></asp:CustomValidator>
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
                                                    <asp:Label ID="lblUploadImage" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImage%>"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblUploadHeight" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, UploadImageHeight%>"></asp:Label><br />
                                                    <asp:Label ID="lblUploadSize" runat="server" EnableViewState="False" Text="(Image size should not exceed 1 mb. Supported file formats are JPG, JPEG)"></asp:Label></span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="2" style="width: 50%" valign="top">
                                    <table style="width: 100%;" border="0">
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" rowspan="1">
                                                <span class="LblSmlGray floatR colonPadding">:</span>
                                                <asp:Label CssClass="LblSmlGray floatR" ID="Span4" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, MiddleName%>"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" rowspan="1" >
                                                <span class="LblSmlGray floatR colonPadding">:</span>
                                                <asp:Label CssClass="LblSmlGray floatR" ID="Span5" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, LastName%>"></asp:Label>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtLastName" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span6" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, MotherName%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" id="tdMotherName" runat="server">
                                                <asp:TextBox ID="txtMotherName" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" >
                                                <asp:Label CssClass="ClsLabel" ID="lblParentName" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, ParentName%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtParentName" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="50"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>&nbsp; <span style="color: red"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" valign="top">
                                                <asp:Label CssClass="ClsLabel" ID="lblAddress" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Address%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:TextBox ID="txtAddress" runat="server" CssClass="MidTxtBox" TextMode="MultiLine"
                                                    Height="72px"></asp:TextBox>
                                                <span style="color: red">* </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblCity" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, City%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                <span style="color: red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblState" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, State%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtState" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" >
                                                <asp:Label CssClass="ClsLabel" ID="lblPIN" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Pin%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPIN" CssClass="MidTxtBox" runat="server" MaxLength="6" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    ondrop="event.returnValue=false" />
                                                <span style="color: red">* </span>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblPlaceOfBirth" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, PlaceOfBirth%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%" id="tdBirthPlace" runat="server">
                                                <asp:TextBox ID="txtBirthPlace" CssClass="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                <asp:Label ID="Label21" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Birth Taluka"></asp:Label>
                                                <span class="colonPadding">:</span>&nbsp;
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtAdditionalBirthTaluka" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1"  style="width: 25%">
                                                <asp:Label ID="Label22" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Birth District"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtAdditionalBirthDistrict" runat="server" CssClass="MidTxtBox"
                                                    onblur="formatName(this)" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="BirthState" runat="server">
                                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                <asp:Label ID="Label63" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Birth State"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtAdditionalBirthState" runat="server" CssClass="MidTxtBox"
                                                    onblur="formatName(this)" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                       <%-- <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Label1" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, ResidencePhoneNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtResPhoneNumber" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span15" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, OfficePhoneNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtOfficeNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Sapn16" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, NeighbourPhoneNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtNeighbourNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" >
                                                <asp:Label ID="Label19" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Religion"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtAdditionalReligion" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblCaste" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, CasteAndSubCaste%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" id="tdCastSubcast" runat="server">
                                                <asp:TextBox ID="txtCasteAndSubcaste" runat="server" CssClass="MidTxtBox" MaxLength="50" />
                                            </td>
                                        </tr>
                                           <tr>
                                            <td align="left"  class="ClsBorderLight" style="width: 25%">
                                                <span id="lblUploadCastCert" class="ClsLabel">Upload Cast Certificate:</span>
                                                <img id="img1" runat="server" width="15" height="15" src="../images/InfoIcon.jpg" title="(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)" />
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload ID="fuCastCertificate" CssClass="" runat="server" />
                                                <asp:ImageButton ID="imgbtnViewCasteCert" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                    ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" style="width: 16px" />
                                                  <asp:ImageButton ID="imgbtnDeleteCasteCert" runat="server" CausesValidation="false"
                                                                ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                Visible="false" EnableViewState="true" OnClick="imgbtnDeleteCasteCert_Click" />
                                              <asp:CustomValidator ID="cstValCasteCertFile" runat="server" ControlToValidate="fuCastCertificate" OnServerValidate="CasteCertFIle_ServerValidate"
                                                ClientValidationFunction="ValidateCastCertificate" Display="None" ValidateEmptyText="True" Enabled="true"></asp:CustomValidator>
                                                   <asp:HiddenField ID="hidCasteCertImage" runat="server" Value="" />
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblSubCaste" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Category%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:DropDownList ID="cmbCategory" runat="server" CssClass="MidCombo" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblUDISEnumber" runat="server" EnableViewState="False"
                                                    Text="Student UDISE number"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtUDISENumber" CssClass="MidTxtBox" runat="server" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblBoardRegNo" runat="server" EnableViewState="False"
                                                    Text="Board Registration Number"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtBoardRegNo" CssClass="MidTxtBox" runat="server" MaxLength="20" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="center" colspan="2" rowspan="1" style="width: 50%" valign="top">
                                    <%--  </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSaveNext"/>
                                            <asp:PostBackTrigger ControlID="btnSave"/>
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                    <table style="width: 100%;" border="0">
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblBirthDate" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtCalDobPopup" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                <rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtCalDobPopup" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                    To-Today="true" />
                                              <asp:ImageButton ID="btnView2" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                    ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" Height="17px" Width="16px" />
                                                <span style="color: #ff0000">&nbsp; *</span><br />
                                                <asp:Label ID="lblAge" runat="server" CssClass="LblI" Style="font-weight: bold;"></asp:Label>
                                            </td>
                                        </tr>
                                       <%-- <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblPlaceOfBirth" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, PlaceOfBirth%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%" id="tdBirthPlace" runat="server">
                                                <asp:TextBox ID="txtBirthPlace" CssClass="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                <asp:Label ID="Label21" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Birth Taluka"></asp:Label>
                                                <span class="colonPadding">:</span>&nbsp;
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtAdditionalBirthTaluka" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                    MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                <asp:Label ID="Label22" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                    Text="Birth District"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:TextBox ID="txtAdditionalBirthDistrict" runat="server" CssClass="MidTxtBox"
                                                    onblur="formatName(this)" MaxLength="200"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="Span20" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Nationality%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%" id="tdNationality" runat="server">
                                                <asp:TextBox ID="txtNationality" CssClass="MidTxtBox" runat="server" MaxLength="50"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span17" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, MotherTongue%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" id="tdMotherTongue" runat="server">
                                                <asp:TextBox ID="txtMotherTongue" runat="server" CssClass="MidTxtBox" MaxLength="20"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblGender" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Gender%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:RadioButton ID="rdoMale" Text="Male" runat="server" GroupName="rdoGroupSex"
                                                    CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                                                <asp:RadioButton ID="rdoFemale" Text="Female" runat="server" GroupName="rdoGroupSex"
                                                    CssClass="ClsLabel clsLabel"></asp:RadioButton>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="lblBlGp" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, BloodGroup%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:DropDownList ID="cmbBloodGroup" runat="server" CssClass="MidCombo" Width="150px">
                                                    <asp:ListItem Value="0">-- Select --</asp:ListItem>
                                                    <asp:ListItem>O+</asp:ListItem>
                                                    <asp:ListItem Value="A+">A+</asp:ListItem>
                                                    <asp:ListItem>B+</asp:ListItem>
                                                    <asp:ListItem>AB+</asp:ListItem>
                                                    <asp:ListItem>O-</asp:ListItem>
                                                    <asp:ListItem>A-</asp:ListItem>
                                                    <asp:ListItem>B-</asp:ListItem>
                                                    <asp:ListItem>AB-</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                <asp:Label CssClass="ClsLabel" ID="Label2" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, ParentOccupation%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:DropDownList ID="cmbOcupation" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                    Width="150px" OnSelectedIndexChanged="cmbOcupation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                                <span style="color: red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <table align="left" id="trOtherOccupation" runat="server" visible="false" width="100%">
                                                            <tr>
                                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%; height: 22px;">
                                                                    <asp:Label CssClass="ClsLabel" ID="lblOther" runat="server" EnableViewState="False"
                                                                        Text="<%$ Resources:LocalizedResources, OtherOccupation%>"></asp:Label>
                                                                    <span class="colonPadding clsLabel">:</span>
                                                                </td>
                                                                <td align="left" colspan="1" style="width: 25%; height: 22px;">
                                                                    <asp:TextBox ID="txtOtherOccupation" runat="server" CssClass="MidTxtBox" MaxLength="25"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="cmbOcupation" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Label1" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, ResidencePhoneNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:TextBox ID="txtResPhoneNumber" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="Span15" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, OfficePhoneNumber%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtOfficeNo" CssClass="MidTxtBox" runat="server" MaxLength="15"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                      <%--  <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblCity" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, City%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtCity" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                <span style="color: red">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblState" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, State%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtState" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                <span style="color: #ff0000">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblPIN" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Pin%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPIN" CssClass="MidTxtBox" runat="server" MaxLength="6" onblur="extractNumber(this,0,false);"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    ondrop="event.returnValue=false" />
                                                <span style="color: red">* </span>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblMobilePhoneNo" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, MobileNumber1%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtMobilePhoneNumber" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                    ondrop="event.returnValue=false" />
                                                <span style="color: red; display: none" id="spnMobileNo">*</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblMobilePhoneNo2" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, MobileNumber2 %>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtMobilePhoneNumber2" CssClass="MidTxtBox" runat="server" MaxLength="10"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                    ondrop="event.returnValue=false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblEmail" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, Email %>"></asp:Label>
                                                <span class="clsLabel colonPadding">:</span>
                                                <asp:CustomValidator ID="cstValEmail" runat="server" ClientValidationFunction="EmailValidation"
                                                    ControlToValidate="txtEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtEmail" CssClass="MidTxtBox" runat="server" MaxLength="50" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblAadharCard" runat="server" EnableViewState="False"
                                                    Text="Aadhar Card Number"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtAadharCardNo" CssClass="MidTxtBox" runat="server" MaxLength="12"
                                                    onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    onpaste="event.returnValue=false" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight">
                                                <asp:Label CssClass="ClsLabel" ID="lblAadharCardName" runat="server" EnableViewState="False"
                                                     Text="Name on Aadhar Card"></asp:Label>
                                                <span class="ClsLabel colonPadding">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtNameOnAadharCard" CssClass="MidTxtBox" runat="server" 
                                                    MaxLength="150"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight">
                                                <span id="lblUpload" class="ClsLabel">Upload Scan Copy of Aadhar Card :</span>
                                            </td>
                                            <td align="left">
                                                <asp:FileUpload Width="170px" ID="fuAadharNumber" CssClass="" runat="server" />
                                                <asp:ImageButton ID="btnView" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                    ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" style="width: 16px" />
                                                <asp:CustomValidator ID="cstValidateAadharScanCopy" Display="None" runat="server"
                                                    ClientValidationFunction="ValidateAadharScanCopy" ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>"
                                                    ControlToValidate="fuAadharNumber" CssClass="LblErrorMsg"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" class="ClsBorderlight">
                                                <span class="LblSmlGray">(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File
                                                    size should not exceed 3MB.)</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderLight">
                                                <span id="lblFamilyPhoto" class="ClsLabel">Family Photo :</span>
                                            </td>
                                            <td align="left">
                                                <asp:UpdatePanel ID="UpdateFamilyPhoto" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:FileUpload Width="170px" ID="FuFamilyPhoto" CssClass="" runat="server" />
                                                        <asp:ImageButton ID="btnView1" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                        <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" ToolTip="Delete"
                                                            ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                            Visible="false" OnClick="imgbtnDelete_Click" EnableViewState="true" />
                                                        <asp:CustomValidator ID="cstValidateFamilyPhoto" Display="None" runat="server" ClientValidationFunction="ValidateFamilyPhoto"
                                                            ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="FuFamilyPhoto"
                                                            CssClass="LblErrorMsg"></asp:CustomValidator>
                                                        <asp:HiddenField ID="hidFamilyImage" runat="server" Value="" />
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="imgbtnDelete" EventName="Click" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" class="ClsBorderlight">
                                                <span class="LblSmlGray">(Supports only .JPG, .PNG, .BMP, .JPEG file type. File size
                                                    should not exceed 3MB.)</span>
                                            </td>
                                        </tr>
                                        <%-------------------------------------------------------------------------%>
                                    </table>
                                    <%--  </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnSaveNext"/>
                                            <asp:PostBackTrigger ControlID="btnSave"/>
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                </td>
                            </tr>                           
                            <tr id="trCheckStaffKid1" runat="server">
                                <td align="center" colspan="2" style="width: 50%" valign="top">
                                    <table style="width: 100%;" border="0">
                                        
                                        <tr id="trCheckStaffKid" runat="server">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Span7" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, IsStaffKid%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsStaffKid" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22%" rowspan="1">
                                                <asp:Label ID="lblUserRole" CssClass="ClsLabel" runat="server" EnableViewState="False"
                                                     Text="Staff User Role"></asp:Label>
                                                 <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="MidCombo" 
                                                    Width="150px" AutoPostBack="True" 
                                                    onselectedindexchanged="ddlUserRole_SelectedIndexChanged"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="Width: 22%">
                                                <asp:Label ID="lblUserName" CssClass="ClsLabel" runat="server" EnableViewState="false"
                                                    Text="Staff Name"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:UpdatePanel ID="upnl21" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>                                                
                                                    <asp:DropDownList ID="ddlUserName" runat="server" CssClass="MidCombo" Width="150px"></asp:DropDownList>
                                                    <asp:CustomValidator ID="CustomValidator6" runat="server" ClientValidationFunction="ValidateStaffName"
                                                    Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlUserRole" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                        <tr id="trAdmissionFor" runat="server" visible="false">
                                               <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                    <asp:Label CssClass="ClsLabel" ID="Label54" runat="server" EnableViewState="False"
                                                        Text="Admission For"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                               <td  align="left" colspan="3">
                                                   <asp:DropDownList ID="cmbAdmissionFor" runat="server" CssClass="MidCombo" Width="150px">
                                                    </asp:DropDownList>                      
                                                </td>            
                                        </tr>
                                        <tr id="trFeeCategory" runat="server" visible="false">
                                               <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                    <asp:Label CssClass="ClsLabel" ID="Label56" runat="server" EnableViewState="False"
                                                        Text="Fee Category"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                               <td  align="left" colspan="3">
                                                   <asp:DropDownList ID="cmbFeeCategory" runat="server" CssClass="MidCombo" Width="150px">
                                                    </asp:DropDownList>   
                                                    <span style="color: #ff0000">*</span>&nbsp; <span style="color: red"></span>
                                                    <asp:CustomValidator ID="cstValidateFeeCategory" runat="server" ClientValidationFunction="ValidateFeeCategory"
                                                    ControlToValidate="cmbFeeCategory" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                </td>            
                                        </tr>
                                        <tr id="trStudentSaralDetails" runat="server" visible="true">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                    <asp:Label CssClass="ClsLabel" ID="lblsaralNo" runat="server" EnableViewState="False"
                                                        Text="Saral No"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtSaralNo" CssClass="MidTxtBox" runat="server" MaxLength="20" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                    <asp:Label CssClass="ClsLabel" ID="Label67" runat="server" EnableViewState="False"
                                                        Text="PEN Number"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPenNo" CssClass="MidTxtBox" runat="server" MaxLength="50" onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    />
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server" visible="true">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 150px" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Label58" runat="server" EnableViewState="False"
                                                    Text="Is Only Child?"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsOnlyChild" runat="server"/>
                                            </td>
                                             
                                        </tr>
                                         
                                    </table>
                                </td>
                                <td align="center" colspan="2" rowspan="1" style="width: 50%" valign="top">
                                    <table style="width: 100%;" border="0">      
                                        <tr id="trApplicableRule" runat="server">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                <asp:Label CssClass="ClsLabel" ID="lblApplicable" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, ApplicableRule%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:DropDownList ID="ddlFeeRule" runat="server" CssClass="MidCombo" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>      
                                         <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                <asp:Label CssClass="ClsLabel" ID="Span11" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, SecondLanguage%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:DropDownList ID="ddlSecondLanguage" runat="server" CssClass="MidCombo" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                <asp:Label CssClass="ClsLabel" ID="Label3" runat="server" EnableViewState="False"
                                                    Text="Third Language"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1" style="width: 25%">
                                                <asp:DropDownList ID="cmbThirdLanguage" runat="server" CssClass="MidCombo" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>                            
                                        <tr id="trHasSibling" runat="server" visible="false">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 150px" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Span14" runat="server" EnableViewState="False"
                                                    Text="<%$ Resources:LocalizedResources, HasSibling%>"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkHasSibling" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trRiseAndShine" runat="server">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 150px" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Label43" runat="server" EnableViewState="False"
                                                    Text="Is Rise & Shine?"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkRiseAndShine" runat="server" />
                                            </td>
                                        </tr>
                                         <tr id="trIsForDayBoarding" runat="server" visible="false">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 150px" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Label55" runat="server" EnableViewState="False"
                                                    Text="Is For Day Boarding?"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsDayBoarding" runat="server"/>
                                            </td>
                                        </tr>
                                        
                                        <tr id="tr4" runat="server" visible="true">
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 150px" rowspan="1">
                                                <asp:Label CssClass="ClsLabel" ID="Label59" runat="server" EnableViewState="False"
                                                    Text="Is Minority?"></asp:Label>
                                                <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left" colspan="1">
                                                <asp:CheckBox ID="chkIsMinority" runat="server"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="1" class="ClsBorderlight" style="width: 22.5%">
                                                    <asp:Label CssClass="ClsLabel" ID="lblApaarId" runat="server" EnableViewState="False"
                                                        Text="APAAR ID"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtApaarId" CssClass="MidTxtBox" runat="server" MaxLength="12" onkeyup="extractNumber(this, 0,false);" onkeypress="return blockNonNumbers (this, event, true, false);"/>
                                            </td>
                                     </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: white; width: 880px" id="Td2" colspan="4" align="center">
                                    <%-------------------------------------------------------------------------%>
                                    <cc1:CollapsablePanel ID="colpnlAdditionalInfo" runat="server" TitleText="Additional Information"
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                        <table id="tblAdditionalInformation" width="100%" runat="server">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </cc1:CollapsablePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: white; width: 880px" id="Td1" colspan="4" align="center">
                                    <cc1:CollapsablePanel ID="colpnlPhotoGallery" runat="server" TitleText="<%$ Resources:LocalizedResources, AdmissionDocumentInformation %>"
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                        <table width="100%">
                                            <tr align="left">
                                                <td style="text-align: left;">
                                                    <table id="LegendTable" runat="server" visible="false" align="left">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label CssClass="ClsLblLgnd" ID="lblLegend" runat="server" EnableViewState="False"
                                                                    Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                            </td>
                                                            <td align="left" style="padding-right: 3px">
                                                                <asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                                                    BackColor="#ffffcc" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:Label CssClass="ClsTextNormal" ID="lblDeactivatedUser" Font-Bold="true" runat="server"
                                                                    EnableViewState="False" Text="Mandatory Documents."></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="ClsBorderLight">
                                                    <asp:ListView ID="lstvwConfiguredDocument" runat="server" DataKeyNames="StudentDocumentId, StandardwiseDocumentId,IsSubmitted,IsApplicable,IsSubmissionMandatory"
                                                        OnItemDataBound="lstvwConfiguredDocument_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table align="center" width="100%" runat="server" id="tblTermInfo" style="color: #333333"
                                                                cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" style="padding-left: 10px; width: 126px">
                                                                        <asp:CheckBox ID="chkIsApplicableAll" runat="server" Text="<%$ Resources:LocalizedResources, IsApplicable%>"
                                                                            onclick="CheckAllUncheckAllsIsApplicableCheckBox()" CssClass="vertical-align-top all-checkbox" />
                                                                    </th>
                                                                    <th align="center" style="padding-left: 10px; width: 126px">
                                                                        <asp:CheckBox ID="ChkSelectAll" runat="server" Text="<%$ Resources:LocalizedResources, IsSubmitted%>"
                                                                            onclick="CheckAllUncheckAlls()" CssClass="vertical-align-top all-checkbox" />
                                                                    </th>
                                                                    <th align="left" style="padding-left: 12px">
                                                                        <asp:Label ID="lblDocument" runat="server" Text="<%$ Resources:LocalizedResources, DocumentName %>" />
                                                                    </th>
                                                                    <th align="center" style="padding-left: 10px; width: 120px">
                                                                        <asp:Label ID="lblAttachment" runat="server" Text="<%$ Resources:LocalizedResources, AttachmentCount %>" />
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trData" runat="server" class="ClsGridRow">
                                                                <td align="center" id="tdIsApplicable" runat="server" style="padding-left: 8px">
                                                                    <asp:CheckBox ID="chkIsApplicable" runat="server" />
                                                                    <asp:HiddenField ID="hidIsDocMandatory" runat="server" Value="0" />
                                                                </td>
                                                                <td align="center" id="tdSelect" runat="server" style="padding-left: 8px">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="left" id="tdDocumentName" runat="server" style="padding-left: 8px">
                                                                    <asp:Label ID="lblDocumentName" CssClass="LblNormal" runat="server" Text='<%#Eval("DocumentName")%>'></asp:Label>
                                                                </td>
                                                                <td id="tdlnkAttachment" runat="server" align="center">
                                                                    <asp:LinkButton ID="lnkAttachment" runat="server" Text='<%#Eval("DocumentCount") %>'
                                                                        CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trData" runat="server" class="ClsGridAltRow">
                                                                <td align="center" id="tdIsApplicable" runat="server" style="padding-left: 8px">
                                                                    <asp:CheckBox ID="chkIsApplicable" runat="server" />
                                                                    <asp:HiddenField ID="hidIsDocMandatory" runat="server" Value="0" />
                                                                </td>
                                                                <td align="center" id="tdSelect" runat="server" style="padding-left: 8px">
                                                                    <asp:CheckBox ID="ChkSelect" runat="server" />
                                                                </td>
                                                                <td align="left" id="tdDocumentName" runat="server" style="padding-left: 8px">
                                                                    <asp:Label ID="lblDocumentName" CssClass="LblNormal" runat="server" Text='<%#Eval("DocumentName")%>'></asp:Label>
                                                                </td>
                                                                <td id="tdlnkAttachment" runat="server" align="center">
                                                                    <asp:LinkButton ID="lnkAttachment" runat="server" Text='<%#Eval("DocumentCount") %>'
                                                                        CausesValidation="false" ToolTip="Click to upload / delete attachment."></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table style="width: 100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
                                                                        <asp:Label ID="lblNoDocument" runat="server" Text="<%$ Resources:LocalizedResources, NoDocument %>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EmptyDataTemplate>
                                                    </asp:ListView>
                                                </td>
                                            </tr>
                                        </table>
                                    </cc1:CollapsablePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: white; width: 880px" id="Td3" colspan="4" align="center">
                                    <cc1:CollapsablePanel ID="colpnlLastSchoolDetails" runat="server" TitleText="<%$ Resources:LocalizedResources,LastSchoolDetails %>"
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="lblSchool" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, SchoolName%>"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="3" id="tdLastSchoolName" runat="server">
                                                    <asp:TextBox ID="txtLastSchoolName" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="Label7" runat="server" EnableViewState="False"
                                                        Text="School Address"></asp:Label>
                                                    <span class="colonPadding clsLabel">:</span>
                                                </td>
                                                <td align="left" colspan="3" id="td5" runat="server">
                                                    <asp:TextBox ID="txtLastSchoolAddress" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="lblStand" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, standard%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtLastStandard" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="50"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:Label CssClass="ClsLabel" ID="Label10" runat="server" EnableViewState="False"
                                                        Text="School UDISE No."></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtLastUDISENo" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label CssClass="ClsLabel" ID="lblBoard" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, SchoolBoardName%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:RadioButtonList ID="rdolstlastSchoolBoard" runat="server" RepeatColumns="2"
                                                        CssClass="ClsLabel">
                                                        <asp:ListItem Text="ICSE" Value="ICSE" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="CBSE" Value="CBSE" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="SSC" Value="SSC" Selected="False"></asp:ListItem>
                                                        <asp:ListItem Text="OTHERS" Value="OTHERS" Selected="False"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label CssClass="ClsLabel" ID="lblRecognised" runat="server" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, IsRecognised%>"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:RadioButton ID="rdobtnRecognisedYes" Text="Yes" runat="server" GroupName="rdoGroupRecognised"
                                                        CssClass="ClsLabel" Checked="True"></asp:RadioButton>
                                                    <asp:RadioButton ID="rdobtnRecognisedNo" Text="No" runat="server" GroupName="rdoGroupRecognised"
                                                        CssClass="ClsLabel"></asp:RadioButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </cc1:CollapsablePanel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr runat="server">
                                <td style="background-color: white; width: 880px" id="Td4" colspan="4" align="center">
                                    <cc1:CollapsablePanel ID="colpnlStudentAdditionalDetails" runat="server" TitleText="Additional Details"
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
                                        <% if (SchoolBase.Settings.IsAdditionalFieldsApplicable)
                                           {%>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" style="width: 25%; text-decoration: underline;">
                                                    Educational Details
                                                </td>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Admission Academic Year"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalAdmissionAcademicYear" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label8" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Admission Standard"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalAdmissionStandard" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label ID="Label13" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Previous Marks Obtained"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalPreviousMarksObtained" runat="server" MaxLength="3"
                                                        onblur="extractNumber(this,0,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" Width="20%"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label ID="Label15" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Previous Marks Out Off"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalPreviousMarksOutOff" runat="server" MaxLength="3" onblur="extractNumber(this,0,false);"
                                                        ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" Width="20%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label9" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Subject Names"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalSubjectNames" runat="server" CssClass="MidTxtBox" Height="40px"
                                                        MaxLength="200" onblur="formatName(this)" TextMode="MultiLine" Width="92%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Previous Year of Passing"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalPreviousYearOfPassing" runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label CssClass="ClsLabel" ID="Label11" runat="server" EnableViewState="False"
                                                        Text="Current Academic Year"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalCurrAcaYear" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label CssClass="ClsLabel" ID="Label12" runat="server" EnableViewState="False"
                                                        Text="Current Standard"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalCurrStandard" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 25%; text-decoration: underline;">
                                                    Other Details
                                                </td>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="width: 25%">
                                                    <asp:Label ID="Label20" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Is Handicapped"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:CheckBox ID="chkAdditionalIsHandicapped" runat="server" />
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label23" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="House No. / Plot No."></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalHouseNoPlotNo" runat="server" CssClass="MidTxtBox"
                                                        onblur="formatName(this)" MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label24" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Main Area"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalMainArea" runat="server" CssClass="MidTxtBox" onblur="formatName(this)"
                                                        MaxLength="200" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label25" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Subarea Name"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalSubareaName" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label26" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Landmark"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalLandMark" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label27" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Taluka"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalTaluka" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label28" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="District"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalDistrict" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td id="tdlblFeeArea" visible="false" runat="server" align="left" class="ClsBorderlight"
                                                    colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label29" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Fee Area Name"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td id="tdCmbFeeArea" visible="false" runat="server" align="left" colspan="1">
                                                    <asp:DropDownList ID="cmbAdditionalFeeAreaName" runat="server" CssClass="MidCombo"
                                                        Width="170px">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <%--<asp:Label ID="Label30" runat="server" CssClass="ClsLabel" 
                                                          EnableViewState="False" Text="Father Occupation"></asp:Label>
                                                      <span class="colonPadding">:</span>&nbsp;&nbsp;&nbsp;</td>--%>
                                                    <asp:Label ID="Label31" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Qualification"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    <%-- <asp:TextBox ID="txtAdditionalFatherOccupation" runat="server" 
                                                          CssClass="MidTxtBox" MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>--%>
                                                    <asp:TextBox ID="txtAdditionalFatherQualification" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label32" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father E-mail"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                    </td>
                                                    <td align="left" colspan="1" style="width: 25%">
                                                        <asp:TextBox ID="txtAdditionalFatherEmail" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                            onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                        <asp:CustomValidator ID="cstAdditionaltxtFatherEmail" runat="server" ClientValidationFunction="EmailValidationAdditionalFatherEmail"
                                                            ControlToValidate="txtAdditionalFatherEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                    </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label33" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Office Name"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalFatherOfficeName" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label34" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Office Address"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalFatherOfficeAddress" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label16" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Designation"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalFatherDesignation" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label14" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father DOB"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalFatherDOB" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                    <rjs:PopCalendar ID="CalFatherDOB" runat="server" Control="txtAdditionalFatherDOB"
                                                        Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                        To-Today="true" />
                                                    <asp:CustomValidator ID="cstValidateFatherDOB" Display="None" runat="server" ClientValidationFunction="ValidateFatherDOB"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="txtAdditionalFatherDOB"
                                                        CssClass="LblErrorMsg"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label17" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Photo"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:FileUpload Width="170px" ID="FUAdditionalFatherPhoto" CssClass="" runat="server" />
                                                            <asp:ImageButton ID="imgViewFatherPhoto" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                                ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                            <asp:ImageButton ID="imgDeleteFatherPhoto" runat="server" CausesValidation="false"
                                                                ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                Visible="false" EnableViewState="true" OnClick="imgDeleteFatherPhoto_Click" />
                                                            <asp:CustomValidator ID="cstValidateFatherPhoto" Display="None" runat="server" ClientValidationFunction="ValidateFatherPhoto"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="FUAdditionalFatherPhoto"
                                                                CssClass="LblErrorMsg"></asp:CustomValidator>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="imgDeleteFatherPhoto" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label44" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Weight (Kg)"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtFatherWeight" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="2" class="ClsBorderlight">
                                                    <span id="spFatherAdditional" runat="server" class="LblSmlGray"></span>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label45" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Height (Cm)"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtFatherHeight" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label46" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Blood Group"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtFatherBloodGroup" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label47" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Aadhar Card Number"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtFatherAdharcardNo" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label60" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Father Annual Income"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtFAnnualIncome" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                       Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                              <td align="left" class="ClsBorderLight">
                                                <span id="lblFatherAadharCard" class="ClsLabel">Upload Father's Aadhar Card :</span>
                                                <img id="img2" runat="server" width="15" height="15" src="../images/InfoIcon.jpg" title="(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)" />
                                              </td>
                                              <td align="left" colspan="3">
                                                <asp:FileUpload Width="170px" ID="flUploadFatherAaadhar" CssClass="" runat="server" />
                                                <asp:ImageButton ID="imgViewFatherAadharCard" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                         ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                   <asp:ImageButton ID="imgDeleteFatherAadharCard" runat="server" CausesValidation="false"
                                                                ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                Visible="false" EnableViewState="true" OnClick="imgDeleteFatherAadharCard_Click" />
                                                <asp:CustomValidator ID="cstValFatherAadharFile" runat="server" ControlToValidate="flUploadFatherAaadhar" Enabled="true" OnServerValidate="FatherAadharFile_ServerValidate"
                                                        ClientValidationFunction="ValidateFatherAadharFile" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                    <asp:HiddenField ID="hidFatherAadharCardFileName" runat="server" Value="" />
                                              </td>
                                           </tr>                                          
                                            <tr>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label35" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Occupation"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalMotherOccupation" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Qualification"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalMotherQualification" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label36" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother E-Mail"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalMotherEmail" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                    <asp:CustomValidator ID="cstAdditionalMotherEmail" runat="server" ClientValidationFunction="EmailValidationAdditionalMotherEmail"
                                                        ControlToValidate="txtAdditionalMotherEmail" Display="None" ValidateEmptyText="True"></asp:CustomValidator>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label37" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Office Name"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalMotherOfficeName" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label38" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Office Address"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalMotherOfficeAddress" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label18" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Designation"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtAdditionalMotherDesignation" runat="server" CssClass="MidTxtBox"
                                                        MaxLength="200" onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label30" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother DOB"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalMotherDOB" CssClass="SmlCombo" runat="server" AutoPostBack="True"></asp:TextBox>
                                                    <rjs:PopCalendar ID="CalMotherDOB" runat="server" Control="txtAdditionalMotherDOB"
                                                        Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                        To-Today="true" />
                                                    <asp:CustomValidator ID="cstValidateMotherDOB" Display="None" runat="server" ClientValidationFunction="ValidateMotherDOB"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="txtAdditionalMotherDOB"
                                                        CssClass="LblErrorMsg"></asp:CustomValidator>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label39" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Photo"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:FileUpload Width="170px" ID="fuAdditionalMotherPhoto" CssClass="" runat="server" />
                                                            <asp:ImageButton ID="imgViewMotherPhoto" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                                ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                            <asp:ImageButton ID="imgDeleteMotherPhoto" runat="server" CausesValidation="false"
                                                                ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                Visible="false" EnableViewState="true" OnClick="imgDeleteMotherPhoto_Click" />
                                                            <asp:CustomValidator ID="cstValidateMotherPhoto" Display="None" runat="server" ClientValidationFunction="ValidateMotherPhoto"
                                                                ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="fuAdditionalMotherPhoto"
                                                                CssClass="LblErrorMsg"></asp:CustomValidator>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="imgDeleteMotherPhoto" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" colspan="2" class="ClsBorderlight">
                                                    <span id="spMotherAdditional" runat="server" class="LblSmlGray"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label48" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Weight (Kg)"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtMotherWeight" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label49" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Height (Cm)"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtMotherHeight" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label50" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Aadhar Card Number"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtMotherAadharCardNo" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label51" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Blood Group"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtMotherBloodGroup" runat="server" CssClass="MidTxtBox" MaxLength="10"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                             <tr> 
                                                <td align="left" class="ClsBorderLight">
                                                    <span id="lblMotherAadharCard" class="ClsLabel">Upload Mother's Aadhar Card :</span>
                                                    <img id="img3" runat="server" width="15" height="15" src="../images/InfoIcon.jpg" title="(Supports only .PDF, .JPG, .PNG, .BMP, .JPEG file type. File size should not exceed 3MB.)" />
                                                </td>
                                                <td align="left" colspan="3">
                                                     <asp:FileUpload ID="flUploadMotherAaadhar" runat="server" ViewStateMode="Enabled" />
                                                        <asp:ImageButton ID="imgViewMotherAadharCard" runat="server" ViewStateMode="Enabled" CausesValidation="false"
                                                            ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif" Visible="false" />
                                                       <asp:ImageButton ID="imgDeleteMotherAadharCard" runat="server" CausesValidation="false"
                                                                    ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                    Visible="false" EnableViewState="true" OnClick="imgDeleteMotherAadharCard_Click" />
                                                        <asp:CustomValidator ID="cstValMotherAadharFile" runat="server" ControlToValidate="flUploadMotherAaadhar" OnServerValidate="MotherAadharFile_ServerValidate"
                                                            ClientValidationFunction="ValidateMotherAadharFile" Display="None" ValidateEmptyText="True" Enabled="true"></asp:CustomValidator>
                                                        <asp:HiddenField ID="hidMotherAadharCardFileName" runat="server" Value="" />
                                                    </td>
                                            </tr>                                  
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label61" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Mother Annual Income"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtMAnnualIncome" runat="server" CssClass="MidTxtBox" MaxLength="10"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" style="height: 5px;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label40" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Marriage Anniversary Date"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtAdditionalAnniversaryDate" CssClass="SmlCombo" runat="server"
                                                        AutoPostBack="True"></asp:TextBox>
                                                    <rjs:PopCalendar ID="CalAnniversaryDate" runat="server" Control="txtAdditionalAnniversaryDate"
                                                        Format="dd MMM yyyy" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth."
                                                        To-Today="true" />
                                                    <asp:CustomValidator ID="cstAnniversaryDate" Display="None" runat="server" ClientValidationFunction="ValidateAnniversaryDate"
                                                        ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>" ControlToValidate="txtAdditionalAnniversaryDate"
                                                        CssClass="LblErrorMsg"></asp:CustomValidator>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label41" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Local Guardian Photo"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:FileUpload Width="170px" ID="FUAdditionalGuardianPhoto" CssClass="" runat="server" />
                                                            <asp:ImageButton ID="imgViewGuardianPhoto" runat="server" ViewStateMode="Enabled"
                                                                CausesValidation="false" ToolTip="View" ImageUrl="../images/iconGridSml_ViewGE.gif"
                                                                Visible="false" />
                                                            <asp:ImageButton ID="imgDeleteGuardianPhoto" runat="server" CausesValidation="false"
                                                                ToolTip="Delete" ImageUrl="../images/IconGrid_Delete.GIF" OnClientClick="return ConfirmDelete()"
                                                                Visible="false" EnableViewState="true" OnClick="imgDeleteGuardianPhoto_Click" />
                                                            <asp:CustomValidator ID="cstValidateGuardianPhoto" Display="None" runat="server"
                                                                ClientValidationFunction="ValidateGuardianPhoto" ErrorMessage="<%$ Resources:LocalizedResources, InvalidFileFormat%>"
                                                                ControlToValidate="FUAdditionalGuardianPhoto" CssClass="LblErrorMsg"></asp:CustomValidator>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="imgDeleteGuardianPhoto" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <td align="left" colspan="2" class="ClsBorderlight">
                                                    <span id="spParentAdditional" runat="server" class="LblSmlGray"></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label52" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Family Monthly Income"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:TextBox ID="txtMonthlyIncome" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label53" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="CWSN"></asp:Label>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:TextBox ID="txtCWSN" runat="server" CssClass="MidTxtBox" MaxLength="200" onblur="formatName(this)"
                                                        Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>                                          
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label42" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Relative Full Name"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left"  colspan="1">
                                                    <asp:TextBox ID="txtRelativeName" runat="server" CssClass="MidTxtBox" MaxLength="200"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                                 <td id="td14" runat="server" align="left" class="ClsBorderlight"
                                                    colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblResidenceType" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Residence Type"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td id="td15"  runat="server" align="left" colspan="1">
                                                    <asp:DropDownList ID="cmbResidenceType" runat="server" CssClass="MidCombo"
                                                        Width="170px">
                                                    </asp:DropDownList>
                                                </td>
                                              
                                            </tr>
                                              <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="Label65" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="RFID"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;
                                                </td>
                                                <td align="left"  colspan="1">
                                                    <asp:TextBox ID="txtRFID" runat="server" CssClass="MidTxtBox" MaxLength="50"
                                                        onblur="formatName(this)" Width="89%"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <b>
                                                        <asp:Label ID="Label64" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                            Text="Details of Brothers and Sisters of the student"></asp:Label>
                                                        <span class="colonPadding">:</span>&nbsp;
                                                    </b>
                                                </td>
                                                <td colspan="2"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td align="left">
                                                                Name
                                                            </td>
                                                            <td align="left">
                                                                Age
                                                            </td>
                                                            <td align="left">
                                                                Name Of The Institution
                                                            </td>
                                                            <td align="left">
                                                                Standard
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px;">
                                                                <asp:TextBox ID="txtBName1" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                                                            </td>
                                                            <td style="width: 50px;">
                                                                <asp:TextBox ID="txtBAge1" runat="server" CssClass="MidTxtNormalAdmission" Width="50px"
                                                                    MaxLength="2" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                            </td>
                                                            <td style="width: 250px;">
                                                                <asp:TextBox ID="txtBInstitution1" runat="server" CssClass="MidTxtNormalAdmission"
                                                                    Width="250px" />
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <asp:TextBox ID="txtBStandard1" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px;">
                                                                <asp:TextBox ID="txtBName2" runat="server" CssClass="MidTxtNormalAdmission" Width="250px" />
                                                            </td>
                                                            <td style="width: 50px;">
                                                                <asp:TextBox ID="txtBAge2" runat="server" CssClass="MidTxtNormalAdmission" Width="50px"
                                                                    MaxLength="2" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false" />
                                                            </td>
                                                            <td style="width: 250px;">
                                                                <asp:TextBox ID="txtBInstitution2" runat="server" CssClass="MidTxtNormalAdmission"
                                                                    Width="250px" />
                                                            </td>
                                                            <td style="width: 100px;">
                                                                <asp:TextBox ID="txtBStandard2" runat="server" CssClass="MidTxtNormalAdmission" Width="100px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </cc1:CollapsablePanel>
                                </td>
                            </tr>
                             <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                             <tr id="TrStreamDetails" runat="server" visible="false">
                                <td style="background-color: white; width: 880px" id="Td6" colspan="4" align="center">
                                    <cc1:CollapsablePanel ID="colpnlStudentSubjectDetails" runat="server" TitleText="Student Stream / Subjects Details"
                                        TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
                                        CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" TitleStyle-Height="25px"
                                        Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle" Visible ="false">
                                        <table>
                                         <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblStream" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Stream"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td align="left" colspan="1" style="width: 25%">
                                                    <asp:DropDownList ID="ddlStream" runat="server" CssClass="MidCombo"  AutoPostBack="true" OnSelectedIndexChanged = "ddlStream_SelectedIndexChanged"
                                                        Width="150px">
                                                    </asp:DropDownList>
                                                       <span style="color: #ff0000; display:none;"  id="spnStream">*</span>
                                                       
                                                
                                                </td>
                                                <td id="td7" visible="true" runat="server" align="left" class="ClsBorderlight"
                                                    colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblGroup" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Group"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td id="td8" visible="true" runat="server" align="left" colspan="1">
                                                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                
                                                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="MidCombo" AutoPostBack ="true" OnSelectedIndexChanged="ddlGroup_SelectedIndexChanged" 
                                                        Width="150px">
                                                    </asp:DropDownList>
                                                       <span style="color: #ff0000; display:none" id="SpnGroup">*</span>
                                                       
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="ddlStream" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                                </asp:UpdatePanel>     
                                                </td>
                                            </tr>
                                             <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblCompsub" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Compulsary Subjects"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td align="left" colspan="2" style="width: 50%">
                                                <asp:UpdatePanel ID="upnl22" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>                                                
                                                        <asp:Label ID="lblCompulsarySubjects" runat="server" CssClass="ClsLabel" EnableViewState="False" AutoPostBack ="true"
                                                             ></asp:Label>
                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlStream" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                </td>
                                                </tr>
                                                <tr>
                                                <td id="td9" visible="true" runat="server" align="left" class="ClsBorderlight"
                                                    colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblOptSub" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Optional Subjects"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td id="td10" visible="true" runat="server" align="left" colspan="2">
                                                  <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>             
                                                        <asp:RadioButtonList ID="RadioOptionalSubjects" runat="server" RepeatDirection ="Horizontal" 
                                                        CssClass="ClsLabel">
                                                        
                                                    </asp:RadioButtonList>

                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlStream" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                                   

                                                </td>
                                            </tr>
                                            <tr>
                                             <td id="td13" visible="true" runat="server" align="left" 
                                                    colspan="1" style="width: 25%">
                                                   
                                                </td>
                                                    <td id="tdopsub"  runat="server" align="left" colspan="2">

                                                     <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>   
                                                     <asp:RadioButtonList ID="RadioOptionalSubjectArts" runat="server" RepeatDirection ="Horizontal" 
                                                        CssClass="ClsLabel" Visible = "false">
                                                        
                                                    </asp:RadioButtonList>

                                                    </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlStream" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>

                                                    </td>
                                                    </tr>
                                             <tr>
                                                <td id="td11" visible="true" runat="server" align="left" class="ClsBorderlight"
                                                    colspan="1" style="width: 25%">
                                                    <asp:Label ID="lblCompitativeExams" runat="server" CssClass="ClsLabel" EnableViewState="False"
                                                        Text="Compitative Exams"></asp:Label>
                                                    <span class="colonPadding">:</span>&nbsp;&nbsp;
                                                </td>
                                                <td id="td12" visible="true" runat="server" align="left" colspan="2">
                                                   <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>   
                                                        <asp:CheckBoxList ID="chkCompitativeExams"  runat="server" RepeatDirection ="Horizontal" CssClass="ClsLabel">
                                                        </asp:CheckBoxList>
                                                        </ContentTemplate>
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="ddlGroup" EventName="SelectedIndexChanged" />
                                                        <asp:AsyncPostBackTrigger ControlID="ddlStream" EventName="SelectedIndexChanged" />
                                                    </Triggers>
                                                </asp:UpdatePanel>
                                                   
                                                </td>
                                            </tr>
                                            </table>
                                        </cc1:CollapsablePanel>
                                        </td>
                                        </tr>
                                         <tr>
                                <td align="center" colspan="4" style="height: 17px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="5">
                                    <asp:Button ID="btnSaveNext" OnClick="btnSaveNext_Click" Text="<%$ Resources:LocalizedResources, SaveAndContinue%>"
                                        runat="server" CssClass="ClsBtnExLrg" CausesValidation="false"></asp:Button>
                                    <asp:Button ID="btnSave" runat="server" CssClass="ClsBtn" OnClick="btnSave_Click"
                                        Text="<%$ Resources:LocalizedResources, Save%>" CausesValidation="true" />
                                    <asp:Button ID="btnAddSiblingDetails" Text="<%$ Resources:LocalizedResources, AddSiblingDetails%>"
                                        runat="server" value="Open Window" CausesValidation="False" CssClass="ClsBtnLrg"
                                        Visible="false" />
                                    <asp:Button ID="btnClear" Text="<%$ Resources:LocalizedResources, Clear%>" runat="server"
                                        Visible="false" OnClientClick="Page_BlockSubmit = false;" CssClass="ClsBtn" CausesValidation="False"
                                        UseSubmitBehavior="false" OnClick="btnClear_Click"></asp:Button>
                                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" Text="<%$ Resources:LocalizedResources, Close %>"
                                        runat="server" OnClientClick="Page_BlockSubmit = false;" CssClass="ClsBtn" CausesValidation="false"
                                        UseSubmitBehavior="false"></asp:Button>
                                    <asp:Button ID="btnAddAchievement" Text="Add Notes" runat="server" value="Open Window"
                                        Visible="false" CausesValidation="False" CssClass="ClsBtnLrg" />
                                </td>
                            </tr>
                            <%-------------------------------------------------------------------------%>
                        </tbody>
                    </table>
                    <div id="divSetting" runat="server" align="center" style="visibility: hidden; display: none;
                        position: absolute; margin: 0px; padding: 0px; width: 28%; border-width: 1px;
                        left: 10px; top: 150px; line-height: normal; border: solid 2px darkgreen; background-color: white;">
                        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right">
                            <div style="font-size: 12px; width: 50%; letter-spacing: 1px; padding-left: 8px;
                                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                Sibling Details
                            </div>
                            <span style="cursor: hand" onclick="javascript:HidePopup();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" border="0" />
                            </span>
                        </div>
                        <div>
                            <table>
                                <tr>
                                    <td style="horrizontal-align: left;">
                                        <asp:Label ID="lblsiblingNote" runat="server" Text="If you click on save button selected Sibling Details will be replaced to the following sibling(s):"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Label ID="lblStudentSiblingName" Font-Bold="true" runat="server" CssClass="ClsLabel"
                                Style="text-align: left"> </asp:Label>
                            <asp:ListView ID="lstvwSiblingsDetails" runat="server" DataKeyNames="CommonFieldId"
                                OnItemDataBound="lstvwSiblingsDetails_ItemDataBound">
                                <LayoutTemplate>
                                    <table align="center" width="100%" hight="100%" runat="server" id="tblSiblingsInfo"
                                        style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th align="left" width="50px" style="padding-left: 9px;">
                                                <asp:CheckBox ID="ChkSelectAllSiblings" runat="server" onclick="CheckAllUncheckAllsSiblings()">
                                                </asp:CheckBox>
                                            </th>
                                            <th align="center" width="100px">
                                                Update details to Sibling
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                        <th align="left" width="40px" style="padding-left: 9px;">
                                            <asp:CheckBox ID="ChkSelectSiblingsSingle" runat="server"></asp:CheckBox>
                                        </th>
                                        <th align="left" width="100%" style="padding-left: 9px;">
                                            <asp:Label ID="lblSiblingName" runat="server" Text='<%# Eval("CommonFieldName") %>'></asp:Label>
                                        </th>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                        <th align="left" width="40px" style="padding-left: 9px;">
                                            <asp:CheckBox ID="ChkSelectSiblingsSingle" Checked="true" runat="server"></asp:CheckBox>
                                        </th>
                                        <th align="left" width="100%" style="padding-left: 9px;">
                                            <asp:Label ID="lblSiblingName" runat="server" Text='<%# Eval("CommonFieldName") %>'></asp:Label>
                                        </th>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                            <div>
                                <table>
                                    <tr>
                                        <td colspan="2" align="center" valign="bottom" style="padding: 10px;">
                                            <asp:Button ID="btnSavePopUp" runat="server" Text="Save" CssClass="ClsBtnMid" CausesValidation="true"
                                                Width="75px" OnClick="btnSave_Click" />
                                            <asp:Button ID="btnClosePopUp" runat="server" Text="Cancel" CssClass="ClsBtnMid"
                                                CausesValidation="true" Width="75px" OnClientClick="ResetSiblingListViewCheckBoxes();"
                                                OnClick="btnSave_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <asp:CustomValidator ID="cstRollNo" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="checkRollNo"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqFieldValidatorFirstName" runat="server" Display="None"
                        ErrorMessage="<%$ Resources:LocalizedResources, FirstNameValidation %>" CssClass="ClsMdtStar"
                        ControlToValidate="txtFirstName"></asp:RequiredFieldValidator>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidatorAmount" runat="server" Display="None"
                        ErrorMessage="AmountValidator" CssClass="ClsMdtStar"
                        ControlToValidate="txtAnnualIncome"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cstValMotherName" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="MotherNameValidation"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="reqFieldValidateDob" runat="server" Display="None"
                        ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthValidation %>" CssClass="ClsMdtStar"
                        ControlToValidate="txtCalDobPopup"></asp:RequiredFieldValidator>&nbsp;
                    <asp:CustomValidator ID="cstDOB" Display="None" runat="server" CssClass="ClsMdtStar"
                        ControlToValidate="txtCalDobPopup" Visible="true" ErrorMessage="<%$ Resources:LocalizedResources, DateOfBirthFutureDate %>"
                        EnableClientScript="true" ClientValidationFunction="checkDOB"></asp:CustomValidator>&nbsp;&nbsp;
                    <asp:RequiredFieldValidator ID="reqValidatorParentName" runat="server" ControlToValidate="txtParentName"
                        CssClass="ClsMdtStar" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ParentNameBlank%>"></asp:RequiredFieldValidator>&nbsp;
                    <asp:CustomValidator ID="CustomValidator1" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="ValidatecasteSubcaste"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator2" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="ValidateBirthPlace"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator3" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="ValidateNationality"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator4" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="ValidateMotherTongueLanguage"></asp:CustomValidator>
                    <asp:CompareValidator ID="cmpValidOcupation" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ParentOccupationBlank%>"
                        CssClass="ClsMdtStar" ControlToValidate="cmbOcupation" Operator="NotEqual" ValueToCompare="0"></asp:CompareValidator>
                    <asp:CustomValidator ID="cstdOccupation" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ErrorMessage="<%$ Resources:LocalizedResources,OtherOccupationBlank%>"
                        EnableClientScript="true" ClientValidationFunction="ReqdOtherOccupation"></asp:CustomValidator>
                    <asp:RequiredFieldValidator Display="None" ID="reqFldValAddress" runat="server" Width=""
                        ErrorMessage="<%$ Resources:LocalizedResources,AddressBlank%>" CssClass="ClsMdtStar"
                        ControlToValidate="txtAddress"> </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="Regu_Expr_ValiAddress" runat="server" Display="None"
                        ControlToValidate="txtAddress" ErrorMessage="<%$ Resources:LocalizedResources, LengthOfAddress%>"
                        ValidationExpression="^[\s\S]{0,300}$"> </asp:RegularExpressionValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                        ControlToValidate="txtAdditionalSubjectNames" ErrorMessage="<%$ Resources:LocalizedResources, LengthOfSubjectNames%>"
                        ValidationExpression="^[\s\S]{0,500}$"> </asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator Display="None" CssClass="ClsMdtStar" ID="reqFldValCity"
                        runat="server" ErrorMessage="<%$ Resources:LocalizedResources, CityBlank%>" ControlToValidate="txtCity"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator Display="None" ID="rqd_Pin" runat="server" CssClass="ClsMdtStar"
                        ErrorMessage="<%$ Resources:LocalizedResources, PinBlank%>" ControlToValidate="txtPIN"> </asp:RequiredFieldValidator>
                    <asp:CustomValidator Display="None" CssClass="ClsMdtStar" ErrorMessage="<%$ Resources:LocalizedResources, PinCodeDigit%>"
                        ID="cst_PIN" runat="server" ClientValidationFunction="PinCodeValidation"></asp:CustomValidator>
                    <%--    <asp:RequiredFieldValidator ID="reqMobileNo" runat="server" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, MobileNumberBlank%>"
        CssClass="ClsMdtStar" ControlToValidate="txtMobilePhoneNumber"></asp:RequiredFieldValidator>--%>
                    <asp:CustomValidator ID="cst_MobileNumber1" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="CheckMobileNumberBlank"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_MobileNumber1and2" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="RequiredMobileNumberValidation"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_MobileNumber" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                    <asp:CustomValidator ID="cst_MobileNumber2" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ErrorMessage="" EnableClientScript="true" ClientValidationFunction="MobileNumberValidation"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstHasSibling" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="SiblingValidation"></asp:CustomValidator>
                    <asp:CustomValidator ID="CustomValidator5" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" EnableClientScript="true" ClientValidationFunction="ValidateLastSchoolName"></asp:CustomValidator>
                    <asp:CustomValidator ID="cust_SelectAtleastOneDocument" Display="None" runat="server"
                        CssClass="ClsMdtStar" ErrorMessage="Mandatory Documents should be selected."
                        ClientValidationFunction="CheckDocumentSelection"></asp:CustomValidator>  
                    <asp:RegularExpressionValidator ID="revApaarId" runat="server" Display="None"
                        ControlToValidate="txtApaarId" ErrorMessage="APAAR ID should be 12-digit number."
                        ValidationExpression="^\d{12}$"></asp:RegularExpressionValidator>
                       <asp:CustomValidator ID="CustStream" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="CheckStream"></asp:CustomValidator>   
                       <asp:CustomValidator ID="CustGroup" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="CheckGroup"></asp:CustomValidator>  
                        <asp:CustomValidator ID="CustOptionalSubject" Display="None" runat="server" CssClass="ClsMdtStar"
                        Visible="true" ClientValidationFunction="CheckOptionalSubject"></asp:CustomValidator>    
                    <%--  <asp:CustomValidator ID="CustomValidator6" Display="None" runat="server"
        CssClass="ClsMdtStar" ErrorMessage="" ClientValidationFunction="ValidateLanguage"></asp:CustomValidator>--%>
                </td>
            </tr>
            <tr>
                <td>
                    <div runat="server" id="divErr">
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hidSchoolId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidClassId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidMode" runat="server" />
                    <asp:HiddenField ID="hidSubCasteId" runat="server" />
                    <asp:HiddenField ID="hidStudentId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidYearWiseStudentId" runat="server" />
                    <asp:HiddenField ID="hidIsAdditionalInformationAvailable" runat="server" />
                    <asp:HiddenField ID="hidFilePath" runat="server" />
                    <asp:HiddenField ID="hidUserId" runat="server" />
                    <asp:HiddenField ID="HidBackUrl" runat="server" />
                    <asp:HiddenField ID="hidEncryptedString" runat="server" />
                    <asp:HiddenField ID="hidSiblingStudentId" runat="server" OnValueChanged="hidSiblingStudentId_ValueChanged" />
                    <asp:HiddenField ID="hidOverwrite" runat="server" Value="N" />
                    <asp:HiddenField ID="hidOldJoiningDate" runat="server" />
                    <asp:HiddenField ID="hidServerDate" runat="server" />
                    <asp:HiddenField ID="hidIsConfig" runat="server" />
                    <asp:HiddenField ID="hidHasAttendance" runat="server" Value="False" />
                    <asp:HiddenField ID="hidAcademicStartDate" runat="server" />
                    <asp:HiddenField ID="hidAcademicEndDate" runat="server" />
                    <asp:HiddenField ID="hidRegPrefix" runat="server" />
                    <asp:HiddenField ID="hidAllRegPrefixes" runat="server" />
                    <asp:HiddenField ID="hidRegPostfix" runat="server" />
                    <asp:HiddenField ID="hidRegPrefixOrgnl" runat="server" />
                    <asp:HiddenField ID="hidRegPostfixOrgnl" runat="server" />
                    <asp:HiddenField ID="hidOrgRegNo" runat="server" />
                    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" />
                    <asp:HiddenField ID="hidAcademicYearStatus" runat="server" Value="" />
                    <asp:HiddenField ID="hidRuleId" runat="server" Value="0" />
                    <asp:HiddenField ID="hidStudentSiblingNames" runat="server" />
                    <asp:HiddenField ID="hidCommonFieldNames" runat="server" />
                    <asp:HiddenField ID="hidIsOverwriteSiblingDetails" runat="server" />
                    <asp:HiddenField ID="hidIsSiblingAdded" runat="server" />
                    <asp:HiddenField ID="hidIsExactMatch" runat="server" Value="False" />
                    <asp:HiddenField ID="hidOperator" runat="server" />
                    <asp:HiddenField ID="hidPrefix" runat="server" />
                    <asp:HiddenField ID="hidPostfix" runat="server" />
                    <asp:HiddenField ID="hidRegNo" runat="server" />
                    <asp:HiddenField ID="hidIsSuperAdmin" runat="server" Value="N" />
                    <asp:HiddenField ID="hidDefaultRollNo" runat="server" />
                    <asp:HiddenField ID="hidItemCount" runat="server" Value="" OnValueChanged="HidItemCount_ValueChanged" />
                    <asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
                    <asp:HiddenField ID="HidIsBinaryImage" runat="server" />
                    <asp:HiddenField ID="hidRegNumberBlank" runat="server" />
                    <asp:HiddenField ID="hidRegNumberEndWith" runat="server" />
                    <asp:HiddenField ID="hidRegNumberIsNumber" runat="server" />
                    <asp:HiddenField ID="hidRegNumberFormat" runat="server" />
                    <asp:HiddenField ID="hidRegNumberStartWith" runat="server" />
                    <asp:HiddenField ID="hidRegNumberZeroValidation" runat="server" />
                    <asp:HiddenField ID="hidAtLeastOneSibling" runat="server" />
                    <asp:HiddenField ID="hidReplaceStudentWithSibling" runat="server" />
                    <asp:HiddenField ID="hidRemoveThisPhoto" runat="server" />
                    <asp:HiddenField ID="hidRTEStudentSelected" runat="server" />
                    <asp:HiddenField ID="hidRTECategorySelected" runat="server" />
                    <asp:HiddenField ID="hidInvalidFileFormat" runat="server" />
                    <asp:HiddenField ID="hidSchoolLogo" runat="server" />
                    <asp:HiddenField ID="hidPinCodeDigit" runat="server" />
                    <asp:HiddenField ID="hidPinBlank" runat="server" />
                    <asp:HiddenField ID="hidMobileNumber1and2Blank" runat="server" />
                    <asp:HiddenField ID="hidMobileNumberBlank" runat="server" />
                    <asp:HiddenField ID="hidMotherNumberZero" runat="server" />
                    <asp:HiddenField ID="hidFatherNumberZero" runat="server" />
                    <asp:HiddenField ID="hidMobileNumber1Zero" runat="server" />
                    <asp:HiddenField ID="hidMobileNumber2Zero" runat="server" />
                    <asp:HiddenField ID="hidMobileNumber2Digit" runat="server" />
                    <asp:HiddenField ID="hidRollNumberBlank" runat="server" />
                    <asp:HiddenField ID="hidRollNumberZero" runat="server" />
                    <asp:HiddenField ID="hidMobileDigit" runat="server" />
                    <asp:HiddenField ID="hidMotherNumberDigit" runat="server" />
                    <asp:HiddenField ID="hidFatherNumberDigit" runat="server" />
                    <asp:HiddenField ID="hidAttendanceValidation" runat="server" />
                    <asp:HiddenField ID="hidSyatemTillDateAttendance" runat="server" />
                    <asp:HiddenField ID="hidSystemAttendance" runat="server" />
                    <asp:HiddenField ID="hidDateOfJoiningValidation" runat="server" />
                    <asp:HiddenField ID="hidDateOfJoining" runat="server" />
                    <asp:HiddenField ID="hidDateOfAdmission" runat="server" />
                    <asp:HiddenField ID="hidDateOfBirthGreaterValidation" runat="server" />
                    <asp:HiddenField ID="hidUploadDocumet" runat="server" />
                    <asp:HiddenField ID="hidReplaceDetails" runat="server" />
                    <asp:HiddenField ID="hidChangeDetails" runat="server" />
                    <asp:HiddenField ID="hidCultureInfo" runat="server" />
                    <asp:HiddenField ID="hidEmailValidation" runat="server" />
                    <asp:HiddenField ID="hidAdditionalFatherEmailValidation" runat="server" />
                    <asp:HiddenField ID="hidAdditionalMotherEmailValidation" runat="server" />
                    <asp:HiddenField ID="hidMandatoryFields" runat="server" />
                    <asp:HiddenField ID="hidValMotherName" runat="server" />
                    <asp:HiddenField ID="hidValCaste" runat="server" />
                    <asp:HiddenField ID="hidValBirthPlace" runat="server" />
                    <asp:HiddenField ID="hidValNationality" runat="server" />
                    <asp:HiddenField ID="hidValMotherToungue" runat="server" />
                    <asp:HiddenField ID="hidValLastSchoolName" runat="server" />
                    <asp:HiddenField ID="hidDOBConfirmationMsg" runat="server" Value="" />
                    <asp:HiddenField ID="hidMinDOB" runat="server" Value="" />
                    <asp:HiddenField ID="hidMaxDOB" runat="server" Value="" />
                    <asp:HiddenField ID="hidStandardName" runat="server" Value="" />
                    <asp:HiddenField ID="hidIsPPSN" runat="server" Value="False" />
                    <asp:HiddenField ID="hidQueryValue" runat="server" Value="False" />
                    <asp:HiddenField ID="hidCurrentDate" runat="server" />
                    <asp:HiddenField ID="hidSubjectGroupIds" runat="server" Value="" />
                    <asp:HiddenField ID="hidAadharImage" runat="server" Value="" />
                    <asp:HiddenField ID="hidBirthCertificatePhoto" runat="server" Value="" />
                    <asp:HiddenField ID="hidFatherPhoto" runat="server" Value="" />
                    <asp:HiddenField ID="hidMotherPhoto" runat="server" Value="" />
                    <asp:HiddenField ID="hidGuardianPhoto" runat="server" Value="" />
                    <asp:HiddenField ID="hidLanguageGroupIds" runat="server" Value="" />
                    <asp:HiddenField ID="hidSectionId" runat="server" Value="" />
                    <asp:HiddenField ID="hidPPSchoolId" runat="server" Value="18" />
                    <asp:HiddenField ID="hidDPISIsFeePaid" runat="server" Value="N" />
                    <asp:HiddenField ID="hidOldIsForDayBoarding" runat="server" Value="" />
                    <asp:HiddenField ID="hidIsAaryanSchool" runat="server" Value="N" />
					<asp:HiddenField ID="hidOldFeeCategoryId" runat="server" Value = "0" />
                    <asp:HiddenField  ID ="hidCompulsorySubjects" runat="server" />
                    <asp:HiddenField ID = "hidOptionalSubjects" runat="server" Value="N" />
                    <asp:HiddenField ID = "hidOldFeeAreaId" runat="server" Value="0" />

                    <asp:HiddenField ID = "hisShowStreamSection" runat="server" Value="0" />
                    <asp:HiddenField ID = "hidRestrictAnnualIncomeForRTE" runat="server" Value="0" />
                 </td>
            </tr>
        </tbody>
    </table>
    <script lang="javascript" type="text/javascript">

        _sClienttxtMobilePhoneNumberId = "<%=this.txtMobilePhoneNumber.ClientID %>";
        _sClienttxtMobilePhoneNumber2Id = "<%=this.txtMobilePhoneNumber2.ClientID %>";
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>";
        _cstValidateAadharScanCopy = "<%= this.cstValidateAadharScanCopy.ClientID %>";
        _cstValidateFamilyPhoto = "<%= this.cstValidateFamilyPhoto.ClientID %>";
        _clientfuAadharNumber = "<%= this.fuAadharNumber.ClientID %>";
        _clientFuFamilyPhoto = "<%=this.FuFamilyPhoto.ClientID %>"
        _clientFileUploadLogo = "<%=this.FileUploadLogo.ClientID%>";
        _clientbtnSaveNext = "<%=this.btnSaveNext.ClientID%>";
        _clientbtnSave = "<%=this.btnSave.ClientID%>";
        _clientbtnCancel = "<%=this.btnCancel.ClientID%>";
        _clienttxtCalDobPopup = "<%=this.txtCalDobPopup.ClientID %>";
        _clienthidServerDate = "<%=this.hidServerDate.ClientID %>";
        _clienthidAcademicStartDate = "<%=this.hidAcademicStartDate.ClientID %>";
        _clienthidAcademicEndDate = "<%=this.hidAcademicEndDate.ClientID %>";
        _clienthidEncryptedString = "<%=this.hidEncryptedString.ClientID %>";
        _clienthidOverwrite = "<%=this.hidOverwrite.ClientID %>";
        _clienthidSchoolwiseStudentId = "<%=this.hidSchoolwiseStudentId.ClientID %>";
        _clienthidYearWiseStudentId = "<%=this.hidYearWiseStudentId.ClientID %>";
        _clienthidSiblingStudentId = "<%=this.hidSiblingStudentId.ClientID %>";
        _clienttxtcalAdmissionDate = "<%=this.txtcalAdmissionDate.ClientID %>";
        _clientcstmsg = "<%=this.cstAdmissionDate.ClientID %>";
        _clienthidOldJoiningDate = "<%=this.hidOldJoiningDate.ClientID %>";
        _clienthidHasAttendance = "<%=this.hidHasAttendance.ClientID %>";
        _clienthidMode = "<%=this.hidMode.ClientID %>";
        _clienttxtJoiningDate = "<%=this.txtJoiningDate.ClientID %>";
        _clientcustJoining = "<%=this.custJoining.ClientID %>";
        _clienttxtOtherOccupation = "<%=this.txtOtherOccupation.ClientID %>";
        _clientcmbOcupation = "<%=this.cmbOcupation.ClientID %>";
        _clientcstdOccupation = "<%=this.cstdOccupation.ClientID %>";
        _clienthidRegPrefix = "<%=this.hidRegPrefix.ClientID %>";
        _clienthidAllRegPrefixes = "<%=this.hidAllRegPrefixes.ClientID %>"
        _clienthidRegPostfix = "<%=this.hidRegPostfix.ClientID %>";
        _clienthidRegPrefixOrgnl = "<%=this.hidRegPrefixOrgnl.ClientID %>";
        _clienthidRegPostfixOrgnl = "<%=this.hidRegPostfixOrgnl.ClientID %>";
        _clienttxtEmailId = "<%=this.txtEmail.ClientID %>";
        _clienttxtAdditionalFatherEmail = "<%=this.txtAdditionalFatherEmail.ClientID %>";
        _clienttxtAdditionalMotherEmail = "<%=this.txtAdditionalMotherEmail.ClientID %>";
        _clienttxtRegNo = "<%=this.txtRegNo.ClientID %>";
        _clientreqFieldValidatorRegNo = "<%=this.reqFieldValidatorRegNo.ClientID %>";
        _clientreqFieldValidatorFormNo = "<%=this.reqFieldValidatorFormNo.ClientID %>";
        _clientchkNewAddmission = "<%=this.chkNewAddmission.ClientID %>";
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>";
        _clientcst_MobileNumber1and2 = "<%=this.cst_MobileNumber1and2.ClientID %>";
        _clientcst_MobileNumber1 = "<%=this.cst_MobileNumber1.ClientID %>";
        _clientcst_MobileNumber = "<%=this.cst_MobileNumber.ClientID %>";
        _clientcst_MobileNumber2 = "<%=this.cst_MobileNumber2.ClientID %>";
        _clientcstRollNo = "<%=this.cstRollNo.ClientID %>";
        _clientRollNo = "<%=this.txtRollNumber.ClientID %>";
        _clientListViewId = "<%=this.lstvwConfiguredDocument.ClientID %>"
        _ClientChkAll = _clientListViewId + "_ChkSelectAll";
        _ClientIsApplicableAll = _clientListViewId + "_chkIsApplicableAll";
        _ClientchkHasSibling = "<%=this.chkHasSibling.ClientID %>";
        _ClienthidIsOverwriteSiblingDetails = "<%=this.hidIsOverwriteSiblingDetails.ClientID %>";
        _ClienthidIsSiblingAdded = "<%=this.hidIsSiblingAdded.ClientID %>";
        _ClienthidStudentSiblingNames = "<%=this.hidStudentSiblingNames.ClientID %>";
        _sClientrRTECategory = "<%=this.trRTECatrgory.ClientID %>";
        _sCliencmbRTECategory = "<%=this.cmbRTECategory.ClientID %>";
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _clientlstvwSiblingss = "<%=this.lstvwSiblingsDetails.ClientID %>"
        _ClientChkAllSiblings = _clientlstvwSiblingss + "_ChkSelectAllSiblings";
        _clienthidMandatoryFields = "<%=this.hidMandatoryFields.ClientID %>"
        _clienttxtFormNo = "<%=this.txtFormNo.ClientID %>"
        _clienttxtMotherName = "<%=this.txtMotherName.ClientID %>";
        _clienttxtCasteAndSubcaste = "<%=this.txtCasteAndSubcaste.ClientID %>";
        _clienttxtBirthPlace = "<%=this.txtBirthPlace.ClientID %>";
        _clienttxtNationality = "<%=this.txtNationality.ClientID %>";
        _clienttxtMotherTongue = "<%=this.txtMotherTongue.ClientID %>";
        _clienttxtLastSchoolName = "<%=this.txtLastSchoolName.ClientID %>";
        _clienthidQueryValue = "<%=this.hidQueryValue.ClientID %>";

        _clienthidDOBConfirmationMsg = "<%=this.hidDOBConfirmationMsg.ClientID %>"

        _clienthidhidMinDOB = "<%=this.hidMinDOB.ClientID %>"
        _clienthidhidMaxDOB = "<%=this.hidMaxDOB.ClientID %>"
        _clienthidIsPPSN = "<%=this.hidIsPPSN.ClientID %>"
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID %>"

        _clienthidSubjectGroupIds = "<%=this.hidSubjectGroupIds.ClientID %>"
        _clientcmbThirdLanguage = "<%=this.cmbThirdLanguage.ClientID %>"
        _clientddlSecondLanguage = "<%=this.ddlSecondLanguage.ClientID %>"

        _clientFUAdditionalFatherPhoto = "<%=this.FUAdditionalFatherPhoto.ClientID %>"
        _clientcstValidateFatherPhoto = "<%=this.cstValidateFatherPhoto.ClientID %>"
        _clientfuAdditionalMotherPhoto = "<%=this.fuAdditionalMotherPhoto.ClientID %>"
        _clientcstValidateMotherPhoto = "<%=this.cstValidateMotherPhoto.ClientID %>"
        _clientFUAdditionalGuardianPhoto = "<%=this.FUAdditionalGuardianPhoto.ClientID %>"
        _clientcstValidateGuardianPhoto = "<%=this.cstValidateGuardianPhoto.ClientID %>"
        _clienttxtAdditionalFatherDOB = "<%=this.txtAdditionalFatherDOB.ClientID %>"
        _clienttxtAdditionalMotherDOB = "<%=this.txtAdditionalMotherDOB.ClientID %>"
        _clienttxtAdditionalAnniversaryDate = "<%=this.txtAdditionalAnniversaryDate.ClientID %>"
        _clientcstValidateFatherDOB = "<%=this.cstValidateFatherDOB.ClientID %>"
        _clientcstValidateMotherDOB = "<%=this.cstValidateMotherDOB.ClientID %>"
        _clientcstAnniversaryDate = "<%=this.cstAnniversaryDate.ClientID %>"
        _clienthidLanguageGroupIds = "<%=this.hidLanguageGroupIds.ClientID %>"
        _clienthidSectionId = "<%=this.hidSectionId.ClientID %>"
        _clienthidDPISIsFeePaid = "<%=this.hidDPISIsFeePaid.ClientID %>"

        _clienthidIsAaryanSchool = "<%=this.hidIsAaryanSchool.ClientID %>"        
        _clientcmbFeeCategory = "<%=this.cmbFeeCategory.ClientID %>"

        _schoolId = "<%=hidSchoolId.ClientID %>"
        _clienthidPPSchoolId = "<%=hidPPSchoolId.ClientID %>"
        _clientStream = "<%= this.ddlStream.ClientID %>"
        _clientGroup = "<%= this.ddlGroup.ClientID %>"
        _ClientCustStream = "<%= this.CustStream.ClientID %>"
        _ClientCustGroup = "<%= this.CustGroup.ClientID %>"
        _ClientRadioOptionalSubject = "<%=this.RadioOptionalSubjects.ClientID %>"
        _ClientOptionalSubject = "<%=this.CustOptionalSubject.ClientID %>"
        _ClientHidOptionalSubject = "<%=this.hidOptionalSubjects.ClientID %>"
        _ClientCompulsorySubject = "<%=this.hidCompulsorySubjects.ClientID %>"
        var schoolIdSNS = 122;
        
        if (document.getElementById(_schoolId).value == schoolIdSNS) {
            document.getElementById("spnMobileNo").style.display = 'none';
            document.getElementById("SpnStream").style.display = 'inline';
            document.getElementById("SpnGroup").style.display = 'inline';
        }
        else {
            document.getElementById("spnMobileNo").style.display = 'inline';
            document.getElementById("SpnStream").style.display = 'none';
            document.getElementById("SpnGroup").style.display = 'none';
        }

        function CloseWindow(obj) {
            window.opener.location = "../Admin/AllStudentsUI.aspx?" + obj;
            window.opener.focus();
            window.close();
        }

        ///This function is used to atleast on check box selection for submitted documents.
        function CheckDocumentSelection(oSrc, args) {
            var isFound = false
            _clientlstvwConfiguredDocument = "<%=this.lstvwConfiguredDocument.ClientID %>"

            var hidIsDocMand;
            var iRowCount = 0;
            var isFound;
            var validateDocument = $('#' + _clienthidIsPPSN).val()
            if (validateDocument == "Y") {
                var chkIsApplicable = document.getElementById(_clientlstvwConfiguredDocument + "_ctrl" + iRowCount + "_chkIsApplicable")

                while (chkIsApplicable != null) {
                    hidIsDocMand = document.getElementById(_clientlstvwConfiguredDocument + "_ctrl" + iRowCount + "_hidIsDocMandatory")
                    var chkSelect = document.getElementById(_clientlstvwConfiguredDocument + "_ctrl" + iRowCount + "_ChkSelect")
                    if (hidIsDocMand.value == "True" && chkIsApplicable.checked && chkSelect.checked == false) {
                        isFound = true;
                        break
                    }
                    iRowCount = iRowCount + 1;
                    chkIsApplicable = document.getElementById(_clientlstvwConfiguredDocument + "_ctrl" + iRowCount + "_chkIsApplicable")
                }
            }

            if (isFound) {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }
        }

        function testDigit(str) {
            return /^ *[0-9]+ *$/.test(str);
        }

        function chckvalidpostfix(sRegNo, sRegPostfix, sRegPrefix) {
            var RegistrationNumValue = new Array();
            var count = 0;
            var iNumberCount = sRegNo.split("-").length - 1;

            if (iNumberCount > 3)
                return false;
            RegistrationNumValue = sRegNo.split("-");

            var FirstNumber = 0;


            if (iNumberCount == 3) {
                FirstNumber = RegistrationNumValue[1];
            }
            else {

                FirstNumber = RegistrationNumValue[0];
            }

            if (!testDigit(FirstNumber)) {
                FirstNumber = FirstNumber.substring(sRegPrefix.length);
            }

            if (testDigit(FirstNumber)) {
                var PostFixValues = new Array();
                var sRegPostfixValue = sRegPostfix;
                PostFixValues = sRegPostfixValue.split(",");
                for (var j = 0; j < PostFixValues.length; j++) {

                    if (sRegNo.match(PostFixValues[j]) != null && FirstNumber != "") {
                        count += 1;
                        break;
                    }
                }
            }
            if (count > 0)
                return true;
            else
                return false;
        }

        function checkRegNo(oSrc, args) {
            var isValid = true;
            var msg = "Registration number should start with valid prefix and contain all digits after prefix.";

            var reDigit = /^<%=this.hidRegPrefix.Value %>\d*/;
            var reDigitPost = /^<%=this.hidRegPostfix.Value %>\d*/;
            var sRegPrefix = document.getElementById(_clienthidRegPrefixOrgnl).value;
            var sRegPostfix = document.getElementById(_clienthidRegPostfixOrgnl).value;
            var sRegNo = document.getElementById(_clienttxtRegNo).value;

            if (sRegPostfix.trim() != "")
                msg = "Registration number should start with valid prefix, end with valid postfix and contain all digits between prefix and postfix.";

            var hidMode = document.getElementById(_clienthidMode).value;

            document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberBlank.ClientID %>").value;
            if (sRegNo != "") {
                if (sRegPrefix != "" && sRegNo.match(sRegPrefix) == null) {
                    if (hidMode.toUpperCase() == "NEW") {
                        document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberStartWith.ClientID %>").value + " '" + sRegPrefix + "' followed by numbers.";
                        isValid = false;
                    }
                    else {
                        var invalid = ValidatePrefixes(sRegNo, sRegPostfix)
                        if (invalid) {
                            document.getElementById(_clientreqFieldValidatorRegNo).errormessage = msg;
                            isValid = false;
                        }
                    }
                }
                else if (sRegPostfix != "") {

                    isValid = chckvalidpostfix(sRegNo, sRegPostfix, sRegPrefix);
                    if (isValid <= 0) {
                        document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberEndWith.ClientID %>").value + sRegPostfix + '.';
                        isValid = false;
                    }
                }
                else if (sRegPrefix == "") {
                    if (!testDigit(sRegNo)) {
                        if (hidMode.toUpperCase() == "NEW") {
                            document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberIsNumber.ClientID %>").value;
                            isValid = false;
                        }
                        else {
                            var invalid = ValidatePrefixes(sRegNo, sRegPostfix)
                            if (invalid) {
                                document.getElementById(_clientreqFieldValidatorRegNo).errormessage = msg;
                                isValid = false;
                            }
                        }
                    }
                }
                else if (sRegPrefix != "" && sRegNo.match(sRegPrefix) != null && hidMode.toUpperCase() == "EDIT") {
                    var invalid = ValidatePrefixes(sRegNo, sRegPostfix)
                    if (invalid) {
                        document.getElementById(_clientreqFieldValidatorRegNo).errormessage = msg;
                        isValid = false;
                    }
                }
            }
            var PrefixLen = 0;
            var PostfixLength = 0;
            var regNo = sRegNo;
            if (sRegPrefix != "" && sRegPostfix == "" && hidMode.toUpperCase() == "NEW") {
                PrefixLen = sRegPrefix.length;
                regNo = sRegNo.substring(PrefixLen);
                if (!testDigit(regNo)) {
                    //document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberFormat.ClientID %>").value;
                    document.getElementById(_clientreqFieldValidatorRegNo).errormessage = msg;
                    isValid = false;
                }
            }
            if (parseInt(regNo / 1) == '0') {
                var validateDocument = $('#' + _clienthidIsPPSN).val()
                if (validateDocument == "N") {
                    document.getElementById(_clientreqFieldValidatorRegNo).errormessage = document.getElementById("<%=this.hidRegNumberZeroValidation.ClientID %>").value; //modified for PPSN
                    isValid = false;
                }
                else
                    isValid = true;
            }
            if (isValid) {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }

        function ValidatePrefixes(sRegNo, sRegPostfix) {
            var invalid = true;
            var sAllRegPrefixes = $('#' + _clienthidAllRegPrefixes).val();
            var prefixes = sAllRegPrefixes.split(',')
            if (prefixes.length > 0) {
                for (var j = 0; j < prefixes.length; j++) {
                    if (prefixes[j] == "NULL") {
                        if (testDigit(sRegNo)) {
                            invalid = false;
                            break;
                        }
                    }
                    else if (prefixes[j] != "" && sRegNo.match(prefixes[j]) != null) {
                        invalid = false;
                        break;
                    }
                }
            }

            var invalidPostfix = false
            if (!invalid) {
                if (sRegPostfix != "") {
                    var pstfxs = sRegPostfix.split(',')
                    if (pstfxs.length > 0) {
                        invalidPostfix = true
                        for (var j = 0; j < pstfxs.length; j++) {
                            if (pstfxs[j] != "" && sRegNo.match(pstfxs[j]) != null) {
                                invalidPostfix = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (invalidPostfix)
                invalid = true;

            var reg = sRegNo;
            if (!invalid) {
                for (var j = 0; j < prefixes.length; j++) {
                    reg = reg.replace(prefixes[j], "");
                }

                if (sRegPostfix != "") {
                    var postfixes = sRegPostfix.split(',')
                    if (postfixes.length > 0) {
                        for (var j = 0; j < postfixes.length; j++) {
                            reg = reg.replace(postfixes[j], "");
                        }
                    }
                }

                if (!testDigit(reg)) {
                    invalid = true
                }
            }

            return invalid;
        }

        function checkFormNo(oSrc, args) {
            var sFormNo = document.getElementById(_clienttxtFormNo).value;
            sFormNo = sFormNo.trim();
            var validateDocument = $('#' + _clienthidIsPPSN).val()
            if (validateDocument == "Y") {
                if (sFormNo == "") {
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function checkDOB(oSrc, args) {
            var dtDob = document.getElementById("<%=this.txtCalDobPopup.ClientID %>").value;

            var dob;
            if (document.all)
                dob = new Date(dtDob.replace('-', ' '));
            else
                dob = new Date(convertdate(dtDob));

            var serverdate = document.getElementById("<%=this.hidServerDate.ClientID %>").value;
            var today = new Date(convertvaliddate2(serverdate));
            if (dob > today) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function checkAdmissionDate(oSrc, args) {

            var dtDob = document.getElementById("<%=this.txtCalDobPopup.ClientID %>").value;
            var dob;
            if (document.all)
                dob = new Date(dtDob.replace('-', ' '));
            else
                dob = new Date(convertdate(dtDob));
            var admissiondate;
            if (document.all)
                admissiondate = new Date((document.getElementById("<%=this.txtcalAdmissionDate.ClientID %>").value.replace('-', ' ')));
            else
                admissiondate = new Date(convertdate(document.getElementById("<%=this.txtcalAdmissionDate.ClientID %>").value));
            var serverdate = document.getElementById("<%=this.hidServerDate.ClientID %>").value;
            var today = new Date(serverdate);
            if (dob > admissiondate) {
                document.getElementById(_clientcstmsg).errormessage = document.getElementById("<%=this.hidDateOfBirthGreaterValidation.ClientID %>").value
                args.IsValid = false;
                return true;
            }
            //            else if (admissiondate > today) {
            //                document.getElementById(_clientcstmsg).errormessage = document.getElementById("<%=this.hidDateOfAdmission.ClientID %>").value
            //                args.IsValid = false;
            //                return true;
            //            }
            args.IsValid = true;
            return false;
        }

        function IsValidJoiningDate(oSrc, args) {

            var dtJoiningDate = document.getElementById(_clienttxtJoiningDate).value;
            var JoiningDate;
            if (document.all)
                JoiningDate = new Date(dtJoiningDate.replace('-', ' '));
            else
                JoiningDate = new Date(convertdate(dtJoiningDate));
            if ((document.getElementById(_clientchkNewAddmission) == undefined) || (!document.getElementById(_clientchkNewAddmission).checked)) {
                if (!(CheckIfDateInAcademicYear(JoiningDate))) {
                    var dtYearStartDate = new Date(document.getElementById(_clienthidAcademicStartDate).value);
                    var dtYearEndDate = new Date(document.getElementById(_clienthidAcademicEndDate).value);
                    var strStartYear = getDateString(dtYearStartDate);
                    var strEndYear = getDateString(dtYearEndDate);
                    document.getElementById(_clientcustJoining).errormessage = document.getElementById("<%=this.hidDateOfJoining.ClientID %>").value + "(i.e. " + strEndYear + ").";
                    args.IsValid = false;
                    return true;
                }
            }
            else {
                if (!(CheckIfDateInAcademicYearForNewStud(JoiningDate))) {
                    var dtYearStartDate = new Date(convertvaliddate2(document.getElementById(_clienthidAcademicStartDate).value));
                    var dtYearEndDate = new Date(convertvaliddate2(document.getElementById(_clienthidAcademicEndDate).value));
                    var strStartYear = getDateString(dtYearStartDate);
                    var strEndYear = getDateString(dtYearEndDate);
                    document.getElementById(_clientcustJoining).errormessage = document.getElementById("<%=this.hidDateOfJoiningValidation.ClientID %>").value + "(i.e. " + strStartYear + " to " + strEndYear + ").";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function getDateString(obj) {

            var strDate = obj.getDate() + "-";
            var strMonth = parseInt(obj.getMonth());
            strMonth = months[strMonth];

            strDate = strDate + strMonth + "-";
            strDate = strDate + obj.getFullYear();
            return strDate;

        }
        function CheckIfDateInAcademicYear(dtObj) {
            var bReturn;

            var dtYearStartDate = new Date(document.getElementById(_clienthidAcademicStartDate).value.replace('-', ' ').replace('-', ' '));
            var dtYearEndDate = new Date(document.getElementById(_clienthidAcademicEndDate).value.replace('-', ' ').replace('-', ' '));

            if (dtObj > dtYearEndDate) {
                bReturn = false;
            }
            else {
                bReturn = true;
            }
            return bReturn;
        }

        function CheckIfDateInAcademicYearForNewStud(dtObj) {
            var bReturn;
            var dtYearStartDate = new Date(convertvaliddate2(document.getElementById(_clienthidAcademicStartDate).value));
            var dtYearEndDate = new Date(convertvaliddate2(document.getElementById(_clienthidAcademicEndDate).value));

            if (dtObj < dtYearStartDate || dtObj > dtYearEndDate) {
                bReturn = false;
            }
            else {
                bReturn = true;
            }
            return bReturn;
        }

        function CheckJoiningDate(CommonFieldNames) {
            document.getElementById(_clientlblErrorMsg).innerHTML = '';
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
            }
            if (validationResult == false) {
                return false;
            }
            var StudentSiblingNames = $get(_ClienthidStudentSiblingNames).value;

            //Ovewrite curr. student details to all his siblings
            if (StudentSiblingNames != "")
                IsOverwriteSiblingDetails(StudentSiblingNames, CommonFieldNames);


            var dtYearStartDate = new Date(convertdate(document.getElementById(_clienthidAcademicStartDate).value));

            var dtJoiningDate = document.getElementById(_clienttxtJoiningDate).value;
            var JoiningDate;
            if (document.all)
                JoiningDate = new Date(dtJoiningDate.replace('-', ' '));
            else
                JoiningDate = new Date(convertdate(dtJoiningDate));

            var dtOldJoiningDate = document.getElementById(_clienthidOldJoiningDate).value;
            var OldJoiningDate;
            if (document.all)
                OldJoiningDate = new Date(dtOldJoiningDate.replace('-', ' '));
            else
                OldJoiningDate = new Date(convertdate(dtOldJoiningDate));

            var serverdate = document.getElementById("<%=this.hidServerDate.ClientID %>").value; // document.getElementById(_clienthidServerDate).value;
            var today = new Date(convertdate(serverdate));
            var hidMode = document.getElementById(_clienthidMode).value;
            var AcademicYearStatus = $get("<%=this.hidAcademicYearStatus.ClientID%>").value
            if (hidMode.toUpperCase() == "NEW" && AcademicYearStatus == "") {
                if (JoiningDate <= today && JoiningDate > dtYearStartDate)
                    return window.confirm(document.getElementById("<%=this.hidAttendanceValidation.ClientID %>").value)
            }

            else {
                var HasAttendance = document.getElementById(_clienthidHasAttendance).value.toLowerCase();
                if (HasAttendance == "true") {
                    if (JoiningDate < OldJoiningDate && AcademicYearStatus == "") {
                        if (window.confirm(document.getElementById("<%=this.hidSyatemTillDateAttendance.ClientID %>").value)) {
                            var ObjBtn = document.getElementById(_clientbtnSave);
                            DisableButtons(ObjBtn);
                            return true;
                        }
                        else
                            return false;
                    }
                    if (JoiningDate > OldJoiningDate && AcademicYearStatus == "") {
                        if (window.confirm(document.getElementById("<%=this.hidSystemAttendance.ClientID %>").value)) {
                            var ObjBtn = document.getElementById(_clientbtnSave);
                            DisableButtons(ObjBtn);
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            var AcademicStartDate = new Date(document.getElementById(_clienthidAcademicStartDate).value);
            var AcademicEndDate = new Date(document.getElementById(_clienthidAcademicEndDate).value);
            if (JoiningDate < AcademicStartDate && JoiningDate > AcademicEndDate) {
                return true;
            }
            var ObjBtn = document.getElementById(_clientbtnSave);
            DisableButtons(ObjBtn);
            return true;
        }

        //This method is called when edited student has at least one sibling.
        function IsOverwriteSiblingDetails(StudentSiblingNames, CommonFieldNames) {
            // if (!window.confirm(CommonFieldNames + " details will be replace to the following sibling(s): \n" + StudentSiblingNames + ". \nDo you want to replace it?")) {
            // document.getElementById(_ClienthidIsOverwriteSiblingDetails).value = 1;
            //                return false;
            //  }
            // else
            // OpenSibblingPopup();         
            //            if ((OpenSibblingPopup())) {
            //                 
            //                document.getElementById(_ClienthidIsOverwriteSiblingDetails).value = 1;
            //                return false;
            //            }
            // document.getElementById(_ClienthidIsOverwriteSiblingDetails).value = 0;
            //            return false;
        }
        function DisableButtons(ObjBtn) {
            document.getElementById(_clientvalSumErrorMsg).style.dispaly = "none";
            if (ObjBtn == document.getElementById(_clientbtnCancel)) {
                if (document.getElementById(_clientbtnSave) != null)
                    document.getElementById(_clientbtnSave).disabled = true;
                if (document.getElementById(_clientbtnSaveNext) != null)
                    document.getElementById(_clientbtnSaveNext).disabled = true;
                if (ObjBtn.value != "Close" && document.getElementById(_clientbtnCancel) != null)
                    document.getElementById(_clientbtnCancel).disabled = true;
            }
            else {
                var isPageValid = true;

                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate();
                }
                if (isPageValid) {
                    if (document.getElementById(_clientbtnSaveNext) != null) {
                        if ($get("<%= this.hidStudentSiblingNames.ClientID %>").value.trim() != "") {
                            document.getElementById(_clientbtnSave).disabled = true;
                            document.getElementById(_clientbtnSaveNext).disabled = true;
                        }
                        document.getElementById(_clientbtnCancel).disabled = true;
                    }
                    else {
                        if ($get("<%= this.hidStudentSiblingNames.ClientID %>").value.trim() != "") {
                            document.getElementById(_clientbtnSave).disabled = true;
                            document.getElementById(_clientbtnCancel).disabled = true;
                        }
                    }
                }
            }
        }

        function CheckMobileNumberBlank(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
            if (document.getElementById(_schoolId).value != schoolIdSNS && sMobileNumber == "") {
                document.getElementById(_clientcst_MobileNumber1).errormessage = document.getElementById("<%=this.hidMobileNumberBlank.ClientID %>").value
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function RequiredMobileNumberValidation(oSrc, args) {
            var sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value;
            var sMobileNumber1 = document.getElementById(_sClienttxtMobilePhoneNumber2Id).value;
            if (document.getElementById(_schoolId).value == schoolIdSNS && (sMobileNumber == "" && sMobileNumber1 == "")) {
                document.getElementById(_clientcst_MobileNumber1and2).errormessage = document.getElementById("<%=this.hidMobileNumber1and2Blank.ClientID %>").value
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        var blanks = " \t\n\r";  // Ek whitespace chars
        function MobileNumberValidation(oSrc, args) {
            var sMobileNumber
            var iMobileNumber
            var MobileNumber1 = document.getElementById(oSrc.id)
            if (MobileNumber1.id == "ctl00_PopupMainBody_cst_MobileNumber") {
                sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumberId).value
                iMobileNumber = 1;
            }
            else {
                sMobileNumber = document.getElementById(_sClienttxtMobilePhoneNumber2Id).value
                iMobileNumber = 2;
            }
            sMobileNumber = stripLeadingTrailingBlanks(sMobileNumber)

            if (sMobileNumber.length < 10) {
                if (sMobileNumber != "" && iMobileNumber == 1) {
                    if (document.getElementById(_schoolId).value != schoolIdSNS)
                        document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileDigit.ClientID %>").value
                    else
                        document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMotherNumberDigit.ClientID %>").value
                    args.IsValid = false
                    return true
                }
                else if (sMobileNumber != "" && iMobileNumber == 2) {
                    if (document.getElementById(_schoolId).value != schoolIdSNS)
                        document.getElementById(_clientcst_MobileNumber2).errormessage = document.getElementById("<%=this.hidMobileNumber2Digit.ClientID %>").value
                    else
                        document.getElementById(_clientcst_MobileNumber2).errormessage = document.getElementById("<%=this.hidFatherNumberDigit.ClientID %>").value
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                }
            }
            else if (sMobileNumber.substring(0, 1) == '0') {
                if (iMobileNumber == 1) {
                    if (document.getElementById(_schoolId).value != schoolIdSNS)
                        document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMobileNumber1Zero.ClientID %>").value;
                    else
                        document.getElementById(_clientcst_MobileNumber).errormessage = document.getElementById("<%=this.hidMotherNumberZero.ClientID %>").value;
                }
                else {
                    if (document.getElementById(_schoolId).value != schoolIdSNS)
                        document.getElementById(_clientcst_MobileNumber2).errormessage = document.getElementById("<%=this.hidMobileNumber2Zero.ClientID %>").value;
                    else
                        document.getElementById(_clientcst_MobileNumber2).errormessage = document.getElementById("<%=this.hidFatherNumberZero.ClientID %>").value;
                }
                args.IsValid = false;
                return true;
            }
            args.IsValid = true
            return false
        }
        function checkRollNo(oSrc, args) {
            var rollNo = document.getElementById(_clientRollNo).value;
            if (rollNo == "")
                document.getElementById(_clientcstRollNo).errormessage = document.getElementById("<%=this.hidRollNumberBlank.ClientID %>").value;
            else if (parseInt(rollNo / 1) == 0)
                document.getElementById(_clientcstRollNo).errormessage = document.getElementById("<%=this.hidRollNumberZero.ClientID %>").value;
            else {
                args.IsValid = true;
                return false;
            }
            args.IsValid = false;
            return true;
        }

        function ReqdOtherOccupation(oSrc, args) {
            args.IsValid = true;
            return false;
        }

        function AssignSubCaste(obj) {
            document.getElementById("ctl00_PopupMainBody_hidSubCasteId").value = obj.value;
        }

        function PinCodeValidation(oSrc, args) {

            var sPIN = document.getElementById('ctl00_PopupMainBody_txtPIN').value;
            sPIN = stripLeadingTrailingBlanks(sPIN);
            if (sPIN.length == 0) {
                document.getElementById('ctl00_PopupMainBody_txtPIN').value = "";
                document.getElementById('ctl00_PopupMainBody_cst_PIN').ErrorMessage = document.getElementById("<%=this.hidPinBlank.ClientID %>").value;
                args.IsValid = true;
                return false;
            }

            if (sPIN.length != 6) {

                document.getElementById('ctl00_PopupMainBody_cst_PIN').ErrorMessage = document.getElementById("<%=this.hidPinCodeDigit.ClientID %>").value;
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;

            return false;
        }
        function test() {


            var xmlHttpObj = CreateHTTPReqObj();

            if (xmlHttpObj) {
                var cntrl = document.getElementById("ctl00_PopupMainBody_txtCasteAndSubcaste");
                var iCasteId = cntrl.value;
                var url = "../Ajax.ashx?CasteId=" + iCasteId + "&task=Caste";

                noCache(url);

                xmlHttpObj.open("GET", url, true);

                xmlHttpObj.onreadystatechange = function () {
                    if (xmlHttpObj.readyState == 4) {
                        if (xmlHttpObj.status == 200) {
                            var optionText = xmlHttpObj.responseText;
                            var cntrlSubCaste = document.getElementById("ctl00_PopupMainBody_cmbCategory");
                            cntrlSubCaste.options.length = 0;
                            var htmlCode = document.createElement("option");
                            htmlCode.text = "--Select--";
                            htmlCode.value = "0";

                            cntrlSubCaste.options.add(htmlCode);

                            if (optionText != "") {


                                var sArray = optionText.split("@@@");
                                var cnt = sArray.length;



                                for (i = 0; i < cnt; i++) {
                                    var soption = sArray[i].split("###");

                                    var sText = soption[1];
                                    var sValue = soption[0];

                                    var htmlCode = document.createElement("option");
                                    htmlCode.text = sText;
                                    htmlCode.value = sValue;
                                    cntrlSubCaste.options.add(htmlCode);
                                }
                            }
                        }
                    }
                }
                xmlHttpObj.send(null);

            }
            else {
            }
        }
        function CheckFileType(sFileName) {
            var bIsValid;

            if (sFileName != "") {

                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG") {

                    bIsValid = true;
                }
                else {
                    bIsValid = false;
                }
            }
            else {
                bIsValid = false;
            }
            return bIsValid;
        }
        function ValidateLogo(aSrc, args) {

            var myImage = new Image();
            myImage.src = document.getElementById(_clientFileUploadLogo).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (myImage.src == "") {
                document.getElementById(_clientCstValidateLogo).errormessage = document.getElementById("<%=this.hidSchoolLogo.ClientID %>").value;
            }
            if (CheckFileType(myImage.src))//if file type is valid
            {

            }
            else//if file type is not valid
            {
                document.getElementById(_clientCstValidateLogo).errormessage = document.getElementById("<%=this.hidInvalidFileFormat.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            if (document.getElementById(_clientFileUploadLogo).files[0].size > 1048576) {
                aSrc.errormessage = "Photo file size should not be more than 1 MB.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;

        }

        function CheckFileTypeForAadharNumber(sFileName) {
            var bIsValid;
            if (sFileName != "") {

                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF") {

                    bIsValid = true;
                }
                else {
                    bIsValid = false;
                }
            }
            else {
                bIsValid = false;
            }
            return bIsValid;
        }

        function ValidateAadharScanCopy(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuAadharNumber).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForAadharNumber(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientfuAadharNumber).files[0].size <= 3145728) {
                }
                else {
                    document.getElementById(_cstValidateAadharScanCopy).errormessage = "File size should not be greater than 3 MB.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_cstValidateAadharScanCopy).errormessage = "File type should be between .pdf, .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;

        }

        function CheckFileTypeForFamilyPhoto(sFamilyFileName) {
            var bIsValid;
            var PPschoolId = document.getElementById(_clienthidPPSchoolId).value;
            var SchoolId = document.getElementById(_schoolId).value;
            if (SchoolId != PPschoolId) {
                if (sFamilyFileName != "") {

                    if (sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP") {

                        bIsValid = true;
                    }
                    else {
                        bIsValid = false;
                    }
                }
                else {
                    bIsValid = false;
                }
            }
            else {
                if (sFamilyFileName != "") {

                    if (sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFamilyFileName.substr(sFamilyFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG") {

                        bIsValid = true;
                    }
                    else {
                        bIsValid = false;
                    }
                }
                else {
                    bIsValid = false;
                }
            }
            return bIsValid;
        }

        function ValidateFamilyPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientFuFamilyPhoto).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForFamilyPhoto(myImage.src))//if file type is valid
            {
                if (document.getElementById(_clientFuFamilyPhoto).files[0].size > 3145728) {
                    document.getElementById(_cstValidateFamilyPhoto).errormessage = "Family Photo file size should not be greater than 3 MB.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                document.getElementById(_cstValidateFamilyPhoto).errormessage = "Family Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;

        }

        function ValidateFatherPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientFUAdditionalFatherPhoto).value;
            var PPschoolId = document.getElementById(_clienthidPPSchoolId).value;
            var SchoolId = document.getElementById(_schoolId).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForFamilyPhoto(myImage.src))//if file type is valid
            {
                if (SchoolId != PPschoolId) {
                    if (document.getElementById(_clientFUAdditionalFatherPhoto).files[0].size > 3145728) {
                        document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Father Photo file size should not be greater than 3 MB.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else if (document.getElementById(_clientFUAdditionalFatherPhoto).files[0].size > 81920) {
                    document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Father Photo file size should not be greater than 81kb.";
                    args.IsValid = false;
                    return true;
                }
            }
            else//if file type is not valid
            {
                if (SchoolId != PPschoolId) {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Father Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Father Photo file type should be between .jpg, .jpeg";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFatherDOB(aSrc, args) {
            var dobpopupDate = new Date(convertdate(document.getElementById(_clienttxtCalDobPopup).value))
            var FatherDOB = new Date(convertdate(document.getElementById(_clienttxtAdditionalFatherDOB).value))

            if (dobpopupDate <= FatherDOB) {
                document.getElementById(_clientcstValidateFatherDOB).errormessage = "Father DOB should be less than students DOB."
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateMotherPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientfuAdditionalMotherPhoto).value;
            var PPschoolId = document.getElementById(_clienthidPPSchoolId).value;
            var SchoolId = document.getElementById(_schoolId).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForFamilyPhoto(myImage.src))//if file type is valid
            {
                if (SchoolId != PPschoolId) {
                    if (document.getElementById(_clientfuAdditionalMotherPhoto).files[0].size > 3145728) {
                        document.getElementById(_clientcstValidateMotherPhoto).errormessage = "Mother Photo file size should not be greater than 3 MB.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else {
                    if (document.getElementById(_clientFUAdditionalFatherPhoto).files[0].size > 81920) {
                        document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Mother Photo file size should not be greater than 81kb.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            else//if file type is not valid
            {
                if (SchoolId != PPschoolId) {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Mother Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Mother Photo file type should be between .jpg, .jpeg";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMotherDOB(aSrc, args) {
            var dobpopupDate = new Date(convertdate(document.getElementById(_clienttxtCalDobPopup).value))
            var MotherDOB = new Date(convertdate(document.getElementById(_clienttxtAdditionalMotherDOB).value));

            if (dobpopupDate <= MotherDOB) {
                document.getElementById(_clientcstValidateMotherDOB).errormessage = "Mother DOB should be less than students DOB."
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateGuardianPhoto(aSrc, args) {
            var myImage = new Image();
            myImage.src = document.getElementById(_clientFUAdditionalGuardianPhoto).value;
            var PPschoolId = document.getElementById(_clienthidPPSchoolId).value;
            var SchoolId = document.getElementById(_schoolId).value;

            var iWidth = myImage.width
            var iHeight = myImage.height

            if (CheckFileTypeForFamilyPhoto(myImage.src))//if file type is valid
            {
                if (SchoolId != PPschoolId) {
                    if (document.getElementById(_clientFUAdditionalGuardianPhoto).files[0].size > 3145728) {
                        document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Local Guardian Photo file size should not be greater than 3 MB.";
                        args.IsValid = false;
                        return true;
                    }
                }
                else {
                    if (document.getElementById(_clientFUAdditionalFatherPhoto).files[0].size > 81920) {
                        document.getElementById(_clientcstValidateFatherPhoto).errormessage = "Local Guardian Photo file size should not be greater than 81kb.";
                        args.IsValid = false;
                        return true;
                    }
                }
            }
            else//if file type is not valid
            {
                if (SchoolId != PPschoolId) {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Local Guardian Photo file type should be between .jpg, .jpeg, .png and .bmp.";
                    args.IsValid = false;
                    return true;
                }
                else {
                    document.getElementById(_clientcstValidateGuardianPhoto).errormessage = "Local Guardian Photo file type should be between .jpg, .jpeg";
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateAnniversaryDate(aSrc, args) {
            var dobpopupDate = new Date(convertdate(document.getElementById(_clienttxtCalDobPopup).value))
            var FatherDOB = new Date(convertdate(document.getElementById(_clienttxtAdditionalFatherDOB).value));
            var MotherDOB = new Date(convertdate(document.getElementById(_clienttxtAdditionalMotherDOB).value));
            var AnniversaryDate = new Date(convertdate(document.getElementById(_clienttxtAdditionalAnniversaryDate).value));

            if (dobpopupDate <= FatherDOB) {
                document.getElementById(_clientcstAnniversaryDate).errormessage = "Marriage Anniversary date should be grater than Father DOB."
                args.IsValid = false;
                return true;
            }
            else if (dobpopupDate <= MotherDOB) {
                document.getElementById(_clientcstAnniversaryDate).errormessage = "Marriage Anniversary date should be grater than Mother DOB."
                args.IsValid = false;
                return true;
            }
            else if (dobpopupDate <= AnniversaryDate) {
                document.getElementById(_clientcstAnniversaryDate).errormessage = "Marriage Anniversary date should be less than student DOB."
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ValidateRTECategory(aSrc, args) {

            if ($get(_sClientrRTECategory).style.visibility == "visible") {

                if ($get(_sCliencmbRTECategory).value == 0) {
                    aSrc.errormessage = document.getElementById("<%=this.hidRTECategorySelected.ClientID %>").value;
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateRTEIncome(aSrc, args) {
            var _clientTRAmount = "<%=this.trAmount.ClientID %>"
            var _clienttxtAnnualIncome = "<%=this.txtAnnualIncome.ClientID %>"
            _clienthidRestrictAnnualIncomeForRTE = "<%=this.hidRestrictAnnualIncomeForRTE.ClientID %>"

            if ($get(_clientTRAmount).style.visibility == "visible" && $get(_clienthidRestrictAnnualIncomeForRTE).value == "1") {

               if ($get(_clienttxtAnnualIncome).value == '' || $get(_clienttxtAnnualIncome).value == 0) {
                   aSrc.errormessage = 'Annual Income should not be blank or zero if selected RTE category is Weaker Section.';
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        

        function HideMessage() {
            if ($get("<%=this.txtRegNo.ClientID %>").value.trim() == "") {
                if ($get("ctl00_PopupMainBody_lblErrorMsg").innerHTML.indexOf("Registration") != -1) {
                    $get("ctl00_PopupMainBody_lblErrorMsg").style.visibility = "hidden"
                }
            }
        }
                
        function EnableDisabledCmb() {
            _sClientchkStaffKid = "<%=this.chkIsStaffKid.ClientID %>";
            _sClientcmbStaffKid = "<%=this.ddlFeeRule.ClientID %>";
            _sClienthidRuleId = "<%=this.hidRuleId.ClientID %>";

            _clientddlUserRole = "<%=this.ddlUserRole.ClientID %>"
            _clientddlUserName = "<%=this.ddlUserName.ClientID %>"

            var iIndex = document.getElementById(_sClienthidRuleId).value;

            if (!$get(_sClientchkStaffKid).checked) {              
                $get(_clientddlUserRole).value = 0;
                $get(_clientddlUserName).value = 0;
            }


//            if ((document.getElementById(_sClientchkStaffKid) != null) && (document.getElementById(_sClientcmbStaffKid) != null)) {
//                var bFlag = document.getElementById(_sClientchkStaffKid).checked;
//                document.getElementById(_sClientcmbStaffKid).disabled = !bFlag;
//                if (bFlag) {
//                    document.getElementById(_sClientcmbStaffKid).selectedIndex = iIndex;
//                }
//                else
//                    document.getElementById(_sClientcmbStaffKid).selectedIndex = 0;
//            }
        }

        var OtherCategory = 10;
        

        function SetValues() {
            _sClientchkStaffKid = "<%=this.chkIsStaffKid.ClientID %>";
            _sClientrRTECategory = "<%=this.trRTECatrgory.ClientID %>";
            _clientcmbCategory = "<%=this.cmbCategory.ClientID %>";
            _clientcmbRTECategory = "<%=this.cmbRTECategory.ClientID %>";
            _clientTRAmount = "<%=this.trAmount.ClientID %>"
            _clienttrRTEFormNo = "<%=this.trRTEFormNo.ClientID %>"

            if (document.getElementById(_sClientchkStaffKid) != null)
                document.getElementById(_sClientchkStaffKid).disabled = false;
            $get(_sClientrRTECategory).style.visibility = "hidden";
            $get(_sClientrRTECategory).style.display = "none";
            $get(_clientcmbCategory).value = OtherCategory;
            $get(_clientcmbCategory).disabled = false;

            $get(_clientTRAmount).style.visibility = "hidden";
            $get(_clientTRAmount).style.display = "none";

            $get(_clienttrRTEFormNo).style.visibility = "hidden";
            $get(_clienttrRTEFormNo).style.display = "none";

        }


        function EnableDisabledStaffKid() {

            _sClientchkIsRTEApplicable = "<%=this.chkIsRTEApplicable.ClientID %>";
            _sClientchkStaffKid = "<%=this.chkIsStaffKid.ClientID %>";
            _sClientcmbStaffKid = "<%=this.ddlFeeRule.ClientID %>";
            _sClientrRTECategory = "<%=this.trRTECatrgory.ClientID %>";
            _clientcmbCategory = "<%=this.cmbCategory.ClientID %>";
            _clientcmbRTECategory = "<%=this.cmbRTECategory.ClientID %>";
            _clientTRAmount = "<%=this.trAmount.ClientID %>"
            _clienttrRTEFormNo = "<%=this.trRTEFormNo.ClientID %>"
            var OtherCategory = 9;


            if (document.getElementById(_sClientchkIsRTEApplicable) != null && document.getElementById(_sClientchkIsRTEApplicable).checked) {
                if (!window.confirm(document.getElementById("<%=this.hidRTEStudentSelected.ClientID %>").value)) {
                    document.getElementById(_sClientchkIsRTEApplicable).checked = false;
                    if (document.getElementById(_sClientchkStaffKid) != null)
                        document.getElementById(_sClientchkStaffKid).disabled = false;
                    $get(_sClientrRTECategory).style.visibility = "hidden";
                    $get(_sClientrRTECategory).style.display = "none";

                    $get(_clientTRAmount).style.visibility = "hidden";
                    $get(_clientTRAmount).style.display = "none";

                    $get(_clienttrRTEFormNo).style.visibility = "hidden";
                    $get(_clienttrRTEFormNo).style.display = "none";
                }
                else {
                    if ((document.getElementById(_sClientchkIsRTEApplicable) != null) && (document.getElementById(_sClientrRTECategory) != null)) {
                        var bFlag = document.getElementById(_sClientchkIsRTEApplicable).checked;
                        if (bFlag) {
                            if ((document.getElementById(_sClientchkStaffKid) != null) && (document.getElementById(_sClientcmbStaffKid) != null)) {
                                document.getElementById(_sClientchkStaffKid).disabled = bFlag;
                                document.getElementById(_sClientchkStaffKid).checked = false;
                                document.getElementById(_sClientcmbStaffKid).selectedIndex = 0;
                                document.getElementById(_sClientcmbStaffKid).disabled = true;
                                $get(_sClientrRTECategory).style.visibility = "visible";
                                $get(_sClientrRTECategory).style.display = "";
                                $get(_clientcmbRTECategory).value = 0;

//                                $get(_clientTRAmount).style.visibility = "visible";
//                                $get(_clientTRAmount).style.display = "";

                                $get(_clienttrRTEFormNo).style.visibility = "visible";
                                $get(_clienttrRTEFormNo).style.display = "";
                            }
                            else {
                                $get(_sClientrRTECategory).style.visibility = "visible";
                                $get(_sClientrRTECategory).style.display = "";
                                $get(_clientcmbRTECategory).value = 0;

//                                $get(_clientTRAmount).style.visibility = "visible";
//                                $get(_clientTRAmount).style.display = "";

                                $get(_clienttrRTEFormNo).style.visibility = "visible";
                                $get(_clienttrRTEFormNo).style.display = "";
                            }
                        }
                        else
                            SetValues();
                    }
                }
            }
            else
                SetValues();

        }

        function ConfirmRemove() {
            var bResult = true;
            if (!window.confirm(document.getElementById("<%=this.hidRemoveThisPhoto.ClientID %>").value)) {
                bResult = false;
            }
            return bResult;
        }


        function ResetSiblingListViewCheckBoxes() {

            $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox][id$=ChkSelectAllSiblings]").attr('checked', false);
            $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox][id$=_ChkSelectSiblingsSingle]").attr('checked', false);
        }
        function ResetListViewCheckBoxes() {
            $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox][id$=chkIsApplicable]").attr('checked', true);
            $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox][id$=_ChkSelect]").attr('checked', false);
        }

        function CheckAllUncheckAllsSiblings() {

            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientChkAllSiblings).checked

            if (checkAll) {
                $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox]").attr('checked', checkAll);
            }
            else {
                $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox][id$=ChkSelectSiblingsSingle]").attr('checked', checkAll);
            }
        }

        function SetSiblingListViewCheckBoxes() {

            $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox][id$=ChkSelectAllSiblings]").attr('checked', true);
            $("#<%=lstvwSiblingsDetails.ClientID %>_tblSiblingsInfo input[type=checkbox][id$=_ChkSelectSiblingsSingle]").attr('checked', true);

        }

        function CheckAllUncheckAllsIsApplicableCheckBox() {
            var checkAll;
            if (document.getElementById(_ClientChkAll) != null)
                checkAll = document.getElementById(_ClientIsApplicableAll).checked
            if (!checkAll) {
                $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox]").attr('checked', checkAll);
            }
            else {
                $("#<%=lstvwConfiguredDocument.ClientID %>_tblTermInfo input[type=checkbox][id$=chkIsApplicable]").attr('checked', checkAll);

            }

        }
        function SetIsApplicableSatus(obj, iRowNo) {
            if (!obj.checked) {
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_ChkSelect").checked = false;
            }
        }

        function SetIsSubmittedSatus(obj, iRowNo) {
            if (obj.checked)
                document.getElementById(_clientListViewId + "_ctrl" + iRowNo + "_chkIsApplicable").checked = true;
        }

        function ShowSiblingDetails() {
            var queryString = document.getElementById(_clienthidEncryptedString).value;
            var bResult = true;
            if (document.getElementById(_clienthidYearWiseStudentId).value == "")
                if (!window.confirm(document.getElementById("<%=this.hidReplaceStudentWithSibling.ClientID %>").value))
                    document.getElementById(_clienthidOverwrite).value = "N";
                else
                    document.getElementById(_clienthidOverwrite).value = "Y";
            window.open('../Teacher/StudentSiblingDetailsUI.aspx?' + queryString, '_blank', 'scrollbars=yes,resizable=yes,top=0,left=0,width=1000,height=700').focus();

        }

        function ShowAchievementDetails() {
            var queryString = document.getElementById(_clienthidQueryValue).value;
            window.open('StudentAchievementPopUp.aspx?' + queryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=700').focus();

        }

        //This method is called only when new student is added
        function GetSiblingStudentIds(StudentIds, SchoolwiseStudentId, SiblingStudentName, iCnt) {
            var s = $get(_clienthidSiblingStudentId).value = StudentIds + ($get(_clienthidSiblingStudentId).value == StudentIds ? "," : "");
            document.getElementById(_clienthidSchoolwiseStudentId).value = SchoolwiseStudentId;
            document.getElementById(_ClienthidStudentSiblingNames).value = SiblingStudentName;
            document.getElementById(_ClienthidIsSiblingAdded).value = iCnt
            if (SchoolwiseStudentId != '') {
                __doPostBack(document.getElementById(_clienthidSiblingStudentId).name, '')
                __doPostBack(document.getElementById(_ClienthidStudentSiblingNames).name, '')
            }
        }

        function OverwriteSiblingDetails() {
            return document.getElementById(_clienthidOverwrite).value;
        }

        //This method is called only "Has Sibling" checkbox is checked
        function SiblingValidation(oSrc, args) {
            var HasSibling = document.getElementById(_ClientchkHasSibling)
            var sIsSiblingAdded = document.getElementById(_ClienthidIsSiblingAdded).value
            if (HasSibling != "undefined" && HasSibling != null) {
                if (HasSibling.checked && (sIsSiblingAdded == "0" || sIsSiblingAdded == "")) {
                    oSrc.errormessage = document.getElementById("<%=this.hidAtLeastOneSibling.ClientID %>").value;
                    args.IsValid = false
                    return true
                }
            }
            args.IsValid = true
            return false
        }
        //To show overwite popup
        //This method is called only when any existjing student is edited.
        function UpdateHidVariablesIfSiblingAdded(iCnt) {
            document.getElementById(_ClienthidIsSiblingAdded).value = iCnt
            __doPostBack(document.getElementById(_ClienthidIsSiblingAdded).name, '')
        }
        //Update all Siblings name (This method is called after popup get closed.)
        function UpdateSibliStudentName(SiblingStudentName, IsDeleted) {
            document.getElementById(_ClienthidStudentSiblingNames).value = SiblingStudentName;
            __doPostBack(document.getElementById(_ClienthidStudentSiblingNames).name, '')
            if (IsDeleted == 'Y')
                document.getElementById(_clienthidSiblingStudentId).value = "";
        }

        function blockNonNumbersAndDecimalOnFirstPlace(obj, e, allowDecimal, allowNegative) {
            var key;
            var isCtrl = false;
            var keychar;
            var reg;
            if (window.event) {
                key = e.keyCode;
                isCtrl = window.event.ctrlKey
            }
            else if (e.which) {
                key = e.which;
                isCtrl = e.ctrlKey;
            }
            if (isNaN(key)) return true;
            keychar = String.fromCharCode(key);
            // check for backspace or delete, or if Ctrl was pressed
            if (key == 8 || isCtrl) {
                return true;
            }
            reg = /\d/;
            var isFirstN = allowNegative ? keychar == '-' && obj.value.indexOf('-') == -1 : false;
            var isFirstD = allowDecimal ? keychar == '.' && obj.value.indexOf('.') == -1 && obj.value.trim() != "" : false;
            return isFirstN || isFirstD || reg.test(keychar);
        }

        _clienthidItemCount = "<%=this.hidItemCount.ClientID %>";

        function UpdateFileUploadCount(ItemCount) {
            document.getElementById(_clienthidItemCount).value = ItemCount;
            __doPostBack(document.getElementById(_clienthidItemCount).name, '')
        }

        function OpenPopup(querystring) {
            window.open('../Payroll/InvestmentDocumentPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500').focus();
            return false;
        }

        function OpenPopup1() {
            window.alert(document.getElementById("<%=this.hidUploadDocumet.ClientID %>").value);
        }

        //EnableDisableCategoryCombo();
        function EnableDisableCategoryCombo() {
            _clientcmbRTECategory = "<%=this.cmbRTECategory.ClientID %>";
            _clientcmbCategory = "<%=this.cmbCategory.ClientID %>";
            var _clientTRAmount = "<%=this.trAmount.ClientID %>"
            var EcoBackwardCategory = 1;

            if ($get(_clientcmbRTECategory) != null && $get(_clientcmbCategory) != null) {

                if ($get(_clientcmbRTECategory).value != EcoBackwardCategory && $get(_clientcmbRTECategory).value != 0) {
                    $get(_clientcmbCategory).value = $get(_clientcmbRTECategory).value;
                }
                else {
                    $get(_clientcmbCategory).value = OtherCategory;
                    $get(_clientcmbCategory).disabled = false;
                }

                if ($get(_clientcmbRTECategory).value == "2") {
                    $get(_clientTRAmount).style.visibility = "visible";
                    $get(_clientTRAmount).style.display = "";
                    $get(_clientTRAmount).val('')
                }
                else {
                    $get(_clientTRAmount).style.visibility = "hidden";
                    $get(_clientTRAmount).style.display = "none";
                }

            }

        }

        function OpenWebcamPopup(sQueryString) {
            window.open('../Common/WebcamNewPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400').focus();
            return true;
        }

        function UpdateHiddenField() {
            $get(_clienthidIsPhotoCaptured).value = "Y";
        }

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function CheckAllUncheckAlls() {
            if (document.getElementById(_ClientChkAll) != null)
                var checkAll = document.getElementById(_ClientChkAll).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwMonths + "_ctrl" + iRowCount + "_ChkSelect")
            }
        }

        function CheckAllUncheckAllsSiblingsN() {
            if (document.getElementById(_ClientChkAllSiblings) != null)
                var checkAll = document.getElementById(_ClientChkAllSiblings).checked
            var chk
            var iRowCount = 0
            chk = document.getElementById(_clientlstvwSiblingss + "_ctrl" + iRowCount + "_ChkSelectSiblings")
            while (chk != null) {
                chk.checked = checkAll
                iRowCount = iRowCount + 1
                chk = document.getElementById(_clientlstvwSiblingss + "_ctrl" + iRowCount + "_ChkSelectSiblings")
            }
        }

        function OpenSiblingPopup() {
            // alert(CommonFieldNames)
            SetSiblingListViewCheckBoxes();
            document.getElementById(_ClienthidIsOverwriteSiblingDetails).value = 0;
            _clientdivTemplates = "<%=this.divSetting.ClientID %>"
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divSetting.ClientID %>").style
            var cssstabletyle = $get("<%=this.tblTop.ClientID %>").style
            var pageWidth = window.screen.width
            var pageHeight = 400
            var left = parseInt((pageWidth / 4.5))
            var top = parseInt((pageHeight / 1.5))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.visibility = "visible"
            //cssstabletyle.visibility = "hidden";
            cssstyle.display = "block"

        }

        function ValidateScreen() {
            var iCommonField = $get("<%= this.hidCommonFieldNames.ClientID %>")
            var iSiblingName = $get("<%= this.hidStudentSiblingNames.ClientID %>")
            var Result = CheckJoiningDate(iCommonField.value)
            if (Result == true) {
                if (iSiblingName.value.trim() != "") {
                    OpenSiblingPopup()
                    return false;
                }
                else
                    return true;
            }
            else
                return false;
        }

        //This function is used to inform user about standardwise DOB information.
        function confirmDOB() {


            var msg = document.getElementById(_clienthidDOBConfirmationMsg).value;

            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
            }
            if (validationResult == false) {
                return false;
            }

            if (msg != "") {

                var minDate = document.getElementById(_clienthidhidMinDOB).value;
                var maxDate = document.getElementById(_clienthidhidMaxDOB).value;
                var dobpopupDate = document.getElementById(_clienttxtCalDobPopup).value;

                minDate = new Date(minDate.replace('-', ' '));
                maxDate = new Date(maxDate.replace('-', ' '));

                if (document.all)
                    dobpopupDate = new Date(dobpopupDate.replace('-', ' '));
                else
                    dobpopupDate = new Date(convertdate(dobpopupDate));

                if (!(minDate <= dobpopupDate && dobpopupDate <= maxDate) || (dobpopupDate > maxDate)) {
                    if (confirm(msg) == true) {
                        return ValidateScreen()
                    }
                    else {
                        return false;
                    }
                }
                else
                    return ValidateScreen()
            }
            else {
                return ValidateScreen()
            }
        }
        function ConfirmAction() {
            var count = 0
            var listView = document.getElementById('<%= lstvwSiblingsDetails.FindControl("tblSiblingsInfo").ClientID %>');
            for (var i = 0; i < listView.rows.length; i++) {
                var inputs = listView.rows[i].getElementsByTagName('input');
                for (var j = 0; j < inputs.length; j++) {
                    if (inputs[j].type == "checkbox" && inputs[j].checked == true) {
                        count = 1
                        break;
                    }
                }
            }
            if (count == 0) {
                alert("At least one detail should be selected to update in the sibling profile.");
                return false
            }
            else
                return true
        }
        
        function HidePopup() {

            $get("<%=this.divSetting.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divSetting.ClientID %>").style.display = "none"
            return false
        }



        function EmailValidation(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtEmailId).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);

            // If email is not blank then validate for valid email address.
            if (!isEmpty(sEmail) && !isEmail(sEmail)) {
                oSrc.errormessage = document.getElementById("<%=this.hidEmailValidation.ClientID %>").value;
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function EmailValidationAdditionalFatherEmail(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtAdditionalFatherEmail).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);

            // If email is not blank then validate for valid email address.
            if (!isEmpty(sEmail) && !isEmail(sEmail)) {
                //oSrc.errormessage = document.getElementById("<%=this.hidAdditionalFatherEmailValidation.ClientID %>").value;
                oSrc.errormessage = "Father's E-mail address should be in valid format. (for example: 'John.Smith @ yahoo.com')";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function EmailValidationAdditionalMotherEmail(oSrc, args) {
            var sEmail = document.getElementById(_clienttxtAdditionalMotherEmail).value;
            sEmail = stripLeadingTrailingBlanks(sEmail);

            // If email is not blank then validate for valid email address.
            if (!isEmpty(sEmail) && !isEmail(sEmail)) {
                //oSrc.errormessage = document.getElementById("<%=this.hidAdditionalMotherEmailValidation.ClientID %>").value;
                oSrc.errormessage = "Mother's E-mail address should be in valid format. (for example: 'John.Smith @ yahoo.com')";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function MotherNameValidation(oSrc, args) {

            if (ShowMessage("txtMotherName", _clienttxtMotherName)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValMotherName.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidatecasteSubcaste(oSrc, args) {

            if (ShowMessage("txtCasteAndSubcaste", _clienttxtCasteAndSubcaste)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValCaste.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateBirthPlace(oSrc, args) {
            if (ShowMessage("txtBirthPlace", _clienttxtBirthPlace)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValBirthPlace.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateNationality(oSrc, args) {
            if (ShowMessage("txtNationality", _clienttxtNationality)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValNationality.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMotherTongueLanguage(oSrc, args) {
            if (ShowMessage("txtMotherTongue", _clienttxtMotherTongue)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValMotherToungue.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateLastSchoolName(oSrc, args) {
            if (ShowMessage("txtLastSchoolName", _clienttxtLastSchoolName)) {
                oSrc.errormessage = document.getElementById("<%=this.hidValLastSchoolName.ClientID %>").value
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function CheckIsDayBoardingFeePaid() {
            var IsFeePaid = document.getElementById(_clienthidDPISIsFeePaid).value;
            var chk = "<%=chkIsDayBoarding.ClientID %>"
            if (IsFeePaid == "Y" && $('#' + chk).is(':checked') == false) {
                alert('Day Boarding fee Is already paid by this student so you cannot remove this option.');
                $("#" + chk).prop("checked", true);
            }
        }

        function ShowMessage(field, clientId) {
            var mandatoryFields = document.getElementById(_clienthidMandatoryFields).value
            var fields = mandatoryFields.split(',')
            for (var index = 0; index < fields.length; index++) {
                if (fields[index].match(field) != null) {
                    if (document.getElementById(clientId).value.trim() == "") {
                        return true;
                    }
                }
            }
            return false;
        }

        $(document).ready(function () {
            SetDate();
        });

        $('[id*=CalDobPopup]').change(function () {        
            SetDate();
        });

        function SetDate() {        
            //Finding Current Date
            var age;
            var curDate = new Date(document.getElementById(_clienthidCurrentDate).value);
            var curr_year = curDate.getFullYear();
            var curr_month = curDate.getMonth() + 1;
            var Curr_Date = curDate.getDate();
            //Finding DOB
            var dt1 = document.getElementById("<%=this.txtCalDobPopup.ClientID %>").value;
            if (dt1 != '') {
                var dob;
                if (document.all)
                    dob = new Date(dt1.replace('-', ' '));
                else
                    dob = new Date(convertdate(dt1));
                var dob_Year = dob.getFullYear();
                var dob_Month = dob.getMonth() + 1;
                var dob_Date = dob.getDate();
                //Calculate Year and Month For display age
                var Year = curr_year - dob_Year;
                var Month = curr_month - dob_Month;
                var DateCount = Curr_Date - dob_Date;
                if (DateCount < 0) {
                    Month = Month - 1;
                }
                if (Month < 0) {
                    Year = Year - 1;
                    Month = Math.abs(Month);
                    Month = 12 - Month;
                }

                if (Year < 0) {
                    age = "- Year(s) - Month(s)";
                }
                else {
                    age = Year.toString() + " Year(s) " + Month.toString() + " Month(s)";
                }

            }
            else {
                age = "- Year(s) - Month(s)";
            }
            var month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

            $('#<%=lblAge.ClientID%>').html(age + " till " + curDate.getDate() + " " + month[curDate.getMonth()] + " " + curDate.getFullYear());
        }


        function ValidateLanguage(oSrc, args) {
            var index = 0
            var subjectGroups = $("#" + _clienthidSubjectGroupIds).val();

            if (subjectGroups != "") {
                var Ids = subjectGroups.split("$")

                var secondLangSubjectid = $("#" + _clientddlSecondLanguage).val()
                var thirdLangSubjectId = $("#" + _clientcmbThirdLanguage).val()

                var secondLangGroupId = 0
                var thirdLangGroupId = 0

                for (var k = 0; k < Ids.length; k++) {
                    if (parseInt(Ids[k].split(",")[0]) == parseInt(secondLangSubjectid)) {
                        secondLangGroupId = parseInt(Ids[k].split(",")[1])
                        break;
                    }
                }

                for (var j = 0; j < Ids.length; j++) {
                    if (parseInt(Ids[j].split(",")[0]) == parseInt(thirdLangSubjectId)) {
                        thirdLangGroupId = parseInt(Ids[j].split(",")[1])
                        break;
                    }
                }

                if (parseInt(secondLangSubjectid) != 0 && parseInt(thirdLangSubjectId) != 0 && secondLangGroupId == thirdLangGroupId) {
                    oSrc.errormessage = "Selected Second and Third Language should not be from same subject group.";
                    args.IsValid = false
                    return true;
                }
            }

            args.IsValid = true
            return false;
        }

        function ConfirmDelete() {
            var bResult = true;
            if (!window.confirm("Are you sure you want to delete this photo?")) {
                bResult = false;
            }
            return bResult;
        }

        function ValidateFeeCategory(oSrc, args) {            
            var IsAaryanSchools = $("#" + _clienthidIsAaryanSchool).val();
            var categoryID = document.getElementById(_clientcmbFeeCategory).value;

            if (IsAaryanSchools == "Y" && categoryID == 0) {
                oSrc.errormessage = "Fee Category should be selected.";
                args.IsValid = false
                return true;
            }
            args.IsValid = true
            return false;
        }
        function CheckStream(oSrc, args) {

            if (document.getElementById(_clientStream) != null) {
                var sStream = document.getElementById(_clientStream).value;

                if (document.getElementById(_schoolId).value == schoolIdSNS && (sStream == 0)) {
                
                    oSrc.errormessage = "Stream should be selected.";
                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true;
            return false;
        }


        function CheckGroup(oSrc, args) {

            if (document.getElementById(_clientGroup) != null) {
                var sGroup = document.getElementById(_clientGroup).value;

                if (document.getElementById(_schoolId).value == schoolIdSNS && (sGroup == 0)) {

                    oSrc.errormessage = "Stream Group should be selected."

                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true;
            return false;
        }
        function CheckOptionalSubject(oSrc, args) {         
            var rb = document.getElementById(_ClientRadioOptionalSubject);

            if (rb != null) {
                var radio = rb.getElementsByTagName("input");

                var isChecked = false;

                if (document.getElementById(_schoolId).value == schoolIdSNS) {

                    for (var i = 0; i < radio.length; i++) {
                        if (radio[i].checked) {
                            isChecked = true;
                            break;
                        }
                    }
                    if (!isChecked) {                       
                        oSrc.errormessage = "Please select at least one optional subject.";
                        args.IsValid = false;
                    }

                    return isChecked;
                }
            }
            args.IsValid = true;
            return false;
        }

        function ValidateStaffName(oSrc, args) {
            if ($('#' + '<%=this.ddlUserName.ClientID %>').val() != 0) {
                if ($('#' + '<%=this.ddlUserRole.ClientID %>').val() == 0 || $get('<%=this.chkIsStaffKid.ClientID %>').checked == false) {
                    oSrc.errormessage = 'if you select Staff Name, Staff User Role and Is Staff Kid option should be selected.'
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateMotherAadharFile(sender, args) {
            var fileInput = document.getElementById('<%= flUploadMotherAaadhar.ClientID %>');
            var filePath = fileInput.value.toLowerCase();

           var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;

           if (filePath !== '' && !allowedExtensions.test(filePath)) {
                sender.errormessage = "Please select valid file type for Mother's Aadhar Card.";
                args.IsValid = false;
                return;
            }

           if (fileInput.files && fileInput.files[0]) {
                var fileSize = fileInput.files[0].size; 
                if (fileSize > 1048576) { 
                    sender.errormessage = 
                        "Size of Mother's Aadhar Card file should not be more than 1 mb.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        function ValidateFatherAadharFile(sender, args) {
            var fileInput = document.getElementById('<%= flUploadFatherAaadhar.ClientID %>');
            var filePath = fileInput.value.toLowerCase();
          
            var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;
          
            if (filePath !== '' && !allowedExtensions.test(filePath)) {
                sender.errormessage = "Please select valid file type for Father's Aadhar Card.";                                
                args.IsValid = false;
                return;
            }

            // Check size if file selected
            if (fileInput.files && fileInput.files[0]) {
                var fileSize = fileInput.files[0].size; // in bytes
                if (fileSize > 1048576) { // 1 MB
                    sender.errormessage = "Size of Father's Aadhar Card file should not be more than 1 mb.";
                    args.IsValid = false;
                    return;
                }
            }

            args.IsValid = true;
        }

        function ValidateCastCertificate(sender, args) {
            var fileInput = document.getElementById('<%= fuCastCertificate.ClientID %>');
            var filePath = fileInput.value.toLowerCase();

            var allowedExtensions = /(\.pdf|\.jpg|\.jpeg|\.png|\.bmp)$/i;

            if (filePath != '') {
                if (!allowedExtensions.test(filePath)) {
                    sender.errormessage  = "Please select valid file type for Caste Certificate.";
                    args.IsValid = false;
                    return;
                }

                if (fileInput.files && fileInput.files[0]) {
                    var fileSize = fileInput.files[0].size; // in bytes
                    if (fileSize > 1048576) { // 1 MB
                        sender.errormessage  = "Size of Caste Certificate file should not be more than 1 mb.";
                        args.IsValid = false;
                        return;
                    }
                }
            }

            args.IsValid = true;
        }

    </script>
    <script type="text/javascript">

        function ChangeSecondAndThirdLanguage(iValue) {
            var LanguageGroups = $("#" + _clienthidLanguageGroupIds).val();
            var secondLangSubjectid = $("#" + _clientddlSecondLanguage).val();
            var thirdLangSubjectId = $("#" + _clientcmbThirdLanguage).val();
            var Section = $("#" + _clienthidSectionId).val();
            if (LanguageGroups != "") {
                var Ids = LanguageGroups.split("$");
                var sFlag = true;
                if (iValue == 1 && secondLangSubjectid == 0) {
                    $("#" + _clientcmbThirdLanguage).val(0);
                    sFlag = false;
                }
                else if (iValue == 2 && thirdLangSubjectId == 0) {
                    $("#" + _clientddlSecondLanguage).val(0);
                    sFlag = false;
                }
                if (sFlag == true) {
                    if (Section == "N") {
                        for (var j = 0; j < Ids.length; j++) {
                            var s = Ids[j].split(",");
                            if (iValue == 1) {
                                if (s[0] == secondLangSubjectid)
                                    $("#" + _clientcmbThirdLanguage).val(s[1])
                            }
                            else {
                                if (s[1] == thirdLangSubjectId)
                                    $("#" + _clientddlSecondLanguage).val(s[0])
                            }
                        }
                    }
                    else {
                        var s = Ids[0].split(",")
                        if (iValue == 1) {
                            if (s[0] == secondLangSubjectid)
                                $("#" + _clientcmbThirdLanguage).val(s[1])
                            else
                                $("#" + _clientcmbThirdLanguage).val(s[0])
                        }
                        else {
                            if (s[1] == thirdLangSubjectId)
                                $("#" + _clientddlSecondLanguage).val(s[0])
                            else
                                $("#" + _clientddlSecondLanguage).val(s[1])
                        }
                    }
                }
            }
        } 
    </script>
</asp:Content>
