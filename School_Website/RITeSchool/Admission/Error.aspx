<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error"
    MasterPageFile="~/RITeSchool/MasterPages/OnlineAdmission.master" %>

<%@ OutputCache Location="None" VaryByParam="None" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td style="background-color: white" id="MainDataTable" align="center">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 55%" class="ClsBorderlight">
                        <tr>
                            <td align="center" class="ClsErrGrayBG" rowspan="3" valign="top">
                                <asp:Image ID="imgError" runat="server" ImageUrl="../images/Exclamation.gif" /></td>
                            <td align="center" valign="middle" class="ClsErrLghtGrayBG" rowspan="3">
                                <asp:Label CssClass="ClsErrTxt" ID="lblPageHeader" runat="server" 
                                    BorderWidth="0px" EnableViewState="False">Your session has been timed out.</asp:Label>
                                <asp:Label CssClass="ClsErrTxt" ID="Label3" runat="server" BorderWidth="0px" 
                                    EnableViewState="False">Please Login again.</asp:Label>
                            </td>
                        </tr>
                        
                        <tr>
                           
                            <td align="right" valign="middle" class="ClsErrLghtGrayBG" >
                                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="ClsBtn" 
                                    OnClick="btnLogin_Click" CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                    <!-- Data Insert End Here -->
                </td>
            </tr>
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
