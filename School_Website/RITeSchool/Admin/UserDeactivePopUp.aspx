<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserDeactivePopUp.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="UserDeactivePopUp" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td >
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True" Text = "<%$ Resources:LocalizedResources, UserActivateDeactivate%>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                    <span class="ClsMdtStar">*</span> 
                    <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText= "<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                        runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlFields" runat="server" Width="100%">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0" runat="server" id="tblHeading" visible="True">
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="0" cellspacing="2" runat="server" id="Table1" visible="True">
                                                    <%--<tr>
                                                        <td class="ClsBorderlight" valign="top">
                                                            <asp:Label ID="lblTestlbl" runat="server" CssClass="ClsLblLgnd" Text="Exam : " EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblUserName" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td class="ClsBorderlight" valign="middle"  style="width:150px; height:24px;">
                                                            <asp:Label ID="lblUser" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, UserName%>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td class="ClsHilightBGB" >
                                                            <asp:Label ID="lblUserHeading" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                        <td >
                                                        </td>
                                                    </tr>
                                                    <tr id="trSendSms" runat="server">
                                                        <td valign="middle" class="ClsBorderlight" style="height:24px;">
                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text= "<%$ Resources:LocalizedResources, SendSMSMsg%>" EnableViewState="False"></asp:Label>
                                                             <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td valign="top" align="left" >
                                                            <asp:CheckBox ID="chkSendSms" runat="server" Checked="false" AutoPostBack="True"
                                                                OnCheckedChanged="chkSendSms_CheckedChanged" TabIndex="1" />
                                                        </td>
                                                        <td align="left"  >
                                                        
                                                        </td>
                                                    </tr>
                                                    <tr id="trReasonOfDectivation" visible="false" runat="server">
                                                        <td class="ClsBorderlight" valign="middle" >
                                                        <asp:Label ID="lblReasonDeactivation" runat="server" CssClass="ClsLabel"  Text= "<%$ Resources:LocalizedResources, DeactivationReason%>"
                                                                EnableViewState="False"></asp:Label>
                                                                 <span class="ClsLabel colonPadding">:</span>
                                                            &nbsp;</td>
                                                        <td align="left" valign="middle" class="ClsBorderlight" style="height:24px;">
                                                        <asp:Label ID="lblReasonForDeactivation" CssClass="ClsLabel" runat="server" EnableViewState="True"></asp:Label>
                                                            &nbsp;</td>
                                                        <td align="left"  >
                                                         
                                                            </td>
                                                    </tr>
                                                    <tr id="trReason" runat="server">
                                                        <td valign="middle" class="ClsBorderlight">
                                                            <asp:Label ID="lblReason" runat="server" CssClass="ClsLabel"  Text= "<%$ Resources:LocalizedResources, ResonForDeactivate%>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtReason" runat="server" CssClass="LrgTxtBox" MaxLength="100" Width="240px"
                                                                TabIndex="2" Rows="3" TextMode="MultiLine" Enabled="false"></asp:TextBox>&nbsp;
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="lblStar" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                                                EnableViewState="false"></asp:Label>&nbsp;
                                                            <asp:RequiredFieldValidator ID="reqValtxtReason" runat="server" ControlToValidate="txtReason"
                                                                ErrorMessage= "<%$ Resources:LocalizedResources, valReasonBlank %>" SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regvalTxtReason" runat="server" Display="None"
                                                                ControlToValidate="txtReason" ErrorMessage="" ValidationExpression="^[\s\S]{0,100}$"> 
                                                             </asp:RegularExpressionValidator>
                                                         </td>
                                                    </tr>
                                                    <tr id="trRemoveReferances" runat="server" visible="false">
                                                        <td valign="middle" class="ClsBorderlight" style="height:24px;">
                                                            <asp:Label ID="lblReferances" runat="server" CssClass="ClsLabel"  Text= "Remove All Referances"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="ClsLabel colonPadding">:</span>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:CheckBox ID="chkRemoveReferances" runat="server" Checked="false" AutoPostBack="false" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" valign="top">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="btnDeactivate" runat="server" Text= "<%$ Resources:LocalizedResources, Deactivate%>" CssClass="ClsBtn"
                                                                TabIndex="3" UseSubmitBehavior="false" OnClick="btnDeactivate_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" Text= "<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtn" TabIndex="4"
                                                                CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="HidSMSTemplateName" runat="server" />
                                                            <asp:HiddenField ID="hidSmsTemplate" runat="server" />
                                                            <asp:HiddenField ID = "hidvalResetAllFields" runat = "server" />
                                                            <asp:HiddenField ID = "hidalertDeactivateUser" runat = "server" />
                                                           <asp:HiddenField ID = "hidalertActivateUser" runat = "server" />
                                                           <asp:HiddenField ID = "hidCultureInfo" runat = "server" />
                                                           <asp:HiddenField ID = "hidTemplateRegId" runat = "server" Value="" />
                                                           <asp:HiddenField ID = "hidUserTypeId" runat = "server" Value="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientErrLabel = "<%=this.lblErrorMsg.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        _clienHidSMSTemplateName = "<%=this.HidSMSTemplateName.ClientID %>"
        _clienthidConfirmSms = "<%=this.hidConfirmSms.ClientID %>"
        _clientbtnDeactivate = "<%=this.btnDeactivate.ClientID %>"
        _ClienttxtReason = "<%=this.txtReason.ClientID %>"

        function ConfirmSms(sender) {

            if (typeof (Page_ClientValidate) == 'function')
                validationResult = Page_ClientValidate();
            if (validationResult == false)
                return false;
            var btnText=document.getElementById(_clientbtnDeactivate).value   
            if (btnText.toString() == "Activate") {

                if (window.confirm(document.getElementById("<%=this.hidalertActivateUser.ClientID %>").value)) {
                    document.getElementById(_clienthidConfirmSms).value = 1;
                    return false;
                }
                else {
                    document.getElementById(_clienthidConfirmSms).value =0;
                    return true;
                }
            }
            else {
                if (window.confirm(document.getElementById("<%=this.hidalertDeactivateUser.ClientID %>").value))
                document.getElementById(_clienthidConfirmSms).value = 0;
                else
                document.getElementById(_clienthidConfirmSms).value = 1;
            }
        }
        
        function Enable(sender, sArgs) {
            document.getElementById(sArgs).selectedIndex = 0
            if (sender.checked)
                document.getElementById(sArgs).disabled = false
            else
                document.getElementById(sArgs).disabled = true
        }
        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            document.getElementById(_clientErrLabel).innerText = ""
        }
        function ResetFields_btnReset() {
            var bResult = window.confirm(document.getElementById("<%=this.hidvalResetAllFields.ClientID %>").value)
            if (bResult) {
                document.aspnetForm.reset()
            }
            return false
        }
        function closewindow() {
            window.opener.location.reload(true)
            window.close()
            window.opener.focus()
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
       
    </script>

    <asp:HiddenField ID="hidUserName" runat="server" />
    <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
    <asp:HiddenField ID="hidQuery" runat="server" />
    <asp:HiddenField ID="hidMobileNo" runat="server" />
    <asp:HiddenField ID="hidUserRoleId" runat="server" />
    <asp:HiddenField ID="hidIsLocked" runat="server" />
    <asp:HiddenField ID="hidNameFilter" runat="server" />
    <asp:HiddenField ID="hidStandarId" runat="server" />
    <asp:HiddenField ID="hidDivisionId" runat="server" />
    <asp:HiddenField ID="hidDeactivationReason" runat="server" />
    <asp:HiddenField ID="hidConfirmSms" runat="server" Value="0" />
</asp:Content>
