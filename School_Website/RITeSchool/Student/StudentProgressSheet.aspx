<%@ Page Language="C#" MasterPageFile="../MasterPages/MasterPage.master" AutoEventWireup="true" enableEventValidation="false"
    CodeFile="StudentProgressSheet.aspx.cs" Inherits="StudentProgressSheet" ViewStateMode="Disabled"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table width="97%">
            <tr id="trHeader" runat="server" visible="false">
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td class="ClsGrayMainTitle" width="98%" height="20px">
                                <table border="0" cellpadding="0" cellspacing="0" style="padding-right: 5px; height: 15px">
                                    <tr>
                                        <td align="center">
                                            <asp:UpdatePanel ChildrenAsTriggers="true" UpdateMode="Conditional" ID="UpdatePanel3"
                                                runat="server" ViewStateMode="Enabled">
                                                <ContentTemplate>
                                                    <span id="lblToppers" class="MainTitleHead">Old Academic Record</span>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:UpdatePanel ID="upnlSearch" runat="server" ViewStateMode="Enabled" UpdateMode="Always">
                        <ContentTemplate>
                            <asp:ValidationSummary ID="ValSum" runat="server" ShowSummary="true" CssClass="ClsLabel">
                            </asp:ValidationSummary>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trMandatory" runat="server">
                <td align="right">
                    <asp:Label ID="lblmandatory" runat="server" CssClass="ClsMdtStar" Text="* Mandatory Fields"
                        ForeColor="Red" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="UPnlProgressSheet" runat="server" ViewStateMode="Enabled" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <table width="100%">
                                            <tr id="trAcademicYear" runat="server">
                                                <td align="left">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left" width="100px" class="ClsBorderlight" id="tdAcademicYrs" runat="server">
                                                                <span class="ClsLabel" id="lblacademicYr" style="height: 16px; width: 95px">Academic
                                                                    Year :</span>
                                                            </td>
                                                            <td align="left" width="100px">
                                                                <asp:DropDownList ID="cmbAcademicYrId" runat="server" AutoPostBack="true" Width="100px"
                                                                    OnSelectedIndexChanged="cmbAcademicYrId_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td class="ErrHeadNew" align="left">
                                                                <asp:Label ID="lblOldAcademicYear" runat="server"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" ID="hlnkOldAcademicRecord"
                                                                    NavigateUrl="javascript:void(0);" runat="server">Old Academic Records</asp:HyperLink>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlFilter" runat="server">
                                                        <table width="100%" cellpadding="0" cellspacing="1" border="0">
                                                            <tr>
                                                                <td runat="server" id="tdlblTeacher" class="ClsBorderlight">
                                                                    <span class="ClsLabel" id="lblTeacher">Class Teacher :</span>
                                                                </td>
                                                                <td runat="server" id="tdcmbTeachers">
                                                                    <asp:DropDownList ID="cmbTeachers" Width="240px" runat="server" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="cmbTeachers_SelectedIndexChanged" CssClass="LrgCombo">
                                                                    </asp:DropDownList>
                                                                    <asp:CompareValidator ID="cmp_TeacherName" runat="server" ControlToValidate="cmbTeachers"
                                                                        Display="None" ErrorMessage="Class Teacher should be selected." Operator="NotEqual"
                                                                        ValueToCompare='0'></asp:CompareValidator>
                                                                    <span style="color: #ff0000" id="spnMandatory" class="ClsMdtStar">*</span>
                                                                </td>
                                                                <td runat="server" id="tdlblStudent" class="ClsBorderlight">
                                                                    <span class="ClsLabel" id="lblStudent">Student :</span>
                                                                </td>
                                                                <td runat="server" id="tdUPanelStudent">
                                                                    <asp:DropDownList ID="cmbStudents" Width="245px" runat="server" AutoPostBack="True"
                                                                        OnSelectedIndexChanged="cmbStudents_SelectedIndexChanged" CssClass="LrgCombo">
                                                                        <asp:ListItem Text="-- All --" Value="0" Selected="True"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td align="right">
                                                                    <table align="right" cellpadding="0" cellspacing="1" border="0">
                                                                        <tr>
                                                                            <td id="tdbtnShow" runat="server">
                                                                                <asp:Button ID="btnShow" runat="server" CssClass="ClsBtnSml" OnClick="btnShow_Click"
                                                                                    Text="Show" />
                                                                            </td>
                                                                            <td id="tdbtnPrint" runat="server">
                                                                                <asp:Button ID="btnPrint" runat="server" CausesValidation="true" CssClass="ClsBtnMid"
                                                                                    Text="Print Preview" />
                                                                                <asp:HiddenField ID="hidQery" runat="server" />
                                                                            </td>
                                                                            <td colspan="1" id="tdhlnkToppers" runat="server">
                                                                                <asp:HyperLink CssClass="ToprLinkHlilight LblNrmlB ClsPaddingGen" Enabled="False"
                                                                                    ID="hlnkToppers" NavigateUrl="~/RITeSchool/Student/ExamToppersUI.aspx" runat="server"
                                                                                    Target="_blank">Toppers</asp:HyperLink>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Panel ID="pnlErrorMsg" Visible="false" align="center" runat="server" Width="100%">
                                                        <table align="center" width="100%" class="LblNoRecord">
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="lblErrorMsgPre" runat="server" CssClass="ClsConfigText" EnableViewState="False"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td id="Hyper" runat="server" align="left">
                                                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/RITeSchool/Admin/displayassignedclassteacherui.aspx"
                                                                        CssClass="ClsConfigLink">Class Teacher Assignment</asp:HyperLink>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
           <tr id = "trblinkmessgae" runat ="server" visible="false">
               <td align="right">
             <img src="~/images/newLink.gif" id="img1" style="width:40px;height:20px;" runat="server" viewstatemode="Enabled" />
               </td>
          </tr>
            <tr id="trbtnDonloadPDF" runat="server">
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnDownload" runat="server" CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                Text="Download PDF" onclick="btnDownload_Click" Visible = "false" />
                            <asp:Button ID="btnDowloadTerm2Report" runat="server" CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                Text="DOWNLOAD TERM 2 REPORT" Visible = "false" 
                                onclick="btnDowloadTerm2Report_Click" />
                            <asp:Button ID="btnDownloadPrelimReport" runat="server" 
                                CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                Text="DOWNLOAD PRELIM REPORT" onclick="btnDownloadPrelimReport_Click" 
                                Visible="False" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnDownload" />
                            <asp:PostBackTrigger ControlID="btnDowloadTerm2Report" />
                            <asp:PostBackTrigger ControlID="btnDownloadPrelimReport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
             <tr id="trSVPbtnDownload" runat="server" visible="false">
                <td align="center">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnSVPDownload" runat="server" CausesValidation="false" CssClass="ClsBtnMid"                                                                                    
                                Text="Download Report" Visible = "false" onclick="btnSVPDownload_Click" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnSVPDownload" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:Button ID="btnDownloadTestReport" runat="server" CausesValidation="false" CssClass="ClsBtnMid"
                                Text="Download Test Report" Visible = "false" />
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnDownloadTestReport" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
            </tr>
            <tr id="trGradeConfiguration" runat="server">
               <td align="right">
               <asp:UpdatePanel ID="UpdatePanellnkbtnGrade" runat="server" ViewStateMode="Enabled" UpdateMode="Always">
                        <ContentTemplate>
                        <asp:LinkButton ID ="lnkbtnGradeConfigurationDetails"  runat="server"  CssClass="SMSLblSMlBlue" Style="vertical-align: bottom;
                                            padding-left: 10px; font-size: 9pt; font-weight: bold; font-family: Verdana;" Visible="false">Grade Configuration Details</asp:LinkButton>
                        </ContentTemplate>
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />                             
                        </Triggers>
           </asp:UpdatePanel>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:UpdatePanel ChildrenAsTriggers="False" UpdateMode="Conditional" runat="server" ViewStateMode="Enabled"
                        ID="uPnl">
                        <ContentTemplate>
                            <table width="99%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblErrorMsg" runat="server" Visible="False" CssClass="LblNoRecord" EnableViewState="False" Width="100%"></asp:Label>
                                    </td>
                                </tr>
								<tr>
								<td style="height:10px"></td>
								</tr>
								<tr >
                                    <td align="left">
                                        <asp:Label ID="lblBlockProgressReportReason" runat="server" CssClass="ClsConfigText" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnCancelUp" runat="server" Visible="false" BorderStyle="Solid" BorderWidth="1px"
                                            CausesValidation="false" CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back" />
                                    </td>
                                </tr>
                                <tr id="trStudentProgressReport" runat="server">
                                    <td align="center">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:UpdatePanel ID="UPanelStandardt" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="GridViewScrollContainer" runat="server" Visible="true" Style="width: 100%;
                                                                left: 0px;">
                                                            </asp:Panel>
                                                            <asp:Panel ID="ResultContainer" runat="server" Visible="true" Style="overflow: auto;
                                                                width: 100%; left: 0px;">
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                         <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="cmbAcademicYrId" EventName="SelectedIndexChanged" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnCancel" runat="server" BorderStyle="Solid" BorderWidth="1px" CausesValidation="false"
                                            CssClass="ClsBtnSml" OnClick="btnCancel_Click" Text="Back" Visible="True" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                            <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbAcademicYrId" EventName="SelectedIndexChanged" />
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
           </tr>
      <%--    </table>  --%>    
      <tr>
      <td>
        <div id="divPopup"  style="display: none; background-image: url(../images/BGline.gif); background-repeat: repeat;"> 
        <asp:UpdatePanel ID="updpnlGrade" runat="server" ViewStateMode="Enabled" UpdateMode="Always">
            <ContentTemplate>
              <table align="center">
                <tr>
                    <td>                   
                    <asp:ListView ID="lstvwGradeConfigurationDetailsSubject" runat="server" DataKeyNames="Standard_Id">                                        
                                        <LayoutTemplate>
                                            <table cellpadding="0" cellspacing="0" width="300px">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">Subjects :</span>
                                                    </td>
                                                </tr>
                                            </table>
                                                <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                                <tr align="right" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;">
                                                     Percentage                                                    
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;">
                                                       Grade Name
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;" id="thRemarkSub" runat="server">
                                                       Remarks
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>
                                         </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblStartingMarkRange" runat="server" Text='<%# Eval("Starting_Marks_Range") %>' /> -                                                                                
                                                    <asp:Label ID="lblEndingMarkRange" runat="server" Text='<%# Eval("Ending_Marks_Range") %>' />  
                                                </td>
                                                <td align="left" class="paddingL">
                                                   <asp:Label ID="lblGradeName" runat="server" Text='<%# Eval("Grade_Name") %>' />  
                                                </td>
                                                <td align="left" class="paddingL" id="tdRemark" runat="server">
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />  
                                                </td>                                               
                                           </tr>
                                         </ItemTemplate>
                     </asp:ListView>
                   </td>
                 </tr>        
                 <tr>
                   <td>                   
                       <asp:ListView ID="lstvwGradingConfigurationDetailsCurricularSubject" runat="server"                        
                           DataKeyNames="Standard_Id">                                        
                                        <LayoutTemplate>
                                            <table cellpadding="0" cellspacing="0" width="300px">
                                                <tr>
                                                    <td style="height: 40px" id="trLbl" runat="server" align="left">
                                                        <span class="ClsLblLgnd">Co-Curricular Subjects :</span>
                                                    </td>
                                                </tr>
                                            </table>         
                                                                              
                                            <table cellpadding="0" cellspacing="1" class="GridBorder" width="100%" style="color: #333333">
                                                <tr align="center" id="trHeader" runat="server" class="ClsGridHeader">
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;">
                                                     Percentage                                                    
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;">
                                                       Grade Name
                                                    </th>
                                                    <th align="left" class="paddingL" style="width: 30%; font-size: 9pt;" id="thRemarkSub" runat="server">
                                                       Remarks
                                                    </th>
                                                </tr>
                                                <tr id="itemPlaceholder" runat="server">
                                                </tr>
                                            </table>                                           
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr id="trGridRow" runat="server" class="ClsGridRow">
                                                <td align="left" class="paddingL">
                                                    <asp:Label ID="lblStartingMarkRange" runat="server" Text='<%# Eval("Starting_Marks_Range") %>' /> -                                                                                
                                                    <asp:Label ID="lblEndingMarkRange" runat="server" Text='<%# Eval("Ending_Marks_Range") %>' />  
                                                </td>
                                                <td align="left" class="paddingL">
                                                   <asp:Label ID="lblGradeName" runat="server" Text='<%# Eval("Grade_Name") %>' />  
                                                </td>
                                                <td align="left" class="paddingL" id="tdRemark" runat="server">
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>' />    
                                                </td>                                               
                                           </tr>
                                        </ItemTemplate>
                           </asp:ListView>
                      </td>
                  </tr>                               
                  <tr>
                           <td align="center">                              
                                <asp:Button ID="btnClose" Text="Close" CssClass="ClsBtn" runat="server" CausesValidation="false" 
                                   OnClientClick="HidePopup();" />
                           </td>
                  </tr>
               </table>
                </ContentTemplate>
                        <Triggers>
                             <asp:AsyncPostBackTrigger ControlID="btnShow" EventName="Click" />
                             <asp:AsyncPostBackTrigger ControlID="cmbStudents" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbTeachers" EventName="SelectedIndexChanged" />
                            <asp:AsyncPostBackTrigger ControlID="cmbAcademicYrId" EventName="SelectedIndexChanged" />
                        </Triggers>
            </asp:UpdatePanel>
        </div>
        </td>
        </tr>
        <tr>
            <td>
                <div id="divTests"  style="display: none; background-image: url(../images/BGline.gif); background-repeat: repeat;"> 
                    <table width="100%">
                        <tr>
                            <td align="center">
                                <asp:DropDownList ID="cmbTests" runat="server" CssClass="ExLrgCombo" style="width:200px;">
                                    <asp:ListItem Text="test1 fsf sdfdss s ret ret retretretert " Value="1"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                               <asp:Button ID="btnDownloadTest" runat="server" Text="Download" CssClass="ClsBtn" OnClientClick="DownloadTestReport(); return false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        </table>
    </div>
    <asp:HiddenField ID="hidSchoolId" runat="server" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidStandardId" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidDivisionId" runat="server" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidStandardDivisionId" runat="server" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidStudId" runat="server" Value="0" ViewStateMode="Enabled"/>    
	<asp:HiddenField ID="hidCurrentAcademicYrId" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidUserHasFullAccess" runat="server" Value="False" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidSubName" runat="server" ViewStateMode="Enabled"></asp:HiddenField>
    <asp:HiddenField ID="hidRowSpan" runat="server" ViewStateMode="Enabled"></asp:HiddenField>
    <asp:HiddenField ID="hidRowNo" runat="server" Value="-1" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidStudentIdForReport" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidShowCurrentYearData" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidStandardName" runat="server" Value="" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidIsPendingFee" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidOldStdDivId" runat="server" Value="0" ViewStateMode="Enabled"/>
    <asp:HiddenField ID="hidOldStudentId" runat="server" Value="0" ViewStateMode="Enabled"/>

    <asp:UpdatePanel ID="upnl11" runat="server" UpdateMode="Always"><ContentTemplate>
    <asp:HiddenField ID="hidLastAcademicYrId" runat="server" Value="0" ViewStateMode="Enabled"/>
    </ContentTemplate></asp:UpdatePanel>

    <script language="javascript" type="text/javascript">

        _sClientbtnPrint = "<%=this.btnPrint.ClientID %>";
        _sClientbtnCancel = "<%=this.btnCancel.ClientID %>";
        _sClienthidCurrentAcademicYrId = "<%=this.hidCurrentAcademicYrId.ClientID %>";
        _clientcmbTests =  "<%=cmbTests.ClientID %>"
        _clienthidStandardId = "<%=this.hidStandardId.ClientID %>"
        _clienthidStandardDivisionId = "<%=this.hidStandardDivisionId.ClientID %>"
        _clienthidStudId = "<%=this.hidStudId.ClientID %>"
        _clienthidStudentIdForReport = "<%=this.hidStudentIdForReport.ClientID %>"
        _clienthidShowCurrentYearData = "<%=this.hidShowCurrentYearData.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance();
          prm.add_endRequest(EndReqHandler);

          function EndReqHandler(sender, args) {
          	var postBackElement = sender._postBackSettings.sourceElement;
          	if (postBackElement.id == _sClientbtnPrint) {
          		GeneratePrint();
          	}
          	if (postBackElement.id == _sClientbtnCancel) {
          		CloseWindow();
          	}
          }

          function CloseWindow() {
            if ($get(_clienthidShowCurrentYearData).value != "1")
                document.location.reload(true);
            window.close();
        }

        function GeneratePrint() {
            _sClienthidQery = "<%=this.hidQery.ClientID %>";
	        var validationResult = true;
	        if (typeof (Page_ClientValidate) == 'function') {
                validationResult = Page_ClientValidate("");
	        }
            if (validationResult == false) {
                return false;
            }
            else
                window.open("../Student/StudentProgressSheetPrint.aspx?" + document.getElementById(_sClienthidQery).value, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=' + screen.width + ' ,height=600');
            return false;
        }

        $(document).ready(function () {
            $("#" + _clientcmbTests).kendoDropDownList();
        });

        function ShowToppers(sQryStr) {
            _sClienthlnkToppers = "<%=this.hlnkToppers.ClientID %>";
	        if ((document.getElementById(_sClienthlnkToppers) == null) || (document.getElementById(_sClienthlnkToppers) == "") || (document.getElementById(_sClienthlnkToppers).disabled))
                return false;
	        window.open(sQryStr, '_blank', 'scrollbars=yes,resizable=no,top=0,left=0,width=900,height=600');
	        return false;
        }
        function ValidateMaxLength(val, maxLength) {
            if (val.value.length > maxLength) {
                val.value = val.value.substring(0, maxLength);
	            return false;
            }
            return true;
        }
        function OpenPopup() {
            $('#divPopup').show(); ContentWindow = $('#divPopup').kendoWindow({ title: "Grade Configuration Details", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }
        function HidePopup() {
            ContentWindow = $('#divPopup').kendoWindow({ title: "Grade Configuration Details", visible: false, modal: true, resizable: false, width: '350px' }).data("kendoWindow"); ContentWindow.close(); ContentWindow.center();
        }

        function OpenTestPopup() {
            $('#divTests').show(); ContentWindow = $('#divTests').kendoWindow({ title: "Exams", visible: false, modal: true, resizable: false, width: '250px' }).data("kendoWindow"); ContentWindow.open(); ContentWindow.center();
        }

        function DownloadTestReport() {
            var testId = $get(_clientcmbTests).value

            if (testId != 0) {
                var data = '{"asStandardId":"' + $get(_clienthidStandardId).value + '","asStdDivId":"' + $get(_clienthidStandardDivisionId).value + '","asTestId":"' + testId + '","asStudentId":"' + $get(_clienthidStudentIdForReport).value + '"}'

                $.ajax({ type: "POST", data: data, url: "StudentProgressSheet.aspx/GetQueryString", contentType: "application/json; charset=utf-8", dataType: "json", success: function (msg) {
    
                    var isChrome = !!window.chrome && !!window.chrome.webstore;

                    var target = '_new'
                    if (isChrome)
                        target = '_blank'
                    window.open(msg.d, target, 'scrollbars=yes,resizable=no,top=0,left=0,width=10,height=10')

                    return false
                }, error: function (msg) { }
                });
            }
            else
                alert('Please select Exam.')
        }
      
    </script>
</asp:Content>
