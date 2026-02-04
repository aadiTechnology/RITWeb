<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master"
    AutoEventWireup="true" CodeFile="OnlineAdmissionlLoginUI.aspx.cs" Inherits="OnlineAdmissionlLoginUI"
    Title="Online Admission for 2012 - 13." ErrorPage="~/RITeSchool/Admission/Error.aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 95%" align="center"
        class="bordergray">
        <tr id="trLogin" runat="Server">
            <td align="Right" valign="top" width="60%" class="" 
                style="background-color: #FFF2F8; font-weight: normal; color: #006699;">
                <div style="float: left">
                    <table>
                        <tr>
                            <td align="left" style="font-size: 10pt;">
                                <strong>
                                <br />
                                This temporary login area is only for the parents who have applied for the admission for the academic year 2012-13.
                                <br />
                                <br />
                                Please enter the User Name and Password which you have received via SMS and then click on the 'Log In' button to check the admission status. You can also print the receipt or print the admission form after login.
                                <br />
                                <br />
                                <br />
                                In case of any issue, please contact at <%--<a href="mailto:info@ppspune.com">info@ppspune.com</a>--%> <asp:HyperLink ID="hlnkEmail" runat="server"></asp:HyperLink>.
                                <br />
                                </strong>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td align="center" valign="middle">
                <table style="padding: 0px; margin: 0px; border: thin solid #C0C0C0; height: 100%;
                    width: 100%;" width="100%" align="center">
                    <tr>
                        <td colspan="1" style="background-color: #669acc; font-size: 12px; color: #FFFFFF;
                            font-weight: bold;" class="bordergray" align="left">
                            Admission Login
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1" align="left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="center" class="GreenBGPatch">
                            <div runat="Server" id="Div1">
                                <asp:Login ID="AdmissionLogin" runat="server" OnAuthenticate="Login1_Authenticate"
                                    RememberMeSet="True">
                                    <TitleTextStyle CssClass="LblUsrNameHead" />
                                    <LayoutTemplate>
                                        <table cellpadding="0" style="height: 100%; width: 100%;" border="0">
                                            <tbody>
                                                <tr>
                                                    <td style="padding-right: 5px" align="left">
                                                        <asp:Label ID="UserNameLabel" runat="server" CssClass="TxtBSml" Width="120px" AssociatedControlID="UserName"
                                                            EnableViewState="False">User Name:</asp:Label><span style="color: red"> </span>
                                                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ValidationGroup="Login1"
                                                            CssClass="ErrMsg" ToolTip="User Name should not be blank." SetFocusOnError="true"
                                                            ErrorMessage="User Name should not be blank." ControlToValidate="UserName" Display="None"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:TextBox ID="UserName" runat="server" CssClass="TxtBoxLogin"></asp:TextBox>
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 5px" align="left">
                                                        <asp:Label ID="PasswordLabel" runat="server" CssClass="TxtBSml" AssociatedControlID="Password"
                                                            EnableViewState="False">Password:</asp:Label>
                                                        <span style="color: #ff0000"></span>
                                                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ValidationGroup="Login1"
                                                            CssClass="ErrMsg" ToolTip="Password is required." SetFocusOnError="true" ErrorMessage="Password should not be blank."
                                                            ControlToValidate="Password" Display="None"></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="padding-right: 5px" align="left">
                                                        <asp:TextBox ID="Password" runat="server" CssClass="TxtBoxLogin" TextMode="Password"></asp:TextBox>
                                                        <span style="color: #ff0000">*</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="ErrMsg" align="center">
                                                        <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Button ID="LoginButton" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                            CommandName="Login" CssClass="ClsButton" Text="Log In" ValidationGroup="Login1" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left" valign="bottom">
                                                        <table class="borderblue" cellpadding="0" border="0">
                                                            <tbody>
                                                                <tr>
                                                                    <td class="Rlink" align="left">
                                                                        Powered by:
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="padding-left: 5px" align="center">
                                                                        <a href="http://www.regulusit.net" target="_blank" border="0">
                                                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/RIT_LogoAnimated.gif" AlternateText="http://www.regulusit.net">
                                                                            </asp:Image>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </LayoutTemplate>
                                </asp:Login>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ErrMsg" ShowMessageBox="true"
        ShowSummary="false" ValidationGroup="Login1" />
    <asp:HiddenField runat="server" ID="hidRedirect" />
</asp:Content>
