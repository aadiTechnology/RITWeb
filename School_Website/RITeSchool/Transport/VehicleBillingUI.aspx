<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="VehicleBillingUI.aspx.cs" Inherits="VehicleBillingUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="right">
                <span class="ClsMdtStar">* Mandatory fields.</span>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="left" class="ClsBorderLight" style="width: 100px;">
                            <span class="clsLabel">Vehicle No. : </span>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="cmbVehicles" runat="server" CssClass="ExLrgCombo">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight">
                            <span class="clsLabel">Start Date : </span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtStartDate" CssClass="MidTxtBox" runat="server" />
                            <rjs:PopCalendar ID="calPassingDate" runat="server" Control="txtStartDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start date should not be blank."
                                AutoPostBack="False" To-Today="true" />
                            <span class="ClsMdtStar">* </span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderLight">
                            <span class="clsLabel">End Date : </span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtEndDate" CssClass="MidTxtBox" runat="server"  />
                            <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtEndDate" Format="dd MMM yyyy"
                                Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="End date should not be blank."
                                AutoPostBack="False" To-Today="true" />
                            <span class="ClsMdtStar">* </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" 
                                onclick="btnShow_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                           <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" Visible="false" CausesValidation="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>        
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
