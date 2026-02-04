<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditFeePopUpUI.aspx.cs" Inherits="EditFeePopUpUI" MasterPageFile="../MasterPages/PopupMaster.master" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 92%;
		vertical-align: top">
		<tr>
			<td style="background-color: white" id="MainDataTable" align="center" valign="top">
				<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
					<tr>
						<td style="height: 19px" align="left" colspan="6" valign="top">
							<table border="0" cellpadding="0" cellspacing="0" width="99%">
								<tr>
									<td class="ClsGrayMainTitle" style="height: 20px; width: 99%;">
										<span class="MainTitleHead" style="font-weight: bold">Edit Fee Payment</span>
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
							<asp:ValidationSummary ID="valErrMsg" HeaderText="Please fix following error(s)" runat="server" />
                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" EnableClientScript="true"
								ClientValidationFunction="ValidatePaymentDate" ErrorMessage="Invalid Payment Date" ControlToValidate=""></asp:CustomValidator>                            
							<asp:CustomValidator ID="reqFeeType" runat="server" Display="none" EnableClientScript="true"
								ClientValidationFunction="ValidateFeeType" ErrorMessage="Fee Type should not be blank."></asp:CustomValidator>
							<asp:CustomValidator ID="reqPayableFor" runat="server" Display="none" EnableClientScript="true"
								ClientValidationFunction="ValidatePayableFor" ErrorMessage="Payable For should not be blank."></asp:CustomValidator>
							<asp:CustomValidator ID="cstAcDateValidator"
												 runat="server"
												 Display="None"
												 EnableClientScript="true"
												 ClientValidationFunction="AccountsValidateDate" />
							<asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"
								Visible="False"></asp:Label>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="left">
				<table width="100%">
					<tr id="tblChequeGrid" runat="server">
						<td align="left">
							<asp:RadioButtonList ID="chkFeePayment" runat="server" RepeatDirection="Horizontal" Onclick="EnableOrDisableControls()">
								<asp:ListItem Text="Fee Payment With Cash"></asp:ListItem>
								<asp:ListItem Text="Fee Payment With Cheque"></asp:ListItem>
								<asp:ListItem Text="Fee Payment With Swipe Card"></asp:ListItem>
							</asp:RadioButtonList>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table align="left" width="100%">
					<tr>
						<td>
							<asp:ListView ID="lstvStudentFeeDetails" runat="server" DataKeyNames="Schoolwise_Student_Fee_Id,FeeMode,DueDate,LateFee,OriginalLateFee"
								OnItemDataBound="lstvStudentFeeDetails_ItemDataBound">
								<LayoutTemplate>
									<table width="100%" runat="server" id="tblHoliday" style="color: #333333" cellpadding="0"
										cellspacing="1" class="GridBorder">
										<tr id="trHeader" runat="server" class="ClsGridHeader">
											<th>
											</th>
											<th align="left" style="padding-left: 10px;">
												Fee Type
											</th>
											<th align="left" style="padding-left: 10px;">
												Payable For
											</th>
											<th align="center">
												Amt. Paid
											</th>
											<th align="center">
												Amt. Payable
											</th>
											<th align="right" style="padding-right: 10px;">
												Late Fee
											</th>
											<th align="left" style="padding-left: 10px;">
												Due Date
											</th>
										</tr>
										<tr id="ItemPlaceHolder" runat="server">
										</tr>
									</table>
								</LayoutTemplate>
								<ItemTemplate>
									<tr id="tr2" runat="server" class="ClsGridRow">
										<td align="center">
											<asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelectStudentFee_Checked"
												AutoPostBack="true" />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPayableFor" runat="server" Text='<%# Eval("Payable_For") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPaidAmt" runat="server" Text='<%# Eval("AmtPaid") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblAmtPayable" runat="server" Text='<%# Eval("AmtPayable") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblLateFeeAmt" runat="server" Text='<%# Eval("LateFee") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPaidDate" runat="server" Text='<%# Eval("DueDate","{0:dd-MMM-yyyy}") %>' />
										</td>
									</tr>
								</ItemTemplate>
								<AlternatingItemTemplate>
									<tr id="tr3" runat="server" class="ClsGridAltRow">
										<td align="center">
											<asp:CheckBox ID="chkSelect" runat="server" OnCheckedChanged="chkSelectStudentFee_Checked"
												AutoPostBack="true" />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPayableFor" runat="server" Text='<%# Eval("Payable_For") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPaidAmt" runat="server" Text='<%# Eval("AmtPaid") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblAmtPayable" runat="server" Text='<%# Eval("AmtPayable") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblLateFeeAmt" runat="server" Text='<%# Eval("LateFee") %>' />
										</td>
										<td align="left" class="ClspaddingL">
											<asp:Label ID="lblPaidDate" runat="server" Text='<%# Eval("DueDate","{0:dd-MMM-yyyy}") %>' />
										</td>
									</tr>
								</AlternatingItemTemplate>
							</asp:ListView>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%" id="tblFeeDetails" runat="server">
					<tr id="trRecieptNo" runat="server">
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Reciept No. :</span>
						</td>
						<td align="left" style="width: 95%">
							<asp:TextBox ID="txtReceiptNo" TabIndex="1" runat="server" MaxLength="6" CssClass="SmlTxtBox"
								Enabled="False"></asp:TextBox>&nbsp;
						</td>
					</tr>
					<tr>
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Payment Date :</span>
						</td>
						<td align="left" style="width: 95%"><%--
							<asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True" onChange="ValidatePaymentDate"
								TabIndex="2"></asp:TextBox>
							<rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
								ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank."
								AutoPostBack="True" onselectionchanged="cal_PaymentDate_SelectionChanged" To-Today="true" />
							<span class="ClsMdtStar"> * </span>--%>
                            										
                                                <asp:TextBox ID="txtPaymentDate" CssClass="SmlTxtBox" runat="server" onChange="ValidatePaymentDate"
                                                    TabIndex="2" AutoPostBack="true"/>
                                                <rjs:PopCalendar ID="cal_PaymentDate" runat="server" Control="txtPaymentDate" Format="dd MMM yyyy"
                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Payment date should not be blank."
                                                    OnSelectionChanged="cal_PaymentDate_SelectionChanged" AutoPostBack="true" To-Today="true" />
                                                <span class="ClsMdtStar"> * </span>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Payable Amount :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:TextBox ID="txtPayableAmt" TabIndex="3" runat="server" MaxLength="6" CssClass="SmlTxtBox"
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
							<asp:TextBox ID="txtLateFeeAmt" TabIndex="4" runat="server" MaxLength="6" CssClass="SmlTxtBox"
								onblur="extractNumber(this,0,false);CalculateTotalAmtToBePaid();VisibleOrHideControls();"
								onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
								onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
							<asp:Label ID="lblDistribution" runat="server" CssClass="LblNormal" EnableViewState="True"></asp:Label>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Concession Amount :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:TextBox ID="txtConcessionAmt" TabIndex="5" runat="server" MaxLength="6" CssClass="SmlTxtBox"
								onblur="extractNumber(this,0,false);CalculateTotalAmtToBePaid();VisibleOrHideControls();"
								onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
								onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>&nbsp;
							<asp:CustomValidator ID="cstValidateTotalFee" runat="server" Display="none" EnableClientScript="true"
								ClientValidationFunction="ValidateConcessionAmt" ErrorMessage="Concession amount should not be greater than amount to be paid."></asp:CustomValidator>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Amount to be paid :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:TextBox ID="txtAmtToBePaid" TabIndex="6" runat="server" MaxLength="6" CssClass="SmlTxtBox"
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
							<asp:TextBox ID="txtActualAmt" TabIndex="7" runat="server" MaxLength="6" onblur="extractNumber(this,0,false);VisibleOrHideControls();"
								onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
								onpaste="event.returnValue=false;" ondrop="event.returnValue=false;" CssClass="SmlTxtBox"></asp:TextBox>&nbsp;
							<span class="ClsMdtStar"> * </span>
							<asp:CustomValidator ID="cstActualAmt" runat="server" Display="none" EnableClientScript="true"
								ClientValidationFunction="ValidateActualAmt" ErrorMessage="Actual amount should not be blank."></asp:CustomValidator>
							<asp:CustomValidator ID="cstBankNameDirectlyPaid" runat="server" ClientValidationFunction="ValidateBankNameDirectlyPaid"
								Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
						</td>
					</tr>
					<tr runat="server" id="trDirectlyPaid">
						<td align="left" colspan="2">
							<asp:CheckBox ID="chkDirectlyPaid" runat="server" Text="Cash directly paid in bank?" />
						</td>
					</tr>
                    <tr runat="server" id="trChallanNoRow" style="width: 100%">
                        <td align="left" class="ClsBorderlight" style="width: 24%">
                            <span class="ClsLabel">Challan No. :</span>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtChallanNo"
											runat="server"
											CssClass="SmlTxtBox"
											MaxLength="15"
											TabIndex="8"
											Width="190px" />
                            <asp:RegularExpressionValidator ID="regexpChallanValidator" runat="server" Display="None" ControlToValidate="txtChallanNo"
                                ErrorMessage="Challan No. can contain only alphabets, numbers and /, \ and - characters." CssClass="ClsMdtStar"
                                ValidationExpression="^[a-zA-Z0-9\\\/\-]{0,15}$"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
					<tr id="trBankName" runat="server" style="width: 100%">
						<td align="left" class="ClsBorderlight" style="width: 24%">
							<asp:Label ID="lblBankName" runat="server" CssClass="ClsLabel" Text="Bank Name :"></asp:Label>
						</td>
						<td align="left">
							<asp:DropDownList ID="cmbBankNameDirectlyPaid" runat="server" CssClass="LrgCombo" TabIndex="9" />
							<asp:Label ID="lblBankMandatory" runat="server" CssClass="ClsMdtStar" Text="*" EnableViewState="false"></asp:Label>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Remarks :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:TextBox ID="txtRemarks" TabIndex="10" runat="server" MaxLength="100" CssClass="SmlTxtBox"
								Width="400px" TextMode="MultiLine" ReadOnly="true"></asp:TextBox>
							<asp:RegularExpressionValidator ID="cst_Remarks" runat="server" Display="None" ControlToValidate="txtRemarks"
								ErrorMessage="Length of remarks should not exceed 1000 characters." CssClass="ClsMdtStar"
								ValidationExpression="^[\s\S]{0,1000}$"></asp:RegularExpressionValidator>
						</td>
					</tr>
					<tr>
						<td align="left" style="width: 24%">
							<asp:Image ID="Image3" runat="server" Height="1px" ImageUrl="~/images/spacer.gif" Width="148px" />
						</td>
						<td align="left" class="ClsMdtStar">
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr id="trChequeEntry" runat="server" style="width: 100%">
			<td style="background-color: white; width: 100%" id="Td1" align="center" valign="top">
				<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
					<tr style="width: 100%">
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%;">
							<span class="ClsLabel">Cheque Number :</span>
						</td>
						<td align="left" class="ClsTextNormal" style="width: 95%">
							<asp:TextBox ID="txtChequeNumber" runat="server" CssClass="SmlTxtBox" MaxLength="6"
								onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
								onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
								ondrop="event.returnValue=false;" TabIndex="11"></asp:TextBox>&nbsp;
							<span class="ClsMdtStar"> * </span>&nbsp;&nbsp;
							<asp:CustomValidator ID="cstChequeNumber" runat="server" ClientValidationFunction="ValidateChequeNo"
								Display="none" EnableClientScript="true" ErrorMessage="Cheque Number should not be blank."></asp:CustomValidator>
						</td>
					</tr>
					<tr>
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel">Cheque Date :</span>
						</td>
						<td align="left" class="ClsTextNormal">
							<asp:TextBox ID="txtDate" CssClass="SmlTxtBox" runat="server" AutoPostBack="True"
								TabIndex="12"></asp:TextBox>
							<rjs:PopCalendar ID="cal_CDate" runat="server" Control="txtDate" Format="dd MMM yyyy"
								ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank." />
							<span class="ClsMdtStar"> * </span>&nbsp;
							<asp:CustomValidator ID="cstChequeDate" runat="server" ClientValidationFunction="ValidateChequeDate"
								Display="none" EnableClientScript="true" ErrorMessage="Cheque date should not be blank."></asp:CustomValidator>
						</td>
					</tr>
					<tr style="width: 100%">
						<td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
							<span class="ClsLabel">Bank Name :</span>
						</td>
						<td align="left" class="ClsTextNormal">
							<asp:DropDownList ID="cmbBankName" runat="server" CssClass="LrgCombo" TabIndex="13">
							</asp:DropDownList>
							&nbsp;
							<span class="ClsMdtStar"> * </span>&nbsp;&nbsp;
							<asp:CustomValidator ID="cstBankName" runat="server" ClientValidationFunction="ValidateBankName"
								Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
						</td>
					</tr>
					<% if(IsAccountsModuleEnabled) { %>
                    <tr id="trAcChqBank" runat="server" style="width: 100%">
                        <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                            <span class="ClsLabel" >Deposit in Bank :</span>
                        </td>
                        <td align="left" class="ClsTextNormal">
                            <asp:DropDownList ID="ddlAcChqBank" runat="server" CssClass="LrgCombo" TabIndex="14">
                            </asp:DropDownList>
                        </td>
                    </tr>
					<% } %>
					<tr>
						<td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
							<span class="ClsLabel">Remarks :</span>
						</td>
						<td align="left" class="ClsTextNormal">
							<asp:TextBox ID="txtChequeRemarks" runat="server" CssClass="SmlTxtBox" MaxLength="50"
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
		<tr id="trCardEntry" runat="server" style="width: 100%">
			<td style="background-color: white; width: 100%" id="Td2" align="center" valign="top">
				<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
					<tr style="width: 100%">
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%">
							<span class="ClsLabel" style="width: 100%">Swipe Number :</span>
						</td>
						<td align="left" class="ClsTextNormal">
							<asp:TextBox ID="txtSwapNumber" runat="server" CssClass="LrgCombo" MaxLength="15"
								TabIndex="16"></asp:TextBox>&nbsp;
							<span class="ClsMdtStar"> * </span>&nbsp;&nbsp;
							<asp:CustomValidator ID="cstCardNumber" runat="server" ClientValidationFunction="ValidateSwapNo"
								Display="none" EnableClientScript="true" ErrorMessage="Swipe number should not be blank."></asp:CustomValidator>
						</td>
					</tr>
					<tr>
						<td align="left" valign="top" class="ClsBorderlight" style="width: 24%; height: 9px;">
							<span class="ClsLabel">Card Type :</span>
						</td>
						<td align="left" class="ClsTextNormal" style="height: 9px">
							<asp:DropDownList ID="cmbCardType" runat="server" CssClass="LrgCombo" TabIndex="17">
							</asp:DropDownList>
							&nbsp;
							<span class="ClsMdtStar"> * </span>&nbsp;&nbsp;
							<asp:CustomValidator ID="cstCardType" runat="server" ClientValidationFunction="ValidateCardType"
								Display="none" EnableClientScript="true" ErrorMessage="Card type should be selected."></asp:CustomValidator>
						</td>
					</tr>
					<tr style="width: 100%">
						<td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
							<span class="ClsLabel">Bank Name :</span>
						</td>
						<td align="left" class="ClsTextNormal">
							<asp:DropDownList ID="cmbBankNameCard" runat="server" CssClass="LrgCombo" TabIndex="18">
							</asp:DropDownList>
							&nbsp;
							<span class="ClsMdtStar"> * </span>&nbsp;&nbsp;
							<asp:CustomValidator ID="cstBankNameCard" runat="server" ClientValidationFunction="ValidateBankNameCard"
								Display="none" EnableClientScript="true" ErrorMessage="Bank name should be selected."></asp:CustomValidator>
						</td>
					</tr>
					<% if(IsAccountsModuleEnabled) { %>
                    <tr id="trAcCardBank" runat="server" style="width: 100%">
                        <td style="height: 19px; width: 24%;" align="left" valign="top" class="ClsBorderlight">
                            <span class="ClsLabel" >Deposit in Bank :</span>
                        </td>
                        <td align="left" class="ClsTextNormal">
                            <asp:DropDownList ID="ddlAcCardBank" runat="server" CssClass="LrgCombo" TabIndex="19">
                            </asp:DropDownList>
                        </td>
                    </tr>
					<% } %>
				</table>
			</td>
		</tr>
		<tr>
			<td align="center" width="100%">
				<table width="100%" id="tblAdditionalEntry" runat="server" class="ClsBorderlight">
					<tr>
						<td width="100%" align="center" class="ClsBorderlight" style="background-color: #ffffc4">
							<asp:Label ID="lblMsg" runat="server" CssClass="LblNrmlB"></asp:Label>
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:RadioButtonList ID="rdoFeeType" runat="server" CssClass="clsLabel" RepeatDirection="Horizontal"
								Onclick="ShowHideControls()" TabIndex="20">
								<asp:ListItem Selected="True">New Fee Type</asp:ListItem>
								<asp:ListItem>Existing Fee Type</asp:ListItem>
							</asp:RadioButtonList>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="left">
				<table cellpadding="0" cellspacing="2" id="tblDCEntry" runat="server" width="100%">
					<tr>
						<td colspan="2">
							<table width="100%" id="trDueDate" runat="server">
								<tr visible="false">
									<td align="left" valign="top" class="ClsBorderlight" style="width: 145px">
										<span class="ClsLabel">Due Date :</span>
									</td>
									<td>
										<asp:TextBox ID="txtChequeDate" CssClass="MidTxtBox" runat="server" AutoPostBack="True"
											TabIndex="21"></asp:TextBox>
										<rjs:PopCalendar ID="cal_ChequeDate" runat="server" Control="txtChequeDate" Format="dd MMM yyyy"
											ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Cheque date should not be blank." />
										<span class="ClsMdtStar"> * </span>
										&nbsp;<asp:CustomValidator ID="reqDueDate" runat="server" Display="none" EnableClientScript="true"
											ClientValidationFunction="ValidateDate" ErrorMessage="Due date should not be blank."></asp:CustomValidator>
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight" style="width: 5%">
							<span class="ClsLabel">Fee Type :</span>
						</td>
						<td align="left" class="ClsMdtStar" style="width: 95%">
							<asp:UpdatePanel ID="pnl" runat="server">
								<ContentTemplate>
									<table cellpadding="0" cellspacing="0">
										<tr>
											<td>
												<asp:TextBox ID="txtFeeType" runat="server" CssClass="MidTxtBox" MaxLength="50" TabIndex="22"></asp:TextBox>
											</td>
											<td>
												<asp:DropDownList ID="cmbFeeType" runat="server" AutoPostBack="True" TabIndex="23"
													OnSelectedIndexChanged="ddlFeeType_SelectedIndexChanged">
												</asp:DropDownList>
											</td>
											<td>
												<span class="ClsMdtStar"> * </span>
											</td>
										</tr>
									</table>
								</ContentTemplate>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight">
							<span class="ClsLabel">Payable For :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:UpdatePanel ID="UpdatePanel1" runat="server">
								<ContentTemplate>
									<table cellpadding="0" cellspacing="0">
										<tr>
											<td>
												<asp:TextBox ID="txtPayableFor" TabIndex="24" runat="server" MaxLength="50" CssClass="MidTxtBox"></asp:TextBox>
											</td>
											<td>
												<asp:DropDownList ID="cmbPayableFor" runat="server" TabIndex="25" AutoPostBack="True">
												</asp:DropDownList>
											</td>
											<td>
												<span class="ClsMdtStar"> * </span>
											</td>
										</tr>
									</table>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="cmbFeeType" EventName="SelectedIndexChanged" />
								</Triggers>
							</asp:UpdatePanel>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight">
							<span class="ClsLabel">Remarks :</span>
						</td>
						<td align="left" class="ClsMdtStar">
							<asp:TextBox ID="txtAddRemarks" TabIndex="26" runat="server" MaxLength="100" CssClass="SmlTxtBox"
								Width="400px"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<td align="left">
							<asp:Image ID="Image2" runat="server" Height="1px" ImageUrl="~/images/spacer.gif"
								Width="148px" />
						</td>
						<td align="left" class="ClsMdtStar">
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="left" colspan="2">
				<asp:HiddenField ID="hidStudentId" runat="server" />
				<asp:HiddenField ID="hidReciptNo" runat="server" />
				<asp:HiddenField ID="hidTotalAmount" runat="server" />
				<asp:HiddenField ID="hidStandardId" runat="server" />
				<asp:HiddenField ID="hidStudentFeeIds" runat="server" />
                <asp:HiddenField ID="hidPaidFeeIds" runat="server" />
				<asp:HiddenField ID="hidRemark" runat="server" />
				<asp:HiddenField ID="hidPaymentMode" runat="server" />
				<asp:HiddenField ID="hidAmtToBePaid" runat="server" />
				<asp:HiddenField ID="hidTotalAmtToPay" runat="server" />
				<asp:HiddenField ID="hidServerDate" runat="server" />
				<asp:HiddenField ID="hidPaymentType" runat="server" />
				<asp:HiddenField ID="hidLateFeeDesc" runat="server" />
				<asp:HiddenField ID="hidLateFeeAmt" runat="server" Value ="0"   />
				<asp:HiddenField ID="hidLateFeeAmtPaid" runat="server" Value ="0"   />
				<asp:HiddenField ID="hidLateFeeRemark" runat="server" />
				<asp:HiddenField ID="hidFinancialYearJSON" runat="server" />
				<asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" />
                <asp:HiddenField ID="hidEditedfeeIds" runat="server" />
                <asp:HiddenField ID="hidPaymentDete" runat="server" />
				<asp:HiddenField ID="hidStudentName" runat="server" />
			</td>
		</tr>
		<tr>
            <td>
                <asp:UpdatePanel ID="uplatefee" runat="server">
                    <ContentTemplate>
                        <table id="trNote" width="100%" visible="false" runat="server">
                            <tr>
                                <td align="left" class="ClsBorderlight" style="width:24%; background-color: #ffffc4;">
                                    <span class="LblNrmlB">Note :</span>
                                </td>
                                <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                    <asp:Label ID="lblVerifyNote" runat="server" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button ID="btnPay" Text="Pay" runat="server" CssClass="ClsBtnMid" TabIndex="27"
					UseSubmitBehavior="false" OnClientClick="DisableButtons(true); if(!ValidateCheckbox()){return false;}" OnClick="btnPay_Click" />
				<asp:Button ID="btnPayAndPrint" Text="Pay and Print" runat="server" CssClass="ClsBtnMid"
					TabIndex="28" UseSubmitBehavior="false" Visible="false" OnClientClick="DisableButtons(true); if(!ValidateCheckbox()){return false;}" />
				<asp:Button ID="btnClose" Text="Close" runat="server" CssClass="ClsBtnMid" OnClick="btnClose_Click"
					CausesValidation="False" TabIndex="29" UseSubmitBehavior="false" />
			</td>
		</tr>
	</table>

