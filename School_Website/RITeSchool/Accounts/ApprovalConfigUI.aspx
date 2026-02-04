<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalConfigUI.aspx.cs" Inherits="ApprovalConfigUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<asp:UpdatePanel ID="mainUpdatePanel"
				 runat="server">
	<ContentTemplate>
		<table id="tblMain" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%; margin-top: 10px;">
			<tr>
				<td align="center">
					<table border="0" cellpadding="0" cellspacing="1">
						<tr>
							<td align="right" colspan="2">
								<span class="ClsMdtStar" style="margin-right: 10px;">* Mandatory Fields</span>
							</td>
						</tr>
						<tr>
							<td align="left">
								<asp:ValidationSummary ID="valsumErrorMessages"
													   runat="server"
													   CssClass="ClsLabel"
													   ShowSummary="true" />
								<asp:Label ID="lblErrorMessage"
										   runat="server"
										   EnableViewState="false"
										   CssClass="ClsLabel"
										   style="width: 100%; text-align: center; margin-bottom: 8px;"
										   ForeColor="Red"
										   Visible="false" />
								<asp:Label ID="lblUpateMessage"
										   runat="server"
										   EnableViewState="false"
										   CssClass="ClsLabel"
										   style="width: 100%; text-align: center; margin-bottom: 8px;"
										   ForeColor="Blue"
										   Font-Bold="true"
										   Visible="false" />
                                 <asp:CustomValidator ID="CreatorDesignationList"
                                                    runat="server"
                                                    Display="None" 
                                                    SetFocusOnError="true" 
                                                    ClientValidationFunction="ValidateCreatorDesignation" />
								<asp:CustomValidator ID="ApprovalOrderValidator"
													 runat="server"
													 ClientValidationFunction="ValidateApprovalOrder"
													 Display="None"
													 SetFocusOnError="True" />
								<asp:CustomValidator ID="EmptyApprovalOrderValidator"
													 runat="server"
													 ClientValidationFunction="ValidateEmptyApprovalOrder"
													 Display="None"
													 SetFocusOnError="True" />                                                     
								<asp:CustomValidator ID="cstCheckAtleastOneSelected"
													 runat="server"
													 ClientValidationFunction="CheckAtleastOneSelected"
													 Display="None"
													 SetFocusOnError="True" />
								<asp:CustomValidator ID="DuplicateApprovalOrderValidator"
													 runat="server"
													 ClientValidationFunction="ValidateDuplicateApprovalOrder"
													 Display="None"
													 SetFocusOnError="True" />
								<asp:CustomValidator ID="FinalApproverValidator"
													 runat="server"
													 ClientValidationFunction="ValidateAtleastOneFinalApprover"
													 Display="None"
													 SetFocusOnError="True"
													 ErrorMessage="Atleast one designation should be selected as Final Approver." />     
							</td>
						</tr>
						<tr>
							<td align="center">
								<table cellpadding="0" cellspacing="4">
									<tr>
										<td align="center" class="ClsBorderlight">
											<span class="ClsLabel">Voucher Type :</span>
										</td>
										<td align="left">
											<asp:DropDownList ID="ddlVoucherTypes"
															  runat="server"
															  CssClass="MidCombo"
															  AutoPostBack="true"
															  OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" />
										</td>
									</tr>								
									<tr>
										<td align="center" class="ClsBorderlight">
											<span class="ClsLabel">Creator Designation :</span>
										</td>
										<td align="left" style="width: 100px">
											<asp:DropDownList ID="ddlCreatorDesignationList"
															  runat="server"
															  CssClass="MidCombo"
															  AutoPostBack="true"
															  Width="222px" onselectedindexchanged="ddlCreatorDesignationList_SelectedIndexChanged"
															   />
                                        </td>
                                        <td>
                                           <span class="ClsMdtStar">*</span>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td id="tdlstvwApprovalConfig" runat="server" align="center" style="width: 50%;">
								<asp:ListView ID="lstvwApprovalConfig"
											  runat="server"
											  DataKeyNames="ApproverDesignation,ApprovalOrder"
											  OnItemDataBound="lstvwApprovalConfig_ItemDataBound" 
											  OnDataBound="lstvwApprovalConfig_DataBound">
									<LayoutTemplate>
										<table cellpadding="0" cellspacing="0" width="500px">
											<tr>
												<td style="height: 40px" id="trLbl" runat="server" align="left">
													<span class="ClsLblLgnd">Approver Designations :</span>
												</td>
											</tr>
										</table>
										<div style="height: 475px; width: 500px; overflow: scroll;">
										<table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
											<tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
												<th align="center" id="thChkSelectAll" runat="server" style="width: 40px; font-size: 9pt;">
													<asp:CheckBox ID="chkSelectAll"
																  runat="server"
																  onclick="CheckUncheckAll(this);" />
												</th>
												<th align="left" class="paddingL" style="width: 175px; font-size: 9pt;">
													Designation
												</th>
												<th align="center" style="font-size: 9pt; width: 120px;">
													Is Final Approver?
												</th>
												<th align="center" style="font-size: 9pt; width: 120px;">
													Approval Order
												</th>
											</tr>
											<tr id="itemPlaceholder" runat="server">
											</tr>
										</table>
										</div>
									</LayoutTemplate>
									<ItemTemplate>
										<tr id="trGridRow" runat="server" class="ClsGridRow">
											<td align="center">
												<asp:CheckBox ID="chkSelect"
															  runat="server"
															  OnClick="ChkOnChange(this);" />
											</td>
											<td align="left" class="paddingL">
												<asp:Label ID="lblDesignation"
														   runat="server"
														   Text='<%# Eval("ApproverDesignation.Designation") %>' />
											</td>
											<td align="center">
												<asp:CheckBox ID="chkFinalApprover"
															  runat="server"
															  Checked='<%# Convert.ToBoolean(Eval("IsFinalApprover")) %>' />
											</td>
											<td align="center" valign="top">
												<asp:DropDownList ID="ddlApprovalOrder"
																  runat="server"
																  Enabled="false"
																  AppendDataBoundItems="true">
													<asp:ListItem Text="-- SELECT --" Value="0" />
												</asp:DropDownList>
												<span id="mdtStar" runat="server" class="ClsMdtStar"> * </span>
											</td>
										</tr>
									</ItemTemplate>
								</asp:ListView>
							</td>
							<td id="tdlstvwConfiguredApprovalChain" runat="server" align="center" valign="top">
								<asp:ListView ID="lstvwConfiguredApprovalChain"
											  runat="server"
											  DataKeyNames="Id,VoucherType,CreatorDesignation"
											  OnItemCommand="lstvwConfiguredApprovalChain_ItemCommand"
											  OnDataBound="lstvwConfiguredApprovalChain_DataBound"
											  OnItemDataBound="lstvwConfiguredApprovalChain_ItemDataBound" >
									<LayoutTemplate>
										<table cellpadding="0" cellspacing="0" width="375px">
											<tr>
												<td style="height: 40px" id="trLbl" runat="server" align="left">
													<span class="ClsLblLgnd">Existing Configurations :</span>
												</td>
											</tr>
										</table>
										<table border="0" cellpadding="3" cellspacing="1" class="GridBorder" width="375px">
											<tr id="trGridHeader" runat="server" class="ClsGridHeader">
												<th align="center"></th>
												<th align="left" style="font-size: 9pt; width: 80px;">Voucher Type</th>
												<th align="left" style="font-size: 9pt; width: 150px;">Creator Designation</th>
												<th align="center" style="font-size: 9pt; width: 50px;">Action</th>
											</tr>
											<tr id="itemPlaceholder" runat="server">
											</tr>
										</table>
									</LayoutTemplate>
									<ItemTemplate>
										<tr id="trGridRow" runat="server" class="ClsGridRow">
											<td align="center" style="width: 24px;">
												<img src="../images/IconGrid_AssignTrue.gif" />
											</td>
											<td align="left">
												<%# Eval("VoucherType.Name") %>
											</td>
											<td align="left">
												<%# Eval("CreatorDesignation.Designation") %>
											</td>
											<td align="center">
												<asp:ImageButton ID="imgbtnEdit"
																 runat="server"
																 AlternateText="Edit"
																 ToolTip="Edit"
																 CausesValidation="false"
																 CommandName="EDIT_ROW"
																 ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
																 style="vertical-align: middle;" />
												<asp:ImageButton ID="imgbtnDelete"
																 runat="server"
																 AlternateText="Delete"
																 ToolTip="Delete"
																 CausesValidation="false"
																 CommandName="DELETE_ROW"
																 ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
																 style="margin-left: 3px; vertical-align: middle;"
																 OnClientClick="if(!WarnOnDelete()){return false}" />
											</td>
										</tr>
									</ItemTemplate>
								</asp:ListView>
							</td>
						</tr>
						<tr>
							<td align="center">
                                <asp:Button ID="btnBack" 
                                            Text="Back" 
                                            CssClass="ClsBtn"                                            
                                            runat="server"                                            
                                            style="margin-right: 5px;"                                           
                                            UseSubmitBehavior="false" 
                                            CausesValidation="False" TabIndex="2" />
								<asp:Button ID="btnSave"
											runat="server"
											Text="Save"
											CssClass="ClsBtn"
											OnClick="btnSave_Click"
											disable-page="true"
											OnClientClick="ClearMessages();"
											style="margin-top: 10px;" TabIndex="3" />
								<asp:Button ID="btnCancel"
											runat="server"
											CssClass="ClsBtn"
											Text="Cancel"
                                            OnClientClick="ClearMessages();"
											CausesValidation="false"
											UseSubmitBehavior="false"
											style="margin-left: 5px;" onclick="btnCancel_Click" TabIndex="4" />                                
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</ContentTemplate>
</asp:UpdatePanel>
<table border="0" cellpadding="0" cellspacing="1" style="width: 475px; margin-top: 10px;">
    <tr id="trDivErr" runat="server" visible="false">
        <td align="left" width="100%" class="LblNoRecord">
            <div class="ClsConfigText" style="height: 20px;">Users are not available. Please add any of following user details:</div>
			<a href="~/RITeSchool/Admin/TeacherUI.aspx" style="height: 20px; font-size: 13px;">Teacher</a><br />
			<a href="~/RITeSchool/Admin/SupervisorDetailsUI.aspx" style="height: 20px; font-size: 13px;">Admin Staff</a><br />
			<a href="~/RITeSchool/Payroll/OtherStaffUI.aspx" style="height: 20px; font-size: 13px;">Other Staff</a><br />
        </td>
    </tr>
