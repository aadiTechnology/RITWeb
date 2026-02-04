<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="../MasterPages/MasterPage.master"
    CodeFile="ControlPanel.aspx.cs" Inherits="ControlPanel" ViewStateMode="Disabled" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>
<%@ Register Src="~/UserControls/NoticeDivUC.ascx" TagName="ucNoticeDivUC" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server"> 
    <link href="../Styles/BootstrapOverride.css?version=1.1" rel="stylesheet" type="text/css" />  
    <link href="../Styles/Dashboard.css?version=1.2.1" rel="stylesheet" type="text/css" />    
    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%">
        <tr>
            <td style="background-color: white" id="MainDataTable" align="center" valign="top">
                <!-- Data Insert Here -->


                   <div id="divComboContainer" runat="server" viewstatemode="Enabled" visible="false">
                            <div id="ace-settings-container" class="ace-settings-container" style="margin-right: 0px; top: 225px; z-index: 10000;">
                                <div id="ace-settings-btn" class="btn btn-app btn-xs btn-warning ace-settings-btn" data-rel="popover" data-trigger="hover" data-placement="left">
                                    <i class="icon- fa fa-cog bigger-150"></i>
                                </div>
                                <div id="ace-settings-box" class="ace-settings-box combo-container-padding">
                                    <table>
                                        <tr id="trAcademicCmb" runat="server" viewstatemode="Enabled" class="col-md-12" style="margin: 0px; padding: 0px;">
                                            <td align="left" style="height: 10px; width: 100px;" runat="server" id="tdAcademicCmblbl"
                                                class=" ClsBorderlight border-0" viewstatemode="Enabled">
                                                <asp:Label ID="Label10" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, AcademicYear%>"
                                                    CssClass="paddingLSML"></asp:Label>
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td align="left" style="height: 18px;" id="tdAcademicCmb" runat="server" viewstatemode="Enabled">
                                                <asp:DropDownList ID="cmbAcademicYearID" runat="server" ViewStateMode="Enabled" CssClass="SmlCombo ClsBorderlight"
                                                    OnSelectedIndexChanged="cmbAcademicYearID_SelectedIndexChanged" AutoPostBack="True">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                    <table>
                                        <tr id="trFinancialYearCombo" runat="server" viewstatemode="Enabled" class="col-md-12" style="margin-top: 3px; padding: 0px;"
                                            visible="false">
                                            <td align="left" style="height: 10px; width: 100px" class="ClsBorderlight  border-0"
                                                id="tdFinancialCmblbl" runat="server">
                                                <asp:Label ID="lblFinancialYear" runat="server" EnableViewState="False" Text="<%$ Resources:LocalizedResources, FinancialYear%>"
                                                    CssClass="paddingLSML" />
                                                <span class="colonPadding">:</span>
                                            </td>
                                            <td id="tdFinancialCmb" align="left" style="height: 18px;" runat="server">
                                                <asp:DropDownList ID="ddlFinancialYears" runat="server" ViewStateMode="Enabled" CssClass="SmlCombo ClsBorderlight"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddlFinancialYears_SelectedIndexChanged" />
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>

                <table border="0" cellpadding="0" cellspacing="2" style="width: 98%;">
                    <tr>
                        <td runat="server" visible="false">
                            <asp:Label ID="lblLastLogin" runat="server" ViewStateMode="Enabled" CssClass="LblGrayMsg" Visible="false"></asp:Label>
                        </td>
                    </tr>
                     <tr>
                        <td align="center">
                            <div id="divClasswiseStudentCount" style="display:none;">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            <table>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label CssClass = "ClsLblLgnd" ID="Label45" runat="server" style="padding-left:10px;" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                    </td>
                                                    <td style="border: thin solid #000000">
                                                        <asp:Label ID="Label46" runat="server" Text="Max. strength exceeded records" style="color:Red;" CssClass="ClsLabel"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <div style="overflow:scroll; height:275px;width:100%;">
                                                <asp:ListView ID="lstvwClasses" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwClasses_ItemDataBound" DataKeyNames="IsExceeded">
                                                    <LayoutTemplate>
                                                        <table align="center" width="95%" runat="server" id="tblStopInfo" style="color: #333333"
                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                <th align="left" class="paddingL">
                                                                    Class
                                                                </th>                                       
                                                                <th align="right" width="120px" style="padding-right:5px;">
                                                                    Student Count
                                                                </th>
                                                                <th align="right" width="120px" style="padding-right:5px;">
                                                                    Max. Strength
                                                                </th>                                        
                                                            </tr>
                                                            <tr runat="server" id="itemPlaceholder" >
                                                            </tr>                                    
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr id="Tr2" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblClassname" runat="server" ViewStateMode="Enabled" Text='<%# Eval("ClassName") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right:5px;">
                                                                <asp:Label ID="lblStudentCount" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentCount") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right:5px;">
                                                                <asp:Label ID="lblStrength" runat="server" ViewStateMode="Enabled" Text='<%# Eval("MaxStrength") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                    <AlternatingItemTemplate>
                                                        <tr id="Tr3" runat="server" viewstatemode="Enabled" class="ClsGridAltRow">
                                                            <td align="left" class="paddingL">
                                                                <asp:Label ID="lblClassname" runat="server" ViewStateMode="Enabled"  Text='<%# Eval("ClassName") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right:5px;">
                                                                <asp:Label ID="lblStudentCount" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentCount") %>'></asp:Label>
                                                            </td>
                                                            <td align="right" style="padding-right:5px;">
                                                                <asp:Label ID="lblStrength" runat="server" ViewStateMode="Enabled" Text='<%# Eval("MaxStrength") %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </AlternatingItemTemplate>
                                                </asp:ListView>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Button ID="btnCloseClassDiv" runat="server" ViewStateMode="Enabled" Text="Close" CssClass="ClsBtn" CausesValidation="false" OnClientClick="CloseClassDiv()" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" width="100%">
                            <div class="col-md-12 col-sm-12 col-sm-12">
                            <!-- Table for admin starts here -->
                            <table id="tblAdmin" runat="server" viewstatemode="Enabled" border="0" cellpadding="0" cellspacing="1" style="display:none;"  visible="false">
                                <tr>
                                    <td align="left" colspan="4" class="DashboardMenuHead">
                                        <asp:Label ID="Label12" runat="server" Text="<%$ Resources:LocalizedResources, StudentRelated%>"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink7" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Teacher/SchoolwiseAttendanceDetails.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, Attendance%>"></asp:HyperLink>
                                    </td>
                                    <td align="left" style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink85" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Admin/ChangeStudentDivision.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentTransfer%>"></asp:HyperLink>
                                    </td>
                                    <td align="left" style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink19" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Teacher/LeavingCertificateUI.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, LeavingCertificate%>"></asp:HyperLink>
                                    </td>
                                    <td align="left" style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink60" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Teacher/LeavingCertificateConfigUI.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, LCReportConfiguration%>"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink115" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Admin/StudentDetailsUI.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentDetails%>"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" id="trAdminMissingAttendance" runat="server" viewstatemode="Enabled" 
                                        visible="false">
                                        <a class="SubTitleMenuAdmin" onclick="ShowAttendanceAlertPopup()" style="cursor: pointer;">
                                            <asp:Label ID="lblMissingAttendance" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MissingAttendance%>"></asp:Label></a>
                                    </td>
                                     <td align="left" style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink129" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Student/DynamicFieldDetailsUI.aspx"
                                            EnableViewState="False" Text="Dynamic Export"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink136" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/StudentCountDetailsUI.aspx"
                                                        EnableViewState="False" Text="Student Count Details"></asp:HyperLink>
                                    </td>
                                    </tr>
                                    <tr>
                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HlinkHouseAssignment" runat="server" CssClass="SubTitleMenuAdmin"  NavigateUrl="~/RITeSchool/Admin/StudentsHouseAssignmentUI.aspx"
                                                        EnableViewState="False" Text="House Assignment" Visible="true" ></asp:HyperLink>
                                    </td>
                                     <td id="trMissingAttendance" runat ="server" style="width: 25%; padding-left: 25px" viewstatemode="Enabled" 
                                        visible="true">
                                        <a class="SubTitleMenuAdmin" onclick="ShowAbsentStudentPopup()" style="cursor: pointer;">
                                            <asp:Label ID="lblAbsentStudents" runat="server" ViewStateMode="Enabled" Text="Absent Student Details"></asp:Label></a>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HLinkCancellationForm" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/CancellationFormUI.aspx"
                                         EnableViewState="false" Text="Cancellation Form Details" Visible="true"></asp:HyperLink>
                                    </td>   
                                    <td style="width: 25%; padding-left: 25px" viewstatemode="Enabled" 
                                        visible="true">
                                        <asp:HyperLink ID="hlnkBlackListedStudents" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/BlackListedStudentUI.aspx"
                                         EnableViewState="false" Text="Blacklisted Students" Visible="true"></asp:HyperLink>
                                    </td>                                                                    
                                </tr>
                                <tr>
                                    <td style="width: 25%; padding-left: 25px" viewstatemode="Enabled" 
                                        visible="true">
                                        <asp:HyperLink ID="HyperLink174" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Teacher/StudentsMonthlyStatusDetailsUI.aspx"
                                         EnableViewState="false" Text="Student Monthly Status" Visible="true"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" viewstatemode="Enabled" visible="true">
                                        <asp:HyperLink ID="HyperLink176" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Teacher/StudentDocumentUI.aspx"
                                         EnableViewState="false" Text="Upload Student Documents" Visible="true"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" viewstatemode="Enabled" visible="true">
                                        <asp:HyperLink ID="HyperLink177" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/teacher/StudentListForNoteDetailsUI.aspx"
                                         EnableViewState="false" Text="Student List for Activity Details" Visible="true"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" viewstatemode="Enabled" visible="true">
                                        <asp:HyperLink ID="HyperLink178" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/admin/UpdateStudentDetailsInBulkUI.aspx"
                                         EnableViewState="false" Text="Update Student Additional Details" Visible="true"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr id="trAcrossBranch" runat="server">
                                    <td colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="6" class="DashboardMenuHead">
                                                    <asp:Label ID="Label65" runat="server" Text="Across Branch"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink146" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/TransferStudentAcrossBranchUI.aspx"
                                                        EnableViewState="False" Text="Transfer Students to Another Branch"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                                <asp:HyperLink ID="HyperLink147" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/TransferredStudentDetailsUI.aspx"
                                                                    EnableViewState="False" Text="Transferd Students to this Branch"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr id="trFeeRelated">
                                    <td colspan="4">
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="6" class="DashboardMenuHead">
                                                    <asp:Label ID="Label14" runat="server" Text="<%$ Resources:LocalizedResources, FeeRelated%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="6" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink48" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Accountant/StudentPayFeeUI.aspx" Text="<%$ Resources:LocalizedResources, Fees%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink39" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Accountant/DebitEntryUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources, StudentPayables%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink5" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Accountant/ClearanceList.aspx"
                                                        Text="<%$ Resources:LocalizedResources,PaymentClearance%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkCautionMoney" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="/RITeSchool/Accountant/StudentCautionMoney.aspx" Visible="true"
                                                        Text="<%$ Resources:LocalizedResources, CautionMoneyDetails%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink26" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Accountant/PayInternalFeesUI.aspx" Text="<%$ Resources:LocalizedResources, InternalFees%>"></asp:HyperLink>
                                                </td>
                                                <td id="tdOnlineAdmission" style="width: 25%; padding-left: 25px" runat="server" viewstatemode="Enabled" >
                                                    <asp:HyperLink ID="HyperLink67" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Accountant/OnlineAdmissionFeeClearanceListUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources,OnlineAdmissionFeeClearance%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                  <asp:HyperLink ID="hlnTran" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                            NavigateUrl="/RITeSchool/SuperAdmin/InCompletedTransactionUI.aspx" Style="position: relative"
                                                                Visible="true">Incomplete Transaction</asp:HyperLink>
                                                </td>                                               
                                                <td align="left" style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink123" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Teacher/StudentSearchUI.aspx"
                                                        EnableViewState="False" Text="Student/Payment Search/Export"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="tr17" runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink164" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Accountant/ManageStudentWalletUI.aspx" Text="Manage Wallets"></asp:HyperLink>
                                                </td>
                                                
                                            </tr>
                                            <tr id="trExternalStudentsFeeDetails" runat="server" visible="false">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnExternalStudentInternalFee" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Accountant/ExternalStudentFeeUI.aspx" Text="Internal Fees For External Students"></asp:HyperLink>
                                                </td>
                                                
                                            </tr>
                                            
                                            <%--<tr id="trPaymentNotificationClearance" runat="server" visible="false"> 
                                              <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlkPaymentNotification" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="#" Text="Payment Clearance Notification"></asp:HyperLink>
                                                </td> 
                                            </tr>  --%>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" colspan="4" class="DashboardMenuHead">
                                        <asp:Label ID="lblExamHead" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ExaminationManagement%>"> </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="/RITeSchool/Teacher/TestMarksConfigurationUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, AssignExamMarks%>"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ExamResults%>"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink35" runat="server" NavigateUrl="/RITeSchool/Student/StudentProgressSheet.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, ProgressReport%>"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink24" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Teacher/StudentResultList.aspx"
                                            EnableViewState="False" Text="<%$ Resources:LocalizedResources, FinalResult%>"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink92" runat="server" NavigateUrl="/RITeSchool/ProgressReport/StudentwiseProgreesReportUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentWiseProgressReport%>"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" runat="server">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <asp:HyperLink ID="HyperLink78" runat="server" NavigateUrl="/RITeSchool/ProgressReport/BlockProgressReportUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, BlockProgressReport%>"></asp:HyperLink>
                                        <%} %>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" id="tdAssignGrades" runat="server" viewstatemode="Enabled" visible ="false">
                                        <asp:HyperLink ID="HyperLink126" runat="server" NavigateUrl="/RITeSchool/Teacher/AssignGradesUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Assign Grades"></asp:HyperLink>
                                    </td>
                                    <td id="tdDescriptiveIndecators" visible="false" style="width: 25%; padding-left: 25px" runat="server">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <asp:HyperLink ID="HyperLink138" runat="server" NavigateUrl="/RITeSchool/DescriptiveIndicators/DescriptiveIndicatorsUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Assign Descriptive Indicators"></asp:HyperLink>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink153" runat="server" NavigateUrl="/RITeSchool/admin/StudentMarksExportUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Export Student Marks"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink172" runat="server" NavigateUrl="/RITeSchool/Admin/ConfigurePeerDetailsUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Configure Peer Details"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink170" runat="server" NavigateUrl="/RITeSchool/Student/StudentListForAssessmentDetailsUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Student List For Self Assessment"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink173" runat="server" NavigateUrl="/RITeSchool/Teacher/ResultDetailsUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Result Details"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr id="trXseed" runat="server" viewstatemode="Enabled" >
                                    <td align="left" colspan="4" class="DashboardMenuHead">
                                        <asp:Label ID="lblXseed" runat="server" Text="Xseed"></asp:Label>
                                    </td>
                                </tr>
                                <tr id="trXseedEmptyRow" runat="server" viewstatemode="Enabled" >
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr id="trXseedScreens" runat="server" viewstatemode="Enabled" >
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="hlkAssignXseedGrades" runat="server" NavigateUrl="/RITeSchool/Xseed/AssignXseedGradesUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text=" AssignXseedGrades"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="hlkXseedResult" runat="server" NavigateUrl="/RITeSchool/Xseed/ClassTeacherXseedGradesUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text=" XseedResult"></asp:HyperLink>
                                    </td>
                                    <td style="width: 25%; padding-left: 25px" colspan="2">
                                        <asp:HyperLink ID="hlkXseedProgressReport" runat="server" NavigateUrl="/RITeSchool/Xseed/XseedProgressReportUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="XseedProgressReport"></asp:HyperLink>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                 <tr id="trOnlineExam" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr id="tr21" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label66" runat="server" Text="Online Exam Related"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr22" runat="server">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink148" runat="server" NavigateUrl="/RITeSchool/OnlineExam/PublishOnlineExamUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Online Exam Result"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%;">
                                                <asp:HyperLink ID="HyperLink149" runat="server" NavigateUrl="/RITeSchool/OnlineExam/OnlineExamProgressReportUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Online Exam Progress Report"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                                 <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>

                                <tr id="trPerformanceRelated" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr id="tr7" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label42" runat="server" Text="<%$ Resources:LocalizedResources, StaffPerformance%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr8" runat="server">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink112" runat="server" NavigateUrl="/RITeSchool/StaffPerformance/PerformanceGradeAssignmentUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, PerformanceGradeAssignment%>"></asp:HyperLink>
                                                </td>
                                                <%--<td style="width: 25%; padding-left: 25px">
                                        <asp:HyperLink ID="HyperLink111" runat="server" NavigateUrl="/RITeSchool/StaffPerformance/PerformanceGradeAssignmentUI.aspx"
                                            CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Performance Evaluation"></asp:HyperLink>
                                    </td>--%>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trSurveyModuleOfJPS" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr id="tr14" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label49" runat="server" Text="<%$ Resources:LocalizedResources, Survey%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr15" runat="server">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink128" runat="server" NavigateUrl="/RITeSchool/Survey/SurveyFormDetailsUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, StudentRegistrationDetails%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trSurveyModuleForAdmin" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr id="tr19" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label51" runat="server" Text="<%$ Resources:LocalizedResources, Survey%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr20" runat="server">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink133" runat="server" NavigateUrl="/RITeSchool/Survey/SurveyUserListUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Survey User List"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                   
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trLessonPlanRelated" runat="server" viewstatemode="Enabled" visible="false">
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr id="tr11" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label47" runat="server" Text="Lesson Plan"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="tr12" runat="server">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink121" runat="server" NavigateUrl="~/RITeSchool/LessonPlan/LessonPlanUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="Lesson Plan"></asp:HyperLink>
                                                </td>                                                
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" colspan="2">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trTimetableRelated">
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="6" class="DashboardMenuHead">
                                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:LocalizedResources, TimetableScheduling%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="6" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 25px; width: 25%;">
                                                    <asp:HyperLink ID="HyperLink23" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/TeacherTimeTable.aspx"
                                                        Text="<%$ Resources:LocalizedResources, WeeklyTimetable%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink59" runat="server" NavigateUrl="~/RITeSchool/Admin/SchoolTimeTable.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolTimeTable%>"></asp:HyperLink>
                                                    <asp:HyperLink ID="HyperLink58" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/WeekDayTimeTable.aspx"
                                                        Visible="False" Text="<%$ Resources:LocalizedResources, DailyTimetableForTeachers%>"></asp:HyperLink>
                                                </td>
                                                <td align="left" style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink63" runat="server" Visible="false" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="~/RITeSchool/Admin/TimeTableGenerationUI.aspx" Text="<%$ Resources:LocalizedResources, AutoTimetableGeneration%>"></asp:HyperLink>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trTimetableRelatedMini">
                                    <td colspan="4">
                                        <%if (Settings.IsMiniSite) %>
                                        <% {%>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="6" class="DashboardMenuHead">
                                                    <asp:Label ID="Label35" runat="server" Text="<%$ Resources:LocalizedResources, TimetableScheduling%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="6" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink102" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/TeacherTimeTable.aspx"
                                                        Text="<%$ Resources:LocalizedResources, WeeklyTimetable%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink103" runat="server" NavigateUrl="~/RITeSchool/Admin/SchoolTimeTable.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolTimeTable%>"></asp:HyperLink>
                                                    <asp:HyperLink ID="HyperLink104" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/WeekDayTimeTable.aspx"
                                                        Visible="False" Text="<%$ Resources:LocalizedResources, DailyTimetableForTeachers%>"></asp:HyperLink>
                                                </td>
                                                <td align="left" style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink105" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/SuperAdmin/ResetTimeTableUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources, ResetTimeTable%>"></asp:HyperLink>
                                                </td>
                                                <td align="left" style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink106" runat="server" Visible="false" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="~/RITeSchool/Admin/TimeTableGenerationUI.aspx" Text="<%$ Resources:LocalizedResources, AutoTimetableGeneration%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trHomework" runat="server" viewstatemode="Enabled" visible="false">
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label32" runat="server" Text="<%$ Resources:LocalizedResources,Homework%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink81" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Homework/HomeworkUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, AssignHomework%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server" viewstatemode="Enabled" id="trLibraryModule" visible="false">
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label19" runat="server" Text="<%$ Resources:LocalizedResources, Library%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkLibraryManagement" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenuAdmin"
                                                        EnableViewState="False" NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryManagementUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources, ManageIssueBooks%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkReturnRenew" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/LibrarianManagement/ReturnRenewUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources, ReturnRenewBooks%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkIssueRenewReturn" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/IssueRenewReturnUI.aspx" Text="<%$ Resources:LocalizedResources, LibrarianDesk%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink139" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryRecordsUI.aspx" Text="Library Records"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="plcHolderInventoy" runat="server" viewstatemode="Enabled" visible="false">
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead" style="width: 2%">
                                                    <asp:Label ID="Label20" runat="server" Text="<%$ Resources:LocalizedResources, Inventory%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" visible="false">
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink30" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/ItemManagementUI.aspx" Text="<%$ Resources:LocalizedResources, ItemsManagement%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink31" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/RequisitionListUI.aspx" Text="<%$ Resources:LocalizedResources, Requisition%>"></asp:HyperLink>
                                                        <span visible="false" class="clsCount" runat="server" viewstatemode="Enabled" id="spnRequisitionCountForAdmin" title=""></span>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink32" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/PurchaseOrderListUI.aspx" Text="<%$ Resources:LocalizedResources, PurchaseOrder%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkGRN" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/GRNListUI.aspx" Text="<%$ Resources:LocalizedResources, GRN%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink159" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/GSTInvoiceDetailsUI.aspx" Text="GST Invoice Details"></asp:HyperLink>
                                                </td>                                            
                                               <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLinkPO" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="false"
                                                     NavigateUrl="~/RITeSchool/Inventory/PODetailsUI.aspx" Text="External PO Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trPayroll" runat="server" visible="false" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead" style="width: 2%">
                                                    <asp:Label ID="Label26" runat="server" Text="<%$ Resources:LocalizedResources,Payroll%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink44" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/SalaryDetailsUI.aspx" Text="<%$ Resources:LocalizedResources,SalaryDetails%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink50" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/PaySalaryUI.aspx" Text="<%$ Resources:LocalizedResources,PaySalary%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink64" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/SalaryDifferenceUI.aspx" Text="<%$ Resources:LocalizedResources,SalaryDifference%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink114" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/AdditionalPaymentsUI.aspx" Text="<%$ Resources:LocalizedResources,AdditionalPayments%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink76" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/InvestmentDeclarationUI.aspx" Text="<%$ Resources:LocalizedResources,InvestmentIncomeDeclaration%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink98" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/TaxDeductionUI.aspx" Text="<%$ Resources:LocalizedResources,TaxDeduction%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink77" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/IncomeTaxDetailsUI.aspx" Text="<%$ Resources:LocalizedResources,IncomeTaxDetails%>"> </asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
													<asp:HyperLink ID="HyperLink117" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/UserAppointmentDetailsUI.aspx" Text="<%$ Resources:LocalizedResources,EmployeeAppointmentDetails%>"> </asp:HyperLink>
                                                </td>
                                            </tr>                                            
                                            <tr id="Tr10" runat="server">
                                               <%-- <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink124" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/InvestmentDetailsUI.aspx" Text="Investment Declaration"></asp:HyperLink>
                                                </td>--%>
                                               
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink140" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/SalaryChangesUI.aspx" Text="Salary Increment Details"></asp:HyperLink>
                                                </td> 
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink83" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Payroll/LeaveDeatilsUI.aspx" Text="Leave Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trAccounts" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <table runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead" style="width: 2%">
                                                    <asp:Label ID="lblAccount" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Accounts%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink90" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Accounts/VoucherListUI.aspx" Text="<%$ Resources:LocalizedResources, Vouchers%>"></asp:HyperLink>&nbsp;&nbsp;
                                                    <img src="../images/document_pending.gif" alt="New Voucher(s)" title="New Voucher(s) for Approval"
                                                        id="imgNewVoucherAdmin" runat="server" viewstatemode="Enabled" visible="false" />
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink91" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Accounts/DayBook.aspx" Text="<%$ Resources:LocalizedResources, DayBook%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 20%; padding-left: 25px">
                                                    <a class="SubTitleMenuAdmin" href="../Accounts/MISReportUI.aspx">
                                                        <asp:Label ID="lblMISReport" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MISReport%>"></asp:Label></a>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <a class="SubTitleMenuAdmin" href="../Accounts/LedgerSummaryUI.aspx">
                                                        <asp:Label ID="lblledgerSummery" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,LedgerSummary%>"></asp:Label></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <a class="SubTitleMenuAdmin" href="../Accounts/TrialBalanceReportUI.aspx">
                                                        <asp:Label ID="lblTrialBalance" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,TrialBalanceReport%>"></asp:Label></a>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <a class="SubTitleMenuAdmin" href="../Accounts/ExportVoucherDetailsUI.aspx">
                                                        <asp:Label ID="Label72" runat="server" ViewStateMode="Enabled" Text="Export Voucher Details"></asp:Label></a>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr id="treStore" runat="server" viewstatemode="Enabled" visible="false">
                                    <td colspan="4">
                                        <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead" style="width: 2%">
                                                    <asp:Label ID="Label71" runat="server" ViewStateMode="Enabled" Text="Store"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>                                               
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink165" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/eStore/StoreItemListUI.aspx" Text="Item Management"></asp:HyperLink>
                                                </td>                                                
                                            </tr>                                            
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>

                                <tr id="trTransport" runat="server" viewstatemode="Enabled" visible="false">
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead" style="width: 2%">
                                                    <asp:Label ID="Label27" runat="server" Text="<%$ Resources:LocalizedResources,Transport%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink52" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/TravelerTransportDetailsUI.aspx" Text="<%$ Resources:LocalizedResources,SchoolTransport%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink65" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/PrivateTransportDetailsUI.aspx" Text="<%$ Resources:LocalizedResources,PrivateTransport%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                  <asp:HyperLink ID="HyperLink116" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/TransportChargesUI.aspx" Text="Transport Charges"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="lnkTransportCommittee" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" NavigateUrl="#" Text="<%$ Resources:LocalizedResources,TransportCommittee%>" ></asp:HyperLink>
                                                </td>
                                            </tr>
                                             <tr>
                                                 <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink130" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/TransportReadingAllocationUI.aspx" Text="<%$Resources:LocalizedResources,TransportReading%>"></asp:HyperLink>
                                                </td>
                                                 <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink131" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/VehicleMaintenanceExpensesUI.aspx" Text="<%$Resources:LocalizedResources,VehicleMaintenanceExpenses%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink155" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/VehicleServicingDetailsUI.aspx" Text="Vehical Servicing Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink156" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/VehiclePassingDetailsUI.aspx" Text="Vehicle Passing Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink157" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/VehiclePUCDetailsUI.aspx" Text="Vehicle PUC Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink158" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/VehicleBillingUI.aspx" Text="Vehicle Billing Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLinkUserAttendance" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="false"
                                                         NavigateUrl="~/RITeSchool/Transport/UserAttendanceInBusDetailsUI.aspx" Text="User Transport Attendance"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink161" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="false"
                                                         NavigateUrl="~/RITeSchool/Transport/TransportCapacityDetailsUI.aspx" Text="Vehicle Capacity Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink162" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/TransportNotificationDetailsUI.aspx" Text="Transport Notification Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink163" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/TransportOverrideDetailsUI.aspx" Text="Transport Override Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink17" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/RFIDDetailsUI.aspx" Text="RFID Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink166" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Transport/BulkDocumentUploadDetailsUI.aspx" Text="Vehicle Document Uploads"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperlinkAllocation" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="false"
                                                     NavigateUrl="~/RITeSchool/Transport/ImportTransportAllocationUI.aspx" Text="Import Transport Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trCommunication" runat="server" >
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr id="Tr4" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label17" runat="server" Text="<%$ Resources:LocalizedResources,Communication%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr id="Tr5" runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink22" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Common/SMSUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,SMSCenter%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="lnkMessageInbox" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Common/MessageInbox.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,MessageCenter%>"></asp:HyperLink>
                                                    <asp:ImageButton ID="imgBtnMessageAlert" runat="server" ViewStateMode="Enabled" ImageUrl="~/RITeSchool/images/NewMail_Blink.gif"
                                                        Visible="false" />
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink75" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Admin/SmsTemplateUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,CreateSMSTemplate%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                <tr id="trStudentRecord" runat="server" >
                                    <td colspan="4">
                                         <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr id="Tr1" runat="server">
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label43" runat="server" Text="Student Records"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr id="Tr13" runat="server">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink124" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="/RITeSchool/Student/StudentRecordStatusUI.aspx"
                                                        EnableViewState="False" Text="Student Record Status"></asp:HyperLink>
                                                </td>
                                                <td colspan="3">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                 <tr>
                                    <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                    </td>
                                </tr>
                                 <tr id="trAssembly" runat="server" visible="false" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="lblAssembly" runat="server" ViewStateMode="Enabled" Text="Assembly Details"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="lnkAssembly" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/AssemblyListUI.aspx"
                                                        EnableViewState="False" Text="Assembly List"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trTaskManagement" runat="server" visible="false" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%;">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label29" runat="server" Text="<%$ Resources:LocalizedResources,TaskAssignment%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink87" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/TaskManagement/TaskListUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,TaskAssignment%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>                               
                                 <tr id="trParentTeacherAssociation" runat="server" viewstatemode="Enabled" >
                                    <td colspan="4">
                                        <% if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label50" runat="server" Text="<%$ Resources:LocalizedResources, ParentTeacherAssociation%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td style="width: 25%; padding-left: 25px">                                                    
                                                    <asp:HyperLink ID="lnkAdminPTA" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="#"
                                                        EnableViewState="False" Visible="true" 
                                                        Text="<%$ Resources:LocalizedResources, ParentTeacherAssociation%>"></asp:HyperLink>
                                                </td>
                                                <td id="Td14" style="width: 25%; padding-left: 25px" runat="server" >
                                                </td>
                                                <td id="Td15" style="width: 25%; padding-left: 25px" runat="server" >
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trOtherUtilities" runat="server" >
                                    <td colspan="4">
                                        <% if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label18" runat="server" Text="<%$ Resources:LocalizedResources,OtherUtilities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Td4" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink49" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/AnnualEventPlanner.aspx" Text="<%$ Resources:LocalizedResources,AnnualPlanner%>"></asp:HyperLink>
                                                </td>
                                                <td id="Td5" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink12" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/NoticeBoardUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,NoticeBoard%>"></asp:HyperLink>
                                                </td>
                                                <td id="Td6" style="width: 25%; padding-left: 25px" runat="server" viewstatemode="Enabled" >
                                                    <asp:HyperLink ID="HyperLink4" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admin/UploadPhotoUI.aspx" Text="<%$ Resources:LocalizedResources,PhotoVideoGallery%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink84" runat="server" NavigateUrl="/RITeSchool/Common/StaffBirthDay.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, StaffBirthdays%>">&nbsp;&nbsp;</asp:HyperLink>
                                                    <asp:ImageButton ID="imgBtnStaffBirthdayAlert" Style="margin-left: 2px;" runat="server" ViewStateMode="Enabled" 
                                                        ImageUrl="~/RITeSchool/images/animated_gift_box3.gif" Visible="false"/>
                                                </td>
                                            </tr>
                                            <tr id="Tr6" runat="server">
                                               
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink94" runat="server" NavigateUrl="/RITeSchool/Common/FeedbackDetailsUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, FeedbackDetails%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink95" runat="server" NavigateUrl="~/RITeSchool/Admin/UploadNoticesUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, SchoolNotices%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink111" runat="server" NavigateUrl="~/RITeSchool/Support/SupportDetailsUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, SupportDetails%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="Tr9" runat="server">
                                                <td style="width: 25%; padding-left: 25px" id="tdAskMeAdmin" runat="server" visible="false">
                                                    <asp:HyperLink ID="HyperLink120" runat="server" NavigateUrl="~/RITeSchool/AskMe/PublishedQueriesUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="Ask Me"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink122" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admin/PANAttachmentUI.aspx" Text="<%$ Resources:LocalizedResources,PANAttachment%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlnkTeacherDetails" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="#" Text="Non Permanent Teachers list"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlkAlumniDetails" Visible = "false" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admin/AlumniStudentsUI.aspx" Text="Alumni Student Details"></asp:HyperLink>
                                                </td>                                      
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink160" runat="server" CssClass="SubTitleMenuAdmin"
                                                        NavigateUrl="~/RITeSchool/Admin/PhotoUpdationUtilityUI.aspx" Text="Update Photos"></asp:HyperLink>
                                                </td>   
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trGuestManagement" runat="server" >
                                    <td colspan="4">
                                        <% if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label55" runat="server" Text="Guest Management"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr id="Tr16" runat="server">                                                
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="hlinkGuestManagement" Visible = "true" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admin/GuestManagementUI.aspx" Text="Guest Management"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                 <tr id="trStudentHealthDetails" runat="server" visible="false">
                                    <td colspan="4">
                                        <% if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label61" runat="server" Text="Health Details"> </asp:Label>
                                                </td>
                                            </tr>                                            
                                            <tr id="Tr18" runat="server">                                                
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink141" Visible = "true" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/HealthDetails/HealthDetailsStudentListUI.aspx" Text="Student Health Details"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink142" Visible = "true" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/HealthDetails/ImportStudentHealthDetailsUI.aspx" Text="Import Student Health Details"></asp:HyperLink>
                                                </td>
                                                <td>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trOtherUtilitiesMini" runat="server" >
                                    <td colspan="4">
                                        <% if (Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label34" runat="server" Text="<%$ Resources:LocalizedResources, OtherUtilities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td id="Td10" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink101" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/AnnualEventPlanner.aspx" Text="<%$ Resources:LocalizedResources, AnnualPlanner%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                  <tr id="trExternalActivities" runat="server">
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label52" runat="server" Text="<%$ Resources:LocalizedResources, ExternalActivities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                 <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink118" runat="server" NavigateUrl="~/RITeSchool/Admin/UploadNewsUI.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="School News"></asp:HyperLink>
                                                </td>
                                                 <td id="Td21" style=" width: 25%;padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink134" runat="server" CssClass="SubTitleMenuAdmin" Visible="false" NavigateUrl="~/RITeSchool/Admin/AddCareerOpenings.aspx"
                                                        EnableViewState="False" Text="Add Career Openings"></asp:HyperLink>
                                                </td>
                                                   <td id="Td13" style="width: 25%;padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="lnkAchievement" runat="server" CssClass="SubTitleMenuAdmin" Visible="false" NavigateUrl="~/RITeSchool/Admin/AchievementDetailsUI.aspx"
                                                        EnableViewState="False" Text="Add Achievements"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trExternalActivities1" runat="server" >
                                    <td colspan="3">
                                        <%if (Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label53" runat="server" Text="<%$ Resources:LocalizedResources,ExternalActivities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                              <td style="width: 25%; padding-left: 25px">
                                                  <asp:HyperLink ID="HyperLink135" runat="server" NavigateUrl="~/RITeSchool/Admin/UploadNewsUI.aspx"
                                                CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true" Text="School News"></asp:HyperLink>
                                                </td>
                                                 <td id="Td24" style="width: 25%;padding-left: 25px" runat="server">
                                                   <asp:HyperLink ID="lnkCareer" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/AddCareerOpenings.aspx"
                                                     EnableViewState="False" Text="Add Career Openings"></asp:HyperLink>
                                                </td>
                                                 <td id="Td16" style=" width: 25%;padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink137" runat="server" CssClass="SubTitleMenuAdmin"  NavigateUrl="~/RITeSchool/Admin/AchievementDetailsUI.aspx"
                                                        EnableViewState="False" Text="Add Achievement"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trAdminActivity" runat="server">
                                    <td colspan="4">
                                        <%if (!Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label21" runat="server" Text="<%$ Resources:LocalizedResources, AdminActivities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink20" runat="server" NavigateUrl="~/RITeSchool/Admin/schoolconfigurationcontrolpanel.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources, SchoolConfiguration%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink21" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/AdminProfileUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, EditProfile%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" id="tdAdminReports" runat="server">
                                                    <asp:HyperLink ID="hlnkEditSchool" runat="server" NavigateUrl="~/RITeSchool/SuperAdmin/RegistrationWizard_Step1.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources,SchoolInformation%>"></asp:HyperLink>
                                                </td>
                                                <td id="Td7" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink41" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/LockingUser.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,UserManagement%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px" id="tdAdminPhoto" runat="server" visible="false">
                                                    <asp:HyperLink ID="HyperLink144" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Admin/UploadPhotosUI.aspx" Text="Upload Photo"></asp:HyperLink>
                                                </td>
                                                <td id="Td8" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink86" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admission/NewStudentAdmisionsListUI.aspx" Text="<%$ Resources:LocalizedResources,NewAdmissions%>"></asp:HyperLink>
                                                    <%--<asp:Label ID="spnCount" visible="false" class="clsCount" runat="server" Text="<%$ Resources:LocalizedResources, AdmissionCount%>"></asp:Label>--%>
                                                    <span visible="false" class="clsCount" runat="server" viewstatemode="Enabled" id="spnCount" title=""></span>
                                                </td>
                                                <td id="Td9" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink33" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Admission/AdmisionLotteryUI.aspx" Text="<%$ Resources:LocalizedResources,AdmissionLottery%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink51" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/UserRolewisePhotoUploadUI.aspx" Text="<%$ Resources:LocalizedResources,UserPhotoUpload%>"></asp:HyperLink>
                                                </td>
                                            </tr>                                                
                                            <tr id="trUserLogin" runat="server" visible = "false">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink152" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False" Visible="true"
                                                        NavigateUrl="/RITeSchool/SuperAdmin/UserLoginDetailsUI.aspx" Text="Userwise Login Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink57" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/SchoolReportUI.aspx" Text="<%$ Resources:LocalizedResources,Reports%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trLcUpload" runat="server" visible="false">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink145" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/StudentLCUploadUI.aspx" Text="Student LC Upload"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trUserDocument" runat="server" visible="false">
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink66" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/UploadUserDocumentsUI.aspx" Text="Upload User Documents"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink80" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/DocumentDetailsUI.aspx" Text="User Document Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                                <tr id="trAdminActivityMini" runat="server" >
                                    <td colspan="4">
                                        <%if (Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" colspan="4" class="DashboardMenuHead">
                                                    <asp:Label ID="Label36" runat="server" Text="<%$ Resources:LocalizedResources,AdminActivities%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink100" runat="server" NavigateUrl="~/RITeSchool/Admin/schoolconfigurationcontrolpanel.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources,SchoolConfiguration%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink107" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/AdminProfileUI.aspx"
                                                        EnableViewState="False" Text="<%$Resources:LocalizedResources,EditProfile%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px" id="td11" runat="server" >
                                                    <asp:HyperLink ID="HyperLink108" runat="server" NavigateUrl="~/RITeSchool/SuperAdmin/RegistrationWizard_Step1.aspx"
                                                        CssClass="SubTitleMenuAdmin" EnableViewState="False" Text="<%$ Resources:LocalizedResources,SchoolInformation%>"></asp:HyperLink>
                                                </td>
                                                <td id="Td12" style="width: 25%; padding-left: 25px" runat="server">
                                                    <asp:HyperLink ID="HyperLink109" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/Admin/LockingUser.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,UserManagement%>"></asp:HyperLink>
                                                </td>
                                                 
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink110" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/SchoolReportUI.aspx" Text="<%$ Resources:LocalizedResources,Reports%>"></asp:HyperLink>
                                                </td>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink113" runat="server" CssClass="SubTitleMenuAdmin" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/UserRolewisePhotoUploadUI.aspx" Text="<%$ Resources:LocalizedResources,UserPhotoUpload%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                             
                                <tr>
                                    <td colspan="4">
                                        <% if (Settings.IsMiniSite) %>
                                        <%{ %>
                                        <table width="100%" id="tblAcademicYearMenu" runat="server">
                                            <tr>
                                                <td align="left" class="DashboardMenuHead" colspan="4">
                                                    <asp:Label ID="Label33" runat="server" Text="<%$ Resources:LocalizedResources,AcademicYearRelated%>"> </asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink97" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/SuperAdmin/StartNextAcademic.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,NextAcademicYearGeneration%>"></asp:HyperLink>
                                                </td>
                                                <td align="left" style="width: 25%; padding-left: 25px">
                                                    <asp:HyperLink ID="HyperLink99" runat="server" CssClass="SubTitleMenuAdmin" NavigateUrl="~/RITeSchool/SuperAdmin/SchoolwiseAcademicYearUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,SchoolWiseAcdemicYears%>"></asp:HyperLink>
                                                </td>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="CPanelSpace" colspan="4" style="height: 2px">
                                                </td>
                                            </tr>
                                        </table>
                                        <%} %>
                                    </td>
                                </tr>
                            </table>
                            </div>
                            <table id="tblStudents" runat="server" viewstatemode="Enabled" border="0" cellpadding="0" cellspacing="1" visible="false" width="100%">
                                <tr class="row">
                                    <td id="tdSidebar" align="center" class="col-lg-2 col-md-3 col-sm-3 col-xs-3 CPanelSpace hide" valign="top">
                                        <table id="tblStudentMenu" runat="server" viewstatemode="Enabled" border="0" cellpadding="2" cellspacing="5" class="table tbl-sub-menu"
                                            style="width: 100%;">
                                            <tr id="trAddStudentDetails" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight" runat = "server" id = "tdAddStudentDetails" visible = "false">
                                                    <asp:HyperLink ID="lnkAddStudentDetails" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/AddStudentDetails.aspx"
                                                         Text="Update Profile"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAnnualPlanner" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink53" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/AnnualEventPlanner.aspx" Text="<%$ Resources:LocalizedResources,AnnualPlanner%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAskMeStudent" runat="server" visible="false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink72" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/AskMe/PublishedQueriesUI.aspx"
                                                        EnableViewState="False" Text="Ask Me"></asp:HyperLink>
                                                    <asp:Label ID="lblStudUnreadCount" runat="server" ViewStateMode="Enabled" Text="" CssClass="badge badge-warning animated bounceIn" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentAttendance" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink3" runat="server" NavigateUrl="~/RITeSchool/Student/StudentAttendance.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,Attendance%>"></asp:HyperLink>
                                                </td>
                                            </tr>  
                                            <tr id="trChangePassword" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink73" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/StudentChangePassword.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, ChangePassword%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentES" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkExamSchedule" runat="server" NavigateUrl="~/RITeSchool/Student/StandardwiseExamScheduleList.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,ExamSchedule%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink47" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/StudentAnnualResult.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,FinalResult%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="true" id="trStudentFee" runat="server">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink36" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Accountant/StudentPayFeeUI.aspx" Text="<%$ Resources:LocalizedResources,Fees%>"></asp:HyperLink>
                                                    <asp:ImageButton ID="lnkVideo" runat="server" ViewStateMode="Enabled" Visible="false" ImageUrl="../images/OnlinePaymentVideo.gif" ToolTip="Click here to view online fee payment video."/>
                                                </td>
                                            </tr>
                                            <tr id="trHolidays" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="lnkHolidayList" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/HolidayListUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,Holidays%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentHomework" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink82" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Homework/StudentHomeworkUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,Homework%>"></asp:HyperLink>
                                                </td>
                                            </tr>                                            
                                            <tr runat="server" id="trStudentLibrary" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkStudentLibrary" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        Visible="true" NavigateUrl="~/RITeSchool/LibrarianManagement/IssuedBookDetails.aspx"
                                                        Text="<%$ Resources:LocalizedResources,Library%>"></asp:HyperLink>
														 <%--<asp:HyperLink ID="hlnkStudentLibraryPPSN" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        Visible="true" NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryLinks.aspx"
                                                        Text="<%$ Resources:LocalizedResources,Library%>"></asp:HyperLink>--%>
                                                </td>
                                            </tr>
                                            <tr visible="false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink69" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryManagementUI.aspx" Text="<%$ Resources:LocalizedResources,LibraryManagement%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentMessageCenter" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink14" runat="server" NavigateUrl="/RITeSchool/Common/MessageInbox.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,MessageCenter%>"></asp:HyperLink>
                                                        <asp:ImageButton ID="imgBtnMsgAlertStud" runat="server" ViewStateMode="Enabled" ImageUrl="~/RITeSchool/images/NewMail_Blink.gif" CssClass="hide"
                                                            Visible="false" />
                                                </td>
                                            </tr>
                                            <tr id="trNextYearAdmission" runat="server" visible="false" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkNextYearAdmission" runat="server" CssClass="SubTitleMenu" NavigateUrl="#"
                                                        EnableViewState="False" Visible="true" Text="Next Year Admission"></asp:HyperLink>
                                                    <asp:Image ImageUrl="~/RITeSchool/images/newLink.gif" runat="server" ViewStateMode="Enabled" ID="Image1" />
                                                </td>
                                            </tr>
                                            <tr visible="true" runat="server" viewstatemode="Enabled" id="trParentTeacherAssociationForStudent">
                                                <td align="left" colspan="1" class="ClsBorderlight" id="tdParentTeacherAssociation"
                                                    runat="server" viewstatemode="Enabled" visible="true">                                                    
                                                    <asp:HyperLink ID="lnkStudentPTA" runat="server" CssClass="SubTitleMenu" NavigateUrl="#" EnableViewState="False" 
                                                        Text="<%$ Resources:LocalizedResources, ParentTeacherAssociation%>" ></asp:HyperLink>
                                                </td>
                                            </tr>                                        
                                            <tr runat="server" id="trStudentPR" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkStudentPR" runat="server" NavigateUrl="~/RITeSchool/Student/StudentProgressSheet.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,ProgressReport%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trReports" runat="server" visible="False" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink42" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SchoolReportUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,Reports%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trSurveyModule" runat="server" visible="false" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">                                                    
                                                    <asp:HyperLink ID="lnkSurveyFeedback" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="#"  
                                                        Text="School Feedback / Survey" CausesValidation="false"></asp:HyperLink>
                                                    <asp:Image ImageUrl="~/RITeSchool/images/newLink.gif" runat="server" ViewStateMode="Enabled" ID="imgNewMenu" />
                                                </td>
                                            </tr>                                             
                                            <tr id="trStudentSMSCenter" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink34" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SMSHistoryUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,SMSCenter%>"></asp:HyperLink>
                                                </td>
                                            </tr>  
                                            <tr id="trSubjectTeacher" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkSubTeachers" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/TeacherSubjectListUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,SubjectTeachers%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentTT" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink6" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/UserTimeTable.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,Timetable%>"></asp:HyperLink>
                                                </td>
                                            </tr>											
                                            <tr visible="true" runat="server" id="trTransportCommitteeForStudentLogin" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight" id="td3"
                                                    runat="server" visible="true">                                                    
                                                    <asp:HyperLink ID="lnkTransportCommitteeForStudentLogin" runat="server" CssClass="SubTitleMenu" NavigateUrl="#" EnableViewState="False" 
                                                        Text="<%$ Resources:LocalizedResources,TransportCommittee%>" ></asp:HyperLink>
                                                </td>
                                            </tr>
											<tr id="trParentHealthDetails" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperlnkParentHealthDetails" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/HealthDetails/ParentHealthDetailsUI.aspx"
                                                     EnableViewState="false" Text="Parent Health Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTransportDetails" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink55" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Transport/StudentTransportDetailsUI.aspx"
                                                        EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources,TransportDetails%>"></asp:HyperLink>
                                                </td>
                                            </tr>                                            
                                            <tr id="trUploadParentDetails" runat="server">
                                                <td align="left" colspan="1" class="ClsBorderlight" runat = "server" id = "tdUploadParentDetails" visible = "false">
                                                    <asp:HyperLink ID="lnkUploadParentDetails" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/UploadParentPhotosUI.aspx"
                                                         Text="Upload Parent Photos"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat = "server" id = "tdOnlineExamResult" visible = "false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink150" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/OnlineExam/OnlineExamDetailsUI.aspx"
                                                         Text="Online Exam Schedule"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat = "server" id = "tdOnlineExamProgressReport" visible = "false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink151" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/OnlineExam/OnlineExamProgressReportUI.aspx"
                                                         Text="Online Exam Progress Report"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat = "server" id = "trLeavingCertificate" visible = "false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink154" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Teacher/StudentLeavingCertificateUI.aspx"
                                                         Text="Transfer Certificate"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trUploadStudentPhoto" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLinkStudentPhoto" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/UploadStudentPhotoUI.aspx"
                                                     EnableViewState="false" Text="Upload Student Photo"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trBonafideRequestApplication" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink167" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/BonafideRequestApplicationUI.aspx"
                                                     EnableViewState="false" Text="Bonafide Request Application"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trStudentAssessment" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink168" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/StudentAssessmentDetailsUI.aspx"
                                                     EnableViewState="false" Text="Student Assessment"></asp:HyperLink>
                                                </td>
                                            </tr>
                                             <tr runat="server" id="trStudentMonthlyDetails" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="auto-style1">
                                                    <asp:HyperLink ID="HyperLink175" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/ExportStudentMonthlyActivityDetailsUI.aspx"
                                                     EnableViewState="false" Text="Student Monthly Activity Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                             <tr runat="server" id="trStudentExamWiseSubjectMarkDetails" viewstatemode="Enabled" visible="false">
                                                <td align="left" class="auto-style1">
                                                    <asp:HyperLink ID="hlnkStudentMarks" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/StudentExamWiseSubjectMarksDetailsUI.aspx"
                                                     EnableViewState="false" Text="Examwise Subject Marks Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="tblClassTeacher" runat="server" viewstatemode="Enabled" border="0" cellpadding="2" cellspacing="2" style="width: 100%;" visible="false" class="table tbl-sub-menu">                                            
                                            <tr id="trBetaVersionForClassTeacher" runat="server" visible="false" class="CPanelSpace"  viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">                                                    
                                                    <asp:HyperLink ID="hlnkBetaVersionForClassTeacher" runat="server" CssClass="SubTitleMenu" NavigateUrl="#"
                                                        EnableViewState="False" Text="Beta Version"></asp:HyperLink>
                                                    <img src="/images/newLink.gif" id="ctl00_MainBody_img1" alt="NEW" style="white-space:nowrap;">
                                                </td>
                                            </tr>
                                            <tr id="TrClassTeacherAbsentStudents" runat="server" viewstatemode="Enabled" class="CPanelSpace" style="width: 40%;"
                                                valign="top" >
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowAbsentStudentPopup()" style="cursor: pointer;">
                                                        Absent Student Details</a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                    <asp:HyperLink ID="HyperLink56" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/AnnualEventPlanner.aspx" Text="<%$ Resources:LocalizedResources, AnnualPlanner%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAskMeClassTeacher" runat="server" viewstatemode="Enabled" class="CPanelSpace" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                   <asp:HyperLink ID="HyperLink119" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/AskMe/PublishedQueriesUI.aspx" Text="Ask Me"></asp:HyperLink>
                                                        <asp:Label ID="lblClassTeacherQueCnt" runat="server" ViewStateMode="Enabled" Text="" CssClass="badge badge-warning animated bounceIn" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trMarkAssignmentClassTeaqcher" runat="server">
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkPrePrimaryAssignExam" runat="server" CssClass="SubTitleMenu"
                                                        NavigateUrl="~/RITeSchool/Teacher/TestMarksConfigurationUI.aspx" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources,AssignExamMarks%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAssignGradeClassTeacher" runat="server" class="CPanelSpace" visible="false" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                   <asp:HyperLink ID="HyperLink125" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Teacher/AssignGradesUI.aspx" Text="Assign Grades"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trClassTeacherHomework" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink93" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Homework/HomeworkUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, AssignHomework%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink10" runat="server" NavigateUrl="~/RITeSchool/Teacher/SchoolwiseAttendanceDetails.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,Attendance%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                    <asp:HyperLink ID="hlnkClassTeacherChangePassword" runat="server" CssClass="SubTitleMenu"
                                                        NavigateUrl="/RITeSchool/Common/StudentChangePassword.aspx" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, ChangePassword%>"></asp:HyperLink>
                                                </td>
                                            </tr>
											<tr id="trPeerConfig" runat="server" visible="false">
                                                <td style="width: 50%;">
                                                    <asp:HyperLink ID="hlnkPeerConfig" runat="server" ViewStateMode="Enabled" CssClass="SubTitleMenu" Visible="true"
                                                         NavigateUrl="/RITeSchool/Admin/ConfigurePeerDetailsUI.aspx" Text="Configure Peer Details"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trExamResults" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkPrePrimaryExamResults" runat="server" NavigateUrl="~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources,ExamResults%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTeacherExamSchedule" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink43" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Teacher/ExamScheduleList.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, ExamSchedule%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="false">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink45" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources,Fees%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="True" id="trFinalResults">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink38" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Teacher/StudentResultList.aspx"
                                                        Text="<%$ Resources:LocalizedResources, FinalResult%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink28" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/HolidayListUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Holidays%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trClassTeacherLibrary" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkClassTeacherLibrary" runat="server" CssClass="SubTitleMenu"
                                                        EnableViewState="False" NavigateUrl="~/RITeSchool/LibrarianManagement/IssuedBookDetails.aspx"
                                                        Text="<%$ Resources:LocalizedResources, Library%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink70" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryManagementUI.aspx" Text="<%$ Resources:LocalizedResources, LibraryManagement%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trClassTeacherMeassagecenter" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink11" runat="server" NavigateUrl="/RITeSchool/Common/MessageInbox.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MessageCenter%>"></asp:HyperLink><asp:ImageButton
                                                            ID="imgBtnMsgAlertClsT" runat="server" ViewStateMode="Enabled" ImageUrl="~/RITeSchool/images/NewMail_Blink.gif" CssClass="hide"
                                                            Visible="false" />
                                                </td>
                                            </tr>
                                            <tr id="TrclassteacherMissingAttendsAlert" runat="server" viewstatemode="Enabled" class="CPanelSpace" style="width: 40%;"
                                                valign="top" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowAttendanceAlertPopup()" style="cursor: pointer;">
                                                        Missing Attendance</a>
                                                </td>
                                            </tr>
                                            
                                            <tr id="trclassTeacherNonPermenant" runat="server" class="CPanelSpace" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowTeacherAlertPopup()" style="cursor: pointer;">
                                                        Non Permanent Teachers</a>
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkPrePrimaryProgressReport" runat="server" CssClass="SubTitleMenu"
                                                        NavigateUrl="~/RITeSchool/Student/StudentProgressSheet.aspx" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources,ProgressReport%>"></asp:HyperLink>
                                                </td>
                                            </tr>  
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" id="tdTeacherReports" runat="server" viewstatemode="Enabled" >
                                                    <asp:HyperLink ID="HyperLink74" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SchoolReportUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Reports%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trClassTeacherInventory" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink40" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/RequisitionListUI.aspx" Text="<%$ Resources:LocalizedResources, Requisition%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trClassTechersmscenter" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink25" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SMSHistoryUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, SMSCenter%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink9" runat="server" NavigateUrl="/RITeSchool/Common/StaffBirthDay.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, StaffBirthdays%>">&nbsp;&nbsp;</asp:HyperLink>
                                                    <asp:ImageButton ID="imgBtnBirthdayAlertClsT" Style="margin-left: 2px;" runat="server" ViewStateMode="Enabled" 
                                                        ImageUrl="~/RITeSchool/images/animated_gift_box3.gif" Visible="false"/>
                                                </td>
                                            </tr>
                                            <tr id="trStudentMenu" runat="server">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLinkStudents" runat="server" NavigateUrl="~/RITeSchool/Teacher/StudentsListUI.aspx"
                                                        Width="59px" CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources, Students%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trStudentListForAssessment" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink169" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/StudentListForAssessmentDetailsUI.aspx"
                                                        EnableViewState="False" Text="Student List For Assessment"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTimetable" runat="server" viewstatemode="Enabled" >
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink15" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/UserTimeTable.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources,Timetable%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td align="left" colspan="1" class="ClsBorderlight" id="td1" runat="server" visible="false" viewstatemode="Enabled" >
                                                    <asp:HyperLink ID="HyperLink61" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Transport/StudentTransportDetailsUI.aspx"
                                                        EnableViewState="False" Visible="false" Text="<%$ Resources:LocalizedResources, TransportDetails%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTeacherPhoto1" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink143" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Admin/UploadPhotosUI.aspx"
                                                        EnableViewState="False" Text="Upload Photo"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trWeeklyTimetable" runat="server" viewstatemode="Enabled" >
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink88" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Admin/TeacherTimeTable.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, WeeklyTimetable%>"></asp:HyperLink>
                                                </td>
                                            </tr>                        
                                        </table>
                                        <table id="tblTeacher" runat="server" viewstatemode="Enabled" border="0" cellpadding="2" cellspacing="2" class="table margin-bottom-5 tbl-sub-menu"
                                            style="width: 100%;" visible="false">
                                            <tr id="trBetaVersionForTeacher" runat="server" visible="false" class="CPanelSpace"  viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">                                                    
                                                    <asp:HyperLink ID="hlnkBetaVersionForTeacher" runat="server" CssClass="SubTitleMenu" NavigateUrl="#"
                                                        EnableViewState="False" Text="Beta Version"></asp:HyperLink>
                                                    <img src="/images/newLink.gif" id="Img2" alt="NEW" style="white-space:nowrap;">
                                                </td>
                                            </tr>  
                                            <tr id="TrAbsentStudentPopup" runat="server" visible="false" class="CPanelSpace"  viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowAbsentStudentPopup()" style="cursor: pointer;">
                                                        Absent Student Details</a>
                                                </td>
                                            </tr>  
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink54" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="/RITeSchool/Common/AnnualEventPlanner.aspx" Text="<%$ Resources:LocalizedResources, AnnualPlanner%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAskMeTeacher" runat="server" class="CPanelSpace" visible="false" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">                                                   
                                                   <asp:HyperLink ID="HyperLink68" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/AskMe/PublishedQueriesUI.aspx" Text="Ask Me"></asp:HyperLink>
                                                    <asp:Label ID="lblUnreadCount" runat="server" viewstatemode="Enabled" Text="" CssClass="badge badge-warning animated bounceIn" Visible="false"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr id="trMarkAssignment" runat="server">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink16" runat="server" NavigateUrl="~/RITeSchool/Teacher/TestMarksConfigurationUI.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources, AssignExamMarks%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trAssignGradesTeacher" runat="server" viewstatemode="Enabled" class="CPanelSpace" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                   <asp:HyperLink ID="HyperLink127" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Teacher/AssignGradesUI.aspx" Text="Assign Grades"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trTeacherHomework" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink96" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Homework/HomeworkUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, AssignHomework%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight" colspan="1">
                                                    <asp:HyperLink ID="hlnkTeacherChangePassword" runat="server" CssClass="SubTitleMenu"
                                                        NavigateUrl="/RITeSchool/Common/StudentChangePassword.aspx" EnableViewState="False"
                                                        Text="<%$ Resources:LocalizedResources, ChangePassword%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trExamSchedule" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink171" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Teacher/ExamScheduleList.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, ExamSchedule%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink27" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/HolidayListUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Holidays%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr runat="server" id="trTeacherLibrary" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="hlnkTeacherLibrary" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/IssuedBookDetails.aspx" Text="<%$ Resources:LocalizedResources, Library%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr visible="false">
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink71" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/LibrarianManagement/LibraryManagementUI.aspx" Text="<%$ Resources:LocalizedResources, LibraryManagement%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTecherMessageCenter" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                  <asp:ImageButton ID="imgBtnMsgAlertT" runat="server" viewstatemode="Enabled" ImageUrl="~/RITeSchool/images/NewMail_Blink.gif" CssClass="count-spacing hide"
                                                            Visible="false" />
                                                    <asp:HyperLink ID="HyperLink18" runat="server" NavigateUrl="/RITeSchool/Common/MessageInbox.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Text="<%$ Resources:LocalizedResources, MessageCenter%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="TrteacherMissingAttendsAlert" runat="server" class="CPanelSpace" visible="false" viewstatemode="Enabled" >
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowAttendanceAlertPopup()" style="cursor: pointer;">
                                                        Missing Attendance</a>
                                                </td>
                                            </tr>  
                                            
                                            <tr id="trNonPermenantTeacherlink" runat="server" class="CPanelSpace" visible="false">
                                                <td align="left" class="ClsBorderlight">
                                                    <a class="SubTitleMenu" onclick="ShowTeacherAlertPopup()" style="cursor: pointer;">
                                                        Non Permanent Teachers</a>
                                                </td>
                                            </tr> 
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" id="tdTeacherReport" runat="server" viewstatemode="Enabled" 
                                                    visible="true">
                                                    <asp:HyperLink ID="HyperLink79" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SchoolReportUI.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Reports%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trSubTeacherInventory" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink37" runat="server" CssClass="SubTitleMenu" EnableViewState="False"
                                                        NavigateUrl="~/RITeSchool/Inventory/RequisitionListUI.aspx" Text="<%$ Resources:LocalizedResources, Requisition%>"></asp:HyperLink>
                                                        <span visible="false" class="clsCount" runat="server" viewstatemode="Enabled" id="spnRequisitionCount" title=""></span>
                                                </td>
                                            </tr>
                                            <tr id="trTeacherSmsCenter" runat="server" viewstatemode="Enabled" >
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink8" runat="server" CssClass="SubTitleMenu" NavigateUrl="/RITeSchool/Common/SMSHistoryUI.aspx"
                                                        Text="<%$ Resources:LocalizedResources, SMSCenter%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink29" runat="server" NavigateUrl="/RITeSchool/Common/StaffBirthDay.aspx"
                                                        CssClass="SubTitleMenu" EnableViewState="False" Visible="true" Text="<%$ Resources:LocalizedResources, StaffBirthdays%>"></asp:HyperLink>
                                                    <asp:ImageButton ID="imgBtnBirthdayAlertT" Style="margin-left: 2px;" runat="server" viewstatemode="Enabled" 
                                                        ImageUrl="~/RITeSchool/images/animated_gift_box3.gif" Visible="false"  />
                                                </td>
                                            </tr>   
                                            <tr>
                                                <td align="left" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink13" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Student/UserTimeTable.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, Timetable%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" colspan="1" class="ClsBorderlight" id="td2" runat="server" visible="false" viewstatemode="Enabled" >
                                                    <asp:HyperLink ID="HyperLink62" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Transport/StudentTransportDetailsUI.aspx"
                                                        EnableViewState="False" Visible="false" Text="<%$ Resources:LocalizedResources, TransportDetails%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trTeacherPhoto2" runat="server" viewstatemode="Enabled" visible="false">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink132" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Admin/UploadPhotosUI.aspx"
                                                        EnableViewState="False" Text="Upload Photo"></asp:HyperLink>
                                                </td>
                                            </tr>
                                            <tr id="trWeeklyTimeTableTeacher" runat="server" viewstatemode="Enabled">
                                                <td align="left" style="width: 50%;" class="ClsBorderlight">
                                                    <asp:HyperLink ID="HyperLink89" runat="server" CssClass="SubTitleMenu" NavigateUrl="~/RITeSchool/Admin/TeacherTimeTable.aspx"
                                                        EnableViewState="False" Text="<%$ Resources:LocalizedResources, WeeklyTimetable%>"></asp:HyperLink>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Table ID="tblTeacherLeft" runat="server" Width="100%" CellSpacing="2" CellPadding="2" CssClass="table tbl-sub-menu" BorderWidth="0" Visible="false" viewstatemode="Enabled" >
                                            <asp:TableRow ID="tblTeacherRow" runat="server" ViewStateMode="Enabled" >
                                                <asp:TableCell CssClass="header-color-green-custom" Text="<%$ Resources:LocalizedResources, ExtraAssignedScreens%>"> </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                        <asp:Table ID="tblSuperLeft" runat="server" ViewStateMode="Enabled" CellSpacing="1" CellPadding="2" CssClass="table tbl-sub-menu width-99-percentage" BorderWidth="0" HorizontalAlign="Left">
                                        </asp:Table>
                                    </td>                               
                                    <td id="tdDashboardContent" class="col-lg-12 col-md-12 col-sm-12 col-xs-12" valign="top">
										 <div class="col-lg-9 col-md-9 col-sm-9 col-xs-9" runat="server" viewstatemode="Enabled" id="SchoolNoticeDiv" visible="false">
                                            <div id="divSchoolNotice" class="schoolNotice hide">
                                                <marquee style="border-top-width: thin; vertical-align: bottom; color: MediumVioletRed; border-top-color: blue;"
                                                    behavior="scroll" direction="left"
                                                    scrollamount="2" scrolldelay="2" onmouseover="javascript:this.setAttribute('scrollamount','0');" onmouseout="javascript:this.setAttribute('scrollamount','2 ');">
														<asp:Label ID="lblNoticeBoardMsg" runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB"></asp:Label></marquee>
                                            </div>
                                        </div>
                                         <div id="divDashboardContainer" runat="server" style="display:none">
                                            <div class="col-lg-9 col-md-9">
                                               <%if (moUserRole != Utility.Constants.UserRoles.Admin
                                                      && (hidPrincipalDesignationId.Value != Utility.Constants.S_PRINCIPAL_DESIGNATION_ID || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value=="Y"))) %>
                                                <%{ %>
                                                   <div class="row padding-bottom-20" runat="server">
                                                    <table visible="false" runat="server" viewstatemode="Enabled" cellpadding="0" style="width: 100%;" id="tblStudentDetails" class="margin-bottom-0 table-teacher-details  table-border-blue">
                                                        <tr>
                                                            <td align="left" class="StudentDOBHead table-header" style="width: 85%">
                                                                <asp:Label ID="Label9" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, StudentDetails%>"></asp:Label>
                                                            </td>
                                                            <td align="center" class="ClsBorderlight vertical-align-middle" rowspan="2" style="width: 50%;">
                                                                <img id="imgPhoto" alt="image" height="212" width="210" src="../images/empty-profile.jpg"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 85%" align="center">
                                                                <table cellpadding="0" cellspacing="1" style="width: 100%;"  class="table margin-bottom-0">
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left" style="width: 40%;">
                                                                            <asp:Label ID="Label4" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right" style="width: 60%;">
                                                                            <asp:Label ID="HyperLink46" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label7" runat="server" CssClass="ClsLabel  text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblDOB" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, DateOfBirth%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label5" runat="server" CssClass="ClsLabel  text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="Label1" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, DivClass%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label6" runat="server" CssClass="ClsLabel  text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblRollNo" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, RollNo%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left remove-border-bottom">
                                                                            <asp:Label ID="Label23" runat="server" CssClass="ClsLabel  text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, MobileNumber%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right remove-border-bottom">
                                                                            <asp:Label ID="lblMobileOne" CssClass="ClsHilightTextB" EnableViewState="false" runat="server"
                                                                                Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table runat="server" visible="false" cellpadding="0" cellspacing="2" class="margin-bottom-0 table-border-blue table-teacher-details"
                                                        id="tblTeacherDetails">
                                                        <tr>
                                                            <td align="left" class="StudentDOBHead table-header">
                                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, TeacherDetails%>"></asp:Label>
                                                            </td>
                                                             <td align="center" class="ClsBorderlight vertical-align-middle td-userimage" rowspan="2">
	                                                            <img ID="imgTeacher" alt="image" height="214" width="160" style="padding:2px" src="../images/empty-profile.jpg"/>
	                                                        </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 85%" align="center" class="line-height-22">
                                                                <table cellpadding="0" cellspacing="1" class="td-width-100 remove-margin-bottom" style="height: 178px !important;">
                                                                    <tr>
                                                                        <td align="left" style="width: 30%" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label8" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, TeacherName%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" style="width: 70%" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblTeacherName" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, TeacherName%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label11" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Designation%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Designation%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trQualification" runat="server" visible="false">
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <asp:Label ID="Label13" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Qualification%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight">
                                                                            <asp:Label ID="lblQualification" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Qualification%>"> </asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="Label15" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, ClassTeacher%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblClassDiv" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, DivClass%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left remove-border-bottom">
                                                                            <asp:Label ID="Label24" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources,MobileNumber%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding">:</span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right remove-border-bottom">
                                                                            <asp:Label ID="lblTeacherMobile" CssClass="ClsHilightTextB" EnableViewState="false"
                                                                                runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table runat="server" viewstatemode="Enabled"  visible="false" cellpadding="0" cellspacing="2" style="width: 100%" class="margin-bottom-0 table-border-blue table-teacher-details"
                                                        id="tblSepervisorDetails"> 
                                                        <tr>
                                                            <td align="left" class="StudentDOBHead table-header" style="width: 85%;">
                                                                <asp:Label ID="lblSupervisorDetailsField" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Details%>"></asp:Label>
                                                            </td>
                                                            <td align="center" class="ClsBorderlight vertical-align-middle td-userimage" rowspan="2">
	                                                            <img ID="imgSuperVisor" alt="image" height="214" width="158" style="padding:2px" src="../images/empty-profile.jpg"/>
	                                                        </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 85%" align="center" class="line-height-22">
                                                                <table cellpadding="0" cellspacing="1" class="table td-width-100 remove-margin-bottom" style="height: 176px !important;">
                                                                    <tr>
                                                                        <td align="left" style="width: 30%; padding:18px !important" class="ClsBorderlight remove-border-left">
                                                                            <asp:Label ID="lblSupervisorNameField" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Name%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding"></span>
                                                                        </td>
                                                                        <td align="left" style="width: 70%; padding:18px !important" class="ClsBorderlight remove-border-right">
                                                                            <asp:Label ID="lblSupervisorName" runat="server" CssClass="ClsHilightTextB" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, SupervisorName%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left" style="padding:18px !important">
                                                                             <asp:Label ID="Label22" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, Designation%>"></asp:Label>
                                                                            <span class="ClsLabel colonPadding"></span>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-right" style="padding:18px !important">
                                                                            <asp:Label ID="lblSupervisorDesignation" runat="server" CssClass="ClsHilightTextB"
                                                                                EnableViewState="False" Text="<%$ Resources:LocalizedResources, Designation%>"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" class="ClsBorderlight remove-border-left remove-border-bottom" style="padding:18px !important">
                                                                                  <asp:Label ID="Label25" runat="server" CssClass="ClsLabel text-info" EnableViewState="False"
                                                                                Text="<%$ Resources:LocalizedResources, MobileNumber%>"></asp:Label>
                                                                        </td>
                                                                        <td align="left" class="ClsBorderlight remove-border-bottom remove-border-right" style="padding:18px !important">
                                                                            <asp:Label ID="lblSuperwiserMob" CssClass="ClsHilightTextB" EnableViewState="false"
                                                                                runat="server" Text=""></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                 </div>
                                                <%} %>

                                                  <%
                                                      string feeWidgetClass = ((miSchoolId == (int)Utility.Constants.SchoolId.JPS || miSchoolId == (int)Utility.Constants.SchoolId.GSS || miSchoolId == (int)Utility.Constants.SchoolId.MVPS) && (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID.ToString()) ? "hide" : "padding-bottom-20");
                                                   %>
                                                <%-- This widget need to show for Principal, Admin only --%>
                                                <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value=="N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)) %>
                                                <%{ %>
                                                  <div class="row padding-bottom-20 <%= feeWidgetClass %>" id="divFeeDetails" runat = "server">
                                                    <div class="col-sm-12 col-xs-12 white-box-container">
                                                        <div class="white-box-header">
                                                            Fee Status
                                                            <span id="lblFeeStatusFilter" class="widget-filter-label" style="text-transform:none;"></span>
                                                            <div id="divFeeStatus" class="widget-toolbar widget-toolbar-padding">
                                                                <a id="hlnkRefreshFeeSummary" class="lnk-refresh margin-left-right-5 settings-link color-grey"
                                                                    onclick="loadFeeWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshFeeSummary', 'FeeSummary')"><i class="icon- fa fa-refresh"></i></a>                      
																<a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-grey"
                                                                    data-toggle="" onclick="showFilter(this,setFeeWidgetSelectedYear())"><i class="icon- fa fa-cog"></i></a>
                                                                <ul id="ulFeeAcademicYearId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                    <li>
                                                                        <div id="divFeeStatusAcademicYearLabel" class="filter-labels">
                                                                            Select Academic Year
                                                                        </div>
                                                                        <div class="padding-left-15">
                                                                            <asp:DropDownList ID="cmbFeeAcademicYear" runat="server" ViewStateMode="Enabled" Width="90%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadFeeWidget(false, true);">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearFeeStatusFilters()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hideFeeStatusWidgetFilter();">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                        <div id="divFeeWidget">
														    <div id="divFeeWidgetContent" class="row padding-left-right-15 overlay-parent">
                                                             <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 padding-top-20 padding-bottom-19">
                                                                <div class="white-box-content">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <span class="label-warning icon- fa fa-hourglass col-md-6 animate" data-animation="rotateIn"></span>
                                                                    </div>
                                                                    <div id="divExpectedAmount" class="widget-box-text col-md-8 col-sm-8 count">
                                                                    </div>
                                                                    <h6 class="label-warning text-white col-md-12 col-sm-12 col-xs-12">
                                                                        <strong>Total Receivable Fee</strong></h6>
                                                                </div>
                                                            </div>
                                                             <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 padding-top-20 padding-bottom-19">
                                                                <div class="white-box-content">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <span class="label-purple  icon- fa fa-hourglass-3 col-md-6 animate" data-animation="rotateIn"></span>
                                                                    </div>
                                                                    <div id="divTodaysCollection" class="widget-box-text col-md-8 count" data-placement="bottom" data-rel="tooltip" title="Including uncleared payments">
                                                                    </div>
                                                                    <h6 class="label-purple  text-white col-md-12 col-sm-12 col-xs-12">
                                                                        <strong id = "stCollection"></strong></h6>
                                                                </div>
                                                            </div>
                                                             <div class="col-lg-4 col-md-4 col-sm-4 col-xs-4 padding-top-20 padding-bottom-19">
                                                                <div class="white-box-content">
                                                                    <div class="col-md-4 col-sm-4 col-xs-4">
                                                                        <span class="label-info icon- fa fa-hourglass-half col-md-6 animate" data-animation="rotateIn"></span>
                                                                    </div>
                                                                    <div id="divConcession" class="widget-box-text col-md-8 col-sm-8 count">
                                                                    </div>
                                                                    <h6 class="label-info text-white col-md-12 col-sm-12 col-xs-12">
                                                                        <strong id = "stConcession"></strong></h6>
                                                                </div>
                                                            </div>
                                                             <div class="col-lg-4 col-md-6 col-sm-6 col-xs-6 padding-top-20 padding-bottom-19 hide">
                                                                <div class="white-box-content">
                                                                    <div class="col-md-4 col-sm-4">
                                                                        <span class="bg-red icon- fa fa-hourglass-start col-md-6 animate" data-animation="rotateIn"></span>
                                                                    </div>
                                                                    <div id="divTotalDues" class="widget-box-text col-md-8 col-sm-8 count">
                                                                    </div>
                                                                    <h6 class="bg-red text-white col-md-12 col-sm-12">
                                                                        <strong>Total dues till date</strong></h6>
                                                                </div>
                                                            </div>
															</div>
                                                            <div id="divFeeWidgetMessage">
                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
														</div>
                                                    </div>
                                                 </div>
                                                <%} %>
                                            
                                                <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)
                                                        || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SENIOR_ADMINISTRATIVE_OFFICER)) %>
                                                <%{ %>
                                                     <div class="row padding-bottom-20">
                                                          <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 white-box-container" id="divAttendance">
                                                        <div class="white-box-header">
                                                            Attendance Summary
                                                            <span id="lblAttendanceSummaryFilter" class="widget-filter-label"></span>
                                                            <div id="divAttendanceSummary" class="widget-toolbar widget-toolbar-padding" >
                                                                <a id="hlnkRefreshAttendanceSummary" class="lnk-refresh margin-left-right-5 settings-link color-grey"
                                                                    onclick="loadAttendanceWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshAttendanceSummary', 'AdminAttendanceSummary')">
                                                                    <i class="icon- fa fa-refresh"></i></a>
																<a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-grey"
                                                                    data-toggle="" onclick="showFilter(this, setPreviousSelectedDate())"><i class="icon- fa fa-cog"></i></a>
                                                                <ul id="ulAttendanceSummaryId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                    <li>
                                                                        <div id="divAttendanceDateLabel" class="filter-labels">
                                                                            Select Date
                                                                        </div>
                                                                        <div class="padding-left-15 margin-left-negative-5">
                                                                            <input id="datepicker" style="width: 90%" />
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadAttendanceWidget(false, true)">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearAttendanceSummaryFilters()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hideAttendanceSummaryFilter()">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
													<div id="divAttendanceSummaryWidgetContent" style="height:303px">
                                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-6 padding-top-bottom-attendanceSummary">
                                                            <div class="white-box-content">
                                                                <div class="widget-sub-header">
                                                                    Attendance Marked for Classes</div>
                                                                <div id="classAttendanceGauge">
                                                                </div>
                                                            </div>
                                                            <div style="text-align:center">
                                                           <span style="float:inherit;text-align: center;font-size: 15px;font-weight:bold;" class="widget-toolbar widget-toolbar-padding text-info-label" id="spanStuentClassCount" onmouseover="SetClassAttendanceTooltip()"
                                                                      data-placement="top" data-trigger="hover" data-rel="popover" data-content=""> 
                                                                </span>
                                                             </div>

                                                        </div>
                                                        <div class="col-lg-4 col-md-6 col-sm-6 col-xs-6 padding-top-bottom-attendanceSummary">
                                                            <div class="white-box-content">
                                                                <div class="widget-sub-header" style="color: #0dacf4;">
                                                                    Attendance Marked for Students</div>
                                                                <div id="studentAttendanceGauge">
                                                                </div>
                                                            </div>

                                                            <div style="text-align:center">
                                                             <span style="float:inherit;text-align: center;font-size: 15px;font-weight:bold;"  class="widget-toolbar widget-toolbar-padding text-info-label" id="spanTotalStudentCount" onmouseover="SetAttendanceTooltip()"
                                                                      data-placement="top" data-trigger="hover" data-rel="popover" data-content="">  
                                                              </span>
                                                           </div>
                                                         
                                                        </div>
                                                        <div class="col-lg-4 col-md-12 col-sm-12 col-xs-12 padding-top-bottom-attendanceSummary">
                                                            <div id="divPendingAttendance" class="white-box-content">
                                                                <div class="widget-sub-header" style="color: #4a5bb9;">
                                                                    Top 3 Classes with Pending Attendance</div>
                                                                <div class="row">
                                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                                        <div id="divSetClassCount_1" class="text-info-label" data-placement="bottom" data-rel="tooltip">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div id="divSetPercentageContainer_1" class="easy-pie-chart percentage" data-animation="rotateIn" data-percent="" data-placement="bottom" 
                                                                            data-color="#d53f40">
                                                                            <div id="divSetPercentage_1" class="percent">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-6 col-sm-6 col-xs-6">
                                                                        <div id="divSetClassCount_2" class="text-info-label" data-placement="bottom" data-rel="tooltip">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div id="divSetPercentageContainer_2" class="easy-pie-chart percentage" data-animation="rotateIn" data-percent="" data-placement="bottom" 
                                                                            data-color="#87ceeb">
                                                                            <div id="divSetPercentage_2" class="percent">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <div class="row">
                                                                    <div id="divSetClassCount_3" class="text-info-label" data-placement="bottom" data-rel="tooltip">
                                                                        &nbsp;
                                                                    </div>
                                                                    <div id="divSetPercentageContainer_3" class="easy-pie-chart percentage" data-animation="rotateIn" data-percent="" data-placement="bottom" 
                                                                        data-color="#9585bf">
                                                                        <div id="divSetPercentage_3" class="percent">
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div> 
														</div>
                                                    <div id="divAttendanceSummaryWidgetMessage">
                                                            <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                        </div>
                                                    </div>
                                                     </div>
                                                <%} %>

                                                 <%
                                                    string setColumnClass = "";
                                                    string setPaddingRight = "";

                                                    if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                        || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME))
                                                    {
                                                        setColumnClass = "col-lg-4 col-md-4 col-sm-4 col-xs-4";
                                                        setPaddingRight = "padding-right-6";
                                                    }
                                                    else {
                                                        setColumnClass = "col-lg-6 col-md-6 col-sm-6 col-xs-6";
                                                        setPaddingRight = "padding-right-0";

                                                    }
                                                %>
                                                <div class="row padding-bottom-20">
                                                     <%if (moUserRole != Utility.Constants.UserRoles.OtherStaff) %>
                                                     <%{ %>
                                                          <div class="<%= setColumnClass %> upcoming-events" style="padding-left: 0px;padding-right: 6px;" id="eventRow">
                                                        <div class="white-box-container" style="height: 410px;">
                                                            <div class="white-box-header" style="margin-left:0; margin-right:0px;">
                                                                Upcoming 
                                                                    <a id="hlnkRefreshUpcomingEvent" class="lnk-refresh margin-left-right-5 settings-link color-grey pull-right"
                                                                        onclick="loadUpcomingEventsWidget(true, true);" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshUpcomingEvent', 'UpcomingEventsList','<%= miUserId %>')"><i class="icon- fa fa-refresh"></i></a>
                                                                    <p>
                                                                        <asp:Label ID="lblEventColor" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
												                                        BackColor="#afeeee" Height="10px" ReadOnly="True" Text=" " Width="10px" EnableViewState="False"></asp:Label>
											                            <asp:Label CssClass = "ClsTextNormal" ID="lblEvents" Font-Bold = "true" runat="server" EnableViewState="False" Text="Events"></asp:Label>
										                                <asp:Label ID="lblHolidayColor" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"
												                            BackColor="#FFEFD5" Height="10px" ReadOnly="True" Text=" " Width="10px" EnableViewState="False"></asp:Label>
											                            <asp:Label CssClass = "ClsTextNormal" ID="lblHoliday" Font-Bold = "true" runat="server" EnableViewState="False" Text="Holiday"></asp:Label>
                                                                        <asp:Label ID="lblExamColor" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" 
												                            BackColor="#d8eb88" Height="10px" ReadOnly="True" Text=" " Width="10px" EnableViewState="False"></asp:Label>
											                            <asp:Label CssClass = "ClsTextNormal" ID="lblExam" Font-Bold = "true" runat="server" EnableViewState="False" Text="Exam"></asp:Label>
                                                                    </p>
                                                            </div>
                                                            <div class="demo-section k-content wide" id="divEventContainer">
                                                                <div id="innerUlEvents" class="remove-padding-left">
                                                                </div>
                                                                <div id="liSeeAllEvents" class="padding-top-10" style="display:none; border-top:1px solid #eee !important;">
                                                                        <div class="center">
                                                                            <a href="/RITeSchool/Common/AnnualEventPlanner.aspx">See all events <i class="icon- fa fa-arrow-right"></i></a>  
                                                                    </div>
                                                                    <div style="text-align:center; height:10px; font-family:Times New Roman; font-size:14px; color:Blue; font-weight:bold;" align="center">
                                                                        <asp:Label align="center" ID="lblUpcominEvents" runat="server" Text="Please re-login or refresh the widget to see the updates"></asp:Label>
                                                                    </div> 
                                                                </div>  
															</div>
                                                            <div id="divInnerUlEvents" class="error-message hide">
                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                          <div class="<%= setColumnClass %> <%= setPaddingRight %>" style="padding-left: 6px;">
                                                     <div id="divMessagecenter" class="white-box-container" style="height: 410px;" runat="server" viewstatemode="Enabled" >
                                                            <div class="white-box-header" style="margin-left:0; margin-right:0px;">
                                                                 Unread Messages
                                                                 <span class="badge badge-pink default-cursor vertical-align-middle margin-top-3" id="unreadmsg-Count">0</span>
                                                            </div>
                                                            <div id="divMessageCotainer" class="demo-section k-content wide overlay-parent">
                                                                <div id="innerUlMessage" class="remove-padding-left" style="overflow: hidden; width: auto; height: 277px; padding: 5px;">
                                                                </div>
                                                                 <div id="liSeeallmessages" class="padding-top-10" style="display:none; border-top:1px solid #eee !important;">
                                                                        <div class="center">
                                                                            <a href="MessageInbox.aspx">See all messages <i class="icon- fa fa-arrow-right"></i></a>  
                                                                    </div>   
                                                                                                                                   
                                                                </div>  
															</div>
                                                            <div id="divInnerUlMessage"  class="error-message hide">
                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <% } %>
                                                    <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)) %>
                                                    <%{ %>
                                                        <div class="<%= setColumnClass %>" style="padding-left: 6px; padding-right: 0px;">
                                                        <div class="white-box-container" style="height: 410px;">
                                                            <div class="white-box-header" style="margin-left:0; margin-right:0px;">
                                                                Exam Result
                                                                <div id="divExamwiseStudentPerformance" class="widget-toolbar" style="right: 0px; top: -5px;">
                                                                    <a id="hlnkRefreshExamStudentPerformance" class="lnk-refresh margin-left-right-5 settings-link color-grey"
                                                                        onclick="loadExamWiseStudentPerformanceWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshExamStudentPerformance', 'ExamWiseStudentPerformance')">
                                                                        <i class="icon- fa fa-refresh"></i></a>

                                                                    <a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-grey"
                                                                        data-toggle="" onclick="showFilter(this),setPreviousSelectedStandardWiseExam()"><i class="icon- fa fa-cog"></i></a>
                                                                    <ul id="ulStandardDivisionId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                        <li>
                                                                            <div id="divStandardLabel" class="filter-labels">
                                                                                Select Standard
                                                                            </div>
                                                                            <div class="padding-left-15">
                                                                                <asp:DropDownList ID="cmbStandardName" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                                                    Style="width: 90%;" AutoPostBack="false" onchange="getExamsForSelectedStandard()"></asp:DropDownList>
                                                                             </div>
                                                                        </li>
                                                                         <li>
                                                                            <div id="divExamLabel" class="filter-labels">
                                                                                Select Exam
                                                                            </div>
                                                                            <div class="padding-left-15">
                                                                                <asp:DropDownList ID="cmbStandardWiseExam" runat="server" ViewStateMode="Enabled" CssClass="LrgCombo"
                                                                                    Style="width: 90%;" AutoPostBack="false">
                                                                                </asp:DropDownList>
                                                                            </div>
                                                                        </li>
                                                                        <li class="divider"></li>
                                                                        <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                            <div class="divFiterButtons">
                                                                                <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                    data-original-title="Apply Filter" onclick="loadExamWiseStudentPerformanceWidget(false, true)">
                                                                                    <i class="icon- fa fa fa-check"></i>
                                                                                </button>
                                                                                <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                    data-original-title="Clear Filter" onclick="clearExamwiseStudentPerformnaceWidgetFilter()">
                                                                                    <i class="icon- fa fa-undo"></i>
                                                                                </button>
                                                                                <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                    data-original-title="Cancel" onclick="hideExamwiseStudentPerformanceWidgetFilter(this);">
                                                                                    <i class="icon- fa fa-remove"></i>
                                                                                </button>
                                                                            </div>
                                                                        </li>
                                                                    </ul>
                                                                </div>
                                                            </div>
                                                            <div class="demo-section k-content wide" id="divExamwiseStudentPerformanceGraph" style="padding:5px;">
                                                                <div id="divExamwiseStudentPerformanceChart" class="resizable" style="width: 100%; height: 300px;"></div>
                                                                <div id="divFilterDetails" class="hide text-align-center"><span id ="lblExamWiseStudentPerformaceFilter" class="widget-filter-label"></span></div>
                                                            </div>
                                                             <div id="divExamwiseStudentPerformanceMessage">
                                                               <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <%} %>
                                                </div>
                                                <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)) %>
                                                <%{ %>
                                                    <div class="row padding-bottom-20">
                                                    <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 white-box-container">
                                                        <div class="white-box-header">
                                                            Accounts&nbsp;<span id="spanAccountWidgetFilter" class="widget-filter-label"></span>
                                                            <span class="widget-filter-label" id="AccountNoticeContent"></span>
                                                            <div id="divAccounts" class="widget-toolbar widget-toolbar-padding">
                                                              <a id="hlnkRefreshAccounts" class="lnk-refresh margin-left-right-5 settings-link color-grey"
                                                                        onclick="loadAccountWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshAccounts', 'AccountSummary')"><i class="icon- fa fa-refresh"></i></a>

                                                                <a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-grey"
                                                                    data-toggle="" onclick="showFilter(this,setAccountWidgetSelectedYear())"><i class="icon- fa fa-cog"></i></a>
                                                                <ul id="ulAccountsYearId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                    <li>
                                                                        <div id="divAccountsLabel" class="filter-labels">
                                                                            Select Financial Year
                                                                        </div>
                                                                        <div class="padding-left-15">
                                                                            <asp:DropDownList id="cmbAccountsFinancialYear" runat="server" ViewStateMode="Enabled" style="width: 90%;" AutoPostBack="false">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadAccountWidget(false, true);">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearAccountsWidgetFilters()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hideAccountsWidgetFilter();">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
                                                        <div class="row demo-section k-content wide overlay-parent padding-left-right-15" id="divAccountFlowChartContent" style="min-height: 200px;">
                                                            <div id="accountFlowChart" class="animate" data-animation="bounceIn">
                                                            </div>
                                                        </div>
														<div id="divAccountFlowChartMessage">
                                                            <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                        </div>
														</div>
                                                </div>
                                                    <div class="row padding-bottom-20">
                                                    <div id="divPayrollWidget" class="col-lg-12 col-md-12 col-sm-12 col-xs-12 white-box-container">
                                                        <div class="white-box-header">
                                                            Payroll&nbsp;<span id="spanPayrollWidgetFilter" class="widget-filter-label"></span>
                                                            <span class="widget-filter-label" id="payrollNoticeContent"></span>
                                                            <div id="divPayrollWidgetToolbar" class="widget-toolbar widget-toolbar-padding">
                                                                <a id="hlnkRefreshPayroll" class="lnk-refresh margin-left-right-5 settings-link color-grey"
                                                                        onclick="loadPayrollWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshPayroll', 'PayrollSummary')"><i class="icon- fa fa-refresh"></i></a>

                                                                <a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-grey"
                                                                     data-toggle="" onclick="showFilter(this,setPayrollWidgetSelectedMonthAndYear())"><i class="icon- fa fa-cog"></i></a>
                                                                <ul id="ulPayrollMonthId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                   <li id="li2">
                                                                        <div id="divPayrollYearFilterLabel" class="filter-labels">
                                                                            Select Year</div>
                                                                        <div class="padding-left-15">
                                                                            <asp:DropDownList ID="cmbPayrollYear" runat="server" ViewStateMode="Enabled" Width="90%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                        </li> 
                                                                    <li id="lstPyroll">
                                                                        <div id="divPayrollMonthFilterLabel" class="filter-labels">
                                                                            Select Month</div>
                                                                        <div class="padding-left-15">
                                                                            <select id="cmbPayrollMonth" style="width: 90%;">                                                                                
                                                                                <option value="1">January</option>
                                                                                <option value="2">February</option>
                                                                                <option value="3">March</option>
                                                                                <option value="4">April</option>
                                                                                <option value="5">May</option>
                                                                                <option value="6">June</option>
                                                                                <option value="7">July</option>
                                                                                <option value="8">August</option>
                                                                                <option value="9">September</option>
                                                                                <option value="10">October</option>
                                                                                <option value="11">November</option>
                                                                                <option value="12">December</option>
                                                                            </select>
                                                                        </div>
                                                                    </li>
                                                                    <li>
                                                                    <div id="divPayrollFinancialYearLabel" class="filter-labels">
                                                                            Select Financial Year
                                                                        </div>
                                                                        <div class="padding-left-15">
                                                                            <asp:DropDownList id="cmbPayrollFinancialYear" runat="server" ViewStateMode="Enabled" style="width: 90%;" AutoPostBack="false">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadPayrollWidget(false, true)">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearPayrollWidgetFilter()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hidPayrollWidgetFilter();">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                            </div>
                                                        </div>
														<div id="divPayrollContent" class="row overlay-parent padding-left-right-15">
                                                            <div id="divPayrollLeft" class="col-lg-8 col-md-8 col-sm-8 col-xs-8 padding-top-bottom-20">
                                                                <div class="white-box-content" style="min-height: 200px; padding-bottom:33px;">
                                                                    <div id="payrollChart" class="animate" data-animation="bounceIn">
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div style="height: 325px; padding-top: 162.5px;" class="col-lg-8 col-md-8 col-sm-8 col-xs-8 padding-top-bottom-20 no-record-found-msg hide" id="divPayrollChartMessage">
                                                               <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                            <div id="divPayrollRight" class="col-lg-4 col-md-4 col-sm-4 col-xs-4 padding-top-bottom-20">
                                                            <div class="white-box-content">
                                                                <div class="col-md-4 col-sm-4">
                                                                    <span class="label-info icon- fa fa-database col-md-6 animate" data-animation="rotateIn"></span>
                                                                </div>
                                                                <div id="divPreviousMonthPaidSalary" class="widget-box-text col-md-8">
                                                                  </div>
                                                                <h6 class="label-info text-white col-md-12 col-sm-12 padding-0">
                                                                    <strong id="lblSalaryWidgetText">Salary Paid for Month</strong>&nbsp;<label id="lblSalaryPaidforMonthYear"></label></h6>
                                                            </div>
                                                            <div class="white-box-content" style="margin-top: 20px;">
                                                                <div class="col-md-4 col-sm-4">
                                                                    <span class="label-success icon- fa fa-check col-md-6 animate" data-animation="rotateIn"></span>
                                                                </div>
                                                                <div id="divIncomeTaxAmount" class="widget-box-text col-md-8">
                                                                </div>
                                                                <h6 class="label-success text-white col-md-12 col-sm-12 padding-0">
                                                                    <strong>Income Tax Paid For</strong>&nbsp;<label id="lblIncomeTaxYear"></label></h6>
                                                            </div>

                                                         </div>
                                                     </div>
                                                    </div>
                                                    </div>
                                                <%} %>
                                            </div>
                                            <div class="col-lg-3 col-md-3 ">
                                                <div class="row" id="birthdayRow">
                                                    <div class="col-lg-12 col-md-12  col-sm-6 col-xs-12">
                                                        <div class="widget-header widget-header-flat header-color-pink">
                                                            Birthdays
                                                            <div id="divBdayWidgerToolbar" class="widget-toolbar small-widget-toolbar">
                                                                <a style="float: left; margin-top: -1px !important"><span id="spanBirthdayCount" class="badge badge-pink hide default-cursor"></span>
                                                                </a><a style="float: right; font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-pink"
                                                                    onclick="showFilter(this,setBirthdayWidgetSelectedUserRoleAndView())" data-toggle=""><i class="icon- fa fa-cog"></i></a>
                                                                <ul id="ulBirthdayFilter" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                    <li id="liUser" style="text-align: left; padding-left: 15px !important;">
                                                                        <div id="divUserTitle">
                                                                            Select Users</div>
                                                                        <div class="btn-group" data-toggle="buttons">
                                                                             <%if (moUserRole != Utility.Constants.UserRoles.Student) %>
                                                                             <%{ %>
                                                                            <label class="btn btn-sm btn-purple" data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Teachers">
                                                                                <input type="radio" value="2" />
                                                                                T
                                                                            </label>
                                                                            <%} %>
                                                                            <label class="btn btn-sm btn-primary " data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Students">
                                                                                <input type="radio" value="3" />
                                                                                S
                                                                            </label>
                                                                             <%if (moUserRole != Utility.Constants.UserRoles.Student) %>
                                                                             <%{ %>
                                                                            <label class="btn btn-sm btn-pink" data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Admin Staff">
                                                                                <input type="radio" value="6" />
                                                                                A
                                                                            </label>
                                                                            <label class="btn btn-sm btn-inverse" data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Other Staff">
                                                                                <input type="radio" value="7" />
                                                                                O
                                                                            </label>
                                                                            <%} %>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li id="liBdayView" style="text-align: left; padding-left: 15px !important;">
                                                                        <div id="divViewTitle">
                                                                            Select View</div>
                                                                        <div class="btn-group" data-toggle="buttons">
                                                                            <label class="btn btn-sm active btn-purple" data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Today's Birthday">
                                                                                <input type="radio" value="T" />
                                                                                <i class="icon- fa fa-only bigger-110" data-action="reload"></i>T
                                                                            </label>
                                                                            <label class="btn btn-sm btn-primary" data-placement="bottom" title="" data-rel="tooltip"
                                                                                data-original-title="Birthdays in a week">
                                                                                <input type="radio" value="W" />
                                                                                <i class="icon- fa fa-only bigger-110" data-action="reload"></i>W
                                                                            </label>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li style="text-align: left; padding-left: 5px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadBirthdayWidget(false, true)">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearBdayListFilters()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hideBdayWidgetFilter()">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
														      <a id="hlnkRefreshBirthDayList" class="lnk-refresh pull-right margin-left-right-5 settings-link color-pink"
                                                                    onclick="loadBirthdayWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshBirthDayList', 'BirthdayList')">
                                                                    <i class="icon- fa fa-refresh"></i></a>
                                                            </div>
                                                        </div>
                                                        <div class="widget-body table-responsive" style="padding: 4px 7px 15px; height: 187px !important;">
                                                            <div id="divBirthday" class="div-birthday-box">
                                                                <ul class="bxslider" id="birthdays">
                                                                </ul>
                                                            </div>
														<div id="divBirthdayWidgetMessage">
                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row padding-top-20" id="photoAlbumRow">
                                                    <div class="col-lg-12 col-md-12  col-sm-6 col-xs-12">
                                                        <div class="widget-header widget-header-flat header-color-green-custom">
                                                            Photo Albums
                                                            <div id="divPhotoGalleryWidget" class="widget-toolbar small-widget-toolbar">
                                                                <a style="float: left; margin-top: -1px !important"><span id="spanAlbumCount" class="badge badge-pink default-cursor"></span>
                                                                </a>
                                                                <a style="float: right; font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-green" id="aGallerySetting" onmouseover="SetTooltip('aGallerySetting', 'Click here to display all galleries of selected year, select All option from month and apply filter.')"
                                                                    onclick="showFilter(this,setPhotoGallerySelectedYearAndMonth())"  data-placement="bottom" data-trigger="hover" data-rel="popover" data-content=""><i class="fa fa-bars animate slideUp"></i></a>
                                                                <ul id="ul1" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close">
                                                                    <li id="li1">
                                                                        <div id="divMonthFilterLabel" class="filter-labels">
                                                                            Select Month</div>
                                                                        <div class="padding-left-15">
                                                                            <select id="cmbPhotoGalleryMonth" style="width: 90%;">
                                                                                <option value="0">All</option>
                                                                                <option value="1">January</option>
                                                                                <option value="2">February</option>
                                                                                <option value="3">March</option>
                                                                                <option value="4">April</option>
                                                                                <option value="5">May</option>
                                                                                <option value="6">June</option>
                                                                                <option value="7">July</option>
                                                                                <option value="8">August</option>
                                                                                <option value="9">September</option>
                                                                                <option value="10">October</option>
                                                                                <option value="11">November</option>
                                                                                <option value="12">December</option>
                                                                                <option value="100">Recent 5</option>
                                                                            </select>
                                                                        </div>
                                                                    </li>
                                                                    <li id="li2">
                                                                        <div id="divYearFilterLabel" class="filter-labels">
                                                                            Select Year</div>
                                                                        <div class="padding-left-15">
                                                                            <asp:DropDownList ID="cmbPhotoGalleryYear" runat="server" ViewStateMode="Enabled" Width="90%">
                                                                            </asp:DropDownList>
                                                                        </div>
                                                                    </li>
                                                                    <li class="divider"></li>
                                                                    <li style="text-align: left; padding-left: 5px !important;">
                                                                        <div class="divFiterButtons">
                                                                            <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Apply Filter" onclick="loadPhotoGalleryWidget(false, true)">
                                                                                <i class="icon- fa fa fa-check"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Clear Filter" onclick="clearPhotoGalleryFilter()">
                                                                                <i class="icon- fa fa-undo"></i>
                                                                            </button>
                                                                            <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                data-original-title="Cancel" onclick="hidePhotoGalleryFilter(this)">
                                                                                <i class="icon- fa fa-remove"></i>
                                                                            </button>
                                                                        </div>
                                                                    </li>
                                                                </ul>
                                                                  <a id="hlnkRefreshPhotoGallery" class="lnk-refresh pull-right margin-left-right-5 settings-link color-green"
                                                                    onclick="loadPhotoGalleryWidget(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshPhotoGallery', 'PhotoGalleryList')">
                                                                    <i class="icon- fa fa-refresh"></i></a>
															</div>
                                                        </div>
                                                        <div class="widget-body table-responsive" style="padding: 4px 10px; min-height: 305px;"
                                                            id="divPhotoGalleryWidgetContainer">
                                                            <div id="divPhotoGallery">
                                                                <ul id="photoGallery">
                                                                </ul>
                                                            </div>
                                                            <div style="text-align:center; font-family:Times New Roman; font-size:14px; color:Blue; font-weight:bold;" align="center">
                                                                <asp:Label align="center" ID="lblEventNote" runat="server" Text="Please re-login or refresh the widget to see the updates"></asp:Label>
                                                            </div>
															<div id="divPhotoGalleryWidgetMessage">
                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                  <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                      || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                        || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)) %>
                                                <%{ %>
                                                        <div class="row padding-top-20">
                                                    <div class="col-lg-12 col-md-12  col-sm-6 col-xs-12">
                                                        <div id="statistics-widget" class="widget-box widget-box-custom">
                                                            <div class="widget-header widget-header-flat header-color-orange-custom">
                                                                Stats
                                                                <span id="lblStatisticFilter" class="widget-filter-label" style="color:white"></span>
                                                                <div class="widget-toolbar no-border" style="padding: 0 !important;">
                                                                        <div id="divStatWidgetToolbar" class="widget-toolbar small-widget-toolbar" style="right: 0px; top: -10px;">
																		  <a id="hlnkRefreshStats" class="lnk-refresh margin-left-right-5 settings-link color-yellow"
                                                                                onclick="getStatisticsCurrentTabDetailsCount(true, true)" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshStats', 'StudentStatistic')">
                                                                                <i class="icon- fa fa-refresh"></i></a>
                                                                             <a style="font-size: 18px;" class="dropdown-toggle k-search btnWidth settings-link color-yellow"
                                                                                data-toggle="" onclick="showFilter(this),setStatisticsWidgetSelectedYear()"><i class="icon- fa fa-cog"></i></a>
                                                                            <ul id="ulAcademicYearId" class="user-menu pull-right dropdown-menu dropdown-yellow dropdown-caret dropdown-close" style="min-width: 210px;">
                                                                                <li id="liStatFilter" style="text-align: left;">
                                                                                    <div id="divStatFilterTitle" class="filter-labels">
                                                                                        Select Stat View</div>
                                                                                    <div class="btn-group" data-toggle="buttons" style="padding-left: 15px !important; padding-right: 15px !important;">
                                                                                        <label class="btn btn-sm btn-purple" style="font-size: 12px !important;">
                                                                                            <input type="radio" value="1" onchange="showHideAcademicYearFilter(this)"/>
                                                                                            Student
                                                                                        </label>
                                                                                        <label class="btn btn-sm btn-primary " style="font-size: 12px !important;">
                                                                                            <input type="radio" value="2" onchange="showHideAcademicYearFilter(this)"/>
                                                                                            Staff
                                                                                        </label>
                                                                                        <label class="btn btn-sm btn-pink" style="font-size: 12px !important;">
                                                                                            <input type="radio" value="3" onchange="showHideAcademicYearFilter(this)"/>
                                                                                            Library
                                                                                        </label>
                                                                                    </div>
                                                                                </li>
                                                                                <li>
                                                                                    <div id="divAcademicYearLabel" class="filter-labels">
                                                                                        Select Academic Year</div>
                                                                                    <div class="padding-left-15">
                                                                                        <asp:DropDownList ID="cmbAcademicYear" runat="server" ViewStateMode="Enabled" Width="90%">
                                                                                        </asp:DropDownList>
                                                                                    </div>
                                                                                </li>
                                                                                <li class="divider"></li>
                                                                                <li class="" style="text-align: left; padding-left: 5px !important; margin-top: -3px !important;">
                                                                                    <div class="divFiterButtons">
                                                                                        <button type="button" class="btn  btn-success btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                            data-original-title="Apply Filter" onclick="getStatisticsCurrentTabDetailsCount(false, true)">
                                                                                            <i class="icon- fa fa fa-check"></i>
                                                                                        </button>
                                                                                        <button type="button" class="btn btn-warning btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                            data-original-title="Clear Filter" onclick="clearStatisticsWidgetFilter()">
                                                                                            <i class="icon- fa fa-undo"></i>
                                                                                        </button>
                                                                                        <button type="button" class="btn btn-danger btn-sm" data-placement="bottom" data-rel="tooltip"
                                                                                            data-original-title="Cancel" onclick="hideStatisticsWidgetFilter()">
                                                                                            <i class="icon- fa fa-remove"></i>
                                                                                        </button>
                                                                                    </div>
                                                                                </li>
                                                                            </ul>
                                                                        </div>
                                                                </div>
                                                            </div>
                                                            <div class="widget-body table-responsive">
                                                                <div style="display: block;" class="box-body ">
                                                                    <div id="divStatisticTabs" class="tab-content z-index-0">
                                                                        <div class="tab-pane active" id="student">
																			<div id="divStudentView">
                                                                            <div class="row" style="margin: 0">
                                                                                <div class="col-md-12 col-sm-6 columns">
                                                                                </div>
                                                                            </div>
                                                                            <div class="row">
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class="summary-nest info-circle circle-pink" data-rel="tooltip" title="Total number of girls" data-placement="bottom">
                                                                                        <span>Girls</span> <span id="spanGirlsCount" class="badge badge-pink count"></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class=" summary-nest info-circle circle-yellow" data-rel="tooltip" title="Total number of boys" data-placement="bottom">
                                                                                        <span>Boys</span> <span id="spanBoysCount" class="badge badge-warning count"></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class="summary-nest info-circle circle-purple" data-rel="tooltip" title="Total number of students" data-placement="bottom">
                                                                                        <span>Total</span> <span id="spanTotalCount" class="badge badge-purple count"></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class="info-circle circle-blue" data-rel="tooltip" title="Total number of left students" data-placement="bottom">
                                                                                        <span>Left</span> <span id="spanLeftCount" class="badge badge-info count"></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class="summary-nest info-circle circle-red" data-rel="tooltip" title="Total number of newly joined students" data-placement="bottom">
                                                                                        <span>New</span> <span id="spanNewJoinCount" class="badge badge-danger count"></span>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 col-xs-6">
                                                                                    <div class=" summary-nest info-circle circle-green" data-rel="tooltip" title="Total number of RTE Students" data-placement="bottom">
                                                                                        <span>RTE</span> <span id="spanRteCount" class="badge badge-success count"></span>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
																			</div>
                                                                            <div id="divStudentMessage">
                                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                                            </div>
																		</div>
                                                                        <div class="tab-pane animate" id="staff" data-animation="bounceInRight">
																			<div id ="divStaffView">
                                                                            <div class="row" style="margin: 0">
                                                                                <div class="col-md-12 col-sm-6 columns">
                                                                                </div>
                                                                            </div>
                                                                            <div class="row summary-border-top" style="margin: 0">
                                                                                <div class="col-md-6 col-sm-6 columns">
                                                                                    <div class="summary-nest">
                                                                                        <h2 class="text-black">
                                                                                            <span id="spanTeacherCount" class="text-orange count"></span>
                                                                                        </h2>
                                                                                        <p>
                                                                                            Teachers
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 columns summary-border-left">
                                                                                    <div class="summary-nest">
                                                                                        <h2 class="text-black">
                                                                                            <span id="spanAdminCount" class="text-green count"></span>
                                                                                        </h2>
                                                                                        <p>
                                                                                            Admin
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row padding-top-20 summary-border-top" style="margin: 0">
                                                                                <div class="col-md-12 col-sm-12 columns">
                                                                                    <div class="summary-nest summary-pad-nest">
                                                                                        <h2 class="text-black">
                                                                                            <span id="spanOtherCount" class="text-blue count"></span>
                                                                                        </h2>
                                                                                        <p class="text-left">
                                                                                            Other
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="row summary-border-top  padding-top-20" style="margin: 0">
                                                                                <div class="col-md-6 col-sm-6 columns">
                                                                                    <div class="summary-nest summary-pad-nest">
                                                                                        <h2 class="text-black ">
                                                                                            <span id="spanTransportCount" class="text-red count"></span>
                                                                                        </h2>
                                                                                        <p>
                                                                                            Transport
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-6 col-sm-6 columns summary-border-left">
                                                                                    <div class="summary-nest summary-pad-nest">
                                                                                        <h2 class="text-black">
                                                                                            <span id="spanResignedCount" class="text-purple count"></span>
                                                                                        </h2>
                                                                                        <p>
                                                                                            Resigned
                                                                                        </p>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
																			</div>
                                                                            <div id="divStaffMessage">
                                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                                            </div>
																		</div>
                                                                        <div class="tab-pane animate" id="library" data-animation="bounceInRight">
																		<div id="divLibararyView">
                                                                            <div id="divLibraryContent" class="infobox-container" style="padding: 5px;">
                                                                                <div class="infobox infobox-green  ">
                                                                                    <div class="infobox-icon">
                                                                                        <i class="icon- icon- fa fa-reorder"></i>
                                                                                    </div>
                                                                                    <div class="infobox-data">
                                                                                        <span id="spanTotal" class="infobox-data-number count"></span>
                                                                                        <div class="infobox-content">
                                                                                            Total</div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="infobox infobox-blue  ">
                                                                                    <div class="infobox-icon">
                                                                                        <i class="icon- fa fa-plus"></i>
                                                                                    </div>
                                                                                    <div class="infobox-data">
                                                                                        <span id="spanReceived" class="infobox-data-number count"></span>
                                                                                        <div class="infobox-content">
                                                                                            Received</div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="infobox infobox-pink  ">
                                                                                    <div class="infobox-icon">
                                                                                        <i class="icon- fa fa-shopping-cart"></i>
                                                                                    </div>
                                                                                    <div class="infobox-data">
                                                                                        <span id="spanPurchased" class="infobox-data-number count"></span>
                                                                                        <div class="infobox-content">
                                                                                            Purchased</div>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="infobox infobox-red  ">
                                                                                    <div class="infobox-icon">
                                                                                        <i class="icon- fa fa-times-circle"></i>
                                                                                    </div>
                                                                                    <div class="infobox-data">
                                                                                        <span id="sapnLost" class="infobox-data-number count"></span>
                                                                                        <div class="infobox-content">
                                                                                            Lost</div>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div id="divNoLibraryModule" class="library-tab-text">
                                                                            </div>
                                                                        </div>
																		<div id="divLibararyWidgetMessage">
                                                                                <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
																			</div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <%} %>
												<% 
                                                    string adminOrPrincipal = string.Empty;
                                                    if (moUserRole == Utility.Constants.UserRoles.Admin || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                        || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                        || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME))
                                                        adminOrPrincipal = "AdminOrPrincipal";
                                                %>
                                                <div class="row padding-top-20" id="feedbackRow">
                                                    <div class="col-lg-12 col-md-12  col-sm-6 col-xs-12">
                                                        <div id="feedback-widget" class="widget-box widget-box-custom">
                                                            <div class="widget-header widget-header-flat header-color-blue">
                                                                Feedback
                                                                <span class="widget-toolbar no-border"><a id="hlnkRefreshUsersFeedback" style="font-size: 18px; cursor:pointer !important;" class="margin-left-5 margin-right-5 settings-link color-blue"
                                                                        onclick="loadUsersFeedbackWidget(true, true, '<%= adminOrPrincipal %>')" data-placement="bottom" data-trigger="hover" data-rel="popover" data-content="" onmouseover="refreshToolTip('hlnkRefreshUsersFeedback', 'FeedbackList', '<%= adminOrPrincipal %>')"><i class="icon- fa fa-refresh"></i></a></span>

                                                            </div>
                                                            <div class="widget-body table-responsive">
																<div id="divFeedbackWidgetContent">
                                                                <div class="comments" style="overflow: hidden; width: auto; height: 277px; padding: 5px;">
                                                                </div>
                                                               <div id="divShowAllFeedback">
                                                                      <div class="center">
                                                                <%if (moUserRole == Utility.Constants.UserRoles.Admin
                                                                   || (hidPrincipalDesignationId.Value == Utility.Constants.S_PRINCIPAL_DESIGNATION_ID && hidIsMVPSSchool.Value == "N")
                                                                   || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_SUPERVISOR_DESIGNATION_NAME)
                                                                      || (moUserRole == Utility.Constants.UserRoles.Supervisor && msSupervisorDesignationName == Utility.Constants.S_DIRECTOR_DESIGNATION_NAME)) %>
                                                                <%{ %>
                                                                    <i class="icon- fa fa-comments-alt icon- fa fa-2x green"></i><a href="FeedbackDetailsUI.aspx">
                                                                        See all feedbacks <i class="icon- fa fa-arrow-right"></i></a> 
                                                                    <%} %>
                                                                </div>
                                                            </div>
                                                        </div>
														 <div id="divFeedbackWidgetMessage">
                                                                    <%= Utility.Constants.S_ERROR_OCCURED_MESSAGE%>
                                                              </div>
                                                    </div>
                                                    </div>
                                                   </div>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <!-- Table for admin ends here -->
                        </td>
                    </tr>
                </table>
                <!-- Data Insert End Here -->
            </td>
        </tr>
        <tr>
            <td align="center" valign="top">
            </td>
        </tr>
        <tr align="center" runat="server" id="trHeader" visible="false">
            <td align="center">
            </td>
        </tr>
        
        <tr align="center">
            <td align="center">
                <div id="divExpiredSanctionedLeaves" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 860px; height: 380px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
                    background-color: white; z-index:495;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 860px; text-align: right;">
                        <div style="font-size: 12px; width: 370px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="ExpiredSanctionedLeaves" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ExpiredSanctionedLeavesOfStudents%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupExpiredSanctionedLeaves();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 340px; width: 854px; margin-left: 1px" id="Div5">
                        <asp:UpdatePanel ID="updSacLeave" runat="server" >
                            <ContentTemplate>
                                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                    vertical-align: top">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblUpdateSucess" runat="server" ForeColor="Blue" Width="100%" Visible="true"
                                                            EnableViewState="False" CssClass="ClsLabel" Font-Bold="True"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ValidationSummary ID="valSumErrorMsg" runat="server" ViewStateMode="Enabled" ValidationGroup="Save"
                                                            CssClass="ClsLabel" ShowSummary="true" />
                                                        <asp:CustomValidator ID="cst_StartAndEndDate" runat="server" ViewStateMode="Enabled" ClientValidationFunction="cstStartAndEndDate"
                                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cst_StartDateValidation" runat="server" ViewStateMode="Enabled" ClientValidationFunction="cstStartDateValidation"
                                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                                        <asp:CustomValidator ID="cst_EndDateValidation" runat="server" ViewStateMode="Enabled" ClientValidationFunction="cstEndDateValidation"
                                                            Display="None" ValidationGroup="Save"></asp:CustomValidator>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                                <tr>
                                                    <td align="center">
                                                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                            <tr id="trPagerStudentSanctionedLeaves" runat="server" >
                                                                <td align="center">
                                                                    <asp:DataPager ID="DtPgCount" runat="server" ViewStateMode="Enabled" PageSize="2" PagedControlID="lstvwStudentSanctionedLeave">
                                                                        <Fields>
                                                                            <asp:TemplatePagerField>
                                                                                <PagerTemplate>
                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" ID="CurrentPageLabel" Text="<%# Container.StartRowIndex + 1%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblTo" runat="server" viewstatemode="Enabled" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, To%>" />
                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" ID="TotalPagesLabel" Text="<%# (Container.StartRowIndex + Container.PageSize > Container.TotalRowCount)? Container.TotalRowCount : Container.StartRowIndex + Container.PageSize%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblOutOf" runat="server" viewstatemode="Enabled" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, OutOf%>" />
                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>"
                                                                                        CssClass="LblNrmlB" />
                                                                                    <asp:Label ID="lblRecords" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" Text="<%$ Resources:LocalizedResources, Records%>" />
                                                                                    <br />
                                                                                </PagerTemplate>
                                                                            </asp:TemplatePagerField>
                                                                        </Fields>
                                                                    </asp:DataPager>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table align="center" width="100%">
                                                                        <tr id="trLegend" runat="server" align="center">
                                                                            <td align="left">
                                                                                <table id="LegendTable" runat="server" align="left" cellpadding="0" cellspacing="1">
                                                                                    <tr>
                                                                                        <td align="left" width="60px">
                                                                                            <asp:Label ID="lblLegend" runat="server" ViewStateMode="Enabled" class="ClsLblLgnd" Style="border-width: 0px;
                                                                                                font-weight: bold" Text="<%$ Resources:LocalizedResources, Legend%>"></asp:Label>
                                                                                            <span class="colonPadding">:</span>
                                                                                        </td>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblLegendImage" runat="server" BorderStyle="Solid" BorderWidth="1px"
                                                                                                BackColor="#FFCCCC" Height="20px" Width="20px" EnableViewState="False"><img src="../images/spacer.gif" width="20px" height="20px"/></asp:Label>
                                                                                        </td>
                                                                                        <td align="center" id="tblSanctionedLeave" style="padding-left: 10px;" runat="server">
                                                                                            <%--<asp:Label ID="lblLongLeaveExceed" runat="server" Text="<%$ Resources:LocalizedResources,LongLeaveExceeded%>".replace("%maxdays%",<%=Session["MaxLeaveDays"]%>)></asp:Label>--%>
                                                                                            <span class="ClsTextNormal" style="font-weight: bold">Long leave exceeded more than
                                                                                                <%= Session["MaxLeaveDays"] %>
                                                                                                days</span>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>                                                                                
                                                                            </td>
                                                                        </tr>
                                                                        <tr align="center" style="width: 100%">
                                                                            <td align="center" style="width: 100%">
                                                                                <asp:ListView ID="lstvwStudentSanctionedLeave" DataKeyNames="SanctionedLeaveDetailsId,StudentId,UserId"
                                                                                    runat="server" ViewStateMode="Enabled" DataSourceID="ObjDSStudentSanctionedLeaves" OnDataBound="lstvwStudentSanctionedLeave_DataBound"
                                                                                    OnItemDataBound="lstvwStudentSanctionedLeave_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table align="center" width="100%" runat="server" viewstatemode="Enabled" id="tblStaffInfo" style="color: #333333"
                                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" width="9%" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, RegNo%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" width="100px" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, StudentName%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="center" width="70px">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" width="120px" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, StartDate%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" width="120px" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, EndDate%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="center" width="20%">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MobileNumber%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="center" width="60px">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, IsUsed %>"></asp:Label>
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr runat="server" id="itemPlaceholder">
                                                                                            </tr>
                                                                                            <tr class="ClsBorderPager" id="trDataPager">
                                                                                                <td colspan="7">
                                                                                                    <asp:DataPager ID="DtPgDropDown" runat="server" ViewStateMode="Enabled" PagedControlID="lstvwStudentSanctionedLeave"
                                                                                                        PageSize="10">
                                                                                                        <Fields>
                                                                                                            <asp:TemplatePagerField>
                                                                                                                <PagerTemplate>
                                                                                                                    <table width="100%">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <asp:Label ID="MessageLabel" Text="<%$ Resources:LocalizedResources, SelectPage%>"
                                                                                                                                    runat="server" ViewStateMode="Enabled" CssClass="LblNrmlB" />
                                                                                                                                <span class="colonPadding">:</span>
                                                                                                                                <asp:DropDownList ID="ddlCnt" runat="server" ViewStateMode="Enabled" AutoPostBack="true" OnSelectedIndexChanged="cmbPageCnt_SelectedIndexChanged">
                                                                                                                                </asp:DropDownList>
                                                                                                                            </td>
                                                                                                                            <td align="right" class="LblNormal">
                                                                                                                                <asp:Label ID="CurrentPageLabel" runat="server" ViewStateMode="Enabled" CssClass="LblNormal" />
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
                                                                                        <tr id="Tr2" runat="server" class="ClsGridRow" viewstatemode="Enabled" >
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblRegistrationNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RegistrationNo") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="lblClass" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Class") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:TextBox ID="txtStartDate" TabIndex="3" runat="server" ViewStateMode="Enabled" MaxLength="50" CssClass="SmlTxtBox"
                                                                                                    AutoPostBack="False" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                                <rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="Enabled" Control="txtStartDate" Format="dd MMM yyyy"
                                                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start date should not be blank." />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:TextBox ID="txtEndDate" TabIndex="3" runat="server" ViewStateMode="Enabled" MaxLength="50" CssClass="SmlTxtBox"
                                                                                                    AutoPostBack="False" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                                <rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="Enabled" Control="txtEndDate" Format="dd MMM yyyy"
                                                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="End date should not be blank." />
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="lblMobileNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="chkIsCanceled" runat="server" ViewStateMode="Enabled" Enabled="true" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" viewstatemode="Enabled" class="ClsGridAltRow">
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblRegistrationNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RegistrationNo") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="lblClass" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Class") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:TextBox ID="txtStartDate" TabIndex="3" runat="server" ViewStateMode="Enabled" MaxLength="50" CssClass="SmlTxtBox"
                                                                                                    AutoPostBack="False" Text='<%# Eval("StartDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                                <rjs:PopCalendar ID="calStartDate" runat="server" ViewStateMode="Enabled" Control="txtStartDate" Format="dd MMM yyyy"
                                                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Start date should not be blank." />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:TextBox ID="txtEndDate" TabIndex="3" runat="server" viewstatemode="Enabled" MaxLength="50" CssClass="SmlTxtBox"
                                                                                                    AutoPostBack="False" Text='<%# Eval("EndDate","{0:dd-MMM-yyyy}") %>'></asp:TextBox>
                                                                                                <rjs:PopCalendar ID="calEndDate" runat="server" ViewStateMode="Enabled" Control="txtEndDate" Format="dd MMM yyyy"
                                                                                                    ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="End date should not be blank." />
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="lblMobileNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("MobileNumber") %>'></asp:Label>
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:CheckBox ID="chkIsCanceled" runat="server" ViewStateMode="Enabled" Enabled="true" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                    <EmptyDataTemplate>
                                                                                        <tr>
                                                                                            <td class="LblNoRecord" align="center">
                                                                                                <asp:Label ID="lblNoRecordFound" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,NoRecordsFound%>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                        <asp:ObjectDataSource TypeName="BusinessLogic.StudentSanctionedLeavesBL" EnablePaging="True"
                                                                            ID="ObjDSStudentSanctionedLeaves" runat="server" ViewStateMode="Enabled" SelectMethod="GetExpiredStudentSanctionedLeaveDetails"
                                                                            SortParameterName="sortExpression" SelectCountMethod="CountTotalExpiredSanctionedLeaves"
                                                                            EnableCaching="False">
                                                                            <SelectParameters>
                                                                                <asp:SessionParameter Name="aiSchoolId" SessionField="I_SCHOOL_ID" Type="int32" />
                                                                                <asp:SessionParameter Name="aiAcademicYearId" SessionField="S_CURRENT_ACADEMIC_YEAR_ID"
                                                                                    Type="int32" />
                                                                                <asp:SessionParameter Name="aiUserId" SessionField="I_USER_ID" Type="int32" />
                                                                                <asp:Parameter Name="maximumRows" Type="Int32" />
                                                                                <asp:Parameter Name="startRowIndex" Type="Int32" />
                                                                            </SelectParameters>
                                                                        </asp:ObjectDataSource>
                                                                        <asp:HiddenField ID="hidPageNo" runat="server" ViewStateMode="Enabled" />
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="bottom">
                                                        <asp:Button ID="btnSave" runat="server" ViewStateMode="Enabled" ValidationGroup="Save" Text="<%$ Resources:LocalizedResources,Save%>"
                                                            CssClass="ClsBtn" OnClick="btnSave_Click" />
                                                        <asp:Button ID="btnClosePopUp" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                                            CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupExpiredSanctionedLeaves();return false;" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </td>
        </tr>
	    <tr align="center" id="trMissingAttendancePoppup" runat="server">
            <td align="center">
                <div id="divAttendanceAlert" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 600px; height: 380px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
                    background-color: white; z-index:499;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 595px; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MissingAttendanceAlert%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupAttendance();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 340px; width: 580px; margin-left: 1px" id="Div10">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                    vertical-align: top">
                                    <tr>
                                        <td>
                                            <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                                <tr>
                                                    <td align="center">
                                                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <td align="left" style="padding-left: 5px; font-weight: bold">
                                                                            <asp:Label ID="Label31" Style="width: 100%;" runat="server" BorderWidth="0px" Text="<%$ Resources:LocalizedResources, ThisIsTheClassWiseMissingAttendanceList%>"
                                                                                CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table align="center" width="100%">
                                                                        <tr align="center" style="width: 100%">
                                                                            <td align="center" style="width: 100%">
                                                                                <asp:ListView ID="lstvwAttendanceDetails" DataKeyNames="StandardDivisionId" runat="server" ViewStateMode="Enabled" 
                                                                                    OnItemCommand="lstvwAttendanceDetails_ItemCommand" OnItemDataBound="lstvwAttendanceDetails_ItemDataBound">
                                                                                    <LayoutTemplate>
                                                                                        <table align="center" width="100%" runat="server" id="tblAttendnaceDetails" style="color: #333333"
                                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ClassTeacherName%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" class="paddingL">
                                                                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MissingDays%>"></asp:Label>
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr runat="server" id="itemPlaceholder">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("TeacherName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblClassName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("ClassName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:LinkButton ID="lnkBtnDetails" Text='<%# Eval("MissingCount") %>' runat="server" ViewStateMode="Enabled" 
                                                                                                    CommandName="DETAILS" CommandArgument="StandardDivisionId" ForeColor="Blue"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trDateDetails" runat="server" viewstatemode="Enabled" visible="false" align="center">
                                                                                            <td id="tdDateDetails" runat="server" align="center" colspan="3" viewstatemode="Enabled">
                                                                                                <table width="35%">
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:ListView ID="lstvwDateDetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwDateDetails_ItemDataBound">
                                                                                                                <LayoutTemplate>
                                                                                                                    <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                                                                        cellspacing="1" class="GridBorder" align="center">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <table width="100%" runat="server" id="tblDates" style="color: #333333" cellpadding="0"
                                                                                                                                    cellspacing="1">
                                                                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                                                        <th align="center">
                                                                                                                                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MissingAttendanceDates%>"></asp:Label>
                                                                                                                                        </th>
                                                                                                                                    </tr>
                                                                                                                                    <tr id="itemPlaceholder" runat="server" >
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </LayoutTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <tr id="Tr2" runat="server" class="ClsGridRow" align="center">
                                                                                                                        <td align="center" style="text-align: center">
                                                                                                                            <asp:Label ID="lblDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Date") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </ItemTemplate>
                                                                                                                <AlternatingItemTemplate>
                                                                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow" align="center">
                                                                                                                        <td align="center" style="text-align: center">
                                                                                                                            <asp:Label ID="lblDate" ViewStateMode="Enabled" runat="server" Text='<%# Eval("Date") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </AlternatingItemTemplate>
                                                                                                            </asp:ListView>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:Button CssClass="ClsBtn margin-bottom-2" ID="BtnCancelDates" CausesValidation="false" runat="server" ViewStateMode="Enabled" 
                                                                                                                Text="<%$ Resources:LocalizedResources,Cancel%>" BorderWidth="1px" OnClick="BtnCancelDates_Click">
                                                                                                            </asp:Button>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("TeacherName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblClassName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("ClassName") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:LinkButton ID="lnkBtnDetails" Text='<%# Eval("MissingCount") %>' runat="server" ViewStateMode="Enabled" 
                                                                                                    CommandName="DETAILS" CommandArgument="StandardDivisionId" ForeColor="Blue"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr id="trDateDetails" runat="server" visible="false" align="center" viewstatemode="Enabled" >
                                                                                            <td id="tdDateDetails" runat="server" colspan="3" align="center" viewstatemode="Enabled" >
                                                                                                <table width="35%">
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:ListView ID="lstvwDateDetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwDateDetails_ItemDataBound">
                                                                                                                <LayoutTemplate>
                                                                                                                    <table width="100%" runat="server" id="tblRange" style="color: #333333" cellpadding="0"
                                                                                                                        cellspacing="1" class="GridBorder" align="center">
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                                <table width="100%" runat="server" id="tblDates" style="color: #333333" cellpadding="0"
                                                                                                                                    cellspacing="1">
                                                                                                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                                                        <th align="center">
                                                                                                                                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MissingAttendanceDates%>"></asp:Label>
                                                                                                                                        </th>
                                                                                                                                    </tr>
                                                                                                                                    <tr id="itemPlaceholder" runat="server" >
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </LayoutTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <tr id="Tr2" runat="server" class="ClsGridRow" align="center">
                                                                                                                        <td align="center" class="ClspaddingL" style="text-align: center">
                                                                                                                            <asp:Label ID="lblDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Date") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </ItemTemplate>
                                                                                                                <AlternatingItemTemplate>
                                                                                                                    <tr id="Tr3" runat="server" class="ClsGridAltRow" align="center">
                                                                                                                        <td align="center" class="ClspaddingL" style="text-align: center">
                                                                                                                            <asp:Label ID="lblDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Date") %>' />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </AlternatingItemTemplate>
                                                                                                            </asp:ListView>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:Button CssClass="ClsBtn margin-bottom-2" ID="BtnCancelDates" CausesValidation="false" runat="server" ViewStateMode="Enabled" 
                                                                                                                Text="<%$ Resources:LocalizedResources,Cancel%>" BorderWidth="1px" OnClick="BtnCancelDates_Click">
                                                                                                            </asp:Button>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </AlternatingItemTemplate>
                                                                                    <EmptyDataTemplate>
                                                                                        <tr>
                                                                                            <td class="LblNoRecord" align="center">
                                                                                                <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,NoRecordsFound%>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" valign="bottom">
                                                        <asp:Button ID="btnCloseAttendance" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                                            CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupAttendance()" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                    
                </div>
            </td>
        </tr> 
        
        
        
        <tr align="center" id="reRetirementPopup" runat="server">
            <td align="center">
                <div id="divRetirementAlert" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 800px; border-width: 0px;max-height:400px; 
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
                    background-color: white; z-index:499;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 595px; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label73" runat="server" ViewStateMode="Enabled" Text="Retirement Notice Alert"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideRetirementPopup();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; max-height: 360px; margin-left: 1px" id="Div19">                       
                                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                    vertical-align: top">
                                    <tr>
                                        <td>
                                            <asp:ListView ID="lstvwRetirementDetails" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwRetirementDetails_ItemDataBound">
                                                <LayoutTemplate>
                                                <table align="center" width="100%" runat="server" id="tblStaffInfo" style="color: #333333;"
                                                        cellpadding="0" cellspacing="1" class="GridBorder">
                                                    <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th style="width:100px">
                                                        Sr. No.
                                                    </th>
                                                        <th align="left" class="paddingL">
                                                        Name (Designation)
                                                        </th>
                                                        <th style="width:150px">
                                                            Retirement Date
                                                        </th>
                                                        <th style="width:150px">
                                                            Remaining Days
                                                        </th>
                                                    </tr>
                                                    <tr id="itemPlaceholder" runat="server">
                                                    </tr>
                                                </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                <tr id="trGridRow" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                    <td align="center">
                                                    <asp:Label ID="lblSrNo" runat="server" ViewStateMode="Enabled"></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                    <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRetirementDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RetirementDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                    <td align="center" class="paddingL">
                                                        <asp:Label ID="lblDays" runat="server" ViewStateMode="Enabled" CssClass="clsLabelC" Text='<%# Eval("RemainingDays") %>'></asp:Label>
                                                    </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                <tr id="trGridRow" runat="server" viewstatemode="Enabled" class="ClsGridAltRow">
                                                    <td align="center">
                                                        <asp:Label ID="lblSrNo" runat="server" ViewStateMode="Enabled"></asp:Label>
                                                    </td>
                                                    <td align="left" class="paddingL">
                                                        <asp:Label ID="lblName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Name") %>'></asp:Label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:Label ID="lblRetirementDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RetirementDate","{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                    </td>
                                                        <td align="center" class="paddingL">
                                                        <asp:Label ID="lblDays" runat="server" ViewStateMode="Enabled" CssClass="clsLabelC" Text='<%# Eval("RemainingDays") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                                <EmptyDataTemplate>
                                                <table width="740px" align="center">
                                                    <tr>
                                                    <td class="LblNoRecord" style="text-align: center">
                                                    <span>No record found.</span>
                                                    </td>
                                                    </tr>
                                                </table>
                                                </EmptyDataTemplate>
                                        </asp:ListView>
                                        </td>
                                    </tr>
                                </table>                            
                    </div>                    
                </div>
            </td>
        </tr> 


               
        <%--<tr align="center">
            <td align="center">
                <div id="divThankingletter" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 40%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:600;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="divThankingletterImage" runat="server" viewstatemode="Enabled">                            
                            <span style="cursor: hand" onclick="javascript:HideThankingletterPopup();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/ArnavAprriciationLetter.jpg" id="imgFeeStructure" style="width:100%;height:500px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>

        <%--<tr align="center">
            <td align="center">
                <div id="divOmAppreciationLetter" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 40%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:614;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div13" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label64" runat="server" ViewStateMode="Enabled" Text="Appreciation Letter"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideOmAppreciationletterPopup();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/App Letter.PNG" id="imgFeeStructure1" style="width:100%;height:800px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>
     <%--   <tr align="center">
            <td align="center">
                <div id="divAppLetter2" runat="server" viewstatemode="Enabled" style="visibility:hidden;position: absolute; margin: 0px; padding: 0px; width: 700px; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color: white; z-index:1000;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div12" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label65" runat="server" ViewStateMode="Enabled" Text="Appreciation Letter"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideOmAppreciationletter2();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/Appreciation_Letter.png" id="img2" style="width:100%;height:800px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>
        <tr align="center">
            <td align="center">
                <div id="divflashNotice" runat="server" viewstatemode="Enabled" style="visibility:hidden;position: absolute; margin: 0px; padding: 0px; width: 800; height:400; border-width: 0px;
                    left: 5px; top: 0px;line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color: white; z-index:1100;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div14" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label64" runat="server" ViewStateMode="Enabled" Text="Notice"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HidedivflashNotice();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/PPSNNotice.jpg" id="img3" style="width:100%;height:400;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>
         <%--<tr align="center">
            <td align="center">
                <div id="DivFlashInvitationPrePrimary" runat="server" viewstatemode="Enabled" style="visibility:hidden;position: absolute; margin: 0px; padding: 0px; width: 800px; height:400; border-width: 0px;
                    left: 5px; top: 0px;line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color: white; z-index:1200;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div15" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label66" runat="server" ViewStateMode="Enabled" Width="500px" Text="Annual Day Invitation - Class Preprimary to 2nd."></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideflashInvitationPrePrimary();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/Pre-PrimaryToClass2.jpg" id="img4" style="width:100%;height:350;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>
         <%--<tr align="center">
            <td align="center">
                <div id="DivFlashInvitationPrimary" runat="server" viewstatemode="Enabled" style="visibility:hidden;position: absolute; margin: 0px; padding: 0px; width: 800px; height:400; border-width: 0px;
                    left: 5px; top: 0px;line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color: white; z-index:1100;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div17" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label67" runat="server" ViewStateMode="Enabled" Width="500px" Text="Annual Day Invitation - Class 3rd to 9th."></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideflashInvitationPrimary();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/AnnualDayClass3to9.jpg" id="img5" style="width:100%;height:350;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>
         <tr align="center">
            <td align="center">
                <div id="divInVitationVideo" runat="server" viewstatemode="Enabled" style="position: absolute; margin: 10px; padding: 0px; width: 500px; height:auto; border-width: 0px;
                    left: 200px; top: 300px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color:White; z-index:1400;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div11" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label28" runat="server" ViewStateMode="Enabled" Text="Invitation Video"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideInVitationVideo();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                     </div>
                     <div>
                        <a href="../DOWNLOADS/Admission.jpg" style="font-family:Cambria; font-size:15px;" target="_blank"><b>Admission Open For 2020-2021.</b></a>
                    </div>
                    <div style="width:100%; height:5px;"></div>
                    <div>
                        <a href="../DOWNLOADS/Carnival.jpg" style="font-family:Cambria; font-size:15px;" target="_blank"><b>JPS Kids Carnival.</b></a>
                    </div>
                    <div style="width:100%; height:5px;"></div>
                    <div style="background-color: #F9E6FF;">
                    <div style="width:100%; height:20px;"></div>
                       <video width="100%" height="200px" controls autoplay controls="controls">
                              <source src="../DOWNLOADS/Video/Carnival.mp4" type="video/mp4">  
                              Your browser does not support the video tag.
                       </video>                      
                       </div>
                    <div style="margin-bottom : 15px; margin-top : 15px;">
                        <span style="font-family:Cambria; font-size:22;"><b>आपल्या जयवंत पब्लिक स्कूल चे वार्षिक स्नेहसंमेलन मंगळवार दि २४-१२-२०१९ रोजी संध्या. ४ ते ८ या वेळात आयोजित केले आहे तरी आपली उपस्थिती प्रार्थनीय आहे.</b></span>
                    </div>
                </div>
           </td>
        </tr>

        <tr align="center">
            <td align="center">
                <div id="divCeremonyVideo" runat="server" viewstatemode="Enabled" style="position: absolute; margin: 10px; padding: 0px; width: 600px; height:auto; border-width: 0px;
                    left: 200px; top: 300px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color:White; z-index:1400;display:none;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div17" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label68" runat="server" ViewStateMode="Enabled" Text="Investiture Ceremony 2023-24"></asp:Label>
                        </div>                         
                            <span style="cursor: hand">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                     </div>                    
                 <%--   <div style="width:100%; height:5px;"></div>                   
                    <div style="width:100%; height:5px;"></div>--%>
                    <div style="background-color: #F9E6FF;">
                    <div style="width:100%; height:20px;"></div>
                       <video width="100%" height="300px" controls controls="controls">
                             
                             
                              Your browser does not support the video tag.
                       </video>                      
                    </div>                   
                </div>
           </td>
        </tr>

        <tr align="center">
            <td align="center">
                <div id="divAnnualDayInvitationVideo" runat="server" viewstatemode="Enabled" style="position: absolute; margin: 10px; padding: 0px; width: 400px; height:auto; border-width: 0px;
                    left: 200px; top: 200px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color:White; z-index:1400;display:none;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div18" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label70" runat="server" ViewStateMode="Enabled" Text="Annual Day 2023-24 Invitation"></asp:Label>
                        </div>                         
                            <span style="cursor: hand">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                     </div>                
                    <div style="background-color: #F9E6FF;">                  
                       <video width="100%" height="500px" autoplay controls controls="controls">                             
                             
                              Your browser does not support the video tag.
                       </video>                      
                    </div>                   
                </div>
           </td>
        </tr>

        <tr align="center">
            <td align="center">
                <div id="divPPSHChildrenDay" runat="server" viewstatemode="Enabled" style="position: absolute; margin: 10px; padding: 0px; width: 600px; height:auto; border-width: 0px;
                    left: 200px; top: 300px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color:White; z-index:1400;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div16" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label67" runat="server" ViewStateMode="Enabled" Text="Happy Children's Day"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideChildrenDayVideo();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                     </div>                     
                    <div style="width:100%; height:5px;"></div>
                    <div style="background-color: #F9E6FF;">
                    <div style="width:100%; height:20px;"></div>
                       <video width="100%" height="400px" controls autoplay controls="controls">
                              <source src="../DOWNLOADS/Video/Happy Children's Day.mp4" type="video/mp4">  
                              Your browser does not support the video tag.
                       </video>                      
                       </div>                    
                </div>
           </td>
        </tr>

         <%--<tr align="center">
            <td align="center">
                <div id="divAdmission" runat="server" viewstatemode="Enabled" style="position: absolute; margin: 0px; padding: 0px; width: 700px; height:auto; border-width: 0px;
                    left: 300px; top:200px; line-height: normal; border: solid 2px darkgreen; margin: -30px 0px 0px 50px;
                    background-color: white; z-index:1500;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div13" runat="server" viewstatemode="Enabled">   
                             <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label63" runat="server" ViewStateMode="Enabled" Text="Admission Notice"></asp:Label>
                        </div>                         
                            <span style="cursor: hand" onclick="javascript:HideAdmission();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    
                    <img src="~/RITeSchool/images/AdmissionPioneer.jpg" id="img1" style="width:100%;height:400px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>

       <%-- <tr align="center">
            <td align="center">
                <div id="divAppreCiation" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 50%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:615;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div11" runat="server" viewstatemode="Enabled">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label63" runat="server" ViewStateMode="Enabled" Text="Appreciation Letter"></asp:Label>
                        </div>                              
                            <span style="cursor: hand" onclick="javascript:HideAppreCiationDiv();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/Prappreciation.jpg" id="img1" style="width:90%;height:700px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>
          <tr align="center">
            <td align="center">
                <div id="divThanksNote" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 40%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:1100;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div14" runat="server" viewstatemode="Enabled">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label66" runat="server" ViewStateMode="Enabled" Text="ThanksNote"></asp:Label>
                        </div>                              
                            <span style="cursor: hand" onclick="javascript:HideThanksNoteDiv();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <img src="~/RITeSchool/images/Thanks.jpg" id="img3" style="width:100%;height:700px;" runat="server" viewstatemode="Enabled" />
                </div>
           </td>
        </tr>--%>
          <%--<tr align="center">
            <td align="center">
                <div id="DivStdXResult" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 50%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:1200;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div15" runat="server" viewstatemode="Enabled">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label67" runat="server" ViewStateMode="Enabled" Text="Std X Result"></asp:Label>
                        </div>                              
                            <span style="cursor: hand" onclick="javascript:HideStdXResultDiv();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />                               
                            </span>
                        </div>
                    <div style="margin-left:10px;margin-right:10px;margin-bottom:8px;">
                        <img src="~/RITeSchool/images/StdXResult.jpg" id="img4" style="width:100%;height:700px;" runat="server" viewstatemode="Enabled" />
                    </div>
                </div>
           </td>
        </tr>--%>
        <tr align="center">
            <td align="center">                                         
                <div id="divParentFeedback" runat="server" viewstatemode="Enabled" style="visibility: hidden;display:none;
                    position: absolute; margin: 0px; padding: 0px; width: 50%; height: auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px;
                    background-color: white; z-index:499;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                         background-repeat: repeat-x; color: Black; width: 100%; text-align: right;" id="div9" runat="server" viewstatemode="Enabled">                            
                         <span style="cursor: hand" onclick="javascript:HideParentFeedbackPopup();">
                             <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                 border="0" />
                            
                         </span>
                    </div>                    
                    <table align="center" width="100%" style="width:100%; height:100%;">
                        <tr align="left">
                            <td style="font-family:Cambria; font-size:18px; font-weight:bold; padding-left:5px; padding-right:10px; text-align:justify">
                                Dr. Anjali Gujar<br />
                                Principal <br />
                                Pawar Public School, <br />
                                Nanded City Pune
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                Dear Madam <br />
                                Greeting of the day and wishing you happy Gudhi Padawa
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                This is Prof Sandeep G Thorat and Prof Alaka S Thorat, parents of Master Omkar S Thorat from 5th D Div.
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                Madam, my son got his admission in your esteem institute in 5th standard i.e. this year only. This year my son has grown up into an honest and hardworking student, 
                                I have to thank for it to PPS and faculty members teaching to Omkar. One to one attention that Omkar received from your teachers has made him confident in his ability 
                                and sparked his interest in many different subjects. We are especially thankful to his class teacher, Prof Jayshree Madam for her support and time to time guidance.
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                More than anything else we would like to thank you for the fun that Omkar has during past year at PPS.
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                The day before yesterday, (15th March) you visited my son’s class and gave him his prize which he won in the test taken by Byju’s Study circle. 
                                This may be a small thing but it’s definitely motivating to my son for which he will be always obliged to you. We are very much thankful to you for such motivating activities.
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr>
                            <td style="font-family:Cambria; font-size:18px; padding-left:5px; padding-right:10px; text-align:justify">
                                We must appreciate to you and your faculty for giving such one to one attention and giving motivation to the student like my son
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                        <tr align="left">
                            <td style="font-family:Cambria; font-size:18px; font-weight:bold; padding-right:10px; padding-left:5px; text-align:justify">
                                Best Regards<br />
                                Prof Sandeep G Thorat <br />
                                Prof Alaka S Thorat
                            </td>
                        </tr>
                        <tr style="height:5px;">
                            <td></td>
                        </tr>
                    </table>
                </div>                
           </td>
        </tr>
        <tr align ="center">
            <td align = "center">
                <div id="divFeeNotice" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: auto; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 50px 0px 0px 50px; 
                    background-color: white; z-index:615; display: inline-block;">
                         <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 50%; height:auto; text-align: right;" id="divNoticeImage" runat="server" viewstatemode="Enabled">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                <asp:Label ID="lblFeeStructure" runat="server" Text="Fee Structure Notice of AY 2025-26"></asp:Label>
                            </div>
                            <span style="cursor: hand" onclick="javascript:HideFeeNoticeDiv();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" 
                                    border="0" />
                               
                            </span>
                        </div>   
                    <%--<img src="../images/Fee%20Structure%202024-2025.png" id="imgFeeStructure2019" style="width:100%;height:600px;" runat="server" viewstatemode="Enabled" onclick="OpenFeeStructure()" />--%>
                    <img src="../images/PPSNFeeStructure2025-26.png" style="display: block; width: auto; height: 450px;" runat="server" viewstatemode="Enabled" onclick="OpenFeeStructure()" />
                   <%-- <img src="../images/new%20fee%20structure%20for%20ppsn.jpg" id="imgFeeStructure2022-23" style="width:100%;height:600px;" runat="server" viewstatemode="Enabled" onclick="OpenFeeStructure()" />--%>
                </div>
            </td>
        </tr>        

       <%-- <tr align ="center">
            <td align = "center">
                <div id="divPPSNResult" runat="server" viewstatemode="Enabled" style="visibility: hidden;
                    position: absolute; margin: 0px; padding: 0px; width: 60%; height:auto; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 120px 0px 0px 350px;
                    background-color: white; z-index:610;">
                         <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 50%; height:auto; text-align: right;" id="div12" runat="server" viewstatemode="Enabled">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                <asp:Label ID="Label67" runat="server" Text="STD.X Result"></asp:Label>
                            </div>
                            <span style="cursor: hand" onclick="javascript:HideResultDiv();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif" 
                                    border="0" />
                               
                            </span>
                        </div>                                                                                                                                                                                                                                                                                          
                    <img src="~/RITeSchool/images/Pawar_Public_School,Result.jpg" id="img2" style="width:100%;height:450px;" runat="server" viewstatemode="Enabled" onclick="OpenResult()" />
                </div>
            </td>
        </tr>        --%>

        

        <tr align="center">
            <td align="center">
                <div id="divMissingAttendancePopup" runat="server" viewstatemode="Enabled" style="position: fixed; display:none; margin: 0px; padding: 0px; width: 600px; height: 380px; border-width: 0px;
                    left: 500px; top: 400px; line-height: normal; border: solid 2px darkgreen; margin: -110px 0px 0px 00px;
                    background-color: white; z-index:499;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 595px; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label54" runat="server" ViewStateMode="Enabled" Text="Absent Student Details"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideMissingAttendancePopup();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 340px; width: 580px; margin-left: 1px" id="Div7">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                    vertical-align: top">
                                    <tr>
                                        <td>
                                            <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                                <tr>
                                                    <td align="center">
                                                        <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <td align="left" style="padding-left: 5px; font-weight: bold">
                                                                            <asp:Label ID="lblAbsentHeader" Style="width: 100%;" runat="server" BorderWidth="0px"
                                                                                CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                                                        </td>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table align="center" width="100%">
                                                                        <tr align="center" style="width: 100%">
                                                                            <td align="center" style="width: 100%">
                                                                                <asp:ListView ID="lstvwMissingAttendance" DataKeyNames="" runat="server" ViewStateMode="Enabled">
                                                                                    <LayoutTemplate>
                                                                                        <table align="center" width="100%" runat="server" id="tblAttendnaceDetails" style="color: #333333"
                                                                                            cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                            <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                <th align="left" class="paddingL">
                                                                                                    <asp:Label ID="Label60" runat="server" ViewStateMode="Enabled" Text="Enrolment No."></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    <asp:Label ID="Label58" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Class%>"></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" class="paddingL">
                                                                                                    <asp:Label ID="Label59" runat="server" ViewStateMode="Enabled" Text="Roll No."></asp:Label>
                                                                                                </th>
                                                                                                <th align="left" style="padding-left: 10px;">
                                                                                                    <asp:Label ID="Label57" runat="server" ViewStateMode="Enabled" Text="Student Name"></asp:Label>
                                                                                                </th>
                                                                                            </tr>
                                                                                            <tr runat="server" id="itemPlaceholder">
                                                                                            </tr>
                                                                                        </table>
                                                                                    </LayoutTemplate>
                                                                                    <ItemTemplate>
                                                                                        <tr id="Tr2" runat="server" viewstatemode="Enabled" class="ClsGridRow">
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblEnrolmentNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("EnrolmentNumber") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblClassName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("className") %>' />
                                                                                            </td>
                                                                                             <td align="center">
                                                                                                <asp:Label ID="lblRollNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RollNo") %>' />
                                                                                            </td>
                                                                                             <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblStudentName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>' />
                                                                                            </td>                                                                                            
                                                                                        </tr>                                                                                        
                                                                                    </ItemTemplate>
                                                                                    <AlternatingItemTemplate>
                                                                                        <tr id="Tr3" runat="server" class="ClsGridAltRow">
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblEnrolmentNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("EnrolmentNumber") %>' />
                                                                                            </td>
                                                                                            <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblClassName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("className") %>' />
                                                                                            </td>
                                                                                             <td align="center">
                                                                                                <asp:Label ID="lblRollNo" runat="server" ViewStateMode="Enabled" Text='<%# Eval("RollNo") %>' />
                                                                                            </td>
                                                                                             <td align="left" class="paddingL">
                                                                                                <asp:Label ID="lblStudentName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>' />
                                                                                            </td>
                                                                                        </tr>                                                                                        
                                                                                    </AlternatingItemTemplate>
                                                                                    <EmptyDataTemplate>
                                                                                        <tr>
                                                                                            <td class="LblNoRecord" align="center">
                                                                                                <asp:Label ID="Label62" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,NoRecordsFound%>"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </EmptyDataTemplate>
                                                                                </asp:ListView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>                                                
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>                    
                </div>
            </td>
        </tr>
        <tr align="center" id="trNonPermenantTeachers" runat="server" viewstatemode="Enabled" >
            <td align="center">
                <div id="divNonPermenantTeacher" runat="server" viewstatemode="Enabled" style="visibility:visible; position: absolute; margin: 0px; padding: 0px; width: 540px; height: 320px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 220px 0px 0px 406px;
                    background-color: White; z-index:499;" align="center">
                        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 635px; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label2" runat="server" Text="Non Permanent Teacher Details"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideTeacherDetails();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 280px; width: 520px; margin-left: 1px" id="Div10"> 
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>                       
                                 <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                     vertical-align: top">
                                     <tr>
                                         <td>
                                             <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                                 <tr>
                                                     <td align="center">
                                                         <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                             <tr>
                                                                 <td>
                                                                     <table>
                                                                         <td align="left" style="padding-left: 5px; font-weight: bold">
                                                                             <asp:Label ID="Label30" Style="width: 100%;" runat="server" BorderWidth="0px" Text="This is the Non Permanent Teachers list whose joining date is gretter than 1 Year."
                                                                                 CssClass="LblSmlV" EnableViewState="False"></asp:Label>
                                                                         </td>
                                                                     </table>
                                                                 </td>
                                                             </tr>
                                                             <tr>
                                                                 <td>
                                                                     <table align="center" width="100%">
                                                                         <tr align="center" style="width: 100%">
                                                                             <td align="center" style="width: 100%">
                                                                                 <asp:ListView ID="lstvwNonPermanantTeachers" DataKeyNames="UserId" runat="server" ViewStateMode="Enabled" OnItemDataBound="lstvwNonPermanantTeachers_ItemDataBound">
                                                                                     <LayoutTemplate>
                                                                                         <table align="center" width="100%" runat="server" id="tblAttendnaceDetails" style="color: #333333"
                                                                                             cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                             <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                 <th align="left" style="padding-left: 10px;">
                                                                                                     <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" Text="Teacher Name"></asp:Label>
                                                                                                 </th>
                                                                                                 <th align="left" style="padding-left: 10px; width:30%;">
                                                                                                     <asp:Label ID="lblJoiningDate" runat="server" ViewStateMode="Enabled" Text="Joining Date"></asp:Label>
                                                                                                 </th>                                                                                                
                                                                                             </tr>
                                                                                             <tr runat="server" id="itemPlaceholder" >
                                                                                             </tr>
                                                                                         </table>
                                                                                     </LayoutTemplate>
                                                                                     <ItemTemplate>
                                                                                         <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                             <td align="left" class="paddingL">
                                                                                                 <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("TeacherName") %>' />
                                                                                             </td>
                                                                                             <td align="left" class="paddingL">
                                                                                                 <asp:Label ID="lblJoiningDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("JoiningDate") %>' />
                                                                                             </td>                                                                                            
                                                                                         </tr>                                                                                        
                                                                                     </ItemTemplate>
                                                                                     <AlternatingItemTemplate>
                                                                                         <tr id="Tr3" runat="server" class="ClsGridAltRow" viewstatemode="Enabled">
                                                                                             <td align="left" class="paddingL">
                                                                                                 <asp:Label ID="lblTeacherName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("TeacherName") %>' />
                                                                                             </td>
                                                                                             <td align="left" class="paddingL">
                                                                                                 <asp:Label ID="lblJoiningDate" runat="server" ViewStateMode="Enabled" Text='<%# Eval("JoiningDate") %>' />
                                                                                             </td>                                                                                           
                                                                                         </tr>                                                                                        
                                                                                     </AlternatingItemTemplate>
                                                                                     <EmptyDataTemplate>
                                                                                         <tr>
                                                                                             <td class="LblNoRecord" align="center">
                                                                                                 <asp:Label ID="Label56" runat="server" Text="<%$ Resources:LocalizedResources,NoRecordsFound%>"></asp:Label>
                                                                                             </td>
                                                                                         </tr>
                                                                                     </EmptyDataTemplate>
                                                                                 </asp:ListView>
                                                                             </td>
                                                                         </tr>                                                                 
                                                                     </table>
                                                                 </td>
                                                             </tr>
                                                         </table>
                                                     </td>
                                                 </tr>
                                                 <tr>
                                                     <td align="center" valign="bottom">
                                                         <asp:Button ID="Button1" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                                             CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HideTeacherDetails()" />
                                                     </td>
                                                 </tr>
                                             </table>
                                         </td>
                                     </tr>
                                 </table>  
                              </ContentTemplate>
                        </asp:UpdatePanel>                      
                    </div>
                </div>
            </td>
        </tr>  
        
        
        
        <tr align="center" id="trPaymentClearance" runat="server" viewstatemode="Enabled" visible='false'>
            <td align="center">
                <div id="div12" runat="server" viewstatemode="Enabled" style="visibility:visible; position: absolute; margin: 0px; padding: 0px; width: 650px; height: 320px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 220px 0px 0px 406px;
                    background-color: White; z-index:499;" align="center">
                        <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 635px; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label69" runat="server" Text="Payment Clearnace Notification"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideClearancePayment();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: white; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 280px; width: 630px; margin-left: 1px" id="Div15"> 
                                    
                                 <table align="center" border="0" cellpadding="0" cellspacing="1" style="width: 100%;
                                     vertical-align: top">
                                     <tr>
                                         <td>
                                             <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                                                 <tr>
                                                     <td align="center">
                                                         <table border="0" cellpadding="0" cellspacing="2" style="height: 100%; width: 100%;">
                                                             <tr>
                                                                 <td>
                                                                     <table align="center" width="100%">
                                                                         <tr align="center" style="width: 100%">
                                                                             <td align="center" style="width: 100%">
                                                                                 <asp:ListView ID="lstpaymentNotification"  runat="server" ViewStateMode="Enabled"  >
                                                                                     <LayoutTemplate>
                                                                                         <table align="center" width="100%" runat="server" id="tblAttendnaceDetails" style="color: #333333"
                                                                                             cellpadding="0" cellspacing="1" class="GridBorder">
                                                                                             <tr id="trHeader" runat="server" class="ClsGridHeader">
                                                                                                 <th align="left" width="120px" class="paddingL">
                                                                                              
                                                                                                  Enrolment No
                                                                                            </th>                                       
                                                                                            <th align="left" width="200px" style="padding-right:5px;">
                                                                                               Student Name
                                                                                            </th>
                                                                                            <th align="left" width="120px" style="padding-right:5px;">
                                                                                                 ClassName
                                                                                            </th>    
                                                                                             
                                                                                               <th align="right" width="120px" style="padding-right:5px;">
                                                                                               Payment Mode
                                                                                            </th> 
                                                                                              <th align="right" width="120px" style="padding-right:5px;">
                                                                                               Payment Date
                                                                                            </th>   
                                                                                                                                                                                               
                                                                                             </tr>
                                                                                             <tr runat="server" id="itemPlaceholder" >
                                                                                             </tr>
                                                                                         </table>
                                                                                     </LayoutTemplate>
                                                                                     <ItemTemplate>
                                                                                         <tr id="Tr2" runat="server" class="ClsGridRow">
                                                                                             <td align="left" class="paddingL">
                                                                                                    <asp:Label ID="lblEnrolmentno" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Enrolment_Number") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblStudentName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblclassname" runat="server" ViewStateMode="Enabled" Text='<%# Eval("className") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblpaymendMode" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Mode") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblpaymentdate" runat="server" ViewStateMode="Enabled" Text='<%#Eval("PaidDate" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                                                </td>                                                                                          
                                                                                         </tr>                                                                                        
                                                                                     </ItemTemplate>
                                                                                     <AlternatingItemTemplate>
                                                                                         <tr id="Tr3" runat="server" class="ClsGridAltRow" viewstatemode="Enabled">
                                                                                           <td align="left" class="paddingL">
                                                                                                    <asp:Label ID="lblEnrolmentno" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Enrolment_Number") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblStudentName" runat="server" ViewStateMode="Enabled" Text='<%# Eval("StudentName") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="left" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblclassname" runat="server" ViewStateMode="Enabled" Text='<%# Eval("className") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblpaymendMode" runat="server" ViewStateMode="Enabled" Text='<%# Eval("Mode") %>'></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="padding-right:5px;">
                                                                                                    <asp:Label ID="lblpaymentdate" runat="server" ViewStateMode="Enabled" Text='<%#Eval("PaidDate" ,"{0:dd-MMM-yyyy}") %>'></asp:Label>
                                                                                                </td>                                                                     
                                                                                         </tr>                                                                                        
                                                                                     </AlternatingItemTemplate>
                                                                                     <EmptyDataTemplate>
                                                                                         <tr>
                                                                                             <td class="LblNoRecord" align="center">
                                                                                                 <asp:Label ID="Label56" runat="server" Text="<%$ Resources:LocalizedResources,NoRecordsFound%>"></asp:Label>
                                                                                             </td>
                                                                                         </tr>
                                                                                     </EmptyDataTemplate>
                                                                                 </asp:ListView>
                                                                             </td>
                                                                         </tr>                                                                 
                                                                     </table>
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
                </div>
            </td>
        </tr>
        
                    
        <tr>
            <td align="center">
                <asp:HiddenField ID="hidServerDate" ViewStateMode="Enabled" runat="server" />
                <asp:HiddenField ID="hidStudentLogin" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidEndDateRequiredForRrow" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidStartDateRequiredForRow" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidIfYouChangeThePageThenSelectedSanctioned" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidAPopupBlockerIsDetected" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidFeeVideolinkurl" runat="server" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidShowClassDiv" runat="server" Value="N" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidShowAttendanceDiv" runat="server" Value="N" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidShowAllGalleries" runat="server" Value="N" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidPhotoGalleryCount" runat="server" Value="0" ViewStateMode="Enabled" />
                <asp:HiddenField ID="hidHideVidgets" runat="server" Value="0" ViewStateMode="Enabled" />
                
                <div id="divSublingAdmission" runat="server" viewstatemode="Enabled"  style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 700px; height: 410px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -100px 0px 0px 100px;
                    background-color: #FFFFBF;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentsAttention%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideMsgPopup();">
                            <img alt="Hide Popup" class ="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: #FFFFBF; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 370px; width: 696px;" id="Div2">
                        <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                            <tr>
                                <td align="left">
                                    <p style="padding-left: 5px; color: maroon; font-weight: bold; font-size: 14; background-color: #cbdfcc"
                                        class="ClsHilightBGB">
                                        <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, Admission201112%>"></asp:Label>
                                    </p>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <p style="padding-left: 5px; color: MediumVioletRed;">
                                        <b>
                                            <asp:Label ID="lblAdmissionForNurserySectionForSiblings" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,AdmissionForNurserySectionForSiblings%>"></asp:Label></b>
                                        <span class="colonPadding">:</span>
                                    </p>
                                    &nbsp;&nbsp;<asp:Label ID="lblIssueAndSubmissionOfForms" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,IssueAndSubmissionOfFormsAndFees%>"></asp:Label>
                                    <span class="colonPadding">:</span>
                                </td>
                            </tr>
                            <tr>
                                <td height="20px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;">
                                        <tr>
                                            <td>
                                                <b>
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrShantanuSugwekar%>"></asp:Label></b><br />
                                                <b>Mr. Shantanu Sugwekar</b><br />
                                                <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ChiefAdministrativeOfficer%>"></asp:Label>
                                            </td>
                                            <td width="45%">
                                            </td>
                                            <td align="left">
                                                <b>
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,DrMrsAnjaliGurjar%>"></asp:Label></b><br />
                                                <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Principal%>"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" valign="bottom">
                                    <asp:Button ID="btnCancel" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                        CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HideMsgPopup();return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divNotice1" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 680px; height: 385px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -100px 0px 0px 00px;
                    background-color: #FFFFBF;">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 220px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 5px" align="left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentsAttention%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupNotice1();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; background-color: #FFFFBF; text-align: left; vertical-align: top;
                        color: #333; overflow: auto; height: 340px; width: 670px;" id="Div3">
                        <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;" colspan="2">
                            <tr>
                                <td align="center" style="line-height: 33px">
                                    <p>
                                        <table>
                                            <tr>
                                                <td align="center" colspan="5">
                                                    <b>
                                                        <asp:Label ID="lblTransportCommitteeForTheAcademicYear" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,TransportCommitteeForTheAcademicYear%>"></asp:Label></b>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="1">
                                            <tr>
                                                <th class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text=""></asp:Label>
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,RouteNo%>"></asp:Label>
                                                </th>
                                                <th class="LblNormal" style="width: 28%" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,FirstStop%>"></asp:Label>
                                                </th>
                                                <th class="LblNormal" style="width: 25%" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentNames%>"></asp:Label>
                                                </th>
                                                <th class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,CurrentClassOfThechild%>"></asp:Label>
                                                </th>
                                                <th class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ContactNo%>"></asp:Label>
                                                </th>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal" class="ClsBorder">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,A%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,SouthGateHeliconia%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsSheetalTolahunase%>"></asp:Label>
                                                    <br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsVidya%>"></asp:Label>
                                                </td> 
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,SrKGE%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,JrKGB%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9011048692<br />
                                                    9881877224
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,B%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Gravellia%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrRajeshSinha%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsAmrutaKadam%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MrAbhayGupta%>"></asp:Label><br />
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, C%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,SrKGE%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, C3%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9158998702<br />
                                                    9881161881<br />
                                                    9822998820
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, C1%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, SundarSankul%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MrFirdoshPatel%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, A5%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9881301887
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    2
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, ECPVastu%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MrUdayDeore%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, NurF%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9689941001
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    3
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, LullaNagarPetrolPump%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    5
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, KoregaonParkPetrolPump%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MrBhausahebAvhad%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, MrAbhijeetTupe%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, A2%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources, C1%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9960746928<br />
                                                    9922112264
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    6
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,SangharshChowk%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    <asp:Label ID="Label41" runat="server" Text="<%$ Resources:LocalizedResources,A7%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,KailashSuperMarket%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsAbhaJha%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsRituPandey%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" Text="<%$ Resources:LocalizedResources,JrKGD%>"></asp:Label><br />
                                                    <asp:Label ID="Label37" runat="server" Text="<%$ Resources:LocalizedResources,B5%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9850923948<br />
                                                    9689141394
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    <asp:Label ID="Label38" runat="server" Text="<%$ Resources:LocalizedResources,B7%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label ID="Label39" runat="server" Text="<%$ Resources:LocalizedResources,HariGanga%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,A8%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,GangaConstella%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsAsawariWakankar%>"></asp:Label><br />
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrsPranjaliMoghe%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center" style="border: 10px">
                                                <td class="LblNormal">
                                                    <asp:Label ID="Label40" runat="server" Text="<%$ Resources:LocalizedResources,B8%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ChandanNagarKharadiChowkSagarHotel%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                                <td class="LblNormal">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    9
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ManjarGaon%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrPrakashParekh%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,NurC%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9823333192
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td class="LblNormal">
                                                    10
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,PhursungiGaon%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal" align="left">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrBalasahebNimbalkar%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,B4%>"></asp:Label>
                                                </td>
                                                <td class="LblNormal">
                                                    9922421230
                                                </td>
                                            </tr>
                                        </table>
                                        <table>
                                            <tr>
                                                <td style="height: 10px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MrAshishKadamWishesToVolunteer%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LblNormal">
                                                    <b>
                                                        <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Responsilities%>"></asp:Label></b>
                                                    <span class="colonPadding">:</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LblNormal">
                                                    <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,CoOrdinateTheRouteStopsDriver%>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 10px">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="LblNormal">
                                                    <b>
                                                        <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentVolunteersAreRequested%>"></asp:Label></b>
                                                </td>
                                            </tr>
                                        </table>
                                    </p>
                                    <center>
                                        <asp:Button ID="btnCancelNotice1" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                            CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupNotice1();return false;" />
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div id="divNotice2" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 620px; height: 200px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 10px 200px 50px 150px;
                    background-color: FFFFBF">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 170px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentsAttention%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupNotice2();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div align="center" style="padding: 2px; text-align: center; vertical-align: top;
                        color: #333; overflow: auto; height: 150px; width: 570px; margin-left: 5px; background-color: FFFFBF"
                        id="Div4">
                        <table width="100%" align="center" style="font-size: 11pt; color: #333; font-family: Arial;"
                            colspan="2">
                            <tr>
                                <td style="height: 10px;">
                                    <hr style="border-style: solid; border-color: Gray; border-bottom-width: thin;" />
                                </td>
                            </tr>
                            <tr style="line-height: 28px">
                                <td align="center" style="color: #333;">
                                    <b>
                                        <asp:HyperLink ID="hlnkNotice24" Font-Bold="true" Font-Size="Medium" runat="server"
                                            NavigateUrl="../DOWNLOADS/School Notices/Young Buzz Event.pdf" Text="<%$ Resources:LocalizedResources,YoungBuzzEvent%>">
                                        </asp:HyperLink></b>
                                </td>
                            </tr>
                            <tr style="line-height: 28px">
                                <td align="center" style="color: #333;">
                                    <b>
                                        <asp:HyperLink ID="hlnkNotice23" Font-Bold="true" Font-Size="Medium" runat="server" 
                                            NavigateUrl="../Gallery/VideoGallery.aspx?" Text="<%$ Resources:LocalizedResources,NCLWorkshopFor%>">
                                        </asp:HyperLink></b>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 10px;">
                                    <hr style="border-style: solid; border-color: Gray; border-bottom-width: thin;" />
                                </td>
                            </tr>
                            <tr align="center">
                                <td align="center" colspan="2" valign="bottom">
                                    <asp:Button ID="btnCancelNotice2" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                        CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupNotice2();return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <uc1:ucNoticeDivUC ID="NoticeDivUC" runat="server" ViewStateMode="Enabled"  DisplayLocation="C" />
                <div id="divNotice3" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 320px; height: 250px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 30px 200px 250px 350px;
                    background-color: #cbdfcc">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 170px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,ParentsAttention%>"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupNotice3();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                </div>
            </td>
        </tr>
        <tr align="center">
            <td align="center">
                <div id="updtpnlPopUp" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 480px; height: 540px; border-width: 0px; left: 5px;
                    top: 100px; line-height: normal; border: solid 2px darkgreen; margin: 0px 0px 0px 100px;
                    background-color: white; z-index: 100">
                    <div style="background-color: Transparent; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; padding: 4px; color: #Black; text-align: right;">
                        <div style="padding: 1px; padding-left: 40%; font-size: 12px; font-weight: bold;
                            color: #Black; float: left">
                            <asp:Label runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,HappyBirthday%>"></asp:Label>
                        </div>
                        <span style="cursor: hand;" onclick="javascript:HidePopup();">
                            <img class="img-align-top" alt="Hide Popup" src="../images/close_vista.gif" border="0" />
                        </span>
                    </div>
                    <div style="padding: 4px; background-color: ThreeDFace WindowFrame; text-align: left;
                        width: 100%; vertical-align: top; color: #333; overflow: auto; height: auto"
                        id="PopupInfo">
                        <table width="100%">
                            <tr style="height: 80px; background-color: Silver" valign="middle">
                                <td align="center" style="width: 100%; font-size: 10pt; height: 115px; color: blue;
                                    font-family: Arial; font-weight: bold" colspan="2">
                                    <hr />
                                    <asp:Label ID="lblMessage" runat="server" ViewStateMode="Enabled" Text="" />
                                    <hr />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <img src="~/RITeSchool/images/New bday img1.jpg" style="width:100%;height:350px;" runat="server" Viewstatemode="Enabled" onclick="OpenBirthdayImage()" />
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                    <embed src="../images/birthday.swf" wmode="transparent" quality="high" width="100%"
                                        height="260" runat="server" viewstatemode="Enabled" id="embdSwf" name="Yourfilename" align="" type="application/x-shockwave-flash"
                                        pluginspage="http://www.macromedia.com/go/getflashplayer"> </embed>
                                </td>
                            </tr>--%>
                            
                            <tr valign="bottom" align="center">
                                <td align="center" style="font-size: 10pt; padding-top: 8px; color: blue; font-family: Arial;
                                    font-weight: bold" colspan="2">
                                    <div style="margin-top: -4px;">
                                        <div>
                                            <asp:Button ID="btnClose" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                                CssClass="ClsBtn" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divSchoolNotices" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 600px; height: 400px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -100px 0px 0px 00px;
                    background-color: White">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 500px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            Message From
                            <%= ConfigurationManager.AppSettings["SchoolName"]%>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupSchoolNotices();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; text-align: left; vertical-align: top; color: #333; overflow: auto;
                        height: 250px; width: 550px; margin-left: 5px; background-color: White" id="Div8">
                        <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;">
                            <tr>
                                <td align="left" class="MainDataTable">
                                    <%= sMenuContent1 %>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" valign="bottom">
                                    <asp:Button ID="Button2" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                        CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupSchoolNotices();return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>       
        <tr align ="center">
            <td align = "center">
            <div id="divOpenDayNotice" runat="server"  viewstatemode="Enabled" style="
                    position: absolute; margin: 0px; padding: 0px; width: 60%;  height: 50%; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -90px 0px 0px 50px;
                    background-color: white;" >
                     <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 100%; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label44" runat="server" Text="Open Day Std  I - IX"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideOpenDayNotice();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                <img  src="../images/Open Day Std  I - IX.jpg" width="88%"; />
            </div>
            </td>
        </tr>
        <tr align ="center">
            <td align = "center">
            <div id="divMediclaimnotice" runat="server" viewstatemode="Enabled" style=" visibility: hidden; display: none; position: absolute;
                    margin: 0px; padding: 0px; width: 420px; height: 125px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 200px 445px;
                    background-color: #FFF2FF; z-index: 500">
                     <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; width: 100%; text-align: right;">
                        <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color:darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label48" runat="server" Text="Mediclaim Notice"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HideMediclaimNoticePopup();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                               
                        </span>
                    </div>
                    <table style="padding-top: 15px; padding-left: 20px; padding-right: 20px; vertical-align:top;
        width: 99%;" align="center">
       <tr style ="height:25px">
            <td >
                <asp:HyperLink ID="hlnkPPSH" runat="server" Text="1) IRDA-Claim Reimbursement Form-Part A" onclick="openMediclaimPopUp(2)"
                    CssClass="navselHPL" style="color: blue; cursor: pointer; font-size: 11pt" /><br />
                    
            </td>
        </tr>
        <tr style ="height:25px">
            <td >
                <asp:HyperLink ID="hlnkPPS" runat="server" Text="2) IRDA-Claim Reimbursement Form-Part B" onclick="openMediclaimPopUp(1)"
                    CssClass="navselHPL" style="color: blue; cursor:pointer; font-size: 11pt;" /><br />
                    
            </td>
        </tr>
        </table>
            </div>
            </td>
        </tr>  
        <%--<tr align ="center">
            <td align = "center">
                <div id="divAnnualDayInvite" runat="server" viewstatemode="Enabled" style=" visibility: hidden; display: none; position: absolute;
                        margin: 0px; padding: 0px; width: 35%; border-width: 0px; left: 5px;
                        top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 200px 445px;
                        background-color: #FFF2FF; z-index: 600">
                         <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                <asp:Label ID="Label63" runat="server" Text="Annual Day Invitation for 3rd-5th std"></asp:Label>
                            </div>
                            <span style="cursor: hand" onclick="javascript:HideAnnualDayInvitePopup();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />
                            </span>
                        </div>       
                        <img src="../images/AnnualDayInvite(Classes3to5).jpg" id="img2" style="width:100%; height:710px;" runat="server" viewstatemode="Enabled" />             
                </div>
            </td>
        </tr>  --%>
       <%-- <tr align ="center">
            <td align = "center">
                <div id="divAnnualDayInvitation6to10" runat="server" viewstatemode="Enabled" style=" visibility: hidden; display: none; position: absolute;
                        margin: 0px; padding: 0px; width: 35%; border-width: 0px; left: 5px;
                        top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 200px 445px;
                        background-color: #FFF2FF; z-index: 500">
                         <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                            background-repeat: repeat-x; color: Black; width: 100%; text-align: right;">
                            <div style="font-size: 12px; width: 270px; letter-spacing: 1px; padding-left: 8px;
                                font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                                <asp:Label ID="Label64" runat="server" Text="Annual Day Invitation for 6-10th std"></asp:Label>
                            </div>
                            <span style="cursor: hand" onclick="javascript:HideAnuualDayInvitationPopup6to10();">
                                <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                    border="0" />
                            </span>
                        </div>       
                        <img src="../images/Annual DayInvite(6to10)2018-19.jpg" id="img3" style="width:100%; height:710px;" runat="server" viewstatemode="Enabled" />             
                </div>
            </td>
        </tr>        --%>     
        <tr>
            <td>
                <div id="divSchoolNotices1" runat="server" viewstatemode="Enabled" style="visibility: hidden; display: none;
                    position: absolute; margin: 0px; padding: 0px; width: 650px; height: 410px; border-width: 0px;
                    left: 5px; top: 0px; line-height: normal; border: solid 2px darkgreen; margin: -50px 350px 50px 150px;
                    background-color: #FFFFBF">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 500px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="lblMessageFrom" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,MessageFrom%>"></asp:Label>
                            <%= ConfigurationManager.AppSettings["SchoolName"] %>
                        </div>
                        <span style="cursor: hand" onclick="javascript:HidePopupSchoolNotices1();">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div style="padding: 2px; text-align: left; vertical-align: top; color: #333; overflow: auto;
                        height: 370px; width: 620px; margin-left: 5px; background-color: #FFFFBF" id="Div6">
                        <table width="100%" style="font-size: 11pt; color: #333; font-family: Arial;">
                            <tr>
                                <td align="left" class="MainDataTable">
                                    <%= sMenuContent%>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2" valign="bottom">
                                    <asp:Button ID="Button3" runat="server" ViewStateMode="Enabled" Text="<%$ Resources:LocalizedResources,Close%>"
                                        CssClass="ClsBtnMid" CausesValidation="false" Width="75px" OnClientClick="javascript:HidePopupSchoolNotices1();return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <asp:HiddenField ID="hidShowPopup" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidShowAdmissionPopup" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidFirstLogIn" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidShowExpiredSanctionedLeavesPopup" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidSchoolNoticesPopUp" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidSchoolNoticesPopUp1" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidSchoolId" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidPrincipalDesignationId" runat="server" Value="0" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidAttendanceAlert" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidAttendanceAlertFirstTime" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidOpenDayNoticeFirstTime" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidMediclaimNoticeFirstTime" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidNoticeFirstTime" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidShowSportNotice" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidShowReadMe" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidSMSTemplateName" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidSmsTemplate" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidBonafideReportId" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidAdmissionQueryString" runat="server" Value="" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidAcademicYearEndDate" runat="server" Value="" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidSupervisorDesignationName" runat="server" Value="" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidIsFirstTimeLogin" runat="server" Value="N" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidIsMVPSSchool" runat="server" viewstatemode="Enabled" Value="N" />
        <asp:HiddenField ID="HidAdmissionNoticeFirstTime" runat="server" viewstatemode="Enabled" /> 
       <asp:HiddenField ID="HidGetAttendanceSummaryResultStudents" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="HidGetAttendanceSummaryResultClasses" runat="server" viewstatemode="Enabled" />
        <asp:HiddenField ID="hidShowRetirementPopup" runat="server" viewstatemode="Enabled" />
        <%--<asp:HiddenField ID="HidAnnualDayPPSH" runat="server" ViewStateMode="Enabled" />--%>		
    </table>
       <style type="text/css">
        .class1
        {
            border: 1;
        }
    </style>
    <script type="text/javascript">
        
    </script>
    <script src="../Scripts/jquery-blink.js" type="text/javascript"></script>     
    <script src="../../js/jquery.qtip-1.0.0-rc3.min.js" type="text/javascript"></script>
    <script src="../Scripts/Dashboard.js?version=2.9" type="text/javascript"></script>   
    <script lang="javascript" type="text/javascript">
        var _dtAcademicYearEndDate = $get("<%=this.hidAcademicYearEndDate.ClientID %>").value;
        var _ddlAcademicYear = $get("<%=this.cmbAcademicYearID.ClientID %>") != null ? $get("<%=this.cmbAcademicYearID.ClientID %>").value : 0;
        var _loggedUserDesignationId = $get("<%=this.hidPrincipalDesignationId.ClientID %>").value;
        var _supervisorDesignationName = $get("<%=this.hidSupervisorDesignationName.ClientID %>").value; 
        var _financialYearId = "<%=miFinancialYearId%>";
        var _defaultProfilePicPath = "<%= Utility.Constants.S_DEFAULT_PROFILE_PIC_PATH %>";
        var _clienthidIsFirstTimeLogin = "<%=this.hidIsFirstTimeLogin.ClientID %>"
        var _clienthidShowAllGalleries = "<%=this.hidShowAllGalleries.ClientID %>"
        var _clienthidIsMVPSSchool = "<%=this.hidIsMVPSSchool.ClientID %>" 
       var _clientHidGetAttendanceSummaryResultStudents = "<%=this.HidGetAttendanceSummaryResultStudents.ClientID %>"
        var _clientHidGetAttendanceSummaryResultClasses = "<%=this.HidGetAttendanceSummaryResultClasses.ClientID %>"
        var _clienthidHideVidgets = '<%=this.hidHideVidgets.ClientID %>'
                
        //This function is used to display video.
        function ShowVideo(s_feevideolinkurl) {
            window.open(s_feevideolinkurl, '_blank', 'scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=20,width=750,height=500,resizable=no');
            return false;
        }

        function showtooltip() {
            $('.class1').qtip({
                content: {
                    text: false // Use each elements title attribute
                },
                style: {
                    name: 'cream',
                    color: 'black',  //'cream', // Give it some style
                    border: {
                        height: 10,
                        width: 3,
                        radius: 5
                    },
                    tip: 'topRight',
                    width: 200
                },

                position: { adjust: { x: -210, y: 0} }
            });
        }

        showtooltip(); 

		function ShowSchoolNoticesPopup() {

			_clienthidSchoolNoticesPopUp = "<%=this.hidSchoolNoticesPopUp.ClientID %>"

			if (document.getElementById(_clienthidSchoolNoticesPopUp).value == "Y") {
				var x, y, tt_ovr_
				var cssstyle = $get("<%=this.divSchoolNotices.ClientID %>").style
				var width = 200
				var height = 53
				var left = parseInt((screen.width / 2) - (width / 2)) - 290
				var top = parseInt((screen.height / 2) - (height / 2)) - 90
				cssstyle.left = left + "px"
				cssstyle.top = top + "px"
				cssstyle.visibility = "visible"
				cssstyle.display = "block"
			}
			else
				HidePopupSchoolNotices()
		}
		function ShowSchoolNoticesPopup1() {

			_clienthidSchoolNoticesPopUp1 = "<%=this.hidSchoolNoticesPopUp1.ClientID %>"

			if (document.getElementById(_clienthidSchoolNoticesPopUp1).value == "Y") {
				var x, y, tt_ovr_
				var cssstyle = $get("<%=this.divSchoolNotices1.ClientID %>").style
				var width = 200
				var height = 53
				var left = parseInt((screen.width / 2) - (width / 2)) - 290
				var top = parseInt((screen.height / 2) - (height / 2)) - 90
				cssstyle.left = left + "px"
				cssstyle.top = top + "px"
				cssstyle.visibility = "visible"
				cssstyle.display = "block"
			}
			else
				HidePopupSchoolNotices1()
		}
		function ShowMsgPopup() {
			var now = new Date($get("<%=this.hidServerDate.ClientID %>").value)
			_clientHidStudentLogin = "<%=this.hidStudentLogin.ClientID %>"
			_clienthidFirstLogIn = "<%=this.hidFirstLogIn.ClientID %>"
			displayAdmissionPopup = "<%=this.hidShowAdmissionPopup.ClientID %>"

			var EndDate = new Date('12/12/2010 12:00:00 AM')
			if (now < EndDate
			) {
				var x, y, tt_ovr_
				var cssstyle = $get("<%=this.divSublingAdmission.ClientID %>").style
				var width = 200
				var height = 53
				var left = parseInt((screen.width / 2) - (width / 2)) - 290
				var top = parseInt((screen.height / 2) - (height / 2)) - 90
				cssstyle.left = left + "px"
				cssstyle.top = top + "px"
				cssstyle.visibility = "visible"
				cssstyle.display = "block"
			}
			else
				HideMsgPopup()
		}
		function ShowPopupNotice1() {
			var now = new Date($get("<%=this.hidServerDate.ClientID %>").value)
			var EndDate = new Date('11/11/2011 12:00:00 AM')
			var iSchoolId = $get("<%=this.hidSchoolId.ClientID %>").value

			if (iSchoolId == "18") {
				if (now < EndDate
		   && document.getElementById(displayAdmissionPopup) != null
		   && document.getElementById(displayAdmissionPopup).value == "Y"
			) {
					var x, y, tt_ovr_
					var cssstyle = $get("<%=this.divNotice1.ClientID %>").style
					var width = 200
					var height = 53
					var left = parseInt((screen.width / 2) - (width / 2)) - 290
					var top = parseInt((screen.height / 2) - (height / 2)) - 90
					cssstyle.left = left + "px"
					cssstyle.top = top + "px"
					cssstyle.visibility = "visible"
					cssstyle.display = "block"
				}
				else
					HidePopupNotice1()
			}
			else
				HidePopupNotice1()
		}

		function ShowPopupNotice2() {
			var now = new Date($get("<%=this.hidServerDate.ClientID %>").value)
			var EndDate = new Date('1/21/2012 11:59:00 PM')
			var iSchoolId = $get("<%=this.hidSchoolId.ClientID %>").value

			if (iSchoolId == "18") {
				if (now < EndDate
					   && document.getElementById(displayAdmissionPopup) != null
					   && document.getElementById(displayAdmissionPopup).value == "Y"
			) {
					var x, y, tt_ovr_
					var cssstyle = $get("<%=this.divNotice2.ClientID %>").style
					var width = 200
					var height = 53
					var left = parseInt((screen.width / 2) - (width / 2)) - 290
					var top = parseInt((screen.height / 2) - (height / 2)) - 90
					cssstyle.left = left + "px"
					cssstyle.top = top + "px"
					cssstyle.visibility = "visible"
					cssstyle.display = "block"
				}
				else
					HidePopupNotice2()
			}
			else
				HidePopupNotice2()
		}

		function ShowPopupNotice3() {
			var now = new Date($get("<%=this.hidServerDate.ClientID %>").value)
			var EndDate = new Date('12/23/2011 12:00:00 AM')
			var iSchoolId = $get("<%=this.hidSchoolId.ClientID %>").value

			if (iSchoolId == "18") {
				if (now < EndDate
					   && document.getElementById(displayAdmissionPopup) != null
					   && document.getElementById(displayAdmissionPopup).value == "Y"
			) {
					var x, y, tt_ovr_
					var cssstyle = $get("<%=this.divNotice3.ClientID %>").style
					var width = 200
					var height = 53
					var left = parseInt((screen.width / 2) - (width / 2)) - 290
					var top = parseInt((screen.height / 2) - (height / 2)) - 90
					cssstyle.left = left + "px"

					cssstyle.top = top + "px"
					cssstyle.visibility = "visible"
					cssstyle.display = "block"
				}
				else
					HidePopupNotice3()
			}
			else
				HidePopupNotice3()
		}

		function ShowExpiredStudentSanctionedLeavePopup() {

			_clientHidShowExpiredSanctionedLeavesPopup = "<%=this.hidShowExpiredSanctionedLeavesPopup.ClientID %>"

			if (document.getElementById(_clientHidShowExpiredSanctionedLeavesPopup).value == "Y") {
				var x, y, tt_ovr_
				var cssstyle = $get("<%=this.divExpiredSanctionedLeaves.ClientID %>").style
				var width = 200
				var height = 53
				var left = parseInt((screen.width / 2) - (width / 2)) - 290
				var top = parseInt((screen.height / 2) - (height / 2)) - 90
				cssstyle.left = left + "px"
				cssstyle.top = top + "px"
				cssstyle.visibility = "visible"
				cssstyle.display = "block"
			}
			else
				HidePopupExpiredSanctionedLeaves()
		}

		function HidePopupSchoolNotices() {

			document.getElementById(_clienthidSchoolNoticesPopUp).value = "N";
			$get("<%=this.divSchoolNotices.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divSchoolNotices.ClientID %>").style.display = "none"
			return false
		}
		function HidePopupSchoolNotices1() {

			document.getElementById(_clienthidSchoolNoticesPopUp1).value = "N";
			$get("<%=this.divSchoolNotices1.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divSchoolNotices1.ClientID %>").style.display = "none"
			return false
		}
		function HideMsgPopup() {
			$get("<%=this.divSublingAdmission.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divSublingAdmission.ClientID %>").style.display = "none"
			return false
		}
		function HidePopupNotice1() {
			$get("<%=this.divNotice1.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divNotice1.ClientID %>").style.display = "none"
			return false
		}
		function HidePopupNotice2() {
			$get("<%=this.divNotice2.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divNotice2.ClientID %>").style.display = "none"
			return false
		}
		function HidePopupNotice3() {
			$get("<%=this.divNotice3.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divNotice3.ClientID %>").style.display = "none"
			return false
		}
		function HidePopupExpiredSanctionedLeaves() {

			document.getElementById(_clientHidShowExpiredSanctionedLeavesPopup).value = "N";
			$get("<%=this.divExpiredSanctionedLeaves.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divExpiredSanctionedLeaves.ClientID %>").style.display = "none"
			return false
		}

		function ShowAttendanceAlertPopup() {
			var x, y, tt_ovr_
			var cssstyle = $get("<%=this.divAttendanceAlert.ClientID %>").style
			var width = 600
			var height = 380
			var left = parseInt((screen.width / 2) - (width / 2))
			var top = parseInt((screen.height / 2) - (height / 2))
			cssstyle.left = left + "px"
			cssstyle.top = top + "px"
			cssstyle.visibility = "visible"
			cssstyle.display = "block"


}

