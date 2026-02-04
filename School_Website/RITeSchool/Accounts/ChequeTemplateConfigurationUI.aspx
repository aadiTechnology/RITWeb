<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="ChequeTemplateConfigurationUI.aspx.cs" Inherits="ChequeTemplateConfigurationUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" Runat="Server">
<style type="text/css">
ul { margin: 0; padding: 0; }
#inputContainer {}
#inputContainer ul { display: block; }
#inputContainer li { display: inline; }
#inputContainer .txtBox, #inputContainer .txtBoxN { width: 30px; border: 1px solid gray; background-color: White; font-size: 9pt; padding: 1px; }
#inputContainer .txtBoxN { width: 200px; }
#inputContainer .label { width: 30px; background-color: transparent !important}
tr input[type="checkbox"] { vertical-align: middle; }
.off .ClsMdtStar { visibility: hidden; }
.on .ClsMdtStar { visibility: visible; }
.off { color: Gray; }
#canvas { border: 1px solid #AAA; padding: 3px; width: 690px; height: 230px; position: relative; }
#canvas span { font-size: 10pt; position: absolute; top: 3px; left: 3px; border: 1px solid gray; text-align: justify; overflow: hidden; }
#company, #signatory1, #signatory2 { text-align: center !important; }
#amount { font-weight: bold; }
label[for] { margin-bottom: 0px;}
input[type="checkbox"] { margin: 0px 4px 0 !important;}