</table>

<script type="text/javascript">
// IDs of controls on page
var _clientlblUpdateMessage = '<%= this.lblUpateMessage.ClientID %>';
var _clientlblErrorMessage = '<%= this.lblErrorMessage.ClientID %>';
var _clientddlVoucherTypes = '<%= this.ddlVoucherTypes.ClientID %>';
var _clientddlCreatorDesignationList = '<%= this.ddlCreatorDesignationList.ClientID %>';
var _clientcstCreatorDesignationList= '<%=this.CreatorDesignationList.ClientID %>'
var _clientbtnSave = '<%= this.btnSave.ClientID %>';
var _clientlstvwApprovalConfig = '<%= this.lstvwApprovalConfig.ClientID %>';
var _clientApprovalOrderValidator = '<%= this.ApprovalOrderValidator.ClientID %>';
var _clientEmptyApprovalOrderValidator = '<%= this.EmptyApprovalOrderValidator.ClientID %>';
var _clientDuplicateApprovalOrderValidator = '<%= this.DuplicateApprovalOrderValidator.ClientID %>';
var _clientcstCheckAtleastOneSelected='<%=this.cstCheckAtleastOneSelected.ClientID %>'

// Common Strings variables
var empty = '';
var _ctrl = '_ctrl';
var _chkSelect = '_chkSelect';
var _lblDesignation = '_lblDesignation';
var _chkFinalApprover = '_chkFinalApprover';
var _ddlApprovalOrder = '_ddlApprovalOrder';
var _mdtStar = '_mdtStar';
var commaSeparator = ',';
var commaSeparator2 = ', ';
var period = '.';


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
}


