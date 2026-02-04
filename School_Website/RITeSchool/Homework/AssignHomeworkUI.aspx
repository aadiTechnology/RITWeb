<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
	AutoEventWireup="true" CodeFile="AssignHomeworkUI.aspx.cs" Inherits="AssignHomeworkUI" %>
<%--<%@ Register TagPrefix="uc" Src="~/UserControls/HomeworkListUC.ascx" TagName="Homework" %>--%>
<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
	TagPrefix="rjs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="headContentPlaceholder" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <style type="text/css">
		.notice-popup-wrapper
		{
			position: absolute;
			left: 50%;
			top: 50%;
			border: solid 2px darkgreen;
			background-color: lightyellow;
			font-family: Tahoma;
		}
		.notice-popup-title-text
		{
			margin: 0;
			text-align: left;
			font-size: 14px;
		}
		
		.notice-popup-title-closebtn
		{
			float: right;
			cursor: pointer;
		}
		
		.notice-popup-content
		{
			padding: 15px;
			text-align: left;
			vertical-align: top;
			overflow: auto;
		}
		.web_dialog_overlay
		{
			position: absolute;
			height: 100%;
			width: 100%;
			background: transparent;
			opacity: .15;
			filter: alpha(opacity=15);
			-moz-opacity: .15;
			z-index: 1001;
			display: none;
		}
		.style1
		{
			height: 26px;
		}
	    #tblControls
        {
            width: 990px;
        }
        .btnDwnload
        {
          font-size:9pt;
          font-weight:8px;  
          font-style:normal;  
          font-family:Arial;
      
        }
	</style>
	<div id="overlay" class="web_dialog_overlay">
	</div>
	<div id="divReason" runat="server" class="notice-popup-wrapper" style="z-index: 5000;
		width: 450px; height: 200px; margin: -65px 0 0 -150px; background-color: white;
		visibility: hidden; display: none;">
		<div class="notice-popup-title">
			<span class="notice-popup-title-closebtn" onclick="HidePopup();">
				<img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
			</span>
			<h4 class="notice-popup-title-text ">
				Unpublish Reason
			</h4>
		</div>
		<div class="notice-popup-content" style="font-family: Times New Roman; font-size: large;
			font-weight: bold; color: #333;">
			<table width="415px">
				<tr>
					<td id="tdReason" colspan="2" class="ClsMdtStar">
						<span>Unpublish reason should not be blank. </span>
					</td>
				</tr>
				<tr>
					<td id="tdReasonLength" colspan="2" class="ClsMdtStar">
						<span>Reason should be of length less than 100. </span>
					</td>
				</tr>
				<tr>
					<td class=" LblNrmlB ClsLabelNrml" valign="middle">
						Unpublish Reason :
					</td>
					<td align="left" valign="middle">
						<asp:TextBox ID="txtUnpublishReason" Width="250px" CssClass="LrgTxtBox" runat="server"
							Height="80px" MaxLength="100" TextMode="MultiLine"></asp:TextBox>
						<span class="ClsMdtStar">*</span>
					</td>
				</tr>
				<tr>
					<td style="height: 10px;">
					</td>
					<td>
					</td>
				</tr>
				<tr>
					<td align="center" colspan="2">
						<asp:Button ID="btnUnpublish" runat="server" Text="OK" CssClass="ClsBtn" CausesValidation="false" OnClientClick="if (!ValideteReason()){return false;}"
							OnClick="btnUnpublish_Click" />
						<%--</td>
					<td align="left">--%>
						<asp:Button ID="btnClosePopup" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
							OnClientClick="javascript:HidePopup();return false;" />
					</td>
				</tr>
			</table>
		</div>
	</div>
	<table width="100%">
		<tr>
			<td align="right" width="100%" class="LblNormal ClsMdtStar">
				* Mandatory Fields
			</td>
		</tr>
		<tr>
			<td>
				<asp:ValidationSummary ID="valSumErrorMsg" runat="server" ValidationGroup="Save"
					CssClass="ClsLabel" />
			</td>
		</tr>
		<tr>
			<td align="left">
				<asp:Label ID="lblError" CssClass="ClsMdtStar" Visible="false" runat="server" Text="File size should not be greater than 1 MB."></asp:Label>
			</td>
		</tr>
		<tr>
			<td align="center">
				<table width="700px">
					<tr id="trLegend" runat="server">
						<td align="center">
							<table width="700px">
								<tr>
									<td align="center">
										<asp:Label ID="lblSuccess" CssClass="ClsLabelUpdate" Font-Bold="True" ForeColor="Blue"
											Visible="false" runat="server" Text="Homework saved successfully !!!"></asp:Label>
									</td>
								</tr>
								<tr>
									<td>
									</td>
								</tr>
								<tr>
									<td align="left">
										<table width="500px">
											<tr>
												<td align="center" valign="middle" width="40px;">
													<span class="ClsLblLgnd">Class :</span>
												</td>
												<td align="center" class="ClsHilightBGB" width="150px;">
													<asp:Label ID="lblClass" runat="server" Text=""></asp:Label>
												</td>
												<td align="center" width="60px;">
													<span class="ClsLblLgnd">Teacher :</span>
												</td>
												<td align="center" class="ClsHilightBGB" width="200px;">
													<asp:Label ID="lblTeacher" runat="server" Text=""></asp:Label>
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
						</td>
					</tr>
					<tr>
						<td align="center">
							<table id="tblControls" runat="server">								
                                <tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;" runat="server" id="tdSubject">
										Subject :
									</td>
									<td style="width: 350px;" runat="server" id="tdSubjectCmb">
										<asp:DropDownList ID="cmbSubject" runat="server" Width="220px" OnSelectedIndexChanged="cmbSubject_SelectedIndexChanged"
											AutoPostBack="true">
										</asp:DropDownList>
									</td>
								</tr>
								<tr>
									<td class="ClsBorderlight paddingL" style="width: 200px;">
										Title :
									</td>
									<td style="width: 350px;">
										<asp:TextBox ID="txtTitle" runat="server" CssClass="LrgTxtBox" Width="220px" MaxLength="100"></asp:TextBox>
										<asp:RequiredFieldValidator ID="reqTitle" runat="server" ErrorMessage="Title should not be blank."
											Display="none" CssClass="ClsLabelNrml" ControlToValidate="txtTitle" ValidationGroup="Save"></asp:RequiredFieldValidator>
										<span class="ClsMdtStar">*</span>
									</td>
								</tr>
								<tr>
									<td class="ClsBorderlight paddingL">
										Assigned Date :
									</td>
									<td>
										<asp:TextBox ID="txtAssignedDt" runat="server" CssClass="LrgTxtBox" ReadOnly = "true" ></asp:TextBox>
										<rjs:PopCalendar ID="PopCalendar1" runat="server" Control="txtAssignedDt" From-Date="" Culture="en"
											ShowErrorMessage="False" From-Today="True" Format="dd mmm yyyy" />
										<span class="ClsMdtStar">*</span>
										<asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Assigned Date should not be blank."
											Display="None" ValidationGroup="Save" ClientValidationFunction="ValidateAssignDate"></asp:CustomValidator>
									</td>
								</tr>
								<tr>
									<td class="ClsBorderlight paddingL">
										Complete by Date :
									</td>
									<td>
										<asp:TextBox ID="txtCompleteByDt" runat="server" CssClass="LrgTxtBox" ReadOnly = "true"></asp:TextBox>
										<rjs:PopCalendar ID="calFromDate" runat="server" Control="txtCompleteByDt" From-Date="" Culture="en" 
											ShowErrorMessage="False" From-Today="True" Format="dd mmm yyyy" />
										<span class="ClsMdtStar">*</span>
										<asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Complete by Date should not be blank."
											Display="None" ValidationGroup="Save" ClientValidationFunction="ValidateCompleteByDate"></asp:CustomValidator>
										<asp:CustomValidator ID="cstValDate" runat="server" ErrorMessage="" Display="None"
											ValidationGroup="Save" ClientValidationFunction="ValidateDate"></asp:CustomValidator>
									</td>
								</tr>
								<tr id="trFirstAttachment" runat="server">
									<td class="ClsBorderlight paddingL">
										Attachment :
									</td>
									<td style="width: 440;">
										<asp:FileUpload ID="fileUpload" runat="server"  />
										<asp:CustomValidator ID="cstFileType" runat="server" CssClass="ClsLabelNrml" Display="None"
											ValidationGroup="Save" ErrorMessage="CustomValidator" ClientValidationFunction="CheckFileType" Enabled="false"></asp:CustomValidator>
										    &nbsp;<br />
										    <asp:LinkButton CssClass="btnDwnload" ID="btnDownload" runat="server" CausesValidation="false" ToolTip="View Attachment"
                                    CommandName="DOWNLOAD" 
                                             />
                                        <img src="~/RITeSchool/images/IconGrid_Delete.gif" alt="" id="imgBtnDelete" 
                                            runat="server" onclick="WarnOnDelete()"
                                         title="DeleteAttachment" /></td>
								</tr>
                                <tr id="trFirstAttachmentSupportFiles" runat="server">
                                    <td>
                                    </td>
                                    <td align="left">
                                        <span class="LblSmlGray">(Supports files of types - .PDF, .BMP, .JPG, .JPEG, .PNG upto 1 MB.)</span>
                                    </td>
                                </tr>
                              <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <span class="ClsLabel">Attachments :</span>
                                                </td>
                                                <td align="left">
                                                    <asp:FileUpload ID="flDocument" runat="server" AllowMultiple="true" />
                                                     <asp:CustomValidator ID="cstFileType1" runat="server" ErrorMessage="" ClientValidationFunction="ValidateFile"></asp:CustomValidator>