function ShowRetirementAlertPopup() {
    var x, y, tt_ovr_
    var cssstyle = $get("<%=this.divRetirementAlert.ClientID %>").style
    var width = 800
    var height = 450
    var left = parseInt((screen.width / 2) - (width / 2))
    var top = parseInt((screen.height / 2) - (height / 2))
        top = top + 100
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"

}

function ShowAbsentStudentPopup() {
    document.getElementById("<%=this.divMissingAttendancePopup.ClientID %>").visiblity = "visible";    
    var x, y, tt_ovr_
    var cssstyle = $get("<%=this.divMissingAttendancePopup.ClientID %>").style
    var width = 600
    var height = 120
    var left = parseInt((screen.width / 2) - (width / 2))
    var top = parseInt((screen.height / 2) - (height / 2))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"
}


function ShowTeacherAlertPopup() {
    var x, y, tt_ovr_
    var cssstyle = $get("<%=this.divNonPermenantTeacher.ClientID %>").style
    var width = 1300
    var height = 950
    var left = parseInt((screen.width / 2) - (width/2))
    var top = parseInt((screen.height / 2) - (height/2))
    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"

}

//function ShowClearanceNotification() {
//    var x, y, tt_ovr_
//    var cssstyle = $get("<%=this.div12.ClientID %>").style
//    var width = 1300
//    var height = 950
//    var left = parseInt((screen.width / 2) - (width / 2))
//    var top = parseInt((screen.height / 2) - (height / 2))
//    cssstyle.left = left + "px"
//    cssstyle.top = top + "px"
//    cssstyle.visibility = "visible"
//    cssstyle.display = "block"

