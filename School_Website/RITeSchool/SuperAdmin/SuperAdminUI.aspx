<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SuperAdminUI.aspx.cs" Inherits="SuperAdminUI"
    Title="Welcome to school." MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table border="0" cellpadding="0" cellspacing="1" style="width: 95%" align="center"
        class="bordergray">
        <tr id="Tr1" runat="Server">
            <td align="left" class="bordergray" valign="top">
                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ErrMsg" ShowMessageBox="true"
                    ShowSummary="false" ValidationGroup="Login1" />
                <asp:HiddenField runat="server" ID="hidRedirect" />
                <asp:HiddenField ID="hidScreenWidth" runat="server" />
            </td>
        </tr>
        <tr id="trLogin" runat="Server">
            <td align="center" class="bordergray ">
                <div runat="Server" id="Div1">
                    <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1_Authenticate" RememberMeSet="True">
                        <TitleTextStyle CssClass="LblUsrNameHead" />
                        <LayoutTemplate>
                            <table cellpadding="0" width="100%" border="0" class="ClsBorderlight">
                                <tbody>
                                    <tr>
                                        <td style="padding-right: 5px" align="left" class="ClsBorderGray">
                                            <asp:Label ID="UserNameLabel" runat="server" CssClass="TxtBSml" Width="120px" AssociatedControlID="UserName"
                                                EnableViewState="False">User Name:</asp:Label><span style="color: red"> </span>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ValidationGroup="Login1"
                                                CssClass="ErrMsg" ToolTip="User Name should not be blank." SetFocusOnError="true"
                                                ErrorMessage="User Name should not be blank." ControlToValidate="UserName" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderGray">
                                            <asp:TextBox ID="UserName" runat="server" CssClass="MidTxtBox"></asp:TextBox>
                                            <span style="color: #ff0000">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 5px" align="left" class="ClsBorderGray">
                                            <asp:Label ID="PasswordLabel" runat="server" CssClass="TxtBSml" AssociatedControlID="Password"
                                                EnableViewState="False">Password:</asp:Label>
                                            <span style="color: #ff0000"></span>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ValidationGroup="Login1"
                                                CssClass="ErrMsg" ToolTip="Password is required." SetFocusOnError="true" ErrorMessage="Password should not be blank."
                                                ControlToValidate="Password" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-right: 5px" align="left" class="ClsBorderGray">
                                            <asp:TextBox ID="Password" runat="server" CssClass="MidTxtBox" TextMode="Password"></asp:TextBox>
                                            <span style="color: #ff0000">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="color: red" class="LblErrorMsg">
                                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderGray">
                                            <asp:Button ID="LoginButton" runat="server" ValidationGroup="Login1" CssClass="ClsButton"
                                                BorderWidth="1px" BorderStyle="Solid" Text="Log In" CommandName="Login"></asp:Button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="bordergray " valign="bottom">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="bottom" class="bordergray ">
                                            <table class="borderblue" cellpadding="0" border="0">
                                                <tbody>
                                                    <tr>
                                                        <td class="TxtNormal" align="left">
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
        <tr>
            <td>
            </td>
        </tr>
        <tr id="Tr2" runat="Server">
            <td class="bordergray" valign="top">
                &nbsp;
            </td>
        </tr>
    </table>

    <script language="javascript">
        _hidScreenWidth = "<%=this.hidScreenWidth.ClientID%>"
        function SetWidth() {
            if (document.getElementById(_hidScreenWidth) != null) {
                var hidScreenWidth = document.getElementById(_hidScreenWidth)
                hidScreenWidth.value = "" + window.screen.width
            } 
        }
        SetWidth()
    </script>
</asp:Content>
