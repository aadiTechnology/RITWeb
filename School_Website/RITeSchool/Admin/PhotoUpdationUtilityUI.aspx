<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PhotoUpdationUtilityUI.aspx.cs" Inherits="PhotoUpdationUtilityUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <asp:ValidationSummary ID="ValSum" runat="server" CssClass="clsLabel" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table style="margin: 10px auto;">
                    <tr>
                        <td colspan="4" id="tdMessage" runat="server">
                            <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="ClsLabelNrml"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderlight">
                            <span class="clsLabel">Folder Path :</span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPath" runat="server" Width="300px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Folder path should not be blank."
                                Display="None" ControlToValidate="txtPath"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="ClsBtn" OnClick="btnUpdate_Click" />
                        </td>
                        <td>
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="ClsBtn" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
            </td>
        </tr>
    </table>
    <script>
        var _clienttxtPath = "<%=this.txtPath.ClientID %>"
        function ClearText() {
            $('#' + _clienttxtPath).val('')
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
