<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="GSTInvoiceDetailsUI.aspx.cs" Inherits="GSTInvoiceDetailsUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="98%">
        <tr>
            <td align="right">
                <div style="float: right;" class="LblErrorMsg" id="lblMandatoryMark" runat="server"
                    viewstatemode="Enabled">
                    <span class="ClsMdtStar">*</span>
                    <asp:Label ID="lblMandatoryFields" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel LblErrorMsg" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="ClsGreenBG" width="150px" style="float: right; padding-right: 5px;">
                                <asp:LinkButton ID="lnkReceiverDetails" runat="server" Text="Service Receiver Configuation"
                                    CssClass="SubTitle" Style="text-align: left;" CausesValidation="false"></asp:LinkButton>
                            </div>
                            <div class="ClsGreenBG" width="150px" style="float: right; padding-right: 5px;">
                                <asp:LinkButton ID="lnkBankDetails" runat="server" Text="Deposite Bank Details"
                                    CssClass="SubTitle" Style="text-align: left;" CausesValidation="false"></asp:LinkButton>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="LblUpdateSuccess" runat="server" ForeColor="Blue" Width="100%" EnableViewState="false"
                                        CssClass="ClsLabel" Font-Bold="true"></asp:Label>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="tr1" runat="server">
            <td align="center">
                <table id="tblGSTDetails" width="80%" runat="server" style="padding-bottom: 15px !important">
                    <tr align="center">
                        <td align="center">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="Table1" runat="server" width="98%">
                                        <tr>
                                            <td class="txtNormal" colspan="4">
                                                <asp:RequiredFieldValidator ID="RequiredReceiverName" runat="server" ErrorMessage="Receiver Name Should not be blank"
                                                    Display="None" ControlToValidate="ddlReceiverName" InitialValue="0"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredInvoiceNo" runat="server" ErrorMessage="Invoice No Should not be blank"
                                                    Display="None" ControlToValidate="txtInvoiceNo"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredInvoiceDate" runat="server" ErrorMessage="Invoice Date Should not be blank"
                                                    Display="None" ControlToValidate="txtInvoiceDate"></asp:RequiredFieldValidator>
                                                <asp:RequiredFieldValidator ID="RequiredGSTCategoryId" runat="server" ErrorMessage="GSTCategoryId should be selected"
                                                    Display="None" ControlToValidate="ddlGSTCategory" InitialValue="0"></asp:RequiredFieldValidator>
                                                <asp:CustomValidator ID="CustomDescription" runat="server" ErrorMessage="Value should be set for at least one Description."
                                                    CssClass="ClsMdtStar" ClientValidationFunction="ValidateDescription" Display="None"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage=""
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateDescriptionLength"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomAmount" runat="server" ErrorMessage="Amount should not be blank or zero for enterd Description."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmount"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Description should not be blank for enterd Amount."
                                                    Display="None" CssClass="ClsMdtStar" ClientValidationFunction="ValidateAmountDescription"></asp:CustomValidator>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Invoice No. should not be duplicate."
                                                    Display="None" CssClass="ClsMdtStar" OnServerValidate="Validate_InvoiceNo"></asp:CustomValidator>                                               
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtAdditionalRemark" ErrorMessage="Length of remarks should not exceed 500 characters." CssClass="ClsMdtStar"
                                                    ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr align="center" style="text-align: center; margin: 0px auto;">
                                            <td align="center" style="text-align: center;">
                                                <table align="center">
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">Receiver Name :</span>
                                                        </td>
                                                        <td style="width: 50%;" align="left">
                                                            <asp:DropDownList ID="ddlReceiverName" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">Invoice No :</span>
                                                        </td>
                                                        <td class="TxtNormal" align="left">
                                                            <asp:TextBox ID="txtInvoiceNoPrefix" CssClass="MidTxtBox" runat="server" MaxLength="20"
                                                                Width="100px" ReadOnly="true">
                                                            </asp:TextBox>
                                                            <asp:TextBox ID="txtInvoiceNo" CssClass="SmlTxtBox" runat="server" MaxLength="10"
                                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                            </asp:TextBox>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">Invoice Date :</span>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtInvoiceDate" CssClass="MidTxtBox" runat="server">
                                                            </asp:TextBox>
                                                            <rjs:PopCalendar ID="cInvoiceDate" runat="server" Control="txtInvoiceDate" Format="dd MMM yyyy"
                                                                ShowWeekend="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid invoice date." />
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="ClsBorderLight" align="left">
                                                            <span class="ClsLabel">GST Category :</span>
                                                        </td>
                                                        <td style="width: 25%;" align="left">
                                                            <asp:DropDownList ID="ddlGSTCategory" runat="server" CssClass="MidCombo" ViewStateMode="Enabled">
                                                            </asp:DropDownList>
                                                            <span class="ClsMdtStar">*</span>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr id="Tr2" runat="server">
                                            <td colspan="4" align="center">
                                                <asp:ListView ID="lstvwGSTDetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwGSTDetails_ItemDataBound">
                                                    <LayoutTemplate>
                                                        <table id="tblGSTDetails" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                            class="GridBorder" width="50%">
                                                            <tr id="TrHeader" runat="server" class="ClsGridHeader">
                                                                <th align="center" width="80%">
                                                                    <asp:Label ID="Label1" runat="server" Text="Description"></asp:Label>
                                                                </th>
                                                                <th align="center" width="20%">
                                                                    <asp:Label ID="Label2" runat="server" Text="Amount"></asp:Label>
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trItemtemplates" runat="server" class="ClsGridRow">
                                                            <td align="center">
                                                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="300" Width="98%" TextMode="MultiLine" Text='<%# Eval("Description") %>'>
                                                                </asp:TextBox>
                                                            </td>
                                                            <td align="center">
                                                                <asp:TextBox ID="txtAmount" runat="server" MaxLength="10" Width="98%" style="text-align:right;padding-right:5px" onblur="extractNumber(this,2,false);"
                                                                    ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                    onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                                </asp:TextBox>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <table width="50%">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label8" runat="server" Style="padding-right: 5px;" CssClass="lblNormal1"
                                                                Text="Total"></asp:Label>
                                                        </td>
                                                        <td align="right" width="20%">
                                                            <asp:TextBox ID="txtTotal" runat="server" Width="98%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label5" runat="server" Style="padding-right: 5px;" CssClass="lblNormal1"
                                                                Text="CGST"></asp:Label>
                                                        </td>
                                                        <td align="center" width="20%">
                                                            <asp:LinkButton ID="lnkPlus" runat="server" Text="<<" style="font-weight:bold;" OnClientClick="Decrease(); return false;"></asp:LinkButton>
                                                            <asp:TextBox ID="txtCGST" runat="server" Width="60%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                            <asp:LinkButton ID="LinkButton2" runat="server" style="font-weight:bold;" OnClientClick="Increase(); return false;">>></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="Label7" runat="server" Style="padding-right: 5px;" CssClass="lblNormal1"
                                                                Text="SGST"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtSGST" runat="server" Width="98%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblTotalAmount" runat="server" Style="padding-right: 5px;" CssClass="lblNormal1"
                                                                Text="Total Amount"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtTotalAmount" runat="server" Width="98%" Text="" onblur="extractNumber(this,2,false);"
                                                                disabled="true" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, true, false);"
                                                                onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" colspan="2">
                                                           <span class="ClsLabel">Additional Remark :</span>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                          <asp:TextBox ID="txtAdditionalRemark" runat="server" Height="75px" Width="100%" TextMode="MultiLine"></asp:TextBox>
                                                       </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4" align="center">
                                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save %>"
                                                    class="ClsBtn" OnClick="btnSave_Click" />
                                                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Cancel %>"
                                                    class="ClsBtn" OnClick="btnCancel_Click" CausesValidation="False" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="hidData" EventName="ValueChanged" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <hr style="border: 1px solid gray; width: 90%; height: 1px; color: Gray;" />
            </td>
        </tr>
        <tr>
            <td colspan="4" align="center">
                <table>
                    <tr>
                        <td class="clsBorderLight">
                            <span class="ClsLabel">Receiver Name / Invoice No. : </span>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" MaxLength="100" Width="300px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>"
                                class="ClsBtn" OnClick="btnSearch_Click" CausesValidation="False" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trLink" runat="server">
            <td>
                <table id="tblLstvwReceiverDetails" align="center" width="90%" runat="server">
                    <tr>
                        <td align="center" class="width-99-percentage">
                            <asp:UpdatePanel ID="upnlLstvwReceiverDetails" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table align="center" width="100%">
                                        <tr id="trDtPgCount" runat="server" visible="true">
                                            <td align="center">
                                                <asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwReceiverDetails"
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
                                        <tr id="trPager" runat="server" width="100%">
                                            <td align="center">
                                                <asp:ListView ID="lstvwReceiverDetails" runat="server" DataKeyNames="Id" ViewStateMode="Enabled"
                                                    OnItemCommand="lstvwReceiverDetails_ItemCommand" OnItemDataBound="lstvwReceiverDetails_ItemDataBound"
                                                    OnDataBound="lstvwReceiverDetails_DataBound" OnSorting="lstvwReceiverDetails_Sorting">
                                                    <LayoutTemplate>
                                                        <table id="tbllstvwReceiverDetails1" runat="server" align="center" cellpadding="0"
                                                            cellspacing="1" class="GridBorder" width="100%">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="PaddingL">
                                                                    <asp:Label ID="Label3" runat="server" CssClass="PaddingL" Style="padding-left: 5px;"
                                                                        Text="Receiver Name"></asp:Label>
                                                                </th>
                                                                <th align="left" class="PaddingL" width="200px">
                                                                    <asp:LinkButton ID="lnkPaymentDate" runat="server" CommandName="Sort" CommandArgument="InvoiceNo"
                                                                        Style="padding-left: 5px;" CausesValidation="false" ForeColor="Black" Text="Invoice No."></asp:LinkButton>
                                                                </th>
                                                                <th align="center" class="PaddingL" width="120px">
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" Style="padding-left: 5px;" CommandName="Sort"
                                                                        CommandArgument="InvoiceDate" CausesValidation="false" ForeColor="Black" Text="Invoice Date"></asp:LinkButton>
                                                                </th>
                                                                <th align="right" class="PaddingLR" width="120px">
                                                                    <asp:Label ID="Label6" runat="server" Text="Total Amount" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="right" class="PaddingLR" width="75px">
                                                                    <asp:Label ID="Label9" runat="server" Text="CGST" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="right" class="PaddingLR" width="75px">
                                                                    <asp:Label ID="Label10" runat="server" Text="SGST" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="right" class="PaddingLR" width="120px">
                                                                    <asp:Label ID="Label11" runat="server" Text="Final Amount" Style="padding-right: 5px;"></asp:Label>
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Edit
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Delete
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                    Details
                                                                </th>
                                                                <th align="center" style="width: 100px;">
                                                                GST Invoice 
                                                                </th>
                                                            </tr>
                                                            <tr id="itemPlaceholder" runat="server">
                                                            </tr>
                                                            <tr id="trDataPager" class="ClsBorderPager">
                                                                <td colspan="10">
                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwReceiverDetails"
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
                                                        <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                            <td align="left" class="PaddingL">
                                                                <asp:Label ID="lblReceiverName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("ReceiverName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td1" align="left" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblInvoiceNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("InvoiceNo") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                            </td>
                                                            <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblInvoiceDate1" runat="server" Text='<%# Eval("InvoiceDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td3" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblTotalAmount1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("TotalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td4" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label12" runat="server" Style="padding-right: 5px;" Text='<%# Eval("CGST") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td5" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label13" runat="server" Style="padding-right: 5px;" Text='<%# Eval("SGST") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td6" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label14" runat="server" Style="padding-right: 5px;" Text='<%# Eval("FinalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateReceiverDetails"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteReceiverDetails"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="LnkBtnDetails" runat="server" Text="Details" CausesValidation="false"></asp:LinkButton>
                                                            </td>
															 <td id="Td7" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkExport" runat="server" CommandName="InvoiceDetails" CausesValidation="false" Text="Open"></asp:LinkButton>
                                                            </td>
														
                                                        </tr>
                                                        <tr id="tr3" runat="server" style="display: none">
                                                            <td align="center" colspan="10">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:ListView ID="lstvwDescription" runat="server" ViewStateMode="Enabled">
                                                                                <LayoutTemplate>
                                                                                    <table id="tbllstvwDescription" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                                        class="GridBorder" width="50%">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th align="left"  width="5%">
                                                                                                <span class="PaddingL" style="padding-left:10px;">Description</span>
                                                                                            </th>
                                                                                            <th align="center" class="PaddingL" width="5%">
                                                                                                <span>Amount</span>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                                        <td align="left" >
                                                                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsPaddingL" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAlterRow">
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescription" runat="server" CssClass="ClsPaddingL" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lnkHide" runat="server" Text="Hide" CausesValidation="false"
                                                                                OnClientClick="HideDescription(this); return false;"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAltRow">
                                                            <td align="left" class="PaddingL">
                                                                <asp:Label ID="lblReceiverName1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("ReceiverName") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td1" align="left" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblInvoiceNo1" runat="server" Style="padding-left: 5px;" Text='<%# Eval("InvoiceNo") %>'>
                                                                </asp:Label>
                                                                <asp:HiddenField ID="hidData1" runat="server" Value="" />
                                                            </td>
                                                            <td id="Td2" align="center" class="PaddingL" runat="server">
                                                                <asp:Label ID="lblInvoiceDate1" runat="server" Text='<%# Eval("InvoiceDate") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td3" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="lblTotalAmount1" runat="server" Style="padding-right: 5px;" Text='<%# Eval("TotalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td4" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label12" runat="server" Style="padding-right: 5px;" Text='<%# Eval("CGST") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td5" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label13" runat="server" Style="padding-right: 5px;" Text='<%# Eval("SGST") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td id="Td6" align="right" class="PaddingLR" runat="server">
                                                                <asp:Label ID="Label14" runat="server" Style="padding-right: 5px;" Text='<%# Eval("FinalAmount") %>'>
                                                                </asp:Label>
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnEdit" runat="server" CausesValidation="false" CommandName="UpdateReceiverDetails"
                                                                    ImageUrl="../images/IconGrid_Edit.GIF" ToolTip="Edit" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:ImageButton ID="imgbtnDelete" runat="server" CausesValidation="false" CommandName="DeleteReceiverDetails"
                                                                    ImageUrl="../images/IconGrid_Delete.GIF" ToolTip="Delete" />
                                                            </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="LnkBtnDetails" runat="server" Text="Details" CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                            
															 <td id="Td7" runat="server" align="center">
                                                                <asp:LinkButton ID="lnkExport" runat="server" CommandName="InvoiceDetails" CausesValidation="false" Text="Open"></asp:LinkButton>
                                                            </td>
														
                                                        </tr>
                                                        <tr id="tr3" runat="server" style="display: none">
                                                            <td align="center" colspan="10">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:ListView ID="lstvwDescription" runat="server" ViewStateMode="Enabled">
                                                                                <LayoutTemplate>
                                                                                    <table id="tbllstvwDescription" runat="server" align="center" cellpadding="0" cellspacing="1"
                                                                                        class="GridBorder" width="50%">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th align="left" width="5%">
                                                                                                <span class="PaddingL" style="padding-left:10px;">Description</span>
                                                                                            </th>
                                                                                            <th align="center" class="PaddingL" width="5%">
                                                                                                <span>Amount</span>
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr id="trItemtemplate" runat="server" class="ClsGridRow">
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescription" CssClass="ClsPaddingL" runat="server" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                                <AlternatingItemTemplate>
                                                                                    <tr id="trItemtemplate" align="center" runat="server" class="ClsGridAlterRow">
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDescription" CssClass="ClsPaddingL" runat="server" Text='<%# Eval("Description") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                        <td align="center" class="PaddingL">
                                                                                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("Amount") %>'>
                                                                                            </asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </AlternatingItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="lnkHide" runat="server" Text="Hide" CausesValidation="false"
                                                                                OnClientClick="HideDescription(this); return false;"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <tr style="width: 800px">
                                                            <td align="center" class="LblNoRecord">
                                                                No record Found
                                                            </td>
                                                        </tr>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                                <asp:ObjectDataSource TypeName="BusinessLogic.GSTInvoiceDetailsBL" EnablePaging="true"
                                                    ID="objdsReceiverDetails" runat="server" SelectMethod="GetAll" SortParameterName="SortExpression"
                                                    SelectCountMethod="GetCount" EnableCaching="false">
                                                    <SelectParameters>
                                                        <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                        <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="int32" />
                                                        <asp:ControlParameter ControlID="txtSearch" Name="asFilter" Type="String" PropertyName="Text" />
                                                        <asp:Parameter Name="SortExpression" Type="String" />
                                                        <asp:Parameter Name="SortDirection" Type="String" />
                                                        <asp:Parameter Name="MaximumRows" Type="Int32" />
                                                        <asp:Parameter Name="StartRowIndex" Type="Int32" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:HiddenField ID="hidId" runat="server" Value="0" />
                                    <asp:HiddenField ID="hidSortDirection" runat="server" />
                                    <asp:HiddenField ID="hidSortExpression" runat="server" />
                                    <asp:HiddenField ID="hidGSTData" runat="server" Value="" />
                                    <asp:HiddenField ID="hidData" runat="server" OnValueChanged="hidData_ValueChanged" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                    <asp:AsyncPostBackTrigger ControlID="lstvwReceiverDetails" EventName="ItemCommand" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

        function ConfirmDelete() {
            return confirm('Are you sure you want to delete this record?')
        }


        function ValidateDescription(oSrc, args) {            
            if ($('[id$=txtDescription][value!=""]').length == 0) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateAmount(oSrc, args) {
            var isFound = false
            $('[id$=txtDescription][value!=""]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwGSTDetails_ctrl', '').replace('_txtDescription', '')
                var amt = $('[id$=' + index + '_txtAmount]').val()
                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function ValidateAmountDescription(oSrc, args) {
            var isFound = false
            $('[id$=txtAmount][value!=""][value!="0"]').each(function () {
                var index = this.id.replace('ctl00_MainBody_lstvwGSTDetails_ctrl', '').replace('_txtAmount', '')
                var amt = $('[id$=' + index + '_txtDescription]').val()
                if (amt == 0 || amt == '') {
                    isFound = true
                }
            })

            if (isFound) {
                args.IsValid = false
                return true
            }

            args.IsValid = true
            return false
        }

        function SetTotalAmount() {
            var gstRules = $('[id$=hidGSTData]').val()
            var gstId = $('[id$=ddlGSTCategory]').val()
            var rules = eval('[' + gstRules + ']')[0]

            var gstRule = rules.filter(function (dt) {
                return dt.Id == gstId
            })

            var totalAmt = 0;
            $('[id$=txtAmount][value!=""][value!="0"]').each(function () {
                totalAmt = totalAmt + parseFloat($(this).val())
            })

            var tax = ((totalAmt * gstRule[0].Percentage) / 100)

            tax = Math.ceil(tax);

            var finalAmt = totalAmt + tax
            var cgst = tax / 2

            finalAmt = Math.ceil(finalAmt);
           // cgst = Math.ceil(cgst);

            $('[id$=txtTotal]').val(totalAmt)
            $('[id$=txtCGST]').val(cgst)
            $('[id$=txtSGST]').val(cgst)
            $('[id$=txtTotalAmount]').val(finalAmt)
        }

        function ShowDescription(index) {
            //$('[id$=tr3]').hide(2000);
            $('#ctl00_MainBody_lstvwReceiverDetails_ctrl' + index + '_tr3').show()
        }

        function HideDescription(obj) {
            var id = obj.id.replace('ctl00_MainBody_lstvwReceiverDetails_ctrl', '').replace('_lnkHide', '')
            $('#ctl00_MainBody_lstvwReceiverDetails_ctrl' + id + '_tr3').hide()
        }

        function ResetLabel() {
            $('[id$=LblUpdateSuccess]').html('')
        }

        function OpenReceiverPopup() {
            window.open('ServiceReceiverDetailsPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700')
        }

        function OpenBankPopup() {
            window.open('DepositeBankDetailsPopup.aspx', '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=700')
        }
        
        function RefreshData() {
            var dt = Math.random()
            $('[id$=hidData]').val(dt)
            __doPostBack(document.getElementById("<%=this.hidData.ClientID %>").name, '')
        }
       
        function OpenReport(index) {
            var str = $('[id$=' + index + '_hidData1]').val()            
            window.open('../Admission/AdmissionFormReport.aspx?' + str, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=500,height=150')
        }

        function Increase() {
            UpdateAmount(0.5);
        }

        function UpdateAmount(val) {
            var totalAmt = $('[id$=txtTotal]').val()
            var cgst = $('[id$=txtCGST]').val()

            if (cgst != '') {
                var finalAmt = $('[id$=txtTotalAmount]').val()

                cgst = parseFloat(cgst) + val;
                finalAmt = parseFloat(totalAmt) + cgst + cgst;

                $('[id$=txtCGST]').val(cgst)
                $('[id$=txtSGST]').val(cgst)
                $('[id$=txtTotalAmount]').val(finalAmt)
            }
        }

        function Decrease() {
            UpdateAmount(-0.5);
        }

        function ValidateDescriptionLength(oSrc, args) {

            var isFound = false;
            $('[id$=txtDescription][value!=""]').each(function () {
                if ($(this).val().length > 500) {
                    $(this).css('background-color', 'yellow');
                    isFound = true
                }
                else
                    $(this).css('background-color', 'white');
            })

            if (isFound == true) {
                args.IsValid = false;
                oSrc.errormessage = 'Description length should be greater than 500 for Yellow coloured rows.'
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
