<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master" AutoEventWireup="true" CodeFile="InsuranceDetailsPopup.aspx.cs" Inherits="InsuranceDetailsPopup" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl" TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
	<script language="javascript" type="text/javascript">
		function CloseWindow(queystring) {
			window.opener.location = window.opener.location.pathname + queystring;
			window.close();
			window.opener.focus();
			return false;
		}
	</script>
	<table width="100%">
		<tr>
			<td align="center">
				<asp:UpdatePanel ID="UpdatePanel1" runat="server">
					<ContentTemplate>
						<table class="ClsGrayMainTitle" cellpadding="0" cellspacing="0" border="0" style="float: none; padding-right: 5px; margin: 10px 0px; width: 98%;">
							<tr>
								<td align="left" style="height: 20px;">
									<span class="MainTitleHead">Insurance Details</span>
								</td>
							</tr>
						</table>
						<table cellpadding="0" cellspacing="0" border="0" style="margin: 10px 0px; width: 98%;">
							<tr>
								<td align="right" class="ClsTextNormal" style="width: 25%; padding-right: 30px; height: 19px;">
									<span class="ClsMdtStar">* Mandatory Fields</span>
								</td>
							</tr>
							<tr align="left" id="trValSummary" runat="server">
								<td align="center">
									<asp:ValidationSummary ID="valSumInsuranceDetails" CssClass="LblErrorMsg" ShowSummary="true"
										runat="server" ValidationGroup="Save" />
									<asp:ValidationSummary ID="valSumDependentDetails" CssClass="LblErrorMsg" ShowSummary="true"
										runat="server" ValidationGroup="SaveChild" />
								</td>
							</tr>
						</table>
						<table width="100%" cellpadding="5">
							<tr align="left" id="trUpdate" runat="server">
								<td align="center">
									<asp:Label ID="lblUpdate" runat="server" ForeColor="Blue" Height="20px" Width="100%" Visible="false" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True" />
									<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red" Visible="false" Width="100%" EnableViewState="False" CssClass="ClsLabel" />
								</td>
							</tr>
						</table>
						<table width="65%" align="center">
							<tr>
								<td align="left" class="ClsBorderlight" style="width: 32%">
									<span class="ClsLabel">Name :</span> 
								</td>
								<td id="tdUserName" class="ClsHilightBGB" style="width: 50%" align="left" runat="server">
									<asp:Label ID="lblUserName" runat="server" CssClass="ClsLabel" Width="76%" EnableViewState="False"></asp:Label>
								</td>
							</tr>
                              <tr>
                                <td class="ClsBorderlight" align="center" style="width: 20%">
                                    <span class="ClsLabel">Insurance Card Number :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" valign="top" style="width: 31%">
                                    <asp:TextBox ID="txtInsuranceCardNumSelf" runat="server" CssClass="MidTxtBox" MaxLength="20" onkeypress="return PreventSpecialChars(event)"></asp:TextBox>                                   
                                </td>
                            </tr>  
							<tr>
								<td align="left" class="ClsBorderlight" style="width: 20%">
									<span class="ClsLabel">Insurance Amount :</span> 
								</td>
								<td align="left" class="ClsMdtStar">
									<asp:TextBox ID="txtInsuranceAmount"
												 runat="server"
												 MaxLength="15"
												 CssClass="MidTxtBox"
												 onblur="extractNumber(this,2,false);"
												 ondrop="event.returnValue=false"
												 onkeypress="return blockNonNumbers (this, event, true, false);"
												 onkeyup="extractNumber(this,2,false);"
												 onpaste="event.returnValue=false"/>
									*&nbsp;
									<asp:RequiredFieldValidator ID="reqInsuranceAmount" runat="server" ControlToValidate="txtInsuranceAmount"
										Display="None" ErrorMessage="Insurance Amount should not be blank." ValidationGroup="Save"></asp:RequiredFieldValidator>
									<asp:CompareValidator ID="cmpValInsuranceAmount" runat="server" ControlToValidate="txtInsuranceAmount"
										ValueToCompare="." Operator="NotEqual" Type="String" Display="None" ErrorMessage="Insurance Amount should be valid."
										ValidationGroup="Save"></asp:CompareValidator>
									<asp:CustomValidator ID="cstInsuranceAmount" Display="None" runat="server" ValidationGroup="Save"
										CssClass="LblErrorMsg" EnableClientScript="true" ErrorMessage="" SetFocusOnError="True"
										ClientValidationFunction="ValidateInsuranceAmount"></asp:CustomValidator>
								</td>
							</tr>
						</table>
						<table width="65%" align="center">
							<tr style="width: 100%">
								<td style="width: 30%" class="ClsBorderlight">
									<asp:RadioButton ID="optUnmarried" runat="server" GroupName="Status" Text="Unmarried"
										OnCheckedChanged="optUnmarried_CheckedChanged" AutoPostBack="True"/>
								</td>
								<td style="width: 30%" class="ClsBorderlight">
									<asp:RadioButton ID="optMarried" runat="server" GroupName="Status" Text="Married" 
										OnCheckedChanged="optMarried_CheckedChanged" AutoPostBack="True"/>
								</td>
								<td style="width: 30%" class="ClsBorderlight">
									<asp:RadioButton ID="optWidow" runat="server" GroupName="Status" Text="Widow" OnCheckedChanged="optWidow_CheckedChanged" 
										AutoPostBack="True" />
								</td>
							</tr>
						</table>
						<table width="65%" id="tblSave" runat="server" align="center">
							<tr style="width: 100%">
								<td align="center">
									<asp:Button ID="btnSave" runat="server" Text="Save" Width="72px" CssClass="ClsBtn" disable-page="true"
										BorderWidth="1px" CausesValidation="true" OnClick="btnSave_Click" ValidationGroup="Save"/>
									<asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="72px" CssClass="ClsBtn"
										BorderWidth="1px" CausesValidation="false" OnClick="btnCancel_Click" />
									<asp:Button ID="btnBack" runat="server" Text="Close" CssClass="ClsBtn" BorderWidth="1px"
										CausesValidation="false" Width="72px"/>
								</td>
							</tr>
						</table>
						<table id="tblDependent" runat="server" width="65%" align="center" visible="false">
							<tr>
								<td class="ClsLblLgnd" style="font-weight: bold; ">
									Dependant Details
								</td>
							</tr>
							<tr>
								<td align="left" class="ClsBorderlight" style="width: 20%">
									<span class="ClsLabel">Name :</span>
									<span class="LblSmlGray floatR">(First Name)</span>
								</td>
								<td align="left" class="ClsMdtStar" style="width: 31%; margin-left: 40px;">
									<asp:DropDownList ID="cmbSalutation" runat="server" CssClass="ExSmlCombo" Width="50px">
									</asp:DropDownList>
									<asp:TextBox ID="txtDependentName" runat="server" MaxLength="100" CssClass="LrgTxtBox" onblur="formatName(this)"></asp:TextBox>
									*&nbsp;
									<asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtDependentName"
										Display="None" ErrorMessage="First name should not be blank for dependant." ValidationGroup="SaveChild"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td align="left" class="ClsBorderlight" style="width: 20%">
									<span class="LblSmlGray floatR">(Middle Initial)</span>
								</td>
								<td align="left" class="ClsMdtStar" style="width: 31%">
									<asp:TextBox ID="txtDependentMiddleName" runat="server" CssClass="MidTxtBox" MaxLength="1" Width="50px" onblur="formatName(this)" />
								</td>
							</tr>
							<tr>
								<td align="left" class="ClsBorderlight" style="width: 20%">
									<span class="LblSmlGray floatR">(Last Name)</span>
								</td>
								<td align="left" class="ClsMdtStar" style="width: 31%">
									<asp:TextBox ID="txtDependentLastName" runat="server" MaxLength="100" CssClass="MidTxtBox" onblur="formatName(this)"></asp:TextBox>
									*&nbsp;
									<asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtDependentLastName"
										Display="None" ErrorMessage="Last name should not be blank for dependant." ValidationGroup="SaveChild"></asp:RequiredFieldValidator>
								</td>
							</tr>							
                             <tr>
                                <td class="ClsBorderlight" align="center" style="width: 20%">
                                    <span class="ClsLabel">Insurance Card Number :</span>
                                </td>
                                <td align="left" class="ClsMdtStar" valign="top" style="width: 31%">
                                    <asp:TextBox ID="txtInsuranceCardNum" runat="server" CssClass="MidTxtBox" MaxLength="20" onkeypress="return PreventSpecialChars(event)"></asp:TextBox>                                   
                                </td>
                            </tr>     
							<tr>
								<td class="ClsBorderlight" align="center" style="width: 20%">
									<span class="ClsLabel">Relation :</span>
								</td>
								<td align="left" class="ClsMdtStar" valign="top" style="width: 31%">
									<asp:TextBox ID="txtRelation" runat="server" CssClass="MidTxtBox" MaxLength="50"></asp:TextBox>
									*&nbsp;
									<asp:RequiredFieldValidator ID="reqRelation" runat="server" ControlToValidate="txtRelation"
										Display="None" ErrorMessage="Relation should not be blank for dependant." ValidationGroup="SaveChild"></asp:RequiredFieldValidator>
								</td>
							</tr>
							<tr>
								<td class="ClsBorderlight" align="center" style="width: 20%">
									<span class="ClsLabel">Date of Birth :</span>
								</td>
								<td align="left" class="ClsMdtStar" valign="top" style="width: 31%">
									<asp:TextBox ID="txtDependentdDOB" runat="server" CssClass="SmlTxtBox" MaxLength="11"></asp:TextBox>
									<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtDependentdDOB" Format="dd MMM yyyy"
										ShowWeekend="True" Enabled="true" ShowErrorMessage="false" InvalidDateMessage="Please select a valid Dependant birth date."
										ControlFocusOnError="True" />
									*&nbsp;
									<asp:RequiredFieldValidator ID="reqDependentDOB" runat="server" ControlToValidate="txtDependentdDOB"
										Display="None" ErrorMessage="Date of birth should not be blank for dependant." ValidationGroup="SaveChild"></asp:RequiredFieldValidator>
									<asp:CustomValidator ID="cstDependent" Display="None" runat="server" CssClass="ClsMdtStar"
										Visible="true" ClientValidationFunction="ValidateDependentDOB" ValidationGroup="SaveChild"></asp:CustomValidator>
								</td>
							</tr>
							<tr>
								<td colspan="2" align="center">
									<asp:Button ID="btnSaveDependent" runat="server" Text="Save" Width="71px" CssClass="ClsBtn" disable-page="true"
										BorderWidth="1px" CausesValidation="true" OnClick="btnSaveDependent_Click" ValidationGroup="SaveChild" />
									<asp:Button ID="btnCancelDependent" runat="server" Text="Cancel" Width="72px" CssClass="ClsBtn"
										BorderWidth="1px" CausesValidation="false" OnClick="btnCancelDependent_Click" />
								</td>
							</tr>
						</table>
						<table width="90%" align="center">                         
							<tr>
								<td colspan="2" align="center">
									<asp:ListView ID="lstvwDependentDetails" runat="server" DataKeyNames="UsersInsuranceDependentId"
										OnItemCommand="lstvwDependentDetails_ItemCommand" OnDataBound="lstvwDependentDetails_DataBound"
										OnItemDataBound="lstvwDependentDetails_ItemDataBound">
										<LayoutTemplate>
											<table width="100%" runat="server" id="tblDependentInfo" style="color: #333333" cellpadding="0"
												cellspacing="1" class="GridBorder">
												<tr id="trHeader" runat="server" class="ClsGridHeader">
													<th align="left" width="33%" class="paddingL">
														<asp:Label ID="lblName" runat="server" ForeColor="Black"> Name </asp:Label>
													</th>
													<th align="left" width="30%" class="paddingL">
                                                        <asp:Label ID="Label1" runat="server" ForeColor="Black"> Insurance Card Number </asp:Label>
                                                    </th>
													<th align="left" width="20%" class="paddingL">
														<asp:Label ID="lblDependentName" runat="server" ForeColor="Black"> Dependant </asp:Label>
													</th>
													<th align="center" width="25%" >
														<asp:Label ID="lnlDesignation" runat="server" ForeColor="Black"> Date of Birth</asp:Label>
													</th>
													<th align="center" width="125px">
														Edit
													</th>
													<th align="center" width="125px">
														Delete
													</th>
												</tr>
												<tr runat="server" id="itemPlaceholder">
												</tr>
											</table>
										</LayoutTemplate>
										<ItemTemplate>
											<tr id="Tr2" runat="server" class="ClsGridRow">
												<td align="left" class="paddingL">
													<asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
												</td>
												<td align="left" class="paddingL">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("InsuranceCardNumber") %>'></asp:Label>
                                                </td> 
												<td align="left" class="paddingL">
													<asp:Label ID="lblDependent" runat="server" Text='<%# Eval("Relation") %>'></asp:Label>
												</td>
												<td align="center">
													<asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("DateOfBirth","{0:dd-MMM-yyyy}") %>'></asp:Label>
												</td>
												<td align="center">
													<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATE_DEPENDENT"
														ImageUrl="../images/IconGrid_Edit.GIF" />
												</td>
												<td align="center">
													<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="REMOVE"
														ImageUrl="../images/IconGrid_Delete.gif" />
												</td>
											</tr>
										</ItemTemplate>
										<AlternatingItemTemplate>
											<tr id="Tr3" runat="server" class="ClsGridAltRow">
												<td class="paddingL" align="left">
													<asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
												</td>
												<td align="left" class="paddingL">
                                                    <asp:Label ID="Label2" runat="server" Text='<%# Eval("InsuranceCardNumber") %>'></asp:Label>
                                                </td> 
												 <td align="left" class="paddingL">
													<asp:Label ID="lblDependent" runat="server" Text='<%# Eval("Relation") %>'></asp:Label>
												</td>
												<td align="center">
													<asp:Label ID="lblDesignation" runat="server" Text='<%# Eval("DateOfBirth","{0:dd-MMM-yyyy}") %>'></asp:Label>
												</td>
												<td align="center">
													<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UPDATE_DEPENDENT"
														ImageUrl="../images/IconGrid_Edit.GIF" />
												</td>
												<td align="center">
													<asp:ImageButton ID="imgBtnDelete" CommandName="REMOVE" CausesValidation="false"
														runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
												</td>
											</tr>
										</AlternatingItemTemplate>
									</asp:ListView>
								</td>
							</tr>
						</table>
						<asp:HiddenField ID="hidUserId" runat="server" />
						<asp:HiddenField ID="hidMode" runat="server" />
						<asp:HiddenField ID="hidAddDependentMode" runat="server" />
						<asp:HiddenField ID="hidUsersInsuranceDependentId" runat="server" />
						<asp:HiddenField ID="hidUserName" runat="server" />
						<asp:HiddenField ID="hidStaffGroupId" runat="server" />
						<asp:HiddenField ID="hidStaffGroupsName" runat="server" />
						<asp:HiddenField ID="hidUserRoleId" runat="server" />
						<asp:HiddenField ID="hidFilter" runat="server" />
						<asp:HiddenField ID="hidIsConfigured" runat="server" />
						<asp:HiddenField ID="hidRowCount" runat="server" Value="0" />					    
                        <asp:HiddenField ID="hidIsLocked" runat="server" Value="N" />					    
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnBack" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwDependentDetails" 
							EventName="ItemCommand" />
					   
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
	</table>
	<script type="text/javascript" language="javascript">
		_clientoptMarried = "<%=this.optMarried.ClientID %>";
		_clientoptWidow = "<%=this.optWidow.ClientID %>";
		_clienttxtInsuranceAmount = "<%=this.txtInsuranceAmount.ClientID %>";
		_clientcstInsuranceAmount = "<%=this.cstInsuranceAmount.ClientID %>";
		_clienttxtDependentdDOB = "<%=this.txtDependentdDOB.ClientID %>";
		_clienttxtDependentName = "<%=this.txtDependentName.ClientID %>";
		_clienttxtDependentMiddleName = "<%=this.txtDependentMiddleName.ClientID %>";
		_clienttxtRelation = "<%=this.txtRelation.ClientID %>";
		_clientcstDependent = "<%=this.cstDependent.ClientID %>";
		_clienttblDependent = "<%=this.tblDependent.ClientID %>";
		_clientbtnSaveDependent = '<%= this.btnSaveDependent.ClientID %>';
		_clientbtnBack = "<%=this.btnBack.ClientID %>";
		_clienthidRowCount = "<%=this.hidRowCount.ClientID %>";
		_clientlblUpdate = "<%=this.lblUpdate.ClientID %>";
		_clientlblErrorMsg = '<%= this.lblErrorMsg.ClientID %>';
		_clienthidAddDependentMode = "<%=this.hidAddDependentMode.ClientID %>";
		
		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_endRequest(EndReqHandler);
		prm.add_beginRequest(beginRequestHandler);
		var iScroll;

		function beginRequestHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement;
			if (postBackElement.id == _clientbtnSaveDependent)
				$get(_clientbtnSaveDependent).disabled = true;
		}
		
		function EndReqHandler(sender, args) {
			var postBackElement = sender._postBackSettings.sourceElement;
			if (postBackElement.id == _clientbtnBack) {
				window.close();
				window.opener.focus();
			}
			else if (postBackElement.id == _clientbtnSaveDependent)
				$get(_clientbtnSaveDependent).disabled = false;
		}
		
		function IsValidDate(date) {
			if(typeof(date) == 'string')
				date = new Date(date);
			return !(date == 'Invalid Date' || date == 'NaN' || date.getFullYear() < 1900);
		}

		function ConfirmDelete() {
			var bResult = true;
			if (!window.confirm('Are you sure you want to delete this record?')) {
				bResult = false;
			}
			else if ($get(_clienttblDependent) != null) {
				$get(_clienttxtDependentName).value = "";
				$get(_clienttxtDependentMiddleName).value = "";
				$get(_clienttxtRelation).value = "";
				$get(_clienttxtDependentdDOB).value = "";
			}
			return bResult;
		}

		function ValidateDependentDOB(oSrc, args) {
			ClearMessages();
			
			var DOB;
			var todayDate = new Date();
			if ($get(_clienttxtDependentdDOB).value != "") {
				if (document.all) {
					DOB = new Date(($get(_clienttxtDependentdDOB).value).replace('-', ' '));
					todayDate = new Date(getDateString(new Date()).replace('-', ' ')); ;
				}
				else {
					DOB = new Date(convertdate($get(_clienttxtDependentdDOB).value));
					todayDate = new Date();
				}

				if (!IsValidDate(DOB)) {
					$get(_clientcstDependent).errormessage = "Please select a valid Dependant birth date.";
					args.IsValid = false;
					return true;
				}
				else if (DOB > todayDate) {
					$get(_clientcstDependent).errormessage = "Dependant birth date should not be future date.";
					args.IsValid = false;
					return true;
				}
			}
		}
		
		function ValidateInsuranceAmount(oSrc, args) {
			ClearMessages();
			
			var txtAmount = $get(_clienttxtInsuranceAmount).value;
			if (txtAmount != "") {
				if (parseInt(txtAmount) == 0) {
					$get(_clientcstInsuranceAmount).errormessage = "Insurance amount should not be zero.";
					args.IsValid = false;
					return true;
				}
				args.IsValid = true;
				return false;
			}
        }

        function PreventSpecialChars(e) {
            var k;
            document.all ? k = e.keyCode : k = e.which;
            return ((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57) || k==0 || k==9);
        }

		function ClearMessages() {
			$('#' + _clientlblUpdate).hide();
			$('#' + _clientlblErrorMsg).hide();
		}
	</script>
</asp:Content>