//}


//This function is used to set styles to div.
function ShowOpenDayNoticeAlert() {
    var x, y, tt_ovr_;
    if ($get("<%=this.divOpenDayNotice.ClientID %>") != null) {
        var cssstyle = $get("<%=this.divOpenDayNotice.ClientID %>").style
        var width = 1000
        var height = 380
        var left = parseInt((screen.width / 2) - (width / 2))
        var top = parseInt((screen.height / 2) - (height / 2))
        left = 330
        top = 312
        cssstyle.left = left + "px"
        cssstyle.top = top + "px"
        cssstyle.visibility = "visible"
        cssstyle.display = "block"
    }
}

//This function is used to set styles to div.
function ShowParentFeedbackPopup() {
    var x, y, tt_ovr_
    if ($get("<%=this.divParentFeedback.ClientID %>") != null) {
        var cssstyle = $get("<%=this.divParentFeedback.ClientID %>").style
        var width = 1000
        var height = 380
        var left = parseInt((screen.width / 2) - (width / 2))
        var top = parseInt((screen.height / 2) - (height / 2))
        left = 350
        top = 200

        cssstyle.left = left + "px"
        cssstyle.top = top + "px"
        cssstyle.visibility = "visible"
        cssstyle.display = "block"
    }
}




//This function is used to set styles to div.
//function ShowOmAppriciationletter2() {
//    var x, y, tt_ovr_
//    if ($get("<=this.divAppLetter2.ClientID %>") != null) {
//        var cssstyle = $get("<=this.divAppLetter2.ClientID %>").style
//        var width = 750
//        var height = 550
//        var left = parseInt((screen.width / 2) - (width / 2))

