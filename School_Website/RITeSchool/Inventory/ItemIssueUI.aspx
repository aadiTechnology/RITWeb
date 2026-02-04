<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ItemIssueUI.aspx.cs" Inherits="ItemIssueUI"
    Title="Untitled Page" %>
    <%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td>
                <asp:UpdatePanel ID="UPanelSenderSearch" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="2" align="center" width="100%">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr style="width: 100%">
                                            <td id="tdError" runat="server" align="left" valign="top" colspan="3">
                                                <asp:ValidationSummary ID="valsumItems" runat="server" CssClass="ClsLabel" ShowMessageBox="False"
                                                    ShowSummary="True" ValidationGroup="valsumItems" />
                                            </td>
                                            <td align="right" valign="top" colspan="1">
                                                <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                                                    ForeColor="Red" EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="4">
                                                <asp:Label ID="lblSuccess" runat="server" CssClass="ClsLabelNrml" Font-Bold="true"
                                                    ForeColor="Blue" Style="text-align: center" EnableViewState="false" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:RadioButton ID="optIssueItem" runat="server" AutoPostBack="True" GroupName="Notice"
                                                    Text="Issue Requisition Item" Checked="True" OnCheckedChanged="optIssueItem_CheckedChanged"></asp:RadioButton>
                                                <asp:RadioButton ID="optAddstock" runat="server" GroupName="Notice" Text="Return Requisition Item"
                                                    AutoPostBack="True" OnCheckedChanged="optAddstock_CheckedChanged"></asp:RadioButton>
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td align="left" colspan="4">
                                                <asp:Label ID="lblSearch" runat="server" CssClass="ClsLblLgnd" EnableViewState="False"
                                                    Text="Search Requisition :" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" style="width: 16%">
                                                <asp:Label ID="lblDesignation" runat="server" Text="Sender Designation :" Font-Bold="False"
                                                    CssClass="ClsLabel" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlDesignation" runat="server" CssClass="ExLrgTxtBox" AutoPostBack="True"
                                                    TabIndex="1" OnSelectedIndexChanged="ddlDesignation_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td id="tdLblSenderName" class="ClsBorderLight" runat="server">
                                                <asp:Label ID="lblSenderName" runat="server" CssClass="ClsLabel" Font-Bold="False"
                                                    Text="Sender Name :" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td id="tdDDLSenderName" runat="server">
                                                <asp:DropDownList ID="ddlSenderName" runat="server" CssClass="ExLrgTxtBox" AutoPostBack="True"
                                                    TabIndex="2" OnSelectedIndexChanged="ddlSenderName_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="ClsBorderLight" style="width: 30%">
                                                <asp:CheckBox ID="chkIsGeneral" runat="server" AutoPostBack="True" Text="Show general requisition only"
                                                    OnCheckedChanged="chkIsGeneral_CheckedChanged"></asp:CheckBox>
                                            </td>
                                        </tr>
                                        <tr>
                                        <td class="ClsBorderLight" style="width:7%">
                                        <asp:Label ID="Label1" runat="server" Text="Expected return date:" Font-Bold="False"
                                                    CssClass="ClsLabel" EnableViewState="False"></asp:Label>
                                        </td >
                                        <td style="width:7%">
                                        <asp:TextBox ID="txtExpectedReturnDate"
											 runat="server"
											 CssClass="SmlTxtBox"
											 style="vertical-align: middle;" />
								<rjs:PopCalendar ID="dtExpectedReturnDate"
												 runat="server"
												 Control="txtExpectedReturnDate"
												 Format="dd mmm yyyy"
												 ShowWeekend="True"
												 ShowErrorMessage="false" />
                                        </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr id="trItemCount" runat="server">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwRequisition"
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
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <div>
                                        <asp:ListView ID="lstvwRequisition" runat="server" DataKeyNames="RequisitionID,User_Id"
                                            OnDataBound="lstvwRequisition_DataBound" OnSorting="lstvwRequisition_Sorting"
                                            OnItemCommand="lstvwRequisition_ItemCommand" DataSourceID="objDSLstvwReq">
                                            <LayoutTemplate>
                                                <table width="80%" align="center" runat="server" id="Table1" style="color: #333333"
                                                    cellpadding="0" cellspacing="0" class="GridBorder">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" runat="server" id="tblRequisition" style="color: #333333" cellpadding="0"
                                                                cellspacing="1">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th id="thReqCode" runat="server" align="left" class="ClspaddingL" width="11%">
                                                                        <asp:LinkButton ID="lnkCode" runat="server" CommandName="Sort" CommandArgument="RequisitionCode"
                                                                            ForeColor="Black">
                                                                                Requisition Code</asp:LinkButton>
                                                                    </th>
                                                                    <th id="thRequisition" runat="server" align="left" class="ClspaddingL" width="25%">
                                                                        <asp:LinkButton ID="lnkRequisition" runat="server" CommandName="Sort" CommandArgument="RequisitionName"
                                                                            ForeColor="Black">
                                                                                            Requisition Items</asp:LinkButton>
                                                                    </th>
                                                                    <th id="thReqDate" runat="server" align="left" class="ClspaddingL" width="13%">
                                                                        <asp:LinkButton ID="lnkReqDate" runat="server" CommandName="Sort" CommandArgument="RequisitionDate"
                                                                            ForeColor="Black">
                                                                                            Requisition Date</asp:LinkButton>
                                                                    </th>
                                                                    <th id="thAppDate" runat="server" align="left" class="ClspaddingL" width="13%">
                                                                        <asp:LinkButton ID="lnkAppDate" runat="server" CommandName="Sort" CommandArgument="ApprovedDate"
                                                                            ForeColor="Black">
                                                                                            Approved Date</asp:LinkButton>
                                                                    </th>
                                                                    <th id="thName" runat="server" align="left" class="ClspaddingL" width="35%">
                                                                        <asp:LinkButton ID="lnkName" runat="server" CommandName="Sort" CommandArgument="CreaterName"
                                                                            ForeColor="Black">
                                                                                            Sender Name</asp:LinkButton>
                                                                    </th>
                                                                    <th id="thView" runat="server" align="center" width="8%">
                                                                        View
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" id="trDataPager">
                                                                    <td colspan="6">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwRequisition">
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
                                                <tr id="trCode" runat="server" class="ClsGridRow">
                                                    <td align="left" id="tdCode" class="ClspaddingL">
                                                        <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdRequisition" class="ClspaddingL">
                                                        <asp:Label ID="lblRequisition" runat="server" Text='<%# Eval("RequisitionName")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdRequisitionDate" class="ClspaddingL">
                                                        <asp:Label ID="lblRequisitionDate" runat="server" Text='<%#Eval("RequisitionDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdApprovedDate" class="ClspaddingL">
                                                        <asp:Label ID="lblApprovedDate" runat="server" Text='<%#Eval("Approved_Date","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdSenderName" class="ClspaddingL">
                                                        <asp:Label ID="lblSenderName" runat="server" Text='<%# Eval("CreaterName")%>'></asp:Label>
                                                    </td>
                                                    <td id="View" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtnViewItem" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View" CommandName="View" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trCode" runat="server" class="ClsGridAltRow">
                                                    <td align="left" id="tdCode" class="ClspaddingL">
                                                        <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdRequisition" class="ClspaddingL">
                                                        <asp:Label ID="lblRequisition" runat="server" Text='<%# Eval("RequisitionName")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdRequisitionDate" class="ClspaddingL">
                                                        <asp:Label ID="lblRequisitionDate" runat="server" Text='<%#Eval("RequisitionDate","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdApprovedDate" class="ClspaddingL">
                                                        <asp:Label ID="lblApprovedDate" runat="server" Text='<%#Eval("Approved_Date","{0:dd-MMM-yyyy}")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdSenderName" class="ClspaddingL">
                                                        <asp:Label ID="lblSenderName" runat="server" Text='<%# Eval("CreaterName")%>'></asp:Label>
                                                    </td>
                                                    <td id="View" runat="server" align="center">
                                                        <asp:ImageButton ID="imgbtnViewItem" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                            ToolTip="View" CommandName="View" />
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
                                    <asp:ObjectDataSource TypeName="BusinessLogic.StockIssueDetailsBL" EnablePaging="true"
                                        ID="objDSLstvwReq" runat="server" SelectMethod="GetAllApprovedRequisitions" SortParameterName="sortExpression"
                                        SelectCountMethod="CountRequisitionRow" EnableCaching="false">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter Name="asSenderDesgID" Type="String" ControlID="ddlDesignation"
                                                PropertyName="SelectedValue" DefaultValue="0" />
                                            <asp:ControlParameter Name="asSenderID" Type="String" ControlID="ddlSenderName" PropertyName="SelectedValue"
                                                DefaultValue="0" />
                                            <asp:ControlParameter Name="abIsGeneral" Type="Int32" ControlID="chkIsGeneral" PropertyName="Checked"
                                                DefaultValue="0" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                            <tr>
                                <td id="tdItemsIssue" valign="top" runat="server">
                                    <div>
                                        <asp:ListView ID="lstvwReqItems" runat="server" OnItemDataBound="lstvwReqItems_ItemDataBound" ViewStateMode = "Enabled"
                                            DataKeyNames="ItemID,UOMUnit,IsConsiderForDetailLevel,PieceCount,RequisitionID" OnItemCommand="lstvwReqItems_ItemCommand">
                                            <LayoutTemplate>
                                                <table width="80%" align="center" runat="server" id="Table1" style="color: #333333"
                                                    cellpadding="0" cellspacing="0" class="GridBorder">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" runat="server" id="tblReqItems" style="color: #333333" cellpadding="0"
                                                                cellspacing="1">
                                                                <tr id="trReqItemsHeader" runat="server" class="ClsGridHeader">
                                                                    <th id="thItemCode" runat="server" align="left" class="ClspaddingL">
                                                                        Item Code
                                                                    </th>
                                                                    <th id="thItemName" runat="server" align="left" class="ClspaddingL">
                                                                        Items Name
                                                                    </th>
                                                                    <th id="thStockQuantity" runat="server" align="center">
                                                                        Stock Quantity
                                                                    </th>
                                                                    <th id="thReqQuantity" runat="server" align="center">
                                                                        Required Quantity
                                                                    </th>
                                                                    <th id="thIssueQuantity" runat="server" align="center" style="width: 27%">
                                                                        Issue Quantity
                                                                    </th>
                                                                    <th id="th1" runat="server" align="center" style="width: 27%">
                                                                        Comment
                                                                    </th>
                                                                    <th id="thIssue" runat="server" align="center">
                                                                        Select
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
                                                <tr id="trItemCode" runat="server" class="ClsGridRow">
                                                    <td align="left" id="tdItemCode" class="ClspaddingL">
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                        <asp:HiddenField ID="hidPieceCount" runat="server" Value='<%# Eval("PieceCount") %>' />
                                                        <asp:HiddenField ID="hidIssueItemId" runat="server" Value='<%# Eval("ItemID") %>' />
                                                    </td>
                                                    <td align="center" id="tdItemName" class="ClspaddingL">
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdStockQuantity" class="ClspaddingL">
                                                        <asp:Label ID="lblStockQuantity" runat="server" Text='<%# Eval("StockQty")%>' CssClass="ClsLabel"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblStockQtyUnit" runat="server" Text="Unit(s)" CssClass="ClsLabel" />
                                                    </td>
                                                    <td align="left" id="tdReqQuantity" class="ClspaddingL">
                                                        <asp:Label ID="lblReqQuantity" runat="server" Text='<%# Eval("ReqQty")%>' CssClass="ClsLabel"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblReqQtyUnit" runat="server" Text="Unit(s)" CssClass="ClsLabel" />
                                                    </td>
                                                    <td id="tdIssueQuantity" align="center" class="Clspadding">
                                                        <asp:TextBox ID="txtIssueQuantity" runat="server" type="txtIssueQuantity" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"ReqQty")))%>'
                                                            onblur="extractNumber(this,3,true);" onkeyup="extractNumber(this,2,true); CheckUncheckItemCheckboxes(this);" onkeypress="return blockNonNumbers (this, event, true, true);"
                                                            onpaste="event.returnValue=false" ondrop="event.returnValue=false" MaxLength="7" />
                                                        <asp:DropDownList ID="cmbUnits" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdComment" align="center">
                                                        <asp:TextBox ID="txtComment" runat="server" type="TextBox" MaxLength="400" Width="75%"></asp:TextBox>
                                                        <asp:Label ID="lblMendetory" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td id="tdIssue" runat="server" align="center">
                                                        <asp:Button ID="btnItemIssue" runat="server" CommandName="Issue" CssClass="ClsBtn"
                                                            Text="Issue" ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>'
                                                            Visible="false" OnClick="btnItemIssue_Click" />
                                                        <asp:LinkButton ID="btnIssue" runat="server" CommandName="IssueItem" Text="Select"
                                                            ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>' Visible="false"></asp:LinkButton>
                                                        <asp:Button ID="btnItemCancel" runat="server" CommandName="ItemCancel" CssClass="ClsBtn"
                                                            Text="Cancel" ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>'
                                                            Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr id="trItemDetails" runat="server" visible="false">
                                                    <td colspan="7" align="center" id="tdItemDetails" runat="server">
                                                        <table width="40%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Panel runat="server" ID="pnl1" Height="300px" Width="700px" ScrollBars="Vertical">
                                                                        <asp:ListView ID="lstItemDetails" runat="server" DataKeyNames="Id" OnItemDataBound="lstItemDetails_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="tblItems" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="center" width="5%">
                                                                                            <input type="checkbox" onclick="CheckUncheckAll(this)" />
                                                                                        </th>
                                                                                        <th class="paddingL" width="30%">
                                                                                            Item Specification Code
                                                                                        </th>
                                                                                        <th class="paddingL" width="65%">
                                                                                            Description
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkItemSelect" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblItemSpcCode" runat="server" CssClass="ClsLabel" Text='<%#Eval("SpecificationCode") %>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkItemSelect" runat="server" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblItemSpcCode" runat="server" CssClass="ClsLabel" Text='<%#Eval("SpecificationCode") %>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                        </asp:ListView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Button ID="btnIssueItem" runat="server" CssClass="ClsBtn" Text="Issue" OnClick="btnIssueItem_Click" />
                                                                    <asp:Button ID="btnCancelIssue" runat="server" CssClass="ClsBtn" Text="Cancel" OnClick="btnCancelIssue_Click"
                                                                        CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <AlternatingItemTemplate>
                                                <tr id="trItemCode" runat="server" class="ClsGridAltRow">
                                                    <td align="left" id="tdItemCode" class="ClspaddingL">
                                                        <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode")%>'></asp:Label>
                                                        <asp:HiddenField ID="hidPieceCount" runat="server" Value='<%# Eval("PieceCount") %>' />
                                                        <asp:HiddenField ID="hidIssueItemId" runat="server" Value='<%# Eval("ItemID") %>' />
                                                    </td>
                                                    <td align="left" id="tdItemName" class="ClspaddingL">
                                                        <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                    </td>
                                                    <td align="left" id="tdStockQuantity" class="ClspaddingL">
                                                        <asp:Label ID="lblStockQuantity" runat="server" Text='<%# Eval("StockQty")%>' CssClass="ClsLabel"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblStockQtyUnit" runat="server" Text="Unit(s)" CssClass="ClsLabel" />
                                                    </td>
                                                    <td align="center" id="tdReqQuantity" class="ClspaddingL">
                                                        <asp:Label ID="lblReqQuantity" runat="server" Text='<%# Eval("ReqQty")%>' CssClass="ClsLabel"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblReqQtyUnit" runat="server" Text="Unit(s)" CssClass="ClsLabel" />
                                                    </td>
                                                    <td id="tdIssueQuantity" align="center" class="Clspadding">
                                                        <asp:TextBox ID="txtIssueQuantity" runat="server" type="txtIssueQuantity" Text='<%# (Convert.ToString(DataBinder.Eval(Container.DataItem,"ReqQty")))%>'
                                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false); CheckUncheckItemCheckboxes(this);" 
                                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false" 
                                                            ondrop="event.returnValue=false" MaxLength="7" />
                                                        <asp:DropDownList ID="cmbUnits" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td id="tdComment" align="center">
                                                        <asp:TextBox ID="txtComment" runat="server" type="TextBox" MaxLength="400" Width="75%"></asp:TextBox>
                                                        <asp:Label ID="lblMendetory" runat="server" Text="*" ForeColor="Red"></asp:Label>
                                                    </td>
                                                    <td id="tdIssue" runat="server" align="center">
                                                        <asp:Button ID="btnItemIssue" runat="server" CommandName="Issue" CssClass="ClsBtn"
                                                            Text="Issue" ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>'
                                                            Visible="false" OnClick="btnItemIssue_Click" />
                                                        <asp:LinkButton ID="btnIssue" runat="server" CommandName="IssueItem" Text="Select"
                                                            ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>' Visible="false"></asp:LinkButton>
                                                        <asp:Button ID="btnItemCancel" runat="server" CommandName="Cancel" CssClass="ClsBtn"
                                                            Text="Cancel" ValidationGroup="IssueItem" CommandArgument='<%# Eval("ItemID") %>'
                                                            Visible="false" />
                                                    </td>
                                                </tr>
                                                <tr id="trItemDetails" runat="server" visible="false">
                                                    <td colspan="7" align="center" id="tdItemDetails" runat="server">
                                                        <table width="50%">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Panel runat="server" ID="pnl1" Height="300px" Width="700px" ScrollBars="Vertical">
                                                                        <asp:ListView ID="lstItemDetails" runat="server" DataKeyNames="Id" OnItemDataBound="lstItemDetails_ItemDataBound">
                                                                            <LayoutTemplate>
                                                                                <table width="100%" runat="server" id="tblItems" style="color: #333333" cellpadding="0"
                                                                                    cellspacing="1" class="GridBorder">
                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                        <th align="left" width="5%">
                                                                                        </th>
                                                                                        <th class="ClspaddingL" width="30%">
                                                                                            Item Specification Code
                                                                                        </th>
                                                                                        <th class="ClspaddingL" width="65%">
                                                                                            Description
                                                                                        </th>
                                                                                    </tr>
                                                                                    <tr id="itemPlaceholder" runat="server">
                                                                                    </tr>
                                                                                </table>
                                                                            </LayoutTemplate>
                                                                            <ItemTemplate>
                                                                                <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkItemSelect" runat="server" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblItemSpcCode" runat="server" CssClass="ClsLabel" Text='<%#Eval("SpecificationCode") %>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </ItemTemplate>
                                                                            <AlternatingItemTemplate>
                                                                                <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                    <td align="center">
                                                                                        <asp:CheckBox ID="chkItemSelect" runat="server" />
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblItemSpcCode" runat="server" CssClass="ClsLabel" Text='<%#Eval("SpecificationCode") %>'></asp:Label>
                                                                                    </td>
                                                                                    <td class="ClspaddingL">
                                                                                        <asp:Label ID="lblDescription" runat="server" CssClass="ClsLabel" Text='<%#Eval("Description") %>'></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </AlternatingItemTemplate>
                                                                            <EmptyDataTemplate>
                                                                                <div class="LblNoRecord">
                                                                                    No Record Found.
                                                                                </div>
                                                                            </EmptyDataTemplate>
                                                                        </asp:ListView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Button ID="btnIssueItem" runat="server" CssClass="ClsBtn" Text="Issue" OnClick="btnIssueItem_Click" />
                                                                    <asp:Button ID="btnCancelIssue" runat="server" CssClass="ClsBtn" Text="Cancel" OnClick="btnCancelIssue_Click"
                                                                        CausesValidation="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                    <asp:HiddenField ID="hidRequisitionID" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidUserID" runat="server" />
                                    <asp:HiddenField ID="hidItemName" runat="server" />
                                    <asp:HiddenField ID="hidIssueQty" runat="server" />
                                    <asp:HiddenField ID="hidItemUnit" runat="server" />
                                    <asp:HiddenField ID="hidSelectedItemQuantity" runat="server" />
                                    <asp:HiddenField ID="hidSelectedItemComment" runat="server" />
                                    <asp:HiddenField ID="hidItemId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidCurrentStock" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidUOM" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidIssueItemCount" runat="server" />
                                     <asp:HiddenField ID="hidCancelRemainig" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidIssueQuantity" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidIssueUnits" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidStockBalance" runat="server" Value="0" />
                                   <%-- <asp:HiddenField ID="hidCancelRemainig" runat="server" Value="N" />
                                    <asp:HiddenField ID="hidIssueQuantity" runat="server" Value="N" />
                                    <asp:HiddenField ID="hidIssueUnits" runat="server" Value="N" />
                                    <asp:HiddenField ID="hidStockBalance" runat="server" Value="N" />--%>
                                    <asp:HiddenField ID="hidComment" runat="server" Value="N" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" style="width: 50%;">
                                    <asp:Button UseSubmitBehavior="false" ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn"
                                        CausesValidation="False" TabIndex="3" PostBackUrl="~/RITeSchool/Inventory/ItemManagementUI.aspx" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlDesignation" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="ddlSenderName" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="Sorting" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="DataBound" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwRequisition" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwReqItems" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
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
            <div style="height:20px;"><span class="ClsLabel">Do you want to cancel remaining items?</span></div>
            <div style="margin:5px auto;width:320px;">
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ClsBtn" OnClick="btnYes_Click" />
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ClsBtn" OnClick="btnNo_Click" />
                <asp:Button ID="btnCancelOp" runat="server" Text="Cancel" CssClass="ClsBtn" />
            </div>
            </div>
            </div>
        </div>
    <script language="javascript" type="text/javascript">

        _clientHidItemName = "<%=this.hidItemName.ClientID %>";
        _clientHidIssueQty = "<%=this.hidIssueQty.ClientID %>";
        _clientHidItemUnit = "<%=this.hidItemUnit.ClientID %>";
        _clientchkIsGeneral = "<%=this.chkIsGeneral.ClientID %>";
        _clientlstvwReqItems = "<%=this.lstvwReqItems.ClientID %>";
        _clienttdItemsIssue = "<%=this.tdItemsIssue.ClientID %>";
        _clienthidIssueItemCount = "<%=this.hidIssueItemCount.ClientID %>";
        _clientdivConfirmation = "<%=this.divConfirmation.ClientID %>";
        _clienthidIssueQuantity = "<%=this.hidIssueQuantity.ClientID %>"
        _clienthidIssueUnits = "<%=this.hidIssueUnits.ClientID %>"
        _clienthidStockBalance = "<%=this.hidStockBalance.ClientID %>"
        _clienthidComment = "<%=this.hidComment.ClientID %>"
        _clienthidItemId = "<%=this.hidItemId.ClientID %>"

        function CheckUncheckItemCheckboxes(obj) {
            var rowIndex = 0
            var IssueQuantity = obj.value;
            var listviewitem = document.getElementById(_clienthidIssueItemCount).value;
            for (var i = 0; i < listviewitem; i++) {
                var chk = document.getElementById(_clientlstvwReqItems + "_ctrl" + rowIndex + "_lstItemDetails_ctrl" + i + "_chkItemSelect")

                if (i < IssueQuantity) {
                    chk.checked = true;
                }
                else {
                    chk.checked = false;
                }
            }

            rowIndex++;
        }

        function HideConfirmationPopup() {
            $('#' + _clientdivConfirmation).fadeOut(700);
        }

    </script>
    <script src="../Scripts/Inventory/ItemIssueUI.js?version=1.5" type="text/javascript"></script>
</asp:Content>
