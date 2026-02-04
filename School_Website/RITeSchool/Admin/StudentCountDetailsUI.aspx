<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    ViewStateMode="Disabled" AutoEventWireup="true" CodeFile="StudentCountDetailsUI.aspx.cs"
    Inherits="StudentCountDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
            <tr>
                <td align="left">
                    <asp:ValidationSummary ID="valSum" runat="server" />
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td class="ClsBorderlight">
                                <span class="clsLabel">Academic Year : </span>
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="cmbAcademicYear" runat="server" ViewStateMode="Enabled" CssClass="MidCombo">
                                </asp:DropDownList>
                                <span class="ClsMdtStar">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:Button ID="btnShow" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnShow_Click" />
                                <asp:RequiredFieldValidator ID="reqAcademicYear" runat="server" Display="None" ErrorMessage="Academic Year should be selected."
                                    ControlToValidate="cmbAcademicYear" InitialValue="0"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