//        var top = parseInt((screen.height / 2) - (height / 2))

//        cssstyle.left = left + "px"
//        cssstyle.top = top + "px"
//        cssstyle.visibility = "visible"
//        cssstyle.display = "block"
//    }
//}



function ShowFeeNoticeDiv() {    
    var x, y, tt_ovr_
    if ($get("<%=this.divFeeNotice.ClientID %>") != null) {
        var cssstyle = $get("<%=this.divFeeNotice.ClientID %>").style
        var width = 1450
        var height = 700
        var left = parseInt((screen.width / 2) - (width / 2))
        var top = parseInt((screen.height / 2) - (height / 2))

        cssstyle.left = left + "px"
        cssstyle.top = top + "px"
        cssstyle.visibility = "visible"
        cssstyle.display = "block"
    }
}




//function HideOmAppreciationletter2() {
////    $get("<=this.divAppLetter2.ClientID %>").style.visibility = "hidden"
////    $get("<=this.divAppLetter2.ClientID %>").style.display = "none"
//    return false
//}
function HidedivflashNotice() {
    $get("<%=this.divflashNotice.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divflashNotice.ClientID %>").style.display = "none"
    return false
}

function ShowflashNotice() {
    var x, y, tt_ovr_
    if ($get("<%=this.divflashNotice.ClientID %>") != null) {
        var cssstyle = $get("<%=this.divflashNotice.ClientID %>").style
        var width = 500
        var height = 400
        var left = parseInt((screen.width / 2) - (width / 2))

        var top = parseInt((screen.height / 2) - (height / 2))
        left = 200;

        cssstyle.left = left + "px"
        cssstyle.top = top + "px"
        cssstyle.visibility = "visible"
        cssstyle.display = "block"
    }
}


