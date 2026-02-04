<%@ Page Title="" Language="C#" MasterPageFile="~/PopupMaster.master" AutoEventWireup="true"
    CodeFile="ForgotPassword.aspx.cs" Inherits="ForgotPassword" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript" src="RITeSchool/PopCalendar2008/PopCalendarFunctionsAjaxNet.js"></script>
    <script language="javascript" type="text/javascript" src="RITeSchool/PopCalendar2008/PopCalendarAjaxNet.js"></script>
    <style type="text/css">
        .ClsHilightTextB
        {
            padding-left: 5px;
            color: #990066;
            font-size: 9pt;
            font-weight: bold;
            font-family: Verdana;
        }
        .style1
        {
            width: 57px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table border="0" cellpadding="1" cellspacing="0" class="ClsFPasswordtable1">
        <tr style="height: 10%">
            <td align="left" valign="top">
                <asp:ValidationSummary ID="valSumForgotPass" runat="server" ValidationGroup="PasswordRecovery1"
                    ShowMessageBox="false" ShowSummary="true" />
            </td>
        </tr>
        <tr id="trFailureText" runat="server" visible="false">
            <td align="center" class="TitleBg ClsBorderlight" bgcolor="Red">
                <asp:Label ID="lblFailureText" runat="server" Font-Bold="true" EnableViewState="False" />
            </td>
        </tr>
        <tr>
            <td align="center" colspan="2" class="ClsTopBorderGray">
                <asp:Label ID="lblHeading" runat="server" Font-Bold="True" CssClass="ClsHilightTextB">
					Please enter below information to receive the login details.
                </asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle" class="">
                <table border="0" cellpadding="3" class="ClsBorderGray ClsFPasswordtable2">
                    <tr>
                        <td align="center">
                            <table border="0" cellpadding="3" width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="3">
                                            <tr>
                                                <td align="left" height="70%" width="70px">
                                                    <asp:Label ID="lblUserName" runat="server" AssociatedControlID="txtUserName" CssClass="TxtBSml"
                                                        Text="User Name :" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="TxtBoxLogin" Width="100px"
                                                        OnTextChanged="txtUserName_TextChanged" AutoPostBack="True" />
                                                    <asp:CustomValidator ID="UserNameRequired" runat="server" ErrorMessage="Please enter User Name or Mobile No."
                                                        ToolTip="User Name is required." ValidationGroup="PasswordRecovery1" Display="None"
                                                        ValidateEmptyText="true" SetFocusOnError="true" ClientValidationFunction="cstValUSerNameOrMobileNo" />
                                                    <asp:CustomValidator ID="custHidMsg" runat="server" ValidationGroup="PasswordRecovery1"
                                                        ClientValidationFunction="cstvalHideMsg" />
                                                </td>
                                                <td align="center">
                                                    <img src="RITeSchool/images/ArrowBlueDblRev.gif" style="margin-left: 7px;" alt="" />
                                                    <span class="ClsHilightTextB" style="padding: 0">OR</span>
                                                    <img src="RITeSchool/images/ArrowBlueDblNw.gif" style="margin-right: 7px;" alt="" />
                                                </td>
                                                <td align="left" width="70px" height="70%">
                                                    <asp:Label ID="lblMobileNo" runat="server" AssociatedControlID="txtMobileNo" CssClass="TxtBSml"
                                                        Text="Mobile No :" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtMobileNo" runat="server" MaxLength="10" onblur="extractNumber(this,0,false);"
                                                        onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                        onpaste="event.returnValue=false" ondrop="event.returnValue=false" CssClass="TxtBoxLogin"
                                                        Width="100px" OnTextChanged="txtMobileNo_TextChanged" AutoPostBack="True" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="3">
                                            <tr>
                                                <td align="left" width="80px">
                                                    <asp:Label ID="lbldateOfBirth" runat="server" AssociatedControlID="txtCalDobPopup"
                                                        CssClass="TxtBSml" Text="Date of Birth :" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtCalDobPopup" runat="server" CssClass="TxtBoxLogin" Width="100px"
                                                        AutoPostBack="True" OnTextChanged="txtCalDobPopup_TextChanged" />
                                                    <rjs:PopCalendar ID="CalDobPopup" runat="server" Control="txtCalDobPopup" Format="dd MMM yyyy"
                                                        ValidationGroup="PasswordRecovery1" ShowWeekend="True" ShowErrorMessage="false"
                                                        InvalidDateMessage="Please select valid Date of Birth." RequiredDate="true" RequiredDateMessage="Please enter Date of Birth."
                                                        AutoPostBack="True" OnSelectionChanged="CalDobPopup_SelectionChanged" />
                                                </td>
                                            </tr>
                                        </table>
                                        <table align="center" id="tblemail" runat="server" visible="false">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Label ID="Label2" runat="server" CssClass="ClsHilightTextB">
					                                    Please enter email id to receive the login details through email.
                                                    </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style1" align="right">
                                                    <asp:Label ID="Label1" runat="server" AssociatedControlID="txtCalDobPopup" CssClass="TxtBSml"
                                                        Text="Email Id :" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="TxtBoxLogin" Width="285px" />
                                                    <asp:RegularExpressionValidator ID="regValEmail" runat="server" ControlToValidate="txtEmailId"
                                                        Display="None" ValidationGroup="PasswordRecovery1" ErrorMessage="Please insert valid Email Id."
                                                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="SubmitButton" runat="server" CommandName="Submit" Text="Submit" ValidationGroup="PasswordRecovery1"
                                            CssClass="ClsButton" OnClick="SubmitButton_Click" />
                                        <asp:Button ID="btnClose" runat="server" Text="Close" ValidationGroup="PasswordRecovery1"
                                            CssClass="ClsButton" OnClick="SubmitButton_Click" OnClientClick="window.close();return false;"
                                            Style="margin-left: 5px;" />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="left">
                                        <asp:LinkButton ID="lnkForgotPassword" runat="server" CssClass="SMSLblSMlBlue" Style="vertical-align: bottom;
                                            padding-left: 10px; font-size: 9pt; font-weight: bold; font-family: Verdana;"
                                            target="_blank" CausesValidation="False" OnClientClick="if(!ForgotPasswordRequest()) return false;"
                                            Visible="True">Change Mobile Number</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left" class="NoteLabel">
                <div>
                    <table class="paddingL5">
                        <tr>
                            <td colspan="2" class="paddingL5">
                                Notes -
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                •
                            </td>
                            <td>
                                Parents need to enter the date of birth of their child.
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                •
                            </td>
                            <td>
                                Please enter user name and date of birth, system will SMS you the password on mobile
                                number registered with RITeSchool account.
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                •
                            </td>
                            <td>
                                If you don&#39;t remember the user name then enter mobile number that is currently
                                registered with the RITeSchool account and date of birth.
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        function cstvalHideMsg(aSrc, args) {
            var trMsg = "<%= this.trFailureText.ClientID %>";
            if (document.getElementById(trMsg) != null) {
                document.getElementById(trMsg).style.display = "none";
            }
            args.IsValid = true;
            return false;
        }

        function cstValUSerNameOrMobileNo(aSrc, args) {

            var clientTxtUserName = "<%= this.txtUserName.ClientID %>";
            var clientTxtMobileNo = "<%= this.txtMobileNo.ClientID %>";
            var sUserName = stripLeadingTrailingBlanks(document.getElementById(clientTxtUserName).value);
            var sMobileNo = stripLeadingTrailingBlanks(document.getElementById(clientTxtMobileNo).value);

            document.getElementById(clientTxtUserName).value = sUserName;
            document.getElementById(clientTxtMobileNo).value = sMobileNo;

            if (sUserName.length == 0 && sMobileNo.length == 0) {
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function ForgotPasswordRequest() {
            window.open("ForgotPasswordRequestPopup.aspx", '_blank', 'fullscreen=no,scrollbars=yes,resizable=no,top=0,left=0,width=650,height=550');
            return false;
        }
    </script>
</asp:Content>
