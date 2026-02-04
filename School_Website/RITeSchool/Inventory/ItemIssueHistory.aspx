<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="ItemIssueHistory.aspx.cs" Inherits="ItemIssueHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel4">
            <ContentTemplate>
                <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                    <tr id="trLstItems" runat="server" visible="true">
                        <td>
                            <table width="80%" align="center">
                                <tr>
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstvwItems">
                                            <Fields>
                                                <asp:TemplatePagerField>
                                                    <PagerTemplate>
                                                        <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                        <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                        <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                            CssClass="LblNrmlB" />
                                                        <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                        <br />
                                                    </PagerTemplate>
                                                </asp:TemplatePagerField>
                                            </Fields>
                                        </asp:DataPager>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <div>
                                            <asp:ListView ID="lstvwItems" runat="server" OnDataBound="lstvwItems_DataBound" OnSorting="lstvwItems_Sorting">
                                                <LayoutTemplate>
                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="Tr1" runat="server" class="ClsGridHeader">
                                                            <th align="left" class="ClspaddingL">
                                                                Item Code
                                                            </th>
                                                            <th align="left" class="ClspaddingL">
                                                                Item Name
                                                            </th>
                                                            <th align="center">
                                                                Item Quantity
                                                            </th>
                                                            <th align="center">
                                                                Item Issued Date
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                        <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                            <td colspan="4">
                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstvwItems">
                                                                    <Fields>
                                                                        <asp:TemplatePagerField>
                                                                            <PagerTemplate>
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="right" class="LblNormal">
                                                                                            <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </PagerTemplate>
                                                                        </asp:TemplatePagerField>
                                                                    </Fields>
                                                                </asp:DataPager>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr class="ClsGridRow">
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                        </td>
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                        </td>
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                        </td>
                                                        <td align="center" class="Clspadding">
                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Issued_Date","{0:dd-MMM-yyyy}")%>' />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr class="ClsGridAltRow">
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                        </td>
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                        </td>
                                                        <td align="left" class="ClspaddingL">
                                                            <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                        </td>
                                                        <td align="center" class="Clspadding">
                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Issued_Date","{0:dd-MMM-yyyy}")%>' />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td class="LblNoRecord" align="center">
                                                                No record found.
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                            </asp:ListView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ObjectDataSource TypeName="BusinessLogic.RequisitionBL" EnablePaging="true"
                                            ID="lstDSobj" runat="server" SelectMethod="GetItemsHistory" SortParameterName="sortExpression"
                                            SelectCountMethod="CountRowsOfItemsHistory" EnableCaching="false">
                                            <SelectParameters>
                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                <asp:ControlParameter ControlID="hidRequistionId" PropertyName="Value" Name="asRequistionId" />
                                                <asp:ControlParameter ControlID="hidUserId" PropertyName="Value" Name="asUserId" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btnClose" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                Text="Close" Visible="True" Width="80px" CausesValidation="false" OnClick="btnClose_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hidRequistionId" runat="server" />
                            <asp:HiddenField ID="hidUserId" runat="server" />
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script language="javascript" type="text/javascript">
        function refreshParent() {
            window.close();
        }
    </script>

</asp:Content>
