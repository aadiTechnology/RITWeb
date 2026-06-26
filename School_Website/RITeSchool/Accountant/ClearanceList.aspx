<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ClearanceList.aspx.cs" Inherits="ClearanceList" ViewStateMode="Disabled" %>

<%@ OutputCache Location="None" VaryByParam="none" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
		<table width="98%" align="center">
			<tr>
				<td align="center" valign="top">
					<asp:UpdatePanel ID="upnlSuccessMsg" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:Label ID="lblSuccessMsg" runat="server" CssClass="LblNormalImg" Font-Bold="true"
							           Font-Size="Small" ForeColor="Blue" Visible="true" ViewStateMode="Enabled"></asp:Label>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td align="left">
					<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
						<ContentTemplate>
							<asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="lblNormal" ValidationGroup="Show" ViewStateMode="Enabled"/>
							<asp:Label ID="lblError" runat="server" Visible="false" CssClass="ClsMdtStar" ViewStateMode="Enabled"></asp:Label>
							<asp:ValidationSummary ID="valSave" runat="server" CssClass="lblNormal" ValidationGroup="Save" ViewStateMode="Enabled"/>
							<asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
							                     Visible="true" ClientValidationFunction="ValidateControls" ValidationGroup="Show" ViewStateMode="Enabled"></asp:CustomValidator>
							<asp:CustomValidator ID="cstClearanceDate" Display="None" runat="server" ViewStateMode="Enabled" CssClass="ClsMdtStar"
							                     Visible="true" ClientValidationFunction="ValidateCashGridControls" ValidationGroup="Save"
							                     EnableClientScript="false"></asp:CustomValidator>
							<asp:CustomValidator ID="cstvalOnlinePayment" Display="None" runat="server" ViewStateMode="Enabled" CssClass="ClsMdtStar"
							                     Visible="true" ClientValidationFunction="ValidateOlineTransactionGridControls"
							                     ValidationGroup="Save" EnableClientScript="false"></asp:CustomValidator>
							<asp:CustomValidator ID="cstChequePayment" Display="None" runat="server" ViewStateMode="Enabled" CssClass="ClsMdtStar"
							                     Visible="true" ClientValidationFunction="ValidateChequeGridControls" ValidationGroup="Save"
							                     EnableClientScript="false"></asp:CustomValidator>
							<asp:CustomValidator ID="cstDepositBankValidator" runat="server" ViewStateMode="Enabled" Display="None" ValidationGroup="Save"
							                     ClientValidationFunction="ValidateDepositBank" />
							<asp:CustomValidator ID="cstAcValidateClearanceDate"
							                     runat="server"
                                                 ViewStateMode="Enabled"
							                     Display="None"
							                     ClientValidationFunction="AccountsValidateClearanceDate"
							                     ValidationGroup="Save"
							                     EnableClientScript="true" />
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="optPaymentDate" EventName="CheckedChanged" />
							<asp:AsyncPostBackTrigger ControlID="optClearanceDate" EventName="CheckedChanged" />
							<asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
							<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
							<asp:AsyncPostBackTrigger ControlID="optCashClearance" EventName="CheckedChanged" />
							<asp:AsyncPostBackTrigger ControlID="optChequeClearance" EventName="CheckedChanged" />
							<asp:AsyncPostBackTrigger ControlID="optCardClearance" EventName="CheckedChanged" />
							<asp:AsyncPostBackTrigger ControlID="optOnlineTransactionClearance" EventName="CheckedChanged" />
						</Triggers>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td align="center">
					<table width="100%" align="center">
						<tr>
							<td colspan="2" width="100%">
								<asp:UpdatePanel ID="UpdatePanel1" runat="server">
									<ContentTemplate>
										<table align="center" cellpadding="1" cellspacing="2" width="100%">
                                             <tr class="ClsBorderlight">
                                                <td align="left" colspan="3">
                                                    <span class="ClsLblLgnd" style="font-weight: bold;font-size:10pt">Clearance For Payment Mode :</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" class="ClsBorderlight">
                                                    <asp:RadioButton ID="optStudentFee" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Text="Student Fee" GroupName="FeeCategry" Width="150px" onchange="hideCaution(0);" />
                                                    <asp:RadioButton ID="optInternalFee" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Text="Internal Fee" GroupName="FeeCategry" onchange="hideCaution(1);" />
                                                </td>
                                            </tr>
											<tr>
												<td colspan="3" class="ClsBorderlight">
													<asp:RadioButton ID="optCashClearance" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Text="Cash" Style= "padding-right:20px"
													                 GroupName="Clearance" OnCheckedChanged="optClearance_CheckedChanged" />
													<asp:RadioButton ID="optChequeClearance" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Text="Cheque" Style= "padding-right:20px"
													                 GroupName="Clearance" OnCheckedChanged="optClearance_CheckedChanged" />
													<asp:RadioButton ID="optCardClearance" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Text="Swipe Card" Style= "padding-right:20px"
													                 GroupName="Clearance" OnCheckedChanged="optClearance_CheckedChanged" />
													<asp:RadioButton ID="optOnlineTransactionClearance" runat="server" ViewStateMode="Enabled" Text="Online Transaction" Style= "padding-right:20px"
													                 AutoPostBack="true" GroupName="Clearance" OnCheckedChanged="optClearance_CheckedChanged" />
                                                    <asp:RadioButton ID="optElectronicPaymentClearance" runat="server" ViewStateMode="Enabled" Text="Electronic (NEFT/RTGS/IMPS)" Style= "padding-right:20px"
													                 AutoPostBack="true" GroupName="Clearance" OnCheckedChanged="optClearance_CheckedChanged" />
												</td>
											</tr>
											<tr id="trChequeNo" runat="server" viewstatemode="Enabled" visible="false">
												<td class="ClsBorderlight" valign="top" style="width: 12%">
													<asp:RadioButton ID="optChequeNumber" runat="server" ViewStateMode="Enabled" GroupName="Filter" AutoPostBack="true"
													                 Checked="true" TabIndex="1" OnCheckedChanged="optChequeNumber_CheckedChanged" />
												</td>
												<td valign="top" class="ClsBorderlight" style="width: 20%">
													<span class="ClsLabel">Cheque Number :</span>
												</td>
												<td valign="top" align="left" width="70%">
													<asp:TextBox ID="txtChequeNumber" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" MaxLength="6"
													             onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
													             onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
													             ondrop="event.returnValue=false;" TabIndex="2"></asp:TextBox>
												</td>
											</tr>
											<tr id="trTransactionNumber" runat="server" viewstatemode="Enabled">
												<td class="ClsBorderlight" valign="top" style="width: 12%">
													<asp:RadioButton ID="optTransactionNumber" runat="server" ViewStateMode="Enabled" GroupName="Filter" AutoPostBack="true"
													                 Checked="true" TabIndex="1" OnCheckedChanged="optTransactionNumber_heckedChanged" />
												</td>
												<td valign="top" class="ClsBorderlight" style="width: 20%">
													<span class="ClsLabel">Transaction ID :</span>
												</td>
												<td valign="top" align="left" width="70%">
													<asp:TextBox ID="txtTransactionIDNumber" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" MaxLength="50" TabIndex="2"></asp:TextBox>
												</td>
											</tr>
											<tr id="trORChequeNo" runat="server" viewstatemode="Enabled">
												<td align="center" class="HilightBGGray" colspan="5">
													<img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
													<img src="../images/ArrowBlueDblNw.gif" />
												</td>
											</tr>
											<tr>
												<td class="ClsBorderlight" valign="top" style="width: 12%">
													<asp:RadioButton ID="optRegNo" runat="server" ViewStateMode="Enabled" AutoPostBack="true" GroupName="Filter"
													                 OnCheckedChanged="optRegNo_CheckedChanged" />
												</td>
												<td class="ClsBorderlight" valign="top" style="width: 20%">
													<span class="ClsLabel">Student Name / Reg. No. :</span>
												</td>
												<td align="left" valign="top" width="70%">
													<asp:TextBox ID="txtRegNo" runat="server" ViewStateMode="Enabled" CssClass="MidTxtBox" MaxLength="50" TabIndex="1"></asp:TextBox>
												</td>
											</tr>
											<tr id="Tr2">
												<td align="center" class="HilightBGGray" colspan="5">
													<img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
													<img src="../images/ArrowBlueDblNw.gif" />
												</td>
											</tr>
											<tr>
												<td colspan="1" valign="top" class="ClsBorderlight" style="width: 12%">
													<asp:RadioButton ID="optPaymentDate" runat="server" ViewStateMode="Enabled" AutoPostBack="true" GroupName="Filter"
													                 OnCheckedChanged="optPaymentDate_CheckedChanged" />
												</td>
												<td valign="top" colspan="2">
													<table width="100%">
														<tr>
															<td class="ClsBorderlight" style="width: 205px">
																<asp:Label class="ClsLabel" runat="server" ViewStateMode="Enabled" ID="lblPaymentDate" Text="Payment Start Date :" />
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtPaymentStartDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="2"></asp:TextBox>
																<rjs:PopCalendar ID="cFromDate" runat="server" ViewStateMode="Enabled" Control="txtPaymentStartDate" Format="dd MMM yyyy" Culture="en"
																                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid from date."
																                 ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderlight" style="width: 144px">
																<span class="ClsLabel">End Date :</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtPaymentEndDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="3"></asp:TextBox>
																<rjs:PopCalendar ID="cToDate" runat="server" ViewStateMode="Enabled" Control="txtPaymentEndDate" Format="dd MMM yyyy" Culture="en"
																                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />																
															</td>
                                                            <td id ="tdPaymentBankName" runat ="server" viewstatemode="Enabled" class="ClsBorderlight" style="width: 144px">
																<span  class="ClsLabel" >Bank Name :</span>
															</td>
															<td id ="tdcmbPaymentBankName" runat ="server" viewstatemode="Enabled" align="left" valign="top">
																<asp:DropDownList ID="cmbPaymentBank" AutoPostBack="true" CssClass="LrgCombo"
																					                  runat="server" ViewStateMode="Enabled" >
																					</asp:DropDownList>																
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr id="Tr3">
												<td align="center" class="HilightBGGray" colspan="5">
													<img src="../images/ArrowBlueDblRev.gif" /><span class="ClsHilightTextB">OR </span>
													<img src="../images/ArrowBlueDblNw.gif" />
												</td>
											</tr>
											<tr>
												<td colspan="1" valign="top" class="ClsBorderlight" style="width: 12%">
													<asp:RadioButton ID="optClearanceDate" runat="server" ViewStateMode="Enabled" AutoPostBack="true" GroupName="Filter"
													                 OnCheckedChanged="optClearanceDate_CheckedChanged" />
												</td>
												<td valign="top" colspan="2">
													<table width="100%">
														<tr>
															<td class="ClsBorderlight" style="width: 206px">
																<span class="ClsLabel">Clearance Start Date:</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtClearanceStartDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="4"></asp:TextBox>
																<rjs:PopCalendar ID="calClearanceStartDate" runat="server" ViewStateMode="Enabled" Control="txtClearanceStartDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderlight" style="width: 146px">
																<span class="ClsLabel">End Date :</span>
															</td>
															<td align="left" valign="top" style="width: 194px">
																<asp:TextBox ID="txtClearanceEndDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="5"></asp:TextBox>
																<rjs:PopCalendar ID="calClearanceEndDate" runat="server" ViewStateMode="Enabled" Control="txtClearanceEndDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid to date." />																
															</td>
                                                            <td class="ClsBorderlight" style="width: 146px">
																<span class="ClsLabel">Bank Name :</span>
															</td>
															<td align="left" valign="top">
																<asp:DropDownList ID="cmbClearanceBank" AutoPostBack="true" CssClass="LrgCombo"
																					                  runat="server" ViewStateMode="Enabled" >
																					</asp:DropDownList>															
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr>
												<td valign="top" class="ClsBorderlight" colspan="3">
													<asp:CheckBox ID="chkIncludeAll" runat="server" ViewStateMode="Enabled" AutoPostBack="false" TabIndex="6"
													              Text="Include cash payments which are cleared." />
												</td>
											</tr>
											<tr id="trCautionMoney" runat="server" viewstatemode="Enabled">
												<td valign="top" class="ClsBorderlight" colspan="3">
													<asp:CheckBox ID="chkCautionMoney" runat="server" ViewStateMode="Enabled" AutoPostBack="false" TabIndex="7"
													              Text="Include caution money details." />
												</td>
											</tr>
                                            <tr id="trGateway" runat="server" visible="false">
                                                <td valign="top" class="clsBorderLight">  
                                                    <span class="clsLabel" runat="server">GateWay :</span>
                                                </td>
                                                <td valign="top" class="clsBorderLight" colspan="3">
                                                    <asp:DropDownList ID="ddlGateway" runat="server" CssClass="MidCombo" ViewStateMode="Enabled"></asp:DropDownList>
                                                </td>
                                            </tr>
											<tr id="trCardtype" runat="server" viewstatemode="Enabled">
												<td valign="top" class="ClsBorderlight" style="width: 12%">
													<asp:Label ID="lblCardType" runat="server" ViewStateMode="Enabled" Text="Swipe Card Type :" TabIndex="6" />
												</td>
												<td colspan="2" valign="top" class="ClsBorderlight">
													<asp:DropDownList ID="cmbCardType" runat="server" ViewStateMode="Enabled" AutoPostBack="false" TabIndex="10">
													</asp:DropDownList>
												</td>
											</tr>                                            
											<tr>
												<td align="center" valign="top" colspan="3">
													<asp:Button ID="btnShow" runat="server" ViewStateMode="Enabled" Text="Show" CssClass="ClsBtn" TabIndex="7"
													            Width="100px" ValidationGroup="Show" OnClick="btnShow_Click" />
													<asp:Button ID="btnImportMIS" runat="server" ViewStateMode="Enabled" Text="Import MIS" CssClass="ClsBtn" TabIndex="8"
													            Width="100px" Visible="false" CausesValidation="false" />
												</td>
											</tr>
											<tr>
												<td align="right" valign="top" colspan="3">
													<table id="Table1" runat="server" width="100%">
														<tr>
															<td align="center" colspan="3">
																<table id="tblLegend" runat="server" viewstatemode="Enabled" align="center" visible="false">
																	<tr>
																		<td align="left" colspan="1">																			
                                                                                       <span class="ClsLblLgnd" style="font:Bold;height:16px" >Legend</span>
																		</td>
																		<td align="left" colspan="1">
																			&nbsp;<asp:Label ID="txtUserStop" runat="server" ViewStateMode="Enabled" BackColor="LightBlue" Height="20px"
																			                 BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
																			                 EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
																		</td>
																		<td align="left" colspan="1">																			
                                                                                       <span class="ClsTextNormal" style="font:Bold" >Caution Money Received</span>
																		</td>
																		<td align="left" colspan="1">
																			&nbsp;<asp:Label ID="Label1" runat="server" BackColor="LightPink" Height="20px"
																			                 BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"
																			                 EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
																		</td>
																		<td align="left" colspan="1">																			
                                                                                       <span class="ClsTextNormal" style="font:Bold" >Caution Money Returned</span>
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
														<tr runat="server" viewstatemode="Enabled" id="trTotalRec" align="center" visible="false">
															<td colspan="6">
																<asp:Label ID="lblStartIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
																<span class="LblNormal">To</span>
																<asp:Label ID="lblEndIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
																<span class="LblNormal">Out Of</span>
																<asp:Label ID="lblTotal" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
																<span class="LblNormal">Records</span>
															</td>
														</tr>
														<tr>
															<td align="right" valign="top" colspan="3">
																<asp:GridView ID="grdvwClearedCash" runat="server" ViewStateMode="Enabled" Width="100%" AutoGenerateColumns="False"
																              AllowSorting="false" CellPadding="2" CellSpacing="1" ForeColor="#333333" GridLines="None"
																              BackColor="White" CssClass="GridBorder" AllowPaging="True" EmptyDataRowStyle-HorizontalAlign="Center"
																              EmptyDataText="No Record Found" TabIndex="8" DataKeyNames="Receipt_Number,NetBankingPaymentTransactionID,StudentCardPaymentDetailsId,PostDated_Cheque_Id,Bank_Id,Student_Id,
                                                                              Payment_Cheque_Id,DepositBankId,IsReturnPayment,StudentElectronicPaymentId,TransactionNumber,IsCautionMoneyPayment"
																              OnPageIndexChanging="grdvwClearedCash_PageIndexChanging" OnRowDataBound="grdvwClearedCash_RowDataBound">
																	<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
																	</PagerStyle>
																	<Columns>
																		<asp:BoundField HeaderText="Reg.No." DataField="RegNo">
																			<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="70px"/>
																			<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" Width="70px"/>
																		</asp:BoundField>
																		<asp:BoundField HeaderText="Name" DataField="StudentName" ItemStyle-Wrap="false">
																			<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="230px"/>
																			<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" Width="270px"/>
																		</asp:BoundField>
																		<asp:BoundField HeaderText="Class" DataField="ClassName">
																			<ItemStyle Width="75px" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" />
																			<HeaderStyle Width="75px" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false"
																			             CssClass="paddingLSML" />
																		</asp:BoundField>
																		<asp:TemplateField HeaderText="TPSLTransaction ID">
																			<ItemTemplate>
																				<asp:TextBox ID="txtTSPLTransactionID" runat="server" ViewStateMode="Enabled" style="text-align: center" CssClass="SmlTxtBox" Width="250px"
																				             MaxLength="30" TabIndex="8" Text='<%# Eval("TPSLTransactionID") %>' onblur="extractNumber(this,0,false);"
																				             onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
																				             onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>
																			</ItemTemplate>
																			<ItemStyle Width="95px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																		<asp:TemplateField HeaderText="Chq. No.">
																			<ItemTemplate>
																				<asp:TextBox ID="txtChequeNo" runat="server" ViewStateMode="Enabled" style="text-align: center" CssClass="SmlTxtBox" Width="100px" MaxLength="6"
																				             TabIndex="8" Text='<%# Eval("Cheque_Number") %>' onblur="extractNumber(this,0,false);"
																				             onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
																				             onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>
																			</ItemTemplate>
																			<ItemStyle Width="70px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="70px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																		<asp:BoundField HeaderText="Bank" DataField="Bank_Name">
																			<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="15%" />
																			<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" CssClass="paddingLSML" />
																		</asp:BoundField>
                                                                        <asp:BoundField HeaderText="Receipt No." DataField="Receipt_Number">
																			<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Width="70px" />
																			<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="true" CssClass="paddingLSML" />
																		</asp:BoundField>
																		<asp:BoundField HeaderText="Amount" DataField="Amount">
																			<ItemStyle Width="60px" HorizontalAlign="Right" VerticalAlign="Middle" CssClass="ClspaddingR" />
																			<HeaderStyle Width="60px" HorizontalAlign="Right" VerticalAlign="Middle" Wrap="false" CssClass="ClspaddingR" />
																		</asp:BoundField>                                                                        
																		<asp:BoundField HeaderText="Paid For" DataField="Payable_For">
																			<ItemStyle Width="350px" HorizontalAlign="left" VerticalAlign="Middle" CssClass="paddingLSML" />
																			<HeaderStyle HorizontalAlign="left" VerticalAlign="Middle" Wrap="false" CssClass="paddingLSML" />
																		</asp:BoundField>
																		<asp:BoundField HeaderText="Transaction Date" DataField="TransactionDateTime">
																			<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" />
																		</asp:BoundField>
																		<asp:TemplateField HeaderText="Cheque Date">
																			<ItemTemplate>
																				<asp:TextBox ID="txtChequeDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
                                                                                TabIndex="8" Text='<%# (!String.IsNullOrEmpty(Eval("Cheque_Date").ToString()) ? Convert.ToDateTime(Eval("Cheque_Date")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) : 
                                                                                 String.Format( new System.Globalization.CultureInfo("en-US"), "{0:C}", Eval("Cheque_Date")) )%> '></asp:TextBox>                                                                                
																				 <%--TabIndex="8" Text='<% #Convert.ToDateTime(Eval("Cheque_Date")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en"))%>'></asp:TextBox>--%>
																				<rjs:PopCalendar ID="cChqDate" runat="server" ViewStateMode="Enabled" Control="txtChequeDate" Format="dd MMM yyyy" Culture="en"
																				                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
																			</ItemTemplate>
																			<ItemStyle Width="130px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="130px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
                                                                         <asp:TemplateField HeaderText="Transaction Number">
																			<ItemTemplate>
																				<asp:TextBox ID="txtTransactionNumber" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" Width="100px"
																				             MaxLength="30" TabIndex="8" Text='<%# Eval("TransactionNumber") %>' onblur="extractNumber(this,0,false);"
																				             onkeyup="extractNumber(this,0,false);" onkeypress="return blockNonNumbers (this, event, false, false);"
																				             onpaste="event.returnValue=false;" ondrop="event.returnValue=false;"></asp:TextBox>
																			</ItemTemplate>
																			<ItemStyle Width="95px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																		<asp:TemplateField HeaderText="Paid Date">
																			<ItemTemplate>
																				<asp:TextBox ID="txtPaidDate" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																				             TabIndex="8" Text='<%# (!String.IsNullOrEmpty(Eval("Paid_Date").ToString()) ? Convert.ToDateTime(Eval("Paid_Date")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) : 
                                                                                 String.Format( new System.Globalization.CultureInfo("en-US"), "{0:C}", Eval("Paid_Date")) ) %>'></asp:TextBox>
																				<rjs:PopCalendar ID="cPaidDate" runat="server" ViewStateMode="Enabled" Control="txtPaidDate" Format="dd MMM yyyy" Culture="en"
																				                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
																			</ItemTemplate>
																			<ItemStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="120px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																		<asp:BoundField HeaderText="Payment Date" DataField="Paid_Date">
																			<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="95px" />
																			<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="false" Width="95px" />
																		</asp:BoundField>
																		<asp:TemplateField HeaderText="Clearance Date">
																			<ItemTemplate>
                                                                                <%--<asp:TextBox ID="txtclearance" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																				             TabIndex="8" Text='<%# String.Format( new System.Globalization.CultureInfo("en-US"), "{0:C}", Eval("ClearanceDate")) %> '></asp:TextBox>--%>
																				<asp:TextBox ID="txtclearance" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" MaxLength="11"
																				             TabIndex="8"                                                                                              
                                                                                             Text='<%# (!String.IsNullOrEmpty(Eval("ClearanceDate").ToString()) ? Convert.ToDateTime(Eval("ClearanceDate")).ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("en")) : 
                                                                                 String.Format( new System.Globalization.CultureInfo("en-US"), "{0:C}", Eval("ClearanceDate")) )%> '                                                                            
                                                                                             ></asp:TextBox>
																				<rjs:PopCalendar ID="cClrDate" runat="server" ViewStateMode="Enabled" Control="txtclearance" Format="dd MMM yyyy" Culture="en"
																				                 ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select valid to date." />
																			</ItemTemplate>
																			<ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																		<asp:TemplateField HeaderText="Bank Name">
																			<ItemTemplate>
																				<asp:DropDownList ID="ddlDepositedBankList" runat="server" ViewStateMode="Enabled" CssClass="MidCombo" />
																			</ItemTemplate>
																			<ItemStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																			<HeaderStyle Width="150px" HorizontalAlign="Center" VerticalAlign="Middle" />
																		</asp:TemplateField>
																	</Columns>
																	<RowStyle CssClass="ClsGridRow" />
																	<HeaderStyle CssClass="ClsGridHeader" />
																	<AlternatingRowStyle CssClass="ClsGridAltRow" />
																	<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
																	<PagerTemplate>
																		<table width="100%" cellpadding="0" cellspacing="0">
																			<tr>
																				<td width="70%" align="left" class="ClsBorderPager" valign="middle">																					
                                                                                    <span class="LblNrmlB">Select a page:</span>
																					<asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
																					                  OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" ViewStateMode="Enabled">
																					</asp:DropDownList>
																				</td>
																				<td width="30%" align="right" class="ClsBorderPager" valign="middle">
																					<asp:Label ID="CurrentPageLabel" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" />
																				</td>
																			</tr>
																		</table>
																	</PagerTemplate>
																</asp:GridView>
															</td>
														</tr>
														<tr>
															<td>
																<table align="center" id="tblTotalAmount" runat="server" viewstatemode="Enabled" visible="false">
																	<tr>
																		<td style="background-color: #e4efc4;" align="left">
																			<span class="LblNrmlB" style="width: 200px">Total Amount :</span>
																		</td>
																		<td align="left" style="background-color: #eaeaea">
																			<asp:Label ID="lblTotalAmount" Width=" 75px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL" />
																		</td>
																	</tr>
																</table>
															</td>
														</tr>
													</table>
													<asp:HiddenField ID="hidPageNo" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidRowCnt" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidReceiptNo" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidCurrentDate" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidServerDate" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidFinancialYearJSON" runat="server" ViewStateMode="Enabled" />
													<asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" ViewStateMode="Enabled" />
                                                    <asp:HiddenField ID="hidBaseFinancialYearId" runat="server" ViewStateMode="Enabled" Value="0" />
												</td>
											</tr>
										</table>
									</ContentTemplate>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td align="right" width="46%">                                
								<asp:Button ID="btnSave" Text="Save" CssClass="ClsBtn" runat="server" ViewStateMode="Enabled" ValidationGroup="Save"
								            TabIndex="9" OnClick="btnSave_Click" />
							</td>
							<td>
                                <asp:Button ID="btnExportFee" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnLrg" Text="Export Fee to XML" onclick="btnExportFee_Click" Visible="false" />
								<asp:Button ID="btnExport" Text="Export" CssClass="ClsBtn" runat="server" ViewStateMode="Enabled" TabIndex="13"
								            OnClick="btnExport_Click" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<script language="javascript" type="text/javascript">
		var _clienthidRowCnt = "<%= hidRowCnt.ClientID %>";
		var _clientGrdId = "<%= grdvwClearedCash.ClientID %>";
		var _clientlblSuccessMsg = "<%= lblSuccessMsg.ClientID %>";
		var _clientlblErrorId = "<%= lblError.ClientID %>";
		var _clientoptClearanceDate = "<%= optClearanceDate.ClientID %>";
		var _clientoptPaymentDate = "<%= optPaymentDate.ClientID %>";
		var _clientClearanceStartDate = "<%= txtClearanceStartDate.ClientID %>";
		var _clientClearanceEndDate = "<%= txtClearanceEndDate.ClientID %>";
		var _clientPaymentStartDate = "<%= txtPaymentStartDate.ClientID %>";
		var _clientPaymentEndDate = "<%= txtPaymentEndDate.ClientID %>";
		var _clientbtnSave = "<%= btnSave.ClientID %>";
		var _clientbtnShow = "<%= btnShow.ClientID %>";
		var _clientoptCashClearance = "<%= optCashClearance.ClientID %>";
		var _clientoptChequeClearance = "<%= optChequeClearance.ClientID %>";
		var _clientoptCardClearance = "<%= optCardClearance.ClientID %>";
		var _clientoptOnlineTransactionClearance = "<%= optOnlineTransactionClearance.ClientID %>";

		var _clienthidPageNo = "<%= hidPageNo.ClientID %>";
		var _clientvalSumErrorMsgId = "<%= valSumErrorMsg.ClientID %>";
		var _clienthidReceiptNo = "<%= hidReceiptNo.ClientID %>";
		var _clientcstForm = "<%= cstForm.ClientID %>";
		var _clienthidCurrentDate = "<%= hidCurrentDate.ClientID %>";
		var _clientbtnExport = "<%= btnExport.ClientID %>";
	
		var _clienttrCautionMoney = "<%=this.trCautionMoney.ClientID %>"

		function hideCaution(type) {
		    if (type==0) {
		        $get(_clienttrCautionMoney).hide();
		    }
		    else if (type == 1) {
		        $get(_clienttrCautionMoney).show();
		    }
		}
        
		// Financial year related
		var _FinancialYear = eval('[' + $get('<%= this.hidFinancialYearJSON.ClientID %>').value + ']')[0];
		var _CanEditOldFinancialYear = Boolean($get('<%= this.hidCanEditOldFinancialYear.ClientID %>').value == 'true');

		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(EndReqHandler);

		function EndReqHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement;
			if (postBackElement != null &&
				(postBackElement.id == _clientbtnShow ||
					postBackElement.id == _clientbtnSave ||
						postBackElement.id == _clientoptCashClearance ||
							postBackElement.id == _clientoptChequeClearance ||
								postBackElement.id == _clientoptCardClearance ||
									postBackElement.id == _clientoptOnlineTransactionClearance)) {
				if ($get(_clientbtnExport).style.visibility == "hidden") {
					if ($get(_clientGrdId) != undefined && $get(_clientGrdId) != null) {
						var iCount = $get(_clientGrdId).rows.length - 1;
						if (iCount > 0) {
							$get(_clientbtnExport).style.visibility = "inherit";
							$get(_clientbtnSave).style.visibility = "inherit";
						}
						else {
							if ($get(_clientlblSuccessMsg) != undefined) {
								$get(_clientlblSuccessMsg).innerHTML = "";
							}
							$get(_clientbtnExport).style.visibility = "hidden";
							$get(_clientbtnSave).style.visibility = "hidden";
						}
					}
					else {
						if ($get(_clientlblSuccessMsg) != undefined) {
							$get(_clientlblSuccessMsg).innerHTML = "";
						}
						$get(_clientbtnExport).style.visibility = "hidden";
						$get(_clientbtnSave).style.visibility = "hidden";
					}
				}
				else {
					if ((postBackElement != null && postBackElement.id == _clientbtnSave)) {
						$get(_clientbtnExport).style.visibility = "inherit";
						$get(_clientbtnSave).style.visibility = "inherit";
						if ($get(_clientGrdId) != undefined && $get(_clientGrdId) != null) {
							var iCount = $get(_clientGrdId).rows.length - 1;
							if (iCount > 0) {
								$get(_clientbtnExport).style.visibility = "inherit";
								$get(_clientbtnSave).style.visibility = "inherit";
							}
							else {
								if ($get(_clientlblSuccessMsg) != undefined) {
									$get(_clientlblSuccessMsg).innerHTML = "";
								}
								$get(_clientbtnExport).style.visibility = "hidden";
								$get(_clientbtnSave).style.visibility = "hidden";
							}
						}
						else {
							if ($get(_clientlblSuccessMsg) != undefined) {
								$get(_clientlblSuccessMsg).innerHTML = "";
							}
							$get(_clientbtnExport).style.visibility = "hidden";
							$get(_clientbtnSave).style.visibility = "hidden";
						}
					}
					else {
						if ($get(_clientlblSuccessMsg) != undefined) {
							$get(_clientlblSuccessMsg).innerHTML = "";
						}
						$get(_clientbtnExport).style.visibility = "hidden";
						$get(_clientbtnSave).style.visibility = "hidden";
					}
				}
			}
		}

		function MessageAboutDate(oCmb) {
			var bIsValid;
			if (window.confirm('If you change the page then entered data from current page will get lost. Do you want to continue?'))
				bIsValid = true;
			else {
				$get(oCmb).value = $get(_clienthidPageNo).value;
				bIsValid = false;
			}
			return bIsValid;
		}

		function ValidateCashGridControls(oSrc, args) {

			if ($get(_clientlblSuccessMsg) != undefined) {
				$get(_clientlblSuccessMsg).innerHTML = "";
			}

			if ($get(_clientlblErrorId) != undefined) {
				$get(_clientlblErrorId).innerHTML = "";
			}
			oSrc.errormessage = "";
			var iRowCount = $get(_clienthidRowCnt).value;
			var iRowNoP = "";
			var iRowNos = "";
			var dtToday;
			var sBlankMessage = "";
			$get(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy");
			var TodayDate = $get(_clienthidCurrentDate).value;

			for (var i = 1; i <= iRowCount; i++) {
				if (i < 9) {
					sRow = "_ctl0" + (i + 1) + "_txtclearance";
					sRowPayment = "_ctl0" + (i + 1) + "_txtPaidDate";
					var PaymentDate = $get(_clientGrdId + sRowPayment);
					var txtClearanceDate = $get(_clientGrdId + sRow);
					if ((PaymentDate).value == "") {
						sBlankMessage += i.toString() + ", ";

					}

					if ((PaymentDate).value != "" && (txtClearanceDate).value != "") {

						var DateOfPayment = new Date(convertvaliddate(PaymentDate.value));
						var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value));
						if (document.all)
							dtToday = new Date(TodayDate.replace('-', ' '));
						else
							dtToday = new Date(convertdate(TodayDate));

						if (DateOfPayment > DateOfClearance)
							iRowNos += i.toString() + ", ";
						else if (dtToday < DateOfClearance)
							iRowNoP += i.toString() + ", ";
					}
				}
				else {
					sRow = "_ctl" + (i + 1) + "_txtclearance";
					sRowPayment = "_ctl" + (i + 1) + "_txtPaidDate";
					var PaymentDate = $get(_clientGrdId + sRowPayment);
					var txtClearanceDate = $get(_clientGrdId + sRow);

					if ((PaymentDate).value == "") {
						sBlankMessage += i.toString() + ", ";

					}

					if ((PaymentDate).value != "" && (txtClearanceDate).value != "") {
						var DateOfPayment = new Date(convertvaliddate(PaymentDate.value));
						var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value));
						if (document.all)
							dtToday = new Date(TodayDate.replace('-', ' '));
						else
							dtToday = new Date(convertdate(TodayDate));

						if (DateOfPayment > DateOfClearance)
							iRowNos += i.toString() + ", ";
						else if (dtToday < DateOfClearance)
							iRowNoP += i.toString() + ", ";

					}
				}

			}
			if (iRowNos != "") {
				iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
				oSrc.errormessage = "Clearance date should be greater than paid date for row(s) : " + iRowNos + "<br/>";
				args.IsValid = false;
				return true;
			}
			if (iRowNoP != "") {
				iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","));
				oSrc.errormessage += "Clearance date should not be future date for row(s) : " + iRowNoP + "<br/>";
				args.IsValid = false;
				return true;
			}
			if (sBlankMessage != "") {
				sBlankMessage = sBlankMessage.substring(0, sBlankMessage.lastIndexOf(","));
				oSrc.errormessage += "Paid Date should not be blank for row(s) : " + sBlankMessage + "<br/>";
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;

		}

		function ValidateOlineTransactionGridControls(oSrc, args) {

			$get(_clienthidCurrentDate).value = new Date().format("dd-MMM-yyyy");
			if ($get(_clientlblSuccessMsg) != undefined) {
				$get(_clientlblSuccessMsg).innerHTML = "";
			}
			if ($get(_clientlblErrorId) != undefined) {
				$get(_clientlblErrorId).innerHTML = "";
			}
			oSrc.errormessage = "";
			var iRowCount = $get(_clienthidRowCnt).value;
			var TodayDate = $get(_clienthidCurrentDate).value;
			var iRowNoP = "";
			var iRowNos = "";
			var iRowChequeNo = "";
			var iChequeDate = "";
			var dtToday;

			for (i = 1; i <= iRowCount; i++) {
				if (i < 9) {
					sRow = "_ctl0" + (i + 1) + "_txtclearance";
					var TransactionDate = $get(_clientGrdId).rows[i].cells[6].innerHTML;
					var txtClearanceDate = $get(_clientGrdId + sRow);

					if ((TransactionDate).value != "" && (txtClearanceDate).value != "") {
						var sDate = TransactionDate.split("");
						var DateString = "";
						for (j = 0; j <= 10; j++) {
							if (j == 2 || j == 6)
								sDate[j] = "-";
							DateString += sDate[j];
						}
						var DateOfTransaction = new Date(convertvaliddate(DateString));
						var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value));
						if (document.all)
							dtToday = new Date(TodayDate.replace('-', ' '));
						else
							dtToday = new Date(convertdate(TodayDate));

						if (DateOfTransaction > DateOfClearance)
							iRowNos += i.toString() + ", ";
						else if (dtToday < DateOfClearance)
							iRowNoP += i.toString() + ", ";
					}
					sRow1 = "_ctl0" + (i + 1) + "_txtTSPLTransactionID";
					txtTSPLTransactionID = $get(_clientGrdId + sRow1);
					if ((txtTSPLTransactionID).value == "")
						iRowChequeNo += i.toString() + ", ";
				}
				else {
					sRow = "_ctl" + (i + 1) + "_txtclearance";
					sRow1 = "_ctl" + (i + 1) + "_txtTSPLTransactionID";
					var TransactionDate = $get(_clientGrdId).rows[i].cells[6].innerHTML;
					var txtClearanceDate = $get(_clientGrdId + sRow);

					if ((TransactionDate).value != "" && (txtClearanceDate).value != "") {
						var sDate = TransactionDate.split("");
						var DateString = "";
						for (j = 0; j <= 10; j++) {
							if (j == 2 || j == 6)
								sDate[j] = "-";
							DateString += sDate[j];
						}
						var DateOfTransaction = new Date(convertvaliddate(DateString));
						var DateOfClearance = new Date(convertvaliddate(txtClearanceDate.value));
						if (document.all)
							dtToday = new Date(TodayDate.replace('-', ' '));
						else
							dtToday = new Date(convertdate(TodayDate));

						if (DateOfTransaction > DateOfClearance)
							iRowNos += i.toString() + ", ";
						else if (dtToday < DateOfClearance)
							iRowNoP += i.toString() + ", ";
					}
					sRow1 = "_ctl" + (i + 1) + "_txtTSPLTransactionID";
					txtTSPLTransactionID = $get(_clientGrdId + sRow1);
					if ((txtTSPLTransactionID).value == "")
						iRowChequeNo += i.toString() + ", ";
				}
			}

			if (iRowChequeNo != "") {
				iRowChequeNo = iRowChequeNo.substring(0, iRowChequeNo.lastIndexOf(","));
				oSrc.errormessage = "TSPLTransactionID should not be blank for row(s) : " + iRowChequeNo + "<br/>";
				if (iRowNos != "") {
					iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
					oSrc.errormessage += "Clearance date should be greater than transaction date for row(s) : " + iRowNos + "<br/>";
				}
				if (iRowNoP != "") {
					iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","));
					oSrc.errormessage += "clearance date should not be future date for row(s) : " + iRowNoP + "<br/>";
				}
				args.IsValid = false;
				return true;
			}
			if (iRowNos != "") {
				iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
				oSrc.errormessage += "Clearance date should be greater than transaction date for row(s) : " + iRowNos + "<br/>";
				args.IsValid = false;
				return true;
			}
			if (iRowNoP != "") {
				iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","));
				oSrc.errormessage += "clearance date should not be future date for row(s) : " + iRowNoP + "<br/>";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true;
			return false;
		}

		function ValidateChequeGridControls(oSrc, args) {

			if ($get(_clientlblSuccessMsg) != undefined) {
				$get(_clientlblSuccessMsg).innerHTML = "";
			}
			if ($get(_clientlblErrorId) != undefined) {
				$get(_clientlblErrorId).innerHTML = "";
			}
			oSrc.errormessage = "";
			var iRowCnt = $get(_clienthidRowCnt).value;
			var iRowNoP = "";
			var iRowNos = "";
			var iRowChequeNo = "";
			var iChequeDate = "";
			for (i = 1; i <= iRowCnt; i++) {
				if (i < 9) {
					sRow = "_ctl0" + (i + 1) + "_txtclearance";
					sRow1 = "_ctl0" + (i + 1) + "_txtChequeDate";
					var paymentDate = $get(_clientGrdId).rows[i].cells[7].innerHTML;
					var txtBox1 = $get(_clientGrdId + sRow);
					var txtChequeDate = $get(_clientGrdId + sRow1);
					if (txtChequeDate!=null && (txtChequeDate).value == "") {
						iChequeDate += i.toString() + ", ";
					}
		            if ((txtBox1).value != "" && txtChequeDate!=null && (txtChequeDate).value != "") {
						var dpaymentDate = new Date(convertvaliddate(txtChequeDate.value));
						var dClearanceDate = new Date(convertvaliddate(txtBox1.value));
						var dserverDate = new Date($get("<%= this.hidServerDate.ClientID %>").value);
						if (dserverDate < dClearanceDate)
							iRowNoP += i.toString() + ", ";
						else if (dpaymentDate > dClearanceDate)
							iRowNos += i.toString() + ", ";
					}
					sRow = "_ctl0" + (i + 1) + "_txtChequeNo";
					txtBox1 = $get(_clientGrdId + sRow);
					if (txtBox1!=null && (txtBox1).value == "")
						iRowChequeNo += i.toString() + ", ";
				}
				else {
					sRow = "_ctl" + (i + 1) + "_txtclearance";
					sRow1 = "_ctl" + (i + 1) + "_txtChequeDate";
					var paymentDate = $get(_clientGrdId).rows[i].cells[7].innerHTML;
					var txtBox1 = $get(_clientGrdId + sRow);
					var txtChequeDate = $get(_clientGrdId + sRow1);
					if ((txtChequeDate).value == "") {
						iChequeDate += i.toString() + ", ";
					}
					if ((txtBox1).value != "" && (txtChequeDate).value != "") {
						var dpaymentDate = new Date(convertvaliddate(txtChequeDate.value));
						var dClearanceDate = new Date(convertvaliddate(txtBox1.value));
						var dserverDate = new Date($get("<%= this.hidServerDate.ClientID %>").value);
						if (dserverDate < dClearanceDate)
							iRowNoP += i.toString() + ", ";
						else if (dpaymentDate > dClearanceDate)
							iRowNos += i.toString() + ", ";
					}
					sRow = "_ctl" + (i + 1) + "_txtChequeNo";
					txtBox1 = $get(_clientGrdId + sRow);
					if ((txtBox1).value == "")
						iRowChequeNo += i.toString() + ", ";
				}
			}
			if (iRowChequeNo != "") {
				iRowChequeNo = iRowChequeNo.substring(0, iRowChequeNo.lastIndexOf(","));
				oSrc.errormessage = "Cheque number should not be blank for row(s) : " + iRowChequeNo + "<br/>";
				if (iRowNoP != "") {
					iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","));
					oSrc.errormessage += "Cheque clearance date should not be future date for row(s) : " + iRowNoP + "<br/>";
				}
				if (iRowNos != "") {
					iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
					oSrc.errormessage += "Cheque clearance date should be greater than cheque date for row(s) : " + iRowNos + "<br/>";
				}
				if (iChequeDate != "") {
					iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","));
					oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate;
				}
				args.IsValid = false;
				return true;
			}
			else if (iRowNoP != "") {
				iRowNoP = iRowNoP.substring(0, iRowNoP.lastIndexOf(","));
				oSrc.errormessage = "Cheque clearance date should not be future date for row(s) : " + iRowNoP + "<br/>";
				if (iRowNos != "") {
					iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
					oSrc.errormessage += "Cheque clearance date should be greater than cheque date for row(s) : " + iRowNos + "<br/>";
				}
				if (iChequeDate != "") {
					iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","));
					oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate;
				}
				args.IsValid = false;
				return true;
			}
			else if (iRowNos != "") {
				iRowNos = iRowNos.substring(0, iRowNos.lastIndexOf(","));
				oSrc.errormessage = "Cheque clearance date should be greater than cheque date for rows : " + iRowNos + "<br/>";
				if (iChequeDate != "") {
					iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","));
					oSrc.errormessage += "Cheque date should not be blank for row(s) : " + iChequeDate;
				}
				args.IsValid = false;
				return true;
			}
			else if (iChequeDate != "") {
				iChequeDate = iChequeDate.substring(0, iChequeDate.lastIndexOf(","));
				oSrc.errormessage = "Cheque date should not be blank for row(s) : " + iChequeDate;
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}

		function ValidateControls(oSrc, args) {
			$get(_clientcstForm).errormessage = "";
			if ($get(_clientlblSuccessMsg) != undefined) {
				$get(_clientlblSuccessMsg).innerHTML = "";
			}
			if ($get(_clientlblErrorId) != undefined) {
				$get(_clientlblErrorId).innerHTML = "";
			}
			if ($get(_clientoptClearanceDate).checked) {
				var fromDate;
				var toDate;
				if (document.all) {
					fromDate = new Date(($get(_clientClearanceStartDate).value).replace('-', ' '));
					toDate = new Date(($get(_clientClearanceEndDate).value).replace('-', ' '));
				}
				else {
					fromDate = new Date(convertdate($get(_clientClearanceStartDate).value));
					toDate = new Date(convertdate($get(_clientClearanceEndDate).value));
				}
				if (fromDate > toDate) {
					$get(_clientcstForm).errormessage = "Clearance end date should be greater than clearance start date";
					args.IsValid = false;
					return true;
				}
			}
			else if ($get(_clientoptPaymentDate).checked) {
				var fromDate;
				var toDate;
				if (document.all) {
					fromDate = new Date(($get(_clientPaymentStartDate).value).replace('-', ' '));
					toDate = new Date(($get(_clientPaymentEndDate).value).replace('-', ' '));
				}
				else {
					fromDate = new Date(convertdate($get(_clientPaymentStartDate).value));
					toDate = new Date(convertdate($get(_clientPaymentEndDate).value));
				}
				if (fromDate > toDate) {
					if ($get(_clientoptOnlineTransactionClearance).checked) {
						$get(_clientcstForm).errormessage = "Transaction end date should be greater than Transaction start date.";
					}
					else {
						$get(_clientcstForm).errormessage = "Payment end date should be greater than Payment start date.";
					}
					args.IsValid = false;
					return true;
				}
			}
			args.IsValid = true;
			return false;
		}

		function ValidateDepositBank(src, args) {
			args.IsValid = true;
			var iRowNos = [];
			var txtClearanceDate, ddlDepositedBank;
			$('tr', $('#' + _clientGrdId))
				.each(function(index) {
					if (!(this.className == "ClsGridRow" || this.className == "ClsGridAltRow"))
						return;

					txtClearanceDate = $('input[id$="_txtclearance"]', this)[0];
					ddlDepositedBank = $('select[id$="_ddlDepositedBankList"]', this)[0];

					if (ddlDepositedBank && txtClearanceDate.value.trim() != '' && ddlDepositedBank.value == '0')
						iRowNos.push(index);
				});

			if (iRowNos.length > 0) {
				args.IsValid = false;
				src.errormessage = "Deposit in should be selected for row(s) : " + iRowNos.join(', ');
			}
			return !args.IsValid;
		}

		function ClearValSum() {
			if ($get(_clientvalSumErrorMsgId) != null) $get(_clientvalSumErrorMsgId).style.display = "none";
			if ($get(_clientvalSumErrorMsgId) != undefined)
				$get(_clientvalSumErrorMsgId).innerHTML = "";
			return true;
		}

		function AccountsValidateClearanceDate(src, args) {
			args.IsValid = true;
			if (!_FinancialYear)
				return;

			if (_FinancialYear.IsClosed && !_CanEditOldFinancialYear) {
				args.IsValid = false;
				src.errormessage = 'Financial year is closed and you do not have edit access.';
			}
			else {
				var dtFinancialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/", ""), 10));
				var dtFinancialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/", ""), 10));
				var clearanceDate;
				var iRowNos = [];
				$('tr', $('#' + _clientGrdId))
					.each(function(index) {
						if (!(this.className == "ClsGridRow" || this.className == "ClsGridAltRow"))
							return;

						clearanceDate = $('input[id$="_txtclearance"]', this)[0].value.replace(/[-\.]/g , ' ');

						if (!clearanceDate || clearanceDate == '')
							return;

						clearanceDate = new Date(clearanceDate);

						if (clearanceDate < dtFinancialYearStartDate || clearanceDate > dtFinancialYearEndDate)
							iRowNos.push(index);
					});
				if (iRowNos.length > 0) {
					args.IsValid = false;
					src.errormessage = 'Clearance date should be within current financial year (i.e. from 1-April-' + dtFinancialYearStartDate.getFullYear() + ' to 31-March-' + dtFinancialYearEndDate.getFullYear() + ') for row(s) : ' + iRowNos.join(', ');
				}
			}
			return !args.ISValid;
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>