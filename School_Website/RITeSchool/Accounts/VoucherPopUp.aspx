<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="VoucherPopUp.aspx.cs" Inherits="VoucherPopUp" %>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>
<%@ Import Namespace="Utility" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
    <style type="text/css">
#ledgerPopup{display:none; position:fixed; overflow:auto; width:200px; border:1px solid gray; background-color:#fff; top:0; right:0; bottom: 0; text-align:left; z-index:999}
#contentPlaceholder {background-color: #F3F5E2; cursor: pointer;}
#contentPlaceholder div{padding:3px; margin:0px !important; height:auto !important;border-bottom:1px solid #ccc;}
#contentPlaceholder.cashbank .cash, #contentPlaceholder.cashbank .bank{display:none}
#contentPlaceholder.onlycashbank div{display:none}
#contentPlaceholder.onlycashbank div.cash, #contentPlaceholder.onlycashbank div.bank{display:block}
#printDialogWrapper{display:none; background:transparent url(../images/opaque.png); position:absolute; top:0; left:0; width:100%; height:100%; z-index:101}
#printDialogWrapper .container{background:white; border:2px solid #8FBC8F; box-shadow:0 0 10px rgba(0,0,0,0.55); width:340px; height:165px; margin:-85px 0 0 -170px; padding:2px; position:absolute; top:50%; left:50%}
#printDialogWrapper .dialogTitle{background:url("../images/GridHeaderBG.gif"); font-weight:bold; margin-bottom:2px; padding:3px; text-align:left}
#printDialogWrapper .content{width:100%}
#printDialogWrapper .dialogTitle .closeButton{cursor:pointer; position:absolute; right:5px}
.addButton{background:transparent url(../images/Add_Grace.png) no-repeat center}
.removeButton{background:transparent url(../images/IconGrid_Delete.gif) no-repeat center}
.removeButtonDisabled{background:transparent url(../images/IconGrid_DeleteDIs.gif) no-repeat center}
.viewButton{background:transparent url(../images/view.png) no-repeat center}
.printButton{background:transparent url(../images/icon_printer.gif) no-repeat center}
.addButton, .removeButton, .removeButtonDisabled, .viewButton, .printButton{display:block; width:20px; height:20px; cursor:pointer}
.match{font-weight:bold}
.current{background-color:#CCC}
.collapsiblePanel{width:580px;margin-top:5px;}
.collapsiblePanel .panelTitle{background-color:#EBCFD7;border-bottom:1px solid #A5C7A7;color:#000;font-size:10pt;font-family:Arial;padding:5px 5px 5px 24px;cursor:pointer;text-align:left;font-weight:bold;}
.collapsiblePanel.collapse .panelTitle{background:#EEBBBC url(../images/node_open.gif) no-repeat 5px 4px;}
.collapsiblePanel.expand .panelTitle{background:#EBCFD7 url(../images/node_close.gif) no-repeat 5px 4px;}
</style>
<!--[if IE 6]>
<style type="text/css">
.ie6fixed {position:absolute !important; top:expression(0+((e=document.documentElement.scrollTop)?e:document.body.scrollTop)+'px !important'); right:expression(0+((e=document.documentElement.scrollRight)?e:document.body.scrollRight)+'px !important');}
</style>
<![endif]-->
<asp:ListView ID="lstvwLedgers"
			  runat="server"
			  OnItemDataBound="lstvwLedgers_ItemDataBound">
	<LayoutTemplate>
		<div id="ledgerPopup" class="ie6fixed" onclick="CancelHide();" onblur="HidePopup();">
			<div id="contentPlaceholder">
				<div id="itemPlaceholder" runat="server"></div>
			</div>
		</div>
	</LayoutTemplate>
	<ItemTemplate>
		<div groupid='<%# Eval("Group.OriginalGroup.Id") %>' ledgerid='<%# Eval("Id") %>' class='<%# GetClassForLedger(Eval("Group.OriginalGroup.Id").ToInt()) %>'><%# Eval("Name") %></div>
	</ItemTemplate>
</asp:ListView>
<asp:UpdatePanel ID="printDialogUpdatePanel"
				 runat="server">
	<ContentTemplate>
		<div id="printDialogWrapper">
			<div class="container">
				<div class="dialogTitle">
					Print Options
					<img class="closeButton img-align-top" title="Close Dialog" alt="Close" src="../images/close_vista.gif" onclick="HidePrintDialog(); return false;" />
				</div>
				<table cellpadding="3" cellspacing="1" class="content">
					<tr>
						<td colspan="3" align="right" class="ClsMdtStar">
							* Mandatory Fields
						</td>
					</tr>
					<tr>
						<td align="left">Template :</td>
						<td colspan="2" align="left">
							<select id="ddlTemplateList" style="width: 199px;"></select>
						</td>
					</tr>
					<tr>
						<td align="left">Payee :</td>
						<td colspan="2" align="left">
							<asp:TextBox ID="txtPayeeName"
											runat="server"
											CssClass="LrgTxtBox"
											MaxLength="100" />
							<span class="ClsMdtStar">*</span>
						</td>
					</tr>
					<tr>
						<td align="left">Date :</td>
						<td align="left">
							<asp:TextBox ID="txtChequeDate"
										 runat="server"
										 CssClass="SmlTxtBox"
										 style="height: 15px; vertical-align: middle;" />
							<rjs:PopCalendar ID="rjsCalendarChq"
											 runat="server"
											 Control="txtChequeDate"
											 Format="dd MMM yyyy"
											 ShowWeekend="True"
											 ShowErrorMessage="false"
											 InvalidDateMessage="Please select a valid Cheque date."
											 From-Message="Please select a valid Cheque date."
											 To-Message="Please select a valid Cheque date." />
							<span class="ClsMdtStar">*</span>
						</td>
						<td align="left" valign="top">
							<asp:CheckBox ID="chkCrossCheque"
										  runat="server"
										  Checked="true"
										  Text="Cross Cheque" />
						</td>
					</tr>
					<tr>
						<td colspan="3" align="center">
							<asp:Button ID="btnPrintCheque"
										runat="server"
										CssClass="ClsBtnMid"
										Text="Print Cheque"
										CausesValidation="false"
										OnClientClick="if(!ValidatePrintChequeInput()){return false;}" onclick="btnPrintCheque_Click" />
							<asp:Button ID="btnCancel"
										runat="server"
										CssClass="ClsBtn"
										Text="Cancel"
										CausesValidation="false"
										UseSubmitBehavior="false"
										OnClientClick="HidePrintDialog(); return false;"
										style="margin-left: 3px;" />
						</td>
					</tr>
				</table>
			</div>
		</div>
	</ContentTemplate>
	<Triggers>
		<asp:PostBackTrigger ControlID="btnExport" />
	</Triggers>
</asp:UpdatePanel>
<asp:UpdatePanel ID="mainUpdatePanel"
				 runat="server"
				 ChildrenAsTriggers="true"
				 UpdateMode="Conditional">
	<ContentTemplate>
		<table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="width: 96%; float: none; padding-right: 5px; margin: 10px 0;">
			<tr>
				<td align="left" style="height: 20px">
					<asp:Label ID="lblMainTitle"
							   runat="server"
							   CssClass="MainTitleHead"
							   Font-Bold="true" />
				</td>
			</tr>
		</table>
		<table cellspacing="1" cellpadding="0" style="width: 580px;">
            <tr id="trMdtStar" runat="server">
				<td align="right" colspan="2">
					<span class="ClsMdtStar">* Mandatory Fields</span>
				</td>
            </tr>
			<tr id="trMessageRow" runat="server">
				<td align="left" colspan="2" style="width: 100%;">
					<asp:ValidationSummary ID="valSummarySave"
										   runat="server"
										   CssClass="ClsLabel"
										   ShowSummary="true"
										   ValidationGroup="Save" />
					<asp:ValidationSummary ID="valSummaryApprove"
										   runat="server"
										   CssClass="ClsLabel"
										   ShowSummary="true"
										   ValidationGroup="Approval" />
					<asp:Label ID="lblErrorMessage"
							   runat="server"
							   EnableViewState="false"
							   CssClass="ClsLabel"
							   style="width: 100%; text-align: center; margin: 8px 0;"
							   ForeColor="Red"
							   Visible="false" />
					<asp:Label ID="lblUpateMessage"
							   runat="server"
							   EnableViewState="false"
							   CssClass="ClsLabel"
							   style="width: 100%; text-align: center; margin: 8px 0;"
							   ForeColor="Blue"
							   Font-Bold="true"
							   Visible="false" />
				</td>
			</tr>
			<tr id="trViewRow" runat="server">
				<td align="left">
					<table cellpadding="0" cellspacing="2">
						<tr>
							<td class="ClsBorderlight" style="height: 24px;">
								<span class="ClsLblLgnd" style="padding: 0 4px;">Serial No. :</span>
							</td>
							<td class="ClsHilightBGB">
								<asp:Label ID="lblSerialNo"
										   runat="server"
										   class="ClsLabel"
										   style="margin: 0px 5px 0px 1px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        	padding: 0px;" />
							</td>
						</tr>
						<tr>
							<td class="ClsBorderlight" style="height: 24px;">
								<span class="ClsLblLgnd" style="padding: 0 4px;">Date :</span>
							</td>
							<td class="ClsHilightBGB">
								<asp:Label ID="lblDate"
										   runat="server"
										   class="ClsLabel"
										   style="margin: 0px 5px 0px 1px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        	                                                                                      	padding: 0px;" />
							</td>
						</tr>
					</table>
				</td>
				<td align="right">
					<table cellpadding="0" cellspacing="2">
						<tr>
							<td class="ClsBorderlight" style="height: 24px;">
								<span class="ClsLblLgnd" style="padding: 0 4px;">Voucher Type :</span>
							</td>
							<td class="ClsHilightBGB">
								<asp:Label ID="lblVoucherType"
										   runat="server"
										   class="ClsLabel"
										   style="margin: 0px 5px 0px 1px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        	                                                                                      	                                                                                      	padding: 0px;" />
							</td>
						</tr>
						<tr>
							<td class="ClsBorderlight" style="height: 24px;">
								<span class="ClsLblLgnd" style="padding: 0 4px;">Created By :</span>
							</td>
							<td class="ClsHilightBGB">
								<asp:Label ID="lblCreatedBy"
										   runat="server"
										   class="ClsLabel"
										   style="margin: 0px 5px 0px 1px;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        	                                                                                      	                                                                                      	                                                                                      	padding: 0px;" />
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr id="trAddRow" runat="server">
				<td align="left" style="width: 50%;">
					<table>
						<tr>
							<td class="ClsBorderlight">
								<span class="ClsLabel">Voucher Type : </span>
							</td>
							<td style="padding: 0px;">
								<asp:DropDownList ID="ddlVoucherTypes"
												  runat="server"
												  CssClass="MidCombo"
												  onchange="VoucherTypeChange(this);" />
							</td>
						</tr>
					</table>
				</td>
				<td align="right">
					<span class="ClsBorderlight" style="font-size: 9pt; padding: 1px 3px; vertical-align: middle;">Date :</span>
					<asp:TextBox ID="txtVoucherDate"
								 runat="server"
								 CssClass="SmlTxtBox"
								 style="height: 15px; vertical-align: middle;" />
					<rjs:PopCalendar ID="dtVoucherDate"
									 runat="server"
									 Control="txtVoucherDate"
									 Format="dd MMM yyyy"
									 ShowWeekend="True"
									 ShowErrorMessage="false"
									 InvalidDateMessage="Please select a valid voucher date."
									 From-Message="Please select a valid voucher date."
									 To-Message="Please select a valid voucher date." />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<asp:ListView ID="lstvwVoucherDetails"
								  runat="server"
								  DataKeyNames="Id,IsDebit,Amount,Ledger"
								  OnDataBound="lstvwVoucherDetails_DataBound"
								  OnItemDataBound="lstvwVoucherDetails_ItemDataBound">
						<LayoutTemplate>
							<table ID="tblVoucherDetails" runat="server" border="0" cellpadding="3" cellspacing="1" class="GridBorder" style="width: 600px; margin-top: 10px;">
								<tr ID="trHeader" runat="server" class="ClsGridHeader">
									<th style="font-size: 9pt; width: 50px;">Sr. No.</th>
									<th id="thToBy" runat="server" style="font-size: 9pt;">To/By</th>
									<th align="left" style="font-size: 9pt; padding-left: 5px;">Particulars</th>
									<th align="right" style="font-size: 9pt; width: 80px;">Debit (Rs.)</th>
									<th align="right" style="font-size: 9pt; width: 80px;">Credit (Rs.)</th>
									<th id="thActionBtn" runat="server" style="font-size: 9pt;"></th>
								</tr>
								<tr ID="itemPlaceHolder" runat="server">
								</tr>
								<tr class="ClsBorderPager">
									<td id="tdTotal" runat="server" align="right" colspan="3">
										<span class="ClsUnread">Total (Rs.) :</span>
									</td>
									<td align="right">
										<asp:Label ID="lblDebitTotal" runat="server" class="ClsUnread" />
									</td>
									<td align="right">
										<asp:Label ID="lblCreditTotal" runat="server" class="ClsUnread" />
									</td>
									<td id="tdActionBtn" runat="server">
									</td>
								</tr>
							</table>
						</LayoutTemplate>
						<ItemTemplate>
							<tr ID="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
								<td align="center">
									<asp:Label ID="lblSrNo"
											   runat="server"
											   Text="<%# Container.DataItemIndex + 1 %>" />
								</td>
								<td align="center" visible="<%# IsAddMode %>">
									<asp:DropDownList ID="ddlToBy"
													  runat="server"
													  OnChange="ToByOnChange(this);"
													  style="width: 50px; font-size : 9pt;">
										<asp:ListItem Text="To" Value="0" />
										<asp:ListItem Text="By" Value="1" />
									</asp:DropDownList>
								</td>
								<td align="left">
									<asp:Label ID="lblLedger"
											   runat="server"
											   CssClass="ClsLabel"
											   Width="250px"
											   Visible="<%# !IsAddMode %>"
											   Text='<%# Eval("Ledger.Name") %>'/>
									<asp:HiddenField ID="hidGroupId" runat="server" Value='<%# Eval("Ledger.Group.Id") %>' />
									<asp:HiddenField ID="hidLedgerId" runat="server" Value='<%# Eval("Ledger.Id") %>' />
									<asp:TextBox ID="txtLedger"
												 runat="server"
												 autocomplete="off"
												 CssClass="LrgTxtBox"
												 MaxLength="100"
												 onblur="HidePopup(); if(VerifyLedger(this)){SelectLedger(this, event);}"
												 onfocus="ShowPopup(this);"
												 onkeydown="SelectLedger(this, event); return (event.keyCode!=13)"
												 onkeyup="FilterLedger(this, event);"
												 Visible="<%# IsAddMode %>"
												 Text='<%# Eval("Ledger.Name") %>'
												 Width="250px" />
								</td>
								<td align="right">
									<asp:Label ID="lblDebitAmount"
											   runat="server"
											   Visible="<%# !IsAddMode %>"
											   style="width: 75px;" />
									<asp:TextBox ID="txtDebitAmount"
												 runat="server"
												 CssClass="SmlTxtBox"
												 MaxLength="11"
												 autocomplete="off"
												 onblur="extractNumber(this,2,false); if(this.value.trim() != '' && VerifyAmount(this)){CalculateDebitTotal(this);}"
												 ondrop="event.returnValue=false"
												 onkeydown="if(event.keyCode==13 || (!event.shiftKey &amp;&amp; event.keyCode==9))ShowNextRow(this);"
												 onkeypress="return blockNonNumbers (this, event, true, false);"
												 onkeyup="extractNumber(this,2,false);"
												 onpaste="event.returnValue=false"
												 style="width: 75px; visibility: hidden; text-align: right;"
												 Visible="<%# IsAddMode %>" />
								</td>
								<td align="right">
									<asp:Label ID="lblCreditAmount"
											   runat="server"
											   Visible="<%# !IsAddMode %>"
											   style="width: 75px;" />
									<asp:TextBox ID="txtCreditAmount"
												 runat="server"
												 CssClass="SmlTxtBox"
												 MaxLength="11"
												 autocomplete="off"
												 onblur="extractNumber(this,2,false); if(this.value.trim() != '' && VerifyAmount(this)){CalculateCreditTotal(this);}"
												 ondrop="event.returnValue=false"
												 onkeydown="if(event.keyCode==13 || (!event.shiftKey &amp;&amp; event.keyCode==9))ShowNextRow(this);"
												 onkeypress="return blockNonNumbers (this, event, true, false);"
												 onkeyup="extractNumber(this,2,false);"
												 onpaste="event.returnValue=false"
												 style="width: 75px; text-align: right;"
												 Visible="<%# IsAddMode %>" />
								</td>
								<td align="center" visible="<%# IsFeeVoucher || IsAddMode || PrintCheque %>">
									<span ID="rowButton" runat="server" class="addButton" title="Add new row"></span>
								</td>
							</tr>
						</ItemTemplate>
					</asp:ListView>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<table style="width: 100%; margin-top: 10px;">
						<tr>
							<td valign="middle" align="left" class="ClsBorderlight" style="width: 70px;">
								<span class="ClsLabel">Narration :</span>
							</td>
							<td align="left" style="padding: 0;">
								<asp:Label ID="lblNarration"
										   runat="server"
										   CssClass="ClsBorderlight"
										   Visible="false"
										   style="display: block; width: 96%; margin: 0; padding: 3px;" />
								<asp:TextBox ID="txtNarration"
											 runat="server"
											 CssClass="LrgTxtBox"
											 TextMode="MultiLine"
											 style="width: 96%; height: 50px; margin: 0; padding: 3px;" />
							</td>
						</tr>
						<tr id="trComment" runat="server">
							<td valign="middle" align="left" class="ClsBorderlight" style="width: 70px;">
								<span class="ClsLabel">Comment :</span>
							</td>
							<td valign="top" align="left" style="padding: 0;">
								<asp:TextBox ID="txtComment"
											 runat="server"
											 CssClass="LrgTxtBox"
											 TextMode="MultiLine"
											 style="width: 96%; height: 50px; margin: 0; padding: 3px;" />
								<span class="ClsMdtStar" style="vertical-align: top;"> * </span>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
		
		<%-- HIDDEN FIELDS --%>
		<asp:HiddenField ID="hidPageMode" runat="server" />
		<asp:HiddenField ID="hidVoucherType" runat="server" />
		<asp:HiddenField ID="hidVoucherId" runat="server" />
		<asp:HiddenField ID="hidTotalAmount" runat="server" />
		<asp:HiddenField ID="hidInsertedById" runat="server" />
		<asp:HiddenField ID="hidCurrentDesigId" runat="server" />
		<asp:HiddenField ID="hidNextApproverDesigId" runat="server" />
		<asp:HiddenField ID="hidNextApproverDesigName" runat="server" />
		<asp:HiddenField ID="hidSourceStatusId" runat="server" Value="0" />
		<asp:HiddenField ID="hidIsFeeVoucher" runat="server" />
		<asp:HiddenField ID="hidFinancialYearJSON" runat="server" />
		<asp:HiddenField ID="hidCanEditOldFinancialYear" runat="server" />
		<asp:HiddenField ID="hidChqConfigJSON" runat="server" />
		<asp:HiddenField ID="hidChqConfigId" runat="server" />
		<asp:HiddenField ID="hidChqAmount" runat="server" />
        <asp:HiddenField ID="hidIsInternalFeeVoucher" runat="server" />
		
		<%-- VALIDATORS --%>
		<asp:CustomValidator ID="DateValidator"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateVoucherDate"
							 EnableClientScript="true" />
		<asp:RegularExpressionValidator ID="NarrationValidator"
										runat="server"
										Display="None"
										ControlToValidate="txtNarration"
										ValidationGroup="Save"
										ErrorMessage="Narration should not exceed 4000 characters."
										ValidationExpression="^[\s\S]{0,4000}$" />
		<asp:CustomValidator ID="VoucherValidator"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateVoucherParticulars"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="VoucherValidatorCashBank"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateVoucherCashBank"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="VoucherLedgerNameValidator"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateLedgerName"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="VoucherValidatorAmount"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateVoucherAmount"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="TotalValidator"
							 runat="server"
							 Display="None"
							 ValidationGroup="Save"
							 ClientValidationFunction="ValidateTotals"
							 EnableClientScript="true" />
		<asp:CustomValidator ID="CommentValidator"
							 runat="server"
							 Display="None"
							 ValidationGroup="Approval"
							 ClientValidationFunction="ValidateComment"
							 EnableClientScript="true" />
		

	<table style="width: 580px;">
		<tr id="trApprovalBtns" runat="server">
			<td align="center">
				<asp:Button ID="btnReject"
							runat="server"
							CssClass="ClsBtn"
							Text="Deny"
							ValidationGroup="Approval"
							OnClick="btnAction_Click"
							OnClientClick="if(!WarnOnReject()){return false}" />
				<asp:Button ID="btnApprove"
							runat="server"
							CssClass="ClsBtn"
							style="margin-left: 5px;"
							Text="Approve"
							ValidationGroup="Approval"
							OnClick="btnAction_Click"
							OnClientClick="if(!WarnOnApprove()){return false}" />
				<asp:CheckBox ID="chkFinalApprove"
							  runat="server"
							  Text="Final Approve"
							  style="margin-left: 5px;" />
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button ID="btnSave"
							runat="server"
                            disable-page="true"
							CssClass="ClsBtn"
							Text="Save"
							ValidationGroup="Save"
							OnClick="btnSave_Click"							
							OnClientClick="ClearMessages();" />
				<asp:Button ID="btnSubmit"
							runat="server"
							CssClass="ClsBtnMid"
                            disable-page="true"
							Text="Save & Submit"
							Visible="false"
							ValidationGroup="Save"
							OnClick="btnSave_Click"
							OnClientClick="ClearMessages();"
							style="margin-left: 5px;" />
				<asp:Button ID="btnSelfApprove"
							runat="server"
                            disable-page="true"
							CssClass="ClsBtnMid"
							Text="Save & Approve"
							Visible="false"
							ValidationGroup="Save"
							OnClick="btnSave_Click"
							OnClientClick="ClearMessages();"
							style="margin-left: 5px;" />
				<asp:Button ID="btnReset"
							runat="server"
							CssClass="ClsBtn"
							Text="Reset"
							CausesValidation="false"
							UseSubmitBehavior="false"
							style="margin-left: 5px;" />
				<asp:Button ID="btnClose"
							runat="server"
							CssClass="ClsBtn"
							Text="Close"
							CausesValidation="false"
							UseSubmitBehavior="false"
							OnClientClick="window.close();"
							style="margin-left: 5px;" /> 
                <asp:Button ID="btnPrint" 
                            runat="server" 
                            CausesValidation="true" 
                            Visible="false" 
                            CssClass="ClsBtnMid"
                            Text="Print Preview"/>
                <asp:Button ID="btnExport" 
                            runat="server" 
                            CausesValidation="true" 
                            Visible="false" 
                            CssClass="ClsBtnMid"
                            Text="Export"
                            OnClick="btnExport_Click"/>
                <asp:Button ID="btnExportToExcel" 
                            runat="server" 
                            CausesValidation="true" 
                            Visible="false" 
                            CssClass="ClsBtnMid"
                            Text="Export To Excel" onclick="btnExportToExcel_Click"/>
                <asp:HiddenField ID="hidQery" runat="server" />
			</td>
		</tr>
        <tr>
            <td align="center">
                <table id="tblNote" visible="false" runat="server">
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
		<tr id="trVoucherAction" runat="server" visible="false">
			<td align="center">
				<div class="collapsiblePanel collapse">
					<div class="panelTitle">Voucher Action History</div>
					<div class="panelContent" style="display: none;">
					<asp:ListView ID="lstvwVoucherAction"
								  runat="server">
						<LayoutTemplate>
							<table border="0" cellpadding="3" cellspacing="1" class="GridBorder" style="width: 580px; margin-top: 10px;">
								<tr ID="trHeader" runat="server" class="ClsGridHeader">
									<th align="center" style="font-size: 9pt; width: 85px;">Date</th>
									<th align="left" style="font-size: 9pt; width: 170px;">Name</th>
									<th align="left" style="font-size: 9pt; width: 260px;">Comment</th>
									<th align="center" style="font-size: 9pt; width: 75px;">Status</th>
								</tr>
								<tr id="itemPlaceHolder" runat="server"></tr>
							</table>
						</LayoutTemplate>
						<ItemTemplate>
							<tr ID="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
								<td align="center"><%# DateTime.Parse(Eval("InsertDate").ToString()).ToString("dd-MMM-yyyy") %></td>
								<td align="left"><%# Eval("UserName") %></td>
								<td align="left"><%# Eval("Comment") %></td>
								<td align="center"><%# ((Constants.RequisitionStatus)Eval("Status")).ToString() %></td>
							</tr>
						</ItemTemplate>
						<EmptyDataTemplate>
							<div class="LblNoRecord" style="margin: 10px 0; width: 570px; text-align: center; clear: both;">No Action taken on this voucher yet.</div>
						</EmptyDataTemplate>
					</asp:ListView>
					</div>
				</div>
			</td>
		</tr>
	</table>
	</ContentTemplate>
	<Triggers>
		<asp:PostBackTrigger ControlID="btnExport" />
        <asp:PostBackTrigger ControlID="btnExportToExcel" />
	</Triggers>
</asp:UpdatePanel>
<script type="text/javascript">
// Values bound from constants & code behind variables
var bankacgroupid = <%= Constants.AccountsGroups.BankAccounts.ToInt() %>;
var cashgroupid = <%= Constants.AccountsGroups.CashInHand.ToInt() %>;
var maxRows = <%= lstvwVoucherDetails.Items.Count %>;

// IDs of controls on page.
var _clientddlVoucherTypes = '<%= ddlVoucherTypes.ClientID %>';
var _clienthidVoucherType = '<%= hidVoucherType.ClientID %>';
var _clienttxtVoucherDate = '<%= txtVoucherDate.ClientID %>';
var _clienthidCurrentDesigId = '<%= hidCurrentDesigId.ClientID %>';
var _clienthidNextApproverDesigId = '<%= hidNextApproverDesigId.ClientID %>';
var _clienthidNextApproverDesigName = '<%= hidNextApproverDesigName.ClientID %>';
var _clientvalSummarySave = '<%= valSummarySave.ClientID %>';
var _clientlblErrorMessage = '<%= lblErrorMessage.ClientID %>';
var _clientlblUpdateMessage = '<%= lblUpateMessage.ClientID %>';
var _clienthidTotalAmount = '<%= hidTotalAmount.ClientID %>';
var _clientlstvwVoucherDetails = '<%= lstvwVoucherDetails.ClientID %>';
var _clientchkFinalApprove = '<%= chkFinalApprove.ClientID %>';
var _clienttxtNarration = '<%= txtNarration.ClientID %>';
var _clienttxtComment = '<%= txtComment.ClientID %>';
var _clientbtnSave = '<%= btnSave.ClientID %>';
var _clientbtnSelfApprove = '<%= btnSelfApprove.ClientID %>';
var _clientbtnClose = '<%= btnClose.ClientID %>';

// Local variables for use.
var hideTimeout, currentLedgerTextbox;

// Common string variables;
var _empty = '';
var _space = ' ';
var _visible = 'visible';
var _hidden = 'hidden';
var _none = 'none';
var _AllLedgers = 'AllLedgers';
var _FilteredLedgers = 'FilteredLedgers';
var _match = 'match';
var _current = 'current';
var __current = ' current';
var _div = 'div';
var _Payment = 'Payment';
var _Receipt = 'Receipt';
var _ctrl = '_ctrl';
var _trGridRow = '_trGridRow';
var _ddlToBy = '_ddlToBy';
var _hidGroupId = '_hidGroupId';
var _hidLedgerId = '_hidLedgerId';
var _txtLedger = '_txtLedger';
var _txtDebitAmount = '_txtDebitAmount';
var _txtCreditAmount = '_txtCreditAmount';
var _rowButton = '_rowButton';
var _addButton = 'addButton';
var _removeButton = 'removeButton';
var _removeButtonDisabled = 'removeButtonDisabled';
// Financial year related
var _FinancialYear = eval('[' + $get('<%= hidFinancialYearJSON.ClientID %>').value + ']')[0];
var _CanEditOldFinancialYear = Boolean($get('<%= hidCanEditOldFinancialYear.ClientID %>').value);

// Cheque Printing
var _ChqConfigs = eval($('#<%= hidChqConfigJSON.ClientID %>').val());
_ChqConfigs = _ChqConfigs && _ChqConfigs.length > 0 ? _ChqConfigs[0] : null;

// Register listeners for Postbacks
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_beginRequest(BeginRequestHandler);
prm.add_endRequest(EndRequestHandler);

OnLoad();

/* -----------------------
 *	PAGE REQUEST HANDLERS
 * -----------------------
 */

// This function is used to disable controls on the page when a postback occurs.
function BeginRequestHandler() {
	// This needs to be done since disabled controls do not reflect their real state in a postback.
	// So we need to enable it before posting back.
	var ddlToBy = $get(_clientlstvwVoucherDetails + '_ctrl0_ddlToBy');
	if(ddlToBy)
		ddlToBy.disabled = false;
	
	ToggleControls(true);
}

// This function is used to enabled controls once a postback is complete.
function EndRequestHandler(sender, args) {
	ToggleControls(false);
}


/* -----------------------
 *	MISC HELPER FUNCTIONS
 * -----------------------
 */

// Returns an escaped string for use in a regular expression
function escapeRegExp(str) {
  return str.replace(/[-\[\]\/\{\}\(\)\*\+\?\.\\\^\$\|]/g, "\\$&");
}

function IsValidDate(date) {
	if(typeof(date) == 'string')
		date = new Date(date);
	return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
}

function FormatCurrency(nStr) {
	nStr += '';
	x = nStr.split('.');
	x1 = x[0];
	x2 = x.length > 1 ? '.' + x[1] : '';
	var rgx = /(\d+)(\d{3})/;
	var z = 0;
	var len = String(x1).length;
	var num = parseInt((len/2)-1);
 
	while (rgx.test(x1)) {
		if(z > 0) {
		  x1 = x1.replace(rgx, '$1' + ',' + '$2');
		}
		else {
		  x1 = x1.replace(rgx, '$1' + ',' + '$2');
		  rgx = /(\d+)(\d{2})/;
		}
		z++;
		num--;
		if(num == 0)
			break;
	}
	return x1 + x2;
}


/* ----------------------
 *	VALIDATION FUNCTIONS
 * ----------------------
 */

// This function validate the Voucher Date
function ValidateVoucherDate(src, args) {
	var txtVoucherDate = $get(_clienttxtVoucherDate);
	args.IsValid = true;
	
	if(txtVoucherDate.value.trim() == '') {
		args.IsValid = false;
		src.errormessage = 'Date should not be empty.';
	}
	else {
		var dtToday = new Date();
		var dtVoucherDate = new Date(txtVoucherDate.value.replace(/-/g,' '));
	
		if(!IsValidDate(dtVoucherDate)) {
			args.IsValid = false;
			src.errormessage = 'Please select a valid Date.';
		}
		else if(dtVoucherDate > dtToday) {
			args.IsValid = false;
			src.errormessage = 'Date should not be a future date.';
		}
		
		// Check if the voucher date falls within the selected financial year.
		var financialYearStartDate = new Date(parseInt(_FinancialYear.StartDate.replace("/Date(", "").replace(")/",""), 10));
		var financialYearEndDate = new Date(parseInt(_FinancialYear.EndDate.replace("/Date(", "").replace(")/",""), 10));
		if (dtVoucherDate < financialYearStartDate || dtVoucherDate > financialYearEndDate) {
			args.IsValid = false;
			src.errormessage = 'Voucher date must be within current financial year (i.e. from 1-April-' + financialYearStartDate.getFullYear() + ' to 31-March-' + financialYearEndDate.getFullYear() + ').';
		}
	}
}

// This function validates if the ledgerid's and their respective ledgernames match
function ValidateVoucherParticulars(src, args) {
	args.IsValid = true;
	var ledgers = $('div', '#ledgerPopup #contentPlaceholder');
	var bExists = false;
	var invalidLedgers = [];
	for (var i = 0; i < maxRows; i++) {
		bExists = false;
		var row = $get(_clientlstvwVoucherDetails + _ctrl + i + _trGridRow);
		if (!row || row.style.display == _none)
			continue;
		var ledgerName = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtLedger).value.trim();
		if (ledgerName == _empty)
			continue;
		var groupId = $get(_clientlstvwVoucherDetails + _ctrl + i + _hidGroupId).value;
		var ledgerId = $get(_clientlstvwVoucherDetails + _ctrl + i + _hidLedgerId).value;
		for (var j = 0; j < ledgers.length; j++) {
			var ledger = ledgers[j];
			if(groupId == ledger.getAttribute('groupid') &&
				ledgerId == ledger.getAttribute('ledgerid') &&
				ledgerName == ledger.innerHTML.replace(/\&amp\;/g,'&')) {
				bExists = true;
				break;
			}
		}
		if (!bExists)
			invalidLedgers.push(ledgerName);
	}
	if (invalidLedgers.length > 0) {
		args.IsValid = false;
		src.errormessage = 'Invalid particulars : ' + invalidLedgers.join(', ');
	}
}

// This function validates if atleast one debit/credit entry for cash/bank
function ValidateVoucherCashBank(src, args) {
	args.IsValid = true;
	var ddlVoucherType = $get(_clientddlVoucherTypes);
	var voucherType = ddlVoucherType.options[ddlVoucherType.selectedIndex].text; //$get(_clienthidVoucherType).value;
	if (voucherType != _Payment && voucherType != _Receipt)
		return;
	
	var ddlToBy, groupId;
	var tobyIndex = voucherType == _Payment ? 0 : 1;
	
	for(var i = 0; i < maxRows; i++) {
		ddlToBy = $get(_clientlstvwVoucherDetails + _ctrl + i + _ddlToBy);
		if(ddlToBy.selectedIndex == tobyIndex) {
			groupId = $get(_clientlstvwVoucherDetails + _ctrl + i + _hidGroupId).value;
			if(groupId == bankacgroupid || groupId == cashgroupid) {
				args.IsValid = true;
				return false;
			}
		}
		args.IsValid = false;
		src.errormessage = 'Atleast one cash or bank a/c should be ' + (voucherType == _Payment ? 'credited' : 'debited') + '.';
	}
}

// Validates the Ledgername
function ValidateLedgerName(src, args) {
	var emptyLedgers = [];
	args.IsValid = true;
	for(var i = 0; i < maxRows; i++) {
		var row = $get(_clientlstvwVoucherDetails + _ctrl + i + _trGridRow);
		if(row.style.display == _none)
			continue;
		var txtLedgerName = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtLedger);
		var txtCreditAmt =  $get(_clientlstvwVoucherDetails + _ctrl + i + _txtCreditAmount);
		var txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtDebitAmount);
		var val = txtCreditAmt.style.visibility != _hidden ? txtCreditAmt.value : txtDebitAmt.value;
			
		if(txtLedgerName.value.trim() == _empty && val != _empty)
			emptyLedgers.push(i+1);
	}
	if(emptyLedgers.length > 0) {
		args.IsValid = false;
		src.errormessage = 'Ledger name should not be empty for row(s) : ' + emptyLedgers.join(', ');
	}
}

// Validates if the Amount entered for a non-empty ledger is not empty.
function ValidateVoucherAmount(src, args) {
	var row, ledgerName, txtDebitAmt, amt;
	var invalidLedgers = [];
	args.IsValid = true;
	for(var i = 0; i < maxRows; i++) {
		row = $get(_clientlstvwVoucherDetails + _ctrl + i + _trGridRow);
		if (row.style.display == _none)
			continue;
		ledgerName = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtLedger).value.trim();
		if (ledgerName == _empty)
			continue;
		
		txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtDebitAmount);
		if(txtDebitAmt.style.visibility != _hidden)
			amt = txtDebitAmt.value;
		else
			amt = $get(_clientlstvwVoucherDetails + _ctrl + i + _txtCreditAmount).value;
			
		args.IsValid = (amt != _empty && parseFloat(amt) > 0);
		if(!args.IsValid)
			invalidLedgers.push(ledgerName);
	}
	
	if(invalidLedgers.length > 0) {
		src.errormessage = 'Amount should not be empty for particulars : ' + invalidLedgers.join(', ');
		args.IsValid = false;
	}
}

