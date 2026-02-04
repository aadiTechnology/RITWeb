<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/PopupMaster.master"
    AutoEventWireup="true" CodeFile="TeacherPhotoUI.aspx.cs" Inherits="TeacherPhotoUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PopupMainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td style="background-color: white;" id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                          <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                                <tr>
                                    <td align="left">
                                        <table border="0" runat="server" id="tblHeader" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td style="height: 20px" class="ClsGrayMainTitle">
                                                    <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px;">
                                                        <tr>
                                                            <td align="center" class="MainTitleHead" style="height: 20px">
                                                                
                                                                <span style="font-weight: bold">Teacher Photos</span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="pnlErrorMsg" runat="server" Width="90%">
                                            <asp:Label ID="lblErrorMsg"  runat="server" Font-Bold="False"
                                                 EnableViewState="False" CssClass="LblErrorMsg"></asp:Label>
                                            <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowMessageBox="true"
                                                ShowSummary="false" />
                                            <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ClientValidationFunction="ValidateLogo"
                                                ErrorMessage="Invalid file format." CssClass="LblErrorMsg"></asp:CustomValidator>
                                        </asp:Panel>
                                    </td>
                                </tr>
								<tr>
									<td colspan="1" class="ClsTextNormal" align="center">
                                   <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                    EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                   </td>
								</tr>
                                <tr>
                                    <td align="center" id="tblSearch" runat="server">
                                        <table cellpadding="0" cellspacing="2">
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" style="padding-left: 10px">
                                                    <asp:Label ID="lblTeacherName" Text="Teacher Name :" runat="server" />
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                    
                                                    <asp:TextBox ID="txtTeacherName" runat="server" CssClass="MidTxtBox" MaxLength="150" autocomplete="off"></asp:TextBox>
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                   <span class="LblNormal" id="lblCheckStudent">Show Teacher without Photo</span>
                                                </td>
                                                <td align="left" class="ClsBorderlight">
                                                     <asp:CheckBox ID="chkTeacherWithoutPhoto" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    &nbsp;<asp:Button ID="btnSearch" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                                        Text="Search" OnClick="btnSearch_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 10px">
                                                   
                                                    <span class="LblSmlGray">Upload or Capture photo for Teacher's.(Max Height: 151px
                                                        and Max Width: 112px).<br />
                                                        (Image size should not exceed 80 kb. Supported file formats are JPG, JPEG)</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
								<tr><td align="left" style="padding-left:40px"><table>
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
				</table></td></tr>
                                <tr>
                                    <td align="center">
                                        <table width="100%">
                                            <tr id="Tr5" runat="server">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwTeacherPhoto">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblTo" runat="server" CssClass="LblNormal" Text=" To " EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblOutOf" runat="server" CssClass="LblNormal" Text=" Out Of " EnableViewState="false" />
                                                                    <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                        CssClass="LblNrmlB" />
                                                                    <asp:Label ID="lblRecords" runat="server" CssClass="LblNormal" Text="Records " EnableViewState="false" />
                                                                    <br />
                                                                </PagerTemplate>
                                                            </asp:TemplatePagerField>
                                                        </Fields>
                                                    </asp:DataPager>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <asp:ListView ID="lstvwTeacherPhoto" runat="server" DataKeyNames="Photo_file_Path,Teacher_Id,User_Id,BinaryPhotoImage"
                                                        OnDataBound="lstvwTeacherPhoto_DataBound" OnItemDataBound="lstvwTeacherPhoto_ItemDataBound">
                                                        <LayoutTemplate>
                                                            <table width="90%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                cellspacing="1" class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="left" class="ClspaddingL">
                                                                        Sr. No.
                                                                    </th>
                                                                    <th class="ClspaddingL">
                                                                        Teacher Name
                                                                    </th>
                                                                    <th class="paddingLR" align="left">
                                                                        Photo Browse
                                                                    </th>
																	<th class="paddingLR" align="center"> Webcam</th>
                                                                    <th class="paddingLR" align="center">
                                                                        Photo
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1">
                                                                    <td align="left" colspan="5">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwTeacherPhoto"
                                                                            PageSize="20">
                                                                            <Fields>
                                                                                <asp:TemplatePagerField>
                                                                                    <PagerTemplate>
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:Label ID="MessageLabel" Text="Select a page:" runat="server" CssClass="LblNrmlB" />
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
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="left" width="10%">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' CssClass="ClspaddingL" />
                                                                </td>
                                                                <td width="35%" align="left">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Teacher_Name")%>' CssClass="ClspaddingL" />
                                                                </td>
                                                                <td class="paddingLR" align="left" width="30%">
                                                                    <asp:FileUpload ID="FileUploadLogo" runat="server" />
                                                                </td>
																<td class="paddingLR" align="center" width="5%">
														       <asp:ImageButton ID="ibtnPhoto" ToolTip="Capture Photo" runat="server" ImageUrl="~/RITeSchool/images/WebCam.png" />
															   <asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
                                                                </td>
                                                                <td class="paddingLR" align="center" width="5%">
                                                                    <asp:Image ID="imgPhoto" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                <td align="left" width="10%">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' CssClass="ClspaddingL" />
                                                                </td>
                                                                <td width="35%" align="left">
                                                                    <asp:Label ID="lblName" runat="server" Text='<%#Eval("Teacher_Name")%>' CssClass="ClspaddingL" />
                                                                </td>
                                                                <td class="paddingLR" align="left" width="30%">
                                                                    <asp:FileUpload ID="FileUploadLogo" runat="server" />
                                                                </td>
																<td class="paddingLR" align="center" width="5%">
															    <asp:ImageButton ID="ibtnPhoto" ToolTip="Capture Photo" runat="server" ImageUrl="~/RITeSchool/images/WebCam.png" />
																 <asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
                                                                 </td>
                                                                <td class="paddingLR" align="center" width="5%">
                                                                    <asp:Image ID="imgPhoto" runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="100%">
                                                                <tr>
                                                                    <td class="LblNoRecord" align="center">
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
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.SchoolUserCollectionBL" EnablePaging="true" ID="lstvwTeacherPhotoDsObj"
                                                        runat="server" SelectMethod="GetTeacherDetailsForPhotoUplaod" SortParameterName="sortExpression"
                                                        SelectCountMethod="CountTeachersForPhotoUplaod" EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="string" />
                                                            <asp:ControlParameter ControlID="txtTeacherName" PropertyName="Text" Name="asName" DefaultValue="" />
                                                            <asp:ControlParameter ControlID="chkTeacherWithoutPhoto" PropertyName="Checked" Name="abPhotoFilePath" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1" id="td1" runat="server">
                                        <asp:HiddenField ID="hidCount" runat="server" />
                                        <asp:HiddenField ID="hidFilePath" runat="server" />
                                        <asp:HiddenField ID="hidUserHasFullAccess" runat="server" />
                                        <asp:HiddenField ID="HidBackUrl" runat="server" />
                                        <asp:HiddenField ID="hidPageNo" runat="server" />
										<asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
                                        <asp:HiddenField ID="hidUserRoleId" runat="server" Value="N" />                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="1" id="tdBack" runat="server">
                                        <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="ClsBtn" OnClick="btnUpload_Click" />
                                        <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ClsBtn" 
											CausesValidation="false" onclick="btnClose_Click"
                                             />
                                    </td>
                                </tr>
                            </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwTeacherPhoto.ClientID %>";
        _clienthidCountID = "<%=this.hidCount.ClientID %>";
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>";
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
        _ClientlableUpdate = "<%=this.lblUpdateSucess.ClientID %>"

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
                var PhotoCapturedStatus = _clientListViewID + "_ctrl" + RowNumber + "_hidPhotoCapturedStatus";
                var myImage = new Image();
                myImage.src = document.getElementById(UploadFile).value;

                var iWidth = myImage.width
                var iHeight = myImage.height

                if (document.getElementById(UploadFile).value != "") {
                    iPhotoCount++;
                    if (!CheckFileType(myImage.src) && $('#' + PhotoCapturedStatus).val() == "N" )//if file type is valid                  
                    {
                        sMessage += "Invalid file format at row number " + (parseInt(RowNumber) + 1) + " . \n";
                        document.getElementById(_clientCstValidateLogo).errormessage += "Invalid file format at row number " + (parseInt(RowNumber) + 1) + " . \n";
                    }
                }
            }
                     if (iPhotoCount == 0 && $get(_clienthidIsPhotoCaptured).value == "N") {
                sMessage = "There is no photo to upload";
                document.getElementById(_clientCstValidateLogo).errormessage = "There is no file to upload.";
                if ($get(_ClientlableUpdate) != null)
                	$get(_ClientlableUpdate).style.display = "none";
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

           function OpenWebcamPopup(sQueryString) {
           	window.open('../Common/WebcamPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=620,height=530');
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
            _slienttxtUserName = '#<%=txtTeacherName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            var UserRole = '<%=hidUserRoleId.ClientID%>';
            $get(UserRole).value = 2;

            BindAutoCompleteEventForStaff(SchoolId, AcademicYearId, _slienttxtUserName, UserRole, 1);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtTeacherName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

    </script>

</asp:Content>