/* -----------------
 *	CHECKBOX EVENTS
 * -----------------
 */

// This function is used to Check Uncheck all checkboxes in the ListView
function CheckUncheckAll(src) {
	if (src == null)
		src = $get(_clientlstvwApprovalConfig + '_chkSelectAll');
	
	var iRowCount = 0;
	var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	while (chk != null) {
		chk.checked = src.checked;
		ChkOnChange(chk);
		iRowCount++;
		chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	}
}

// This function is used to enable/disable textbox when it's checkbox is clicked.
function ChkOnChange(src) {
	var iRowNo = src.id.match(/_ctrl(\d+)_chkSelect/)[1];
	var isFinalApprover = $get(_clientlstvwApprovalConfig + _ctrl + iRowNo + _chkFinalApprover);
	isFinalApprover.disabled = !src.checked;

	var ddlApprovalOrder = $get(_clientlstvwApprovalConfig + _ctrl + iRowNo + _ddlApprovalOrder);
	ddlApprovalOrder.disabled = !src.checked;

	var mdtStar = $get(_clientlstvwApprovalConfig + _ctrl + iRowNo + _mdtStar);
	mdtStar.style.visibility = src.checked ? "visible" : "hidden";
}


/* ----------------------
 *	VALIDATION FUNCTIONS
 * ----------------------
 */