// Validates if the Debit & Credit column totals match.
function ValidateTotals(src, args) {
	var tblVoucherDetails = $get(_clientlstvwVoucherDetails + '_tblVoucherDetails');
	var count = 0;
	for(var i = 1; i < tblVoucherDetails.rows.length - 1; i++) {
		var row = tblVoucherDetails.rows[i];
		if(row.style.display != _none)
			count++;
	}
	
	// The reason for setting the IsValid flag to true is becuase if the count is less than 2, totals will never ever match, hence this validation is not required to show.
	if(count < 2)
		args.IsValid = true;
	else {
		CalculateCreditTotal();
		CalculateDebitTotal();
		var lblDebitTotal = $get(_clientlstvwVoucherDetails + '_lblDebitTotal').innerHTML.replace(/,/g, _empty);
		var lblCreditTotal = $get(_clientlstvwVoucherDetails + '_lblCreditTotal').innerHTML.replace(/,/g, _empty);
		
		args.IsValid = (parseFloat(lblDebitTotal) == parseFloat(lblCreditTotal));
	}
	
	if(!args.IsValid)
		src.errormessage = 'Total Amount for Debit & Credit columns should match.';
}

// Validates if the Comment is empty or if it exceeds 4000 characters.
function ValidateComment(src, args) {
	args.IsValid = true;
	var txtComment = $get(_clienttxtComment);
	if(txtComment) {
		if(txtComment.value == _empty) {
			src.errormessage = 'Comment should not be empty.';
			args.IsValid = false;
		}
		else if(txtComment.value.length > 4000) {
			args.IsValid = false;
			src.errormessage = 'Comment should not exceed 4000 characters.';
		}
	}
}


