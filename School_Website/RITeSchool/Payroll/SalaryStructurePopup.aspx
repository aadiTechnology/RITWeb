<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
    AutoEventWireup="true" CodeFile="SalaryStructurePopup.aspx.cs" Inherits="SalaryStructurePopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td>
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td class="ClsGrayMainTitle" align="left">
                            <asp:Label ID="Label2" runat="server" CssClass="MainTitleHead" Text="<%$ Resources:LocalizedResources, SalaryStructure%>"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="80%">
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="100px">
                                        <span class="ClsLabel">Name :</span>
                                    </td>
                                    <td class="ClsHilightBGB">
                                        <asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderlight" width="100px">
                                        <span class="ClsLabel">Photo :</span>
                                    </td>
                                    <td align="left">
                                        <img id="imgPhoto" alt="-" runat="server" height="151" width="119" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:ListView ID="lstvwEarningsDeductions" runat="server" DataKeyNames="EarningsDeductionsId"
                                OnItemDataBound="lstvwEarningsDeductions_ItemDataBound">
                                <LayoutTemplate>
                                    <table width="80%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                        cellspacing="1" class="GridBorder">
                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                            <th class="paddingLSML">
                                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, EarningDeductionName%>"></asp:Label>
                                            </th>
                                            <th align="right" style="padding-right: 5px;">
                                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>"></asp:Label>
                                            </th>
                                        </tr>
                                        <tr id="itemPlaceholder" runat="server">
                                        </tr>
                                    </table>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr id="trItem" runat="server" class="ClsGridRow">
                                        <td class="paddingLSML">
                                            <asp:Label ID="lblShortName" runat="server" Text='<%#Eval("ShortName") %>'></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-right: 5px;" Text='<%# Convert.ToInt32(Eval("EarningsDeductionsValue")) %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <AlternatingItemTemplate>
                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                        <td class="paddingLSML">
                                            <asp:Label ID="lblShortName" runat="server" Text='<%#Eval("ShortName") %>'></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lblAmount" runat="server" Style="padding-right: 5px;" Text='<%# Convert.ToInt32(Eval("EarningsDeductionsValue")) %>'></asp:Label>
                                        </td>
                                    </tr>
                                </AlternatingItemTemplate>
                            </asp:ListView>
                        </td>
                    </tr>
                    <tr style="height: 10px;">
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close%>"
                                CssClass="ClsBtn" OnClientClick="ClosePopup()" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript" language="javascript">
        function ClosePopup() {
            window.close();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