// This function checks if atleast one designation is selected
function CheckAtleastOneSelected() {
	var iRowCount = 0;
	var bSelected = false;

	var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	while (chk != null) {
		if (chk.checked) {
			bSelected = true;
			break;
		}
		
		iRowCount++;
		chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	}

    if (!bSelected)
        $get(_clientcstCheckAtleastOneSelected).errormessage = "Alteast one designation should be selected.";
	return bSelected;
}

//Check that Creator Designation is selected or not
function ValidateCreatorDesignation(src, args) {
    var ddl = $get(_clientddlCreatorDesignationList)
    if (ddl.value == 0) {
        $get(_clientcstCreatorDesignationList).errormessage = "Creator Designation should be selected.";
        args.IsValid = false;
        return true;
    }
    args.IsValid = true;
    return false;
}

// This function is used to validate if atleast one Final Approver is selected.
function ValidateAtleastOneFinalApprover(src, args) {
	var iRowCount = 0;

	var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	while (chk != null) {
		var chkFinalApprover = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkFinalApprover);

		if (chk.checked && chkFinalApprover && chkFinalApprover.checked) {
			args.IsValid = true;
			return false;
			break;
		}      

		iRowCount++;
		chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	}

	args.IsValid = false;
	return true;
}

// This function is used to validate approval order of selected designations
function ValidateApprovalOrder(src, args) {
	var _approvalOrder = PopulateApprovalOrder();

	var _list = empty;
	
	// Iterate over the items in _approvalOrder
	for (var i in _approvalOrder) {
		if (_approvalOrder[i].finalApprover) {
			var _order = parseInt(_approvalOrder[i].approvalOrder);
			for (var j in _approvalOrder) {
				if (!(_approvalOrder[j].finalApprover) && parseInt(_approvalOrder[j].approvalOrder) >= _order) {
					if (_list == empty)
						_list = _approvalOrder[i].designation;
					else
						_list = _list + commaSeparator2 + _approvalOrder[i].designation;
					break;
				}
			}
		}
	}

	if (_list != empty) {
		$get(_clientApprovalOrderValidator).errormessage = 'Approval order should be higher for : ' + (_list) + period;
		$get(_clientApprovalOrderValidator).innerHTML = 'Approval order should be higher for : ' + (_list) + period;
		args.IsValid = false;
	}

	return !args.IsValid;		
}

// This function is used to validate empty approval order of selected designations
function ValidateEmptyApprovalOrder(src, args) {
	var iRowCount = 0;
	var sortOrders = empty;
	var notSelected = true;
	var isDuplicate = false;
	var sCount = empty;
	var sCnt = empty;

	var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	var desig = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _lblDesignation);
	var cmb = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _ddlApprovalOrder);
	$get(_clientEmptyApprovalOrderValidator).errormessage = empty;
	
	while (chk != null) {
		if (chk.checked == true) {
			if (cmb.value == '0') {
				notSelected = false;
				if (sCount != empty)
					sCount = sCount + commaSeparator2 + desig.innerHTML; //(iRowCount + 1);
				else
					sCount = desig.innerHTML; 
			}
			else {
				if (sortOrders.match(commaSeparator + cmb.value + commaSeparator) != null) {
					isDuplicate = true;
					if (sCnt != empty)
						sCnt = sCnt + commaSeparator2 + (iRowCount + 1);
					else
						sCnt = (iRowCount + 1);
				}
				else {
					if (cmb.value != '9999')
						sortOrders = sortOrders + commaSeparator + cmb.value + commaSeparator;
				}
			}
		}
		else {
			cmb.value = '0';
		}
		
		iRowCount++;
		chk = $get(_clientlstvwApprovalConfig + _ctrl + (iRowCount) + _chkSelect);
		desig = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _lblDesignation);
		cmb = $get(_clientlstvwApprovalConfig + _ctrl + (iRowCount) + _ddlApprovalOrder);
	}
	
	if (!notSelected) {
		$get(_clientEmptyApprovalOrderValidator).errormessage = 'Approval order should be selected for : ' + (sCount) + period;
		$get(_clientEmptyApprovalOrderValidator).innerHTML = 'Approval order should be selected for : ' + (sCount) + period;
		args.IsValid = false;
	}

	return !args.IsValid;
}

