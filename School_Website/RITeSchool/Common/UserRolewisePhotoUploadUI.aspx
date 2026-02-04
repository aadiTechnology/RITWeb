<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="UserRolewisePhotoUploadUI.aspx.cs" Inherits="UserRolewisePhotoUploadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="False" EnableViewState="False"
                                    CssClass="LblErrorMsg"></asp:Label>
                                <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                    ValidationGroup="Upload" />
                                <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ErrorMessage="Invalid file format."
                                    ClientValidationFunction="ValidateLogo" ValidationGroup="Upload" CssClass="LblErrorMsg"></asp:CustomValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" class="ClsTextNormal" align="center">
                                <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                    Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server">
                                <table cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">User Role :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:DropDownList ID="cmbUserRole" runat="server" AutoPostBack="true" CssClass="MidCombo"
                                                OnSelectedIndexChanged="cmbUserRole_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trStd" runat="server" visible="false">
                                        <td align="center" class="ClsBorderlight" style="width: 48%">
                                            <span class="ClsLabel">Standard :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr id="trDiv" runat="server" visible="false">
                                        <td align="center" class="ClsBorderlight" style="width: 48%">
                                            <span class="ClsLabel">Division :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:DropDownList ID="cmbDivision" CssClass="MidCombo" runat="server" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" EnableViewState="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <span class="ClsLabel">User Name :</span>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="MidTxtBox" MaxLength="100" autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel" id="lblCheckStudent">Show users without Photo</span>
                                        </td>
                                        <td align="left" colspan="1" class="ClsBorderlight">
                                            <asp:CheckBox ID="chkUserWithPhoto" runat="server" Checked="True" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center" class="ClsBorderlight">
                                            &nbsp;<asp:Button ID="btnSearch" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                                Text="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight" colspan="2" style="padding-left: 10px">
                                            <span class="LblSmlGray"> Upload or Capture photo for selected user(s). (Max Height: 151px and Max Width: 112px).<br />
                                                (Image size should not exceed 1 mb. Supported file formats are JPG, JPEG)</span>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="100%">
								<tr>
								<td style="padding-left:273px">
								<table>
									<tr>
										<td>
											<span class="ClsLblLgnd" style="font: Bold; width:auto">Legend :</span>
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
								</td>
								</tr>
                                    <tr id="trPhotoPager" runat="server">
                                        <td align="center">
                                            <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwUserPhotoDetails">
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
                                        <td valign="top" align="center">
                                            <asp:ListView ID="lstvwUserPhotoDetails" runat="server" DataKeyNames="UserId, UserRoleId,PhotoFilePath,BinaryPhotoImage"
                                                OnItemDataBound="lstvwUserPhotoDetails_ItemDataBound" OnDataBound="lstvwUserPhotoDetails_DataBound">
                                                <LayoutTemplate>
                                                    <table width="70%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                        cellspacing="1" class="GridBorder">
                                                        <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                            <th align="center">
                                                                Sr. No.
                                                            </th>
                                                            <th class="paddingLR" align="left">
                                                                Name
                                                            </th>
                                                            <th class="paddingLR" align="left">
                                                                Photo Browse
                                                            </th>
															<th class="paddingLR" align="center"> Webcam</th>
                                                            <th class="paddingLR" align="center">
                                                                Photo
                                                            </th>
                                                            <th class="paddingLR" align="center">
                                                                Remove Photo
                                                            </th>
                                                        </tr>
                                                        <tr id="itemPlaceholder" runat="server">
                                                        </tr>
                                                        <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                            cellpadding="0" cellspacing="1">
                                                            <td align="left" colspan="6">
                                                                <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwUserPhotoDetails"
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
                                                        <td align="center" width="9%">
                                                            <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' />
                                                        </td>
                                                        <td width="35%" align="left">
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="paddingLR" />
                                                        </td>
                                                        <td class="paddingLR" align="left" width="25%">
                                                            <asp:FileUpload ID="FileUploadPhoto" runat="server" />
                                                        </td>
														<td class="paddingLR" align="center" width="5%">
														<asp:ImageButton ID="ibtnPhoto" ToolTip="Capture Photo" runat="server" ImageUrl="~/RITeSchool/images/WebCam.png" />
															<asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
                                                        </td>
                                                        <td class="paddingLR" align="center" width="5%">
                                                            <asp:Image ID="imgPhoto"  runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                        </td>
                                                        <td class="paddingLR" align="center" width="15%">
                                                             <asp:CheckBox ID="chkremovephoto" runat="server" />
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                        <td align="center" width="9%">
                                                            <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("RowNo") %>' />
                                                        </td>
                                                        <td width="35%" align="left">
                                                            <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName")%>' CssClass="paddingLR" />
                                                        </td>
                                                        <td class="paddingLR" align="left" width="25%">
                                                            <asp:FileUpload ID="FileUploadPhoto" runat="server" />
                                                        </td>
														<td class="paddingLR" align="center" width="5%">
															<asp:ImageButton ID="ibtnPhoto" ToolTip="Capture Photo" runat="server" ImageUrl="~/RITeSchool/images/WebCam.png" />
															<asp:HiddenField ID="hidPhotoCapturedStatus" runat="server" Value="N" />
                                                        </td>
                                                        <td class="paddingLR" align="center" width="5%">
                                                            <asp:Image ID="imgPhoto"  runat="server" ImageUrl="~/RITeSchool/images/IconGrid_AssignTrue.gif" />
                                                        </td>
                                                         <td class="paddingLR" align="center" width="15%">
                                                             <asp:CheckBox ID="chkremovephoto" runat="server" />
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:ListView>
                                        </td>
                                    </tr>
                                    <tr id="trNoRecordMsg" runat="server">
                                        <td style="height: 10px;" align="center">
                                            <asp:Label ID="lblNoRecordMsg" runat="server" CssClass="LblNoRecord" Font-Bold="True"
                                                Text="No record found." EnableViewState="False" Width="70%"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ObjectDataSource TypeName="BusinessLogic.UserRolewisePhotoUploadBL" EnablePaging="true"
                                                ID="lstvwDsObj" runat="server" SelectMethod="GetUserDetailsForPhotoUplaod" SelectCountMethod="CountUserForPhotoUplaod"
                                                EnableCaching="false">
                                                <SelectParameters>
                                                    <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                    <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                        Type="int32" />
                                                    <asp:ControlParameter ControlID="cmbUserRole" PropertyName="SelectedValue" Name="aiUserRoleId" />
                                                    <asp:ControlParameter ControlID="txtUserName" PropertyName="Text" Name="asUserName"
                                                        DefaultValue="" />
                                                    <asp:ControlParameter ControlID="hidStandardId" Type="Int32" PropertyName="Value"
                                                        DefaultValue="0" Name="aiStandardId" />
                                                    <asp:ControlParameter ControlID="hidDivisionId" Type="Int32" PropertyName="Value"
                                                        DefaultValue="0" Name="aiDivisionId" />
                                                    <asp:ControlParameter ControlID="chkUserWithPhoto" PropertyName="Checked" Name="abChkUserWithPhotoFlag" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                            <asp:HiddenField ID="hidCount" runat="server" />
                                            <asp:HiddenField ID="hidPageNo" runat="server" />
                                            <asp:HiddenField ID="hidRowCnt" runat="server" />
                                            <asp:HiddenField ID="hidFilePath" runat="server" />
                                            <asp:HiddenField ID="hidStandardId" runat="server" Value="0" />
                                            <asp:HiddenField ID="hidDivisionId" runat="server" Value="0" />
											<asp:HiddenField ID="hidIsPhotoCaptured" runat="server" Value="N" />
                                            <asp:HiddenField ID="hidRemovePhoto" runat="server" Value="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="1" id="tdBack" runat="server">
                                            <asp:Button ID="btnUpload" runat="server" Text="Save" CssClass="ClsBtn" ValidationGroup="Upload"
                                                OnClick="btnUpload_Click" />
                                            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="ClsBtn" CausesValidation="false"
                                                onclick="btnExport_Click"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwUserPhotoDetails.ClientID %>";
        _clienthidCountID = "<%=this.hidCount.ClientID %>";
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>";
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>";
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _clienthidIsPhotoCaptured = "<%=this.hidIsPhotoCaptured.ClientID %>"
     

        //This function is used to check file type.
        function CheckFileType(sFileName) {
            if (document.getElementById(_clientlblErrorMsg) != undefined)
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
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
            if (document.getElementById(_clientlblUpdateSucess) != undefined) {
                document.getElementById(_clientlblUpdateSucess).innerHTML = "";
                document.getElementById(_clientlblUpdateSucess).innerText = "";
            }
            if (document.getElementById(_clientlblErrorMsg) != undefined)
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            var sMessage = "";
            var iPhotoCount = 0;
            var removePhotoCount = 0
            var iRowCount = document.getElementById(_clienthidCountID).value;
            document.getElementById(_clientCstValidateLogo).errormessage = "";
            var iCnt = 0;
            var fileSizeIndexes = 0
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i;
                var UploadFile = _clientListViewID + "_ctrl" + RowNumber + "_" + "FileUploadPhoto";
                var PhotoCapturedStatus = _clientListViewID + "_ctrl" + RowNumber + "_hidPhotoCapturedStatus";
                var RemovePhotoStatus = _clientListViewID + "_ctrl" + RowNumber + "_chkremovephoto";
                
                if (document.getElementById(RemovePhotoStatus)!= null && document.getElementById(RemovePhotoStatus).checked)
                    removePhotoCount = 1;

                var myImage = new Image();
                myImage.src = document.getElementById(UploadFile).value;

                var iWidth = myImage.width
                var iHeight = myImage.height

                if (document.getElementById(UploadFile).value != "") {
                    iPhotoCount++;
                    if (!CheckFileType(myImage.src))//if file type is valid
                    {
                        if (iCnt == 0)
                            iCnt = (parseInt(RowNumber) + 1);
                        else
                            iCnt = iCnt + ", " + (parseInt(RowNumber) + 1);
                    }
                    else {
                        if (document.getElementById(UploadFile).files[0].size > 1048576) {
                            if (fileSizeIndexes == 0)
                                fileSizeIndexes = (parseInt(RowNumber) + 1);
                            else
                                fileSizeIndexes = fileSizeIndexes + ", " + (parseInt(RowNumber) + 1);
                        }
                    }
                }
            }
            if (iCnt != 0 && $('#' + PhotoCapturedStatus).val() == "N") {
            	sMessage = "Invalid file format at row number(s): " + iCnt + ". ";
            	document.getElementById(_clientCstValidateLogo).errormessage += "Invalid file format at row number(s): " + iCnt + ". ";
         }

         if (fileSizeIndexes != 0 && $('#' + PhotoCapturedStatus).val() == "N") {
             sMessage = "Photo file size should not be more than 1 MB at row number(s): " + fileSizeIndexes + ". ";
             document.getElementById(_clientCstValidateLogo).errormessage += "Photo file size should not be more than 1 MB at row number(s): " + fileSizeIndexes + ". ";
         }

         if (iPhotoCount == 0 && $get(_clienthidIsPhotoCaptured).value == "N" && removePhotoCount == 0) {
                sMessage = "There is no photo to upload.";
                document.getElementById(_clientCstValidateLogo).errormessage = "There is no file to upload.";
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
            if (window.confirm('If you change the page then selected file paths from current page will get lost. Do you want to continue?'))
                bIsValid = true;
            else {
                document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
                bIsValid = false;
            }
            return bIsValid;
           }

           function OpenWebcamPopup( sQueryString) {
               window.open('WebcamNewPopup.aspx?' + sQueryString, 'mywindow', 'scrollbars=yes,resizable=no,top=0,left=0,width=700,height=400');
           	return true;
           }

           function UpdateHiddenField(aRowNo) {
           	$get(_clienthidIsPhotoCaptured).value = "Y";
           	var PhotoCapturedStatus = _clientListViewID + "_ctrl" + aRowNo + "_hidPhotoCapturedStatus";
            	$('#'+PhotoCapturedStatus).val("Y");
           	if (aRowNo % 2 == 0) {
           		var Row2 = _clientListViewID + "_ctrl" + aRowNo +"_" + "Tr2";
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
            _slienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"
            _clientddlUserRole = '<%=cmbUserRole.ClientID%>';
            var _clientddlStandard = '<%=cmbStandard.ClientID%>';
            var _clientddlDivision = '<%=cmbDivision.ClientID%>';


            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _slienttxtUserName, _clientddlUserRole, 1, _clientddlStandard, _clientddlDivision, null);
        }

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

	</script>

</asp:Content>
