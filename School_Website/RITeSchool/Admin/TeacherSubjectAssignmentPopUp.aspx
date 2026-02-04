<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMasterSml.master"
	AutoEventWireup="true" CodeFile="TeacherSubjectAssignmentPopUp.aspx.cs" Inherits="TeacherSubjectAssignmentPopUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupSmlMainBody" runat="Server">
	<div class="MainBodyDiv">
		<div style="width: 100%; overflow: auto">
			<table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
				<tr>
					<td style="background-color: white;" id="MainDataTable" align="center" valign="top">
						<!-- Data Insert Here -->
						<table border="0" cellpadding="0" cellspacing="0" style="width: 98%;">
							<tr>
								<td align="left">
									<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
										<ContentTemplate>
											<table border="0" cellpadding="2" cellspacing="0" width="100%">
												<tr>
													<td align="left" rowspan="1" style="height: 5%">
														<table class="ClsGrayMainTitle" border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px">
															<tr>
																<td style="height: 20px">
																	<asp:Label ID="lblIndentDetails" runat="server" CssClass="MainTitleHead" Font-Bold="True"
																		Text="<%$ Resources:LocalizedResources, AssignTeacherToSubjects %>" EnableViewState="false"></asp:Label>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr id="Tr1">
													<td align="left" style="">
														<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="valGrpAddEduDetails"
															CssClass="ClsLabel" />
														<div style="float: right; vertical-align: top">
															<span class="ClsMdtStar">* 
                                                            <asp:Label ID="lblMandatoryText" runat="server"
																		Text="<%$ Resources:LocalizedResources, MandatoryFields %>" EnableViewState="false"></asp:Label>
                                                            </span>
														</div>
													</td>
												</tr>
												<tr style="height: 0">
													<td align="center">
														<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
															<ContentTemplate>
																<asp:Label ID="lblDuplicateDetails" runat="server" CssClass="ClsMdtStar" ForeColor="Red"></asp:Label>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="cmbTeacherName" EventName="SelectedIndexChanged" />
                                                                <asp:AsyncPostBackTrigger ControlID="grdDivisionWiseSubjects" EventName="RowCommand"/>
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td align="center">
														<table width="80%" cellpadding="2" cellspacing="1">
															<tr id="Tr2">
																<td align="left" class="ClsBorderlight" width="30%">
																	<span class="ClsLabel">
                                                                    <asp:Label ID="lblStandardText" runat="server" Text="<%$ Resources:LocalizedResources, StandardName %>"></asp:Label>
                                                                         <span class="colonPadding"> :</span>
																</td>
																<td align="left" class="ClsHilightBG" width="15%">
																	<asp:Label ID="lblStandardName" runat="server" CssClass="LblNrmlB" EnableViewState="true"></asp:Label>
																</td>
																<td>
																</td>
															</tr>
															<tr id="Tr9">
																<td align="left" class="ClsBorderlight" width="30%">
																	<span class="ClsLabel">
                                                                     <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, DivisionName %>"></asp:Label>
                                                                         <span class="colonPadding"> :</span>
                                                                    </span>
																</td>
																<td align="left" class="ClsHilightBG" width="43%">
																	<asp:Label ID="lblDivisionName" runat="server" CssClass="LblNrmlB" EnableViewState="true"></asp:Label>
																</td>
																<td>
																</td>
															</tr>
															<tr id="Tr3">
																<td align="left" class="ClsBorderlight" width="30%">
																	<span class="ClsLabel">
                                                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LocalizedResources, SubjectName %>"></asp:Label>
                                                                         <span class="colonPadding"> :</span>
																</td>
																<td align="left" class="ClsHilightBG" width="15%">
																	<asp:Label ID="lblSubjectName" runat="server" CssClass="LblNrmlB" EnableViewState="true"></asp:Label>
																</td>
																<td>
																</td>
															</tr>
														</table>
													</td>
												</tr>
												<tr>
													<td style="height: 5px">
													</td>
												</tr>
												<tr>
													<td align="center">
														<asp:UpdatePanel ID="UPanelInput" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
															<ContentTemplate>
																<table width="80%" cellpadding="2" cellspacing="1">
																	<tr id="Tr4">
																		<td align="left" class="ClsBorderlight" width="">
																			<span class="ClsLabel">
                                                                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, TeacherName %>"></asp:Label>
                                                                         <span class="colonPadding"> :</span>
                                                                            </span>
																		</td>
																		<td align="left">
																			<asp:DropDownList ID="cmbTeacherName" runat="server" CssClass="ExLrgCombo" OnSelectedIndexChanged="cmbTeacherName_SelectedIndexChanged"
																				AutoPostBack="True">
																			</asp:DropDownList>
																			<span class="ClsMdtStar">*</span>
																			<asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeacherName"
																				Display="None" ErrorMessage="<%$ Resources:LocalizedResources, ValTeacherName %>" Operator="NotEqual"
																				ValueToCompare="0" ValidationGroup="valGrpAddEduDetails"></asp:CompareValidator>
																		</td>
																		<td align="left" style="width: 23%">
																		</td>
																	</tr>
																	<tr>
																		<td align="left" class="ClsBorderlight" width="">
																			<span class="ClsLabel">
                                                                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources:LocalizedResources, IsExclusive %>"></asp:Label>
                                                                         <span class="colonPadding"> :</span>
                                                                            </span>
																		</td>
																		<td align="left">
																			<asp:CheckBox ID="chkIsExclusive" runat="server" />
																		</td>
																	</tr>
																	<tr>
																		<td>
																		</td>
																		<td align="center">
																			<asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="False">
																				<ContentTemplate>
																					<asp:Button ID="btnAddDetails" runat="server" Text="<%$ Resources:LocalizedResources, AddTeacher %>" CssClass="ClsBtnMid"
																						BorderStyle="Solid" ValidationGroup="valGrpAddEduDetails" BorderWidth="1px" OnClick="btnAddDetails_Click" />
																				</ContentTemplate>
																				<Triggers>
																					<asp:AsyncPostBackTrigger ControlID="cmbTeacherName" EventName="SelectedIndexChanged" />
																				</Triggers>
																			</asp:UpdatePanel>
																		</td>
																	</tr>
																	<tr>
																		<td align="left" colspan="3" style="height: 5px">
																		</td>
																	</tr>
																	<tr>
																		<td align="left" colspan="3" style="width: 100%">
																			<asp:GridView CssClass="GridBorder" ID="grdDivisionWiseSubjects" runat="server" Width="100%"
																				Height="90%" AutoGenerateColumns="False" PageSize="1000" CellPadding="0" CellSpacing="1"
																				ForeColor="#333333" GridLines="None" OnRowDataBound="grdDivisionWiseSubjects_RowDataBound"
																				DataKeyNames="Teacher_Subject_Id,SchoolWise_Standard_Division_Id,Subject_Id,Teacher_Id,IsExclusive"
																				OnRowCommand="grdDivisionWiseSubjects_RowCommand">
																				<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
																				</PagerStyle>
																				<PagerSettings NextPageText="Next" LastPageText="Last" PreviousPageText="Previous"
																					FirstPageText="First" Position="TopAndBottom" Mode="NumericFirstLast"></PagerSettings>
																				<Columns>
																					<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, IsExclusiveText %>" ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center">
																						<ItemTemplate>
																							<asp:Image ID="imgIsExclusive" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif"
																								runat="server" />
																							<asp:HiddenField ID="hidIsExclusive" runat="server" />
																						</ItemTemplate>
																						<HeaderStyle HorizontalAlign="Center" Width="80px" VerticalAlign="Middle" />
																						<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px" Wrap="True" />
																					</asp:TemplateField>
																					<asp:BoundField DataField="TeacherName" HeaderText="<%$ Resources:LocalizedResources, TeacherName %>" SortExpression="TeacherName">
																						<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																						<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																					</asp:BoundField>
																					<asp:ButtonField ButtonType="Image" CommandName="EDIT_ROW" HeaderText="<%$ Resources:LocalizedResources, Edit %>" ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif"
																						Text="<%$ Resources:LocalizedResources, Edit %>">
																						<ItemStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Middle" />
																						<HeaderStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Middle" />
																					</asp:ButtonField>
																					<asp:ButtonField ButtonType="Image" CommandName="DELETE_ROW" HeaderText="<%$ Resources:LocalizedResources, Delete %>"
																						ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Text="<%$ Resources:LocalizedResources, Delete %>">
																						<ItemStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Middle" />
																						<HeaderStyle HorizontalAlign="Center" Width="60px" VerticalAlign="Middle" />
																					</asp:ButtonField>
																				</Columns>
																				<RowStyle CssClass="ClsGridRow" />
																				<HeaderStyle CssClass="ClsGridHeader" />
																				<AlternatingRowStyle CssClass="ClsGridAltRow" />
																				<EmptyDataRowStyle BackColor="#E6EEFC" CssClass="LblNoRecord" />
																			</asp:GridView>
																		</td>
																	</tr>
																</table>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="cmbTeacherName" EventName="SelectedIndexChanged" />
																<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
																<asp:AsyncPostBackTrigger ControlID="grdDivisionWiseSubjects" EventName="RowCommand" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td>
														&nbsp;
													</td>
												</tr>
											</table>
										</ContentTemplate>
									</asp:UpdatePanel>
								</td>
							</tr>
							<tr>
								<td align="center">
									<asp:HiddenField ID="hidSelectedIndex" runat="server" />									
									<asp:HiddenField ID="hidDisplayMember" runat="server"></asp:HiddenField>									
									<asp:HiddenField ID="hidRowCnt" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hidStandard" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="hidCategoryId" runat="server" Value="0"></asp:HiddenField>
                                    <asp:HiddenField ID="hidName" runat="server" Value="0"></asp:HiddenField>
								</td>
							</tr>
							<tr>
								<td>
									&nbsp;
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align="center" id="tdSubmit" runat="server">
						&nbsp;
						<asp:Button ID="btnSubmit" runat="server" Width="110px" CausesValidation="true" Text="<%$ Resources:LocalizedResources, Submit %>" CssClass="ClsBtnSml"
							BorderStyle="Solid" ValidationGroup="Submit" UseSubmitBehavior="false" OnClick="btnSubmit_Click" />
						<asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close %>" CssClass="ClsBtn" CausesValidation="false"
							BorderStyle="Solid" UseSubmitBehavior="false" OnClick="btnClose_Click" />
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
				</tr>
				<tr>
					<td>
						&nbsp;
					</td>
				</tr>
			</table>
		</div>
        <asp:HiddenField ID="hidAtleastTeacherAssignedToSubject" runat="server" />
        <asp:HiddenField ID="hidAreYouSureDeleteDetails" runat="server" />
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidbtnAddDetails" runat="server" />
	</div>
	<script type="text/javascript" language="javascript">
		_clientbtnSubmit = "<%=this.btnSubmit.ClientID%>";
		_clientbtnClose = "<%=this.btnClose.ClientID%>";
		_clientgrdDivisionWiseSubjects = "<%=this.grdDivisionWiseSubjects.ClientID%>";
		_clienthidRowCnt = "<%=this.hidRowCnt.ClientID%>";
		_clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID%>";

		function fnover(varname) {
			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "maroon";
			objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
		}

		function fnout(varname) {
			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "#a3c07b";
			objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
		}
		//This function is used to display confirmation message at the time of assignment delete operation.
		function ConfirmDelete() {
			var bResult = true;
			if (!window.confirm(document.getElementById("<%=this.hidAreYouSureDeleteDetails.ClientID %>").value))
			{ bResult = false; }
			return bResult;
		}

		function CheckRecordCount() {
			var returnValue = true;
			var iRowCount = document.getElementById(_clienthidRowCnt).value;
			var sRow = ""
			var iCount = 0;
			var i = 1
			if (document.getElementById(_clientgrdDivisionWiseSubjects) != null) {
				var gridCount = document.getElementById(_clientgrdDivisionWiseSubjects).rows.length;
				if (gridCount == 0)
					returnValue = false;
			}
			else
				returnValue = false;
			if (returnValue == false) {
				if (document.getElementById(_clientvalSumErrorMsg) != null)
					document.getElementById(_clientvalSumErrorMsg).style.display = 'none'
				alert(document.getElementById("<%=this.hidAtleastTeacherAssignedToSubject.ClientID %>").value)
				return false;
			}
			return returnValue;
		}
	</script>
</asp:Content>
