<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" 
CodeFile="BonafideRequestApplicationUI.aspx.cs" Inherits="BonafideRequestApplicationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" Runat="Server">
<div class="MainBodyDiv">
    <table width="100%">
        <tr>
            <td align="center">
                <asp:Button ID="btnDownload" runat="server" CssClass="ClsBtn" Text="Download Bonafide Request Application" OnClick="btnDownload_Click" />
            </td>
        </tr>
    </table>
</div>

</asp:Content>

