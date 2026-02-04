<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="DayBook.aspx.cs" Inherits="DayBook" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<style>
label[for] {
    margin-bottom: 0;
    vertical-align: bottom !important;
}
</style>
<asp:UpdatePanel ID="mainUpdatePanel"
				 runat="server">
	<ContentTemplate>
			<table width="800px">
			<tr>
				<td align="right">
					<span class="ClsMdtStar">* Mandatory Fields</span>
				</td>
			</tr>
			<tr>
				<td>
					<asp:ValidationSummary ID="valSummary"
										   runat="server"
										   CssClass="ClsLabel"
										   ShowSummary="true" />
				</td>
			</tr>
			<tr>
				<td align="center">
					<table cellspacing="4">
						<tr>
							<td>
								<asp:Label ID="lblStartDate"
										   runat="server"
										   Text="Date :"
										   CssClass="ClsBorderlight"
										   style="font-size: 9pt; padding: 5px 3px; vertical-align: middle;" />
								<asp:TextBox ID="txtStartDate"
											 runat="server"
											 CssClass="SmlTxtBox"
											 style="vertical-align: middle;" />
								<rjs:PopCalendar ID="dtStartDate"
												 runat="server"
												 Control="txtStartDate"
												 Format="dd mmm yyyy"
												 ShowWeekend="True"
												 To-Today="true"
												 ShowErrorMessage="false" />                                
								<span class="ClsMdtStar"> * </span>
							</td>
							<td id="tdEndDate" runat="server" style="display: none;">
								<span class="ClsBorderlight" style="font-size: 9pt; padding: 5px 3px; vertical-align: middle;">To :</span>
								<asp:TextBox ID="txtEndDate"
											 runat="server"
											 CssClass="SmlTxtBox"
											 style="vertical-align: middle;" />
								<rjs:PopCalendar ID="dtEndDate"
												 runat="server"
												 Control="txtEndDate"
												 Format="dd mmm yyyy"
												 ShowWeekend="True"
												 To-Today="true"
												 ShowErrorMessage="false" />
								<span class="ClsMdtStar"> * </span>
							</td>
							<td>
								<span style="font-size: 9pt;">
									<asp:CheckBox ID="chkDateRange"
												  runat="server"
												  Text="Date Range"
												  CausesValidation="false"
												  Checked="false"
												  OnClick="ChkOnChange(this);" />
								</span>
								<span style="font-size: 9pt;">
									<asp:CheckBox ID="chkIncludePending"
												  runat="server"
												  Text="Include Pending Vouchers"
												  CausesValidation="false"
												  Checked="false" />
								</span>
								<asp:Button ID="btnShow"
											runat="server"
											CssClass="ClsBtn"
											Text="Show"
											OnClick="btnShow_Click" />
								<asp:Button ID="btnChangeInput"
											runat="server"
											CssClass="ClsBtnMid"
											Text="Change Input"
											Visible="false"
											OnClick="btnChangeInput_Click" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<table style="width: 100%" cellspacing="0" cellpadding="0">
						<tr>
							<td>
								<asp:ObjectDataSource ID="objdsDayBook"
													  runat="server"
													  TypeName="SchoolBusinessService.AccountVoucherClient"
													  SelectMethod="GetAllVouchersForDayBook"
													  SelectCountMethod="GetAllVouchersForDayBookCount"
													  OnSelecting="objdsDayBook_Selecting"
													  EnablePaging="true" >
									<SelectParameters>
										<asp:SessionParameter Name="aiSchoolId"
															  SessionField="I_SCHOOL_ID"
															  Type="Int32" />
										<asp:SessionParameter Name="aiFinancialYearId"
															  SessionField="S_FINANCIAL_YEAR_ID"
															  Type="Int32" />
										<asp:ControlParameter ControlID="txtStartDate"
															  PropertyName="Text"
															  Name="adtStartDate"
															  Type="DateTime" />
										<asp:ControlParameter ControlID="txtEndDate"
															  PropertyName="Text"
															  Name="adtEndDate"
															  Type="DateTime" />
										<asp:ControlParameter ControlID="chkIncludePending"
															  PropertyName="Checked"
															  Name="abIncludePending"
															  Type="Boolean" />
										<asp:ControlParameter ControlID="hidSortExpression"
															  PropertyName="Value"
															  Name="sortExpression"
															  Type="String" />
										<asp:ControlParameter ControlID="hidSortDirection"
															  PropertyName="Value"
															  Name="sortDirection"
															  Type="String" />
										<asp:Parameter Name="startRowIndex" Type="Int32" />
										<asp:Parameter Name="maximumRows" Type="Int32" />
									</SelectParameters>
								</asp:ObjectDataSource>
								<asp:ListView ID="lstvwDayBook"
											  runat="server"
											  DataSourceID="objdsDayBook"
											  DataKeyNames="VoucherId,Status,IsFeeVoucher,IsInternalFeeVoucher"
											  OnItemDataBound="lstvwDayBook_ItemDataBound"
											  OnDataBound="lstvwDayBook_DataBound"
											  OnItemCommand="lstvwDayBook_ItemCommand" >
									<LayoutTemplate>
										<table style="margin: 5px 0;">
											<tr>
												<td>
													<span class="ClsLblLgnd">Legend</span>
												</td>
												<td>
													<span style="display: inline-block; background-color: LightBlue; border: 1px solid black; height: 20px; width: 20px;">
													</span>
												</td>
												<td>
													<span class="ClsLblLgnd">Fee Voucher</span>
												</td>
												<td>
													<span style="display: inline-block; background-color: LightPink; border: 1px solid black; height: 20px; width: 20px;">
													</span>
												</td>
												<td>
													<span class="ClsLblLgnd">Pending Voucher</span>
												</td>
                                                <td style="width:5px;">
                                                </td>
                                                <td style="border:1px solid black;">
													<span class="ClsLblLgnd" style="color:Maroon; padding-left:5px; padding-right:5px;font-weight:550;">Internal Fee Voucher</span>
												</td>
											</tr>
										</table>
										<table cellpadding="3" cellspacing="0" width="100%">
											<tr>
												<td align="center">
													<asp:DataPager ID="DtPgCount"
																   runat="server"
																   PagedControlID="lstvwDayBook"
																   PageSize="20">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" EnableViewState="false" Text="<%# Container.StartRowIndex + 1%>" />
																	<span class="LblNormal"> To </span>
																	<asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount) ? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
																	<span class="LblNormal"> Out of </span>
																	<asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" Text="<%# Container.TotalRowCount%>" />
																	<span class="LblNormal"> Records</span>
																	<br />
																</PagerTemplate>
															</asp:TemplatePagerField>
														</Fields>
													</asp:DataPager>
												</td>
											</tr>
										</table>
										<table width="100%" border="0" cellpadding="3" cellspacing="1" class="GridBorder">
											<tr id="trHeader" runat="server" class="ClsGridHeader">
												<th style="font-size: 9pt; padding: 1px; width: 100px; white-space: nowrap;">
													<asp:LinkButton ID="lnbtnSerialNo"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT_ROW"
																	CommandArgument="SerialNumber"
																	Text="Sr. No."
																	ForeColor="Black" />
												</th>
												<th style="font-size: 9pt; width: 120px; white-space: nowrap;">
													<asp:LinkButton ID="lnkbtnCreatedOn"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT_ROW"
																	CommandArgument="VoucherDate"
																	Text="Voucher Date"
																	ForeColor="Black" />
												</th>
												<th style="font-size: 9pt; width: 120px; white-space: nowrap;">
													<asp:LinkButton ID="lnkbtnVoucherType"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT_ROW"
																	CommandArgument="VoucherType"
																	Text="Voucher Type"
																	ForeColor="Black" />
												</th>
												<th align="left" style="font-size: 9pt; width: 250px; white-space: nowrap;">
													<asp:LinkButton ID="lnkbtnCreatedBy"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT_ROW"
																	CommandArgument="CreatedBy"
																	Text="Created By"
																	ForeColor="Black" />
												</th>
												<th align="right" style="font-size: 9pt; width: 100px; padding-right: 6px; white-space: nowrap;">
													<asp:LinkButton ID="lnkbtnAmount"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT_ROW"
																	CommandArgument="TotalAmount"
																	Text="Amount (Rs.)"
																	ForeColor="Black" />
												</th>
												<th style="font-size: 9pt;">
													View
												</th>
											</tr>
											<tr id="itemPlaceHolder" runat="server"></tr>
											<tr id="trDataPager" runat="server" class="ClsBorderPager">
												<td colspan="6">
													<asp:DataPager ID="DtPgDropDown"
																   runat="server"
																   PagedControlID="lstvwDayBook"
																   PageSize="20">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<table width="100%">
																		<tr>
																			<td align="left">
																				<span class="LblNrmlB">Select a page :</span>
																				<asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged" />
																			</td>
																			<td align="right" class="LblNormal">
																				<asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNormal" />
																			</td>
																		</tr>
																	</table>
																</PagerTemplate>
															</asp:TemplatePagerField>
														</Fields>
													</asp:DataPager>
												</td>
											</tr>
										</table>
									</LayoutTemplate>
									<ItemTemplate>
										<tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
											<td align="center">
												<%# Eval("SerialNumber") %>
											</td>
											<td align="center">
												<%# Convert.ToDateTime(Eval("Date")).ToString("dd-MMM-yyyy")%>
											</td>
											<td align="center">
												<%# Eval("VoucherType.Name") %>
											</td>
											<td align="left">
												<%# Eval("CreatedBy") %>
											</td>
											<td align="right">
												<span style="padding-right: 2px;"><%# Utility.CommonUtility.FormatCurrency(Eval("Amount")) %></span>
											</td>
											<td align="center">
												<asp:ImageButton ID="imgbtnView"
																 runat="server"
																 AlternateText="View"
																 ToolTip="View"
																 CausesValidation="false"
																 ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif" />
											</td>
										</tr>
									</ItemTemplate>
									<EmptyDataTemplate>
										<div class="LblNoRecord" style="margin: 10px 0; text-align: center; width: auto;">No record found.</div>
									</EmptyDataTemplate>
								</asp:ListView>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		
		<%-- HIDDEN FIELDS --%>
		<asp:HiddenField ID="hidSortExpression" runat="server" />
		<asp:HiddenField ID="hidSortDirection" runat="server" />
		
		<%-- VALIDATOR --%>
		<asp:CustomValidator ID="cstStartDateValidator"
							 runat="server"
							 Display="None"
							 ClientValidationFunction="ValidateStartDate"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="cstEndDateValidator"
							 runat="server"
							 Display="None"
							 ClientValidationFunction="ValidateEndDate"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="cstDateValidator"
							 runat="server"
							 Display="None"
							 ClientValidationFunction="ValidateDates"
							 EnableClientScript="true" />
		
		<table width="800px" cellspacing="2" cellpadding="0">
			<tr>
				<td align="center">
					<table style="width: 100%" cellspacing="2">
						<tr>
							<td align="left" class="ClsBorderlight " style="background-color: #ffffc4; padding: 3px 6px 3px 3px;">
								<span class="LblNrmlB" style="padding: 0; white-space: nowrap;">Note :</span>
							</td>
							<td align="left" class="ClsBorderlight" style="padding: 3px 3px 3px 6px;">
								<span class="LblSmlV">You may not be able to import the exported file of voucher details in Tally if the master data (ledgers, voucher types etc.) is not matching with the configuration in the school software.</span>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:Button ID="btnExport"
								runat="server"
								CssClass="ClsBtn"
								Text="Export"
								OnClick="btnExport_Click" />
                    <asp:Button ID="btnExportToExcel"
								runat="server"
								CssClass="ClsBtn"
								Text="Export to Excel" onclick="btnExportToExcel_Click" />
                    <asp:Button ID="btnExportDayBookDetails"
                                runat="server"
                                CssClass="ClsBtn"
                                Text="Export DayBook Details"
                                OnClick="btnExportDayBookDetails_Click" />
				</td>
			</tr>
		</table>
	</ContentTemplate>
	<Triggers>
		<asp:PostBackTrigger ControlID="btnExport" />
        <asp:PostBackTrigger ControlID="btnExportToExcel" />
        <asp:PostBackTrigger ControlID="btnExportDayBookDetails" />
	</Triggers>
