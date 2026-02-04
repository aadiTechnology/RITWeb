<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master"
    AutoEventWireup="true" CodeFile="PerformanceEvaluationUI.aspx.cs" Inherits="PerformanceEvaluationUI" %>

<%@ Register Assembly="RJS.Web.WebControl.PopCalendar.Net.2008" Namespace="RJS.Web.WebControl"
    TagPrefix="rjs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainBody" runat="Server">
    <div class="MainBodyDiv">
        <table align="center" border="0" cellpadding="0" cellspacing="0" width="97%">
        <tr>
        <td align="right">
        <span class="ClsMdtStar">* Mandatory Fields</span>
        </td>
        </tr>
            <tr>
                <td align="left" id="td1" runat="server">
                    <asp:ValidationSummary ID="valSum" runat="server" CssClass="ClsMdtStar" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Display="None" ErrorMessage="Classes Taught should not be blank." ControlToValidate="txtClassestaught"></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Display="None" ErrorMessage="Teaching Subjects should not be blank." ControlToValidate="txtTeachersubjects"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cstValGrade" runat="server" ClientValidationFunction="ValidateGrade"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstvalObservation" runat="server" ClientValidationFunction="ValidateObservation"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValObservationLength" runat="server" ClientValidationFunction="ValidateObservationLength"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>                    
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ClientValidationFunction="ValidateEffectiveDate"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>
                    <asp:CustomValidator ID="cstValidateApproverObservation" runat="server" ClientValidationFunction="ValidateApproverObservation"
                        SetFocusOnError="True" Display="None" ErrorMessage=""></asp:CustomValidator>                    
                </td>
            </tr>
            <tr>
                <td align="center" id="tdMessage" runat="server">
                    <asp:Label ID="lblMessage" runat="server" EnableViewState="false" CssClass="ClsLabelNrml"
                        ForeColor="Blue" Style="text-align: center"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="center" id="tdFinalApprover" runat="server">
                    <table width="70%">
                        <tr>
                            <td align="center" class="LblNoRecord">
                                <asp:Label ID="lblAdminMessage" runat="server" EnableViewState="false" Text="<%$ Resources:LocalizedResources, msgStaffPerformanceEvalNotPublished%>"
                                    ForeColor="Blue" Style="text-align: center"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnBackUp" runat="server" Text="<%$ Resources:LocalizedResources, Back%>"
                                    CssClass="ClsBtn" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%" id="tblEvaluation" runat="server">
                        <tr>
                            <td align="center">
                                <table width="70%" id="tblSchoolDetails" runat="server">
                                    <tr>
                                        <td align="center" class="SocietyName">
                                            <asp:Label ID="lblOrgName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ActualSchoolName">
                                            <asp:Label ID="lblSchoolName" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ActualSchoolName">
                                            <asp:Label ID="lblSchoolAddress" runat="server" Style="font-size: 15px;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" class="ClsReportHead">
                                            <asp:Label ID="lblStaffPerformance" runat="server" Text="<%$ Resources:LocalizedResources, StaffPerformanceEvaluation%>"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                        
                              

                        <tr>
                            <td align="center">
                                <table width="70%" id="tblUserDetails" runat="server">                                    
                                    <tr>
                                        <td align="left" colspan="4">
                                            <asp:Label ID="lblFormFor" runat="server" CssClass="ClsHilightTextB" Style="font-weight: bold;
                                                color: Maroon;"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="20%" class="ClsBorderlight">
                                            <span class="ClsLabel">Status</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" width="30%">
                                            <asp:Label ID="lblJobStatus" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                        <td align="left" width="20%" class="ClsBorderlight">
                                            <span class="ClsLabel">Year</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" width="30%">
                                            <asp:Label ID="lblAcademicYear" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" width="20%" class="ClsBorderlight">
                                            <span class="ClsLabel">Name</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" width="30%">
                                            <asp:Label ID="lblName" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                        <td align="left" width="20%" class="ClsBorderlight">
                                            <span class="ClsLabel">Post</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" width="30%">
                                            <asp:Label ID="lblDesignation" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Employee Code</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblEmployeeNo" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Length Of Service</span> <span class="ClsLabel colonPadding">
                                                :</span>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblServiceLength" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Date of Joining</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblJoiningDate" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Date of Last Increment</span> <span class="ClsLabel colonPadding">
                                                :</span>
                                        </td>
                                        <td align="left">                                            
                                             <asp:TextBox ID="txtLastIncrementDate" CssClass="MidTxtBox" runat="server" ReadOnly="true" />
                                            <rjs:PopCalendar ID="cal_LastIncrementDate" runat="server" Control="txtLastIncrementDate" Format="dd MMM yyyy" Culture = "en"
                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Effective From Date  should not be blank."
                                                AutoPostBack="False" To-Today ="true" />                                            
                                            <asp:Label ID="lblLastIncrementDate" runat="server" CssClass="ClsHilightTextB" EnableViewState="true"></asp:Label>
                                        </td>                                      
                                    </tr>    
                                     <tr>
                                        <td align="left"  class="ClsBorderlight">
                                            <span class="ClsLabel">Address</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left">
                                            <asp:Label ID="lblAddress" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                        <td align="left"  class="ClsBorderlight">
                                            <span class="ClsLabel">Highest Education Qualification and Year of passing</span> <span class="ClsLabel colonPadding"></span>
                                        </td>
                                        <td align="left" >
                                            <asp:Label ID="lblHighestEducation" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                     </tr> 
                                           <tr id="trStandards" runat="server" visible="false">
                                               <td align="left"  class="ClsBorderlight">
                                            <span class="ClsLabel">Classes taught</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:TextBox ID="txtClassestaught" CssClass="MidTxtBox" style="width:98%" runat="server" MaxLength="100" />
                                            <span class="ClsMdtStar">*</span>
                                            
                                        </td>
                                        </tr>
                                        <tr>
                                       <td align="left"  class="ClsBorderlight">
                                            <span class="ClsLabel">Teaching Subjects</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="3">
                                              <asp:TextBox ID="txtTeachersubjects" CssClass="MidTxtBox" style="width:98%"  runat="server"  MaxLength="100" />
                                              <span class="ClsMdtStar">*</span>
                                              
                                        </td>
                                   
                                     </tr>                       
                                   <%-- <tr id="trStandards" runat="server" visible="false">
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Grades Taught</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:Label ID="lblStandardTaught" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                    </tr>--%>
                                    <tr id="trSubjects" runat="server" visible="false">
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Subjects Taught</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:Label ID="lblSubjectTaught" runat="server" CssClass="ClsHilightTextB"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height:10px;">
                                        <td colspan="4">
                                        </td>
                                    </tr>
                                    <tr id="trEffectiveDate" runat="server" visible="false">
                                        <td align="left" class="ClsBorderlight">
                                            <span class="ClsLabel">Effective From Date</span> <span class="ClsLabel colonPadding">:</span>
                                        </td>
                                        <td align="left" colspan="3">
                                            <asp:TextBox ID="txtEffectiveFromDate" CssClass="MidTxtBox" runat="server" ReadOnly="true" />
                                            <rjs:PopCalendar ID="cal_EffectiveFromDate" runat="server" Control="txtEffectiveFromDate" Format="dd MMM yyyy" Culture = "en"
                                                ShowWeekend="True" ShowErrorMessage="false" InvalidDateMessage="Effective From Date should not be blank."
                                                AutoPostBack="False" From-Today ="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="tblGrades" runat="server" width="70%">
                                </table>
                            </td>
                        </tr>
                     
                        <tr>
                            <td align="center">
                                <table id="tblParameter" runat="server" width="70%">
                                </table>
                            </td>
                        </tr>   
                        <tr>                        
                        <td align="center">
                                <table id="tblLinks" runat="server" width="70%">
                                </table>                      
                        </td>
                        </tr>           
                        <tr id="trButtons" runat="server">
                            <td align="center">
                                <asp:Button ID="btnBack" runat="server" Text="<%$ Resources:LocalizedResources, Back%>"
                                    CssClass="ClsBtn" CausesValidation="false" />                                   
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LocalizedResources, Save%>"
                                    CssClass="ClsBtn" OnClick="btnSave_Click" UseSubmitBehavior="False" CausesValidation="true"/>
                                <asp:Button ID="btnSubmit" runat="server" Text="<%$ Resources:LocalizedResources, Submit%>"
                                    CssClass="ClsBtn" UseSubmitBehavior="False" CausesValidation="true" OnClick="btnSubmit_Click" />
                                <asp:Button ID="btnPublish" runat="server" Text="<%$ Resources:LocalizedResources, Publish%>"
                                    CssClass="ClsBtn" Width="100px" OnClick="btnPublish_Click" CausesValidation="true"
                                    UseSubmitBehavior="False" />
                                <asp:Button ID="btnViewReport" runat="server" Text="<%$ Resources:LocalizedResources, ViewReport%>"
                                    CssClass="ClsBtn" Width="100px" CausesValidation="false" />
                                <asp:Button ID="btnRejectSubmittion" runat="server" Text="Reject Submission" 
                                    CssClass="ClsBtn" OnClientClick="OpenRejectionPopup(); return false;" Visible="false"
                                    Width="125px" CausesValidation="false" />                              
                                <asp:HiddenField ID="hidIsViewMode" runat="server" Value="N" />
                                <asp:HiddenField ID="hidValGradeSelected" runat="server" Value="" />
                                <asp:HiddenField ID="hidvalBlankObservation" runat="server" Value="" />
                                <asp:HiddenField ID="hidvalObservationLength" runat="server" Value="" />
                                <asp:HiddenField ID="hidvalActionSaveandPublish" runat="server" Value="" />
                                <asp:HiddenField ID="hidvalActionSaveandSubmit" runat="server" Value="" />
                                <asp:HiddenField ID="hidvalActionUnPublish" runat="server" Value="" />
                                <asp:HiddenField ID="hidIsPublishAction" runat="server" Value="Y" />
                                <asp:HiddenField ID="hidIsFinalApprover" runat="server" Value="N" />
                                <asp:HiddenField ID="hidBtnState" runat="server" Value="" />
                                <asp:HiddenField ID="hidQueryString" runat="server" Value="N" />   
                                <asp:HiddenField ID="hidIsLoginUser" runat="server" Value="N" />                          
                            </td>
                        </tr>
                        <tr id="trButtonClose" runat="server" visible="false">
                            <td align="center">
                                <asp:Button ID="btnClose" runat="server" Text="<%$ Resources:LocalizedResources, Close%>"
                                    CssClass="ClsBtn" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:UpdatePanel ID="upnlTimer" runat="server">
                                    <ContentTemplate>
                                        <asp:Timer ID="timer" runat="server" Interval="60000" Enabled="false" 
                                            ontick="timer_Tick">
                                        </asp:Timer>                                        
                                    </ContentTemplate>
                                    <Triggers>
                                        <%--<asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />--%>
                                        <asp:AsyncPostBackTrigger ControlID="timer" EventName="Tick" />
                                        <%--<asp:AsyncPostBackTrigger ControlID="btnSubmit" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnContinue" EventName="Click" />--%>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>        
        <div id="RejectionDiv" runat="server" style="visibility: hidden; display: none; position: fixed;
                    padding: 0px; width: 500px; height: 180px; border-width: 0px; left: 5px;
                    top: 0px; line-height: normal; border: solid 2px darkgreen; margin: 300px 500px;
                    background-color: white">
                    <div style="background-color: Transparent; padding-top: 3px; height: 30px; background-image: url(../images/GridHeaderBG.gif);
                        background-repeat: repeat-x; color: Black; text-align: right;">
                        <div style="font-size: 12px; width: 170px; letter-spacing: 1px; padding-left: 8px;
                            font-weight: bold; color: darkgreen; float: left; height: 10px" align="left">
                            <asp:Label ID="Label2" runat="server" Text="Reject Last submit"></asp:Label>
                        </div>
                        <span style="cursor: hand" onclick="javascript:CloseRejectionPopup(); return false;">
                            <img alt="Hide Popup" class="img-align-top" src="../images/close_vista.gif"
                                border="0" />
                        </span>
                    </div>
                    <div align="center" style="padding: 2px; text-align: center; vertical-align: top;
                        color: #333; overflow: auto; height: 140px; width: 475px; margin-left: 5px; background-color: white"
                        id="Div4">
                        <table width="100%">
                            <tr>
                                <td align="left" class="ClsBorderlight" width="100px">
                                    <asp:Label ID="Label3" runat="server" Text="Rejection Reply Text :" CssClass="ClsLabel"></asp:Label>
                                </td>
                                <td align="left" valign="middle">
                                    <asp:TextBox ID="txtReason" runat="server" CssClass="ExLrgTxtBox" TextMode="MultiLine" Height="100px" Width="350px"></asp:TextBox>
                                    <span class="ClsErroMsg">*</span>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                     <asp:Button ID="btnContinue" runat="server" Text="Continue" CssClass="ClsBtn" OnClick="btnContinue_Click" CausesValidation="false" UseSubmitBehavior="False" />
                                    <asp:Button ID="btnClosePopup" runat="server" Text="Close" CssClass="ClsBtn" CausesValidation="false" OnClientClick="CloseRejectionPopup(); return false;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

    </div>
    <script type="text/javascript" language="javascript">
        _clienthidValGradeSelected = "<%=this.hidValGradeSelected.ClientID %>"
        _clienthidvalBlankObservation = "<%=this.hidvalBlankObservation.ClientID %>";
        _clienthidvalObservationLength = "<%=this.hidvalObservationLength.ClientID %>"
        _clienthidvalActionSaveandPublish = "<%=this.hidvalActionSaveandPublish.ClientID %>";
        _clienthidvalActionSaveandSubmit = "<%=this.hidvalActionSaveandSubmit.ClientID %>";
        _clienthidvalActionUnPublish = "<%=this.hidvalActionUnPublish.ClientID %>";
        _clienthidIsPublishAction = "<%=this.hidIsPublishAction.ClientID %>"
        _clienthidIsFinalApprover = "<%=this.hidIsFinalApprover.ClientID %>"
        _ClienthidQueryString = "<%=this.hidQueryString.ClientID %>"
        _clienthidIsLoginUser = "<%=this.hidIsLoginUser.ClientID %>"

        
        function OpenReport(querystring) {
            window.open('PerformanceEvaluationUI.aspx?' + querystring, '_new', 'scrollbars=yes,resizable=no,top = 50,left=50,width=900,height=600')
        }

        function ConfirmPublish(isPublish) {
            $get("<%=this.lblMessage.ClientID %>").innerHTML = "";
            var validationResult = false;
            if (typeof (Page_ClientValidate) == 'function')
                validationResult = Page_ClientValidate("");

            if (validationResult) {
                if (isPublish == 1)
                    return confirm($get(_clienthidvalActionSaveandPublish).value);
                else
                    return confirm($get(_clienthidvalActionUnPublish).value);
            }
        }

        function ConfirmSubmit() {
            return confirm($get(_clienthidvalActionSaveandSubmit).value);
        }

        function ValidateGrade(oSrc, args) {            
            var sRows = ""
            var IsPublishAction = $('#' + _clienthidIsPublishAction).val()            

            if (IsPublishAction == "Y" || IsPublishAction == "") {
                var grades = document.getElementsByTagName("select");
                for (var k = 0; k < grades.length; k++) {
                    var grade = grades[k]
                    if (grade.value == 0 && grade.value != "") {

                        if (sRows.match((k + 1)) == null)
                            sRows = sRows + ", " + (k + 1)
                    }
                }

                if (sRows != "") {
                    sRows = sRows.substring(1)
                    oSrc.errormessage = $get(_clienthidValGradeSelected).value + sRows;
                    args.IsValid = false;
                    return true;
                }
            }

            args.IsValid = true;
            return false;
        }

        function ValidateObservation(oSrc, args) {
//            var sRows = ""
//            var observations = document.getElementsByTagName("textarea");
//            for (var k = 0; k < observations.length; k++) {
//                var observation = observations[k]
//                if (observation.value.trim() == "") {
//                    if (sRows.match((k + 1)) == null)
//                        sRows = sRows + ", " + (k + 1)
//                }
//            }

//            if (sRows != "") {
//                sRows = sRows.substring(1)
//                oSrc.errormessage = $get(_clienthidvalBlankObservation).value + sRows;
//                args.IsValid = false;
//                return true;
            //                        }
              
            var found = false;
            var observations = document.getElementsByTagName("textarea");

            for (var k = 0; k < observations.length; k++) {
                var observation = observations[k]
                if (observation.value.trim() != "") {                    
                    found = true;
                    break;
                }
            }

            if (!found) {
                oSrc.errormessage = 'Value for at least one observation should be set.';
                args.IsValid = false;
                return true;
            }
            
            args.IsValid = true;
            return false;
        }

        function ValidateApproverObservation(oSrc, args) {            
            var IsLoginUser = $('#' + _clienthidIsLoginUser).val()
            if (IsLoginUser == "N") {
                var observations = $('[id*=txtObservation]')[0];

                if (observations.value == "") {
                    oSrc.errormessage = "Observation should not be blank.";
                    args.IsValid = false;
                    return true;
                }
            }             
            args.IsValid = true;
            return false;
        }

        function ValidateObservationLength(oSrc, args) {
        
            var sRows = ""
            var observations = document.getElementsByTagName("textarea");
            
            for (var k = 0; k < observations.length; k++) {
                var observation = observations[k]
                if (observation.value.trim() != "" && observation.value.trim().length > 4000) {
                    if (sRows.match((k + 1)) == null)
                        sRows = sRows + ", " + (k + 1)

                }
            }

            if (sRows != "") {
                sRows = sRows.substring(1)
                //oSrc.errormessage = "Observation length should not be greater than 500 characters for row(s) : " + sRows;
                oSrc.errormessage = "Observation length should not be greater than 4000 characters."
                args.IsValid = false;
                return true;
            }

            args.IsValid = true;
            return false;
        }

        function ValidateReason() {
            var reason = $('#' + "<%=this.txtReason.ClientID %>").val();
            reason = reason.trim()
            if (reason == "") {
                alert('Rejection Reply Text should not be blank.');
                return false;
            }
            else if (reason.length > 500) {
                alert('Rejection Reply Text length should not be greater than 500 characters.')
                return false;
            }
            return true;
        }

        function OpenRejectionPopup() {
            var cssstyle = $get("<%=this.RejectionDiv.ClientID %>").style
            var txt = "<%=this.txtReason.ClientID %>";
            cssstyle.visibility = "visible"
            cssstyle.display = "block"
//            window.scrollTo(0, 0);

            if (document.getElementById(txt) != null)
                document.getElementById(txt).focus();

            //setPopupPosition()
        }

        function CloseRejectionPopup() {
            $get("<%=this.RejectionDiv.ClientID %>").style.visibility = "hidden"
            $get("<%=this.RejectionDiv.ClientID %>").style.display = "none"
            $('#' + "<%=this.txtReason.ClientID %>").val("");
            return false
        }

        function ValidateEffectiveDate(oSrc, args) {
            var isFinalApprover = $('#' + _clienthidIsFinalApprover).val()

            if (isFinalApprover == "Y") {
                var effectiveDate = $('#' + "<%=this.txtEffectiveFromDate.ClientID %>").val()
                if (effectiveDate.trim() == "") {
                    oSrc.errormessage = "Effective From Date should not be blank.";
                    args.IsValid = false
                    return true
                }
            }

            args.IsValid = true
            return false
        }