<script language="javascript" type="text/javascript">
_clientddlCardType = "<%=this.cmbCardType.ClientID %>";
_clienttxtBankName = "<%=this.cmbBankName.ClientID %>";
_clientddlBankNameCard = "<%=this.cmbBankNameCard.ClientID %>";
_sClientddlFeeType = "<%=this.cmbFeeType.ClientID %>";
_clientddlPayableFor = "<%=this.cmbPayableFor.ClientID %>";
_sClientddlBankNameDirectlyPaid = "<%=this.cmbBankNameDirectlyPaid.ClientID %>";
_sClientchkDirectlyPaid = "<%=this.chkDirectlyPaid.ClientID %>";
_sClientchkFeePaymentId = "<%=this.chkFeePayment.ClientID %>";
_sClientcstActualAmt = "<%=this.cstActualAmt.ClientID %>";
_clientcstBankName = "<%=this.cstBankName.ClientID %>";
_sClientcstValidateTotalFee = "<%=this.cstValidateTotalFee.ClientID %>";
_sClienreqDueDate = "<%=this.reqDueDate.ClientID %>";
_clienthidPaymentMode = "<%=this.hidPaymentMode.ClientID %>";
_sClienthidAmtToBePaid = "<%=this.hidAmtToBePaid.ClientID %>";
_clienthidTotalAmount = "<%=this.hidTotalAmtToPay.ClientID %>";
_clientServerDate = "<%=this.hidServerDate.ClientID %>";
_sClienthidPaymentType = "<%=this.hidPaymentType.ClientID %>";
_sClienttblEntry = "<%=this.tblAdditionalEntry.ClientID %>";
_sClienttblDCEntry = "<%=this.tblDCEntry.ClientID %>";
_sClienttrDirectlyPaid = "<%=this.trDirectlyPaid.ClientID %>";
_sClienttrBankName = "<%=this.trBankName.ClientID %>";
_sClientlblBankMandatory = "<%=this.lblBankMandatory.ClientID %>";
_sClienttrDueDate = "<%=this.trDueDate.ClientID %>";
_clienttrChequeEntry = "<%=this.trChequeEntry.ClientID %>";
_clienttrCardEntry = "<%=this.trCardEntry.ClientID %>";
_sClientrdoFeeType = "<%=this.rdoFeeType.ClientID %>";
_sClientGridId = "<%=this.lstvStudentFeeDetails.ClientID %>";
_sClienttxtAmtToBePaid = "<%=this.txtAmtToBePaid.ClientID %>";
_sClienttxtLateFeeAmt = "<%=this.txtLateFeeAmt.ClientID %>";
_sClienttxtActualAmt = "<%=this.txtActualAmt.ClientID %>";
_sClienttxtConcessionAmt = "<%=this.txtConcessionAmt.ClientID %>";
_sClienttxtPayableAmt = "<%=this.txtPayableAmt.ClientID %>";
_sClienttxtChequeDate = "<%=this.txtChequeDate.ClientID %>";
_sClienttxtPayableFor = "<%=this.txtPayableFor.ClientID %>";
_sClienttxtFeeType = "<%=this.txtFeeType.ClientID %>";
_clienttxtChequeNo = "<%=this.txtChequeNumber.ClientID %>";
_clienttxtSwapNumber = "<%=this.txtSwapNumber.ClientID %>";
_sClienttxtRemarks = "<%=this.txtRemarks.ClientID %>";
_clienttxtPaymentDate = "<%=this.txtPaymentDate.ClientID %>";
_clienttxtDate = "<%=this.txtDate.ClientID %>";
_clientvalErrMsgId = "<%=this.valErrMsg.ClientID%>";

