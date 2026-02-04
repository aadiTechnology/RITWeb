<%@ Page Language="C#" MasterPageFile="~/RITeSchool/SuperAdmin/SuperAdminMasterPage.master"
    AutoEventWireup="true" CodeFile="ScreensUI.aspx.cs" Inherits="ScreensUI" Title="Screens page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table id="tblAcademicCmb" runat="server" border="0" cellpadding="0" cellspacing="1"
        style="width: 70%; position: relative" visible="true">
        <tr>
            <td style="width: 100%;" colspan="3">
                <asp:ValidationSummary ID="valsumReturnRenewBook" runat="server" CssClass="LblErrorMsg" />
            </td>
        </tr>
        <tr>
            <td align="left" runat="server" id="tdAcademicCmblbl" class=" ClsBorderlight" width="20%">
                <asp:Label ID="Label10" runat="server" EnableViewState="False" Text="Academic Year : "
                    CssClass="paddingLSML"></asp:Label>
            </td>
            <td align="left" id="tdAcademicCmb" runat="server" width="80%">
                <asp:DropDownList ID="cmbAcademicYearID" runat="server" CssClass="MidCombo" OnSelectedIndexChanged="cmbAcademicYearID_SelectedIndexChanged"
                    AutoPostBack="True">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <table id="tblAdmin" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 70%;
        position: relative;" visible="true">
        <tr>
            <td align="center" class="CPanelSpace" style="height: 18px">
            </td>
            <td align="center" class="CPanelSpace" style="height: 18px">
            </td>
            <td align="center" class="CPanelSpace" style="height: 18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnkDashBoard" runat="server" CssClass="SubTitleMenu" EnableViewState="true"
                    NavigateUrl="../Common/ControlPanel.aspx" Style="position: relative" Visible="true">Dashboard</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnkUserLogin" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="../Admin/LockingUser.aspx" Style="position: relative" Visible="true">User Login</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" style="background-color: orange; height: 2px" colspan="3">
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink3" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/StartNextAcademic.aspx" Style="position: relative"
                    Visible="true">Next Academic Year Generation</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnkAcademinYear" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="../SuperAdmin/SchoolwiseAcademicYearUI.aspx" Style="position: relative"
                    Visible="true">Schoolwise Academic Year</asp:HyperLink>
            </td>
        </tr>

        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
		  <tr id="trFinancialYear" runat="server">
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink5" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/NextFinancialYearUI.aspx" Style="position: relative"
                    Visible="true">Next Financial Year Generation</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink14" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="../SuperAdmin/SchoolwiseFinancialYearsUI.aspx" Style="position: relative"
                    Visible="true">Schoolwise Financial Year</asp:HyperLink>
            </td>
        </tr>

        <tr id="trEmpty" runat="server">
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink6" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/ImportCautionMoney.aspx" Style="position: relative"
                    Visible="true">Import Caution Money</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink8" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/ImportFee.aspx" Style="position: relative"
                    Visible="true">Import Fee</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink9" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/ImportTeacherUI.aspx" Style="position: relative"
                    Visible="true">Import Teachers</asp:HyperLink>
            </td>
            <td align="left">               
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnkStudentTransfer" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="../Admin/ChangeStudentDivision.aspx" Style="position: relative"
                    Visible="true">Student Transfer</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink1" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/SchoolActivationUI.aspx" Style="position: relative">School Activation</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink2" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/RegistrationWizard_Step1.aspx" Style="position: relative">School Registration</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnkChangePassword" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="../Common/StudentChangePassword.aspx" Style="position: relative"
                    Visible="true">Change Password</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink4" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/FeedbackDetailsUI.aspx" Style="position: relative">Feedback Details</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
          <td align="left" class="ClsBorderlight">
              <asp:HyperLink ID="hlnkImportNewAdmissions" runat="server" CssClass="SubTitleMenu"
                    EnableViewState="False" NavigateUrl="~/RITeSchool/Admission/ImportNewAdmissionsUI.aspx"
                    Style="position: relative" Visible="true">Import New Admissions</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="hlnResetTimeTable" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/ResetTimeTableUI.aspx" Style="position: relative"
                    Visible="true">Reset Timetable</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                 <asp:HyperLink ID="HyperLink11" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/IdentityCardDetailsUI.aspx" Style="position: relative">Identity Cards Details</asp:HyperLink>
            </td>
            <td align="left">
            </td>
             <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink7" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/GenerateAllStudentsPRNandlogin.aspx" Style="position: relative">Generate Student Logins Details</asp:HyperLink>
            </td>          
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td class="ClsBorderlight">
                 <asp:HyperLink ID="hlnkStudentListUI" runat="server" CssClass="SubTitleMenu" EnableViewState="false"
                Style="position: relative" Visible="true" NavigateUrl="#" onclick="OpenStudentList()">Student List</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td class="ClsBorderlight">
                <asp:HyperLink ID="hlnkPublishAll" NavigateUrl="#" runat="server" CssClass="SubTitleMenu"
                    EnableViewState="False" Style="position: relative; cursor: hand">Publish All Exams Result</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            
            <td class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink16" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/RITeSchoolUsageUI.aspx" 
                    Style="position: relative" Visible="true">RITeSchool Usage Details</asp:HyperLink>
            </td>  
            <td align="left">
            </td>  
            <td align="left" class="ClsBorderlight">              
                <asp:HyperLink ID="hlnkBankBlocking" runat="server" CssClass="SubTitleMenu" EnableViewState="false" NavigateUrl="~/RITeSchool/SuperAdmin/DisableOnlineBank.aspx"
                Style="position: relative" Visible="true">Disable Bank For Online Transaction</asp:HyperLink>
            </td>       
        </tr>
         <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
          <tr>
            
            <td class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink17" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/Admin/OnlinePaymentTermsUI.aspx" 
                    Style="position: relative" Visible="true">Add Online Payment Terms</asp:HyperLink>
            </td>  
            <td align="left">
            </td>  
            <td align="left" class="ClsBorderlight">              
                <asp:HyperLink ID="HyperLink18" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/SchoolSettingsUI.aspx" 
                    Style="position: relative" Visible="true">School Settings</asp:HyperLink>
            </td>       
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>            
        </tr>
        <tr>
            <td class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink19" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/UserLoginDetailsUI.aspx" Style="position: relative"
                    >Userwise Login Details</asp:HyperLink>
            </td>  
            <td align="left">
            </td>  
            <td align="left" class="ClsBorderlight">              
                <asp:HyperLink ID="HyperLink21" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/AdmissionProcessDetailsUI.aspx" 
                    Style="position: relative" Visible="true">Admission Process Details</asp:HyperLink>
            </td>       
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>            
        </tr>
          <tr>
             
            <td class="ClsBorderlight">             
                <asp:HyperLink ID="linkReadmit" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/Student/LeftStudentsDetailsUI.aspx" 
                    Style="position: relative" Visible="true">Student Re-Admit</asp:HyperLink>
            </td>       
              <td align="left">
            </td> 
            <td align="left" class="ClsBorderlight">             
                <asp:HyperLink ID="linkRTE" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/MarkRTEStudentsUI.aspx" 
                    Style="position: relative" Visible="true">Mark/Remove Student as RTE </asp:HyperLink>
            </td>       
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>            
        </tr>
        <tr>
            <td class="ClsBorderlight">             
                <asp:HyperLink ID="linkStudentCount" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/AllSchoolStudentsCount.aspx" 
                    Style="position: relative" Visible="true">All Schools Student Count</asp:HyperLink>
            </td> 
            <td>               
            </td>      
            <td class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink20" runat="server" CssClass="SubTitleMenu" 
                    EnableViewState="false" 
                    NavigateUrl="~/RITeSchool/SuperAdmin/MobileAppDetailsIUI.aspx" 
                    Style="position: relative" Visible="true">Mobile App Details</asp:HyperLink>
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>            
        </tr>
        <tr>            
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink10" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/RegNoReassignUI.aspx" Style="position: relative"
                    Visible="false">Regenerate Registration Number</asp:HyperLink>
            </td>
             <td align="left">
            </td> 
           <td align="left" class="ClsBorderlight">
                <asp:LinkButton ID="lnkZipAllPhotoGalleries" runat="server" Visible="false" CssClass="SubTitleMenu"
                    EnableViewState="false" Style="position: relative" OnClick="lnkZipAllPhotoGalleries_Click">Zip All Photo Galleries</asp:LinkButton>
            </td>
        </tr>
    </table>
    <table id="tblManagement" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 70%;
        position: relative" visible="false">
        <tr>
            <td align="center" class="CPanelSpace" style="height: 18px" width="49%">
            </td>
            <td align="center" class="CPanelSpace" style="height: 18px">
            </td>
            <td align="center" class="CPanelSpace" style="height: 18px" width="49%">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink12" runat="server" CssClass="SubTitleMenu" EnableViewState="true"
                    NavigateUrl="~/RITeSchool/Accounts/MISReportUI.aspx" Style="position: relative" Visible="true">MIS Report</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="ClsBorderlight">
            
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
        <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink13" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/Accounts/LedgerSummaryUI.aspx" Style="position: relative" Visible="true">Ledger Summary</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left">                
            </td>
        </tr>
        <tr>
            <td align="left" class="CPanelSpace" colspan="3" style="height:18px">
            </td>
        </tr>
         <tr>
            <td align="left" class="ClsBorderlight">
                <asp:HyperLink ID="HyperLink15" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                    NavigateUrl="~/RITeSchool/SuperAdmin/ManagementFileSharingUI.aspx" Style="position: relative" Visible="true">Management File Sharing</asp:HyperLink>
            </td>
            <td align="left">
            </td>
            <td align="left" class="CPanelSpace">
            </td>            
        </tr>
    </table>
    <br />
    <div id="divMainLateFee" runat="server" class="overlay" style="visibility: hidden;
        display: none;">
    </div>
    <div id="DivPublishAll" runat="server" style="visibility: hidden; display: none;
        position: absolute; margin: 0px; padding: 0px; width: 350px; height: 220px; border-width: 0px;
        left: 0px; top: 0px; line-height: normal; width: auto; border: solid 1px black;
        margin: -100px 0px -100px -20px; background-color: white; filter: progid:DXImageTransform.Microsoft.dropshadow(OffX=5, OffY=5, Color=#7D7E7E);">
        <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
            background-repeat: repeat-x; width: 270px; color: #Black; text-align: Right;">
            <div style="padding: 1px; font-size: 12px; text-align: left; font-weight: bold; color: #Black;
                float: left;">
                Publish All Exams</div>
            <span style="cursor: hand;" onclick="javascript:HidePopup();">
                <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
            </span>
        </div>
        <table width="270px">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Publish all the exams which are previously published. First all exams will be unpublished and then published."
                        Font-Size="9" ForeColor="#000333" />
                </td>
            </tr>
            <tr align="left">
                <td colspan="2">
                    <asp:Label ID="lblReasonForLoss" runat="server" Text="Reason for unpublish the exam :"
                        Font-Size="9" ForeColor="#000333" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:TextBox ID="txtReason" CssClass="SmlCombo" runat="server" Height="80px" Width="95%"
                        TextMode="MultiLine"></asp:TextBox>
                    <span style="color: #ff0000">*</span>
                    <asp:CustomValidator ID="cstvalReason" runat="server" CssClass="ClsMdtStar" Visible="true"
                        ErrorMessage="Reason should not be blank." EnableClientScript="true" Display="None"
                        ClientValidationFunction="validateReason"></asp:CustomValidator>
                    <asp:CustomValidator ID="cstvalReasonLength" runat="server" ErrorMessage="Reason should not exceed than 100 characters."
                        CssClass="ClsMdtStar" Visible="true" EnableClientScript="true" Display="None"
                        ClientValidationFunction="validateReasonLength"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="btnPublishAll" runat="server" Text="Publish All" CssClass="ClsBtn"
                        CausesValidation="true" OnClick="btnPublishAll_Click" />
                    <asp:Button ID="Button1" runat="server" Text="Cancel" CssClass="ClsBtn" CausesValidation="false"
                        OnClientClick="javascript:HidePopup();return false;" />
                </td>
            </tr>
        </table>
                <asp:HiddenField ID="hidQueryString" runat="server"/>
    </div>
    <script type="text/javascript" language="javascript">
        _ClienttxtReason = "<%=this.txtReason.ClientID %>"
        _ClientcstvalReason = "<%=this.cstvalReason.ClientID %>"
        _ClientcstvalReasonLength = "<%=this.cstvalReasonLength.ClientID %>"
        _clientvalsumReturnRenewBook = "<%=this.valsumReturnRenewBook.ClientID %>"
        _clienthidQueryString = "<%=this.hidQueryString.ClientID %>"

        function ShowPopUpWindow(sQryStr) {
            _sClienthlnkDashBoard = "<%=this.hlnkDashBoard.ClientID %>"
            if ((document.getElementById(_sClienthlnkDashBoard) == null) || (document.getElementById(_sClienthlnkDashBoard) == "") || (document.getElementById(_sClienthlnkDashBoard).disabled))
                return false
            window.open(sQryStr, '_blank')
            return false
        }
        function ConfirmReset() {
            var bResult = true
            if (!window.confirm("Are you sure you want to reset timetable of all teachers?")) {
                bResult = false
            }
            return bResult
        }
        function ConfirmZip() {
            var bResult = true
            if (!window.confirm("Are you sure you want to create zip file of all photo galleries?")) {
                bResult = false
            }
            return bResult
        }
        function ShowPopup() {
            var x, y, tt_ovr_
            var cssstyle = $get("<%=this.DivPublishAll.ClientID %>").style
            var now = new Date()
            var width = 250
            var height = 180
            var left = parseInt((screen.width / 2) - (width / 2))
            var top = parseInt((screen.height / 2) - (height / 2))
            cssstyle.left = left + "px"
            cssstyle.top = top + "px"
            cssstyle.zIndex = Math.max((window.dd && dd.z) ? (dd.z + 2) : 0, 1010)
            $get("<%=this.DivPublishAll.ClientID %>").style.visibility = "visible"
            cssstyle.visibility = "visible"
            cssstyle.display = "block"

        }
        function HidePopup() {
            document.getElementById(_clientvalsumReturnRenewBook).innerText = "";
            document.getElementById(_clientvalsumReturnRenewBook).innerHTML = "";
            $get("<%=this.DivPublishAll.ClientID %>").style.visibility = "hidden"
            $get("<%=this.DivPublishAll.ClientID %>").style.display = "none"
            var cssstyleMain = $get("<%=this.DivPublishAll.ClientID %>").style
            cssstyleMain.visibility = "hidden"
            cssstyleMain.display = "none"

            return false
        }
        function validateReason(oSrc, args) {
            var txtReason = (document.getElementById(_ClienttxtReason).value).trim();

            if (txtReason == '') {
                args.IsValid = false
                return true
            }
            args.IsValid = true
            return false
        }
        function validateReasonLength(oSrc, args) {
            var txtReason = (document.getElementById(_ClienttxtReason).value).trim();
            if (txtReason != '') {
                if (txtReason.length > 100) {
                    args.IsValid = false;
                    return true;
                }
            }
            args.IsValid = true
            return false
        }

        function OpenStudentList() {
        window.open("../Teacher/StudentsListUI.aspx?" +$get(_clienthidQueryString).value , '_new','scrollbars=yes,resizable=yes,top=0,left=0,width=1000,height=700');
        }
    </script>
</asp:Content>
