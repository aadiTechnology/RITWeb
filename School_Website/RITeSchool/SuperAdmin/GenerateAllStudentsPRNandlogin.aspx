<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="GenerateAllStudentsPRNandlogin.aspx.cs" Inherits="GenerateAllStudentsPRNandlogin" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel runat="server" ID="UpnlwizNextAcaGen">
            <ContentTemplate>
                <table>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" HeaderText="Please fix following error(s):"
                                ShowMessageBox="False" ShowSummary="True" ValidationGroup="SMSSend"/>
                            <asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
                                Visible="true" ErrorMessage="" ClientValidationFunction="ValidateControls" ValidationGroup="SMSSend"></asp:CustomValidator>
                            
                       </td>
                    </tr>
                    <tr>
                    <td align="center" colspan="2">
                      <asp:Label ID="lblUpdate" runat="server" Text=""
                                CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                    </td>
                       
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblGenerate" runat="server" Text="Click on Generate Login to auto  generate login Ids of existent students."
                                CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;<asp:Button ID="btnGenerate" runat="server" CausesValidation="False" CssClass="ClsBtnMid"
                                OnClick="btnGenerate_Click" Text="Generate Login" />
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td >
                         
                            <span ID="asds" runat="server" style="color:#000;padding-left:5px;font-size:9pt;font-weight:700;font-family:Verdana;"> SMS</span>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkSMSOldStudents" runat="server" CssClass="Lbl10pt" Text="Send SMS to old students." />
                        </td>
                        <td rowspan="2">
                            <asp:Button ID="btnSMS" runat="server" CssClass="ClsBtnMid" OnClick="btnSMS_Click"
                                Text="Send SMS" ValidationGroup="SMSSend" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkSMSNewStudents" runat="server" CssClass="Lbl10pt" Text="Send SMS to new mid year students." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkSendMobileSMS" runat="server" CssClass="Lbl10pt" Text="Send mobile URL SMS to selected students." />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" CssClass="LblNrmlB" 
                                EnableViewState="False" Text="Send SMS to Teacher's and Admin Staff."></asp:Label>
                        <td>
                            &nbsp;</td>
                           
                           
                        </tr>
                   
                    <tr>
                        <td colspan="3" >
                        </td>
                    </tr>                                                  
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Button ID="btnSMSAdminstaff" runat="server" CssClass="ClsBtnMid" 
                                onclick="btnSMSAdminstaff_Click" Text="Send SMS To Admin Staff" Width="157px" ValidationGroup="SendSMS" />
                            <asp:Button ID="btnSMSTeacher" runat="server" CssClass="ClsBtnMid" 
                                onclick="btnSMSTeacher_Click" Text="Send SMS To Teacher's" Width="140px" ValidationGroup="SendSMS" />
                        </td>
                    </tr>
                    <tr>
                        <td height="20px" colspan="3" >
                        </td>
                    </tr>  
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Click on Activate Logins to activate all student's login."
                                CssClass="LblNrmlB" EnableViewState="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnActivateLogin" runat="server" CausesValidation="False" 
                                CssClass="ClsBtnMid" Text="Activate Logins" onclick="btnActivateLogin_Click" />                                
                        </td>
                    </tr>   
                    <tr>
                        <td align="center" colspan="2" height="40px">
                         </td>                        
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnBack" runat="server" CausesValidation="False" CssClass="ClsBtnMid"
                                Text="Back" OnClick="btnBack_Click" />                            
                        </td>
                    </tr>
                       <%-- </table>--%>
                           
                  
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        _clientchkSMSOldStudentsId = "<%=this.chkSMSOldStudents.ClientID %>";
        _clientchkSMSNewStudentsId = "<%=this.chkSMSNewStudents.ClientID %>";
        _clientchkSendMobileSMS = "<%=this.chkSendMobileSMS.ClientID %>";
        _clientcstFormId = "<%=this.cstForm.ClientID %>";

        function ConfirmGenerate() {
            return window.confirm('It will generate logins for old (existing) students as well. Do you want to proceed?')
        }
                
        function ValidateControls(oSrc, args) {
            if ($get(_clientchkSMSOldStudentsId).checked || $get(_clientchkSMSNewStudentsId).checked) {                
                args.IsValid = true
                return false
            }
            else if (($get(_clientchkSendMobileSMS) != null && $get(_clientchkSendMobileSMS).checked))
            {
                oSrc.errormessage = "Please select at least one student type.";
                args.IsValid = false;
                return true;
            }
            else {
                oSrc.errormessage = "Please select at least one checkbox.";
                args.IsValid = false;
                return true;
            } 
        }
    </script>
</asp:Content>
