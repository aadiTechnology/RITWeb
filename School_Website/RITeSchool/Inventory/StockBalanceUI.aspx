<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StockBalanceUI.aspx.cs" Inherits="StockBalanceUI"
    Title="Untitled Page" %>

<asp:Content ID="CntBalanceStock" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:UpdatePanel ID="UPanelItemSearch" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="2" align="center" width="80%">
                            <tr>
                                <td align="left" valign="top" colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td align="right" style="width: 23%; padding-right: 30px; height: 21px;" >
                                                <span class="ClsMdtStar">* Mandatory Fields</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td id="tdError"  align="left" style="background-color: white;" valign="top">
                                                <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" ShowMessageBox="false"
                                                    ShowSummary="false" ValidationGroup="ItemBalance" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" colspan="4">                                   
                                        <span class="ClsLblLgnd" style="font-weight:bold">Search Item Stock :</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderLight" width="20%">                                   
                                        <span class="ClsLabel"> Item Name :</span>
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="txtItemName" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                        TabIndex="1"></asp:TextBox><span style="color: red"></span>
                                </td>
                                <td class="ClsBorderLight" width="20%">                                   
                                         <span class="ClsLabel"> Item Code :</span>
                                </td>
                                <td width="20%">
                                    <asp:TextBox ID="txtItemCode" runat="server" CssClass="ExLrgTxtBox" MaxLength="100"
                                        TabIndex="2"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="ClsBorderLight">                                   
                                         <span class="ClsLabel">Item Catagory :</span>
                                        
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="ExLrgTxtBox" 
                                        TabIndex="3">
                                    </asp:DropDownList>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkShowItemBelowReorder" runat="server" CssClass="ClsLabel" Text="Items Below Reorder Level"
                                        TabIndex="4" Style="padding-left : 0px; padding-right : 0px" />
                                </td>
                            </tr>
                            <tr>
                            <td class="ClsBorderLight" width="20%">
                            <span class="ClsLabel"> Hall :</span>
                            </td>
                            <td width="20%">
                             <asp:TextBox ID="txtHall" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                        TabIndex="1"></asp:TextBox>
                            </td>
                           
                            
                            <td class="ClsBorderLight" width="20%">
                            <span class="ClsLabel"> Rack No :</span>
                            </td>
                            <td width="20%">
                            <asp:TextBox ID="txtRackNo" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                        TabIndex="1"></asp:TextBox>
                            </td>
                            </tr>
                             <tr>
                            <td class="ClsBorderLight" width="20%">
                            <span class="ClsLabel"> Shelf No :</span>
                            </td>
                            <td width="20%">
                            <asp:TextBox ID="txtShelfNo" runat="server" CssClass="ExLrgTxtBox" MaxLength="50"
                                        TabIndex="1"></asp:TextBox>
                            </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%">
                                        <tr>
                                            <td align="center" style="height: 26px">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="ClsBtn" Font-Bold="True" TabIndex="8"
                                                    Text="Search" Width="100px"  onclick="btnSearch_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="DataBound" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemDataBound" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpanelItemList" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%">
                            <tr id="trItemCount" runat="server">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwItems"
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
                                        <asp:ListView ID="lstvwItems" runat="server" OnDataBound="lstvwItems_DataBound"
                                            OnItemDataBound="lstvwItems_ItemDataBound" 
                                            OnItemCommand="lstvwItems_ItemCommand" DataSourceID="lstvwDSobj" DataKeyNames="ItemID">
                                            <LayoutTemplate>
                                                <table width="80%" align="center" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                    cellspacing="0" class="GridBorder">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" runat="server" id="tblItems" style="color: #333333" cellpadding="0"
                                                                cellspacing="1">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th id="thItemCode" runat="server" align="left" class="ClspaddingL" style="width:10%">
                                                                        Item Code
                                                                    </th>
                                                                    <th id="thItemName" runat="server" align="left" class="ClspaddingL" style="width:20%">
                                                                        Item Name
                                                                    </th>
                                                                    <th id="thItemPrice" runat="server" align="right" class="ClspaddingR" style="width:8%">
                                                                        Unit Price
                                                                    </th>
                                                                    <th id="thStock" runat="server" align="left" class="ClspaddingL" style="width:12%">
                                                                        Current Stock
                                                                    </th>
                                                                    <th id="thBalanceStock" runat="server" align="center" style="width:18%" visible="false">
                                                                        Updated Stock
                                                                    </th>
                                                                    <th id="thReorder" runat="server" align="left" class="ClspaddingL" style="width:14%">
                                                                        Reorder Level
                                                                    </th>
                                                                    <th id="thLevel" runat="server" align="center" style="width:14%" visible="false">
                                                                        Reason
                                                                    </th>                                                                   
                                                                    <th id="thUpdated" runat="server" align="center" style="width:17%">
                                                                          Add Stock Details    
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="8">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwItems">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
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
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr id="trItem" runat="server" class="ClsGridRow">
                                                    <td align="left" id="tdItemCode" class="ClspaddingL">
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>' ></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdItemName" class="ClspaddingL">
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>' ></asp:Label>
                                                    </td>
                                                     <td align="right" id="tdItemPrice" class="ClspaddingR">
                                                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemPrice")%>' ></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdItemStock" class="ClspaddingL">
                                                        <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                    </td>
                                                    <td id="tdNewStock" align="center" class="ClspaddingL" visible="false">
                                                        <asp:TextBox ID="txtNewStock" runat="server" type="TextBox" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"ItemQty")))%>'
                                                            onblur="extractNumberVisibleControl(this,3,true);" onkeyup="extractNumberVisibleControl(this,3,true);"
                                                            onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" MaxLength="7" Width="59%"/>
                                                        <asp:Label ID="lblNewStockUnit" runat="server" Text='<%# Eval("UOMUnit")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdReorder" class="ClspaddingL">
                                                        <asp:Label ID="lblReorder" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
                                                        
                                                    </td>
                                                    <td id="tdReason" align="center" visible="false">
                                                        <asp:TextBox ID="txtReason" runat="server" type="TextBox" MaxLength="300" Width="75%"></asp:TextBox>
                                                        <asp:Label ID="lblMend" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td id="tdUpdateStock" runat="server" align="center">
                                                        <asp:LinkButton ID="btnUpdateStock" runat="server"  Width="70%" CssClass="SMSLblSMlBlue"
                                                            Text="Add Stock Details" CommandName="UpdateStockDetails" CommandArgument='<%# Eval("ItemID")%>'/>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                    <td align="left" id="tdItemCode" class="ClspaddingL">
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdItemName" class="ClspaddingL">
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                    </td>
                                                    <td align="right" id="tdItemPrice" class="ClspaddingR">
                                                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("ItemPrice")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdItemStock" class="ClspaddingL">
                                                        <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                    </td>
                                                    <td id="tdNewStock" align="center" class="ClspaddingL" visible="false">
                                                        <asp:TextBox ID="txtNewStock" runat="server" type="TextBox" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"ItemQty")))%>'
                                                            onblur="extractNumberVisibleControl(this,3,true);" onkeyup="extractNumberVisibleControl(this,3,true);"
                                                            onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                            ondrop="event.returnValue=false" MaxLength="7" Width="59%"/>
                                                        <asp:Label ID="lblNewStockUnit" runat="server" Text='<%# Eval("UOMUnit")%>'></asp:Label>
                                                    </td>
                                                     <td align="left" id="tdReorder" class="ClspaddingL">
                                                        <asp:Label ID="lblReorder" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
                                                        
                                                    </td>
                                                    <td id="tdReason" align="center" visible="false">
                                                        <asp:TextBox ID="txtReason" runat="server" type="TextBox" MaxLength="300" Width="75%"></asp:TextBox>
                                                        <asp:Label ID="lblMend" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td id="tdUpdateStock" runat="server" align="center">
                                                        <asp:LinkButton ID="btnUpdateStock" runat="server"  Width="70%" CssClass="SMSLblSMlBlue"
                                                            Text="Add Stock Details" CommandName="UpdateStockDetails" CommandArgument='<%# Eval("ItemID")%>'/>
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
                                                 <asp:ControlParameter Name="asHall" Type="String" ControlID="txtHall" DefaultValue=""
                                                PropertyName="Text" />
                                            <asp:ControlParameter Name="asRack" Type="String" ControlID="txtRackNo" DefaultValue=""
                                                PropertyName="Text" />
                                                <asp:ControlParameter Name="asShelf" Type="String" ControlID="txtShelfNo" DefaultValue=""
                                                PropertyName="Text" />
                                            
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidRowIndex" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="DataBound" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemDataBound" />
                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center" style="width: 50%;">
                <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                    CausesValidation="False" TabIndex="10" PostBackUrl="~/RITeSchool/Inventory/ItemManagementUI.aspx" />
            </td>
        </tr>
    </table>

    <script language="javascript" type="text/javascript">
        
        
        _clientlstvwItems="<%=this.lstvwItems.ClientID %>";
       
        function ValidateReason(othis) {
            var isValidate = 0
            var lstvwItem = _clientlstvwItems
            var lblOldStock = 'lblItemStock'
            var txtNewStock = 'txtNewStock'
            var txtReason = 'txtReason'
            var lblItemName = 'lblItemName'
            var btnUpdate = 'btnUpdate'
            isValidate = validateReasonForBalance(lstvwItem, lblOldStock, txtNewStock, txtReason, lblItemName, btnUpdate, othis)
            if (isValidate > 0)
                return true
            else
                return false
        }
        function validateReasonForBalance(lstvwItem, lblOldStock, txtNewStock, txtReason, lblItemName, btnUpdate, othis) {
            var iCount = 0
            var lstvwItemBalance = document.getElementById(lstvwItem + '_Table1')
            if (lstvwItemBalance != null) {
                var lstvwItemBalanceTable = document.getElementById(lstvwItem + '_tblItems')
                var sButton = "" + othis.id
                var iIndex = sButton.substring(sButton.indexOf("ctrl") + 4, sButton.indexOf("_btn"))
                var OldStock = lstvwItem + '_ctrl' + iIndex + '_' + lblOldStock
                var NewStock = lstvwItem + '_ctrl' + iIndex + '_' + txtNewStock
                var Reason = lstvwItem + '_ctrl' + iIndex + '_' + txtReason
                var ItemName = lstvwItem + '_ctrl' + iIndex + '_' + lblItemName
                var sOldStock = document.getElementById(OldStock)
                var sNewStock = document.getElementById(NewStock)
                var sReason = document.getElementById(Reason)
                var sItemName = document.getElementById(ItemName).innerHTML
                var sMessage = ''
                if (sReason != null && !sReason.disabled) {
                    if (trimAll(sNewStock.value) == '') {
                        iCount = iCount + 1
                        sMessage = ' Updated Stock should not be blank for item \'' + sItemName + '\'.'
                        alert(sMessage)
                    }
                    else if (trimAll(sNewStock.value) == '.') {
                        iCount = iCount + 1
                        sMessage = ' Please enter valid Updated Stock for item \'' + sItemName + '\'.'
                        alert(sMessage)
                    }
                    else if (parseFloat(sOldStock.innerHTML) == parseFloat(sNewStock.value)) {
                        iCount = iCount + 1
                        sMessage = ' Current Stock and Updated Stock should not be same for item \'' + sItemName + '\'.'
                        alert(sMessage)
                    }
                    else if (trimAll(sReason.value) == '') {
                        iCount = iCount + 1
                        sMessage = ' Reason should not be blank for item \'' + sItemName + '\'.'
                        alert(sMessage)
                    } 
                }
                else {
                    iCount = iCount + 1
                    sMessage = ' Current Stock and Updated Stock should not be same for item \'' + sItemName + '\'.'
                    alert(sMessage)
                } 
            }
            return iCount
        }
        function extractNumberVisibleControl(obj, decimalPlaces, allowNegative) {
            var temp = obj.value
            var sTextBox = "" + obj.id
            var iIndex = sTextBox.substring(sTextBox.indexOf("ctrl") + 4, sTextBox.indexOf("_txt"))
            var Reason = _clientlstvwItems + '_ctrl' + iIndex + '_' + 'txtReason'
            var Mendatory = _clientlstvwItems + '_ctrl' + iIndex + '_' + 'lblMend'
            var txtReason = document.getElementById(Reason)
            var lblMendatory = document.getElementById(Mendatory)
            txtReason.disabled = false
            lblMendatory.disabled = false
            if (extractNumber(obj, decimalPlaces, allowNegative))
                return true
        }
    </script>
</asp:Content>