<%--                                                    <span class="ClsMdtStar">*</span>--%>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td align="left">
                                                    <span class="LblSmlGray">(Attachment supports files of types - .BMP, .DOC, .DOCX, .JPG,
                                                        .JPEG, .PNG, .PDF, .XLS, .XLSX upto 5 MB.)</span>
                                                </td>
                                            </tr>
								<tr>
									<td class="ClsBorderlight paddingL" valign="middle">
										Details :
									</td>
									<td>
										<asp:TextBox ID="txtDetails" runat="server" TextMode="MultiLine" Width="340px" Height="100px"></asp:TextBox>
										<span class="ClsMdtStar">*</span>
										<asp:CustomValidator ID="cstDetails" runat="server" Display="None" ValidationGroup="Save"
											ClientValidationFunction="Validate"></asp:CustomValidator>
									</td>
								</tr>
                                <tr id="trDivisions" runat="server" visible="false">
                                    <td id="Td1" class="ClsBorderlight paddingL" style="width: 200px;" runat="server">
                                        Copy For :
                                    </td>
                                    <td>
                                        <asp:CheckBoxList ID="ChkDivisionList" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"></asp:CheckBoxList>
                                    </td>
                                </tr>
								<tr>
									<td align="center" colspan="2">
										<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="ClsBtn" ValidationGroup="Save"
											OnClick="btnSave_Click" />
										<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
											 onclick="btnCancel_Click" />
										<asp:Button ID="btnBack" runat="server" CausesValidation="false" Text="Back" CssClass="ClsBtn" />
									</td>
								</tr>
							</table>
						</td>
					</tr>
					<tr>
						<td>
							<asp:HiddenField ID="hidSubjectId" runat="server" />
							<asp:HiddenField ID="hidStdDivId" runat="server" />
							<asp:HiddenField ID="hidTeacherId" runat="server" />
							<asp:HiddenField ID="hidDate" runat="server" />
							<asp:HiddenField ID="hidMode" runat="server" />
							<asp:HiddenField ID="hidId" runat="server" Value="0" />
							<asp:HiddenField ID="hidFileName" runat="server" />
							<asp:HiddenField ID="hidClassName" runat="server" Value="0" />
							<asp:HiddenField ID="hidTeacherName" runat="server" />
                            <asp:HiddenField ID="hidFilePath" runat="server" /> 
                            <asp:HiddenField ID="hidSendSMS" runat="server" Value="N" />
                            <asp:HiddenField ID="hidSMSText" runat="server" Value="" />
                            <asp:HiddenField ID="hisSMSStatus" runat="server" Value="N" />
                            <asp:HiddenField ID="hidListViewType" runat="server"  />  
                            <asp:HiddenField ID="hidDeleteFromAll" runat="server" Value="N" />                          
						</td>
					</tr>
					<tr>
						<td align="center">
							<table>
								<tr>
                                     <td class="ClsBorderlight paddingL">
							           Select Homework Status:
						             </td>
						             <td>
						             <asp:DropDownList runat="server" ID="drdwnHomeWorkStatus"
                                        style="height: 22px" 
                                        onselectedindexchanged="drdwnHomeWorkStatus_SelectedIndexChanged" 
                                             AutoPostBack="True">
                                        <asp:ListItem Text="All" Selected="True" Value="All"></asp:ListItem>
                                        <asp:ListItem Text="Assigned Date"  Value="AssignedDate"></asp:ListItem>
                                        <asp:ListItem Text="Complete By Date"  Value="CompleteByDate"></asp:ListItem>
                                     </asp:DropDownList>
					                 </td>
									 <td>
										<asp:TextBox ID="txtSearchDt" runat="server" CssClass="SmlTxtBox" ReadOnly = "true"  
                                         ></asp:TextBox>
										<rjs:PopCalendar ID="calAssignedDtSearch" runat="server" Control="txtSearchDt" Format="d mmm yyyy" Culture="en"
											ShowErrorMessage="false" OnSelectionChanged="calAssignedDtSearch_SelectionChanged"
											AutoPostBack="True" />
										<span class="ClsMdtStar">*</span>

									</td>                                                   
                                     <td class="ClsBorderlight paddingL">
							            Homework Title:
						             </td>
                                    <td >
                                       <asp:TextBox ID="txtHomeworkTitle" runat="server" CssClass="LrgTxtBox"  autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" TabIndex="2" CssClass="ClsBtnMid remove-margin-top"
                                         OnClick="btnSearch_Click" CausesValidation="false"/>
                                    </td>                                 

							</table>
						</td>
					</tr>
					<tr>
						<td align="center">
							<table width="1000px">
								<tr id="trGridTitle" runat="server">
									<td class="ClsLblLgnd">
										<span class="ClsLblLgnds LblNrmlB">Assigned homework for selected subject :</span>
									</td>
								</tr>
								<tr id="trLstview" runat="server">
									<td>
										<asp:ListView ID="lstvwHomeworkTeacher" runat="server" DataKeyNames="flag,Id,HasLinkedHomework" OnItemDataBound="lstvwHomeworkTeacher_ItemDataBound"
											OnItemCommand="lstvwHomeworkTeacher_ItemCommand">
											<LayoutTemplate>
												<table id="tblhomework" align="center" width="100%" runat="server" class="GridBorder">
													<tr id="trHeader" runat="server" class="ClsGridHeader">
														<th align="left" class="paddingL">
															<asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
														</th>
														<th align="left" class="paddingL">
															<asp:Label ID="Label2" runat="server" Text="Title"></asp:Label>
														</th>
														<th align="center" width="100px">
															<asp:Label ID="Label3" runat="server" Text="Assigned Date"></asp:Label>
														</th>
														<th align="center" width="150px">
															<asp:Label ID="Label4" runat="server" Text="Complete By Date"></asp:Label>
														</th>
														<th align="left" class="paddingL" width="150px">
															<asp:Label ID="Label5" runat="server" Text="Attachment"></asp:Label>
														</th>
                                                        <th align="left" class="paddingL" width="50px">
															<asp:Label ID="Label1" runat="server" Text="View"></asp:Label>
														</th>
														<th align="center" width="150px">
															<asp:Label ID="Label6" runat="server" Text="Publish/Unpublish"></asp:Label>
														</th>
														<th align="center" width="50px">
															<asp:Label ID="Label7" runat="server" Text="Edit"></asp:Label>
														</th>
														<th align="center" width="50px">
															<asp:Label ID="lblAdd" runat="server" Text="Delete"></asp:Label>
														</th>
													</tr>
													<tr runat="server" id="itemPlaceholder">
													</tr>
												</table>
											</LayoutTemplate>
											<ItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridRow">
													<td align="left" class="paddingL">
														<asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
															runat="server" Text='<%# Eval("Title") %>'></asp:LinkButton>
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblAssignedDt" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:HyperLink ID="lnkAttachment" runat="server" Text='<%# Eval("AttachmentPath") %>'></asp:HyperLink>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                     <asp:ImageButton ID="imgView" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                    />
                                                    </td>
													<td align="center">
														<asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" CommandName="PUBLISH" CausesValidation="false" />
													</td>
													<td align="center">
														<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
															ToolTip="Edit homework" ImageUrl="../images/IconGrid_Edit.GIF" />
													</td>
													<td align="center">
														<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
															ToolTip="Delete homework" ImageUrl="../images/IconGrid_Delete.gif" />
													</td>
												</tr>
											</ItemTemplate>
											<AlternatingItemTemplate>
												<tr id="Tr2" runat="server" class="ClsGridAltRow">
													<td align="left" class="paddingL">
														<asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
															runat="server" Text='<%# Eval("Title") %>'></asp:LinkButton>
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblAssignedDt" runat="server" Text='<%# Eval("AssignedDate") %>'></asp:Label>
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:HyperLink ID="lnkAttachment" runat="server" Text='<%# Eval("AttachmentPath") %>'></asp:HyperLink>
                                                    </td>
                                                       <td align="left" class="paddingL">
                                                          <asp:ImageButton ID="imgView" runat="server" ImageUrl="~/RITeSchool/images/iconGridSml_ViewGE.gif"
                                                                                  />
                                                    </td>
													<td align="center">
														<asp:Button ID="btnPublish" runat="server" Text="Publish" CssClass="ClsBtn" CommandName="PUBLISH" CausesValidation="false" />
													</td>
													<td align="center">
														<asp:ImageButton ID="imgBtnEdit" runat="server" CausesValidation="false" CommandName="UpdateCommand"
															ToolTip="Edit homework" ImageUrl="../images/IconGrid_Edit.GIF" />
													</td>
													<td align="center">
														<asp:ImageButton ID="imgBtnDelete" runat="server" CausesValidation="false" CommandName="RemoveCommand"
															ToolTip="Delete homework" ImageUrl="../images/IconGrid_Delete.gif" />
													</td>
												</tr>
											</AlternatingItemTemplate>
											<EmptyDataTemplate>
												<tr>
													<td width="550px" align="center" class="LblNoRecord">
														No record found.
													</td>
												</tr>
											</EmptyDataTemplate>
										</asp:ListView>
									</td>
								</tr>
								<tr>
									<td class="style1">
									</td>
								</tr>
								<tr id="trOtherSubjectGrid" runat="server">
									<td class="ClsLblLgnd">
										<span class="ClsLblLgnds LblNrmlB">Homework assigned for other subjects :</span>
									</td>
								</tr>
								<tr>
									<td>
										<asp:ListView ID="lstvwOtherSubjectHomework" runat="server" DataKeyNames="Id" OnItemDataBound="lstvwOtherSubjectHomework_ItemDataBound"											>
											<LayoutTemplate>
												<table id="tblhomework" align="center" width="1000px" runat="server" class="GridBorder">
													<tr id="trHeader" runat="server" class="ClsGridHeader">
                                                        <th align="left" class="paddingL" style="width:40px;">		                                                            											
														</th>
                                                        <th align="center" class="paddingL" style="text-align:center; width:60px;">
															<asp:Label ID="lblSrNo" runat="server" Text="Sr. No."></asp:Label>
														</th>
														<th align="left" class="paddingL">
															<asp:Label ID="lblSubject" runat="server" Text="Subject"></asp:Label>
														</th>
														<th align="left" class="paddingL">
															<asp:Label ID="Label2" runat="server" Text="Title"></asp:Label>
														</th>
														<th id="thPublish">
															Is Published?
														</th>
														<th align="center" class="paddingL">
															<asp:Label ID="Label4" runat="server" Text="Complete By Date"></asp:Label>
														</th>
													</tr>
													<tr runat="server" id="itemPlaceholder">
													</tr>
												</table>
											</LayoutTemplate>
											<ItemTemplate>
												<tr id="trItem" runat="server" class="ClsGridRow">
                                                    <td align="left" class="paddingL">
														<asp:CheckBox ID="chkPublish" runat="server" />
                                                        <asp:HiddenField ID="hidHomeworkId" runat="server" Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
													</td>
                                                    <td align="center" class="paddingL">
														<asp:Label ID="lblSrNo" runat="server" Text=""></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
															runat="server" Text='<%# Eval("Title") %>'></asp:LinkButton>
													</td>
													<td align="center" id="tdPublish">
														<asp:Image ID="imgBtbPublished" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
														<asp:Image ID="imgBtbNotPublished" runat="server" CausesValidation="false"
															ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
													</td>
												</tr>
											</ItemTemplate>
											<AlternatingItemTemplate>
												<tr id="trItem" runat="server" class="ClsGridAltRow">
                                                    <td align="left" class="paddingL">
                                                        <asp:CheckBox ID="chkPublish" runat="server" />
                                                        <asp:HiddenField ID="hidHomeworkId" runat="server" Value='<%# Eval("Id") %>' />
                                                        <asp:HiddenField ID="hidIsPublished" runat="server" Value='<%# Eval("IsPublished") %>' />
													</td>
                                                    <td align="center" class="paddingL">
														<asp:Label ID="lblSrNo" runat="server" Text=""></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:Label ID="lblSubject" runat="server" Text='<%# Eval("Subject.SubjectName") %>'></asp:Label>
													</td>
													<td align="left" class="paddingL">
														<asp:LinkButton ID="lnkTitle" PostBackUrl="ViewHomeworkUI.aspx?"
															runat="server" Text='<%# Eval("Title") %>'></asp:LinkButton>
													</td>
													<td align="center" id="tdPublish">
														<asp:Image ID="imgBtbPublished" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
														<asp:Image ID="imgBtbNotPublished" runat="server" CausesValidation="false" ImageUrl="~/RITeSchool/images/IconGrid_Delete.gif" />
													</td>
													<td align="center" class="paddingL">
														<asp:Label ID="lblCompleteDt" runat="server" Text='<%# Eval("CompleteByDate") %>'></asp:Label>
													</td>
												</tr>
											</AlternatingItemTemplate>
											<EmptyDataTemplate>
												<tr>
													<td width="550px" align="center" class="LblNoRecord">
														No record found.
													</td>
												</tr>
											</EmptyDataTemplate>
										</asp:ListView>
									</td>
								</tr>
                                <tr align="center" style="text-align:center; margin:0px auto;">
                                    <td align="center" style="text-align:center;">
                                        <asp:Button ID="btnPublishAll" runat="server" Text="Publish All" 
                                            CssClass="ClsBtn" onclick="btnPublishAll_Click" Visible = "false" />
                                        <asp:Button ID="btnUnpublishAll" runat="server" Text="UnPublish All" 
                                            CssClass="ClsBtn" Visible = "false" />
                                    </td>
                                </tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
    <%--<div id="divPublishMessage" style="display:none">
        <table align="center">
        <tr>
            <td align="center" colspan="2">
                <span id="spnText" class="ClsLabel" style="float:inherit"></span>
            </td>
        </tr>  
        <tr>
            <td align="center" colspan="2">
                <span id="spnSubMessage" class="ClsLabel" style="float:inherit"></span>
            </td>
        </tr>    
        <tr>
            <td align="right">
                <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="ClsBtn" />
            </td>
            <td align="left">
                <asp:Button ID="btnNo" runat="server" Text="No" CssClass="ClsBtn" />
            </td>
        </tr>
        </table>
    </div>--%>
	<script type="text/javascript">
		_clienttxtDetails = "<%=this.txtDetails.ClientID %>";
		_clientcstDetails = "<%=this.cstDetails.ClientID %>";
		_clienttxtTitle = "<%=this.txtTitle.ClientID %>";
		_clienttxtCompleteByDt = "<%=this.txtCompleteByDt.ClientID %>";
		_clienttxtAssignedDt = "<%=this.txtAssignedDt.ClientID %>";
		_clientcmbSubject = "<%=this.cmbSubject.ClientID %>";
		_clienthidSubjectId = "<%=this.hidSubjectId.ClientID  %>";
		_clienthidMode = "<%=this.hidMode.ClientID  %>";
		_clientbtnCancel = "<%=this.btnCancel.ClientID %>";
		_clienthidId = "<%=this.hidId.ClientID %>";
		_clienthidFileName = "<%=this.hidFileName.ClientID %>";
		_clientbtnDownload = "<%=this.btnDownload.ClientID %>";
		_clientbtnDelete = "<%=this.imgBtnDelete.ClientID %>"; 
		_clienttxtUnpublishReason = "<%=this.txtUnpublishReason.ClientID %>";
		_clientlblSucess = "<%=this.lblSuccess.ClientID %>";
		_clientfileUpload = "<%=this.fileUpload.ClientID %>";
		_clientcstFileType = "<%=this.cstFileType.ClientID  %>";
		_clientvalSumErrorMsg = "<%=this.valSumErrorMsg.ClientID %>";
		_clienttxtSearchDt = "<%=this.txtSearchDt.ClientID %>";
		_clientcstValDate = "<%=this.cstValDate.ClientID  %>";
		_clienthidSendSMS = "<%=this.hidSendSMS.ClientID %>"
		_clienthidSMSText = "<%=this.hidSMSText.ClientID %>"
		_clienthisSMSStatus = "<%=this.hisSMSStatus.ClientID %>"
		_clientListValue = "<%=this.lstvwOtherSubjectHomework.ClientID %>"
		_clienthidDeleteFromAll = '<%=this.hidDeleteFromAll.ClientID %>'
     
      //This method use to warn user when he want to delete attachemant.
        function WarnOnDelete() {
            if (window.confirm("Are you sure you want to delete this attachment?\nAttachment will get removed only after homework record is saved.")) {
		        document.getElementById(_clientbtnDownload).style.visibility = "hidden"
		        document.getElementById(_clientbtnDelete).style.visibility = "hidden"
                $get(_clienthidFileName).value = "";
           }
        }

		function ValidateFile(oSrc, args) {
		    var fl = $get("<%=this.flDocument.ClientID %>").value;

		    if (fl == "") {
		        oSrc.errormessage = "Please select file to upload.";
		        args.IsValid = false;
		        return true;
		    }

		    if (!(fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPG" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".JPEG" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".BMP" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PNG" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOC" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".DOCX" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLS" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".XLSX" ||
                      fl.substr(fl.lastIndexOf('.')).toUpperCase() == ".PDF"
                    )) {
		        oSrc.errormessage = "Please select valid file type.";
		        args.IsValid = false;
		        return true;
		    }

		    args.IsValid = true;
		    return false;
		}
       /// This method is used to validate homework details.
		function Validate(oSrc, args) {
			if ($("#" + _clienttxtDetails).val().replace(/ /g, '') == '') {
				oSrc.errormessage = "Details should not be blank.";
				args.IsValid = false;
				return true;
			}
			else if ($("#" + _clienttxtDetails).val().length > 1000) {
				oSrc.errormessage = "Details should be of length less than 1000.";
				args.IsValid = false;
				return true;
			}
		}

		function ValidateAssignDate(oSrc, args) {
			if ($("#" + _clienttxtAssignedDt).val() == "") {
				oSrc.errormessage = "Assigned Date should not be blank.";
				args.IsValid = false;
				return true;
			}
			else {
				if (new Date(convertdate($("#" + _clienttxtAssignedDt).val())) == "NaN" || new Date(convertdate($("#" + _clienttxtAssignedDt).val())) == "Invalid Date") {
					oSrc.errormessage = "Assigned Date should be valid date.";
					$("#" + _clienttxtAssignedDt).val('');
					args.IsValid = false;
					return true;
				}
				args.IsValid = true;
				return false;
			}
		}

		function ValidateCompleteByDate(oSrc, args) {
			if ($("#" + _clienttxtCompleteByDt).val() == "") {
				oSrc.errormessage = "Complete by Date should not be blank.";
				args.IsValid = false;
				return true;
			}
			else {
				if (new Date(convertdate($("#" + _clienttxtCompleteByDt).val())) == "NaN" || new Date(convertdate($("#" + _clienttxtCompleteByDt).val())) == "Invalid Date") {
					oSrc.errormessage = "Complete by Date should be valid date.";
					$("#" + _clienttxtCompleteByDt).val('');
					args.IsValid = false;
					return true;
				}
				args.IsValid = true;
				return false;
			}
		}

		function ValidateDate(oSrc, args) {
			if (new Date(convertdate($("#" + _clienttxtAssignedDt).val())) > new Date(convertdate($("#" + _clienttxtCompleteByDt).val()))) {
				oSrc.errormessage = "Complete by Date should not be less than assigned date.";
				args.IsValid = false;
				return true;
			}
		}
		

		/// This funxton is used to show unpublish reason pop up.
		function ShowPopup(id) {		    
			$("#" + _clientlblSucess).html('');
			$("#" + _clienttxtUnpublishReason).val('');
			$("#overlay").show();
			$("#tdReason").hide();
			$("#tdReasonLength").hide();
			var cssstyle = $get("<%=this.divReason.ClientID %>").style
			cssstyle.visibility = "visible";
			cssstyle.display = "block"; 		//		
			$("#" + _clienthidId).val(id);
}

