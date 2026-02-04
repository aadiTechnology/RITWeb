<%@ Control Language="C#" AutoEventWireup="true"
    Inherits="SchoolWebApp.AdmissionWizardSteps" Codebehind="AdmissionWizardStepsUC.ascx.cs" %>
<div>
    <table style="width: 100%; height: 100%;" id="tblAdmission" runat="server">
        <tr>
            <td align="left" valign="top">
                <asp:Image ID="ImageStep1" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Select Standard »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top">
                <asp:Image ID="ImageStep2" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Submit Form »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top" id="tdStep3" runat="server">
                <asp:Image ID="ImageStep3" runat="server"/>
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle" id="tdSelectBank" runat="server">
                Select Bank »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top" id="tdSteps4" runat="server">
                <asp:Image ID="ImageStep4" runat="server"/>
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle" id="tdConfirmAmount" runat="server">
                Confirm Amount »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top">
                <asp:Image ID="ImageStep5" runat="server" />
            </td>
            <td id ="tdCompletion" runat ="server" align="left" class="lblAdmissionSteps" valign="middle">
                Completion and Receipt&nbsp;
            </td>
        </tr>
    </table>
    <table style="width: 100%; height: 100%;" id="tblStudentFee" runat="server">
        <tr>
            <td align="left" valign="top">
                <asp:Image ID="imgFeeStep1" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Amount Details »&nbsp;&nbsp;
            </td>           
            <td align="left" valign="top">
                <asp:Image ID="imgFeeStep2" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Select Bank »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top">
                <asp:Image ID="imgFeeStep3" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Confirm Amount »&nbsp;&nbsp;
            </td>
            <td align="left" valign="top">
                <asp:Image ID="imgFeeStep4" runat="server" />
            </td>
            <td align="left" class="lblAdmissionSteps" valign="middle">
                Completion&nbsp;
            </td>
        </tr>
    </table>
</div>
