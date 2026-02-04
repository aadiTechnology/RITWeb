<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="AddRequisitionUI.aspx.cs" Inherits="AddRequisitionUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
            <ContentTemplate>
                <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" valign="top">
                            <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" Height="20px"
                                Width="100%" CssClass="LblErrorMsg"></asp:Label>
                            <asp:ValidationSummary ID="valRegNumber" runat="server" ShowMessageBox="False" ValidationGroup="Search"
                                ShowSummary="True" CssClass="ClsLabel" />
                            <asp:ValidationSummary ID="valSave" runat="server" ShowMessageBox="false" ValidationGroup="Save"
                                ShowSummary="true" CssClass="ClsLabel" />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Item quantity should not be greater than current stock" ValidationGroup="Save"
                                                             OnServerValidate="ItemQuantity_Validate" Display="None"></asp:CustomValidator>
                         </td>
                        <td align="right" valign="top">
                            <asp:Label ID="Label3" runat="server" ForeColor="Red" CssClass="LblNormalImg" EnableViewState="false">* Mandatory Fields</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" width="100%" colspan="2">
                            <asp:Label ID="lblMessage" runat="server" CssClass="LblNormalImg" Font-Bold="True"
                                ForeColor="Blue" Visible="false" EnableViewState="false"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trModify" runat="server" visible="false">
                        <td align="left" class="ClsGreenBG" id="trHistory" runat="server" width="15%">
                            <asp:HyperLink ID="lnkHistory" runat="server" CssClass="SubTitle" NavigateUrl="ItemIssueHistory.aspx?"
                                Text="Issue History" />
                        </td>
                        <td align="right" id="tdModify" runat="server">
                            <asp:Button ID="btnModify" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                Text="Modify" Visible="True" Width="80px" CausesValidation="false" OnClick="btnModify_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td id="MainDataTable" align="center" valign="top" colspan="2">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr valign="middle">
                                    <td align="center">
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="uPnl">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2" style="width: 90%" align="center">
                                                    <tr id="trIsGeneral" runat="server">
                                                        <td class="ClsBorderlight" width="19%" valign="middle">
                                                            <span class="ClsLabel">Is General Requisition? :</span>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chkIsGeneral" runat="server" Checked="false" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trCategory" runat="server">
                                                        <td class="ClsBorderlight" width="17%" valign="middle">
                                                            <span class="ClsLabel">Category :</span>
                                                        </td>
                                                        <td align="left" valign="middle" width="20%">
                                                            <asp:DropDownList ID="cmbCategory" runat="server" CssClass="MidCombo" 
                                                                TabIndex="1">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSearch" runat="server">
                                                        <td class="ClsBorderlight" width="17%" valign="middle">
                                                            <span class="ClsLabel">Item Code/Name :</span>
                                                        </td>
                                                        <td align="left" valign="middle" width="20%">
                                                            <asp:TextBox ID="txtItemCode" TabIndex="2" runat="server" MaxLength="50" 
                                                                CssClass="MidTxtBox"></asp:TextBox><span
                                                                style="color: #ff0000">*</span>
                                                            <asp:RequiredFieldValidator ID="reqItemCode" Display="None" runat="server" ErrorMessage="Item Code / Name should not be blank."
                                                                ControlToValidate="txtItemCode" ValidationGroup="Search" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="3" CssClass="ClsBtnMid"
                                                                ValidationGroup="Search" OnClick="btnSearch_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <tr id="trLstItems" runat="server" visible="false">
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
                                                                <asp:ListView ID="lstvwItems" runat="server" DataKeyNames="ItemID,ItemCode,ItemName,CurrentStock,UOMUnit,UOMID, PieceCount, ActualQuantity,ImageCount"
                                                                    OnDataBound="lstvwItems_DataBound" OnItemCommand="lstvwItems_ItemCommand" OnItemDataBound="lstvwItems_ItemDataBound">
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
                                                                                <th align="right" class="ClspaddingR">
                                                                                    Current Stock
                                                                                </th>
                                                                                <th align="left" class="ClspaddingL">
                                                                                    Category
                                                                                </th>
                                                                                <th align="center">
                                                                                    Item Image
                                                                                </th>
                                                                                <th align="center">
                                                                                    Add Item
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="6">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstvwItems">
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
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr class="ClsGridRow">
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                            </td>
                                                                            <td align="right" class="ClspaddingR">
                                                                                <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("CurrentStock") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("ItemCategoryName") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnItemImage" runat="server" CausesValidation="false" CommandName="ItemImage"
                                                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ToolTip="Add" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <AlternatingItemTemplate>
                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                            </td>
                                                                            <td align="right" class="ClspaddingR">
                                                                                <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("CurrentStock") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingL">
                                                                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("ItemCategoryName") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnItemImage" runat="server" CausesValidation="false" CommandName="ItemImage"
                                                                                    ImageUrl="../images/iconGridSml_ViewGE.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnStudent" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ToolTip="Add" />
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
                                                                ID="lstDSobj" runat="server" SelectMethod="GetAllItems" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountRowsOfItems" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter ControlID="txtItemCode" PropertyName="Text" Name="asName" />
                                                                    <asp:ControlParameter ControlID="cmbCategory" PropertyName="SelectedValue" Name="aiItemCategoryId" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                                <tr id="trLstReqItems" runat="server" visible="false">
                                    <td valign="top">
                                        <asp:UpdatePanel ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel3">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2" style="width: 90%" align="center">
                                                    <tr>
                                                        <td colspan="3">
                                                            <asp:ListView ID="LstVwReqItems" runat="server" DataKeyNames="ItemID,ItemCode,ItemName,CurrentStock,ItemStatus,UOMUnit,RequisitionDetailsID, ConsiderUnitQuantity,UOMPieceCount, IssueQty, ReturnQty, CancelQty"
                                                                OnItemCommand="LstVwReqItems_ItemCommand" OnDataBound="LstVwReqItems_DataBound" OnItemDataBound="LstVwReqItems_ItemDataBound">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th align="left" class="ClspaddingL" width="8%">
                                                                                Item Code
                                                                            </th>
                                                                            <th align="left" class="ClspaddingL" width="18%">
                                                                                Item Name
                                                                            </th>
                                                                            <th align="right" class="ClspaddingR" width="12%">
                                                                                Current Stock
                                                                            </th>
                                                                            <th align="right" class="paddingLR" width="18%">
                                                                                Item Quantity
                                                                            </th>
                                                                            <th class="paddingLR" width="10%" id="thorgQty" runat="server" align="right">
                                                                                Original Qty.
                                                                            </th>
                                                                            <th class="paddingLR" width="10%" id="thIssueQty" runat="server" align="right">
                                                                                Issued Qty
                                                                            </th>
                                                                            <th class="paddingLR" width="10%" id="thReturnQty" runat="server" align="right">
                                                                                Returned Qty
                                                                            </th>
                                                                            <th class="paddingLR" width="10%" id="thCancelQty" runat="server" align="right">
                                                                                Cancelled Qty
                                                                            </th>
                                                                            <th id="thDelete" runat="server" width="7%">
                                                                                Delete
                                                                            </th>
                                                                             <th id="thDeny" runat="server" width="7%">
                                                                                 <input id="ChkDenySelectAll" runat="server" viewstatemode="Enabled" type="checkbox" onclick="CheckUncheckAll(this);">
                                                                                    <asp:Label ID="lblDenyReq" runat="server" Text="Deny"></asp:Label>
                                                                                 </input>
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="lstDataRow" runat="server" class="ClsGridRow">
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                        </td>
                                                                        <td align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("CurrentStock") %>' />
                                                                        </td>
                                                                        <td align="right" class="paddingLR">
                                                                            <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,true)" onkeyup="extractNumber(this,2,true)"
                                                                                MaxLength="10" Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                CssClass="TxtAlignRght"></asp:TextBox><asp:DropDownList ID="cmbUnits" runat="server"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                                </asp:DropDownList>
                                                                            <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UOMUnit") %>'
                                                                                    Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>' />--%><span style="color: #ff0000"
                                                                                        id="star" runat="server" visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>*</span>
                                                                            <asp:Label ID="lblQty" runat="server" Visible='<%# !Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                Text='<%# Convert.ToString(Eval("ItemQty"))+"  "+ Convert.ToString(Eval("UOMUnit"))%>'
                                                                                CssClass="ClspaddingL"> </asp:Label><br />
                                                                            <asp:RequiredFieldValidator ID="reqItemQty" Display="None" runat="server" ErrorMessage='<%# "Quantity should not be blank for item " + Eval("ItemName") + "." %> '
                                                                                ControlToValidate="txtQty" ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="cmpItemQty" runat="server" ControlToValidate="txtQty" CssClass="errormsg"
                                                                                Display="None" ErrorMessage='<%# "Quantity should be greater than zero for item " + Eval("ItemName") + "." %> '
                                                                                ValueToCompare="0" Operator="GreaterThan" ValidationGroup="Save" Type="Double"></asp:CompareValidator>
                                                                        </td>
                                                                        <td align="right" class="paddingLR" id="tdOrgQty" runat="server" visible='<%# !Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                            <asp:Label ID="lblOriginalQuantity" runat="server" Text='<%# Convert.ToString(Eval("ItemOrgQty"))+"  "+ Convert.ToString(Eval("UOMUnit"))%>'
                                                                                CssClass="ClspaddingL"> </asp:Label>
                                                                        </td>
                                                                        <td id ="tdIssueQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblIssueQty" runat="server" Text='<%# Eval("IssueQty") +"  "+ Convert.ToString(Eval("UOMUnit")) %>' />
                                                                        </td>
                                                                         <td  id ="tdReturnQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblReturnQty" runat="server" Text='<%# Eval("ReturnQty") +"  "+ Convert.ToString(Eval("UOMUnit"))%>' />
                                                                        </td>
                                                                        <td  id ="tdCancelQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblCancelQty" runat="server" Text='<%# Eval("CancelQty") +"  "+ Convert.ToString(Eval("UOMUnit"))%>' />
                                                                        </td>
                                                                        <td align="center" runat="server" id="tdDelete" visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                            <asp:ImageButton ID="imgbtnDeleteItem" CommandArgument='<%# Eval("ItemID") %>' runat="server"
                                                                                ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                ToolTip="Delete" />
                                                                        </td>
                                                                        <td align="center" runat="server" id="tdDeny">
                                                                            <asp:CheckBox ID="ChkIsRequisitionToDeny" runat="server" ViewStateMode="Enabled" Checked= "false" onclick="UncheckCheck(this);"
                                                                                Enabled="true" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="lstDataRow" runat="server" class="ClsGridAltRow">
                                                                        <td align="left">
                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="right">
                                                                            <asp:Label ID="lblItemQty" runat="server" Text='<%# Eval("CurrentStock") %>' CssClass="ClspaddingR" />
                                                                        </td>
                                                                        <td align="right" class="paddingLR">
                                                                            <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,true)" onkeyup="extractNumber(this,2,true)"
                                                                                MaxLength="10" Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                CssClass="TxtAlignRght"></asp:TextBox><asp:DropDownList ID="cmbUnits" runat="server"
                                                                                    Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                                </asp:DropDownList>
                                                                            <%--<asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UOMUnit") %>'
                                                                                    Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>' />--%><span style="color: #ff0000"
                                                                                        id="star" runat="server" visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>*</span>
                                                                            <asp:Label ID="lblQty" runat="server" Visible='<%# !Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                Text='<%# Convert.ToString(Eval("ItemQty"))+"  "+ Convert.ToString(Eval("UOMUnit"))%>'
                                                                                CssClass="ClspaddingL"> </asp:Label><br />
                                                                            <asp:RequiredFieldValidator ID="reqItemQty" Display="None" runat="server" ErrorMessage='<%# "Quantity should not be blank for item " + Eval("ItemName") + "." %> '
                                                                                ControlToValidate="txtQty" ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ID="cmpItemQty" runat="server" ControlToValidate="txtQty" CssClass="errormsg"
                                                                                Display="None" ErrorMessage='<%# "Quantity should be greater than zero for item " + Eval("ItemName") + "." %> '
                                                                                ValueToCompare="0" Operator="GreaterThan" ValidationGroup="Save" Type="Double"></asp:CompareValidator>
                                                                        </td>
                                                                        <td align="right" class="paddingLR" id="tdOrgQty" runat="server" visible='<%# !Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                            <asp:Label ID="lblOriginalQuantity" runat="server" Text='<%# Convert.ToString(Eval("ItemOrgQty"))+"  "+ Convert.ToString(Eval("UOMUnit"))%>'
                                                                                CssClass="ClspaddingL"> </asp:Label>
                                                                        </td>
                                                                        <td id ="tdIssueQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblIssueQty" runat="server" Text='<%# Eval("IssueQty")+"  "+ Convert.ToString(Eval("UOMUnit")) %>' />
                                                                        </td>
                                                                        <td id ="tdReturnQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblReturnQty" runat="server" Text='<%# Eval("ReturnQty")+"  "+ Convert.ToString(Eval("UOMUnit")) %>' />
                                                                        </td>
                                                                        <td  id ="tdCancelQty" align="right" class="ClspaddingR">
                                                                            <asp:Label ID="lblCancelQty" runat="server" Text='<%# Eval("CancelQty") +"  "+ Convert.ToString(Eval("UOMUnit"))%>' />
                                                                        </td>
                                                                        <td align="center" runat="server" id="tdDelete" visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'>
                                                                            <asp:ImageButton ID="imgbtnDeleteItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                                CommandName="Remove" CommandArgument='<%# Eval("ItemID") %>' Visible='<%# Convert.ToBoolean(Eval("CanEdit"))%>'
                                                                                ToolTip="Delete" />
                                                                        </td>
                                                                        <td align="center" runat="server" id="tdDeny">
                                                                           <asp:CheckBox ID="ChkIsRequisitionToDeny" runat="server" ViewStateMode="Enabled" Checked= "false" onclick="UncheckCheck(this);"
                                                                                Enabled="true" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                    <tr id="trReqName" runat="server">
                                                        <td align="left" class="ClsBorderlight" width="20%">
                                                            <span class="ClsLabel" style="font-weight: bold">Requisition Name:</span>
                                                        </td>
                                                        <td align="left" style="background-color: #F3F3F3" width="45%">
                                                            <asp:TextBox ID="txtReqName" runat="server" MaxLength="40" CssClass="LrgTxtBox" Width="100%"
                                                                TabIndex="4"></asp:TextBox>
                                                        </td>
                                                        <td id="abc" align="left">
                                                            <span id="spanReqName" runat="server" class="ClsMdtStar">*</span>
                                                            <%--<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateStock"
                                                                Display="None" ValidationGroup="Save"></asp:CustomValidator>--%>
                                                            <asp:RequiredFieldValidator ID="reqNameRequisition" Display="None" runat="server"
                                                                ErrorMessage="Requisition Name should not be blank." ControlToValidate="txtReqName"
                                                                ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" width="20%">
                                                            <span class="ClsLabel" style="font-weight: bold">Requisition Description:</span>
                                                        </td>
                                                        <td align="left" style="background-color: #F3F3F3" width="45%">
                                                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" TextMode="MultiLine"
                                                                CssClass="LrgTxtBox" Height="100px" Width="100%" TabIndex="5"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <span id="spanReqDescription" runat="server" class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqtxtDescription" Display="None" runat="server"
                                                                ErrorMessage="Requisition Description should not be blank." ControlToValidate="txtDescription"
                                                                ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="Reg_Expr_ValidDescription" runat="server" Display="None"
                                                                ControlToValidate="txtDescription" ErrorMessage="Requisition Description should be of length less than 300."
                                                                ValidationExpression="^[\s\S]{0,300}$" CssClass="ClsLabel" ValidationGroup="Save"></asp:RegularExpressionValidator>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="3">
                                                            <asp:Button ID="btnSave" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
                                                                disable-page="true" Text="Save" Visible="True" ValidationGroup="Save" OnClick="btnSave_Click"
                                                                TabIndex="6" />
                                                            <asp:Button ID="btnSendReq" runat="server" BorderStyle="Solid" BorderWidth="1px" OnClick="btnSendReq_Click"
                                                                TabIndex="7" CssClass="ClsBtnSml" Text="Send Requisition" Visible="True" Width="120px"
                                                                ValidationGroup="Save" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="LstVwReqItems" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="btnSendReq" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="trAction" runat="server" visible="false">
                                    <td>
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel6">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2" style="width: 90%" align="center">
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight" width="20%">
                                                            <span class="ClsLabel" style="font-weight: bold">Comment:</span>
                                                        </td>
                                                        <td align="left" style="background-color: #F3F3F3" width="45%">
                                                            <asp:TextBox ID="txtComment" runat="server"  TextMode="MultiLine"
                                                                CssClass="LrgTxtBox" Height="100px" Width="100%" TabIndex="8"></asp:TextBox>
                                                        </td>
                                                        <td id="tdComment" align="left">
                                                            <span id="spanComment" runat="server" class="ClsMdtStar">*</span>
                                                            <asp:RequiredFieldValidator ID="reqtxtComment" Display="None" runat="server" ErrorMessage="Comment should not be blank."
                                                                ControlToValidate="txtComment" ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="reg_Expr_txtComment" runat="server" Display="None"
                                                                ControlToValidate="txtComment" ErrorMessage="Comment should be of length less than 300."
                                                                ValidationExpression="^[\s\S]{0,300}$" CssClass="ClsLabel" ValidationGroup="Save"> </asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <span class="ClsLabel" style="font-weight: bold">Expiry Date:</span>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="SmlCombo"></asp:TextBox>
                                                            <rjs:PopCalendar ID="calExpiryDate" runat="server" Control="txtExpiryDate" Format="dd MMM yyyy"
                                                             ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid expiry date." />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" colspan="3">
                                                            <asp:Button ID="btnApproval" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                CssClass="ClsBtnSml" Text="Process Requisition" Visible="True" ValidationGroup="Save" Width="80px"
                                                                OnClick="btnApproval_Click" TabIndex="9" />
                                                        </td>
                                                    </tr>
                                                    <tr id="trFinalApprove" runat="server" visible="false">
                                                        <td align="center" colspan="3">
                                                            <asp:Button ID="btnFinalApproval" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                CssClass="ClsBtnSml" Text="Final Approve" Visible="false" ValidationGroup="Save"
                                                                Width="100px" TabIndex="11" OnClick="btnFinalApproval_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemCommand" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr id="tr4" runat="server">
                                    <td>
                                        <asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
                                            ID="UpdatePanel1">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2" style="width: 90%" align="center">
                                                    <tr>
                                                        <td>
                                                            <asp:ListView ID="lstvwRequisitionWorkFlow" runat="server">
                                                                <LayoutTemplate>
                                                                    <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="Tr1" runat="server" class="ClsGridHeader">
                                                                            <th align="left" class="ClspaddingL">
                                                                                Status Changed by
                                                                            </th>
                                                                            <th align="left" class="ClspaddingL">
                                                                                Request Status
                                                                            </th>
                                                                            <th>
                                                                                Date
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Action") %>' />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="Tr2" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblReg_No" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                                        </td>
                                                                        <td align="left" class="ClspaddingL">
                                                                            <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Action") %>' />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblDate" runat="server" Text='<%#Eval("Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                         <asp:HiddenField ID="hidRowCount" runat="server" />
                                        <asp:HiddenField ID="hidItemName" runat="server" />
                                        <asp:HiddenField ID="hidItemCode" runat="server" />
                                        <asp:HiddenField ID="hidItemUnit" runat="server" />
                                        <asp:HiddenField ID="hidRequisitionId" runat="server" />
                                        <asp:HiddenField ID="hidRequisitionName" runat="server" />
                                        <asp:HiddenField ID="hidStatusId" runat="server" />
                                        <asp:HiddenField ID="hidNextDesignationId" runat="server" />
                                        <asp:HiddenField ID="hidRequisitionMode" runat="server" />
                                        <asp:HiddenField ID="hidIsRequisitionModified" runat="server" />
                                        <asp:HiddenField ID="hidComment" runat="server" />
                                        <asp:HiddenField ID="hidUserID" runat="server" />
                                        <asp:HiddenField ID="hidCreatorName" runat="server" />
                                        <asp:HiddenField ID="hidReqCode" runat="server" />
                                        <asp:HiddenField ID="hidRequisitionItemCount" runat="server" />
                                        <asp:HiddenField ID="hidCreatorID" runat="server" />
                                        <asp:HiddenField ID="hidCanSendRequisition" runat="server" />
                                        <asp:HiddenField ID="hidCanCreateGeneralRequisition" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidPrincipalUserId" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="2" cellspacing="2" style="width: 100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="ClsBtnSml" Height="24px" Visible = "false"
                                                        UseSubmitBehavior="false" CausesValidation="false" TabIndex="13" OnClick="btnAddItem_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtnSml" Height="24px"
                                                        UseSubmitBehavior="false" CausesValidation="false" TabIndex="13" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtnSml" Height="24px"
                                                        CausesValidation="false" Visible="false" OnClick="btnCancel_Click" 
                                                        TabIndex="12" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="LstVwReqItems" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnSendReq" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="divPopup" style="display: none; background-image: url(../images/BGline.gif);
            background-repeat: repeat;">
            <asp:UpdatePanel ID="updpnlGrade" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <table align="center">
                        <tr>
                            <td>
                                <asp:Image ID="imgItem1" runat="server" Width="50px" Height="50px" Style="cursor: pointer"
                                    TabIndex="1" />
                            </td>
                            <td>
                                <asp:Image ID="imgItem2" runat="server" Width="50px" Height="50px" Style="cursor: pointer"
                                    TabIndex="2" />
                            </td>
                            <td>
                                <asp:Image ID="imgItem3" runat="server" Width="50px" Height="50px" Style="cursor: pointer"
                                    TabIndex="3" />
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="lstvwItems" EventName="ItemDataBound" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <div id="divConfirmation" runat="server" viewstatemode="Enabled" style="position: fixed;
                            display: none; margin: 0px; padding: 0px; width: 400px; height: 100px; border-width: 0px;
                            left: 500px; top: 400px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 100px 00px;
                            background-color: white; z-index: 499;">
                            <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                background-repeat: repeat-x; color: Black; width: 390px; text-align: right;">                               
                                <span style="cursor: hand" onclick="javascript:HideConfirmationPopup();">
                                    <img alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                </span>
                                <div style="margin: 10px auto; text-align: center;" align="center">
            <div style="height:20px;"><span class="ClsLabel">Do you want to send notification message to approver?</span></div>
            <div style="margin:5px auto;width:320px;">
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ClsBtn" OnClientClick="SendNotification()" OnClick="btnYes_Click" ValidationGroup="Save" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ClsBtn" OnClientClick="CancelNotification()" OnClick="btnNo_Click" ValidationGroup="Save" />
                <asp:Button ID="btnCancelOp" runat="server" Text="Cancel" CssClass="ClsBtn" />
            </div>
            </div>
            </div>
        </div>
        <asp:HiddenField ID="hidSendNotification" runat="server" Value="Y" />
    </div>
    <script language="javascript" type="text/javascript">

        _clienthidRequisitionItemCount = "<%=this.hidRequisitionItemCount.ClientID %>";
        _clientLstVwReqItems = "<%=this.LstVwReqItems.ClientID %>";
        _clienttxtComment = "<%=this.txtComment.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidSendNotification = "<%=this.hidSendNotification.ClientID %>"
        _clientbtnSendReq = "<%=this.btnSendReq.ClientID %>"