function GetHomeworIdsAndShowPopup() {    
    var lbl
    var iRowCount = 0
    lbl = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSubject");
    var HomeworkIds = "";
    var ValidatedHomeworkIds = "";
    while (lbl != null) {
        var chk = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_chkPublish");
        var isPublished = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_hidIsPublished").value;
        var homwrkID = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_hidHomeworkId").value;
        var SrNo = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSrNo").innerText;        
        if (chk != null && chk.checked) {
            if (isPublished == "True") {
                HomeworkIds = HomeworkIds + "," + homwrkID;
            }
            else {
                ValidatedHomeworkIds = ValidatedHomeworkIds + ',' + SrNo;
            }
        }
        iRowCount++;
        lbl = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSubject");
    }
    if (ValidatedHomeworkIds == "") {
        if (HomeworkIds != "" && HomeworkIds != null) {
            HomeworkIds = HomeworkIds.substr(1);
            ShowPopup(HomeworkIds);
            return true;
        }
        else {
            alert('Atleast one subject should be selected to unpublish.');
            return false;
        }
    }
    else {
        ValidatedHomeworkIds = ValidatedHomeworkIds.substr(1);
        alert('Homework is not in published state for Sr. No. : ' + ValidatedHomeworkIds + '. Please remove selection.');
        return false;
    }
}

		/// This method is used to show confirmation message.
		function ConfirmPublish(date) {
            var smsText = $('#'+_clienthidSMSText).val()
		    $("#" + _clientlblSucess).html('');
		    $('#' + _clienthidSendSMS).val('N')
			var currentDate = new Date();
			if (new Date(date) >= new Date(currentDate.getFullYear() + '/' + (currentDate.getMonth() + 1) + '/' + currentDate.getDate())) {
			    if (confirm("Are you sure you want to publish the homework?")) {
                             
			        if ($('#' + _clienthisSMSStatus).val() == "N") {
			            if (window.confirm('Do you want to send SMS about Homework assignment?\n\nSMS Text - ' + smsText))
			                $('#' + _clienthidSendSMS).val('Y')
			        }
			        else
			            $('#' + _clienthidSendSMS).val('N')
                        			       
			        return true;
			    }
			    else
			        return false;
			    //OpenPopup('divPublishMessage', 'SMS Confirmation', 'Do you want to send following SMS?', '(We have made available homework for date 28 Jan 2015.)', '350px');
			    
			}
			else {
				window.alert("Homework for past assigned date cannot be published. Please change assigned date of homework.");
				return false;
			}
		}

		function ConfirmPublishAllHomework() {		    
		    var smsText = $('#' + _clienthidSMSText).val()
		    $("#" + _clientlblSucess).html('');
		    $('#' + _clienthidSendSMS).val('N')
		    var lbl = "";
		    var iRowCount = 0;
		    lbl = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSubject");
		    var HomeworkIds = "";
		    var ValidatedHomeworkIds = "";
		    while (lbl != null) {
		        var chk = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_chkPublish");
		        var isPublished = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_hidIsPublished").value;		            
		        var SrNo = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSrNo").innerText;
		            if (chk != null && chk.checked) {
		                    if (isPublished == "True") {
		                        ValidatedHomeworkIds = ValidatedHomeworkIds + ', ' + SrNo;
		                    }
		            }
		        iRowCount++;
		        lbl = document.getElementById(_clientListValue + "_ctrl" + iRowCount + "_lblSubject");
		    }
		    if (ValidatedHomeworkIds == "") {
              if (confirm("Are you sure you want to publish selected homework(s)?")) {
		         if ($('#' + _clienthisSMSStatus).val() == "N") {
		             if (window.confirm('Do you want to send SMS about Homework assignment?\n\nSMS Text - ' + smsText))
		                  $('#' + _clienthidSendSMS).val('Y')
		         }
		         else
		            $('#' + _clienthidSendSMS).val('N')
		            return true;
                }                
		        else
			        return false;	
		       }
		       else {
		          ValidatedHomeworkIds = ValidatedHomeworkIds.substr(1);
		          alert('Homework is already in published state for Sr. No. : ' + ValidatedHomeworkIds + '. Please remove selection.');
		          return false;
                }	
        }

		/// This method is used to show confirmation message.
        function ConfirmDelete(HasLinkedHomework) {        
		    $("#" + _clientlblSucess).html('');
		    $('#' + _clienthidDeleteFromAll).val('N')

		    if (confirm("Are you sure you want to delete the homework?")) {
		        if (HasLinkedHomework == 1) {
		            if (confirm('Do you want to delete same homework of all other classes?\n\nClick on - \nOk Button - To delete from all classes.\nCancel Button - To delete from only this class.')) {
		                $('#' + _clienthidDeleteFromAll).val('Y')		                
		            }
		        }
		        return true;
		    }
		    else
		        return false;
		}

		///This function is used to hide unpublished reason pop up.
		function HidePopup() {
			$("#" + _clientlblSucess).html('');
			$("#overlay").hide();
			var cssstyle = $get("<%=this.divReason.ClientID %>").style
			cssstyle.visibility = "hidden";
			cssstyle.display = "none";
			$("#" + _clienthidId).val('');
		}

		///This function is used to validate unpublished reason
		function ValideteReason() {
			$("#" + _clientlblSucess).html('');
			if ($("#" + _clienttxtUnpublishReason).val().length < 1) {
				$("#tdReason").show();
				$("#tdReasonLength").hide();
				return false;
			}
			else if ($("#" + _clienttxtUnpublishReason).val().length > 100) {
				$("#tdReasonLength").show();
				$("#tdReason").hide()
				return false;
			}

			return true;
		}

		/// This function is used to check valid filr types.
		function CheckFileType(aSrc, args) {
		    $("#" + _clientlblSucess).html('');		    
		    if (document.getElementById(_clientfileUpload) != null) {            
		        var sFileName = document.getElementById(_clientfileUpload).value
		        if (sFileName != "") {
		            var extension = sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase()

		            if (extension == ".PDF" || extension == ".BMP" || extension == ".JPG" || extension == ".JPEG" || extension == ".PNG") {
		                args.IsValid = true;
		                return false;
		            }
		            else {
		                document.getElementById(_clientcstFileType).errormessage = "Invalid file format."
		                document.getElementById(_clientcstFileType).empty = "";
		                args.IsValid = false
		                return true;
		            }
		        }
		    }
		    else {            
		        args.IsValid = true;
		        return false;
		    }
		}

		function ChangeSearch() {
			$("#" + _clienttxtSearchDt).val($("#" + _clienttxtAssignedDt).val());
        }
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