/* ----------------
 *	EVENT HANDLERS
 * ----------------
 */

// OnChange event handler for VoucherType. Sets the To/By value for the first row.
function VoucherTypeChange(src) {
	var ddlToBy = $get(_clientlstvwVoucherDetails + '_ctrl0_ddlToBy');
	ddlToBy.disabled = false;
	ddlToBy.selectedIndex = 0;
	ddlToBy.disabled = true;
	$get(_clientlstvwVoucherDetails + '_ctrl0_txtDebitAmount').style.visibility = _hidden;
	$get(_clientlstvwVoucherDetails + '_ctrl0_txtCreditAmount').style.visibility = _visible;
	
	for(var i = 0; i < src.options.length; i++) {
		if(src.options[i].selected) {
			$get(_clienthidVoucherType).value = src.options[i].text;
			if(src.options[i].text == _Payment) {
				ddlToBy.disabled = false;
				ddlToBy.selectedIndex = 1;
				ddlToBy.disabled = true;
				$get(_clientlstvwVoucherDetails + '_ctrl0_txtDebitAmount').style.visibility = _visible;
				$get(_clientlstvwVoucherDetails + '_ctrl0_txtCreditAmount').style.visibility = _hidden;
				break;
			}
		}
	}
	
	CalculateDebitTotal();
	CalculateCreditTotal();
}

