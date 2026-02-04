<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PayTransportChargesPopUp.aspx.cs" Inherits="PayTransportChargesPopUp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                            <tr>
                                <td style="height: 19px" align="left" colspan="6" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                        <tr>
                                            <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
                                                <span class="MainTitleHead" id="lblMainTitleHead" runat="server" style="font-weight: bold">
                                                    Pay Transport Charges</span>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="valErrMsg" HeaderText="Please fix following error(s)"
                                        ValidationGroup="Pay" runat="server" />
                                    <asp:ValidationSummary ID="valErrRefund" HeaderText="Please fix following error(s)"
                                        ValidationGroup="Refund" runat="server" />
                                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                        Visible="False"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="1">
                                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Font-Bold="true"
                                        Width="100%" Visible="false" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="left">
                <table>
                    <tr>
                        <td class="ClsBorderlight" valign="middle" style="padding-left: 5px">
                            <asp:Label ID="lblStudent" runat="server" CssClass="TextNormal" Text="Name " EnableViewState="False"></asp:Label>
                            <span class="TextNormal">:</span>
                        </td>
                        <td class="ClsHilightBGB">
                            <asp:Label ID="lblStudentHeading" runat="server" EnableViewState="True"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trLegend" runat="server">
            <td align="left">
                <table>
                    <tr>
                        <td align="left" width="25px">
                            <asp:Label ID="lblLegend" runat="server" class="ClsLblLgnd" Style="font: Bold; width: 50px"
                                Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                        </td>
                        <td align="left" style="padding-right: 3px" width="25px">
                            <asp:Label ID="Label15" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                CssClass="PendingFees" EnableViewState="False" Height="20px" Text=" " Width="20px">
					<img height="20px" src="../images/spacer.gif" width="20px" />
                            </asp:Label>
                        </td>
                        <td align="left" width="85px">
                            <asp:Label ID="lblDelayedFees" EnableViewState="false" runat="server" class="ClsTextNormal"
                                Style="font: Bold" Text="<%$ Resources:LocalizedResources, DelayedFees%>"></asp:Label>
                        </td>
                        <td align="left" colspan="1" style="padding-right: 3px" width="30px">
                            <asp:Label ID="TextBox2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
                                CssClass="ClsGridNA" Height="17px" Text=" " Width="20px" EnableViewState="False">
					<img src="../images/spacer.gif" width="18px" height="14px"/>
                            </asp:Label>
                        </td>
                        <td align="left" width="85">
                            <asp:Label ID="lblRefundFees" EnableViewState="false" runat="server" class="ClsTextNormal"
                                Style="font: Bold" Text="<%$ Resources:LocalizedResources, RefundFees%>"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="pnl" runat="server">
                    <ContentTemplate>
                        <table width="100%">
                            <tr runat="server" id="aaa">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwTransportFee" runat="server" OnItemDataBound="lstvwTransportFee_ItemDataBound"
                                                    DataKeyNames="TransportFeeDetailsId,oStudentPaidFeeDetails,oStudentPayFeeDetails,IsArrears,IsRefund,IsConcession,IsLastCredit,IsAutoRefund"
                                                    OnItemCommand="lstvwTransportFee_ItemCommand" OnDataBound="lstvwTransportFee_DataBound">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                                                <th id="thchk" runat="server" align="center" width="3%">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" AutoPostBack="false"
                                                                        TabIndex="1" />
                                                                </th>
                                                                <th id="thMonth" runat="server" align="left" style="padding-left: 5px; font-weight: inherit">
                                                                    Month Name
                                                                </th>
                                                                <th id="thPaybleFor" runat="server" align="left" width="25%" style="padding-left: 5px;
                                                                    font-weight: inherit">
                                                                    Payble For
                                                                </th>
                                                                <th id="thAmount" runat="server" align="right" width="9%" style="padding-right: 5px;
                                                                    font-weight: inherit">
                                                                    Amount
                                                                </th>
                                                                <th id="thDueDate" runat="server" align="center" width="12%" style="font-weight: inherit;
                                                                    white-space: nowrap">
                                                                    <asp:Label ID="lbllstDueDate" runat="server" Text="Due Date" Style="float: none;
                                                                        font-size: 9pt; font-family: Arial;"></asp:Label>
                                                                </th>
                                                                <th id="thLateFee" runat="server" align="right" width="9%" style="font-weight: inherit;
                                                                    padding-right: 5px; white-space: nowrap">
                                                                    Late Fee
                                                                </th>
                                                                <th id="thDelete" runat="server" align="center" width="8%" style="font-weight: inherit">
                                                                    Delete
                                                                </th>
                                                                <th id="thPrint" runat="server" align="center" width="8%" style="font-weight: inherit" visible="false">
                                                                    Receipt
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trlstvwRow" runat="server" class="ClsMarksGridAltRowN">
                                                            <td id="tdchk" runat="server" align="center">
                                                                <asp:CheckBox ID="chkSelect" runat="server" AutoPostBack="false" TabIndex="2" />
                                                                <asp:HiddenField ID="hidTransportFeeDetailsId" runat="server" Value='<%#Eval("TransportFeeDetailsId") %>' />
                                                            </td>
                                                            <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("MonthName") %>' />
                                                            </td>
                                                            <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("oStudentPaidFeeDetails.PayableFor") %>' />
                                                            </td>
                                                            <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("oStudentPaidFeeDetails.Amount") %>' />
                                                            </td>
                                                            <td id="tdDueDate" runat="server" align="center">
                                                                <asp:Label ID="lblDueDate" runat="server" Text='<%# Eval("oStudentPayFeeDetails.PaymentDate","{0:dd-MMM-yyyy}") %>' />
                                                            </td>
                                                            <td id="tdLateFee" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblLateFee" runat="server" Text='<%# Eval("oStudentPaidFeeDetails.LateFeeAmount") %>' />
                                                            </td>
                                                            <td id="tdDelete" runat="server" align="center">
                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    CausesValidation="false" CommandName="RemoveCommand" TabIndex="2" CommandArgument='<%#Eval("oStudentPayFeeDetails.ReceiptNumberOutput") %>' />
                                                            </td>
                                                            <td id="tdPrint" runat="server" align="center" visible="false">
                                                                <asp:HyperLink ID="hlnkReceipt" runat="server" Text="Receipt" Visible="true" NavigateUrl="#"
                                                                    Enabled="false"> </asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    No Records Found.
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <table width="100%" id="tblPendingFeeDetails" runat="server">
                                        <tr>
                                            <td align="left" valign="middle" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Refund Date :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtRefundDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="False"
                                                    TabIndex="5"></asp:TextBox>
                                                <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtRefundDate" Format="dd MMM yyyy"
                                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                                    AutoPostBack="False" To-Today="true" />
                                                <asp:CustomValidator ID="cstvalRefund" runat="server" Display="None" ClientValidationFunction="ValidateRefundFees"
                                                    ValidationGroup="Refund" ErrorMessage=""></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstvalRefundDate" runat="server" ClientValidationFunction="ValidateRefundDate"
                                                    ValidationGroup="Refund" Display="none" EnableClientScript="true" ErrorMessage=""></asp:CustomValidator>
                                                <span class="ClsMdtStar">* </span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="middle" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Total Refund Amount :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtTotalRefundAmount" CssClass="SmlTxtBox" runat="server" AutoPostBack="False"
                                                    MaxLength="6" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" ReadOnly="true"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="100%" id="tblFeeDetails" runat="server">
                                        <tr>
                                            <td align="left
                                            " valign="middle" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Payment Date :</span>
                                            </td>
                                            <td align="left" style="width: 95%">
                                                <asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                                    TabIndex="5" OnTextChanged="txtPaymentDate_TextChanged"></asp:TextBox>
                                                <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
                                                    Culture="en" ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                                    OnSelectionChanged="cal_PaymentDate_SelectionChanged" AutoPostBack="True" To-Today="true" />
                                                <span class="ClsMdtStar">* </span>
                                            </td>
                                        </tr>
                                        <tr>
						                    <td align="left" class="ClsBorderlight " style="background-color: #ffffc4; padding: 3px;width:17%" >
                                                <span class="LblNrmlB" style="font-weight: bold; height: 16px;">Note :</span>
                                            </td>
                                            <td  align="left" class="ClsBorderlight"  style="padding: 3px; width:80%" >
                                                <div id="div1" style="font-family: Verdana; font-size: 8pt; border:100%;">
                                                    If you change the payment date then all the fee type selection after page load will lost.
                                                </div>
                                            </td>
										</tr>    
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Payable Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:TextBox ID="txtPayableAmt" TabIndex="6" runat="server" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" ReadOnly="true"></asp:TextBox>&nbsp; <span class="ClsMdtStar">
                                                        * </span>
                                                <asp:CustomValidator ID="cstFeeType" runat="server" Display="None" ClientValidationFunction="ValidateFees"
                                                    ValidationGroup="Pay" ErrorMessage=""></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstValidatePaidDate" runat="server" ClientValidationFunction="ValidateDate"
                                                    ValidationGroup="Pay" Display="none" EnableClientScript="true" ErrorMessage=""></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Late Fee Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:TextBox ID="txtLateFeeAmt" TabIndex="7" runat="server" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);CalculateActualAmtForLateFee();" AutoPostBack="false"
                                                    Text="0" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Concession Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:TextBox ID="txtConcessionAmt" TabIndex="8" runat="server" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);CalculateActualAmtForConcession();" AutoPostBack="false"
                                                    Text="0" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="height: 23px; width: 24%;">
                                                <span class="ClsLabel">Actual Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="height: 23px">
                                                <asp:TextBox ID="txtActualAmt" TabIndex="9" runat="server" MaxLength="6" onblur="extractNumber(this,0,false)"
                                                    ReadOnly="true" onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" CssClass="SmlTxtBox"></asp:TextBox>&nbsp;
                                                <span class="ClsMdtStar">* </span>
                                            </td>
                                        </tr>
                                        <tr id="trRemarks" runat="server">
                                            <td align="left" class="ClsBorderlight" style="width: 24%; height: 40px;">
                                                <span class="ClsLabel">Remarks :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="height: 40px">
                                                <asp:TextBox ID="txtRemarks" TabIndex="10" runat="server" MaxLength="100" CssClass="SmlTxtBox"
                                                    ReadOnly="false" Width="400px" TextMode="MultiLine"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="cst_Remarks" runat="server" Display="None" ControlToValidate="txtRemarks"
                                                    ValidationGroup="Pay" ErrorMessage="Length of remarks should not exceed 1000 characters."
                                                    CssClass="ClsMdtStar" ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 24%">
                                                <asp:Image ID="Image3" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                                    Width="148px" />
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:HiddenField ID="hidUserId" runat="server" />
                                    <asp:HiddenField ID="hidUserName" runat="server" />
                                    <asp:HiddenField ID="hidSearch" runat="server" />
                                    <asp:HiddenField ID="hidRole" runat="server" />
                                    <asp:HiddenField ID="hidIsOnlyRefund" runat="server" />
                                    <asp:HiddenField ID="hidYearEndDate" runat="server" />
                                    <asp:HiddenField ID="hidYearStartDate" runat="server" />
                                    <asp:HiddenField ID="hidRemarks" runat="server" />
                                    <asp:HiddenField ID="hidTotalAmount" runat="server" />
                                    <asp:HiddenField ID="hidPageIndex" runat="server" />
                                    <asp:HiddenField ID="hidPostBackElementId" runat="server" />
                                    <asp:HiddenField ID="hidQueryString" runat="server" />
                                    <asp:HiddenField ID="hidRowCount" runat="server" />
                                    <asp:HiddenField ID="hidCurrentDate" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnRefund" Text="Refund" runat="server" CssClass="ClsBtnMid" disable-page="true"
                    ValidationGroup="Refund" TabIndex="11" UseSubmitBehavior="true" OnClick="btnRefund_Click" />
                <asp:Button ID="btnPay" Text="Pay" runat="server" CssClass="ClsBtnMid" OnClick="btnPay_Click"
                    ValidationGroup="Pay" disable-page="true" TabIndex="11" UseSubmitBehavior="true" />
                <asp:Button ID="btnPayAndPrint" Text="Pay and Print" runat="server" CssClass="ClsBtnMid"
                    ValidationGroup="Pay" Enabled="false" disable-page="true" TabIndex="12" UseSubmitBehavior="true"
                    OnClick="btnPay_Click" />
                <asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnMid" CausesValidation="False"
                    TabIndex="13" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">

        _clientbtnPay = "<%=this.btnPay.ClientID %>";
        _clientbtnPayAndPrint = "<%=this.btnPayAndPrint.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
        _clientlstvwTransportFee = "<%=this.lstvwTransportFee.ClientID %>";
        _clienttxtPayableAmt = "<%=this.txtPayableAmt.ClientID %>";
        _clienttxtActualAmt = "<%=this.txtActualAmt.ClientID %>";
        _clienttxtLateFeeAmt = "<%=this.txtLateFeeAmt.ClientID %>";
        _clienttxtConcessionAmt = "<%=this.txtConcessionAmt.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clienttxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>";
        _clienttxtRefundDate = "<%=this.txtRefundDate.ClientID %>";
        _clienthidIsOnlyRefund = "<%=this.hidIsOnlyRefund.ClientID %>";
        _clienttxtTotalRefundAmount = "<%=this.txtTotalRefundAmount.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>";
        _clienthidCurrentDate = "<%=this.hidCurrentDate.ClientID %>";

        function CloseWindow() {
            window.opener.location = window.opener.location.pathname + "?" + $get(_clienthidQueryString).value;
            window.opener.focus();
            window.close();
        }
        
        function ValidateDate(src, args) {
            var bIsValid = true;
            var StartDate = $get(_clientYearStartDate).value;
            var EndDate = $get(_clientYearEndDate).value;
            var today = $get(_clienthidCurrentDate).value;
            var dtPaymentDate = $get(_clienttxtPaymentDate);
            dtPaymentDate.value = dtPaymentDate.value.trim();

            if (dtPaymentDate.value == "") {
                src.errormessage = "Payment Date should not be blank.";
                bIsValid = false;
            }
            else if (dtPaymentDate.value != "" && !validateDate(dtPaymentDate)) {
                src.errormessage = "Payment Date should be in valid format.";
                bIsValid = false;
            }
            else if (getDate(today) < getDate(dtPaymentDate.value)) {
                src.errormessage = "Payment Date should not be future date.";
                bIsValid = false;
            }
            else if (getDate(StartDate) > getDate(dtPaymentDate.value) || getDate(EndDate) < getDate(dtPaymentDate.value)) {
                src.errormessage = "Payment Date should be within the current academic year ( " + StartDate + " To " + EndDate + " ).";
                bIsValid = false;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function validateDate(txtDueDate) {
            var isValid = true;
            if (document.all) {
                if (isNaN(new Date(convertdate(txtDueDate.value).replace(/-/g, ' '))))
                    isValid = false;
            }
            else {
                if (isNaN(new Date(convertdate(txtDueDate.value).replace('-', ' '))))
                    isValid = false;
            }
            return isValid;
        }

        function getDate(obj) {
            var strDate = obj.replace('-', ' ').replace('-', ' ');
            return new Date(strDate);
        }

        function CheckAll(Src) {
            var listView = parseInt($get(_clienthidRowCount).value);
            var first = true;
            for (var i = 0; i < listView; i++) {
                chk = $get(_clientlstvwTransportFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    chk.checked = Src.checked;
                    if ($get(_clienthidIsOnlyRefund).value == "False") {
                        if (first) {
                            $get(_clienttxtPayableAmt).value = "0";
                            $get(_clienttxtActualAmt).value = "0";
                            first = false;
                        }
                        CheckSelected(chk, i);
                    }
                    else {
                        if (first) {
                            $get(_clienttxtTotalRefundAmount).value = "0";
                            first = false;
                        }
                        CheckSelected(chk, i);
                    }
                }
            }
        }

        function ValidateRefundDate(src, args) {
            var bIsValid = true;
            var bIsValidRefundDate = true;
            var today = $get(_clienthidCurrentDate).value;
            var dtPaymentDate = $get(_clienttxtRefundDate);
            dtPaymentDate.value = dtPaymentDate.value.trim();
            var PaidDate;
            var listView = parseInt($get(_clienthidRowCount).value);
            var first = true;
            for (var i = 0; i < listView; i++) {
                chk = $get(_clientlstvwTransportFee + "_ctrl" + i + "_chkSelect");
                PaidDate = $get(_clientlstvwTransportFee + "_ctrl" + i + "_lblDueDate");
                if (chk != null && chk.checked && PaidDate != null && getDate(dtPaymentDate.value) < getDate(PaidDate.innerHTML)) {
                    bIsValidRefundDate = false;
                    break;
                }
            }

            if (dtPaymentDate.value == "") {
                src.errormessage = "Refund Date should not be blank.";
                bIsValid = false;
            }
            else if (dtPaymentDate.value != "" && !validateDate(dtPaymentDate)) {
                src.errormessage = "Refund Date should be in valid format.";
                bIsValid = false;
            }
            else if (getDate(today) < getDate(dtPaymentDate.value)) {
                src.errormessage = "Refund Date should not be future date.";
                bIsValid = false;
            }
            else if (!bIsValidRefundDate) {
                src.errormessage = "Refund Date should be greater than Paid Date for selected payment(s).";
                bIsValid = false;
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CheckSelected(obj, iRowCount) {
            // if block will execute in case of pay charges.
            if ($get(_clienthidIsOnlyRefund).value == "False") {
                var PreviousPayble;
                var PreviousLateFee;
                var Concession;
                var chk = $get(_clientlstvwTransportFee + "_ctrl" + iRowCount + "_chkSelect");
                if (chk != null) {

                    var lblAmount = $get(_clientlstvwTransportFee + "_ctrl" + iRowCount + "_lblAmount");
                    var lblLateFee = $get(_clientlstvwTransportFee + "_ctrl" + iRowCount + "_lblLateFee");

                    if (lblAmount != null && lblLateFee != null) {

                        PreviousPayble = $get(_clienttxtPayableAmt).value;
                        PreviousLateFee = $get(_clienttxtLateFeeAmt).value;

                        if (PreviousLateFee == "-" || PreviousLateFee == "")
                            PreviousLateFee = 0;
                        if (PreviousPayble == "-" || PreviousPayble == "")
                            PreviousPayble = 0;

                        if (chk.checked) {
                            $get(_clienttxtPayableAmt).value = parseInt(PreviousPayble) + parseInt(lblAmount.innerHTML);
                            $get(_clienttxtLateFeeAmt).value = parseInt(PreviousLateFee) + parseInt(lblLateFee.innerHTML);
                        }

                        if (!chk.checked) {
                            if (PreviousPayble != "0")
                                $get(_clienttxtPayableAmt).value = parseInt(PreviousPayble) - parseInt(lblAmount.innerHTML);
                            if (PreviousLateFee != "0" && parseInt(PreviousLateFee) >= parseInt(lblLateFee.innerHTML))
                                $get(_clienttxtLateFeeAmt).value = parseInt(PreviousLateFee) - parseInt(lblLateFee.innerHTML);
                        }
                    }

                    Concession = $get(_clienttxtConcessionAmt).innerHTML;
                    if (Concession == "-" || Concession == "")
                        Concession = 0;
                    $get(_clienttxtActualAmt).value = parseInt($get(_clienttxtPayableAmt).value) + parseInt($get(_clienttxtLateFeeAmt).value) - parseInt(Concession);
                }
            }
            //Following code will execute in case of refund only.
            else {
                var PreviousPayble;
                var chk = $get(_clientlstvwTransportFee + "_ctrl" + iRowCount + "_chkSelect");
                if (chk != null) {
                    var lblAmount = $get(_clientlstvwTransportFee + "_ctrl" + iRowCount + "_lblAmount");
                    if (lblAmount != null) {
                        PreviousPayble = $get(_clienttxtTotalRefundAmount).value;
                        if (PreviousPayble == "-" || PreviousPayble == "")
                            PreviousPayble = 0;

                        if (chk.checked)
                            $get(_clienttxtTotalRefundAmount).value = parseInt(PreviousPayble) + parseInt(lblAmount.innerHTML);

                        if (!chk.checked && PreviousPayble != "0")
                            $get(_clienttxtTotalRefundAmount).value = parseInt(PreviousPayble) - parseInt(lblAmount.innerHTML);
                    }
                }
            }
        }

        function CalculateActualAmtForConcession() {
            var PaybleAmount = $get(_clienttxtPayableAmt).value;
            var ConcessionAmount = $get(_clienttxtConcessionAmt).value;
            var LateFeeAmount = $get(_clienttxtLateFeeAmt).value;

            if (PaybleAmount == "")
                PaybleAmount = 0;
            if (ConcessionAmount == "")
                ConcessionAmount = 0;
            if (LateFeeAmount == "")
                LateFeeAmount = 0;
            if (parseInt(ConcessionAmount) >= (parseInt(PaybleAmount) + parseInt(LateFeeAmount)))
                $get(_clienttxtConcessionAmt).value = "0";
            CaluclateActualAmount();
        }

        function CalculateActualAmtForLateFee() {
            var PaybleAmount = $get(_clienttxtPayableAmt).value;
            var LateFeeAmount = $get(_clienttxtLateFeeAmt).value;
            var ConcessionAmount = $get(_clienttxtConcessionAmt).value;
            if (PaybleAmount == "")
                PaybleAmount = 0;
            if (LateFeeAmount == "")
                LateFeeAmount = 0;
            if (ConcessionAmount == "")
                ConcessionAmount = 0;
            if ((parseInt(PaybleAmount) + parseInt(LateFeeAmount)) < parseInt(ConcessionAmount))
                $get(_clienttxtConcessionAmt).value = 0;
            CaluclateActualAmount();
        }

        function CaluclateActualAmount() {
            var PaybleAmount = $get(_clienttxtPayableAmt).value;
            var LateFeeAmount = $get(_clienttxtLateFeeAmt).value;
            var ConcessionAmount = $get(_clienttxtConcessionAmt).value;

            if (PaybleAmount == "")
                PaybleAmount = 0;
            if (LateFeeAmount == "")
                LateFeeAmount = 0;
            if (ConcessionAmount == "")
                ConcessionAmount = 0;
            $get(_clienttxtActualAmt).value = parseInt(PaybleAmount) + parseInt(LateFeeAmount) - parseInt(ConcessionAmount);
        }

        function ValidateFees(oSrc, args) {
            var Selected = false;
            var listView = parseInt($get(_clienthidRowCount).value);
            for (var i = 0; i < listView; i++) {
                chk = $get(_clientlstvwTransportFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    if (chk.checked)
                        Selected = true;
                }
            }
            if (!Selected) {
                if ($get(_clientlblUpdateSucess) != null)
                    $get(_clientlblUpdateSucess).innerHTML = "";
                oSrc.errormessage = "At least one Month should be selected to pay.";

                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false;
            }
        }

        function ValidateRefundFees(oSrc, args) {
            var Selected = false;
            var listView = parseInt($get(_clienthidRowCount).value);
            for (var i = 0; i < listView; i++) {
                chk = $get(_clientlstvwTransportFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    if (chk.checked)
                        Selected = true;
                }
            }
            if (!Selected) {
                if ($get(_clientlblUpdateSucess) != null)
                    $get(_clientlblUpdateSucess).innerHTML = "";
                oSrc.errormessage = "At least one Month should be selected to refund.";

                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false;
            }
        }       

        function ConfirmDelete() {
            return window.confirm("Are you sure you want to delete this fee payment?")
        }

    </script>
</asp:Content>