_clientbtnPay = '<%= this.btnPay.ClientID %>';
_clientbtnPayAndPrint = '<%= this.btnPayAndPrint.ClientID %>';
_clientbtnClose = '<%= this.btnClose.ClientID %>';

_sClientlblMsg = "<%=this.lblMsg.ClientID %>";
_sClientreqPayableFor = "<%=this.reqPayableFor.ClientID %>";
_sClientreqFeeType = "<%=this.reqFeeType.ClientID %>";

_clienttrChallanRow = '<%= this.trChallanNoRow.ClientID %>';
_clienttxtChallanNo = '<%= this.txtChallanNo.ClientID %>';

// Financial year related
var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');

EnableControlsDirectlyPaid();
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(EndReqHandler);

function EndReqHandler(sender, args) {
	DisableButtons(false);

	var OptCheque = $get(_sClientchkFeePaymentId + '_1');
	var OptCard = $get(_sClientchkFeePaymentId + '_2');
	var OptCash = $get(_sClientchkFeePaymentId + '_0');
	if (OptCheque.checked) {
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienttrChequeEntry).style.display = '';
		$get(_clienttrCardEntry).style.display = 'none';
		$get(_clienthidPaymentMode).value = "By Cheque";
		$get(_sClienthidPaymentType).value = "By Cheque";
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_sClientlblBankMandatory).style.display = 'none';
		$get(_clienttrChallanRow).style.display = 'none';
	}
	else if (OptCard.checked) {
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_clienttrCardEntry).style.display = '';
		$get(_clienthidPaymentMode).value = "By Card";
		$get(_sClienthidPaymentType).value = "By Card";
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_sClientlblBankMandatory).style.display = 'none';
		$get(_clienttrChallanRow).style.display = 'none';
	}
	else {
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienthidPaymentMode).value = "By Cash";
		$get(_sClienthidPaymentType).value = "By Cash";
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_clienttrCardEntry).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = '';
		$get(_sClienttrBankName).style.width = "100%";
		$get(_sClienttrBankName).style.display = '';
		$get(_sClientlblBankMandatory).style.display = '';
		$get(_clienttrChallanRow).style.display = '';
	}
	
	VisibleOrHideControls();
}