//        _clientdentcheckboxes = '#<%=LstVwReqItems.ClientID%> > input[id*="ChkIsRequisitionToDeny"]:checkbox';
//        $("input:checkbox[id*=ChkIsRequisitionToDeny]")
//        function ValidateStock(oSrc, args) {
//            var iRowCount = 0
//            var chk = document.getElementById(_clientLstVwReqItems + "_ctrl" + iRowCount + "_lblItemCode");
//            while (chk != null) {
//                txtQty = document.getElementById(_clientLstVwReqItems + "_ctrl" + iRowCount + "_txtQty").value;
//                lblItemQty = document.getElementById(_clientLstVwReqItems + "_ctrl" + iRowCount + "_lblItemQty").innerHTML
//                if (parseFloat(txtQty) > parseFloat(lblItemQty)) {
//                    oSrc.errormessage = "Item quantity should not be greater than current stock";
//                    args.IsValid = false
//                    return true
//                }
//                iRowCount++;
//                chk = document.getElementById(_clientLstVwReqItems + "_ctrl" + iRowCount + "_lblItemCode");
//            }
//            args.IsValid = true
//            return false
//        }

        function ConfirmApproved() {
            var Count = document.getElementById(_clienthidRequisitionItemCount).value;
            var rowscount = document.getElementById(_clienthidRowCount).value;
            var denyvalue = 0;

            denyvalue = $("input:checkbox[id*=ChkIsRequisitionToDeny]:checked").length;
            var bResult = true;
            if (Count == '0') {
                if (window.confirm('You delete all the items in the requisition so requisition will delete. Do you want to continue?')) {
                    bResult = true;
                }
                else {
                    bResult = false;
                }
            }
            else if (document.getElementById(_clienttxtComment).value.length == 0) {
                alert('Comment should not be blank.');
                bResult = false;
            }
            else if (document.getElementById(_clienttxtComment).value.length > 300) {
                alert('Comment should not be greater than 300 characters.');
                bResult = false;
            }
            else if (denyvalue == 0) {
                if (!window.confirm('Are you sure you want to approve this Requisition?')) {
                    bResult = false;
                }
            }
            else if (denyvalue == rowscount) {
                if (!window.confirm('Are you sure you want to denied this Requisition?')) {
                    bResult = false;
                }
            }
            else if (denyvalue != rowscount) {
                if (!window.confirm('Are you sure you want to partially approve this Requisition?')) {
                    bResult = false;
                }
            }
            return bResult;
        }

        function ConfirmDenied() {
            var bResult = true;
            if (document.getElementById(_clienttxtComment).value.length == 0) {
                alert('Comment should not be blank.');
                bResult = false;
            }
            else if (document.getElementById(_clienttxtComment).value.length > 300) {
                alert('Comment should not be greater than 300 characters.');
                bResult = false;
            }
            else if (!window.confirm('Are you sure you want to denied this Requisition?')) {
                bResult = false;
            }
            return bResult;
        }

        function ConfirmFinalApproved() {
            var Count = document.getElementById(_clienthidRequisitionItemCount).value;
            var bResult = true;
            var bResult = true;
            if (Count == '0') {
                if (window.confirm('You delete all the items in the requisition so requisition will delete. Do you want to continue?')) {
                    bResult = true;
                }
                else {
                    bResult = false;
                }
            }
            else if (document.getElementById(_clienttxtComment).value.length == 0) {
                alert('Comment should not be blank.');
                bResult = false;
            }
            else if (document.getElementById(_clienttxtComment).value.length > 300) {
                alert('Comment should not be greater than 300 characters.');
                bResult = false;
            }
            else if (!window.confirm('Are you sure you want to final approve this Requisition?')) {
                bResult = false;
            }
            return bResult;
        }

        function AllConfirmDelete() {
            var Count = document.getElementById(_clienthidRequisitionItemCount).value;

            if (Count == parseInt(0)) {
                if (window.confirm('You delete all the items in the requisition so requisition will delete. Do you want to continue?')) {
                    bIsValid = true;
                }
                else {
                    bIsValid = false;
                }
            }
            else {
                bIsValid = true;
            }
            return bIsValid;
        }

        function CheckUncheckAll(src) {
            if (src == null)
                src = $get(_clientLstVwReqItems + '_ChkDenySelectAll');

            var iRowCount = 0;
            var chk = $get(_clientLstVwReqItems + '_ctrl' + iRowCount + '_ChkIsRequisitionToDeny');
            while (chk != null) {
                chk.checked = src.checked;

                iRowCount++;
                chk = $get(_clientLstVwReqItems + '_ctrl' + iRowCount + '_ChkIsRequisitionToDeny');
            }
        }

        function UncheckCheck(src) {
            if (src == null)
                src = $get(_clientLstVwReqItems + '_ChkIsRequisitionToDeny');
            src1 = $get(_clientLstVwReqItems + '_ChkDenySelectAll');
            var iRowCount = 0;
            var icheckcount = 0;
            var chk = $get(_clientLstVwReqItems + '_ctrl' + iRowCount + '_ChkIsRequisitionToDeny');
            while (chk != null) {
                if (chk.checked == true)
                    icheckcount++
                iRowCount++;
                chk = $get(_clientLstVwReqItems + '_ctrl' + iRowCount + '_ChkIsRequisitionToDeny');
            }
            if (iRowCount == icheckcount) {
                src1.checked = true;
            }
            else {
                src1.checked = false;
            }
        }

        function OpenPopup() {
            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Item Images", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }
        function HidePopup() {
            ContentWindow = $('#divPopup').kendoWindow({ title: "Grade Configuration Details", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow"); ContentWindow.close(); ContentWindow.center();
        }

        _clientdivConfirmation = "<%=this.divConfirmation.ClientID %>";
        function OpenConfirmationPopup() {
        $('#' + _clientdivConfirmation).fadeIn(700);
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.divConfirmation.ClientID %>").style
            var width = 600
            var height = 120
            var left = parseInt((screen.width / 2) - (width / 2.3)) - 100
            var top = parseInt((screen.height / 2) - (height / 2)) - 70
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
        }

        function HideConfirmationPopup() {
            $('#' + _clientdivConfirmation).fadeOut(700);
        }

        function SendNotification() {
            $get(_clienthidSendNotification).value = "Y";
        }

        function CancelNotification() {
            $get(_clienthidSendNotification).value = "N";
        }

    </script>
</asp:Content>
