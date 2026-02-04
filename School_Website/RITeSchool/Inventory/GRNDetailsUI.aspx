<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GRNDetailsUI.aspx.cs" Inherits="GRNDetailsUI"
    Title="Untitled Page" %>

<asp:Content ID="CntGRNDetails" ContentPlaceHolderID="MainBody" runat="Server">
    <center>
        <asp:UpdatePanel ID="UPanelItemSearch" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td align="left" valign="top">
                            <table width="100%">
                                <tr>
                                    <td colspan="1" align="left" valign="top">
                                        <asp:ValidationSummary ID="valsumGRN" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                            ShowSummary="True" ValidationGroup="valsumGRN" />
                                        <asp:ValidationSummary ID="valReqQty" runat="server" ShowMessageBox="true" ShowSummary="false"
                                            CssClass="ClsLabel" ValidationGroup="ReqAdd" />
                                        <asp:CustomValidator ID="cstvalQty" runat="server" CssClass="ClsLabel" ClientValidationFunction="ValidateQuantity"
                                            Display="Dynamic" ValidateEmptyText="True" ValidationGroup="Add"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstvalPOQty" runat="server" ClientValidationFunction="ValidatePOQty"
                                            Display="Dynamic" ValidateEmptyText="True" CssClass="ClsLabel" ValidationGroup="ReqAdd"></asp:CustomValidator>
                                    </td>
                                    <td align="right" valign="top">
                                        <asp:Label ID="lblMandatory" runat="server" ForeColor="Red" CssClass="LblNormalImg"
                                            EnableViewState="false">* Mandatory Fields</asp:Label>
                                    </td>
                                </tr>
                                <tr id="trViewOptionButton" runat="server" style="width: 100%">
                                    <td class="ClsBorderlight" style="width: 50%">
                                        <asp:RadioButton ID="optItemWise" runat="server" GroupName="grpOption" Text="Item Wise"
                                            OnCheckedChanged="optItemWise_CheckedChanged" AutoPostBack="True" />
                                    </td>
                                    <td class="ClsBorderlight">
                                        <asp:RadioButton ID="optPOWise" runat="server" GroupName="grpOption" Text="Purchase Order Wise"
                                            OnCheckedChanged="optPOWise_CheckedChanged" AutoPostBack="True" />
                                    </td>
                                </tr>
                                <tr id="trItemWiseDetails" runat="server">
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpanelItemList" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="trItemCount" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstvwItemWiseDetails"
                                                                Visible="True">
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
                                                            <asp:ListView ID="lstvwItemWiseDetails" runat="server" DataKeyNames="ItemID,UOMUnit,PieceCount"
                                                                OnDataBound="lstvwItemWiseDetails_DataBound" OnItemDataBound="lstvwItemWiseDetails_ItemDataBound"
                                                                OnSorting="lstvwItemWiseDetails_Sorting" OnItemCommand="lstvwItemWiseDetails_ItemCommand">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th id="thItemCode" runat="server" align="left" class="ClspaddingL" width="13%">
                                                                                <asp:LinkButton ID="lnkItemCode" runat="server" CommandName="Sort" CommandArgument="ItemCode"
                                                                                    ForeColor="Black">
                                                                                            Item Code</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thItemName" runat="server" align="left" class="ClspaddingL" width="40%">
                                                                                <asp:LinkButton ID="lnkItemName" runat="server" CommandName="Sort" CommandArgument="ItemName"
                                                                                    ForeColor="Black">
                                                                                            Item Name</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thItemQty" runat="server" align="left" class="ClspaddingL" width="18%">
                                                                                <asp:LinkButton ID="lnkItemQty" runat="server" CommandName="Sort" CommandArgument="ItemQty"
                                                                                    ForeColor="Black">
                                                                                            Item Quantity</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thPOCount" runat="server" align="center" width="14%">
                                                                                <asp:LinkButton ID="lnkPOCount" runat="server" CommandName="Sort" CommandArgument="POCount"
                                                                                    ForeColor="Black">
                                                                                            PO Count</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thAdd" runat="server" align="center" width="15%">
                                                                                Add
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" id="trDataPager">
                                                                            <td colspan="6">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstvwItemWiseDetails">
                                                                                    <Fields>
                                                                                        <asp:TemplatePagerField>
                                                                                            <PagerTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                                    <tr id="trItemWise" runat="server" class="ClsGridRow">
                                                                        <td align="left" id="tdItemCode" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdItemName" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdItemQty" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center" id="tdPOCount">
                                                                            <asp:Label ID="lblPOCount" runat="server" Text='<%# Eval("POCount")%>'></asp:Label>
                                                                        </td>
                                                                        <td id="tdAdd" runat="server" align="center">
                                                                            <asp:ImageButton ID="imgbtnAddItem" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                ToolTip="Add" CommandName="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("ItemID") %>' Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trItemWise" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" id="tdItemCode" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdItemName" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdItemQty" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center" id="tdPOCount">
                                                                            <asp:Label ID="lblPOCount" runat="server" Text='<%# Eval("POCount")%>'></asp:Label>
                                                                        </td>
                                                                        <td id="tdAdd" runat="server" align="center">
                                                                            <asp:ImageButton ID="imgbtnAddItem" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                ToolTip="Add" CommandName="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("ItemID") %>' Visible="false" />
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
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.GRNDetailsBL" EnablePaging="true" ID="objDSPODetails"
                                                                runat="server" SelectMethod="GetPODetails" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountItemsInPO" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter Name="asGRNId" Type="String" ControlID="hidGRNId" PropertyName="Value"
                                                                        DefaultValue="0" />
                                                                    <asp:ControlParameter Name="abItemWise" Type="Boolean" ControlID="optItemWise" PropertyName="Checked"
                                                                        DefaultValue="False" />
                                                                    <asp:ControlParameter Name="abPOWise" Type="Boolean" ControlID="optPOWise" PropertyName="Checked"
                                                                        DefaultValue="False" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="DataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="ItemDataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="optItemWise" EventName="CheckedChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="optPOWise" EventName="CheckedChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel ID="UpanelPOList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="trPOCount" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPOPgCount" runat="server" PageSize="5" PagedControlID="lstvwPOWiseDetails"
                                                                Visible="True">
                                                                <Fields>
                                                                    <asp:TemplatePagerField>
                                                                        <PagerTemplate>
                                                                            <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                            <asp:Label runat="server" ID="TotalPOsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                CssClass="LblNrmlB" />
                                                                            <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                            <br />
                                                                        </PagerTemplate>
                                                                    </asp:TemplatePagerField>
                                                                </Fields>
                                                            </asp:DataPager>
                                                        </td>
                                                    </tr>
                                                    <tr id="trPOWiseDetails" runat="server">
                                                        <td valign="top">
                                                            <asp:ListView ID="lstvwPOWiseDetails" runat="server" DataKeyNames="PurchaseOrderID"
                                                                OnDataBound="lstvwPOWiseDetails_DataBound" OnSorting="lstvwPOWiseDetails_Sorting"
                                                                OnItemCommand="lstvwPOWiseDetails_ItemCommand" OnItemDataBound="lstvwPOWiseDetails_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th id="thPOCode" runat="server" align="left" class="ClspaddingL" width="12%">
                                                                                <asp:LinkButton ID="lnkPOCode" runat="server" CommandName="Sort" CommandArgument="PurchaseOrderCode"
                                                                                    ForeColor="Black">
                                                                                            PO Code</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thPOName" runat="server" align="left" class="ClspaddingL" width="32%">
                                                                                <asp:LinkButton ID="lnkPOName" runat="server" CommandName="Sort" CommandArgument="POName"
                                                                                    ForeColor="Black">
                                                                                            PO Name</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thPODate" runat="server" align="center" width="15%">
                                                                                <asp:LinkButton ID="lnkPODate" runat="server" CommandName="Sort" CommandArgument="Insert_Date"
                                                                                    ForeColor="Black">
                                                                                            Creation Date</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thPOCreatorName" runat="server" align="left" class="ClspaddingL" width="26%">
                                                                                <asp:LinkButton ID="lnkPOCreatorName" runat="server" CommandName="Sort" CommandArgument="CreaterName"
                                                                                    ForeColor="Black">
                                                                                            Creator Name</asp:LinkButton>
                                                                            </th>
                                                                            <th id="thAdd" runat="server" align="center" width="15%">
                                                                                Add
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="ItemPlaceholder" runat="server">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" id="trDataPager">
                                                                            <td colspan="6">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstvwPOWiseDetails">
                                                                                    <Fields>
                                                                                        <asp:TemplatePagerField>
                                                                                            <PagerTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPOCnt_SelectedIndexChanged">
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
                                                                    <tr id="trPOWise" runat="server" class="ClsGridRow">
                                                                        <td align="left" id="tdPOCode" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("PurchaseOrderCode")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdPOName" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOName" runat="server" Text='<%# Eval("POName")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center" id="tdPODate">
                                                                            <asp:Label ID="lblPODate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdPOCreator" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCreator" runat="server" Text='<%# Eval("CreaterName")%>'></asp:Label>
                                                                        </td>
                                                                        <td id="tdAdd" runat="server" align="center">
                                                                            <asp:ImageButton ID="imgbtnAddPO" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                ToolTip="Add" CommandName="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("PurchaseOrderID") %>' Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trPOWise" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" id="tdPOCode" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("PurchaseOrderCode")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdPOName" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOName" runat="server" Text='<%# Eval("POName")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="center" id="tdPODate">
                                                                            <asp:Label ID="lblPODate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                                        </td>
                                                                        <td align="left" id="tdPOCreator" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCreator" runat="server" Text='<%# Eval("CreaterName")%>'></asp:Label>
                                                                        </td>
                                                                        <td id="tdAdd" runat="server" align="center">
                                                                            <asp:ImageButton ID="imgbtnAddPO" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                ToolTip="Add" CommandName="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("PurchaseOrderID") %>' Visible="false" />
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
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="DataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="ItemDataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="optPOWise" EventName="CheckedChanged" />
                                                <asp:AsyncPostBackTrigger ControlID="optPOWise" EventName="CheckedChanged" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="2" id="trModify">
                                        <asp:Button ID="btnModify" runat="server" CssClass="ClsBtn" Text="Modify" OnClick="btnModify_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UPanelPOItems">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="2" style="width: 100%">
                                                    <tr id="trlstvwPOItems" runat="server" visible="true">
                                                        <td valign="top">
                                                            <asp:ListView ID="lstvwPOItems" runat="server" DataKeyNames="PurchaseOrderID,ItemID,Unit,PieceCount"
                                                                OnItemCommand="lstvwPOItems_ItemCommand" OnItemDataBound="lstvwPOItems_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="0" class="GridBorder">
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th id="thPOCode" runat="server" align="left" class="ClspaddingL" width="10%">
                                                                                            PO Code
                                                                                        </th>
                                                                                        <th id="thItemCode" runat="server" align="left" class="ClspaddingL" width="10%">
                                                                                            Item Code
                                                                                        </th>
                                                                                        <th id="thItemName" runat="server" align="left" class="ClspaddingL" width="15%">
                                                                                            Item Name
                                                                                        </th>
                                                                                        <th id="thPOQty" runat="server" class="ClspaddingL" width="12%">
                                                                                            PO Quantity
                                                                                        </th>
                                                                                        <th id="thAcceptedQty" runat="server" align="center" width="200px">
                                                                                            Accepted Quantity
                                                                                        </th>
                                                                                        <th id="th1" runat="server" class="ClspaddingL" width="16%">
                                                                                            Rejected Quantity
                                                                                        </th>
                                                                                        <th id="Th8" runat="server" class="ClspaddingL" width="14%">
                                                                                            Add to GRN
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="trPOItems" runat="server" class="ClsGridRow">
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("PurchaseOrderCode") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                        </td>
                                                                        <td class="ClspaddingL">
                                                                            <asp:Label ID="lblPOQty" runat="server" Text='<%# Eval("POQty") %>' />
                                                                            <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("POQty") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:TextBox ID="txtAcceptedQty" runat="server" onblur="extractNumber(this,3,false)"
                                                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                                                ondrop="event.returnValue=false" MaxLength="7" Text='<%# Eval("POQty") %>' Width="55%"></asp:TextBox>
                                                                            <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblRejectedQty" runat="server" />
                                                                            <asp:Label ID="lblRejectedUnit" runat="server" Text="Unit(s)" CssClass="ClspaddingR" />
                                                                        </td>
                                                                        <td align="center" valign="middle">
                                                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd"
                                                                                ToolTip="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("ItemID") %>' Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trPOItems" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("PurchaseOrderCode") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                        </td>
                                                                        <td class="ClspaddingL">
                                                                            <asp:Label ID="lblPOQty" runat="server" Text='<%# Eval("POQty") %>' />
                                                                            <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("POQty") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:TextBox ID="txtAcceptedQty" runat="server" onblur="extractNumber(this,3,false)"
                                                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                                                ondrop="event.returnValue=false" MaxLength="7" Text='<%# Eval("POQty") %>' Width="55%"></asp:TextBox>
                                                                            <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblRejectedQty" runat="server" />
                                                                            <asp:Label ID="lblRejectedUnit" runat="server" Text="Unit(s)" CssClass="ClspaddingR" />
                                                                        </td>
                                                                        <td align="center" valign="middle">
                                                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd"
                                                                                ToolTip="Add" /><br />
                                                                            <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From GRN" CommandName="Remove"
                                                                                CommandArgument='<%# Eval("ItemID") %>' Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnAddAll" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="8"
                                                                Text="Add All" OnClick="btnAddAll_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOItems" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwPOItems" EventName="ItemDataBound" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwGRN" EventName="ItemDataBound" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UPanelGRNDetails">
                                            <ContentTemplate>
                                                <table width="100%" id="Table2" runat="server" style="background-color: white; width: 100%;"
                                                    border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td valign="top">
                                                            <div>
                                                                <asp:ListView ID="lstvwGRN" runat="server" DataKeyNames="ItemID,ItemRejectedQty,ItemQtyDiff,ItemUnit,PieceCount"
                                                                    OnItemCommand="lstvwGRN_ItemCommand" OnItemDataBound="lstvwGRN_ItemDataBound">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                            cellspacing="0" class="GridBorder">
                                                                            <tr>
                                                                                <td>
                                                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                        cellspacing="1">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th id="thCode" runat="server" align="left" class="ClspaddingL" width="12%">
                                                                                                Item Code
                                                                                            </th>
                                                                                            <th id="thName" runat="server" align="left" class="ClspaddingL" width="53%">
                                                                                                Item Name
                                                                                            </th>
                                                                                            <th id="thCreaterName" runat="server" class="ClspaddingL" width="15%">
                                                                                                Quantity
                                                                                            </th>
                                                                                            <th id="Th6" runat="server" width="10%">
                                                                                                Details
                                                                                            </th>
                                                                                            <th id="thDelete" runat="server" width="10%">
                                                                                                Delete
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trItem" runat="server" class="ClsGridRow">
                                                                            <td align="left" id="tdCode" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="left" id="tdName" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                            </td>
                                                                            <td align="left" id="tdCreaterName" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                                                <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' />--%>
                                                                                <asp:HiddenField ID="hidItemQty" runat="server" Value='<%# Eval("ItemPOQty") %>' />
                                                                                <asp:HiddenField ID="hidUnitName" runat="server" Value='<%# Eval("ItemName") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Details" CommandName="Details"
                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                            </td>
                                                                            <td align="center" id="tdimgbtnDeleteItem">
                                                                                <asp:ImageButton ID="imgbtnDeleteItem" CommandArgument='<%# Eval("ItemID") %>' runat="server"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip="Delete" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trtxtQty" runat="server" visible="false">
                                                                            <td id="tdHideDetails" runat="server" colspan="1" align="left" valign="top">
                                                                                <asp:Button ID="btnHideDetails" runat="server" CssClass="ClsBtn" Text="Hide Details"
                                                                                    OnClick="btnHideDetails_Click" />
                                                                            </td>
                                                                            <td id="tdtxtQty" runat="server" colspan="3" style="padding-right: 10px;">
                                                                                <asp:ListView ID="lstVwItemDetails" runat="server" DataKeyNames="ItemID,POID,ItemOriginalQty,ItemUnit"
                                                                                    OnItemCommand="lstVwItemDetails_ItemCommand" OnItemDataBound="lstVwItemDetails_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder" align="center">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                        cellspacing="1">
                                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                            <th id="Th7" runat="server" align="left" class="ClspaddingL" width="15%">
                                                                                                                PO Code
                                                                                                            </th>
                                                                                                            <th id="Th2" runat="server" align="left" class="ClspaddingL" width="15%">
                                                                                                                Item Code
                                                                                                            </th>
                                                                                                            <th id="Th3" runat="server" align="left" class="ClspaddingL" width="20%">
                                                                                                                Item Name
                                                                                                            </th>
                                                                                                            <th id="Th5" runat="server" align="center" style="width: 33%">
                                                                                                                Quantity
                                                                                                            </th>
                                                                                                            <th id="thUpdate" runat="server" class="ClspaddingL" width="8%">
                                                                                                                Update
                                                                                                            </th>
                                                                                                            <th id="thDelete" runat="server" class="ClspaddingL" width="8%">
                                                                                                                Delete
                                                                                                            </th>
                                                                                                        </tr>
                                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("POCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,3,false)" onkeyup="extractNumber(this,3,false)"
                                                                                                    MaxLength="7" Text='<%# Eval("ItemGRNQty") %>'></asp:TextBox>
                                                                                                <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                                                </asp:DropDownList>
                                                                                                <%-- <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' CssClass="ClspaddingL" />--%>
                                                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("ItemOriginalQty") %>' />
                                                                                                <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("ItemOrgQty") %>' />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="Modify"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="Remove"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("POCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,3,false)" onkeyup="extractNumber(this,3,false)"
                                                                                                    MaxLength="7" Text='<%# Eval("ItemGRNQty") %>'></asp:TextBox>
                                                                                                <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                                                </asp:DropDownList>
                                                                                                <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' CssClass="ClspaddingL" />--%>
                                                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("ItemOriginalQty") %>' />
                                                                                                <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("ItemOrgQty") %>' />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="Modify"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="Remove"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" id="tdCode" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="left" id="tdName" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                            </td>
                                                                            <td align="left" id="tdCreaterName" runat="server" class="ClspaddingL">
                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                                                <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' />--%>
                                                                                <asp:HiddenField ID="hidItemQty" runat="server" Value='<%# Eval("ItemPOQty") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Details" CommandName="Details"
                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                            </td>
                                                                            <td align="center" id="tdimgbtnDeleteItem">
                                                                                <asp:ImageButton ID="imgbtnDeleteItem" CommandArgument='<%# Eval("ItemID") %>' runat="server"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip="Delete" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr id="trtxtQty" runat="server" visible="false">
                                                                            <td id="tdHideDetails" runat="server" colspan="1" align="left" valign="top">
                                                                                <asp:Button ID="btnHideDetails" runat="server" CssClass="ClsBtn" Text="Hide Details"
                                                                                    OnClick="btnHideDetails_Click" />
                                                                            </td>
                                                                            <td id="tdtxtQty" runat="server" colspan="4" align="right" style="padding-right: 10px;">
                                                                                <asp:ListView ID="lstVwItemDetails" runat="server" DataKeyNames="ItemID,POID,ItemOriginalQty,ItemUnit"
                                                                                    OnItemCommand="lstVwItemDetails_ItemCommand" OnItemDataBound="lstVwItemDetails_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1" class="GridBorder" align="center">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                                        cellspacing="1">
                                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                            <th id="Th7" runat="server" align="left" class="ClspaddingL" width="15%">
                                                                                                                PO Code
                                                                                                            </th>
                                                                                                            <th id="Th2" runat="server" align="left" class="ClspaddingL" width="15%">
                                                                                                                Item Code
                                                                                                            </th>
                                                                                                            <th id="Th3" runat="server" align="left" class="ClspaddingL" width="20%">
                                                                                                                Item Name
                                                                                                            </th>
                                                                                                            <th id="Th5" runat="server" align="center" style="width: 33%">
                                                                                                                Quantity
                                                                                                            </th>
                                                                                                            <th id="Th8" runat="server" class="ClspaddingL" width="8%">
                                                                                                                Update
                                                                                                            </th>
                                                                                                            <th id="Th11" runat="server" class="ClspaddingL" width="8%">
                                                                                                                Delete
                                                                                                            </th>
                                                                                                        </tr>
                                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("POCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,3,false)" onkeyup="extractNumber(this,3,false)"
                                                                                                    MaxLength="7" Text='<%# Eval("ItemGRNQty") %>'></asp:TextBox>
                                                                                                <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                                                </asp:DropDownList>
                                                                                                <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' CssClass="ClspaddingL" />--%>
                                                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("ItemOriginalQty") %>' />
                                                                                                <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("ItemOrgQty") %>' />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="Modify"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="Remove"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblPOCode" runat="server" Text='<%# Eval("POCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("UOMName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="ClspaddingL">
                                                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,3,false)" onkeyup="extractNumber(this,3,false)"
                                                                                                    MaxLength="7" Text='<%# Eval("ItemGRNQty") %>'></asp:TextBox>
                                                                                                <asp:DropDownList ID="cmbUnits" runat="server" CssClass="SmlCombo">
                                                                                                </asp:DropDownList>
                                                                                                <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("ItemUnit") %>' CssClass="ClspaddingL" />--%>
                                                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("ItemOriginalQty") %>' />
                                                                                                <asp:HiddenField ID="hidActualPOQty" runat="server" Value='<%# Eval("ItemOrgQty") %>' />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="Modify"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" />
                                                                                            </td>
                                                                                            <td align="center" valign="middle">
                                                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="Remove"
                                                                                                    CommandArgument='<%# Eval("ItemID") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                </asp:ListView>
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
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                        <asp:UpdatePanel runat="server" ID="UpanelGRNSave">
                                            <ContentTemplate>
                                                <table width="100%" id="tblSave" runat="server" style="width: 100%;" border="0" cellpadding="0"
                                                    cellspacing="0">
                                                    <tr id="trDesc" runat="server" visible="false" width="20%">
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="lblContent" runat="server" Font-Bold="True" Text="GRN Description:"
                                                                CssClass="ClsLabel" EnableViewState="false"></asp:Label>
                                                        </td>
                                                        <td align="left" width="50%">
                                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" TextMode="MultiLine"
                                                                CssClass="LrgTxtBox" Height="100px" TabIndex="3" Width="100%"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="reqtxtDescription" Display="None" runat="server"
                                                                ErrorMessage="GRN Description should not be blank." ControlToValidate="txtDescription"
                                                                ValidationGroup="valsumGRN" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="regtxtDescription" runat="server" Display="None"
                                                                ControlToValidate="txtDescription" ErrorMessage="GRN Description should be of length less than 300."
                                                                ValidationExpression="^[\s\S]{0,300}$" CssClass="ClsLabel" ValidationGroup="valsumGRN"> </asp:RegularExpressionValidator>
                                                        </td>
                                                        <td width="30%" align="left">
                                                            <asp:Label ID="lblDespMendMark" runat="server" CssClass="ClsLabel" ForeColor="Red"
                                                                EnableViewState="false" Text="*"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="3">
                                                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" Width="120"
                                                                disable-page="true" OnClick="btnSave_Click" ValidationGroup="valsumGRN" Visible="false" />
                                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                                                Width="120" PostBackUrl="~/RITeSchool/Inventory/GRNListUI.aspx" />
                                                            <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="Cancel" Visible="true"
                                                                OnClick="btnCancel_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hidQty" runat="server" />
                                                            <asp:HiddenField ID="hidPOCode" runat="server" />
                                                            <asp:HiddenField ID="hidItemName" runat="server" />
                                                            <asp:HiddenField ID="hidActualQty" runat="server" />
                                                            <asp:HiddenField ID="hidActualPOQty" runat="server" />
                                                            <asp:HiddenField ID="hidGRNName" runat="server" />
                                                            <asp:HiddenField ID="hidGRNId" runat="server" Value="0" />
                                                            <asp:HiddenField ID="hidIsModify" runat="server" />
                                                            <asp:HiddenField ID="hidGRNItemCount" runat="server" />
                                                            <asp:HiddenField ID="hidUOMName" runat="server" />
                                                            <asp:HiddenField ID="hidPieceCount" runat="server" />
                                                            <asp:HiddenField ID="hidCmbValue" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwGRN" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="optItemWise" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="optPOWise" EventName="CheckedChanged" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItemWiseDetails" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwPOWiseDetails" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwPOItems" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwGRN" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwPOItems" EventName="ItemDataBound" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAddAll" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
    </center>
    <script language="javascript" type="text/javascript">
        _sClienthidQty = "<%=this.hidQty.ClientID %>"
        _clientcstvalQtyId = "<%=this.cstvalQty.ClientID %>"
        _clientcstvalPOQtyId = "<%=this.cstvalPOQty.ClientID %>"
        _sClienthidItemName = "<%=this.hidItemName.ClientID %>"
        _sClienthidPOCode = "<%=this.hidPOCode.ClientID %>"
        _sClienthidActualQty = "<%=this.hidActualQty.ClientID %>"
        _sClienthidActualPOQty = "<%=this.hidActualPOQty.ClientID %>"
        _clientListViewId = "<%=this.lstvwPOItems.ClientID %>"
        _sClienthidGRNItemCount = "<%=this.hidGRNItemCount.ClientID %>"
        _sClienthidIsModify = "<%=this.hidIsModify.ClientID %>"
        _sClienthidCmbValue = "<%=this.hidCmbValue.ClientID %>"
        _sClienthidPieceCount = "<%=this.hidPieceCount.ClientID %>"
        function SetValueToHiddenField(otxtAcptQty, oActualQty, oCmbUnit, oItemName, oPOCode) {
            document.getElementById(_sClienthidQty).value = document.getElementById(otxtAcptQty).value
            document.getElementById(_sClienthidItemName).value = document.getElementById(oItemName).innerHTML
            document.getElementById(_sClienthidPOCode).value = document.getElementById(oPOCode).innerHTML
            document.getElementById(_sClienthidActualQty).value = document.getElementById(oActualQty).value
            document.getElementById(_sClienthidActualPOQty).value = document.getElementById(oActualQty).value
            document.getElementById(_sClienthidCmbValue).value = document.getElementById(oCmbUnit).value
        }


        function ValidateQuantity() {
            var sQty = document.getElementById(_sClienthidQty).value
            var sItem = document.getElementById(_clientcstvalQtyId).value
            if (sQty != '') {
                if (sQty == '.') {
                    document.getElementById(_clientcstvalQtyId).errormessage = "Enter valid Accepted quantity for item " + sItem + "."
                    args.IsValid = false
                    return true
                }
                else if (parseFloat(sQty) != parseFloat(0)) {
                    document.getElementById(_clientcstvalQtyId).errormessage = " "
                    args.IsValid = true
                    return false
                }
                else {
                    document.getElementById(_clientcstvalQtyId).errormessage = "Accepted quantity should be greater than zero for item " + sItem + "."
                    args.IsValid = false
                    return true
                }
            }
            else {
                document.getElementById(_clientcstvalQtyId).errormessage = "Accepted quantity should not be blank for item " + sItem + "."
                args.IsValid = false
                return true
            }
        }
        function ValidatePOQty(oSrc, args) {
            var sQty = document.getElementById(_sClienthidQty).value
            var sItem = document.getElementById(_sClienthidItemName).value
            var sActualQty = document.getElementById(_sClienthidActualPOQty).value
            var sPOCode = document.getElementById(_sClienthidPOCode).value
            var shidIsModify = document.getElementById(_sClienthidIsModify).value
            var cmbValue = document.getElementById(_sClienthidCmbValue).value
            var sPieceCount = document.getElementById(_sClienthidPieceCount).value
            if (sQty != '') {
                if (sQty == '.') {
                    document.getElementById(_clientcstvalPOQtyId).errormessage = "Enter valid quantity for item \'" + sItem + " from PO code " + sPOCode + "\'."
                    args.IsValid = false
                    return true
                }
                else if (parseFloat(sQty) != parseFloat(0)) {
                    if (cmbValue == 0) {
                        var sQuantity = parseFloat(sQty) * sPieceCount;
                        if (parseFloat(sQuantity) > parseFloat(sActualQty) && parseFloat(sActualQty) != parseFloat(0)) {
                            document.getElementById(_clientcstvalPOQtyId).errormessage = "Quantity should not be greater than actual quantity for item \'" + sItem + "\' from PO code \'" + sPOCode + "\'."
                            args.IsValid = false
                            return true
                        }
                    }
                    else if (cmbValue == 1) {
                        if (parseFloat(sQty) > parseFloat(sActualQty) && parseFloat(sActualQty) != parseFloat(0)) {
                            document.getElementById(_clientcstvalPOQtyId).errormessage = "Quantity should not be greater than actual quantity for item \'" + sItem + "\' from PO code \'" + sPOCode + "\'."
                            args.IsValid = false
                            return true
                        }
                    }
                    else {
                        document.getElementById(_clientcstvalPOQtyId).errormessage = " "
                        args.IsValid = true
                        return false
                    }
                }
                else {
                    document.getElementById(_clientcstvalPOQtyId).errormessage = "Quantity should be greater than zero for item \'" + sItem + "\' from PO code \'" + sPOCode + "\'."
                    args.IsValid = false
                    return true
                }
            }
            else {
                document.getElementById(_clientcstvalPOQtyId).errormessage = "Quantity should not be blank for item \'" + sItem + "\' from PO code " + sPOCode + "\'."
                args.IsValid = false
                return true
            }
        }
        function CalculateRejectedQuantity(othis, count, ofalse, otxtAcptQty, oActualQty, oRejectQty, ocmbSelected, oPieceCount) {

            document.getElementById(_sClienthidQty).value = document.getElementById(otxtAcptQty).value
            document.getElementById(_sClienthidActualQty).value = document.getElementById(oActualQty).value
            var cmbSelected = document.getElementById(ocmbSelected).value;
            var sQty = document.getElementById(_sClienthidQty).value
            var sActualQty = document.getElementById(_sClienthidActualQty).value

            if (!parseFloat(sQty))
                sQty = '0'
            if (parseFloat(sQty) <= parseFloat(sActualQty)) {
                if (cmbSelected == 0) {
                    var sQuantity = parseFloat(sQty) * oPieceCount
                    document.getElementById(oRejectQty).innerHTML = parseFloat(sActualQty) - parseFloat(sQuantity)
                }
                else {
                    document.getElementById(oRejectQty).innerHTML = parseFloat(sActualQty) - parseFloat(sQty)
                }
            }

            else
                document.getElementById(oRejectQty).innerHTML = parseFloat(0)

            if (extractNumber(othis, count, ofalse))
                return true
        }
        function AddAllReqItems(iRowCount, oCmbVal) {
            var oPieceCount = document.getElementById(_sClienthidPieceCount).value;
            var sMessage
            var Max = 0
            var MaxDot = 0
            var i
            var ItemName = ""
            var ItemNameDot = ""
            var sRowNumber = ""
            var sRowNumberDot = ""
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var ActualQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualPOQty"
                var ItemQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtAcceptedQty"
                var Name = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblItemName"
                var sActualQty = document.getElementById(ActualQty).value
                var sItemQty = document.getElementById(ItemQty).value
                var sName = document.getElementById(Name).innerHTML

                if (sItemQty != '') {
                    if (sItemQty == '.') {
                        sRowNumberDot = sRowNumberDot + (i + 1).toString() + ", "
                        ItemNameDot = ItemNameDot + sName + ", "
                        MaxDot = 1
                    }
                    else if (sItemQty != 0) {
                        if (oCmbVal == 0) {
                            var oQuantity = parseFloat(sItemQty) * oPieceCount;
                            if (parseFloat(sActualQty) < parseFloat(oQuantity)) {
                                Max = Max + 1
                                if (ItemName.match(sName + ", ") == null) {
                                    ItemName = ItemName + sName + ", "
                                }
                                sRowNumber = sRowNumber + (i + 1).toString() + ", "
                            }
                        }
                        else {
                            if (parseFloat(sActualQty) < parseFloat(sItemQty)) {
                                Max = Max + 1
                                if (ItemName.match(sName + ", ") == null) {
                                    ItemName = ItemName + sName + ", "
                                }
                                sRowNumber = sRowNumber + (i + 1).toString() + ", "
                            }
                        }
                    }
                }
            }
            if (MaxDot == 1) {
                ItemNameDot = ItemNameDot.substring(0, ItemNameDot.length - 2)
                sRowNumberDot = sRowNumberDot.substring(0, sRowNumberDot.length - 2)
                sMessage = "Enter valid Quantity for item(s) " + ItemNameDot + " at row number(s) " + sRowNumberDot + "."
            }
            else if (Max != 0) {
                ItemName = ItemName.substring(0, ItemName.length - 2)
                sRowNumber = sRowNumber.substring(0, sRowNumber.length - 2)
                sMessage = "Quantity should not be greater than actual quantity for item(s) " + ItemName + " at row number(s) " + sRowNumber + "."
            }
            else {
                sMessage = ""
            }
            if (sMessage != "") {
                alert("Please fix following error(s): \n\r\n\r" + sMessage)
                return false
            }
            else {
                var ItemNameZero = ""
                var ItemNameBlank = ""
                var ItemNameDot = ""
                var sRowNumberZero = ""
                var sRowNumberBlank = ""
                var sRowNumberDot = ""
                for (i = 0; i < iRowCount; i++) {
                    RowNumber = i
                    var ActualQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualPOQty"
                    var ItemQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtAcceptedQty"
                    var Name = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblItemName"
                    var sActualQty = document.getElementById(ActualQty).value
                    var sItemQty = document.getElementById(ItemQty).value
                    var sName = document.getElementById(Name).innerHTML
                    if (sItemQty != '') {
                        if (sItemQty == '.') {
                            sRowNumberDot = sRowNumberDot + (i + 1).toString() + ", "
                        }
                        else if (sItemQty == 0) {
                            Max++
                            if (ItemNameZero.match(sName + ", ") == null) {
                                ItemNameZero = ItemNameZero + sName + ", "
                            }
                            sRowNumberZero = sRowNumberZero + (i + 1).toString() + ", "
                        }
                    }
                    else {
                        Max++
                        if (ItemNameBlank.match(sName + ", ") == null) {
                            ItemNameBlank = ItemNameBlank + sName + ", "
                        }
                        sRowNumberBlank = sRowNumberBlank + (i + 1).toString() + ", "
                    }
                }
                if (Max != 0) {
                    ItemNameZero = ItemNameZero.substring(0, ItemNameZero.length - 2)
                    sRowNumberZero = sRowNumberZero.substring(0, sRowNumberZero.length - 2)
                    ItemNameBlank = ItemNameBlank.substring(0, ItemNameBlank.length - 2)
                    sRowNumberBlank = sRowNumberBlank.substring(0, sRowNumberBlank.length - 2)
                    ItemNameDot = ItemNameDot.substring(0, ItemNameDot.length - 2)
                    sRowNumberDot = sRowNumberDot.substring(0, sRowNumberDot.length - 2)
                    if (sRowNumberDot != "") {
                        sMessage = "Enter valid Quantity at row number(s) " + sRowNumberDot + "."
                    }
                    else if (ItemNameBlank != "" && ItemNameZero != "") {
                        sMessage = "Quantity is blank for item(s) " + ItemNameBlank + " at row number(s) " + sRowNumberBlank + ". And  " + " quantity is zero for item(s) " + ItemNameZero + " at row number(s) " + sRowNumberZero + "."
                    }
                    else if (ItemNameBlank == "") {
                        sMessage = "Quantity is zero for item(s) " + ItemNameZero + " at row number(s) " + sRowNumberZero + "."
                    }
                    else if (ItemNameZero == "") {
                        sMessage = "Quantity is blank for item(s) " + ItemNameBlank + " at row number(s) " + sRowNumberBlank + "."
                    }
                    if (window.confirm(sMessage + " Are you sure you want to continue?")) {
                        return true
                    }
                    else {
                        return false
                    }
                }
            }
            return true
        }
        function AllConfirmDelete() {
            var Count = document.getElementById(_sClienthidGRNItemCount).value
            if (Count == parseInt(0)) {
                if (window.confirm('You delete all the items from this GRN, so this action will delete GRN. Do you want to continue?')) {
                    bIsValid = true
                }
                else {
                    bIsValid = false
                }
            }
            else {
                bIsValid = true
            }
            return bIsValid
        }
    </script>
</asp:Content>