Onload();

function Onload() {
	var OptCheque = $get(_sClientchkFeePaymentId + '_1');
	var OptCard = $get(_sClientchkFeePaymentId + '_2');
	var OptCash = $get(_sClientchkFeePaymentId + '_0');
	$get(_sClienttxtActualAmt).disabled = false;
	if ($get(_sClientGridId) == null) {
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_clienttrCardEntry).style.display = 'none';
	}
	if ($get(_clienthidPaymentMode).value == "By Cash") {
		OptCash.checked = true;
		$get(_sClienthidPaymentType).value = "By Cash";
		$get(_sClienttblEntry).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_sClienttblDCEntry).style.display = 'none';
		$get(_sClienttrDueDate).style.display = 'none';
		$get(_sClientddlFeeType).style.display = 'none';
		$get(_clientddlPayableFor).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = '';
		$get(_sClienttrBankName).style.display = '';
        $get(_clienttrChallanRow).style.display = '';
	}
	else if ($get(_clienthidPaymentMode).value == "By Card") {
		OptCard.checked = true;
		$get(_sClienthidPaymentType).value = "By Card";
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_clienthidPaymentMode).value = "By Card";
		$get(_clienttrCardEntry).style.display = '';
        $get(_clienttrChallanRow).style.display = 'none';
	}
	else if ($get(_clienthidPaymentMode).value == "By Cheque") {
		OptCheque.checked = true;
		$get(_sClienthidPaymentType).value = "By Cheque";
		$get(_clienttrChequeEntry).style.display = '';
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_clienthidPaymentMode).value = "By Cheque";
		$get(_clienttrCardEntry).style.display = 'none';
        $get(_clienttrChallanRow).style.display = 'none';
	}
	$get(_sClienttxtAmtToBePaid).disabled = true;
	VisibleOrHideControls();
}

