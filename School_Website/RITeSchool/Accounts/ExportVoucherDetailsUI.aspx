<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ExportVoucherDetailsUI.aspx.cs" Inherits="ExportVoucherDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table id="tblMain" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
        <tr>
            <td align="right">
                <span class="LblNormal ClsMdtStar">* Mandatory Fields</span>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:ValidationSummary ID="valsum" runat="server" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Deposited Bank should be selected."
                    ControlToValidate="cmbDepostedBank" InitialValue="0" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Start Date should not be blank."
                    ControlToValidate="txtStartDate" Display="None"></asp:RequiredFieldValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="End Date should not be blank."
                    ControlToValidate="txtEndDate" Display="None"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="left" class="clsBorderLight">
                            <span class="ClsLabel">Start Date : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                            <rjs:PopCalendar ID="dtStartDate" runat="server" Control="txtStartDate" Format="dd mmm yyyy"
                                ShowWeekend="True" To-Today="true" ShowErrorMessage="false" />
                            <span class="ClsMdtStar">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="clsBorderLight">
                            <span class="ClsLabel">End Date : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="SmlTxtBox"></asp:TextBox>
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd mmm yyyy"
                                ShowWeekend="True" To-Today="true" ShowErrorMessage="false" />
                            <span class="ClsMdtStar">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="clsBorderLight">
                            <span class="ClsLabel">Deposited Bank : </span>
                        </td>
                        <td>
                            <asp:DropDownList ID="cmbDepostedBank" runat="server" CssClass="LrgCombo">
                            </asp:DropDownList>
                            <span class="ClsMdtStar">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" OnClick="btnExport_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
