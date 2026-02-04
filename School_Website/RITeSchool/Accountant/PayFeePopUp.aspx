<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PayFeePopUp.aspx.cs" Inherits="PayFeePopUp"
    MasterPageFile="../MasterPages/PopupMaster.master" ViewStateMode="Disabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td style="height: 19px" align="left" colspan="6" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
                                        <span class="MainTitleHead" style="font-weight: bold">Fee Payment</span>
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
                                runat="server" ViewStateMode="Enabled"/>
                            <asp:CustomValidator ID="cstPaymentDate" runat="server" ViewStateMode="Enabled" Display="none" EnableClientScript="true"
                                ClientValidationFunction="ValidatePaymentDate" ErrorMessage="Payment date should not be blank."></asp:CustomValidator>                           
                            <asp:CustomValidator ID="cstAcDateValidator" runat="server" ViewStateMode="Enabled" Display="None" EnableClientScript="true"
                                ClientValidationFunction="AccountsValidateDate" />
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
                                Visible="False"></asp:Label>
                            <asp:CustomValidator ID="cstValCautionMoneyAdjst" runat="server" ViewStateMode="Enabled" Display="none" EnableClientScript="true" Enabled="false"
                                ClientValidationFunction="ValidateCautionMoneyAmount" ErrorMessage="If need to adjust from caution money then payable amount should not be greater than caution money amount."></asp:CustomValidator>
                            <asp:CustomValidator ID="CustFileUpload" runat="server" ClientValidationFunction="ValidateFileType" ViewStateMode="Enabled"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
                            <asp:CustomValidator ID="CustFileSize" runat="server" ClientValidationFunction="ValidateFileSize" ViewStateMode="Enabled"
                                CssClass="clsLabel" Display="None" ErrorMessage=""></asp:CustomValidator>
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
                            <tr>
                                <td align="left">
                                    <span class="ClsLblLgnd" style="font-weight: bold">Payment Mode :</span>
                                </td>
                            </tr>
                            <tr id="tblChequeGrid" runat="server" viewstatemode="Enabled" visible="false">
                                <td align="left">
                                    <asp:RadioButtonList ID="chkFeePayment" runat="server" ViewStateMode="Enabled" RepeatDirection="Horizontal" AutoPostBack="true"
                                        OnSelectedIndexChanged="chkFeePayment_SelectedIndexChanged" Onclick="EnableOrDisableControls()">
                                        <%--<asp:ListItem Text="Cash" Value="Cash" ></asp:ListItem>
                                        <asp:ListItem Text="Cheque" Value="Cheque" ></asp:ListItem>
                                        <asp:ListItem Text="PDC" Value="PDC"></asp:ListItem>
                                        <asp:ListItem Text="Swipe Card" Value="SwapCard"></asp:ListItem>
                                        <asp:ListItem Text="Electronic (NEFT/RTGS/IMPS)" Value="Electronic" ></asp:ListItem>
                                        <asp:ListItem Text="Journal Voucher" Value="JournalVoucher" ></asp:ListItem>--%>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr id="tdChequeGrid" runat="server" viewstatemode="Enabled" style="width: 100%; height: 100%">
                                <td align="left" class="ClsBorderlight">
                                    <asp:GridView ID="grdPostDatedCheque" AutoGenerateColumns="False" runat="server" ViewStateMode="Enabled"
                                        PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333" GridLines="None"
                                        DataKeyNames="Postdated_Cheque_Id,BankId,Status" Width="100%" BackColor="White" EmptyDataText="Cheques are not available."
                                        CssClass="GridBorder" onrowdatabound="grdPostDatedCheque_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <input id="ChkAll" type="checkbox" runat="server" viewstatemode="Enabled" onclick="CheckAllAndCalculateAmt()" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="ChkBoxPay" runat="server" ViewStateMode="Enabled" Onclick="CalculateActualAmt()" />
                                                </ItemTemplate>
                                                <ItemStyle Width="1%" HorizontalAlign="left" CssClass="paddingLSML" />
                                                <HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Cheque No." DataField="Cheque_Number" SortExpression="Cheque_Number">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Cheque Date" DataField="Cheque_Date" HtmlEncode="False">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderText="Cheque Amount" SortExpression="Cheque_Amount" DataField="Cheque_Amount">
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                                            </asp:BoundField>
                                        </Columns>
                                        <RowStyle CssClass="ClsGridAltRow" />
                                        <HeaderStyle CssClass="ClsGridHeader" />
                                        <AlternatingRowStyle CssClass="ClsGridRow" />
                                        <EmptyDataRowStyle CssClass="LblNoRecord" HorizontalAlign="Center" VerticalAlign="Middle" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr runat="server" id="tblFeesToBePaid" viewstatemode="Enabled" visible="false">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="center" class="ClsBorderlight">
                                                <asp:Label ID="lblPaidHeader" runat="server" Font-Size="12pt" Font-Bold="true" CssClass="ClsLabel"
                                                    Text="Fees to be paid" EnableViewState="false"></asp:Label>
                                            </td>
                                        </tr>                                     
                                    </table>
                                </td>
                            </tr>
                            <tr runat="server" id="aaa">
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwStudentFee" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwStudentFee_ItemDataBound"
                                                    DataKeyNames="SchoolwiseStudentFeeId,DebitOrCredit,SerialNumber,ReceiptNumberOutput,StandardwiseFeeTypeId,ConcessionAmount,AccountHeaderId">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" viewstatemode="Enabled" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                                                <th id="thchk" runat="server" align="center" width="3%">                                                                    
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" ViewStateMode="Enabled" onclick="CheckAll(this);" AutoPostBack="false" />
                                                                </th>
                                                                <th id="thFeeType" runat="server" align="left" width="15%" style="padding-left: 5px;
                                                                    font-weight: inherit">
                                                                    Fee Type
                                                                </th>
                                                                <th id="thPaybleFor" runat="server" align="left" width="18%" style="padding-left: 5px;
                                                                    font-weight: inherit">
                                                                    Payble For
                                                                </th>
                                                                <th id="thAmount" runat="server" align="right" width="8%" style="padding-right: 5px;
                                                                    font-weight: inherit">
                                                                    Amount
                                                                </th>
                                                                <th id="thDueDate" runat="server" align="center" width="16%" style="font-weight: inherit;
                                                                    white-space: nowrap">
                                                                    Due Date
                                                                </th>
                                                                <th id="thAmountPayable" runat="server" align="right" width="12%" style="padding-right: 5px;
                                                                    font-weight: inherit; white-space: nowrap">
                                                                    Amt. Payable
                                                                </th>                                                               
                                                                <th id="thLateFee" runat="server" align="center" width="8%" style="font-weight: inherit;
                                                                    white-space: nowrap">
                                                                    Late Fee
                                                                </th>
                                                                <th id="thActualAmount" runat="server" align="center" width="12%" style="font-weight: inherit;
                                                                    white-space: nowrap">
                                                                    Actual Amount
                                                                </th>
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trlstvwRow" runat="server" viewstatemode="Enabled" class="ClsMarksGridAltRowN">
                                                            <td id="tdchk" runat="server" align="center">                                                               
                                                                <asp:CheckBox ID="chkSelect" runat="server" ViewStateMode="Enabled" AutoPostBack="false"/>                                                                
                                                                <asp:HiddenField ID="hidStudentFeeId" runat="server" ViewStateMode="Enabled" Value='<%#Eval("SchoolwiseStudentFeeId") %>' />
                                                                <asp:HiddenField ID="hidConcessionAmount" runat="server" ViewStateMode="Enabled" Value='<%#Eval("ConcessionAmount") %>' />
                                                            </td>
                                                            <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblFeeType" runat="server" ViewStateMode="Enabled" Text='<%# Eval("FeeType") %>' />
                                                                <asp:DropDownList ID="cmbFeeType" runat="server" ViewStateMode="Enabled" CssClass="MidCombo" Visible="false"
                                                                    Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="cmbFeeType_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtNewFeeType" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" Visible="false"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblPaybleFor" runat="server" ViewStateMode="Enabled" Text='<%# Eval("PayableFor") %>' />
                                                                <asp:DropDownList ID="cmbPayableFor" runat="server" ViewStateMode="Enabled" Visible="false" CssClass="MidCombo"
                                                                    Enabled="false">
                                                                </asp:DropDownList>
                                                                <asp:TextBox ID="txtNewPayableFor" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" Visible="false"
                                                                    Enabled="false"></asp:TextBox>
                                                            </td>
                                                            <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblAmount" runat="server" viewstatemode="Enabled" Text='<%# Eval("Amount") %>' />
                                                            </td>
                                                            <td id="tdDueDate" runat="server" align="center">                                                               
                                                                <asp:Label ID="lblDueDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("PaymentDate","{0:dd-MMM-yyyy}") %>' />
                                                                <asp:TextBox ID="txtDueDate" runat="server" ViewStateMode="Enabled" Width="80px" CssClass="MidCombo" MaxLength="11"
                                                                    AutoPostBack="false" Visible="false"></asp:TextBox>
                                                                <rjs:PopCalendar ID="calDueDate" runat="server" ViewStateMode="Enabled" Control="txtDueDate" Format="dd MMM yyyy" Culture="en"
                                                                    Visible="false" ShowWeekend="true" ShowErrorMessage="false" />
                                                                <rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer3" runat="server" ViewStateMode="Enabled"
                                                                    Calendar="PopCalendar3" Visible="false" />
                                                            </td>
                                                            <td id="tdAmountPayable" runat="server" viewstatemode="Enabled" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblAmountPayable" runat="server" ViewStateMode="Enabled" Text='<%# Eval("AmountPayable") %>' />
                                                            </td>                                                          
                                                            <td id="tdLateFee" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblLateFee" runat="server" ViewStateMode="Enabled" Text='<%# Eval("LateFeeAmount") %>' />
                                                            </td>
                                                            <td id="tdActualAmount" runat="server" align="center">                                                               
                                                                <asp:TextBox ID="txtActualAmount" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="MidTxtBox" AutoPostBack="false" 
                                                                    Width="70px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                                <asp:HiddenField ID="hidPreviousActualAmt" runat="server" ViewStateMode="Enabled"/>
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
                                <td align="center">
                                    <table width="100%" id="tblFeeDetails" runat="server">
                                        <tr id="trReceiptNo" runat="server" viewstatemode="Enabled">
                                            <td align="left
                                            " valign="top" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Reciept No. :</span>
                                            </td>
                                            <td align="left" style="width: 95%">
                                                <asp:DropDownList ID="cmbReceiptNo" runat="server" ViewStateMode="Enabled" CssClass="MidCombo">
                                                </asp:DropDownList>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left
                                            " valign="top" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Payment Date :</span>
                                            </td>
                                            <td align="left" style="width: 95%">
                                                <asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" ViewStateMode="Enabled" AutoPostBack="True"
                                                    TabIndex="1" OnTextChanged="txtPaymentDate_TextChanged"  onblur="CheckUncheckCheckboxAsperDate()"></asp:TextBox>
                                                <rjs:PopCalendar ID="cal_PaymentDate" runat="server" ViewStateMode="Enabled" Control="txtPaymentDate" Format="dd MMM yyyy" Culture="en"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                                    OnSelectionChanged="cal_PaymentDate_SelectionChanged" AutoPostBack="True" To-Today="true" />
                                                <span class="ClsMdtStar">* </span>
                                            </td>
                                        </tr>
                                        <tr>
										 <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;
                                                                                padding: 3px;width:17%" >
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
                                                <asp:TextBox ID="txtPayableAmt" TabIndex="2" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" Enabled="False"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Late Fee Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">                                          
                                                <asp:TextBox ID="txtLateFeeAmt" TabIndex="3" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="SmlTxtBox"                                                
                                                    onblur="extractNumber(this,0,false);CalculateTotalAmtToBePaid();"  AutoPostBack="false" 
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
                                                <asp:Label ID="lblDistribution" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Concession Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">                                              
                                                <asp:TextBox ID="txtConcessionAmt" TabIndex="4" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);CalculateTotalAmtToBePaid();" AutoPostBack="false" 
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
                                                <asp:CustomValidator ID="cstValidateTotalFee" runat="server" ViewStateMode="Enabled" Display="none" EnableClientScript="true"
                                                    ClientValidationFunction="ValidateConcessionAmt" ErrorMessage="Concession amount should not be greater than amount to be paid."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Amount to be paid :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:TextBox ID="txtAmtToBePaid" TabIndex="5" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="SmlTxtBox"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" class="ClsBorderlight" style="height: 23px; width: 24%;">
                                                <span class="ClsLabel">Actual Amount :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="height: 23px">
                                                <asp:TextBox ID="txtActualAmt" TabIndex="6" runat="server" ViewStateMode="Enabled" MaxLength="6" onblur="extractNumber(this,0,false);VisibleOrHideControls();"
                                                    onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);" 
                                                    onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" CssClass="SmlTxtBox"></asp:TextBox>&nbsp;
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstActualAmt" runat="server" ViewStateMode="Enabled" Display="none" EnableClientScript="true"
                                                    ClientValidationFunction="ValidateActualAmt" ErrorMessage="Actual amount should not be blank."></asp:CustomValidator>
                                                <asp:CustomValidator ID="cstBankNameDirectlyPaid" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateBankNameDirectlyPaid"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trPDCBank" runat="server" viewstatemode="Enabled">
                                            <td align="left" class="ClsBorderlight" style="height: 23px; width: 24%;">
                                                <span class="ClsLabel">Deposit in Bank :</span>
                                            </td>
                                            <td align="left" style="height: 23px">
                                                <asp:DropDownList ID="ddlAcPDCBank" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="7">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                         <tr runat="server" viewstatemode="Enabled" id="trSMS">
                                        	
                                            <td align="left" colspan="2">
                                                <asp:CheckBox ID="chkPaymentSMSAcknowledgement" runat="server" viewstatemode="Enabled" 
                                                    Text="Send Fee Payment Acknowledgement SMS?" 
                                                    />
                                            </td>
                                        </tr>
                                        <tr runat="server" viewstatemode="Enabled" id="trCaution">
                                            <td align="left" colspan="2">
                                                <asp:CheckBox ID="ChkPaymentCautionMoneyAdjusted" runat="server" ViewStateMode="Enabled"
                                                 Text="Adjuste Payment From Caution Money?" />
                                            </td>
                                        </tr>
										<tr runat="server" viewstatemode="Enabled" id="trSMSNote">
										 <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;
                                                                                padding: 3px;width:17%" >
                                                                                <span class="LblNrmlB" style="font-weight: bold; height: 16px;">Note :</span>
                                                                            </td>
                                                                            <td  align="left" class="ClsBorderlight"  style="padding: 3px; width:80%" >
                                                                                <div id="div" style="font-family: Verdana; font-size: 8pt; border:100%;">
                                                                                     If selected payment date is other than current date then check box will get unchecked.<span> <%if (!Settings.IsMiniSite) %><%{ %> To send fee payment acknowledgement SMS to student, you have to again select the check box.<%} %></span></div></td>
										</tr>
                                        <tr runat="server" viewstatemode="Enabled" id="trDirectlyPaid">
                                            <td align="left" colspan="2">
                                                <asp:CheckBox ID="chkDirectlyPaid" runat="server" ViewStateMode="Enabled" Text="Cash directly paid in bank?" />
                                            </td>
                                        </tr>
                                        <tr runat="server" viewstatemode="Enabled" id="trChallanNoRow" style="width: 100%">
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Challan No. :</span>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtChallanNo" ViewStateMode="Enabled" runat="server" CssClass="SmlTxtBox" MaxLength="15"
                                                    TabIndex="8" Width="190px" />
                                                <asp:RegularExpressionValidator ID="regexpChallanValidator" runat="server" ViewStateMode="Enabled" Display="None"
                                                    ControlToValidate="txtChallanNo" ErrorMessage="Challan No. can contain only alphabets, numbers and /, \ and - characters."
                                                    CssClass="ClsMdtStar" ValidationExpression="^[a-zA-Z0-9\\\/\-]{0,15}$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr runat="server" viewstatemode="Enabled" id="trBankName" style="width: 100%">
                                            <td align="left" class="ClsBorderlight" style="width: 24%">
                                                <asp:Label ID="lblBankName" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" Text="Bank Name :"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="ddlBankNameDirectlyPaid" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                    TabIndex="9">
                                                </asp:DropDownList>
                                                <asp:Label ID="lblBankMandatory" runat="server" CssClass="ClsMdtStar" Text=" *" EnableViewState="false" />
                                            </td>
                                        </tr>
                                        <tr id="trRemarks" runat="server">
                                            <td align="left" class="ClsBorderlight" style="width: 24%; height: 40px;">
                                                <span class="ClsLabel">Remarks :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="height: 40px">
                                                <asp:TextBox ID="txtRemarks" TabIndex="10" runat="server" viewstatemode="Enabled" MaxLength="100" CssClass="SmlTxtBox"
                                                    ReadOnly="true" Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="cst_Remarks" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtRemarks"
                                                    ErrorMessage="Length of remarks should not exceed 1000 characters." CssClass="ClsMdtStar"
                                                    ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td align="left" class="ClsBorderlight" style="width: 24%; height: 40px;">
                                                <span class="ClsLabel">Additional Remark :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar" style="height: 40px">
                                                <asp:TextBox ID="txtAdditionalRemark" TabIndex="11" runat="server" viewstatemode="Enabled" MaxLength="1000" CssClass="SmlTxtBox"
                                                    Width="100%" TextMode="MultiLine" Height="50px"></asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtAdditionalRemark"
                                                    ErrorMessage="Length of Additional Remark should not exceed 1000 characters." CssClass="ClsMdtStar"
                                                    ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr id="trAttachment" runat="server">
                                            <td align="left" class="ClsBorderlight">
                                                <span class="ClsLabel">Attachment :</span>
                                            </td>
                                            <td align="left" class="ClsMdtStar">
                                                <asp:FileUpload ID="flAttachment" runat="server" ViewStateMode="Enabled" />
                                                <asp:ImageButton ID="imgbtnView" runat="server" CausesValidation="false" CommandName="UpdateUploadedFile" Visible="false"
                                                    ToolTip="Update" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
                                                <span class="LblSmlGray">(Attachment supports files of types - .JPG, .JPEG, .BMP, .PNG, .PDF). File size should not exceed 5 MB.</span>
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
                            <tr id="trChequeEntry" runat="server" viewstatemode="Enabled" style="width: 100%">
                                <td style="background-color: white; width: 100%" id="Td1" align="center" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr style="width: 100%">
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%;">
                                                <span class="ClsLabel">Cheque Number :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="width: 95%">
                                                <asp:TextBox ID="txtChequeNumber" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="6"
                                                    onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                    ondrop="event.returnValue=false;" TabIndex="11"></asp:TextBox>&nbsp; <span class="ClsMdtStar">
                                                        * </span>
                                                <asp:CustomValidator ID="cstChequeNumber" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateChequeNo"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Cheque Number should not be blank."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel">Cheque Date :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" ViewStateMode="Enabled" AutoPostBack="True"
                                                    TabIndex="12"></asp:TextBox>
                                                <rjs:PopCalendar ID="cal_CDate" runat="server" ViewStateMode="Enabled" Control="txtDate" Format="dd MMM yyyy" Culture="en"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank." />
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstChequeDate" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateChequeDate"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Cheque date should not be blank."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Bank Name :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:DropDownList ID="ddlBankName" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="13">
                                                </asp:DropDownList>
                                                &nbsp; <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstBankName" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateBankName"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <% if (IsAccountsModuleEnabled)
                                           { %>
                                        <tr id="trAcChqBank" runat="server" style="width: 100%">
                                            <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Deposit in Bank :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:DropDownList ID="ddlAcChqBank" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="14">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <% } %>
                                        <tr>
                                            <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Remarks :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:TextBox ID="txtChequeRemarks" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="50"
                                                    TabIndex="15" Width="400px"></asp:TextBox>&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right" valign="top" style="width: 24%">
                                                <asp:Image ID="Image1" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
                                                    Width="148px" />
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trCardEntry" runat="server" viewstatemode="Enabled" style="width: 100%">
                                <td style="background-color: white; width: 100%" id="Td2" align="center" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr style="width: 100%">
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel" style="width: 100%">Txn Number :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:TextBox ID="txtSwapNumber" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" MaxLength="25"
                                                    TabIndex="16"></asp:TextBox>&nbsp; <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstCardNumber" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateSwapNo"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Txn number should not be blank."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trCardType" runat="server" viewstatemode="Enabled">
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%; height: 9px;">
                                                <span class="ClsLabel">Card Type :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="height: 9px">
                                                <asp:DropDownList ID="ddlCardType" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="17">
                                                </asp:DropDownList>
                                                &nbsp; <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstCardType" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateCardType"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Card type should be selected."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr id="trElectronicTypes" runat="server" viewstatemode="Enabled">
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%; height: 9px;">
                                                <span class="ClsLabel">Type :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal" style="height: 9px">
                                                <asp:DropDownList ID="cmbElectronicTypes" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="17">
                                                </asp:DropDownList>
                                                &nbsp; <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstElectronicTypes" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateElectronicType"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Type should be selected."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <tr style="width: 100%">
                                            <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Bank Name :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:DropDownList ID="ddlBankNameCard" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="18">
                                                </asp:DropDownList>
                                                &nbsp; <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="cstBankNameCard" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateBankNameCard"
                                                    Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
                                            </td>
                                        </tr>
                                        <% if (IsAccountsModuleEnabled)
                                           { %>
                                        <tr id="trAcCardBank" runat="server" style="width: 100%" visible='<%# IsAccountsModuleEnabled %>'>
                                            <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                                <span class="ClsLabel">Deposit in Bank :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:DropDownList ID="ddlAcCardBank" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="19">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <% } %>
                                    </table>
                                </td>
                            </tr>       
                            <tr id="trJVEntry" runat="server" viewstatemode="Enabled">
                                <td align="left">
                                    <table width="100%">
                                        <tr>
                                            <td class="ClsBorderLight">
                                                 <span class="ClsLabel">From Ledger :</span>
                                            </td>
                                            <td align="left">
                                                <asp:DropDownList ID="cmbJVLedgers" runat="server" CssClass="LrgCombo" ViewStateMode="Enabled">
                                                </asp:DropDownList>
                                                <span class="ClsMdtStar">* </span>
                                                <asp:CustomValidator ID="CustomValidator2" runat="server" 
                                                    ErrorMessage="From Ledger should be selected." Display="None" 
                                                    ClientValidationFunction="ValidateLegers" ViewStateMode="Enabled"></asp:CustomValidator>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>                     
                            <tr>
                                <td align="center">
                                    <asp:HiddenField ID="hidYearEndDate" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidYearStartDate" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidServerDate" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidStudentFeeIds" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidStudentId" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidPaymentType" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidPDCId" runat="server" ViewStateMode="Enabled" />
                                    <asp:HiddenField ID="hidRemarks" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidLateFeeDesc" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidAmtToBePaid" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidActualLateFeeAmt" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidLateFeeDistribution" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidTotalAmount" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidPostBackElementId" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidFinancialYearJSON" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidStudentName" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidQueryString" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidPreviousLateFee" runat="server" ViewStateMode="Enabled" Value=""/>   
                                    <asp:HiddenField ID="hidTotalActualAmt" runat="server" ViewStateMode="Enabled" Value="0" />              
                                    <asp:HiddenField ID="hidReceiptNumber" runat="server" ViewStateMode="Enabled" Value="0" />
                                    <asp:HiddenField ID="hidPaymentMode" runat="server" ViewStateMode="Enabled" Value="0" />   
                                    <asp:HiddenField ID="hidPostbackControl" runat="server" ViewStateMode="Enabled"/>                                 
                                    <asp:HiddenField ID="hidPDCAmount" runat="server" ViewStateMode="Enabled" Value="0" /> 
                                    <asp:HiddenField ID="hidPDCActualAmount" runat="server" ViewStateMode="Enabled" Value="0" />
                                    <asp:HiddenField ID="hidApplicableMaxLateFee" runat="server" ViewStateMode="Enabled" Value="0" />
                                    <asp:HiddenField ID="hidDefaultFeeType" runat="server" ViewStateMode="Enabled" Value="Cheque" />                                    
                                    <asp:HiddenField ID="hidDefaultBank" runat="server" ViewStateMode="Enabled" Value="0" />                                    
                                    <asp:HiddenField ID="hidMode" runat="server" ViewStateMode="Enabled"/>
                                    <asp:HiddenField ID="hidLastChequeBank" runat="server" ViewStateMode="Enabled" Value="0" />  
                                    <asp:HiddenField ID="hidAccountHeaderId" runat="server" ViewStateMode="Enabled" Value="0" />  
                                     <asp:HiddenField ID="hidParticularFeeRestriction" runat="server" ViewStateMode="Enabled" Value="0" />  
                                    <asp:HiddenField ID="hidAllowPartialFee" runat="server" ViewStateMode="Enabled" Value="N" /> 
                                    <asp:HiddenField ID="hidBaseFinancialYearId" runat="server" ViewStateMode="Enabled" Value="0" /> 
                                    <asp:HiddenField ID="hidCautionMoney" runat="server" ViewStateMode="Enabled" />
                                    <asp:HiddenField ID="hidRemaingCautionMoneyAmount" runat="server" ViewStateMode="Enabled" Value="0" /> 
                                    <asp:HiddenField ID = "hidShowLimitedAccess" runat="server" ViewStateMode="Enabled" Value="N" />
                                    <asp:HiddenField ID = "hidFileUpload" runat="server" ViewStateMode="Enabled" Value="" />                                    
                               </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="uFeepnl" runat="server">
                    <ContentTemplate>
                        <table style="width: 100%;" runat="server" viewstatemode="Enabled" id="trNote" visible="false">
                            <tr>
                                <td align="left" colspan="1" class="ClsBorderlight " style="width: 19%; background-color: #ffffc4;">
                                    <span class="LblNrmlB">Note 1 :</span>
                                </td>
                                <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; width: 60%">
                                    <asp:Label ID="lblVerifyNote" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr id="trConcesionMessage" runat="server" viewstatemode="Enabled" visible="false">
            <td align="center">
                <asp:Image ImageUrl="~/RITeSchool/images/newLink.gif" runat="server" ID="Image2" />
                <asp:Label ID="lblConcessionMessage" runat="server" ViewStateMode="Enabled" Text="" CssClass="ClsLabel" style="font-weight:bold;color:maroon;float:inherit;"></asp:Label>                
                <div style="height:5px;">
                </div>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Button ID="btnPay" Text="Pay" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" OnClick="btnPay_Click" disable-page="true"
                    TabIndex="27" UseSubmitBehavior="true" />
                <asp:Button ID="btnPayAndPrint" Text="Pay and Print" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" disable-page="true"
                    TabIndex="28" UseSubmitBehavior="true" OnClick="btnPay_Click" />
                <asp:Button ID="btnClose" Text="Close" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" OnClick="btnClose_Click"
                    CausesValidation="False" TabIndex="29" UseSubmitBehavior="false" />

                 <asp:CustomValidator ID="cstvalPDC" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidatePDCAmount"
                 Display="none" EnableClientScript="true" ErrorMessage="PDC should be selected."></asp:CustomValidator>
                  <asp:CustomValidator ID="cstValidateExtraFee" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateFeeType"
                 Display="none" EnableClientScript="true" ErrorMessage="Fee Type should be selected for selected extra fee."></asp:CustomValidator>
                 <asp:CustomValidator ID="cstValidatePayble" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidatePayableFor"
                 Display="none" EnableClientScript="true" ErrorMessage="Payble For should be selected for selected extra fee."></asp:CustomValidator>
                 <asp:CustomValidator ID="cstValidatePaidDate" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateDate"
                 Display="none" EnableClientScript="true" ErrorMessage="Payble For should be selected for selected extra fee."></asp:CustomValidator>
                 <asp:CustomValidator ID="cstNewActualAmt" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateNewActualAmt"
                 Display="none" EnableClientScript="true" ErrorMessage="Payble For should be selected for selected extra fee."></asp:CustomValidator>
                 <asp:CustomValidator ID="cstNewFeeeType" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateNewFeeeType"
                 Display="none" EnableClientScript="true" ErrorMessage="Fee Type should not be blank for selected extra fee."></asp:CustomValidator>
                 <asp:CustomValidator ID="cstNewPayableFor" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateNewPayableFor"
                 Display="none" EnableClientScript="true" ErrorMessage="Payable For should not be blank for selected extra fee."></asp:CustomValidator>
            </td>
        </tr>
    </table>
    <script language="javascript" type="text/javascript">
        _clientIsAccountsModuleEnabled = Boolean.parse('<%= this.IsAccountsModuleEnabled %>');

        _clientbtnPay = "<%=this.btnPay.ClientID %>";
        _clientbtnPayAndPrint = "<%=this.btnPayAndPrint.ClientID %>";
        _clientbtnClose = "<%=this.btnClose.ClientID %>";
        _clientYearStartDate = "<%=this.hidYearStartDate.ClientID %>";
        _clientYearEndDate = "<%=this.hidYearEndDate.ClientID %>";
        _sClientchkFeePaymentId = "<%=this.chkFeePayment.ClientID %>";
        _sClientGridId = "<%=this.grdPostDatedCheque.ClientID %>";
        
        _sClienttdChequeGrid = "<%=this.tdChequeGrid.ClientID %>";
        _sClienttxtActualAmt = "<%=this.txtActualAmt.ClientID %>";
        _sClienttxtAmtToBePaid = "<%=this.txtAmtToBePaid.ClientID %>";
        _sClientchkDirectlyPaid = "<%=this.chkDirectlyPaid.ClientID %>";
        _sClientddlBankNameDirectlyPaid = "<%=this.ddlBankNameDirectlyPaid.ClientID %>";
        _sClienttrDirectlyPaid = "<%=this.trDirectlyPaid.ClientID %>";
        _sClienttrBankName = "<%=this.trBankName.ClientID %>";
        _sClientlblBankMandatory = "<%=this.lblBankMandatory.ClientID %>";
        
        _sclienttrPDCBank = '<%=this.trPDCBank.ClientID %>';                        
        _sClienthidPaymentType = "<%=this.hidPaymentType.ClientID %>";
        _sClientcstActualAmt = "<%=this.cstActualAmt.ClientID %>";
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clienttrChequeEntry = "<%=this.trChequeEntry.ClientID %>";
        _clienttrCardEntry = "<%=this.trCardEntry.ClientID %>";
        _clienttrJVEntry = "<%=this.trJVEntry.ClientID %>"
        _clientcstChequeNo = "<%=this.cstChequeNumber.ClientID %>";
        _clienttxtChequeNo = "<%=this.txtChequeNumber.ClientID %>";
        _clienttxtSwapNumber = "<%=this.txtSwapNumber.ClientID %>";
        _clientcstChequeDate = "<%=this.cstChequeDate.ClientID %>";
        _clienttxtDate = "<%=this.txtDate.ClientID %>";
        _clientcstBankName = "<%=this.cstBankName.ClientID %>";
        _clienttxtBankName = "<%=this.ddlBankName.ClientID %>";
        _clientddlBankNameCard = "<%=this.ddlBankNameCard.ClientID %>";
        _clientddlCardType = "<%=this.ddlCardType.ClientID %>";
        _sClienttxtRemarks = "<%=this.txtRemarks.ClientID %>";
        _clienttxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>";
        _sClientcstPaymentDate = "<%=this.cstPaymentDate.ClientID %>";
        _sClientcal_PaymentDate = "<%=this.cal_PaymentDate.ClientID %>";
        _sClienttxtPayableAmt = "<%=this.txtPayableAmt.ClientID %>";
        _sClienttxtLateFeeAmt = "<%=this.txtLateFeeAmt.ClientID %>";
        _sClienttxtConcessionAmt = "<%=this.txtConcessionAmt.ClientID %>";
        _sClientcstValidateTotalFee = "<%=this.cstValidateTotalFee.ClientID %>";
        _sClienthidRemarks = "<%=this.hidRemarks.ClientID %>";
        _sClienthidAmtToBePaid = "<%=this.hidAmtToBePaid.ClientID %>";      
        _clientlblError = "<%=this.lblErrorMsg.ClientID %>";
        _clienthidActualLateFeeAmt = "<%=this.hidActualLateFeeAmt.ClientID %>";
        _clientvalErrMsgId = "<%=this.valErrMsg.ClientID %>";
        _clienttblFeesToBePaidId = "<%=this.tblFeesToBePaid.ClientID %>";
        _clienthidTotalAmount = "<%=this.hidTotalAmount.ClientID %>";
        _clienthidPostBackElementId = "<%=this.hidPostBackElementId.ClientID %>";
        _clienthidPDCId = "<%=this.hidPDCId.ClientID %>";
        _clienttblChequeGrid = "<%=this.tblChequeGrid.ClientID %>";
        _clienttrChallanRow = '<%= this.trChallanNoRow.ClientID %>';
        _clienttxtChallanNo = '<%= this.txtChallanNo.ClientID %>';
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
        _clientlstvwStudentFee = "<%=this.lstvwStudentFee.ClientID %>";
        _clienthidStudentFeeIds = "<%=this.hidStudentFeeIds.ClientID %>";
        _clienthidPreviousLateFee = "<%=this.hidPreviousLateFee.ClientID %>";
        _clienthidTotalActualAmt = "<%=this.hidTotalActualAmt.ClientID %>";
        _clienthidPaymentMode = "<%=this.hidPaymentMode.ClientID %>";
        _clienthidPostbackControl = "<%=this.hidPostbackControl.ClientID %>";
        _clienthidLateFeeDesc = "<%=this.hidLateFeeDesc.ClientID %>";
        _clientcstvalPDC = "<%=this.cstvalPDC.ClientID %>";
        _clienthidPDCAmount = "<%=this.hidPDCAmount.ClientID %>";
        _clienthidPDCActualAmount = "<%=this.hidPDCActualAmount.ClientID %>";
        _clienttrElectronicTypes = "<%=this.trElectronicTypes.ClientID %>";
        _clientcmbElectronicTypes = "<%=this.cmbElectronicTypes.ClientID %>";
        _clienttrCardType = "<%=this.trCardType.ClientID %>";
        _clienthidDefaultFeeType = "<%=this.hidDefaultFeeType.ClientID %>";
        _clienthidMode = "<%=this.hidMode.ClientID %>";
        _clienthidDefaultBank = "<%=this.hidDefaultBank.ClientID %>";
        _clientddlAcChqBank = "<%=this.ddlAcChqBank.ClientID %>";
        _clientddlAcPDCBank = "<%=this.ddlAcPDCBank.ClientID %>";
        _clientddlAcCardBank = "<%=this.ddlAcCardBank.ClientID %>";
        _clienthidLastChequeBank = "<%=this.hidLastChequeBank.ClientID %>";

        _clientcmbJVLedgers = "<%=this.cmbJVLedgers.ClientID %>"
        _clientChkPaymentCautionMoneyAdjusted =  "<%=this.ChkPaymentCautionMoneyAdjusted.ClientID %>"

        function CloseWindow() {
            var sQueryString = document.getElementById(_clienthidQueryString).value;
            window.opener.location = window.opener.location.pathname + sQueryString;
            window.opener.focus();
            window.close();
        }

        // Financial year related
        var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
        var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');
        var _checkedElementId; 
        if ($get(_sClientddlBankNameDirectlyPaid) != null)
            $get(_sClientddlBankNameDirectlyPaid).disabled = true;
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_beginRequest(beginRequestHandler)
        prm.add_endRequest(EndReqHandler);

        function beginRequestHandler(sender, args) {
            DisableListView();            
        }

        function SetDefaultBank() {
            var mode = $get(_clienthidMode).value;
            var defaultbank = $get(_clienthidDefaultBank).value;
            var IsDirectlypaid = $get(_sClientchkDirectlyPaid);            

            if (mode == "Pay" && defaultbank != "" && defaultbank != "0") {
                $get(_sClientddlBankNameDirectlyPaid).value = defaultbank;
            }            
        }

        function SetLastChequeBank() {
            var mode = $get(_clienthidMode).value;            
            var chequeBank = $get(_clienthidLastChequeBank).value;

            if (mode == "Pay" && chequeBank != "0" && chequeBank != "") {
                var optPDC = $get(_sClientchkFeePaymentId + '_2');
                var OptCheque = $get(_sClientchkFeePaymentId + '_1');

                if ((OptCheque != null && OptCheque.checked) || (optPDC != null && optPDC.checked))
                    $get(_clienttxtBankName).value = chequeBank;
            }
        }

        function EndReqHandler(sender, args) {                        
            var hidPostBackElementId = $get(_clienthidPostBackElementId).value;
            
            if (hidPostBackElementId == _clientbtnClose) {
                $get(_clientbtnClose).disabled = true;
                $get(_clientbtnPay).disabled = true;
                $get(_clientbtnPayAndPrint).disabled = true;
            }
            else {                
                if (hidPostBackElementId == _sClientcal_PaymentDate ||
					hidPostBackElementId == _clienttxtPaymentDate ||
                    hidPostBackElementId == _checkedElementId || hidPostBackElementId == $get(_clienthidPostbackControl).value) {

                    var optPDC = $get(_sClientchkFeePaymentId + '_2');
                    var OptCheque = $get(_sClientchkFeePaymentId + '_1');
                    var OptCard = $get(_sClientchkFeePaymentId + '_3');
                    var OptElectronic = $get(_sClientchkFeePaymentId + '_4');
                    var OptJV = $get(_sClientchkFeePaymentId + '_5');

                    if ($get(_sClientchkFeePaymentId + '_2') != null) {
                        var optPDC = $get(_sClientchkFeePaymentId + '_2');
                        var OptCheque = $get(_sClientchkFeePaymentId + '_1');
                        var OptCard = $get(_sClientchkFeePaymentId + '_3');
                        // When PDC payment is selected.
                        if (optPDC!=null && optPDC.checked) {
                            $get(_sClienttdChequeGrid).style.display = '';
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_sClienttxtLateFeeAmt).disabled = true;
                            $get(_sClienthidPaymentType).value = "2";
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = "none";                            
                            $get(_sClienttrDirectlyPaid).style.display = "none";
                            $get(_sClienttrBankName).style.display = "none";
                            $get(_sClientlblBankMandatory).style.display = "none";
                            $get(_clienttrChallanRow).style.display = 'none';
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = '';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";

                            SetLastChequeBank();
                        }
                        // When Cheque payment is selected.
                        else if (OptCheque!=null && OptCheque.checked) {
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_clienttrChequeEntry).style.display = '';
                            $get(_clienttrCardEntry).style.display = "none";                            
                            $get(_sClienthidPaymentType).value = "1";                            
                            $get(_sClienttrDirectlyPaid).style.display = "none";
                            $get(_sClienttrBankName).style.display = "none";
                            $get(_sClientlblBankMandatory).style.display = "none";
                            $get(_clienttrChallanRow).style.display = 'none';
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = 'none';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";
                            DeSelectAllChkBox();
                            SetLastChequeBank();
                        }
                        // When Card payment is selected
                        else if (OptCard != null && OptCard.checked) {
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = '';                            
                            $get(_sClienthidPaymentType).value = "3";                            
                            $get(_sClienttrDirectlyPaid).style.display = "none";
                            $get(_sClienttrBankName).style.display = "none";
                            $get(_sClientlblBankMandatory).style.display = "none";
                            $get(_clienttrChallanRow).style.display = 'none';
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = 'none';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";
                            DeSelectAllChkBox();
                        }
                        // Electronic payment is selected.
                        else if (OptElectronic != null && OptElectronic.checked) {
                            $get(_sClienttdChequeGrid).style.display = "none";
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = '';                            
                            $get(_clienttrCardType).style.display = "none";
                            $get(_sClienthidPaymentType).value = "4";
                            $get(_sClienttrDirectlyPaid).style.display = "none";
                            $get(_sClienttrBankName).style.display = "none";
                            $get(_sClientlblBankMandatory).style.display = "none";
                            $get(_clienttrChallanRow).style.display = 'none';
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = 'none';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = '';

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";
                        }
                        else if (OptJV != null && OptJV.checked) {
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_sClienthidPaymentType).value = "5";
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = "none";

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "";

                            $get(_sClienttrDirectlyPaid).style.display = 'none';
                            $get(_sClienttrBankName).style.width = "100%";
                            $get(_sClienttrBankName).style.display = 'none';
                            $get(_sClientlblBankMandatory).style.display = 'none';
                            $get(_clienttrChallanRow).style.display = 'none';
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = 'none';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";

                            if ($get(_clientChkPaymentCautionMoneyAdjusted) != null) {
                                $get(_clientChkPaymentCautionMoneyAdjusted).disabled = true;
                                $get(_clientChkPaymentCautionMoneyAdjusted).checked = true;
                            }

                            DeSelectAllChkBox();
                        }
                        // When Cash payment is selected.
                        else {
                            DeSelectAllChkBox();

                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_sClienthidPaymentType).value = "0";
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = "none";
                            $get(_sClienttrDirectlyPaid).style.display = '';
                            $get(_sClienttrBankName).style.width = "100%";
                            $get(_sClienttrBankName).style.display = '';
                            $get(_sClientlblBankMandatory).style.display = '';
                            $get(_clienttrChallanRow).style.display = '';

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";
                                
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = 'none';
                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";
                        }
                    }
                    else if (OptCheque == null && OptCard == null && optPDC == null && OptElectronic == null && OptJV == null && parseInt($get(_clienthidPDCId).value) != 0) {
                        
                            $get(_sClienttdChequeGrid).style.display = '';
                            $get(_sClienttxtActualAmt).disabled = true;
                            $get(_sClienthidPaymentType).value = "2";
                            $get(_clienttrChequeEntry).style.display = "none";
                            $get(_clienttrCardEntry).style.display = "none";

                            if ($get(_clienttrJVEntry) != null)
                                $get(_clienttrJVEntry).style.display = "none";

                            if ($get(_clienttrElectronicTypes) != null)
                                $get(_clienttrElectronicTypes).style.display = "none";
                            $get(_sClienttrDirectlyPaid).style.display = "none";                            
                            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                                $get(_sclienttrPDCBank).style.display = '';
                    }
                    VisibleOrHideControls();
                    
                    if (hidPostBackElementId == _checkedElementId || hidPostBackElementId == $get(_clienthidPostbackControl).value)
                        EnableDisableFeeControls();
                    if ($get(_clienttblFeesToBePaidId) != null) {
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        if ($get(_sClienttrBankName) != null)
                            $get(_sClienttrBankName).style.display = "none";
                    }
                    else {
                        EnableControlsDirectlyPaid();
                    }
                }
                EnableListView();
            }            
        }
        
        Onload();

        function Onload() {        
            var sDefaultFeeType = $get(_clienthidDefaultFeeType).value;
            var Mode = $get(_clienthidPaymentMode).value;

            if ($get(_sClienttdChequeGrid) != null) {
                $get(_sClienttdChequeGrid).style.display = "none";
                $get(_clienttrCardEntry).style.display = "none";
                if ($get(_clienttrElectronicTypes)!=null)
                    $get(_clienttrElectronicTypes).style.display = "none";
            }
            
            if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank)) {
                if ($get(_clienttblFeesToBePaidId) != null)
                    $get(_sclienttrPDCBank).style.display = '';
                else
                    $get(_sclienttrPDCBank).style.display = 'none';
            }

            $get(_sClienttxtActualAmt).disabled = true;

            if ($get(_sClientGridId) == null) {
                $get(_clienttrChequeEntry).style.display = "none";
                $get(_clienttrCardEntry).style.display = "none";
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";
            }
            else {
                var optCash = $get(_sClientchkFeePaymentId + '_0');
                var OptCheque = $get(_sClientchkFeePaymentId + '_1');
                var optPDC = $get(_sClientchkFeePaymentId + '_2');
                var OptCard = $get(_sClientchkFeePaymentId + '_3');
                var OptElectronic = $get(_sClientchkFeePaymentId + '_4');
                var OptJV = $get(_sClientchkFeePaymentId + '_5');

                if (Mode == "0") {
                    if (sDefaultFeeType == "Cash" && optCash != null)
                        optCash.checked = true;
                    else if (sDefaultFeeType == "Cheque" && OptCheque != null)
                        OptCheque.checked = true;
                    else if (sDefaultFeeType == "PDC" && optPDC != null)
                        optPDC.checked = true;
                    else if (sDefaultFeeType == "SwapCard" && OptCard != null)
                        OptCard.checked = true;
                    else if (sDefaultFeeType == "Electronic" && OptElectronic != null)
                        OptElectronic.checked = true;
                    else if(sDefaultFeeType == "JournalVoucher" && OptJV != null)
                        OptJV.checked = true;
                }

                if ($get(_sClientchkFeePaymentId + '_1') != null) {                    
                    if (OptCard.checked) {                       
                        OptCard.checked = true;
                        $get(_clienttrChequeEntry).style.display = "none";
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        $get(_sClienttrBankName).style.display = "none";
                        $get(_sClienthidPaymentType).value = "3";
                        $get(_clienttrCardEntry).style.display = '';
                        $get(_clienttrChallanRow).style.display = 'none';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = "none";
                    }
                    else if (OptElectronic.checked) {
                        OptElectronic.checked = true;
                        $get(_sClienttdChequeGrid).style.display = "none";
                        $get(_clienttrChequeEntry).style.display = "none";
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        $get(_sClienttrBankName).style.display = "none";
                        $get(_sClienthidPaymentType).value = "4";
                        $get(_clienttrCardEntry).style.display = '';
                        $get(_clienttrCardType).style.display = "none";
                        $get(_clienttrChallanRow).style.display = 'none';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = '';
                    }
                    else if (OptJV != null && OptJV.checked) {
                        OptJV.checked = true;
                        $get(_sClienttdChequeGrid).style.display = "none";
                        $get(_clienttrChequeEntry).style.display = "none";
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        $get(_sClienttrBankName).style.display = "none";
                        $get(_sClienthidPaymentType).value = "5";
                        $get(_clienttrCardEntry).style.display = 'none';
                        $get(_clienttrCardType).style.display = "none";
                        $get(_clienttrChallanRow).style.display = 'none';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = 'none';
                        if($get(trJVEntry) != null)
                            $get(trJVEntry).style.display = '';
                        if ($get(_clientChkPaymentCautionMoneyAdjusted) != null) {
                            $get(_clientChkPaymentCautionMoneyAdjusted).disabled = true;
                            $get(_clientChkPaymentCautionMoneyAdjusted).checked = true;
                        }
                    }
                    else if (optCash.checked) {
                        optCash.checked = true;
                        $get(_clienttrChequeEntry).style.display = 'none';
                        $get(_sClienttrDirectlyPaid).style.display = '';
                        $get(_sClienthidPaymentType).value = "0";
                        $get(_clienttrCardEntry).style.display = "none";
                        $get(_clienttrChallanRow).style.display = '';
                        $get(_sClienttrBankName).style.width = "100%";
                        $get(_sClienttrBankName).style.display = '';
                        $get(_sClientlblBankMandatory).style.display = '';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = "none";

                        if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                            $get(_sclienttrPDCBank).style.display = 'none';

                        EnableControlsDirectlyPaid();
                    }
                    else if (optPDC != null && optPDC.checked) {
                        $get(_sClienttdChequeGrid).style.display = '';
                        $get(_sClienttxtActualAmt).disabled = true;
                        $get(_sClienttxtLateFeeAmt).disabled = true;
                        $get(_sClienthidPaymentType).value = "2";
                        $get(_clienttrChequeEntry).style.display = "none";
                        $get(_clienttrCardEntry).style.display = "none";
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        $get(_sClienttrBankName).style.display = "none";
                        $get(_sClientlblBankMandatory).style.display = "none";
                        $get(_clienttrChallanRow).style.display = 'none';
                        if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                            $get(_sclienttrPDCBank).style.display = '';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = "none";
                    }
                    else {                        
                        OptCheque.checked = true;
                        $get(_clienttrChequeEntry).style.display = '';
                        $get(_sClienttrDirectlyPaid).style.display = "none";
                        $get(_sClienttrBankName).style.display = "none";
                        $get(_sClienthidPaymentType).value = "1";
                        $get(_clienttrCardEntry).style.display = "none";
                        $get(_clienttrChallanRow).style.display = 'none';
                        if ($get(_clienttrElectronicTypes) != null)
                            $get(_clienttrElectronicTypes).style.display = "none";
                    }
                }
                $get(_sClienttxtAmtToBePaid).disabled = true;
                $get(_sClienttxtActualAmt).disabled = true;
            }

            VisibleOrHideControls();
            EnableOrDisableControls();
            EnableDisableFeeControls();
            SetLastChequeBank();
        }

        function CheckAllAndCalculateAmt() {            
            var iAmt = 0;
            var ChkBox = $get(_sClientGridId + "_ctl01_ChkAll");
            CheckAllOrUncheckAllGridItems(document, _sClientGridId, ChkBox, 'ChkBoxPay', false);
            var iRowCnt = $get(_sClientGridId).rows.length - 1;
            if (ChkBox.checked) {
                for (i = 0; i < iRowCnt; i++) {
                    iAmt = parseInt(iAmt) + parseInt($get(_sClientGridId).rows[(i + 1)].cells[3].innerHTML);
                }
            }
            $get(_sClienttxtActualAmt).value = iAmt;
            VisibleOrHideControls();
        }

        function EnableOrDisableControls() {        
            ResetValidationMessages();
            var optPDC = $get(_sClientchkFeePaymentId + '_2');
            var OptCheque = $get(_sClientchkFeePaymentId + '_1');
            var OptCard = $get(_sClientchkFeePaymentId + '_3');
            var OptElectronic = $get(_sClientchkFeePaymentId + '_4');
            var optJV = $get(_sClientchkFeePaymentId + '_5');

            if (optPDC != null && optPDC.checked) {
                $get(_sClienttdChequeGrid).style.display = '';
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_sClienthidPaymentType).value = "2";
                $get(_clienttrChequeEntry).style.display = "none";
                $get(_clienttrCardEntry).style.display = "none";
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";
                ShowHideControls();

                $get(_sClienttrDirectlyPaid).style.display = "none";
                $get(_sClienttrBankName).style.display = "none";
                $get(_clienttrChallanRow).style.display = "none";
                if ($get(_clientlblError) != null) {
                    $get(_clientlblError).innerHTML = "";
                }
                $get(_sClienttxtLateFeeAmt).disabled = true;
                if ($get(_sClientGridId).rows.length == 1)
                    $get(_sClienttxtConcessionAmt).disabled = true;
                else
                    $get(_sClienttxtConcessionAmt).disabled = false;
                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                    $get(_sclienttrPDCBank).style.display = '';

                $get(_clienttrChallanRow).style.display = "none";

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "none";

                CalculateTotalAmtToBePaid();

            }
            else if (OptCheque != null && OptCheque.checked) {            
                $get(_sClienttxtLateFeeAmt).disabled = false;
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_clienttrChequeEntry).style.display = '';
                $get(_sClienttrDirectlyPaid).style.display = "none";
                $get(_sClienttrBankName).style.display = "none";
                $get(_sClienthidPaymentType).value = "1";
                $get(_clienttrCardEntry).style.display = "none";
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";
                $get(_sClienttxtConcessionAmt).disabled = false;
                $get(_clienttrChallanRow).style.display = "none";
                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                    $get(_sclienttrPDCBank).style.display = "none";

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "none";

                DeSelectAllChkBox();
                ShowHideControls();
            }
            else if (OptCard != null && OptCard.checked) {
                $get(_sClienttxtLateFeeAmt).disabled = false;
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_clienttrChequeEntry).style.display = "none";
                $get(_sClienttrDirectlyPaid).style.display = "none";
                $get(_sClienttrBankName).style.display = "none";
                $get(_sClienthidPaymentType).value = "3";
                $get(_clienttrCardEntry).style.display = '';
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";
                $get(_sClienttxtConcessionAmt).disabled = false;
                $get(_clienttrChallanRow).style.display = "none";
                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                    $get(_sclienttrPDCBank).style.display = "none";

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "none";

                DeSelectAllChkBox();
                ShowHideControls();
            }
            else if (OptElectronic != null && OptElectronic.checked) {
                $get(_sClienttdChequeGrid).style.display = "none";
                $get(_sClienttxtLateFeeAmt).disabled = false;
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_clienttrChequeEntry).style.display = "none";
                $get(_sClienttrDirectlyPaid).style.display = "none";
                $get(_sClienttrBankName).style.display = "none";
                $get(_sClienthidPaymentType).value = "4";
                $get(_clienttrCardEntry).style.display = '';
                $get(_clienttrCardType).style.display = "none";
                $get(_sClienttxtConcessionAmt).disabled = false;
                $get(_clienttrChallanRow).style.display = "none";
                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank))
                    $get(_sclienttrPDCBank).style.display = "none";
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = '';

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "none";
                DeSelectAllChkBox();
                ShowHideControls();
            }
            else if (optJV != null && optJV.checked) {            
                DeSelectAllChkBox();
                EnableControlsDirectlyPaid();
                $get(_sClienttxtLateFeeAmt).disabled = false;
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_sClienthidPaymentType).value = "0";
                $get(_clienttrChequeEntry).style.display = "none";
