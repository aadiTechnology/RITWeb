<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ItemImageUI.aspx.cs" Inherits="RITeSchool_Inventory_ItemImageUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div>
        <table>
            <tr>
                <td>
                    <asp:Image ID="imgItem1" runat="server" Width="50px" Height="50px" style="cursor:pointer" />
                </td>
                <td>
                    <asp:Image ID="imgItem2" runat="server" Width="50px" Height="50px" style="cursor:pointer" />
                </td>
                <td>
                    <asp:Image ID="imgItem3" runat="server" Width="50px" Height="50px" style="cursor:pointer"/>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
