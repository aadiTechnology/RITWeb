<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CautionMoneyChequePopUp.aspx.cs"
    MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master" Inherits="CautionMoneyChequePopUp" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">

    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 95%;
        vertical-align: top">
        <tr>
            <td style="background-color: white" id="MainDataTable" valign="top">
                <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                    <tr>
                        <td align="left" colspan="6" valign="top">
                            <table border="0" cellpadding="0" cellspacing="0" width="99%">
                                <tr>
                                    <td class="ClsGrayMainTitle" style="width: 99%;">
                                        <span class="MainTitleHead" style="font-weight: bold">
                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, CautionMoneyChequeDetails%>"></asp:Label></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <span class="ClsMdtStar">*
                                <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields%>"></asp:Label></span>
                        </td>
                    </tr>
                    <tr align="left" valign="top">
                        <td >
                            <asp:ValidationSummary runat="server" ID="valChequeData" ShowMessageBox="false" ShowSummary="true"
                                HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError%>" ValidationGroup="Save" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblErrMsg" runat="server" Visible="False" CssClass="LblErrorMsg"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trStudentName" runat="server" visible="true">
                        <td class="ClsBorderlight">
                            <span class="ClsLabel">
                                <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                <span id="Span2" class="colonPadding">:</span> </span>
                            <asp:Label ID="lblStudName" runat="server" CssClass="ClsLblRslt"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trPaymentDetails" runat="server">
                        <td align="left" class="ClsBtmBorderGray">
                            <asp:Label ID="lblPaymentDetails" runat="server" Font-Bold="True" CssClass="ClsLblLgnd"
                                Text="<%$ Resources:LocalizedResources, PaidDetails%>" EnableViewState="false"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: white" id="Td1" align="center" valign="top">
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr runat="server" id="trPaidDetails" align="left">
                                    <td valign="top" class="ClsBorderlight" colspan="4">
                                        <table width="100%">
                                            <tr align="left">
                                                <td width="25%" class="ClsBorderlight">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, PaymentDate%>"></asp:Label>
                                                        <span id="Span1" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" width="25%">
                                                    <asp:Label ID="lblPaymentDate" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                                </td>
                                                <td style="width: 25%;" class="ClsBorderlight">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, PaymentMode%>"></asp:Label>
                                                    </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight" width="25%">
                                                    <asp:Label ID="lblPaymmentMode" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trChqDetails" align="left">
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="lblChequeDateHeader" runat="server" Text="<%$ Resources:LocalizedResources, ChequeDate%>"></asp:Label>
                                                        <span id="Span3" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblChequeDate" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                                </td>
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, ChequeNumber%>"></asp:Label>
                                                        <span id="Span4" class="colonPadding">:</span> </span>
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblChequeNumber" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trBankDetails" align="left">
                                                <td class="ClsBorderlight">
                                                    <span class="ClsLabel">
                                                        <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, BankName%>"></asp:Label>
                                                        <span id="Span5" class="colonPadding">:</span></span>
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:Label ID="lblBankName" runat="server" CssClass="ClsLblRslt"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="ClsBtmBorderGray" colspan="4">
                                        <asp:Label ID="lblPayment" runat="server" Font-Bold="True" CssClass="ClsLblLgnd"
                                            Text="<%$ Resources:LocalizedResources, PaymentDetails%>" EnableViewState="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="3">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="ClsBorderlight" valign="top" style="width: 22%">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, Mode%>"></asp:Label>
                                        </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                        <asp:RadioButton ID="optCheque" runat="server" GroupName="PaymentMode" Text="<%$ Resources:LocalizedResources, Cheque%>"
                                            OnCheckedChanged="optCheque_CheckedChanged" AutoPostBack="true" TabIndex="1" />
                                        &nbsp;
                                        <asp:RadioButton ID="optCash" runat="server" GroupName="PaymentMode" Text="<%$ Resources:LocalizedResources, Cash%>"
                                            OnCheckedChanged="optCash_CheckedChanged" AutoPostBack="true"  TabIndex="1"/>&nbsp;
                                        <asp:RadioButton ID="optElectronic" runat="server" GroupName="PaymentMode" Text="Electronic (NEFT/RTGS/IMPS)"
                                            AutoPostBack="true"  TabIndex="1" 
                                            oncheckedchanged="optElectronic_CheckedChanged"/>
                                    </td>
                                   
                                </tr>
                                <tr>
                                     <td align="right" class="ClsBorderlight" style="width: 20%;">
                                        <asp:Label ID="lblDate" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, PaymentDate%>"
                                            EnableViewState="False"></asp:Label>
                                        <span class="ClsLabel colonPadding">:</span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" colspan="3">
                                        <asp:TextBox ID="txtDate" runat="server" CssClass="SmlTxtBox" Width="80px" AutoPostBack="True"
                                            TabIndex="3"></asp:TextBox>
                                        <rjs:PopCalendar ID="cal_Date" runat="server" Control="txtDate" Format="dd MMM yyyy"
                                            Culture="en" To-Message="<%$ Resources:LocalizedResources, PleaseEnterAValidDate%>"
                                            From-Message="<%$ Resources:LocalizedResources, PleaseEnterAValidDate%>" ShowErrorMessage="false"
                                            ShowWeekend="True" InvalidDateMessage="<%$ Resources:LocalizedResources, DateShouldNotBeBlank%>" />
                                        &nbsp;<asp:Label ID="Label19" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" class="ClsBorderlight" valign="top">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, Amount%>" ></asp:Label>
                                            <span id="Span6" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                        <asp:TextBox ID="txtAmount" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="6" onblur="extractNumber(this,0,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" TabIndex="4"></asp:TextBox>
                                        &nbsp;<asp:Label ID="lblAmountErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trReturnAmount" runat="server" visible="false">
                                    <td align="right" class="ClsBorderlight" valign="top">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label6" runat="server" Text="Return Amount" ></asp:Label>
                                            <span id="Span11" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                        <asp:TextBox ID="txtReturnAmount" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="6" onblur="extractNumber(this,0,false);"
                                            ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false" TabIndex="4"></asp:TextBox>
                                        &nbsp;<asp:Label ID="Label15" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>                                        
                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" ValidationGroup="Save" ClientValidationFunction="ValidateReturnAmount"></asp:CustomValidator>
                                    </td>
                                </tr>
                                <tr id="trConcessionAmount" runat="server">
                                    <td align="left" class="ClsBorderlight" style="width: 24%">
                                        <span class="ClsLabel">Concession Amount :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar">
                                           <asp:TextBox ID="txtConcessionAmt" TabIndex="4" runat="server" ViewStateMode="Enabled" MaxLength="6" CssClass="SmlTxtBox"
                                            onblur="CalculateTotalAmtToBePaid()" AutoPostBack="false" 
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
                                            onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>                                                                                 
                                        <asp:CustomValidator ID="cstValidateTotalFee" runat="server" ViewStateMode="Enabled" Display="none" EnableClientScript="true"
                                            ClientValidationFunction="ValidateConcessionAmt" ErrorMessage="Concession amount should not be greater than amount to be paid." ValidationGroup="Save"></asp:CustomValidator>
                                    </td>
                                </tr>
                                  <tr id="trNetAmount" runat="server">
                                    <td align="left" class="ClsBorderlight" style="height: 23px; width: 24%;">
                                        <span class="ClsLabel">Net Amount :</span>
                                    </td>
                                    <td align="left" class="ClsMdtStar" style="height: 23px">
                                       <asp:TextBox ID="txtActualAmt" TabIndex="6" runat="server" ViewStateMode="Enabled" MaxLength="6" onblur="extractNumber(this,0,false); VisibleOrHideControls();"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);" 
                                            onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" CssClass="SmlTxtBox"></asp:TextBox>