</asp:UpdatePanel>

<%-- JAVASCRIPT --%>
<script type="text/javascript">
var _clienttdEndDate = '<%= this.tdEndDate.ClientID %>';
var _clientlblStartDate = '<%= this.lblStartDate.ClientID %>';
var _clienttxtStartDate = '<%= this.txtStartDate.ClientID %>';
var _clienttxtEndDate = '<%= this.txtEndDate.ClientID %>';
var _clientchkDateRange = '<%= this.chkDateRange.ClientID %>';
var _clientbtnShow = '<%= this.btnShow.ClientID %>';


// Register listeners for Postbacks
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(BeginRequestHandler);
prm.add_endRequest(EndRequestHandler);


/* -----------------------
 *	PAGE REQUEST HANDLERS
 * -----------------------
 */

// This function is used to disable controls on the page when a postback occurs.
function BeginRequestHandler() {
	ToggleControls(true);
}

// This function is used to enabled controls once a postback is complete.
function EndRequestHandler() {
	ToggleControls(false);

	// Show/hide the End Date controls based on Range checked.
	var chkDateRange = $get(_clientchkDateRange);
	var lblStartDate = $get(_clientlblStartDate);
	if (lblStartDate)
		lblStartDate.innerHTML = chkDateRange.checked ? 'From :' : 'Date :';
	var tdEndDate = $get(_clienttdEndDate);

	if (tdEndDate)
		tdEndDate.style.display = chkDateRange.checked ? '' : 'none';
}


