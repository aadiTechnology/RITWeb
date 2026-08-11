<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Welcome to <%: System.Configuration.ConfigurationManager.AppSettings["SchoolName"]%></title>
    <link rel="SHORTCUT ICON" href="/images/favicon.ico" type="image/x-icon" />
    <style>
            .clsLabel {
                color: #4682B4;
                font-weight: 700;
                font-family: Tahoma;
                font-size: 10pt;
                line-height: 10pt;
            }
            
            .clsText {
                font-family: Tahoma;
                font-size: 10pt;
                line-height: 12pt;
            }
            
            .clsText:focus { background-color:lightyellow; }
            
            .button {
                width: 100px; 
                border-style: solid;
                font-size:12px;
                height:30px;
                border-radius: 25px;
                box-shadow: 2px 2px 2px #888888;
                border-color: Gray; 
                color: Black; 
                font-weight: bold;
            }
            
            .button:hover
            {
            	background-color:white;
            	font-size:13px;
            }

    </style>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
            <tr>
                <td align="right">
                    <span style="color: Red;font-family:Verdana;font-size:10px;">* Mandatory fields.</span>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <img src="RITeSchool/images/Logos/School_Logo.bmp?version=1.0" style="" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <div style="border-style: double; border-color: Silver; height: 300px; width: 400px;
                        background-color: #E4E4E4; box-shadow: 5px 5px 5px #888888;border-radius: 25px;">
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="valSum" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <span style="font-size: 25px; font-weight: bold;"><%: System.Configuration.ConfigurationManager.AppSettings["SchoolName"]%></span>
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="font-family: Times New Roman;">
                                    <table>
                                        <tr>
                                            <td align="left" width="100px">
                                                <span class="clsLabel">User Name</span>
                                            </td>
                                            <td width="10px">
                                                <span class="clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="clsText"></asp:TextBox>
                                                <span style="color: Red">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="User Name should not be blank."
                                                    ControlToValidate="txtUserName" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <span class="clsLabel">Password</span>
                                            </td>
                                            <td>
                                                <span class="clsLabel">:</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="clsText"></asp:TextBox>
                                                <span style="color: Red">*</span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password should not be blank."
                                                    ControlToValidate="txtPassword" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                            <td colspan="3" align="center">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Label ID="lblMessage" runat="server" Text="" Style="color: Red;font-family:Verdana;font-size:12px;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <asp:Button ID="btnLogin" runat="server" CssClass="button" Text="Login"  OnClick="btnLogin_Click" />
                                            </td>
                                        </tr>
                                        <tr style="height: 20px;">
                                            <td colspan="3" align="center">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3" align="center">
                                                <u>
                                                    <asp:LinkButton ID="lnkbtn1" runat="server" Text="Forgot Password" CssClass="clsText" Style="font-weight: bold;font-size:12px;"
                                                        OnClientClick="OpenPopup(); return false;"></asp:LinkButton>
                                                    <asp:HiddenField ID="hidScreenWidth" runat="server" />
                                            </td>
                                        </tr> 
										<tr style="display:none;">
                                            <td colspan="3" align="center">
                                                <u>
                                                    <asp:LinkButton ID="lnkbtn2" runat="server" Text="Policy Details" CssClass="clsText" Style="font-weight: bold;font-size:12px;"
                                                        OnClientClick="OpenPolicyPopup(); return false;"></asp:LinkButton>                                                    
                                            </td>
                                        </tr> 										
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    <script type="text/javascript">
        _hidScreenWidth = "<%=this.hidScreenWidth.ClientID%>";
        function OpenPopup() {
            window.open("ForgotPassword.aspx", '_new', 'fullscreen=no,scrollbars=yes,resizable=no,top=200,left=200,width=540,height=430');
            return false;
        }

        function OpenPolicyPopup() {
            window.open("PrivacyPolicy.aspx", '_new');
            return false;
        }

        function SetWidth() {
            if (document.getElementById(_hidScreenWidth) != null) {
                var hidScreenWidth = document.getElementById(_hidScreenWidth)
                hidScreenWidth.value = "" + window.screen.width;
            }
        }
        SetWidth();
    </script>
    </form>
</body>
</html>
