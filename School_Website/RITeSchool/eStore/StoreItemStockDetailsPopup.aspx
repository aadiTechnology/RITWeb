<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StoreItemStockDetailsPopup.aspx.cs" Inherits="StoreItemStockDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
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
                                        <td align="left">
                                            <asp:UpdatePanel ID="upnl5" runat="server" UpdateMode="Always">
                                                <ContentTemplate>
                                                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                                                    <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="MRP should not be blank or zero."
                                                        Display="None" ClientValidationFunction="ValidateMRP"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Quantity should not be blank or zero."
                                                        Display="None" ClientValidationFunction="ValidateQuantity"></asp:CustomValidator>
                                                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Net Price should not be blank or zero."
                                                        Display="None" ClientValidationFunction="ValidatePrice"></asp:CustomValidator>
                                                    <asp:RequiredFieldValidator ID="ReqDate" runat="server" ErrorMessage="Date should not be blank."
                                                        ControlToValidate="txtDate" Display="None"></asp:RequiredFieldValidator>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ViewStateMode="Enabled"
                                                        Display="None" ControlToValidate="txtDescription" ErrorMessage="Length of Description should not exceed 500 characters."
                                                        CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div style="float: right; vertical-align: top;">
                                                <span style="width: 150px" class="ClsMdtStar">* Mandatory Fields </span>
                                            </div>
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
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                                            <ContentTemplate>
                                                                <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                                                    ForeColor="Blue"></asp:Label>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <table>
                                                            <tr>
                                                                <td align="center" class="ClsBorderlight">
                                                                    <asp:Label ID="label1" runat="server" Text="Store Category :" CssClass="ClsLabel"></asp:Label>
                                                                </td>
                                                                <td class="ClsHilightBGB" colspan="2">
                                                                    <asp:Label ID="lblStoreCategory" runat="server" Text="" CssClass="ClsLabel"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="clsBorderLight">
                                                                    <span class="ClsLabel">Item Code / Name : </span>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:TextBox ID="txtSearch" runat="server" CssClass="LrgTxtBox" MaxLength="100"></asp:TextBox>
                                                                </td>
                                                                <td align="left">
                                                                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="false"
                                                                        OnClick="btnSearch_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:ListView ID="lstvwVariationDetails" runat="server" OnDataBound="lstvwVariationDetails_DataBound"
                                                                                OnSorting="lstvwVariationDetails_Sorting" DataKeyNames="Id" OnItemCommand="lstvwVariationDetails_ItemCommand">
                                                                                <LayoutTemplate>
                                                                                    <table align="center" cellpadding="0" cellspacing="1" class="GridBorder" width="100%">
                                                                                        <tr id="TrHeader" runat="server" class="ClsGridHeader">
                                                                                            <th align="left" style="width: 100px">
                                                                                                <asp:LinkButton ID="lnkColor" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="Color" CausesValidation="false" ForeColor="Black">Color</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="left" style="width: 75px">
                                                                                                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="Size" CausesValidation="false" ForeColor="Black">Size</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <asp:LinkButton ID="LinkButton6" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="ItemCode" CausesValidation="false" ForeColor="Black">Item Code</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="left">
                                                                                                <asp:LinkButton ID="LinkButton2" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="Title" CausesValidation="false" ForeColor="Black">Title</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <asp:LinkButton ID="LinkButton3" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="Price" CausesValidation="false" ForeColor="Black">Price</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <asp:LinkButton ID="LinkButton4" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                                                    CommandArgument="Quantity" CausesValidation="false" ForeColor="Black">Quantity</asp:LinkButton>
                                                                                            </th>
                                                                                            <th align="center" style="width: 100px">
                                                                                                <asp:Label ID="lblAddStock" runat="server" Text="Select"></asp:Label>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                        <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                                                                            <td colspan="7">
                                                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVariationDetails"
                                                                                                    PageSize="5">
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
                                                                                    <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblColor" runat="server" Text='<%#Eval("Color") %>' CssClass="clsLabel"></asp:Label>
                                                                                            <asp:HiddenField ID="hidIdNew" runat="server" Value='<%#Eval("Id") %>' />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblSize" runat="server" Text='<%#Eval("Size") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%#Eval("ItemCode") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="Label9" runat="server" Text='<%#Eval("Price") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="Label10" runat="server" Text='<%#Eval("Quantity") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td id="tdlnkupload" runat="server" align="center">
                                                                                            <asp:LinkButton ID="lnkAddStock" runat="server" Text="Select" CausesValidation="false"
                                                                                                CommandName="SELECT" ToolTip="Click to select."></asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <EmptyDataTemplate>
                                                                                    <tr>
                                                                                        <td class="LblNoRecord" align="center">
                                                                                            <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </EmptyDataTemplate>
                                                                            </asp:ListView>
                                                                            <asp:ObjectDataSource TypeName="BusinessLogic.eStoreBL.StoreItemVariationBL" EnablePaging="true"
                                                                                ID="objdsVariations" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                                                SelectCountMethod="GetCount" EnableCaching="false">
                                                                                <SelectParameters>
                                                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                        Type="int32" />
                                                                                    <asp:ControlParameter ControlID="hidStoreItemMasterId" Name="aiStoreItemMasterId"
                                                                                        Type="String" PropertyName="Value" />
                                                                                    <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                                                    <asp:ControlParameter ControlID="hidFirstSortExpression" Name="SortExpression" Type="String"
                                                                                        PropertyName="Value" />
                                                                                    <asp:ControlParameter ControlID="hidFirstSortDirection" Name="SortDirection" Type="String"
                                                                                        PropertyName="Value" />
                                                                                    <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                                    <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                                </SelectParameters>
                                                                            </asp:ObjectDataSource>
                                                                            <asp:HiddenField ID="hidStoreItemMasterId" runat="server" Value="0" />
                                                                            <asp:HiddenField ID="hidFirstSortExpression" runat="server" />
                                                                            <asp:HiddenField ID="hidFirstSortDirection" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="Sorting" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 20px;">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:ListView ID="lstvwStockItems" runat="server" DataKeyNames="Id,ItemVariationDetailId"
                                                                                OnItemCommand="lstvwStockItems_ItemCommand" OnItemDataBound="lstvwStockItems_ItemDataBound">
                                                                                <LayoutTemplate>
                                                                                    <table align="center" cellpadding="0" cellspacing="1" class="GridBorder" width="100%">
                                                                                        <tr id="TrHeader" runat="server" class="ClsGridHeader">
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">Color</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 75px">
                                                                                                <span class="ClsLabel">Size</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">Item Code</span>
                                                                                            </th>
                                                                                            <th align="left">
                                                                                                <span class="ClsLabel">Title</span>
                                                                                            </th>
                                                                                            <th align="left">
                                                                                                <span class="ClsLabel">UOM</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">MRP</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">Discount</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">Quantity</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">GST Category</span>
                                                                                            </th>
                                                                                            <th align="left" style="width: 100px">
                                                                                                <span class="ClsLabel">Price</span>
                                                                                            </th>
                                                                                            <th align="center" style="width: 100px">
                                                                                                <asp:Label ID="lblAddStock" runat="server" Text="Delete"></asp:Label>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblColor" runat="server" Text='<%#Eval("Color") %>' CssClass="clsLabel"></asp:Label>
                                                                                            <asp:HiddenField ID="hidIdNew" runat="server" Value='<%#Eval("Id") %>' />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblSize" runat="server" Text='<%#Eval("Size") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblItemCode" runat="server" Text='<%#Eval("ItemCode") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblTitle" runat="server" Text='<%#Eval("Title") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblUOM" runat="server" Text='<%#Eval("UOM") %>' CssClass="clsLabel"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtMRP" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("MRP") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtDiscount" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Discount") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtQuantity" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("NewQuantity") %>'></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblGST" runat="server" Text='<%#Eval("GST") %>' CssClass="clsLabel"></asp:Label>
                                                                                            <asp:HiddenField ID="hidGSTCategoryId" runat="server" Value='<%#Eval("GSTCategoryId") %>' />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtTPrice" runat="server" CssClass="SmlTxtBox" Text='<%#Eval("Price") %>'
                                                                                                onkeypress="return false;" onpaste="event.returnValue=false"></asp:TextBox>
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                                ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <EmptyDataTemplate>
                                                                                    <tr>
                                                                                        <td class="LblNoRecord" align="center">
                                                                                            <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </EmptyDataTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemStockDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="upnl3" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <table>
                                                                    <tr>
                                                                        <td align="center" class="ClsBorderlight" style="width: 150px">
                                                                            <asp:Label ID="Label6" runat="server" Text="Amount :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAmount" runat="server" CssClass="LrgTxtBox" MaxLength="8" Width="130"
                                                                                onkeypress="return false;" onpaste="event.returnValue=false"></asp:TextBox>
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                        <td align="center" class="ClsBorderlight" style="width: 150px">
                                                                            <asp:Label ID="Label4" runat="server" Text="Transport Amount :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTransportAmount" runat="server" CssClass="LrgTxtBox" MaxLength="8"
                                                                                Width="130" onblur="extractNumber(this,2,true);" onkeyup="extractNumber(this,2,true);"
                                                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                                                ondrop="event.returnValue=false"></asp:TextBox>
                                                                        </td>
                                                                        <td align="center" class="ClsBorderlight" style="width: 150px">
                                                                            <asp:Label ID="Label5" runat="server" Text="Adjusted Amount :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAdjustedAmount" runat="server" CssClass="LrgTxtBox" MaxLength="8"
                                                                                Width="130" onblur="extractNumber(this,2,true);" onkeyup="extractNumber(this,2,true);"
                                                                                onkeypress="return blockNonNumbers (this, event, true, false);" onpaste="event.returnValue=false"
                                                                                ondrop="event.returnValue=false"></asp:TextBox>
                                                                        </td>
                                                                        <td align="center" class="ClsBorderlight" style="width: 150px">
                                                                            <asp:Label ID="lblPrice" runat="server" Text="Net Price :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPrice" runat="server" CssClass="LrgTxtBox" MaxLength="8" Width="130"
                                                                                onkeypress="return false;" onpaste="event.returnValue=false"></asp:TextBox>
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" class="ClsBorderlight">
                                                                            <asp:Label ID="lblDate" runat="server" Text="Date :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtDate" CssClass="SmlCombo" runat="server" ReadOnly="true" AutoPostBack="True"
                                                                                Width="130px"></asp:TextBox>
                                                                            <rjs:PopCalendar ID="calDate" runat="server" Control="txtDate" ShowErrorMessage="false"
                                                                                InvalidDateMessage="Please select valid end date." Format="dd MMM yyyy" ShowWeekend="True" />
                                                                            <span class="ClsMdtStar">*</span>
                                                                        </td>
                                                                        <td align="center" class="ClsBorderlight">
                                                                            <asp:Label ID="lblDesc" runat="server" Text="Description :" CssClass="ClsLabel"></asp:Label>
                                                                        </td>
                                                                        <td colspan="5">
                                                                            <asp:TextBox ID="txtDescription" runat="server" CssClass="ExLrgTxtBox" Width="100%"
                                                                                MaxLength="280"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemStockDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwStockItems" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>
                                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" OnClick="btnCancel_Click"
                                                                    CausesValidation="false" />
                                                            </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemStockDetails" EventName="ItemCommand" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                                <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                                            </Triggers>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%">
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td align="left" class="clsBorderLight">
                                                        <span class="ClsLabel">Item Code / Name : </span>
                                                    </td>
                                                    <td align="left">
                                                        <asp:TextBox ID="txtFilter" runat="server" CssClass="LrgTxtBox" MaxLength="100"></asp:TextBox>
                                                    </td>
                                                    <td align="left">
                                                        <asp:Button ID="btnShow" runat="server" Text="Search" CssClass="ClsBtn" CausesValidation="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <table>
                                                        <tr id="trItemCount" runat="server">
                                                            <td align="center">
                                                                <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStoreItemStockDetails"
                                                                    Visible="true">
                                                                    <Fields>
                                                                        <asp:TemplatePagerField>
                                                                            <PagerTemplate>
                                                                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false"
                                                                                    Text="<%# Container.StartRowIndex + 1%>" />
                                                                                <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                                    Text=" To " />
                                                                                <asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
                                                                                <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                                    Text=" Out Of " />
                                                                                <asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
                                                                                <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" EnableViewState="false"
                                                                                    Text="Records " />
                                                                                <br />
                                                                            </PagerTemplate>
                                                                        </asp:TemplatePagerField>
                                                                    </Fields>
                                                                </asp:DataPager>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:ListView ID="lstvwStoreItemStockDetails" runat="server" DataKeyNames="StockMasterId"
                                                                    OnItemCommand="lstvwStoreItemStockDetails_ItemCommand" OnDataBound="lstvwStoreItemStockDetails_DataBound"
                                                                    OnItemDataBound="lstvwStoreItemStockDetails_ItemDataBound" OnSorting="lstvwStoreItemStockDetails_Sorting">
                                                                    <LayoutTemplate>
                                                                        <table cellpadding="0" cellspacing="1" class="GridBorder" style="color: #333333"
                                                                            align="center">
                                                                            <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th align="center" class="paddingL" style="width: 100px;">
                                                                                    <asp:LinkButton ID="lnkbtnDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                                                        CommandArgument="Date" CommandName="Sort">Date</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right" class="paddingR" style="width: 150px; padding-right: 5px;">
                                                                                    <asp:LinkButton ID="lnkbtnQuantity" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                                                        Style="float: inherit" CommandArgument="TotalPrice" CommandName="Sort">Total Price</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right" style="width: 150px; padding-right: 5px;">
                                                                                    <asp:LinkButton ID="lnkbtnTransportAmount" runat="server" CausesValidation="false"
                                                                                        CssClass="clsLabel" Style="float: inherit" CommandArgument="TransportAmount"
                                                                                        CommandName="Sort">Transport Amount</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right" style="width: 150px; padding-right: 5px;">
                                                                                    <asp:LinkButton ID="LinkButton9" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                                                        Style="float: inherit" CommandArgument="AdjustedAmount" CommandName="Sort">Adjusted Amount</asp:LinkButton>
                                                                                </th>
                                                                                <th align="right" style="width: 100px; padding-right: 5px;">
                                                                                    <asp:LinkButton ID="LinkButton10" runat="server" CausesValidation="false" CssClass="clsLabel"
                                                                                        Style="float: inherit" CommandArgument="NetPrice" CommandName="Sort">Net Price</asp:LinkButton>
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    <asp:Label ID="lblEdit" runat="server" Text="Edit"></asp:Label>
                                                                                </th>
                                                                                <th align="center" width="100px">
                                                                                    <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                                                <td colspan="7">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStoreItemStockDetails">
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
                                                                        <tr id="trItemtemplates" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                            <td align="center">
                                                                                <asp:Label ID="lblDate" CssClass="clsLabel" runat="server" Text='<%# Eval("Date") %>'
                                                                                    Style="float: inherit" />
                                                                            </td>
                                                                            <td align="right" style="padding-right: 5px;">
                                                                                <asp:Label ID="lblTotalAmount" CssClass="clsLabel" runat="server" Text='<%# Eval("TotalAmount") %>'
                                                                                    Style="float: inherit" />
                                                                            </td>
                                                                            <td align="right" style="padding-right: 5px;">
                                                                                <asp:Label ID="lblTransportAmount" CssClass="clsLabel" runat="server" Text='<%# Eval("TransportAmount") %>'
                                                                                    Style="float: inherit" />
                                                                            </td>
                                                                            <td align="right" style="padding-right: 5px;">
                                                                                <asp:Label ID="Label2" CssClass="clsLabel" runat="server" Text='<%# Eval("AdjustedAmount") %>'
                                                                                    Style="float: inherit" />
                                                                            </td>
                                                                            <td align="right" style="padding-right: 5px;">
                                                                                <asp:Label ID="Label3" CssClass="clsLabel" runat="server" Text='<%# Eval("NetPrice") %>'
                                                                                    Style="float: inherit" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                                    ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                                            </td>
                                                                            <td align="center">
                                                                                <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                                                    ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                    <EmptyDataTemplate>
                                                                        <table align="center">
                                                                            <tr>
                                                                                <td class="LblNoRecord" align="center">
                                                                                    No record found.
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EmptyDataTemplate>
                                                                </asp:ListView>
                                                                <asp:ObjectDataSource TypeName="BusinessLogic.eStoreBL.StoreItemStockDetailsBL" EnablePaging="true"
                                                                    ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                                                    SortParameterName="asSortExpression" EnableCaching="false">
                                                                    <SelectParameters>
                                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                        <asp:ControlParameter Name="aiItemMasterId" ControlID="hidItemMasterId" PropertyName="Value" />
                                                                        <asp:ControlParameter Name="aiItemVariationDetailId" ControlID="hidItemVariationDetailId"
                                                                            PropertyName="Value" />
                                                                        <asp:ControlParameter Name="asFilter" ControlID="txtFilter" PropertyName="Text" />
                                                                        <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                                                        <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                                                        <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                        <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                    </SelectParameters>
                                                                </asp:ObjectDataSource>
                                                                <asp:HiddenField ID="hidItemMasterId" runat="server" />
                                                                <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                                                <asp:HiddenField ID="hidItemVariationDetailId" runat="server" />
                                                                <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                                <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                                <asp:HiddenField ID="hidGSTData" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="lstvwStoreItemStockDetails" EventName="ItemCommand" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                    <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="center">
                            <td align="center">
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function hidepopup() {
            window.opener.location.reload();
            window.close();
            window.opener.focus();
        }

        function CalculatePrice(index) {
            var mrp = $('[id$=' + index + '_' + 'txtMRP ]').val();
            var discount = $('[id$=' + index + '_' + 'txtDiscount ]').val();
            var qty = $('[id$=' + index + '_' + 'txtQuantity ]').val();

            if (mrp == '')
                mrp = '0'

            if (discount == '')
                discount = '0'

            if (qty == '')
                qty = '0'

            var gst = $('[id$=' + index + '_' + 'hidGSTCategoryId ]').val();

            var gstRules = $('[id$=hidGSTData]').val()
            var rules = eval('[' + gstRules + ']')[0]

            var gstRule = rules.filter(function (dt) {
                return dt.Id == gst
            })

            var gstPercentage = gstRule[0].Percentage

            mrp = mrp - ((parseFloat(mrp) * parseFloat(discount)) / 100)

            mrp = mrp * parseFloat(qty)

            var total = parseFloat(mrp) + ((parseFloat(mrp) * parseFloat(gstPercentage)) / 100);

            $('[id$=' + index + '_' + 'txtTPrice ]').val(total)

            SetGrandTotal();
        }

        function SetGrandTotal() {
            var totalAmount = 0
            $('[id$=txtTPrice]').each(function () {
                if ($(this).val() != '')
                    totalAmount += parseFloat($(this).val())
            })

            $('[id$=txtAmount]').val(totalAmount)
            SetFinalAmount();
        }

        function SetFinalAmount() {
            var amt = $('[id$=txtAmount]').val()
            var transportAmt = $('#' + '<%=this.txtTransportAmount.ClientID %>').val()
            var adjstAmt = $('#' + '<%=this.txtAdjustedAmount.ClientID %>').val()

            if (amt == '')
                amt = '0'

            if (transportAmt == '')
                transportAmt = '0'

            if (adjstAmt == '')
                adjstAmt = '0'

            var finalAmt = Math.round(parseFloat(amt) + parseFloat(transportAmt) + parseFloat(adjstAmt), 0)
            $('#' + '<%=this.txtPrice.ClientID %>').val(finalAmt)
        }

//        function ConfirmDelete() {
//            if (window.confirm('Are you sure you want to delete this record?'))
//                return true;
//            else
//                return false;
//        }

        function ValidateQuantity(src, arg) {
            var isIssueFound = false;
            $('[id$=_txtQuantity]').each(function () {
                var qty = $(this).val()
                if (qty == '' || qty == '0') {
                    isIssueFound = true;
                }
            })

            if (isIssueFound) {
                arg.IsValid = false;
                return true;
            }
            else {
                arg.IsValid = true;
                return false;
            }
        }

        function ValidateMRP(src, arg) {
            var isIssueFound = false;
            $('[id$=_txtMRP]').each(function () {
                var mrp = $(this).val()
                if (mrp == '' || mrp == '0') {
                    isIssueFound = true;
                }
            })

            if (isIssueFound) {
                arg.IsValid = false;
                return true;
            }
            else {
                arg.IsValid = true;
                return false;
            }
        }

        function ValidatePrice(src, args) {
            var price = $('#' + '<%=this.txtPrice.ClientID %>').val()
            if (price == '' || price == '0') {
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
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