// This is the onchange event handler for To/By dropdown list. It shows/hides controls according to the selection.
function ToByOnChange(src) {
	var rowNo = src.id.match(/_ctrl(\d+)_ddlToBy/)[1];
	var txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtDebitAmount);
	var txtCreditAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtCreditAmount);
	
	if(src.selectedIndex == 1) {
		txtDebitAmt.style.visibility = _visible;
		txtCreditAmt.style.visibility = _hidden;
		txtDebitAmt.value = txtCreditAmt.value;
		txtCreditAmt.value = _empty;
	}
	else {
		txtDebitAmt.style.visibility = _hidden;
		txtCreditAmt.style.visibility = _visible;
		txtCreditAmt.value = txtDebitAmt.value;
		txtDebitAmt.value = _empty;
	}
	
	CalculateDebitTotal();
	CalculateCreditTotal();
}


/* ----------------
 *	CORE FUNCTIONS
 * ----------------
 */

// Called immed on load.
function OnLoad() {
	// Adds a click event listener on the document, to close the ledgers popup when the user clicks outside it.
	$(document).click(function(e) {
						e = e || event;
						var src = e.target;
						
						// If the element clicked was a Ledger textbox, we do not hide the popup.
						if (src.id.match(/txtLedger/))
							return;
						
						// If the element clicked was the popup itself or one of its children, we do not hide it.
						if ($(src).closest('#ledgerPopup').length > 0) {
							$('.current', $('#ledgerPopup')).attr('class', function() { this.className = this.className.replace(_current, _empty); });
							src.className += __current;
							SelectLedger($get(currentLedgerTextbox), e);
						}
		
						$('#ledgerPopup').hide();
					});

	// Set the event handler for Voucher Action History.
	if ($('.collapsiblePanel').length > 0) {
		$('.collapsiblePanel .panelTitle')
			.click(function() {
				$('.panelContent', this.parentNode).slideToggle('normal');
				$(this.parentNode).toggleClass('collapse').toggleClass('expand');
			});
	}
}

