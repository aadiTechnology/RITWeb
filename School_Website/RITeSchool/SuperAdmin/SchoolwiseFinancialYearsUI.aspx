<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="SchoolwiseFinancialYearsUI.aspx.cs" Inherits="SchoolwiseFinancialYearUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <asp:UpdatePanel ID="upnl1" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblSuccess" CssClass="LblNrmlB" EnableViewState="false" ForeColor="Blue" runat="server"
                                        Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:ListView ID="lstvwFinancialYears" runat="server" OnItemDataBound="lstvwFinancialYears_ItemDataBound"
                                        DataKeyNames="FinancialYearId" OnItemCommand="lstvwFinancialYears_ItemCommand">
                                        <LayoutTemplate>
                                            <table align="center" width="700px" class="GridBorder" cellspacing="1" cellpadding="3">
                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" style="font-size: 9pt; width: 100px;">
                                                        Current Year
                                                    </th>
                                                    <th align="center" style="font-size: 9pt; width: 175px;">
                                                        <asp:LinkButton ID="lnkStartDate" runat="server" CommandArgument="StartDate" CommandName="SORT_ROW"
                                                            CausesValidation="false" ForeColor="Black" Text="Start Date" />
                                                    </th>
                                                    <th align="center" style="font-size: 9pt; width: 150px;">
                                                        <asp:LinkButton ID="lnkbtnEndDate" runat="server" CommandArgument="EndDate" CommandName="SORT_ROW"
                                                            CausesValidation="false" ForeColor="Black" Text="End Date" />
                                                    </th>
                                                    <th align="center" style="font-size: 9pt; width: 70px;">
                                                        <span>Close Year</span>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="center">
                                                    <asp:RadioButton ID="optCurrentYear" runat="server" onclick="UnchekAll(this);" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkClosed" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:RadioButton ID="optCurrentYear" runat="server" onclick="UnchekAll(this);" />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# Eval("StartDate") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkClosed" runat="server" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <div class="LblNoRecord" style="margin: 10px 0; width:500px;">
                                                No record found.</div>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" PostBackUrl="~/RITeSchool/SuperAdmin/ScreensUI.aspx" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <script type="text/javascript">
        function UnchekAll(obj) {            
            $("input:radio").removeAttr("checked");
            obj.checked = true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
