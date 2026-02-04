<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StoreItemListUI.aspx.cs" Inherits="StoreItemListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="100%">
        <tr>
            <td>
                <div style="float: right;">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="Mandatory Fields"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:UpdatePanel ID="Up2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                        CssClass="clsLabel" Font-Bold="true"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="ddlStoreCategory" EventName="SelectedIndexChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderLight" align="left">
                            <span class="ClsLabel">Store Category : </span>
                        </td>
                        <td align="left" valign="top">
                            <asp:DropDownList ID="ddlStoreCategory" runat="server" CssClass="LrgCombo" AutoPostBack="True"
                                OnSelectedIndexChanged="ddlStoreCategory_SelectedIndexChanged" />
                            <span class="ClsMdtStar">* </span>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderLight" align="left">
                            <asp:Label ID="lblStandards" runat="server" CssClass="ClsLabel" Text="Associated Standards :"></asp:Label>
                            <asp:CheckBox ID="ChkSelectAllStd" runat="server" onclick="CheckAll(this)" />
                        </td>
                        <td align="left">
                            <asp:CheckBoxList ID="chklstStandards" runat="server" RepeatDirection="Horizontal"
                                onclick="CheckMain();" CssClass="ClsLabel" RepeatColumns="5">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td class="ClsBorderLight" align="left">
                            <span class="ClsLabel">Item Code / Title / Price / Quantity / Reorder Quantity : </span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="BtnSearch" runat="server" Text="Search" class="ClsBtn" CausesValidation="false"
                                OnClick="BtnSearch_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right">
                <div align="center" style="height: 25px;width:100px;margin-right:10px;" class="ClsGreenBG">
                    <asp:HyperLink ID="hlnkStock" runat="server">Add Stock</asp:HyperLink>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table id="tbl1" align="center" width="98%" runat="server">
                            <tr id="trDtPgCount" runat="server" visible="true">
                                <td align="center">
                                    <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwStoreItemDetails"
                                        PageSize="20">
                                        <Fields>
                                            <asp:TemplatePagerField>
                                                <PagerTemplate>
                                                    <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.StartRowIndex + 1%>" />
                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                    <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                    <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                    <br />
                                                </PagerTemplate>
                                            </asp:TemplatePagerField>
                                        </Fields>
                                    </asp:DataPager>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ListView ID="lstvwStoreItemDetails" runat="server" DataKeyNames="Id,ItemName,IsVariationAvailable"
                                        ViewStateMode="Enabled" OnItemDataBound="lstvwStoreItemDetails_ItemDataBound"
                                        OnItemCommand="lstvwStoreItemDetails_ItemCommand" OnDataBound="lstvwStoreItemDetails_DataBound"
                                        OnSorting="lstvwStoreItemDetails_Sorting">
                                        <LayoutTemplate>
                                            <table id="lstvwtable1" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                class="GridBorder" width="100%">
                                                <tr id="trheader" runat="server" class="ClsGridHeader">
                                                    <th align="left" style="width:100px">
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandName="Sort" CommandArgument="ItemCode"
                                                            class="ClsLabel" Text="Item Code" CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 300px;">
                                                        <asp:LinkButton ID="lnkBtnTitle" runat="server" CommandName="Sort" CommandArgument="Title"
                                                            class="ClsLabel" Text="Title" CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 100px;">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Sort" CommandArgument="Price"
                                                            class="ClsLabel" Text="Price" CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 100px;">
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandName="Sort" CommandArgument="Quantity"
                                                            class="ClsLabel" Text="Quantity" CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 100px;">
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandName="Sort" CommandArgument="ReorderQuantity"
                                                            class="ClsLabel" Text="Reorder Qty." CausesValidation="false" ForeColor="Black"> </asp:LinkButton>
                                                    </th>
                                                    <th align="left" style="width: 150px;">
                                                        <span class="ClsLabel" style="float: inherit">UOM</span>
                                                    </th>
                                                    <th align="left" style="width: 300px;">
                                                        <span class="ClsLabel" style="float: inherit">Associated Standards</span>
                                                    </th>
                                                    <th align="center" width="150px">
                                                        <span class="ClsLabel" style="float: inherit">Manage Variations</span>
                                                    </th>
                                                    <th align="center" style="width: 100px;">
                                                        <span class="ClsLabel" style="float: inherit">Edit</span>
                                                    </th>
                                                    <th align="center" style="width: 100px;">
                                                        <span class="ClsLabel" style="float: inherit">Delete</span>
                                                    </th>
                                                    <th align="center" style="width: 100px">
                                                        <span class="clsLabel" style="float: inherit; color: Black;">Free Item</span>
                                                    </th>
                                                    <%--<th align="center" style="width: 100px;">
                                                            <asp:Label ID="lblAddStock" runat="server" Text="Add Stock"></asp:Label>
                                                    </th>--%>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                                    <td colspan="11">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStoreItemDetails"
                                                            PageSize="20">
                                                            <Fields>
                                                                <asp:TemplatePagerField>
                                                                    <PagerTemplate>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" Text="Select a page:" />
                                                                                    <asp:DropDownList ID="ddlCnt" ViewStateMode="Enabled" runat="server" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged"
                                                                                        AutoPostBack="true">
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
                                            <tr id="trItemtemplate" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                <td align="left">
                                                    <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Text='<%# Eval("ItemCode") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblTitle" runat="server" CssClass="ClsLabel" Text='<%# Eval("ItemName") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%# Eval("Price") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Text='<%# Eval("ReorderQuantity") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label5" runat="server" CssClass="ClsLabel" Text='<%# Eval("UOM") %>'></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblStandards" runat="server" CssClass="ClsLabel" Text='<%# Eval("StandardNames") %>'>
                                                    </asp:Label>
                                                </td>
                                                <td id="td1" runat="server" align="center">
                                                    <asp:LinkButton ID="lnkbtnVarioation" runat="server" CssClass="SMSLblSMlBlue" Text="Manage" CommandName="ADDVARIATION" />
                                                    <asp:Label ID="lblVariation" runat="server" Text="N/A" CssClass="ClsLabel" style="float:inherit;" Visible="false"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td id="Delete" runat="server" align="center">
                                                    <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                                <td align="center">
                                                    <asp:LinkButton ID="lnkbtnFreeItem" runat="server" CausesValidation="false" CssClass="SMSLblSMlBlue" Text="Add/Edit" CommandName="ADDFREEITEM" />
                                                    <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
                                                </td>
                                                <%--<td align="center">
                                                    <asp:LinkButton ID="lnkAddStock" runat="server" Text="Add Stock"
                                                      CausesValidation="false" ToolTip="Click to Add stock."></asp:LinkButton>
                                                </td>--%>
                                            </tr>
                                        </ItemTemplate>
                                        <EmptyDataTemplate>
                                            <tr style="width: 800px">
                                                <td align="center" class="LblNoRecord">
                                                    No record Found
                                                </td>
                                            </tr>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.StoreItemBL" EnablePaging="true" ID="lstvwObjDS"
                                        runat="server" SelectMethod="GetStoreItemList" SortParameterName="sortExpression"
                                        SelectCountMethod="CountStoreItem" EnableCaching="false">
                                        <SelectParameters>
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                Type="int32" />
                                            <asp:ControlParameter ControlID="ddlStoreCategory" Name="aiStoreCategory" Type="Int32"
                                                PropertyName="Text" />
                                            <asp:ControlParameter ControlID="hidStandards" Name="asStandardIds" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                            <asp:ControlParameter Name="sortExpression" ControlID="hidSortExpression" Type="String"
                                                PropertyName="Value" />
                                            <asp:ControlParameter Name="sortDirection" ControlID="hidSortDirection" Type="String"
                                                PropertyName="Value" />
                                            <asp:Parameter Name="maximumRows" Type="Int32" />
                                            <asp:Parameter Name="startRowIndex" Type="Int32" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidStandards" runat="server" Value="" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="BtnSearch" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddlStoreCategory" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemDetails" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:LocalizedResources, Add%>"
                    CssClass="ClsBtn" BorderWidth="1px" CausesValidation="False" OnClick="btnAdd_Click" />
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        function ConfirmDelete() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this item?')) {
                bResult = false
            }
            return bResult
        }

        function CheckAll(obj) {
            if (obj.checked) {
                $('[id*=_chklstStandards_]').attr('checked', 'checked')
            }
            else {
                $('[id*=_chklstStandards_]').removeAttr('checked')
            }
        }

        function CheckMain() {
            if ($('[id*=_chklstStandards_]').length == $('[id*=_chklstStandards_]:checked').length)
                $('[id$=ChkSelectAllStd]').attr('checked', 'checked')
            else
                $('[id$=ChkSelectAllStd]').removeAttr('checked')
        }

        function OpenFeeItemPopup(str) {        
            var ss = $('[id$=' + str + '_hidQueryString]').val()
            window.open('FreeItemDetailsPopup.aspx?' + ss, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=800')
        }

//        function OpenPopup(querystring) {
//            window.open('../eStore/StoreItemStockDetailsPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700').focus();
//            return false;
//        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