// Enable disable controls on the page
function ToggleControls(state) {
	var btnSave = $get(_clientbtnSave);
	if(btnSave)
		btnSave.disabled = state;
	
	var btnSelfApprove = $get(_clientbtnSelfApprove);
	if(btnSelfApprove)
		btnSelfApprove.disabled = state;
	
	var btnClose = $get(_clientbtnClose);
	if(btnClose)
		btnClose.disabled = state;
}

// Displays the row after the row which triggered this function.
function AddRow(rowIndex) {
	if(rowIndex + 1 != maxRows) {
		var row = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _trGridRow);
		var cur_btn = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _rowButton);
		var nxt_btn = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _rowButton);
		
		// Modify attributes for current button.
		if(rowIndex != 0) {
			cur_btn.className = _removeButton;
			cur_btn.onclick = Function('ShiftRows('+rowIndex+')');
		}
		
		var amtDiff = GetDifference();
		
		if(amtDiff > 0) {
			var ddlToBy = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _ddlToBy);
			ddlToBy.selectedIndex = 1;
			ToByOnChange(ddlToBy);
		}
		
		row.style.display = _empty;
		
		// If the current row is the second last row.
		if(rowIndex + 1 == maxRows - 1) {
			nxt_btn.className = _removeButton;
			nxt_btn.onclick = Function('ShiftRows('+(rowIndex + 1)+')');
		}
		else
			nxt_btn.className = _addButton;
		
		// We need this since
		window.setTimeout(function(){$get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _ddlToBy).focus();},50);

		var txtCreditAmount = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _txtCreditAmount);
		if(txtCreditAmount && txtCreditAmount.style.visibility != _hidden)
			AutoFillAmount(txtCreditAmount);
		else {
			var txtDebitAmount = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _txtDebitAmount);
			if(txtDebitAmount && txtDebitAmount.style.visibility != _hidden)
				AutoFillAmount(txtDebitAmount);
		}

		CalculateCreditTotal();
		CalculateDebitTotal();
	}
}

