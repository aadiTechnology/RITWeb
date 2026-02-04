<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    CodeFile="PayInternalFeePopup.aspx.cs" Inherits="PayInternalFeePopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="100%">
            <tr>
                <td align="left" colspan="2" rowspan="1">
                    <table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
                        <tr>
                            <td style="height: 20px">
                                <asp:Label ID="lblHeader" runat="server" CssClass="MainTitleHead" Font-Bold="True" Text="<%$ Resources:LocalizedResources, PayInternalFees %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="color: #ff3333" valign="top">
                <asp:Label ID="Label1" runat="server" CssClass="ClsMdtStar" Text="* "
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    <asp:ValidationSummary ID="valSumErrorMsg" HeaderText="<%$ Resources:LocalizedResources, PleaseFixFollowingError %>"
                        CssClass="lblNormal" runat="server" />
                    <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="1">
                    <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Font-Bold="true"
                        Width="100%" Visible="false" EnableViewState="false" CssClass="ClsLabel"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Panel ID="pnlFields" runat="server" Width="100%">
                        <table width="100%">
                            <tr>
                                <td align="center">
                                    <table cellpadding="0" cellspacing="0" runat="server" id="tblHeading" visible="True"
                                        width="800px">
                                        <tr>
                                            <td>
                                                <table align="left">
                                                    <tr>
                                                        <td class="ClsBorderlight" valign="middle" style="padding-left: 5px">
                                                            <asp:Label ID="lblStudent" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, StudentName %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="TextNormal"> :</span>
                                                        </td>
                                                        <td class="ClsHilightBGB">
                                                            <asp:Label ID="lblStudentHeading" runat="server" EnableViewState="True"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table align="right">
                                                    <tr>
                                                        <td align="right" class="ClsGreenBG" style="padding-right: 10px; width: 110px; white-space:nowrap" id="tdCustomReceipt" runat="server" Visible="false">
                                                            <asp:HyperLink ID="hlnkCustomReceipt" runat="server" CssClass="SubTitle" NavigateUrl="CustomizeInternalRecieptPopUp.aspx" 
                                                                Text="<%$ Resources:LocalizedResources, CustomReceipt %>" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table>
                                                    <tr style="height:10px;">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" width="100px" class="ClsBorderlight">
                                                            <asp:RadioButton ID="optCash" runat="server" CssClass="ClsLabel" Text="Cash"
                                                                GroupName="FeeType" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" width="100px">
                                                            <asp:RadioButton ID="optCheque" runat="server" CssClass="ClsLabel" GroupName="FeeType"
                                                                Text="Cheque" />
                                                        </td>
                                                        <td align="left" class="ClsBorderlight" width="220px" id="tdElectronic" runat="server">
                                                            <asp:RadioButton ID="optElectronic" runat="server" CssClass="ClsLabel" GroupName="FeeType"
                                                                Text="Electronic(NEFT/RTGS/IMPS)" />
                                                        </td>
                                                    </tr>
                                                    <tr style="height:10px;">
                                                        <td>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
											<td>
												<table cellpadding="0" cellspacing="1">
													<tr>
														<td align="left" width="25px">	
                                                            <asp:Label ID="lblLegend" runat="server" class="ClsLblLgnd" style="font:Bold;width:50px" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label> 															
														</td>														
														<td id="tdUnclearedTransLegend" runat="server" align="left" colspan="1" style="padding-right: 3px" width="30px">
															<asp:Label ID="Label3" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	    CssClass="UnclearedChq" Height="17px" Text=" " Width="20px" EnableViewState="False">
																<img src="../images/spacer.gif" width="18px" height="14px"/>
																	
															</asp:Label>
														</td>
														<td id="tdUnclearedTransLabel" runat="server" align="left">
                                                                <asp:Label ID="lblUncleardTrasanction" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources,UnclearedTransactions%>"></asp:Label>																	
														</td>
                                                        <td width="10px">
                                                        </td>
                                                        <td align="left" style="padding-right: 3px" width="25px" >
															<asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	    CssClass="BounceCheque" EnableViewState="False" Height="20px" Text=" " Width="20px">
																<img height="20px" src="../images/spacer.gif" width="20px" />
																	
															</asp:Label>
														</td>
														<td align="left" width="175px">
                                                                        <asp:Label ID="lblBouncedChkTransaction" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources, BouncedChequeTransactions%>"></asp:Label>																
														</td>
                                                        <td width="10px">
                                                        </td>
                                                        <td align="left" style="padding-right: 3px" width="25px">
															<asp:Label ID="Label15" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	    CssClass="PendingFees" EnableViewState="False" Height="20px" Text=" " Width="20px">
																<img height="20px" src="../images/spacer.gif" width="20px" />
																	
															</asp:Label>
														</td>
														<td align="left" width="85px">
                                                                        <asp:Label ID="lblDelayedFees" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources, DelayedFees%>"></asp:Label>
														</td>
													</tr>
												</table>
											</td>
										</tr>
                                        <tr>
                                            <td>
                                                <asp:ListView ID="lstvwInternalFee" runat="server" OnItemDataBound="lstvwInternalFee_ItemDataBound"
                                                    DataKeyNames="InternalFeeDetailsId,SerialNumber,FeeDetailsId,DebitCredit,ReceiptNo,InternalFeeMasterId,IsLastCredit,NetBankingPaymentTransactionId,AccountHeaderId,PaymentDoneDate"
                                                    OnItemCommand="lstvwInternalFee_ItemCommand">
                                                    <LayoutTemplate>
                                                        <table width="100%" runat="server" id="tblStudentInfo" style="color: #333333" cellpadding="0"
                                                            cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsMarksGridHeader">
                                                                <th id="thchk" runat="server" align="center" width="4%">
                                                                    <asp:CheckBox ID="chkSelectAll" runat="server" onclick="CheckAll(this);" />
                                                                </th>
                                                                <th id="thFeeType" runat="server" align="left" width="15%" style="padding-left: 5px">
                                                                    <asp:Label ID="lblStudent" runat="server" Text="<%$ Resources:LocalizedResources, FeeType %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thPaybleFor" runat="server" align="left" width="15%" style="padding-left: 5px">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, PaybleFor %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thAmount" runat="server" align="right" width="6%" style="padding-right: 5px">
                                                                   <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Amount %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thPartialFee" runat="server" align="center" width="13%">
                                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, AmountToBePaid %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thDueDate" runat="server" align="center" width="8%">
                                                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, DueDate %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thDelete" runat="server" align="center" width="6%">
                                                                    <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, Delete %>" EnableViewState="False"></asp:Label>
                                                                </th>
                                                                <th id="thPrint" runat="server" align="center" width="6%">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, Print %>" EnableViewState="False"></asp:Label>

                                                                  
                                                                </th>
                                                              
                                                            </tr>

                                                            <tr runat="server" id="itemPlaceholder">
                                                            </tr>

                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="trlstvwRow" runat="server" class="ClsMarksGridAltRowN">
                                                            <td id="tdchk" runat="server" align="center">
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </td>
                                                            <td id="tdFeeType" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("FeeType") %>' />
                                                            </td>
                                                            <td id="tdPaybleFor" runat="server" align="left" style="padding-left: 5px">
                                                                <asp:Label ID="lblPaybleFor" runat="server" Text='<%# Eval("PayableFor") %>' />
                                                            </td>
                                                            <td id="tdAmount" runat="server" align="right" style="padding-right: 5px">
                                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' />
                                                            </td>
                                                            <td id="tdPartialFee" runat="server" align="center">
                                                                <asp:TextBox ID="txtPartialFee" runat="server" MaxLength="6" CssClass="TxtNormal"
                                                                    Width="90px" onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                    onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
                                                                    ondrop="event.returnValue=false"></asp:TextBox>
                                                            </td>
                                                            <td id="tdDueDate" runat="server" align="center">
                                                                <asp:Label ID="lblPaidDate" runat="server" Text='<%#Eval("PaidDate","{0:dd-MMM-yyyy}")%>' />
                                                            </td>
                                                            <td id="tdDelete" runat="server" align="center">
                                                                <asp:ImageButton ID="imgDelete" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
                                                                    CausesValidation="false" CommandName="Remove" />
                                                            </td>
                                                            <td id="tdPrint" runat="server" align="center">
                                                                <asp:HyperLink ID="hlnkReceipt" runat="server" Text="<%$ Resources:LocalizedResources, Receipt %>" Visible="true" NavigateUrl="InternalFeePaymentReceipt.aspx"> </asp:HyperLink>
                                                                  <asp:HiddenField ID="hidRemark" runat="server" Value='<%# Eval("Remarks") %>' />
                                                            </td>
                                                           
                                                        </tr>
                                                    </ItemTemplate>
                                                    <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>
                                                </asp:ListView>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 11px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <table cellpadding="0" cellspacing="2" runat="server" id="Table1" visible="True">                                                  
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px;width:150px">
                                                            <asp:Label ID="lblDate" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, PaymentDate %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="TextNormal"> :</span>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" TabIndex="1"></asp:TextBox>
                                                            <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtDate" Format="dd MMM yyyy" Culture="en"
                                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotBlank %>" />
                                                            <asp:Label ID="Label17" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>
                                                            <asp:CustomValidator ID="cstDate" runat="server" Display="none" EnableClientScript="true"
                                                                ClientValidationFunction="ValidateDate" ErrorMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotFutureDate %>"></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="lblAmount" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, AmountPayable %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="TextNormal">:</span>
                                                        </td>
                                                        <td valign="middle" align="left">
                                                            <asp:Label ID="txtAmount" Style="padding-left: 5px" runat="server" CssClass="TextNormalB"
                                                                Text="-" Width="16px"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:CustomValidator ID="cstFeeType" runat="server" Display="None" ClientValidationFunction="ValidateFeeType"
                                                                ErrorMessage=""></asp:CustomValidator>
                                                            <asp:CustomValidator ID="cstValTxtAmount" runat="server" Display="None" ClientValidationFunction="ValidateAmount"
                                                                ErrorMessage=""></asp:CustomValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="Label4" runat="server" CssClass="TextNormal" Text="<%$ Resources:LocalizedResources, ActualAmount %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class=""> :</span>
                                                        </td>
                                                        <td valign="middle">
                                                            <asp:Label ID="lblPaybleAmount" runat="server" Text="-" CssClass="ClsLabel"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="middle" class="ClsBorderlight" style="padding-left: 5px">
                                                            <asp:Label ID="lblRemark" runat="server" CssClass="TextNormal " Text="<%$ Resources:LocalizedResources, Remarks %>"
                                                                EnableViewState="False"></asp:Label>
                                                                <span class="TextNormal"> :</span>
                                                        </td>
                                                        <td valign="top" align="left">
                                                            <asp:TextBox ID="txtRemark" runat="server" CssClass="LrgTxtBox" MaxLength="100" Width="346px"
                                                                TabIndex="2" Rows="3"></asp:TextBox>
                                                        </td>
                                                        <td align="left" valign="middle">
                                                            <asp:RequiredFieldValidator ID="reqValDate" runat="server" ControlToValidate="txtDate"
                                                                ErrorMessage="Date should not be blank." SetFocusOnError="True" Display="None"></asp:RequiredFieldValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <%--<asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>--%>
                                                                 <table width="100%" id="trChequeDetails" runat="server">
                                                                <tr>
                                                                    <td class="ClsBorderlight" width="150px">
                                                                        <span class="ClsLabel">Cheque Number : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtChequeNumber" runat="server" MaxLength="6" CssClass="SmlTxtBox"
                                                                        onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
                                                                        onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
                                                                        ondrop="event.returnValue=false;"></asp:TextBox>
                                                                        <span class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="cstChequeNumber" runat="server" ClientValidationFunction="ValidateChequeNo"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Cheque Number should not be blank."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight" width="100px">
                                                                        <span class="ClsLabel">Cheque Date : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txtChequeDate" CssClass="SmlTxtBox" runat="server" TabIndex="1"></asp:TextBox>
                                                                        <rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtChequeDate" Format="dd MMM yyyy" Culture="en"
                                                                            ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="<%$ Resources:LocalizedResources, PaymentDateShouldNotBlank %>" />
                                                                        <asp:Label ID="Label9" runat="server" CssClass="ClsMdtStar" ForeColor="Red" Text="*"></asp:Label>
                                                                        <asp:CustomValidator ID="cstChequeDate" runat="server" ClientValidationFunction="ValidateChequeDate"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Cheque date should not be blank."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight" width="150px">
                                                                        <span class="ClsLabel">Bank Name : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="cmbBank" runat="server" CssClass="LrgCombo">
                                                                        </asp:DropDownList>
                                                                        <span class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="cstBankName" runat="server" ClientValidationFunction="ValidateBankName"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="ClsBorderlight" width="150px">
                                                                        <span class="ClsLabel">Deposit in Bank : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="cmbDepositInBank" runat="server" CssClass="LrgCombo">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                           <%-- </ContentTemplate>
                                                            <Triggers>
                                                                <asp:AsyncPostBackTrigger ControlID="optCash" EventName="CheckedChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="optCheque" EventName="CheckedChanged" />
                                                            </Triggers>
                                                            </asp:UpdatePanel>--%>
                                                        </td>
                                                    </tr>
                                                    <tr id="trElectronicDetails" runat="server">
                                                    <td colspan="3">
                                                            <table width="100%">
                                                                <tr style="width: 100%" id="d1" runat="server">
                                                                    <td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
                                                                        <span class="ClsLabel" style="width: 100%">Txn Number :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsTextNormal">
                                                                        <asp:TextBox ID="txtTransactionNo" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                                            MaxLength="16" TabIndex="16"></asp:TextBox>&nbsp; <span class="ClsMdtStar">*
                                                                        </span>
                                                                        <asp:CustomValidator ID="cstTransactionNo" runat="server" ClientValidationFunction="ValidateTrasactionNo"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Transaction Number should not be blank."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trElectronicTypes" viewstatemode="Enabled" runat="server">
                                                                    <td align="left" valign="top" class="ClsBorderlight" style="width: 24%; height: 9px;">
                                                                        <span class="ClsLabel">Type :</span>
                                                                    </td>
                                                                    <td align="left" class="ClsTextNormal" style="height: 9px">
                                                                        <asp:DropDownList ID="cmbElectronicTypes" runat="server" ViewStateMode="Enabled"
                                                                            CssClass="LrgCombo" TabIndex="17">
                                                                        </asp:DropDownList>
                                                                        <span class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="CustomValidator2" runat="server" ClientValidationFunction="ValidateTrasactionType"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Type should be selected."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="d3" runat="server">
                                                                    <td class="ClsBorderlight" width="150px">
                                                                        <span class="ClsLabel">Bank Name : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlBankName" runat="server" CssClass="LrgCombo">
                                                                        </asp:DropDownList>
                                                                        <span class="ClsMdtStar">*</span>
                                                                        <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateElectronicBank"
                                                                        Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="d2" runat="server">
                                                                    <td class="ClsBorderlight" width="150px">
                                                                        <span class="ClsLabel">Deposit in Bank : </span>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlDepositeInBank" runat="server" CssClass="LrgCombo">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="" valign="top">
                                                        </td>
                                                        <td align="left" valign="top">
                                                            <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Pay %>" CssClass="ClsBtn" TabIndex="3"
                                                                UseSubmitBehavior="false" OnClick="btnSave_Click" />
                                                            <asp:Button ID="btnSaveAndPrint" runat="server" Text="<%$ Resources:LocalizedResources, PayAndPrint %>" CssClass="ClsBtn"
                                                                TabIndex="4" UseSubmitBehavior="false" Width="110px" OnClick="btnSaveAndPrint_Click" />
                                                            <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" TabIndex="5"
                                                                CausesValidation="False" UseSubmitBehavior="false" OnClick="btnCancel_Click" />
                                                        </td>
                                                        <td align="left" valign="top">
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </td>
            </tr>
        </table>
        <asp:HiddenField ID="hidRemark" runat="server" Value="1"/>
        <asp:HiddenField ID="hidStudentId" runat="server" />
        <asp:HiddenField ID="hidNextAcademicYearId" runat="server" />
        <asp:HiddenField ID="hidStudentName" runat="server" />
        <asp:HiddenField ID="hidValueConfigAmount" runat="server" />
        <asp:HiddenField ID="hidReceiptNo" runat="server" />
        <asp:HiddenField ID="hidRegNo" runat="server" />
        <asp:HiddenField ID="hidFromDate" runat="server" />
        <asp:HiddenField ID="hidToDate" runat="server" />
        <asp:HiddenField ID="hidIncludePaid" runat="server" />
        <asp:HiddenField ID="hidPayForNextYear" runat="server" />
        <asp:HiddenField ID="hidIsRegNoFilter" runat="server" />
        <asp:HiddenField ID="hidServerDate" runat="server" />
        <asp:HiddenField ID="hidStandardID" runat="server" />
        <asp:HiddenField ID="hidDivisionID" runat="server" />
        <asp:HiddenField ID="hidFeeTypeID" runat="server" />
        <asp:HiddenField ID="hidInternalFeeDetailsId" runat="server" />
        <asp:HiddenField ID="hidPageIndex" runat="server" />
        <asp:HiddenField ID="hidQueryString" runat="server" />
        <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisFeeDetails" runat="server" />
        <asp:HiddenField ID="hidValOneFeeTypeSelected" runat="server" />
        <asp:HiddenField ID="hidValAmountNotBlankOrZero" runat="server" />
        <asp:HiddenField ID="hidAmountShouldNotGreaterThan" runat="server" />
        <asp:HiddenField ID="hidPaymentDateShouldNotFutureDate" runat="server" />
        <asp:HiddenField ID="hidDefaultDepositeBank" runat="server" Value="0" />
        <asp:HiddenField ID="hidDefaultBankId" runat="server" Value="0" />        
        <asp:HiddenField ID="hidIsNextYearFeePayment" runat="server" Value="0" />        
        </div>
       <script language="javascript" type="text/javascript">

        _clienthidRemark = "<%=this.hidRemark.ClientID %>";
        _clienthidValueConfigAmount = "<%=this.hidValueConfigAmount.ClientID %>";
        _clientcstValTxtAmount = "<%=this.cstValTxtAmount.ClientID %>";
        _clienttxtAmount = "<%=this.txtAmount.ClientID %>";
        _clientlblPaybleAmount = "<%=this.lblPaybleAmount.ClientID %>";
        _clientcstDate = "<%=this.cstDate.ClientID%>";
        _clienttxtDate = "<%=this.txtDate.ClientID %>"
        _clientServerDate = "<%=this.hidServerDate.ClientID %>";
        _clientlstvwInternalFee = "<%=this.lstvwInternalFee.ClientID %>";
        _clientcstFeeType = "<%=this.cstFeeType.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>";
        _clienttxtRemark = "<%=this.txtRemark.ClientID %>";
        
        function ValidateFeeType(oSrc, args) {
            var Selected = false;
            var listView = $get('<%= lstvwInternalFee.FindControl("tblStudentInfo").ClientID %>');
            for (var i = 0; i < listView.rows.length; i++) {
                chk = $get(_clientlstvwInternalFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    if (chk.checked)
                        Selected = true;
                }
            }
            if (!Selected) {
                if ($get(_clientlblUpdateSucess) != null)
                    $get(_clientlblUpdateSucess).innerHTML = "";
                $get(_clientcstFeeType).errormessage = document.getElementById("<%=this.hidValOneFeeTypeSelected.ClientID %>").value;

                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false;
            }
        }

        function ValidateAmount(oSrc, args) {
            var txtAmounteValue = $get(_clienttxtAmount).value;
            var ConfigValue = $get(_clienthidValueConfigAmount).value;
            if (txtAmounteValue == "" || txtAmounteValue <= "0" || txtAmounteValue == "-") {
                if ($get(_clientlblUpdateSucess) != null)
                    $get(_clientlblUpdateSucess).innerHTML = "";
                $get(_clientcstValTxtAmount).errormessage = document.getElementById("<%=this.hidValAmountNotBlankOrZero.ClientID %>").value;
                args.IsValid = false
                return true
            }

            else if (parseInt(txtAmounteValue) > parseInt(ConfigValue)) {
                $get(_clientlblUpdateSucess).innerHTML = "";
                $get(_clientcstValTxtAmount).errormessage = document.getElementById("<%=this.hidAmountShouldNotGreaterThan.ClientID %>").value + ConfigValue + ".";
                args.IsValid = false
                return true
            }
            else {
                args.IsValid = true
                return false;
            }

        }

        function OpenRecieptPopup(sQueryString) {

            window.open('InternalFeePaymentReceipt.aspx?' +
                    sQueryString, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=875,height=690');

            return false;
        }
        
        function CheckAll(Src) {
            var listView = $get('<%= lstvwInternalFee.FindControl("tblStudentInfo").ClientID %>');
            var first = true;
            for (var i = 0; i < listView.rows.length; i++) {
                chk = $get(_clientlstvwInternalFee + "_ctrl" + i + "_chkSelect");
                if (chk != null) {
                    chk.checked = Src.checked;
                    if (first) {
                        $get(_clienttxtAmount).innerHTML = "0";
                        $get(_clientlblPaybleAmount).innerHTML = "0";
                        first = false;
                    }
                    CheckSelected(chk, i);
                }
            }
        }

        function CheckSelected(obj, iRowCount) {            
            var PreviousTotal;
            var PreviousPayble;
            var chk = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_chkSelect");
            if (chk != null) {

                var lblAmount = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_lblAmount");
                var txtPartialFee = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_txtPartialFee");

                if (lblAmount != null && txtPartialFee != null ) {

                    PreviousTotal = $get(_clienttxtAmount).innerHTML;
                    PreviousPayble = $get(_clientlblPaybleAmount).innerHTML;
                    if (PreviousTotal == "-" || PreviousTotal == "")
                        PreviousTotal = 0;
                    if (PreviousPayble == "-" || PreviousPayble == "")
                        PreviousPayble = 0;
                        
                        if (chk.checked) {
                        txtPartialFee.disabled = false;
                        txtPartialFee.value = lblAmount.innerHTML;
                        $get(_clienttxtAmount).innerHTML = parseInt(PreviousTotal) + parseInt(lblAmount.innerHTML);
                        $get(_clientlblPaybleAmount).innerHTML = parseInt(PreviousPayble) + parseInt(txtPartialFee.value);
                    }
                    if (!chk.checked) {

                        if (PreviousTotal != "0")
                            $get(_clienttxtAmount).innerHTML = parseInt(PreviousTotal) - parseInt(lblAmount.innerHTML);
                        if (PreviousPayble != "0")
                            $get(_clientlblPaybleAmount).innerHTML = parseInt(PreviousPayble) - parseInt(txtPartialFee.value);
                        txtPartialFee.value = "";
                        txtPartialFee.disabled = true;
                    }
                }
            }

            if ($('#' + _clienthidRemark).val() == "1") {
                var rmk = ''
                $("[id$=chkSelect]").each(function () {

                    if ($(this).prop('checked')) {
                        var hd = this.id.replace('chkSelect', 'hidRemark')
                        if ($('#' + hd).val().trim() != "")
                            rmk = rmk + ', ' + $('#' + hd).val()
                    }
                })

                if (rmk.length > 0)
                    $('#' + _clienttxtRemark).val(rmk.substring(2))
                else
                    $('#' + _clienttxtRemark).val("")
            }
       }



       
        function ChangeFees(obj, iRowCount) {            
            if (obj.value != null) {
                var PreviousTotal;
                var PreviousPayble;
                var PartialFees = 0, PartialFeeTotal = 0;
                chk = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_chkSelect");
                if (chk != null) {

                    var lblAmount = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_lblAmount");
                    var txtPartialFee = $get(_clientlstvwInternalFee + "_ctrl" + iRowCount + "_txtPartialFee");

                    if (lblAmount != null && txtPartialFee != null) {

                        //if (txtPartialFee.value == "" && chk.checked) 
                       //     txtPartialFee.value = lblAmount.innerHTML;                    

                        if ((txtPartialFee.value == "" || parseInt(lblAmount.innerHTML) < parseInt(txtPartialFee.value) || parseInt(txtPartialFee.value) == 0) && chk.checked) {
                            txtPartialFee.value = lblAmount.innerHTML;
                            ChangeFees(obj, iRowCount);
                        }
                        else {
                            var listView = $get('<%=  lstvwInternalFee.FindControl("tblStudentInfo").ClientID%>');
                            for (var i = 0; i < listView.rows.length; i++) {
                                var chk1 = $get(_clientlstvwInternalFee + "_ctrl" + i + "_chkSelect");
                                var txtPartial = $get(_clientlstvwInternalFee + "_ctrl" + i + "_txtPartialFee");
                                if (chk1 != null && chk1.checked && txtPartial != null) {
                                    PartialFeeTotal = parseInt(PartialFeeTotal) + parseInt(txtPartial.value);
                                }
                            }
                            $get(_clientlblPaybleAmount).innerHTML = PartialFeeTotal;

                        }
                    }

                }
            }         
        }

        function ValidateDate(source, args) {
            var bIsValid = true;

            if ($get(_clienttxtDate).value != "") {
                var serverDate = $get(_clientServerDate).value;
                dtStartDate = new Date(convertdate($get(_clienttxtDate).value));
                var today = new Date(serverDate);
                if (today < dtStartDate) {
                    if ($get(_clientlblUpdateSucess) != null)
                        $get(_clientlblUpdateSucess).innerHTML = "";
                    $get(_clientcstDate).errormessage = document.getElementById("<%=this.hidPaymentDateShouldNotFutureDate.ClientID %>").value;
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function OpenPopup(sQueryString) {
            window.open('../Accountant/InternalFeePaymentReceipt.aspx?' + sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=670,height=300');
            return false;
        }

        function OpenReceiptPopup(sQueryString) {
            window.open(sQueryString, 'blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500');
            return false;
        }

        function ConfirmDelete() {
            return window.confirm(document.getElementById("<%=this.hidAreYouSureYouWantToDeleteThisFeeDetails.ClientID %>").value)
        }

        _clienttrChequeDetails = "<%=this.trChequeDetails.ClientID %>"
        _clienttxtChequeNumber = "<%=this.txtChequeNumber.ClientID %>"
        _clienttxtChequeDate = "<%=this.txtChequeDate.ClientID %>"
        _clientcmbBank = "<%=this.cmbBank.ClientID %>"
        _clientvalErrMsgId = "<%=this.valSumErrorMsg.ClientID %>"
        _clienttrElectronicDetails = "<%=this.trElectronicDetails.ClientID %>"
        _clienttxtTransactionNo = "<%=this.txtTransactionNo.ClientID %>"
        _clientcmbElectronicTypes = "<%=this.cmbElectronicTypes.ClientID %>"
        _clientddlBankName = "<%=this.ddlBankName.ClientID %>"   

//        function ShowFields() {
//            $get(_clienttrChequeDetails).style.display = 'none';
//            $get(_clienttrElectronicDetails).style.display = 'none';
//        }

        function SetFeeType(typeId) {            
            ResetValidationMessages();
            if (typeId == 1) {
                $get(_clienttrChequeDetails).style.display = 'none';
                $get(_clienttrElectronicDetails).style.display = 'none';
            }
            else if (typeId == 2) {
                $get(_clienttrChequeDetails).style.display = '';
                $get(_clienttrElectronicDetails).style.display = 'none';
            }
            else if (typeId == 3) {
                $get(_clienttrElectronicDetails).style.display = '';
                $get(_clienttrChequeDetails).style.display = 'none';
            }   
        }


        function ValidateChequeNo(source, args) {
            var bIsValid = true;
                    if ($get(_clienttrChequeDetails).style.display != "none") {
                        if ($get(_clienttxtChequeNumber).value == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateTrasactionNo(source, args) {
            var bIsValid = true;
            if ($get(_clienttrElectronicDetails).style.display != "none") {
                if ($get(_clienttxtTransactionNo).value == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateTrasactionType(source, args) {
            var bIsValid = true;
            if ($get(_clienttrElectronicDetails).style.display != "none") {
                if ($get(_clientcmbElectronicTypes).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateElectronicBank(source, args) {
            var bIsValid = true;
            if ($get(_clienttrElectronicDetails).style.display != "none") {
                if ($get(_clientddlBankName).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateChequeDate(source, args) {
            var bIsValid = true;
                    if ($get(_clienttrChequeDetails).style.display != "none") {
                        if ($get(_clienttxtChequeDate).value == "") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ValidateBankName(source, args) {
            var bIsValid = true;
                    if ($get(_clienttrChequeDetails).style.display != "none") {
                        if ($get(_clientcmbBank).value == "0") {
                    bIsValid = false;
                }
            }
            args.IsValid = bIsValid;
            return !bIsValid;
        }

        function ResetValidationMessages() {
            var valSum = $get(_clientvalErrMsgId);
            if (valSum)
                valSum.style.display = 'none';
        }

        function DisableButtons() {
            var validationResult = true;
            if (typeof (Page_ClientValidate) == 'function')
                validationResult = Page_ClientValidate("");

            if (validationResult) {            
                document.getElementById('<%=this.btnSave.ClientID %>').disabled = true
                document.getElementById('<%=this.btnSaveAndPrint.ClientID %>').disabled = true
                document.getElementById('<%=this.btnCancel.ClientID %>').disabled = true            
            }
        }

    </script>
</asp:Content>
