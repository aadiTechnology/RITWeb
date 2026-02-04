<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error"
    MasterPageFile="../MasterPages/PopupMaster.master" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 650px" class="ClsBorderlight">
                        <tr>
                            <td align="center" class="ClsErrGrayBG td-vertical-align-top" rowspan="3" valign="top" style="width: 75px">
                                <asp:Image ID="imgError" runat="server" ImageUrl="../images/Exclamation.gif" />
                            </td>
                            <td align="center" valign="middle" class="ClsErrLghtGrayBG" rowspan="3" style="width: 450px">
                                <asp:Label CssClass="ClsErrTxt" ID="lblPageHeader" runat="server" BorderWidth="0px"
                                    EnableViewState="False" Height="20px">Your session has timed out.</asp:Label>
                                <%-- <span class="ClsErrTxt">Your session has been timed out.</span>--%>
                                <asp:Label CssClass="ClsErrTxt" ID="lblLogin" runat="server" BorderWidth="0px" EnableViewState="False"
                                    Height="20px">Please Login again.</asp:Label>
                                <asp:Label CssClass="ClsErrTxt" ID="lblNavigateToDashboard" runat="server" BorderWidth="0px" EnableViewState="False"
                                    Height="20px">Click <a title="here" href="ControlPanel.aspx">here</a> to see dashboard.</asp:Label>                                
                                <%--<span class="ClsErrTxt">Please Login again.</span>--%>
                            </td>
                        </tr>
                        <tr>
                            <td id="tdLogin" runat="server" align="right" valign="middle" class="ClsErrLghtGrayBG"
                                style="height: 39px">
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="ClsBtn" OnClick="btnLogin_Click"
                                    CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
            <%--<tr id="trDashboard" runat="Server" visible="false">
                <td style="background-color: white" id="MainDataTable" align="center">
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 650px" class="ClsBorderlight">
                        <tr>
                            <td  align="center" class="ClsErrLghtGrayBG">
                                <span runat="server" enableviewstate="False" class="ClsErrTxt" style="border-width: 0px;
                                    height: 20px;">Click <a title="here" href="ControlPanel.aspx">here</a> to go to the dashboard.</span>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
        </table>
    </div>
    <script type="text/javascript" language="javascript">
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
</asp:Content>
