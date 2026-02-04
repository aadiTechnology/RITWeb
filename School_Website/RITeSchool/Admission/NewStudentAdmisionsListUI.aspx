<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	CodeFile="NewStudentAdmisionsListUI.aspx.cs" Inherits="NewStudentAdmisionsListUI"  ViewStateMode="Enabled" %>
    <%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
		<table align="center" border="0" cellpadding="0" cellspacing="0" width="90%">
			<tr id="trPrecondition" runat="server" align="center" visible="false">
				<td>
					<table align="center" width="90%">
						<tr>
							<td id="tdError" runat="server">
								<div runat="server" id="divErr">
								</div>
							</td>
						</tr>
					</table>
				</td>
			</tr>
            <tr align="center" style="text-align:center; margin:0px auto;">
                <td align="center" style="text-align:center;">
                    <table width="20%" align="center">
                        <tr>
                            <td class="ClsBorderLight paddingL" style="width:150px;">
								<span id="Span3" class="ClsLabel">Academic Year :</span>
						    </td>
						    <td>
							    <asp:DropDownList ID="ddlAcademicYEar" runat="server" CssClass="MidCombo" 
                                    AutoPostBack="true" ViewStateMode="Enabled" 
                                    onselectedindexchanged="ddlAcademicYEar_SelectedIndexChanged">
							    </asp:DropDownList>
						    </td>
                        </tr>                        
                    </table>
                </td>
            </tr>

			<tr align="center" style="width: 100%;">
				<td align="center">
					<table>
                        <tr>
                            <td style="height:5px;"></td>
                        </tr>   
                        <tr>
                            <td colspan="4" style="border-bottom:1px solid; border-color:Gray;">                                
                            </td>
                        </tr>
                        <tr>
                            <td style="height:10px;"></td>
                        </tr>
                        <tr>
							<td class="ClsBorderLight paddingL" style="width: 130px">
								<span id="lblStandard" class="ClsLabel">Standard :</span>
							</td>
							<td>
								<asp:DropDownList ID="ddlStandard" runat="server" CssClass="MidCombo" AutoPostBack="true" ViewStateMode="Enabled"
									OnSelectedIndexChanged="ddlStandard_SelectedIndexChanged">
								</asp:DropDownList>
							</td>
							<td class="ClsBorderLight paddingL" style="width: 140px">
								<span id="lblAdmissionType" class="ClsLabel">Admission Type : </span>&nbsp;
							</td>
							<td>
								<asp:DropDownList ID="ddlAdmissionType" runat="server" CssClass="MidCombo" AutoPostBack="true" ViewStateMode="Enabled">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td>
								&nbsp;
							</td>
							<td class="ClsBorderlight paddingL">
								<span class="ClsLabel">Form No. / Student Name : </span>
							</td>
							<td>
								<asp:TextBox ID="txtStudentName" CssClass="MidTxtBox" MaxLength="50" runat="server"></asp:TextBox>
							</td>
						</tr>           
                       <tr>
							<td>
								&nbsp;
							</td>
							<td class="ClsBorderlight paddingL">
								<span class="ClsLabel">Status : </span>
							</td>
							<td>
								<asp:DropDownList ID="cmbStatus" runat="server" CssClass="MidCombo" 
                                    AutoPostBack="True" onselectedindexchanged="cmbStatus_SelectedIndexChanged" ViewStateMode="Enabled">
								</asp:DropDownList>
							</td>
						</tr>
                        <tr id="trLocation" runat="server">
                            <td>
                            </td>
                            <td class="ClsBorderlight paddingL" align="center">
                                <span class="ClsLabel">School Location :</span>
                            </td>
                            <td>
                                <asp:DropDownList ID="cmbSchoolLocation" Width="121px" runat="server" 
                                    ViewStateMode="Enabled" CssClass="SmlCombo" Height="19px">
								</asp:DropDownList>
                           </td>
                        </tr>
                        <tr id="enquiryFilter" runat="server" visible="false">
															<td class="ClsBorderLight paddingL" style="width: 130px">
																<span class="ClsLabel">Enquiry Start Date:</span>
															</td>
															<td >
																<asp:TextBox ID="txtEnquiryStartDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="4"></asp:TextBox>
																<rjs:PopCalendar ID="calEnquiryStartDate" runat="server" ViewStateMode="enabled"  Control="txtEnquiryStartDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderLight paddingL" style="width: 140px">
																<span class="ClsLabel">Enquiry End Date :</span>
															</td>
															<td >
																<asp:TextBox ID="txtEnquiryEndDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="5"></asp:TextBox>
																<rjs:PopCalendar ID="calEnquiryEndDate" runat="server" ViewStateMode="enabled" Control="txtEnquiryEndDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid to date." />																
															</td>
                                                          
														</tr>
                                                        <tr id="AdmissionDateTR" runat="server">
															<td class="ClsBorderLight paddingL" style="width: 130px">
																<span class="ClsLabel">Start Date:</span>
															</td>
															<td >
																<asp:TextBox ID="txtAdmissionStartDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="4"></asp:TextBox>
																<rjs:PopCalendar ID="PopCalendar1" runat="server" ViewStateMode="enabled"  Control="txtAdmissionStartDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid from date." ControlFocusOnError="True" />																
															</td>
															<td class="ClsBorderLight paddingL" style="width: 140px">
																<span class="ClsLabel">End Date :</span>
															</td>
															<td >
																<asp:TextBox ID="txtAdmissionEndDate" runat="server" EnableViewState="true" CssClass="SmlTxtBox" MaxLength="11"
																             TabIndex="5"></asp:TextBox>
																<rjs:PopCalendar ID="PopCalendar2" runat="server" ViewStateMode="enabled" Control="txtAdmissionEndDate" Culture="en"
																                 Format="dd MMM yyyy" ShowWeekend="True" Enabled="true" ShowErrorMessage="false"
																                 InvalidDateMessage="Please select valid to date." />																
															</td>
                                                          
														</tr>

                         <tr id="trAdmissionFor" runat="server" visible="false">
							<td>
								&nbsp;
							</td>
							<td class="ClsBorderlight paddingL">
								<span class="ClsLabel">Admission For : </span>
							</td>
							<td>
								<asp:DropDownList ID="cmbAdmissionFor" runat="server" CssClass="MidCombo" 
                                    AutoPostBack="True" onselectedindexchanged="cmbAdmissionFor_SelectedIndexChanged" ViewStateMode="Enabled">                                    
								</asp:DropDownList>
							</td>
						</tr>
                        <tr id="trSubmissionStatus" runat="server" visible="false">
							<td>
								&nbsp;
							</td>
							<td class="ClsBorderlight paddingL">
								<span class="ClsLabel">Submission Status : </span>
							</td>
							<td colspan="2">								
                                <asp:RadioButton ID="rbSuccessful" runat="server" Text="Successful" GroupName="Admission" Checked="true" class="ClsLabel"  ViewStateMode="Enabled"/>
                                <asp:RadioButton ID="rbUnSuccessful" runat="server" Text="Unsuccessful" GroupName="Admission" class="ClsLabel" ViewStateMode="Enabled"/>
							</td>
						</tr>                       
					</table>
				</td>
			</tr>
			<tr>
				<td >
                <div id="divDivisionConfirmation" runat="server" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 380px; height: 185px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 180px;
                    background-color: white;">
                                <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                                    background-repeat: repeat-x; padding: 4px; color: Black; text-align: right;">
                                    <div style="padding: 1px; font-size: 12px; font-weight: bold; color: Black; float: left;">
                                        Division Selection Popup!!!</div>
                                    <span style="cursor: hand" onclick="javascript:HidePopup(false);">
                                        <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                                    </span>
                                </div>
                              <div style="padding: 15px; text-align: left; height: 107px; width: 349px;" 
                                    class="ClsLabel">
                                    <table style="width: 353px; height: 120px">
                                          <tr >
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span2" class="LblNormal">Standard :</span>
                                            </td>
                                            <td class="ClsHilightBGB">
                                                <span id="lblStandardName" runat="server" class="LblNormal" EnableViewState="False" style="width:240px;" />
                                            </td>
                                        </tr> 
                                                                             
                                        <tr>
                                            <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="lblDivision" class="LblNormal">Select Division :</span>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="cmbStandardNamePopup" runat="server" CssClass="LrgCombo" Width="240"  AutoPostBack="false" CausesValidation="true" ViewStateMode="Enabled"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                             <td class="ClsBorderlight" style="white-space:nowrap">
                                                <span id="Span1" class="LblNormal">Confirmation Type :</span>
                                            </td>
                                            <td>
                                                <asp:RadioButton ID="rdoProvisional" runat="server" Text="Provisional" GroupName="Confirmation" />
                                                <asp:RadioButton ID="rdoFinal" runat="server" Text="Final" GroupName="Confirmation" />
                                            </td>
                                        </tr>
                                        
                                        <tr style="height:60px;">
                                            <td align="center" colspan="2">
                                                <asp:Button ID="btnSave" runat="server"  CssClass="ClsBtn" Style="margin-left: 5px; cursor: pointer;" Text="Save" onclick="btnSave_Click" />
                                                <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                                                    OnClientClick="javascript:HidePopup(false);return false;" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
			</td>
			</tr>
			<tr>
				<td align="left">
					<table width="500">
						<tr>
							<td align="left" class="ClsBorderlight " style="background-color: #ffffc4; width: 20%;">
								<span class="LblNrmlB" style="font-weight: bold">Note 1 :</span>
							</td>
							<td align="left" class="ClsBorderlight" style="padding-left: 5px; width: 80%">
								<span class="LblSmlV" style="padding-left: 5px;">Student admission confirmation can
									be done Standardwise. </span>
							</td>
						</tr>
					</table>
				</td>
			</tr>
           
			<tr>
				<td align="center" colspan="6">
					<asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" BorderWidth="1px"  
						OnClick="btnShow_Click" CausesValidation="False" />
					<asp:Button ID="btnClear" runat="server" Text="Clear" CausesValidation="false" CssClass="ClsBtn" 
						BorderWidth="1px"  OnClick="btnClear_Click" />
				</td>
			</tr>
			<tr>
				<td align="center" colspan="6">
					<asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="uPnl" >
						<ContentTemplate>
                                  <asp:Label ID="lblUpdateSuccess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                  CssClass="ClsLabel" Font-Bold="True" Visible="true"></asp:Label>
							<table width="100%">
								<tr id ="trStud" runat="server">
									<td align="center">
										<asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
											<Fields>
												<asp:TemplatePagerField>
													<PagerTemplate>
														<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
														<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
														<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
														<br />
													</PagerTemplate>
												</asp:TemplatePagerField>
											</Fields>
										</asp:DataPager>
									</td>
								</tr>
								<tr>
									<td valign="top">
										<div style="width: 100%;">
											<asp:ListView ID="lstvwStudentDetails" runat="server" OnDataBound="lstvwStudentDetails_DataBound"
												OnItemDataBound="lstvwStudentDetails_ItemDataBound" OnSorting="lstvwStudentDetails_Sorting" OnItemCommand ="lstvwStudentDetails_ItemCommand"
												DataSourceID="lstvwObjDS" DataKeyNames="Student_Admission_Id, Form_Number, Standard_Name, SelectedInLottery, IsLotteryConfirmed, CanConfirmDirectly, Standard_Id, IsConfirmed" ViewStateMode="Enabled">
												<LayoutTemplate>
													<table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
														cellspacing="1" class="GridBorder">
														<tr id="trHeader" runat="server" class="ClsGridHeader">
															<th align="center" width="13%">
																Is Confirmed?
															</th>
															<th align="left" class="ClspaddingL" width="10%">
																<asp:LinkButton ID="lnkFormNo" runat="server" CommandName="Sort" CommandArgument="Form_Number"
																	ForeColor="Black">Form No.</asp:LinkButton>
															</th>
															<th align="left" class="ClspaddingL">
																<asp:LinkButton ID="lnkStandardName" runat="server" CommandName="Sort" CommandArgument="Original_Standard_Id"
																	ForeColor="Black">Standard Name</asp:LinkButton>
															</th>
															<th align="left" class="ClspaddingL" width="30%">
																<asp:LinkButton ID="lnlStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
																	ForeColor="Black">Student Name</asp:LinkButton>
															</th>
                                                            <th id="th1" align="left" class="Clspadding" width="10%">
																Mobile Number
															</th>
                                                            <th id="th2" align="center" class="Clspadding" width="5%">
																Edit
															</th>
															<th id="thReceipt" align="center" width="10%" class="Clspadding">
																Receipt
															</th>
															<th align="center" width="8%">
																Admission Form
															</th>
                                                            <th id="thConfirmationForm" runat="server" align="center" width="20%">
																Confirmation Form
															</th>
                                                             <th align="center" width="8%" runat="server" id="thRegForm" visible="false">
															     Reg Form
															</th>
                                                            <th align="center" width="50%">
																Status
															</th>
                                                            <th id="thDelete" runat="server">
                                                                Delete
                                                            </th>
														</tr>
														<tr id="itemPlaceholder" runat="server">
														</tr>
														<tr class="ClsBorderPager" id="trDataPager">
															<td colspan="9">
																<asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstvwStudentDetails">
																	<Fields>
																		<asp:TemplatePagerField>
																			<PagerTemplate>
																				<table width="100%">
																					<tr>
																						<td>
																							<asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
																							<asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
													<tr class="ClsGridRow"  id="trItemWise" runat="server">
														<td align="center">
															<asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
																Visible='<%#Convert.ToBoolean(Eval("IsConfirmed")) %>' />
															<asp:CheckBox ID="chkIsConfirm" runat="server" Visible='<%#!Convert.ToBoolean(Eval("IsConfirmed")) %>' />
                                                            <asp:HiddenField ID="hidQueryString" runat="server" />
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
														</td>
                                                        <td class="ClspaddingL">
															<asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber")%>'></asp:Label>
														</td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                        </td>
														<td align="center" id="tdReceipt">
															<asp:HyperLink ID="lnkReceipt" runat="server" Text="Receipt" NavigateUrl="../Accountant/FeesMiniReceipt.aspx" />
														</td>
														<td align="center">
															<asp:HyperLink ID="lnkbtnForm" runat="server" Text="Admission Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                         <td align="center" id="tdConfirmationForm" runat="server">
															<asp:HyperLink ID="lnkConfirmationForm" runat="server" Text="Confirmation Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center" runat="server" id="tdRegForm" visible="false">
															<asp:HyperLink ID="lnkbtnRegForm" runat="server" Text="Reg Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center">															
                                                            <asp:LinkButton ID="lnkStatus" runat="server" Text='<%# Eval("Status")%>'></asp:LinkButton>
														</td>
                                                        <td align="center" id="tdimgBtnDelete" runat="server">
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand"
                                                                ImageUrl="../images/IconGrid_Delete.gif"  ToolTip="<%$ Resources:LocalizedResources, Delete%>"/>
                                                        </td>
													</tr>
												</ItemTemplate>
												<AlternatingItemTemplate>
													<tr id="trItemWise" runat="server" class="ClsGridAltRow">
														<td align="center">
															<asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"
																Visible='<%#Convert.ToBoolean(Eval("IsConfirmed")) %>' />
															<asp:CheckBox ID="chkIsConfirm" runat="server" Visible='<%#!Convert.ToBoolean(Eval("IsConfirmed")) %>' />
                                                            <asp:HiddenField ID="hidQueryString" runat="server" />
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Form_Number")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
														</td>
                                                        <td class="ClspaddingL">
															<asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber")%>'></asp:Label>
														</td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="btnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
                                                                ToolTip="<%$ Resources:LocalizedResources, Edit%>" CommandArgument="<%# Container.DataItemIndex %>"
                                                                ImageUrl="~/RITeSchool/images/IconGrid_Edit.gif" />
                                                        </td>
														<td align="center" id="tdReceipt">
															<asp:HyperLink ID="lnkReceipt" runat="server" Text="Receipt" NavigateUrl="../Accountant/FeesMiniReceipt.aspx" />
														</td>
														<td align="center">
															<asp:HyperLink ID="lnkbtnForm" runat="server" Text="Admission Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center" id="tdConfirmationForm" runat="server">
															<asp:HyperLink ID="lnkConfirmationForm" runat="server" Text="Confirmation Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                          <td align="center" runat="server" id="tdRegForm" visible="false">
															<asp:HyperLink ID="lnkbtnRegForm" runat="server" Text="Reg Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center">															
                                                            <asp:LinkButton ID="lnkStatus" runat="server" Text='<%# Eval("Status")%>'></asp:LinkButton>
														</td>
                                                         <td align="center" id="tdimgBtnDelete" runat="server" >
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand" 
                                                                ImageUrl="../images/IconGrid_Delete.gif"  ToolTip="<%$ Resources:LocalizedResources, Delete%>"/>
                                                        </td>
													</tr>
												</AlternatingItemTemplate>
												<EmptyDataTemplate>
													<tr>
														<td class="LblNoRecord" align="center">
															No record found.
														</td>
													</tr>
												</EmptyDataTemplate>
											</asp:ListView>
										</div>
									</td>
								</tr>
								<tr>
									<td>
										<asp:ObjectDataSource TypeName="BusinessLogic.StudentAdmissionsBL" EnablePaging="true"
											ID="lstvwObjDS" runat="server" SelectMethod="GetAllNewStudentDetails" SortParameterName="sortExpression"
											SelectCountMethod="CountAllNewStudentDetails" EnableCaching="false">
											<SelectParameters>
												<asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
												<asp:ControlParameter Name="aiAcademicYearID" Type="Int16" ControlID="hidNextAcademiYearId"
													PropertyName="Value" DefaultValue="0" />
												<asp:ControlParameter Name="aiStandardID" Type="Int16" ControlID="ddlStandard" PropertyName="SelectedValue"
													DefaultValue="0" />
												<asp:ControlParameter Name="aiAdmissionType" Type="Int16" ControlID="ddlAdmissionType"
													PropertyName="SelectedValue" DefaultValue="False" />
												<asp:ControlParameter ControlID="txtStudentName" Name="asStudentName" Type="String"
													PropertyName="Text" DefaultValue=" " />
                                                <asp:ControlParameter Name="aiAdmissionStatusId" Type="Int16" ControlID="cmbStatus"
													PropertyName="SelectedValue" DefaultValue="False" />    
                                                 <asp:ControlParameter Name="abIsAdmitted" ControlID="rbSuccessful" Type="Boolean"
                                                    PropertyName="Checked" />  
                                                <asp:ControlParameter Name="aiAdmissionForId" Type="Int16" ControlID="hidAdmissionFor"
													PropertyName="Value" DefaultValue="0" />
                                                <asp:ControlParameter Name="asAdmissionStartDate" Type="String" ControlID="txtAdmissionStartDate" PropertyName="text" DefaultValue= "" />
                                                <asp:ControlParameter Name="asAdmissionEndDate" Type="String" ControlID="txtAdmissionEndDate" PropertyName="text" DefaultValue= "" />                             
											</SelectParameters>
										</asp:ObjectDataSource>
									</td>
								</tr>
								<tr>
									<td>
										<asp:HiddenField ID="hidSortDirection" runat="server"></asp:HiddenField>
										<asp:HiddenField ID="hidSortExpression" runat="server"></asp:HiddenField>
										<asp:HiddenField ID="hidNextAcademiYearId" runat="server"></asp:HiddenField>
										<asp:HiddenField ID="hidIsConfigured" runat="server"></asp:HiddenField>  
									</td>
								</tr>
							</table>
                            <table>
                            <tr >
                                <td align="center" valign=middle>
                                    <asp:Label ID="lblEnqError" runat="server" CssClass="LblErrorMsg" Visible=false></asp:Label>
                                </td>
                            </tr>
                            </table>
                             <table width="100%" id="tblEnquiry">
								<tr id ="trenq" runat="server">
									<td align="center">
										<asp:DataPager ID="DataPager1" runat="server" PageSize="20" PagedControlID="lstviewEnquiryDetails">
											<Fields>
												<asp:TemplatePagerField>
													<PagerTemplate>
														<asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " />
														<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " />
														<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
															CssClass="LblNrmlB" />
														<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " />
														<br />
													</PagerTemplate>
												</asp:TemplatePagerField>
											</Fields>
										</asp:DataPager>
									</td>
								</tr>
                                <tr>
                                    <td valign="top">
										<div style="width: 100%;">
											<asp:ListView ID="lstviewEnquiryDetails" runat="server"  OnItemDataBound="lstviewEnquiryDetails_ItemDataBound"
                                            OnDataBound="lstviewEnquiryDetails_DataBound" OnItemCommand ="lstviewEnquiryDetails_ItemCommand" DataSourceID="ObjectDataSource1" ViewStateMode="Enabled" OnSorting="lstviewEnquiryDetails_Sorting"
												 DataKeyNames="Enquiry_No, Standard_Name, Id, Address, FeeAreaName,MobileNumber,StudentName,StatusId,AdmissionFor,IsConfirmed">
												<LayoutTemplate>
													<table width="100%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
														cellspacing="1" class="GridBorder">
														<tr id="trHeader" runat="server" class="ClsGridHeader">
															<th align="center" width="50px">
															</th>
                                                            <th align="center" width="50px" id="thIsConfirmed" runat="server">
																Is Confirmed?
															</th>
															<th id="thEnquiry" runat="server" align="left" class="ClspaddingL" width="175px">
																<asp:LinkButton ID="lnkFormNo" runat="server" CommandName="Sort" CommandArgument="Enquiry_No"
																	ForeColor="Black">Enquiry No.</asp:LinkButton>
															</th>
                                                             <th align="left" class="ClspaddingL" width="100px">
																<asp:LinkButton ID="lnkDate" runat="server" CommandName="Sort" CommandArgument="date"
																	ForeColor="Black">Date</asp:LinkButton>
															</th>
															<th align="left" class="ClspaddingL" width="150px">
																<asp:LinkButton ID="lnkStandardName" runat="server" CommandName="Sort" CommandArgument="Standard_Name"
																	ForeColor="Black">Standard Name</asp:LinkButton>
															</th>
															<th align="left" class="ClspaddingL" >
																<asp:LinkButton ID="lnlStudentName" runat="server" CommandName="Sort" CommandArgument="StudentName"
																	ForeColor="Black">Student Name</asp:LinkButton>
															</th>
                                                            <th id="th1" align="left" class="Clspadding" width="125px">
																Mobile Number
															</th>
                                                            <th align="center" width="100px">
																Status
															</th>
                                                            <th align="center" width="50px">Edit
                                                            </th>
                                                            <th align="center" width="100px" runat="server" id="thEnquiryForm" visible="false">
															     Enquiry Form
															</th>
                                                            <th align="center" width="50px">Delete
                                                            </th>
														</tr>
														<tr id="itemPlaceholder" runat="server">
														</tr>
														<tr class="ClsBorderPager" id="trDataPager">
															<td colspan="10">
																<asp:DataPager ID="DtPgDropDown" runat="server" PageSize="20" PagedControlID="lstviewEnquiryDetails">
																	<Fields>
																		<asp:TemplatePagerField>
																			<PagerTemplate>
																				<table width="100%">
																					<tr>
																						<td>
																							<asp:Label ID="lblMessage" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
																							<asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="ddlEnqCnt_SelectedIndexChanged">
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
													<tr class="ClsGridRow"  id="trEnqItemWise" runat="server">
														<td align="center">
															<asp:HyperLink ID="lnkbtnAdmsn" runat="server" Text="Register" />
                                                            <asp:LinkButton ID="lnkbtnPay" CommandName="Paid" runat="server" Text="Pay" Visible="false"></asp:LinkButton>
                                                            <asp:HiddenField ID="hidQueryString" runat="server" />
														</td>
                                                        <td align="center" id="tdSelect" runat="server">
															<asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif"/>
															<asp:CheckBox ID="chkIsConfirm" runat="server" />
                                                            <asp:Label ID="lblDash" runat="server" Text="-"></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Enquiry_No")%>'></asp:Label>
														</td>
                                                        <td class="ClspaddingL">
														<asp:Label ID="Label1" runat="server" Text='<%#Eval("date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
														</td>
                                                        <td class="ClspaddingL">
															<asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber")%>'></asp:Label>
														</td>
                                                        <td align="center">															
                                                            <asp:LinkButton ID="lnkStatuss" runat="server" Text='<%# Eval("Status")%>'></asp:LinkButton>
														</td>
                                                       <td align="center">
                                                             <asp:HyperLink ID="lnkEditEnquiryDetails" runat="server" Text="Edit">
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td align="center" runat="server" id="tdEnquiryForm" visible="false">
															<asp:HyperLink ID="lnkbtnEnquiryForm" runat="server" Text="Enquiry Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand"
                                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                                        </td>
													</tr>
												</ItemTemplate>
												<AlternatingItemTemplate>
													<tr id="trEnqItemWise" runat="server" class="ClsGridAltRow">
														<td align="center">
															<asp:HyperLink ID="lnkbtnAdmsn" runat="server" Text="Register" /> 
                                                            <asp:LinkButton ID="lnkbtnPay" CommandName="Paid" runat="server" Text="Pay" Visible="false"></asp:LinkButton>                                                           
                                                            <asp:HiddenField ID="hidQueryString" runat="server" />
														</td>
                                                        <td align="center" id="tdSelect" runat="server">
															<asp:Image ID="imgConfirm" runat="server" ImageUrl="../images/IconGrid_AssignTrue.gif" />
															<asp:CheckBox ID="chkIsConfirm" runat="server" />
                                                            <asp:Label ID="lblDash" runat="server" Text="-"></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblFormNo" runat="server" Text='<%# Eval("Enquiry_No")%>'></asp:Label>
														</td>
                                                         <td class="ClspaddingL">
															<asp:Label ID="Label1" runat="server" Text='<%#Eval("date" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStdName" runat="server" Text='<%# Eval("Standard_Name")%>'></asp:Label>
														</td>
														<td class="ClspaddingL">
															<asp:Label ID="lblStudentdName" runat="server" Text='<%# Eval("StudentName")%>'></asp:Label>
														</td>
                                                        <td class="ClspaddingL">
															<asp:Label ID="lblMobileNumber" runat="server" Text='<%# Eval("MobileNumber")%>'></asp:Label>
														</td>
                                                        <td align="center">															
                                                            <asp:LinkButton ID="lnkStatuss" runat="server" Text='<%# Eval("Status")%>'></asp:LinkButton>
														</td>
                                                        
                                                        <td align="center">
                                                              <asp:HyperLink ID="lnkEditEnquiryDetails" runat="server" Text="Edit">
                                                            </asp:HyperLink>
                                                        </td>
                                                        <td align="center" runat="server" id="tdEnquiryForm" visible="false">
															<asp:HyperLink ID="lnkbtnEnquiryForm" runat="server" Text="Enquiry Form" NavigateUrl="AdmissionFormReport.aspx" />
														</td>
                                                        <td align="center">
                                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="DeleteCommand"
                                                                ImageUrl="../images/IconGrid_Delete.gif" />
                                                        </td>
													</tr>
												</AlternatingItemTemplate>
                                                <EmptyDataTemplate>
													<tr>
														<td class="LblNoRecord" align="center">
															No record found.
														</td>
													</tr>
												</EmptyDataTemplate>
											</asp:ListView>
										</div>
									</td>
                                </tr>
                                <tr>
									<td>
                                        <asp:HiddenField ID="HidenqSortDirection" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="HidEnqSortExprsn" runat="server"></asp:HiddenField>
                                        <asp:HiddenField ID="hidAdmissionFor" runat="server" Value="0"></asp:HiddenField>
									</td>
								</tr>
                                <tr>
									<td>
										<asp:ObjectDataSource TypeName="BusinessLogic.SchoolEnquiryBL" EnablePaging="true"
											ID="ObjectDataSource1" runat="server" SelectMethod="GetAllStuEnquiryDetails" SortParameterName="sortExpression" SelectCountMethod="CountAllNewStudentEnqDetails" 
											EnableCaching="false">
											<SelectParameters>
												<asp:SessionParameter Name="aiSchoolID" SessionField="I_SCHOOL_ID" Type="int32" />
												<asp:ControlParameter Name="aiAcademicYearID" Type="Int16" ControlID="hidNextAcademiYearId"
													PropertyName="Value" DefaultValue="0" />
												<asp:ControlParameter Name="aiStandardID" Type="Int16" ControlID="ddlStandard" PropertyName="SelectedValue"
													DefaultValue="0" />
												<asp:ControlParameter Name="aiAdmissionType" Type="Int16" ControlID="ddlAdmissionType"
													PropertyName="SelectedValue" DefaultValue="False" />
                                                     <asp:ControlParameter Name="aiAdmissionStatusId" Type="Int16" ControlID="cmbStatus"
													PropertyName="SelectedValue" DefaultValue="False" />  
                                                <asp:ControlParameter Name="aiLocationId" Type="Int16" ControlID="cmbSchoolLocation"
                                                    PropertyName="SelectedValue" DefaultValue="False" /> 
												<asp:ControlParameter ControlID="txtStudentName" Name="asStudentName" Type="String"
													PropertyName="Text" DefaultValue=" " />
                                                <asp:ControlParameter Name="aiAdmissionFor" Type="Int16" ControlID="hidAdmissionFor"
													PropertyName="Value" DefaultValue="0" />
                                                    <asp:ControlParameter  Name="asStartDate" Type="string" ControlID="txtEnquiryStartDate" PropertyName="text" DefaultValue= "" />
                                                     <asp:ControlParameter  Name="asEndDate" Type="string" ControlID="txtEnquiryEndDate" PropertyName="text"  DefaultValue= ""/>
											</SelectParameters>
										</asp:ObjectDataSource>
									</td>
								</tr>
                              </table>
						</ContentTemplate>
						<Triggers>
							<asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
							<asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="Sorting" />
							<asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstvwStudentDetails" EventName="DataBound" />
                            <asp:AsyncPostBackTrigger ControlID="lstviewEnquiryDetails" EventName="ItemCommand" />
                            <asp:AsyncPostBackTrigger ControlID="lstviewEnquiryDetails" EventName="DataBound" />                           
						</Triggers>
					</asp:UpdatePanel>
				</td>
			</tr>
			<tr>
				<td align="center" colspan="6" style="height: 20px; padding-top: 5px">
					<asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ClsBtn" BorderWidth="1px"
						CausesValidation="False" onclick="btnAdd_Click" />
					<asp:Button ID="btnConfirm" runat="server" Text="Confirm" CssClass="ClsBtn" BorderWidth="1px"
						CausesValidation="False" Visible="true"  />  
                        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" BorderWidth="1px"
						CausesValidation="False" Visible="true" onclick="btnExport_Click"  />
                   <asp:Button ID="btnExportEnq" runat="server" Text="Export" CssClass="ClsBtn" BorderWidth="1px" 
						CausesValidation="False" Visible="false" onclick="btnExportEnq_Click"/>          
                    <asp:Button ID="btnAddEnquiry" runat="server" Text="Add Enquiry" 
                        CssClass="ClsBtn" BorderWidth="1px" Visible="false"
						CausesValidation="False" onclick="btnAddEnquiry_Click" />
                    <asp:HiddenField ID="hidStudentAdmissionId" runat="server" onvaluechanged="hidStudentAdmissionId_ValueChanged"></asp:HiddenField>                                                            
				</td>
			</tr>
		</table>
	</div>
	<script type="text/javascript" language="javascript">

		_clientddlStandard = "<%=this.ddlStandard.ClientID %>"
		_clientddlAdmissionType = "<%=this.ddlAdmissionType.ClientID %>"
		_clienttxtStudentName = "<%=this.txtStudentName.ClientID %>"
		_clientlstvwGroup = '<%= lstvwStudentDetails.ClientID %>'
		_clientlstviewEnquiryDetails = "<%= lstviewEnquiryDetails.ClientID %>"
		_clientcmbStatus = "<%=this.cmbStatus.ClientID %>"
		_clienthidStudentAdmissionId = "<%=this.hidStudentAdmissionId.ClientID %>";
		_cltdivAttendanceAlert = "<%=this.divDivisionConfirmation.ClientID %>";
		_ClienttxtStartDate = "<%=this.txtEnquiryStartDate.ClientID %>";
		_ClienttxtEndDate = "<%=this.txtEnquiryEndDate.ClientID %>";
		_ClienttxtAdmissionStartDate = "<%=this.txtAdmissionStartDate.ClientID %>";
		_ClienttxtAdmissionEndDate = "<%=this.txtAdmissionEndDate.ClientID %>";
		var cssstyle = $get("<%=this.divDivisionConfirmation.ClientID %>").style
		var standardname = document.getElementById("<%=this.lblStandardName.ClientID %>");
		var ddlReport = document.getElementById("<%=ddlStandard.ClientID%>");
		var divconfirm = document.getElementById("<%=this.divDivisionConfirmation.ClientID %>")


		function ClearContriols() {		    
		    document.getElementById(_clientddlAdmissionType).value = "0";
		    document.getElementById(_clientddlStandard).value = "0";
		    document.getElementById(_clienttxtStudentName).value = "";
		    $get(_clientcmbStatus).value = "0";

		    if (document.getElementById(_ClienttxtStartDate)!= null)
		        document.getElementById(_ClienttxtStartDate).value = "";

		    if (document.getElementById(_ClienttxtEndDate)!= null)
		        document.getElementById(_ClienttxtEndDate).value = "";

		    if (document.getElementById(_ClienttxtAdmissionStartDate) != null)
		        document.getElementById(_ClienttxtAdmissionStartDate).value = "";

		    if (document.getElementById(_ClienttxtAdmissionEndDate) != null)
		        document.getElementById(_ClienttxtAdmissionEndDate).value = "";
		    return false;
		}
		function OpenReceiptPopup(rowIndex) {
		    var queryString = document.getElementById(_clientlstviewEnquiryDetails + "_ctrl" + rowIndex + "_hidQueryString").value
		    window.open('AdmissionFormReport.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=500,height=400')
		}

		function OpenEnquiryStatusPopup(rowIndex){
		    var queryString = document.getElementById(_clientlstviewEnquiryDetails + "_ctrl" + rowIndex + "_hidQueryString").value
		    window.open('EnquiryStatusPopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=850,height=700');
		}

		function openNewReport(url) {
		    window.open(url, '_blank', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=800,height=300');
		}

    </script> 
    <script src="../Scripts/Admission/NewAdmissionStudentListUI.js" type="text/javascript"></script>
</asp:Content>