</style>
<table width="100%">
	<tr>
		<td align="center">
			<table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; margin: 5px 0; float: none;">
				<tr>
					<td align="left" style="height: 20px">
						<span class="MainTitleHead" style="font-weight: bold;">Cheque Template Configuration</span>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:UpdatePanel ID="mainUpdatePanel"
							 runat="server">
				<ContentTemplate>
					<div>
						<div id="mdtStar" style="color: red; text-align: right; display: none;">* Mandatory Fields</div>
						<table width="700px" cellpadding="0" cellspacing="0" border="0">
							<tr>
								<td align="left">
									<asp:ValidationSummary ID="valSummary"
														   runat="server"
														   CssClass="ClsLabel"
														   ShowSummary="true" />
								</td>
							</tr>
							<tr>
								<td align="center">
									<asp:Label ID="lblMessage"
											   runat="server"
											   EnableViewState="false"
											   style="margin: 5px 0; display: block;" />
								</td>
							</tr>
							<tr>
								<td align="center">
									<table cellpadding="0" cellspacing="2" border="0" id="tblBankList">
										<tr>
											<td align="left" class="ClsBorderlight" style="width: 100px; padding: 2px;">Bank :</td>
											<td align="left" style="width: 210px;">
												<asp:DropDownList ID="ddlBankList"
																  runat="server"
																  CssClass="LrgCombo"
																  AutoPostBack="true"
																  OnSelectedIndexChanged="ddlBankList_SelectedIndexChanged"
																  onchange="if(_cancelPostBack){return false;}" />
											</td>
										</tr>
									</table>
								</td>
							</tr>
						</table>
					</div>
					<div id="configList" runat="server" clientidmode="Static">
						<table>
							<tr>
								<td align="center">
									<asp:ListView ID="lstvwChqConfigurations"
												  runat="server"
												  DataKeyNames="Id, Name"
												  OnItemCommand="lstvwChqConfigurations_ItemCommand"
												  OnItemDataBound="lstvwChqConfigurations_ItemDataBound">
										<LayoutTemplate>
											<table cellspacing="1" cellpadding="3" class="GridBorder">
												<tr id="trHeader" runat="server" class="ClsGridHeader">
													<th align="center" style="width: 50px; font-size: 9pt;">Sr.No.</th>
													<th align="left" style="width: 200px; font-size: 9pt;">Template Name</th>
													<th align="center" style="width: 100px; font-size: 9pt;">Action</th>
												</tr>
												<tr id="itemPlaceholder" runat="server"></tr>
											</table>
										</LayoutTemplate>
										<ItemTemplate>
											<tr id="trGridRow" runat="server" class='<%# Container.DisplayIndex %2 == 0 ? "ClsGridRow" : "ClsGridAltRow" %>'>
												<td align="center"><%# Container.DisplayIndex + 1 %></td>
												<td align="left"><%# Eval("Name") %></td>
												<td align="center">
													<asp:ImageButton ID="imgbtnEdit"
																	 runat="server"
																	 ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
																	 AlternateText="Edit"
																	 ToolTip="Edit Configuration"
																	 CausesValidation="false"
																	 style="vertical-align: middle; cursor: pointer;" />
													<asp:ImageButton ID="imgbtnCopy"
																	 runat="server"
																	 ImageUrl="~/RITeSchool/images/Icon_BookAdd.gif"
																	 AlternateText="Copy"
																	 ToolTip="Copy Configuration"
																	 CausesValidation="false"
																	 CommandName="DELETE_ROW2"
																	 style="vertical-align: middle; margin-left: 3px; cursor: pointer;" />
													<asp:ImageButton ID="imgbtnDelete"
																	 runat="server"
																	 AlternateText="Delete"
																	 ToolTip="Delete Configuration"
																	 CausesValidation="false"
																	 CommandName="DELETE_ROW"
																	 CommandArgument='<%# Eval("Id") %>'
																	 OnClientClick="if(!WarnDelete()){return false}"
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
					</div>
					<asp:HiddenField ID="hidConfigJSON" runat="server" />
					<asp:HiddenField ID="hidConfigId" runat="server" />
				</ContentTemplate>
			</asp:UpdatePanel>
			<div id="layoutEditor">
				<div id="inputContainer" runat="server" clientidmode="Static" style="width: 750px;">
					<table cellpadding="0" cellspacing="2" border="0">
						<tr>
							<td align="left" class="ClsBorderlight" style="width: 100px; padding: 2px;">Template Name :</td>
							<td align="left" style="width: 210px;">
								<asp:TextBox ID="txtTemplateName"
												runat="server"
												ClientIDMode="Static"
												CssClass="MidTxtBox"
												MaxLength="100"
												style="width: 190px;" />
								<span class="ClsMdtStar">*</span>
							</td>
						</tr>
					</table>
					<table cellspacing="1" cellpadding="3" class="GridBorder">
						<tr class="ClsGridHeader">
							<th align="left" style="font-size: 9pt;">Element</th>
							<th align="left" style="font-size: 9pt;">Dimensions</th>
						</tr>
						<tr class="ClsGridAltRow on">
							<td align="left">
								<asp:CheckBox ID="chkDate"
											  runat="server"
											  clientidmode="Static"
											  Text="Date"
											  Checked="true"
											  item="date" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtDateTop"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBox"
													 default="20"
													 item="date"
													 prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtDateLeft"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBox"
													 default="600"
													 item="date"
													 prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridRow on">
							<td align="left">
								<asp:CheckBox ID="chkPayee"
											  runat="server"
											  clientidmode="Static"
											  Text="Payee"
											  Checked="true"
											  item="payee" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtPayeeTop"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="50"
														item="payee"
														prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtPayeeLeft"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="50"
														item="payee"
														prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Width :</span>
										<asp:TextBox ID="txtPayeeWidth"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="500"
														item="payee"
														prop="width" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridAltRow on">
							<td align="left">
								<asp:CheckBox ID="chkAmount"
												runat="server"
											  clientidmode="Static"
												Text="Amount"
												Checked="true"
												item="amount" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtAmountTop"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="70"
														item="amount"
														prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtAmountLeft"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="575"
														item="amount"
														prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridRow on">
							<td align="left">
								<asp:CheckBox ID="chkAmountInWords"
												runat="server"
											  clientidmode="Static"
												Text="Amount in Words"
												Checked="true"
												item="amountinwords" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtAmountInWordsTop"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="67"
														item="amountinwords"
														prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtAmountInWordsLeft"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="10"
														item="amountinwords"
														prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Width :</span>
										<asp:TextBox ID="txtAmountInWordsWidth"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="540"
														item="amountinwords"
														prop="width" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:44px;">Height :</span>
										<asp:TextBox ID="txtAmountInWordsHeight"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="46"
														item="amountinwords"
														prop="height" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:44px;">Indent :</span>
										<asp:TextBox ID="txtAmountInWordsIndent"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="40"
														item="amountinwords"
														prop="text-indent" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:80px;">Line-Spacing :</span>
										<asp:TextBox ID="txtAmountInWordsLineHeight"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="24"
														item="amountinwords"
														prop="line-height" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridAltRow off">
							<td align="left">
								<asp:CheckBox ID="chkCompanyName"
												runat="server"
											  clientidmode="Static"
												Text="Company"
												item="company" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtCompanyTop"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="130"
														item="company"
														prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtCompanyLeft"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="500"
														item="company"
														prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Width :</span>
										<asp:TextBox ID="txtCompanyWidth"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="175"
														item="company"
														prop="width" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Name :</span>
										<asp:TextBox ID="txtCompanyName"
														runat="server"
														clientidmode="Static"
														CssClass="txtBoxN"
														item="company"
													 MaxLength="100" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridRow off">
							<td align="left">
								<asp:CheckBox ID="chkSignatory1"
												runat="server"
											  clientidmode="Static"
												Text="First Signatory"
												item="signatory1" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtSignatory1Top"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="205"
														item="signatory1"
														prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtSignatory1Left"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="500"
														item="signatory1"
														prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Width :</span>
										<asp:TextBox ID="txtSignatory1Width"
														runat="server"
														clientidmode="Static"
														CssClass="txtBox"
														default="175"
														item="signatory1"
														prop="width" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Name :</span>
										<asp:TextBox ID="txtSignatory1Name"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBoxN"
													 item="signatory1"
													 MaxLength="100" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
						<tr class="ClsGridAltRow off">
							<td align="left">
								<asp:CheckBox ID="chkSignatory2"
											  runat="server"
											  clientidmode="Static"
											  Text="Second Signatory"
											  item="signatory2" />
							</td>
							<td align="left">
								<ul>
									<li>
										<span class="label">Top :</span>
										<asp:TextBox ID="txtSignatory2Top"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBox"
													 default="205"
													 item="signatory2"
													 prop="top" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label">Left :</span>
										<asp:TextBox ID="txtSignatory2Left"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBox"
													 default="310"
													 item="signatory2"
													 prop="left" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Width :</span>
										<asp:TextBox ID="txtSignatory2Width"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBox"
													 default="175"
													 item="signatory2"
													 prop="width" />
										<span class="ClsMdtStar">*</span>
									</li>
									<li>
										<span class="label" style="width:41px;">Name :</span>
										<asp:TextBox ID="txtSignatory2Name"
													 runat="server"
													 clientidmode="Static"
													 CssClass="txtBoxN"
													 item="signatory2"
													 MaxLength="100" />
										<span class="ClsMdtStar">*</span>
									</li>
								</ul>
							</td>
						</tr>
					</table>
				</div>
				<div style="margin: 5px 0;">
					<asp:Button ID="btnAdd"
								runat="server"
								CssClass="ClsBtn"
								Text="Add"
								CausesValidation="false"
								UseSubmitBehavior="false"
								OnClientClick="NewLayout(); return false;" />
					<asp:Button ID="btnSave"
								runat="server"
								CssClass="ClsBtn"
								Text="Save"
								style="margin-left: 3px;"
								disable-page="true"
								OnClick="btnSave_Click" />
					<asp:Button ID="btnCancel"
								runat="server"
								CssClass="ClsBtn"
								Text="Cancel"
								CausesValidation="false"
								UseSubmitBehavior="false"
								style="margin-left: 3px;"
								OnClientClick="Cancel(); return false;" />
					<asp:Button ID="btnClose"
								runat="server"
								CssClass="ClsBtn"
								Text="Close"
								CausesValidation="false"
								UseSubmitBehavior="false"
								OnClientClick="window.close(); return false;"
								style="margin-left: 3px;" />
				</div>
				<div id="canvas" runat="server" clientidmode="Static">
					<span id="date"><%= DateTime.Now.ToString("dd-MMM-yyyy") %></span>
					<span id="payee">Payee Name</span>
					<span id="amount">** 12,34,56,789.12 **</span>
					<span id="amountinwords">** Twelve Crore Thrity Four Lakh Fifty Six Thousand Seven Hundred Eighty Nine and Twelve paise Only **</span>
					<span id="company">Company Name</span>
					<span id="signatory1">First Signatory</span>
					<span id="signatory2">Second Signatory</span>
				</div>
				
				<asp:CustomValidator ID="cstValidateTemplateName"
									 runat="server"
									 Display="None"
									 EnableClientScript="true"
									 ClientValidationFunction="ValidateTemplateName" />
				<asp:CustomValidator ID="cstValidateSelection"
									 runat="server"
									 Display="None"
									 EnableClientScript="true"
									 ClientValidationFunction="ValidateSelection" />
			</div>
		</td>
	</tr>
