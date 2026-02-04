<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master" AutoEventWireup="true" CodeFile="ManagementFileSharingUI.aspx.cs" Inherits="ManagementFileSharingUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" Runat="Server">
<table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%; vertical-align: top">
	<tr id="trFileHeader" runat="server">
        <td align="center" style="height: 20px; width: 99%; margin-bottom: 5px;" class="ClsGrayMainTitle">
            <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                <tr>
                    <td class="MainTitleHead" style="height: 20px">
                        <span style="font-weight:bold">Files from PPS Pune</span>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
	<tr id="FileUploadRow" runat="server" visible="false">
		<td>
			<asp:UpdatePanel ID="UpdatePanel1"
							 runat="server"
							 UpdateMode="Conditional">
				<ContentTemplate>
					<table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
						<tr>
							<td>
								<asp:Label ID="lblErrorMsg"
										   runat="server"
										   EnableViewState="false"
										   ForeColor="Red"
										   CssClass="ClsMdtStar"
										   style="text-align: left;"> </asp:Label>
							</td>
							<td align="right" class="ClsTextNormal" style="padding-right: 10px; top: 20px; height: 19px;">
								<span class="ClsMdtStar">* Mandatory Fields</span>
							</td>
						</tr>
						<tr>
							<td colspan="2">
								<asp:ValidationSummary ID="valSumErrorMessage"
													   runat="server"
													   ValidationGroup="Upload"
													   CssClass="ClsLabel"
													   ShowSummary="true"/>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<asp:Label ID="lblUpdateMessage"
										   runat="server"
										   EnableViewState="false"
										   ForeColor="Blue"
										   Width="100%"
										   CssClass="ClsLabel"
										   Font-Bold="true"> </asp:Label>
							</td>
						</tr>
						<tr>
							<td align="right" valign="top">
								<table border="0" cellpadding="0" cellspacing="2">
									<tr>
										<td class="ClsBorderlight" valign="middle">
											<span class="ClsLabel" style="width:100%;">Title :</span>
										</td>
										<td>
											<asp:TextBox ID="txtTitle"
														 runat="server"
														 CssClass="LrgTxtBox"
														 MaxLength="100"
														 style="width: 300px; float: left;"> </asp:TextBox>
											<span class="ClsMdtStar">&nbsp;*</span>
											<asp:RequiredFieldValidator ID="reqTitleRequiredValidator"
																		runat="server"
																		ErrorMessage="Title should not be blank."
																		Display="None"
																		ControlToValidate="txtTitle"
																		ValidationGroup="Upload"> </asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr>
										<td  class="ClsBorderlight" valign="middle">
											<span class="ClsLabel" style="width:100%;">Description :</span>
										</td>
										<td>
											<asp:TextBox ID="txtDescription"
														 runat="server"
														 CssClass="LrgTxtBox"
														 style="width: 300px; float: left;"
														 TextMode="MultiLine"
														 Rows="2" MaxLength="100"></asp:TextBox>
											<asp:CustomValidator ID="cstDescriptionVal"
																 runat="server"
																 EnableClientScript="true"
																 ClientValidationFunction="ValidateDescription"
																 ValidationGroup="Upload"
																 Display="None"
																 ErrorMessage="Description can be a maximum of 4000 characters long."
																 SetFocusOnError="True"> </asp:CustomValidator>
										</td>
									</tr>
									<tr>
										<td class="ClsBorderlight" valign="middle">
											<span class="ClsLabel" style="width:100%;">File Path :</span>
										</td>
										<td>
											<asp:FileUpload ID="FileUploadControl"
															runat="server"
															CssClass="LrgTxtBox" />
											<span id="fileRequired" runat="server" class="ClsMdtStar"> * </span>
										</td>
									</tr>
									<tr id="UpdateWarning" runat="server" visible="false">
										<td></td>
										<td><span class="LblSmlGray">(Select a file only if you wish to over-write old file.)</span></td>
									</tr>
									<tr>
										<td class="ClsBorderlight" valign="middle">
											<span class="ClsLabel" style="width:100%;">Send SMS :</span>
										</td>
										<td>
											<asp:CheckBox ID="chkSendSMS"
														  runat="server"
														  Checked="false"/>
										</td>
									</tr>
								</table>
							</td>
							<td align="left" valign="top" style="width: 50%;">
								<table border="0" cellpadding="0" cellspacing="0" style="width: 390px;">
									<tr>
										<td valign="top" style="padding: 0">
											<table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
												<tr>
													<td class="ClsBorderlight" style="padding: 1px 4px;">Select Users for File Sharing:</td>
													<td>
														<span class="ClsMdtStar">&nbsp;*</span>
													</td>
												</tr>
												<tr>
													<td class="ClsBorderlight" style="width: 100%;">
														<div style=" height: 100px; max-height: 100px; overflow: auto;">
															<asp:CheckBoxList ID="chklstSuperAdmin"
																			  runat="server"
																			  style="font-size: 9pt !important;">
															</asp:CheckBoxList>
														</div>
													</td>
													<td valign="top">
														<asp:CustomValidator ID="cstSMSListVal"
																			 runat="server"
																			 EnableClientScript="true"
																			 ClientValidationFunction="ValidateSuperAdminList"
																			 ValidationGroup="Upload"
																			 Display="None"
																			 ErrorMessage="At least one user must be selected from the list."
																			 SetFocusOnError="True"> </asp:CustomValidator>
													</td>
												</tr>
												<tr>
													<td align="left" colspan="2">
														<div id="tdEditProfile" runat="server" class="ClsGreenBG" style="width: 230px; height: 18px; vertical-align: bottom; display: block; padding: 4px 4px 2px;">
															<asp:HyperLink ID="lnkEditProfile"
																		   runat="server"
																		   NavigateUrl="SuperAdminDetailsUI.aspx"
																		   CssClass="SubTitle"
																		   Text="Add/Edit Management User Profile"> </asp:HyperLink>
														</div>
													</td>
												</tr>
											</table>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>
							<td colspan="2" align="center">
								<asp:Button ID="btnUpload"
											runat="server"
											Text="Upload"
											CssClass="ClsBtn"
											ValidationGroup="Upload"
											OnClick="btnUpload_Click" />
								<asp:Button ID="btnUpdate"
											runat="server"
											Text="Update"
											CssClass="ClsBtn"
											Visible="false"
											ValidationGroup="Upload"
											OnClick="btnUpdate_Click" />
								<asp:Button ID="btnCancel"
											runat="server"
											Text="Cancel"
											CssClass="ClsBtn"
											CausesValidation="false"
											OnClick="btnCancel_Click" />
								<asp:CustomValidator ID="cstUploadValidator"
													 runat="server"
													 EnableClientScript="true"
													 ClientValidationFunction="ValidateUpload"
													 ValidationGroup="Upload"
													 Display="None"
													 ErrorMessage="File to upload should be selected."
													 SetFocusOnError="True"> </asp:CustomValidator>
							</td>
						</tr>
					</table>
				</ContentTemplate>
				<Triggers>
					<asp:PostBackTrigger ControlID="btnUpload" />
					<asp:PostBackTrigger ControlID="btnCancel" />
					<asp:PostBackTrigger ControlID="btnUpdate" />
					<asp:AsyncPostBackTrigger ControlID="lstvwFileList" EventName="ItemCommand" />
				</Triggers>
			</asp:UpdatePanel>
		</td>
	</tr>
	<tr>
		<td align="center">
			<table style="width: 880px;">
				<tr id="trAcademicYrNotice" runat="server">
					<td align="center">
						<span class="ClsMdtStar">You will not be able to upload a file when viewing old academic year files.<br />If you wish to upload a new file, please select the current academic year from the drop-down list.</span>
					</td>
				</tr>
				<tr id="AcademicYearRow" runat="server">
					<td align="center" valign="middle">
						<table>
							<tr>
								<td class="ClsBorderlight">
									<span class="ClsLabel" style="float: none; margin-right: 5px;">Academic Year : </span>
								</td>
								<td>
									<asp:DropDownList ID="ddlAcademicYear"
													  runat="server"
													  CssClass="MidCombo"
													  AutoPostBack="True"
													  style="font-size: 9pt;"
													  OnSelectedIndexChanged="ddlAcademicYear_SelectedIndexChanged">
									</asp:DropDownList>
								</td>
							</tr>
						</table>
					</td>
				</tr>
				<tr>
					<td align="center">
						<asp:UpdatePanel ID="UpdatePanel2"
										 runat="server"
										 UpdateMode="Conditional">
							<ContentTemplate>
								<asp:ListView ID="lstvwFileList"
											  runat="server"
											  DataSourceID="FileListObjDataSource"
											  DataKeyNames="UploadId,FilePath,IsRead"
											  OnDataBound="lstvwFileList_DataBound"
											  OnItemCommand="lstvwFileList_ItemCommand"
											  OnItemDataBound="lstvwFileList_ItemDataBound"
											  OnSorting="lstvwFileList_Sorting">
									<LayoutTemplate>
										<table>
											<tr>
												<td align="center">
													<asp:DataPager ID="DtPgCount"
																   runat="server"
																   PagedControlID="lstvwFileList"
																   PageSize="20">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<asp:Label EnableViewState="false" runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
																		CssClass="LblNrmlB" />
																	<asp:Label ID="lblTo" runat="server" EnableViewState="false" CssClass="LblNormal"
																		Text=" To " />
																	<asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
																		CssClass="LblNrmlB" />
																	<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" EnableViewState="false"
																		Text=" Out Of " />
																	<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
																		CssClass="LblNrmlB" />
																	<asp:Label ID="lblRecords" runat="server" EnableViewState="false" CssClass="LblNormal"
																		Text="Records " />
																	<br />
																</PagerTemplate>
															</asp:TemplatePagerField>
														</Fields>
													</asp:DataPager>
												</td>
											</tr>
										</table>
										<table border="0" cellpadding="4" cellspacing="1" class="GridBorder" width="100%">
											<tr id="trHeader" runat="server" class="ClsGridHeader">
												<th align="left" style="width: 150px;">
													<asp:LinkButton ID="lnkbtnTitle"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT"
																	CommandArgument="Title"
																	Text="Title"
																	ForeColor="Black"> </asp:LinkButton>
												</th>
												<th align="left" style="width: 300px;">
													<asp:LinkButton ID="lnkbtnDescription"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT"
																	CommandArgument="Description"
																	Text="Description"
																	ForeColor="Black"> </asp:LinkButton>
												</th>
												<th align="left" style="width: 110px;">
													<asp:LinkButton ID="lnkbtnUploadedBy"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT"
																	CommandArgument="UserName"
																	Text="Uploaded By"
																	ForeColor="Black"> </asp:LinkButton>
												</th>
												<th style="width: 100px;">
													<asp:LinkButton ID="lnkbtnUpdateDate"
																	runat="server"
																	CausesValidation="false"
																	CommandName="SORT"
																	CommandArgument="UpdateDate"
																	Text="Updated On"
																	ForeColor="Black"> </asp:LinkButton>
												</th>
												<th id="EditColHeader" runat="server" style="width: 40px;">Edit</th>
												<th id="DeleteColHeader" runat="server" style="width: 40px;">Delete</th>
												<th style="width: 65px;">Download</th>
											</tr>
											<tr id="itemPlaceHolder" runat="server"></tr>
											<tr id="trDataPager" runat="server" class="ClsBorderPager">
												<td colspan="7">
													<asp:DataPager ID="DtPgDropDown"
																   runat="server"
																   PagedControlID="lstvwFileList"
																   PageSize="20">
														<Fields>
															<asp:TemplatePagerField>
																<PagerTemplate>
																	<table width="100%">
																		<tr>
																			<td align="left">
																				<asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
																				<asp:DropDownList ID="ddlCnt"
																								  runat="server"
																								  AutoPostBack="true"
																								  OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
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
										<tr id="trGridRow" runat="server" class="ClsGridRow">
											<td>
												<asp:Label ID="lblTitle"
														   runat="server"
														   Text='<%# Eval("Title") %>'> </asp:Label>
											</td>
											<td>
												<asp:Label ID="lblDescription"
														   runat="server"
														   Text='<%# Eval("Description") %>'> </asp:Label>
											</td>
											<td align="left">
												<asp:Label ID="lblUploadedBy"
														   runat="server"
														   Text='<%# Eval("UploadedBy") %>'> </asp:Label>
											</td>
											<td align="center">
												<asp:Label ID="lblUpdateDate"
														   runat="server"
														   Text='<%# ((DateTime)Eval("UpdatedDate")).ToString("dd-MMM-yyyy") %>'> </asp:Label>
											</td>
											<td id="EditButtonCell" runat="server" align="center">
												<asp:ImageButton ID="imgBtnEdit"
																 runat="server"
																 CausesValidation="false"
																 CommandName="EDIT"
																 ImageUrl="../images/IconGrid_Edit.gif" />
											</td>
											<td id="DeleteButtonCell" runat="server" align="center">
												<asp:ImageButton ID="imgBtnDelete"
																 runat="server"
																 CausesValidation="false"
																 CommandName="DELETEFILE"
																 ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
																 OnClientClick="if(!WarnOnDelete()){return false;}"/>
											</td>
											<td align="center">
												<asp:ImageButton ID="imgBtnDownload"
																 runat="server"
																 ImageUrl="../images/download_transparent.png"/>
											</td>
										</tr>
									</ItemTemplate>
									<AlternatingItemTemplate>
										<tr id="trGridRow" runat="server" class="ClsGridAltRow">
											<td>
												<asp:Label ID="lblTitle"
														   runat="server"
														   Text='<%# Eval("Title") %>'> </asp:Label>
											</td>
											<td>
												<asp:Label ID="lblDescription"
														   runat="server"
														   Text='<%# Eval("Description") %>'> </asp:Label>
											</td>
											<td align="left">
												<asp:Label ID="lblUploadedBy"
														   runat="server"
														   Text='<%# Eval("UploadedBy") %>'> </asp:Label>
											</td>
											<td align="center">
												<asp:Label ID="lblUpdateDate"
														   runat="server"
														   Text='<%# ((DateTime)Eval("UpdatedDate")).ToString("dd-MMM-yyyy") %>'> </asp:Label>
											</td>
											<td id="EditButtonCell" runat="server" align="center">
												<asp:ImageButton ID="imgBtnEdit"
																 runat="server"
																 CausesValidation="false"
																 CommandName="EDIT"
																 ImageUrl="../images/IconGrid_Edit.gif" />
											</td>
											<td id="DeleteButtonCell" runat="server" align="center">
												<asp:ImageButton ID="imgBtnDelete"
																 runat="server"
																 CausesValidation="false"
																 CommandName="DELETEFILE"
																 ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif"
																 OnClientClick="if(!WarnOnDelete()){return false;}"/>
											</td>
											<td align="center">
												<asp:ImageButton ID="imgBtnDownload"
																 runat="server"
																 ImageUrl="../images/download_transparent.png"/>
											</td>
										</tr>
									</AlternatingItemTemplate>
									<EmptyDataTemplate>
										<div class="LblNoRecord">No record found.</div>
									</EmptyDataTemplate>
								</asp:ListView>
								<asp:ObjectDataSource ID="FileListObjDataSource"
													  runat="server"
													  TypeName="BusinessLogic.ManagementFileSharingBL"
													  SelectMethod="GetAllFiles"
													  SelectCountMethod="GetCount"
													  EnablePaging="True"
													  SortParameterName="sortExpression"
													  OnSelecting="FileListObjDataSource_Selecting">
									<SelectParameters>
										<asp:SessionParameter Name="aiSchoolId"
															  SessionField="I_SCHOOL_ID"
															  Type="Int32" />
										<asp:ControlParameter Name="aiAcademicYearId"
															  ControlID="hidAcademicYearId"
															  PropertyName="Value"
															  Type="Int32" />
										<asp:SessionParameter Name="aiUserId"
															  SessionField="I_SUPER_ADMIN_USER_ID"
															  Type="Int32"
															  DefaultValue="-1" />
										<asp:Parameter Name="sortExpression" Type="String" />
										<asp:Parameter Name="maximumRows" Type="Int32" />
										<asp:Parameter Name="startRowIndex" Type="Int32" />
									</SelectParameters>
								</asp:ObjectDataSource>
								
								<%--HIDDEN FIELDS--%>
								<asp:HiddenField ID="hidSchoolId" runat="server" />
								<asp:HiddenField ID="hidAcademicYearId" runat="server"/>
								<asp:HiddenField ID="hidSortExpression" runat="server" />
								<asp:HiddenField ID="hidSortDirection" runat="server" />
								<asp:HiddenField ID="hidOldUploadId" runat="server" />
								<asp:HiddenField ID="hidOldFilePath" runat="server" />
								<asp:HiddenField ID="hidOldUploadedForIds" runat="server" />
							</ContentTemplate>
							<Triggers>
								<asp:AsyncPostBackTrigger ControlID="lstvwFileList" EventName="ItemCommand" />
							</Triggers>
						</asp:UpdatePanel>
					</td>
				</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td align="center">
			<asp:Button ID="btnBack"
						runat="server"
						CssClass="ClsBtn"
						Height="24px"
						CausesValidation="false"
						UseSubmitBehavior="false"
						TabIndex="1"
						Text="Back"/>
		</td>
	</tr>
