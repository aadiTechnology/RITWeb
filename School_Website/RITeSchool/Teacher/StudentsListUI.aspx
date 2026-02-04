<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
	EnableEventValidation="false" CodeFile="StudentsListUI.aspx.cs" Inherits="StudentsListUI" ViewStateMode="Disabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
		<table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
			<tr>
				<td style="background-color: white;" id="MainDataTable" align="center" valign="top">
					<!-- Data Insert Here -->
					<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
						<tr>
							<td align="left" colspan="4">
								<table border="0" runat="server" id="tblHeader" viewstatemode="Enabled" cellpadding="0" cellspacing="0" width="100%">
									<tr>
										<td style="height: 24px" class="ClsGrayMainTitle">
											<table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
												<tr>
													<td align="center" class="MainTitleHead" style="height: 20px">
														<span style="font: Bold; border-width: 0px"></span>
                                                         <asp:Label Font-Bold= "true" BorderWidth = "0px" ID="lblTitle" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources,StudentList%>"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td align="right" valign="top">
											<span class="ClsMdtStar">*</span>
                                            <asp:Label  ID="lblMandatoryFields" CssClass="ClsMdtStar" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
										</td>
									</tr>
								</table>
								<asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
									<asp:UpdatePanel ID="upnlErrorMessage" runat="server">
										<ContentTemplate>
											<asp:Label ID="lblErrorMsg" runat="server" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
											<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled" CssClass="ClsLabel" ValidationGroup="Search" />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" viewstatemode="Enabled" Display="None" ControlToValidate="txtComment" ErrorMessage="Length of comment should not exceed 500 characters." CssClass="ClsMdtStar"
                                            ValidationExpression="^[\s\S]{0,500}$"></asp:RegularExpressionValidator>
										</ContentTemplate>
									</asp:UpdatePanel>
								</asp:Panel>
							</td>
						</tr>
						<tr>
							<td align="left">
								<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
									ID="UpdatePanel11">
									<ContentTemplate>
										<asp:Panel ID="Panel1" runat="server" Width="90%">
											<asp:Label ID="lblDuplicateMsg" runat="server" ViewStateMode="Enabled" CssClass="LblErrorMsg" EnableViewState="False"></asp:Label>
										</asp:Panel>
									</ContentTemplate>
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="btnDeleteStud" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
										<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
										<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="RowCommand" />
									</Triggers>
								</asp:UpdatePanel>
							</td>
						</tr>
						<tr>
							<td align="left">
								<table id="LegendTable" runat="server">
									<tr>
										<td align="left">
											 <asp:Label CssClass = "ClsLblLgnd" ID="lblLegend" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
										</td>
										<td align="left" style="padding-right: 3px">
											<asp:Label ID="TextBox1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
												BackColor="Gainsboro" Height="20px" ReadOnly="True" Text=" " Width="20px" EnableViewState="False"></asp:Label>
										</td>
										<td align="left">
											<asp:Label CssClass = "ClsTextNormal" ID="lblDeactivatedUser" Font-Bold = "true" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, DeactivatedUser%>"></asp:Label>
										</td>
										<td align="left" style="padding-right: 3px">
											<asp:Label ID="Label1" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
												BackColor="LightSteelBlue" Height="20px" ReadOnly="True" Text=" " Width="20px"
												EnableViewState="False"></asp:Label>
										</td>
										<td align="left">
											<asp:Label CssClass = "ClsTextNormal" ID="lblLongLeave" Font-Bold = "true" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, LongLeave%>"></asp:Label>
										</td>
										<td align="right" style="width: 5px">
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td align="center" id="tblSearch" runat="server" viewstatemode="Enabled" >
								<table cellpadding="0" cellspacing="2">
									<tr id="trMsg" align="center" runat="server" visible="false" viewstatemode="Enabled" >
										<td align="center" colspan="5" class="ClsLabelUpdate" style="height: 30px;">
											<asp:Label ID="lblMsg" runat="server" ViewStateMode="Enabled" Text=""></asp:Label>
										</td>
									</tr>
									<tr id="trCombo">
										<td align="left" class="ClsBorderlight" colspan="1" width="80px">
									 <asp:Label CssClass = "ClsLabel" ID="lblStandard" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Standard%>"></asp:Label>
                                      <span class="ClsLabel colonPadding">:</span>
										</td>
										<td align="left" colspan="1" width="135px">
											<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server" 
												ID="UpdatePanel12">
												<ContentTemplate>
													<asp:DropDownList ID="cmbStandard" Width="121px" AutoPostBack="true" OnSelectedIndexChanged="cmbStd_SelectedIndexChanged"
														runat="server" ViewStateMode="Enabled" CssClass="SmlCombo" Height="19px">
													</asp:DropDownList>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td align="left" class="ClsBorderlight" colspan="1">
									<asp:Label CssClass = "ClsLabel" ID="lblDivision" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Division%>"></asp:Label>
                                      <span class="ClsLabel colonPadding">:</span>
										</td>
										<td align="left">
											<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
												ID="uPnl">
												<ContentTemplate>
													<asp:DropDownList ID="cmbDivision" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" Width="95px"
														AutoPostBack="true" OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged">
													</asp:DropDownList>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td>
										</td>
									</tr>
									<tr>
										<td align="left" class="ClsBorderlight" colspan="1" width="80px">
									  <asp:Label CssClass = "ClsLabel" ID="lblClass" runat="server" EnableViewState="False" 
                                                Text="<%$ Resources:LocalizedResources, Class %>" Height="16px" Width="40px"></asp:Label>
                                      <span class="ClsLabel colonPadding">:</span>
										</td>
										<td align="left">
											<asp:UpdatePanel ID="upnl1" runat="server" UpdateMode="Always">
												<ContentTemplate>
													<asp:DropDownList ID="cmbClass" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" Width="121px"
														AutoPostBack="true" OnSelectedIndexChanged="cmbClass_SelectedIndexChanged" Height="19px">
													</asp:DropDownList>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr id="Tr1" runat="server">
										<td align="left" class="ClsBorderlight" colspan="1">
											<asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel10">
												<ContentTemplate>
													<asp:RadioButton ID="optMain" runat="server" ViewStateMode="Enabled" onclick="SetControlsUponCriteria()"
														GroupName="Search" AutoPostBack="false" />
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
													<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td align="left" class="ClsBorderlight" colspan="1">
											
                                      <asp:Label CssClass = "ClsLabel" ID="lblNameorReg" runat="server" Width = "120px" EnableViewState="False" Text="<%$ Resources:LocalizedResources, NameOrReg%>"></asp:Label>
                                      <span class="ClsLabel colonPadding">:</span>
										</td>
										<td align="Center" class="ClsBorderlight">
                                             <asp:Label  ID="lblLike" runat="server" Font-Bold= "true" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Like%>"></asp:Label>
                                      

										</td>
										<td colspan="2" align="left">
											<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel7">
												<ContentTemplate>
													<asp:TextBox ID="txtName" runat="server" ViewStateMode="Enabled" CssClass="ClsTxtLarge" Width="150px" MaxLength="50"></asp:TextBox>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
													<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td align="left" colspan="1" rowspan="1" valign="middle">
											&nbsp;<asp:Button ID="btnSearch" CausesValidation="true" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
												ValidationGroup="Search" OnClick="btnSearch_Click" Text= "<%$ Resources:LocalizedResources, Search%>"/>
										</td>
									</tr>
									<tr id="Tr2" runat="server">
										<td align="left" class="ClsBorderlight" colspan="1">
											<asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel9">
												<ContentTemplate>
													<asp:RadioButton ID="optExact" runat="server" ViewStateMode="Enabled" GroupName="Search" onclick="SetControlsUponCriteria()"
														AutoPostBack="false" />
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
													<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td align="left" class="ClsBorderlight" colspan="1">
											
                                             <asp:Label CssClass = "ClsLabel" ID="lblRegNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, RegistrationNumber%>"></asp:Label>
                                      <span class="ClsLabel colonPadding">:</span>
											<asp:UpdatePanel ID="upnlOperation" runat="server" UpdateMode="Always">
												<ContentTemplate>
													<asp:DropDownList ID="cmbOperation" runat="server" ViewStateMode="Enabled" CssClass="SmlCombo"
														Height="19px">
													</asp:DropDownList>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td align="left" width="80px">
											<asp:UpdatePanel ID="upnlPrefix" runat="server" UpdateMode="Always">
												<ContentTemplate>
													<asp:DropDownList ID="cmbPrefix" runat="server" ViewStateMode="Enabled" CssClass="SmlCombo" Style="width: 100px">
													</asp:DropDownList>
												</ContentTemplate>
											</asp:UpdatePanel>
										</td>
										<td colspan="2" align="left">
											<asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel8">
												<ContentTemplate>
													<asp:TextBox ID="txtReg" runat="server" ViewStateMode="Enabled" CssClass="ClsTxtLarge" Width="150px" MaxLength="15"
														onblur="extractNumber(this,0,false);" CausesValidation="true" onkeyup="extractNumber(this,0,false);"
														onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false;"
														ondrop="event.returnValue=false;"></asp:TextBox>
													<span class="ClsMdtStar">*</span>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
													<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td align="center" runat="server" colspan="4" width="100%">
								<table cellpadding="0" cellspacing="2">
									<tr align="center">
										<td align="right" colspan="1" rowspan="1" valign="top" style="padding-left: 25px">
											&nbsp;<asp:Button ID="btnExport" CausesValidation="true" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
												ValidationGroup="Search" OnClick="btnExport_Click" OnClientClick="ConfirmIncludeLeftStudentExport()" Text="<%$ Resources:LocalizedResources, ExportStudent%>" Width="100px" />
										</td>
										<td align="center" colspan="1" rowspan="1" valign="top">
											<asp:Button ID="btnUpload" CausesValidation="true" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
												ValidationGroup="Search"  Text="<%$ Resources:LocalizedResources, StudentPhotos%>" Width="120px" OnClick="btnUpload_Click" />
										</td>
										<td align="left" colspan="1" rowspan="1" valign="top" class="style2">
											<asp:Button ID="btnUpdateRegNo" CausesValidation="true" runat="server" ViewStateMode="Enabled" CssClass="ClsBtn"
												ValidationGroup="Search" Text="<%$ Resources:LocalizedResources, UpdateRegNo%>" Width="120px" OnClick="btnUpdateRegNo_Click" />
										</td>
										<td valign="middle" align="left" style="width: 85px">
											<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel6">
												<ContentTemplate>
													<div id="div1" runat="server" viewstatemode="Enabled" align="center" class="ToprLinkHlilight" style="width: 80px;
														height: 18px; float: right">
														<asp:HyperLink ID="hlnkPhotos" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightTextB" Enabled="False" Text ="<%$ Resources:LocalizedResources, Photo%>" 
															Target="_blank" NavigateUrl="~/RITeSchool/Student/ExamToppersUI.aspx"></asp:HyperLink>
													</div>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td valign="middle" align="right" style="width: 125px" visible="false">
											<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel5">
												<ContentTemplate>
													<div id="divIdentity" runat="server" align="center" class="ToprLinkHlilight" style="width: 120px;
														height: 18px; float: right" visible="false">
														<asp:HyperLink ID="hlnkIdentity" runat="server" ViewStateMode="Enabled" CssClass="ClsHilightTextB" Enabled="False" Text = "<%$ Resources:LocalizedResources, Identitycards%>"
															NavigateUrl="~/RITeSchool/Student/ExamToppersUI.aspx" Target="_blank"></asp:HyperLink>
													</div>
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
									</tr>
									<tr>
										<td>
											<asp:CustomValidator ID="cstvalRegNo" runat="server" ViewStateMode="Enabled" ClientValidationFunction="ValidateRegNo" 
												ValidationGroup="Search" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, RegNumberBlank%>"
												SetFocusOnError="True"> </asp:CustomValidator>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="4" align="center" valign="top" class="ClspaddingT">
								<asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="UpdatePanel2">
									<ContentTemplate>
										<table width="100%" cellpadding="0" cellspacing="2">
											<tr>
												<td>
													<table width="20%" id="tblClass" runat="server" viewstatemode="Enabled" >
														<tr>
															<td align="left" width="50px" id="tdStandardDivisionLabel" runat="server">
																
                                                                     <asp:Label ID="lblCla" Font-Bold = "true" Font-Size = "9pt" ForeColor = "#006666" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                      <span class="colonPadding">:</span>

															</td>
															<td>
																<asp:DropDownList ID="ddlClassTeacher" runat="server" ViewStateMode="Enabled" CssClass="SmlTxtBox" Width="121px"
																	AutoPostBack="true"  Height="19px" onselectedindexchanged="ddlClassTeacher_SelectedIndexChanged" >
																</asp:DropDownList>
															</td>
														</tr>
													</table>
													<table width="100%">
														<tr>
															<td id="tdTotalRec" runat="server" viewstatemode="Enabled" align="center">
																<asp:Label ID="lblStartIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
																<asp:Label ID="lblTo" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, To%>"></asp:Label>
																<asp:Label ID="lblEndIndex" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
																<asp:Label ID="lblOutOf" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, OutOf%>"></asp:Label>
																<asp:Label ID="lblTotal" runat="server" CssClass="LblNrmlB" ViewStateMode="Enabled" />
																<asp:Label ID="lblRecords" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Records%>"></asp:Label>
															</td>
														</tr>
													</table>
												</td>
											</tr>
											<tr runat="server" align="center">
												<td>
													<asp:GridView CssClass="GridBorder" ID="grdStudents" runat="server" 
                                                        ViewStateMode="Enabled" AllowPaging="True"
														DataKeyNames="Is_Leave,Photo_file_Path,SchoolWise_Student_Id,SchoolLeft_Date,Standard_Id,Division_id,Joining_Date,IsAttendanceAvailable,Is_Locked,CancellationFormNo,StudentIsOnLeave,Photo_file_Path_Image,Admission_Date"
														AutoGenerateColumns="False" AllowSorting="True" OnRowCreated="grdStudents_RowCreated"
														OnRowDataBound="grdStudents_RowDatabound" Width="100%" PageSize="20" CellPadding="0"
														CellSpacing="1" ForeColor="#333333" GridLines="None" OnPageIndexChanging="grdStudents_PageIndexChanging"
														OnDataBound="grdStudents_DataBound" OnRowCommand="grdStudents_RowCommand" >
                                                       
														<PagerStyle ForeColor="Black" HorizontalAlign="Right" Font-Bold="True" Font-Underline="False">
														</PagerStyle>
														<Columns>
															<asp:BoundField DataField="Enrolment_Number" HeaderText="<%$ Resources:LocalizedResources, RegistrationNumber%>" SortExpression="Enrolment_Number">
																<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
																	Wrap="False" />
															</asp:BoundField>
															<asp:BoundField DataField="Roll_No" HeaderText="<%$ Resources:LocalizedResources, RollNo%>" SortExpression="Roll_No">
																<ItemStyle Width="70px" HorizontalAlign="Right" VerticalAlign="Middle" CssClass="ClspaddingR" />
																<HeaderStyle Width="70px" HorizontalAlign="Right" VerticalAlign="Middle" CssClass="ClspaddingR"
																	Wrap="False" />
															</asp:BoundField>
															<asp:BoundField DataField="StandardDivision" HeaderText="<%$ Resources:LocalizedResources, Class%>" SortExpression="StandardDivision">
																<ItemStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																<HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Wrap="False" CssClass="ClspaddingL" />
															</asp:BoundField>
															<asp:BoundField DataField="Name" HeaderText="<%$ Resources:LocalizedResources, StudentName%>" SortExpression="First_Name">
																<ItemStyle Width="30%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL" />
																<HeaderStyle Width="30%" HorizontalAlign="Left" VerticalAlign="Middle" CssClass="ClspaddingL"
																	Wrap="False" />
															</asp:BoundField>
															<asp:BoundField DataField="DOB" HeaderText="<%$ Resources:LocalizedResources, DateOfBirth%>" SortExpression="DOB">
																<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" />
																<HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" CssClass="" Wrap="False" />
															</asp:BoundField>
															<asp:HyperLinkField DataNavigateUrlFields="SchoolWise_Student_Id,Name,StandardDivision,Enrolment_Number"
																HeaderText="<%$ Resources:LocalizedResources, Edit%>" DataNavigateUrlFormatString="../Teacher/StudentUI.aspx?StudentId={0}&amp;StudentName={1}&amp;ClassName={2}&amp;RegNo={3}"
																Text="Edit Basic Details">
																<ItemStyle HorizontalAlign="Center" Wrap="true" Width="90px" />
															</asp:HyperLinkField>
															<asp:TemplateField HeaderText="<%$ Resources:LocalizedResources, Photo%>">
																<ItemTemplate>
																	<asp:Image ID="imgPhoto" runat="server" ViewStateMode="Enabled" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
																</ItemTemplate>
																<ItemStyle Width="30px" HorizontalAlign="Center" />
																<HeaderStyle Width="30px" HorizontalAlign="Center" />
															</asp:TemplateField>
															<asp:ButtonField ButtonType="Image" CommandName="DELETE_STUDENT" HeaderText="<%$ Resources:LocalizedResources, Delete%>"
																ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" Text="Delete">
																<ItemStyle HorizontalAlign="Center" />
															</asp:ButtonField>
															<asp:HyperLinkField DataNavigateUrlFields="SchoolWise_Student_Id,Name,StandardDivision,Enrolment_Number"
																HeaderText="<%$ Resources:LocalizedResources, LCAddOrEdit%>" DataNavigateUrlFormatString="../Teacher/LeavingCertificateUI.aspx?StudentId={0}&amp;StudentName={1}&amp;ClassName={2}&amp;RegNo={3}"
																Text="Add">
																<ItemStyle HorizontalAlign="Center" Width="90px" />
															</asp:HyperLinkField>
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
																		<asp:Label ID="lblRecords" CssClass = "LblNormal"  runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SelectPage%>"></asp:Label>
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
												</td>
											</tr>
										</table>
										<asp:ObjectDataSource TypeName="BusinessLogic.StudentBL" EnablePaging="true" ID="GrdDSobj"
											runat="server" ViewStateMode="Enabled" SelectMethod="GetAllStudents" SortParameterName="sortExpression"
											SelectCountMethod="CountRows" EnableCaching="false" OnSelected="GrdDSobj_Selected">
											<SelectParameters>
												<asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
												<asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
													Type="string" />
												<asp:ControlParameter ControlID="hidStandardId" PropertyName="Value" Name="aiStandardId"
													DefaultValue="0" />
												<asp:ControlParameter ControlID="hidDivisionId" PropertyName="Value" Name="aiDivisionId"
													DefaultValue="0" />
												<asp:ControlParameter ControlID="hidStandardDivisionId" PropertyName="Value" Name="aiStandardDivisionId"
													DefaultValue="0" />
												<asp:ControlParameter ControlID="hidStudentName" PropertyName="Value" Name="asName" />
												<asp:ControlParameter ControlID="hidStudentReg" PropertyName="Value" Name="asRegNo" />
												<asp:ControlParameter ControlID="hidIsExactMatch" PropertyName="Value" Name="abIsExactMatch" />
												<asp:ControlParameter ControlID="cmbOperation" PropertyName="SelectedValue" Name="asOperator" />
												<asp:ControlParameter ControlID="cmbPrefix" PropertyName="SelectedValue" Name="asPrefix" />
											</SelectParameters>
										</asp:ObjectDataSource>
									</ContentTemplate>
									<Triggers>
										<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="btnDeleteStud" EventName="Click" />
										<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
										<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
										<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
										<asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="RowCommand" />
										<asp:AsyncPostBackTrigger ControlID="ddlClassTeacher" EventName="SelectedIndexChanged" />
									</Triggers>
								</asp:UpdatePanel>
							</td>
                            
						</tr>
						<tr>
							<td align="center" colspan="1" id="tdBack" runat="server" viewstatemode="Enabled" >
								<table>
									<tr>
										<td>
											<asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
												ID="UpdatePanel3">
												<ContentTemplate>
													<asp:Button ID="btnAdd" CausesValidation="false" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Add%>" CssClass="ClsBtn"
														OnClick="btnAdd_Click" />
												</ContentTemplate>
												<Triggers>
													<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
													<asp:AsyncPostBackTrigger ControlID="ddlClassTeacher" EventName="SelectedIndexChanged" />
												</Triggers>
											</asp:UpdatePanel>
										</td>
										<td>
											<asp:Button ID="btnBack" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Close%>" CssClass="ClsBtn" CausesValidation="false"
												OnClick="btnBack_Click" />
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<div id="updtpnlPopUp" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none; position: absolute;
						margin: 0px; padding: 0px; width: 240px; height: 275px; border-width: 0px; left: 0px;
						top: 0px; line-height: normal; width: auto; border: solid 1px black; margin: 0px 0px 0px 20px;
						background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
						<div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
							background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
							<div style="padding: 1px; font-size: 12px; font-weight: bold; color: #Black; float: left;">
                            <asp:Label ID="lblPopUpHeader" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PopUpSchoolLeavingDate %>"></asp:Label>
						</div>
							<span style="cursor: hand" onclick="javascript:HidePopup(true);">
								<img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
							</span>
						</div>
						<div style="padding: 10px; text-align: left;" class="ClsLabel">
							<asp:UpdatePanel ChildrenAsTriggers="True" UpdateMode="Conditional" runat="server"
								ID="UpdatePanel4">
								<ContentTemplate>
									<table>
										<tr>
											<td colspan="2">
												<asp:RadioButton GroupName="Studentdelete" ID="chkSchoolLeaving" Checked="true" runat="server" ViewStateMode="Enabled" 
													CausesValidation="false" AutoPostBack="true" OnCheckedChanged="chkSchoolLeaving_CheckedChanged"
													Text= "<%$ Resources:LocalizedResources, SchoolLeaving%>" CssClass="LblNormal" />
											</td>
										</tr>
										<tr>
                                            <td align="left">                                                
                                                <asp:Label CssClass = "LblNormal" ID="Label2" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolLeavingDate %>"></asp:Label>
													<span class="LblNormal">:</span>
                                            </td>
											<td>
												<asp:TextBox ID="txtDeletedDate" CssClass="SmlCombo" runat="server" ViewStateMode="Enabled" AutoPostBack  = "true" ></asp:TextBox>
												<rjs:PopCalendar ID="caltxtDeletedDate" runat="server" ViewStateMode="Enabled" 
                                                    Control="txtDeletedDate" ShowErrorMessage="false" Culture = "en"
													Format="dd MMM yyyy" ShowWeekend="True" To-Date="" />
												<span style="color: #ff0000">*</span>
											</td>
										</tr>
										<tr>
                                            <td align="left">
                                                <asp:Label CssClass = "LblNormal" ID="lblCancellationFormNo" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, CancellationFormNo%>"></asp:Label>
												<span class="LblNormal">:</span>
                                            </td>
											<td>
												<asp:TextBox ID="txtCancFormNo" runat="server" viewstatemode="Enabled" CssClass="SmlTxtBox" MaxLength="10"
													onblur="extractNumber(this,2,false);" ondrop="event.returnValue=false" onkeypress="return blockNonNumbers(this, event, false, false);"
													onkeyup="extractNumber(this,2,false);" onpaste="event.returnValue=false"></asp:TextBox>
											</td>
										</tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label CssClass = "LblNormal" ID="lblIncludeinBlackList" runat="server" EnableViewState="False" Text="Include in Blacklist?"></asp:Label>
												    <span class="LblNormal">:</span>
                                            </td>
                                            <td>
                                                <asp:CheckBox AutoPostBack="True" ID="chkIncludeinBlackList" runat="server" Visible="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Label CssClass = "LblNormal" ID="lblComment" runat="server" EnableViewState="False" Text="Comment"></asp:Label>
												    <span class="LblNormal">:</span>
                                            </td>
                                            <td> 
                                                <asp:TextBox ID="txtComment" runat="server" viewstatemode="Enabled" CssClass="LrgTxtBox" TextMode="MultiLine"></asp:TextBox>
                                            </td>
                                        </tr>
										<tr>
											<td colspan="2">
												<asp:RadioButton GroupName="Studentdelete" ID="chkcompleteDelete" runat="server" ViewStateMode="Enabled" Visible = "false"
													CausesValidation="false" AutoPostBack="true" OnCheckedChanged="chkcompleteDelete_CheckedChanged"
													Text= "<%$ Resources:LocalizedResources, CompleteDelete%>" CssClass="LblNormal"  />
											</td>
										</tr>
										<tr>
											<td colspan="2" align="center">
												<asp:Button ID="btnDeleteStud" runat="server" ViewStateMode="Enabled" Text= "<%$ Resources:LocalizedResources, OK%>" CssClass="ClsBtn" OnClick="btnDelete_Click" 
													OnClientClick="if(!ConfirmDelete('Leave')){return false;}" />
												<asp:Button ID="btnCancel" runat="server" ViewStateMode="Enabled" Text= "<%$ Resources:LocalizedResources, Cancel%>" CssClass="ClsBtn" CausesValidation="false"
													OnClientClick="javascript:HidePopup(true);return false;" />
											</td>
										</tr>
									</table>
								</ContentTemplate>
								<Triggers>
									<asp:AsyncPostBackTrigger ControlID="chkSchoolLeaving" EventName="CheckedChanged" />
									<asp:AsyncPostBackTrigger ControlID="chkcompleteDelete" EventName="CheckedChanged" />
									<asp:AsyncPostBackTrigger ControlID="ddlClassTeacher" EventName="SelectedIndexChanged" />
								</Triggers>
							</asp:UpdatePanel>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
			ID="UpdatePanel1">
			<ContentTemplate>
				<asp:HiddenField ID="hidSortDirection" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidSortExpression" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStandardId" runat="server" Value="0" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidDivisionId" runat="server" Value="0" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStandardDivisionId" runat="server" Value="0" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidLastSelectedDiv" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidLastSelectedStd" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidLastSelectedClass" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStudentId" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStudJoiningDate" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidAcademicStartDate" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidAcademicEndDate" runat="server" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidCanEdit" runat="server" Value="Y" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidIsExactMatch" runat="server" Value="False" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStudentName" runat="server" Value="" ViewStateMode="Enabled" />
				<asp:HiddenField ID="hidStudentReg" runat="server" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidStudAdmissionDate" runat="server" Value="" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidLockBlacklistOption" runat="server" Value="N" />
			</ContentTemplate>
			<Triggers>
				<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
				<asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
				<asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
				<asp:AsyncPostBackTrigger ControlID="cmbClass" EventName="SelectedIndexChanged" />
				<asp:AsyncPostBackTrigger ControlID="grdStudents" EventName="Sorting" />
				<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
				<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
				<asp:AsyncPostBackTrigger ControlID="ddlClassTeacher" EventName="SelectedIndexChanged" />
			</Triggers>
		</asp:UpdatePanel>
		<asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidAcademicYearId" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidIsConfig" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidIsAdmin" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidIsSuperAdmin" runat="server" Value="N" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidOperator" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidPrefix" runat="server" ViewStateMode="Enabled" />
         <asp:HiddenField ID="hidPostfix" runat="server" ViewStateMode="Enabled" />
		<asp:HiddenField ID="hidSearchedNumber" runat="server" ViewStateMode="Enabled" />

        <asp:HiddenField ID="hidValSchoolLeavingDate" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidValSchoolLeavingDateForAdmission" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidValSchoolLeavingFutureDate" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidValSchoolLeavingDateBlank" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID = "hidCultureInfo" runat = "server" viewstatemode="Enabled" />

         <asp:HiddenField ID="hidValLeavingDateOutSide" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidValStudentLeaving" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidStudentDelete" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidDeleteStudent" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidIncludeLeft" runat ="server" Value = "1" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidAllowFutureDate" runat ="server" Value = "0" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidIsMidYear" runat ="server" Value = "0" viewstatemode="Enabled" />

	</div>
	<style type="text/css">
		.ClsTxtLarge
		{
			---x-system-font: none;
			background-color: #FFFFFF;
			border: 1px solid gray;
			font-family: arial;
			font-size: 9pt;
			font-size-adjust: none;
			font-stretch: normal;
			font-style: normal;
			font-variant: normal;
			font-weight: normal;
			line-height: normal;
			padding: 1px;
			width: 260px;
		}
	</style>
	<script language="javascript" type="text/javascript">
		_sClientAcademicYearId = "<%=this.hidAcademicYearId.ClientID %>";
		_sClientGridId = "<%=this.grdStudents.ClientID %>";
		_sClienthidSchoolId = "<%=this.hidSchoolId.ClientID %>";
		_sClientbtnAdd = "<%=this.btnAdd.ClientID %>";
		_sClienthidDivisionId = "<%=this.hidDivisionId.ClientID %>";
		_sClienthidStandardId = "<%=this.hidStandardId.ClientID %>";
		_clienthidAcademicStartDate = "<%=this.hidAcademicStartDate.ClientID %>";
		_clienthidAcademicEndDate = "<%=this.hidAcademicEndDate.ClientID %>";
		
		_clienttxtDeletedDate = "<%=this.txtDeletedDate.ClientID %>";
		_clienChkcompleteDelete = "<%=this.chkcompleteDelete.ClientID %>";
		_clienhidIsAdmin = "<%=this.hidIsAdmin.ClientID %>";
		_clientcmbStandard = "<%=this.cmbStandard.ClientID %>"
		_clientcmbDivision = "<%=this.cmbDivision.ClientID %>"
		_clientcmbClass = "<%=this.cmbClass.ClientID %>";
		_clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
		_clienthidIsSuperAdmin = "<%=this.hidIsSuperAdmin.ClientID %>";
		_clienttxtReg = "<%=this.txtReg.ClientID %>";
		_clientcstvalRegNo = "<%=this.cstvalRegNo.ClientID %>";
		_clientcmbOperation = "<%=this.cmbOperation.ClientID %>";
		_clientoptExact = "<%=this.optExact.ClientID %>";
		_clientcmbPrefix = "<%=this.cmbPrefix.ClientID %>";
		_clienttxtName = "<%=this.txtName.ClientID %>";
		_clientoptMain = "<%=this.optMain.ClientID %>";
		_clienthidIsExactMatch = "<%=this.hidIsExactMatch.ClientID %>";
		_clienthidStudentReg = "<%=this.hidStudentReg.ClientID %>";
		_clienthidStudentName = "<%=this.hidStudentName.ClientID %>";
		_clienthidIncludeLeft = "<%=this.hidIncludeLeft.ClientID %>";
		_clienthidhidStudAdmissionDate = "<%=this.hidStudAdmissionDate.ClientID %>";
		_clienthidhidIsMidYear = "<%=this.hidIsMidYear.ClientID %>";

		_ClienthidLockBlacklistOption = "<%=this.hidLockBlacklistOption.ClientID %>"
		_clientchkIncludeinBlackList = "<%=this.chkIncludeinBlackList.ClientID %>"

		var prm = Sys.WebForms.PageRequestManager.getInstance();
		prm.add_beginRequest(BeginRequestHandler);
		prm.add_endRequest(EndRequestHandler);

		function BeginRequestHandler() {

		}

		// This function is used to enabled controls once a postback is complete.
		function EndRequestHandler() {
			AutoSearch();
		}

		function ValidateInput(aObj) {

			if (document.getElementById(_clientcmbStandard) == aObj && document.getElementById(_clientcmbStandard).value == "0") {
				document.getElementById(_clientcmbClass).value = "0"
				document.getElementById(_clientcmbDivision).value = "0"
			}
			if (document.getElementById(_clientcmbStandard) == aObj && document.getElementById(_clientcmbStandard).value != "0") {
				document.getElementById(_clientcmbClass).value = "0"
			}
			if (document.getElementById(_clientcmbDivision) == aObj && document.getElementById(_clientcmbDivision).value != "0") {
				document.getElementById(_clientcmbClass).value = "0"
			}
		}

		var IsAttendanceAvailable = "N"

		var EventElement;
		var posx; var posy;
		function getMouse(e) {
			posx = 0; posy = 0;
			var ev = (!e) ? window.event : e; //IE:Moz
			if (ev.pageX) {//Moz
				posx = ev.pageX + window.pageXOffset;
				posy = ev.pageY + window.pageYOffset;
			}
			else if (ev.clientX) {//IE
				posx = ev.clientX + document.body.scrollLeft;
				posy = ev.clientY + document.body.scrollTop;
			}
			else
				return false //old browsers            
		}
		document.onmousemove = getMouse

		function tt_GetScrollX() {
			return (window.pageXOffset || (tt_db ? (tt_db.scrollLeft || 0) : 0));
		}
		function tt_GetScrollY() {
			return (window.pageYOffset || (tt_db ? (tt_db.scrollTop || 0) : 0));
		}
		function tt_GetClientW() {
			var de = document.documentElement;
			return ((de && de.clientWidth) ? de.clientWidth : (document.body.clientWidth || window.innerWidth || 0));
		}
		function tt_GetClientH() {
			var de = document.documentElement;
			return ((de && de.clientHeight) ? de.clientHeight : (document.body.clientHeight || window.innerHeight || 0));
		}

		function tt_GetEvtX(e) {
			return (e ? ((typeof (e.pageX) != tt_u) ? e.pageX : (e.clientX + tt_scrlX)) : 0);
		}
		function tt_GetEvtY(e) {
			return (e ? ((typeof (e.pageY) != tt_u) ? e.pageY : (e.clientY + tt_scrlY)) : 0);
		}

		// This method is used to display popup on click of delete button.
		function ShowPopup(e, iStudentId, Joiningdate, Admissiondate, IsAttendanceAvail) {
		    $get("<%=this.hidStudentId.ClientID %>").value = iStudentId;
		    $get("<%=this.hidStudJoiningDate.ClientID %>").value = Joiningdate;
		    $get("<%=this.hidStudAdmissionDate.ClientID %>").value = Admissiondate;
		    var now = new Date();
		    $get("<%=this.txtDeletedDate.ClientID %>").value = now.format("dd-MMM-yyyy");

		    var x, y, tt_ovr_;
		    var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style;
		    var btnDelete = $get("<%=this.btnDeleteStud.ClientID %>");
		    var width = 240;
		    var height = 150;
		    var left = parseInt((screen.width / 2) - (width / 2) - 300);
		    var top = parseInt((screen.height / 2) - (height / 2));

		    cssstyle.left = left + "px";
		    cssstyle.top = top + "px";

		    IsAttendanceAvailable = IsAttendanceAvail

		    // Override the z-index of the topmost wz_dragdrop.js D&D item
		    cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010);
		    cssstyle.visibility = "visible";
		    cssstyle.display = "block";

		    var LockBlacklistOption = document.getElementById(_ClienthidLockBlacklistOption).value;

		    if (LockBlacklistOption == "Y") {
		        $get(_clientchkIncludeinBlackList).checked = true;
		        $get(_clientchkIncludeinBlackList).disabled = true;
		    }
		    else {
		        $get(_clientchkIncludeinBlackList).checked = false
		        $get(_clientchkIncludeinBlackList).disabled = false
		    }
		}

		// This function is used to hide popup on click of delete button.
		function HidePopup(IsCancelled) {
			$get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden";
			$get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none";
			var now = new Date();

			if ($get("<%=this.chkcompleteDelete.ClientID %>").checked)
				$get("<%=this.txtDeletedDate.ClientID %>").value = now.format("dd-MMM-yyyy");

			var dt;
			if (IsCancelled)
				dt = now;
			else
				dt = $get("<%=this.txtDeletedDate.ClientID %>").value;

			if (!CheckIfDateInAcademicYear(dt))
				dt = new Date(document.getElementById(_clienthidAcademicEndDate).value);

			if (IsCancelled)
				dt = dt.format("dd-MMM-yyyy");

			if ($get("<%=this.chkSchoolLeaving.ClientID %>").checked)
				$get("<%=this.txtDeletedDate.ClientID %>").value = dt

			var validationResult = true;
			if (typeof (Page_ClientValidate) == 'function')
				validationResult = Page_ClientValidate("");

			if (validationResult == false)
				return false;
			return false;
		}

		// This function is used to validate school leaving date.
		function IsValidLeavingDate() { 
		    var msg = "";
		    var now = new Date();
		    if (document.getElementById(_clienChkcompleteDelete) != null && document.getElementById(_clienChkcompleteDelete).checked) {
		        $get("<%=this.txtDeletedDate.ClientID %>").value = now.format("dd-MMM-yyyy");
		    } else {
		        var dtDeletedDate = document.getElementById(_clienttxtDeletedDate).value;
		        if (dtDeletedDate.length > 0) {
		            dtDeletedDate = dtDeletedDate.replace('-', ' ').replace('-', ' ')
		            var DeletedDate = new Date(dtDeletedDate);                   
		            var dtStudJoiningDate = new Date($get("<%=this.hidStudJoiningDate.ClientID %>").value);
		            var dtStudAdmissionDate = new Date($get("<%=this.hidStudAdmissionDate.ClientID %>").value);
                    var strStudJoiningDate = getDateString(dtStudJoiningDate);
                    var strStudAdmissionDate = getDateString(dtStudAdmissionDate);
                    var strIsMidYear = $get("<%=this.hidIsMidYear.ClientID %>").value

                    if (strIsMidYear == "1") {
		                if (DeletedDate < dtStudAdmissionDate) {
		                    msg = document.getElementById("<%=this.hidValSchoolLeavingDateForAdmission.ClientID %>").value + "(i.e " + strStudAdmissionDate + " ).";
		                }
                    }
                    else if (DeletedDate < dtStudJoiningDate) {
		                msg = document.getElementById("<%=this.hidValSchoolLeavingDate.ClientID %>").value + "(i.e " + strStudJoiningDate + " ).";
		            }
		            else if (DeletedDate > now) {
		                var allowFutureDate = $get("<%=this.hidAllowFutureDate.ClientID %>").value
		                if (allowFutureDate != "1") {
		                    msg = document.getElementById("<%=this.hidValSchoolLeavingFutureDate.ClientID %>").value;
		                }
		            }
		        } else {
		            msg = document.getElementById("<%=this.hidValSchoolLeavingDateBlank.ClientID %>").value;
		        }
		    }

		    if (msg == "")
		        return true
		    else {
		        alert(msg)
		        return false;
		    }
		}

		//This function is used to display string into required format.
		function getDateString(obj) {

			var strDate = obj.getDate() + "-";
			var strMonth = parseInt(obj.getMonth());
			strMonth = months[strMonth];
			strDate = strDate + strMonth + "-";
			strDate = strDate + obj.getFullYear();
			return strDate;
		}
		// This function is used to check whether leaving date is in current academic year or not.
		function CheckIfDateInAcademicYear(dtObj) {
			var bReturn;
			var dtYearStartDate = new Date(document.getElementById(_clienthidAcademicStartDate).value);
			var dtYearEndDate = new Date(document.getElementById(_clienthidAcademicEndDate).value);
			if (dtObj < dtYearStartDate || dtObj > dtYearEndDate)
				bReturn = false;
			else
				bReturn = true;
			return bReturn;
		}
		function test(sRef, sDestination, sTask) {

			var xmlHttpObj = CreateHTTPReqObj();
			if (xmlHttpObj) {

				var cntrl = document.getElementById(sRef); //("ctl00$MainBody$cmbStandard");
				var iSchoolId = document.getElementById(_sClienthidSchoolId).value;
				var iStandardId = cntrl.value;
				var iAcademicYearId = document.getElementById(_sClientAcademicYearId).value;
				var url = "../Ajax.ashx?SchoolId=" + iSchoolId + "&StandardId=" + iStandardId + "&AcademicYearId=" + iAcademicYearId + "&task=" + sTask;

				xmlHttpObj.open("GET", url, true);
				xmlHttpObj.onreadystatechange = function () {
					if (xmlHttpObj.readyState == 4) {
						if (xmlHttpObj.status == 200) {
							var optionText = xmlHttpObj.responseText;
							var cntrlDivision = document.getElementById(sDestination); //("ctl00$MainBody$cmbDivision");
							cntrlDivision.options.length = 0;
							var sArray = optionText.split("@@@");
							var cnt = sArray.length;


							var htmlCode = document.createElement("option");
							htmlCode.text = "--All--";
							htmlCode.value = "0";
							cntrlDivision.options.add(htmlCode);
							if (optionText != "") {

								for (i = 0; i < cnt; i++) {
									var soption = sArray[i].split("###");

									var sText = soption[1];
									var sValue = soption[0];

									var htmlCode = document.createElement("option");
									htmlCode.text = sText;
									htmlCode.value = sValue;
									cntrlDivision.options.add(htmlCode);
								}
							}
						}
					}
				}
				xmlHttpObj.send(null);
			}
			else
				alert('Sad!!');

			document.getElementById(_sClientbtnAdd).style.display = 'none';
		}

		function assignDivision(obj) {
			document.getElementById(_sClienthidDivisionId).value = obj.value;
		}

		function assignStandard(obj) {

			if (obj.value == 0)
				document.getElementById(_sClienthidStandardId).value = obj.value;
			else
				document.getElementById(_sClienthidStandardId).value = obj.value;
		}

		function HideAddButton(objIdStd, objIdDiv) {

			if (document.getElementById(objIdDiv).value == 0)
				document.getElementById(_sClientbtnAdd).style.display = 'none';
			else
				document.getElementById(_sClientbtnAdd).style.display = '';
		}
		//This function is used to refresh parent screen.
		function refreshParent() {
			if (document.getElementById(_clienthidIsSuperAdmin).value == "N")
				window.opener.location.reload(true);
			window.close();
			window.opener.focus();
		}

		function fnover(varname, doc) {

			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "maroon";
			objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)";
		}

		function fnout(varname, doc) {
			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1";
			objTXT.style.borderColor = "#a3c07b";
			objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)";
		}
		//This function is used to check the attendance of leaving student.
		function ConfirmDelete(LeftOrNot, IsAttendanceAvail) {
			var bResult = true;
			var validationResult = true;
			if (IsAttendanceAvail == "Y" || IsAttendanceAvail == "N")
				IsAttendanceAvailable = IsAttendanceAvail

			var dt1 = $get("<%=this.txtDeletedDate.ClientID %>").value;
			var dt = new Date();
			if (!CheckIfDateInAcademicYear(dt)) {
				if (LeftOrNot != 'Leave') {
					dt = new Date(document.getElementById(_clienthidAcademicEndDate).value);
					$get("<%=this.txtDeletedDate.ClientID %>").value = dt;
				}
			}

			if (typeof (Page_ClientValidate) == 'function')
				validationResult = Page_ClientValidate("");

            if (validationResult == false)
                return false;
            else {
                var isValid = IsValidLeavingDate()
                if (isValid == false)
                    return false;

                $get("<%=this.txtDeletedDate.ClientID %>").value = dt1;
            }

			var sMsg;
			var dtDeletedDate = document.getElementById(_clienttxtDeletedDate).value;
			var DeletedDate = new Date(dtDeletedDate).format("dd/MMM/yyyy");
			if (IsAttendanceAvailable == "N") {
				if ($get("<%=this.chkSchoolLeaving.ClientID %>").checked && LeftOrNot == 'Leave') {
					if (!(CheckIfDateInAcademicYear(DeletedDate)))
					    sMsg = document.getElementById("<%=this.hidValLeavingDateOutSide.ClientID %>").value;
					else
					    sMsg = document.getElementById("<%=this.hidValStudentLeaving.ClientID %>").value;
				}
				else
				    sMsg = document.getElementById("<%=this.hidStudentDelete.ClientID %>").value;

				if (!window.confirm(sMsg))
					bResult = false;
				else {
					HidePopup(false);
					bResult = true;
				}
			}
			else {
				if ($get("<%=this.chkSchoolLeaving.ClientID %>").checked && LeftOrNot == 'Leave') {
					if (!(CheckIfDateInAcademicYear(DeletedDate)))
					    sMsg = document.getElementById("<%=this.hidValLeavingDateOutSide.ClientID %>").value;
					else
					    sMsg = document.getElementById("<%=this.hidValStudentLeaving.ClientID %>").value;

					if (!window.confirm(sMsg)) {
						bResult = false;
					}
					else {
						HidePopup(false);
						bResult = true;
					}
				}
				else if (document.getElementById(_clienhidIsAdmin).value == 'True') {
				    sMsg = document.getElementById("<%=this.hidDeleteStudent.ClientID %>").value;
					if (!window.confirm(sMsg)) {
						bResult = false;
					}
					else {
						HidePopup(false);
						bResult = true;
					}
				}
				else {
				    sMsg = document.getElementById("<%=this.hidDeleteStudent.ClientID %>").value;
					window.alert(sMsg);
					HidePopup(true);
					bResult = false;
				}
			}
			return bResult;
		}
		//This function is used to set date.
		function SetDate() {
			var dt1 = $get("<%=this.txtDeletedDate.ClientID %>").value;
			var dt = new Date();
			if (!CheckIfDateInAcademicYear(dt)) {
				dt = new Date(document.getElementById(_clienthidAcademicEndDate).value);
			}
			$get("<%=this.txtDeletedDate.ClientID %>").value = dt;

			if (typeof (Page_ClientValidate) == 'function')
				validationResult = Page_ClientValidate("");
			if (validationResult == false)
				return false;
		}
		//This function is used to show identity card popup.
		function ShowIdentities(sQryStr) {
			_sClienthhlnkIdentity = "<%=this.hlnkIdentity.ClientID %>";

			if ((document.getElementById(_sClienthhlnkIdentity) == null) || (document.getElementById(_sClienthhlnkIdentity) == "") || (document.getElementById(_sClienthhlnkIdentity).disabled))
				return false;

			window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
			return false;
		}
		function ShowPhotos(sQryStr) {
			_sClienthlnkPhotos = "<%=this.hlnkPhotos.ClientID %>";

			if ((document.getElementById(_sClienthlnkPhotos) == null) || (document.getElementById(_sClienthlnkPhotos) == "") || (document.getElementById(_sClienthlnkPhotos).disabled))
				return false;

			window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
			return false;
		}

		function ValidateRegNo(aSrc, args) {
			if ($get(_clientoptExact).checked) {
				if ($get(_clienttxtReg).value == "") {
					args.IsValid = false;
					$get(_clientlblErrorMsg).innerHTML = "";
				}
				else {
					args.IsValid = true;
					return false;
				}
				return false;
			}
		}

		function SetControlsUponCriteria() {
			var flag = $get(_clientoptMain).checked;
			if (flag) {
				$get(_clientcmbPrefix).selectedIndex = 0;
				$get(_clientcmbOperation).selectedIndex = 0;
			}
			$get(_clientoptMain).checked = flag;
			$get(_clientoptExact).checked = !flag;
			$get(_clienttxtName).value = "";
			$get(_clienttxtReg).value = "";
			$get(_clienttxtName).disabled = !flag;
			$get(_clienttxtReg).disabled = flag;
			$get(_clientcmbPrefix).disabled = flag;
			$get(_clientcmbOperation).disabled = flag;
			$get(_clientcmbPrefix).selectedIndex = 0;
			$get(_clienthidIsExactMatch).value = (!flag).toString();
			$get(_clienthidStudentReg).value = $get(_clienttxtReg).value.toString();
			$get(_clienthidStudentName).value = $get(_clienttxtName).value.toString();
			$get(_clientlblErrorMsg).innerHTML = "";
		}

		//This function is used to get confirmation from user about left student include.
        function ConfirmIncludeLeftStudentExport(e) {
            if (!window.confirm('Do you want to include the left students in the student list to be exported? Click on "OK" button to include details of left students and click on "Cancel" button to export only details of current students in the selected class.')) {
		        $get(_clienthidIncludeLeft).value = '0';
		    }
            else
                $get(_clienthidIncludeLeft).value = '1';

        }

	</script>
	 <script language="javascript" type="text/javascript">

	 	$(document).ready(function () {
	 		AutoSearch();
	 	});
	 	function AutoSearch() {
	 		var SchoolId = "<%=miSchoolId %>";
	 		_clienttxtRegNumber = '#<%=txtName.ClientID%>';
	 		var AcademicYearId = "<%=miAcademicYearId %>"
	 		BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, _clientcmbStandard, _clientcmbDivision, _clientcmbClass, 1);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function OpenWindow(sUserGuidURL) {
            window.open(sUserGuidURL, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=600').focus();
            return false;
        }             
    </script>

</asp:Content>