</table>
<script type="text/javascript">
	// Local variables for page controls
	var _valSum		= '#<%= valSummary.ClientID %>';
	var _message	= '#<%= lblMessage.ClientID %>';
	var _bankList	= '#<%= ddlBankList.ClientID %>';
	var _configJSON = '#<%= hidConfigJSON.ClientID %>';
	var _configId	= '#<%= hidConfigId.ClientID %>';
	var _btnAdd		= '#<%= btnAdd.ClientID %>';
	var _btnSave	= '#<%= btnSave.ClientID %>';
	var _btnClose	= '#<%= btnClose.ClientID %>';
	var _btnCancel	= '#<%= btnCancel.ClientID %>';
	
	var _cancelPostBack = false;
	
	// A JSON object to map default values;
	var _defaultMap = {
		Date : {
			Show : "true",
			Top  : 20,
			Left : 600
		},
		Payee : {
			Show : "true",
			Top  : 50,
			Left : 50,
			Width: 500
		},
		Amount : {
			Show : "true",
			Top  : 70,
			Left : 575
		},
		AmountInWords : {
			Show		: "true",
			Top			: 67,
			Left		: 10,
			Width		: 540,
			Height		: 46,
			Indent		: 40,
			LineSpacing	: 24
		},
		Company : {
			Show  : "false",
			Top	  : 130,
			Left  : 500,
			Width : 175,
			Name  : 'Company Name'
		},
		Signatory1 : {
			Show  : "false",
			Top	  : 205,
			Left  : 500,
			Width : 175,
			Name  : 'First Signatory'
		},
		Signatory2 : {
			Show  : "false",
			Top	  : 205,
			Left  : 310,
			Width : 175,
			Name  : 'Second Signatory'
		}
	};
	
	// document onload function.
	$(document).ready(function () {
		if ($('#configList').filter(':visible').length > 0)
			BindLayout(_defaultMap);//Cancel();
		
		// Event handler for Bank list dropdown
		$(_bankList).each(function() { $('#bankLabel').text(this.options[this.selectedIndex].text); })
					.change(function() { $('#bankLabel').text(this.options[this.selectedIndex].text); });
		
		// Disable all text boxes for sections which are unchecked.
		$('.off input[type="text"]').attr('disabled', true);
	
		$('input[type="checkbox"]').click(CheckboxChange) // Event listsner for checkboxes.
								   .each(CheckboxChange); // Hide elements on canvas according to their checkbox selection
	
		// Event handlers for Numeric Textboxes
		$('.txtBox').keyup(function(event) {
						extractNumber(this, 0, false);
						var keyCode = window.event ? event.keyCode : event.which ? event.which : 0;
						if (!(keyCode == 38 || keyCode == 40)) {
							PositionElement(this);
						}
					}) // Extract numbers and Position element on canvas accordingly.
					.keydown(NumericUpDown) // Numeric Up/Down functionality
					.bind("keypress paste drop", null, function(event) { return blockNonNumbers(this, event, false, false); }) // Block NonNumbers.
					.attr('autocomplete', 'off'); // Disabled autocomplete
		
		// Event Handler for Text input.
		$('.txtBoxN').keyup(function() {
			var id = $(this).attr('item');
			if (this.value != '')
				$('#' + id).text(this.value);
		});
	});
	
	function GetConfig(configId) {
		var configs = eval($(_configJSON).val())[0];
		return configs[configId];
	}

	function CheckboxChange() {
		var node = this.parentNode.parentNode.parentNode;
		node.className = this.checked ? node.className.replace('off', 'on') : node.className.replace('on', 'off');
		$('input[type="text"]', node).attr('disabled', !this.checked);
		Toggle(this.parentNode, this.checked);
		SetDefaultValue(node, this.checked);
	}

	function NumericUpDown(event) {
		var keyCode;
	   
		if (window.event)
			keyCode = event.keyCode;
		else if (event.which)
			keyCode = event.which;
		
		if (!(keyCode == 38 || keyCode == 40))
			return;

		var value = parseInt(this.value, 10);
	
		if (isNaN(value))
			value = 0;

		event.preventDefault();
	
		var lowerBound = 0;
		
		if (keyCode == 38)
			value++;
		else if (keyCode == 40)
			value--;
	
		if (value < lowerBound)
			value = lowerBound;
//		else if (value > upperBound)
//			value = upperBound;
		
		this.value = value;
		PositionElement(this);
	}
	
	function NewLayout() {
		$('#mdtStar').show();
//		$('#tblBankList').hide();
		$('#configList').hide();
		$('#inputContainer').show();
		$('#canvas').show();
		$(_btnSave).show();
		$(_btnCancel).show();
		$(_btnAdd).hide();
		$(_message).text('');
		$(_bankList).attr('disabled', true);
		$('#txtTemplateName').val('');
	}
	
	function Cancel() {
		$('#mdtStar').hide();
//		$('#tblBankList').show();
		$('#configList').show();
		$('#inputContainer').hide();
		$('#canvas').hide();
		$(_btnAdd).show();
		$(_btnSave).hide();
		$(_btnCancel).hide();
		$(_configId).val('0');
		$(_message).text('');
		$(_valSum).text('');
		$(_bankList).attr('disabled', false);
		_cancelPostBack = false;
		$('#txtTemplateName').val('');
		BindLayout(_defaultMap);
	}
	
	function PositionElement(element) {
		var id = $(element).attr('item');
		var prop = $(element).attr('prop');
		if (prop && prop != '')
			$('#'+id).css(prop, element.value + 'px');
		if (element.value != '')
			$(element).attr('default', element.value);
	}
	
	function Toggle(element, show) {
		var id = $(element).attr('item');
		var el = $('#'+id);
		if (show)
			el.show();
		else
			el.hide();
	}
	
	function SetDefaultValue(element, show) {
		$('input[type="text"]', element).each(function() {
			if (show) {
				this.value = $(this).attr('default');
				PositionElement(this);
			}
			else
				this.value = '';
		});
	}
	
	function EditConfig(configId, configName) {
		CopyConfig(configId);
		$('#txtTemplateName').val(configName);
		$(_configId).val(configId);
		$(_bankList).attr('disabled', true);
	}
	
	function CopyConfig(configId) {
		NewLayout();
		var config = GetConfig(configId);
		BindLayout(config);
		$(_bankList).attr('disabled', false);
		_cancelPostBack = true;
	}
	
	function BindLayout(config) {
		if (!config)
			return;

		var bShow = config.Date.Show == "true";
		$('#chkDate').attr('checked', bShow);
		SetValue('#txtDateTop' , bShow, config.Date.Top , _defaultMap.Date.Top );
		SetValue('#txtDateLeft', bShow, config.Date.Left, _defaultMap.Date.Left);
		
		bShow = config.Payee.Show == "true";
		$('#chkPayee').attr('checked', bShow);
		SetValue('#txtPayeeTop'  , bShow, config.Payee.Top	, _defaultMap.Payee.Top);
		SetValue('#txtPayeeLeft' , bShow, config.Payee.Left , _defaultMap.Payee.Left);
		SetValue('#txtPayeeWidth', bShow, config.Payee.Width, _defaultMap.Width);
		
		bShow = config.Amount.Show == "true";
		$('#chkAmount').attr('checked', bShow);
		SetValue('#txtAmountTop' , bShow, config.Amount.Top , _defaultMap.Amount.Top);
		SetValue('#txtAmountLeft', bShow, config.Amount.Left, _defaultMap.Amount.Left);
		
		bShow = config.AmountInWords.Show == "true";
		$('#chkAmountInWords').attr('checked', bShow);
		SetValue('#txtAmountInWordsTop'		  , bShow, config.AmountInWords.Top		   , _defaultMap.AmountInWords.Top);
		SetValue('#txtAmountInWordsLeft'	  , bShow, config.AmountInWords.Left	   , _defaultMap.AmountInWords.Left);
		SetValue('#txtAmountInWordsWidth'	  , bShow, config.AmountInWords.Width	   , _defaultMap.AmountInWords.Width);
		SetValue('#txtAmountInWordsHeight'	  , bShow, config.AmountInWords.Height	   , _defaultMap.AmountInWords.Height);
		SetValue('#txtAmountInWordsIndent'	  , bShow, config.AmountInWords.Indent	   , _defaultMap.AmountInWords.Indent);
		SetValue('#txtAmountInWordsLineHeight', bShow, config.AmountInWords.LineSpacing, _defaultMap.AmountInWords.LineSpacing);
		
		bShow = config.Company.Show == "true";
		$('#chkCompanyName').attr('checked', bShow);
		SetValue('#txtCompanyTop'  , bShow, config.Company.Top  , _defaultMap.Company.Top);
		SetValue('#txtCompanyLeft' , bShow, config.Company.Left , _defaultMap.Company.Left);
		SetValue('#txtCompanyWidth', bShow, config.Company.Width, _defaultMap.Company.Width);
		SetValue('#txtCompanyName' , bShow, config.Company.Name , _defaultMap.Company.Name);
		SetText('#chkCompanyName'  , bShow, config.Company.Name , _defaultMap.Company.Name);
		
		bShow = config.Signatory1.Show == "true";
		$('#chkSignatory1').attr('checked', bShow);
		SetValue('#txtSignatory1Top'  , bShow, config.Signatory1.Top  , _defaultMap.Signatory1.Top);
		SetValue('#txtSignatory1Left' , bShow, config.Signatory1.Left , _defaultMap.Signatory1.Left);
		SetValue('#txtSignatory1Width', bShow, config.Signatory1.Width, _defaultMap.Signatory1.Width);
		SetValue('#txtSignatory1Name' , bShow, config.Signatory1.Name , _defaultMap.Signatory1.Name);
		SetText('#chkSignatory1'	  , bShow, config.Signatory1.Name , _defaultMap.Signatory1.Name);
		
		bShow = config.Signatory2.Show == "true";
		$('#chkSignatory2').attr('checked', bShow);
		SetValue('#txtSignatory2Top'  , bShow, config.Signatory2.Top  , _defaultMap.Signatory2.Top);
		SetValue('#txtSignatory2Left' , bShow, config.Signatory2.Left , _defaultMap.Signatory2.Left);
		SetValue('#txtSignatory2Width', bShow, config.Signatory2.Width, _defaultMap.Signatory2.Width);
		SetValue('#txtSignatory2Name' , bShow, config.Signatory2.Name , _defaultMap.Signatory2.Name);
		SetText('#chkSignatory2'	  , bShow, config.Signatory2.Name , _defaultMap.Signatory2.Name);

		$('input[type="checkbox"]').each(CheckboxChange);
	}
	
	function SetValue(element, show, value, defaultValue) {
		$(element).val(show ? value : '').attr('default', show ? value : defaultValue);
	}
	
	function SetText(element, show, value, defaultValue) {
		$(element).each(function () {
			var id = $(this.parentNode).attr('item');
			$('#'+id).text(show ? value : defaultValue).attr('default', show ? value : defaultValue);
		});
	}
	
	function ValidateTemplateName(src, args) {
		args.IsValid = $('#txtTemplateName').val().trim() != '';
		
		if (!args.IsValid)
			src.errormessage = 'Template Name should not be blank.';

		return !args.IsValid;
	}
	
	function ValidateSelection(src, args) {
		args.IsValid = true;
		
		if ($('#inputContainer input[type="checkbox"]:checked').length == 0) {
			args.IsValid = false;
			src.errormessage = 'Atleast one element should be selected.';
		}
		else {
			var bFlag = false;
			var elements = [];
			$('#inputContainer input[type="checkbox"]:checked').each(function() {
				$('input', this.parentNode.parentNode.parentNode).each(function() {
					if (this.value == '' && !bFlag)
						bFlag = true;
				});
				if (bFlag)
					elements.push($('label', this.parentNode).text());
				bFlag = false;
			});
			
			if (elements.length > 0) {
				args.IsValid = false;
				src.errormessage = 'All dimensions should be specified for element(s) : ' + elements.join(', ') + '.';
			}
		}
		
		return !args.IsValid;
	}
	
	function WarnDelete() {
		return confirm('Are you sure you want delete this configuration?');
	}
</script>
</asp:Content>