function HideFeeNoticeDiv() {
    $get("<%=this.divFeeNotice.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divFeeNotice.ClientID %>").style.display = "none"
    return false
}


//This function is used to set styles to div.
function ShowMediclaimNoticeAlertPopup() {
    var x, y, tt_ovr_
    var cssstyle = $get("<%=this.divMediclaimnotice.ClientID %>").style
    var width = 1000
    var height = 380
    var left = parseInt((screen.width / 2) - (width / 2))
    var top = parseInt((screen.height / 2) - (height / 2))

    left = 100
    top = 150

    cssstyle.left = left + "px"
    cssstyle.top = top + "px"
    cssstyle.visibility = "visible"
    cssstyle.display = "block"

}

function HideClearancePayment() {

    $get("<%=this.div12.ClientID %>").style.visibility = "hidden"
    $get("<%=this.div12.ClientID %>").style.display = "none"
    return false
}

function HideParentFeedbackPopup() {

    $get("<%=this.divParentFeedback.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divParentFeedback.ClientID %>").style.display = "none"
    return false
}

//This function is used to close mediclaim Popup.
function HideMediclaimNoticePopup() {
    
    $get("<%=this.divMediclaimnotice.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divMediclaimnotice.ClientID %>").style.display = "none"
    return false
}


function HideOpenDayNotice() {

    $get("<%=this.divOpenDayNotice.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divOpenDayNotice.ClientID %>").style.display = "none"
    return false
}

		function HidePopupAttendance() {

			$get("<%=this.divAttendanceAlert.ClientID %>").style.visibility = "hidden"
			$get("<%=this.divAttendanceAlert.ClientID %>").style.display = "none"
			return false

}