function VisibleOrHideControls() {
	CalculateTotalAmtToBePaid();
	
	var payableAmount = $get(_sClienttxtPayableAmt);
	payableAmount = !payableAmount || payableAmount.value == '' ? 0 : parseInt(RemoveLeadingZeroes(payableAmount.value));
	
	if (payableAmount != 0) {
		var iAmt = ($get(_sClienttxtActualAmt).value);
		var AmtTobePaid = ($get(_sClienttxtAmtToBePaid).value);
		var DifferenceAmt;
		if (iAmt != AmtTobePaid && iAmt != "" && iAmt != 0 && AmtTobePaid != "") {
			iAmt = parseInt($get(_sClienttxtActualAmt).value);
			AmtTobePaid = parseInt($get(_sClienttxtAmtToBePaid).value);
			if ($get(_sClienttblEntry) != null) {
				$get(_sClienttblEntry).style.display = '';
				$get(_sClienttblDCEntry).style.display = '';
			}
			if (AmtTobePaid < iAmt) {
				$get(_sClienttrDueDate).style.display = 'none';
				DifferenceAmt = parseInt(iAmt) - parseInt(AmtTobePaid);
				$get(_sClientlblMsg).innerHTML = "Excess Amount ( Rs. " + DifferenceAmt + "/-)";
			}
			else {
				$get(_sClienttrDueDate).style.display = '';
				DifferenceAmt = parseInt(AmtTobePaid) - parseInt(iAmt);
				$get(_sClientlblMsg).innerHTML = "Outstanding Amount ( Rs. " + DifferenceAmt + "/-)";
			}
		}
		else {
			$get(_sClienttblEntry).style.display = 'none';
			$get(_sClienttblDCEntry).style.display = 'none';
			$get(_sClienttrDueDate).style.display = 'none';
		}
		ShowHideControls();
	}
	else {
		$get(_sClienttblEntry).style.display = 'none';
		$get(_sClienttblDCEntry).style.display = 'none';
		$get(_sClienttrDueDate).style.display = 'none';
	}
	ShowHideControls();
}