// This function shifts up all the rows (along with all their values) below the row which triggered this function.
function ShiftRows(rowIndex) {
	var rowCount = 0;
	for(var i = 0; i < maxRows; i++) {
		var _row = $get(_clientlstvwVoucherDetails + _ctrl + i + _trGridRow);
		if(_row.style.display == _empty)
			rowCount++;
	}
	
	var row, prv_btn;
	var cur_ddlToBy, cur_hidGroupId, cur_hidLedgerId, cur_txtLedger, cur_txtDebitAmt, cur_txtCreditAmt, cur_btn;
	var nxt_ddlToBy, nxt_hidGroupId, nxt_hidLedgerId, nxt_txtLedger, nxt_txtDebitAmt, nxt_txtCreditAmt, nxt_btn;
	
	// If it's the last row in the list
	if(rowIndex + 1 == maxRows) {
		row = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _trGridRow);
		cur_btn = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _rowButton);
		cur_btn.className = _addButton;
		cur_btn.onclick = Function('AddRow('+rowIndex+')');
		row.style.display = _none;
		
		prv_btn = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex - 1) + _rowButton);
		prv_btn.className = _addButton;
		prv_btn.onclick = Function('AddRow('+(rowIndex - 1)+')');
	}
	else {
		for( ; rowIndex < rowCount; rowIndex++) {
			row = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _trGridRow);
			if(row && row.style.display == _empty) {
				cur_ddlToBy = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _ddlToBy);
				cur_hidGroupId = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _hidGroupId);
				cur_hidLedgerId = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _hidLedgerId);
				cur_txtLedger = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtLedger);
				cur_txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtDebitAmount);
				cur_txtCreditAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtCreditAmount);
				cur_btn = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _rowButton);
				
				nxt_ddlToBy = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _ddlToBy);
				nxt_hidGroupId = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _hidGroupId);
				nxt_hidLedgerId = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _hidLedgerId);
				nxt_txtLedger = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _txtLedger);
				nxt_txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _txtDebitAmount);
				nxt_txtCreditAmt = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _txtCreditAmount);
				nxt_btn = $get(_clientlstvwVoucherDetails + _ctrl + (rowIndex + 1) + _rowButton);
				
				if(rowIndex > 1)
					cur_ddlToBy.selectedIndex = nxt_ddlToBy.selectedIndex;
				
				cur_hidGroupId.value = nxt_hidGroupId.value;
				cur_hidLedgerId.value = nxt_hidLedgerId.value;
				
				cur_txtLedger.value = nxt_txtLedger.value;
				cur_txtDebitAmt.value = nxt_txtDebitAmt.value;
				cur_txtCreditAmt.value = nxt_txtCreditAmt.value;
				if(rowIndex > 1) {
					cur_txtDebitAmt.style.visibility = nxt_txtDebitAmt.style.visibility;
					cur_txtCreditAmt.style.visibility = nxt_txtCreditAmt.style.visibility;
					
					nxt_ddlToBy.selectedIndex = 0;
					nxt_txtDebitAmt.style.visibility = _hidden;
					nxt_txtCreditAmt.style.visibility = _visible;
				}
				
				if (cur_btn) {
					cur_btn.className = nxt_btn.className;
					if(nxt_btn.className == _addButton)
						cur_btn.onclick = Function('AddRow('+rowIndex+')');
					else
						cur_btn.onclick = Function('ShiftRows('+rowIndex+')');
				}
				
				// When the button clicked is second last button
				// Check for cur_btn not being null is added because now we show 2 rows
				// When the for loop is on the first row, we do not want to hide the next row, i.e. second row.
				if(cur_btn && rowIndex + 1 == rowCount - 1) {
					nxt_btn.className = _addButton;
					nxt_btn.onclick = Function('AddRow('+(rowIndex + 1)+')');
					row.style.display = _none;
				}
			}
			else {
				cur_hidGroupId = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _hidGroupId);
				cur_hidLedgerId = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _hidLedgerId);
				cur_txtLedger = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtLedger);
				cur_txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtDebitAmount);
				cur_txtCreditAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _txtCreditAmount);
				
				cur_hidGroupId.value = _empty;
				cur_hidLedgerId.value = _empty;
				cur_txtLedger.value = _empty;
				cur_txtDebitAmt.value = _empty;
				cur_txtCreditAmt.value = _empty;
			}
		}
		
		if(rowCount == maxRows) {
			row = $get(_clientlstvwVoucherDetails + _ctrl + (rowCount - 1) + _trGridRow);
			row.style.display = _none;
			nxt_btn = $get(_clientlstvwVoucherDetails + _ctrl + (rowCount - 1) + _rowButton);
			nxt_btn.className = _addButton;
			nxt_btn.onclick = Function('AddRow('+(rowCount - 1)+')');
			nxt_btn = $get(_clientlstvwVoucherDetails + _ctrl + (rowCount - 2) + _rowButton);
			nxt_btn.className = _addButton;
			nxt_btn.onclick = Function('AddRow('+(rowCount - 2)+')');
		}
	}
	
	CalculateCreditTotal();
	CalculateDebitTotal();
}

