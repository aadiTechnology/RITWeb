<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="StudentLCUploadUI.aspx.cs" Inherits="StudentLCUploadUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 98%; height: 100%;">
            <tr>
                <td id="MainDataTable" align="center" valign="top">
                    <!-- Data Insert Here -->
                    <table border="0" cellpadding="0" cellspacing="2" style="width: 100%;">
                        <tr>
                            <td align="left">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="False" EnableViewState="False"
                                            CssClass="LblErrorMsg"></asp:Label>
                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" CssClass="ClsLabel" ShowSummary="true"
                                            ValidationGroup="Upload" />
                                        <asp:CustomValidator ID="cstValidateLogo" Display="None" runat="server" ErrorMessage="Invalid file format."
                                            ClientValidationFunction="ValidateLogo" ValidationGroup="Upload" CssClass="LblErrorMsg"></asp:CustomValidator>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnRemoveLC" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="chkUserWithLC" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="1" class="ClsTextNormal" align="center">
                                <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Height="20px" Width="100%"
                                            Visible="False" EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnRemoveLC" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="chkUserWithLC" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" id="tblSearch" runat="server">
                                <table cellpadding="0" cellspacing="2">
                                    <tr>
                                        <td align="center" class="ClsBorderlight">
                                            <span class="ClsLabel">Standard :</span>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="cmbStandard" runat="server" CssClass="MidCombo" AutoPostBack="true"
                                                OnSelectedIndexChanged="cmbStandard_SelectedIndexChanged" ViewStateMode="Enabled">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsBorderlight">
                                            <span class="ClsLabel">Division :</span>
                                        </td>
                                        <td align="left">
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="cmbDivision" CssClass="MidCombo" runat="server" AutoPostBack="true"
                                                        OnSelectedIndexChanged="cmbDivision_SelectedIndexChanged" EnableViewState="true">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">User Name :</span>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="MidTxtBox" MaxLength="100"
                                                autocomplete="off"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel" id="lblCheckStudent">Show users without LC</span>
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkUserWithLC" runat="server" Checked="True" OnCheckedChanged="chkUserWithLC_CheckedChanged"
                                                AutoPostBack="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            &nbsp;<asp:Button ID="btnSearch" CausesValidation="false" runat="server" CssClass="ClsBtn"
                                                Text="Search" OnClick="btnSearch_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr align="center">
                            <td>
                                <table align="center" style="text-align: center;" width="65%">
                                    <tr>
                                        <td align="left" class="ClsBorderlight " style="background-color: #ffffc4;">
                                            <asp:Label ID="Label9" runat="server" BorderWidth="0px" Font-Bold="True" Text="Note :"
                                                CssClass="LblNrmlB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">
                                            <asp:Label ID="Label11" runat="server" BorderWidth="0px" CssClass="LblSmlV" Text="File size should not exceed 250kb. Supported file formats are JPG, JPEG, PNG, BMP, PDF."></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr id="trLCPager" runat="server">
                                                <td align="center">
                                                    <asp:DataPager ID="DtPgCount" runat="server" PageSize="20" PagedControlID="lstvwStudentLCDetails">
                                                        <Fields>
                                                            <asp:TemplatePagerField>
                                                                <PagerTemplate>
                                                                    <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                        CssClass="LblNrmlB" EnableViewState="false" />
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
                                                    <asp:ListView ID="lstvwStudentLCDetails" runat="server" DataKeyNames="StudentId, LCFilePath, LCUploadStatus"
                                                        OnItemDataBound="lstvwStudentLCDetails_ItemDataBound" OnDataBound="lstvwStudentLCDetails_DataBound">
                                                        <LayoutTemplate>
                                                            <table width="65%" runat="server" id="Table1" style="color: #333333" cellpadding="0"
                                                                class="GridBorder">
                                                                <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                    <th align="center" style="width: 5%;">
                                                                        Reg. No.
                                                                    </th>
                                                                    <th align="center" style="width: 5%;">
                                                                        Roll No.
                                                                    </th>
                                                                    <th class="paddingLR" align="left">
                                                                        Name
                                                                    </th>
                                                                    <th class="paddingLR" align="left" style="width: 10%;">
                                                                        LC Browse
                                                                    </th>
                                                                    <th class="paddingLR" align="center" style="width: 10%;">
                                                                        View
                                                                    </th>
                                                                    <th class="paddingLR" align="center" style="width: 10%;">
                                                                        Remove
                                                                    </th>
                                                                </tr>
                                                                <tr id="itemPlaceholder" runat="server">
                                                                </tr>
                                                                <tr class="ClsBorderPager" width="100%" runat="server" id="trDataPager" style="color: #333333"
                                                                    cellpadding="0" cellspacing="1">
                                                                    <td align="left" colspan="6">
                                                                        <asp:DataPager ID="DtPgDropDown" runat="server" PagedControlID="lstvwStudentLCDetails"
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
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("EnrollmentNo") %>' />
                                                                    <asp:HiddenField ID="hidlc" runat="server" />
                                                                </td>
                                                                <td align="center" width="9%">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RollNo") %>' />
                                                                </td>
                                                                <td width="35%" align="left">
                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("StudentName")%>' CssClass="paddingLR" />
                                                                </td>
                                                                <td class="paddingLR" align="left">
                                                                    <asp:FileUpload ID="FileUploadLC" runat="server" Text='<%#Eval("LCFilePath")%>' CssClass="paddingLR" />
                                                                </td>
                                                                <td class="paddingLR" align="center">
                                                                    <asp:ImageButton ID="ibtnIsStudentLC" runat="server" Visible="false" ImageAlign="Right" />
                                                                    <asp:HiddenField ID="hidPhotoUploadStatus" runat="server" Value="N" />
                                                                </td>
                                                                <td class="paddingLR" align="center">
                                                                    <asp:CheckBox ID="chkRemoveLC" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                        <AlternatingItemTemplate>
                                                            <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                <td align="center">
                                                                    <asp:Label ID="lblRollNo" runat="server" Text='<%# Eval("EnrollmentNo") %>' />
                                                                    <asp:HiddenField ID="hidlc" runat="server" />
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("RollNo") %>' />
                                                                </td>
                                                                <td width="35%" align="left">
                                                                    <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("StudentName")%>' CssClass="paddingLR" />
                                                                </td>
                                                                <td class="paddingLR" align="left">
                                                                    <asp:FileUpload ID="FileUploadLC" runat="server" Text='<%#Eval("LCFilePath")%>' CssClass="paddingLR" />
                                                                </td>
                                                                <td class="paddingLR" align="center">
                                                                    <asp:ImageButton ID="ibtnIsStudentLC" runat="server" Visible="false" ImageAlign="Right" />
                                                                    <asp:HiddenField ID="hidPhotoUploadStatus" runat="server" Value="N" />
                                                                </td>
                                                                <td class="paddingLR" align="center">
                                                                    <asp:CheckBox ID="chkRemoveLC" runat="server" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </AlternatingItemTemplate>
                                                        <EmptyDataTemplate>
                                                            <table width="65%">
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
                                                <td align="center" colspan="1" id="tdBack" runat="server">
                                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" CssClass="ClsBtn" ValidationGroup="Upload"
                                                        OnClick="btnUpload_Click" />
                                                    <asp:Button ID="btnRemoveLC" runat="server" Text="Remove" CssClass="ClsBtn" OnClick="btnRemoveLC_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:ObjectDataSource TypeName="BusinessLogic.StudentLCUploadBL" EnablePaging="true"
                                                        ID="lstvwDsObj" runat="server" SelectMethod="GetStudentLCUpload" SelectCountMethod="CountLCUplaod"
                                                        EnableCaching="false">
                                                        <SelectParameters>
                                                            <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                            <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                Type="int32" />
                                                            <asp:ControlParameter ControlID="txtUserName" PropertyName="Text" Name="asUserName"
                                                                DefaultValue="" />
                                                            <asp:ControlParameter ControlID="cmbStandard" Type="String" PropertyName="SelectedValue"
                                                                DefaultValue="0" Name="aiStandardId" />
                                                            <asp:ControlParameter ControlID="cmbDivision" Type="String" PropertyName="SelectedValue"
                                                                DefaultValue="0" Name="aiDivisionId" />
                                                            <asp:ControlParameter ControlID="chkUserWithLC" PropertyName="Checked" Name="abChkUserWithLCFlag" />
                                                        </SelectParameters>
                                                    </asp:ObjectDataSource>
                                                    <asp:HiddenField ID="hidCount" runat="server" />
                                                    <asp:HiddenField ID="hidPageNo" runat="server" />
                                                    <asp:HiddenField ID="hidRowCnt" runat="server" />
                                                    <asp:HiddenField ID="hidFilePath" runat="server" />
                                                    <asp:HiddenField ID="hidRemoveLC" runat="server" Value="False" />
                                                    <asp:HiddenField ID="hidFirstFxFollowingErrors" runat="server" Value="" />                                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnUpload" />
                                        <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnRemoveLC" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbStandard" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="cmbDivision" EventName="SelectedIndexChanged" />
                                        <asp:AsyncPostBackTrigger ControlID="chkUserWithLC" EventName="CheckedChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">

        _clientListViewID = "<%=this.lstvwStudentLCDetails.ClientID %>"
        _clienthidCountID = "<%=this.hidCount.ClientID %>"
        _clientCstValidateLogo = "<%=this.cstValidateLogo.ClientID %>"
        _clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
        _clientlblErrorMsg = "<%=this.lblErrorMsg.ClientID %>";
        _clientlblUpdateSucess = "<%=this.lblUpdateSucess.ClientID %>"
        _ClientListValue = "<%=this.lstvwStudentLCDetails.ClientID %>"        

        $(document).ready(function () {
            AutoSearch();
        });

        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(EndRequestHandler);

        // This function is used to enabled controls once a postback is complete.
        function EndRequestHandler() {
            AutoSearch();
        }

        //This function is used to check file type.
        function CheckFileType(sFileName) {
            if (document.getElementById(_clientlblErrorMsg) != undefined)
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            var bIsValid;
            if (sFileName != "") {
                if (sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".JPG" || sFileName.substr(sFileName.lastIndexOf('.'), 5).toUpperCase() == ".JPEG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PDF" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".PNG" || sFileName.substr(sFileName.lastIndexOf('.'), 4).toUpperCase() == ".BMP")
                    bIsValid = true;
                else
                    bIsValid = false;
            }
            else
                bIsValid = false;

            return bIsValid;
        }

        function CheckForFileSize(UploadFile) {
            if (document.getElementById(_clientlblErrorMsg) != undefined)
                document.getElementById(_clientlblErrorMsg).innerHTML = ""
            var bIsValid;

            var maxFileSize = 4194304; // 4MB -> 4 * 1024 * 1024
            var fileUpload = $('#UploadFile');

            if (fileUpload.val() == '') {
                return false;
            }
            else {
                if (fileUpload[0].files[0].size < maxFileSize) {                    
                    bIsValid = true;
                } else {                    
                    bIsValid = false;
                }
            }
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
            var iCount = 0;
            for (i = 0; i < iRowCount; i++) {
                RowNumber = i;
                var UploadFile = _clientListViewID + "_ctrl" + RowNumber + "_" + "FileUploadLC";
                var PhotoCapturedStatus = _clientListViewID + "_ctrl" + RowNumber + "_hidPhotoUploadStatus";
                var RemovePhotoStatus = _clientListViewID + "_ctrl" + RowNumber + "_chkremovephoto";

                if (document.getElementById(RemovePhotoStatus) != null && document.getElementById(RemovePhotoStatus).checked)
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
                }
            }
            if (iCnt != 0) {
                sMessage = "Invalid file format at row number(s): " + iCnt + ". ";
                document.getElementById(_clientCstValidateLogo).errormessage += "Invalid file format at row number(s): " + iCnt + ". ";
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

        function CheckCheckBoxisChecked() {
            var iRowCount = 0;
            var chkRemove = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_chkRemoveLC");
            var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value
            var sValue = false;
            while (chkRemove != null) {
                if (chkRemove != null && chkRemove.checked) {
                    sValue = true;
                    break;
                }                
                iRowCount++;
                chkRemove = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_chkRemoveLC");
            }
            if (sValue == true) {
                return true;
            }
            else {
                alert(msgHeader + "\n" + "Please select at least one check box for remove LC.");
                return false;   
            }            
        }

        function CheckFileIsUploaded() {            
            var iRowCount = 0;
            var FileUpload = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_FileUploadLC");
            var msgHeader = document.getElementById("<%=hidFirstFxFollowingErrors.ClientID %>").value
            var sValue = false;
            while (FileUpload != null) {
                if (FileUpload != null && FileUpload.files.length != 0) {                    
                    sValue = true;
                    break;
                }
                iRowCount++;
                FileUpload = document.getElementById(_ClientListValue + "_ctrl" + iRowCount + "_FileUploadLC");
            }
            if (sValue == true) {
                return sValue;
            }
            else {
                alert(msgHeader + "\n" + "Please upload at least one file.");
                return false;
            }
        }

        function AutoSearch() {
            _clienttxtUserName = '#<%=txtUserName.ClientID%>';
            var SchoolId = "<%=miSchoolId %>";
            var AcademicYearId = "<%=miAcademicYearId %>"

            var _clientddlStandard = '<%=cmbStandard.ClientID%>';
            var _clientddlDivision = '<%=cmbDivision.ClientID%>';

            BindAutoCompleteEventForUser(SchoolId, AcademicYearId, _clienttxtUserName, 1, _clientddlStandard, _clientddlDivision, null);
        }

        function SearchSelectedValue(val) {
            txt = document.getElementById("<%=this.txtUserName.ClientID %>");
            bt = document.getElementById("<%=this.btnSearch.ClientID %>");
            SearchResult(txt, val, bt);
        }

        function openfile(index) {
            var Path = document.getElementById(_clientListViewID + '_ctrl' + index + '_hidlc').value;
            window.open(Path);
        }

    </script>
</asp:Content>
