<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="FreeItemDetailsPopup.aspx.cs" Inherits="FreeItemDetailsPopup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; height: 100%;">
            <tr>
                <td align="center" valign="top">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="height: 20px" class="ClsGrayMainTitle" valign="middle">
                                            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                <tr>
                                                    <td align="center" class="MainTitleHead" style="height: 20px">
                                                        <span style="font-weight: bold">Free Item Details</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                                    <asp:CustomValidator ID="CustItemName" runat="server" ClientValidationFunction="ValidateItemName"
                                                        Display="none"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustItemCode" runat="server" ClientValidationFunction="ValidateItemCode"
                                                        Display="none"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateBase" ErrorMessage="Free item should not be same as base item."
                                                        Display="none"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateDuplication" ErrorMessage="Free item should not be duplicate."
                                                        Display="none"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="ReqQuantity" runat="server" ErrorMessage="Quantity should not be blank."
                                                        Display="None" ControlToValidate="txtQuantity"></asp:RequiredFieldValidator>
                                                    <div style="float: right; vertical-align: top;">
                                                        <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItems" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItemDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
                                    <tr>
                                        <td align="center" valign="top">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                                        ForeColor="Blue"></asp:Label>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItems" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItemDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr align="center">
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="label7" runat="server" Text="Item Name :" class="ClsLabel"></asp:Label>
                                                    </td>
                                                    <td class="ClsHilightBGB" colspan="2">
                                                        <asp:Label ID="lblBaseItemName" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" class="ClsBorderlight">
                                                        <asp:Label ID="label11" runat="server" Text="Item Code :" class="ClsLabel"></asp:Label>
                                                    </td>
                                                    <td class="ClsHilightBGB" colspan="2">
                                                        <asp:Label ID="lblBaseItemCode" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px;">
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr id="trSearch" runat="server">
                                                    <td align="center" class="ClsBorderlight">
                                                        <span class="ClsLabel">Item Code / Name :</span>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" class="MidTxtBox"></asp:TextBox>
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtnMid" CausesValidation="false"
                                                            OnClick="btnSearch_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="upnl1" runat="server">
                                                <ContentTemplate>
                                                    <table width="100%">
                                                        <tr id="trDataPager" runat="server">
                                                            <td align="center">
                                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="5" PagedControlID="lstvwFreeItems">
                                                                    <Fields>
                                                                        <asp:TemplatePagerField>
                                                                            <PagerTemplate>
                                                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                    CssClass="LblNrmlB" />
                                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
                                                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                    CssClass="LblNrmlB" />
                                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
                                                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                    CssClass="LblNrmlB" />
                                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
                                                                                <br />
                                                                            </PagerTemplate>
                                                                        </asp:TemplatePagerField>
                                                                    </Fields>
                                                                </asp:DataPager>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ListView ID="lstvwFreeItems" runat="server" DataKeyNames="Id" OnDataBound="lstvwFreeItems_DataBound"
                                                                    OnItemCommand="lstvwFreeItems_ItemCommand" OnItemUpdating="lstvwFreeItems_ItemUpdating"
                                                                    OnSorting="lstvwFreeItems_Sorting">
                                                                    <LayoutTemplate>
                                                                        <table width="95%" align="center" runat="server" id="Table1" style="color: #333333"
                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingLR">
                                                                                    <asp:LinkButton ID="LinkButton2" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                        CommandArgument="Title" CausesValidation="false" ForeColor="Black">Title</asp:LinkButton>
                                                                                </th>
                                                                                <th align="left">
                                                                                    <asp:LinkButton ID="LinkButton6" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                        CommandArgument="ItemCode" CausesValidation="false" ForeColor="Black">Item Code</asp:LinkButton>
                                                                                </th>
                                                                                <th align="left" style="width: 100px">
                                                                                    <asp:LinkButton ID="lnkColor" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                        CommandArgument="Color" CausesValidation="false" ForeColor="Black">Color</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right" style="width: 60px">
                                                                                    <asp:LinkButton ID="LinkButton1" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                        Style="float: inherit; padding-right: 10px;" CommandArgument="Size" CausesValidation="false"
                                                                                        ForeColor="Black">Size</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right">
                                                                                    <asp:LinkButton ID="LinkButton3" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                        Style="float: inherit; padding-right: 10px;" CommandArgument="Price" CausesValidation="false"
                                                                                        ForeColor="Black">Sale Price</asp:LinkButton>
                                                                                </th>
                                                                                <th align="center">
                                                                                    Add Free Item
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                                                                <td colspan="9">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwFreeItems" PageSize="5">
                                                                                        <Fields>
                                                                                            <asp:TemplatePagerField>
                                                                                                <PagerTemplate>
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
                                                                        <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                            <td align="left" class="paddingLR">
                                                                                <asp:Label ID="lblName" runat="server" CssClass="ClsLabel" Text='<%# Eval("Title") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingR">
                                                                                <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="left" class="ClspaddingR">
                                                                                <asp:Label ID="Label2" runat="server" CssClass="ClsLabel" Text='<%# Eval("Color") %>' />
                                                                            </td>
                                                                            <td align="right" class="ClspaddingR">
                                                                                <asp:Label ID="Label3" runat="server" CssClass="ClsLabel" Style="float: inherit;"
                                                                                    Text='<%# Eval("Size") %>' />
                                                                            </td>
                                                                            <td align="right" class="ClspaddingR">
                                                                                <asp:Label ID="Label4" runat="server" CssClass="ClsLabel" Style="float: inherit;"
                                                                                    Text='<%# Eval("Price") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnStudent" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/Selection5.gif"
                                                                                    CommandName="Add" CommandArgument='<%# Eval("Id") %>' ToolTip="Add" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
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
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                <asp:HiddenField ID="hidStoreCategoryId" runat="server" />
                                                                <asp:HiddenField ID="HidId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidBaseItemVariationId" runat="server" />
                                                                <%--<asp:HiddenField ID="hidBaseItemMasterId" runat="server" />--%>
                                                                <asp:HiddenField ID="hidItemVariationId" runat="server" />
                                                                <asp:HiddenField ID="hidTitle" runat="server" />
                                                                <asp:HiddenField ID="hidStoreItemMasterId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidItemType" runat="server" Value="" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.eStoreBL.StoreItemVariationBL" EnablePaging="true"
                                                                    ID="lstDSobj" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                                    SelectCountMethod="GetCount" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                            Type="int32" />
                                                                        <asp:ControlParameter ControlID="hidStoreItemMasterId" Name="aiStoreItemMasterId"
                                                                            Type="String" PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                                        <asp:ControlParameter ControlID="hidSortExpression" Name="SortExpression" Type="String"
                                                                            PropertyName="Value" />
                                                                        <asp:ControlParameter ControlID="hidSortDirection" Name="SortDirection" Type="String"
                                                                            PropertyName="Value" />
                                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr id="trControls" runat="server" align="center">
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr align="center">
                                                            <td align="center" class="ClsBorderlight" style="width: 100px;height:25px;">
                                                                <asp:Label ID="label1" runat="server" Text="Item Name :" class="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <asp:Label ID="lblItemName" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height:25px;">
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="label9" runat="server" Text="Item Code :" class="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td class="ClsBorderlight">
                                                                <asp:Label ID="lblItemCode" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="ClsBorderlight">
                                                                <asp:Label ID="label10" runat="server" Text="Quantity :" class="ClsLabel"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtQuantity" runat="server" MaxLength="50" class="MidTxtBox"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItems" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItemDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                <ContentTemplate>
                                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                        OnClick="btnCancel_Click" />
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItems" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItemDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                <ContentTemplate>
                                                    <table width="95%" align="center">
                                                        <tr>
                                                            <td>
                                                                <asp:ListView ID="lstvwFreeItemDetails" runat="server" DataKeyNames="Id" OnItemCommand="lstvwFreeItemDetails_ItemCommand"
                                                                    OnItemDataBound="lstvwFreeItemDetails_ItemDataBound" OnItemUpdating="lstvwFreeItemDetails_ItemUpdating">
                                                                    <LayoutTemplate>
                                                                        <table width="100%" align="center" runat="server" id="Table1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="left" class="paddingLR">
                                                                                    <span class="ClsLabel">Item Name</span>
                                                                                </th>
                                                                                <th align="left" width="100px">
                                                                                    <span class="ClsLabel">Item Code</span>
                                                                                </th>
                                                                                <th align="right" width="100px">
                                                                                    <span class="ClsLabel" style="float:inherit;padding-right:5px;">Quantity</span>
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    <span class="ClsLabel" style="float:inherit;">Edit</span>
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    <span class="ClsLabel" style="float:inherit;">Delete</span>
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                        </table>
                                                                    </LayoutTemplate>
                                                                    <ItemTemplate>
                                                                        <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                            <td align="left" class="paddingLR">
                                                                                <asp:Label ID="lblItemTitle" runat="server" Text='<%# Eval("Title") %>' />
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:Label ID="lblItemCodeInLst" runat="server" CssClass="ClsLabel" Text='<%# Eval("ItemCode") %>' />
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:Label ID="Label6" runat="server" CssClass="ClsLabel" style="float:inherit;padding-right:5px;" Text='<%# Eval("Quantity") %>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnEditItem" runat="server" CommandName="Update" CausesValidation="false" style="float:inherit;"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgbtnDeleteItem" runat="server" CommandName="Remove" CausesValidation="false" style="float:inherit;"
                                                                                    ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwFreeItemDetails" EventName="ItemCommand" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript" src="../Scripts/Validations.js"></script>
    <script language="javascript" type="text/javascript">
        _clientlblItemName = '<%=lblItemName.ClientID %>'
        _clientlblItemCode = '<%=lblItemCode.ClientID %>'

        function CloseWindow() {
            window.close();
        }

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function ValidateItemCode(oSrc, args) {
            var ItemCodeLabel = document.getElementById(_clientlblItemCode).innerHTML;
            if (ItemCodeLabel == '') {
                oSrc.errormessage = "Item code should not be blank."
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateItemName(oSrc, args) {
            var ItemNameLabel = document.getElementById(_clientlblItemName).innerHTML;
            if (ItemNameLabel == '') {
                oSrc.errormessage = "Item Name should not be blank."
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateBase(src, args) {
            var ItemNameLabel = document.getElementById(_clientlblItemName).innerHTML;
            var ItemCodeLabel = document.getElementById(_clientlblItemCode).innerHTML;
            var baseItemName = document.getElementById('<%=this.lblBaseItemName.ClientID %>').innerHTML;
            var baseItemCode = document.getElementById('<%=this.lblBaseItemCode.ClientID %>').innerHTML;
            if (baseItemName == ItemNameLabel && baseItemCode == ItemCodeLabel) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateDuplication(src, args) {
            var ItemNameLabel = document.getElementById(_clientlblItemName).innerHTML;
            var ItemCodeLabel = document.getElementById(_clientlblItemCode).innerHTML;
            var isFound = false;
            $('[id$=_lblItemTitle]').each(function () {
                var itemName = $(this).html()
                var id = this.id.replace('_lblItemTitle', '_lblItemCodeInLst')
                var itemCode = document.getElementById(id).innerHTML

                if (ItemNameLabel == itemName && ItemCodeLabel == itemCode) {
                    isFound = true;
                }
            })

            if (isFound) {
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

    </script>
</asp:Content>