/* ------------------
 *	HELPER FUNCTIONS
 * ------------------
 */

// This function is used to the toggle the disabled state of input buttons on the page.
function ToggleControls(state) {
	var btnShow = $get(_clientbtnShow);
	
	if(btnShow)
		btnShow.disabled = state;
}

function DateStr() {
	return $get(_clientchkDateRange).checked ? 'From date' : 'Date';
}

function IsValidDate(date) {
	if(typeof(date) == 'string')
		date = new Date(date);
	return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
}


/* ----------------
 *	EVENT HANDLERS
 * ----------------
 */

// This event is fired when the 'Date Range' checkbox is clicked.
// It is used to show/hide the 'To' date selection feilds.
function ChkOnChange(src) {
	var lblStartDate = $get(_clientlblStartDate);
	var tdEndDate = $get(_clienttdEndDate);
	
	if(lblStartDate)
		lblStartDate.innerHTML = src.checked ? 'From :' : 'Date :';
	if(tdEndDate)
		tdEndDate.style.display = src.checked ? '' : 'none';
}


/* ----------------------
 *	VALIDATION FUNCTIONS
 * ----------------------
 */
 
 // This function validates the start date
function ValidateStartDate(src, args) {
	var txtStartDate = $get(_clienttxtStartDate);
	var dtStr = DateStr();

	args.IsValid = true;

	if(txtStartDate.value.trim() == '') {
		args.IsValid = false;
		src.errormessage = dtStr + ' should be selected.';	
	}
	else {
		var dtToday = new Date();
		var dtStartDate = new Date(txtStartDate.value.replace(/-/g, ' '));
		
		if(!IsValidDate(dtStartDate)) {
			args.IsValid = false;
			src.errormessage = 'Please select a valid ' + dtStr + '.';
		}
		else if(dtStartDate > dtToday) {
			args.IsValid = false;
			src.errormessage = dtStr + ' should not be a future date.';
		}
	}

	return !args.IsValid;
}

