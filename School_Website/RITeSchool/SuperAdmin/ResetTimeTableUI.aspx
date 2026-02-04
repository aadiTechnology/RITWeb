<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="ResetTimeTableUI.aspx.cs" Inherits="ResetTimeTableUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel runat="server" ID="UpnlwizNextAcaGen">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError %>"
                                ShowMessageBox="False" ShowSummary="True" ValidationGroup="SMSSend" />
                            <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="" ClientValidationFunction="ValidateControls" ValidationGroup="SMSSend"></asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                            <asp:Label ID="lblReset" runat="server" CssClass="lblBlkB" EnableViewState="False" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>
                            <asp:Label ID="lblGenerate" runat="server" Text="Click on Generate Logins to auto  generate login Ids of existent students."
                                CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="btnGenerate" runat="server" CausesValidation="False" CssClass="ClsBtnMid"
                                OnClick="btnGenerate_Click" Text="Generate Logins" />
                            &nbsp;
                        </td>
                    </tr>--%>
                    <%--<tr>
                        <td>
                            &nbsp;
                            <asp:Label ID="lblSMS" runat="server" CssClass="lblBlkB" Text="Reset" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>--%>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkResetTimeTable" runat="server" CssClass="Lbl10pt" Text="<%$ Resources:LocalizedResources, ResetTimeTable %>" />
                        </td>
                        <td rowspan="2">
                            <asp:Button ID="btnReset" runat="server" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, Reset%>" ValidationGroup="SMSSend"
                                OnClick="btnReset_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkResetSubjectTeacher" runat="server" CssClass="Lbl10pt" Text="<%$ Resources:LocalizedResources, MsgResetTimeTable %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkResetClassTeacher" runat="server" CssClass="Lbl10pt" Text="<%$ Resources:LocalizedResources, MsgResetClassTeacher %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtnMid"
                                Text="<%$ Resources:LocalizedResources, Back %>" OnClick="btnBack_Click" />
                        </td>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hidMsgResetTimeTable1" runat="server" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
    </div>

    <script language="javascript" type="text/javascript">
     _clientchkResetTimeTable = "<%=this.chkResetTimeTable.ClientID %>"
     _clientchkResetClassTeacher = "<%=this.chkResetClassTeacher.ClientID %>"
     _clientchkResetSubjectTeacher = "<%=this.chkResetSubjectTeacher.ClientID %>"
        _clientcstFormId = "<%=this.cstForm.ClientID %>"
        function ValidateControls(oSrc, args) {
            if (document.getElementById(_clientchkResetTimeTable).checked ||
                document.getElementById(_clientchkResetClassTeacher).checked ||
                document.getElementById(_clientchkResetSubjectTeacher).checked) {
                args.IsValid = true
                return false
            }
            else {
                oSrc.errormessage = document.getElementById("<%=this.hidMsgResetTimeTable1.ClientID %>").value
                args.IsValid = false
                return true
            } 
        }
    </script>

</asp:Content>
