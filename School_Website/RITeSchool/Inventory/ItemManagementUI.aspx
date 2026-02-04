<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ItemManagementUI.aspx.cs" Inherits="ItemManagementUI"
    Title="Untitled Page" %>

<asp:Content ID="CntItemManagement" ContentPlaceHolderID="MainBody" runat="Server">
    <center>
        <div class="MainBodyDiv" style="width:80%">
            <table width="100%" align="center">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UPanelItemSearch" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table cellpadding="0" cellspacing="2" align="center" width="100%">
                                    <tr>
                                        <td align="left" valign="top" colspan="4">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="background-color: white;" valign="top">
                                                        <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" Font-Bold="False"
                                                             Height="20px" Style="text-align: left" Width="100%"></asp:Label>
                                                        <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                                            ShowSummary="True" ValidationGroup="valsumItems" />
                                                    </td>
                                                    <td align="right" valign="top">                                                       
                                                        <span class="ClsMdtStar">* Mandatory Fields</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="4">
                                                <span class="ClsLblLgnd">Search Item Details :</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderLight">                                           
                                            <span id="lblItemName" class="ClsLabel" style="width: 100px">Item Name :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                                TabIndex="1"></asp:TextBox><span style="color: red"></span>
                                        </td>
                                        <td class="ClsBorderLight">                                           
                                            <span id="lblItemCode" class="ClsLabel" style="width: 100px">Item Code :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" runat="server" CssClass="ExLrgTxtBox" MaxLength="10"
                                                TabIndex="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td class="ClsBorderLight">                                           
                                            <span id="lblRackNumber " class="ClsLabel">Rack Number :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRackNumber" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                                TabIndex="1"></asp:TextBox><span style="color: red"></span>
                                        </td>
                                        <td class="ClsBorderLight">                                           
                                            <span id="lblhallNumber" class="ClsLabel">Hall Number :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtHallNumber" runat="server" CssClass="ExLrgTxtBox" MaxLength="10"
                                                TabIndex="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                                <td class="ClsBorderLight">                                           
                                            <span id="lblShelfNumber" class="ClsLabel"> Shelf Number :</span>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtShelfNumber" runat="server" CssClass="ExLrgTxtBox" MaxLength="10"
                                                TabIndex="2"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ClsBorderLight">                                           
                                            <span id="lblCategory" class="ClsLabel">Item Category :</span>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ExLrgTxtBox" AutoPostBack="False"
                                                TabIndex="3">
                                            </asp:DropDownList>
                                        </td>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkShowItemBelowReorder" runat="server" CssClass="ClsLabel" Text="Items Below Reorder Level"
                                                TabIndex="4" Style="padding-left: 0px" />
                                        </td>
                                    </tr>
                                    
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkNonMoveItem" runat="server" CssClass="ClsLabel" Text="Non Moving Items"
                                                AutoPostBack="true" TabIndex="5" Style="padding-left: 0px" />
                                        </td>
                                        <td class="ClsBorderlight" id="tdLblFromDate" runat="server">                                           
                                            <span id="lblFromDate" runat="server" class="ClsLabel" style="width: 100px">From Days
                                                :</span>
                                        </td>
                                        <td align="left" class="ClsTextNormal" style="padding-right: 10px; width: 260px;
                                            height: 19px;" id="tdTxtFromDate" runat="server">
                                            <asp:TextBox ID="txtFromDays" runat="server" CssClass="ExLrgTxtBox" MaxLength="4"
                                                TabIndex="6" onblur="extractNumber(this,0,true);" onkeyup="extractNumber(this,0,true);"
                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                ondrop="event.returnValue=false">
                                            </asp:TextBox>&nbsp
                                            <span class="ClsMdtStar" style="color: Red">*</span>
                                            <asp:RequiredFieldValidator ID="reqFromDay" runat="server" ControlToValidate="txtFromDays"
                                                ValidationGroup="valsumItems" ErrorMessage="From Days should not be blank." Display="None" EnableClientScript="True"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 26px" colspan="4">
                                            <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="8"
                                                Text="Change Input" Width="100px" OnClick="btnSearch_Click" ValidationGroup="valsumItems"/>
                                        </td>
                                    </tr>
                                    <tr><td style="height:22px"></td></tr>
                                    <tr>
                                        <td colspan="4">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="width: 18%;" valign="middle">
                                                        <div class="ClsGreenBG" style="width: 85px; height: 18px; vertical-align: bottom;
                                                            padding-top: 4px; padding-right: 2px">
                                                            <asp:HyperLink ID="hlnkAddItem" runat="server" CssClass="SubTitle " NavigateUrl="ItemDetailsUI.aspx"
                                                                Text="Add Item" TabIndex="9"></asp:HyperLink>
                                                        </div>
                                                    </td>
                                                    <td align="left" style="width: 20%;" valign="middle">
                                                        <div class="ClsGreenBG" style="width: 105px; height: 18px; vertical-align: middle;
                                                            padding-top: 4px; padding-right: 2px">
                                                            <asp:HyperLink ID="hlnkImportItem" runat="server" CssClass="SubTitle " NavigateUrl="ImportItemUI.aspx"
                                                                Text="Import Items" TabIndex="10"></asp:HyperLink>
                                                        </div>
                                                    </td>
                                                    <td align="left" style="width: 23%;" valign="middle">
                                                        <div class="ClsGreenBG" style="width: 140px; height: 18px; vertical-align: middle;
                                                            padding-top: 4px; padding-right: 2px">
                                                            <asp:HyperLink ID="hlnkManageCategoryOrUOM" runat="server" CssClass="SubTitle " NavigateUrl="ManageCategoriesOrUOMUI.aspx"
                                                                Text="Add Category/UOM" TabIndex="11"></asp:HyperLink>
                                                        </div>
                                                    </td>
                                                    <td align="left" style="width: 14%;" valign="middle">
                                                        <div class="ClsGreenBG" style="width: 223px; height: 18px; vertical-align: middle;
                                                            padding-top: 4px; padding-right: 2px">
                                                            <asp:HyperLink ID="hlnkItemIssue" runat="server" CssClass="SubTitle " NavigateUrl="ItemIssueUI.aspx"
                                                                Text="Issue / Return Requisition Items" TabIndex="11"></asp:HyperLink>
                                                        </div>
                                                    </td>
                                                    <td align="right" style="width: 20%;" valign="middle">
                                                        <div class="ClsGreenBG" style="width: 115px; height: 18px; vertical-align: middle;
                                                            padding-top: 4px; padding-right: 2px; text-align: left">
                                                            <asp:HyperLink ID="hlnkStockBalance" runat="server" CssClass="SubTitle " NavigateUrl="StockBalanceUI.aspx"
                                                                Text="Stock Balance" TabIndex="12"></asp:HyperLink>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="ItemCommand" />                                
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="UpanelItemList" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table width="100%">
                                    <tr id="trItemCount" runat="server">
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwItemDetails"
                                                Visible="true">
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
                                                <asp:ListView ID="lstvwItemDetails" runat="server" DataKeyNames="ItemID, ConsiderUnitQuantity, ConsiderUnitReorderLevel,ItemCategoryID" OnDataBound="lstvwItemDetails_DataBound"
                                                    OnItemDataBound="lstvwItemDetails_ItemDataBound" OnSorting="lstvwItemDetails_Sorting"
                                                    OnItemCommand="lstvwItemDetails_ItemCommand" DataSourceID="lstvwDSobj">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th id="thItemCode" runat="server" align="left" class="ClspaddingL" style="width:150px;">
                                                                    <asp:LinkButton ID="lnkItemCode" runat="server" CommandName="Sort" CommandArgument="ItemCode"
                                                                        ForeColor="Black">
                                                                                            Item Code</asp:LinkButton>
                                                                </th>
                                                                <th id="thItemName" runat="server" align="left" class="ClspaddingL" style="width:180px">
                                                                    <asp:LinkButton ID="lnkItemName" runat="server" CommandName="Sort" CommandArgument="ItemName"
                                                                        ForeColor="Black">
                                                                                            Item Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL" style="width:150px;">
                                                                    <asp:LinkButton ID="lnkRackNumber" runat="server" CommandName="Sort" CommandArgument="RackNo"
                                                                        ForeColor="Black">
                                                                                            Rack Number</asp:LinkButton>
                                                                </th>
                                                                 <th align="left" class="ClspaddingL" style="width:150px;">
                                                                    <asp:LinkButton ID="lnkShelfNumber" runat="server" CommandName="Sort" CommandArgument="ShelfNo"
                                                                        ForeColor="Black">
                                                                                         Shelf Number</asp:LinkButton>

                                                                </th>
                                                                <th align="left" class="ClspaddingL" style="width:150px;">
                                                                    <asp:LinkButton ID="lnkHall" runat="server" CommandName="Sort" CommandArgument="Hall"
                                                                        ForeColor="Black">
                                                                                           Hall Number</asp:LinkButton>

                                                                </th>
                                                               
                                                                <th id="thItemPrice" runat="server" align="right" class="ClspaddingL" style="width:140px;padding-right:5px; text-align:right"> 
                                                                     <asp:LinkButton ID="lnkbtnUnitPrice" runat="server" CommandName="Sort" CommandArgument="[dbo].[udf_GetPriceOfItem](ItemID)" ForeColor="Black">
                                                                                            Unit Price</asp:LinkButton>
                                                                </th>
                                                                <th id="thStock" runat="server" align="left" class="ClspaddingL" style="width:200px;">
                                                                    <asp:Label ID="lblCurrentStock" runat="server" Text="Current Stock" style="font-weight:bold; color:Black;" />                                                                                            
                                                                </th>
                                                                <th id="thLevel" runat="server" align="left" class="ClspaddingL" style="width:200px;">
                                                                    <asp:Label ID="lblReorderLevel" runat="server" Text="Reorder Level" style="font-weight:bold; color:Black;" />
                                                                </th>
                                                                <th id="th1" runat="server" align="center" style="width:150px;">
                                                                    <asp:Label ID="lblItemDetails" runat="server" Text="Item Details" style="font-weight:bold; color:Black;" /> 
                                                                </th>
                                                                <th id="thEdit" runat="server" align="center" style="width:70px;">
                                                                    <asp:Label ID="Label1" runat="server" Text="Edit" style="font-weight:bold; color:Black;" /> 
                                                                </th>
                                                                <th id="thRemove" runat="server" align="center" style="width:70px;">
                                                                    <asp:Label ID="Label2" runat="server" Text="Remove" style="font-weight:bold; color:Black;" /> 
                                                                </th>
                                                                <th id="th2" runat="server" align="center" style="width:70px;">
                                                                    <asp:Label ID="Label3" runat="server" Text="Details" style="font-weight:bold; color:Black;" /> 
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                <td colspan="12">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwItemDetails">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <span class="LblNrmlB">Select a page:</span>
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
                                                        <tr id="trItem" runat="server" class="ClsGridRow">
                                                            <td align="left" id="tdItemCode" class="ClspaddingL">
                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                                <asp:HiddenField ID="hidData" runat="server" Value="" />
                                                            </td>
                                                            <td align="left" id="tdItemName" class="ClspaddingL">
                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                            </td>
                                                             <td align="left" id="tdRackNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblRRackNumber" runat="server" Text='<%# Eval("RackNo")%>'></asp:Label>
                                                            </td>
                                                              <td align="left" id="tdShelfNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblShelfNumber" runat="server" Text='<%# Eval("ShelfNo")%>'></asp:Label>
                                                            </td>
                                                              <td align="left" id="tdHallNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblHallNumber" runat="server" Text='<%# Eval("Hall")%>'></asp:Label>
                                                            </td>
                                                            <td align="right" id="tdItemPrice" class="ClspaddingR">
                                                                <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice")%>'></asp:Label>
                                                            </td>
                                                            <td align="left" id="tdItemStock" class="ClspaddingL">
                                                                <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                            </td>
                                                            <td align="left" id="tdRLevel" class="ClspaddingL">
                                                                <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" id="tdItemDetails" >
                                                                <asp:LinkButton ID="lnkbtnItemDetails" runat="server" Text="Item Details" CommandName = "ItemDetails" />
                                                            </td>
                                                            <td id="Edit" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtnEditItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                    ToolTip="Edit" />
                                                            </td>
                                                            <td id="Remove" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtnRemoveItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    ToolTip="Remove" CommandName="Remove" />
                                                            </td>
                                                            <td id="Td1" runat="server" align="center">
                                                               <asp:LinkButton ID="lnkExport" runat="server" CommandName="IssueDetails">Details</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                            <td align="left" id="tdItemCode" class="ClspaddingL">
                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                                <asp:HiddenField ID="hidData" runat="server" Value="" />
                                                            </td>
                                                            <td align="left" id="tdItemName" class="ClspaddingL">
                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                            </td>
                                                             <td align="left" id="tdRackNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblRackNumber" runat="server" Text='<%# Eval("RackNo")%>'></asp:Label>
                                                            </td>
                                                              <td align="left" id="tdShelfNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblShelfNumber" runat="server" Text='<%# Eval("ShelfNo")%>'></asp:Label>
                                                            </td>
                                                              <td align="left" id="tdHallNumber" class="ClspaddingL">
                                                                <asp:Label ID="lblHallNumber" runat="server" Text='<%# Eval("Hall")%>'></asp:Label>
                                                            </td>
                                                            <td align="right" id="tdItemPrice" class="ClspaddingR">
                                                                <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice")%>'></asp:Label>
                                                            </td>
                                                            <td align="left" id="tdItemStock" class="ClspaddingL">
                                                                <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                            </td>
                                                            <td align="left" id="tdRLevel" class="ClspaddingL">
                                                                <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
                                                            </td>
                                                            <td align="center" id="tdItemDetails" >
                                                                <asp:LinkButton ID="lnkbtnItemDetails" runat="server" Text="Item Details" CommandName = "ItemDetails" />
                                                            </td>
                                                            <td id="Edit" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtnEditItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                                    ToolTip="Edit" />
                                                            </td>
                                                            <td id="Remove" runat="server" align="center">
                                                                <asp:ImageButton ID="imgbtnRemoveItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    ToolTip="Remove" CommandName="Remove" />
                                                            </td>
                                                            <td id="Td1" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkExport" runat="server" CommandName="IssueDetails">Details</asp:LinkButton>
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
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.ItemsMasterBL" EnablePaging="true"
                                                ID="lstvwDSobj" runat="server" SelectMethod="GetAllItemDetails" SortParameterName="sortExpression"
                                                SelectCountMethod="CountItemRows" EnableCaching="false">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:ControlParameter Name="asItemName" Type="String" ControlID="txtItemName" DefaultValue=""
                                                        PropertyName="Text" />
                                                    <asp:ControlParameter Name="asItemCode" Type="String" ControlID="txtItemCode" DefaultValue=""
                                                        PropertyName="Text" />
                                                    <asp:ControlParameter Name="asItemCategory" Type="String" ControlID="ddlCategory"
                                                        PropertyName="SelectedValue" DefaultValue="0" />
                                                    <asp:ControlParameter Name="abIsBelowReoder" Type="Boolean" ControlID="chkShowItemBelowReorder"
                                                        PropertyName="Checked" DefaultValue="False" />
                                                    <asp:ControlParameter Name="abIsNonMoveItem" Type="Boolean" ControlID="chkNonMoveItem"
                                                        PropertyName="Checked" DefaultValue="False" />
                                                      <asp:ControlParameter Name="asHall" Type="String" ControlID="txtHallNumber" DefaultValue=""
                                                        PropertyName="Text" />
                                                    <asp:ControlParameter Name="asRack" Type="String" ControlID="txtRackNumber" DefaultValue=""
                                                        PropertyName="Text" />
                                                     <asp:ControlParameter Name="asShelf" Type="String" ControlID="txtShelfNumber" DefaultValue=""
                                                        PropertyName="Text" />
                                                    <asp:ControlParameter Name="asFromDate" Type="String" ControlID="txtFromDays" DefaultValue=""
                                                        PropertyName="Text" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                            <asp:HiddenField ID="hidAllowDeletion" runat="server" Value="1" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td align="center" colspan="1" style="padding-top: 5px">
                                        <asp:Button CssClass="ClsBtn" ID="btnExport" runat="server" Text="<%$ Resources:LocalizedResources, Export %>"
                                            BorderWidth="1px" UseSubmitBehavior="false" OnClick="btnExport_Click" Visible="false"></asp:Button>
                                    </td>
                                </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="Sorting" />
                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="ItemCommand" />
                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="DataBound" />
                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                <asp:PostBackTrigger ControlID="btnExport" />                                
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </div>
    </center>

    <script language="javascript" type="text/javascript">
        _clientvalsumItems = "<%=this.valsumItems.ClientID %>"
        _clientbtnSave = "<%=this.btnSearch.ClientID %>"
        
        function closewindow() {
            window.opener.location.href = window.opener.location.href
            if (window.opener.progressWindow)
                window.opener.progressWindow.close()
            window.close()
        }
        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to remove this item?')) {
                bResult = false
            }
            return bResult
        }

        function OpenReport(index) {
            var str = $('[id$=' + index + '_hidData]').val()
            window.open('../Admission/AdmissionFormReport.aspx?'+str)
        }
        
    </script>

</asp:Content>
