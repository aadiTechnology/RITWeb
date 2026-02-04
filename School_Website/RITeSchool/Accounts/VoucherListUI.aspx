<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="VoucherListUI.aspx.cs" Inherits="VoucherListUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
    <div id="divAccessMessage" runat="server" class="LblNoRecord" style="margin: 10px 0; width: 750px; text-align: center;" visible="false">You do not have access rights to create or approve Vouchers. Please contact the Accounts department.</div>
<asp:UpdatePanel ID="mainUpdatePanel"
				 runat="server">
	<ContentTemplate>
		<table cellpadding="0" cellspacing="4">
			<tr>
				<td align="center" valign="middle">
					<asp:Label ID="lblMessage"
							   runat="server"
							   CssClass="ClsLabel"
							   Visible="false"
							   EnableViewState="false"
							   style="width: 100%; text-align: center; margin: 5px 0;" />
				</td>
			</tr>
			<tr id="trLedgerLink" runat="server" Visible="False">
				<td align="right">
					<div class="ClsGreenBG" style="float: right; height: 18px; vertical-align: bottom; padding-top: 4px; padding-right: 2px">
						<a href="LedgerMasterUI.aspx" class="SubTitle">Add Ledger</a>
                    </div>
				</td>
			</tr>
			<tr id="trStatusDDL" runat="server">
				<td align="center" valign="middle">
					<table>
						<tr>
							<td class="ClsBorderlight">
								<span class="ClsLabel" style="float: none; margin-right: 5px;">Status : </span>
							</td>
							<td>
								<asp:DropDownList ID="ddlStatus"
												  runat="server"
												  CssClass="LrgCombo"
												  AutoPostBack="true"
												  OnSelectedIndexChanged="ddlStatus_OnSelectedIndexChanged"/>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td align="center">
					<asp:ObjectDataSource ID="objdsVouchers"
										  runat="server"
										  TypeName="SchoolBusinessService.AccountVoucherClient"
										  SelectMethod="GetAllVouchers"
										  SelectCountMethod="GetCount"
										  EnablePaging="true">
						<SelectParameters>
							<asp:SessionParameter Name="aiSchoolId"
												  SessionField="I_SCHOOL_ID"
												  Type="Int32" />
							<asp:SessionParameter Name="aiAcademicYearId"
												  SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
												  Type="Int32" />
							<asp:SessionParameter Name="aiFinancialYearId"
												  SessionField="S_FINANCIAL_YEAR_ID"
												  Type="Int32" />
							<asp:SessionParameter Name="aiUserId"
												  SessionField="I_USER_ID"
												  Type="Int32" />
							<asp:ControlParameter Name="aiStatusId"
												  ControlID="ddlStatus"
												  PropertyName="SelectedValue"
												  Type="Int32"
												  DefaultValue="4" />
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
					<asp:ListView ID="lstvwVouchers"
								  runat="server"
								  DataKeyNames="VoucherId,NextApproverDesigId,IsSubmitted"
								  OnDataBound="lstvwVouchers_DataBound"
								  OnItemCommand="lstvwVouchers_ItemCommand"
								  OnItemDataBound="lstvwVouchers_ItemDataBound">
						<LayoutTemplate>
							<table>
								<tr>
									<td align="center">
										<asp:DataPager ID="DtPgCount"
													   runat="server"
													   PagedControlID="lstvwVouchers"
													   PageSize="20">
											<Fields>
												<asp:TemplatePagerField>
													<PagerTemplate>
														<asp:Label ID="CurrentPageLabel"
																   runat="server"
																   CssClass="LblNrmlB"
																   EnableViewState="false"
																   Text="<%# Container.StartRowIndex + 1%>" />
														<asp:Label ID="lblTo"
																   runat="server"
																   EnableViewState="false"
																   CssClass="LblNormal"
																   Text=" To " />
														<asp:Label ID="TotalPagesLabel"
																   runat="server"
																   CssClass="LblNrmlB"
																   Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
														<asp:Label ID="lblOutOf"
																   runat="server"
																   CssClass="LblNormal"
																   EnableViewState="false"
																   Text=" Out Of " />
														<asp:Label ID="TotalItemsLabel"
																   runat="server"
																   CssClass="LblNrmlB"
																   Text="<%# Container.TotalRowCount%>" />
														<asp:Label ID="lblRecords"
																   runat="server"
																   CssClass="LblNormal"
																   EnableViewState="false"
																   Text="Records" />
														<br />
													</PagerTemplate>
												</asp:TemplatePagerField>
											</Fields>
										</asp:DataPager>
									</td>
								</tr>
							</table>
							<table border="0" cellpadding="3" cellspacing="1" class="GridBorder" width="850px">
								<tr id="trHeader" runat="server" class="ClsGridHeader">
									<th align="center" style="font-size: 9pt; padding: 1px; width: 85px; white-space: nowrap;">
										<asp:LinkButton ID="lnbtnSerialNo"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="SerialNumber"
														Text="Sr. No."
														ForeColor="Black" />
									</th>
									<th align="center" style="font-size: 9pt; width: 85px; white-space: nowrap;">
										<asp:LinkButton ID="lnkbtnCreatedOn"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="CreatedOn"
														Text="Created On"
														ForeColor="Black" />
									</th>
									<th align="center" style="font-size: 9pt; width: 100px; white-space: nowrap;">
										<asp:LinkButton ID="lnkbtnVoucherType"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="VoucherType"
														Text="Voucher Type"
														ForeColor="Black" />
									</th>
									<th id="thCreatedBy" runat="server" align="left" style="font-size: 9pt; width: 170px; white-space: nowrap;">
										<asp:LinkButton ID="lnkbtnCreatedBy"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="CreatedBy"
														Text="Created By"
														ForeColor="Black" />
									</th>
									<th align="right" style="font-size: 9pt; width: 80px; padding-right: 6px; white-space: nowrap;">
										<asp:LinkButton ID="lnkbtnAmount"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="Amount"
														Text="Amount (Rs.)"
														ForeColor="Black" />
									</th>
									<th id="thNextApprover" runat="server" align="center" style="font-size: 9pt; width: 150px; white-space: nowrap;">
										<asp:LinkButton ID="lnkbtnNextApprover"
														runat="server"
														CausesValidation="false"
														CommandName="SORT_ROW"
														CommandArgument="NextApprover"
														Text="Next Approver"
														ForeColor="Black" />
									</th>
									<th id="thIsSubmitted" runat="server" align="center" style="font-size: 9pt; width: 80px; white-space: nowrap;">Is Submitted?</th>
									<th align="center" style="font-size: 9pt; width: 80px; white-space: nowrap;">Actions</th>
								</tr>
								<tr id="itemPlaceHolder" runat="server"></tr>
								<tr id="trDataPager" runat="server" class="ClsBorderPager">
									<td colspan="8">
										<asp:DataPager ID="DtPgDropDown"
													   runat="server"
													   PagedControlID="lstvwVouchers"
													   PageSize="20">
											<Fields>
												<asp:TemplatePagerField>
													<PagerTemplate>
														<table width="100%">
															<tr>
																<td align="left">
																	<asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
																	<asp:DropDownList ID="ddlCnt"
																					  runat="server"
																					  AutoPostBack="true"
																					  OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
																	</asp:DropDownList>
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
									<%# ((DateTime)Eval("Date")).ToString("dd-MMM-yyyy") %>
								</td>
								<td align="center">
									<%# Eval("VoucherType.Name") %>
								</td>
								<td id="tdCreatedBy" runat="server">
									<%# Eval("CreatedBy") %>
								</td>
								<td align="right">
									<span style="padding-right: 2px;"><%# Utility.CommonUtility.FormatCurrency(Eval("Amount")) %></span>
								</td>
								<td id="tdNextApprover" runat="server" align="center">
									<asp:Label ID="lblNextApprover"
											   runat="server"
											   Text='<%# Eval("NextApprover") %>' />
								</td>
								<td id="tdIsSubmitted" runat="server" align="center">
									<img id="imgSubmitted"
										 runat="server"
										 src="../images/IconGrid_AssignTrue.gif"
										 alt="Submitted"
										 visible='<%# Convert.ToBoolean(Eval("IsSubmitted")) %>' />
									<asp:Button ID="btnSubmit"
												runat="server"
												Text="Submit"
												CssClass="ClsBtn"
												ToolTip="Submit for Approval"
												CommandName="SUBMIT_ROW"
												CausesValidation="false"
												UseSubmitBehavior="false"
												OnClientClick="if(!WarnOnSubmit()){return false;}"
												Visible='<%# !Convert.ToBoolean(Eval("IsSubmitted")) %>' />
								</td>
								<td align="center">
									<asp:ImageButton ID="imgbtnView"
													 runat="server"
													 AlternateText="View"
													 ToolTip="View"
													 CausesValidation="false"
													 ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
													 style="vertical-align: middle;" />
									<asp:ImageButton ID="imgbtnEdit"
													 runat="server"
													 AlternateText="Edit"
													 ToolTip="Edit"
													 CausesValidation="false"
													 ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
													 style="vertical-align: middle; margin-left: 3px;" />
									<asp:ImageButton ID="imgbtnDelete"
													 runat="server"
													 AlternateText="Delete"
													 ToolTip="Delete"
													 CausesValidation="false"
													 CommandName="DELETE_ROW"
													 ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
													 style="vertical-align: middle; margin-left: 3px;" />
								</td>
							</tr>
						</ItemTemplate>
						<EmptyDataTemplate>
							<div class="LblNoRecord" style="margin: 10px 0; width: 750px; text-align: center;">No record found.</div>
						</EmptyDataTemplate>
					</asp:ListView>
				</td>
			</tr>
		</table>
		
		<%-- HIDDEN FIELDS --%>
		<asp:HiddenField ID="hidSortExpression" runat="server" />
		<asp:HiddenField ID="hidSortDirection" runat="server" />
		<asp:HiddenField ID="hidStatusId" runat="server"/>
        <asp:HiddenField ID="hidUserAccess" runat="server" Value=""/>
		
	</ContentTemplate>
