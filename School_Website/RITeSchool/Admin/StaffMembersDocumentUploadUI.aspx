<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="StaffMembersDocumentUploadUI.aspx.cs" Inherits="StaffMembersDocumentUploadUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
	<table width="1200px">
		<tr>
			<td colspan="4">
				<asp:UpdatePanel runat="server" UpdateMode="Always">
					<ContentTemplate>
						<asp:HiddenField ID="hidItemCount" runat="server" OnValueChanged="HidItemCount_ValueChanged" />
						<asp:Label ID="lblDuplicateDetails" CssClass="ClsMdtStar" Visible="false" runat="server"></asp:Label>
					</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td colspan="4">
			<asp:UpdatePanel ID="UpdatePanel10" runat="server" UpdateMode="Always">
					<ContentTemplate>
				<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="valGrpExpDetails"
					HeaderText="Please fix following error(s):" CssClass="ClsLabel" />
				<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="valGrpAddEduDetails"
					HeaderText="Please fix following error(s):" CssClass="ClsLabel" />
						</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight">
				<span class="ClsLabel">User Role : </span>
			</td>
			<td>
				<asp:UpdatePanel runat="server" UpdateMode="Always">
					<ContentTemplate>
						<asp:DropDownList ID="cmbUserRole" runat="server" CssClass="LrgCombo" AutoPostBack="true"
							OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
						</asp:DropDownList>
						<span class="ClsMdtStar">*</span>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" Display="None"
							CssClass="ClsLabel" ErrorMessage="User role should be selected." ValidationGroup="valGrpExpDetails"
							ControlToValidate="cmbUserRole" InitialValue="0"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" Display="None"
							CssClass="ClsLabel" ErrorMessage="User role should be selected." ValidationGroup="valGrpAddEduDetails"
							ControlToValidate="cmbUsers" InitialValue="0"></asp:RequiredFieldValidator></ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td class="ClsBorderlight">
				<span class="ClsLabel">User Name : </span>
			</td>
			<td>
				<asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
					<ContentTemplate>
						<asp:DropDownList ID="cmbUsers" runat="server" CssClass="LrgCombo" AutoPostBack="true"
							OnSelectedIndexChanged="cmbUsers_SelectedIndexChanged">
						</asp:DropDownList>
						<span class="ClsMdtStar">*</span>
						<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Display="None"
							CssClass="ClsLabel" ErrorMessage="User should be selected." ValidationGroup="valGrpExpDetails"
							ControlToValidate="cmbUsers" InitialValue="0"></asp:RequiredFieldValidator>
						<asp:RequiredFieldValidator ID="reqUser" runat="server" Display="None" CssClass="ClsLabel"
							ErrorMessage="User should be selected." ValidationGroup="valGrpAddEduDetails"
							ControlToValidate="cmbUsers" InitialValue="0"></asp:RequiredFieldValidator>
					</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" colspan="4" style="height: 19px" class="ClsBtmBorderGray">
				<span class="ClsLblLgnd" style="width: 200px; font: Bold">Experience Details : </span>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" style="width: 25%;">
				<asp:CustomValidator ID="cstJoinResignDate" Display="None" runat="server" CssClass="ClsMdtStar"
					Visible="true" ErrorMessage="" EnableClientScript="true"></asp:CustomValidator>
			</td>
		</tr>
		<%--	<tr>
			<td align="left" style="width: 20%; height: 41px;" valign="top" class="ClsBorderlight">
				<span class="ClsLabel">Achievements : </span>
			</td>
			<td align="left" colspan="3" style="height: 41px">
				<asp:TextBox ID="txtAchivements" runat="server" TextMode="MultiLine" Width="578px"></asp:TextBox>
				<asp:RegularExpressionValidator ID="Regu_Vali_Achiv" runat="server" Display="None"
					ControlToValidate="txtAchivements" ErrorMessage="Length of achievement should not exceed 300 charecters."
					ValidationExpression="^[\s\S]{0,300}$"> </asp:RegularExpressionValidator>
			</td>
		</tr>--%>
		<%--<tr>
			<td align="left" style="width: 20%">
			</td>
			<td align="left" style="width: 25%">
				<span class="LblSmlGray">(Years)</span> <span class="LblSmlGray">(Months)</span>
			</td>
			<td align="left" style="width: 15%">
			</td>
			<td align="left" style="width: 23%">
			</td>
		</tr>--%>
		<%--	<tr>
			<td align="left" style="width: 20%;" class="ClsBorderlight">
				<span class="ClsLabel">Past Experience : </span>
			</td>
			<td align="left" style="width: 25%;">
				<asp:TextBox ID="txtExpYears" MaxLength="2" CssClass="ExSmlTxtBox" runat="server"
					onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
					onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
					ondrop="event.returnValue=false" Text="00" />
				<asp:TextBox ID="txtExpMonths" CssClass="ExSmlTxtBox" runat="server" MaxLength="2"
					onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
					onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
					ondrop="event.returnValue=false" Text="00" />
			</td>
			<td align="left" style="width: 15%;">
			</td>
			<td align="left" style="width: 23%;">
			</td>
		</tr>--%>
		<tr>
			<td align="left" style="width: 20%;" class="ClsBorderlight">
				<span class="ClsLabel">School Name : </span>
			</td>
			<td align="left" style="width: 23%;" colspan="3">
			<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel9">
					<ContentTemplate>
				<asp:TextBox ID="txtSchoolname" runat="server" MaxLength="100" CssClass="MidTxtBox"
					Width="578px" CausesValidation="true"></asp:TextBox>
				<span class="ClsMdtStar">*</span>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSchoolname"
					Display="None" ErrorMessage="School Name should not be blank." ValidationGroup="valGrpExpDetails"></asp:RequiredFieldValidator>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;" class="ClsBorderlight">
				<span class="ClsLabel">Joined Date : </span>
			</td>
			<td align="left" style="width: 25%;">
				<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel8">
					<ContentTemplate>
						<asp:TextBox ID="txtjoinedDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
						<rjs:PopCalendar ID="calender_JoinDate" runat="server" Control="txtjoinedDate" To-Today="true"
							Enabled="true" ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
							To-Message="Please select valid Joined Date." From-Message="Please select valid Joined Date."
							ShowWeekend="True" InvalidDateMessage="Please select valid Joined Date." />
						<span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ControlToValidate="txtjoinedDate"
							ID="ReqJoinDate" runat="server" ErrorMessage="Joined Date should not be blank."
							ValidationGroup="valGrpExpDetails" Display="None"></asp:RequiredFieldValidator>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
			<td align="left" style="width: 15%;" class="ClsBorderlight">
				<span class="ClsLabel">Left Date : </span>
			</td>
			<td align="left" style="width: 25%;">
				<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel7">
					<ContentTemplate>
						<asp:TextBox ID="txtLeftDate" runat="server" MaxLength="100" CssClass="SmlCombo"></asp:TextBox>
						<rjs:PopCalendar ID="calender_LeftDate" runat="server" Control="txtLeftDate" To-Today="true"
							ValidationGroup="valGrpExpDetails" ShowErrorMessage="false" Format="dd MMM yyyy"
							To-Message="Please select valid Left Date." From-Message="Please select valid Left Date."
							ShowWeekend="True" Enabled="true" InvalidDateMessage="Please select valid Left Date." />
						<span class="ClsMdtStar">*</span><asp:RequiredFieldValidator ControlToValidate="txtLeftDate"
							ID="RequiredFieldValidator3" runat="server" ValidationGroup="valGrpExpDetails"
							ErrorMessage="Left Date should not be blank." Display="None"></asp:RequiredFieldValidator>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" style="" colspan="2">
				<asp:UpdatePanel runat="server">
					<ContentTemplate>
						<asp:Button ID="btnAdd" runat="server" Text="Add Details " OnClick="btnAdd_Click"
							CssClass="ClsBtnMid" BorderStyle="Solid" ValidationGroup="valGrpExpDetails" BorderWidth="1px" />
						<asp:Button ID="btnCancelDetails" runat="server" BorderStyle="Solid" BorderWidth="1px"
							CausesValidation="False" CssClass="ClsBtnSml" Text="Cancel" />
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<asp:CustomValidator ID="cstForm" Display="None" runat="server" CssClass="ClsMdtStar"
			Visible="true" ErrorMessage="Left Date should not futuredate." ClientValidationFunction="ValidateControls"
			ValidationGroup="valGrpExpDetails"></asp:CustomValidator>
		<tr>
			<td align="center" colspan="4">
				<asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
					<ContentTemplate>
						<asp:ListView ID="lstvwExpDetails" runat="server" DataKeyNames="Id" OnItemDataBound="lstvwExpDetails_ItemDataBound"
							OnItemCommand="lstvwExpDetails_ItemCommand">
							<LayoutTemplate>
								<table align="center" width="100%" runat="server" id="tblExperienceInfo" style="color: #333333"
									cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
									<tr id="trHeader" runat="server" class="ClsGridHeader">
										<th align="left" width="40%" class="paddingL">
											School Name
										</th>
										<th align="center">
											Joined Date
										</th>
										<th align="center">
											Left Date
										</th>
										<th>
											Count
										</th>
										<th align="center">
											Edit
										</th>
										<th align="center">
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
										<asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkAttachmentCnt" runat="server" Text='<%# Eval("AttachmentCount") %>'>LinkButton</asp:LinkButton>
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
											ImageUrl="../images/IconGrid_Edit.GIF" />
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
											ImageUrl="../images/IconGrid_Delete.gif" />
									</td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
								<tr id="Tr3" runat="server" class="ClsGridAltRow">
									<td align="left" class="paddingL">
										<asp:Label ID="lblName" runat="server" Text='<%# Eval(" SchoolName") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval(" JoiningDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval(" leftDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkAttachmentCnt" runat="server" Text='<%# Eval("AttachmentCount") %>'>LinkButton</asp:LinkButton>
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
											ImageUrl="../images/IconGrid_Edit.GIF" />
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnDelete" CommandName="RemoveCommand" CausesValidation="false"
											runat="server" ImageUrl="../images/IconGrid_Delete.gif" />
									</td>
								</tr>
							</AlternatingItemTemplate>
							<EmptyDataTemplate>
								<tr>
									<td align="center" colspan="4">
										<div style="width: 100%;" class="LblNoRecord">
											No record found.</div>
									</td>
								</tr>
							</EmptyDataTemplate>
						</asp:ListView>
						<asp:HiddenField ID="hidExpDetailsId" Value="0" runat="server" />
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="hidItemCount" EventName="ValueChanged" />
						<asp:AsyncPostBackTrigger ControlID="btnAdd" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="lstvwExpDetails" EventName="ItemDataBound" />
						<asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
						<asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" style="width: 25%;">
				<%--	<asp:CompareValidator ID="cmp_ExpYears" runat="server" ControlToValidate="txtExpYears"
					Display="None" ErrorMessage="Past Experience in years should be less than 60."
					Operator="LessThanEqual" Type="Integer" ValueToCompare="60"></asp:CompareValidator>--%>
			</td>
			<td align="left" style="width: 15%;">
			</td>
			<td align="left" style="width: 23%;">
			</td>
		</tr>
		<tr>
			<td align="left" colspan="4" class="ClsBtmBorderGray">
				<span class="ClsLblLgnd" style="width: 200px; font: Bold">Educational Information :</span>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" style="width: 25%;">
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;" class="ClsBorderlight">
				<span class="ClsLabel">Qualification: </span>
			</td>
			<td align="left" style="width: 25%;">
				<asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel2">
					<ContentTemplate>
						<asp:DropDownList ID="cmbQualification" runat="server" CssClass="MidTxtBox">
						</asp:DropDownList>
						<span class="ClsMdtStar">*
						</span>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwEducationDetails" EventName="ItemCommand" />						
					</Triggers>
				</asp:UpdatePanel>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" Display="None"
							CssClass="ClsLabel" ErrorMessage="Qualification should be selected." ValidationGroup="valGrpAddEduDetails"
							ControlToValidate="cmbQualification" InitialValue="0"></asp:RequiredFieldValidator>
			</td>
			<td align="left" style="width: 15%;" class="ClsBorderlight">
				<span class="ClsLabel">Year of Passing : </span>
			</td>
			<td align="left" style="width: 23%;">
				<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel1">
					<ContentTemplate>
						<asp:TextBox ID="txtYearOfPassing" CssClass="SmlTxtBox" runat="server" MaxLength="4"
							onblur="extractNumber(this,0,false);" onkeyup="extractNumber(this,0,false);"
							onkeypress="return blockNonNumbers (this, event, false, false);" onpaste="event.returnValue=false"
							ondrop="event.returnValue=false" />
						<span class="ClsMdtStar" style="color: #ff0000">*&nbsp;</span>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwEducationDetails" EventName="ItemCommand" />
					</Triggers>
				</asp:UpdatePanel>
				<asp:CustomValidator ID="cst_YearOfPassing" runat="server" ValidationGroup="valGrpAddEduDetails"
					ClientValidationFunction="YearValidation" ControlToValidate="txtYearOfPassing"
					Display="None"></asp:CustomValidator>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtYearOfPassing"
					Display="None" ErrorMessage="Year of passing should not be blank." ValidationGroup="valGrpAddEduDetails"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;" class="ClsBorderlight">
				<span class="ClsLabel">Class : </span>
			</td>
			<td align="left" style="width: 25%;">
				<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel6">
					<ContentTemplate>
						<asp:DropDownList ID="cmbPassingClass" runat="server" CssClass="LrgTxtBox">
						</asp:DropDownList>
							<span class="ClsMdtStar">*
						</span>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwEducationDetails" EventName="ItemCommand" />
					</Triggers>
				</asp:UpdatePanel>
				<asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" Display="None"
							CssClass="ClsLabel" ErrorMessage="Class should be selected." ValidationGroup="valGrpAddEduDetails"
							ControlToValidate="cmbPassingClass" InitialValue="0"></asp:RequiredFieldValidator>

				<%--<asp:CompareValidator ID="CompareValidator1" runat="server"
							ControlToValidate="cmbPassingClass" Display="None" ErrorMessage="Class should be selected."
							Operator="NotEqual" ValueToCompare="0" ValidationGroup="valGrpAddEduDetails"></asp:CompareValidator>--%>
			</td>
			<td align="left" style="width: 15%;" class="ClsBorderlight">
				<span class="ClsLabel">University : </span>
			</td>
			<td align="left" style="width: 23%;">
				<asp:UpdatePanel ChildrenAsTriggers="false" UpdateMode="Conditional" runat="server"
					ID="UpdatePanel3">
					<ContentTemplate>
						<asp:TextBox ID="txtPassingUniversity" runat="server" MaxLength="100" CssClass="MidTxtBox"></asp:TextBox>
						<span class="ClsMdtStar"><span style="color: #ff0000">*&nbsp;</span> </span>
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="lstvwEducationDetails" EventName="ItemCommand" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
				<asp:CustomValidator ID="cstValGrid" runat="server" Display="None" ErrorMessage="At least one educational information should be added."
					ClientValidationFunction="validateGridData" ValidationGroup="Save"></asp:CustomValidator>
			</td>
			<td align="left" style="" colspan="2">
				<asp:UpdatePanel ID="upnlButtons" runat="server">
					<ContentTemplate>
						<asp:Button ID="btnAddDetails" runat="server" Text="Add Details " OnClick="btnAddDetails_Click"
							CssClass="ClsBtnMid" BorderStyle="Solid" ValidationGroup="valGrpAddEduDetails"
							BorderWidth="1px" />
						<asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="False"
							CssClass="ClsBtnSml" Text="Cancel" />
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="lstvwEducationDetails" EventName="ItemCommand" />
						<asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
			<td align="left" style="width: 23%;">
				<asp:RequiredFieldValidator ID="req_PassingUniversity" runat="server" ControlToValidate="txtPassingUniversity"
					Display="None" ErrorMessage="University should not be blank." ValidationGroup="valGrpAddEduDetails"></asp:RequiredFieldValidator>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" colspan="2" style="">
			</td>
			<td align="left" style="width: 23%;">
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" colspan="2" style="">
			</td>
			<td align="left" style="width: 23%;">
			</td>
		</tr>
		<tr>
			<td align="center" colspan="4">
				<asp:UpdatePanel UpdateMode="Conditional" runat="server">
					<ContentTemplate>
						<asp:ListView ID="lstvwEducationDetails" runat="server" DataKeyNames="Id" OnItemCommand="lstvwEducationDetails_ItemCommand"
							OnItemDataBound="lstvwEducationDetails_ItemDataBound">
							<LayoutTemplate>
								<table align="center" width="100%" runat="server" id="tblExperienceInfo" style="color: #333333"
									cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
									<tr id="trHeader" runat="server" class="ClsGridHeader">
										<th align="left" width="40%" class="paddingL">
											Qualification
										</th>
										<th align="center">
											Year of Passing
										</th>
										<th align="center">
											University
										</th>
										<th align="center">
											Class
										</th>
										<th>
											Count
										</th>
										<th align="center">
											Edit
										</th>
										<th align="center">
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
										<asp:Label ID="lblName" runat="server" Text='<%# Eval("Qualification.Name") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval("YearOfPassing") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval("University") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblClass" runat="server" Text='<%# Eval(" Class") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkAttachmentCnt" runat="server" Text='<%# Eval("AttachmentCount") %>'>LinkButton</asp:LinkButton>
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
											ImageUrl="../images/IconGrid_Edit.GIF" />
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
											ImageUrl="../images/IconGrid_Delete.gif" />
									</td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
								<tr id="Tr3" runat="server" class="ClsGridAltRow">
									<td align="left" class="paddingL">
										<asp:Label ID="lblName" runat="server" Text='<%# Eval("Qualification.Name") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval("YearOfPassing") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblLeftDate" runat="server" Text='<%# Eval("University") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:Label ID="lblClass" runat="server" Text='<%# Eval(" Class") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkAttachmentCnt" runat="server" Text='<%# Eval("AttachmentCount") %>'>LinkButton</asp:LinkButton>
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
											ImageUrl="../images/IconGrid_Edit.GIF" />
									</td>
									<td align="center">
										<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
											ImageUrl="../images/IconGrid_Delete.gif" />
									</td>
								</tr>
							</AlternatingItemTemplate>
							<EmptyDataTemplate>
								<tr>
									<td align="center" colspan="4">
										<div style="width: 100%;" class="LblNoRecord">
											No record found.</div>
									</td>
								</tr>
							</EmptyDataTemplate>
						</asp:ListView>
						<asp:HiddenField ID="hidEducationId" Value="0" runat="server" />
					</ContentTemplate>
					<Triggers>
						<asp:AsyncPostBackTrigger ControlID="btnAddDetails" EventName="Click" />
						<asp:AsyncPostBackTrigger ControlID="cmbUserRole" EventName="SelectedIndexChanged" />
						<asp:AsyncPostBackTrigger ControlID="cmbUsers" EventName="SelectedIndexChanged" />
						<asp:AsyncPostBackTrigger ControlID="hidItemCount" EventName="ValueChanged" />
					</Triggers>
				</asp:UpdatePanel>
			</td>
		</tr>
		<tr>
			<td align="left" colspan="4" class="ClsBtmBorderGray">
				<span class="ClsLblLgnd" style="width: 200px; font: Bold">Other Details :</span>
			</td>
		</tr>
		<tr>
			<td align="left" style="width: 20%;">
			</td>
			<td align="left" style="width: 25%;">
			</td>
		</tr>
		<tr>
			<td align="center" colspan="4">
				<asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
					<ContentTemplate>
						<asp:ListView ID="lstvwUserDocuments" runat="server" DataKeyNames="Id,DocumentTypeId"
							OnItemDataBound="lstvwUserDocuments_ItemDataBound">
							<LayoutTemplate>
								<table align="center" width="100%" runat="server" id="tblExperienceInfo" style="color: #333333"
									cellpadding="0" cellspacing="1" class="GridBorder" datapagesize="20">
									<tr id="trHeader" runat="server" class="ClsGridHeader">
										<th align="left" class="ClspaddingL">
											Document Name
										</th>
										<th align="center">
											AttachmentCount
										</th>
									</tr>
									<tr runat="server" id="itemPlaceholder">
									</tr>
								</table>
							</LayoutTemplate>
							<ItemTemplate>
								<tr id="Tr2" runat="server" class="ClsGridRow">
									<td align="left" class="ClspaddingL">
										<asp:Label ID="lblJoinDate" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkDocumentCount" runat="server" Text='<%# Eval("DocumentCount") %>'>LinkButton</asp:LinkButton>
									</td>
								</tr>
							</ItemTemplate>
							<AlternatingItemTemplate>
								<tr id="Tr3" runat="server" class="ClsGridAltRow">
									<td align="left" class="ClspaddingL">
										<asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
									</td>
									<td align="center">
										<asp:LinkButton ID="lnkDocumentCount" runat="server" Text='<%# Eval("DocumentCount") %>'>LinkButton</asp:LinkButton>
									</td>
								</tr>
							</AlternatingItemTemplate>
							<EmptyDataTemplate>
								<tr>
									<td align="center" colspan="4">
										<div style="width: 100%;" class="LblNoRecord">
											No record found.</div>
										.
									</td>
								</tr>
							</EmptyDataTemplate>
						</asp:ListView>
					</ContentTemplate>
				</asp:UpdatePanel>
			</td>
		</tr>
	</table>
	<script type="text/javascript">
		_clienttxtJoinDate = "<%=this.txtjoinedDate.ClientID %>"
		_clienttxtEndDate = "<%=this.txtLeftDate.ClientID %>"
		_clientbtnCancel = "<%=this.btnCancel.ClientID %>"
		_clienttxtPassingUniversity = "<%=this.txtPassingUniversity.ClientID %>"
		_clientcmbPassingClass = "<%=this.cmbPassingClass.ClientID %>"
		_clienttxtYearOfPassing = "<%=this.txtYearOfPassing.ClientID %>"
		_clientcmbQualification = "<%=this.cmbQualification.ClientID %>"
		_clienbtnCancelDetails = "<%=this.btnCancelDetails.ClientID %>"
		_clienttxtSchoolname = "<%=this.txtSchoolname.ClientID %>"
		_clienthidItemCount = "<%=this.hidItemCount.ClientID %>";

		$("#" + _clientbtnCancel).click(function () {
			$("#" + _clienttxtPassingUniversity).val('');
			$("#" + _clienttxtYearOfPassing).val('');
			$("#" + _clientcmbPassingClass)
		});

		$("#" + _clienbtnCancelDetails).click(function () {
			$("#" + _clienttxtJoinDate).val('');
			$("#" + _clienttxtEndDate).val('');
			$("#" + _clienttxtSchoolname).val('');
		})

		function ValidateControls(oSrc, args) {
			var JoinDate
			var LeftDate

			if (document.all) {
				JoinDate = new Date((document.getElementById(_clienttxtJoinDate).value).replace('-', ' '))
				LeftDate = new Date((document.getElementById(_clienttxtEndDate).value).replace('-', ' '))
			}
			else {
				JoinDate = new Date(document.getElementById(_clienttxtJoinDate).value.replace(/-/g, ' '))
				LeftDate = new Date(document.getElementById(_clienttxtEndDate).value.replace(/-/g, ' '))
			}
			if (JoinDate > LeftDate) {
				oSrc.errormessage = "Left Date should be greater than Joined Date."
				args.IsValid = false
				return true
			}
		}

		
		function UpdateFileUploadCount(ItemCount) {
			document.getElementById(_clienthidItemCount).value = ItemCount;
			__doPostBack(document.getElementById(_clienthidItemCount).name, '')
		}
		// This fuvtion is sued to open upload attachment pop up
		function OpenPopup(querystring) {
			window.open('../Payroll/InvestmentDocumentPopup.aspx?' + querystring, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=800,height=500')
			return false;
		}

		//This function is used to display confirmation message to delete educational details.
		function ConfirmDelete() {
			var bResult = true;
			if (!window.confirm("Are you sure you want to delete following educational details?"))
				bResult = false;
			return bResult;
		}

		// This function is used to sgow confirmation messge for deleting experience details.
		function DeleteExpDetails() {
			var bResult = true
			if (!window.confirm('Are you sure you want to delete following experience details?')) {
				bResult = false
			}
			return bResult
		}
	</script>
</asp:Content>