<%--                                        <asp:TextBox ID="txtActualAmt" TabIndex="6" runat="server" ViewStateMode="Enabled" MaxLength="6" onblur="extractNumber(this,0,false);"
                                            onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);" 
                                            onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" CssClass="SmlTxtBox"></asp:TextBox>&nbsp;--%>
                                        <span class="ClsMdtStar">* </span>                                     
                                    </td>
                                </tr>
                                <tr id="trChequeNumber" runat="server">
                                    <td align="right" class="ClsBorderlight" valign="top">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, ChequeNumber%>"></asp:Label>
                                            <span id="Span7" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                        <asp:TextBox ID="txtChequeNumber" runat="server" CssClass="SmlTxtBox" MaxLength="6"
                                            onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                            onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                            ondrop="event.returnValue=false;" TabIndex="5" Width="95px"></asp:TextBox>
                                        &nbsp;<asp:Label ID="lblChqNumberErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trChequeDate" runat="server">
                                    <td align="right" valign="top" class="ClsBorderlight">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, ChequeDate%>"></asp:Label>
                                            <span id="Span8" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px;" colspan="3">
                                        <asp:TextBox ID="txtChequeDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
                                            TabIndex="6"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="regChequeDate" runat="server" ErrorMessage="<%$ Resources:LocalizedResources, ChequeDateIsInvalid%>"
                                            ControlToValidate="txtChequeDate" ValidationExpression="^(([0-9])|([0-2][0-9])|([3][0-1]))\-(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\-\d{4}$"
                                            ValidationGroup="Save" Display="none"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="reqChequeDate" runat="server" ControlToValidate="txtChequeDate"
                                            ValidationGroup="Save" Display="none"></asp:RequiredFieldValidator>
                                        <rjs:PopCalendar ID="cal_ChequeDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
                                            Culture="en" To-Message="<%$ Resources:LocalizedResources, PleaseSelectAValidChequeDate%>"
                                            From-Message="<%$ Resources:LocalizedResources, PleaseSelectAValidChequeDate%>"
                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, ChequeDateShouldNotBeBlank%>" />
                                        &nbsp;<asp:Label ID="lblChqDateErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>
                                    </td>
                                </tr>                                
                                <tr id="trBankList" runat="server">
                                    <td align="right" valign="top" class="ClsBorderlight">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources, BankName%>"></asp:Label>
                                            <span id="Span9" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" style="padding-right: 10px" colspan="3">
                                        <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo" TabIndex="7">
                                        </asp:DropDownList>
                                        &nbsp;<asp:Label ID="lblBankErr" runat="server" CssClass="ClsMdtStar" ForeColor="Red"
                                            Text="*"></asp:Label>
                                    </td>
                                </tr>
                                <% if (IsAccountsModuleEnabled)
                                   { %>
                                <tr id="trChequeBankName" runat="server">
                                    <td align="left" valign="top" class="ClsBorderlight">
                                        <asp:Label ID="lblAcBankName" runat="server" class="ClsLabel" Text="<%$ Resources:LocalizedResources, DepositInBank%>" />
                                    </td>
                                    <td align="left" class="ClsTextNormal" colspan="3">
                                        <asp:DropDownList ID="ddlAcBankList" runat="server" CssClass="LrgCombo" TabIndex="8" />
                                        <span id="ddlAcBankMdtStar" runat="server" class="ClsMdtStar" visible="false">*
                                        </span>
                                    </td>
                                </tr>
                                <% } %>
                                <tr id="trRemark" runat="server" visible= "false">
                                    <td align="right" valign="top" class="ClsBorderlight">
                                        <span class="ClsLabel">
                                            <asp:Label ID="Label14" runat="server" Text="<%$ Resources:LocalizedResources, Remarks%>"></asp:Label>
                                            <span id="Span10" class="colonPadding">:</span> </span>
                                    </td>
                                    <td align="left" class="ClsTextNormal" colspan="3">
                                        <asp:TextBox ID="txtRemarks" runat="server" CssClass="SmlTxtBox" MaxLength="50" TabIndex="9"
                                            Width="400px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr id="trOnlinePayment" runat="server" viewstatemode="Enabled" style="width: 100%">
                                <td style="background-color: white; width: 100%" id="Td2" align="center" colspan="2" valign="top">
                                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                        <tr style="width: 100%">
                                            <td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
                                                <span class="ClsLabel" style="width: 100%">Txn Number :</span>
                                            </td>
                                            <td align="left" class="ClsTextNormal">
                                                <asp:TextBox ID="txtTxnNumber" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" MaxLength="16"
                                                    TabIndex="16"></asp:TextBox>&nbsp; <span class="ClsMdtStar">* </span>
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
                            <tr id="trPaidByName" runat = "server" style="width: 100%">
                                <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                                    <span class="ClsLabel">Paid by :</span>
                                </td>
                                <td align="left" class="ClsTextNormal">
                                    <asp:TextBox ID="txtPaidByName" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo" TabIndex="16" Width="250px"></asp:TextBox>
                                </td>
                            </tr> 
                            </table>
                            <asp:HiddenField ID="hidStudentId" runat="server" />
                            <asp:HiddenField ID="hidYearwiseStudentId" runat="server" />
                            <asp:HiddenField ID="hidMode" runat="server" />
                            <asp:HiddenField ID="hidCautionMode" runat="server" />
                            <asp:HiddenField ID="hidPaymentChequeId" runat="server" />
                            <asp:HiddenField ID="hidPaymentDate" runat="server" />
                            <asp:HiddenField ID="hidReturnChequeId" runat="server" />
                            <asp:HiddenField ID="hidServerDate" runat="server" />
                            <asp:HiddenField ID="hidStudentRegNo" runat="server" />
                            <asp:HiddenField ID="hidPageIndex" runat="server" />
                            <asp:HiddenField ID="hidAdmissionDate" runat="server" />
                            <asp:HiddenField ID="hidToDate" runat="server" />
                            <asp:HiddenField ID="hidFromDate" runat="server" />
                            <asp:HiddenField ID="hidChequeNo" runat="server" />
                            <asp:HiddenField ID="hidPostBackUrl" runat="server" />
                            <asp:HiddenField ID="hidFinancialYearJSON" runat="server" />
                            <asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" />
                            <asp:HiddenField ID="hidElectronicPaymentId" runat="server" />
                            <table>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                            CssClass="ClsBtnMid" OnClick="btnSave_Click" disable-page="true" ValidationGroup="Save" TabIndex="10" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSavePrint" runat="server" Text="<%$ Resources:LocalizedResources, SavePrint%>"
                                            CssClass="ClsBtnMid" Visible="false" OnClick="btnSave_Click" ValidationGroup="Save" TabIndex="11"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close%>"
                                            CssClass="ClsBtnMid" CausesValidation="false" OnClick="btnClose_Click" TabIndex="12"/>
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
                <asp:CustomValidator ID="cst_Date" runat="server" ClientValidationFunction="cstDate"
                    Display="None" SetFocusOnError="True" ErrorMessage="Date." ValidationGroup="Save"></asp:CustomValidator>
                <asp:CustomValidator ID="cstChequeNo" runat="server" CssClass="ClsMdtStar" Display="None"
                    EnableClientScript="true" ClientValidationFunction="ValidateChequeNumber" ErrorMessage="Error msg"
                    ValidationGroup="Save"></asp:CustomValidator>
                <asp:CustomValidator ID="cst_ChequeDate" runat="server" ClientValidationFunction="cstStartDate"
                    Display="None" SetFocusOnError="True" ErrorMessage="Cheque date." ValidationGroup="Save"></asp:CustomValidator>
                <asp:CustomValidator ID="cstChequeAmt" runat="server" CssClass="ClsMdtStar" Display="None"
                    EnableClientScript="true" ClientValidationFunction="ValidateChequeAmt" ErrorMessage="Error msg"
                    ValidationGroup="Save"></asp:CustomValidator>
                <asp:CustomValidator ID="cstBankName" runat="server" CssClass="ClsMdtStar" Display="None"
                    EnableClientScript="true" ClientValidationFunction="ValidateBankName" ErrorMessage="Error msg"
                    ValidationGroup="Save"></asp:CustomValidator>
                <asp:CustomValidator ID="cstAcDateValidator" runat="server" Display="None" EnableClientScript="true"
                    ClientValidationFunction="AccountsValidateDate" ValidationGroup="Save" />
                <asp:CustomValidator ID="cstTransationNo" runat="server" ClientValidationFunction="ValidateTransactionNo"
                    Display="None" SetFocusOnError="True" ErrorMessage="Txn number should not be blank." ValidationGroup="Save">
                </asp:CustomValidator>
                <asp:CustomValidator ID="cstElectronicType" runat="server" ClientValidationFunction="ValidateType"
                    Display="None" SetFocusOnError="True" ErrorMessage="Type should be selected." ValidationGroup="Save">
                </asp:CustomValidator>
                <asp:CustomValidator ID="cstEletronicBank" runat="server" ClientValidationFunction="ValidateElectronicBank"
                    Display="None" SetFocusOnError="True" ErrorMessage="Bank name should be selected." ValidationGroup="Save">
                 </asp:CustomValidator>               
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hidAmountShouldNotBeBlank" runat="server" />
    <asp:HiddenField ID="hidCultureInfo" runat="server" />
    <asp:HiddenField ID="hidChequeNumberShouldNotBeBlank" runat="server" />
    <asp:HiddenField ID="hidChequeDateShouldNotBeBlank" runat="server" />
    <asp:HiddenField ID="hidBankNameShouldBeSelected" runat="server" />
    <asp:HiddenField ID="hidAmountShouldBeGreaterThanZero" runat="server" />
    <asp:HiddenField ID="hidReturnDateShouldNotBeBlank" runat="server" />
    <asp:HiddenField ID="hidPaymentDateShouldNotBlank" runat="server" />
    <asp:HiddenField ID="hidReturnDateIsInvalid" runat="server" />
    <asp:HiddenField ID="hidDepositInBankShouldBeSelected" runat="server" />
    <asp:HiddenField ID="hidPaymentDateIsInvalid" runat="server" />
    <asp:HiddenField ID="hidshouldNotBeFutureDate" runat="server" />
    <asp:HiddenField ID="hidReturnDateShouldBeGreaterThanPaymentDate" runat="server" />
    <asp:HiddenField ID="hidFinancialYearIsClosedAndYouDoNotHaveEditAccess" runat="server" />
    <asp:HiddenField ID="hidFrom1April" runat="server" />
    <asp:HiddenField ID="hidPaymentDateShouldBeGreaterThanOrEqualAdmissionDate" runat="server"  />
    <asp:HiddenField ID="hidDateShouldBeWithinCurrentFinancialYear" runat="server"  />
    <asp:HiddenField ID="hidAllowCautionMoneyAdjustment" runat="server" Value="0"  />

    <script language="javascript" type="text/javascript" >

        _clientcstChequeAmtID = "<%= cstChequeAmt.ClientID %>";
        _sClienttxtConcessionAmt = "<%=this.txtConcessionAmt.ClientID %>";
        _clienttxtChequeAmtID = "<%= txtAmount.ClientID %>";
        _sClienttxtActualAmt = "<%=this.txtActualAmt.ClientID %>";
        _sClientcstActualAmt = "<=this.cstActualAmt.ClientID >";
        _sClientcstValidateTotalFee = "<%=this.cstValidateTotalFee.ClientID %>";

        function CalculateTotalAmtToBePaid() {        
            var TotalAmount = parseInt(RemoveLeadingZeroes($get(_clienttxtChequeAmtID).value));

            if (parseInt(TotalAmount) != 0) {
                var TotAmt;
                var ActualAmt = $get(_sClienttxtActualAmt).value;

                if ($get(_sClienttxtConcessionAmt).value == "") {
                    $get(_sClienttxtConcessionAmt).value = "0";
                }
                TotAmt = parseInt(RemoveLeadingZeroes($get(_clienttxtChequeAmtID).value)) - parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
                $get(_sClienttxtActualAmt).value = TotAmt;
                
                if (TotAmt < 0 && parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value)) > $get(_sClienttxtActualAmt).value) {
                    $get(_sClientcstValidateTotalFee).errormessage = "Concession amount should not be greater than amount to be paid.";

                }
            }
        }

    </script>

    <script language="javascript" type="text/javascript">
        _clientCstStartDate = "<%= cst_ChequeDate.ClientID %>";
        _clientcst_Date = "<%= cst_Date.ClientID %>";
        _clientcstChequeAmtID = "<%= cstChequeAmt.ClientID %>";
        _clientcstBankNameID = "<%= cstBankName.ClientID %>";
        _clientcstChequeNoID = "<%= cstChequeNo.ClientID %>";
        _clientcalStartDateID = "<%= txtChequeDate.ClientID %>";
        _clienttxtBankNameID = "<%= ddlBankName.ClientID %>";
        _clientddlAcBankList = "<%= ddlAcBankList.ClientID %>";
        _clienttxtChequeNumberID = "<%= txtChequeNumber.ClientID %>";
        _clienttxtChequeAmtID = "<%= txtAmount.ClientID %>";
        _clienttxtRemarksID = "<%= txtRemarks.ClientID %>";
        _clientoptChequeID = "<%= optCheque.ClientID %>";
        _clientoptCashID = "<%= optCash.ClientID %>";
        _clienttxtDateID = "<%= txtDate.ClientID %>";
        _clientServerDate = "<%= hidServerDate.ClientID %>";
        _clienthidMode = "<%= hidMode.ClientID %>";
        _clienthidPaymentDateId = "<%= hidPaymentDate.ClientID %>";
        _clienthidAdmissionDateId = "<%= hidAdmissionDate.ClientID %>";
        _clientlblDateId = "<%= lblDate.ClientID %>";
        _clientHideMode = "<%= hidMode.ClientID %>";
        _clienttxtTxnNumber = "<%=this.txtTxnNumber.ClientID %>"
        _clientcmbElectronicTypes = "<%=this.cmbElectronicTypes.ClientID %>"
        _clientddlBankNameCard = "<%=this.ddlBankNameCard.ClientID %>"
        _clientoptElectronic = "<%=this.optElectronic.ClientID %>"        
        _clientcmbElectronicTypes = "<%=this.cmbElectronicTypes.ClientID %>"
        _clientddlBankNameCard = "<%=this.ddlBankNameCard.ClientID %>"
        _sClienttxtConcessionAmt = "<%=this.txtConcessionAmt.ClientID %>";

        // Financial year related
        var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
        var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');

        function ValidateChequeAmt(aSrc, args) {
            args.IsValid = true;
            if ($get(_clienttxtChequeAmtID).value == "") {
                $get(_clientcstChequeAmtID).errormessage = document.getElementById("<%=this.hidAmountShouldNotBeBlank.ClientID %>").value;
                args.IsValid = false;
            }
            else if (parseInt($get(_clienttxtChequeAmtID).value) == 0) {
                $get(_clientcstChequeAmtID).errormessage = document.getElementById("<%=this.hidAmountShouldBeGreaterThanZero.ClientID %>").value;
                args.IsValid = false;
            }
            return !args.IsValid;
        }

        function ValidateChequeNumber(aSrc, args) {
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked && $get(_clienttxtChequeNumberID).value == "") {
                $get(_clientcstChequeNoID).errormessage = document.getElementById("<%=this.hidChequeNumberShouldNotBeBlank.ClientID %>").value;
                args.IsValid = false;
            }
            return !args.IsValid;
        }

        function ValidateTransactionNumber(aSrc, args) {
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked && $get(_clienttxtChequeNumberID).value == "") {
                $get(_clientcstChequeNoID).errormessage = document.getElementById("<%=this.hidChequeNumberShouldNotBeBlank.ClientID %>").value;
                args.IsValid = false;
            }
            return !args.IsValid;
        }

        function ValidateBankName(aSrc, args) {
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked) {
                if (!$get(_clienttxtBankNameID) && $get(_clientddlAcBankList) && $get(_clientddlAcBankList).value == "0") {
                    $get(_clientcstBankNameID).errormessage = document.getElementById("<%=this.hidDepositInBankShouldBeSelected.ClientID %>").value;
                    args.IsValid = false;
                }
                else if ($get(_clienttxtBankNameID) && $get(_clienttxtBankNameID).value == "0") {
                    $get(_clientcstBankNameID).errormessage = document.getElementById("<%=this.hidBankNameShouldBeSelected.ClientID %>").value;
                    args.IsValid = false;
                }
            }
            return !args.IsValid;
        }

        function cstDate(aSrc, args) {
            args.IsValid = true;
            var dtPaidDate, dtAdmissionDate, dtToday;
            var sMode = $get(_clientHideMode).value;
            if ($get(_clienttxtDateID).value == "") {
                if (sMode == "EditReturn" || sMode == "AddReturn")
                    $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidReturnDateShouldNotBeBlank.ClientID %>").value;
                else
                    $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidPaymentDateShouldNotBlank.ClientID %>").value

;
                args.IsValid = false;
            }
            else {
                if (document.all) {
                    dtPaidDate = new Date(($get(_clienttxtDateID).value).replace('-', ' '));
                    dtReturnDate = new Date(($get(_clienttxtDateID).value).replace('-', ' '));
                    dtAdmissionDate = new Date(($get(_clienthidAdmissionDateId).value).replace('-', ' '));
                    dtToday = new Date(($get(_clientServerDate).value).replace('-', ' '));
                }
                else {
                    dtPaidDate = new Date(getDate($get(_clienttxtDateID).value));
                    dtReturnDate = new Date(getDate($get(_clienttxtDateID).value));
                    dtAdmissionDate = new Date(getDate($get(_clienthidAdmissionDateId).value));
                    dtToday = new Date(getDate($get(_clientServerDate).value));
                }

                if (dtPaidDate == 'Invalid Date' || dtPaidDate == 'NaN' || dtPaidDate.getFullYear() < 1900) {
                    if (sMode == "EditReturn" || sMode == "AddReturn")
                        $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidReturnDateIsInvalid.ClientID %>").value;
                    else
                        $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidPaymentDateIsInvalid.ClientID %>").value;
                    args.IsValid = false;
                }

                var strAdmissionDate = getDateString(dtAdmissionDate);
                var lblDisplaydate = $get(_clientlblDateId).innerHTML;
                if (dtToday < dtPaidDate) {
                    $get(_clientcst_Date).errormessage = lblDisplaydate.replace(":", "") + document.getElementById("<%=this.hidshouldNotBeFutureDate.ClientID %>").value;
                    args.IsValid = false;
                }

                if (sMode == "EditReturn" || sMode == "AddReturn") {
                    if (document.all)
                        dtPaidDate = new Date(($get(_clienthidPaymentDateId).value).replace('-', ' '));
                    else
                        dtPaidDate = new Date(getDate($get(_clienthidPaymentDateId).value));
                    if (dtReturnDate < dtPaidDate) {
                        $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidReturnDateShouldBeGreaterThanPaymentDate.ClientID %>").value;
                        args.IsValid = false;
                    }
                }
                else if (dtAdmissionDate > dtReturnDate) {
                    $get(_clientcst_Date).errormessage = document.getElementById("<%=this.hidPaymentDateShouldBeGreaterThanOrEqualAdmissionDate.ClientID %>").value + " ( " + strAdmissionDate + " ).";
                    args.IsValid = false;
                }
            }
            return !args.IsValid;
        }

        function getDateString(obj) {
            var strDate = obj.getDate() + "-";
            var strMonth = parseInt(obj.getMonth());
            strMonth = months[strMonth];
            strDate = strDate + strMonth + "-";
            strDate = strDate + obj.getFullYear();
            return strDate;
        }

        function cstStartDate(aSrc, args) {
            args.IsValid = true;
            if ($get(_clientoptChequeID).checked) {
                var dtChequeDate;
                if ($get(_clientcalStartDateID).value == "") {
                    $get(_clientCstStartDate).errormessage = document.getElementById("<%=this.hidChequeDateShouldNotBeBlank.ClientID %>").value;
                    args.IsValid = false;
                }
                else if ($get(_clienttxtDateID).value != "") {
                    if (document.all)
                        dtChequeDate = new Date(($get(_clientcalStartDateID).value).replace('-', ' '));
                    else
                        dtChequeDate = new Date(getDate($get(_clientcalStartDateID).value));

                }
            }
            return !args.IsValid;
        }

        function AccountsValidateDate(src, args) {
            args.IsValid = true;

            if (!$get(_clientoptCashID).checked)
                return !args.IsValid;

            if (!_FinancialYear)
                return !args.IsValid;

            if (_FinancialYear.IsClosed && !_CanEditOldFinancialYear) {
                args.IsValid = false;
                src.errormessage = document.getElementById("<%=this.hidFinancialYearIsClosedAndYouDoNotHaveEditAccess.ClientID %>").value;
            }
            else {
                var dtFinancialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/", ""), 10));
                var dtFinancialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/", ""), 10));

                var dtPaymentDate = new Date(convertdate($get(_clienttxtDateID).value));

                if (dtPaymentDate < dtFinancialYearStartDate || dtPaymentDate > dtFinancialYearEndDate) {
                    args.IsValid = false;
                    src.errormessage = document.getElementById("<%=this.hidDateShouldBeWithinCurrentFinancialYear.ClientID %>").value + document.getElementById("<%=this.hidFrom1April.ClientID %>").value + dtFinancialYearStartDate.getFullYear() + ' to 31-March-' + dtFinancialYearEndDate.getFullYear() + ').';
                }
            }
            return !args.ISValid;
        }

        function getDate(obj) {
            var strDate = obj.replace('-', ' ').replace('-', ' ');
            return new Date(strDate);
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

        function ValidateTransactionNo(src, args) {            
            if (document.getElementById(_clienttxtTxnNumber) != null) {
                var TransNo = document.getElementById(_clienttxtTxnNumber);
                if (TransNo.value == "" && $get(_clientoptElectronic).checked) {
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
        }

        function ValidateType(src, args) {
            if (document.getElementById(_clientcmbElectronicTypes) != null) {
                var Type = document.getElementById(_clientcmbElectronicTypes);
                if (Type.value == 0 && $get(_clientoptElectronic).checked) {
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
        }

        function ValidateElectronicBank(src, args) {
            if (document.getElementById(_clientddlBankNameCard) != null) {
                var Bank = document.getElementById(_clientddlBankNameCard);
                if (Bank.value == 0 && $get(_clientoptElectronic).checked) {
                    args.IsValid = false
                    return true
                }
                args.IsValid = true
                return false
            }
        }

        function ValidateConcessionAmt(oSrc, args) {            
            var TotAmt;
            TotAmt =  parseInt(RemoveLeadingZeroes($get(_clienttxtChequeAmtID).value)) - parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
          
            if ($get(_sClienttxtConcessionAmt).value == "") {
                $get(_sClienttxtConcessionAmt).value = "0";
            }
            if (TotAmt < 0 && parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value)) > $get(_sClienttxtActualAmt).value) {
                TotAmt = parseInt(RemoveLeadingZeroes($get(_clienttxtChequeAmtID).value));
                $get(_sClienttxtActualAmt).value = TotAmt;
                
                $get(_sClienttxtActualAmt).value = TotAmt;
                $get(_sClienttxtConcessionAmt).value = "0";

                oSrc.errormessage = "Concession amount should not be greater than amount to be paid.";
                args.IsValid = false;
                return true;
            }
            args.IsValid = true;
            return false;
        }

        function calculateAmount(source, args) {        
            var totalAmount = $get(_clienttxtChequeAmtID).value;
            var ConcessionAmt = $get(_sClienttxtConcessionAmt).value;
            var actualAmt = $get(_sClienttxtActualAmt).value;
            var result = parseInt(totalAmount) - parseInt(ConcessionAmt);
            if (!isNaN(result)) {
                document.getElementById(actualAmt).value = result;
            }
        }

        function ValidateReturnAmount(oSrc, args) {

            var retAmt = $('#' + '<%=this.txtReturnAmount.ClientID %>').val()
            var amt = $('#' + '<%=this.txtAmount.ClientID %>').val()
            var mode = $('#' + '<%=this.hidMode.ClientID %>').val()
            var allowAdjustment = $('#' + '<%=this.hidAllowCautionMoneyAdjustment.ClientID %>').val()
            
            var isValid = true;
            if (allowAdjustment == 1 && (mode == 'AddReturn' || mode == 'EditReturn')) {
                
                if (retAmt == '') {
                    oSrc.errormessage = 'Return Amount should not be blank.'
                    isValid = false
                }
                else if (parseInt(retAmt) > parseInt(amt)) {
                    oSrc.errormessage = 'Return Amount should not be greater than amount.'
                    isValid = false
                }
            }

            args.IsValid = isValid
            return !isValid
        }

    </script>
</asp:Content>
