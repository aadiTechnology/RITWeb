<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StockDetailsPopup.aspx.cs" Inherits="StockDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table width="100%" align="center">
        <tr>
            <td align="left">
                <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                    <tr>
                        <td style="height: 20px">
                            <asp:Label ID="lblAddStockDetails" runat="server" class="MainTitleHead" Text="Add Stock Details"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="right" style="color: #ff3333" valign="middle">
                <span class="ClsMdtStar">*</span>
                <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UPanelItemSearch" runat="server" ChildrenAsTriggers="false"
                    UpdateMode="Conditional">
                    <ContentTemplate>
                        <table align="center">
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>"
                                        runat="server" CssClass="ClsMdtStar" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center" id="tdMessage" runat="server" colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                                        Font-Bold="true" ForeColor="Blue" Style="text-align: center"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="100%" align="center">
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight" style="width: 120px">
                                                <asp:Label ID="lblItemName" runat="server" class="ClsLabel" Style="height: 16px;"
                                                    Text="Item Name"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" style="width: 120px" class="ClsHilightBGB ">
                                                <asp:Label ID="lblItemOriginalName" runat="server" class="ClsLabel" Style="height: 16px;"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblItemCode" runat="server" class="ClsLabel" Style="height: 16px;
                                                    margin-left: 0px;" Text="Item Code"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left" class="ClsHilightBGB">
                                                <asp:Label ID="lblItemOriginalCode" runat="server" class="ClsLabel" Style="height: 16px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblCurrentStock" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="Current Quantity"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left" class="ClsHilightBGB">
                                                <asp:Label ID="lblCurrentOriginalStock" runat="server" class="ClsLabel" Style="height: 16px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="LblQuantity" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="New Quantity"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtQuantity" runat="server" CssClass="LrgTxtBox" MaxLength="6" type="txtQuantity"
                                                    onblur="extractNumber(this,3,true);" onkeyup="extractNumber(this,3,true);" onkeypress="return blockNonNumbers (this, event, true, true);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false" TabIndex="1"
                                                    Width="130px"></asp:TextBox>
                                                    <asp:DropDownList ID="cmbUnits" runat="server" TabIndex="2">
                                                        </asp:DropDownList> &nbsp; <span class="ClsMdtStar">*</span>&nbsp;
                                                <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" ControlToValidate="txtQuantity"
                                                    ErrorMessage="Item Quantity should not be blank." SetFocusOnError="True"
                                                    Display="None"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="rvQuantity" ControlToValidate="txtQuantity" Display="None"
                                                   MinimumValue="1" MaximumValue="999999" Type="Double" ErrorMessage="Item Quantity should not be zero."
                                                    runat="server">
                                                </asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblItemPrice" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="Unit Price"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtItemPrice" runat="server" CssClass="LrgTxtBox" MaxLength="8"
                                                    TabIndex="3" Width="130" type="txtIssueQuantity" onblur="extractNumber(this,2,true);"
                                                    onkeyup="extractNumber(this,2,true);" onkeypress="return blockNonNumbers (this, event, true, false);"
                                                    onpaste="event.returnValue=false" ondrop="event.returnValue=false"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblDate" runat="server" class="ClsLabel" Style="height: 16px" Text="Date"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtDate" CssClass="SmlCombo" runat="server" ReadOnly="true" AutoPostBack="True"
                                                    Width="130px" TabIndex="4"></asp:TextBox>
                                                <rjs:PopCalendar ID="calEndDate" runat="server" Control="txtDate" ShowErrorMessage="false"
                                                    InvalidDateMessage="Please select valid end date." Format="dd MMM yyyy" ShowWeekend="True" />
                                                &nbsp; <span class="ClsMdtStar">*</span>&nbsp;
                                                <asp:RequiredFieldValidator ID="rfvDate" runat="server" ControlToValidate="txtDate"
                                                    ErrorMessage="Date should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblDescription" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="Description"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtDescription" CssClass="SmlCombo" runat="server" TextMode="MultiLine"
                                                    Width="430px" TabIndex="5" MaxLength="280" OnKeyPress="javascript:Check(this, 280);"
                                                    OnKeyUp="javascript:Check(this, 280);"></asp:TextBox>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateDescription"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblInvoiceNo" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="Invoice No."></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                                <asp:TextBox ID="txtInvoiceNo" CssClass="SmlCombo" runat="server"  AutoPostBack="True"
                                                    Width="130px" TabIndex="6"></asp:TextBox>
                                                    </td>
                                            </tr>
                                            <tr>
                                            <td valign="middle" class="ClsBorderlight">
                                                <asp:Label ID="lblVendorName" runat="server" class="ClsLabel" Style="height: 16px"
                                                    Text="Vendor Name"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td valign="middle" align="left">
                                             <asp:DropDownList ID="cmbVendor" runat="server" TabIndex="7" Width="300px">
                                                        </asp:DropDownList> 
                                            </td>
                                            </tr>
                                        <tr>
                                            <td align="center" valign="middle" colspan="2">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" TabIndex="8"
                                                    disable-page="true" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" TabIndex="9"
                                                    CausesValidation="False" UseSubmitBehavior="false" 
                                                    OnClick="btnCancel_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwStockDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwStockDetails" EventName="DataBound" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
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
                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStockDetails"
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
                                    <asp:ListView ID="lstvwStockDetails" runat="server" DataKeyNames="Id" OnItemCommand="lstvwStockDetails_ItemCommand"
                                        OnDataBound="lstvwStockDetails_DataBound" OnItemDataBound="lstvwStockDetails_ItemDataBound">
                                        <LayoutTemplate>
                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="80%" style="color: #333333"
                                                align="center">
                                                <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="center" class="paddingL" style="width: 100px; font-size: 9pt;">
                                                        <asp:LinkButton ID="lnkbtnDate" runat="server" CausesValidation="false" ForeColor="Black"
                                                            CommandArgument="NewStockDate" CommandName="SortRow">Date</asp:LinkButton>
                                                    </th>
                                                    <th align="left" class="paddingL" style="font-size: 9pt;">
                                                        <asp:Label ID="lblItemQuantity" runat="server" Text="New Quantity"></asp:Label>
                                                    </th>
                                                    <th align="right" class="paddingLR" runat="server" style="width: 100px; font-size: 9pt;">
                                                        <asp:Label ID="lblItemPrice" runat="server" Text="Price of a Unit"></asp:Label>
                                                    </th>
                                                    <th align="center" width="50px">
                                                        <asp:Label ID="lblEdit" runat="server" Text="Edit"></asp:Label>
                                                    </th>
                                                    <th align="center" width="50px">
                                                        <asp:Label ID="lblDelete" runat="server" Text="Delete"></asp:Label>
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                                <tr class="ClsBorderPager" id="trDataPager" runat="server">
                                                    <td colspan="5">
                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStockDetails">
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
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("ItemQuantityWithUnits") %>' />
                                                </td>
                                                <td align="right" class="paddingLR">
                                                    <asp:Label ID="lblprice" runat="server" Text='<%# Eval("price") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <AlternatingItemTemplate>
                                            <tr id="trItem" runat="server" class="ClsGridAltRow">
                                                <td align="center">
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Eval("Date") %>' />
                                                </td>
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("ItemQuantityWithUnits") %>' />
                                                </td>
                                                <td align="right" class="paddingLR">
                                                    <asp:Label ID="lblprice" runat="server" Text='<%# Eval("price") %>' />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Edit%>" ImageUrl="../images/IconGrid_Edit.GIF" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
                                                        ToolTip="<%$ Resources:LocalizedResources, Delete%>" ImageUrl="../images/IconGrid_Delete.gif" />
                                                </td>
                                            </tr>
                                        </AlternatingItemTemplate>
                                        <EmptyDataTemplate>
                                            <table width="70%" align="center">
                                                <tr>
                                                    <td class="LblNoRecord" align="center">
                                                        No record found.
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:ListView>
                                    <asp:HiddenField ID="hidId" runat="server" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidItemId" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:ObjectDataSource TypeName="BusinessLogic.StockDetailsBL" EnablePaging="true"
                                        ID="lstvwDSobj" runat="server" SelectMethod="GetAll" SelectCountMethod="Count"
                                        EnableCaching="false">
                                        <SelectParameters>
                                            <asp:ControlParameter Name="aiItemId" ControlID="hidItemId" PropertyName="Value" />
                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                            <asp:ControlParameter Name="asSortExpression" ControlID="hidSortExpression" PropertyName="Value" />
                                            <asp:ControlParameter Name="asSortDirection" ControlID="hidSortDirection" PropertyName="Value" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lstvwStockDetails" EventName="ItemCommand" />
                        <asp:AsyncPostBackTrigger ControlID="lstvwStockDetails" EventName="DataBound" />
                        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr align="center">
            <td align="center" colspan="3">
                <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false"
                    OnClick="btnClose_Click" TabIndex="8" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _clientlblMessage = "<%=this.lblMessage.ClientID %>";
        _clienttxtDescription = "<%=this.txtDescription.ClientID %>"

        function Check(textBox, maxLength) {
            if (textBox.value.length > maxLength) {
                alert("Maximum characters allowed are " + maxLength);
                textBox.value = textBox.value.substr(0, maxLength);
            }
        }

        function ConfirmRemove() {
            var bResult = true
            if (!window.confirm('Are you sure you want to delete this record?')) {
                bResult = false
            }
            return bResult
        }

        function ClearMessage() {
            $get(_clientlblMessage).innerHTML = "";
        }

        function ValidateDescription(oSrc, args) {
            var desc = $('#' + _clienttxtDescription).val().trim()
            
            if (desc.length > 300) {
                oSrc.errormessage = "Description length should not be greater than 300 character(s).";
                args.IsValid = false
                return true;
            }

            args.IsValid = true
            return false
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
</asp:Content>