function CalculateTotalAmtToBePaid() {
	var OptCheque = $get(_sClientchkFeePaymentId + '_1');
	var OptCash = $get(_sClientchkFeePaymentId + '_0');
	var OptCard = $get(_sClientchkFeePaymentId + '_2');
	
	var payableAmount = $get(_sClienttxtPayableAmt);
		payableAmount = !payableAmount || payableAmount.value == '' ? 0 : parseInt(RemoveLeadingZeroes(payableAmount.value));
	
	var txtConcession = $get(_sClienttxtConcessionAmt);
	
	if (payableAmount != 0) {
		var TotAmt;
		var lateFee = $get(_sClienttxtLateFeeAmt);
		lateFee = !lateFee || lateFee.value == '' ? 0 : parseInt(RemoveLeadingZeroes(lateFee.value));

		var ActualAmt = $get(_sClienttxtActualAmt).value;
		
		if (txtConcession.value == "")
			txtConcession.value = "0";
		
		var concession = parseInt(RemoveLeadingZeroes(txtConcession.value));

		TotAmt = (payableAmount + lateFee) - concession;
		
		$get(_sClienttxtAmtToBePaid).value = TotAmt;
		$get(_sClienthidAmtToBePaid).value = TotAmt;
		
		if (TotAmt < 0 && concession > TotAmt) {
			$get(_sClientcstValidateTotalFee).errormessage = "Concession amount should not be greater than amount to be paid.";
			TotAmt = payableAmount + lateFee;
			$get(_sClienttxtAmtToBePaid).value = TotAmt;
			$get(_sClienthidAmtToBePaid).value = TotAmt;
			$get(_sClienttxtConcessionAmt).value = "0";
		}
		$get(_sClienthidAmtToBePaid).value = payableAmount + lateFee;
		if ($get(_sClientGridId) == null) {
			if ($get(_sClientchkFeePaymentId + '_2').checked)
				$get(_sClienttxtActualAmt).value = ActualAmt;
		}
		else
			$get(_sClienttxtActualAmt).value = ActualAmt;
		if ($get(_clienthidPaymentMode).value == "By Cheque")
			OptCheque.checked = true;
		else if ($get(_clienthidPaymentMode).value == "By Card")
			OptCard.checked = true;
		else
			OptCash.checked = true;
	}
}

