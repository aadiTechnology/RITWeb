<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestUnpublishPopUp.aspx.cs"
    MasterPageFile="../MasterPages/PopupMasterSml.master" Inherits="TestUnpublishPopUp" %>

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
                            <td style="height: 20px">                                
                                <span class="MainTitleHead" style="font-weight:bold">Enter Reason For Unpublish</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="Please fix following error(s)"
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
                                                    <tr>
                                                        <td class="ClsBorderlight" valign="top">
                                                            <asp:Label ID="lblTestlbl" runat="server" CssClass="ClsLblLgnd" Text="Exam : " EnableViewState="False"></asp:Label>
                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblTestName" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                        <td >
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderlight" valign="top">                                                            
                                                                <span class="ClsLblLgnd" >Class Teacher : </span>

                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblTeacherHeading" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                        <td >
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="ClsBorderlight">                                                           
                                                                <span class="ClsLblLgnd" >Reason for Unpublish :</span>

                                                        </td>
                                                        <td valign="top" align="left" >
                                                            <asp:TextBox ID="txtUnPublishReason" runat="server" CssClass="LrgTxtBox" MaxLength="100" Width="240px"
                                                                TabIndex="3" Rows="3" TextMode="MultiLine"></asp:TextBox>&nbsp;                                                            
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="Label3" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"
                                                                EnableViewState="false"></asp:Label>&nbsp;
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUnPublishReason"
                                                                ErrorMessage="Reason for Unpublish should not be blank." SetFocusOnError="True"
                                                                Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" valign="top">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="btnUnPublish" runat="server" Text="Unpublish" CssClass="ClsBtn" TabIndex="5"
                                                                OnClick="btnUnPublish_Click" UseSubmitBehavior="false" /><asp:Button ID="btnCancel"
                                                                    runat="server" Text="Close" CssClass="ClsBtn" TabIndex="6" CausesValidation="False"
                                                                    UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
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
                    <asp:BulletedList ID="BulletedList1" runat="server">
                    </asp:BulletedList>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clientErrLabel = "<%=this.lblErrorMsg.ClientID %>"
        _clientbtnCancel = "<%=this.btnCancel.ClientID %>"
        function Enable(sender, sArgs) {
            document.getElementById(sArgs).selectedIndex = 0
            if (sender.checked)
                document.getElementById(sArgs).disabled = false
            else
                document.getElementById(sArgs).disabled = true
        }
        function ClearErrorLabel(hidDependentExamNames) {        
            var sDependentExamNames = document.getElementById(hidDependentExamNames).value
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
            document.getElementById(_clientErrLabel).innerText = ""
            if (isPageValid == true && sDependentExamNames != "")
                if (!window.confirm(sDependentExamNames + " exams will be unpublished. \nAre you sure you want to continue?"))
                    return false;
           return true;
        }
        function ResetFields_btnReset() {
            var bResult = window.confirm("Are you sure to Reset all Field?")
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
    <asp:HiddenField ID="hidStandardDivisionId" runat="server" Value="0" />
    <asp:HiddenField ID="hidTestId" runat="server" Value="0" />
    <asp:HiddenField ID="hidFrom" runat="server" Value="0" />
    <asp:HiddenField ID="hidQuery" runat="server" />
    <asp:HiddenField ID="hidDependentExamNames" runat="server" />    
    &nbsp;&nbsp;
</asp:Content>
