<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PurchaseOrderListUI.aspx.cs" Inherits="PurchaseOrderListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        &nbsp;<table width="97%">
            <tr  id="trCombo">
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr id="trLstItems" runat="server">
                                    <td>
                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
                                            <ContentTemplate>
                                                <table width="100%">
                                                    <tr id="Tr5" runat="server">
                                                        <td align="center">
                                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwPOList">
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
                                                            <asp:ListView ID="lstvwPOList" runat="server" DataKeyNames="PurchaseOrderID,Editable,IsFinalApproved"
                                                                OnDataBound="lstvwPOList_DataBound" OnItemCommand="lstvwPOList_ItemCommand" OnItemDataBound="lstvwPOList_ItemDataBound"
                                                                OnSorting="lstvwPOList_Sorting">
                                                                <LayoutTemplate>
                                                                    <table width="70%" align="center" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                        cellspacing="1" class="GridBorder">
                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                            <th align="left" class="ClspaddingL" width="20%">
                                                                                <asp:LinkButton ID="lnkSortCode" runat="server" CommandName="Sort" CommandArgument="PurchaseOrderCode"
                                                                                    ForeColor="Black">
                                                                                                  Code</asp:LinkButton>
                                                                            </th>
                                                                            <th align="left" class="ClspaddingL">
                                                                                <asp:LinkButton ID="lnkSortName" runat="server" CommandName="Sort" CommandArgument="PurchaseOrderName"
                                                                                    ForeColor="Black">
                                                                                                  Purchase Order</asp:LinkButton>
                                                                            </th>
                                                                            <th width="20%">
                                                                                <asp:LinkButton ID="lnkSortDate" runat="server" CommandName="Sort" CommandArgument="Insert_Date"
                                                                                    ForeColor="Black">
                                                                                                  Create Date</asp:LinkButton>
                                                                            </th>
                                                                            <th width="10%">
                                                                                View
                                                                            </th>
                                                                            <th width="10%" id="thDelete" runat="server">
                                                                                Delete
                                                                            </th>
                                                                            <th width="10%" id="thExport" runat="server">
                                                                                <asp:Label ID="Label2" runat="server" Text="Export"> </asp:Label>
                                                                            </th>
                                                                        </tr>
                                                                        <tr id="itemPlaceholder" runat="server">
                                                                        </tr>
                                                                        <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager">
                                                                            <td colspan="5">
                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwPOList" PageSize="20">
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
                                                                                                        <td align="right" cssclass="LblNormal">
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
                                                                        <td align="left">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("PurchaseOrderCode") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("PurchaseOrderName") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:ImageButton ID="imgbtnViewPO" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                CommandArgument='<%# Convert.ToBoolean(Eval("Editable"))%>' ToolTip="View" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:ImageButton ID="imgbtnDeletePO" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                                                                runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                                Visible='<%# Convert.ToBoolean(Eval("Editable"))%>' ToolTip="Delete" />
                                                                        </td>
                                                                        <td align="center">                                                                           
                                                                            <asp:LinkButton ID="lbtnExport" runat="server" ViewStateMode="Enabled" Text="Export"
                                                                                CommandName="EXPORT"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </ItemTemplate>
                                                                <AlternatingItemTemplate>
                                                                    <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                                        <td align="left">
                                                                            <asp:Label ID="lblCode" runat="server" Text='<%# Eval("PurchaseOrderCode") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("PurchaseOrderName") %>' CssClass="ClspaddingL" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblReqDate" runat="server" Text='<%#Eval("Insert_Date","{0:dd-MMM-yyyy}")%>' />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:ImageButton ID="imgbtnViewPO" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                CommandArgument='<%# Convert.ToBoolean(Eval("Editable"))%>' ToolTip="View" />
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:ImageButton ID="imgbtnDeletePO" CommandArgument='<%# Eval("PurchaseOrderID") %>'
                                                                                runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" CommandName="Remove"
                                                                                Visible='<%# Convert.ToBoolean(Eval("Editable"))%>' ToolTip="Delete" />
                                                                        </td>
                                                                        <td align="center">                                                                           
                                                                            <asp:LinkButton ID="lbtnExport" runat="server" ViewStateMode="Enabled" Text="Export"
                                                                                CommandName="EXPORT"></asp:LinkButton>
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
                                                        <td>
                                                            <asp:ObjectDataSource TypeName="BusinessLogic.PurchaseOrderBL" EnablePaging="true"
                                                                ID="lstDSobj" runat="server" SelectMethod="GetPOList" SortParameterName="sortExpression"
                                                                SelectCountMethod="CountRowsOfPO" EnableCaching="false">
                                                                <SelectParameters>
                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                    <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                    <asp:ControlParameter Name="asPOId" ControlID="hidPOId" PropertyName="Value" />
                                                                    <asp:ControlParameter Name="asRequesterId" ControlID="hidUserId" PropertyName="Value" />
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="lstvwPOList" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnAdd" runat="server" BorderStyle="Solid" Height="24px" CssClass="ClsBtnSml"
                                            CausesValidation="false" Text="Add" Visible="True" PostBackUrl="~/RITeSchool/Inventory/PurchaseOrderDetailsUI.aspx" />
                                        <asp:Button ID="btnBack" runat="server" BorderStyle="Solid" Height="24px" CssClass="ClsBtnSml"
                                            CausesValidation="false" Text="Back" Visible="false" OnClientClick = "if(!SetPostBackURL()) return false;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidPOId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidUserId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidStatusId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidIsFromApproverScreen" runat="server" Value = "N" />
                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <%--<asp:AsyncPostBackTrigger ControlID="lstvwPOList" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwPOList" EventName="Sorting" />--%>
                            <asp:PostBackTrigger ControlID="lstvwPOList" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this purchase order?');                               
        }

        function SetPostBackURL() {            
            var sEncryptedString = document.getElementById(_clienthidQueryString).value;
            window.open('PurchaseOrderApproverUI.aspx?' + sEncryptedString, '_self')
            return false;
        }
    </script>
</asp:Content>