</table>
<script type="text/javascript">
var _clientFileUpload = "<%=this.FileUploadControl.ClientID%>";
var _clienttxtDescription = "<%=this.txtDescription.ClientID%>";
var _clientchkSuperAdminList = "<%=this.chklstSuperAdmin.ClientID%>";
var _clientbtnUpload = "<%=this.btnUpload.ClientID%>";

function ValidateUpload(source, args) {
	var fileUpload = document.getElementById(_clientFileUpload);
	var uploadBtn = document.getElementById(_clientbtnUpload);
	if(uploadBtn && fileUpload.value.trim() == "") {
		args.IsValid = false;
		return true;
	}
	args.IsValid = true;
	return false;
}

function ValidateDescription(source, args) {
	var description = document.getElementById(_clienttxtDescription);
	if(description.value.trim() != "" && description.value.length > 4000) {
		args.IsValid = false;
		return true;
	}
	args.IsValid = true;
	return false;
}

function ValidateSuperAdminList(source, args) {
	var chkboxlstSuperAdmin = document.getElementById(_clientchkSuperAdminList);
	var items = chkboxlstSuperAdmin.getElementsByTagName('input');
	var selected = false;
	for(var i = 0, l = items.length; i < l; i++) {
		var item = items[i];
		if(item && item.type == 'checkbox' && item.checked) {
			selected = true;
			break;
		}
	}
	if(selected) {
		args.isValid = true;
		return false;
	}
	args.IsValid = false;
	return true;
}

function WarnOnDelete() {
	return confirm("Are you sure you want to delete this file?");
}
</script>
</asp:Content>