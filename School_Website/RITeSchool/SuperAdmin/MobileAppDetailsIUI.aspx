<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="MobileAppDetailsIUI.aspx.cs" Inherits="MobileAppDetailsIUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table style="width: 100%;">
            <tr>
                <td style="height: 20px;">
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 50%; text-align: center; margin: 0px auto;"
                        align="center">
                        <tr>
                            <td style="width: 220px; height:25px;" align="left" class="ClsBorderLight">
                                <asp:Label ID="lblDOBMax" runat="server" CssClass="ClsLabel" Font-Size="14px" Text="Mobile App download Count"></asp:Label>
                                <span class="ClsLabel colonPadding">:</span>
                            </td>
                            <td align="left" class="ClsBorderLight" style="Font-Size:14px; font-weight:bold; padding-left : 10px;">
                                <asp:Label ID="lblStudentCount" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px;">
                            </td>
                        </tr>
                        <tr>
                            <td align="center" colspan="2">
                                <asp:ListView ID="lstvwLoginDetails" runat="server">
                                    <LayoutTemplate>
                                        <table width="100%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                <th align="left" width="300px" class="clsLabelgrd">
                                                    <span><b>Month</b></span>
                                                </th>
                                                <th align="center" width="150px" class="clsLabelgrd">
                                                    <span><b>Mobile Login User Count</b></span>
                                                </th>
                                                <th align="center" class="clsLabelgrd" width="150px">
                                                    <span><b>Website Login User Count</b></span>
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:Label ID="lblStanderd" runat="server" CssClass="ClsLabel" Text='<%#Eval("MonthName") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("MobileUserCount") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("WebsiteUserCount") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("MonthName") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblFormOpenDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("MobileUserCount") %>'></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:Label ID="lblFormCloseDate" runat="server" CssClass="ClsLabel" Style="float: inherit"
                                                    Text='<%#Eval("WebsiteUserCount") %>'></asp:Label>
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                </asp:ListView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