function ValidatePaymentDate(source, args) {
    var lblErrorMsg = $get("#<%=this.lblErrorMsg.ClientID %>");
    var bIsValid = true;
    args.IsValid = true;
    if ($get(_clienttxtPaymentDate).value != "") {
        var serverDate = $get(_clientServerDate).value;
        var dtStartDate;
        try 
        {
            var dt = convertvaliddate2($get(_clienttxtPaymentDate).value);           
            if (dt != "") {                
                dtStartDate = new Date(dt);
            }
            else {
                args.IsValid = false;
                bIsValid = false;
            }
        }
        catch (e) { args.IsValid = false; bIsValid = false; }
        
        var today = new Date(serverDate);
        if (today < dtStartDate) {
            args.IsValid = false;
            bIsValid = false;
        }
    }
    else if ($get(_clienttxtPaymentDate).value == "") {
        args.IsValid = false;
        bIsValid = false;
    }

   // bIsValid ? $(lblErrorMsg).show() : $(lblErrorMsg).hide();   
        return bIsValid;
}
function ValidateActualAmt(source, args) {
	args.IsValid = true;
	if ($get(_sClienttxtActualAmt).value == "") {
		args.IsValid = false;
		$get(_sClientcstActualAmt).errormessage = "Actual amount should not be blank.";
	}
	else if ($get(_sClienttxtActualAmt).value == "0") {
		if ($get(_sClienttxtConcessionAmt).value == "" || $get(_sClienttxtConcessionAmt).value == 0) {
			args.IsValid = false;
			$get(_sClientcstActualAmt).errormessage = "Actual amount should not be zero.";
		}
	}
	
	if (!args.IsValid)
		DisableButtons(false);
	
	return !args.IsValid;
}
function ValidateConcessionAmt() {
	var TotAmt;
	TotAmt =
	parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value)) -
	parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value));
	if ($get(_sClienttxtConcessionAmt).value == "") {
		$get(_sClienttxtConcessionAmt).value = "0";
	}
	if (TotAmt < 0 && parseInt(RemoveLeadingZeroes($get(_sClienttxtConcessionAmt).value)) > $get(_sClienttxtAmtToBePaid).value) {
		$get(_sClientcstValidateTotalFee).errormessage =
		"Concession amount should not be greater than amount to be paid.";
		TotAmt = parseInt(RemoveLeadingZeroes($get(_sClienttxtPayableAmt).value)) +
				parseInt(RemoveLeadingZeroes($get(_sClienttxtLateFeeAmt).value));
		$get(_sClienttxtAmtToBePaid).value = TotAmt;
		$get(__sClienthidAmtToBePaid).value = TotAmt;
		$get(_sClienttxtConcessionAmt).value = "0";
	}
}

