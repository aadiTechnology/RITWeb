<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PurchaseOrderDetailsUI.aspx.cs" Inherits="PurchaseOrderDetailsUI" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel4">
            <ContentTemplate>
                <table style="width: 98%; height: 100%;" border="0" cellpadding="0" cellspacing="0">
                   <tr align="center">
                        <td align="center">
                                <asp:Label ID="lblMessage" runat="server" Visible = "false" Text="" EnableViewState="false" CssClass="LblNormal"
                                    ForeColor="Blue" Style="font-weight: bold"></asp:Label>                            
                        </td>
                    </tr>
                    <tr>
                        <td id="MainDataTable" align="center" valign="top">
                            <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0" id="tblBasic"
                                runat="server">
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="valSearch" runat="server" ShowMessageBox="false" ShowSummary="true"
                                            CssClass="ClsLabel" ValidationGroup="Search" />
                                        <asp:ValidationSummary ID="valSave" runat="server" ShowMessageBox="true" ShowSummary="false"
                                            CssClass="ClsLabel" ValidationGroup="Add" />
                                        <asp:ValidationSummary ID="valReqQty" runat="server" ShowMessageBox="true" ShowSummary="false"
                                            CssClass="ClsLabel" ValidationGroup="ReqAdd" />
                                        <asp:ValidationSummary ID="ValPOSave" runat="server" ShowMessageBox="false" ShowSummary="true"
                                            CssClass="ClsLabel" ValidationGroup="Save" />
                                        <asp:ValidationSummary ID="valAddAll" runat="server" ShowMessageBox="true" ShowSummary="false"
                                            CssClass="ClsLabel" ValidationGroup="ReqAddAll" />
                                        <asp:CustomValidator ID="cstValQty" runat="server" ClientValidationFunction="QtyValidation"
                                            Display="None" ValidateEmptyText="True" ValidationGroup="Add"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValAmount" runat="server" ClientValidationFunction="PriceValidation"
                                            Display="None" ValidateEmptyText="True" ValidationGroup="Add"></asp:CustomValidator>
                                        <asp:CustomValidator ID="cstValReqQty" runat="server" ClientValidationFunction="ReqQtyValidation"
                                            Display="None" ValidateEmptyText="True" ValidationGroup="ReqAdd"></asp:CustomValidator>
                                        <asp:RequiredFieldValidator ID="reqtxtDescription" Display="None" runat="server"
                                            ErrorMessage="PO Description should not be blank." ControlToValidate="txtDescription"
                                            ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="Reg_Expr_ValidDescription" runat="server" Display="None"
                                            ControlToValidate="txtDescription" ErrorMessage="PO Description should be of length less than 300."
                                            ValidationExpression="^[\s\S]{0,300}$" CssClass="ClsLabel" ValidationGroup="Save"> </asp:RegularExpressionValidator>
                                    </td>
                                    <td align="right" colspan="2" valign="top">
                                        <span class="LblNormalImg" style="color: Red">* Mandatory Fields</span>
                                    </td>
                                </tr>
                                <tr id="trPOTypes" runat="server" width="60%">
                                    <td class="ClsBorder" align="left" width="20%">
                                        <asp:RadioButton ID="optItemWise" runat="server" CssClass="ClsLabel" GroupName="PurchaseOrder"
                                            OnCheckedChanged="optItemWise_CheckedChanged" AutoPostBack="true" 
                                            TabIndex="1" />
                                        <span class="ClsLabel">Item Wise</span>
                                    </td>
                                    <td class="ClsBorder" align="left" width="20%">
                                        <asp:RadioButton ID="optReqWise" runat="server" CssClass="ClsLabel" GroupName="PurchaseOrder"
                                            OnCheckedChanged="optReqWise_CheckedChanged" AutoPostBack="true" 
                                            TabIndex="2" />
                                        <span class="ClsLabel">Requisition Wise</span>
                                    </td>
                                    <td class="ClsBorder" align="left" width="20%">
                                        <asp:RadioButton ID="optIndividual" runat="server" CssClass="ClsLabel" GroupName="PurchaseOrder"
                                            AutoPostBack="true" OnCheckedChanged="optIndividual_CheckedChanged" 
                                            TabIndex="3" />
                                        <span class="ClsLabel">Individual Items</span>
                                    </td>
                                </tr>
                            </table>
                            <table id="tblModify" runat="server" width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnModify" runat="server" CssClass="ClsBtn" Text="Modify" 
                                            OnClick="btnModify_Click" TabIndex="4" />
                                    </td>
                                </tr>
                            </table>                            
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                <ContentTemplate>
                                    <table id="tblItems" runat="server" style="width: 100%;" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr>
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstvwItemsOfRequisitions"
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
                                                <asp:ListView ID="lstvwItemsOfRequisitions" runat="server" DataKeyNames="ItemID,ItemUnit"
                                                    OnDataBound="lstvwItemsOfRequisitions_DataBound" OnSorting="lstvwItemsOfRequisitions_Sorting"
                                                    OnItemCommand="lstvwItemsOfRequisitions_ItemCommand" OnItemDataBound="lstvwItemsOfRequisitions_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="60%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder" align="center">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="ClspaddingL" width="10%">
                                                                    <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="Sort" CommandArgument="ItemCode"
                                                                        ForeColor="Black">
                                                                                                   Item Code</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL" width="19%">
                                                                    <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="ItemName"
                                                                        ForeColor="Black">
                                                                                                   Item Name</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL" visible="false" width="8%">
                                                                    <asp:LinkButton ID="lnkSortQty" runat="server" CommandName="Sort" CommandArgument="ItemQty"
                                                                        ForeColor="Black">
                                                                                                   Item Quantity</asp:LinkButton>
                                                                </th> 
                                                                <th  align="left" class="ClspaddingL" width="10%" >                                                               
                                                                    <asp:Label ID="lblItemQuantity" runat="server" Text="Item Quantity"></asp:Label>                               
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    <asp:LinkButton ID="lnkSortCount" runat="server" CommandName="Sort" CommandArgument="ReqCnt"
                                                                        ForeColor="Black">
                                                                                                   Requisition Count</asp:LinkButton>
                                                                </th>
                                                                <th width="3%">
                                                                    Add
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                                cellpadding="0" cellspacing="1">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="lstvwItemsOfRequisitions">
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
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="ClspaddingL" width="10%">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="20%">
                                                                <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ItemName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="10%" visible="false">
                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="10%">
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("OriginalQuantity") %>' />
                                                            </td>
                                                            <td align="center" width="7%">
                                                                <asp:Label ID="lblReqCnt" runat="server" Text='<%# Eval("ReqCnt") %>' />
                                                            </td>
                                                            <td align="center" width="3%">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("ItemID") %>' Visible="false" ToolTip="Remove From PO" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="ClspaddingL" width="10%">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="20%">
                                                                <asp:Label ID="lblClass" runat="server" Text='<%# Eval("ItemName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="10%" visible="false">
                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("ItemQty") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL" width="10%">
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("OriginalQuantity") %>' />
                                                            </td>
                                                            <td align="center" width="7%">
                                                                <asp:Label ID="lblReqCnt" runat="server" Text='<%# Eval("ReqCnt") %>' />
                                                            </td>
                                                            <td align="center" width="3%">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("ItemID") %>' Visible="false" ToolTip="Remove From PO" />
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.RequisitionBL" EnablePaging="true"
                                                    ID="lstDSobj" runat="server" SelectMethod="GetItemsFromAllRequisition" SortParameterName="sortExpression"
                                                    SelectCountMethod="CountItemsFromAllRequisition" EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter Name="asPOId" ControlID="hidPOId" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel1">
                                <ContentTemplate>
                                    <table width="100%" id="tblReqItems" runat="server" style="width: 100%;" border="0"
                                        cellpadding="0" cellspacing="0">
                                        <tr id="Tr4" runat="server">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgReqCnt" runat="server" PageSize="5" PagedControlID="LstVwRquisition"
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
                                                <asp:ListView ID="LstVwRquisition" runat="server" DataKeyNames="RequisitionID" OnDataBound="LstVwRquisition_DataBound"
                                                    OnSorting="LstVwRquisition_Sorting" OnItemCommand="LstVwRquisition_ItemCommand"
                                                    OnItemDataBound="LstVwRquisition_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="60%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" align="center">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="ClspaddingL" width="8%">
                                                                    <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="Sort" CommandArgument="RequisitionCode"
                                                                        ForeColor="Black">
                                                                                                  Code</asp:LinkButton>
                                                                </th>
                                                                <th align="left" class="ClspaddingL" width="15%">
                                                                    <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="RequisitionName"
                                                                        ForeColor="Black">
                                                                                                  Requisition</asp:LinkButton>
                                                                </th>
                                                                <th id="thCreaterName" runat="server" class="ClspaddingL" width="20%">
                                                                    <asp:LinkButton ID="lnkCreaterName" runat="server" CommandName="Sort" CommandArgument="CreaterName"
                                                                        ForeColor="Black">
                                                                                                Requestor</asp:LinkButton>
                                                                </th>
                                                                <th id="thDate" runat="server" align="center" width="15%">
                                                                    <asp:LinkButton ID="lnkSortDate" runat="server" CommandName="Sort" CommandArgument="Created_Date"
                                                                        ForeColor="Black">
                                                                                                  Request Date</asp:LinkButton>
                                                                </th>
                                                                <th id="Th6" runat="server" width="2%">
                                                                    Add
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                                cellpadding="0" cellspacing="1">
                                                                <td colspan="5">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="LstVwRquisition"
                                                                        PageSize="5">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlReqCnt_SelectedIndexChanged">
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
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("RequisitionName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCreaterName" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Created_Date","{0:dd-MMM-yyyy}")%>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("RequisitionID") %>' ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("RequisitionID") %>' Visible="false" ToolTip="Remove From PO" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("RequisitionName") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblCreaterName" runat="server" Text='<%# Eval("CreaterName") %>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Created_Date","{0:dd-MMM-yyyy}")%>' />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("RequisitionID") %>' ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("RequisitionID") %>' Visible="false" ToolTip="Remove From PO" />
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
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.RequisitionBL" EnablePaging="true"
                                                    ID="objlstVwReq" runat="server" SelectMethod="GetApproveRequisitionForPO" SortParameterName="sortExpression"
                                                    SelectCountMethod="CountRowsOfApproveRequisitionForPO" EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:ControlParameter Name="asPOId" ControlID="hidPOId" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
                                <ContentTemplate>
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;" align="center"
                                        id="tblSearch" visible="false" runat="server">
                                        <tr id="trSearch" runat="server">
                                            <td align="center" id="tblSearc" runat="server">
                                                <table cellpadding="0" cellspacing="2" width="100%">
                                                    <tr id="trCombo">
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="15%">
                                                            <sapn class="ClsLabel">Item Code/Name :</sapn>
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:TextBox ID="txtItemCode" TabIndex="5" runat="server" MaxLength="50" 
                                                                CssClass="MidTxtBox"></asp:TextBox><span
                                                                style="color: #ff0000">*</span>
                                                            <asp:RequiredFieldValidator ID="reqItemCode" Display="None" runat="server" ErrorMessage="Item Code / Name should not be blank."
                                                                ControlToValidate="txtItemCode" ValidationGroup="Search" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="6" CssClass="ClsBtnMid"
                                                                ValidationGroup="Search" OnClick="btnSearch_Click" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>                                                                                                                          
                                        <tr id="trLstItems" runat="server" visible="false">
                                            <td>
                                                <table width="100%">
                                                    <tr id="Tr1" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="dtpgIndividual" runat="server" PageSize="5" PagedControlID="LstVwIndividualItem">
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
                                                            <asp:ListView ID="LstVwIndividualItem" runat="server" DataKeyNames="ItemID,ItemCode,ItemName,UOMUnit,School_Id,UOMID,PieceCount"
                                                                OnItemDataBound="LstVwIndividualItem_ItemDataBound" OnItemCommand="LstVwIndividualItem_ItemCommand"
                                                                OnDataBound="LstVwIndividualItem_DataBound">
                                                                <LayoutTemplate>
                                                                    <table width="60%" align="center" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr class="ClsGridHeader">
                                                                            <th align="left" class="ClspaddingL" width="15%">
                                                                                Item Code
                                                                            </th>
                                                                            <th align="left" class="ClspaddingL" width="25%">
                                                                                Item Name
                                                                            </th>
                                                                            <th align="center" width="10%">
                                                                                Per Item Price
                                                                            </th>
                                                                            <th align="center" width="15%">
                                                                                Item Qty
                                                                            </th>                                                                            
                                                                            <th align="center" width="5%">
                                                                                Add
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                                            cellpadding="0" cellspacing="1">
                                                                            <td colspan="5">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="5" PagedControlID="LstVwIndividualItem">
                                                                                    <Fields>
                                                                                        <asp:TemplatePagerField>
                                                                                            <PagerTemplate>
                                                                                                <table width="100%">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                            <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlIndividualCnt_SelectedIndexChanged">
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
                                                                    <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                        <td align="left" width="5%" class="ClspaddingL">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                        </td>
                                                                        <td align="left" width="30%" class="ClspaddingL">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                        </td>
                                                                        <td align="center" width="10%">
                                                                            <asp:TextBox ID="txtPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>                                                                            
                                                                            <span style="color: #ff0000; width: 2%" id="Span1" runat="server">*</span>
                                                                        </td>
                                                                        <td align="center" width="20%">
                                                                            <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                            <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UOMUnit") %>' Width="20%" class="ClspaddingL" style="display:none" />
                                                                            <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                            <span style="color: #ff0000; width: 2%" id="star" runat="server">*</span>
                                                                        </td>                                                                        
                                                                        <td align="center" width="5%">
                                                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="Add"
                                                                                ToolTip="Add" />
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                        <td align="left" width="5%" class="ClspaddingL">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                        </td>
                                                                        <td align="left" width="30%" class="ClspaddingL">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                        </td>
                                                                        <td align="center" width="10%">
                                                                            <asp:TextBox ID="txtPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>                                                                            
                                                                            <span style="color: #ff0000; width: 2%" id="Span1" runat="server">*</span>
                                                                        </td>
                                                                        <td align="center" width="21%">
                                                                            <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("UOMUnit") %>' Width="20%" class="ClspaddingL" style="display:none" />
                                                                                <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                                <span style="color: #ff0000; width: 2%" id="star" runat="server">*</span>
                                                                        </td>                                                                        
                                                                        <td align="center" width="4%">
                                                                            <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="Add"
                                                                                ToolTip="Add" />
                                                                        </td>
                                                                    </tr>
                                                                </AlternatingItemTemplate>
                                                                <EmptyDataTemplate>
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td class="LblNoRecord" align="Center">
                                                                                No record found.
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                            </asp:ListView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.RequisitionBL" EnablePaging="true"
                                                                ID="objlstIndividual" runat="server" SelectMethod="GetAllItems" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountRowsOfItems" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:ControlParameter ControlID="txtItemCode" PropertyName="Text" Name="asName" />
                                                                    <asp:ControlParameter ControlID="hidIntemCategoryId" PropertyName="Value" Name="aiItemCategoryId"
                                                                        Type="Int32" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                            <asp:HiddenField ID="hidIntemCategoryId" runat="server" Value="0" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="optItemWise" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="optIndividual" EventName="CheckedChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="optReqWise" EventName="CheckedChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel5">
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="2" style="width: 100%" border="0" align="center">
                                        <tr id="trLstReqItems" runat="server" visible="true" width="100%">
                                            <td valign="top">
                                                <asp:ListView ID="LstVwAppReqItems" runat="server" DataKeyNames="ItemID,RequisitionID,Unit,PieceCount"
                                                    OnItemCommand="LstVwAppReqItems_ItemCommand" OnItemDataBound="LstVwAppReqItems_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table width="60%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" align="center">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="ClspaddingL" width="8%">
                                                                    Requisition Code
                                                                </th>
                                                                <th align="left" class="ClspaddingL" width="8%">
                                                                    Item Code
                                                                </th>
                                                                <th align="left" class="ClspaddingL" width="12%">
                                                                    Item Name
                                                                </th>
                                                                <th class="ClspaddingL" width="7%" style="display:none">
                                                                    Item Quantity
                                                                </th>
                                                                <th class="ClspaddingL" width="7%">
                                                                    Item Quantity
                                                                </th>
                                                                 <th class="ClspaddingL" width="5%">
                                                                    Per Item Price
                                                                </th>
                                                                <th align="center" width="10%">
                                                                    PO Quantity
                                                                </th>
                                                                <th align="center" width="6%">
                                                                    Add to PO
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                            </td>
                                                            <td class="ClspaddingL" style="display:none">
                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("OriginalQty") %>' />
                                                                <asp:Label ID="lblOrgQtyUnit" runat="server" Text='<%# Eval("Unit") %>' />
                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQty") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblOriginalQuantity" runat="server" Text='<%# Eval("OriginalQtyUnit") %>' />
                                                               <%-- <asp:HiddenField ID="hidQuantityInUnits" runat="server" Value='<%# Eval("QuantityInUnits") %>' />
                                                                <asp:HiddenField ID="hidQuantityInUOM" runat="server" Value='<%# Eval("QuantityInUOM") %>' />--%>
                                                            </td>
                                                            <td align="right" class="ClspaddingL">
                                                                <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                    MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>                                                                
                                                            </td>
                                                            <td align="right" class="ClspaddingL">
                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                    MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                <asp:DropDownList ID="cmbUnits" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="center" valign="middle">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup=""
                                                                    ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("ItemID") %>' Visible="false" ToolTip="Remove From PO" />
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                            </td>
                                                            <td class="ClspaddingL" style="display:none">
                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Eval("OriginalQty") %>' />
                                                                <asp:Label ID="lblOrgQtyUnit" runat="server" Text='<%# Eval("Unit") %>' />
                                                                <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQty") %>' />
                                                            </td>
                                                            <td align="left" class="ClspaddingL">
                                                                <asp:Label ID="lblOriginalQuantity" runat="server" Text='<%# Eval("OriginalQtyUnit") %>' />
                                                               <%-- <asp:HiddenField ID="hidQuantityInUnits" runat="server" Value='<%# Eval("QuantityInUnits") %>' />
                                                                <asp:HiddenField ID="hidQuantityInUOM" runat="server" Value='<%# Eval("QuantityInUOM") %>' />--%>
                                                            </td>
                                                            <td align="right" class="ClspaddingL">
                                                                <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                    MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>                                                                
                                                            </td>
                                                            <td align="right" class="ClspaddingL">
                                                                <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                    MaxLength="6" CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                <asp:DropDownList ID="cmbUnits" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="center" valign="middle">
                                                                <asp:ImageButton ID="imgbtnAdd" runat="server" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                    CommandName="Add" CommandArgument='<%# Eval("ItemID") %>' ValidationGroup=""
                                                                    ToolTip="Add" /><br />
                                                                <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Remove From PO" CommandName="Remove"
                                                                    CommandArgument='<%# Eval("ItemID") %>' Visible="false" ToolTip="Remove From PO" />
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="lstvwItemsOfRequisitions" EventName="ItemCommand" />
                                    <asp:AsyncPostBackTrigger ControlID="LstVwRquisition" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel UpdateMode="Always" runat="server" ID="UpdatePanel8">
                                <ContentTemplate>
                                    <table width="100%" id="tblAddAll" runat="server" style="width: 100%;" border="0"
                                        cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnAddAll" runat="server" Text="Add All" CssClass="ClsBtn" Width="120"
                                                    OnClick="btnAddAll_Click" ValidationGroup="ReqAddAll" TabIndex="7" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel7">
                                <ContentTemplate>
                                    <table width="100%" id="Table2" runat="server" style="width: 100%; padding-top: 15px"
                                        border="0" cellpadding="0" cellspacing="0">
                                         <tr id="trPODetails" align="center" style="text-align:center;" runat="server" visible="false">
                                            <td align="center">
                                                <table align="center" width="60%">
                                                    <tr>
                                                        <td style="height:15px;">
                                                        </td>
                                                    </tr>
                                                    <tr>                                                        
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="100px">
                                                            <asp:Label ID="lblLocation" runat="server" CssClass="ClsLabel" Text="Order Type" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left" style="padding-left: 5px; width: 300px; height: 32px;">
                                                            <asp:RadioButton ID="rdoPurchase" GroupName="POType" Text="Purchase" runat="server" />
                                                            <asp:RadioButton ID="rdoWork" GroupName="POType" Text="Work" runat="server" />
                                                        </td>
                                                    </tr>
                                                     <tr>                                                        
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="100px">
                                                            <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text="Expected Delivery Date" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPODeliveryDate" CssClass="SmlTxtBox" runat="server" ReadOnly="true"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cal_PODeliveryDate" runat="server" Control="txtPODeliveryDate" Format="dd MMM yyyy"
                                                                Culture="en" ShowWeekend="True" AutoPostBack="False" From-Today="true" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text="Select Vendor" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbVendors" runat="server" CssClass="ExLrgCombo"
                                                             AutoPostBack="false" Width="300px"></asp:DropDownList>
                                                             <span style="color: #ff0000">*</span>      
                                                             <asp:RequiredFieldValidator ID="reqVendorSelection" Display="None" runat="server" ErrorMessage="Vendor should be selected."
                                                                ControlToValidate="cmbVendors" InitialValue = "0" ValidationGroup="Save" SetFocusOnError="true"></asp:RequiredFieldValidator>                                                      
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text="Select Header" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="cmbHeader" runat="server" CssClass="ExLrgCombo"
                                                             AutoPostBack="false" Width="300px"></asp:DropDownList>                                                                                                                  
                                                        </td>
                                                    </tr>
                                                    <tr>                                                        
                                                        <td align="left" class="ClsBorderlight" colspan="1" width="100px">
                                                            <asp:Label ID="Label7" runat="server" CssClass="ClsLabel" Text="Discount (%)" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAmountDiscount" CssClass="ExSmlTxtBox" runat="server"></asp:TextBox>                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" class="ClsBorderlight">
                                                            <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" Text="Note" Height="16px"></asp:Label>
                                                            <sapn class="ClsLabel">:</sapn>
                                                        </td>
                                                        <td align="left">
                                                             <asp:TextBox ID="txtPONote" CssClass="SmlTxtBox" runat="server" Width="500px" Height="70px"
                                                                 TextMode="MultiLine" MaxLength="300"></asp:TextBox>                                                                                                                 
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                         </tr>
                                         <tr>
                                            <td style="height:10px;">
                                            </td>
                                         </tr>
                                        <tr>
                                            <td valign="top" style="background-color: white;">
                                                <div>
                                                    <asp:ListView ID="lstVwPurchaseOrder" runat="server" DataKeyNames="ItemID" OnItemCommand="lstVwPurchaseOrder_ItemCommand"
                                                        OnItemDataBound="lstVwPurchaseOrder_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="60%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                cellspacing="1" class="GridBorder" align="center">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" class="ClspaddingL" width="10%">
                                                                        Item Code
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="20%">
                                                                        Item Name
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="10%">
                                                                        Per Item Price
                                                                    </th>
                                                                    <th align="left" class="ClspaddingL" width="10%">
                                                                        Quantity
                                                                    </th>
                                                                    <th width="5%">
                                                                        Details
                                                                    </th>
                                                                    <th id="thDelete" runat="server" width="5%">
                                                                        Delete
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trItem" runat="server" class="ClsGridRow">
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("ItemPrice") %>' />
                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value= '<%# Eval("ItemPrice") %>'/>
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("QuantityWithUnit") %>' />
                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' CssClass="ClspaddingL" style="display:none" />
                                                                    <asp:HiddenField ID="hidUnitQty" runat="server" Value= '<%# Eval("Qty") %>'/>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Details" CommandName="Details"
                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Details" />
                                                                </td>
                                                                <td align="center" id="tdDeleteItem">
                                                                    <asp:ImageButton ID="imgbtnDeleteItem" CommandArgument='<%# Eval("ItemID") %>' runat="server"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip="Delete" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trtxtQty" runat="server" visible="false">
                                                                <td id="tdHideDetails" runat="server" colspan="1" align="center" valign="top">
                                                                    <asp:Button ID="btnHideDetails" runat="server" CssClass="ClsBtn" Text="Hide Details"
                                                                        OnClick="btnHideDetails_Click" />
                                                                </td>
                                                                <td id="tdtxtQty" runat="server" colspan="4" style="padding-right: 10px;">
                                                                    <asp:ListView ID="lstVwItemDetails" runat="server" DataKeyNames="ItemID,RequisitionID,UOMUnitCount"
                                                                        OnItemCommand="lstVwItemDetails_ItemCommand" OnItemDataBound="lstVwItemDetails_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder" align="center">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" class="ClspaddingL" width="18%">
                                                                                                    Requisition Code
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="13%">
                                                                                                    Item Code
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="15%">
                                                                                                    Item Name
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="12%">
                                                                                                    Per Item Price
                                                                                                </th>
                                                                                                <th align="center" width="31%">
                                                                                                    PO Quantity
                                                                                                </th>
                                                                                                <th align="center" width="9%">
                                                                                                    Update
                                                                                                </th>
                                                                                                <th id="thDelete" runat="server" align="center" width="9%">
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
                                                                                    <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                </td>   
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,true)" onkeyup="extractNumber(this,2,true)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPrice") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice") %>' style="display:none" />                                                                                    
                                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value='<%# Eval("ItemPrice") %>' />
                                                                                </td>                                                                            
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPOQty") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' style="display:none" />
                                                                                    <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                                    <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQtyUnit") %>' />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="ModifyItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" ToolTip="Update" />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="RemoveItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Delete" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPrice") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice") %>' style="display:none" />                                                                                    
                                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value='<%# Eval("ItemPrice") %>' />
                                                                                </td>   
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPOQty") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' style="display:none" />
                                                                                    <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                                    <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQtyUnit") %>' />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="ModifyItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" ToolTip="Update" />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="RemoveItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Delete" />
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:ListView>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("ItemPrice") %>' />
                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value='<%# Eval("ItemPrice") %>' />
                                                                </td>
                                                                <td align="left" class="ClspaddingL">
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("QuantityWithUnit") %>' />
                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' CssClass="ClspaddingL" style="display:none" />
                                                                    <asp:HiddenField ID="hidUnitQty" runat="server" Value= '<%# Eval("Qty") %>'/>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Details" CommandName="Details"
                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Details" />
                                                                </td>
                                                                <td align="center" id="tdDeleteItem">
                                                                    <asp:ImageButton ID="imgbtnDeleteItem" CommandArgument='<%# Eval("ItemID") %>' runat="server"
                                                                        ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove" ToolTip="Delete" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trtxtQty" runat="server" visible="false">
                                                                <td id="tdHideDetails" runat="server" colspan="1" align="center" valign="top">
                                                                    <asp:Button ID="btnHideDetails" runat="server" CssClass="ClsBtn" Text="Hide Details"
                                                                        OnClick="btnHideDetails_Click" />
                                                                </td>
                                                                <td id="tdtxtQty" runat="server" colspan="4" align="right" style="padding-right: 10px;">
                                                                    <asp:ListView ID="lstVwItemDetails" runat="server" DataKeyNames="ItemID,RequisitionID,UOMUnitCount"
                                                                        OnItemCommand="lstVwItemDetails_ItemCommand" OnItemDataBound="lstVwItemDetails_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder" align="center">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" class="ClspaddingL" width="18%">
                                                                                                    Requisition Code
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="13%">
                                                                                                    Item Code
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="15%">
                                                                                                    Item Name
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL" width="12%">
                                                                                                    Per Item Price
                                                                                                </th>
                                                                                                <th align="center" width="31%">
                                                                                                    PO Quantity
                                                                                                </th>
                                                                                                <th align="center" width="9%">
                                                                                                    Update
                                                                                                </th>
                                                                                                <th align="center" width="9%">
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
                                                                                    <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPrice") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice") %>' style="display:none" />                                                                                    
                                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value='<%# Eval("ItemPrice") %>' />
                                                                                </td>  
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPOQty") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' style="display:none" />
                                                                                    <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                                    <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQty") %>' />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="ModifyItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" ToolTip="Update" />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="RemoveItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Delete" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblRequisitionCode" runat="server" Text='<%# Eval("RequisitionCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemCode" runat="server" Text='<%# Eval("ItemCode") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName") %>' />
                                                                                </td>
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtItemPrice" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPrice") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblItemPrice" runat="server" Text='<%# Eval("ItemPrice") %>' style="display:none" />                                                                                    
                                                                                    <asp:HiddenField ID="hidItemPrice" runat="server" Value='<%# Eval("ItemPrice") %>' />
                                                                                </td>  
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:TextBox ID="txtQty" runat="server" onblur="extractNumber(this,2,false)" onkeyup="extractNumber(this,2,false)"
                                                                                        MaxLength="6" Text='<%# Eval("ItemPOQty") %>' CssClass="TxtAlignRght" Width="60px"></asp:TextBox>
                                                                                    <asp:Label ID="lblUnit" runat="server" Text='<%# Eval("Unit") %>' style="display:none" />
                                                                                    <asp:DropDownList ID="cmbUnit" runat="server" CssClass="SmlCombo"></asp:DropDownList>
                                                                                    <asp:HiddenField ID="hidActualQty" runat="server" Value='<%# Eval("OriginalQty") %>' />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" Text="Update" CommandName="ModifyItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ValidationGroup="ReqAdd" ToolTip="Update" />
                                                                                </td>
                                                                                <td align="center" valign="middle">
                                                                                    <asp:LinkButton ID="lnkbtnRemove" runat="server" Text="Delete" CommandName="RemoveItem"
                                                                                        CommandArgument='<%# Eval("ItemID") %>' ToolTip="Delete" />
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
                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel9">
                                <ContentTemplate>
                                    <table width="100%" id="tblSave" style="width: 60%;" border="0" cellpadding="0"
                                        cellspacing="0">
                                        <tr id="trDesc" runat="server" visible="false" width="20%">
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel" style="font-weight: bold">PO Description: </span>
                                            </td>
                                            <td align="left" width="50%">
                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" TextMode="MultiLine"
                                                    CssClass="LrgTxtBox" Height="100px" TabIndex="8" Width="100%"></asp:TextBox>
                                            </td>
                                            <td width="30%" align="left">
                                                <asp:Label ID="lblStar" runat="server" CssClass="LblNormalImg" ForeColor="Red" EnableViewState="false"
                                                    Text="*"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="3">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click"
                                                    disable-page="true" ValidationGroup="Save" Visible="false" TabIndex="9" />
                                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ClsBtn"
                                                    disable-page="true" ValidationGroup="Save" Visible="false" TabIndex="10" 
                                                    onclick="btnSubmit_Click" />
                                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false"
                                                    TabIndex="12" />
                                                <asp:Button ID="btnCancel" runat="server" CssClass="ClsBtn" Text="Cancel" Visible="true"
                                                    OnClick="btnCancel_Click" TabIndex="11" />                                                
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                            <asp:HiddenField ID="hidQty" runat="server" />
                            <asp:HiddenField ID="hidItemPrice" runat="server" />
                            <asp:HiddenField ID="hidQtyInUOM" runat="server" />
                            <asp:HiddenField ID="hidCmbUnits" runat="server" />
                            <asp:HiddenField ID="hidReqCode" runat="server" />
                            <asp:HiddenField ID="hidItemName" runat="server" />
                            <asp:HiddenField ID="hidActualQty" runat="server" />
                            <asp:HiddenField ID="hidPOName" runat="server" />
                            <asp:HiddenField ID="hidPOId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidCanModify" runat="server" />
                            <asp:HiddenField ID="hidReadOnly" runat="server" />
                            <asp:HiddenField ID="hidPOItemCount" runat="server" />
                            <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                            <asp:HiddenField ID="hidPOStatusId" runat="server" Value="0" />
                            <asp:HiddenField ID="hidIsFromApproverSCreen" runat="server" Value="N" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="LstVwRquisition" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItemsOfRequisitions" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItemsOfRequisitions" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="LstVwRquisition" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="LstVwAppReqItems" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="LstVwIndividualItem" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnAddAll" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnModify" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lstVwPurchaseOrder" EventName="ItemCommand" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <script language="javascript" type="text/javascript">
        _sClienthidQty = "<%=this.hidQty.ClientID %>"
        _clientcstValQtyId = "<%=this.cstValQty.ClientID %>"
        _sClienthidItemName = "<%=this.hidItemName.ClientID %>"
        _sClienthidReqCode = "<%=this.hidReqCode.ClientID %>"
        _clientcstValReqQty = "<%=this.cstValReqQty.ClientID %>"
        _sClienthidActualQty = "<%=this.hidActualQty.ClientID %>"
        _clientListViewId = "<%=this.LstVwAppReqItems.ClientID %>"
        _sClienthidPOItemCount = "<%=this.hidPOItemCount.ClientID %>"
        _sClienthidQtyInUOM = "<%=this.hidQtyInUOM.ClientID %>"
        _sClienthidCmbUnits = "<%=this.hidCmbUnits.ClientID %>"
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        _clienthidItemPrice = "<%=this.hidItemPrice.ClientID %>"
        _clientcstValAmountId = "<%=this.cstValAmount.ClientID %>"

        function ShowHideValidation(otxtQty, oItemName, otxtPrice) {            
            document.getElementById(_sClienthidQty).value = (otxtQty).value
            document.getElementById(_sClienthidItemName).value = oItemName.innerHTML
            document.getElementById(_clienthidItemPrice).value = (otxtPrice).value
        }
        function SetValueToHiddenField(otxtQty, oActualQty, oItemName, oReqCode, oCmbUnits, unitCount) {
            document.getElementById(_sClienthidQty).value = document.getElementById(otxtQty).value
            document.getElementById(_sClienthidItemName).value = document.getElementById(oItemName).innerHTML
            document.getElementById(_sClienthidReqCode).value = document.getElementById(oReqCode).innerHTML
            document.getElementById(_sClienthidActualQty).value = document.getElementById(oActualQty).value
            //document.getElementById(_sClienthidQtyInUOM).value = document.getElementById(unitCount).value
            document.getElementById(_sClienthidQtyInUOM).value = unitCount
            document.getElementById(_sClienthidCmbUnits).value = document.getElementById(oCmbUnits).value
        }
        function QtyValidation(oSrc, args) {
            var sQty = document.getElementById(_sClienthidQty).value
            var sItem = document.getElementById(_sClienthidItemName).value  

            if (sQty != '') {
                if (sQty == '.') {
                    document.getElementById(_clientcstValQtyId).errormessage = "Enter valid quantity for item " + sItem + "."
                    args.IsValid = false
                    return true
                }
                else if (parseFloat(sQty) != parseFloat(0)) {
                    document.getElementById(_clientcstValQtyId).errormessage = " "
                    args.IsValid = true
                    return false
                }
                else {
                    document.getElementById(_clientcstValQtyId).errormessage = "Quantity should be greater than zero for item " + sItem + "."
                    args.IsValid = false
                    return true
                }
            }
            else {
                document.getElementById(_clientcstValQtyId).errormessage = "Quantity should not be blank for item " + sItem + "."
                args.IsValid = false
                return true
            }
        }

        function PriceValidation(oSrc, args) {            
            var sItem = document.getElementById(_sClienthidItemName).value
            var sItemPrice = document.getElementById(_clienthidItemPrice).value

            if (sItemPrice == '') {
                document.getElementById(_clientcstValAmountId).errormessage = "Item Price should not be blank for item " + sItem + "."
                args.IsValid = false
                return true
            }
            else if (parseFloat(sItemPrice) == parseFloat(0)) {
                document.getElementById(_clientcstValAmountId).errormessage = "Item Price should be greater than zero for item " + sItem + "."
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            }              
            
        }


        function ReqQtyValidation(oSrc, args) {        
            var sQty = document.getElementById(_sClienthidQty).value
            var unitCount = document.getElementById(_sClienthidQtyInUOM).value
            var sCmbUnits=document.getElementById(_sClienthidCmbUnits).value
            var sItem = document.getElementById(_sClienthidItemName).value
            var sActualQty = document.getElementById(_sClienthidActualQty).value
            var sReqCode = document.getElementById(_sClienthidReqCode).value            

            if (sCmbUnits == "0") {
                sQty = parseFloat(sQty) * parseInt(unitCount);
            }

            if (sQty != '') {
                if (sQty == '.') {
                    document.getElementById(_clientcstValReqQty).errormessage = "Enter valid quantity for item " + sItem + " from requisition code " + sReqCode + "."
                    args.IsValid = false
                    return true
                }
                else if (parseFloat(sQty) != parseFloat(0)) {
                    if (parseFloat(sQty) > parseFloat(sActualQty) && parseFloat(sActualQty) != parseFloat(0)) {
                        document.getElementById(_clientcstValReqQty).errormessage = "Quantity should not be greater than actual quantity for item " + sItem + " from requisition code " + sReqCode + "."
                        args.IsValid = false
                        return true
                    }
                    else {
                        document.getElementById(_clientcstValReqQty).errormessage = " "
                        args.IsValid = true
                        return false
                    }
                }
                else {
                    document.getElementById(_clientcstValReqQty).errormessage = "Quantity should be greater than zero for item " + sItem + " from requisition code " + sReqCode + "."
                    args.IsValid = false
                    return true
                }
            }
            else {
                document.getElementById(_clientcstValReqQty).errormessage = "Quantity should not be blank for item " + sItem + " from requisition code " + sReqCode + "."
                args.IsValid = false
                return true
            }
        }
        function AddAllReqItems(iRowCount) {
            var sMessage
            var Max = 0, MaxDot = 0, i
            var ItemName = "", ItemNameDot = "", sRowNumber = "", sRowNumberDot = ""
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var ActualQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualQty"
                var ItemQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtQty"
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
            if (MaxDot == 1) {
                ItemNameDot = ItemNameDot.substring(0, ItemNameDot.length - 2)
                sRowNumberDot = sRowNumberDot.substring(0, sRowNumberDot.length - 2)
                sMessage = "Enter valid quantity for item(s) " + ItemNameDot + " at row number(s) " + sRowNumberDot + "."
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
                var ItemNameZero = "", ItemNameBlank = "", ItemNameDot = ""
                var sRowNumberZero = "", sRowNumberBlank = "", sRowNumberDot = ""
                for (i = 0; i < iRowCount; i++) {
                    RowNumber = i
                    var ActualQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualQty"
                    var ItemQty = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtQty"
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
                    sRowNumberDot = sRowNumberDot.substring(0, sRowNumberDot.length - 2)
                    if (sRowNumberDot != "") {
                        sMessage = "Enter valid quantity at row number(s) " + sRowNumberDot + "."
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
            var Count = document.getElementById(_sClienthidPOItemCount).value
            if (Count == parseInt(0)) {
                if (window.confirm('You delete all the items from this PO so this action will delete PO. Do you want to continue?')) {
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

        function RedirectToPage() {            
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('PurchaseOrderListUI.aspx?' + sEncryptedString, '_self')
            return false;
        }
    </script>
</asp:Content>
