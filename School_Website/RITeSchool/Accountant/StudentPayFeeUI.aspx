<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" CodeFile="StudentPayFeeUI.aspx.cs" Inherits="StudentPayFeeUI" ViewStateMode="Disabled" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <div id="divMsg" runat="server" visible = "false" style="border:1 solid navy;background-color:#c5d0dc;color:Navy;font-size:large;margin:10px auto;width:50%">
            <span>There is issue with fee record. Please contact s/w co-ordinator.</span>
        </div>
		<table width="98%" id="tblMain" runat="server">
			<tr>
				<td>
					<asp:Panel ID="pnlInput" runat="server" Width="100%">
						<table style="width: 100%;" cellpadding="0" cellspacing="1">
							<tr>
								<td align="center" colspan="8">
									<asp:UpdatePanel runat="server" ID="UpdatePnlFeeGrid" UpdateMode="Conditional">
										<ContentTemplate>
											<table width="100%" cellpadding="0" cellspacing="5px">
                                            <tr id="trmandatormark" runat="server" >
                                            <td class="ClsMdtStar" align="left" valign="top" style="width: 70%"></td>
                                            <td align ="right" valign="top" style="width: 200px" colspan="8">
														<div style="float: right;" class="LblErrorMsg" 
                                                            id="lblMandatoryMark" runat="server" viewstatemode="Enabled" >
                                                            
															<asp:Label ID="lblMandatoryFields" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
														    *</div>
													    &nbsp;</td>
                                            </tr>
												
                                                <tr>
													<td align="center" colspan="8">
														<table runat="server" id="Table2" cellpadding="0" cellspacing="1"
														       width="100%">
															<tr>
																<td align="left">
																	<asp:ValidationSummary ID="valRegNumber" runat="server" ViewStateMode="Enabled" ShowMessageBox="False" ValidationGroup="RegNumber"
														                       ShowSummary="True" CssClass="ClsLabel" Width="292px" Height="35px" />
														<div style="float: left; padding-left: 5px; padding-right: 5px; height: 15px;" 
                                                            class="LblErrorMsg">
															<asp:Label ID="lblStuError" runat="server" ViewStateMode="Enabled"></asp:Label>
														</div>
																</td>
																<td runat="server" id="td2" align ="right">
															
																</td>
																
																<td align="right"  runat="server" id="td4">
																	<table>
                                                                    <tr>
                                                                    <td style="width:auto"><%if (!SchoolBase.Settings.IsMiniSite) %>
													<%{ %>
														<div style="height: 20px; float:left"  >
															<asp:HyperLink Font-Size="12pt" style="cursor:pointer; text-decoration:underline;" Font-Bold="true" ForeColor="Brown" ID="hlnkFeestructure" 
															               runat="server" ViewStateMode="Enabled" Text="Current Year Fee Structure"  /></div>
																		   <%} %></td>
                                                                    <td style="width:30px;"> 
                                                                    <td style="width:auto"><%if (!SchoolBase.Settings.IsMiniSite) %>
													<%{ %>
														<div style="height: 20px; float:right"  >
                                                                    <asp:HyperLink ID="hlnkNextYearFeeStructure" runat="server" ViewStateMode="Enabled" Font-Bold="true" 
                                                                            Font-Size="12pt" ForeColor="Brown" style="cursor:pointer; text-decoration:underline;" 
                                                                            Text="Next Year Fee Structure" />
                                                                            </div>
																		   <%} %>
                                                                          <%-- <div style="height: 20px; float:right"  >
                                                                    <asp:HyperLink ID="hlnkNextFeeStructure" runat="server" ViewStateMode="Enabled" Font-Bold="true" 
                                                                            Font-Size="12pt" ForeColor="Brown" style="cursor:pointer; text-decoration:underline;" 
                                                                            Text="Next Year Fee Structure" />
                                                                            </div>--%>

                                                                            </td>
                                                                    </tr>
                                                                    
                                                                    </table>
                                                                </td>                                                             
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td align="center" colspan="8">
														<table runat="server" viewstatemode="Enabled" id="tblStudentInputFields" cellpadding="0" cellspacing="1"
														       width="100%">
															<tr>
																<td align="left">
																	<table runat="server" id="Table3" cellpadding="0" cellspacing="1">
																		<tr>
																			<td align="left" class="ClsBorderlight">
																				<asp:Label ID="lblNameRegNo" CssClass="clsLabel" EnableViewState="false" runat="server" Text="<%$ Resources:LocalizedResources, NameRegNo%>"></asp:Label>
																				<span class="colonPadding clsLabel">:</span>
																			</td>
																			<td align="left" class="ClsMdtStar">
																				<asp:TextBox ID="txtRegNumber" autocomplete="off" TabIndex="1" runat="server" ViewStateMode="Enabled" MaxLength="50" CssClass="MidTxtBox" 
																				             Width="290px"></asp:TextBox>&nbsp;
																							 
																				<asp:RequiredFieldValidator ID="reqRegName" Display="None" runat="server" ViewStateMode="Enabled" ErrorMessage="<%$ Resources:LocalizedResources, NameRegNoShouldNotBeBlank%>" 
																				                            ControlToValidate="txtRegNumber" ValidationGroup="RegNumber" SetFocusOnError="true"></asp:RequiredFieldValidator>
																			</td>
																			<td align="left" class="ClsMdtStar">
																				*
																			</td>
                                                                            <td runat="server" viewstatemode="Enabled" id="tdSearch">
                                                                                <asp:Button ID="btnSearch" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Show%>"
                                                                                    TabIndex="2" CssClass="ClsBtnMid remove-margin-top" ValidationGroup="RegNumber" OnClick="btnSearch_Click" />
                                                                            </td>
																		</tr>
																	</table>
																</td>
																<td align="left" style="width: 160px" class="ClsGreenBG" runat="server" id="tdBankChallan">
																	<asp:HyperLink ID="HyperLink1" runat="server" Text="<% $ Resources:LocalizedResources, PayFeeByChallan%>" 
																	               NavigateUrl="~/RITeSchool/Admin/BankChallanImportUI.aspx" CssClass="SubTitle " />
																</td>
																<td align="left" style="width: 260px" class="ClsGreenBG" runat="server" viewstatemode="Enabled" id="tdSms">
																	<asp:HyperLink ID="hlnkSendMessage" runat="server" ViewStateMode="Enabled" Text="<% $ Resources:LocalizedResources, PendingFeesSMSMessageReminder%>" 
																	               NavigateUrl="PendingFeeStudentList.aspx" CssClass="SubTitle " />
																</td>
																<td>
																	&nbsp;
																</td>
																<td align="right" style="width: 140px" class="ClsGreenBG" runat="server" viewstatemode="Enabled" id="tdBank">
																	<asp:HyperLink ID="hlnkBankDetails" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, AddBankName%>" NavigateUrl="BankDetailsPopup.aspx"
																	               CssClass="SubTitle " Style="padding-right: 5px" />
																</td> 
                                                                <td align="right" style="width: 160px" class="ClsGreenBG" runat="server" viewstatemode="Enabled" id="tdResetRecipt">
																	<asp:HyperLink ID="hlnkReceiptNo" runat="server" ViewStateMode="Enabled" Text="Reset Receipt Number" NavigateUrl="#" 
																	               CssClass="SubTitle " Style="padding-right: 5px" />
																</td>                                                              
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td colspan="8" align="left" id="trTitle" runat="server" viewstatemode="Enabled" visible="false" style="width: 100%">
														<table border="0" id="Table1" cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
															<tr>
																<td style="height: 20px" class="ClsGrayMainTitle">
																	<table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
																		<tr>
																			<td align="center" class="MainTitleHead" style="height: 20px">
																				<asp:Label style="font-weight: bold" ID="lblOldFeeRecord" runat="server" EnableViewState="false" Text="<%$ Resources:LocalizedResources,OldFeeRecords%>"></asp:Label>
																			</td>
																		</tr>
																	</table>
																</td>
															</tr>
														</table>
													</td>
												</tr>                            
                                                <tr id="trNoDebit" runat="server" viewstatemode="Enabled" visible="false">
                                                    <td colspan="3" align="center">
                                                        <asp:Label ID="lblNoDebitEntry" runat="server" ViewStateMode="Enabled" CssClass="LblNoRecord"></asp:Label>
                                                    </td>
                                                </tr>                                                
												<tr runat="server" viewstatemode="Enabled" id="trStudents" visible="false">
													<td colspan="8">
														<table width="100%">
															<tr runat="server" viewstatemode="Enabled" id="trTotalRec" align="center" visible="false">
																<td>

                                                            		<asp:Label ID="lblStartIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                       <asp:Label ID="lblTo" runat="server" ViewStateMode="Enabled" class="LblNormal" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label>
																	<asp:Label ID="lblEndIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                       <asp:Label ID="lblOutOf" runat="server" ViewStateMode="Enabled" class="LblNormal" Text="<%$ Resources:LocalizedResources,OutOf%>"></asp:Label>
																	<asp:Label ID="lblTotal" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                       <asp:Label ID="lblRecords" runat="server" ViewStateMode="Enabled" class="LblNormal" Text="<%$ Resources:LocalizedResources,Records%>"></asp:Label>
																</td>
															</tr>
															<tr>
																<td>
																	<asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" ViewStateMode="Enabled" AutoGenerateColumns="False"
																	              Height="100%" PageSize="20" CellPadding="0" CellSpacing="1" ForeColor="#333333"
																	              GridLines="None" DataKeyNames="Yearwise_Student_Id,SchoolLeft_Date,Is_RTE_Student,Form_Number,CancellationFormNo,Schoolwise_Student_Id" Width="100%"
																	              OnRowDataBound="grdStudents_RowDataBound" ShowFooter="False" OnRowCommand="grdStudents_RowCommand"
																	              EmptyDataText="No record found." EmptyDataRowStyle-HorizontalAlign="Center" AllowPaging="true">
																		<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
																		</PagerStyle>
																		<Columns>
																			<asp:BoundField DataField="Enrolment_Number" HeaderText="<%$ Resources:LocalizedResources, RegNo%>" SortExpression="Enrolment_Number">
																				<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																				<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
																				             Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="StandardDivision" HeaderText="<%$ Resources:LocalizedResources, Class%>" SortExpression="StandardDivision">
																				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources, RollNo%>" SortExpression="Roll_No">
																				<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																				<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
																				             Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedResources, StudentName%>" SortExpression="First_Name">
																				<ItemStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																				<HeaderStyle Width="35%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
																				             Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="PeningFeeTillDate" HeaderText="<%$ Resources:LocalizedResources, DuesTillDate%>" SortExpression="PeningFeeTillDate">
																				<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" CssClass="paddingLR" />
																				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLR"
																				             Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="TotalPeningFee" HeaderText="<%$ Resources:LocalizedResources,TotalDues%>"  SortExpression="TotalPeningFee">
																				<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" CssClass="paddingLR" />
																				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLR"
																				             Wrap="False" />
																			</asp:BoundField>
																			<asp:BoundField DataField="SchoolLeft_Date" HeaderText="<%$ Resources:LocalizedResources,LeftDate%>"  SortExpression="SchoolLeft_Date">
																				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" />
																				<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False" />
																			</asp:BoundField>
																			<asp:ButtonField ButtonType="Image" CommandName="SELECT_STUDENT" HeaderText="<%$ Resources:LocalizedResources, Selects%>" 
																			                 Text="Select" ImageUrl="~/RITeSchool/images/Selection5.gif">
																				<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																			</asp:ButtonField>
																		</Columns>
																		<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
																		<RowStyle CssClass="ClsGridRow" />
																		<HeaderStyle CssClass="ClsGridHeader" />
																		<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center" />
																		<AlternatingRowStyle CssClass="ClsGridAltRow" />
																		<PagerTemplate>
																			<table width="100%" cellpadding="0" cellspacing="0">
																				<tr>
																					<td width="70%" align="left" class="ClsBorderPager" valign="middle">
																						<asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                                        <span class="colonPadding">:</span>
																						<asp:DropDownList ID="PageDropDownList" AutoPostBack="true" CssClass="LblNormal"
																						                  OnSelectedIndexChanged="PageDropDownList_SelectedIndexChanged" runat="server" ViewStateMode="Enabled" >
																						</asp:DropDownList>
																					</td>
																					<td width="30%" align="right" class="ClsBorderPager" valign="middle">
																						<asp:Label ID="CurrentPageLabel" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" />
																					</td>
																				</tr>
																			</table>
																		</PagerTemplate>
																	</asp:GridView>
																	<asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
																	                      runat="server" ViewStateMode="Enabled" SelectMethod="GetAllStudentsForFee" SortParameterName="sortExpression"
																	                      SelectCountMethod="CountStudentsForFee" EnableCaching="false" OnSelected="GrdDSobj_Selected">
																		<SelectParameters>
																			<asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
																			<asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
																			                      Type="string" />
																			<asp:Parameter DefaultValue="0" DbType="Int32" Name="aiStandardId" />
																			<asp:Parameter DefaultValue="0" DbType="Int32" Name="aiDivisionId" />
																			<asp:ControlParameter ControlID="txtRegNumber" PropertyName="Text" Name="asName" />
																		</SelectParameters>
																	</asp:ObjectDataSource>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr id="trAcademicYear" runat="server" >
													<td align="left" style="width: 850px">
														<table width="100%">
															<tr>
																<td align="left" width="100">
																	<asp:Label ID="lblacademicYr" BorderWidth="1px" BorderColor="Silver" runat="server" CssClass="ClsLabel" Text="<%$ Resources:LocalizedResources, AcademicYear%>"
																	           Visible="false" EnableViewState="False"></asp:Label>
																</td>
																<td align="left" width="100">
																	<asp:DropDownList ID="cmbAcademicYrId" runat="server" ViewStateMode="Enabled" AutoPostBack="true" Width="100px"
																	                  OnSelectedIndexChanged="cmbAcademicYrId_SelectedIndexChanged" Visible="false">
																	</asp:DropDownList>
																</td>
																<td class="ErrHeadNew" align="left">
																	<asp:Label ID="lblOldAcademicYear" runat="server" ViewStateMode="Enabled"></asp:Label>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr id="tblLegend" runat="server" viewstatemode="Enabled" visible="false">
													<td colspan="7">
														<table cellpadding="0" cellspacing="1">
															<tr>
																<td align="left" width="25px">	
                                                                 <asp:Label ID="lblLegend" runat="server" ViewStateMode="Enabled" class="ClsLblLgnd" style="font:Bold;width:50px" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label> 															
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
																<td align="left" style="padding-right: 3px" width="25px">
																	<asp:Label ID="Label15" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	           CssClass="PendingFees" EnableViewState="False" Height="20px" Text=" " Width="20px">
																		<img height="20px" src="../images/spacer.gif" width="20px" />
																	
																	</asp:Label>
																</td>
																<td align="left" width="85px">
                                                                                <asp:Label ID="lblDelayedFees" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources, DelayedFees%>"></asp:Label>
																</td>
																<td align="left" colspan="1" style="padding-right: 3px" width="30px">
																	<asp:Label ID="TextBox2" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	           CssClass="ClsGridNA" Height="17px" Text=" " Width="20px" EnableViewState="False">
																		<img src="../images/spacer.gif" width="18px" height="14px"/>
																	
																	</asp:Label>
																</td>
																<td align="left" width="85">
                                                                   <asp:Label ID="lblRefundFees" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources, RefundFees%>"></asp:Label>																	
																</td>
																<td id="tdUnclearedTransLegend" runat="server" align="left" colspan="1" style="padding-right: 3px" width="30px">
																	<asp:Label ID="Label3" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
																	           CssClass="UnclearedChq" Height="17px" Text=" " Width="20px" EnableViewState="False">
																		<img src="../images/spacer.gif" width="18px" height="14px"/>
																	
																	</asp:Label>
																</td>
																<td id="tdUnclearedTransLabel" runat="server" viewstatemode="Enabled" align="left" width="260px">
                                                                     <asp:Label ID="lblUncleardTrasanction" EnableViewState="false" runat="server" class="ClsTextNormal" style="font:Bold" Text="<%$ Resources:LocalizedResources,UnclearedTransactions%>"></asp:Label>																	
																</td>
															</tr>
														</table>
													</td>                                                   
													<td align="right" colspan="1" style="padding-right: 0px; width: 20%">
                                                        <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="20px" ID="hlnkStudentFeeChallan"
														                   NavigateUrl="#" runat="server" ViewStateMode="Enabled" Target="_blank" Visible="false" Text="Generate Challan"></asp:HyperLink>
														<asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="20px" ID="hlnkOldFeeRecord"
														               NavigateUrl="#" runat="server" ViewStateMode="Enabled" Target="_blank" Visible="false" Text="<%$ Resources:LocalizedResources,OldFeeRecords%>"></asp:HyperLink>
													</td>
												</tr>                                                
												<tr align="center">
													<td colspan="3">
														<blink>
															<asp:Label ID="lblLeaveMessage" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled" Visible="false"></asp:Label>
														</blink>
													</td>
												</tr>
												<tr id="tblStudentInfo" runat="server" viewstatemode="Enabled" visible="false">
													<td colspan="8">
														<table style="width: 100%;" cellpadding="0" cellspacing="1">
															<tr>
																<td align="left" class="ClsBorderlight">
                                                                    <asp:Label ID="lblClass" EnableViewState="false" runat="server" Text="<%$ Resources:LocalizedResources,Class%>"></asp:Label>
																	<span class="colonPadding">:</span>
																</td>
																<td align="left" class="HilightBGGray">
																	<asp:Label ID="lblStandardDivision" runat="server" CssClass="LblNrmlB" Text="" ViewStateMode="Enabled"></asp:Label>
																</td>
																<td align="left" class="ClsBorderlight">
																    <asp:Label ID="lblStudName" EnableViewState="false" runat="server" Text="<%$ Resources:LocalizedResources,StudentName%>"></asp:Label>
																	<span class="colonPadding">:</span>
																</td>
																<td align="left" class="HilightBGGray" style="width: 50%">																	
                                                                    <asp:HyperLink runat="server" id="lblStudentName"  NavigateUrl="#" Enabled = "false" ViewStateMode="Enabled"></asp:HyperLink>
																</td>
																<td align="left" class="ClsBorderlight">
																	 <asp:Label ID="lblRollNo" EnableViewState="false" runat="server" Text="<%$ Resources:LocalizedResources,RollNo%>"></asp:Label>
																	<span class="colonPadding">:</span>
																</td>
																<td align="left" class="HilightBGGray">
																	<asp:Label ID="lblRollNumber" runat="server" CssClass="LblNrmlB" Text="" ViewStateMode="Enabled"></asp:Label>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr id="trAmtToBePaid" runat="server" viewstatemode="Enabled" visible="false">
													<td colspan="8" style="width: 100%;">
														<div id="divFeesPaid" runat="server" visible="true" style="background-color: #eaeaea; width: 100%;">
															<table style="width: 100%;">
                                                               <tr id="trOnlinePaymentWaitingMsg" runat="server" visible = "false" align="center">
                                                                    <td class="ClsHilightBGB"> 
                                                                        <asp:Label ID="lblOnlineWaitingMsg" runat="server" viewstatemode="Enabled" Text="If amount is deducted from your bank account and not reflected on fee screen then please wait for 1 hour and then if required send transaction details to Software Coordinator with Message Center facility." ></asp:Label>
                                                                    </td>
                                                                </tr>
																<tr>
																	<td valign="top" align="center">
																		<div id="div4" style="float: left; width: 45%;" class="" runat="server">
																			<asp:Label ID="lblConcessionRule" runat="server" CssClass="Lbl10ptB" ViewStateMode="Enabled"></asp:Label></div>
																		<div id="div3" style="float: right; width: 55%;" class="" runat="server">
                                                                            <asp:Label ID="lblFeeDetail" runat="server" EnableViewState="false" class="ClsLabel" style="font-size:12pt;font:Bold;width:100px" Text="<%$ Resources:LocalizedResources,FeeDetails%>"></asp:Label>
																			<blink>
																				<asp:Label ID="lblLeft" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled"></asp:Label>&nbsp;                                                                    
																				<asp:Label ID="lblPDCDetails" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled"></asp:Label>&nbsp;                                                                    
																				<asp:Label ID="lblNextYearPayment" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled"></asp:Label>
																				<asp:Label ID="lblLastPayment" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled"></asp:Label>&nbsp;
                                                                                <asp:Label ID="lblStudentAbsent" runat="server" CssClass="ErrHeadNew" viewstatemode="Enabled"></asp:Label>
																			</blink>
																		</div>
																	</td>
																</tr>
																<tr>
																	<td style="width: 100%;">
																		<div style="background-color: #ffffff; width: 100%;">
																			<asp:GridView CssClass="FeeGridBorder" ID="grdFeesToBePaid" runat="server" ViewStateMode="Enabled" AutoGenerateColumns="False"
																			              Height="100%" PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333"
																			              GridLines="None" DataKeyNames="Schoolwise_Student_Fee_Id,Amount_Paid,Receipt_Number,Is_Cheque_Bounce,IsTransactionCleared,Is_Concession,RefundFeeDetailsID,
                                                                                          Is_LastRefund,Is_Arrears,IsPartialPayemnt,HeaderId,RefundReceiptNo,HideInstalment,Paid_Date,FileName"
																			              Width="100%" OnRowDataBound="grdFeesToBePaid_RowDataBound" ShowFooter="False"
																			              OnRowCommand="grdFeesToBePaid_RowCommand" EmptyDataText="No record found." EmptyDataRowStyle-HorizontalAlign="Center" OnDataBound="grdFeesToBePaid_DataBound">
																				<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
																				</PagerStyle>
																				<PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
																				               FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast">
																				</PagerSettings>
																				<Columns>
																					<asp:TemplateField>																						
																						<ItemTemplate>
																							<asp:CheckBox ID="ChkBoxStudentPay" runat="server" ViewStateMode="Enabled" OnCheckedChanged="ChkBoxStudentPay_Checked" 
																							              AutoPostBack="true" />                                                                                            
																						</ItemTemplate>
																						<ItemStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
																						<HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
																					</asp:TemplateField>
																					<asp:TemplateField>
																						<HeaderTemplate>
																							<input id="ChkAllDel" type="checkbox" runat="server" viewstatemode="Enabled" onclick="CheckOrUncheckAllChkBox()" />                                                                                          
																						</HeaderTemplate>
																						<ItemTemplate>
																							<asp:CheckBox ID="ChkBoxPay" runat="server" ViewStateMode="Enabled" onclick="OnChkBoxPay()" /> 
                                                                                            <asp:RadioButton ID="rdoPayFee" runat="server" ViewStateMode="Enabled" Visible = "false" onclick="CleareOtherRadioButton(this.id)" />                                                                                           
																						</ItemTemplate>
																						<ItemStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
																						<HeaderStyle Width="1%" HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" />
																					</asp:TemplateField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources,FeeType%>" DataField="Fee_Type" SortExpression="Fee_Type">
																						<ItemStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle" Wrap="False" />
																						<HeaderStyle HorizontalAlign="Left" CssClass="paddingLSML" VerticalAlign="Middle"
																						             Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources,PayableFor%>" DataField="Payable_For" SortExpression="Payable_For">
																						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML" Wrap="true" />
																						<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="paddingLSML"
																						             Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources,Amount%>" SortExpression="Amount" DataField="Amount">
																						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" CssClass="paddingLR" Wrap="False" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="paddingLR"
																						             Width="5%" Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources,DueDate%>" SortExpression="Paid_Date" DataField="Paid_Date"
																					                DataFormatString="{0:dd MMM yyyy}">
																						<ItemStyle HorizontalAlign="center" CssClass="" VerticalAlign="Middle" Wrap="False" />
																						<HeaderStyle HorizontalAlign="center" CssClass="" VerticalAlign="Middle" Wrap="False" />  
																					</asp:BoundField>
                                                                                    <asp:BoundField HeaderText="<%$ Resources:LocalizedResources,PaidDate%>"  DataField="Paid_Date1"
																					                DataFormatString="{0:dd MMM yyyy}">
																						<ItemStyle HorizontalAlign="center" CssClass="" VerticalAlign="Middle" Wrap="False" />
																						<HeaderStyle HorizontalAlign="center" CssClass="" VerticalAlign="Middle" Wrap="False" />  
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources,AmtPaid%>" SortExpression="Amount_Paid" DataField="Amount_Paid"
																					                FooterText="1">
																						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR"
																						             Width="5%" />
																						<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, AmtPayable%>" SortExpression="Amount_Payable" DataField="Amount_Payable"
																					                FooterText="2" NullDisplayText="-">
																						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																						<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR"
																						             Width="5%" />
																						<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, LateFee%>" SortExpression="Late_Fee_Amt" DataField="Late_Fee_Amt"
																					                FooterText="2" NullDisplayText="-">
																						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR"
																						             Width="5%" />
																						<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" CssClass="paddingLR" />
																					</asp:BoundField>
																					<asp:ButtonField ButtonType="Image" CommandName="Edit_FeeDetails" HeaderText="<%$ Resources:LocalizedResources, Edit%>"
																					                 Text="Edit" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																					</asp:ButtonField>
																					<asp:ButtonField ButtonType="Image" CommandName="Delete_FeeDetails" HeaderText="<%$ Resources:LocalizedResources, Delete%>"
																					                 Text="Delete" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																					</asp:ButtonField>
																					<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Print%>">
																						<ItemTemplate>
																							<asp:HyperLink ID="lnkMini" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Receipt%>" Visible="true" NavigateUrl="FeesMiniReceipt.aspx?"/>
                                                                                            <asp:HyperLink ID="lnkRefundRecpt" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Receipt%>" Visible="true" NavigateUrl="FeesMiniReceipt.aspx?"/>
																						</ItemTemplate>
																						<ItemStyle Wrap="True" HorizontalAlign="Center" />                                                                                        
																					</asp:TemplateField>
                                                                                    <asp:ButtonField ButtonType="Image" CommandName="View" HeaderText="View"
																					                 Text="View" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
																					</asp:ButtonField>                                                                                                                                                 
																				</Columns>
																				<RowStyle CssClass="ClsMarksGridAltRowN" />
																				<HeaderStyle CssClass="ClsMarksGridHeader" />
																				<FooterStyle CssClass="ClsMarksGridHeader" />
																				<AlternatingRowStyle CssClass="ClsMarksGridAltRowN" />
																				<EmptyDataRowStyle CssClass="LblNoRecord" />
																			</asp:GridView>
																		</div>
																	</td>
																</tr>
															</table>
														</div>
													</td>
												</tr>
												<tr runat="server" id="trTotalAmt" viewstatemode="Enabled" visible="false">
													<td align="right" colspan="8">
														<table width="100%" cellpadding="0" cellspacing="1" class="ClsBorderlight" >
															<tr>
																<td align="left" class="HilightBGGray" width="10%">
                                                                    <asp:Label ID="lblTotals" EnableViewState="false" class="LblNrmlB" runat="server" Text="<%$ Resources:LocalizedResources,Total%>"></asp:Label>
																	<span class="colonPadding">:</span>
																</td>
																<td style="background-color: LightSteelBlue" align="left" width="12%">	
                                                                   <asp:Label ID="lblFeesApplicable" runat="server" class="LblNrmlB" style="width:105px" EnableViewState="false" Text="<%$ Resources:LocalizedResources,FeesApplicable%>"></asp:Label>
																   <span class="colonPadding">:</span>																       
																</td>
																<td align="left" style="background-color: #eaeaea" width="6%">
																	<asp:Label ID="lblTotalFee" Width="60px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL" />
																</td>
																<td style="background-color: #b3def2;" align="left" width="12%">
                                                                   <asp:Label ID="lblFeesPaid" runat="server" class="LblNrmlB" style="width:80px" EnableViewState="false" Text="<%$ Resources:LocalizedResources,FeesPaid%>"></asp:Label>
																   <span class="colonPadding">:</span>																	              
																</td>
																<td align="left" style="background-color: #eaeaea" width="6%">
																	<asp:Label ID="txtAmtPaid" Width="65px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightAmtPaid" />
																</td>
																<td style="background-color: powderblue;" width="12%">	
                                                                   <asp:Label ID="lblFeesPayable" runat="server" class="LblNrmlB" style="width:90px" EnableViewState="false" Text="<%$ Resources:LocalizedResources,FeesPayable%>"></asp:Label>
																   <span class="colonPadding">:</span>																		
																</td>
																<td style="background-color: #eaeaea" width="6%">
																	<asp:Label ID="txtAmtPayable" Width="60px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightAmtPayable" />
																</td>
																<td class="ClsMarksGridAltRowN" width="12%">
                                                                   <asp:Label ID="lblLateFees" runat="server" class="LblNrmlB" style="width:70px" EnableViewState="false" Text="<%$ Resources:LocalizedResources,LateFee%>"></asp:Label>
																   <span class="colonPadding">:</span>																	
																</td>
																<td style="background-color: #E6E9C7" width="6%">
																	<asp:Label ID="txtLateFee" Width=" 55px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL" />
																</td>
																<td style="background-color: #e4efc4;" width="12%" id="tdrefund" runat="server" viewstatemode="Enabled">
                                                                <asp:Label ID="lblRefundFee" runat="server" class="LblNrmlB" style="width:83px" EnableViewState="false" Text="<%$ Resources:LocalizedResources,RefundFee%>"></asp:Label>
																   <span class="colonPadding">:</span>																	         
																</td>
																<td style="background-color: #eaeaea" width=" 6%" id="tdrefundAmt" runat="server" viewstatemode="Enabled">
																	<asp:Label ID="lblRefund" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFeeL " Width="65px" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr id="trPay" runat="server" viewstatemode="Enabled" visible="false">
													<td colspan="8" align="center">
														<table width="100%">
															<tr>
																<td align="center">                                                                                 
																	<asp:Button ID="btnPayCautionMoney" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnLrg" Text="<%$ Resources:LocalizedResources, PayCautionMoney%>"
																	            OnClick="btnPayCautionMoney_Click" />
																	<asp:Button ID="btnOnlinePayment" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="<%$ Resources:LocalizedResources, PayOnline%>"
																	            ValidationGroup="PayFee" OnClick="btnOnlinePayment_Click" Visible="false" />
                                                                    <asp:Button ID="btnOnlineCautionMoneyPayment" runat="server" 
                                                                        ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="Pay Caution Money Online" 
                                                                        Visible="false" onclick="btnOnlineCautionMoneyPayment_Click" />
																	<asp:Button ID="btnLastYearFee" runat="server" 
                                                                        ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="Pay Last Year Fee Online" 
                                                                        Visible="false"/>
                                                                    <asp:Button ID="btnOnlineInternalFeePayment" runat="server" 
                                                                        ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="Pay Internal Fee Online" 
                                                                        Visible="false"/>
                                                                     <asp:Button ID="btnShowInternalFee" runat="server" 
                                                                        ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="Show Internal Fee" 
                                                                        Visible="false"/>
																	<asp:Button ID="btnPay" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" OnClick="btnPay_Click"
																	            Text="<%$ Resources:LocalizedResources, Pay%>" ValidationGroup="PayFee" />  
                                                                    <asp:Button ID="btnSibling" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnLrg" 
																	            Text="Pay Fee for Sibling" onclick="btnSibling_Click" /> 
                                                                    <asp:Button ID="btnInauguralCertificate" runat="server" ViewStateMode="Enabled" CssClass="ClsBtnMid" Text="Download Inaugural Certificate" OnClick="btnInauguralCertificate_Click" Visible="false" />
                                                                    <span id="spnPopupBlockerHelp" runat ="server"  class="ClsMdtStar" style="color: #ff0000" >Ensure that Popup blocker is off before proceeding to Pay.</span>
                                                                    <img id="imgPopupBlockerHelp" runat ="server"  onclick="PopupBlockerOpenPopup()"  src="../images/UrlHelp.png" alt="Help"
                                                                                    title="Click here to view Popup blocker details." style="cursor: pointer" />                                                                                                                                    
																</td>
                                                                <td align="right" class="ClsGreenBG" style="padding-right: 10px; width: 105px;" id="td1" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="hlnkInternalFee" runat="server" ViewStateMode="Enabled" CssClass="SubTitle" NavigateUrl="PayInternalFeePopup.aspx?"
																	               Text="<%$ Resources:LocalizedResources, InternalFee%>" />
																</td>
																<td align="right" class="ClsGreenBG" style="width: 90px;" id="tdlnkRefund" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="lnkRefund" runat="server" ViewStateMode="Enabled" CssClass="SubTitle" NavigateUrl="FeeRefundUI.aspx?"
																	               Text="<%$ Resources:LocalizedResources,Refund%>" style="padding-right: 5px" />
																</td>
																<td align="left" class="ClsGreenBG" style="width: 215px" id="tdPDCOpen" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="lnkOpenPDC" runat="server" ViewStateMode="Enabled" CssClass="SubTitle" NavigateUrl="PostDated_Cheque_Entry_PopUp.aspx?"
																	               Text="<%$ Resources:LocalizedResources, AddPostDatedChequeDetails%>" Style="padding-right: 5px"/>
																</td>
                                                                <td align="left" class="ClsGreenBG" style="width: 132px" id="tdSPOpen" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="lnkOpenSP" runat="server" CssClass="SubTitle" NavigateUrl="DebitEntryUI.aspx?" viewstatemode="Enabled"
																	               Text="<%$ Resources:LocalizedResources, StudentPayables%>" Style="padding-right: 5px"/>
																</td>
															</tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                </td>
                                                                <td align="right" class="ClsGreenBG" style="padding-right: 10px; width: 190px;white-space:nowrap;" id="tdNextYearInternalFee" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="hlnkIntFeeNextYr" runat="server" ViewStateMode="Enabled" CssClass="SubTitle" NavigateUrl="PayInternalFeePopup.aspx?" Font-Bold="true" style="white-space:nowrap;"
																	               Text="Pay Internal Fee For Next Year" />
                                                                    <img src="/images/newLink.gif" runat="server" viewstatemode="Enabled" id="img1" alt="NEW" style="white-space:nowrap;"/>
																</td>
                                                                <td align="right" class="ClsGreenBG" style="padding-right: 10px; width: 190px;white-space:nowrap;" id="tdNextYearLink" runat="server" viewstatemode="Enabled">
																	<asp:HyperLink ID="hlnkNextYr" runat="server" ViewStateMode="Enabled" CssClass="SubTitle" NavigateUrl="#" Font-Bold="true" style="white-space:nowrap;"
																	               Text="<%$ Resources:LocalizedResources, PayFeesForNextYear%>" />
                                                                    <img src="/images/newLink.gif" runat="server" viewstatemode="Enabled" id="imgNewNotice" alt="NEW" style="white-space:nowrap;"/>
																</td>
                                                            </tr>
														</table>
													</td>
												</tr>                                                
												<tr id="trCheque" runat="server" viewstatemode="Enabled" visible="false">
													<td align="center" colspan="8">
														<div id="div1" runat="server" visible="true" style="background-color: #eaeaea; width: 100%;">
															<table style="width: 100%;">
																<tr>
																	<td>
																		<div id="div2" style="float: right; width: 60%;" class="" runat="server">	
                                                                           <asp:Label ID="lblPostDatedChequeDetails" runat="server" class="ClsLabel" style="font-size:12pt;font:Bold" EnableViewState="false" Text="<%$ Resources:LocalizedResources,PostDatedChequeDetails%>"></asp:Label>																		                                                                                       
                                                                        </div>
																	</td>
																</tr>
																<tr>
																	<td style="width: 100%;">
																		<div style="background-color: #ffffff">
																			<asp:GridView CssClass="GridBorder" ID="grdPostdatedCheque" runat="server" ViewStateMode="Enabled" AutoGenerateColumns="False"
																			              Height="100%" PageSize="1100" CellPadding="0" CellSpacing="1" ForeColor="#333333"
																			              GridLines="None" DataKeyNames="PostDated_Cheque_Id,Status,Is_Cheque_Bounce" Width="100%"
																			              OnRowDataBound="grdPostdatedCheque_RowDataBound" ShowFooter="false">
																				<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
																				</PagerStyle>
																				<PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
																				               FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
																				<Columns>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeNo%>" DataField="Cheque_Number" SortExpression="Cheque_Number">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" Wrap="False" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%" Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeDate%>" SortExpression="Cheque_Date" DataField="Cheque_Date"
																					                DataFormatString="{0:dd MMM yyyy}">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" Wrap="False" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" Wrap="False" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, ChequeAmount%>" SortExpression="Cheque_Amount" DataField="Cheque_Amount">
																						<ItemStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" Width="10%"
																						           CssClass="paddingLR" />
																						<HeaderStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" Width="10%"
																						             CssClass="paddingLR" />
																						<FooterStyle HorizontalAlign="Right" VerticalAlign="Middle" Wrap="False" Width="10%"
																						             Font-Bold="true" CssClass="paddingLR" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, BankName%>" SortExpression="Bank_Name" DataField="Bank_Name">
																						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="45%"
																						           CssClass="ClspaddingL" />
																						<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" Width="45%"
																						             CssClass="ClspaddingL" />
																					</asp:BoundField>
																					<asp:BoundField HeaderText="<%$ Resources:LocalizedResources, Status%>" SortExpression="Status" DataField="Status">
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" Wrap="False" />
																						<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" Wrap="False" />
																						<FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%" Wrap="False"
																						             Font-Bold="true" />
																					</asp:BoundField>
																					<asp:TemplateField HeaderText="Pay">
																						<ItemTemplate>
																							<asp:Button ID="btnCheque" runat="server" ViewStateMode="Enabled" Text="Pay" CssClass="ClsBtnMid" />
																						</ItemTemplate>
																						<ItemStyle Wrap="False" Width="110px" HorizontalAlign="Center" />
																					</asp:TemplateField>
																				</Columns>
																				<RowStyle CssClass="ClsGridAltRow" />
																				<HeaderStyle CssClass="ClsGridHeader" />
																				<FooterStyle CssClass="ClsGridAltRow" />
																				<AlternatingRowStyle CssClass="ClsGridRow" />
																				<EmptyDataRowStyle CssClass="LblNoRecord" />
																			</asp:GridView>
																		</div>
																	</td>
																</tr>
															</table>
														</div>
													</td>
												</tr>
												<tr runat="server" viewstatemode="Enabled" id="trCheQueSummary" visible="false">
													<td align="right" colspan="8">
														<table width="100%" cellpadding="0" cellspacing="1" class="ClsBorderlight">
															<tr>
																<td align="right" style="background-color: #eaeaea; padding-right: 9px; width: 30%;">
																</td>
																<td style="background-color: #b3def2;" align="left">
                                                                 <asp:Label ID="Label1" runat="server" class="LblNrmlB" style="width: 161px" EnableViewState="false" Text="<%$ Resources:LocalizedResources, NoOfChequeDeposited%>"></asp:Label>
																 <span class="colonPadding">:</span>	
																</td>
																<td align="left" style="background-color: #eaeaea">
																	<asp:Label ID="txtChequesDeposited" Width="141px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFee" />
																</td>
																<td style="background-color: powderblue;">
                                                                <asp:Label ID="lblNoOfChkInHand" runat="server" class="LblNrmlB" style="width: 148px" EnableViewState="false" Text="<%$ Resources:LocalizedResources, NoOfChequesInHand %>"></asp:Label>
																 <span class="colonPadding">:</span>
																</td>
																<td style="background-color: #eaeaea">
																	<asp:Label ID="txtChequeInHand" Width="141px" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightFee" />
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td style="width: 850px">
                                                    <asp:HiddenField ID="hidAPopupBlockerIsDetected" runat="server" ViewStateMode="Enabled"/>	
                                                        <asp:HiddenField ID="hidSearch" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidStudentId" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidSchoolwiseStudentId" runat="server" ViewStateMode="Enabled" Value="0"/>
														<asp:HiddenField ID="hidCanEdit" runat="server" Value="N" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidQueryString" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidLastEntryId" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidStandardId" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidLeftDate" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidIsOnLeave" runat="server" ViewStateMode="Enabled"/>
														<asp:HiddenField ID="hidIsRTEStudent" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidbaseUrl" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidAreYouSureYouWantToDeleteThisRecords" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidCultureInfo" runat="server" ViewStateMode="Enabled"/>                                                        
                                                        <asp:HiddenField ID="hidUserHeUrl" runat="server" viewstatemode="Enabled"/>
                                                        <asp:HiddenField ID="hidUserHasFullAccess" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidDivisionId" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidSearchDetails" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidSiblingId" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidInternalFeeDetails" runat="server" ViewStateMode="Enabled" Value="0" 
                                                            onvaluechanged="hidInternalFeeDetails_ValueChanged" />
                                                        <asp:HiddenField ID="hidSNSSchoolId" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidIsCautionMoneyPaid" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidStudIdForCautionMoney" runat="server" ViewStateMode="Enabled"/>
                                                        <asp:HiddenField ID="hidIsOnlineInternalFeeApplicable" runat="server" ViewStateMode="Enabled" Value="0"/>
                                                        <asp:HiddenField ID="hidRestrictFeePaymentForSequence" runat="server" ViewStateMode="Enabled" Value="N"/>
                                                        <asp:HiddenField ID="hidCautionMoneyButton" runat="server" ViewStateMode="Enabled" Value="N"/>
                                                        <asp:HiddenField ID="hidFeePayable" runat="server" ViewStateMode="Enabled" Value="0"/>
                                                        <asp:HiddenField ID="hidRestrictCurrentYearPayment" runat="server" ViewStateMode="Enabled" Value="N"/>
                                                        <asp:HiddenField ID="hidBaseFinancialYearId" runat="server" ViewStateMode="Enabled" Value="0" /> 
                                                        <asp:HiddenField ID="hidStdDivId" runat="server" ViewStateMode="Enabled" Value="0" />
                                                        <asp:HiddenField ID="hidHideCautionMoneyButton" runat="server" ViewStateMode="Enabled" Value="N" />
														<asp:HiddenField ID="hidNewStdId" runat="server" ViewStateMode="Enabled" />
													   </td>
												</tr>
											</table>

                                            <div id="divSetting" runat="server" viewstatemode="Enabled" align="center" style="visibility: hidden; display: none;
                                                position: absolute; margin: 0px; padding: 0px; width: 33%; border-width: 1px;
                                                left: 10px; top: 150px; line-height: normal; border: solid 2px darkgreen; background-color: white;">
                                                <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                                                    background-repeat: repeat-x; color: Black; width: 100%; text-align: right">
                                                    <div style="font-size: 12px; width: 50%; letter-spacing: 1px; padding-left: 5px;
                                                        font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                                        Sibling Details
                                                    </div>
                                                    <span style="cursor: hand" onclick="javascript:HidePopup();">
                                                        <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                                            border="0" />
                                                    </span>
                                                </div>       
                                                <div>      
                                                    <asp:Label ID="lblStudentSiblingName" Font-Bold="true" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" 
                                                        Style="text-align: left"> </asp:Label>
                                                      <asp:ListView ID="lstvwSiblingsDetails" runat="server" ViewStateMode="Enabled" DataKeyNames="YearwiseStudentId">
                                                        <LayoutTemplate>    
                                                            <table cellpadding="0" cellspacing="1" align="center" width="95%" id="tblPagerUserDetails"
                                                                                                                        runat="server">
                                                                                                                    </table>
                                                            <table align="center" width="95%" hight="100%" runat="server" id="tblSiblingsInfo"
                                                                style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" width="10px">                                
                                
                                                                    </th>
                                                                    <th class="ClspaddingL" width="100px">
                                                                        Reg. No.
                                                                    </th>
                                                                    <th class="ClspaddingL" width="250px">
                                                                        Name
                                                                    </th>
                                                                </tr>
                                                                <tr runat="server" id="itemPlaceholder">
                                                                </tr>
                                                            </table>
                                                        </LayoutTemplate>
                                                        <ItemTemplate>
                                                            <tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>' > 
                                                                <th>
                                                                     <asp:RadioButton ID="rdoSelect" runat="server" ViewStateMode="Enabled" onclick="CheckOne(this);" AutoPostBack="false"></asp:RadioButton>
                                                                </th>
                                                                <th class="ClspaddingL">
                                                                    <asp:Label ID="lblEnrollmentNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("EnrollmentNo") %>'></asp:Label>
                                                                </th>
                                                                <th class="ClspaddingL">
                                                                    <asp:Label ID="lblSiblingName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Name") %>'></asp:Label>
                                                                </th>
                                                            </tr>
                                                        </ItemTemplate>        
                                                        <EmptyDataTemplate>
                                                        <table width="100%">
                                                            <tr>
                                                                <td class="LblNoRecord" align="center">
                                                                    <asp:Label ID="lblNoRecordFound" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, NoRecordFound%>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>        
                                                    </asp:ListView>
                                                      <div>
                                                        <table>
                                                            <tr>
                                                                <td colspan="2" align="center" valign="bottom" style="padding: 10px;">
                                                                    <asp:Button ID="btnSelect" runat="server" ViewStateMode="Enabled" Text="Select" CssClass="ClsBtnMid" CausesValidation="false" OnClick="btnSelect_Click"
                                                                        Width="75px" />
                                                                    <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="Close" CssClass="ClsBtnMid" OnClientClick="HidePopup(); return false;"
                                                                        CausesValidation="false" Width="75px"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>          
                                                </div>
                                            </div>
										</ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnInauguralCertificate" />
                                        </Triggers>
									</asp:UpdatePanel>
								</td>
							</tr>
						</table>
					</asp:Panel>
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:UpdatePanel ID="uFeepnl" runat="server" >
						<ContentTemplate>
							<table style="width: 100%;" runat="server" viewstatemode="Enabled" id="trNote" visible="false">
								<tr id="trNote1" runat="server" viewstatemode="Enabled">
									<td align="left" class="ClsBorderlight " style="background-color: #ffffc4; width: 5%;">
                                     <asp:Label ID="lblNote1" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="<%$ Resources:LocalizedResources, Note1%>"></asp:Label>
								     <span class="colonPadding">:</span>
									</td>
									<td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 78%">
										<asp:Label ID="lblVerifyNote1" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
									</td>
								</tr>
								<tr>
									<td align="left" colspan="2" style="height: 3px">
									</td>
								</tr>
								<tr id="trCautionMoneyNote" runat="server" viewstatemode="Enabled">
									<td align="left" class="ClsBorderlight " style="background-color: #ffffc4; width: 5%;">
										 <asp:Label ID="Label14" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="<%$ Resources:LocalizedResources, Note2%>"></asp:Label>
									</td>
									<td align="left" class="ClsBorderlight" style="padding-left: 5px" id="tdVerifyNote3" runat="server" viewstatemode="Enabled" >
										<asp:Label ID="lblVerifyNote3" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
									</td>
								</tr>
                                <tr id="trCautionMoneyNewNote" runat="server">
                                    <td align="left" class="ClsBorderlight" style="background-color: #ffffc4; width: 5%;">
                                        <asp:Label ID="lblCautionMoneyNewNote" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="Note:"></asp:Label>
                                    </td>
                                    <td align="left" class="ClsBorderlight" style="padding-left: 5px" id="tdVerifyNote" runat="server" viewstatemode="Enabled">
                                        <asp:Label ID="lblcautioneNote" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="The total amount for the CMD is Rs. 70,000, which is broken down as : Rs. 55,000 - Refundable, Rs. 15,000 - Non-refundable."></asp:Label>
                                    </td>
                                </tr>
								<tr id="trCautionMoneySpace" runat="server" viewstatemode="Enabled">
									<td align="left" colspan="2" style="height: 3px">
									</td>
								</tr>
								<tr id="trNote2" runat="server" viewstatemode="Enabled" visible="false">
									<td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
										 <asp:Label ID="lblNote3" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="<%$ Resources:LocalizedResources, Note3%>"></asp:Label>
								        <span class="colonPadding">:</span>
									</td>
									<td align="left" class="ClsBorderlight" style="padding-left: 5px;">
										<asp:Label ID="lblVerifyNote2" runat="server" ViewStateMode="Enabled" BorderWidth="0px" CssClass="LblSmlV"></asp:Label>
									</td>
								</tr>
                                <tr id="trNotePPSHStudent" runat="server" viewstatemode="Enabled" visible="false">
									<td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
										 <asp:Label ID="lblPPSHNote" runat="server" class="LblNrmlB" style="font-weight: bold" EnableViewState="false" Text="Note 3"></asp:Label>
								        <span class="colonPadding">:</span>
									</td>
									<td align="left" class="ClsBorderlight" style="padding-left: 5px;">
										<asp:Label ID="lblPPSHNoteData" runat="server" ViewStateMode="Enabled" BorderWidth="0px" Text="The late fee displayed here may vary later as per the revised Late fee structure." CssClass="LblSmlV"></asp:Label>
									</td>
								</tr>
							</table>
						</ContentTemplate>
					</asp:UpdatePanel>
				</td>
			</tr>
            <tr id="trNEFTDetails" runat="server" visible="false">
                 <td style="text-align:left; margin:0px suto; width:100%;">
                     <table style="font-family:Cambria; font-size:11pt; font-weight:bold;">
                         <tr>
                            <td style="height:5px;"></td>
                         </tr>
                         <tr>
                             <td colspan="2">
                                 <u>NEFT Details For Fee Payment</u>
                             </td>
                         </tr>
                         <tr>
                            <td style="height:5px;"></td>
                         </tr>
                         <tr>
                             <td style="width:100px;">
                                 Name : 
                             </td>
                             <td>
                                 PAWAR PUBLIC SCHOOL, HINJEWADI
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 A/c No. :
                             </td>
                             <td>
                                 912010033385065
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Bank name :
                             </td>
                             <td>
                                 Axis Bank Ltd.
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 Branch :
                             </td>
                             <td>
                                 Hinjewadi
                             </td>
                         </tr>
                         <tr>
                             <td>
                                 IFS Code :
                             </td>
                             <td>
                                 UTIB0001034
                             </td>
                         </tr>
                         <tr>
                             <td style="height:5px;"></td>
                         </tr>
                         <tr>
                             <td colspan="2">
                                 <span style="font-size:12pt;">Note : Mail the screen shot of successful payment and Transaction details to rohit.bhosale@ppshinjewadi.com / accountsofficer@ppshinjewadi.com</span>
                             </td>
                         </tr>
                     </table>
                 </td>
            </tr>
			<tr>
				<td align="center">
					<asp:Button ID="btnBack" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Back%>" CssClass="ClsBtnMid" PostBackUrl="~/RITeSchool/Common/ControlPanel.aspx"
					            CausesValidation="False" />
				</td>
			</tr>
		</table>
	</div>      
    
   
	<script language="javascript" type="text/javascript">

	    $(document).ready(function () {
	        AutoSearch();
	    });

	       function AutoSearch() {
	       	_clienttxtRegNumber = '#<%=txtRegNumber.ClientID%>';
		    var SchoolId = "<%=miSchoolId %>";
		    var AcademicYearId = "<%=miAcademicYearId %>"		    
		    var showLeftStudents = 1;
		    BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, null, null, null, 1);
		}
		
		function SearchSelectedValue(val) {
		    txt = document.getElementById("<%=this.txtRegNumber.ClientID %>");		    
		    bt = document.getElementById("<%=this.btnSearch.ClientID %>");
		    SearchResult(txt, val, bt);
		}

	</script>

    <script type="text/javascript">
    //This function is used to detect popup blocker status.
        function PopupBlocker() {
            var myTest = window.open("", "", "directories=no,height=10,width=10,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,top=0,location=no");
            if (!myTest) {
                if (window.confirm(document.getElementById("<%=hidAPopupBlockerIsDetected.ClientID%>").value)) {
                   window.location = '../Common/PopupBlockerUI.aspx'
                    
                }
            }
            else {
                myTest.close();
            }
        }
        window.onload = PopupBlocker;
        PopupBlocker()
    </script>

	<script language="javascript" type="text/javascript">		
		_sClientGridId = "<%= this.grdFeesToBePaid.ClientID %>";
		_sClientPayBtnId = "<%= this.btnPay.ClientID %>";
		_sClientbtnOnlinePayment = "<%= this.btnOnlinePayment.ClientID %>";
		_sClienthidStudentId = "<%= this.hidStudentId.ClientID %>";
		_sClienthidQueryString = "<%= this.hidQueryString.ClientID %>";
		_sClientOnlinePaymentBtnId = "<%= this.btnOnlinePayment.ClientID %>";
		_sClientPayCautionMoney = "<%= this.btnPayCautionMoney.ClientID %>";
		_sClienthidIsOnLeave = "<%= this.hidIsOnLeave.ClientID %>";
		_clientlblStudentSiblingName = "<%=this.lstvwSiblingsDetails.ClientID %>";
		_clienthidSNSSchoolId = "<%=this.hidSNSSchoolId.ClientID %>"
		_clientbtnOnlineCautionMoneyPayment = "<%=this.btnOnlineCautionMoneyPayment.ClientID %>"
		_clienthidRestrictFeePaymentForSequence = "<%=this.hidRestrictFeePaymentForSequence.ClientID %>"
		_clienthidRestrictCurrentYearPayment = "<%=this.hidRestrictCurrentYearPayment.ClientID %>"

		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(EndReqHandler);
		prm.add_beginRequest(BeginRequestHandler);

		//This function is used to open popun on click on link news.
		function OpenWindow(sfilepath) {
		    window.open(sfilepath, '_new', 'scrollbars=yes,resizable=yes,top=0,left=0,width=800,height=600');
		    return false;
		}

		function EndReqHandler(sender, args) {
		    var postBackElement = sender._postBackSettings.sourceElement;
		    var sEncrypt = $get(_sClienthidQueryString).value;

		    if (postBackElement != null && postBackElement.id == _sClientPayBtnId) {
		        window.open("PayFeePopUp.aspx?" + sEncrypt, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,height=650, width=900').focus();
		        return false;
		    }
		    else if (postBackElement != null && (postBackElement.id == _sClientbtnOnlinePayment || postBackElement.id == _clientbtnOnlineCautionMoneyPayment)) {
		        if ($('#' + _clienthidRestrictCurrentYearPayment).val() == "N") {
		            window.open("PayFeeOnline.aspx?" + sEncrypt, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=550').focus();
		        }
		        else {
		            alert('You cannot pay current year fee till the pending payment of last year fee.');
                }
		        return false;
		    }
		    else if (postBackElement != null && postBackElement.id == _sClientPayCautionMoney) {
		        window.open("CautionMoneyChequePopUp.aspx?" + sEncrypt, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=750,height=470').focus();
		        return false;
		    }
		    if ($get(_sClientGridId) != null && $get(_sClienthidIsOnLeave).value != "Y") {
		        if (CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'ChkBoxStudentPay', "At least one fee entry should be selected for paying fee.", 'false', 20, 'false')) {
		            if ($get(_sClientbtnOnlinePayment) != null)
		                $get(_sClientbtnOnlinePayment).disabled = false;
		        }
		    }
		    AutoSearch();
		}

		function BeginRequestHandler(sender, args) {
		    if ($get(_sClientbtnOnlinePayment) != null) 
		        $get(_sClientbtnOnlinePayment).disabled = true;
		}
		
		function CheckOrUncheckAllChkBox() {
			var grid = $get(_sClientGridId);
			var chkBox = $('input[type=checkbox][id$=ChkAllDel]', grid).get(0);
		    var checked = chkBox && chkBox.checked;   
			
			$('input[type=checkbox][id$=ChkBoxPay]:not(:disabled)', grid)
				.each(function() {
					this.checked = checked;
				});
        }        
		
		function OnChkBoxPay() {
			var grid = $get(_sClientGridId);
			
			var allChecked = $('input[type=checkbox][id$=ChkBoxPay]:not(:disabled):checked', grid).length == $('input[type=checkbox][id$=ChkBoxPay]:not(:disabled)', grid).length;
			$('input[type=checkbox][id$=ChkAllDel]', grid).get(0).checked = allChecked;
		}

		function ConfirmAction(iPageCount, sActionName) {
        
			var validationResult = true;
			if (typeof(Page_ClientValidate) == 'function')
				validationResult = Page_ClientValidate("");
			if (validationResult == false)
				return false;
			
			return CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'ChkBoxPay', sActionName, 'false', iPageCount, 'true');
		}

		function ConfirmActionForStudent(iPageCount, sActionName) {		        
			var validationResult = true;
			if (typeof(Page_ClientValidate) == 'function')
				validationResult = Page_ClientValidate("");
			if (validationResult == false)
				return false;

            var result;
			if ($get(_clienthidSNSSchoolId).value == "Y") {                
                result = CheckIfAtleastOneRadioButtonInGridIsSelected(document, _sClientGridId, 'rdoPayFee', sActionName, 'false', iPageCount, 'true');
            }			
            else
                result = CheckIfAtleastOneCheckboxInGridIsSelected(document, _sClientGridId, 'ChkBoxStudentPay', sActionName, 'false', iPageCount, 'true');

            var restrict = $('#' + _clienthidRestrictFeePaymentForSequence).val()
            if (result && restrict == 'Y') {
                var isFirst = true, index = 1, checkedIndex = 0, uncheckedIndex = 0;
                $('[Id$=_ChkBoxStudentPay]').each(function () {

                    if ($(this).prop('checked') == false && isFirst) {
                        uncheckedIndex = index
                        isFirst = false
                    }

                    if ($(this).prop('checked')) {
                        checkedIndex = index
                    }

                    index++;
                })

                if (uncheckedIndex !=0 && uncheckedIndex < checkedIndex) {
                    alert('Selection of fees should be as per sequence.')
                    result = false
                }
            }

            return result;
		}

		function ConfirmDelete() {
		    return window.confirm(document.getElementById("<%=hidAreYouSureYouWantToDeleteThisRecords.ClientID%>").value);
		}

		function clickButton(e, buttonid) {		    
			var evt = e ? e : window.event;
			var bt = $get(buttonid);
			if (bt) {
			    if (evt.keyCode == 13) {			        
			        $('ul').hide();                    		        
					bt.click();
					return false;
				}
			}
		}

		function ShowOldFeeRecord(sQryStr) {
			_sClienthlnkOldFeeRecord = "<%= this.hlnkOldFeeRecord.ClientID %>";
			if (($get(_sClienthlnkOldFeeRecord) == null) || ($get(_sClienthlnkOldFeeRecord) == "") || ($get(_sClienthlnkOldFeeRecord).disabled))
			    return false;

			window.open(sQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1200,height=1000').focus();
			return false;
		}
		function ShowStudentRecord(string) {
		    _sClienthlnkStudentRecord = "<%= this.lblStudentName.ClientID %>";
		    if (($get(_sClienthlnkStudentRecord) == null) || ($get(_sClienthlnkStudentRecord) == "") || ($get(_sClienthlnkStudentRecord).disabled))
		        return false;
		    window.open(string, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1200,height=1000').focus();
		    return false;
        }
		function CloseWindow() {
			window.close();
		}

		function blinkIt() {
			if (!document.all)
				return;
			else {
				for (i = 0; i < document.all.tags('blink').length; i++) {
					s = document.all.tags('blink')[i];
					s.style.visibility = (s.style.visibility == 'visible') ? 'hidden' : 'visible';
				}
			}
        }

		function OpenInternalFeePopup(sQueryString) {
		    window.open(sQueryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=500').focus();
		    return false;
		}

		function OpenFeeChallanPopup(aQryStr) {		    
		    window.open(aQryStr, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=600,height=500').focus();
		    return false;
		}

		function OpenSiblingPopup() {
		    _clientdivTemplates = "<%=this.divSetting.ClientID %>"
		    var x, y, tt_ovr_
		    var cssstyle = $get("<%=this.divSetting.ClientID %>").style		    
		    var pageWidth = window.screen.width
		    var pageHeight = 400
		    var left = parseInt((pageWidth / 4.5))
		    var top = parseInt((pageHeight / 1.5))
		    cssstyle.left = left + "px"
		    cssstyle.top = top + "px"
		    cssstyle.visibility = "visible"		    
		    cssstyle.display = "block"
		}

		function HidePopup() {

		    $get("<%=this.divSetting.ClientID %>").style.visibility = "hidden"
		    $get("<%=this.divSetting.ClientID %>").style.display = "none"
		    return false
		}

		//This function is used for open popup blocker help.
		function PopupBlockerOpenPopup() {

		    window.open("../Common/PopupBlockerUI.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,height=650, width=900').focus();
		}

		function OpenReceiptResetPopup() {
		    window.open("../Admin/ResetFeeReceiptPopUp.aspx", '_new', 'scrollbars=yes,resizable=no,top=0,left=0,height=500, width=750').focus();
		}

		function CheckOne(Src) {
		    var iRowCount = 0;
		    var select = $get(_clientlblStudentSiblingName + "_ctrl" + iRowCount + "_rdoSelect");
		    while (select != null) {
		        if (select.name != Src.name)
		            select.checked = false;
		        iRowCount = iRowCount + 1;
		        select = $get(_clientlblStudentSiblingName + "_ctrl" + iRowCount + "_rdoSelect");
		    }
		}

		function UpdateStaus() {
		    var _clienthidInternalFeeDetails = "<%=this.hidInternalFeeDetails.ClientID %>"
		    $get(_clienthidInternalFeeDetails).value = Math.random();
		    __doPostBack(document.getElementById(_clienthidInternalFeeDetails).name, '')
		}

		function CleareOtherRadioButton(Id) {		    
		    var grid = $get(_sClientGridId);
		    var rdoFee = document.getElementById(Id);
		    var List = grid.getElementsByTagName("input");
		    for (i = 0; i < List.length; i++) {
		        if (List[i].type == "radio" && List[i].id != rdoFee.id) {
                List[i].checked = false;
                }
            }
    }

    function ShowPendingFeeAlert(msg) {
        alert(msg);
    }

    function OpenFile(file) {
        window.open(file);
    }

	</script>
   
</asp:Content>