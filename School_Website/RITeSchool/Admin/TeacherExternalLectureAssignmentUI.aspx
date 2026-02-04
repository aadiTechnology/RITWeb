<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TeacherExternalLectureAssignmentUI.aspx.cs"
	MasterPageFile="../MasterPages/MasterPage.master" Inherits="TeacherExternalLectureAssignmentUI" %>

<%@ Register Assembly="eWorld.UI.Compatibility, Version=2.0.6.2393, Culture=neutral, PublicKeyToken=24d65337282035f2"
	Namespace="eWorld.UI.Compatibility" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table width="97%" align="center">
		<tr>
			<td>
				<asp:UpdatePanel ID="upnl1" runat="server">
					<ContentTemplate>
						<div id="divExternalLectureAssignment" runat="server">
							<cc1:CollapsablePanel ID="colpnlExternalLectures" runat="server" TitleText="<%$ Resources:LocalizedResources, MsgAssignExternalLectures %>"
								TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
								CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left"
								TitleStyle-Height="25px" Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
								<table width="100%">
									<tr>
										<td align="center">
											<asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
												EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
										</td>
									</tr>
									<tr>
										<td align="center">
											<table id="tblSearch" runat="server">
												<tr>
													<td class="ClsBorderlight" style="width: 100px">
														<span class="ClsLabel">
                                                        <asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, Name %>"></asp:Label>
                                                        <span class="colonPadding"> :</span>
                                                        </span>
													</td>
													<td style="width: 300px">
													<asp:TextBox ID="txtSearch" runat="server" MaxLength="50" CssClass="LrgTxtBox"></asp:TextBox>
														<asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:LocalizedResources, Search %>" CssClass="ClsBtn" OnClick="btnSearch_Click">
														</asp:Button>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="trPagerTeacherDetails" runat="server">
										<td align="center">
											<asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwExtenalLectureDetails">
												<Fields>
													<asp:TemplatePagerField>
														<PagerTemplate>
															<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
																CssClass="LblNrmlB" />
															<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To %>" />
															<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
																CssClass="LblNrmlB" />
															<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf %>" />
															<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
																CssClass="LblNrmlB" />
															<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records %>" />
															<br />
														</PagerTemplate>
													</asp:TemplatePagerField>
												</Fields>
											</asp:DataPager>
										</td>
									</tr>
									<tr>
										<td>
											<table align="center" style="width: 700px">
												<tr align="center">
													<td align="center">
														<asp:ListView ID="lstvwExtenalLectureDetails" runat="server" DataKeyNames="TeacherId,IsAssembly,IsMPT,IsStayBack,WeeklyTestApplicable"
															OnItemDataBound="lstvwExtenalLectureDetails_ItemDataBound" OnDataBound="lstvwExtenalLectureDetails_DataBound">
															<LayoutTemplate>
																<table align="center" width="700px" runat="server" id="tblTeacherDetails" style="color: #333333"
																	cellpadding="0" cellspacing="1" class="GridBorder">
																	<tr id="trHeader" runat="server" class="ClsGridHeader">
																		<th id="thAssemblyApplicable" runat="server" align="center" width="90px" style="padding-left: 10px;">
																			<asp:Label ID="Label5" runat="server" Text="<%$ Resources:LocalizedResources, IsAssembly %>"></asp:Label>
																		</th>
																		<th id="thMPTApplicable" align="center" runat="server" width="80px" style="padding-left: 10px;">
																			<asp:Label ID="Label8" runat="server" Text="<%$ Resources:LocalizedResources, IsMPT %>"></asp:Label>
																		</th>
																		<th id="thStayBackApplicable" align="center" runat="server" width="90px" style="padding-left: 10px;">
																			<asp:Label ID="Label9" runat="server" Text="<%$ Resources:LocalizedResources, IsStayBack %>"></asp:Label>
																		</th>
                                                                        <th id="thWeeklyTest" align="center" runat="server" width="160px" style="padding-left: 10px;">
																			<asp:Label ID="Label28" runat="server" Text="Weekly Test Applicable?"></asp:Label>
																		</th>
																		<th align="left" style="padding-left: 10px;">
                                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, TeacherName %>"></asp:Label>
																		</th>
																	</tr>
																	<tr runat="server" id="itemPlaceholder">
																	</tr>
																	<tr class="ClsBorderPager" id="trDataPager">
																		<td colspan="5">
																			<asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwExtenalLectureDetails"
																				PageSize="20">
																				<Fields>
																					<asp:TemplatePagerField>
																						<PagerTemplate>
																							<table width="100%">
																								<tr>
																									<td align="left">
																										<asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage %>" runat="server" CssClass="LblNrmlB" />
                                                                                                         <span class="colonPadding"> :</span>
																										<asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
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
																<tr id="Tr2" runat="server" class="ClsGridRow">
																	<td id="tdAssemblyApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkAssembly" />
																	</td>
																	<td id="tdMPTApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkMPT" />
																	</td>
																	<td id="tdStayBackApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkStayback" />
																	</td>
                                                                    <td id="tdWeeklyTestApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkWeeklyTest" />
																	</td>
																	<td align="left" style="padding-left: 10px">
																		<asp:Label ID="Label1" runat="server" Text='<%# Eval("TeacherName") %>'></asp:Label>
																	</td>
																</tr>
															</ItemTemplate>
															<AlternatingItemTemplate>
																<tr id="Tr3" runat="server" class="ClsGridAltRow">
																	<td id="tdAssemblyApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkAssembly" />
																	</td>
																	<td id="tdMPTApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkMPT" />
																	</td>
																	<td id="tdStayBackApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkStayback" />
																	</td>
                                                                    <td id="tdWeeklyTestApplicable" runat="server" align="center">
																		<asp:CheckBox runat="server" ID="chkWeeklyTest" />
																	</td>
																	<td align="left" style="padding-left: 10px">
																		<asp:Label ID="Label2" runat="server" Text='<%# Eval("TeacherName") %>'></asp:Label>
																	</td>
																</tr>
															</AlternatingItemTemplate>
															<EmptyDataTemplate>
																<tr>
																	<td class="LblNoRecord" align="center">
																		<asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, NoRecordsFound %>"></asp:Label>
																	</td>
																</tr>
															</EmptyDataTemplate>
														</asp:ListView>
													</td>
												</tr>
												<asp:ObjectDataSource TypeName="BusinessLogic.ExternalLecturesBL" EnablePaging="True"
													ID="ObjDSTeacherDetails" runat="server" SelectMethod="GetPagedTeacherExternalLectureDetails"
													SelectCountMethod="CountPagedTeacherExternalLectureDetails" EnableCaching="False">
													<SelectParameters>
														<asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
														<asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
															Type="Int32" />
														<asp:Parameter Name="maximumRows" Type="Int32" />
														<asp:Parameter Name="startRowIndex" Type="Int32" />
														<asp:ControlParameter Name="asCriteria" Type="String" ControlID="txtSearch" PropertyName="Text" />
													</SelectParameters>
												</asp:ObjectDataSource>
												<asp:HiddenField ID="hidAssemblyApplicable" runat="server" />
												<asp:HiddenField ID="hidMPTApplicable" runat="server" />
												<asp:HiddenField ID="hidStayBackApplicable" runat="server" />
												<asp:HiddenField ID="hidIsConfigured" runat="server" />
												<asp:HiddenField ID="hidPageNo" runat="server" />
											</table>
										</td>
									</tr>
									<tr>
										<td align="center">
											<asp:Button ID="btnSave" Text="<%$ Resources:LocalizedResources, Save %>" runat="server" CssClass="ClsBtn" BorderWidth="1px"
												ValidationGroup="Save" CausesValidation="true" OnClick="btnSave_Click" />
										</td>
									</tr>
									<tr>
										<td align="left">
											<table width="45%">
												<tr>
													<td align="left" colspan="1" class="ClsBorderlight " style="width: 10%; background-color: #ffffc4;">
														<span class="LblNrmlB">
                                                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:LocalizedResources, Note %>"></asp:Label>
                                                        <span class="colonPadding"> :</span>
                                                        </span>
													</td>
													<td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 5px; width: 100%">
														<span class="LblSmlV">
                                                        <asp:Label ID="Label10" runat="server" Text="<%$ Resources:LocalizedResources, NoteExernalLectureAssignments %>"></asp:Label>
                                                        </span>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</cc1:CollapsablePanel>
						</div>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr id="trExternalLecturesNotAplicable" runat="server" visible="false">
			<td style="height: 10px;" align="center">
				<asp:Label ID="Label4" runat="server" CssClass="LblNoRecord" Font-Bold="True" Text="<%$ Resources:LocalizedResources, ValExternalLectures %>"
					EnableViewState="False" Width="85%"></asp:Label>
			</td>
		</tr>

		   <tr>
		  <td>	  <div id="divAssemblyLecture" runat="server">
							<cc1:CollapsablePanel ID="colpnlAssemblyLectures" runat="server" TitleText="Assign Assembly Lectures"
								TitleStyle-CssClass="CollapsTitle" AllowSliding="false" ExpandImageUrl="../images/node_open.gif"
								CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left" 
								TitleStyle-Height="25px" Collapsed="False" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
								<table width="100%" >
									<tr id="tr1" runat="server">
										<td align="center">
											<table id="Table1" runat="server">
												<tr>
													<td align="left" colspan="1">
														<asp:Label ID="Label18" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
															Text="Legend" EnableViewState="false"></asp:Label>
													</td>
													<td align="right">
														<asp:Label ID="Label8" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label9" runat="server" Font-Bold="True" Text="Assembly lectures to be assigned"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
													<td align="left" style="width: 5px">
													</td>
													<td align="left">
														<asp:Label ID="Label19" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label20" runat="server" Font-Bold="True" Text="Assigned Assembly lectures"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="tr4" runat="server">
										<td align="center">
											<div id="Div2" runat="server" class="GridBorder" style="width: 635pt;
												overflow: scroll;">
											
                                                    <asp:ListView ID="lstvwAssemblyLectures" runat="server" DataKeyNames="StandardId,StandardwiseDivisionId" 
                                                        OnItemDataBound="lstvwAssemblyLectures_ItemDataBound" ondatabound="lstvwAssemblyLectures_DataBound"
                                                       >
													<LayoutTemplate>
														<table align="center" width="1200px" runat="server" id="tblTeacherDetails" style="color: #333333"
															cellpadding="0" cellspacing="1" class="GridBorder">
															<tr id="trHeader" runat="server" class="ClsGridHeader">
																<th id="thAssemblyApplicable" runat="server" style="padding-left: 0px" align="center"
																	width="100px">
																	<asp:Image ID="imgStdWeekDays" Height="22px" Width="100px" runat="server" ImageUrl="~/RITeSchool/images/GridHeader_StdWeekday.gif" />
																</th>
																<th id="thMonday" runat="server" align="center">
																	<asp:Label ID="lblMon" runat="server"></asp:Label>
																</th>
																<th id="thTuesday" runat="server" align="center">
																	<asp:Label ID="lblTue" runat="server"></asp:Label>
																</th>
																<th id="thWednesday" runat="server" align="center">
																	<asp:Label ID="lblWed" runat="server"></asp:Label>  
																</th>
																<th id="thThursday" runat="server" align="center">
																	<asp:Label ID="lblThu" runat="server"></asp:Label>      
																</th>
																<th id="thFriday" runat="server" align="center">
																	<asp:Label ID="lblFri" runat="server"></asp:Label>  
																</th>
																<th id="thSaturday" runat="server" align="center">
																	<asp:Label ID="lblSat" runat="server"></asp:Label> 
																</th>
																<th id="thSunday" runat="server" align="center">
																	<asp:Label ID="lblSun" runat="server"></asp:Label>  
																</th>
															</tr>
															<tr runat="server" id="itemPlaceholder">
															</tr>
														</table>
													</LayoutTemplate>
													<ItemTemplate>
														<tr id="Tr2" runat="server" class="ClsGridRow">
															<td align="left" style="padding-left: 10px;" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</ItemTemplate>
													<AlternatingItemTemplate>
														<tr id="Tr3" runat="server" class="ClsGridAltRow">
															<td align="left" style="padding-left: 10px" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</AlternatingItemTemplate>
												</asp:ListView>
											</div>
										</td>
									</tr>
									<tr id="tr5" runat="server" visible="false">
										<td style="height: 10px;" align="center">
											<asp:Label ID="Label12" runat="server" CssClass="LblNoRecord" Font-Bold="True" Text="Assebbly lectures are not applicable."
												EnableViewState="False" Width="85%"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<div runat="server" id="div3">
											</div>
										</td>
									</tr>
								</table>
							</cc1:CollapsablePanel>
						</div>
				    
			</td>
		</tr>
        <tr>
		  <td>	  <div id="div1" runat="server">
							<cc1:CollapsablePanel ID="CollapsablePanel1" runat="server" TitleText="Assign M.P.T. Lectures"
								TitleStyle-CssClass="CollapsTitle" AllowSliding="False" ExpandImageUrl="../images/node_open.gif"
								CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left"
								TitleStyle-Height="25px" Collapsed="False" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
								<table width="100%" >
									<tr id="tr6" runat="server">
										<td align="center">
											<table id="Table2" runat="server">
												<tr>
													<td align="left" colspan="1">
														<asp:Label ID="Label13" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
															Text="Legend" EnableViewState="false"></asp:Label>
													</td>
													<td align="right">
														<asp:Label ID="Label14" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label15" runat="server" Font-Bold="True" Text="M.P.T. lectures to be assigned"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
													<td align="left" style="width: 5px">
													</td>
													<td align="left">
														<asp:Label ID="Label16" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label17" runat="server" Font-Bold="True" Text="Assigned M.P.T. lectures"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="tr7" runat="server">
										<td align="center">
											<div id="Div4" runat="server" class="GridBorder" style="width: 635pt;
												overflow: scroll;">
											
                                                    <asp:ListView ID="lstvwMPTLecture" runat="server" DataKeyNames="StandardId,StandardwiseDivisionId" 
                                                        OnItemDataBound="lstvwMPTLecture_ItemDataBound" ondatabound="lstvwMPTLecture_DataBound"
                                                       >
													<LayoutTemplate>
														<table align="center" width="1200px" runat="server" id="tblTeacherDetails" style="color: #333333"
															cellpadding="0" cellspacing="1" class="GridBorder">
															<tr id="trHeader" runat="server" class="ClsGridHeader">
																<th id="thAssemblyApplicable" runat="server" style="padding-left: 0px" align="center"
																	width="100px">
																	<asp:Image ID="imgStdWeekDays" Height="22px" Width="100px" runat="server" ImageUrl="~/RITeSchool/images/GridHeader_StdWeekday.gif" />
																</th>
																<th id="thMonday" runat="server" align="center">
																	<asp:Label ID="lblMon" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thTuesday" runat="server" align="center">
																	<asp:Label ID="lblTue" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thWednesday" runat="server" align="center">
																	<asp:Label ID="lblWed" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thThursday" runat="server" align="center">
																	<asp:Label ID="lblThu" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thFriday" runat="server" align="center">
																	<asp:Label ID="lblFri" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thSaturday" runat="server" align="center">
																	<asp:Label ID="lblSat" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thSunday" runat="server" align="center">
																	<asp:Label ID="lblSun" runat="server"></asp:Label>                                                                    
																</th>
															</tr>
															<tr runat="server" id="itemPlaceholder">
															</tr>
														</table>
													</LayoutTemplate>
													<ItemTemplate>
														<tr id="Tr2" runat="server" class="ClsGridRow">
															<td align="left" style="padding-left: 10px;" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</ItemTemplate>
													<AlternatingItemTemplate>
														<tr id="Tr3" runat="server" class="ClsGridAltRow">
															<td align="left" style="padding-left: 10px" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</AlternatingItemTemplate>
												</asp:ListView>
											</div>
										</td>
									</tr>
									<tr id="tr8" runat="server" visible="false">
										<td style="height: 10px;" align="center">
											<asp:Label ID="Label21" runat="server" CssClass="LblNoRecord" Font-Bold="True" Text="Assebbly lectures are not applicable."
												EnableViewState="False" Width="85%"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<div runat="server" id="div5">
											</div>
										</td>
									</tr>
								</table>
							</cc1:CollapsablePanel>
						</div>
				    
			</td>
		</tr>         
        <tr>
			<td>
				<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
					<ContentTemplate>
						<div id="divStayBackLectures" runat="server">
							<cc1:CollapsablePanel ID="colpnlStayBackLectures" runat="server" TitleText="<%$ Resources:LocalizedResources, MsgAssignStayBackLectures %>"
								TitleStyle-CssClass="CollapsTitle" AllowSliding="true" ExpandImageUrl="../images/node_open.gif"
								CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left"
								TitleStyle-Height="25px" Collapsed="True" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
								<table width="100%">
									<tr id="trLegends" runat="server">
										<td align="center">
											<table id="LegendTable" runat="server">
												<tr>
													<td align="left" colspan="1">
														<asp:Label ID="Label" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
															Text="<%$ Resources:LocalizedResources, Legend %>" EnableViewState="false"></asp:Label>
													</td>
													<td align="right">
														<asp:Label ID="TextBox2" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label6" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgStayBackAssigned %>"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
													<td align="left" style="width: 5px">
													</td>
													<td align="left">
														<asp:Label ID="TextBox3" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label7" runat="server" Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgAssignedStayBackLectures %>"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="trlstvwStaybackLectures" runat="server">
										<td align="center">
											<div id="ListViewScrollContainer" runat="server" class="GridBorder" style="width: 635pt;
												overflow: scroll;">
												<asp:ListView ID="lstvwStaybackLectures" runat="server" DataKeyNames="StandardId,StandardwiseDivisionId"
													OnItemDataBound="lstvwStaybackLectures_ItemDataBound" ondatabound="lstvwStaybackLectures_DataBound">
													<LayoutTemplate>
														<table align="center" width="1200px" runat="server" id="tblTeacherDetails" style="color: #333333"
															cellpadding="0" cellspacing="1" class="GridBorder">
															<tr id="trHeader" runat="server" class="ClsGridHeader">
																<th id="thAssemblyApplicable" runat="server" style="padding-left: 0px" align="center"
																	width="100px">
																	<asp:Image ID="imgStdWeekDays" Height="22px" Width="100px" runat="server" ImageUrl="~/RITeSchool/images/GridHeader_StdWeekday.gif" />
																</th>
																<th id="thMonday" runat="server" align="center">
																	<asp:Label ID="lblMon" runat="server"></asp:Label>
																</th>
																<th id="thTuesday" runat="server" align="center">
																	<asp:Label ID="lblTue" runat="server"></asp:Label>
																</th>
																<th id="thWednesday" runat="server" align="center">
																	<asp:Label ID="lblWed" runat="server"></asp:Label>
																</th>
																<th id="thThursday" runat="server" align="center">
																	<asp:Label ID="lblThu" runat="server"></asp:Label>
																</th>
																<th id="thFriday" runat="server" align="center">
																	<asp:Label ID="lblFri" runat="server"></asp:Label>
																</th>
																<th id="thSaturday" runat="server" align="center">
																	<asp:Label ID="lblSat" runat="server"></asp:Label>
																</th>
																<th id="thSunday" runat="server" align="center">
																	<asp:Label ID="lblSun" runat="server"></asp:Label>
																</th>
															</tr>
															<tr runat="server" id="itemPlaceholder">
															</tr>
														</table>
													</LayoutTemplate>
													<ItemTemplate>
														<tr id="Tr2" runat="server" class="ClsGridRow">
															<td align="left" style="padding-left: 10px;" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</ItemTemplate>
													<AlternatingItemTemplate>
														<tr id="Tr3" runat="server" class="ClsGridAltRow">
															<td align="left" style="padding-left: 10px" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</AlternatingItemTemplate>
												</asp:ListView>
											</div>
										</td>
									</tr>
									<tr id="trStayBackNotAplicable" runat="server" visible="false">
										<td style="height: 10px;" align="center">
											<asp:Label ID="Label3" runat="server" CssClass="LblNoRecord" Font-Bold="True" Text="<%$ Resources:LocalizedResources, MsgStayBackNotApplicable %>"
												EnableViewState="False" Width="85%"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<div runat="server" id="divErr">
											</div>
										</td>
									</tr>
								</table>
							</cc1:CollapsablePanel>
						</div>
					</ContentTemplate>
					<Triggers>
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
        <tr id="trWeeklyTestApplicable" runat="server">
		  <td>	  <div id="div6" runat="server">
							<cc1:CollapsablePanel ID="ColWeeklyTest" runat="server" TitleText="Assign Weekly Test"
								TitleStyle-CssClass="CollapsTitle" AllowSliding="False" ExpandImageUrl="../images/node_open.gif"
								CollapseImageUrl="../images/node_close.gif" CollapserAlign="Left"
								TitleStyle-Height="25px" Collapsed="False" SlideSpeed="25" CollapsedTitleStyle-CssClass="CollapsedTitle">
								<table width="100%" >
									<tr id="tr9" runat="server">
										<td align="center">
											<table id="Table3" runat="server">
												<tr>
													<td align="left" colspan="1">
														<asp:Label ID="Label22" runat="server" BorderWidth="0px" CssClass="ClsLblLgnd" Font-Bold="True"
															Text="Legend" EnableViewState="false"></asp:Label>
													</td>
													<td align="right">
														<asp:Label ID="Label23" runat="server" BackColor="#5dad8e" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label24" runat="server" Font-Bold="True" Text="Weekly Test to be assigned"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
													<td align="left" style="width: 5px">
													</td>
													<td align="left">
														<asp:Label ID="Label25" runat="server" BackColor="#eaeaea" Height="20px" BorderColor="Black"
															BorderStyle="Solid" BorderWidth="1px" ReadOnly="True" Width="20px"> <img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
													</td>
													<td align="left">
														<asp:Label ID="Label26" runat="server" Font-Bold="True" Text="Assigned Weekly Test"
															CssClass="ClsTextNormal" EnableViewState="false"></asp:Label>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr id="tr10" runat="server">
										<td align="center">
											<div id="Div7" runat="server" class="GridBorder" style="width: 635pt;
												overflow: scroll;">
                                                    <asp:ListView ID="lstvwWeeklyTest" runat="server" 
                                                        onitemdatabound="lstvwWeeklyTest_ItemDataBound" 
                                                        ondatabound="lstvwWeeklyTest_DataBound">
													<LayoutTemplate>
														<table align="center" width="1200px" runat="server" id="tblWeeklyTest" style="color: #333333"
															cellpadding="0" cellspacing="1" class="GridBorder">
															<tr id="trHeader" runat="server" class="ClsGridHeader">
																<th id="thAssemblyApplicable" runat="server" style="padding-left: 0px" align="center"
																	width="100px">
																	<asp:Image ID="imgStdWeekDays" Height="22px" Width="100px" runat="server" ImageUrl="~/RITeSchool/images/GridHeader_StdWeekday.gif" />
																</th>
																<th id="thMonday" runat="server" align="center">
																	<asp:Label ID="lblMon" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thTuesday" runat="server" align="center">
																	<asp:Label ID="lblTue" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thWednesday" runat="server" align="center">
																	<asp:Label ID="lblWed" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thThursday" runat="server" align="center">
																	<asp:Label ID="lblThu" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thFriday" runat="server" align="center">
																	<asp:Label ID="lblFri" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thSaturday" runat="server" align="center">
																	<asp:Label ID="lblSat" runat="server"></asp:Label>                                                                    
																</th>
																<th id="thSunday" runat="server" align="center">
																	<asp:Label ID="lblSun" runat="server"></asp:Label>                                                                    
																</th>
															</tr>
															<tr runat="server" id="itemPlaceholder">
															</tr>
														</table>
													</LayoutTemplate>
													<ItemTemplate>
														<tr id="Tr2" runat="server" class="ClsGridRow">
															<td align="left" style="padding-left: 10px;" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</ItemTemplate>
													<AlternatingItemTemplate>
														<tr id="Tr3" runat="server" class="ClsGridAltRow">
															<td align="left" style="padding-left: 10px" width="100px">
																<asp:Label ID="lblStdDivisionName" runat="server" Width="100px" Text='<%# Eval("StandardDivision") %>'></asp:Label>
															</td>
															<td id="tdMonday" align="center">
															</td>
															<td id="tdTuesday" align="center">
															</td>
															<td id="tdWednesday" align="center">
															</td>
															<td id="tdThursday" align="center">
															</td>
															<td id="tdFriday" align="center">
															</td>
															<td id="tdSaturday" align="center">
															</td>
															<td id="tdSunday" align="center">
															</td>
														</tr>
													</AlternatingItemTemplate>
												</asp:ListView>
											</div>
										</td>
									</tr>
									<tr id="tr11" runat="server" visible="false">
										<td style="height: 10px;" align="center">
											<asp:Label ID="Label27" runat="server" CssClass="LblNoRecord" Font-Bold="True" Text="Weekly Test are not applicable."
												EnableViewState="False" Width="85%"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<div runat="server" id="div8">
											</div>
										</td>
									</tr>
								</table>
							</cc1:CollapsablePanel>
						</div>
				    
			</td>
		</tr>
		<tr>
			<td align="center">
				<asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back %>" CssClass="ClsBtn" BorderWidth="1px"
					CausesValidation="False" UseSubmitBehavior="false" />
			</td>
		</tr>
        <asp:HiddenField ID="hidCultureInfo" runat="server" />
        <asp:HiddenField ID="hidWarringExternalLecture" runat="server" />
	</table>
	<script type="text/javascript" language="javascript">
	    _clientlstvwExtenalLectureDetails = "<%=this.lstvwExtenalLectureDetails.ClientID %>"
	    _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"

	    function ConfirmMsg(oCmb) {
	        var bIsValid
	        if (window.confirm(document.getElementById("<%=this.hidWarringExternalLecture.ClientID %>").value))
	            bIsValid = true
	        else {
	            document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
	            bIsValid = false
	        }
	        return bIsValid
	    }
	</script>
</asp:Content>
