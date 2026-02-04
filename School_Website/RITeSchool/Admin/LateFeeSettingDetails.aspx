<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
	CodeFile="LateFeeSettingDetails.aspx.cs" Inherits="LateFeeSettingsDetalis" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <table align="center" cellpadding="2" cellspacing="2" width="97%" style="margin-top: 10px">
		<tr align="center">
			<td align="center">
				<table width="100%" align="center">
					<tr>
						<td class="ClsGrayMainTitle" style="height: 20px;" align="left">
							<asp:Label ID="lblAddAcademicYear" runat="server" CssClass="MainTitleHead" Font-Bold="True"
								Text="<%$ Resources:LocalizedResources, LateFeeSettingDetails%>" EnableViewState="false"></asp:Label>
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr align="center">
			<td align="left">
				<table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
					<tr>
						<td align="left">
							<!--lblError label insert here-->
							<asp:ValidationSummary ID="valsumLateFee" runat="server" CssClass="NewClsLabel" ShowSummary="true"
								ValidationGroup="LateFee" />
							<asp:ValidationSummary ID="deactivationValidationSummary" runat="server" CssClass="NewClsLabel"
								ShowSummary="true" ValidationGroup="Deactivation" />
						</td>
					</tr>
					<tr>
						<td align="center">
							<asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="false"
								Style="width: 100%;" />
							<asp:Label ID="lblUpateMessage" runat="server" EnableViewState="false" CssClass="ClsLabel"
								ForeColor="Blue" Font-Bold="true" Visible="false" Style="width: 100%;" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr align="center">
			<td align="left">
				<table style="width: 100%" cellpadding="0" cellspacing="1">
					<tr>
						<td colspan="2" style="padding-left: 10px" align="center">
							<div style="width: 30%">
								<table align="center" cellspacing="2px" cellpadding="2px">
									<tr>
										<td class="ClsBorderlight" align="left" style="width: 55px">
											<asp:Label ID="Label1" runat="server" CssClass="ClsHilightText" Text="<%$ Resources:LocalizedResources, Standard %>">
											           EnableViewState="False" Width="75px"></asp:Label><span id="Span2" class="colonPadding">:</span>
										</td>
										<td class="ClsHilightBG" align="left">
											<asp:Label ID="lblStandard" runat="server" Font-Bold="True"></asp:Label>
										</td>
									</tr>
								</table>
							</div>
						</td>
					</tr>
					<tr>
						<td align="center" colspan="2">
							<asp:GridView ID="grdLateFeeTypeConfig" runat="server" AutoGenerateColumns="False"
								CellPadding="3" CellSpacing="1" DataKeyNames="Fee_Type_Id,Original_Fee_Type_Id,Fee_Type,school_id,LateFeePerTypeId,ValueForType,IsStudentFeeCount,DueDateDetailsId"
								ForeColor="#333333" GridLines="None" PageSize="3" CssClass="GridBorder" EmptyDataText="<%$ Resources:LocalizedResources, NoRecordFound %>"
								EmptyDataRowStyle-HorizontalAlign="Center" OnRowCreated="grdLateFeeTypeConfig_RowCreated"
								Width="1035px" OnDataBound="grdLateFeeTypeConfig_DataBound" OnRowDataBound="grdLateFeeTypeConfig_RowDataBound">
								<PagerSettings FirstPageText="<%$ Resources:LocalizedResources, First %>" LastPageText="<%$ Resources:LocalizedResources, Last %>"
									Mode="NumericFirstLast" NextPageText="<%$ Resources:LocalizedResources, Next %>"
									Position="TopAndBottom" PreviousPageText="<%$ Resources:LocalizedResources, Previous %>" />
								<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
								<Columns>
									<asp:BoundField DataField="Fee_Type" HeaderText="<%$ Resources:LocalizedResources, FeeType %>">
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" />
									</asp:BoundField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, InstallmentName %>">
										<ItemTemplate>
											<asp:TextBox ID="txtIntervalName" runat="server" Width="140px" MaxLength="30" CssClass="MidCombo"
												Text='<%# Eval("IntervalName")%>'></asp:TextBox>
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="140px" />
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="140px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, DueDate %>">
										<ItemTemplate>
											<asp:TextBox ID="txtDueDate" runat="server" Width="80px" CssClass="MidCombo" MaxLength="11"
												AutoPostBack="false"></asp:TextBox>
											<rjs:PopCalendar ID="PopCalendar3" runat="server" Control="txtDueDate" Format="dd MMM yyyy"
												Culture="en" Visible="True" ShowWeekend="true" ShowErrorMessage="false" />
											<rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer3" runat="server"
												Calendar="PopCalendar3" Visible="false" />
											<asp:HiddenField ID="hidFeeType" runat="server" Value='<%# Eval("Fee_Type")%>' />
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, InstallmentStartDate %>">
										<ItemTemplate>
											<asp:TextBox ID="txtIntervalStartDate" runat="server" Width="80px" CssClass="MidCombo"
												MaxLength="11"></asp:TextBox>
											<rjs:PopCalendar ID="PopCalendar2" runat="server" Control="txtIntervalStartDate"
												Culture="en" Format="dd MMM yyyy" Visible="True" ShowWeekend="true" ShowErrorMessage="false" />
											<rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer2" runat="server"
												Calendar="PopCalendar2" Visible="True" />
											<asp:HiddenField ID="hidIntervalStart" runat="server" Value="" />
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, InstallmentEndDate %>"
										HeaderStyle-Width="100px">
										<ItemTemplate>
											<table>
												<tr>
													<td width="150px" align="center">
														<asp:TextBox ID="txtIntervalEndDate" runat="server" Width="80px" CssClass="MidCombo"
															MaxLength="11"></asp:TextBox>
														<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtIntervalEndDate" Format="dd MMM yyyy"
															Culture="en" Visible="True" ShowWeekend="true" ShowErrorMessage="false" />
														<rjs:PopCalendarMessageContainer ID="PopCalendarMessageContainer1" runat="server"
															Calendar="PopCalendar2" Visible="True" />
													</td>
												</tr>
											</table>
										</ItemTemplate>
										<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
										<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="110px" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, ValueForType%>" Visible="false">
										<ItemTemplate>
											<asp:TextBox ID="txtLateFeeTypePeriod" Style="padding-right: 5px; text-align: right;"
												CssClass="MidCombo" runat="server" Visible="true" Width="80px" MaxLength="3"
												onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
												onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
												ondrop="event.returnValue=false" />
										</ItemTemplate>
										<ItemStyle Width="80px" HorizontalAlign="Center" />
										<HeaderStyle Width="80px" HorizontalAlign="Center" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, LateFeeType %>" Visible="false">
										<ItemTemplate>
											<asp:DropDownList ID="cmbLateFeeType" runat="server" AutoPostBack="false" CssClass="MidCombo"
												Width="80px">
											</asp:DropDownList>
										</ItemTemplate>
										<ItemStyle Width="70px" HorizontalAlign="Center" />
										<HeaderStyle Width="70px" HorizontalAlign="Center" />
									</asp:TemplateField>
									<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, AmountRs %>" Visible="false">
										<ItemTemplate>
											<asp:TextBox ID="NumAmount" CssClass="MidCombo" Width="80px" Style="padding-right: 5px;
												text-align: right;" runat="server" Visible="true" MaxLength="3" onblur="extractNumber(this,0,false);"
												onkeyup="extractNumber(this,0,false);" Text='<%# Eval("Late_Fee")%>' onkeypress="return blockNonNumbers (this, event, false, false);"
												onpaste="event.returnValue=false" ondrop="event.returnValue=false" />
										</ItemTemplate>
										<ItemStyle Width="80px" HorizontalAlign="Center" />
										<HeaderStyle Width="80px" HorizontalAlign="Center" />
									</asp:TemplateField>
								</Columns>
								<RowStyle CssClass="ClsGridRow" />
								<PagerStyle Font-Bold="True" Font-Underline="False" ForeColor="Black" HorizontalAlign="Right" />
								<HeaderStyle CssClass="ClsGridHeader" Wrap="False" />
								<AlternatingRowStyle CssClass="ClsGridAltRow" />
								<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" HorizontalAlign="Center"
									VerticalAlign="Middle" />
							</asp:GridView>
						</td>
					</tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                 <asp:ListView ID="lstvwFeeTypes" runat="server" 
                                     onitemdatabound="lstvwFeeTypes_ItemDataBound">
                                    <LayoutTemplate>
                                        <table width="99%" style="color: #333333" cellpadding="0" cellspacing="1" class="GridBorder">
                                            <tr class="ClsGridHeader" id="trHeader" runat="server">
                                                <th align="center" style="width:30px;">
                                                    <input type="checkbox" id="chkSelectAll" onclick="CheckAll(this)" />
                                                </th>
                                                <th align="right" style="width:70px;padding-right:5px;">
                                                    Sr. No.
                                                </th>
                                                <th align="center" class="ClsLabelL">
                                                    Fee Type
                                                </th>
                                                <th align="center" style="width:150px;">
                                                    Value For Type
                                                </th>
                                                <th align="center" style="width:130px;">
                                                    Late Fee Type
                                                </th>
                                                <th align="center" style="width:100px;">
                                                    Amount
                                                </th>
                                                <th align="center" style="width:130px;">
                                                    Repeat Count
                                                </th>
                                                <th align="center" style="width:100px;">
                                                    Sort Order
                                                </th>
                                                <th align="center" style="width:170px;">
                                                    Exclude Holidays?
                                                </th>
                                                <th align="center" style="width:170px;">
                                                    Exclude Weekends?
                                                </th>
                                            </tr>
                                            <tr id="itemPlaceholder" runat="server">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr id="Tr2" runat="server" class="ClsGridRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lblSrNo" runat="server" Text="" CssClass="ClsLabel" style="float:inherit;padding-right:5px;"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbFeeType" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                            </td>
                                           <td align="center">
                                                <asp:TextBox ID="txtValueForType" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("ValueForType") %>' onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                </asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbLateFeeType" runat="server" CssClass="SmlCombo">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("Amount") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtRepeatCount" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("RepeatCount") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;"  Text='<%#Eval("SortOrder") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkExcludeHolidays" runat="server" Enabled="false" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkExcludeWeekends" runat="server" Enabled="false" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <AlternatingItemTemplate>
                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                            <td align="center">
                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lblSrNo" runat="server" Text="" CssClass="ClsLabel" style="float:inherit;padding-right:5px;"></asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbFeeType" runat="server" CssClass="LrgCombo">
                                                </asp:DropDownList>
                                            </td>
                                           <td align="center">
                                                <asp:TextBox ID="txtValueForType" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("ValueForType") %>' onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false">
                                                </asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:DropDownList ID="cmbLateFeeType" runat="server" CssClass="SmlCombo">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtAmount" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("Amount") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtRepeatCount" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;" Text='<%#Eval("RepeatCount") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:TextBox ID="txtSortOrder" runat="server" CssClass="SmlTxtBox" style="width:50px;text-align:right;padding-right:5px;"  Text='<%#Eval("SortOrder") %>'
                                                onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false"
                                                onkeypress="return blockNonNumbers (this, event, false, false);" onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkExcludeHolidays" runat="server" Enabled="false" />
                                            </td>
                                            <td align="center">
                                                <asp:CheckBox ID="chkExcludeWeekends" runat="server" Enabled="false" />
                                            </td>
                                        </tr>
                                    </AlternatingItemTemplate>
                                    <EmptyDataTemplate>
                                        <tr>
                                            <td class="LblNoRecord" align="center">
                                                <asp:Label ID="lblNoRecFound" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
                                            </td>
                                        </tr>
                                    </EmptyDataTemplate>
                                </asp:ListView>
                                <asp:HiddenField ID="hidLastRecordNumber" runat="server" Value="0" />
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddMoreRows" EventName="Click" />
                            </Triggers>
                            </asp:UpdatePanel>
                            <div style="float:right">
                                <asp:Button ID="btnAddMoreRows" runat="server" Text="Add More Rows" 
                                    CssClass="ClsBtn" onclick="btnAddMoreRows_Click" Visible="false" />
                            </div>                            
                        </td>
                    </tr>
					<tr>
						<td colspan="2" align="center">
							<asp:Button ID="btn_Save" runat="server" CssClass="ClsBtn" OnClick="btn_Save_Click"
								OnClientClick="DisableButtons();" CausesValidation="true" Text="<%$ Resources:LocalizedResources, Save %>"
								UseSubmitBehavior="false" Style="margin: 8px 0;" ValidationGroup="LateFee" />
							<asp:CustomValidator ID="custValidateEmptyIntervalName" runat="server" ClientValidationFunction="validateEmptyIntervalName"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateEmptyDueDate" runat="server" ClientValidationFunction="validateEmptyDueDate"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="cstDueDateValidator" runat="server" ClientValidationFunction="validateDueDates"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateEmptyStartDate" runat="server" ClientValidationFunction="validateEmptyIntervalStartDate"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateEmptyEndDate" runat="server" ClientValidationFunction="validateEmptyIntervalEndDate"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="cstInterval" runat="server" ClientValidationFunction="validateIntevalDates"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="cstCompareIntervalDates" runat="server" ClientValidationFunction="CompareIntevalDates"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateValueForType" runat="server" ClientValidationFunction="validateEmptyValueForType"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateLateFeeType" runat="server" ClientValidationFunction="validateEmptyLateFeeType"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
							<asp:CustomValidator ID="custValidateEmptyAmount" runat="server" ClientValidationFunction="validateEmptyAmount"
								Display="None" SetFocusOnError="True" ValidateEmptyText="True" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateFeeType" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateFeeValueForType" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator3" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateLateFeeType" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator4" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateAmount" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator5" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateRepeatCount" ValidationGroup="LateFee"></asp:CustomValidator>
                            <asp:CustomValidator ID="CustomValidator6" runat="server" ErrorMessage="" Display="None" ClientValidationFunction="ValidateSortOrder" ValidationGroup="LateFee"></asp:CustomValidator>
						</td>
					</tr>
					<tr>
						<td colspan="2" align="center">
							<%if (!Settings.IsMiniSite) %>
							<%{ %>
							<table>
								<tr>
									<td colspan="2" align="center">
										<div class="ClsLblLgnd" style="width: 1035px; text-align: left; float: none; margin: 5px 0;">
											<asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, DeactivationSettings %>"></asp:Label>
										</div>
									</td>
								</tr>
								<tr>
									<td colspan="2" align="center">
										<asp:ListView ID="lstvwFeesDeactivationSettings" runat="server" DataKeyNames="Fee_Type_Id,IsConfigured"
											OnItemDataBound="lstvwFeesDeactivationSettings_ItemDataBound" OnDataBound="lstvwFeesDeactivationSettings_DataBound">
											<LayoutTemplate>
												<table id="deactivationSettings" class="GridBorder" cellpadding="3" cellspacing="1"
													width="1035px">
													<tr class="ClsGridHeader" style="font-size: 9pt;">
														<th align="center" style="padding: 0;">
															<asp:CheckBox ID="chkSelectAll" runat="server" onclick="SelectAll(this);" />
														</th>
														<th align="left">
															<asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, FeeType %>"></asp:Label>
														</th>
														<th align="center">
															<asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, DeactivationThreshold %>"></asp:Label>
														</th>
														<th align="center">
															<asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, Remainder %>"></asp:Label>
														</th>
													</tr>
													<tr id="itemPlaceholder" runat="server">
													</tr>
												</table>
											</LayoutTemplate>
											<ItemTemplate>
												<tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
													<td align="center">
														<asp:CheckBox ID="chkSelect" runat="server" onclick="Select(this);" Checked='<%# Convert.ToBoolean(Eval("DeactivateUser")) %>' />
													</td>
													<td align="left">
														<asp:Label ID="lblFeeType" runat="server" Text='<%# Eval("Fee_Type") %>' />
													</td>
													<td align="center">
														<span><span>
															<asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Month %>"></asp:Label>
															<span id="Span1" class="colonPadding">:</span> </span>
															<asp:TextBox ID="txtThresholdMonths" runat="server" CssClass="SmlTxtBox" Style="width: 20px;
																margin-right: 10px;" MaxLength="2" Text='<%# Eval("ThresholdMonths") %>' onblur="extractNumber(this,2,false);"
																ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
																onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
															<span>
																<asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, Day %>"></asp:Label>
																<span id="Span3" class="colonPadding">:</span> </span>
															<asp:TextBox ID="txtThresholdDays" runat="server" CssClass="SmlTxtBox" Style="width: 30px;"
																MaxLength="3" Text='<%# Eval("ThresholdDays") %>' onblur="extractNumber(this,2,false);"
																ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
																onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
														</span>
													</td>
													<td align="center">
														<span><span>
															<asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Day %>"></asp:Label>
															<span id="Span2" class="colonPadding">:</span> </span>
															<asp:TextBox ID="txtReminderDays" runat="server" CssClass="SmlTxtBox" Style="width: 20px;
																margin-right: 10px;" MaxLength="2" Text='<%# Eval("ReminderDays") %>' onblur="extractNumber(this,2,false);"
																ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
																onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
															<span>
																<asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, IntervalDays %>"></asp:Label>
																<span id="Span4" class="colonPadding">:</span> </span>
															<asp:TextBox ID="txtReminderInterval" runat="server" CssClass="SmlTxtBox" Style="width: 20px;
																margin-right: 10px;" MaxLength="2" Text='<%# Eval("ReminderInterval") %>' onblur="extractNumber(this,2,false);"
																ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
																onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
															<span>
																<asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, SMS %>"></asp:Label>
																<span id="Span5" class="colonPadding">:</span></span>
															<asp:TextBox ID="txtReminderSMS" runat="server" CssClass="SmlTxtBox" Style="width: 20px;"
																MaxLength="2" Text='<%# Eval("ReminderSMS") %>' onblur="extractNumber(this,2,false);"
																ondrop="event.returnValue=false" onkeypress="return blockNonNumbers (this, event, false, false);"
																onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false" />
														</span>
													</td>
												</tr>
											</ItemTemplate>
											<EmptyDataTemplate>
												<table width="100%">
													<tr>
														<td class="LblNoRecord" align="center">
															<asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordFound %>"></asp:Label>
														</td>
													</tr>
												</table>
											</EmptyDataTemplate>
										</asp:ListView>
									</td>
								</tr>
								
								<tr>
									<td>
									<table width="1035px">
					<tr>
						<td align="left" class="ClsBorderlight " style="width: 53px; background-color: #ffffc4;
							padding: 3px;">
							<span class="LblNrmlB" style="font-weight: bold; height: 16px; width: 46px;">
								<asp:Label ID="Label17" runat="server" Text="<%$ Resources:LocalizedResources, Note1%>"></asp:Label>
								<span id="Span13" class="colonPadding">:</span></span>
						</td>
						<td align="left" class="ClsBorderlight" style="padding: 3px;">
							<div class="LblSmlV">
								<b>
									<asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, DeactivationThreshold %>"></asp:Label></b>
								<span id="Span6" class="colonPadding">:</span>
								<asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, ThePeriodAfterWhichUserACWillBeDeactivated %>"></asp:Label>
							</div>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight " style="width: 53px; background-color: #ffffc4;
							padding: 3px;">
							<span class="LblNrmlB" style="font-weight: bold; height: 16px; width: 46px;">
								<asp:Label ID="Label18" runat="server" Text="<%$ Resources:LocalizedResources, Note2%>"></asp:Label>
								<span id="Span12" class="colonPadding">:</span></span>
						</td>
						<td align="left" class="ClsBorderlight" style="padding: 3px;">
							<div class="LblSmlV">
								<b>
									<asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, ReminderDays %>"></asp:Label></b>
								<span id="Span7" class="colonPadding">:</span>
								<asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, TheNumberOfDaysPriorToTheDeactivation %>"></asp:Label></div>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight " style="width: 53px; background-color: #ffffc4;
							padding: 3px;">
							<span class="LblNrmlB" style="font-weight: bold; height: 16px; width: 46px;">
								<asp:Label ID="Label19" runat="server" Text="<%$ Resources:LocalizedResources, Note3%>"></asp:Label>
								<span id="Span11" class="colonPadding">:</span></span>
						</td>
						<td align="left" class="ClsBorderlight" style="padding: 3px;">
							<div class="LblSmlV">
								<b>
									<asp:Label ID="Label13" runat="server" Text="<%$ Resources:LocalizedResources, ReminderIntervalDays %>"></asp:Label></b>
								<span id="Span8" class="colonPadding">:</span>
								<asp:Label ID="Label14" runat="server" Text="<%$ Resources:LocalizedResources, TheNumberOfDaysAfterTheInitialReminderToResendTheReminderMessageToTheStudent%>"></asp:Label></div>
						</td>
					</tr>
					<tr>
						<td align="left" class="ClsBorderlight " style="width: 53px; background-color: #ffffc4;
							padding: 3px;">
							<span class="LblNrmlB" style="font-weight: bold; height: 16px; width: 46px;">
								<asp:Label ID="Label20" runat="server" Text="<%$ Resources:LocalizedResources, Note4%>"></asp:Label>
								<span id="Span10" class="colonPadding">:</span></span>
						</td>
						<td align="left" class="ClsBorderlight" style="padding: 3px;">
							<div class="LblSmlV">
								<b>
									<asp:Label ID="Label15" runat="server" Text="<%$ Resources:LocalizedResources, ReminderSMS%>"></asp:Label></b>
								<span id="Span9" class="colonPadding">:</span>
								<asp:Label ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources, TheNoOfDaysPriorToTheDeactivationDateWhenTheUserWillBeNotifiedAboutPendingFeesBySMS %>"></asp:Label></div>
						</td>
					</tr>
				</table>
									</td>
								</tr>
								<tr>
						<td align="center" colspan="2">
							<asp:Button ID="btnSaveDeactivationSettings" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Save %>"
								ValidationGroup="Deactivation" OnClick="btnSaveDeactivationSettings_Click" />
							<asp:Button ID="btn_Cancel" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Close %>"
								CausesValidation="False" OnClientClick="window.close();return false;" UseSubmitBehavior="false" />&nbsp;
						</td>
					</tr>
							</table>
							<%} %>
						</td>
					</tr>
					<tr>
									<td colspan="2" align="left">
										<br />
										<asp:HyperLink ID="lnkMarkGrades" CssClass="ClsConfigLink" Text="<%$ Resources:LocalizedResources, MarksGrades %>"
											runat="server" NavigateUrl="~/RITeSchool/Admin/MarksGradeConfiguration.aspx"
											Visible="false"></asp:HyperLink>
										<asp:CustomValidator ID="cstDeactivationSettingsThresholdValidator" runat="server"
											ClientValidationFunction="ValidateDeactivationSettings_Threshold" ValidationGroup="Deactivation"
											Display="None" SetFocusOnError="True" />
										<asp:CustomValidator ID="cstDeactivationSettingsReminderValidator" runat="server"
											ClientValidationFunction="ValidateDeactivationSettings_Reminder" ValidationGroup="Deactivation"
											Display="None" SetFocusOnError="True" />
										<asp:CustomValidator ID="cstThresholdReminderValidator" runat="server" ClientValidationFunction="ValidateThresholdReminder"
											ValidationGroup="Deactivation" Display="None" SetFocusOnError="True" />
									</td>
								</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td align="center">
				
				<asp:HiddenField ID="hidDueDatesShouldBeSelectedFor" runat="server" />
				<asp:HiddenField ID="hidCultureInfo" runat="server" />
				<asp:HiddenField ID="hidDueDateshouldBeInTheValidFormatFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentStartDatesShouldBeInValidFormatFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentStartDatesShouldBeSelectedFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentEndDateShouldBeSelectedFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentEndDatesShouldBeInValidFormatFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentNamesShouldNotBeBlankFor" runat="server" />
				<asp:HiddenField ID="hidValueForTypesShouldNotBeblankFor" runat="server" />
				<asp:HiddenField ID="hidAmountRsShouldNotBeBlankFor" runat="server" />
				<asp:HiddenField ID="hidLateFeeTypeShouldBeSelectedFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentNamesShouldNotBeDuplicatedFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentDatesShouldBeWithinTheCurrentAcademicYear" runat="server" />
				<asp:HiddenField ID="hidDueDateShouldBeLessThanOrEqualToInstallmentEndDateFor" runat="server" />
				<asp:HiddenField ID="hidInstallmentEndDateShouldBeGreaterThanInstallmentStartDateFor"
					runat="server" />
				<asp:HiddenField ID="hidSelectedDateFor" runat="server" />
				<asp:HiddenField ID="hidIsAHoliday" runat="server" />
				<asp:HiddenField ID="hidIsNotAWorkingDay" runat="server" />
				<asp:HiddenField ID="hidDoYouWantToContinue" runat="server" />
				<asp:HiddenField ID="hidPleaseSelectAmountGreaterThanZero" runat="server" />
				<asp:HiddenField ID="hidHoliday" runat="server" />
				<asp:HiddenField ID="hidMonthsAndDaysBothShouldNotBeZeroForDeactivationThresholdForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="hidMonthsAndDaysShouldBeSpecifiedForDeactivationThresholdForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="hidDaysIntervalAndSMSShouldBeSpecifiedForReminderForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="hidDaysIntervalAndSMSShouldNotBezeroForReminderForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="hidIntervalShouldNotBeGreaterThanDaysForFeeTypes" runat="server" />
				<asp:HiddenField ID="hidReminderDaysShouldNotBeGreaterThanDeactivationThresholdForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="hidReminderSMSShouldNotBeGreaterThanDeactivationThresholdForFeeTypes"
					runat="server" />
				<asp:HiddenField ID="HidTo" runat="server" />
				<asp:HiddenField ID="HidFor1" runat="server" />
				<asp:Label ID="lblPrecondition" CssClass="ClsConfigText" runat="server" EnableViewState="false"> </asp:Label>
				<asp:HiddenField ID="hidStandardID" runat="server" />
				<asp:HiddenField ID="hidLateFeeId" runat="server" />
				<asp:HiddenField ID="hidIsConfigured" runat="server" />
				<asp:HiddenField ID="hidFeeTypeID" runat="server" />
				<asp:HiddenField ID="hidLateFeeDueDate" runat="server" />
				<asp:HiddenField ID="hidYearEndDate" runat="server" />
				<asp:HiddenField ID="hidYearStartDate" runat="server" />
				<asp:HiddenField ID="hidTermName" runat="server" />
				<asp:HiddenField ID="hidAcademicYearStartDate" runat="server" />
				<asp:HiddenField ID="hidAcademicYearEndDate" runat="server" />
                <asp:HiddenField ID="hidAtleastOneFeeTypeSelectedForSaving" runat="server" />
			</td>
		</tr>
	</table>
	<script language="javascript" type="text/javascript">
		_clientYearStartDate = "<%= this.hidYearStartDate.ClientID %>";
		_clienthidTermName = "<%= this.hidTermName.ClientID %>";
		_clientYearEndDate = "<%= this.hidYearEndDate.ClientID %>";
		_clientFeeTypeGridId = "<%= this.grdLateFeeTypeConfig.ClientID %>";
		_clientbtn_Save = "<%= this.btn_Save.ClientID %>";
		_clientbtn_Cancel = "<%= this.btn_Cancel.ClientID %>";
		_clientlblErr = "<%= this.lblErrorMsg.ClientID %>";
		_clientlblUpdateMessage = '<%= this.lblUpateMessage.ClientID %>';
		_clientvalSum = '<%= this.valsumLateFee.ClientID %>';
		_clientvalDeactivation = '<%= this.deactivationValidationSummary.ClientID %>';
		_clientlstvwFeesDeactivationSettings = '<%= this.lstvwFeesDeactivationSettings.ClientID %>';
		_clientcstDueDateValidator = '<%=this.cstDueDateValidator.ClientID %>';
		_clientcustValidateEmptyDueDate = '<%=this.custValidateEmptyDueDate.ClientID %>';
		_clientcustValidateEmptyIntervalName = '<%=this.custValidateEmptyIntervalName.ClientID %>';
		_clientcustValidateValueForType = '<%=this.custValidateValueForType.ClientID %>';
		_clientcustValidateEmptyEndDate = '<%=this.custValidateEmptyEndDate.ClientID %>';
		_clientcustValidateEmptyStartDate = '<%=this.custValidateEmptyStartDate.ClientID %>';
		_clientcustValidateEmptyAmount = '<%=this.custValidateEmptyAmount.ClientID %>';
		_clientcustValidateLateFeeType = '<%=this.custValidateLateFeeType.ClientID %>';
		_clienthidAcademicYearStartDate = '<%=this.hidAcademicYearStartDate.ClientID %>';
		_clientcstCompareIntervalDates = '<%=this.cstCompareIntervalDates.ClientID %>';
		_clientvalidateIntevalDates = '<%=this.cstInterval.ClientID %>';
		_clienthidAcademicYearEndDate = '<%=this.hidAcademicYearEndDate.ClientID %>';

		_clientlstvwFeeTypes = "<%=this.lstvwFeeTypes.ClientID %>"

		function fnover(varname) {
			var objTXT = document.getElementById(varname);
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "maroon";
			objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
		}

		function fnout(varname) {
			var objTXT = document.getElementById(varname);
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "#a3c07b";
			objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
		}

		function ClearLabel() {
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function') {
				isPageValid = Page_ClientValidate();
			}
			if (isPageValid) {
				document.getElementById(_clientbtn_Save).disabled = true;
				document.getElementById(_clientbtn_Cancel).disabled = true;
			}

			if (document.getElementById(_clientlblErr)) {
				document.getElementById(_clientlblErr).innerText = "";
				document.getElementById(_clientlblErr).innerHTML = "";
			}
		}

		function closewindow() {
			document.getElementById(_clientbtn_Save).disabled = true;
			document.getElementById(_clientbtn_Cancel).disabled = true;
			window.close();
		}

		function DisableButtons() {
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function') {
				isPageValid = Page_ClientValidate();
			}
			if (isPageValid) {
				document.getElementById(_clientbtn_Save).enabled = true;
				document.getElementById(_clientbtn_Cancel).enabled = true;
			}
		}

		function fun(obj, iRowCount) {
			var Date1 = obj.value;
			var CntType, FeeType, NewFeeType;
			var grid = document.getElementById(_clientFeeTypeGridId);
			var iRowCount = iRowCount + 2;
			var cntrl
			if (iRowCount < 10)
				cntrl = "_ctl0";
			else
				cntrl = "_ctl";

			FeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;
			iRowCount = iRowCount + 1;

			if (iRowCount < 10)
				cntrl = "_ctl0";
			else
				cntrl = "_ctl";

			if (document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType') != null)
				NewFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

			var isValid = validateDate(Date1);
			if (isValid) {

				var nDate1 = Date1.replace(/-/g, ' ');
				nDate1 = new Date(nDate1);

				var d = nDate1.getDate();
				var m = nDate1.getMonth();
				var y = nDate1.getFullYear();
				var NextDate = new Date(y, m, d + 1);

				var month = new Array();
				month[0] = "Jan";
				month[1] = "Feb";
				month[2] = "Mar";
				month[3] = "Apr";
				month[4] = "May";
				month[5] = "Jun";
				month[6] = "Jul";
				month[7] = "Aug";
				month[8] = "Sep";
				month[9] = "Oct";
				month[10] = "Nov";
				month[11] = "Dec";

				var Ndate = NextDate.getDate() + "-" + month[NextDate.getMonth()] + "-" + NextDate.getFullYear();
				if (FeeType == NewFeeType) {
					document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalStartDate').value = Ndate;
					document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidIntervalStart').value = Ndate;
				}

			}
		}

		//This function is used to validate empty DueDate.
		function validateEmptyDueDate(aSrc, args) {
		    
			var Msg = "";
			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeType = "";

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				txtDueDate = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtDueDate').value;
				hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				if (i == 0) {
					if (txtDueDate == "" || txtDueDate == null) {
						if (!Msg.match(hidFeeType))
							Msg = Msg + "," + hidFeeType;

					}
				}

				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";

					var txtDueDate1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtDueDate').value;
					var hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;
					if (txtDueDate1 == "" || txtDueDate1 == null) {
						if (!Msg.match(hidFeeTypeMain))
							Msg = Msg + "," + hidFeeTypeMain;
					}
				}
			}

			if (Msg != "") {
				Msg = Msg.substring(1, Msg.length);
				document.getElementById(_clientcustValidateEmptyDueDate).errormessage = document.getElementById("<%=this.hidDueDatesShouldBeSelectedFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false

		}

		//This function is used to validate empty IntervalName.
		function validateEmptyIntervalName(aSrc, args) {
		    
			var Msg = "";

			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeType = "";

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				var txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalName').value;
				hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				if (i == 0) {

					if (txtIntervalName == "" || txtIntervalName == null) {
						if (!Msg.match(hidFeeType))
							Msg = Msg + "," + hidFeeType;

					}
				}

				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";

					var txtIntervalName1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalName').value;
					var hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;

					if ((hidFeeType != hidFeeTypeMain) && (txtIntervalName1 == "" || txtIntervalName1 == null)) {
						if (!Msg.match(hidFeeTypeMain))
							Msg = Msg + "," + hidFeeTypeMain;

					}
				}

			}
			if (Msg != "") {
				Msg = Msg.substring(1, Msg.length);
				document.getElementById(_clientcustValidateEmptyIntervalName).errormessage = document.getElementById("<%=this.hidInstallmentNamesShouldNotBeBlankFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false

		}

		//This function is used to validate the interval start date of each interval.
		function validateEmptyIntervalStartDate(aSrc, args) {
		    
			var Msg = ""; var Msg1 = "";

			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeType = "";

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				var txtIntervalStartDate = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalStartDate').value;
				hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				if (i == 0) {

					if (txtIntervalStartDate == "" || txtIntervalStartDate == null) {
						if (!Msg.match(hidFeeType))
							Msg = Msg + "," + hidFeeType;

					}
					else if (txtIntervalStartDate != null && txtIntervalStartDate != "") {
						if (!validateDate(txtIntervalStartDate)) {
							if (!Msg1.match(hidFeeType))
								Msg1 = Msg1 + "," + hidFeeType;
						}
					}
				}

				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";

					var txtIntervalStartDate1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalStartDate').value;
					var hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;

					if (txtIntervalStartDate1 == "" || txtIntervalStartDate1 == null) {
						if (!Msg.match(hidFeeTypeMain))
							Msg = Msg + "," + hidFeeTypeMain;

					}
					else if (txtIntervalStartDate1 != null && txtIntervalStartDate1 != "") {
						if (!validateDate(txtIntervalStartDate1)) {
							if (!Msg1.match(hidFeeTypeMain))
								Msg1 = Msg1 + "," + hidFeeTypeMain;
						}
					}
				}

			}
			if (Msg != "") {
				Msg = Msg.substring(1, Msg.length);
				document.getElementById(_clientcustValidateEmptyStartDate).errormessage = document.getElementById("<%=this.hidInstallmentStartDatesShouldBeSelectedFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			if (Msg1 != "") {
				Msg1 = Msg1.substring(1, Msg1.length);
				document.getElementById(_clientcustValidateEmptyStartDate).errormessage = document.getElementById("<%=this.hidInstallmentStartDatesShouldBeInValidFormatFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false

		}

		//This function is used to validate empty IntervalEndDate.
		function validateEmptyIntervalEndDate(aSrc, args) {
		    
			var Msg = ""; var Msg1 = "";

			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeType = "";

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				var txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalEndDate').value;
				hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				if (i == 0) {

					if (txtIntervalName == "" || txtIntervalName == null) {
						if (!Msg.match(hidFeeType))
							Msg = Msg + "," + hidFeeType;

					}
					else if (txtIntervalName != "" && txtIntervalName != null) {
						if (!validateDate(txtIntervalName)) {
							if (!Msg1.match(hidFeeType))
								Msg1 = Msg1 + "," + hidFeeType;
						}
					}
				}

				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";

					var txtIntervalName1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalEndDate').value;
					var hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;

					if (txtIntervalName1 == "" || txtIntervalName1 == null) {
						if (!Msg.match(hidFeeTypeMain))
							Msg = Msg + "," + hidFeeTypeMain;

					}
					else if (txtIntervalName1 != "" && txtIntervalName1 != null) {
						if (!validateDate(txtIntervalName1)) {
							if (!Msg1.match(hidFeeTypeMain))
								Msg1 = Msg1 + "," + hidFeeTypeMain;
						}
					}
				}

			}
			if (Msg != "") {
				Msg = Msg.substring(1, Msg.length);
				document.getElementById(_clientcustValidateEmptyEndDate).errormessage = document.getElementById("<%=this.hidInstallmentEndDateShouldBeSelectedFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			if (Msg1 != "") {
				Msg1 = Msg1.substring(1, Msg1.length);
				document.getElementById(_clientcustValidateEmptyEndDate).errormessage = document.getElementById("<%=this.hidInstallmentEndDatesShouldBeInValidFormatFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false

		}

		//This function is used to validate empty ValueForType.
		function validateEmptyValueForType(aSrc, args) {
//			var Msg = "";

//			var grid = document.getElementById(_clientFeeTypeGridId);
//			var icount = grid.rows.length;
//			var iRowCount = 0;
//			var hidFeeType = "";

//			for (var i = 0; i < icount - 1; i++) {

//				iRowCount = i + 2;
//				var checkbox;
//				if (iRowCount < 10)
//					cntrl = "_ctl0";
//				else
//					cntrl = "_ctl";
//				var type = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtLateFeeTypePeriod');
//				var txtIntervalName = "";
//				if (type != null) {
//					txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtLateFeeTypePeriod').value;
//					hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;
//					if (txtIntervalName == "" || txtIntervalName == null) {
//						if (!Msg.match(hidFeeType))
//							Msg = Msg + "," + hidFeeType;

//					}
//				}
//			}
//			if (Msg != "") {
//				Msg = Msg.substring(1, Msg.length);
//				document.getElementById(_clientcustValidateValueForType).errormessage = document.getElementById("<%=this.hidValueForTypesShouldNotBeblankFor.ClientID %>").value
// + Msg + ".";
//				args.IsValid = false;
//				return true;
//			}

			args.IsValid = true
			return false

		}

		//This function is used to validate empty amount.
		function validateEmptyAmount(aSrc, args) {
//			var Msg = "";

//			var grid = document.getElementById(_clientFeeTypeGridId);
//			var icount = grid.rows.length;
//			var iRowCount = 0;
//			var hidFeeType = "";

//			for (var i = 0; i < icount - 1; i++) {

//				iRowCount = i + 2;
//				var checkbox;
//				if (iRowCount < 10)
//					cntrl = "_ctl0";
//				else
//					cntrl = "_ctl";
//				var type = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_NumAmount');
//				var txtIntervalName = "";
//				if (type != null) {
//					txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_NumAmount').value;
//					hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;
//					if (txtIntervalName == "" || txtIntervalName == null) {
//						if (!Msg.match(hidFeeType))
//							Msg = Msg + "," + hidFeeType;

//					}
//				}
//			}
//			if (Msg != "") {
//				Msg = Msg.substring(1, Msg.length);
//				document.getElementById(_clientcustValidateEmptyAmount).errormessage = document.getElementById("<%=this.hidAmountRsShouldNotBeBlankFor.ClientID %>").value + Msg + ".";
//				args.IsValid = false;
//				return true;
//			}

			args.IsValid = true
			return false

		}

		//This function is used to validate empty Fee type.
		function validateEmptyLateFeeType(aSrc, args) {
//			var Msg = "";

//			var grid = document.getElementById(_clientFeeTypeGridId);
//			var icount = grid.rows.length;
//			var iRowCount = 0;
//			var hidFeeType = "";

//			for (var i = 0; i < icount - 1; i++) {

//				iRowCount = i + 2;
//				var checkbox;
//				if (iRowCount < 10)
//					cntrl = "_ctl0";
//				else
//					cntrl = "_ctl";
//				var type = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_cmbLateFeeType');
//				var txtIntervalName = "";
//				if (type != null) {
//					txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_cmbLateFeeType').value;
//					hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;
//					if (txtIntervalName == 0) {
//						if (!Msg.match(hidFeeType))
//							Msg = Msg + "," + hidFeeType;

//					}
//				}
//			}
//			if (Msg != "") {
//				Msg = Msg.substring(1, Msg.length);
//				document.getElementById(_clientcustValidateLateFeeType).errormessage = document.getElementById("<%=this.hidLateFeeTypeShouldBeSelectedFor.ClientID %>").value + Msg + ".";
//				args.IsValid = false;
//				return true;
//			}

			args.IsValid = true
			return false

		}

		//This functoin is used to validate Installment duedates.
		function validateDueDates(aSrc, args) {
		    
			var Msg = ""; var Msg1 = ""; var txtIntervalName = ""; var txtIntervalName1 = "";
			var txtDueDate = ""; var txtDueDate1 = ""; var dtDueDate = "";
			var cntrl; var isValid = true; var MsgDate = ""

			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeType = "";

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				txtIntervalName = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalName').value;
				txtDueDate = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtDueDate').value;
				hidFeeType = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				var DueDate = "";
				if (txtDueDate != "") {
					if (document.all) {
						if (isNaN(new Date(convertdate(txtDueDate).replace('-', ' '))))
							isValid = false;
						else
							DueDate = new Date(convertdate(txtDueDate).replace('-', ' '));
					}
					else {
						if (isNaN(new Date(convertdate(txtDueDate).replace(/-/g, ' '))))
							isValid = false;
						else
							DueDate = new Date(convertdate(txtDueDate).replace('-', ' '));
					}

					if (i == 0 && txtDueDate != "") {
						isValid = validateDate(txtDueDate)
						if (!isValid) {
							var n = MsgDate.indexOf(hidFeeType);
							if (n == -1)
								MsgDate = MsgDate + ", " + hidFeeType;
						}
					}
				}
				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;
					var checkbox1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";


					txtIntervalName1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalName').value;
					txtDueDate1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtDueDate').value;
					hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;

					if (txtDueDate1 != "") {
						isValid = validateDate(txtDueDate1)
						if (!isValid) {
							var n = MsgDate.indexOf(hidFeeTypeMain);
							if (n == -1)
								MsgDate = MsgDate + ", " + hidFeeTypeMain;
						}
					}

					if (txtIntervalName != "" && txtIntervalName1 != "" && txtIntervalName == txtIntervalName1 && hidFeeType == hidFeeTypeMain) {
						var n = Msg.indexOf(hidFeeType);
						if (n == -1)
							Msg = Msg + ", " + hidFeeType;
					}
				}
			}

			if (MsgDate != "") {
				MsgDate = MsgDate.substring(1, MsgDate.length);
				document.getElementById(_clientcstDueDateValidator).errormessage = document.getElementById("<%=this.hidDueDateshouldBeInTheValidFormatFor.ClientID %>").value + MsgDate + ".";
				args.IsValid = false;
				return true;
			}
			if (Msg != "") {
				Msg = Msg.substring(1, Msg.length);
				document.getElementById(_clientcstDueDateValidator).errormessage = document.getElementById("<%=this.hidInstallmentNamesShouldNotBeDuplicatedFor.ClientID %>").value + Msg + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false
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

		//This function is used to validate Installment Dates.
		function validateIntevalDates(aSrc, args) {
		    
			var MsgEnd = "", MsgBoth = "", MsgCompare = "", MsgEql = "", MsgBoth1 = "", MsgDueDt = "", MsgStart = "";

			var txtIntervalStart = ""; var txtIntervalEnd = "";
			var txtIntervalStart1 = ""; var txtIntervalEnd1 = "";
			var MsgAcademicYear = "";
			var cntrl; var isValid = true; var isValid = 1; var MsgDate = ""
			var dtStartDate, dtEndDate, dtStartDate1, dtEndDate1;
			var AcademicStartDate, AcademicEndDate;
			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeTypeMain = "";
			var txtDueDate;

			AcademicStartDate = new Date(convertdate(document.getElementById(_clienthidAcademicYearStartDate).value).replace('-', ' '));
			AcademicEndDate = new Date(convertdate(document.getElementById(_clienthidAcademicYearEndDate).value).replace('-', ' '));


			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				txtDueDate = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtDueDate').value;
				txtIntervalStart = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalStartDate').value;
				txtIntervalEnd = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalEndDate').value;
				hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				isValid = validateDate(txtIntervalStart);

				if (isValid && txtIntervalStart != null && txtIntervalStart != "") {
					dtStartDate = new Date(convertdate(txtIntervalStart).replace('-', ' '));
					if ((dtStartDate < AcademicStartDate) || (dtStartDate > AcademicEndDate)) {
						if (!MsgAcademicYear.match(hidFeeTypeMain))
							MsgAcademicYear = MsgAcademicYear + "," + hidFeeTypeMain;
					}
				}
				isValid1 = validateDate(txtIntervalEnd);
				if (isValid1 && txtIntervalEnd != null && txtIntervalEnd != "") {
					dtEndDate = new Date(convertdate(txtIntervalEnd).replace('-', ' '));
					if ((dtEndDate < AcademicStartDate) || (dtEndDate > AcademicEndDate)) {
						if (!MsgAcademicYear.match(hidFeeTypeMain))
							MsgAcademicYear = MsgAcademicYear + "," + hidFeeTypeMain;
					}
				}


				if (isValid && isValid1) {
					if (dtStartDate >= dtEndDate)
						if (!MsgCompare.match(hidFeeTypeMain))
							MsgCompare = MsgCompare + "," + hidFeeTypeMain;
				}

				if (txtDueDate != "" || txtDueDate != null) {

					txtDueDate = new Date((convertdate(txtDueDate)).replace('-', ' '));
					if (txtDueDate > dtEndDate) {
						if (!MsgDueDt.match(hidFeeTypeMain))
							MsgDueDt = MsgDueDt + "," + hidFeeTypeMain;
					}
				}

				var iRowCount1 = 0;
				for (var j = i + 1; j < icount - 1; j++) {

					iRowCount1 = j + 2;
					var cntrl1;
					var checkbox1;

					if (iRowCount1 < 10)
						cntrl1 = "_ctl0";
					else
						cntrl1 = "_ctl";

					var FeeType = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_hidFeeType').value;
					txtIntervalStart1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalStartDate').value;
					txtIntervalEnd1 = document.getElementById(_clientFeeTypeGridId + cntrl1 + iRowCount1 + '_txtIntervalEndDate').value;

					if (!validateDate(txtIntervalStart1)) {
						if (!MsgStart.match(hidFeeTypeMain))
							MsgStart = MsgStart + ", " + hidFeeTypeMain;
					}
				}
			}

			if (MsgAcademicYear != "") {
			    MsgAcademicYear = MsgAcademicYear.substring(1, MsgAcademicYear.length);
				document.getElementById(_clientvalidateIntevalDates).errormessage = document.getElementById("<%=this.hidInstallmentDatesShouldBeWithinTheCurrentAcademicYear.ClientID %>").value.toString().trim() + document.getElementById(_clienthidAcademicYearStartDate).value + " "+ document.getElementById("<%=this.HidTo.ClientID %>").value + " " + document.getElementById(_clienthidAcademicYearEndDate).value + ") " + document.getElementById("<%=this.HidFor1.ClientID %>").value + " : " + MsgAcademicYear + ".";
				args.IsValid = false;
				return true;
			}
			if (MsgDueDt != "") {
				MsgDueDt = MsgDueDt.substring(1, MsgDueDt.length);
				document.getElementById(_clientvalidateIntevalDates).errormessage = document.getElementById("<%=this.hidDueDateShouldBeLessThanOrEqualToInstallmentEndDateFor.ClientID %>").value + MsgDueDt + ".";
				args.IsValid = false;
				return true;
			}
			args.IsValid = true
			return false
		}


		function CompareIntevalDates(aSrc, args) {
		    
			var MsgEnd = "", MsgBoth = "", MsgCompare = "";
			var txtIntervalStart = ""; var txtIntervalEnd = "";
			var txtIntervalStart1 = ""; var txtIntervalEnd1 = "";
			var cntrl; var isValid = true; var isValid = 1; var MsgDate = ""
			var dtStartDate, dtEndDate;
			var grid = document.getElementById(_clientFeeTypeGridId);
			var icount = grid.rows.length;
			var iRowCount = 0;
			var hidFeeTypeMain = "";
			var txtDueDate;

			for (var i = 0; i < icount - 1; i++) {

				iRowCount = i + 2;
				var checkbox;
				if (iRowCount < 10)
					cntrl = "_ctl0";
				else
					cntrl = "_ctl";

				txtIntervalStart = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalStartDate').value;
				txtIntervalEnd = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_txtIntervalEndDate').value;
				hidFeeTypeMain = document.getElementById(_clientFeeTypeGridId + cntrl + iRowCount + '_hidFeeType').value;

				isValid = validateDate(txtIntervalStart);
				isValid1 = validateDate(txtIntervalEnd);
				if (isValid && isValid1 && (txtIntervalStart != null && txtIntervalStart != "") && (txtIntervalEnd != null && txtIntervalEnd != "")) {
					dtStartDate = new Date(convertdate(txtIntervalStart).replace('-', ' '));
					dtEndDate = new Date(convertdate(txtIntervalEnd).replace('-', ' '));
					if (dtStartDate >= dtEndDate)
						if (!MsgCompare.match(hidFeeTypeMain))
							MsgCompare = MsgCompare + "," + hidFeeTypeMain;
				}
			}

			if (MsgCompare != "") {
				MsgCompare = MsgCompare.substring(1, MsgCompare.length);
				document.getElementById(_clientcstCompareIntervalDates).errormessage = document.getElementById("<%=this.hidInstallmentEndDateShouldBeGreaterThanInstallmentStartDateFor.ClientID %>").value + MsgCompare + ".";
				args.IsValid = false;
				return true;
			}

			args.IsValid = true
			return false

		}


		function ValidateLateFeeSettings() {
			var Weekend1 = "Sat";
			var Weekend2 = "Sun";
			var sMessage = "";
			var Days = '';
			var Days1 = '';
			var LeaveMessage = '';
			var LeaveMessage1 = '';
			var dtDeuDateQua1 = '';
			var dtDeuDateQuaII = '';
			var dtDeuDateQuaIII = '';
			var dtDeuDateQuaIV = '';
			var dtDeuDateTermI = '';
			var dtDeuDateTermII = '';
			var iRowNo = 2;
			var isPageValid = true;
			if (typeof (Page_ClientValidate) == 'function') {
				isPageValid = Page_ClientValidate();
			}
			if (isPageValid) {
				var txtDateMonth = '';

				if (Days != '') {
					LeaveMessage = document.getElementById("<%=this.hidSelectedDateFor.ClientID %>").value + Days + document.getElementById("<%=this.hidIsAHoliday.ClientID %>").value;
					LeaveMessage = LeaveMessage;
				}


				if (Days1 != '')
					LeaveMessage1 = document.getElementById("<%=this.hidSelectedDateFor.ClientID %>").value + Days1 + document.getElementById("<%=this.hidIsNotAWorkingDay.ClientID %>").value;
				if (LeaveMessage != '')
					LeaveMessage = LeaveMessage + "\n" + LeaveMessage1;
				else
					LeaveMessage = LeaveMessage1;
				if (LeaveMessage != '') {
					if (!window.confirm(LeaveMessage + document.getElementById("<%=this.hidDoYouWantToContinue.ClientID %>").value))
						return false;
				}
			}
			ClearLabel();
			return true;
		}

		//This function is used to validate amount.
		function ValidateAmountTextBoxForZero(aSrc, args) {
			var istart = 3;
			var iCount = document.getElementById(_clientFeeTypeGridId).rows.length + 1;
			var ifeetypecnt = 0;
			var bReturn = false;
			var iChkCount = 0;
			var sMessage = "";
			if (args.Value == "0") {
				sMessage = document.getElementById("<%=this.hidPleaseSelectAmountGreaterThanZero.ClientID %>").value;
			}
			if (sMessage != "") {
				aSrc.errormessage = sMessage;
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}

		function ConfirmToSave() {
			var bResult = true;
			{
				if (!window.confirm(document.getElementById("<%=this.hidHoliday.ClientID %>").valueg400
                )) {
					bResult = false;
				}
			}
			return bResult;
		}

		// Enables / Disables all input controls in the grid.
		function SelectAll(src) {
			var table = $(src).closest('table').get(0);
			var select = !ChkSelectStatus(table);

			$('input[type=checkbox][id$=chkSelect]:not(:disabled)', table)
				.each(function () {
					this.checked = select;
					SelectInternal(this);
				});
		}

		// Enables / Disables the input controls for the row for which checkbox was clicked.
		// Also checks / unchecks the chkSelectAll checkbox depending on the number of checkboxes checked in the grid.
		function Select(src) {
			SelectInternal(src);

			var table = $(src).closest('table').get(0);
			var allChecked = ChkSelectStatus(table);

			$('input[type=checkbox][id$=chkSelectAll]', table).get(0).checked = allChecked;
		}

		// Internal select function which actually performs the task.
		function SelectInternal(src) {
			var row = $(src).closest('tr').get(0);
			$("input[type=text]", row)
				.each(function () {
					this.disabled = !src.checked;
					if (src.checked && this.value.trim() == '')
						this.value = '0';
					else if (!src.checked)
						this.value = '';
				});
		}

		// Determines if all the checkboxes (excluding header checkbox) are checked.
		// Returns true if they are, false otherwise.
		function ChkSelectStatus(src) {
			var chkTotalCount = $('input[type=checkbox][id$=chkSelect]:not(:disabled)', src).length;
			var chkSelectedCount = $('input[type=checkbox][id$=chkSelect]:checked', src).length;
			return chkTotalCount == chkSelectedCount;
		}

		// Validates the Deactivation Threshold settings for each fee type.
		function ValidateDeactivationSettings_Threshold(src, args) {
			ResetMessages();

			var table = $('#deactivationSettings').get(0);

			var checkedRows = $('input[type=checkbox][id$=chkSelect]:checked', table).closest('tr');

			var txtThresholdMonths,
				txtThresholDays,
				lblFeeType;

			var invalidFeeTypes1 = [];
			var invalidFeeTypes2 = [];

			$(checkedRows)
				.each(function () {
					lblFeeType = $('[id$=lblFeeType]', this).get(0).innerHTML;
					txtThresholdMonths = $('input[type=text][id$=txtThresholdMonths]', this).get(0);
					txtThresholDays = $('input[type=text][id$=txtThresholdDays]', this).get(0);

					if (txtThresholDays.value.trim() == '' || txtThresholdMonths.value.trim() == '')
						invalidFeeTypes1.push(lblFeeType);
					else if (parseInt(txtThresholDays.value.trim()) == 0 && parseInt(txtThresholdMonths.value.trim()) == 0)
						invalidFeeTypes2.push(lblFeeType);
				});

			if (invalidFeeTypes1.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidMonthsAndDaysShouldBeSpecifiedForDeactivationThresholdForFeeTypes.ClientID %>").value + invalidFeeTypes1.join(', ') + '.';
				args.IsValid = false;
			}

			if (invalidFeeTypes2.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidMonthsAndDaysBothShouldNotBeZeroForDeactivationThresholdForFeeTypes.ClientID %>").value + invalidFeeTypes2.join(', ') + '.';
				args.IsValid = false;
			}
		}

		// Validates the Reminder settings for each fee type.
		function ValidateDeactivationSettings_Reminder(src, args) {
			ResetMessages();

			var table = $('#deactivationSettings').get(0);

			var checkedRows = $('input[type=checkbox][id$=chkSelect]:checked', table).closest('tr');

			var txtReminderDays,
				txtReminderInterval,
			    txtReminderSMS,
				lblFeeType;

			var invalidFeeTypes1 = [];
			var invalidFeeTypes2 = [];
			var invalidFeeTypes3 = [];

			$(checkedRows)
				.each(function () {
					lblFeeType = $('[id$=lblFeeType]', this).get(0).innerHTML;
					txtReminderDays = $('input[type=text][id$=txtReminderDays]', this).get(0);
					txtReminderInterval = $('input[type=text][id$=txtReminderInterval]', this).get(0);
					txtReminderSMS = $('input[type=text][id$=txtReminderSMS]', this).get(0);

					if (txtReminderDays.value.trim() == '' || txtReminderInterval.value.trim() == '' || txtReminderSMS.value.trim() == '')
						invalidFeeTypes1.push(lblFeeType);
					else if (parseInt(txtReminderDays.value.trim()) == 0 || parseInt(txtReminderInterval.value.trim()) == 0 || parseInt(txtReminderSMS.value.trim()) == 0)
						invalidFeeTypes2.push(lblFeeType);
					else if (parseInt(txtReminderDays.value.trim()) < parseInt(txtReminderInterval.value.trim()))
						invalidFeeTypes3.push(lblFeeType);
				});

			if (invalidFeeTypes1.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidDaysIntervalAndSMSShouldBeSpecifiedForReminderForFeeTypes.ClientID %>").value + invalidFeeTypes1.join(', ') + '.';
				args.IsValid = false;
			}

			if (invalidFeeTypes2.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidDaysIntervalAndSMSShouldNotBezeroForReminderForFeeTypes.ClientID %>").value + invalidFeeTypes2.join(', ') + '.';
				args.IsValid = false;
			}

			if (invalidFeeTypes3.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidIntervalShouldNotBeGreaterThanDaysForFeeTypes.ClientID %>").value + invalidFeeTypes3.join(', ') + '.';
				args.IsValid = false;
			}
		}

		// Validates that Reminder days are not greater than Deactivation Threshold put together (months + days).
		function ValidateThresholdReminder(src, args) {
			ResetMessages();
			var table = $('#deactivationSettings').get(0);
			if ($('input[type=checkbox][id$=chkSelect]:checked', table).length <= 0) {
			    src.errormessage = document.getElementById("<%=this.hidAtleastOneFeeTypeSelectedForSaving.ClientID %>").value;
				args.IsValid = false;
				return;
			}

			var checkedRows = $('input[type=checkbox][id$=chkSelect]:checked', table).closest('tr');

			var lblFeeType,
			    txtThresholdMonths,
				txtThresholDays,
			    txtReminderDays,
				txtReminderSMS;

			var invalidFeeTypes1 = [];
			var invalidFeeTypes2 = [];

			$(checkedRows)
				.each(function () {
					lblFeeType = $('[id$=lblFeeType]', this).get(0).innerHTML;
					txtThresholdMonths = $('input[type=text][id$=txtThresholdMonths]', this).get(0);
					txtThresholDays = $('input[type=text][id$=txtThresholdDays]', this).get(0);
					txtReminderDays = $('input[type=text][id$=txtReminderDays]', this).get(0);
					txtReminderSMS = $('input[type=text][id$=txtReminderSMS]', this).get(0);

					if (txtThresholDays.value.trim() == '' || txtThresholdMonths.value.trim() == '' || txtReminderDays.value.trim() == '' || txtReminderSMS.value.trim() == '')
						return;

					var iThresholdMonths = parseInt(txtThresholdMonths.value.trim());
					var iThresholdDays = parseInt(txtThresholDays.value.trim());
					var iReminderDays = parseInt(txtReminderDays.value.trim());
					var iReminderSMS = parseInt(txtReminderSMS.value.trim());

					if (iThresholdMonths == 0 && iThresholdDays == 0 && iReminderDays == 0 && iReminderSMS == 0)
						return;

					if (iReminderDays > (iThresholdMonths * 30) + iThresholdDays - 2)
						invalidFeeTypes1.push(lblFeeType);

					if (iReminderSMS > (iThresholdMonths * 30) + iThresholdDays - 2)
						invalidFeeTypes2.push(lblFeeType);
				});

			if (invalidFeeTypes1.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidReminderDaysShouldNotBeGreaterThanDeactivationThresholdForFeeTypes.ClientID %>").value + invalidFeeTypes1.join(', ') + '.';
				args.IsValid = false;
			}

			if (invalidFeeTypes2.length > 0) {
				src.errormessage = document.getElementById("<%=this.hidReminderSMSShouldNotBeGreaterThanDeactivationThresholdForFeeTypes.ClientID %>").value + invalidFeeTypes2.join(', ') + '.';
				args.IsValid = false;
			}
		}

		DisableAll();

		function DisableAll() {
            var rowIndex = 0
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {
                DisableFields(rowIndex, chk.checked)
                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }
        }

        function DisableFields(rowIndex, flag) {
            var cmbFeeType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_cmbFeeType")
            var txtValueForType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtValueForType")
            var cmbLateFeeType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_cmbLateFeeType")
            var txtAmount = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtAmount")
            var txtRepeatCount = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtRepeatCount")
            var txtSortOrder = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtSortOrder")
            var chkExcludeHolidays = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkExcludeHolidays")
            var chkExcludeWeekends = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkExcludeWeekends")

            if (flag) {
                cmbFeeType.disabled = false;
                txtValueForType.disabled = false;
                cmbLateFeeType.disabled = false;
                txtAmount.disabled = false;
                txtRepeatCount.disabled = false;
                txtSortOrder.disabled = false;
//                chkExcludeHolidays.disabled = false;
//                chkExcludeWeekends.disabled = false;
            }
            else {
                cmbFeeType.value = "0"
                txtValueForType.value = "0"
                cmbLateFeeType.value = "0"
                txtAmount.value = "0"
                txtRepeatCount.value = "0"
                txtSortOrder.value = "0"
                chkExcludeHolidays.checked = false
                chkExcludeWeekends.checked = false

                cmbFeeType.disabled = true;
                txtValueForType.disabled = true;
                cmbLateFeeType.disabled = true;
                txtAmount.disabled = true;
                txtRepeatCount.disabled = true;
                txtSortOrder.disabled = true;
                chkExcludeHolidays.disabled = true;
                chkExcludeWeekends.disabled = true;
            }
        }

        function EnableDisableFields(rowIndex, obj) {
            DisableFields(rowIndex, obj.checked);
            CheckHeaderCheckbox();
        }

        function CheckAll(obj) {
            var rowIndex = 0
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {
                chk.checked = obj.checked

                DisableFields(rowIndex, chk.checked)

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }
        }

        function CheckHeaderCheckbox() {
            var rowIndex = 0
            var isFound = false;
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {
                if (chk.checked == false) {
                    isFound = true;
                    break;
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (!isFound)
                $('#chkSelectAll').attr('checked', 'checked');
            else
                $('#chkSelectAll').removeAttr('checked');
        }

		// Resets all the messages & validations displayed on the screen.
		function ResetMessages() {
			$('#' + _clientlblErr).hide();
			$('#' + _clientlblUpdateMessage).hide();
			$('#' + _clientvalSum).hide();
			$('#' + _clientvalDeactivation).hide();
        }

        function ValidateFeeType(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {
                
                if (chk.checked) {
                    var cmbFeeType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_cmbFeeType")

                    if (cmbFeeType.value == "0")
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Fee Type should be selected for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateFeeValueForType(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {

                if (chk.checked) {
                    var txtValueForType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtValueForType")

                    if (txtValueForType.value == "" || parseInt(txtValueForType.value) == 0)
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Value For Type should not be blank or zero for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateLateFeeType(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {

                if (chk.checked) {
                    var cmbLateFeeType = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_cmbLateFeeType")

                    if (cmbLateFeeType.value == "0")
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Late Fee Type should be selected for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateAmount(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {

                if (chk.checked) {
                    var txtAmount = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtAmount")

                    if (txtAmount.value == "")
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Amount should not be blank for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateRepeatCount(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {

                if (chk.checked) {
                    var txtRepeatCount = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtRepeatCount")

                    if (txtRepeatCount.value == "")
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Repeat Count should not be blank for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateSortOrder(oSrc, args) {
            var rowIndex = 0
            var isFound = false;
            var SrNos = ''
            var chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            while (chk != null) {

                if (chk.checked) {
                    var txtSortOrder = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_txtSortOrder")

                    if (txtSortOrder.value == "" || parseInt(txtSortOrder.value) == 0)
                        SrNos = SrNos + "," + (rowIndex + 1)
                }

                rowIndex++
                chk = document.getElementById(_clientlstvwFeeTypes + '_ctrl' + rowIndex + "_chkSelect")
            }

            if (SrNos != '') {
                SrNos = SrNos.substring(1);
                oSrc.errormessage = "Sort Order should not be blank or zero for row(s) : " + SrNos + '.'
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

	</script>
</asp:Content>
