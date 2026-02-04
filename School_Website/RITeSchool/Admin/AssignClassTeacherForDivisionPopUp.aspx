<%@ Page Language="C#" MasterPageFile="../MasterPages/PopupMasterSml.master" AutoEventWireup="true"
	CodeFile="AssignClassTeacherForDivisionPopUp.aspx.cs" Inherits="AssignClassTeacherForDivisionPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
	<div style="width: 100%; overflow: auto">
		<table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
			<tr>
				<td style="background-color: white;" id="MainDataTable" align="center">
					<!-- Data Insert Here -->
					<table border="0" cellpadding="0" cellspacing="2" style="width: 95%;">
						<tr>
							<td align="left" colspan="4" style="height: 5%">
								<table border="0" cellpadding="2" cellspacing="2" width="100%">
									<tr>
										<td align="left" colspan="2" rowspan="1">
											<table border="0" cellpadding="0" cellspacing="0" class="ClsGrayMainTitle" style="padding-right: 5px">
												<tr>
													<td style="height: 20px" align="left">
														<span class="MainTitleHead" style="font-weight: bold">
                                                        <asp:Label runat="server" ID="lblAssignClassTeacherText" Text="<%$ Resources:LocalizedResources, AssignClassTeacher %>"></asp:Label>
                                                        </span>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr align="center">
										<td>
											<asp:Label ID="lblErr" runat="server" CssClass="LblErrorMsg" EnableViewState="false"></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="left" colspan="2">
											<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ShowMessageBox="false"
												ShowSummary="true" />
										</td>
									</tr>
									<tr align="left">
										<td>
											<asp:Label ID="lblError" runat="server" CssClass="LblErrorMsg" Visible="false"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<table>
												<tr>
													<td>
														<span class="ClsLblLgnd">
                                                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:LocalizedResources, Legend %>"></asp:Label> </span>
													</td>
													<td>
														<span style="background-color: #FEEABA; height: 20px; border: 1px solid black; width: 20px;">
															<img src="../images/spacer.gif" width="20px" height="10px" /></span>
													</td>
													<td class="ClsTextNormal" style="font-weight: bold">
														<asp:Label runat="server" ID="Label2" Text="<%$ Resources:LocalizedResources, AlreadyAssignedClassTeacher %>"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td align="right">
											<table width="540">
												<tr>
													<td colspan="2">
													</td>
													<td colspan="1" align="left">
														<span class="ClsMdtStar" visible="false" style="width: 100px;"><span>* </span>
                                                        <asp:Label runat="server" ID="Label3" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label></span>
													</td>
												</tr>
												<tr id="Tr2">
													<td align="left" colspan="1" style="width: 75px; height: 15px" class="ClsBorderlight">
														<span class="ClsLabel" style="width: 102px">                                                        
                                                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, StandardName %>"></asp:Label>
                                                        <span class="colonPadding"> :</span></span>
													</td>
													<td align="left" style="width: 186px; height: 15px" class="ClsHilightBG">
														<asp:Label ID="lblStandardName" runat="server" CssClass="LblNrmlB"></asp:Label>
													</td>
													<td align="left" style="width: 25%; height: 15px">
													</td>
												</tr>
												<tr id="Tr3">
													<td align="left" colspan="1" style="height: 15px" valign="top" class="ClsBorderlight">
														<span class="ClsLabel"><asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, DivisionName %>"></asp:Label>
                                                        <span class="colonPadding"> :</span></span></span>
													</td>
													<td align="left" class="ClsHilightBG" colspan="1" style="height: 15px; width: 186px;">
														<asp:Label ID="lblDivisionName" runat="server" CssClass="LblNrmlB"></asp:Label>
													</td>
													<td align="left" colspan="1" style="height: 15px">
													</td>
												</tr>
												<tr id="Tr4">
													<td align="left" colspan="1" style="height: 15px" valign="top" class="ClsBorderlight">
														<span class="ClsLabel">
                                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:LocalizedResources, AssignTeacher %>"></asp:Label>
                                                        <span class="colonPadding"> :</span>
                                                        </span>
													</td>
													<td align="left" class="ClsMdtStar" colspan="1" style="height: 15px; width: 186px;">
														<asp:DropDownList ID="cmbTeacherName" runat="server" CssClass="ExLrgCombo">
														</asp:DropDownList>
													</td>
													<td align="left" class="ClsMdtStar" colspan="1" style="height: 15px">
														<asp:Label ID="lblMandatory" runat="server" Text="*" Width="1px" EnableViewState="false"></asp:Label>
														<asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeacherName"
															Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValTeacherName %>" Operator="NotEqual"
															ValueToCompare='0'></asp:CompareValidator>
													</td>
												</tr>
												<tr id="Tr1">
													<td align="left" colspan="2" style="height: 10px" valign="top">
														<asp:CheckBox ID="chkAddTeacher" runat="server" Text="<%$ Resources:LocalizedResources, AssignOneMoreClassTeacher %>" />
													</td>
													<td align="left" colspan="1" style="width: 10px; height: 10px" valign="top">
													</td>
												</tr>
												<tr id="Tr5">
													<td align="left" colspan="1" style="height: 15px" valign="top" class="ClsBorderlight">
														<span class="ClsLabel" style="width: 125px">
                                                        <asp:Label ID="Label7" runat="server" Text="<%$ Resources:LocalizedResources, AdditionalTeacher %>"></asp:Label>
                                                        <span class="colonPadding"> :</span>
                                                        </span>
													</td>
													<td align="left" class="ClsMdtStar" colspan="1" style="height: 15px; width: 186px;">
														<asp:DropDownList ID="cmbAddTeacherName" runat="server" CssClass="ExLrgCombo" Enabled="false">
														</asp:DropDownList>
													</td>
													<td align="left" class="ClsMdtStar" colspan="1" style="height: 15px">
														<asp:Label ID="lblAddMandatory" runat="server" Text="*" Width="1px" EnableViewState="true"></asp:Label>
													</td>
												</tr>
												<tr>
													<td align="center" colspan="2" style="height: 10px" valign="top">
														<asp:CustomValidator ID="cstTeachers" runat="server" ControlToValidate="cmbAddTeacherName"
															ClientValidationFunction="ValidateTeachers" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ErrorText %>"></asp:CustomValidator>
													</td>
													<td align="center" colspan="1" style="height: 10px" valign="top">
													</td>
												</tr>
												<tr id="Tr21">
													<td align="center" colspan="2" style="height: 10px" valign="top">
														<asp:Button ID="btnSave" runat="server" CausesValidation="true" Text="<%$ Resources:LocalizedResources, Save %>" CssClass="ClsBtnSml" disable-page="true"
															OnClientClick=" if(!showMultipleClassTeacherMessage()){return false;}" BorderStyle="Solid" OnClick="btnSave_Click"
															 />
														<asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtnSml" CausesValidation="false"
															BorderStyle="Solid" UseSubmitBehavior="false" OnClick="btnClose_Click" />
													</td>
													<td align="center" colspan="1" style="height: 10px" valign="top">
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td align="center" colspan="2">
											<table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
												<tr>
													<td align="right" colspan="4" rowspan="" style="padding-right: 15px;">
														&nbsp;
													</td>
												</tr>
											</table>
											<asp:HiddenField ID="hidStandardId" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidDivisionId" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidDisplayMember" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidAddDisplayMember" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidTeacherId" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidAddTeacherId" runat="server"></asp:HiddenField>
											<asp:HiddenField ID="hidIsConfig" runat="server" />
											<asp:HiddenField ID="hidTeacherName" runat="server" />
											<asp:HiddenField ID="hidAddTeacherName" runat="server" />
											<asp:HiddenField ID="hidClassTeacherJsonObject" runat="server" />
                                            <asp:HiddenField ID="hidvalTeachersBoth" runat="server" />
                                            <asp:HiddenField ID="hidCultureInfo" runat="server" />
                                            <asp:HiddenField ID="hidValTeacherAdditional" runat="server" />
                                            <asp:HiddenField ID="hidValTeacherOf" runat="server" />
                                            <asp:HiddenField ID="hidAreYouSureYouWantToContinue" runat="server" />
										</td>
									</tr>
									<!-- Data Insert End Here -->
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
		</table>
	</div>
	<script src="../Scripts/jquery-1.7.min.js" type="text/javascript"></script>
	<script language="javascript" type="text/javascript">
		_clientcstbtnSave = "<%=this.btnSave.ClientID%>";
		_clientcstbtnClose = "<%=this.btnClose.ClientID%>";
		_clientcmpTeacherName = "<%=this.cmp_TeacherName.ClientID%>";
		_clientcmbAddTeacherId = "<%=this.cmbAddTeacherName.ClientID%>";
		_clientcmbTeacherId = "<%=this.cmbTeacherName.ClientID%>";
		_clientlblAddMandatory = "<%=this.lblAddMandatory.ClientID%>";
		_clientchkAddTeacher = "<%=this.chkAddTeacher.ClientID%>";
		_clientcstTeachers = "<%=this.cstTeachers.ClientID%>";
		_clienthidTeacherId = "<%=this.hidTeacherId.ClientID%>";
		_clienthidAddTeacherId = "<%=this.hidAddTeacherId.ClientID%>";
		_clientlblError = "<%=this.lblError.ClientID%>";
		
		/*Read JSON object*/
		_classTeacherList = eval('[' + document.getElementById('<%=this.hidClassTeacherJsonObject.ClientID%>').value + ']')[0];
		
		hideMandatoryLabel();

		function hideMandatoryLabel() {
			if (document.getElementById(_clientchkAddTeacher).checked)
				document.getElementById(_clientlblAddMandatory).style.display = "block";
			else
				document.getElementById(_clientlblAddMandatory).style.display = "none";
		}
		function ValidateTeachers(src, args) {
			if (document.getElementById(_clientchkAddTeacher).checked) {
				if (document.getElementById(_clientchkAddTeacher).disabled) {
					if (document.getElementById(_clientcmbAddTeacherId).value != '0' && document.getElementById(_clientcmbTeacherId).value != '0') {
						if (document.getElementById(_clientcmbAddTeacherId).value == document.getElementById(_clientcmbTeacherId).value) {
						    document.getElementById(_clientcstTeachers).errormessage = document.getElementById('<%=this.hidvalTeachersBoth.ClientID%>').value; 
							args.IsValid = false;
							return true;
						}
					}
				}
				else {
					if (document.getElementById(_clientcmbAddTeacherId).value != '0') {
						if (document.getElementById(_clientcmbAddTeacherId).value == document.getElementById(_clientcmbTeacherId).value) {
						    document.getElementById(_clientcstTeachers).errormessage = document.getElementById('<%=this.hidvalTeachersBoth.ClientID%>').value; 
							args.IsValid = false;
							return true;
						}
					}
					else {
					    document.getElementById(_clientcstTeachers).errormessage = document.getElementById('<%=this.hidValTeacherAdditional.ClientID%>').value; 
						args.IsValid = false;
						return true;
					}
				}
			}
			args.IsValid = true;
			return false;
		}
		function DisableAdditionalCombo() {
			if (document.getElementById(_clientchkAddTeacher).checked) {
				document.getElementById(_clientcmbAddTeacherId).disabled = false;
				document.getElementById(_clientlblAddMandatory).style.display = "block";
			}
			else {
				document.getElementById(_clientcmbAddTeacherId).disabled = true;
				document.getElementById(_clientcmbAddTeacherId).value = '0';
				document.getElementById(_clientlblAddMandatory).style.display = "none";
			}
		}
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
		function DisableButtons() {
			if (document.getElementById(_clientcmpTeacherName) == null && document.getElementById(_clientcstTeachers) == null) {
				document.getElementById(_clientcstbtnSave).disabled = true;
				document.getElementById(_clientcstbtnClose).disabled = true;
			}
			else {
				var isPageValid = true;
				if (typeof (Page_ClientValidate) == 'function')
					isPageValid = Page_ClientValidate();
				if (isPageValid) {
					document.getElementById(_clientcstbtnSave).disabled = true;
					document.getElementById(_clientcstbtnClose).disabled = true;
				}
			}
		}

		function closewindow() {
			document.getElementById(_clientcstbtnSave).disabled = true;
			document.getElementById(_clientcstbtnClose).disabled = true;
			window.close();
		}

		function ResetUpdateLbl() {
			if (document.getElementById(_clientlblError) != null) {
				document.getElementById(_clientlblError).style.display = "none";
			}
		}

		
		/*This function is used to show confirmation message while saving.*/
		function showMultipleClassTeacherMessage() {
			var teacherName = $("#" + _clientcmbTeacherId).find("option:selected").text();
			var teacherId = $("#" + _clientcmbTeacherId).find("option:selected").val();
			var result = true;
			
			if (!isMultipleClassTeacherId(_clientcmbTeacherId, $("#" + _clienthidTeacherId).val(), teacherId, teacherName))
				result = false;
			
			if ($("#" + _clientchkAddTeacher)[0].checked) {
				teacherName = $("#" + _clientcmbAddTeacherId).find("option:selected").text();
				teacherId = $("#" + _clientcmbAddTeacherId).find("option:selected").val();
				if (!isMultipleClassTeacherId(_clientcmbAddTeacherId, $("#" + _clienthidAddTeacherId).val(), teacherId, teacherName))
					result = false;
			}
			return result;
		}

		/*Find out that class teacher is class teacher of multiple classes or not*/
		var Page_IsValid = true;
		function isMultipleClassTeacherId(dropdownName, hiddenTeacherId, teacherId, teacherName) {
			 Page_IsValid = true;
			 for (var i = 0; i < _classTeacherList.length; i++) {
			     if (_classTeacherList[i].TeacherId == teacherId && $("#" + dropdownName).find("option:selected").val() != hiddenTeacherId) {
			         if (!window.confirm(teacherName + " " + document.getElementById('<%=this.hidValTeacherOf.ClientID%>').value + " " + _classTeacherList[i].ClassName + ". " + document.getElementById('<%=this.hidAreYouSureYouWantToContinue.ClientID%>').value)) {
			             Page_IsValid = false;
			             return false;
			         }
			     }
			 }
			return true;
		}
	</script>
</asp:Content>
