<%@ Page Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="ImportItemUI.aspx.cs" Inherits="ImportItemUI"
    Title="Untitled Page" %>

<asp:Content ID="CntImportItem" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 97%;">
                    <tr>
                        <td align="right">
                            <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valsumErrorMsg" runat="server" CssClass="ClsLabel" ValidationGroup="valGrpUFile"/>
                                        <asp:Label ID="lblUploadErrMsg" runat="server" 
                                            Visible="False" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                                    </td>
                                    <td>
                                        <div style="float: right; vertical-align: top">
                                        <span class="ClsMdtStar">* Mandatory Fields</span><br />
                                        <asp:HyperLink ID="hlnkDownloadTemplate" runat="server" CssClass="CursorHand" Target="_blank"
                                            ImageUrl="~/RITeSchool/images/DownloadTemplate.gif" ToolTip="Download the template for adding inventory item by template."></asp:HyperLink>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lblUploadMsg" runat="server" Text="Your file has been uploaded sucessfully."
                                            Visible="False" CssClass="LblNrmlB" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table align="center" border="0" cellpadding="0" cellspacing="3" width="100%">
                                <tr>
                                    <td align="center">
                                        <table border="0" cellpadding="0" cellspacing="3">
                                            <tr>
                                                <td align="left" colspan="1">
                                                    <asp:CustomValidator ID="cstvalFileType" runat="server" ClientValidationFunction="validateFile"
                                                        ControlToValidate="fileUploadItems" CssClass="ClsLabel" Display="None" ValidateEmptyText="true"
                                                        ErrorMessage="Invalid file type." ValidationGroup="valGrpUFile"></asp:CustomValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsOnlyBorderlght" colspan="1">
                                                   
                                                        <span class="ClsLabel">Select File :</span>
                                                </td>
                                                <td align="left" colspan="1">
                                                    <asp:FileUpload ID="fileUploadItems" runat="server" />
                                                    <span style="color: #ff0000; font-size: 9pt;">*&nbsp;</span>
                                                </td>
                                                <td align="center" colspan="1">
                                                  
                                                        <span class="LblSmlGray">(Supports only .XLS/.XLSX files type)</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsOnlyBorderlght" colspan="3">
                                                    <asp:CheckBox ID="chkSetAutoItemCode" runat="server" CssClass="ClsLabel" Text="Set Auto Item Code"
                                                        TabIndex="4" Style="padding-left : 0px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="width: 50%;">
                                        <asp:Button ID="btnImportItems" Text="Import Items" runat="server" CssClass="ClsBtnMid"
                                            BorderStyle="Solid" OnClick="btnImportItems_Click" Visible="True" CausesValidation="true"
                                            BorderWidth="1px" UseSubmitBehavior="false" ValidationGroup="valGrpUFile"/>
                                        <asp:Button ID="btnBack" Text="Back" runat="server" CssClass="ClsBtnSml" 
                                            BorderStyle="Solid" Visible="True" BorderWidth="1px" CausesValidation="false"
                                            UseSubmitBehavior="false" 
                                            PostBackUrl="~/RITeSchool/Inventory/ItemManagementUI.aspx" />
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
                                                                <asp:ListView ID="lstvwItemDetails" runat="server" DataKeyNames="ItemID" OnDataBound="lstvwItemDetails_DataBound"
                                                                    OnSorting="lstvwItemDetails_Sorting" DataSourceID="lstvwDSobj">
                                                                    <LayoutTemplate>
                                                                        <table width="70%" align="center" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                            cellspacing="1" class="GridBorder">
                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                <th id="thItemCode" runat="server" align="left" class="ClspaddingL" style="width:15%">
                                                                                    <asp:LinkButton ID="lnkItemCode" runat="server" CommandName="Sort" CommandArgument="ItemCode"
                                                                                        ForeColor="Black">
                                                                                            Item Code</asp:LinkButton>
                                                                                </th>
                                                                                <th id="thItemName" runat="server" align="left" class="ClspaddingL" style="width:30%">
                                                                                    <asp:LinkButton ID="lnkItemName" runat="server" CommandName="Sort" CommandArgument="ItemName"
                                                                                        ForeColor="Black">
                                                                                            Item Name</asp:LinkButton>
                                                                                </th>
                                                                                <th id="thStock" runat="server" align="left" class="ClspaddingL" style="width:15%">
                                                                                    <asp:Label ID="lblCurrentStock" runat="server" Text="Current Stock" style="font-weight:bold; color:Black;" />     
                                                                                </th>
                                                                                <th id="thCatagory" runat="server" align="left" class="ClspaddingL" style="width:13%">
                                                                                    <asp:LinkButton ID="lnkCatagory" runat="server" CommandName="Sort" CommandArgument="ItemCategoryName"
                                                                                        ForeColor="Black">
                                                                                            Item Catagory</asp:LinkButton>
                                                                                </th>
                                                                                <th id="thLevel" runat="server" align="right" class="ClspaddingR" style="width:10%">
                                                                                    <asp:LinkButton ID="lnkLevel" runat="server" CommandName="Sort" CommandArgument="ItemReorderLevelQty"
                                                                                        ForeColor="Black">
                                                                                            Reorder Level </asp:LinkButton>
                                                                                </th>
                                                                            </tr>
                                                                            <tr id="itemPlaceholder" runat="server">
                                                                            </tr>
                                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                                <td colspan="7">
                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwItemDetails">
                                                                                        <Fields>
                                                                                            <asp:TemplatePagerField>
                                                                                                <PagerTemplate>
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
                                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPageNos_SelectedIndexChanged">
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
                                                                            </td>
                                                                            <td align="left" id="tdItemName" class="ClspaddingL">
                                                                                <asp:Label ID="lblItemName" runat="server" Text='<%# Eval("ItemName")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" id="tdItemStock" class="ClspaddingL">
                                                                                <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" id="tdCategory" class="ClspaddingL">
                                                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("ItemCategoryName")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="right" id="tdRLevel" class="ClspaddingR">
                                                                                <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
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
                                                                            <td align="left" id="tdItemStock" class="ClspaddingL">
                                                                                <asp:Label ID="lblItemStock" runat="server" Text='<%# Eval("ItemQty")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="left" id="tdCategory" class="ClspaddingL">
                                                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("ItemCategoryName")%>'></asp:Label>
                                                                            </td>
                                                                            <td align="right" id="tdRLevel" class="ClspaddingR">
                                                                                <asp:Label ID="lblReorderLevel" runat="server" Text='<%# Eval("ItemReorderLevelQty")%>'></asp:Label>
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
                                                                </SelectParameters>
                                                            </asp:ObjectDataSource>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:HiddenField ID="hidSortDirection" runat="server" />
                                                            <asp:HiddenField ID="hidSortExpression" runat="server" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="Sorting" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="ItemCommand" />
                                                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="DataBound" />
                                                <asp:PostBackTrigger ControlID="btnImportItems" />
                                                <asp:PostBackTrigger ControlID="btnBack" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="Sorting" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="ItemCommand" />
                <asp:AsyncPostBackTrigger ControlID="lstvwItemDetails" EventName="DataBound" />
                <asp:PostBackTrigger ControlID="btnImportItems" />
                <asp:PostBackTrigger ControlID="btnBack" />
            </Triggers>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript" language="javascript">

        _clientFileUploadClientId = "<%=this.fileUploadItems.ClientID%>"
        _clientCstvalFileTypeId = "<%=this.cstvalFileType.ClientID%>"
        _clientbtnImportItems = "<%=this.btnImportItems.ClientID%>"
        _clientBtnBack = "<%=this.btnBack.ClientID%>"
        _clientlblUploadMsg = "<%=this.lblUploadMsg.ClientID%>"
        _clientlblUploadErrMsg = "<%=this.lblUploadErrMsg.ClientID%>"
        function ClearLabel() {
            if (document.getElementById(_clientlblUploadMsg)) {
                document.getElementById(_clientlblUploadMsg).innerText = ""
                document.getElementById(_clientlblUploadMsg).innerHTML = ""
            }
            if (document.getElementById(_clientlblUploadErrMsg)) {
                document.getElementById(_clientlblUploadErrMsg).innerText = ""
                document.getElementById(_clientlblUploadErrMsg).innerHTML = ""
            } 
        }
        function validateFile(source, args) {
            ClearLabel()
            var oFileName = document.getElementById(_clientFileUploadClientId).value
            var oCusVal = document.getElementById(_clientCstvalFileTypeId)
            var bIsValid = true
            if (oFileName != "") {
                var sFileExtension = oFileName.substring(oFileName.indexOf('.'))
                sFileExtension = sFileExtension.toUpperCase()
                if (sFileExtension != ".XLS" && sFileExtension != ".XLSX") {
                    bIsValid = false
                    oCusVal.errormessage = "File to upload should be in valid format."
                } 
            }
            else {
                bIsValid = false
                oCusVal.errormessage = "File to upload should be selected."
            }
            args.IsValid = bIsValid
            return !bIsValid
        }
        function DisableButtons(ObjBtn) {
            if (ObjBtn == document.getElementById(_clientbtnImportItems)) {
                var isPageValid = true
                if (typeof (Page_ClientValidate) == 'function') {
                    isPageValid = Page_ClientValidate()
                }
                if (isPageValid) {
                    document.getElementById(_clientbtnImportItems).disabled = true
                    document.getElementById(_clientBtnBack).disabled = true
                } 
            }
            else if (ObjBtn == document.getElementById(_clientBtnBack)) {
                document.getElementById(_clientbtnImportItems).disabled = true
                document.getElementById(_clientBtnBack).disabled = true
            } 
        }
    </script>
</asp:Content>