function HideRetirementPopup() {

    $get("<%=this.divRetirementAlert.ClientID %>").style.visibility = "hidden"
    $get("<%=this.divRetirementAlert.ClientID %>").style.display = "none"
    return false
}



function HideMissingAttendancePopup() {
    if ($get("<%=this.divMissingAttendancePopup.ClientID %>") != null) {
        $get("<%=this.divMissingAttendancePopup.ClientID %>").style.visibility = "hidden"
        $get("<%=this.divMissingAttendancePopup.ClientID %>").style.display = "none"
    }
    return false;
}

function HideTeacherDetails() {
    if ($get("<%=this.divNonPermenantTeacher.ClientID %>") != null) {
        $get("<%=this.divNonPermenantTeacher.ClientID %>").style.visibility = "hidden"
        $get("<%=this.divNonPermenantTeacher.ClientID %>").style.display = "none"
    }
    return false
}



		ShowSchoolNoticesPopup()
		ShowSchoolNoticesPopup1()
		ShowMsgPopup()
		ShowPopupNotice1()
		ShowPopupNotice2()
		ShowPopupNotice3()
		ShowExpiredStudentSanctionedLeavePopup()
		
		_clientHidOpenDayNoticefirsttime = "<%=this.HidOpenDayNoticeFirstTime.ClientID %>"
		if (document.getElementById(_clientHidOpenDayNoticefirsttime).value == "Y") {		    
		    ShowOpenDayNoticeAlert()
		    //ShowParentFeedbackPopup()
		}

		_clientHidNoticefirsttime = "<%=this.HidNoticeFirstTime.ClientID %>"

		if (document.getElementById(_clientHidNoticefirsttime).value == "Y") {
		    ShowFeeNoticeDiv()
		    //  ShowOmAppriciationletter2()
		    ShowflashNotice()
		    //ShowAppreciationDiv()
		    //ShowThankNoteDiv()
		    ShowInVitationVideo()
		    //ShowCeremonyVideo()
		    ShowChildrenDayVideo()
		    //ShowResultDiv()		    
		}
		else {
		    HideTeacherDetails()
		}

        _clientHidAdmissionNoticeFirstTime = "<%=this.HidAdmissionNoticeFirstTime.ClientID %>"
        if (document.getElementById(_clientHidAdmissionNoticeFirstTime).value == "Y") {
//            ShowAdmission()
        }

		_clientHidMediclaimNoticefirsttime = "<%=this.HidMediclaimNoticeFirstTime.ClientID %>"		
		if (document.getElementById(_clientHidMediclaimNoticefirsttime).value == "Y") {
		    ShowMediclaimNoticeAlertPopup()
		}


		_clientHidShowAttendanceAlertPopupfisttime = "<%=this.HidAttendanceAlertFirstTime.ClientID %>"
        if (document.getElementById(_clientHidShowAttendanceAlertPopupfisttime).value == "Y")
            ShowAttendanceAlertPopup()
            
        if(document.getElementById("<%=this.hidShowRetirementPopup.ClientID %>").value == "Y")
            ShowRetirementAlertPopup();

        _clientlstvwStudentSanctionedLeave = "<%=this.lstvwStudentSanctionedLeave.ClientID %>"
		_clientcst_StartAndEndDate = "<%=this.cst_StartAndEndDate.ClientID %>";
		_clientlbl_UpdateSucess = "<%=this.lblUpdateSucess.ClientID %>";
		_clienthidPageNo = "<%=this.hidPageNo.ClientID %>"
		_clientcst_StartDateValidation = "<%=this.cst_StartDateValidation.ClientID %>";
		_clientcst_EndDateValidation = "<%=this.cst_EndDateValidation.ClientID %>";
		_clienthidFirstLogIn = "<%=this.hidFirstLogIn.ClientID %>";
		_clienthidShowReadMe = "<%=this.hidShowReadMe.ClientID %>";
		_clientlblLastLogin = "<%=this.lblLastLogin.ClientID %>";

		$(document).ready(function () {
		    if ($("#" + _clientlblLastLogin).text() == 'Welcome to RITeSchool! Thank you for loging into the system.' && $("#" + _clienthidShowReadMe).val() == "Y")
		        window.open("../DOWNLOADS/ReadMe.txt", '_blank', 'scrollbar=yes,resizable=yes,height=600,width=800');
		    $("#" + _clienthidShowReadMe).val('N');

		    CheckVisibilityOfAbsetStudents();

		    if ($("[id*=imgBtnMsgAlert]").length > 0) {
		        var newMsgCount = $("[id*=imgBtnMsgAlert]").attr('title').split(' ')[0];
		        if (newMsgCount > 0) {
		            $("[id*=imgBtnMsgAlert]").parent().append('<a id="divCount" onclick="window.open(\'/RITeSchool/Common/MessageInbox.aspx\',\'_self\');return false;" title="' + newMsgCount + ' Unread Message(s)" class="badge badge-warning animated bounceIn ">' + newMsgCount + '</a>');
		            $('[id*=imgBtnMsgAlert]').remove();
		        }
		    }


		    if ($("[id*=imgBtnBirthdayAlert]").length > 0) {
		        var birthdayCount = $("[id*=imgBtnBirthdayAlert]").attr('title').split(' ')[0];
		        if (birthdayCount > 0) {
		            $("[id*=imgBtnBirthdayAlert]").parent().append('<a id="divBithdayCount" onclick="window.open(\'/RITeSchool/Common/StaffBirthDay.aspx\',\'_self\');return false;" title="' + birthdayCount + ' Staff Birthday(s)" class="badge badge-warning animated bounceIn">' + birthdayCount + '</a>');
		            $("[id*=imgBtnBirthdayAlert]").remove();
		        }
		    }

		    $("#" + _clienthidShowReadMe).val('N');

		    //Disable user typing in Date textbox, but still allow user to open Calendar on click of icon
		    $("#datepicker").attr("disabled", "disabled");
		    $("#datepicker").attr("style", "background-color: white !important; color: black !important");
		    $("#datepicker").data("kendoDatePicker");

		    return false;
		});

        function HideInVitationVideo() {
		    $get("<%=this.divInVitationVideo.ClientID %>").style.visibility = "hidden"
		    $get("<%=this.divInVitationVideo.ClientID %>").style.display = "none"
		    return false
		}

        function HideCeremonyVideo() {
            $get("<%=this.divCeremonyVideo.ClientID %>").style.visibility = "hidden"
            $get("<%=this.divCeremonyVideo.ClientID %>").style.display = "none"
            return false
        }

