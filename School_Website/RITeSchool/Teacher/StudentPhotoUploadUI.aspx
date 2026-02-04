<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="StudentPhotoUploadUI.aspx.cs" Inherits="StudentPhotoUploadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
	<div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <asp:UpdatePanel ID="UPnlPhotoGallery" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                <span style="font-weight: bold">Student Photos</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" valign="top">
                                                    <span class="ClsMdtStar">* Mandatory Fields</span>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                            <table width="100%">
                                                <tr>
                                                    <td>
                                                    <asp:UpdatePanel ID="upnlSummary" runat="server" UpdateMode="Always"><ContentTemplate>
                                                        <asp:Label ID="lblErrorMsg" runat="server" EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="false"
                                                            EnableViewState="false" ValidationGroup="Save" ShowSummary="true" HeaderText="Please fix following error(s):" />
                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ClsLabel"
                                                            ShowMessageBox="false" EnableViewState="false" ValidationGroup="Photo" ShowSummary="true"
                                                            HeaderText="Please fix following error(s):" />
                                                        <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction=" ValidateLogo"
                                                            ValidationGroup="Photo" ErrorMessage="Invalid file format." CssClass="LblErrorMsg"></asp:CustomValidator>
                                                        </ContentTemplate>                                                        
                                                        </asp:UpdatePanel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                               <tr>
									<td colspan="1" class="ClsTextNormal" align="center">
                                   <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                     EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                   </td>
								</tr>
								<tr><td style="height:2px"></td></tr>
                                    <tr>
										<td ID="tblSearch" runat="server" align="center">
											<table cellpadding="0" cellspacing="1" width="52%">
												<tr ID="trCombo">
													<td align="left" class="ClsBorderlight" colspan="1" width="80px">
														<span ID="lblDivision" class="ClsLabel">Standard :</span>
													</td>
													<td align="left" colspan="1" style="width: 120px">
														<asp:DropDownList ID="cmbStandard" runat="server" AutoPostBack="true" 
															CssClass="SmlTxtBox" OnSelectedIndexChanged="cmbStd_SelectedIndexChanged" 
															Width="118px">
														</asp:DropDownList>
													</td>
													<td align="left" class="ClsBorderlight" colspan="1" width="62px">
														<span class="ClsLabel" style="width: 62px">Division :</span>
													</td>
													<td align="left" class="ClsBorderlight" width="155px">
														<asp:UpdatePanel ID="uPnl" runat="server" ChildrenAsTriggers="false" 
															UpdateMode="Conditional">
															<ContentTemplate>
																<asp:DropDownList ID="cmbDivision" runat="server" AutoPostBack="true" 
																	CausesValidation="false" CssClass="SmlTxtBox" 
																	OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" Width="95px">
																</asp:DropDownList>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="cmbStandard" 
																	EventName="SelectedIndexChanged" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr ID="Tr1" runat="server">
													<td align="left" class="ClsBorderlight" colspan="1">
														<asp:UpdatePanel ID="UpdatePanel10" runat="server" ChildrenAsTriggers="true" 
															UpdateMode="Conditional">
															<ContentTemplate>
																<asp:RadioButton ID="optMain" runat="server" AutoPostBack="true" 
																	GroupName="Search" OnCheckedChanged="optMain_CheckedChanged" />
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
																<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
													<td align="left" class="ClsBorderlight" colspan="1">
														<span class="ClsLabel" style="width: 110px">Name / Reg. No. :</span>
													</td>
													<td align="Center" class="ClsBorderlight">
														<asp:Label ID="lblLike" runat="server" Style="font-weight: bold" Text="LIKE"></asp:Label>
													</td>
													<td align="left" class="ClsBorderlight">
														<asp:UpdatePanel ID="UpdatePanel7" runat="server" ChildrenAsTriggers="false" 
															UpdateMode="Conditional">
															<ContentTemplate>
																<asp:TextBox ID="txtName" runat="server" CssClass="ClsTxtLarge" MaxLength="15" 
																	Width="140px" autocomplete="off"></asp:TextBox>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
																<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr ID="Tr24" runat="server">
													<td align="left" class="ClsBorderlight" colspan="1">
														<asp:UpdatePanel ID="UpdatePanel9" runat="server" ChildrenAsTriggers="true" 
															UpdateMode="Conditional">
															<ContentTemplate>
																<asp:RadioButton ID="optExact" runat="server" AutoPostBack="true" 
																	GroupName="Search" OnCheckedChanged="optExact_CheckedChanged" />
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
																<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
													<td align="left" class="ClsBorderlight" colspan="1">
														<span class="ClsLabel">Reg. No. : </span>
														<asp:UpdatePanel ID="upnlOperation" runat="server" UpdateMode="Always">
															<ContentTemplate>
																<asp:DropDownList ID="cmbOperation" runat="server" CssClass="SmlCombo" 
																	Height="19px" Style="width: 55px">
																</asp:DropDownList>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="left" class="ClsBorderlight" width="80px">
														<asp:UpdatePanel ID="upnlPrefix" runat="server" UpdateMode="Always">
															<ContentTemplate>
																<asp:DropDownList ID="cmbPrefix" runat="server" CssClass="SmlCombo" 
																	Style="width: 100px">
																</asp:DropDownList>
															</ContentTemplate>
														</asp:UpdatePanel>
													</td>
													<td align="left" class="ClsBorderlight">
														<asp:UpdatePanel ID="UpdatePanel8" runat="server" ChildrenAsTriggers="true" 
															UpdateMode="Conditional">
															<ContentTemplate>
																<asp:TextBox ID="txtReg" runat="server" CausesValidation="true" 
																	CssClass="ClsTxtLarge" MaxLength="15" onblur="extractNumber(this,0,false);" 
																	ondrop="event.returnValue=false;" 
																	onkeypress="return blockNonNumbers (this, event, false, false);" 
																	onkeyup="extractNumber(this,0,false);" onpaste="event.returnValue=false;" 
																	Width="140px"></asp:TextBox>
																<span class="ClsMdtStar">*</span>
															</ContentTemplate>
															<Triggers>
																<asp:AsyncPostBackTrigger ControlID="optMain" EventName="CheckedChanged" />
																<asp:AsyncPostBackTrigger ControlID="optExact" EventName="CheckedChanged" />
															</Triggers>
														</asp:UpdatePanel>
													</td>
												</tr>
												<tr>
													<td align="left" class="ClsBorderlight" colspan="2">
													<span ID="Span1" class="LblNormal">Show students without Photo</span>
														
													</td>
													<td align="left" class="ClsBorderlight" colspan="2">
														<asp:CheckBox ID="chkStudentWithoutPhoto" runat="server" Checked="true" />
													</td>
												</tr>
												<tr>
													<td>
														<asp:CustomValidator ID="cstvalRegNo" runat="server" 
															ClientValidationFunction="ValidateRegNo" Display="None" 
															ErrorMessage="Reg. No. should not be empty." SetFocusOnError="True" 
															ValidationGroup="Save">
                                                    </asp:CustomValidator>
													</td>
												</tr>
												<tr>
													<td align="center" colspan="5">
														&nbsp;<asp:Button ID="btnSearch" runat="server" CausesValidation="true" 
															CssClass="ClsBtn" OnClick="btnSearch_Click" Text="Search" 
															ValidationGroup="Save" />
													</td>
												</tr>
												<tr>
													<td align="left" class="ClsBorderlight" colspan="5" style="padding-left: 10px">
														<span class="LblSmlGray">Upload or Capture photo for student's(Max 
														Height: 151px and Max Width: 112px).<br /> (Image size should not exceed 80 kb. 
														Supported file formats are JPG, JPEG)</span>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
									<td align="left">
									<table>
					<tr>
						<td>
							<span class="ClsLblLgnd" style="font: Bold; width: 50px">Legend :</span>
						</td>
						<td>
							<asp:Label ID="lblPendingFee" runat="server" BorderColor="Black" BorderStyle="Solid"
								BorderWidth="1px" CssClass="PhotoCapturd" EnableViewState="False" Height="20px"
								Text=" " Width="20px">
					       <img height="20px" src="../images/spacer.gif" width="20px" />													          
							</asp:Label>
						</td>
						<td>
							<span class="ClsTextNormal" style="font-weight:bold;" >Photo captured by webcam</span>
						</td>
					</tr>
				</table>
									</td></tr>
									<tr>
										<td align="center">
											<table width="100%">
												<tr ID="Tr5" runat="server">
													<td align="center">
														<asp:DataPager ID="DtPgCount" runat="server" PagedControlID="lstvwStudentPhoto" 
															PageSize="20">
															<Fields>
																<asp:TemplatePagerField>
																	<PagerTemplate>
																		<asp:Label ID="CurrentPageLabel" runat="server" CssClass="LblNrmlB" 
																			Text="<%# Container.StartRowIndex + 1%>" />
																		<asp:Label ID="lblTo" runat="server" CssClass="LblNormal" 
																			EnableViewState="false" Text=" To " />
																		<asp:Label ID="TotalPagesLabel" runat="server" CssClass="LblNrmlB" 
																			Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>" />
																		<asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" 
																			EnableViewState="false" Text=" Out Of " />
																		<asp:Label ID="TotalItemsLabel" runat="server" CssClass="LblNrmlB" 
																			Text="<%# Container.TotalRowCount%>" />
																		<asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" 
																			EnableViewState="false" Text="Records " />
																		<br />
																	</PagerTemplate>
																</asp:TemplatePagerField>
															</Fields>
														</asp:DataPager>
													</td>
												</tr>
												<tr>
													<td valign="top">
														<asp:ListView ID="lstvwStudentPhoto" runat="server" 
															DataKeyNames="Is_Leave,Photo_file_Path,SchoolWise_Student_Id,SchoolLeft_Date, Standard_Id, Division_id,Joining_Date,IsAttendanceAvailable,Photo_file_Path_Image" 
															OnDataBound="lstvwStudentPhoto_DataBound" 
															OnItemDataBound="lstvwStudentPhoto_ItemDataBound">
															<LayoutTemplate>
																<table ID="Table1" runat="server" cellpadding="0" cellspacing="1" 
																	class="GridBorder" style="color: #333333" width="100%">
																	<tr ID="trHeader" runat="server" class="ClsGridHeader">
																		<th align="left" class="ClspaddingL">
																			Reg. No.
																		</th>
																		<th align="center" class="paddingLR">
																			Class
																		</th>
																		<th align="left" class="ClspaddingL">
																			Roll No.
																		</th>
																		<th class="ClspaddingL">
																			Student Name
																		</th>
																		<th align="left" class="paddingLR">
																			Photo Browse
																		</th>
																		<th align="center" class="paddingLR">
																			Webcam</th>
																		<th align="center" class="paddingLR">
																			Photo
																		</th>
																	</tr>
																	<tr ID="itemPlaceholder" runat="server">
																	</tr>
																	<tr ID="trDataPager" runat="server" cellpadding="0" cellspacing="1" 
																		class="ClsBorderPager" style="color: #333333" width="100%">
																		<td align="left" colspan="7">
																			<asp:DataPager ID="DtPgDropDown" runat="server" 
																				PagedControlID="lstvwStudentPhoto" PageSize="20">
																				<Fields>
																					<asp:TemplatePagerField>
																						<PagerTemplate>
																							<table width="100%">
																								<tr>
																									<td>
																										<asp:Label ID="MessageLabel" runat="server" CssClass="LblNrmlB" 
																											Text="Select a page:" />
																										<asp:DropDownList ID="ddlCnt" runat="server" AutoPostBack="true" 
																											OnSelectedIndexChanged="ddlCnt_SelectedIndexChanged">
																										</asp:DropDownList>
																									</td>
																									<td align="right" cssclass="LblNormal">
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
																<tr ID="Tr2" runat="server" class="ClsGridRow">
																	<td align="left" width="10%">
																		<asp:Label ID="lblRegNo" runat="server" CssClass="ClspaddingL" 
																			Text='<%# Eval("Enrolment_Number") %>' />
																	</td>
																	<td align="center" class="paddingLR" width="10%">
																		<asp:Label ID="lblClass" runat="server" CssClass="paddingLR" 
																			Text='<%# Eval("StandardDivision") %>' />
																	</td>
																	<td align="left" width="10%">
																		<asp:Label ID="lblRollNo" runat="server" CssClass="ClspaddingL" 
																			Text='<%# Eval("Roll_No") %>' />
																	</td>
																	<td align="left" width="35%">
																		<asp:Label ID="lblName" runat="server" CssClass="ClspaddingL" 
																			Text='<%#Eval("Name")%>' />
																	</td>
																	<td align="left" class="paddingLR" width="30%">
																		<asp:FileUpload ID="FileUploadLogo" runat="server" />
																	</td>
																	<td align="center" class="paddingLR" width="5%">
																		<asp:ImageButton ID="ibtnPhoto" runat="server" 
																			ImageUrl="~/RITeSchool/images/WebCam.png" ToolTip="Capture Photo" />
																			<asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
																	</td>
																	<td align="center" class="paddingLR" width="5%">
																		<asp:Image ID="imgPhoto" runat="server" 
																			ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
																	</td>
																</tr>
															</ItemTemplate>
															<AlternatingItemTemplate>
																<tr ID="Tr3" runat="server" class="ClsGridAltRow">
																	<td align="left" width="10%">
																		<asp:Label ID="lblRegNo" runat="server" CssClass="ClspaddingL" 
																			Text='<%# Eval("Enrolment_Number") %>' />
																	</td>
																	<td align="center" class="paddingLR" width="10%">
																		<asp:Label ID="lblClass" runat="server" CssClass="paddingLR" 
																			Text='<%# Eval("StandardDivision") %>' />
																	</td>
																	<td align="left" width="10%">
																		<asp:Label ID="lblRollNo" runat="server" CssClass="ClspaddingL" 
																			Text='<%# Eval("Roll_No") %>' />
																	</td>
																	<td align="left" width="35%">
																		<asp:Label ID="lblName" runat="server" CssClass="ClspaddingL" 
																			Text='<%#Eval("Name")%>' />
																	</td>
																	<td align="left" class="paddingLR" width="30%">
																		<asp:FileUpload ID="FileUploadLogo" runat="server" />
																	</td>
																	<td align="center" class="paddingLR" width="5%">
																		<asp:ImageButton ID="ibtnPhoto" runat="server" 
																			ImageUrl="~/RITeSchool/images/WebCam.png" ToolTip="Capture Photo" />
																			<asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
																	</td>
																	<td align="center" class="paddingLR" width="5%">
																		<asp:Image ID="imgPhoto" runat="server" 
																			ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
																	</td>
																</tr>
															</AlternatingItemTemplate>
															<EmptyDataTemplate>
																<table width="100%">
																	<tr>
																		<td align="Center" class="LblNoRecord">
																			No record found.
																		</td>
																	</tr>
																</table>
															</EmptyDataTemplate>
														</asp:ListView>
													</td>
												</tr>
												<tr>
													<td>
														<asp:ObjectDataSource ID="lstvwStudentDSobj" runat="server" 
															EnableCaching="false" EnablePaging="true" SelectCountMethod="CountRows" 
															SelectMethod="GetAllStudents" SortParameterName="sortExpression" 
															TypeName="BusinessLogic.StudentBL">
															<SelectParameters>
																<asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" 
																	Type="int32" />
																<asp:SessionParameter Name="aiAcademicYearId" 
																	SessionField="S_CURRENT_ACADEMIC_YEAR_ID" Type="string" />
																<asp:ControlParameter ControlID="hidStandardId" Name="aiStandardId" 
																	PropertyName="Value" />
																<asp:ControlParameter ControlID="hidDivisionId" Name="aiDivisionId" 
																	PropertyName="Value" />
																<asp:ControlParameter ControlID="txtName" Name="asName" PropertyName="Text" />
																<asp:ControlParameter ControlID="txtReg" Name="asRegNo" PropertyName="Text" />
																<asp:ControlParameter ControlID="hidIsExactMatch" Name="abIsExactMatch" 
																	PropertyName="Value" />
																<asp:ControlParameter ControlID="cmbOperation" Name="asOperator" 
																	PropertyName="SelectedValue" />
																<asp:ControlParameter ControlID="cmbPrefix" Name="asPrefix" 
																	PropertyName="SelectedValue" />
																<asp:ControlParameter ControlID="chkStudentWithoutPhoto" Name="abPhotoFilePath" 
																	PropertyName="Checked" />
															</SelectParameters>
														</asp:ObjectDataSource>
													</td>
												</tr>
											</table>
										</td>
									</tr>
									<tr>
										<td ID="td1" runat="server" align="center" colspan="1">
											<asp:HiddenField ID="hidStandardId" runat="server" />
											<asp:HiddenField ID="hidDivisionId" runat="server" />
											<asp:HiddenField ID="hidCount" runat="server" />
											<asp:HiddenField ID="hidFilePath" runat="server" />
											<asp:HiddenField ID="hidUserHasFullAccess" runat="server" />
											<asp:HiddenField ID="HidBackUrl" runat="server" />
											<asp:HiddenField ID="hidPageNo" runat="server" />
											<asp:HiddenField ID="hidIsExactMatch" runat="server" Value="False" />
											<asp:HiddenField ID="hidOperator" runat="server" />
											<asp:HiddenField ID="hidPrefix" runat="server" />
											<asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
										</td>
									</tr>
									<tr>
										<td ID="tdBack" runat="server" align="center" colspan="1">
											<asp:Button ID="btnUpload" runat="server" CssClass="ClsBtn" 
												OnClick="btnUpload_Click" Text="Upload" ValidationGroup="Photo" />
											<asp:Button ID="btnClose" runat="server" CausesValidation="false" 
												CssClass="ClsBtn" OnClick="btnClose_Click" Text="Close" />
										</td>
									</tr>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnUpload" />
                            <asp:PostBackTrigger ControlID="cmbDivision" />
                            <asp:PostBackTrigger ControlID="btnSearch" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwStudentPhoto.ClientID %>";
        _clienthidCountID = "<%=this.hidCount.ClientID %>";
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>";
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";
        _clientoptExact = "<%=this.optExact.ClientID %>";
        _clienttxtReg = "<%=this.txtReg.ClientID %>";
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _ClientLblSucessMessage = "<%=this.lblUpdateSucess.ClientID %>"
        _clientddlDivision = "<%=this.cmbDivision.ClientID%>";
        _clientddlStandard = "<%=this.cmbStandard.ClientID%>";
        //This function is used to check file type.
        function CheckFileType(sFileName) {
            var bIsValid;
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG")
                    bIsValid = true;
                else
                    bIsValid = false;
            }
            else
                bIsValid = false;

            return bIsValid;
        }


        //This function is used to validate logo.
        function ValidateLogo(aSrc, args) {
		
	
            var sMessage = "";
            var iPhotoCount = 0;
            var iRowCount = document.getElementById(_clienthidCountID).value;
            document.getElementById(_clientCstValidateLogo).errormessage = "";

            for (i = 0; i < iRowCount; i++) {
                RowNumber = i;
                var UploadFile = _clientListViewID + "_ctrl" + RowNumber + "_" + "FileUploadLogo";
                var myImage = new Image();
                myImage.src = document.getElementById(UploadFile).value;

                var iWidth = myImage.width
                var iHeight = myImage.height

                if (document.getElementById(UploadFile).value != "") {
                    iPhotoCount++;
                    if (!CheckFileType(myImage.src))//if file type is valid                  
                    {
                        sMessage += "Invalid file format at row number " + (parseInt(RowNumber) + 1) + " . \n";
                        document.getElementById(_clientCstValidateLogo).errormessage += "Invalid file format at row number " + (parseInt(RowNumber) + 1) + " . \n";
                    }
                }
            }
                     if (iPhotoCount == 0 && $get(_clienthidIsPhotoCaptured).value == "N") {
                     	sMessage = "There is no photo to upload.";
                document.getElementById(_clientCstValidateLogo).errormessage = "There is no file to upload.";
                $get(_clientlblErrorMsg).innerHTML = "";
                if ($get(_ClientLblSucessMessage) != null)
                	$get(_ClientLblSucessMessage).style.display = "none";
            }

            if (sMessage == "") {
                args.IsValid = true;
                return false;
            }
            else {
                args.IsValid = false;
                return true;
            }
        }

        //This function is used to display message when page index will be changed.
        function MessageAboutUpload(oCmb) {

            var bIsValid;
            if (window.confirm('If you change the page then selected photos from current page will get lost. Do you want to continue?'))
                bIsValid = true;
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false;
            }
            return bIsValid;
        }

        function SetWaitCursor() {            
        }

        function ClearErrorLabel() {
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function') {
                isPageValid = Page_ClientValidate()
            }
        }

        function ValidateRegNo(aSrc, args) {
            if ($get(_clientoptExact).checked) {
                if ($get(_clienttxtReg).value == "") {
                    args.IsValid = false;
                    $get(_clientlblErrorMsg).innerHTML = "";
                    return true;
                }
            }
            args.IsValid = true;
            return false;
           }

           function OpenWebcamPopup(sQueryString) {
               window.open('../Common/WebcamPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=620,height=530').focus();
           	return true;
           }

           function UpdateHiddenField(aRowNo) {
		  
           	$get(_clienthidIsPhotoCaptured).value = "Y";
           	var PhotoCapturedStatus = _clientListViewID + "_ctrl" + aRowNo + "_hidPhotoCapturedStatus";
           	$('#' + PhotoCapturedStatus).val("Y");
           	if (aRowNo % 2 == 0) {
           		var Row2 = _clientListViewID + "_ctrl" + aRowNo + "_" + "Tr2";
           		$('#' + Row2).removeClass('ClsGridRow');
           		$('#' + Row2).addClass('PhotoCapturd');
           	}
           	else {
           		var Row3 = _clientListViewID + "_ctrl" + aRowNo + "_" + "Tr3";
           		$('#' + Row3).removeClass('ClsGridAltRow');
           		$('#' + Row3).addClass('PhotoCapturd');
           	}
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
            BindAutoCompleteEvent(SchoolId, AcademicYearId, _clienttxtRegNumber, _clientddlStandard, _clientddlDivision, null, 1);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.

        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }
    </script>

</asp:Content>
