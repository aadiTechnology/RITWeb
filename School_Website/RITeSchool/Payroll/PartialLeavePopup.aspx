<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="PartialLeavePopup.aspx.cs" Inherits="PartialLeavePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <div class="MainBodyDiv" style="vertical-align: top;height:570px">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; vertical-align: top;">
            <tr>
                <td align="left" style="vertical-align:top;">
                    <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="height: 20px" class="ClsGrayMainTitle">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                    <tr>
                                        <td align="center" class="MainTitleHead" style="height: 20px">
                                            <span style="font-weight: bold">Partial Leaves</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="height20">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center">
                        <tr>
                            <td align="left" class="ClsBorderlight" style="width: 45px;">
                                <asp:Label ID="Label" runat="server" CssClass="ClsLabel" Text="User: " Font-Bold="True"></asp:Label>
                            </td>
                            <td width="1%">
                            </td>
                            <td align="left">
                                <asp:Label ID="lblUserName" runat="server" Text="User" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="height20">
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table align="center" width="100%" align="center">
                        <tr>
                            <td align="center" class="ClsHilightBGB">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Partial leave(s) are allowed only for existing half leave(s). Here partial leave(s) are set temporarily and will be permanently saved on User Leave popup."
                                    EnableViewState="False"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="height20">
                </td>
            </tr>
            <tr>
                <td align="center" valign="top">
                    <asp:ListView ID="lstvwPartialLEaves" runat="server" DataKeyNames="DatewisePartialStaffLeavesId,PartialLeaveId,ExistingLeaveId"
                        OnItemDataBound="lstvwPartialLEaves_ItemDataBound">
                        <LayoutTemplate>
                            <table width="50%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                cellspacing="1" class="GridBorder">
                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                    <th align="center" class="locked">
                                        Existing Leave
                                    </th>
                                    <th align="center" class="locked">
                                        Date
                                    </th>
                                    <th align="center" class="locked">
                                        Partial Leave
                                    </th>
                                </tr>
                                <tr id="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="trItem" runat="server" class="ClsGridRow">
                                <td class="paddingL" align="center">
                                    <asp:Label ID="lblExistingLeave" runat="server" Width="200px" Text='<%#Eval("ShortName") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLeaveDate" runat="server" Width="200px" Text='<%#Eval("LeaveDate") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="cmbLeave" Width="120px" runat="server" CssClass="MidCombo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr id="trAltItem" runat="server" class="ClsGridAltRow">
                                <td class="paddingL" align="center">
                                    <asp:Label ID="lblExistingLeave" runat="server" Width="200px" Text='<%#Eval("ShortName") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblLeaveDate" runat="server" Width="200px" Text='<%#Eval("LeaveDate") %>'></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:DropDownList ID="cmbLeave" Width="120px" runat="server" CssClass="MidCombo">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr>
                            <td align="right">
                                <asp:Button ID="btnSet" runat="server" Text="Set" CssClass="ClsBtn" disable-page="true"
                                    onclick="btnSet_Click" />
                            </td>                           
                            <td align="left">
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" />
                            </td>
                        </tr>
                        <asp:HiddenField ID="hidMonthId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidYear" runat="server" Value="0" />
                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidStaffGroupId" runat="server" Value="0" />
                        <asp:HiddenField ID="hidFilter" runat="server" Value="" />
                    </table>
                </td>
            </tr>
        </table>
    </div>   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
