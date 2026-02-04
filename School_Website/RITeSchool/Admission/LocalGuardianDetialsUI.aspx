<%@ Page Title="Admission process" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="LocalGuardianDetialsUI.aspx.cs" Inherits="LocalGuardianDetials"
    ViewStateMode="Disabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
    Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <script src="../PopCalendar2008/PopCalendarAjaxNet.js" type="text/javascript"></script>
    <script src="../PopCalendar2008/PopCalendarFunctionsAjaxNet.js" type="text/javascript"></script>
    <div id="nifty" align="center">
        <table align="center" style="text-align: center;" class="paddingLR" cellspacing="1"
            cellpadding="1" border="0" width="100%">
            <tbody align="center">
                <tr align="center" style="text-align: center; margin: 0px auto;">
                    <td align="center" style="text-align: center; margin: 0px auto;">
                        <table align="center" style="text-align: center; margin: 0px auto;" width="40%">
                            <tr>
                                <td colspan="3" runat="server" align="center" id="tdErrorMessage" class="ClsHilightBGB"
                                    visible="false">
                                    <asp:Label ID="lblError" runat="server" Text="" class="LblNrmlB" Style="border-width: 0px;
                                        font-weight: bold;"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="HeadTxtBWOPadding borderBtm" align="center" colspan="3">
                                    LOCAL GUARDIAN (ONLY IF APPLICABLE)
                                    <asp:ValidationSummary ID="valSum" runat="server" DisplayMode="BulletList" ShowMessageBox="true"
                                        ShowSummary="false" />
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="width: 200px; height: 20px;">
                                    First Name :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtFName" MaxLength="50" />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtSName" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Middle Name :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtFMName" MaxLength="50" />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtSMName" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Last Name :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtFLName" MaxLength="50" />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox runat="server" CssClass="MidTxtBox" ID="txtSLName" MaxLength="50" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Date of Birth :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtFCalDobPopup" CssClass="MidTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="CalDobPopup" runat="server" Control="txtFCalDobPopup" Format="dd MMM yyyy"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth." />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSCalDobPopup" CssClass="MidTxtBox" runat="server" AutoPostBack="True"></asp:TextBox>&nbsp;<rjs:PopCalendar
                                        ID="PopCalendar1" runat="server" Control="txtSCalDobPopup" Format="dd MMM yyyy"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Please select valid date of birth." />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Aadhar Card Number :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtFAadharCardNo" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSAadharCardNo" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    PAN Number :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtFPANNo" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSPANNo" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Qualification :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtFQualification" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtSQualification" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Mobile Number :
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtFMobile" runat="server" CssClass="MidTxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtSMobile" runat="server" CssClass="MidTxtBox" MaxLength="10" onblur="extractNumber(this,0,false);"
                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
                                </td>
                            </tr>
                            <tr>
                                <td class="TxtNormal" align="left" style="height: 20px;">
                                    Email Address :
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtFEmail" runat="server" CssClass="MidTxtBox" MaxLength="100" ViewStateMode="Enabled" />
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtSEmail" runat="server" CssClass="MidTxtBox" MaxLength="100" ViewStateMode="Enabled" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Relation With Student :
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtFStudentRelation" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtSStudentRelation" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height:10px;"></td>
                </tr>
                <tr style="text-align: center; margin: 0px auto;" align="center">
                    <td style="text-align: center; margin: 0px auto;" align="center">
                        <table style="text-align: center; margin: 0px auto; width: 30%;"
                            align="center">
                            <tr>
                                <td class="HeadTxtBWOPadding borderBtm" align="center" colspan="2">
                                    ACADEMIC INFORMATION
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 5px;">
                                </td>
                            </tr>
                            <tr>                            
                                <td align="left" class="TxtNormal" style="height: 20px; width:220px;">
                                    Last Exam Details :
                                </td>
                                <td align="left" class="TxtNormal">
                                    <asp:TextBox ID="txtLastExamDetails" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Syllabus Followed :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSyllabusFollowed" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    English :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtEnglish" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Second Language :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSecondLanguage" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Maths :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtMaths" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Science :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSceince" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    SST :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtSST" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Other :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtOther" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Total Marks :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtTotalMarks" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="TxtNormal" style="height: 20px;">
                                    Maximum Marks :
                                </td>
                                <td class="TxtNormal" align="left">
                                    <asp:TextBox ID="txtMaximumMarks" runat="server" CssClass="MidTxtBox" MaxLength="20" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center" class="TxtNormal" style="font-size: 10pt">
                        <asp:Button runat="server" ID="btnSubmit" Text="Submit" CausesValidation="true" 
                            CssClass="ClsBtn" onclick="btnSubmit_Click" />                        
                        <asp:HiddenField ID="hidStudentAdmissionId" runat="server" Value="0" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
