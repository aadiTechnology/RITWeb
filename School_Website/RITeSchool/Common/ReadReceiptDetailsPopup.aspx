<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="ReadReceiptDetailsPopup.aspx.cs" Inherits="ReadReceiptDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td align="center">
                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="98%">
                    <tr>
                        <td style="height: 20px" class="ClsGrayMainTitle">
                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                <tr>
                                    <td align="left" class="MainTitleHead" style="height: 20px">
                                        <span style="font-weight: bold">Read Receipt Information</span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height:10px">
        </tr>
        <tr>
            <td align="center">
                <asp:ListView ID="lstvwUsers" runat="server">
                    <LayoutTemplate>
                        <table width="90%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                <th align="left" class="clsLabelgrd">
                                    User Role
                                </th>
                                <th align="left" class="clsLabelgrd">
                                    User Name
                                </th>
                                <th align="left" class="clsLabelgrd">
                                    Class Name
                                </th>
                                <th width="150px" align="center">
                                    Read Date/Time
                                </th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="Tr2" runat="server" class="ClsGridRow">
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserRole") %>'></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%#Eval("ClassName") %>'></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblReadDateTime" runat="server" Text='<%#Eval("ReadingDatetime") %>'></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                            <td align="left">
                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserRole") %>'></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Text='<%#Eval("UserName") %>'></asp:Label>
                            </td>
                            <td align="left">
                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%#Eval("ClassName") %>'></asp:Label>
                            </td>
                            <td align="center">
                                <asp:Label ID="lblReadDateTime" runat="server" Text='<%#Eval("ReadingDatetime") %>'></asp:Label>
                            </td>
                        </tr>
                    </AlternatingItemTemplate>
                    <EmptyDataTemplate>
                        <tr>
                            <td class="LblNoRecord" align="center">
                                No record found.
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                </asp:ListView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