</asp:UpdatePanel>
<table width="100%">
	<tr>
		<td align="center">
			<asp:Button ID="btnAdd"
						runat="server"
						CssClass="ClsBtn"
						Text="Add"
						CausesValidation="false"
						UseSubmitBehavior="false" />
		</td>
	</tr>
</table>
<table id="tblLedgerAccessNotice" runat="server" visible="false" style="width: 850px; margin-top: 10px;">
	<tr>
		<td class="ClsBorderlight" style="background-color: #ffffc4; width: 40px;">
			<div class="LblNrmlB" style="padding: 4px;">Note :</div>
		</td>
		<td class="ClsBorderlight">
			<div class="LblSmlV" style="padding: 4px;">You do not have access of Ledgers Configuration screen. If you need to create a new Ledger for a Voucher, please contact the Accounts Officer.</div>
		</td>
	</tr>
</table>
<script type="text/javascript">
    _clienthidUserAccess = "<%=this.hidUserAccess.ClientID %>"
function WarnOnDelete() {
	return confirm('Are you sure you want to delete this Voucher?');
}
function WarnOnSubmit() {
	return confirm('Are you sure you want to submit this voucher for approval?');
}

function CheckConfiguration() {    
    var UserAccess = document.getElementById(_clienthidUserAccess).value
    if (UserAccess == "1")
        window.open('VoucherPopUp.aspx', '_blank', 'location=0,menubar=0,status=0,titlebar=0,toolbar=0,scrollbars=1,resizable=1,top=0,left=0,width=1000,height=600')
    else
        alert('Approval configuration is not present for your designation. Please contact admin officer to either configure it or enable self approval.')
    return false;
}
</script>
</asp:Content>