//        function HideAnnualDayInvitationVideo() {
//            $get("<%=this.divAnnualDayInvitationVideo.ClientID %>").style.visibility = "hidden"
//            $get("<%=this.divAnnualDayInvitationVideo.ClientID %>").style.display = "none"
//            return false
//        }

		function HideChildrenDayVideo() {
		    $get("<%=this.divPPSHChildrenDay.ClientID %>").style.visibility = "hidden"
		    $get("<%=this.divPPSHChildrenDay.ClientID %>").style.display = "none"
		    return false
		}

		function ShowChildrenDayVideo() {
		    var x, y, tt_ovr_
		    if ($get("<%=this.divPPSHChildrenDay.ClientID %>") != null) {
		        var cssstyle = $get("<%=this.divPPSHChildrenDay.ClientID %>").style
		        var width = 400
		        var height = 850
		        var left = parseInt((screen.width / 2) - (width / 2))
		        var top = parseInt((screen.height / 2) - (height / 2))
		        top = top + 50

		        cssstyle.left = left + "px"
		        cssstyle.top = top + "px"
		        cssstyle.visibility = "visible"
		        cssstyle.display = "block"
		    }
		}

		function ShowInVitationVideo() {
		    var x, y, tt_ovr_
		    if ($get("<%=this.divInVitationVideo.ClientID %>") != null) {
		        var cssstyle = $get("<%=this.divInVitationVideo.ClientID %>").style
		        var width = 400
		        var height = 850
		        var left = parseInt((screen.width / 2) - (width / 2))
		        var top = parseInt((screen.height / 2) - (height / 2))
		        top = top + 50

		        cssstyle.left = left + "px"
		        cssstyle.top = top + "px"
		        cssstyle.visibility = "visible"
		        cssstyle.display = "block"
		    }
		}

		function ShowCeremonyVideo() {
		    var x, y, tt_ovr_
		    if ($get("<%=this.divCeremonyVideo.ClientID %>") != null) {
		        var cssstyle = $get("<%=this.divCeremonyVideo.ClientID %>").style
		        var width = 400
		        var height = 850
		        var left = parseInt((screen.width / 2) - (width / 2))
		        var top = parseInt((screen.height / 2) - (height / 2))
		        top = top + 50

		        cssstyle.left = left + "px"
		        cssstyle.top = top + "px"
		        cssstyle.visibility = "visible"
		        cssstyle.display = "block"
		    }
		}

        function ShowAnnualDayInvitationVideo() {
            var x, y, tt_ovr_
            if ($get("<%=this.divAnnualDayInvitationVideo.ClientID %>") != null) {
                var cssstyle = $get("<%=this.divAnnualDayInvitationVideo.ClientID %>").style
                var width = 400
                var height = 850
                var left = parseInt((screen.width / 2) - (width / 2))
                var top = parseInt((screen.height / 2) - (height / 2))
                top = top + 50

                cssstyle.left = left + "px"
                cssstyle.top = top + "px"
                cssstyle.visibility = "visible"
                cssstyle.display = "block"
            }
        }


		function CheckVisibilityOfAbsetStudents() {
		    var showAttendanceDiv = $('#' + "<%=this.hidShowAttendanceDiv.ClientID %>").val();
		    if (showAttendanceDiv == "Y") {
		        ShowAbsentStudentPopup();
		    }
		    else
		        HideMissingAttendancePopup();
        }

		function fnover(varname) {
			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1"
			objTXT.style.borderColor = "maroon"
			objTXT.style.backgroundImage = "url(../images/BtnBGRollNew.jpg)"
		}

		function fnout(varname) {
			var objTXT = document.getElementById(varname)
			objTXT.style.borderWidth = "1"
			objTXT.style.borderColor = "#a3c07b"
			objTXT.style.backgroundImage = "url(../images/BtnBG.jpg)"
		}

		function cstStartAndEndDate(oSrc, args) {
			var dtStartDate;
			var dtEndDate;
			var sMsg = "";
			var isValid = true;
			var chk
			var i = 1;
			var iRow = 0;
			var iPercent = "";
			var sHolidayName = "";
			var maxRows;
			if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
				maxRows = 20;
			else
				maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
			while (i < maxRows) {
				var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
				var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
				if (HolidyStartDate != null && HolidyStartDate != "" && HolidyEndDate != null && HolidyEndDate != "") {
					var dtStartdt;
					var dtEnddt;
					if (document.all) {
						dtStartdt = new Date(HolidyStartDate.replace('-', ' '));
						dtEnddt = new Date(HolidyEndDate.replace('-', ' '));
					}
					else {
						dtStartdt = new Date(convertdate(HolidyStartDate));
						dtEnddt = new Date(convertdate(HolidyEndDate));
					}
					if ((dtStartdt > dtEnddt))
						sMsg = sMsg + i + ", ";
				}
				i = i + 1;
				iRow = iRow + 1;
			}
			if (sMsg != "") {
			    sMsg = sMsg.substring(0, sMsg.length - 2);
			    document.getElementById(_clientcst_StartAndEndDate).errormessage = document.getElementById("<%=hidEndDateRequiredForRrow.ClientID%>").value + sMsg;
				args.IsValid = false;
				return true;
			}
			args.IsValid = true;
			return false;
		}

		function cstEndDateValidation(oSrc, args) {
			var dtStartDate;
			var dtEndDate;
			var sMsg = "";
			var isValid = true;
			var chk
			var i = 1;
			var iRow = 0;
			var iPercent = "";
			var sHolidayName = "";
			var maxRows;
			if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
				maxRows = 20;
			else
				maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
			while (i < maxRows) {
				var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
				var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
				if (HolidyStartDate != null && HolidyStartDate != "" && (HolidyEndDate == null || HolidyEndDate == "")) {
					sMsg = sMsg + i + ", ";
				}
				i = i + 1;
				iRow = iRow + 1;
			}
			if (sMsg != "") {
			    sMsg = sMsg.substring(0, sMsg.length - 2);
			    document.getElementById(_clientcst_EndDateValidation).errormessage = document.getElementById("<%=hidEndDateRequiredForRrow.ClientID %>").value + " : " + sMsg;
				args.IsValid = false;
				return true;
			}

			args.IsValid = true;
			return false;
}

            SetGalleryCount();
            function SetGalleryCount() {
                var opt = $('#' + '<%=this. hidPhotoGalleryCount.ClientID %>').val()
                $('#cmbPhotoGalleryMonth option[value=100]').text('Recent '+opt);
            }

		function cstStartDateValidation(oSrc, args) {

			var dtStartDate;
			var dtEndDate;
			var sMsg = "";
			var isValid = true;
			var chk
			var i = 1;
			var iRow = 0;
			var iPercent = "";
			var sHolidayName = "";
			var maxRows;
			if ((document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) > 20)
				maxRows = 20;
			else
				maxRows = (document.getElementById(_clientlstvwStudentSanctionedLeave + '_tblStaffInfo').rows.length) - 1;
			while (i < maxRows) {
				var HolidyStartDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtStartDate").value.trim();
				var HolidyEndDate = document.getElementById(_clientlstvwStudentSanctionedLeave + "_ctrl" + iRow + "_txtEndDate").value.trim();
				if ((HolidyStartDate == null || HolidyStartDate == "") && HolidyEndDate != null && HolidyEndDate != "") {
					sMsg = sMsg + i + ", ";
				}
				i = i + 1;
				iRow = iRow + 1;
			}
			if (sMsg != "") {
			    sMsg = sMsg.substring(0, sMsg.length - 2);
			    document.getElementById(_clientcst_StartDateValidation).errormessage = document.getElementById("<%=hidStartDateRequiredForRow.ClientID%>").value + " : " + sMsg;
				args.IsValid = false;
				return true;
			}

			args.IsValid = true;
			return false;
		}

		function btnsaveonclick(varname) {

			var lbl1 = document.getElementById(_clientlbl_UpdateSucess);
			lbl1.innerHTML = "";
		}

		function MessageAboutDate(oCmb) {
		    var bIsValid
		    if (window.confirm(document.getElementById("<%=hidIfYouChangeThePageThenSelectedSanctioned.ClientID %>").value))
				bIsValid = true
			else {
				document.getElementById(oCmb).value = document.getElementById(_clienthidPageNo).value
				bIsValid = false
			}
			return bIsValid
		}
    </script>
    <script type="text/javascript">
		function HideAllControls(url) {
			document.body.style.backgroundImage = "url(RITeSchool/images/pleaseWaitUK.gif)"
			document.body.style.backgroundRepeat = "no-repeat"
			var elements = document.getElementsByTagName('*')
			var iCount = elements.length
			var i = 0
			sdsad(fgg)
			for (i = 0; i < iCount; i++) {
				if (elements[i].tagName == "TR")
					elements[i].style.display = "none"
			}
			alert(url)
			if (url != null)
				window.open(url, '_self')
			return false
		}

		function SetFunctionToAllControls(obj, url) {
			var elements = document.getElementById(obj)
			elements.onclick = function () { HideAllControls(url); }
		}

		function ShowPopup() {
			displayPopup = "<%=this.hidShowPopup.ClientID %>"
			if (document.getElementById(displayPopup) != null && document.getElementById(displayPopup).value == "Y") {
				var x, y, tt_ovr_
				var cssstyle = $get("<%=this.updtpnlPopUp.ClientID %>").style
				var width = 500
				var height = 400
				var pageWidth = window.screen.width
				var pageHeight = 500
				var left = parseInt((pageWidth / 2) - (width / 2))
				var top = parseInt((pageHeight / 2) - (height / 2)) + 100;
				cssstyle.left = left + "px"
				cssstyle.top = top + "px"
				cssstyle.visibility = "visible"
				cssstyle.display = "block"
			}
		}
		function HidePopup() {
			$get("<%=this.updtpnlPopUp.ClientID %>").style.visibility = "hidden"
			$get("<%=this.updtpnlPopUp.ClientID %>").style.display = "none"
			$get("<%=this.hidShowPopup.ClientID %>").value = "N"
			return false
		}
		ShowPopup()

		function detectPopupBlocker() {
			_clienthidFirstLogIn = "<%=this.hidFirstLogIn.ClientID %>"
			if (document.getElementById(_clienthidFirstLogIn).value != "N") {
				var myTest = window.open("", "", "directories=no,height=10,width=10,menubar=no,resizable=no,scrollbars=no,status=no,titlebar=no,top=0,location=no");
				if (!myTest) {
				    if (window.confirm(document.getElementById("<%=hidAPopupBlockerIsDetected.ClientID%>").value)) {
						window.location = 'PopupBlockerUI.aspx'
					}

				}
				else {
					myTest.close();
				}
			}
		}
		window.onload = detectPopupBlocker;
		detectPopupBlocker()


		function OpenFeeStructure() {
		    window.open('../images/PPSNFeeStructure2025-26.png?version=2.3', '_blank')
		}

		function OpenResult() {
		    window.open('../images/Pawar_Public_School,Result.jpg', '_blank')
		}

		function OpenBirthdayImage() {
		    window.open('../images/New bday img1.jpg?version=2.2', '_blank')
		}

    </script>

    <script language="javascript" type="text/javascript">

		_cltdivAttendanceAlert = "<%=this.divAttendanceAlert.ClientID %>"
		var isLibraryModuleEnabled = "<%=Settings.EnableLibraryModule%>";
		 var externalLibrarySite = "<%=Settings.ExternalLibrarySite%>";

        var isAccountModuleEnabled = "<%=Settings.EnableAccountsModule%>";
        var isPayrollModuleEnabled  = "<%=Settings.EnablePayrollModule%>";

		var _totalWinHeight;
		var _adjWinHeight;
		var _rightFooterPos;
		var _bottomFooterPos;

		window.onresize = setTotal;
		window.onscroll = setTotal;
		window.onload = setTotal;

		function setTotal() {
			_totalWinHeight = document.body.scrollHeight;
			_adjWinHeight = _totalWinHeight; //-608;

			if (document.getElementById(_cltdivAttendanceAlert) != null) {
				_rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivAttendanceAlert).style.height);
				document.getElementById(_cltdivAttendanceAlert).style.top = _rightFooterPos;
			}
			window_onscroll();
		}

		function window_onscroll() {
			if (document.body.scrollTop <= _adjWinHeight) {
				if (document.getElementById(_cltdivAttendanceAlert) != null) {
					document.getElementById(_cltdivAttendanceAlert).style.top = document.body.scrollTop + _rightFooterPos;
				}
			}

		}
    </script>  

    <script type="text/javascript">
        function DisplayClassStudentCountDiv() {     
            var showDiv = $('#' + "<%=this.hidShowClassDiv.ClientID %>").val();
            if (showDiv == "Y") {                
                $('#divClasswiseStudentCount').css('display', '');
                ContentWindow = $('#divClasswiseStudentCount').kendoWindow({
                    title: "Classwise Student Count",
                    visible: false,
                    modal: false,
                    resizable: false,
                    width: '500px',
                    height:'380px'
                }).data("kendoWindow");
                ContentWindow.open();
                ContentWindow.center();
            }           
        }

        function DisplayMissingAttendanceDiv() {
            var showAttendanceDiv = $('#' + "<%=this.hidShowAttendanceDiv.ClientID %>").val();
            if (showAttendanceDiv == "Y") {
                $('#divMissingAttendancePopup').css('display', '');
                ContentWindow = $('#divMissingAttendancePopup').kendoWindow({
                    title: "Absent Student Details",
                    visible: false,
                    modal: true,
                    resizable: false,
                    width: '500px',
                    height: '380px'
                }).data("kendoWindow");
                ContentWindow.open();
                ContentWindow.center();
            }
        }

        DisplayClassStudentCountDiv();
        DisplayMissingAttendanceDiv();

        function CloseClassDiv() {
            $("#divClasswiseStudentCount").data("kendoWindow").close();
        }

        function OpenAdmissionPopup() {
            var queryString = $('#' + "<%=this.hidAdmissionQueryString.ClientID %>").val();
            window.open('../Admission/AdmissionFormPopup.aspx?' + queryString, '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1100,height=750');
        }

       function SetAttendanceTooltip() {
            var students = $('#' + _clientHidGetAttendanceSummaryResultStudents).val()
            SetTooltip('spanTotalStudentCount', students)
        }
        function SetClassAttendanceTooltip() {
            var Classes = $('#' + _clientHidGetAttendanceSummaryResultClasses).val()
            SetTooltip('spanStuentClassCount', Classes)
        
        }
      


    </script>
    <script lang ="javascript" type="text/javascript">

          function openMediclaimPopUp(k) {
              if (k == 1)
                  window.open('../DOWNLOADS/School Notices/IRDA -CLAIM REIMBURSEMENT FORM.PART B (1).pdf', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1100,height=750');
              if (k == 2)
                  window.open('../DOWNLOADS/School Notices/IRDA -CLAIM REIMBURSEMENT FORM.pdf', '_new', 'scrollbars=yes,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=20,left=100,width=1100,height=750');
          }
    </script>
</asp:Content>
 