//        function UpdateObservationLength(txt, lbl) {
//            
//            
//            $('#' + lbl).text(txt.value.length)

//            document.getElementById(lbl).innerHTML = txt.value.length

//            alert(document.getElementById(lbl).innerHTML)
//        }

    </script>

    <script language="javascript" type="text/javascript">

//        _cltdivTemplates = "<%=this.RejectionDiv.ClientID %>"

//        var _adjWinHeight;
//        var _adjWinWidth;

//        var _totalWinHeight;        
//        var _rightFooterPos;        
//        var _rightPosition;

//        window.onresize = setPopupPosition;
//        window.onscroll = setPopupPosition;
//        window.onload = setPopupPosition;

//        function setPopupPosition() {
//            _totalWinHeight = document.body.scrollHeight;
//            _adjWinHeight = _totalWinHeight; //-608;
//            _adjWinWidth = document.body.scrollWidth;

//            if (document.getElementById(_cltdivTemplates) != null) {

//                _rightFooterPos = document.body.clientHeight - parseInt(document.getElementById(_cltdivTemplates).style.height);
//                alert("Client Height : " + document.body.clientHeight)
//                document.getElementById(_cltdivTemplates).style.top = _rightFooterPos;
//            }

//            if (document.getElementById(_cltdivTemplates) != null) {
//                _rightPosition = parseInt(screen.width / 2) - parseInt(parseInt(document.getElementById(_cltdivTemplates).style.width) / 2);                
//                document.getElementById(_cltdivTemplates).style.left = _rightPosition;
//            }

//            window_onscroll();
//        }

//        function window_onscroll() {
//            if (document.body.scrollTop <= _adjWinHeight) {
//                if (document.getElementById(_cltdivTemplates) != null) {                    
//                    document.getElementById(_cltdivTemplates).style.top = document.body.scrollTop + 200;
//                }
//            }

//            if (document.body.scrollLeft <= _adjWinWidth) {
//                if (document.getElementById(_cltdivTemplates) != null) {                    
//                    document.getElementById(_cltdivTemplates).style.left = document.body.scrollLeft + 350
//                }
//            }
//        }


        function RefreshLinkButton(Count, ClientId) {
            document.getElementById("ctl00_MainBody_" + ClientId).innerHTML = Count + " Files Uploaded";
        }
    </script>

   

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