//                if ($get(_sClienttrDirectlyPaid) != null)
//                    $get(_sClienttrDirectlyPaid).style.display = '';
//                if ($get(_sClienttrBankName) != null) {
//                    $get(_sClienttrBankName).style.width = "100%";
//                    $get(_sClienttrBankName).style.display = '';
//                }
                if ($get(_sClientlblBankMandatory) != null)
                    $get(_sClientlblBankMandatory).style.display = "none";
                if ($get(_sClienttxtConcessionAmt) != null)
                    $get(_sClienttxtConcessionAmt).disabled = false;
                if ($get(_clienttrCardEntry) != null)
                    $get(_clienttrCardEntry).style.display = "none";
//                if ($get(_clienttrChallanRow) != null)
//                    $get(_clienttrChallanRow).style.display = '';
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";

                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank) && OptCheque == null && OptCard == null && OptElectronic == null && optJV == null && optPDC == null && parseInt($get(_clienthidPDCId).value) != 0)
                    $get(_sclienttrPDCBank).style.display = '';
                else
                    $get(_sclienttrPDCBank).style.display = "none";

                if ($get(_sClienttrDirectlyPaid) != null)
                    $get(_sClienttrDirectlyPaid).style.display = "none";

                if ($get(_sClienttrBankName) != null)
                    $get(_sClienttrBankName).style.display = "none";

                if ($get(_clienttrChallanRow) != null)
                    $get(_clienttrChallanRow).style.display = 'none';

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "";
                if ($get(_clientChkPaymentCautionMoneyAdjusted) != null) {
                    $get(_clientChkPaymentCautionMoneyAdjusted).disabled = true;
                    $get(_clientChkPaymentCautionMoneyAdjusted).checked = true;
                }
                ShowHideControls();
            }
            else {
                DeSelectAllChkBox();
                EnableControlsDirectlyPaid();
                $get(_sClienttxtLateFeeAmt).disabled = false;
                $get(_sClienttxtActualAmt).disabled = true;
                $get(_sClienthidPaymentType).value = "0";
                $get(_clienttrChequeEntry).style.display = "none";
                if ($get(_sClienttrDirectlyPaid) != null)
                    $get(_sClienttrDirectlyPaid).style.display = '';
                if ($get(_sClienttrBankName) != null) {
                    $get(_sClienttrBankName).style.width = "100%";
                    $get(_sClienttrBankName).style.display = '';
                }
                if ($get(_sClientlblBankMandatory) != null)
                    $get(_sClientlblBankMandatory).style.display = "none";
                if ($get(_sClienttxtConcessionAmt) != null)
                    $get(_sClienttxtConcessionAmt).disabled = false;
                if ($get(_clienttrCardEntry) != null)
                    $get(_clienttrCardEntry).style.display = "none";
                if ($get(_clienttrChallanRow) != null)
                    $get(_clienttrChallanRow).style.display = '';
                if ($get(_clienttrElectronicTypes) != null)
                    $get(_clienttrElectronicTypes).style.display = "none";

                if ($get(_clienttrJVEntry) != null)
                    $get(_clienttrJVEntry).style.display = "none";

                if (_clientIsAccountsModuleEnabled && $get(_sclienttrPDCBank) && OptCheque == null && OptCard == null && OptElectronic == null && optJV == null && optPDC == null && parseInt($get(_clienthidPDCId).value) != 0)
                    $get(_sclienttrPDCBank).style.display = '';
                else
                    $get(_sclienttrPDCBank).style.display = "none";

                ShowHideControls();
            }
        }

        function ResetValidationMessages() {
            var valSum = $get(_clientvalErrMsgId);
            if (valSum)
                valSum.style.display = 'none';
        }

        function DeSelectAllChkBox() {
            if ($get(_sClientGridId + "_ctl01_ChkAll") != null) {
                var ChkBox = $get(_sClientGridId + "_ctl01_ChkAll");
                ChkBox.checked = false;
                var iRowCnt = $get(_sClientGridId).rows.length - 1;
                for (i = 0; i < iRowCnt; i++) {
                    if (i < 8) {
                        sRow = "_ctl0" + (i + 2) + "_ChkBoxPay";
                    }
                    else {
                        sRow = "_ctl" + (i + 2) + "_ChkBoxPay";
                    }
                    var ChkBox = $get(_sClientGridId + sRow);
                    ChkBox.checked = false;
                }
            }
            $get(_sClienttdChequeGrid).style.display = "none";
        }

        function CalculateActualAmt() {            
            var sRow;
            var i;
            var iRowCnt = $get(_sClientGridId).rows.length - 1;
            var iAmt;
            iAmt = 0;
            for (i = 0; i < iRowCnt; i++) {
                if (i < 8) {
                    sRow = "_ctl0" + (i + 2) + "_ChkBoxPay";
                }
                else {
                    sRow = "_ctl" + (i + 2) + "_ChkBoxPay";
                }
                var ChkBox = $get(_sClientGridId + sRow);
                if (ChkBox.checked) {
                    iAmt = parseInt(iAmt) + parseInt($get(_sClientGridId).rows[(i + 1)].cells[3].innerHTML);
                }
            }
            $get(_sClienttxtActualAmt).value = iAmt;
           // $get(_clienthidPDCActualAmount).value = iAmt;
            VisibleOrHideControls();
        }

        function VisibleOrHideControls() {
            CalculateTotalAmtToBePaid();            
            var PayableAmount = parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value));
            if (parseInt(PayableAmount) != 0) {
                var iAmt = ($get(_sClienttxtActualAmt).value);
                var AmtTobePaid = ($get(_sClienttxtAmtToBePaid).value);
                var DifferenceAmt;
                if (iAmt != AmtTobePaid && iAmt != "" && iAmt != 0 && AmtTobePaid != "") {
                    iAmt = parseInt($get(_sClienttxtActualAmt).value);
                    AmtTobePaid = parseInt($get(_sClienttxtAmtToBePaid).value);
                }
                ShowHideControls();
            }
            ShowHideControls();
        }

        function ValidateActualAmt(source, args) {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');            
            var iAmount = 0;
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && chk.checked && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998" && txtActualAmount != null && txtActualAmount.value != "0") {
                    iAmount =parseInt(iAmount) + parseInt(txtActualAmount.value);
                }
            }

            var bIsValid = true;
            if ($get(_sClienttxtActualAmt).value == "") {
                bIsValid = false;
                $get(_sClientcstActualAmt).errormessage = "Actual amount should not be blank.";
            }
            else if ($get(_sClienttxtActualAmt).value == "0") {
//                if ($get(_sClienttxtConcessionAmt).value == "" || $get(_sClienttxtConcessionAmt).value == 0) {
//                    bIsValid = false;
//                    $get(_sClientcstActualAmt).errormessage = "Actual amount should not be zero.";
//                }
            }
            else if (iAmount == 0) {
                bIsValid = false;
                $get(_sClientcstActualAmt).errormessage = "Actual amount should not be zero for selected fee type(s).";
            }
            else if (parseInt($get(_sClienttxtActualAmt).value) > parseInt($get(_clienthidTotalAmount).value)) {
                bIsValid = false;
                $get(_sClientcstActualAmt).errormessage = "Actual amount should not be greater than total paid amount i.e. ( Rs. " + $get(_clienthidTotalAmount).value + "/-).";
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidatePDCAmount(source, args) {        
            var optPDC = $get(_sClientchkFeePaymentId + '_2');
            var bIsValid = true;
            $get(_clienthidPDCActualAmount).value = "0";
            ActualPDCAmount();
            var ActualAmount = parseInt($get(_clienthidPDCActualAmount).value);

                var iAmt = 0;
                if ($get(_sClientchkFeePaymentId) != null) {
                    if (optPDC != null && optPDC.checked) {
                        var sRow, i;
                        var iRowCnt = $get(_sClientGridId).rows.length - 1;
                        for (i = 0; i < iRowCnt; i++) {
                            if (i < 8) {
                                sRow = "_ctl0" + (i + 2) + "_ChkBoxPay";
                            }
                            else {
                                sRow = "_ctl" + (i + 2) + "_ChkBoxPay";
                            }
                            var ChkBox = $get(_sClientGridId + sRow);
                            if (ChkBox.checked) {
                                iAmt = parseInt(iAmt) + parseInt($get(_sClientGridId).rows[(i + 1)].cells[3].innerHTML);
                                $get(_clienthidPDCAmount).value = iAmt;
                            }
                        }
                    }

                }
                else if ($get(_sClientchkFeePaymentId) == null) {
                    iAmt = parseInt($get(_clienthidPDCAmount).value);                    
                }
                if (iAmt == 0 && (optPDC != null && optPDC.checked)) {
                    document.getElementById(_clientcstvalPDC).errormessage = "At least one cheque should be selected.";
                    args.IsValid = false;
                    return true;
                }
                else if (ActualAmount != iAmt && ((optPDC != null && optPDC.checked) || $get(_sClientchkFeePaymentId) == null)) {
                    document.getElementById(_clientcstvalPDC).errormessage = "Actual amount should be equal to PDC amount.";
                    args.IsValid = false;
                    return true;
                }

            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ActualPDCAmount() {        
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            var IsPDC = 0;
            var optPDC = $get(_sClientchkFeePaymentId + '_2');

            if ((optPDC != null && optPDC.checked) || $get(_sClienthidPaymentType).value == "2")
                IsPDC = 1;

            for (var i = 0; i < listView.rows.length; i++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + i + "_hidStudentFeeId");
                if (chk != null && chk.checked && (hidFeeIds.value == "-9999"  || hidFeeIds.value == "-9998")) {
                    var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtActualAmount");                    
                    $get(_clienthidPDCActualAmount).value = parseInt($get(_clienthidPDCActualAmount).value) + parseInt(txtActualAmount.value);
                }
            }

            $get(_clienthidPDCActualAmount).value = parseInt($get(_clienthidPDCActualAmount).value) + parseInt($get(_sClienttxtActualAmt).value);
        }

        function ValidateChequeNo(source, args) {
            var bIsValid = true;
            if ($get(_clienttrChequeEntry).style.display != "none") {
                if ($get(_clienttxtChequeNo).value == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateSwapNo(source, args) {
            var bIsValid = true;
            if ($get(_clienttrCardEntry).style.display != "none") {
                if ($get(_clienttxtSwapNumber).value.trim() == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateChequeDate(source, args) {
            var bIsValid = true;
            if ($get(_clienttrChequeEntry).style.display != "none") {
                if ($get(_clienttxtDate).value == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateBankName(source, args) {
            var bIsValid = true;
            if ($get(_clienttrChequeEntry).style.display != "none") {
                if ($get(_clienttxtBankName).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateBankNameCard(source, args) {
            var bIsValid = true;
            if ($get(_clienttrCardEntry).style.display != "none") {
                if ($get(_clientddlBankNameCard).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateCardType(source, args) {
            var bIsValid = true;
            if ($get(_clienttrCardEntry).style.display != "none" && $get(_clienttrCardType).style.display != "none") {
                if ($get(_clientddlCardType).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateElectronicType(source, args) {
            var bIsValid = true;
            if ($get(_clienttrElectronicTypes).style.display != "none") {
                if ($get(_clientcmbElectronicTypes).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateBankNameDirectlyPaid(source, args) {
            var bIsValid = true;
            if ($get(_sClienttrBankName) != null) {
                if ($get(_sClienttrBankName).style.display != "none") {
                    if ($get(_sClientchkDirectlyPaid).checked) {
                        if ($get(_sClientddlBankNameDirectlyPaid).value == "0") {
                            bIsValid = false;
                        }
                    }
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ConfirmAction(iPageCount, sActionName) {
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
            }
            if (validationResult == false) {
                return false;
            }
            var bResult = true;
            if ($get(_sClientgrdFeesToBePaid) != null) {
                if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientgrdFeesToBePaid, 'ChkBoxPayFee', sActionName, 'false', iPageCount, 'true')) { }
                else {
                    bResult = false;
                    validationResult = false;
                }
            }
            else {
                var ChkBox = $get(_sClientchkFeePaymentId + '_2');
                if (ChkBox != null) {
                    if (ChkBox.checked) {
                        sActionName = "Atleast one cheque should be selected from grid.";
                        if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'ChkBoxPay', sActionName, 'false', iPageCount, 'true')) { }
                        else {
                            bResult = false;
                            validationResult = false;
                        }
                    }
                }
            }
            if (validationResult == true) {
                $get(_clientbtnClose).disabled = true;
                $get(_clientbtnPay).disabled = true;
                $get(_clientbtnPayAndPrint).disabled = true;
            }
            return bResult;
        }

        function ValidatePaymentDate(source, args) {
            var bIsValid = true;
            var dtPaymentDate = $get(_clienttxtPaymentDate);
            dtPaymentDate.value = dtPaymentDate.value.trim();
            if (dtPaymentDate.value == "") {
                $get(_sClientcstPaymentDate).errormessage = "Payment Date should not be blank.";
                bIsValid = false;
            }
            else if (dtPaymentDate.value != "") {
                var serverDate = $get(_clientServerDate).value;
                dtStartDate = new Date(convertvaliddate2(dtPaymentDate.value));                                
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    $get(_sClientcstPaymentDate).errormessage = "Payment Date should not be future date.";
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CalculateTotalAmtToBePaid() {            
            var PayableAmount = parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value));
            var PreviousLateFee;

            if ($get(_clienthidPreviousLateFee).value == "")
                PreviousLateFee = 0;            
            else
                PreviousLateFee = parseInt($get(_clienthidPreviousLateFee).value);

            var PreviousActualAmount = parseInt($get(_sClienttxtPayableAmt).value);
            if ($get(_sClienttxtConcessionAmt) != null && $get(_sClienttxtConcessionAmt).value != "" && parseInt($get(_sClienttxtConcessionAmt).value) > (PreviousActualAmount + parseInt($get(_sClienttxtLateFeeAmt).value)) && ($get(_clienthidPaymentMode).value) != "1")
                $get(_sClienttxtConcessionAmt).value = "0";

            if (parseInt(PayableAmount) != 0) {
                var TotAmt;
                var ActualAmt = $get(_sClienttxtActualAmt).value;
                if ($get(_sClienttxtLateFeeAmt).value == "") {
                    $get(_sClienttxtLateFeeAmt).value = "0";
                }
                if ($get(_sClienttxtConcessionAmt).value == "") {
                    $get(_sClienttxtConcessionAmt).value = "0";
                }
                TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value)) + parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value)) - parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
                $get(_sClienttxtAmtToBePaid).value = TotAmt;
                $get(_sClienthidAmtToBePaid).value = TotAmt;
                
                var ActualAmount = parseInt($get(_clienthidTotalActualAmt).value) - PreviousLateFee + parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value));
                
                if (ActualAmount > TotAmt)
                    ActualAmount = TotAmt;

                $get(_sClienttxtActualAmt).value = TotAmt;                
                $get(_sClienttxtAmtToBePaid).value = TotAmt;
                $get(_sClienttxtActualAmt).value = ActualAmount;
                
                if (TotAmt < 0 && parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value)) > $get(_sClienttxtAmtToBePaid).value) {
                    $get(_sClientcstValidateTotalFee).errormessage = "Concession amount should not be greater than amount to be paid.";
                    TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value)) + parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value));
                    $get(_sClienttxtAmtToBePaid).value = TotAmt;
                    $get(_sClienthidAmtToBePaid).value = TotAmt;
                    $get(_sClienttxtConcessionAmt).value = "0";                
                    $get(_sClienttxtActualAmt).value = ActualAmt;
                
                }
              
                $get(_sClienthidAmtToBePaid).value = $get(_sClienttxtAmtToBePaid).value;
                if ($get(_sClienttdChequeGrid) == null) {
                    if ($get(_sClientchkFeePaymentId + '_2').checked)
                        $get(_sClienttxtActualAmt).value = ActualAmt;
                }
                else
                    $get(_sClienttxtActualAmt).value = ActualAmount;

                $get(_clienthidTotalActualAmt).value = $get(_sClienttxtActualAmt).value;
                $get(_clienthidPreviousLateFee).value = $get(_sClienttxtLateFeeAmt).value;

                CalculateTotalActualAmount();
                GenerateRemarks();              
            }
        }

        function GenerateLateFeeRemarks() {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            var strRemark = "";
            var strSelectedFees = "";
            var finalRemark = "";
            var PaybleFor = "";
            var FeeType = "";            
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");                
                var lblPaybleFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblPaybleFor");
                var lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");
                if (chk != null && chk.checked && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998" && lblLateFee != null && lblLateFee.innerHTML != "" && lblLateFee.innerHTML != "0") {
                    if (strRemark.match(lblPaybleFor.innerHTML) == null) {
                        strRemark = strRemark + lblPaybleFor.innerHTML;
                        strRemark = strRemark + ", ";
                    }
                }

                if (chk != null && chk.checked && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998") {
                    if (strSelectedFees.match(lblPaybleFor.innerHTML) == null) {
                        strSelectedFees = strSelectedFees + lblPaybleFor.innerHTML;
                        strSelectedFees = strSelectedFees + ", ";
                    }
                }
            }
//            var maxfee = $get("<%=this.hidApplicableMaxLateFee.ClientID %>");

//            if (maxfee!=null && maxfee.value != 0 && parseInt(maxfee.value) < parseInt($get(_sClienttxtLateFeeAmt).value))
//                strRemark = strRemark + " ( Rs. " + maxfee.value + " /-)  ";
//            else
//                strRemark = strRemark + " ( Rs. " + $get(_sClienttxtLateFeeAmt).value + " /-)  ";            

            var index = strRemark.lastIndexOf(",");
            strRemark = strRemark.substring(0, index) + strRemark.substring(index + 1);

            var isLateFee = 0;
            if (strRemark.trim() != "")
                isLateFee = 1;
            else
                isLateFee = 0;

            index = strSelectedFees.lastIndexOf(",");
            strSelectedFees = strSelectedFees.substring(0, index) + strSelectedFees.substring(index + 1);

            if (isLateFee == 1) {
                $get(_clienthidLateFeeDesc).value = strRemark;
                strRemark = strRemark + " ( Rs. " + $get(_sClienttxtLateFeeAmt).value + "/-)  ";
                finalRemark = "& Late fee for " + strRemark;            
            }
            else {
                $get(_clienthidLateFeeDesc).value = strSelectedFees;
                strSelectedFees = strSelectedFees + " ( Rs. " + $get(_sClienttxtLateFeeAmt).value + "/-)  ";
                finalRemark = "& Late fee for " + strSelectedFees;            
            }
            
            $get(_sClienttxtRemarks).value =$get(_sClienttxtRemarks).value + finalRemark;
        }

        function ValidateConcessionAmt() {
            var TotAmt;
            TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value)) +
					 parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value)) -
					 parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
            if ($get(_sClienttxtLateFeeAmt).value == "") {
                $get(_sClienttxtLateFeeAmt).value = "0";
            }
            if ($get(_sClienttxtConcessionAmt).value == "") {
                $get(_sClienttxtConcessionAmt).value = "0";
            }
            if (TotAmt < 0 && parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value)) > $get(_sClienttxtAmtToBePaid).value) {
                $get(_sClientcstValidateTotalFee).errormessage = "Concession amount should not be greater than amount to be paid.";
                TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value)) +
						 parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value));
                $get(_sClienttxtAmtToBePaid).value = TotAmt;
                $get(__sClienthidAmtToBePaid).value = TotAmt;
                $get(_sClienttxtConcessionAmt).value = "0";
            }
        }

        function ShowHideControls() {
            var TotAmt;            
            var AmtTobePaid = 0;
            if ($get(_sClienttxtLateFeeAmt).value == "") {
                $get(_sClienttxtLateFeeAmt).value = "0";
            }
            if ($get(_sClienttxtConcessionAmt).value == "") {
                $get(_sClienttxtConcessionAmt).value = "0";
            }
            if ($get(_sClienttxtPayableAmt).value != "") {
                TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value)) +
						 parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value)) -
						 parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
                $get(_sClienttxtAmtToBePaid).value = TotAmt;
                $get(_sClienthidAmtToBePaid).value = TotAmt;
                AmtTobePaid = ($get(_sClienttxtAmtToBePaid).value);
            }
            else
                AmtTobePaid = 0;
            var iAmt = ($get(_sClienttxtActualAmt).value);
        }        

        function DisableButtons() {
            $get(_clientbtnClose).disabled = true;
            $get(_clientbtnPay).disabled = true;
            $get(_clientbtnPayAndPrint).disabled = true;
        }

        function EnableControlsDirectlyPaid() {
          var optCash = $get(_sClientchkFeePaymentId + '_0');      
           
            if ($get(_sClientchkFeePaymentId) != null) {
                if ($get(_sClientchkDirectlyPaid).checked) {
                    $get(_sClientlblBankMandatory).style.display = '';
                    $get(_sClientddlBankNameDirectlyPaid).disabled = false;
                    if (optCash != null && optCash.checked) {
                        $get(_clienttrChallanRow).style.display = '';
                        $get(_clienttxtChallanNo).disabled = false;
                        SetDefaultBank();
                    }
                }
                else {
                    $get(_sClientlblBankMandatory).style.display = "none";
                    $get(_sClientddlBankNameDirectlyPaid).disabled = true;
                    $get(_sClientddlBankNameDirectlyPaid).value = "0";
                    $get(_clienttxtChallanNo).value = '';
                    $get(_clienttxtChallanNo).disabled = true;
                }
            }
        }

        function AccountsValidateDate(src, args) {
            args.IsValid = true;
            var optCash = $get(_sClientchkFeePaymentId + '_0');
            if (optCash==null || !optCash.checked)
                return !args.IsValid;

            if (!_FinancialYear)
                return !args.IsValid;

            if (_FinancialYear.IsClosed && !_CanEditOldFinancialYear) {
                args.IsValid = false;
                src.errormessage = 'Financial year is closed and you do not have edit access.';
            }
            else {
                var dtFinancialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtFinancialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtPaymentDate = new Date(convertdate($get(_clienttxtPaymentDate).value));
                var chkDirectlyDeposited = $get(_sClientchkDirectlyPaid);

                if ((chkDirectlyDeposited && !chkDirectlyDeposited.checked) && (dtPaymentDate < dtFinancialYearStartDate || dtPaymentDate > dtFinancialYearEndDate)) {
                    args.IsValid = false;
                    src.errormessage = 'Payment date should be within current financial year (i.e. from 1-April-' + dtFinancialYearStartDate.getFullYear() + ' to 31-March-' + dtFinancialYearEndDate.getFullYear() + ').';
                }
            }
            return !args.ISValid;
        }

        function CheckAll(Src) {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            $get(_sClienttxtLateFeeAmt).value = "0";
            $get(_sClienttxtPayableAmt).value = "0";
            $get(_sClienttxtAmtToBePaid).value = "0";
            $get(_sClienttxtActualAmt).value = "0";
            $get(_sClienttxtConcessionAmt).value = "0";            
            for (var i = 0; i < listView.rows.length; i++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    chk.checked = Src.checked;                    
                    CheckSelected(chk, i);
                }
            }
        }

        function CalculateAmounts() {            
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            $get(_sClienttxtLateFeeAmt).value="0";
            $get(_sClienttxtPayableAmt).value="0";
            $get(_sClienttxtAmtToBePaid).value="0";
            $get(_sClienttxtActualAmt).value="0";
            $get(_sClienttxtConcessionAmt).value="0";
            for (var i = 0; i <= listView.rows.length; i++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
                if (chk != null && i < listView.rows.length) {
                    CheckSelected(chk, i);
                    }
                else if (i == listView.rows.length)
                    {
                    var LateFee = $get(_sClienttxtLateFeeAmt).value;
                    var Payable = $get(_sClienttxtPayableAmt).value;
                    var Concession = $get(_sClienttxtConcessionAmt).value;
                    $get(_sClienttxtAmtToBePaid).value = parseInt($get(_sClienttxtAmtToBePaid).value) + parseInt(LateFee) - parseInt(Concession);                    
                    $get(_sClienttxtActualAmt).value = parseInt($get(_sClienttxtActualAmt).value) + parseInt(LateFee) - parseInt(Concession);                    
                    }
                }            
        }

        function CheckSelected(obj, iRowCount) {
            var IsPDC = 0;
            var optPDC = $get(_sClientchkFeePaymentId + '_2');

            if ((optPDC != null && optPDC.checked) || $get(_sClientchkFeePaymentId) == null)
                IsPDC = 1;
            
            var PreviousLateFee,PreviousPayable;
            var PreviousAmtToBePaid, PreviousActualAmt;            
            var StudentFeeId;
            var iCount=0;

            var chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
            _checkedElementId = _clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect";
            var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");

            if (chk != null && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998") {
                var cmbFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbFeeType");
                var txtNewFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewFeeType");
                var cmbPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbPayableFor");
                var txtNewPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewPayableFor");
                var txtDueDate = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtDueDate");
                var calDueDate = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_calDueDate");
                var hidPreviousActualAmt = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidPreviousActualAmt");
                var lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmountPayable");              
                if (lblAmountPayable != null && parseInt(lblAmountPayable.innerHTML) == "0")
                    lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmount");

                var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
                var lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                var hidConcessionAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidConcessionAmount");

                if (lblAmountPayable != null && lblLateFee != null && txtActualAmount!=null) {

                    PreviousLateFee = $get(_sClienttxtLateFeeAmt).value;
                    PreviousPayable = $get(_sClienttxtPayableAmt).value;
                    PreviousAmtToBePaid = $get(_sClienttxtAmtToBePaid).value;
                    PreviousActualAmt = $get(_sClienttxtActualAmt).value;
                    PreviousConcession = $get(_sClienttxtConcessionAmt).value;                    

                    if (PreviousLateFee == "-" || PreviousLateFee == "")
                        PreviousLateFee = 0;
                    if (PreviousPayable == "-" || PreviousPayable == "")
                        PreviousPayable = 0;
                    if (PreviousAmtToBePaid == "-" || PreviousAmtToBePaid == "")
                        PreviousAmtToBePaid = 0;
                    if (PreviousActualAmt == "-" || PreviousActualAmt == "")
                        PreviousActualAmt = 0;
                    if (IsPDC == 1)
                        PreviousLateFee = 0;

                    if (chk.checked) {
                            if(AllowPartialEdit())
                                txtActualAmount.disabled = false;
                            txtActualAmount.value = lblAmountPayable.innerHTML;
                            hidPreviousActualAmt.value = txtActualAmount.value;
                            var lblFeewiseLateFee = parseInt(lblLateFee.innerHTML);
                            if (IsPDC == 1)
                                lblFeewiseLateFee = 0;
                            $get(_sClienttxtPayableAmt).value = parseInt(PreviousPayable) + parseInt(lblAmountPayable.innerHTML);
                            $get(_sClienttxtAmtToBePaid).value = parseInt(PreviousAmtToBePaid) + parseInt(lblAmountPayable.innerHTML) + lblFeewiseLateFee - parseInt(hidConcessionAmount.value);
                            $get(_sClienttxtLateFeeAmt).value = parseInt(PreviousLateFee) + lblFeewiseLateFee;
                            $get(_sClienttxtActualAmt).value = parseInt(PreviousActualAmt) + parseInt(txtActualAmount.value) + lblFeewiseLateFee - parseInt(hidConcessionAmount.value);
                            $get(_sClienttxtConcessionAmt).value = parseInt(PreviousConcession) + parseInt(hidConcessionAmount.value)

                            if (!$get(_clienthidStudentFeeIds).value.match(hidFeeIds.value))
                                $get(_clienthidStudentFeeIds).value = $get(_clienthidStudentFeeIds).value + "," + hidFeeIds.value;
                            //AddConcession();   
                        }

                        if (!chk.checked) {                           
                        var lblFeewiseLateFee = parseInt(lblLateFee.innerHTML);
                        if (IsPDC == 1)
                            lblFeewiseLateFee = 0;

                        if (PreviousPayable != 0)
                        //$get(_sClienttxtPayableAmt).value = parseInt(PreviousPayable) - parseInt(lblAmountPayable.innerHTML);
                            $get(_sClienttxtPayableAmt).value = parseInt($get(_sClienttxtPayableAmt).value) - parseInt(lblAmountPayable.innerHTML);

                        if (PreviousActualAmt != 0)
                            $get(_sClienttxtActualAmt).value = parseInt(PreviousActualAmt) - parseInt($get(_sClienttxtLateFeeAmt).value) - parseInt(txtActualAmount.value);

                        if (PreviousConcession != 0)
                            $get(_sClienttxtConcessionAmt).value = parseInt(PreviousConcession) - parseInt(hidConcessionAmount.value);

                        if (parseInt($get(_sClienttxtActualAmt).value) < 0)
                            $get(_sClienttxtActualAmt).value = 0;
                        if (PreviousAmtToBePaid != 0)
                            $get(_sClienttxtAmtToBePaid).value = parseInt(PreviousAmtToBePaid) - (parseInt(lblAmountPayable.innerHTML) + parseInt($get(_sClienttxtLateFeeAmt).value));
                        if (parseInt($get(_sClienttxtAmtToBePaid).value) < 0)
                            $get(_sClienttxtAmtToBePaid).value = 0;

                        if (lblFeewiseLateFee >= PreviousLateFee)
                            $get(_sClienttxtLateFeeAmt).value = 0;
                        else
                            $get(_sClienttxtLateFeeAmt).value = parseInt(PreviousLateFee) - lblFeewiseLateFee;

                        $get(_sClienttxtAmtToBePaid).value = parseInt($get(_sClienttxtAmtToBePaid).value) + parseInt($get(_sClienttxtLateFeeAmt).value) + +parseInt(hidConcessionAmount.value);
                        $get(_sClienttxtActualAmt).value = parseInt($get(_sClienttxtActualAmt).value) + parseInt($get(_sClienttxtLateFeeAmt).value) + +parseInt(hidConcessionAmount.value);
                        txtActualAmount.value = "0";
                        hidPreviousActualAmt.value = txtActualAmount.value;
                        txtActualAmount.disabled = true;
                        
                        if ($get(_clienthidStudentFeeIds).value.match(hidFeeIds.value))
                            $get(_clienthidStudentFeeIds).value = $get(_clienthidStudentFeeIds).value.replace(hidFeeIds.value, "");

                    }
                    $get(_clienthidPreviousLateFee).value = $get(_sClienttxtLateFeeAmt).value;
                    $get(_clienthidTotalActualAmt).value = $get(_sClienttxtActualAmt).value;                    
                }
                else {
                    if (chk != null) {                        
                        if ((hidFeeIds.value == "-9999" || hidFeeIds.value == "-9998") && chk.checked)
                        {
                            txtDueDate.disabled = false;                                                        
                            if (hidFeeIds.value == "-9999") {
                                cmbFeeType.disabled = false;
                                cmbPayableFor.disabled = false;
                            }
                            if (hidFeeIds.value == "-9998") {
                                txtNewPayableFor.disabled = false;
                                txtNewFeeType.disabled = false;
                            }
                        }
                        else {                                                     
                            txtDueDate.disabled = true;
                            txtActualAmount.disabled = true;                            
                            if (hidFeeIds.value == "-9999") {
                                cmbFeeType.disabled = true;
                                cmbPayableFor.disabled = true;
                            }
                            if (hidFeeIds.value == "-9998") {
                                txtNewPayableFor.disabled = true;
                                txtNewFeeType.disabled = true;
                            }
                        }
                    }
                }
            }
            EnableDisableFeeControls();
            GenerateRemarks();
            AddConcession();         
        }

        function AddConcession() {
            var removeAmount=0;            
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            
            for (var iRowCount = 0; iRowCount <= listView.rows.length; iRowCount++) {
                var chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998" && chk.checked) {
                    removeAmount = 1;
                }
            }

            if (removeAmount == 0) {
                $get(_sClienttxtLateFeeAmt).value = "0";
                $get(_sClienttxtPayableAmt).value = "0";
                $get(_sClienttxtAmtToBePaid).value = "0";
                $get(_sClienttxtActualAmt).value = "0";
                $get(_sClienttxtConcessionAmt).value = "0";
                GenerateRemarks();
            }
        }

        function GenerateRemarks() {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            var strRemark = "";
            var finalRemark = "";
            var PaybleFor = "";
            var FeeType = "";
            var Amount = 0;
            $get(_sClienttxtRemarks).value = "";
            $get(_clienthidStudentFeeIds).value = "";
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                var lblFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblFeeType");
                var lblPaybleFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblPaybleFor");
                var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");

                if (chk != null && chk.checked && hidFeeIds.value != "-9999" && hidFeeIds.value != "-9998" && txtActualAmount != null && txtActualAmount.value != "0") {

                    strRemark = strRemark + lblPaybleFor.innerHTML + " (" + lblFeeType.textContent + " - Rs. " + txtActualAmount.value + "/-) , ";
                    $get(_clienthidStudentFeeIds).value = $get(_clienthidStudentFeeIds).value + hidFeeIds.value;
                }
                else if (chk != null && chk.checked && (hidFeeIds.value == "-9999" || hidFeeIds.value == "-9998")) {
                    if (hidFeeIds.value == "-9998") {                        
                        var txtNewFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewFeeType");
                        var txtNewPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewPayableFor");
                        if (txtNewPayableFor != null && txtNewPayableFor.value != "" && txtNewFeeType != null && txtNewFeeType.value != "" && txtActualAmount != null && txtActualAmount.value != "0") {
                            strRemark = strRemark + txtNewPayableFor.value + " (" + txtNewFeeType.value + " - Rs. " + txtActualAmount.value + "/-) , ";
                        }
                }
                    else if (hidFeeIds.value == "-9999") {                        
                        var cmbFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbFeeType");
                        var cmbPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbPayableFor");
                        if (cmbPayableFor != null && cmbPayableFor.selectedIndex != "0" && cmbFeeType != null && cmbFeeType.selectedIndex != "0" && txtActualAmount != null && txtActualAmount.value !="0") {
                            //strRemark = strRemark + cmbPayableFor.value + " (" + cmbFeeType.selectedItem.value + " - Rs. " + txtActualAmount.value + " /-) , ";
                            strRemark = strRemark + cmbPayableFor.value + " (" + cmbFeeType.options[cmbFeeType.selectedIndex].text + " - Rs. " + txtActualAmount.value + "/-) , ";
                        }
                }
                }

        }
         
            finalRemark = " Amount paid for " + strRemark;
            var index = finalRemark.lastIndexOf(",");
            finalRemark = finalRemark.substring(0, index) + finalRemark.substring(index + 1);

            $get(_sClienttxtRemarks).value = finalRemark;      

        if (_sClienttxtConcessionAmt != null && $get(_sClienttxtConcessionAmt).value != "0")
            $get(_sClienttxtRemarks).value = $get(_sClienttxtRemarks).value + " with  Concession Fee (Concession Fee - Rs. " + $get(_sClienttxtConcessionAmt).value + "/-) ";
        if (_sClienttxtLateFeeAmt != null && $get(_sClienttxtLateFeeAmt).value != "0")
            GenerateLateFeeRemarks();
        $get(_sClienthidRemarks).value = $get(_sClienttxtRemarks).value;
        }

        function CalculateTotalActualAmount() {
             var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
             var TotalActualAmount = 0;            
             for (var i = 0; i < listView.rows.length; i++) {
                 chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
                 var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + i + "_hidStudentFeeId");
                 if (chk != null && chk.checked && hidFeeIds.value !== "-9999" && hidFeeIds.value != "-9998") {                 
                         var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtActualAmount");                         
                         TotalActualAmount = TotalActualAmount + parseInt(txtActualAmount.value);
                     }
                 }
             var optPDC = $get(_sClientchkFeePaymentId + '_2');
             
             if ((optPDC != null && optPDC.checked) ||  $get(_sClientchkFeePaymentId) == null)
                 $get(_sClienttxtLateFeeAmt).value = "0";
              
              var LateFee = $get(_sClienttxtLateFeeAmt).value;              
              var Concession = $get(_sClienttxtConcessionAmt).value;
              if ((TotalActualAmount + parseInt(LateFee) - parseInt(Concession)) < 0) {
              	$get(_sClienttxtConcessionAmt).value = "0";
              	Concession = $get(_sClienttxtConcessionAmt).value;
              }

              $get(_sClienttxtActualAmt).value = TotalActualAmount + parseInt(LateFee) - parseInt(Concession);
        }

        function CalculateActualAmt(obj, iRowCount) {
            var PreviousTotalActualAmt = 0 , PreviousActualAmt, AmountAdded;            
            
            var chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
            if (chk != null && chk.checked) {
                var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
                var lblLateFee = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblLateFee");
                var lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmountPayable");
                if (lblAmountPayable != null && lblAmountPayable.innerHTML == "0")
                    lblAmountPayable = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_lblAmount");
                var hidPreviousActualAmt = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidPreviousActualAmt");
                PreviousTotalActualAmt = $get(_sClienttxtActualAmt).value;

                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");

                if (txtActualAmount != null && hidFeeIds.value !== "-9999" && hidFeeIds.value != "-9998") {
                    
                    if (PreviousTotalActualAmt == "-" || PreviousTotalActualAmt == "")
                        PreviousTotalActualAmt = 0;                    
                    
                    if (parseInt(txtActualAmount.value) > parseInt(lblAmountPayable.innerHTML))
                        txtActualAmount.value = lblAmountPayable.innerHTML;

                    if (txtActualAmount.value == "")
                        txtActualAmount.value = "0";
                    if (hidPreviousActualAmt.value == "")
                        hidPreviousActualAmt.value = lblAmountPayable.innerHTML;

                    $get(_sClienttxtActualAmt).value = parseInt(PreviousTotalActualAmt) - parseInt(hidPreviousActualAmt.value) + parseInt(txtActualAmount.value);
                    if (parseInt($get(_sClienttxtActualAmt).value) < 0) {
                    	$get(_sClienttxtConcessionAmt).value = "0";
                    	CalculateTotalActualAmount();
                    	var LateFee = $get(_sClienttxtLateFeeAmt).value;
                    	var Concession = $get(_sClienttxtConcessionAmt).value;
                    	$get(_sClienttxtAmtToBePaid).value = parseInt($get(_sClienttxtPayableAmt).value) + parseInt(LateFee) - parseInt(Concession);
                    }
					hidPreviousActualAmt.value = txtActualAmount.value;
                    $get(_clienthidTotalActualAmt).value = $get(_sClienttxtActualAmt).value;                    
                }
            }
            //CalculateTotalActualAmount();
            GenerateRemarks();
        }

        function EnableListView() {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');           
            if(listView!=null)
                listView.disabled = false;

            if ($get(_clientbtnPayAndPrint) != null)
                $get(_clientbtnPayAndPrint).disabled = false;
            if ($get(_clientbtnPay) != null)
                $get(_clientbtnPay).disabled = false;
        }

        function DisableListView(){
           var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');           
            if(listView!=null)
                listView.disabled = true;

            if ($get(_clientbtnPayAndPrint) != null)
                $get(_clientbtnPayAndPrint).disabled = true;
            if ($get(_clientbtnPay) != null)
                $get(_clientbtnPay).disabled = true;
        }

        function EnableDisableFeeControls() {
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');           
            var i = 0;
            chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
             while(chk!=null) {
                 
                 var cmbFeeType = $get(_clientlstvwStudentFee + "_ctrl" + i + "_cmbFeeType");
                 var cmbPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + i + "_cmbPayableFor");
                 var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtActualAmount");
                 var txtDueDate = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtDueDate");

                 if (chk != null && chk.checked) {
                     if (AllowPartialEdit())
                        txtActualAmount.disabled = false;
                 }
                 else
                     txtActualAmount.disabled = true;
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + i + "_hidStudentFeeId");          

                if (chk != null && hidFeeIds.value == "-9999") {
                    if (chk.checked) {
                        if (AllowPartialEdit())
                            txtActualAmount.disabled = false;
                        cmbFeeType.disabled = false;
                        cmbPayableFor.disabled = false;
                        txtDueDate.disabled = false;
                        if (txtDueDate.value == "")
                            txtDueDate.value = GetTodaysDate();     
                    }
                    else if (!chk.checked && hidFeeIds.value == "-9999") {
                        txtActualAmount.value = "";
                        txtActualAmount.disabled = true;                        
                        cmbPayableFor.selectedIndex = "0";
                        cmbPayableFor.disabled = true;                        
                        cmbFeeType.selectedIndex = "0";
                        cmbFeeType.disabled = true;
                        txtDueDate.value = "";
                        txtDueDate.disabled = true;
                    }
                }
                if (chk != null && hidFeeIds.value == "-9998") {
                    var txtNewFeeType = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtNewFeeType");
                    var txtNewPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + i + "_txtNewPayableFor");
                    if (chk.checked) {
                        txtNewFeeType.disabled = false;
                        txtNewPayableFor.disabled = false;

                        if (AllowPartialEdit())
                            txtActualAmount.disabled = false;
                        txtDueDate.disabled = false;
                        if (txtDueDate.value == "")
                            txtDueDate.value = GetTodaysDate();     
                    }
                    else if (!chk.checked && hidFeeIds.value == "-9998") {
                        txtNewFeeType.value = "";
                        txtNewFeeType.disabled = true;
                        txtNewPayableFor.value = "";
                        txtNewPayableFor.disabled = true;
                        txtActualAmount.value = "";
                        txtActualAmount.disabled = true;
                        txtDueDate.value = "";
                        txtDueDate.disabled = true;
                    }
                }

                i = i + 1;
                chk = $get(_clientlstvwStudentFee + "_ctrl" + i + "_chkSelect");
            }
        }

        function GetTodaysDate() {
            var currentDate = new Date()
            var day = currentDate.getDate();
            var month = currentDate.getMonth();
            var year = currentDate.getFullYear();
            var monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun","Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
            var my_date = day + "-" + monthNames[month] + "-" + year;
            return my_date;
        }
        function ValidateFeeType(source, args) {
            var bIsValid = true;
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var cmbFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbFeeType");                
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && hidFeeIds.value == "-9999" && chk.checked) {
                    if (cmbFeeType.selectedIndex == "0") {
                        bIsValid = false;
                    }
                }                                 
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidatePayableFor(source, args) {
            var bIsValid = true;
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var cmbPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_cmbPayableFor");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && hidFeeIds.value == "-9999" && chk.checked) {
                    if (!cmbPayableFor.disabled && cmbPayableFor.selectedIndex == "0") {
                        bIsValid = false;
                    }
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateDate(source, args) {
            args.IsValid = true;
            var sMsgBlank = "";
            var sMsgInvalid = "";            
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var txtDueDate = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtDueDate");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && chk.checked && (hidFeeIds.value == "-9999" || hidFeeIds.value == "-9998")) {

                    var iCnt = iRowCount+1;
                    if (txtDueDate.value == "") {
                        if (!sMsgBlank.match(iCnt))
                            sMsgBlank = sMsgBlank + "," + iCnt;                        
                    }
                    else if (!validateDate(txtDueDate)) {
                        if (!sMsgInvalid.match(iCnt))
                            sMsgInvalid = sMsgInvalid + "," + iCnt;                        
                    }
                }
            }

            if (sMsgBlank != "") {
                sMsgBlank = sMsgBlank.substring(1, sMsgBlank.length);
                source.errormessage = 'Due Date should not be blank for the row(s) ' + sMsgBlank + ' .';
                args.IsValid = false;
            }
            else if (sMsgInvalid != "") {
                sMsgInvalid = sMsgInvalid.substring(1, sMsgInvalid.length);
                source.errormessage = 'Due Date should be in valid format for the row(s) ' + sMsgInvalid + '.';
                args.IsValid = false;
            }
            return !args.ISValid
        }

        function ValidateNewActualAmt(source, args) {
            args.IsValid = true;
            var sMsgBlank = "";
            var chkCount = 0;
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');            
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var txtActualAmount = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtActualAmount");
                var PayableAmount = parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value));
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && chk.checked) {
                    chkCount = 1;
                    var iCnt = iRowCount + 1;
                    if (PayableAmount != 0) {
                        if (txtActualAmount.value == "" || txtActualAmount.value == "0") {
                            if (!sMsgBlank.match(iCnt))
                                sMsgBlank = sMsgBlank + "," + iCnt;
                        }
                    }
                }

                if (chk != null && chk.checked && (hidFeeIds.value == "-9999" || hidFeeIds.value == "-9998")) {
                    var iCnt = iRowCount + 1;
                    if (txtActualAmount.value == "" || txtActualAmount.value == "0") {
                        if (!sMsgBlank.match(iCnt))
                            sMsgBlank = sMsgBlank + "," + iCnt;
                    }                 
                }
            }

            if (chkCount == 0) {
                alert("At least one fee entry should be selected for paying fee.");
                return !args.IsValid
            }           
            if (sMsgBlank != "") {
                sMsgBlank = sMsgBlank.substring(1, sMsgBlank.length);
                source.errormessage = 'Actual Amount should be greater than 0 for the row(s) ' + sMsgBlank + ' .';
                args.IsValid = false;
            }

            return !args.ISValid
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

        function ValidateNewFeeeType(source, args) {
            var bIsValid = true;
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var txtNewFeeType = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewFeeType");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && hidFeeIds.value == "-9998" && chk.checked) {
                    if (txtNewFeeType.value.trim() == "") {
                        bIsValid = false;
                    }
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateNewPayableFor(source, args) {
            var bIsValid = true;
            var listView = $get('<%= lstvwStudentFee.FindControl("tblStudentInfo").ClientID %>');
            for (var iRowCount = 0; iRowCount < listView.rows.length; iRowCount++) {
                chk = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_chkSelect");
                var txtNewPayableFor = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_txtNewPayableFor");
                var hidFeeIds = $get(_clientlstvwStudentFee + "_ctrl" + iRowCount + "_hidStudentFeeId");
                if (chk != null && hidFeeIds.value == "-9998" && chk.checked) {
                    if (txtNewPayableFor.value.trim() == "") {
                        bIsValid = false;
                    }
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function CheckUncheckCheckboxAsperDate() {
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth()
            var yyyy = today.getFullYear();
            var textBox = $get(_clienttxtPaymentDate).value;
            var arr = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
            var today = dd + '-' + arr[mm] + '-' + yyyy;
            if (today == textBox)
                $("input:checkbox[id*=chkPaymentSMSAcknowledgement]").attr('checked', true);
            else
                $("input:checkbox[id*=chkPaymentSMSAcknowledgement]").attr('checked', false);

        }

        function AllowPartialEdit() {
            if (parseInt($('#' + '<%=this.hidParticularFeeRestriction.ClientID %>').val()) == 0 || (parseInt($('#' + '<%=this.hidParticularFeeRestriction.ClientID %>').val()) == 1 && $('#' + '<%=this.hidAllowPartialFee.ClientID %>').val() == "Y"))
                return true
            else
                return false
        }

        function ValidateCautionMoneyAmount(oSrc, args) {
            adjustIt = $('#' + '<%=this.ChkPaymentCautionMoneyAdjusted.ClientID %>').prop('checked')
            amt = $('#' + '<%=this.hidRemaingCautionMoneyAmount.ClientID %>').val()
            actAmt = $('#' + '<%=this.txtActualAmt.ClientID %>').val()

            var optCash = $get(_sClientchkFeePaymentId + '_0');
            var optJV = $get(_sClientchkFeePaymentId + '_5');

            var msg = 'If need to adjust from caution money then payable amount should not be greater than caution money amount.'
            if (optJV.checked) {
                adjustIt = true;
                msg = 'Payable amount should not be greater than remaining caution money amount (Rs. ' + parseInt(amt)+').'
            }

            if (adjustIt && (!(optCash.checked == true || optJV.checked == true))) {
                oSrc.errormessage = 'If need to adjust from caution money then payment mode should be Cash or Journal Voucher.'
                args.IsValid = false
                return true;
            }
            else if (adjustIt && parseInt(actAmt) > parseInt(amt)) {
                oSrc.errormessage = msg;
                args.IsValid = false
                return true;
            }
            else {
                args.IsValid = true
                return false;
            }
        }

        function ValidateLegers(src, args) {        
            if ($('[id$=chkFeePayment_5]').prop('checked')) {
                var jv = $('#' + _clientcmbJVLedgers).val()
                if (jv == "0") {
                    src.errormessage  = 'From Ledger should be selected.'
                    args.IsValid = false;
                    return true;
                }
                else {
                    var jvName = $("#" + _clientcmbJVLedgers + " option:selected").text();

                    var isFound = false;
                    $('[id$=_chkSelect]:checked').each(function () {
                        var fee = this.id.replace('_chkSelect', '_lblFeeType')
                        var feeText = $('#' + fee).html()
                        if (feeText == jvName) {
                            isFound = true;
                        }
                    })

                    if (isFound) {
                        src.errormessage = 'Selected ledger and fee type should not be same.'
                        args.IsValid = false;
                        return true;
                    }
                }
            }

            args.IsValid = true;
            return false;
        }

        function OpenFile(file) {
            window.open(file, '_blank')
            return false;
        }

        function ValidateFileType(oSrc, args) {
            var isFound = false
            var file = $get('<%=this.flAttachment.ClientID %>').value

            if (file.trim() != '') {
                var extension = file.substr(file.lastIndexOf('.')).toUpperCase()
                if (extension != ".BMP" && extension != ".JPG" && extension != ".JPEG" && extension != ".PNG" && extension != ".PDF") {
                    isFound = true
                }
            }

            if (isFound) {
                oSrc.errormessage = "Image type should be in only in BMP, JPG, JPEG, PNG and PDF format.";
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFileSize(oSrc, args) {
            var file = $get('<%=this.flAttachment.ClientID %>').value

            if (file.size >= 5242880) {
                oSrc.errormessage = "File size should be less than 5 MB."
                args.IsValid = false
                return true
            }

            args.IsValid = true;
            return false;
        }

    </script>
</asp:Content>
