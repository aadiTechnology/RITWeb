<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StoreItemVariationDetailsUI.aspx.cs" Inherits="StoreItemVariationDetailsUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%" align="center">
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Color should be selected."
                                Display="None" InitialValue="0" ControlToValidate="cmbColor"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Size should be selected."
                                Display="None" InitialValue="0" ControlToValidate="cmbSize"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Title should not blank."
                                Display="None" ControlToValidate="txtTitle"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Item Code should not be blank."
                                Display="None" ControlToValidate="txtItemCode"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="custValItemCode" runat="server" ErrorMessage="Item Code should not be duplicate." ClientValidationFunction="ValidateItemCode" Display="None" OnServerValidate="Validate_Duplication"></asp:CustomValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="UOM should be selected."
                                Display="None" ControlToValidate="cmbUOM" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="MRP should not be blank."
                                Display="None" ControlToValidate="txtMRP"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="GST should be selected."
                                Display="None" ControlToValidate="cmbGST" InitialValue="0"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Sale Rate should not blank."
                                Display="None" ControlToValidate="txtPrice"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Quantity should not blank."
                                Display="None" ControlToValidate="txtQuantity"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Reorder Quantity should not blank."
                                Display="None" ControlToValidate="txtReorderQuantity"></asp:RequiredFieldValidator>
                            <asp:CustomValidator ID="custValColorSize" runat="server" ErrorMessage="Color-Size combination should not be duplicate."
                                Display="None" ClientValidationFunction="ValidateColorAndSize" OnServerValidate="Validate_Duplication"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None"
                                ClientValidationFunction="ValidateTitle" OnServerValidate="Validate_Duplication"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustFileUpload" runat="server" ClientValidationFunction="ValidateFileType"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="CustFileSize" runat="server" ClientValidationFunction="ValidateFileSize"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <div style="float: right;">
                        <span class="ClsMdtStar">*</span>
                        <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="Mandatory Fields"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl2" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Label ID="lblMessage" runat="server" Text="" EnableViewState="false" Font-Bold="true"
                                            ForeColor="Blue"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight" style="width: 150px">
                                        <span class="clsLabel">Store Item Category : </span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblStoreItemCategory" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">For : </span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblGender" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Item Name : </span>
                                    </td>
                                    <td align="left" class="ClsHilightBGB">
                                        <asp:Label ID="lblItemName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Color : </span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbColor" runat="server" CssClass="MidCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Size : </span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbSize" runat="server" CssClass="SmlCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <span><u><a href='#' onclick="CopyFields()">Copy item details from base</a></u></span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Title : </span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtTitle" runat="server" CssClass="ExLrgTxtBox" Width="300px" MaxLength="100"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Item Code :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtItemCode" runat="server" CssClass="MidTxtBox" ViewStateMode="Enabled" MaxLength="10"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                        <asp:Image id="img" runat="server" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">UOM :</span>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbUOM" runat="server" CssClass="SmlCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">MRP :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtMRP" runat="server" CssClass="SmlTxtBox" MaxLength="8" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" style="text-align:right;padding-right:5px;"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Discount in % :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDiscount" runat="server" CssClass="SmlTxtBox" MaxLength="6" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" style="text-align:right;padding-right:5px;"></asp:TextBox>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="clsBorderLight">
                                        <span class="ClsLabel">GST :</span>
                                    </td>
                                    <td align="left">                                        
                                        <asp:DropDownList ID="cmbGST" runat="server" CssClass="SmlCombo">
                                        </asp:DropDownList>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Sale Rate : </span>
                                    </td>
                                    <td align="left">                                        
                                        <asp:TextBox ID="txtPrice" runat="server" CssClass="SmlTxtBox" MaxLength="10" style="text-align:right;padding-right:5px;" onkeydown="return false" onpaste="return false"/>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>                                
                                <tr>
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">HSN Code :</span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtHSNCode" runat="server" CssClass="SmlTxtBox" MaxLength="50" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Available Quantity : </span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtQuantity" runat="server" CssClass="SmlTxtBox" MaxLength="4" Style="text-align: right;
                                            padding-right: 5px" onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                            onkeypress="return blockNonNumbers(this, event, false, false);" onkeyup="extractNumber(this,2,false);"
                                            onpaste="event.returnValue=false"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Reorder Quantity : </span>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtReorderQuantity" runat="server" CssClass="SmlTxtBox" MaxLength="4"
                                            Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,2,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                            onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBorderLight">
                                        <span class="clsLabel">Images : </span>
                                    </td>
                                    <td align="left">
                                        <asp:FileUpload ID="flImage" runat="server" accept=".JPG, .JPEG, .BMP, .PNG" multiple="true" />
                                        <span class="ClsMdtStar">*</span>
                                    </td>
                                </tr>
                                <tr id="trAttachments" runat="server" visible="false">
                                    <td class="clsBorderLight" align="left">
                                        <span class="ClsLabel">Attachments :</span>
                                    </td>
                                    <td>
                                        <asp:Panel ID="AttachmentPanel" runat="server" Style="height: auto">
                                            <table id="tblAttachments" runat="server">
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="2">
                                        <span class="LblSmlGray">(Attachment supports files of types - .JPG, .JPEG, .BMP, .PNG). Total file size should not exceed 10 MB.</span>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
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
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                OnClick="btnCanel_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table>
                        <tr style="height:10px;">
                        </tr>
                        <tr>
                            <td align="left" class="clsBorderLight">
                                <span class="clsLabel">Title / Color / Size : </span>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="ExLrgTxtBox"></asp:TextBox>
                            </td>
                            <td align="left">
                                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ClsBtn" OnClick="btnSearch_Click"
                                    CausesValidation="False" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="95%">
                                <tr>
                                    <td align="center">
                                        <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwVariationDetails"
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
                                        <asp:ListView ID="lstvwVariationDetails" runat="server" OnDataBound="lstvwVariationDetails_DataBound"
                                            OnSorting="lstvwVariationDetails_Sorting" DataKeyNames="Id,Title,ItemCode" OnItemCommand="lstvwVariationDetails_ItemCommand"
                                            OnItemDataBound="lstvwVariationDetails_ItemDataBound">
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
                                                        <th align="left" style="width: 100px">
                                                            <asp:LinkButton ID="LinkButton5" runat="server" CssClass="clsLabel" CommandName="Sort"
                                                                CommandArgument="ReorderQuantity" CausesValidation="false" ForeColor="Black">Reorder Qty.</asp:LinkButton>
                                                        </th>
                                                        <th style="width: 75px" align="center">
                                                            <span class="clsLabel" style="float: inherit; color: Black;">Edit</span>
                                                        </th>
                                                        <th style="width: 75px" align="center">
                                                            <span class="clsLabel" style="float: inherit; color: Black;">Delete</span>
                                                        </th>  
                                                        <th align="center" style="width: 100px">
                                                            <span class="clsLabel" style="float: inherit; color: Black;">Free Item</span>
                                                        </th>                                                      
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                    <tr id="trDataPager" runat="server" class="ClsBorderPager">
                                                        <td colspan="9">
                                                            <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwVariationDetails"
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
                                                    <td align="left">
                                                        <asp:Label ID="Label11" runat="server" Text='<%#Eval("ReorderQuantity") %>' CssClass="clsLabel"></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Edit%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:ImageButton ID="btnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                            CommandArgument="<%# Container.DataItemIndex %>" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                            ToolTip="<%$ Resources:LocalizedResources, Delete%>" />
                                                    </td>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lnkbtnFreeItem" runat="server" CausesValidation="false" CssClass="SMSLblSMlBlue" Text="Add/Edit" CommandName="ADDFREEITEM" />
                                                        <asp:HiddenField ID="hidQueryString" runat="server" Value="" />
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
                                                <asp:ControlParameter ControlID="hidSortExpression" Name="SortExpression" Type="String"
                                                    PropertyName="Value" />
                                                <asp:ControlParameter ControlID="hidSortDirection" Name="SortDirection" Type="String"
                                                    PropertyName="Value" />
                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                        <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidStoreItemMasterId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidSortDirection" runat="server" />
                                        <asp:HiddenField ID="hidSortExpression" runat="server" />
                                        <asp:HiddenField ID="hidDeleteedIds" runat="server" Value="" />
                                        <asp:HiddenField ID="hidAttachmentCount" runat="server" Value="0" />
                                        <asp:HiddenField ID="hidBaseTitle" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBasePrice" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseQuantity" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseReordQty" runat="server" Value="" />
                                        <asp:HiddenField ID="hidStoreCategoryName" runat="server" />
                                        <asp:HiddenField ID="hidBaseItemCode" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseUOMId" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseMRP" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseDiscount" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseGSTId" runat="server" Value="" />
                                        <asp:HiddenField ID="hidBaseHSNCode" runat="server" Value="" />
                                        <asp:HiddenField ID="hidGSTData" runat="server" Value="" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="lstvwVariationDetails" EventName="ItemCommand" />
                            <asp:PostBackTrigger ControlID="btnSave" />
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="ClsBtn" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">

        var _clienttxtTitle = "<%=this.txtTitle.ClientID %>"
        var _clienthidId = "<%=this.hidId.ClientID %>"
        var _clientcmbColor = "<%=this.cmbColor.ClientID %>"
        var _clientcmbSize = "<%=this.cmbSize.ClientID %>"
        var _clienthidAttachmentCount = "<%=this.hidAttachmentCount.ClientID %>"
        var _clienttxtItemCode = "<%=this.txtItemCode.ClientID %>"

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }

        function ValidateTitle(src, args) {
            var title = $('#' + _clienttxtTitle).val()
            var isFound = false;
            $('[id$=lblTitle]').each(function () {

                if ($(this).html() == title) {

                    var newId = $('#' + this.id.replace('_lblTitle', '_hidIdNew')).val()

                    if ($('#' + _clienthidId).val() != newId) {
                        isFound = true;
                    }
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

        function ValidateColorAndSize(src, args) {
            var newColor = $("#" + _clientcmbColor + " option:selected").text();
            var newSize = $("#" + _clientcmbSize + " option:selected").text();

            var isFound = false;
            $('[id$=lblColor]').each(function () {
                var newId = $('#' + this.id.replace('_lblColor', '_hidIdNew')).val()
                var size = $('#' + this.id.replace('_lblColor', '_lblSize')).html()

                if ($(this).html() == newColor && newSize == size && $('#' + _clienthidId).val() != newId) {
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

        function ResetMessage() {
            $('#' + '<%=this.lblMessage.ClientID %>').html('')
        }

        function ValidateFileType(oSrc, args) {
            var isFound = false
            var files = $('[id$=flImage]')[0].value;
            var attachentCnt = $('#' + _clienthidAttachmentCount).val();

            if (files.trim() == '' && attachentCnt == 0) {
                oSrc.errormessage = "Please select image(s) to upload.";
                args.IsValid = false;
                return true;
            }
            else if (files.trim() != '') {
                var fileList = files.split(',')
                for (var k = 0; k < fileList.length; k++) {
                    var file = fileList[k].trim()

                    var extension = file.substr(file.lastIndexOf('.')).toUpperCase()
                    if (extension != ".BMP" && extension != ".JPG" && extension != ".JPEG" && extension != ".PNG") {
                        isFound = true
                        break;
                    }
                }
            }

            if (isFound) {
                oSrc.errormessage = "Image type should be in only BMP, .JPG, .JPEG and .PNG format.";
                args.IsValid = false;
                return true;
            }


            args.IsValid = true;
            return false;
        }

        function ValidateFileSize(oSrc, args) {
            var obj = document.getElementById('<%=flImage.ClientID %>')
            var fileSize = GetFileSize(obj)

            if (fileSize >= 10485760) {
                oSrc.errormessage = "Image's total file size should be less than 10 MB."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

        function GetFileSize(obj) {
            var size = 0;
            for (var k = 0; k < obj.files.length; k++) {
                size += obj.files[k].size;
            }
            return size;
        }

        function HideAttachment(index) {
            $('[id$=hyper_' + index + ']').hide();
            $('[id$=img_' + index + ']').hide();

            var sData = $('[id$=hidDeleteedIds]').val()
            if (sData == '')
                $('[id$=hidDeleteedIds]').val(index)
            else
                $('[id$=hidDeleteedIds]').val(sData + ',' + index)

            var cnt = $('#' + _clienthidAttachmentCount).val();
            cnt = cnt - 1;
            $('#' + _clienthidAttachmentCount).val(cnt)
        }

        function CopyFields() {
            var color = ''
            if ($('#' + _clientcmbColor).val() != 0)
                color = '-'+ $('#' + _clientcmbColor + ' option:selected').text();

            var size = ''
            if ($('#' + _clientcmbSize).val() != 0)
                size = '-' + $('#' + _clientcmbSize + ' option:selected').text();

            $('#' + _clienttxtTitle).val($('#' + '<%=this.hidBaseTitle.ClientID %>').val() + color + size)
            $('#' + '<%=this.txtPrice.ClientID %>').val($('#' + '<%=this.hidBasePrice.ClientID %>').val())
            $('#' + '<%=this.txtQuantity.ClientID %>').val($('#' + '<%=this.hidBaseQuantity.ClientID %>').val())
            $('#' + '<%=this.txtReorderQuantity.ClientID %>').val($('#' + '<%=this.hidBaseReordQty.ClientID %>').val())

            $('#' + '<%=this.txtItemCode.ClientID %>').val($('#' + '<%=this.hidBaseItemCode.ClientID %>').val())
            $('#' + '<%=this.cmbUOM.ClientID %>').val($('#' + '<%=this.hidBaseUOMId.ClientID %>').val())
            $('#' + '<%=this.txtMRP.ClientID %>').val($('#' + '<%=this.hidBaseMRP.ClientID %>').val())
            $('#' + '<%=this.txtDiscount.ClientID %>').val($('#' + '<%=this.hidBaseDiscount.ClientID %>').val())
            $('#' + '<%=this.cmbGST.ClientID %>').val($('#' + '<%=this.hidBaseGSTId.ClientID %>').val())
            $('#' + '<%=this.txtHSNCode.ClientID %>').val($('#' + '<%=this.hidBaseHSNCode.ClientID %>').val())
        }

        function OpenPopup(querystring) {
            window.open('../eStore/StoreItemStockDetailsPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700').focus();
            return false;
        }

        function SetAmount() {
            var amt = $('#' + '<%=this.txtMRP.ClientID %>').val()
            var discount = $('#' + '<%=this.txtDiscount.ClientID %>').val()
            var gst = $('#' + '<%=this.cmbGST.ClientID %>').val()
            var gstPercentage = 0;

            if (gst != 0) {
                var gstRules = $('[id$=hidGSTData]').val()
                var rules = eval('[' + gstRules + ']')[0]

                var gstRule = rules.filter(function (dt) {
                    return dt.Id == gst
                })
                gstPercentage = gstRule[0].Percentage
            }

            if (amt == '')
                amt = '0';

            var price = 0;

            if (discount == '')
                discount = 0;

            if (gst == '' || gst == '0')
                gst = 0;

            price = (parseFloat(amt) * parseFloat(discount)) / 100
            amt = amt - price
            amt = amt + Math.round((amt * gstPercentage) / 100, 0)

            $('#' + '<%=this.txtPrice.ClientID %>').val(amt);
        }

        function ValidateItemCode(src, arg) {
            var itemCode = $('#' + _clienttxtItemCode).val()
            var isFound = false;
            $('[id$=lblItemCode]').each(function () {

                if ($(this).html() == itemCode) {

                    var newId = $('#' + this.id.replace('_lblItemCode', '_hidIdNew')).val()

                    if ($('#' + _clienthidId).val() != newId) {
                        isFound = true;
                    }
                }
            })
            
            if (isFound) {
                arg.IsValid = false;
                return true;
            }
            else {
                arg.IsValid = true;
                return false;
            }
        }

        function OpenFeeItemPopup(str) {
            var ss = $('[id$=' + str + '_hidQueryString]').val()
            window.open('FreeItemDetailsPopup.aspx?' + ss,'_new','scrollbars=yes,resizable=no,top=0,left=0,width=800,height=800')
        }

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
