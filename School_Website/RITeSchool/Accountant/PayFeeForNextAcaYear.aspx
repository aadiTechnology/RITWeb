<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="PayFeeForNextAcaYear.aspx.cs" Inherits="PayFeeForNextAcaYear" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel11">
        <ContentTemplate>
            <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                vertical-align: top">
                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel3">
                    <ContentTemplate>
                        <tr>
                            <td style="background-color: white" id="MainDataTable" valign="top">
                                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                    <tr>
                                        <td align="left" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td class="ClsGrayMainTitle" style="width: 99%;">
                                                        <asp:Label ID="lblHeader" runat="server" Text="Pay Fee For Next Academic Year" CssClass="MainTitleHead"
                                                            Font-Bold="True" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trMandetory" runat="server">
                                        <td align="right">
                                            <span class="ClsMdtStar">* Mandatory Fields</span>
                                        </td>
                                    </tr>
                                    <tr align="left" valign="top">
                                        <td>
                                            <asp:ValidationSummary ID="valChequeData" runat="server" ShowMessageBox="false" ShowSummary="true"
                                                ValidationGroup="Pay" />
                                        </td>
                                    </tr>
                                    <tr id="trlblErrMsg" runat="server" visible="false">
                                        <td align="left">
                                            <asp:Label ID="lblErrMsg" runat="server" CssClass="LblErrorMsg" Visible="false" Style="padding-left: 20px" />
                                        </td>
                                    </tr>
                                    <tr id="trSuccessMsg" runat="server" visible="false">
                                        <td align="center">
                                            <asp:Label ID="lblUpdateMessage" Style="text-align: center; font-weight: bold;" runat="server"
                                                ForeColor="blue" Width="100%" CssClass="ClsLabel" EnableViewState="false"></asp:Label><br />
                                        </td>
                                    </tr>
                                </table>
                                <table width="70%" align="left">
                                    <tr>
                                        <td align="left" colspan="4" style="height: 3px">
                                        </td>
                                    </tr>
                                    <tr id="trNote" runat="server" visible="true">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td align="left" class="ClsBorderlight " style="width: 11%; background-color: #ffffc4;">
                                            <span class="LblNrmlB">Note :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <span class="LblSmlV">You can pay partial fee for a payable only once.</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="4" style="height: 3px">
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
                                        <td style="background-color: white" id="Td1" align="center" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                                <tr runat="server" id="trPaidDetails" align="left">
                                                    <td valign="top">
                                                        <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel5">
                                                            <ContentTemplate>
                                                                <table cellpadding="2" cellspacing="2" style="width: 100%">
                                                                    <tr id="trLstReqItems" runat="server" visible="true">
                                                                        <td valign="top">
                                                                            <asp:ListView ID="lstvwPayFee" runat="server" DataKeyNames="Std_FeeType_Id,SerialNo,RowNo,Amount,PaidDate,ConcessionAmount"
                                                                                OnItemCommand="lstvwPayFee_ItemCommand" OnItemDataBound="lstvwPayFee_ItemDataBound"
                                                                                OnDataBound="lstvwPayFee_DataBound">
                                                                                <LayoutTemplate>
                                                                                    <table id="lstvwPayFee" width="100%" style="color: #333" cellpadding="3" cellspacing="1"
                                                                                        class="GridBorder">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th id="thchkPay" runat="server" align="center" style="padding: 0;">
                                                                                                <asp:CheckBox ID="chkAll" runat="server" onclick="ChkAllOnClick();" />
                                                                                            </th>
                                                                                            <th align="left" style="padding: 0 0 0 15px;">
                                                                                                Fee Type
                                                                                            </th>
                                                                                            <th align="left" style="padding: 0 0 0 15px;">
                                                                                                Payable For
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Amt. Paid
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Amt. Payable
                                                                                            </th>
                                                                                            <th align="center" class="paddingLR" id="thActualAmount" runat="server">
                                                                                                Actual Amt.
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Late Fee Amount
                                                                                            </th>
                                                                                            <th class="paddingLR" align="center">
                                                                                                Due Date
                                                                                            </th>
                                                                                            <th id="thDelete" runat="server" class="paddingLR" align="center">
                                                                                                Delete
                                                                                            </th>
                                                                                            <th id="thRecipt" runat="server" class="paddingLR" align="center">
                                                                                                Print
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                                        <td align="center" id="tdchkPay" runat="server">
                                                                                            <asp:CheckBox ID="chkPay" runat="server" onclick="ChkPayOnClick(this);" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("FeeType") %>' CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("PayableFor") %>' CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblPaidAmt" runat="server" Text='<%# Eval("PaidAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("PayableAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="center" runat="server" id="tdActualAmount">
                                                                                            <asp:Label ID="lblActualAmount" runat="server" Text="-" CssClass="paddingLR" Visible="false" />
                                                                                            <asp:TextBox ID="txtActualAmount" runat="server" CssClass="SmlTxtBox" Enabled="false"
                                                                                                onblur="extractNumber(this,0,false); ActualAmountOnBlur(this);" onkeyup="extractNumber(this,0,false);"
                                                                                                onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                                                                ondrop="event.returnValue=false;" />
                                                                                            <asp:HiddenField ID="hidConcessionAmount" runat="server" Value='<%# Eval("ConcessionAmount") %>' />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblLateFee" runat="server" Text='<%# Eval("LateFeeAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate","{0:dd-MMM-yyyy}")%>'
                                                                                                CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="center" id="tdDelete" runat="server">
                                                                                            <asp:ImageButton ID="imgbtnDeleteItem" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                                                CommandName="Remove" ToolTip="Delete" />
                                                                                        </td>
                                                                                        <td align="center" id="tdRecipt" runat="server">
                                                                                            <asp:HyperLink ID="lnkMini" runat="server" Text="Receipt" Visible="true" NavigateUrl="FeesMiniReceipt.aspx?" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table cellpadding="2" cellspacing="2" style="width: 100%">
                                                                    <tr id="tr1" runat="server" visible="true" class="GridBorder">
                                                                        <td valign="top">
                                                                            <asp:ListView ID="lstvwPaidDetails" runat="server" DataKeyNames="Std_FeeType_Id,SerialNo,RowNo,Amount,PaidDate,Receipt_Number,NextAcademicYear"
                                                                                OnItemDataBound="lstvwPaidDetails_ItemDataBound" OnDataBound="lstvwPaidDetails_DataBound">
                                                                                <LayoutTemplate>
                                                                                    <table width="100%" style="color: #333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                            <th id="thchkPay" runat="server" align="left" class="ClspaddingL">
                                                                                            </th>
                                                                                            <th align="left" class="ClspaddingL">
                                                                                                Fee Type
                                                                                            </th>
                                                                                            <th align="left" class="ClspaddingL">
                                                                                                Payable For
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Amt. Paid
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Amt. Payable
                                                                                            </th>
                                                                                            <th align="right" class="paddingLR">
                                                                                                Late Fee Amount
                                                                                            </th>
                                                                                            <th class="paddingLR" align="center">
                                                                                                Due Date
                                                                                            </th>
                                                                                            <th id="thRecipt" runat="server" class="paddingLR" align="center">
                                                                                                Print
                                                                                            </th>
                                                                                        </tr>
                                                                                        <tr id="itemPlaceholder" runat="server">
                                                                                        </tr>
                                                                                    </table>
                                                                                </LayoutTemplate>
                                                                                <ItemTemplate>
                                                                                    <tr class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
                                                                                        <td align="left" class="ClspaddingL" id="tdchkPay" runat="server">
                                                                                            <asp:CheckBox ID="chkPay" runat="server" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("FeeType") %>' CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("PayableFor") %>' CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblPaidAmt" runat="server" Text='<%# Eval("PaidAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("PayableAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblLateFee" runat="server" Text='<%# Eval("LateFeeAmount") %>' CssClass="paddingLR" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate","{0:dd-MMM-yyyy}")%>'
                                                                                                CssClass="ClspaddingL" />
                                                                                        </td>
                                                                                        <td align="center" id="tdRecipt" runat="server">
                                                                                            <asp:HyperLink ID="lnkMini" runat="server" Text="Receipt" Visible="true" NavigateUrl="FeesMiniReceipt.aspx?" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </ItemTemplate>
                                                                            </asp:ListView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ContentTemplate>
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2" ChildrenAsTriggers="false">
                                                <ContentTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;" id="tblPaymentDetail"
                                                        runat="server">
                                                        <tr>
                                                            <td align="left" class="ClsBtmBorderGray" colspan="4">
                                                                <span class="ClsLblLgnd" style="font-weight: bold">Details :</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" class="CPanelSpace" colspan="4">
                                                            </td>
                                                        </tr>
                                                        <tr height="30px">
                                                            <td align="left">
                                                                <asp:Label ID="lblPaymentMode" class="ClsLblLgnd" runat="server" Style="font-weight: bold"
                                                                    Text="Payment Mode :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px; width: 25%" colspan="3">
                                                                <asp:RadioButton ID="optCheque" runat="server" GroupName="PaymentMode" Text="Cheque"
                                                                    AutoPostBack="true" TabIndex="1" OnCheckedChanged="optCheque_CheckedChanged" />
                                                                <asp:RadioButton ID="optCash" runat="server" GroupName="PaymentMode" Text="Cash"
                                                                    AutoPostBack="true" TabIndex="2" OnCheckedChanged="optCash_CheckedChanged" />
                                                                <asp:RadioButton ID="optCard" runat="server" GroupName="PaymentMode" Text="Swipe Card"
                                                                    AutoPostBack="true" TabIndex="3" OnCheckedChanged="optCard_CheckedChanged" />
                                                                <asp:RadioButton ID="optElectronic" runat="server" GroupName="PaymentMode" Text="Electronic (NEFT/RTGS)"
                                                                    AutoPostBack="true" TabIndex="4" OnCheckedChanged="optElectronic_CheckedChanged"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight">
                                                                <asp:Label ID="lblChequeNumber" class="ClsLabel" runat="server" Text="Cheque Number :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px;">
                                                                <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                                                    TabIndex="4" Width="120px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers(this, event, false, false);" onpaste="event.returnValue=false;"
                                                                    ondrop="event.returnValue=false;" />
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblAmountPayable" class="ClsLabel" runat="server" Text="Amount Payable :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtAmountPayable" runat="server" Enabled="false" CssClass="SmlTxtBox" TabIndex="6"
                                                                    Width="120px" Style="text-align: right; padding-right: 5px" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight">
                                                                <asp:Label ID="lblChequeDate" class="ClsLabel" runat="server" Text="Cheque Date :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px;">
                                                                <asp:TextBox ID="txtChequeDate" runat="server" CssClass="SmlTxtBox" AutoPostBack="True"
                                                                    TabIndex="7" />
                                                                <rjs:PopCalendar ID="cal_ChequeDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank." />
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="Label1" class="ClsLabel" runat="server" Text="Concession Amount :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtConcessionAmount" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                                                    TabIndex="8" Width="120px" Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,0,false);SetAmountForTextBox()"
                                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" onchange="SetFeeRemarks()"/>
                                                            </td>   
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight">
                                                                <asp:Label ID="lblTxnNumber" class="ClsLabel" runat="server" Text="Txn Number :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px;">
                                                                <asp:TextBox ID="txtSwapNumber" runat="server" CssClass="SmlTxtBox" MaxLength="16"
                                                                    TabIndex="9" Width="120px" />
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblLateFeeAmount" class="ClsLabel" runat="server" Text="Late Fee Amount :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtLateFeeAmount" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                                                    TabIndex="8" Width="120px" Style="text-align: right; padding-right: 5px" onblur="extractNumber(this,0,false);SetAmountForTextBox()"
                                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers(this, event, false, false);"
                                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" onchange="SetFeeRemarks()"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight">
                                                                <asp:Label ID="lblCardType" class="ClsLabel" runat="server" Text="Card Type :"></asp:Label>
                                                                <asp:Label ID="lblType" class="ClsLabel" runat="server" Text="Type :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px;">
                                                                <asp:DropDownList ID="ddlCardType" runat="server" CssClass="LrgCombo" TabIndex="11" />
                                                                <asp:DropDownList ID="cmbElectronicTypes" runat="server" CssClass="LrgCombo" TabIndex="12">
                                                                </asp:DropDownList>
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblTotalAmount" class="ClsLabel" runat="server" Text="Total Amount :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtTotalAmount" runat="server" Enabled="false" CssClass="SmlTxtBox" TabIndex="10"
                                                                    Width="120px" Style="text-align: right; padding-right: 5px" />
                                                            </td>                                                            
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblBankName" class="ClsLabel" runat="server" Text="Bank Name :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="padding-right: 10px">
                                                                <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo" TabIndex="14" />
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblActualAmount" class="ClsLabel" runat="server" Text="Actual Amount :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtActualAmount" runat="server" Enabled="false" CssClass="SmlTxtBox"
                                                                    Width="120px" Style="text-align: right; padding-right: 5px" TabIndex="13" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                            </td>
                                                            <td align="right" class="ClsBorderlight" style="width: 15%;">
                                                                <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="Paid Date :" EnableViewState="False" />
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" style="width: 25%">
                                                                <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox" AutoPostBack="True"
                                                                    ReadOnly="true" TabIndex="15" />
                                                                <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                                                    To-Today="true" ShowErrorMessage="false" ShowWeekend="True" InvalidDateMessage="Date should not be blank." />
                                                                <span class="ClsMdtStar" style="color: red">*</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right" class="ClsBorderlight">
                                                                <asp:Label ID="lblRemarks" class="ClsLabel" runat="server" Text="Remarks :"></asp:Label>
                                                            </td>
                                                            <td align="left" class="ClsTextNormal" colspan="3">
                                                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="SmlTxtBox" MaxLength="50" TabIndex="16"
                                                                    Width="98%" TextMode="MultiLine" Columns="2" Enabled="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="optCheque" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="optCash" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="optCard" EventName="CheckedChanged" />
                                                    <asp:AsyncPostBackTrigger ControlID="optElectronic" EventName="CheckedChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <asp:HiddenField ID="hidStudentId" runat="server" />
                                            <asp:HiddenField ID="hidStudentIdQurStr" runat="server" />
                                            <asp:HiddenField ID="hidPaymentDate" runat="server" />
                                            <asp:HiddenField ID="hidMaxdate" runat="server" />
                                            <asp:HiddenField ID="hidStudentRegNo" runat="server" />
                                            <asp:HiddenField ID="hidAcadmicYear" runat="server" />
                                            <asp:HiddenField ID="hidRowCount" runat="server" />
                                            <asp:HiddenField ID="hidStdDivId" runat="server" />
                                            <asp:HiddenField ID="hidServerDate" runat="server" />
                                            <asp:HiddenField ID="hidTotalAmt" runat="server" />
                                            <asp:HiddenField ID="hidQueryString" runat="server" />
                                            <asp:HiddenField ID="hidLateFeeRemark" runat="server" />
                                            <asp:HiddenField ID="hidLateFeeAmt" runat="server" />
                                            <asp:HiddenField ID="hidActualAmt" runat="server" />
                                            <asp:HiddenField ID="hidIsFInalYear" runat="server" />
                                            <asp:HiddenField ID="hidIsPaidForNextYear" runat="server" />
                                            <asp:HiddenField ID="hidTotalActualAmount" runat="server" />
                                            <asp:HiddenField ID="hidRestrictMultipleFees" runat="server" Value="0" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr id="trConcesionMessage" runat="server" visible="false">
                    <td align="center">
                        <asp:Image ImageUrl="~/RITeSchool/images/newLink.gif" runat="server" ID="Image1" />
                        <asp:Label ID="lblConcessionMessage" runat="server" Text="" CssClass="ClsLabel" style="font-weight:bold;color:maroon;float:inherit;"></asp:Label>
                        <div style="height:5px;">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table id="tblButton" runat="server">
                            <tr>
                                <td>
                                    <asp:Button ID="btnPay" runat="server" Text="Pay" CssClass="ClsBtnMid" ValidationGroup="Pay"
                                        OnClick="btnPay_Click" TabIndex="17" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPayPrint" runat="server" Text="Pay And Print" CssClass="ClsBtnMid"
                                        ValidationGroup="Pay" TabIndex="18" OnClick="btnPayPrint_Click" />
                                </td>
                                <td>
                                    <asp:Button ID="btnClose" Text="close" CssClass="ClsBtnMid" runat="server" CausesValidation="false"
                                        OnClick="btnClose_Click" TabIndex="19" />
                                </td>
                                <td>
                                    <asp:Button ID="btnPayOnline" runat="server" Text="Pay Online" CssClass="ClsBtnMid" TabIndex="20"
                                        Visible="false" OnClick="btnPayOnline_Click" />
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
                                    ErrorMessage="Cheque Number should not be blank." ValidationGroup="Pay" />
                                <asp:CustomValidator ID="cstValChequeDate" runat="server" CssClass="ClsMdtStar" Display="None"
                                    EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateChequeDate"
                                    ValidationGroup="Pay" />
                                <asp:CustomValidator ID="cstCardNumber" runat="server" ClientValidationFunction="ValidateSwapNo"
                                    Display="none" EnableClientScript="true" ErrorMessage="Txn Number should not be blank."
                                    ValidationGroup="Pay" />
                                <asp:CustomValidator ID="cstCardType" runat="server" ClientValidationFunction="ValidateCardType"
                                    Display="none" EnableClientScript="true" ErrorMessage="Card Type should be selected."
                                    ValidationGroup="Pay" />
                                <asp:CustomValidator ID="cstBankName" runat="server" CssClass="ClsMdtStar" Display="None"
                                    EnableClientScript="true" Visible="true" ClientValidationFunction="ValidateBankName"
                                    ErrorMessage="Bank Name should be selected." ValidationGroup="Pay" />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateConcessionAmount"
                                    Display="none" EnableClientScript="true" ValidationGroup="Pay" />
                                <asp:CustomValidator ID="cstActualAmountValidator" runat="server" ClientValidationFunction="ValidateActualAmount"
                                    Display="none" EnableClientScript="true" ValidationGroup="Pay" />                                
                            </td>
                        </tr>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPay" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="btnPayPrint" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="btnPayOnline" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="btnClose" EventName="click" />
            <asp:AsyncPostBackTrigger ControlID="lstvwPayFee" EventName="ItemCommand" />
        </Triggers>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        _clientcstBankNameID = "<%=this.cstBankName.ClientID %>";
        _clientcstChequeNoID = "<%=this.cstChequeNo.ClientID %>";
        _clientcalStartDateID = "<%=this.txtChequeDate.ClientID %>";
        _clienttxtBankNameID = "<%=this.ddlBankName.ClientID %>";
        _clienttxtChequeNumberID = "<%=this.txtChequeNumber.ClientID %>";
        _clienttxtRemarksID = "<%=this.txtRemarks.ClientID %>";
        _clientoptChequeID = "<%=this.optCheque.ClientID %>";
        _clientoptCardID = "<%=this.optCard.ClientID %>";
        _clientddlCardType = "<%=this.ddlCardType.ClientID %>";
        _clienttxtSwapNumber = "<%=this.txtSwapNumber.ClientID %>";
        _clienttxtDateID = "<%=this.txtDate.ClientID %>";
        _clienthidPaymentDateId = "<%=this.hidPaymentDate.ClientID %>";
        _clientlblDateId = "<%=this.lblDate.ClientID %>";
        _clientListViewId = "<%=this.lstvwPayFee.ClientID %>";
        _clientlblErrMsgId = "<%=this.lblErrMsg.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _clientbtnPay = "<%=this.btnPay.ClientID %>";
        _clienthidRowCount = "<%=this.hidRowCount.ClientID %>";
        _clienthidMaxdate = "<%=this.hidMaxdate.ClientID %>";
        _clientvalChequeData = "<%=this.valChequeData.ClientID %>";
        _txtAmountPayable = "<%= this.txtAmountPayable.ClientID %>";
        _txtTotalAmount = "<%= this.txtTotalAmount.ClientID %>";
        _txtActualAmount = "<%= this.txtActualAmount.ClientID %>";
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _lbltxtRemarks = "<%=this.txtRemarks.ClientID %>";
        _txtLateFeeAmount = "<%=this.txtLateFeeAmount.ClientID %>";
        _clienthidTotalAmt = "<%=this.hidTotalAmt.ClientID %>";
        _clienttrlblErrMsg = "<%=this.trlblErrMsg.ClientID %>";
        _sClientbtnOnlinePayment = "<%=this.btnPayOnline.ClientID %>";
        _hidLateFeeRemark = "<%=this.hidLateFeeRemark.ClientID %>";
        _hidLateFeeAmt = "<%=this.hidLateFeeAmt.ClientID %>";
        _hidActualAmt = "<%=this.hidActualAmt.ClientID %>";
        _hidTotalActualAmount = "<%=this.hidTotalActualAmount.ClientID %>";
        _clientlstvwPaidDetails = "<%=this.lstvwPaidDetails.ClientID %>";
        _clientbtnPay = "#<%=this.btnPay.ClientID %>";
        _clientbtnPayPrint = "#<%=this.btnPayPrint.ClientID %>";
        _clientbtnPayOnline = "#<%=this.btnPayOnline.ClientID %>";
        _clientoptElectronic = "<%=this.optElectronic.ClientID %>";
        _clientcmbElectronicTypes = "<%=this.cmbElectronicTypes.ClientID %>";
        _clientlblUpdateMessage = "<%=this.lblUpdateMessage.ClientID %>";
        _clientoptCash = "<%=this.optCash.ClientID %>";
        _txtConcessionAmount = "<%= this.txtConcessionAmount.ClientID %>";

        // Setup begin and end page request handler so we can disable buttons accordingly.
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndReqHandler);
        prm.add_beginRequest(BeginReqHandler);

        function BeginReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement != null) {
                $(_clientbtnPay + ',' + _clientbtnClose + ',' + _clientbtnPayPrint + ',' + _clientbtnPayOnline)
				.attr('disabled', 'disabled');
            }
        }

        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement;
            if (postBackElement != null) {
                $(_clientbtnPay + ',' + _clientbtnClose + ',' + _clientbtnPayPrint + ',' + _clientbtnPayOnline)
				.removeAttr('disabled');
            }
        }

        // Click handler for each checkbox of the fee types grid.
        function ChkPayOnClick(src) {
            var row = $(src).closest('tr').get(0);
            var txtActualAmount = $('input[id*=txtActualAmount]', row).get(0);
            if (txtActualAmount != null && txtActualAmount) {
                var lblAmount = $('[id*=lblAmount]', row).get(0);
                var amount = lblAmount ? lblAmount.innerHTML : 0;
                if (src.checked)
                    $(txtActualAmount).val(amount).removeAttr('disabled');
                else
                    $(txtActualAmount).val('').attr('disabled', 'disabled');
            }

            SetTotalAmount(false);
            SetConcessionAmount(src);

            var chkAll = $('input[id*=chkAll]', $('#lstvwPayFee')).get(0);
            if (chkAll)
                chkAll.checked = AllChecked();
        }

        function SetConcessionAmount(src) {
            var row = $(src).closest('tr').get(0);          
            var ConcessionAmount = $('input[id*=hidConcessionAmount]', row).get(0);
            if (ConcessionAmount.value != 0) {
                var Amount = 0
                Amount = $get(_txtConcessionAmount).value;
                if (src.checked) {
                    if (Amount != "") {
                        var TotalConcession = parseInt(Amount) + parseInt(ConcessionAmount.value);
                        $('#' + _txtConcessionAmount).val(TotalConcession)
                    }
                    else {
                        $('#' + _txtConcessionAmount).val(ConcessionAmount.value)
                    }
                }
                else {
                    var Concession = parseInt(Amount) - parseInt(ConcessionAmount.value);
                    $('#' + _txtConcessionAmount).val(Concession);
                }
                SetConcessionRemark();
            }
        }

        // Click handler for the common checkbox of fee types grid.
        function ChkAllOnClick() {
            var chkPayItems = $(AllChecked() ? 'input[id*=chkPay]' : 'input[id*=chkPay]:not(:checked)', $('#lstvwPayFee'));
            if (chkPayItems)
                chkPayItems.each(function () { this.checked = !this.checked; ChkPayOnClick(this); });
        }

        // Returns true if all the checkboxes in the listview are checked; false otherwise.
        function AllChecked() {
            return $('input[id*=chkPay]:not(:checked)', $('#lstvwPayFee')).length <= 0;
        }

        // Custom validation functions.

        function ResetValidationSummary() {
            if ($get(_clientvalChequeData) != null)
                $get(_clientvalChequeData).innerHTML = "";
            ResetErrorLabel();
            return true;
        }

        function ResetErrorLabel() {
            if ($get(_clientlblErrMsgId) != null)
                $get(_clientlblErrMsgId).innerHTML = "";

            if ($get(_clientlblUpdateMessage) != null)
                $get(_clientlblUpdateMessage).innerHTML = "";
            return true;
        }

        function ValidateChequeNumber(aSrc, args) {
            ResetErrorLabel();
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked && $get(_clienttxtChequeNumberID).value == "")
                args.IsValid = false;
            return !args.IsValid;
        }

        function ValidateChequeDate(src, args) {
            ResetErrorLabel();
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked) {
                var dtPaymentDate = $get(_clientcalStartDateID);
                dtPaymentDate.value = dtPaymentDate.value.trim();
                if (dtPaymentDate.value == "") {
                    src.errormessage = "Cheque Date should not be blank.";
                    args.IsValid = false;
                }
                else {
                    if (!validateDate(dtPaymentDate.value)) {
                        src.errormessage = "Cheque Date should be in valid format.";
                        args.IsValid = false;
                    }
                }
            }
            return !args.IsValid;
        }

        //This is used to validate date format.
        function validateDate(txtIntervalStart) {
            var isValid = true;
            if (document.all) {
                if (isNaN(new Date(convertdate(txtIntervalStart).replace('-', ' '))))
                    isValid = false;
            }
            else {
                if (isNaN(new Date(convertdate(txtIntervalStart).replace(/-/g, ' '))))
                    isValid = false;
            }
            return isValid;
        }

        function ValidateBankName(aSrc, args) {
            ResetErrorLabel();
            args.IsValid = true;
            if (($get(_clientoptChequeID).checked || $get(_clientoptCardID).checked || $get(_clientoptElectronic).checked) && $get(_clienttxtBankNameID).value == "0")
                args.IsValid = false;
            return !args.IsValid;
        }

        function ValidateCardType(source, args) {
            ResetErrorLabel();
            args.IsValid = true;
            if ($get(_clientoptCardID).checked && $get(_clientddlCardType).value == "0") {
                args.IsValid = false;
                source.errormessage = "Card Type should be selected.";
            }
            else if ($get(_clientoptElectronic).checked && $get(_clientcmbElectronicTypes).value == "0") {
                args.IsValid = false;
                source.errormessage = "Type should be selected.";
            }
            return !args.IsValid;
        }

        function ValidateSwapNo(source, args) {
            ResetErrorLabel();
            args.IsValid = true;
            if (($get(_clientoptCardID).checked || $get(_clientoptElectronic).checked) && $get(_clienttxtSwapNumber).value.trim() == "")
                args.IsValid = false;
            return !args.IsValid;
        }

        function SelectedCount() {
            ResetErrorLabel();
            Page_IsValid = true;
            var n = $('input[id*=chkPay]:checked', $('#lstvwPayFee')).length;
            if (n == 0) {
                alert("At least one fee entry should be selected for paying fee.")
                Page_IsValid = false;
                return false;
            }            
            return true;
        }

        function ValidateActualAmount(src, args) {
            ResetErrorLabel();
            args.IsValid = true;
            var row, lblFeeType, lblPayableFor, lblAmount, txtActualAmount;
            var amount = 0, actualAmount = -1;
            var feetypes = [];
            $('input[id*=chkPay]:checked', $get(_clientListViewId))
			.each(function () {
			    row = $(this).closest('tr').get(0);
			    lblAmount = $('[id*=lblAmount]', row).get(0);
			    txtActualAmount = $('[id*=txtActualAmount]', row).get(0);

			    amount = actualAmount = 0;

			    if (lblAmount)
			        amount = parseInt($(lblAmount).text());
			    if (txtActualAmount && txtActualAmount.value != '')
			        actualAmount = parseInt(txtActualAmount.value);

			    if (actualAmount <= 0 || actualAmount > amount) {
			        lblFeeType = $('[id*=lblFeeType]', row).get(0);
			        lblPayableFor = $('[id*=lblPaybleFor]', row).get(0);

			        feetypes.push(lblFeeType.innerHTML + '(' + lblPayableFor.innerHTML + ')');
			    }
			});

            if (feetypes.length > 0) {
                ResetErrorLabel();
                args.IsValid = false;
                src.errormessage = 'Actual Amount should not be empty, zero or greater than Amt. Payable for: ' + feetypes.join(', ') + '.';
            }

            return !args.IsValid;
        }

        function fnover(varname) {
            var objTXT = $get(varname);
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "maroon";
            objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
        }

        function fnout(varname) {
            var objTXT = $get(varname);
            objTXT.style.borderWidth = "1";
            objTXT.style.borderColor = "#a3c07b";
            objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
        }

        // Checks if at least one fee is selected for payment. Alerts the user if not 
        function CheckAtleastOneCheckBoxForNextYear(sSourceClientId) {

        var restrict = $('#'+'<%=this.hidRestrictMultipleFees.ClientID %>').val()
        var ln = $('input[id*=chkPay]:checked', $get(sSourceClientId)).length
        if (ln <= 0) {
            alert("Please fix following error(s):\n\r\n\rAt least one fee should be selected for pay.");
            return false;
        }
        else if (restrict == true && ln > 1) {
            alert('Please fix following error(s):\n\r\n\rPlease select only one fee type for online payment.')           
            return false;
        }
            return true;
        }

        function SetTotalAmount(IsFromLateFee) {            
            // Declare some required variables
            var totalAmount = 0;
            var totalLateFeeAmount = 0;
            var totalActualAmount = 0;
            var remarks = '';
            var latefeeRemarks = '';
            
            // Some required variables for the loop;
            var row, lblAmount, amount, lblLateFee, latefee, txtActualAmount, actualamount, lblPayableFor;
            var lblfeeType;

            // Find and loop over all checked checkboxes in the listview.
            $('input[id*=chkPay]:checked', $get(_clientListViewId))
			.each(function () {
			    row = $(this).closest('tr').get(0);
			    lblAmount = $('[id*=lblAmount]', row).get(0);
			    lblLateFee = $('[id*=lblLateFee]', row).get(0);
			    txtActualAmount = $('[id*=txtActualAmount]', row).get(0);
			    lblPayableFor = $('[id*=lblPaybleFor]', row).get(0);
			    lblfeeType = $('[id*=lblFeeType]', row).get(0);

			    amount = latefee = actualamount = 0;

			    if (lblAmount)
			        amount = parseInt($(lblAmount).text());
			    if (lblLateFee)
			        latefee = parseInt($(lblLateFee).text());
			    if (txtActualAmount != null && txtActualAmount && txtActualAmount.value != '')
			        actualamount = parseInt(txtActualAmount.value);
			    if (lblPayableFor)
			        lblPayableFor = $(lblPayableFor).text();

			    totalAmount += amount;
			    totalLateFeeAmount += latefee;
			    totalActualAmount += actualamount;

			    if (latefee > 0)
			        latefeeRemarks += lblPayableFor + ', ';
			    
			    if (actualamount > 0 && actualamount <= amount)
			        lblPayableFor = lblPayableFor + ' (' + $(lblfeeType).text() + ' - ' + ' Rs.' + actualamount + '/-)';

			    if (remarks.indexOf(lblPayableFor + ', ') == -1)
			        remarks += lblPayableFor + ', ';
			});

			var concessionAmount = $get(_txtConcessionAmount).value
			if (concessionAmount.trim() != "")
			    concessionAmount = parseInt(concessionAmount)
			else
			    concessionAmount = 0

            // Sets values to hidden fields.
			$get(_hidActualAmt).value = totalAmount;			
			$get(_hidTotalActualAmount).value =totalActualAmount;
            if(!IsFromLateFee)
                $get(_hidLateFeeAmt).value = totalLateFeeAmount;
            $get(_clienthidTotalAmt).value = totalAmount + totalLateFeeAmount;
            // Update labels.

            if ($get(_clientoptCardID) != null && $get(_clientoptChequeID) != null && $get(_clientoptElectronic) != null && $get(_clientoptCash) != null) {
                $get(_txtAmountPayable).value = totalAmount
                $get(_txtTotalAmount).value = totalAmount + totalLateFeeAmount - concessionAmount;
                $get(_txtActualAmount).value = totalActualAmount + totalLateFeeAmount - concessionAmount;
                if(!IsFromLateFee)
                    $get(_txtLateFeeAmount).value = totalLateFeeAmount;

                if (remarks != '') {
                    var finalRemark = "Amount paid for " + remarks.substring(0, remarks.length - 2)

                    if (concessionAmount > 0)
                        finalRemark = finalRemark + " with Concession Fee (Concession Fee - Rs. " + concessionAmount + "/-) "

                    $get(_lbltxtRemarks).value = finalRemark;
                }
                else
                    $get(_lbltxtRemarks).value = '';
            }

            if(!IsFromLateFee)
                SetLateFeeRemarks();
        }
        
        function SetFeeRemarks() {
            SetTotalAmount(true);
            SetLateFeeRemarks();
        }

        function SetLateFeeRemarks() {            
            // Declare some required variables                        
            var TotalLateAmount = 0;                        
            var actualLateFeeTypes = '';
            var allfeetypes = '';
            var IsLateFeeApplicable = false;

            // Some required variables for the loop;
            var row, lblAmount, lblLateFee, latefee, lblPayableFor;
            var lblfeeType;

            // Find and loop over all checked checkboxes in the listview.
            $('input[id*=chkPay]:checked', $get(_clientListViewId))
			.each(function () {
			    
			    row = $(this).closest('tr').get(0);
			    lblLateFee = $('[id*=lblLateFee]', row).get(0);
			    lblPayableFor = $('[id*=lblPaybleFor]', row).get(0);

			    latefee =  0;

			    if (lblLateFee)
			        latefee = parseInt($(lblLateFee).text());
			    if (lblPayableFor) {
			        if (latefee > 0)
                    {
                        if (actualLateFeeTypes == "")
                            actualLateFeeTypes = $(lblPayableFor).text();
                        else
                            actualLateFeeTypes = actualLateFeeTypes+', '+ $(lblPayableFor).text();

                            IsLateFeeApplicable = true;
                    }
                    
                    if (allfeetypes == "")
                        allfeetypes = $(lblPayableFor).text();
                    else
                        allfeetypes = allfeetypes + ', ' + $(lblPayableFor).text();                    
			    }
			});

            TotalLateAmount = $get(_txtLateFeeAmount).value;
            if (IsLateFeeApplicable && parseInt(TotalLateAmount) > 0) 
                $get(_lbltxtRemarks).value += " and Late fee for " + actualLateFeeTypes + ' (Rs.' + TotalLateAmount + '/-)';
            
            if (!IsLateFeeApplicable && parseInt(TotalLateAmount) > 0) 
                $get(_lbltxtRemarks).value += " and Late fee for " + allfeetypes + ' (Rs.' + TotalLateAmount + '/-)';
        }        

        function SetAmountForTextBox() {
            var totalAmount = 0;
            var txtTotalAmount = $get(_txtTotalAmount);
            if (txtTotalAmount)
                totalAmount = parseInt(txtTotalAmount.value);

            if (totalAmount > 0) {
                var actualAmount = 0;
                var lateFeeAmount = 0;
                var concessionAmount = 0
                var txtLateFee = $get(_txtLateFeeAmount);
                var txtConcessionFee = $get(_txtConcessionAmount)
                if (($get(_txtLateFeeAmount).value).trim() == "")
                    $get(_txtLateFeeAmount).value = "0";                
                if (txtLateFee)
                    lateFeeAmount = parseInt(RemoveLeadingZeroes($get(_txtLateFeeAmount).value));
                
                if (txtConcessionFee) {
                    if (txtConcessionFee.value.trim() == "")
                        txtConcessionFee.value = "0"
                    concessionAmount = parseInt(txtConcessionFee.value);
                }
                if (lateFeeAmount >= 0) {
                    totalAmount = 0;
                    if ($get(_hidActualAmt).value != '')
                        totalAmount = parseInt(RemoveLeadingZeroes($get(_hidActualAmt).value));
                        
                    $get(_txtTotalAmount).value = totalAmount + lateFeeAmount - concessionAmount;
                    
                    //if ($get(_txtActualAmount).value != '')
                        //actualAmount = parseInt($get(_txtActualAmount).value);
                    if ($get(_hidTotalActualAmount).value != '')
                        actualAmount = parseInt(RemoveLeadingZeroes($get(_hidTotalActualAmount).value));

                    $get(_txtActualAmount).value = actualAmount + lateFeeAmount - concessionAmount;
                }
                else {
                    totalAmount = 0;
                    $get(_hidLateFeeRemark).value = "";

                    if ($get(_clienthidTotalAmt).value != '')
                        totalAmount = parseInt(RemoveLeadingZeroes($get(_clienthidTotalAmt).value));

                    $get(_txtTotalAmount).value = (totalAmount - concessionAmount)
                    var ActAmount = GetActualAmount();
                    $get(_txtActualAmount).value = (ActAmount - concessionAmount)                    
                }
            }
                }

        function ActualAmountOnBlur(src) {
            ResetErrorLabel();
            if (src.value.trim() != '') {
                var row = $(src).closest('tr').get(0);
                var lblAmount = $('[id*=lblAmount]', row).get(0);
                var amount = lblAmount ? parseInt($(lblAmount).text()) : 0;
                var actualAmount = parseInt(src.value.trim());

                if (actualAmount > amount) {
                    alert('Actual Amount should not be greater than Amount Payable.');
                    src.value = amount;
                }

                SetTotalAmount(false);
            }
        }

        function GetActualAmount() {
            var totalActualAmount = 0, actualAmount;
            $('input[id*=chkPay]:checked', $get(_clientListViewId))
			.each(function () {
			    row = $(this).closest('tr').get(0);
			    txtActualAmount = $('[id*=txtActualAmount]', row).get(0);
			    actualAmount = 0;

			    if (txtActualAmount && txtActualAmount.value != '')
			        actualAmount = parseInt(txtActualAmount.value);

			    totalActualAmount += actualAmount;
			});
            return totalActualAmount;
        }

        function SetConcessionRemark() {
            var lateFee = $get(_txtLateFeeAmount).value
            if (lateFee.trim() != "" && parseInt(lateFee) > 0)
                SetTotalAmount(false)
            else
                SetTotalAmount(true)
        }

        function ValidateConcessionAmount(oSrc, args) {
            var payableAmount = $('#' + _txtAmountPayable).val();
            var concessionAmount = $('#' + _txtConcessionAmount).val();
            if (concessionAmount != "" && parseInt(concessionAmount) > 0 && parseInt(concessionAmount) > parseInt(payableAmount)) {
                oSrc.errormessage = "Concession Amount should be less than Payable Amount.";
                args.IsValid = false;
                return true
            }

            args.IsValid = true
            return false
        }

        function ConfirmDelete() {            
            return window.confirm('Are you sure you want to delete this fee details?');
        }
    </script>
</asp:Content>
