<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AssemblyQuestionConfigurationUI.aspx.cs" Inherits="AssemblyQuestionConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%" align="center">
            <tr align="left">
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ErrMsg" />
                </td>
            </tr>
            <tr align="center">
                <td align="center" id="tdMessage" runat="server" colspan="2">
                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" CssClass="LblNormal"
                        ForeColor="Blue" Style="font-weight: bold"></asp:Label>
                </td>
            </tr>
            <tr align="center">
                <td align="center">
                    <table width="80%" align="center">
                        <tr align="center">
                            <td class="ClsBorderlight" style="width:200px;">
                                <asp:Label ID="lblDate" runat="server" CssClass="clsLabel" Text="Name"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:TextBox ID="txtName" CssClass="" Width="250px" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="ClsBorderlight">
                                <asp:Label ID="Label1" runat="server" CssClass="clsLabel" Text="Group"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:DropDownList ID="cmbGroups" CssClass="MidTxtBox" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="ClsBorderlight">
                                <asp:Label ID="Label2" runat="server" CssClass="clsLabel" Text="Sort Order"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:TextBox ID="TextBox1" CssClass="MidTxtBox" runat="server" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="ClsBorderlight">
                                <asp:Label ID="Label3" runat="server" CssClass="clsLabel" Text="Parent Question"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:DropDownList ID="cmbParentQuestion" CssClass="MidTxtBox" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr align="center">
                            <td class="ClsBorderlight">
                                <asp:Label ID="Label4" runat="server" CssClass="clsLabel" Text="Allow Free Text"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="right" style="text-align: left;">
                                <asp:CheckBox ID="chkAllowFreeText" runat="server" />
                            </td>
                        </tr>
                        <tr align="center">
                            <td style="height: 10px;" colspan="2">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" 
                                    UseSubmitBehavior="false" onclick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" UseSubmitBehavior="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
