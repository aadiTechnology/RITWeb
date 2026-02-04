<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentLeavingCertificateUI.aspx.cs" Inherits="StudentLeavingCertificateUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table style="width: 100%; text-align: center; margin: 0px auto;" align="center">
        <tr>
            <td style="height: 20px;" colspan="2">
            </td>
        </tr>
        <tr align="center" style="text-align:center; margin:0px auto;">
            <td align="center" colspan="2" style="text-align:center; margin:0px auto;">
                <table id="tblLCDetails" runat="server" style="text-align:center; margin:0px auto;">
                    <tr>
                        <td align="center">
                            <table>
                                <tr align="center" style="text-align: center; margin: 0px auto;">
                                    <td class="ClsBorderlight" style="width: 150px;">
                                        <asp:Label ID="lblNameRegNo" runat="server" class="ClsLabel" Text="TC downloaded by"></asp:Label>
                                        <span class="ClsLabel colonpadding">:</span>
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rdoParent" runat="server" Text="Parent" GroupName="TC" />
                                        <asp:RadioButton ID="rdoSchool" runat="server" Text="School" GroupName="TC" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px;">
                        </td>
                    </tr>
                    <tr align="center" style="text-align: center; margin: 0px auto;">
                        <td align="center" style="vertical-align: top; width: 100px;" colspan="2">
                            <asp:Button ID="btnReport" runat="server" Text="Download TC" CssClass="ClsBtn"
                                Width="99px" CausesValidation="true" ValidationGroup="Save" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>  
        <tr id="trPendingFee" runat="server" visible="false" align="center" style="text-align:center; margin:0px auto;">
            <td class="ClsBorderlight" align="center" style="text-align:center; margin:0px auto;" colspan="2">
                <table align="center" style="text-align: center; margin: 0px auto;">
                    <tr align="center" style="text-align: center;
                        margin: 0px auto;">
                        <td align="center" style="font-size: 12px; font-weight: bold; color: Blue; text-align: center;
                            margin: 0px auto;">
                            <asp:Label ID="Label1" runat="server" class="ClsLabel" Text="Please clear all the school dues."></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>   
        <tr>
            <td colspan="2">
                <asp:HiddenField ID="hidFileName" runat="server" Value="" />
            </td>
        </tr>   
    </table>

    <script language="javascript" type="text/javascript">

        function OpenDocument(FilePath) {
            window.open("../DOWNLOADS/User Documents/FormNo16/" + FilePath, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=700');
            return false;
        }

        </script>

</asp:Content>
