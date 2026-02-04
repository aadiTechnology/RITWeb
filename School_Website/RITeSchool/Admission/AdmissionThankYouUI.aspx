<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    ErrorPage="~/RITeSchool/Admission/Error.aspx" CodeFile="AdmissionThankYouUI.aspx.cs"
    Inherits="AdmissionThankYouUI" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table>
        <tr>
            <td>
                <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="5">
                </Wizard:AdmissionSteps>
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;
            height: 250px">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table id="tblMainDetails" runat="server" border="0" cellpadding="0" cellspacing="2" style="width: 97%; height: 300px">
                        <tr>
                            <td align="center" colspan="4" valign="top">
                                <div class="supportForm" style="width: 62%">
                                    &nbsp;<div class="pageContainer">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%; height: 100%" class="ClsBorderlight">
                                            <tr>
                                                <td style="width: 100%" align="left" valign="top">
                                                    <table style="width: 100%" align="left">
                                                        <tr id="trPaymentAdmissionForm" runat="server">
                                                            <td align="center" class="ClsBorderGray" style="padding-left: 2px; color: Blue; font-weight: bold">
                                                                <asp:Label ID="lblSuccessMessage" runat="server" Text="Your payment of admission form is successfully received." EnableViewState="false"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAdmissionForm" runat="server">
                                                            <td align="center" class="ClsBorderGray" style="padding-left: 2px; color: Blue; font-weight: bold">
                                                                Your admission form is successfully received.
                                                            </td>
                                                        </tr>
                                                        <tr align="center" id="trPaymentThankYou" runat="server">
                                                            <td >
                                                                <div style="vertical-align: middle">
                                                                    <strong>Thank you for your interest in admitting your child to <%= ConfigurationManager.AppSettings["SchoolName"]%>.
                                                                        We have received your payment of amount Rs. <%= SchoolBase.Settings.AdmissionFormFees%>. </strong></div>
                                                            </td>
                                                        </tr>
                                                        <tr align="center" id="trThankYou" runat="server">
                                                            <td >
                                                                <div style="vertical-align: middle">
                                                                    <strong>Thank you for your interest in admitting your child to <%= ConfigurationManager.AppSettings["SchoolName"]%>.</strong></div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr id="trPaymentReceipt" runat="server">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="hlnkReceipt" runat="server" NavigateUrl="../Accountant/FeesMiniReceipt.aspx">here</asp:HyperLink>
                                                                &nbsp;to view your payment receipt.
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="hlnkAdmissionForm" runat="server" NavigateUrl="../Admission/AdmissionFormReport.aspx">here</asp:HyperLink>
                                                                to view admission form.
                                                            </td>
                                                        </tr>
                                                        <tr id="trMedicalForm" runat="server" visible="false">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/RITeSchool/DOWNLOADS/AdmissionForms/Revised Medical History Sheet.pdf" Target="_blank">here</asp:HyperLink>
                                                                &nbsp;to view medical history form.
                                                            </td>
                                                        </tr>
                                                        <tr id="trParentalConsentForm" runat="server" visible="false">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/RITeSchool/DOWNLOADS/AdmissionForms/Parental Consent Form.pdf" Target="_blank">here</asp:HyperLink>
                                                                &nbsp;to view parental consent form.
                                                            </td>
                                                        </tr>
                                                        <tr id="trPPShUndertakingForm" runat="server" visible="false">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="HyperLink4" runat="server" NavigateUrl="~/RITeSchool/DOWNLOADS/AdmissionForms/Undertaking Form.pdf" Target="_blank">here</asp:HyperLink>
                                                                &nbsp;to view Undertaking Form.
                                                            </td>
                                                        </tr>
                                                        <tr id="trPPSDocuments" runat="server" visible="false">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Click
                                                                <asp:HyperLink ID="HyperLink5" runat="server" NavigateUrl="~/RITeSchool/DOWNLOADS/AdmissionForms/Consent_Undertaking_medical_and_apaar_form.pdf" Target="_blank">here</asp:HyperLink>
                                                                &nbsp;to view Consent Form, Medical History Form and Undertaking Form.
                                                            </td>
                                                        </tr>
                                                       <%-- <tr>
                                                            <td class="TxtNormal">
                                                                &nbsp;
                                                            </td>
                                                        </tr>--%>
                                                        <tr id="trSMS" runat="server" visible="false">
                                                            <td align="left" class="TxtNormal ColorBgThankYou" >
                                                                <b>You will shortly receive an sms containing form number on specified mobile number.</b>
                                                            </td>
                                                        </tr>
                                                        <tr id="trPaymentNote" runat="server">
                                                            <td align="left" class="TxtNormal ColorBgThankYou">
                                                                <b>Please print the receipt and the admission form before closing the screen. If the screen is closed, the admission form/receipt will not be available. 
                                                                This receipt and admission form is required to be used as a proof at the time of taking admission.</b>
                                                            </td>
                                                        </tr>
                                                        <tr id="trNote" runat="server">
                                                            <td align="left" class="TxtNormal ColorBgThankYou">
                                                                <b>Please print the admission form before closing the screen. If the screen is closed, the admission form will not be available. 
                                                                This admission form is required to be used as a proof at the time of taking admission.</b>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr id="trSibling" runat="server">
                                                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                                                Please click&nbsp;<asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RITeSchool/Admission/OnlineAdmissionUI.aspx">here</asp:HyperLink>
                                                            <asp:Label ID="lblNoteSms1" runat="server" 
                                                                    Text="to submit admission form for your another child."></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="trCloseButton" runat="server" visible="false">
                                                            <td align="center">
                                                                <asp:Button ID="btnClose" runat="server" CssClass="ClsBtn" Text="Close" CausesValidation="false" OnClientClick="window.close(); return false;"  />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <%--<td class="TxtNormal" align="center">
                                                    <asp:Button ID="btnYes" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                                        CssClass="ClsButton" Text="Yes" 
                                                                    PostBackUrl="~/RITeSchool/Admission/OnlineAdmissionUI.aspx" Visible="True" />
                                                    <asp:Button ID="btnNo" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                                        CssClass="ClsButton" Text="No" Visible="True" onclick="btnNo_Click" />
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="4" valign="top">
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                    <table id="tblSPSRegistrationLink" runat="server" visible = "false" border="0" cellpadding="0" cellspacing="2" style="width: 50%; border:1px groove;">
                        <tr>
                            <td style="height:100px;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                Registration form submitted successfully.
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td class="TxtNormal" align="center" style="font-weight: bold">
                                Click
                                <asp:HyperLink ID="hlnkSPSRegistration" runat="server" NavigateUrl="../Admission/AdmissionFormReport.aspx" >here</asp:HyperLink>
                                &nbsp;to download form.
                            </td>
                        </tr>
                         <tr>
                            <td style="height:100px;">
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HiddenField ID="hidEnquiryId" runat="server" Value="0" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