// This function is used to validate duplicate approval order for selected designations
function ValidateDuplicateApprovalOrder(src, args) {
	var iRowCount = 0;
	var sortOrders = empty;
	var isDuplicate = false;

	var sCnt = empty;
	var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	var desig = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _lblDesignation);
	var cmb = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _ddlApprovalOrder);

	while (chk != null) {
		if (chk.checked == true) {
			if (cmb.value != 0) {
				if (sortOrders.match(commaSeparator + cmb.value + commaSeparator) != null) {
					isDuplicate = true;
					if (sCnt != empty)
						sCnt = sCnt + commaSeparator2 + desig.innerHTML; 
					else
						sCnt = desig.innerHTML; 
				}
				else {
					if (cmb.value != '9999')
						sortOrders = sortOrders + commaSeparator + cmb.value + commaSeparator;
				}
			}
		}

		iRowCount++;
		chk = $get(_clientlstvwApprovalConfig + _ctrl + (iRowCount) + _chkSelect);
		desig = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _lblDesignation);
		cmb = $get(_clientlstvwApprovalConfig + _ctrl + (iRowCount) + _ddlApprovalOrder);
	}
	
	if (isDuplicate) {
		$get(_clientDuplicateApprovalOrderValidator).errormessage = 'Approval order should not be duplicate for : ' + (sCnt) + period;
		$get(_clientDuplicateApprovalOrderValidator).innerHTML = 'Approval order should not be duplicate for : ' + (sCnt) + period;
		args.IsValid = false;
	}

	return !args.IsValid;
}


/* -----------------------
 *	MISC HELPER FUNCTIONS
 * -----------------------
 */

function ClearMessages() {
	var lblUpdateMsg = $get(_clientlblUpdateMessage);
	if(lblUpdateMsg)
		lblUpdateMsg.innerHTML = empty;
	
	var lblErrorMsg = $get(_clientlblErrorMessage);
	if(lblErrorMsg)
		lblErrorMsg.innerHTML = empty;
}

function ToggleControls(state) {
	var _ddlVoucherTypes = $get(_clientddlVoucherTypes);
	var _ddlCreatorDesignationList = $get(_clientddlCreatorDesignationList);
	var _btnSave = $get(_clientbtnSave);

	if (_ddlVoucherTypes)
		_ddlVoucherTypes.disabled = state;
	if (_ddlCreatorDesignationList)
		_ddlCreatorDesignationList.disabled = state;
	if (_btnSave)
		_btnSave.disabled = state;
}

// This function popoulates the _approvalOrder object with values in the listview on the page.
function PopulateApprovalOrder() {
	var iRowCount = 0;
	var _approvalOrder = {};

	var _chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);

	while (_chk != null) {
		if (_chk.checked) {
			var chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkFinalApprover);
			var desig = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _lblDesignation);
			var cmb = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _ddlApprovalOrder);

			_approvalOrder[iRowCount.toString()] = { designation: desig.innerHTML, finalApprover: chk.checked, approvalOrder: cmb.value };
		}

		iRowCount++;
		_chk = $get(_clientlstvwApprovalConfig + _ctrl + iRowCount + _chkSelect);
	}

	return _approvalOrder;
}

// This function is used to warn the user when he is deleting a configuration.
function WarnOnDelete() {
	return confirm("Are you sure you want to delete this configuration?");
}
</script>
</asp:Content>
