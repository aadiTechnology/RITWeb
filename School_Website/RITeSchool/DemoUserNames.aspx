<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DemoUserNames.aspx.cs" Inherits="DemoUserNames" Title="RITeSchool - Demo Logins" %>

<html>
<head id="Head1" runat="server">
    <title></title>
    <link href="Styles/Styles.css" type="text/css" rel="stylesheet" />
    <link href="Styles/Styles1.css" type="text/css" rel="stylesheet" />
    <link href="Styles/Styles2.css" type="text/css" rel="stylesheet" />
    <link href="Scripts/Script.js" type="text/javascript" rel="stylesheet" />
    <style type="text/css">
<!--
A.ssmItems:link		{color:black;text-decoration:none;}
A.ssmItems:hover	{color:black;text-decoration:none;}
A.ssmItems:active	{color:black;text-decoration:none;}
A.ssmItems:visited	{color:black;text-decoration:none;}
//-->
</style>

    <script language="javascript">
function closewindow()
{
window.close();
}

    </script>

</head>
<body style="width: 100%; height: 100%; background-image: url(images/BGline.gif);
    background-repeat: repeat;">
    <form runat="server" style="margin: 0; width: 100%; height: 100%; position: absolute">
        <table border="0" cellpadding="0" cellspacing="1" style="width: 97%; height: 97%">
            <tr valign="top">
                <td align="center">
                    <!-- Data Insert Here -->
                    <table cellpadding="0" cellspacing="0" id="TABLE1" style="width: 100%; padding-right: 5px;
                        padding-left: 5px;">
                        <tr>
                            <td align="left">
                                <table  border="0" cellpadding="0" cellspacing="0" style="width: 100%;
                                    padding-right: 5px; padding-left: 5px;">
                                    <tr>
                                    <td style="background-image: url(images/subpgTopImgSml.jpg); background-repeat: no-repeat;
                                        width: 800px; height: 77px" align="right" valign="bottom">
                                    </td>
                                </tr>
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle">
                                            <asp:Label ID="lblSelectSupplier" runat="server" Font-Bold="True" Text="Demo User Details"></asp:Label></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="3" style="height: 5px">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" align="right">
                                <table id="tblForStudentDiv" runat="server" width="100%" style="float: left">
                                    <tr>
                                        <td class="ClsBorderlight" colspan="3" align="left">
                                            <asp:Label ID="Label1" runat="server" Text="Use followiing details for Demo School Login of RITeSchool"
                                                CssClass="ClsTextNormal" ForeColor="#C000C0" EnableViewState="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight" colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td class="ClsBorderlight" align="Right">
                                            <asp:Label ID="Label4" runat="server" Text="Teacher Login - " CssClass="ClsTextNormal" EnableViewState="False"></asp:Label></td>
                                        <td align="left" class="ClsBorderlight" width="25%">
                                            <asp:Label ID="Label5" runat="server" CssClass="ClsTextNormal" Text="User Name: " EnableViewState="False"></asp:Label></td>
                                        <td class="ClsBorderlight" align="left">
                                            <asp:Label ID="Label8" runat="server" Text="Ramesh.Ketkar" CssClass="ClsTextNormal" Font-Bold="True" EnableViewState="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            &nbsp;
                                        </td>
                                        <td align="left" class="ClsBorderlight" width="50">
                                            <asp:Label ID="Label3" runat="server" CssClass="ClsTextNormal" Text="Password:" EnableViewState="False"></asp:Label></td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="Label7" runat="server" CssClass="ClsTextNormal" Text="111111" Font-Bold="True" EnableViewState="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight" colspan="3">
                                            &nbsp;&nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight" align="Right">
                                            <asp:Label ID="Label9" runat="server" CssClass="ClsTextNormal" Text="Parent / Student Login - " EnableViewState="False"></asp:Label></td>
                                        <td align="left" class="ClsBorderlight" width="25%">
                                            <asp:Label ID="Label10" runat="server" CssClass="ClsTextNormal" Text="User Name: " EnableViewState="False"></asp:Label></td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="Label11" runat="server" CssClass="ClsTextNormal" Font-Bold="True"
                                                Text="144" EnableViewState="False"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight">
                                            &nbsp;
                                        </td>
                                        <td align="left" class="ClsBorderlight" width="50">
                                            <asp:Label ID="Label12" runat="server" CssClass="ClsTextNormal" Text="Password:" EnableViewState="False"></asp:Label></td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:Label ID="Label13" runat="server" CssClass="ClsTextNormal" Text="111111" Font-Bold="True" EnableViewState="False"></asp:Label>
                                       </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderlight" colspan="3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="right" class="ClsBorderlight">
                                            <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnSml" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
        </table>
    </form>
</body>

<script language="javascript" type="text/javascript">
        window.focus();
        
        function fnover(varname)
        {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(images/BtnBGRollNew.jpg)";
            //objTXT.style.color = "maroon";
        }

        function fnout(varname)
        {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(images/BtnBG.jpg)";
            //objTXT.style.color = "Black";
        }
        
</script>

</html>
