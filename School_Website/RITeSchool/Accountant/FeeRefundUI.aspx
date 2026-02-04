<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="FeeRefundUI.aspx.cs" Inherits="FeeRefundUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
            <ContentTemplate>
                <tr>
                    <td style="background-color: white" id="MainDataTable" valign="top">
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                            <tr>
                                <td align="left" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr>
                                            <td class="ClsGrayMainTitle" style="width: 99%;">
                                                <asp:Label ID="lblHeader" Text="<%$ Resources:LocalizedResources, FeeRefund %>" runat="server" CssClass="MainTitleHead"
                                                    Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                     <span class="ClsMdtStar" >*</span>
                                     <asp:Label ID="lblMandatoryFields" runat="server" class="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>                                   
                                </td>
                            </tr>
                            <tr align="left" valign="top">
                                <td>
                                    <asp:ValidationSummary runat="server" ID="valChequeData" ShowMessageBox="false" ShowSummary="true"
                                        ValidationGroup="Refund" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
        <tr>
            <td align="center">
                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel8">
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblErrMsg" runat="server" CssClass="LblErrorMsg" Visible="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="background-color: white" id="Td1" align="center" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr runat="server" id="trPaidDetails" align="left">
                                            <td valign="top">
                                                
                                                        <table cellpadding="2" cellspacing="2" style="width: 100%">
                                                            <tr id="trLstReqItems" runat="server" visible="true">
                                                                <td valign="top">
                                                                    <asp:ListView ID="lstvwRefundFee" runat="server" DataKeyNames="Schoolwise_Student_Fee_Id,Std_FeeType_Id,Student_Fee_Id,AccountHeaderId"
                                                                        OnDataBound="lstvwRefundFee_DataBound" OnItemDataBound="lstvwRefundFee_ItemDataBound">
                                                                        <LayoutTemplate>
                                                                            <table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                                cellspacing="1" class="GridBorder">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" runat="server" id="tblContacts" style="color: #333333" cellpadding="0"
                                                                                            cellspacing="1">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" class="ClspaddingL">
                                                                                                    <asp:CheckBox ID="chkAll" runat="server" />
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL">
                                                                                                   <asp:Label ID="lblFeeType" runat="server" Text="<%$ Resources:LocalizedResources, FeeType %>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" class="ClspaddingL">
                                                                                                   <asp:Label ID="lblPayableFor" runat="server" Text="<%$ Resources:LocalizedResources, PayableFor %>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="right" class="paddingLR">
                                                                                                   <asp:Label ID="lblPaidAmount" runat="server" Text="<%$ Resources:LocalizedResources,  PaidAmount %>"></asp:Label>   
                                                                                                </th>
                                                                                                <th class="ClspaddingL">
                                                                                                   <asp:Label ID="lblPaidDate" runat="server" Text="<%$ Resources:LocalizedResources,   PaidDate%>"></asp:Label>   
                                                                                                </th>
                                                                                                <th class="paddingLR" align="right">
                                                                                                   <asp:Label ID="lblRefundAmount" runat="server" Text="<%$ Resources:LocalizedResources, RefundAmount%>"></asp:Label>   
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
                                                                                    <asp:CheckBox ID="chkRefund" runat="server" />
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("Payable_For") %>' CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' CssClass="paddingLR" />
                                                                                    <asp:HiddenField ID="hidActualAmount" runat="server" Value='<%# Eval("Amount") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPaidDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Paid_Date")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                                                        CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td class="paddingLR" align="right">
                                                                                    <asp:TextBox ID="txtRefundAmount" runat="server" onblur="extractNumber(this,0,false);SetTotalAmount()"
                                                                                        onkeyup="extractNumber(this,0,false)" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                                        onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" MaxLength="6"
                                                                                        Text='<%# Eval("Amount") %>' CssClass="TxtAlignRght"></asp:TextBox>
                                                                                    <asp:Label ID="lblStar" runat="server" Text="*" ForeColor="Red" />
                                                                                </td>
                                                                            </tr>
                                                                        </ItemTemplate>
                                                                        <AlternatingItemTemplate>
                                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                <td align="left" class="ClspaddingL">
                                                                                    <asp:CheckBox ID="chkRefund" runat="server"/>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("Payable_For") %>' CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' CssClass="paddingLR" />
                                                                                    <asp:HiddenField ID="hidActualAmount" runat="server" Value='<%# Eval("Amount") %>' />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblPaidDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Paid_Date")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) %>'
                                                                                        CssClass="ClspaddingL" />
                                                                                </td>
                                                                                <td class="paddingLR" align="right">
                                                                                    <asp:TextBox ID="txtRefundAmount" runat="server" onblur="extractNumber(this,0,false);SetTotalAmount()"
                                                                                        onkeyup="extractNumber(this,0,false)" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                                                        onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" MaxLength="6"
                                                                                        Text='<%# Eval("Amount") %>' CssClass="TxtAlignRght"></asp:TextBox>
                                                                                    <asp:Label ID="lblStar" runat="server" Text="*" ForeColor="Red" />
                                                                                </td>
                                                                            </tr>
                                                                        </AlternatingItemTemplate>
                                                                    </asp:ListView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                 
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 94%;">
                                        <tr style="padding-right: 20px">
                                            <td align="right" style="width: 65%; height: 20px; padding-left: 20px">
                                                <asp:Label ID="lblTotalrefundAmount" runat="server" CssClass="LblNrmlB" Text="<%$ Resources:LocalizedResources,TotalRefundAmount%>" 
                                                    Font-Size="10pt" Font-Bold="True" EnableViewState="False"></asp:Label>
                                            </td>
                                            <td align="right" class="TxtAlignRght" width="20%">
                                                <asp:Label ID="lblRefund" runat="server" CssClass="LblNrmlB" Font-Bold="true" Font-Size="10pt"
                                                    Text="0"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                            <tr>
                                <td align="left" class="ClsBtmBorderGray" colspan="4">
                                    <asp:Label ID="lblPayment" runat="server" Font-Bold="True" CssClass="ClsLblLgnd"
                                        Text="<%$ Resources:LocalizedResources,RefundDetails%>" EnableViewState="false"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="CPanelSpace" colspan="3">
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="ClsBorderlight" valign="top" style="width: 25%">
                                    <asp:Label ID="lblMode" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, Mode%>" EnableViewState="False"></asp:Label>
                                </td>
                                <td align="left" class="ClsTextNormal" style="padding-right: 10px; width: 25%">
                                    <asp:RadioButton ID="optCheque" runat="server" GroupName="PaymentMode" Text="<%$ Resources:LocalizedResources,Cheque%>"
                                        OnCheckedChanged="optCheque_CheckedChanged" AutoPostBack="true" TabIndex="1" />
                                    &nbsp;<asp:RadioButton ID="optCash" runat="server" GroupName="PaymentMode" Text="<%$ Resources:LocalizedResources,Cash%>"
                                        AutoPostBack="true" OnCheckedChanged="optCash_CheckedChanged" TabIndex="2" />
                                </td>
                                <td align="right" class="ClsBorderlight" style="width: 18%;">
                                    <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources,RefundDate%>"></asp:Label>
                                    <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsTextNormal" style="width: 25%">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox" AutoPostBack="True"
                                        TabIndex="3"></asp:TextBox>
                                    <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtDate" Format="dd MMM yyyy" Culture="en"
                                        ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources,DateShouldNotBeBlank%>" />
                                    &nbsp;
                                    <%--<asp:Label ID="Label19" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>--%>
                                    <span class="ClsMdtStar">*</span>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" class="ClsBorderlight" valign="top">
                                    <asp:Label ID="Label1" runat="server" CssClass="ClsLabel" 
                                        Text="<%$ Resources:LocalizedResources, ChequeNo%>"  EnableViewState="False"></asp:Label>
                                        <span class="ClsLabel">:</span>
                                </td>
                                <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                    <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                        ondrop="event.returnValue=false;" TabIndex="4" Width="120px"></asp:TextBox>
                                    &nbsp;
                                    <asp:Label ID="lblChqNumberErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                        Text="*"></asp:Label>
                                    &nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="ClsBorderlight">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, ChequeDate%>" CssClass="ClsLabel" 
                                        EnableViewState="False"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                    <asp:TextBox ID="txtChequeDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                        TabIndex="5"></asp:TextBox>
                                    <rjs:PopCalendar ID="cal_ChequeDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy" Culture="en"
                                        ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, ChequeDateShouldNotBeBlank%>"/>
                                    &nbsp;
                                    <asp:Label ID="lblChqDateErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                        Text="*"></asp:Label>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="ClsBorderlight">
                                    <asp:Label ID="lblBankName" runat="server" Text="<%$ Resources:LocalizedResources, BankName%>"  CssClass="ClsLabel" 
                                        EnableViewState="False"></asp:Label>
                                          <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsTextNormal" style="padding-right: 10px" colspan="3">
                                    <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo" TabIndex="6">
                                    </asp:DropDownList>
                                    &nbsp;<asp:Label ID="lblBankErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                        Text="*"></asp:Label>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="right" valign="top" class="ClsBorderlight">
                                    <asp:Label ID="lblRemark" runat="server" Text="<%$ Resources:LocalizedResources, Remarks%>"  CssClass="ClsLabel" 
                                        EnableViewState="False"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                </td>
                                <td align="left" class="ClsTextNormal" colspan="3">
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="SmlTxtBox" MaxLength="50" TabIndex="7"
                                        Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="optCheque" EventName="CheckedChanged" />
                        <asp:AsyncPostBackTrigger ControlID="optCash" EventName="CheckedChanged" />
                    </Triggers>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hidStudentId" runat="server" />
                <asp:HiddenField ID="hidPaymentDate" runat="server" />
                <asp:HiddenField ID="hidMaxdate" runat="server" />
                <asp:HiddenField ID="hidMaximumPaidDate" runat="server" />
                <asp:HiddenField ID="hidStudentRegNo" runat="server" />
                <asp:HiddenField ID="hidPageIndex" runat="server" />
                <asp:HiddenField ID="hidRowCount" runat="server" />
                <asp:HiddenField ID="hidStdDivId" runat="server" />
                <asp:HiddenField ID="hidServerDate" runat="server" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" />
                <asp:HiddenField ID="hidChequeNumberShouldNotBeBlank" runat="server" />
                <asp:HiddenField ID="hidBankNameShouldBeSelected" runat="server" />
                <asp:HiddenField ID="hidRefundDateShouldBeGreaterThanOrEqualToPaymentDate" runat="server" />
                <asp:HiddenField ID="hidRefundDateShouldNotBeFutureDate" runat="server" />
                <asp:HiddenField ID="hidRefundDateShouldNotBeBlank" runat="server" />
                <asp:HiddenField ID="hidChequeDateShouldNotBeBlank" runat="server" />
                <asp:HiddenField ID="hidRefundAmountShouldNotBeGreaterThanActualAmount" runat="server" />
                <asp:HiddenField ID="hidRefundAmountShouldBeGreaterThanZero" runat="server" />
                <asp:HiddenField ID="hidRefundAmountShouldNotBeBlank" runat="server" />
                <asp:HiddenField ID="hidPleaseFixFollowingError" runat="server" />
                <asp:HiddenField ID="hidAtLeastOneFeeShouldBeSelectedForRefund" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnRefund" runat="server" Text= "<%$ Resources:LocalizedResources, Refund %>" CssClass="ClsBtnMid" OnClick="btnRefund_Click"
                                ValidationGroup="Refund" TabIndex="8" />
                        </td>
                        <td>
                            <asp:Button ID="btnClose" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnMid" runat="server" CausesValidation="false"
                                OnClick="btnClose_Click" TabIndex="9" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <tr>
                    <td>
                        <asp:CustomValidator ID="cstChequeNo" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateChequeNumber"
                            ErrorMessage="<%$ Resources:LocalizedResources, ErrorMsg%>" ValidationGroup="Refund"></asp:CustomValidator>

                        <asp:CustomValidator ID="cst_ChequeDate" runat="server" ClientValidationFunction="cstStartDate"
                            Display="None" Visible="true" SetFocusOnError="True" ErrorMessage="Cheque date."
                            ValidationGroup="Refund"></asp:CustomValidator>

                        <asp:CustomValidator ID="cstBankName" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateBankName"
                            ErrorMessage="<%$ Resources:LocalizedResources, ErrorMsg%>" ValidationGroup="Refund"></asp:CustomValidator>

                        <asp:CustomValidator ID="custValidAmount" runat="server" CssClass="ClsMdtStar" Display="None"
                            EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateAmount"
                            ErrorMessage="<%$ Resources:LocalizedResources, ErrorMsg%>" ValidationGroup="Refund"></asp:CustomValidator>
                    </td>
                </tr>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>

    <script language="javascript" type="text/javascript">
        _clientcustValidAmount = "<%=this.custValidAmount.ClientID %>"
        _clientCstStartDate = "<%=this.cst_ChequeDate.ClientID %>"
        _clientcstBankNameID = "<%=this.cstBankName.ClientID %>"
        _clientcstChequeNoID = "<%=this.cstChequeNo.ClientID %>"
        _clientcalStartDateID = "<%=this.txtChequeDate.ClientID %>"
        _clienttxtBankNameID = "<%=this.ddlBankName.ClientID %>"
        _clienttxtChequeNumberID = "<%=this.txtChequeNumber.ClientID %>"
        _clienttxtRemarksID = "<%=this.txtRemarks.ClientID %>"
        _clientoptChequeID = "<%=this.optCheque.ClientID %>"
        _clienttxtDateID = "<%=this.txtDate.ClientID %>"
        _clienthidPaymentDateId = "<%=this.hidPaymentDate.ClientID %>"
        _clientlblDateId = "<%=this.lblDate.ClientID %>"
        _clientListViewId = "<%=this.lstvwRefundFee.ClientID %>"
        _clientlblErrMsgId = "<%=this.lblErrMsg.ClientID %>"
        _clientbtnClose = "<%=this.btnClose.ClientID %>"
        _clientbtnRefund = "<%=this.btnRefund.ClientID %>"
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>"
        _clienthidMaxdate = "<%=this.hidMaxdate.ClientID %>"
        _clienthidMaximumPaidDate = "<%=this.hidMaximumPaidDate.ClientID %>"
        _clientvalChequeData = "<%=this.valChequeData.ClientID %>"
        _clientlblRefund = "<%=this.lblRefund.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>"
        function ValidateChequeNumber(aSrc, args) {
            if (document.getElementById(_clientoptChequeID).checked) {
                if (document.getElementById(_clienttxtChequeNumberID).value == "") {
                    document.getElementById(_clientcstChequeNoID).errormessage = document.getElementById("<%=hidChequeNumberShouldNotBeBlank.ClientID%>").value
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
            else {
                args.IsValid = true
                return false
            } 
        }
        function ValidateBankName(aSrc, args) {
            if (document.getElementById(_clientoptChequeID).checked) {
                if (document.getElementById(_clienttxtBankNameID).value == "0") {
                    document.getElementById(_clientcstBankNameID).errormessage = document.getElementById("<%=hidBankNameShouldBeSelected.ClientID%>").value
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
            else {
                args.IsValid = true
                return false
            } 
        }
        function cstStartDate(aSrc, args) {
            document.getElementById(_clientCstStartDate).errormessage = ""
            var dtPaidDate = new Date(document.getElementById(_clienthidMaximumPaidDate).value)
            var MaxDate = document.getElementById(_clienthidMaxdate).value
            var sServerDate = document.getElementById(_clientServerDate).value
            var ChequeDate = document.getElementById(_clientcalStartDateID).value
            var payDate = document.getElementById(_clienttxtDateID).value
            var dtMaxDate = new Date(document.getElementById(_clienthidMaxdate).value)
            var dtServerDate = new Date(document.getElementById(_clientServerDate).value)
            if (document.getElementById(_clientoptChequeID).checked) {
                if (ChequeDate != "") {
//                    var dtChequeDate = new Date(convertdate(ChequeDate))
//                    if (dtPaidDate > dtChequeDate) {
//                        document.getElementById(_clientCstStartDate).errormessage = "Cheque date should be greater than payment date."
//                        args.IsValid = false
//                        return true
//                    }
                     if (payDate != "") {
                        var dtpayDate = new Date(convertdate(payDate))
                        if (dtPaidDate > dtpayDate) {
                            document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldBeGreaterThanOrEqualToPaymentDate.ClientID%>").value
                            args.IsValid = false
                            return true
                        }
                        else if (dtServerDate < dtpayDate) {
                            document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldNotBeFutureDate.ClientID%>").value
                            args.IsValid = false
                            return true
                        }
                        else {
                            args.IsValid = true
                            return false
                        } 
                    }
                    else {
                        document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldNotBeBlank.ClientID%>").value
                        args.IsValid = false
                        return true
                    } 
                }
                else {
                    document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidChequeDateShouldNotBeBlank.ClientID%>").value
                    args.IsValid = false
                    return true
                } 
            }
            else
                if (payDate != "") {
                var dtpayDate = new Date(convertdate(payDate))
                if (dtPaidDate > dtpayDate) {
                    document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldBeGreaterThanOrEqualToPaymentDate.ClientID%>").value
                    args.IsValid = false
                    return true
                }
                else if (dtServerDate < dtpayDate) {
                    document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldNotBeFutureDate.ClientID%>").value
                    args.IsValid = false
                    return true
                }
                else {
                    args.IsValid = true
                    return false
                } 
            }
            else {
                document.getElementById(_clientCstStartDate).errormessage = document.getElementById("<%=hidRefundDateShouldNotBeBlank.ClientID%>").value
                args.IsValid = false
                return true
            }
        }

        function ValidateAmount(aSrc, args) {
            var sMessage = ""
            var i
            var iRowCount = document.getElementById(_clienthidRowCount).value
            document.getElementById(_clientcustValidAmount).errormessage = ""
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var chk = _clientListViewId + "_ctrl" + RowNumber + "_" + "chkRefund"
                var txt = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtRefundAmount"
                var lbl = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualAmount"
                var lblFeeType = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblFeeType"
                var lblPayable = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblPaybleFor"
                var iRefundAmt = document.getElementById(txt).value
                var iActualAmt = document.getElementById(lbl).value
                var sFeeType = document.getElementById(lblFeeType).innerHTML
                var sPayable = document.getElementById(lblPayable).innerHTML
                if (document.getElementById(chk).checked) {
                    if (iRefundAmt != '')
                     {
                        if (parseInt(iRefundAmt) != 0) {
                            if (parseInt(iRefundAmt) > parseInt(iActualAmt)) {
                                sMessage += document.getElementById("<%=hidRefundAmountShouldNotBeGreaterThanActualAmount.ClientID%>").value + ":" + sFeeType + " (" + sPayable + ")<BR/> "
                                document.getElementById(_clientcustValidAmount).errormessage += document.getElementById("<%=hidRefundAmountShouldNotBeGreaterThanActualAmount.ClientID%>").value + " : " + sFeeType + " (" + sPayable + ").<BR/>"
                            } 
                        }
                        else {
                            sMessage += document.getElementById("<%=hidRefundAmountShouldBeGreaterThanZero.ClientID%>").value + " : " + sFeeType + " (" + sPayable + ")<BR/> "
                            document.getElementById(_clientcustValidAmount).errormessage += document.getElementById("<%=hidRefundAmountShouldBeGreaterThanZero.ClientID%>").value + " : " + sFeeType + " (" + sPayable + ").<BR/> "
                        } 
                    }
                    else 
                    {
                        sMessage += document.getElementById("<%=hidRefundAmountShouldNotBeBlank.ClientID%>").value + " : " + sFeeType + " (" + sPayable + ").<BR/> "
                        document.getElementById(_clientcustValidAmount).errormessage += document.getElementById("<%=hidRefundAmountShouldNotBeBlank.ClientID%>").value + " : " + sFeeType + " (" + sPayable + ").<BR/> "
                    } 
                } 
            }
            if (sMessage != "") {
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false
            } 
        }
        function fnover(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "maroon"
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
        }
        function fnout(varname) {
            var objTXT = document.getElementById(varname)
            objTXT.style.borderWidth = "1"
            objTXT.style.borderColor = "#a3c07b"
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
        }
        function EnabledCheckBox(ochk, iRowCount) {
            var bFlag
            bFlag = document.getElementById(ochk).checked
            var sMessage
            var Max = 0
            var i
            var ItemName = ""
            var sRowNumber = ""
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i
                var chk = _clientListViewId + "_ctrl" + RowNumber + "_" + "chkRefund"
                var txt = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtRefundAmount"
                var lbl = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblStar"
                var hid = _clientListViewId + "_ctrl" + RowNumber + "_" + "hidActualAmount"
                document.getElementById(chk).checked = bFlag
                document.getElementById(txt).disabled = !bFlag
                document.getElementById(lbl).disabled = !bFlag
                if (!bFlag)
                    document.getElementById(txt).value = document.getElementById(hid).value
            }
            SetTotalAmount()
        }
        function EnabledTextBox(ochk, otxt, olbl, ohid) {
            var bFlag
            bFlag = document.getElementById(ochk).checked
            document.getElementById(otxt).disabled = !bFlag
            document.getElementById(olbl).disabled = !bFlag
            if (!bFlag)
                document.getElementById(otxt).value = document.getElementById(ohid).value
            SetTotalAmount()
        }
        function CheckAtleastOneCheckBox(ochk, iRowCount) {
            var bFlag = false
            var sMessage = ""
            var Max = 0
            var i
            var ItemName = ""
            var sRowNumber = ""
            if (document.getElementById(ochk).checked)
                bFlag = true
            else {
                for (i = 0; i < iRowCount; i++) {
                    RowNumber = i
                    var chk = _clientListViewId + "_ctrl" + RowNumber + "_" + "chkRefund"
                    if (document.getElementById(chk).checked) {
                        bFlag = true
                        break
                    } 
                } 
            }
            if (bFlag)
                return true
            else {
                alert(document.getElementById("<%=hidPleaseFixFollowingError.ClientID%>").value + "\n\r\n\r" + document.getElementById("<%=hidAtLeastOneFeeShouldBeSelectedForRefund.ClientID%>").value)
                return false
            } 
        }
        function SetTotalAmount() {
            var iCount = document.getElementById(_clienthidRowCount).value
            var i
            var iTotalAmount = 0
            var odtPaidDate
            for (i = 0; i < iCount; i++) {
                RowNumber = i
                var chk = _clientListViewId + "_ctrl" + RowNumber + "_" + "chkRefund"
                var txt = _clientListViewId + "_ctrl" + RowNumber + "_" + "txtRefundAmount"
                var lblPaidDate = _clientListViewId + "_ctrl" + RowNumber + "_" + "lblPaidDate"
               
                if (document.getElementById(chk).checked) {
                    if (document.getElementById(txt).value != '') {

                        iTotalAmount = parseInt(iTotalAmount) + parseInt(document.getElementById(txt).value)
                        odtPaidDate = new Date(convertdate(document.getElementById(lblPaidDate).innerHTML))
                        if (document.getElementById(_clienthidMaximumPaidDate).value != '') {
                            var odtMaxDate = new Date(document.getElementById(_clienthidMaximumPaidDate).value)
                                if(odtMaxDate<odtPaidDate)
                                    odtPaidDate = new Date(convertdate(document.getElementById(lblPaidDate).innerHTML))
                                else
                                    odtPaidDate = new Date(document.getElementById(_clienthidMaximumPaidDate).value)
                            }
                            document.getElementById(_clienthidMaximumPaidDate).value = odtPaidDate
                          
                    } 
                } 
            }
            document.getElementById(_clientlblRefund).innerHTML = iTotalAmount
        }
    </script>
</asp:Content>