// This function is used to show the next row when the user presses tab or enter (in amount textbox)
function ShowNextRow(src) {
	if(src.value != _empty) {
		var rowNo = parseInt(src.id.match(/_ctrl(\d+)_txt/)[1]);
		if(rowNo + 1 != maxRows) {
			var row = $get(_clientlstvwVoucherDetails + _ctrl + (rowNo + 1) + _trGridRow);
				AddRow(rowNo);
		}	
	}
}

// This function closes the ledgers popup after a timeout of 200 milliseconds.
function HidePopup() {
	hideTimeout = window.setTimeout(function() {
										$('#ledgerPopup').hide();
									}, 200);
}

// This function is used to prevent the ledgers popup from closing.
function CancelHide() {
	window.clearTimeout(hideTimeout);
}

// This function is used to show the ledgers popup.
function ShowPopup(src) {
	window.clearTimeout(hideTimeout);
	$('#ledgerPopup').show();
	
	currentLedgerTextbox = src.id;
	var rowNo = currentLedgerTextbox.match(/_ctrl(\d+)_txtLedger/)[1];
	SetPopupRootClassName(rowNo);
	
	// Clear all the selections in the Popup
	$('.match', $('#ledgerPopup')).attr('class', function() { this.className = this.className.replace(_match, _empty).replace(_current, _empty); });
	
	FilterLedger(src);
}

// Sets the className of contentPlaceholder according to the Voucher Type and To/By selection.
function SetPopupRootClassName(rowIndex) {
	var ddlToBy = $get(_clientlstvwVoucherDetails + _ctrl + rowIndex + _ddlToBy);
	var voucherType = $get(_clienthidVoucherType).value;
	var contentPlaceholder = $('#contentPlaceholder').get(0);
	if(voucherType == _Payment) {
		contentPlaceholder.className = ddlToBy.selectedIndex == 1 ? 'cashbank' : 'all';
	}
	else if(voucherType == _Receipt) {
		contentPlaceholder.className = ddlToBy.selectedIndex == 1 ? 'all' : 'cashbank';
	}
	else if(voucherType == 'Journal') {
		contentPlaceholder.className = 'cashbank';
	}
	else if(voucherType == 'Contra') {
		contentPlaceholder.className = 'onlycashbank';
	}
}

// This function is used to highlight the Ledgers in the Popup which match the passed val
function FilterLedger(src, event) {
	// If the keys pressed were either the up or down arrow, exit function.
	if(event && (event.keyCode == 38 || event.keyCode == 40))
		return;
	
	var popup = $('#ledgerPopup').get(0);
	var val = src.value.toLowerCase().replace(/\&/g,'&amp;');
	var ledgers = $('div:visible', popup);
	var bSet = false;
	for(var i = 1; i < ledgers.length; i++) {
		var ledger = ledgers[i];
		ledger.className = ledger.className.replace(_match, _empty).replace(_current, _empty);
		if(ledger.innerHTML.toLowerCase().match(new RegExp('^'+escapeRegExp(val)))) {
			if(!bSet) {
				ledger.className += ' match current';
				bSet = true;
				if(ledger.offsetTop + ledger.offsetHeight <= popup.scrollTop // When current ledger is above the viewable portion of the popup
					|| ledger.offsetTop + ledger.offsetHeight > popup.scrollTop + popup.offsetHeight) // When the current ledger is below the viewable portion of the popup
					popup.scrollTop = ledger.offsetTop;
			}
			else
				ledger.className = ledger.className.replace(_current, _empty) + ' match';
		}
	}
}

// This function is used to Select a ledger from the Popup, when the user presses ENTER(13) or TAB(9)  key.
function SelectLedger(src, event) {
	var rowNo = src.id.match(/_ctrl(\d+)_txtLedger/)[1];
	var ledgerPopup = $('#ledgerPopup').get(0);
	var matches;
	if(event.type == "blur" || event.type == "click" || event.keyCode == 13 || event.keyCode == 9) {
		var curr = $('.current', ledgerPopup);
		if(curr.length > 0) {
			src.value = curr[0].innerHTML.replace(/\&amp\;/g,'&');
			var hidGroupId = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _hidGroupId);
			var hidLedgerId = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _hidLedgerId);
			hidGroupId.value = curr[0].getAttribute('groupid');
			hidLedgerId.value = curr[0].getAttribute('ledgerid');
			
			// This obj will be passed to the AutoFillAmount method
			var obj;
			var txtDebitAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtDebitAmount);
			var txtCreditAmt = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtCreditAmount);
				
			// Focus on the amount field if the user pressed enter.
			if(event.type == "click" || event.keyCode == 13) {
				if(txtDebitAmt && txtDebitAmt.style.visibility == _visible)
					txtDebitAmt.focus();
				else
					txtCreditAmt.focus();
			}
			else {
				if(txtDebitAmt.style.visibility == _visible)
					obj = txtDebitAmt;
				else
					obj = txtCreditAmt;
			}

			AutoFillAmount(obj);
		}
		// If there are no matches in the popup and the key pressed was a tab key, cancel the tab event.
		else if(!event.shiftKey && event.keyCode == 9) {
			if(event.preventDefault)
				event.preventDefault();
			event.returnValue = false;
			return false;
		}
	}
	else if(event.keyCode == 38) { // UP ARROW
		matches = $('.match:visible', ledgerPopup);
		if(matches.length > 1) {
			for(var i = 0; i < matches.length; i++) {
				if(matches[i].className.indexOf(_current) > 0) {
					if(i == 0) {
						matches[matches.length-1].className += __current;
						ledgerPopup.scrollTop = matches[matches.length-1].offsetTop + matches[matches.length-1].offsetHeight - ledgerPopup.offsetHeight;
					}
					else {
						matches[i-1].className += __current;
						if(matches[i-1].offsetTop < ledgerPopup.scrollTop)
							ledgerPopup.scrollTop = matches[i-1].offsetTop;
					}
					
					matches[i].className = matches[i].className.replace(_current, _empty) + _match;
					return;
				}	
			}
		}
	}
	else if(event.keyCode == 40) { // DOWN ARROW
		matches = $('.match:visible', ledgerPopup);
		if(matches.length > 1) {
			for(var i = 0; i < matches.length; i++) {
				if(matches[i].className.indexOf(_current) > 0) {
					if(i == matches.length-1) {
						matches[0].className += __current;
						ledgerPopup.scrollTop = matches[0].offsetTop;
					}
					else {
						matches[i+1].className += __current;
						if(matches[i+1].offsetTop + matches[i+1].offsetHeight > ledgerPopup.scrollTop + ledgerPopup.offsetHeight)
							ledgerPopup.scrollTop = matches[i+1].offsetTop + matches[i+1].offsetHeight - ledgerPopup.offsetHeight;
					}
					
					matches[i].className = matches[i].className.replace(_current, _empty) + _match;
					return;
				}	
			}
		}
	}
}

// Verifies if the textbox for Ledger is not empty. If it is, its respective hidden fields are cleared.
function VerifyLedger(src) {
	var rowNo = src.id.match(/_ctrl(\d+)_txt/)[1];
	var txtLedger = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtLedger);
	if(txtLedger.value.trim() != _empty)
		return true;
	else {
		var hidGroupId = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _hidGroupId);
		var hidLedgerId = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _hidLedgerId);
		hidGroupId.value = _empty;
		hidLedgerId.value = _empty;
		return false;
	}
}