function ValidateChequeNo(source, args) {
	args.IsValid = true;
	if ($get(_clienttrChequeEntry).style.display != 'none') {
		if ($get(_clienttxtChequeNo).value == "") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function ValidateBankNameDirectlyPaid(source, args) {
	args.IsValid = true;
	if ($get(_sClienttrBankName).style.display != 'none') {
		if ($get(_sClientchkDirectlyPaid).checked) {
			if ($get(_sClientddlBankNameDirectlyPaid).value == "0") {
				args.IsValid = false;
				DisableButtons(false);
			}
		}
	}
	
	return !args.IsValid;
}

function ValidateBankName(source, args) {
	args.IsValid = true;
	if ($get(_clienttrChequeEntry).style.display != 'none') {
		if ($get(_clienttxtBankName).value == "0") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function ValidateSwapNo(source, args) {
	args.IsValid = true;
	if ($get(_clienttrCardEntry).style.display != 'none') {
		if ($get(_clienttxtSwapNumber).value.trim() == "") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function ValidateBankNameCard(source, args) {
	args.IsValid = true;
	if ($get(_clienttrCardEntry).style.display != 'none') {
		if ($get(_clientddlBankNameCard).value == "0") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function ValidateChequeDate(source, args) {
	args.IsValid = true;
	if ($get(_clienttrChequeEntry).style.display != 'none') {
		if ($get(_clienttxtDate).value == "") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function ValidateCardType(source, args) {
	args.IsValid = true;
	if ($get(_clienttrCardEntry).style.display != 'none') {
		if ($get(_clientddlCardType).value == "0") {
			args.IsValid = false;
			DisableButtons(false);
		}
	}
	
	return !args.IsValid;
}

function EnableOrDisableControls() {
	ResetValidationMessages();
	var OptCash = $get(_sClientchkFeePaymentId + '_0');
	var OptCheque = $get(_sClientchkFeePaymentId + '_1');
	var OptCard = $get(_sClientchkFeePaymentId + '_2');
	var rdoNew = $get(_sClientrdoFeeType + '_0');
	if (OptCheque.checked) {
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienttrChequeEntry).style.display = '';
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_clienthidPaymentMode).value = "By Cheque";
		$get(_sClienthidPaymentType).value = "By Cheque";
		$get(_sClienttblEntry).style.display = 'none';
		$get(_sClienttblDCEntry).style.display = 'none';
		$get(_sClienttrDueDate).style.display = 'none';
		$get(_clienttrCardEntry).style.display = 'none';
		$get(_sClienttxtConcessionAmt).disabled = false;
        $get(_clienttrChallanRow).style.display = 'none';
		rdoNew.checked = true;
		ShowHideControls();
		$get(_sClienttxtActualAmt).value = $get(_sClienttxtAmtToBePaid).value;
	}
	else if (OptCard.checked) {
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = 'none';
		$get(_sClienttrBankName).style.display = 'none';
		$get(_clienthidPaymentMode).value = "By Card";
		$get(_sClienthidPaymentType).value = "By Card";
		$get(_sClienttblEntry).style.display = 'none';
		$get(_sClienttblDCEntry).style.display = 'none';
		$get(_sClienttrDueDate).style.display = 'none';
		$get(_clienttrCardEntry).style.display = '';
		$get(_sClienttxtConcessionAmt).disabled = false;
        $get(_clienttrChallanRow).style.display = 'none';
		rdoNew.checked = true;
		ShowHideControls();
		$get(_sClienttxtActualAmt).value = $get(_sClienttxtAmtToBePaid).value;
	}
	else {
		EnableControlsDirectlyPaid();
		$get(_sClienttblEntry).style.display = 'none';
		$get(_sClienttblDCEntry).style.display = 'none';
		$get(_sClienttrDueDate).style.display = 'none';
		$get(_sClienttxtActualAmt).disabled = false;
		$get(_clienthidPaymentMode).value = "By Cash";
		$get(_sClienthidPaymentType).value = "By Cash";
		$get(_clienttrChequeEntry).style.display = 'none';
		$get(_sClienttrDirectlyPaid).style.display = '';
		$get(_sClienttrBankName).style.width = "100%";
		$get(_sClienttrBankName).style.display = '';
		$get(_sClientlblBankMandatory).style.display = 'none';
		$get(_sClienttxtConcessionAmt).disabled = false;
		$get(_clienttrCardEntry).style.display = 'none';
        $get(_clienttrChallanRow).style.display = '';
		rdoNew.checked = true;
		ShowHideControls();
		$get(_sClienttxtActualAmt).value = $get(_sClienttxtAmtToBePaid).value;
}
$get("<%=this.lblErrorMsg.ClientID %>").innerHTML = "";
}

function ResetValidationMessages() {
	var valSum = $get(_clientvalErrMsgId);
	if (valSum)
		valSum.style.display = 'none';
}

function EnableControlsDirectlyPaid() {
	if ($get(_sClientchkDirectlyPaid).checked) {
		$get(_sClientlblBankMandatory).style.display = '';
		$get(_sClientddlBankNameDirectlyPaid).disabled = false;
        $get(_clienttrChallanRow).style.display = '';
        $get(_clienttxtChallanNo).disabled = false;
	}
	else {
		$get(_sClientlblBankMandatory).style.display = 'none';
		$get(_sClientddlBankNameDirectlyPaid).disabled = true;
		$get(_sClientddlBankNameDirectlyPaid).value = "0";
        $get(_clienttxtChallanNo).value = '';
        $get(_clienttxtChallanNo).disabled = true;
	}
}

function ShowHideControls() {
	var rdoNew = $get(_sClientrdoFeeType + '_0');
	var rdoExisting = $get(_sClientrdoFeeType + '_1');
	var totalAmount;
	
	if (rdoNew.checked) {
		$get(_sClientddlFeeType).style.display = 'none';
		$get(_clientddlPayableFor).style.display = 'none';
		$get(_sClienttxtFeeType).style.display = '';
		$get(_sClienttxtPayableFor).style.display = '';
	}
	
	if (rdoExisting.checked) {
		$get(_sClientddlFeeType).style.display = '';
		$get(_clientddlPayableFor).style.display = '';
		$get(_sClienttxtFeeType).style.display = 'none';
		$get(_sClienttxtPayableFor).style.display = 'none';
	}
	
	var amountToBePaid = 0;
	if ($get(_sClienttxtConcessionAmt).value == "") {
		$get(_sClienttxtConcessionAmt).value = "0";
	}
	
	if ($get(_sClienttxtPayableAmt).value != "") {
		var lateFee = $get(_sClienttxtLateFeeAmt);
		lateFee = !lateFee || lateFee.value == '' ? 0 : parseInt(RemoveLeadingZeroes(lateFee.value));
		
		var payableAmount = $get(_sClienttxtPayableAmt);
		payableAmount = !payableAmount || payableAmount.value == '' ? 0 : parseInt(RemoveLeadingZeroes(payableAmount.value));

		var concessionAmount = $get(_sClienttxtConcessionAmt);
		concessionAmount = !concessionAmount || concessionAmount.value == '' ? 0 : parseInt(RemoveLeadingZeroes(concessionAmount.value));

		totalAmount = (payableAmount + lateFee) - concessionAmount;
		$get(_sClienttxtAmtToBePaid).value = totalAmount;
		$get(_sClienthidAmtToBePaid).value = totalAmount;
		amountToBePaid = ($get(_sClienttxtAmtToBePaid).value);
	}
	else
		amountToBePaid = 0;
	
	var iAmt = ($get(_sClienttxtActualAmt).value);
	if (parseInt(amountToBePaid) < parseInt(iAmt)) {
		DifferenceAmt = parseInt(iAmt) - parseInt(amountToBePaid);
		$get(_sClientlblMsg).innerHTML = "Excess Amount (Rs. " + DifferenceAmt + "/-)";
	}
	else {
		DifferenceAmt = parseInt(amountToBePaid) - parseInt(iAmt);
		$get(_sClientlblMsg).innerHTML = "Outstanding Amount (Rs. " + DifferenceAmt + "/-)";
	}
}

function ValidateFeeType(source, args) {
	args.IsValid = true;
	if ($get(_sClienttblEntry).style.display != 'none') {
		if ($get(_sClienttxtFeeType).style.display != 'none') {
			if ($get(_sClienttxtFeeType).value == "") {
				$get(_sClientreqFeeType).errormessage = "Fee Type should not be blank.";
				args.IsValid = false;
			}
		}
		else if ($get(_sClientddlFeeType) != null) {
			if ($get(_sClientddlFeeType).value == 0) {
				$get(_sClientreqFeeType).errormessage = "Fee Type should be selected.";
				args.IsValid = false;
			}
		}
	}
	else
		$get(_sClientreqFeeType).errormessage = "";
	
	if (!args.IsValid)
		DisableButtons(false);
	
	return !args.IsValid;
}

function ValidatePayableFor(source, args) {
	args.IsValid = true;
	if ($get(_sClienttblEntry).style.display != 'none') {
		if ($get(_sClienttxtPayableFor).style.display != 'none') {
			if ($get(_sClienttxtPayableFor).value == "") {
				$get(_sClientreqPayableFor).errormessage = "Payable for should not be blank.";
				args.IsValid = false;
			}
		}
		else if ($get(_clientddlPayableFor) != null) {
			if ($get(_clientddlPayableFor).value == 0) {
				$get(_sClientreqPayableFor).errormessage = "Payable for should be selected.";
				args.IsValid = false;
			}
		}
	}
	else
		$get(_sClientreqPayableFor).errormessage = "";
	
	if (!args.IsValid)
		DisableButtons(false);
	
	return !args.IsValid;
}
		
function ValidateCheckbox() {
	var chkSelect = $('#' + _sClientGridId + '_tblHoliday input[type=checkbox]:checked');
	
	if (!(chkSelect && chkSelect.length > 0)) {
		alert('At least one fee entry should be selected for paying fee.');
		DisableButtons(false);
		return false;
	}

    return true;
}

function AccountsValidateDate(src, args) {
	args.IsValid = true;

	if (!$get(_sClientchkFeePaymentId + '_0').checked)
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
	
	if (!args.IsValid)
		DisableButtons(false);

	return !args.ISValid;
}

function DisableButtons(flag) {
	var btnPay = $get(_clientbtnPay);
	if (btnPay)
		btnPay.disabled = flag;
	
	var btnPayPrint = $get(_clientbtnPayAndPrint);
	if (btnPayPrint)
		btnPayPrint.disabled = flag;
	
	var btnClose = $get(_clientbtnClose);
	if (btnClose)
		btnClose.disabled = flag;
}
</script>
</asp:Content>