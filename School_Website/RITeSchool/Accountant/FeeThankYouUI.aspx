<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="FeeThankYouUI.aspx.cs" Inherits="RITeSchool_Accountant_FeeThankYouUI" %>

<%@ Register TagPrefix="Wizard" TagName="AdmissionSteps" Src="~/UserControls/AdmissionWizardStepsUC.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
<style>
  
  .msg-box {
    margin: 20px auto;
    padding: 15px 20px;
    width: 90%;
    max-width: 600px;
    border-radius: 8px;
    font-size: 15px;
    font-family: Arial, sans-serif;
    display: block;
    animation: fadeIn 0.4s ease-in-out;
}

/* message types */
.msg-info {
    background: #e8f3ff;
    border-left: 5px solid #0078ff;
    color: #074a9c;
}

.msg-success {
    background: #e9f8ec;
    border-left: 5px solid #28a745;
    color: #1a7a33;
}

.msg-warning {
    background: #fff8e6;
    border-left: 5px solid #ffb400;
    color: #9c6b00;
}

.msg-error {
    background: #ffeaea;
    border-left: 5px solid #dc3545;
    color: #a12828;
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(-5px); }
    to   { opacity: 1; transform: translateY(0); }
}

</style>
    <table>
        <tr>
            <td>
                <Wizard:AdmissionSteps ID="SubmissionWizardSteps" runat="server" ActiveSteps="4"
                    IsStudentFee="true"></Wizard:AdmissionSteps>
            </td>
        </tr>
    </table>
    <div class="MainBodyDiv">
        <table cellpadding="0" cellspacing="2" style="width: 100%; padding: 1px 1px 1px 1xp"
            class="ClsBorderlight">
            <tr>
                <td align="center" class="ClsThankYouBG" id="tdThankyou" runat="server">
                    Thank you !!!
                </td>
            </tr>
            <tr>
                <td align="left" class="TxtNormal" style="padding-left: 2px; color: Blue; font-weight: bold">
                    <asp:Label ID="lblSuccess" runat="server"></asp:Label>

                    <div id="pageMessage" class="msg-box msg-warning" runat="server">
                        <span id="msgText" runat="server"></span>
                    </div>
                </td>
            </tr>
            <%--<tr>
                <td align="left" id="tdStatus" runat="server" class="TxtNormal" style="padding-left: 2px">
                    To check the fee status of your child, click on the Close button.
                </td>
            </tr>--%>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="right">
                    <span class="TxtNormal"><strong>Best regards,</strong>&nbsp;&nbsp;<br>
                    </span>
                    <asp:Label ID="lblSiteName" runat="server" CssClass="TxtNormal" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div>
                        <asp:Button ID="btnClose" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                            CssClass="ClsBtnSml" Text="Close" Visible="True" OnClick="btnClose_Click" /></div>
                </td>
            </tr>
        </table>
    </div>
    <script language="JavaScript" type="text/javascript">
        window.history.forward(1)
        history.go(1);
    </script>
    <script type="text/javascript" for="window" event="onunload">
            window.opener.location = window.opener.location;
    </script>
</asp:Content>