// Verifies if the textbox for both Ledger and its amount are not empty.
function VerifyAmount(src) {
	var rowNo = src.id.match(/_ctrl(\d+)_txt/)[1];
	var txtLedger = $get(_clientlstvwVoucherDetails + _ctrl + rowNo + _txtLedger);
	if(txtLedger && txtLedger.value.trim() != _empty && parseFloat(src.value) > 0)
		return true;
	else {
		src.value = _empty;
		CalculateCreditTotal();
		CalculateDebitTotal();
		return false;
	}
}

// Calculates the total of the Debit column.
function CalculateDebitTotal(src) {
	if(src && src.value != _empty)
		src.value = parseFloat(src.value).toFixed(2);
	
	var tblVoucherDetails = $get(_clientlstvwVoucherDetails + '_tblVoucherDetails');
	var amtTotal = 0.00;
	for(var i = 1; i < tblVoucherDetails.rows.length - 1; i++) {
		if (tblVoucherDetails.rows[i].style.display == _none)
			continue;
		
		var amt = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtDebitAmount);
		if (amt.style.visibility != _hidden && amt.value != _empty)
			amtTotal += parseFloat(amt.value);
	}
	
	var lblDebitTotal = $get(_clientlstvwVoucherDetails + '_lblDebitTotal');
	lblDebitTotal.innerHTML = FormatCurrency(amtTotal.toFixed(2));
	$get(_clienthidTotalAmount).value = lblDebitTotal.innerHTML;
}

// Calculates the total of the Credit column.
function CalculateCreditTotal(src) {
	if(src && src.value != _empty)
		src.value = parseFloat(src.value).toFixed(2);
	
	var tblVoucherDetails = $get(_clientlstvwVoucherDetails + '_tblVoucherDetails');
	var amtTotal = 0.00;
	for(var i = 1; i < tblVoucherDetails.rows.length - 1; i++) {
		if (tblVoucherDetails.rows[i].style.display == _none)
			continue;
		
		var amt = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtCreditAmount);
		if (amt.style.visibility != _hidden && amt.value != _empty)
			amtTotal += parseFloat(amt.value);
	}
	
	var lblCreditTotal = $get(_clientlstvwVoucherDetails + '_lblCreditTotal');
	lblCreditTotal.innerHTML = FormatCurrency(amtTotal.toFixed(2));
	$get(_clienthidTotalAmount).value = lblCreditTotal.innerHTML;
}

// Fills in the amount for a newly added row.
function AutoFillAmount(obj) {
	if(!obj || obj.value.trim() != _empty) return;
	
	var tblVoucherDetails = $get(_clientlstvwVoucherDetails + '_tblVoucherDetails');
	var amtTotal = 0.00;
	for(var i = 1; i < tblVoucherDetails.rows.length - 1; i++) {
		if(tblVoucherDetails.rows[i].style.display == _none)
			continue;
		
		var txtCreditAmount = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtCreditAmount);
		var txtDebitAmount = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtDebitAmount);

		// Skip if the current row is the same as the row that triggered this function.
		if(obj.id == txtCreditAmount.id || obj.id == txtDebitAmount.id)
			continue;
		
		if(txtCreditAmount.style.visibility != _hidden && txtCreditAmount.value.trim() != _empty)
			amtTotal += parseFloat(txtCreditAmount.value);
		else if(txtDebitAmount.style.visibility != _hidden && txtDebitAmount.value.trim() != _empty)
			amtTotal -= parseFloat(txtDebitAmount.value);
	}

	if(amtTotal != 0)
		obj.value = Math.abs(amtTotal).toFixed(2);
}

function GetDifference() {
	var tblVoucherDetails = $get(_clientlstvwVoucherDetails + '_tblVoucherDetails');
	var amtTotal = 0.00;
	for(var i = 1; i < tblVoucherDetails.rows.length - 1; i++) {
		var row = tblVoucherDetails.rows[i];
		if(row.style.display != _none) {
			var txtCreditAmount = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtCreditAmount);
			var txtDebitAmount = $get(_clientlstvwVoucherDetails + _ctrl + (i - 1) + _txtDebitAmount);

			if(txtCreditAmount.style.visibility != _hidden && txtCreditAmount.value.trim() != _empty)
				amtTotal += parseFloat(txtCreditAmount.value);
			else if(txtDebitAmount.style.visibility != _hidden && txtDebitAmount.value.trim() != _empty)
				amtTotal -= parseFloat(txtDebitAmount.value);
		}
	}
	return amtTotal;
}

function WarnOnReject() {
	var currDesigId = $get(_clienthidCurrentDesigId).value;
	var nextDesigId = $get(_clienthidNextApproverDesigId).value;
	if(currDesigId != nextDesigId) {
		var nextDesigName = $get(_clienthidNextApproverDesigName).value;
		return confirm('This voucher is yet to be reviewed by a ' + nextDesigName + '\nAre you sure you want to reject it anyway?');
	}
	else
		return confirm("Are you sure you want to reject this voucher?");
}

function WarnOnApprove() {
	var currDesigId = $get(_clienthidCurrentDesigId).value;
	var nextDesigId = $get(_clienthidNextApproverDesigId).value;
	if(currDesigId != nextDesigId) {
		var nextDesigName = $get(_clienthidNextApproverDesigName).value;
		return confirm('This voucher is yet to be reviewed by a ' + nextDesigName + '\nAre you sure you want to approve it anyway?');
	}
	else
		return confirm("Are you sure you want to approve this voucher?");
}

function ClearMessages() {
	var valSummarySave = $get(_clientvalSummarySave);
	if (valSummarySave)
		valSummarySave.style.display = _none;

	var lblErrorMessage = $get(_clientlblErrorMessage);
	if(lblErrorMessage)
		lblErrorMessage.innerHTML = _empty;
	
	var lblUpdateMessage = $get(_clientlblUpdateMessage);
	if(lblUpdateMessage)
		lblUpdateMessage.innerHTML = _empty;
}

function ResetControls(bConfirm) {
	if (bConfirm && !confirm("This will reset all existing entries on the screen.\nAre you sure you want to continue?"))
		return false;

	var i = maxRows - 1;
	while(i >= 0)
		ShiftRows(i--);

	ClearMessages();

	var txtNarration = $get(_clienttxtNarration);
	txtNarration.value = _empty;

	var txtVoucherDate = $get(_clienttxtVoucherDate);
	txtVoucherDate.value = new Date().format("dd-MMM-yyyy");
}

function GeneratePrint() {
    _sClienthidQery = "<%=this.hidQery.ClientID %>";
    window.open("../Accounts/VoucherPrint.aspx?" + document.getElementById(_sClienthidQery).value, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=1000,height=600');
}

function ShowPrintDialog(bankId, amount) {
	var config = _ChqConfigs[bankId];
	$('#ddlTemplateList').each(function() {
		this.options.length = 0;
		var i = 0;
		for(j in config)
			this.options[i++] = new Option(config[j].Name, config[j].Id);
	});
	$('#<%= hidChqAmount.ClientID %>').val(amount);
	$('#printDialogWrapper').show();
}

function ValidatePrintChequeInput() {
	var bIsValid = true;
	
	if ($('#<%= txtPayeeName.ClientID %>').val() == '') {
		bIsValid = false;
		alert('Payee Name should not be blank.');
	}
	else {
		var txtChqDate = $('#<%= txtChequeDate.ClientID %>').val();
	
		if (txtChqDate == '')
		{
			bIsValid = false;
			alert('Cheque date should not be blank.');
		}
		else
		{
			var dtChqDate = new Date(txtChqDate.replace(/-/g, ' '));
			if (!IsValidDate(dtChqDate))
			{
				bIsValid = false;
				alert('Please select a valid Cheque Date.');
			}
		}
	}
	
	$('#<%= hidChqConfigId.ClientID %>').val($('#ddlTemplateList').val());

	return bIsValid;
}

function HidePrintDialog() {
	$('#printDialogWrapper').hide();
}
</script>
</asp:Content>