// This function validates the end date.
function ValidateEndDate(src, args) {
	var chkDateRange = $get(_clientchkDateRange);
	
	args.IsValid = true;
	
	if(chkDateRange.checked) {
		var txtEndDate = $get(_clienttxtEndDate);
		
		if(txtEndDate.value.trim() == '') {
			args.IsValid = false;
			src.errormessage = 'To date should be selected.';
		}
		else {
			var dtToday = new Date();
			var dtEndDate = new Date(txtEndDate.value.replace(/-/g, ' '));
			
			if(!IsValidDate(dtEndDate)) {
				args.IsValid = false;
				src.errormessage = 'Please select a valid To date.';
			}
			else if(dtEndDate > dtToday) {
				args.IsValid = false;
				src.errormessage = 'To date should not be a future date.';
			}
		}
	}
	
	return !args.IsValid;
}

// This function validates both the dates to check if start date is not greater than end date.
function ValidateDates(src, args) {
	var chkDateRange = $get(_clientchkDateRange);
	var dtStr = DateStr();
	
	args.IsValid = true;
	
	if(chkDateRange.checked) {
		var txtStartDate = $get(_clienttxtStartDate);
		var txtEndDate = $get(_clienttxtEndDate);
		var dtStartDate = new Date(txtStartDate.value.replace(/-/g,' '));
		var dtEndDate = new Date(txtEndDate.value.replace(/-/g,' '));
		
		if(IsValidDate(dtStartDate) && IsValidDate(dtEndDate) && dtStartDate > dtEndDate) {
			args.IsValid = false;
			src.errormessage = dtStr + ' should not be greater than To date.';
		}
	}
	
	return !args.IsValid;
}
</script>
</asp:Content>

