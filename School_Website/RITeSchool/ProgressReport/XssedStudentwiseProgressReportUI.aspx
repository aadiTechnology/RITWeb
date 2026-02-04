<%@ Page Title="" Language="C#" MasterPageFile="~/RITeSchool/MasterPages/MasterPage.master" EnableEventValidation="false"
    AutoEventWireup="true" CodeFile="XssedStudentwiseProgressReportUI.aspx.cs" Inherits="XssedStudentwiseProgressReportUI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
    <table align="center" border="0" cellpadding="0" style="width: 100%; vertical-align: top">
        <tr>
            <td align="center">
                <table width="100%">
                    <tr>
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        <asp:ValidationSummary ID="valSummary" runat="server" ValidationGroup="Save" CssClass="ClsLabel"
                                            ForeColor="Red" />
                                        <asp:ValidationSummary ID="valSummary1" runat="server" ValidationGroup="Show" CssClass="ClsLabel"
                                            ForeColor="Red" />
                                    </td>
                                    <td align="right">
                                        <span class="ClsLabelNrml" style="color: Red;">
                                        * <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LocalizedResources, MandatoryFields %>"></asp:Label>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trFilterSelection" runat="server">
                        <td align="center">
                            <table>
                                <tr>
                                    <td colspan="3">
                                        <asp:UpdatePanel ID="updtpnl2" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <asp:Label ID="lblSuccessfulMsg" Style="text-align: center" runat="server" ForeColor="blue"
                                                    Width="100%" CssClass="ClsConfigText"></asp:Label>                                                    
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                                <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
                                                
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100px" class="ClsBorderlight">
                                        <span class="ClsLabel"><asp:Label ID="Label3" runat="server" Text="<%$ Resources:LocalizedResources, Assessment %>"></asp:Label>
                                        <span id="Span2" class="colonPadding">:</span>
                                        </span>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="cmbAssessment" runat="server" CssClass="LrgCombo" AutoPostBack="true" 
                                            onselectedindexchanged="cmbAssessment_SelectedIndexChanged" ></asp:DropDownList>
                                        
                                    </td>
                                    <td align="right" colspan="2">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <%--<asp:Button ID="btnShow" runat="server" Text="Show" CssClass="ClsBtn" ValidationGroup="Show"
                                                        OnClick="btnShow_Click" />--%>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="0"
                                            ValidationGroup="Show" Display="None" ErrorMessage="<%$ Resources:LocalizedResources, AssessmentShouldSelected %>"
                                            ControlToValidate="cmbAssessment"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <%--<tr id="trOldAcademicYear" runat="server">
                        <td>
                            <table width="100%">
                                <tr>
                                    <td align="left" width="100px" class="ClsBorderlight" id="tdAcademicYrs" runat="server">
                                        <span class="ClsLabel" id="lblacademicYr" style="height: 16px; width: 95px">Academic
                                            Year :</span>
                                    </td>
                                    <td align="left" width="100px">
                                        <asp:DropDownList ID="cmbAcademicYear" runat="server" AutoPostBack="true" Width="100px">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="ErrHeadNew" align="left">
                                        <asp:Label ID="lblOldAcademicYear" runat="server" EnableViewState="False"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:HyperLink CssClass="AtteendeceToppers ClsPaddingGen" Height="15px" ID="hlnkOldAcademicRecord"
                                            NavigateUrl="#" runat="server" Target="_blank">Old Academic Records</asp:HyperLink>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>--%>
                    <tr id="tblProgressReportDetails" runat="server">
                        <td align="center">
                            <table id="tblMainProgressReport" runat="server" width="850px">
                            </table>
                        </td>
                    </tr>
                    <tr id="trErrorMessage" runat="server" visible="false">
                        <td align="center">
                            <asp:Label ID="lblErrorMsg" runat="server" CssClass="LblNoRecord" Text="<%$ Resources:LocalizedResources, MsgAssessmentResultUnAvailable %>"
                                EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trPrecondition" runat="server">
                        <td>
                            <div runat="server" id="divErr">
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="center">
                <%--<asp:CustomValidator ID="cstvalObservations" runat="server" Display="None" ClientValidationFunction="Validate"
                    ValidationGroup="Save" ErrorMessage="Observation(s) should not be blank."></asp:CustomValidator>--%>
                <asp:CustomValidator ID="cstvalLearningOutcomeGrades" runat="server" Display="None" ClientValidationFunction="ValidateLearningOutcomeGrades"
                    ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, GradeShouldBeSelected%>"></asp:CustomValidator>
                <asp:CustomValidator ID="cstvalNonXseedSubjectGrades" runat="server" Display="None" ClientValidationFunction="ValidateNonXseedSubjectGrades"
                    ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, GradeShouldBeSelected%>"></asp:CustomValidator>
                <asp:CustomValidator ID="cstvalCoCurricularSubjectGrades" runat="server" Display="None" ClientValidationFunction="ValidateCoCurricularSubjectGrades"
                    ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, GradeShouldBeSelected%>"></asp:CustomValidator>
                <asp:CustomValidator ID="cstvalObservationsLength" runat="server" Display="None"
                    ClientValidationFunction="ValidateObservationLength" ValidationGroup="Save" ErrorMessage="<%$ Resources:LocalizedResources, ObservationShouldNotBeGreaterThanCharacters %>"></asp:CustomValidator>
                <asp:CustomValidator ID="cstValRemarkLength" runat="server" Display="None" ClientValidationFunction="ValidateRemark"
                    ValidationGroup="Save" ErrorMessage="Remark length should not be greater than 300 characters."></asp:CustomValidator>
                <asp:UpdatePanel ID="updtpnl1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnBack" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Back %>" CausesValidation="false"
                            OnClick="btnBack_Click" />
                        <asp:Button ID="btnSave" runat="server" ValidationGroup="Save" CssClass="ClsBtn" disable-page="true"
                            Text="<%$ Resources:LocalizedResources, Save %>" Visible="false" CausesValidation="true" OnClick="btnSave_Click" />
                        <asp:Button style="width: 90px" ID="btnPublish" runat="server" CssClass="ClsBtn" Text="<%$ Resources:LocalizedResources, Publish %>" Visible="false"
                            CausesValidation="true"  ValidationGroup="Save" OnClick="btnPublish_Click" />
                        <asp:Button ID="btnView" runat="server" CausesValidation="false" CssClass="ClsBtnLrg"
                            Text="<%$ Resources:LocalizedResources, ViewProgressReport %>" Visible="false" OnClick="btnView_Click" />
                            <asp:HiddenField ID="HidPublish" runat="server"  />
                            <asp:HiddenField ID="HidUnpublish" runat="server"  />
                            <asp:HiddenField ID="hidViewProgressReport" runat="server"  />
                            <asp:HiddenField ID="hidObservationShouldNotBeGreaterThanCharacters" runat="server"  />
                            <asp:HiddenField ID="hidValPublishResult" runat="server"  />                
                            <asp:HiddenField ID="hidValRecentlyAddedData" runat="server"  />
                            <asp:HiddenField ID="hidGradeShouldBeSelectedForLearningOutcome" runat="server"  />
                            <asp:HiddenField ID="hidGradeShouldBeSelectedForNonXseedSubject" runat="server"  />                
                <asp:HiddenField ID="HidSave" runat="server"  />                
                <asp:HiddenField ID="HidShow" runat="server"  />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:HiddenField ID="hidAssessmentId" runat="server" Value="0" />
                <asp:HiddenField ID="hidEditMode" runat="server" />
                <asp:HiddenField ID="hidIsPublishButtonClick" runat="server" />
                <asp:HiddenField ID="hidViewUrl" runat="server" />
                <asp:HiddenField ID="hidClassTacherID" runat="server" />
                <asp:HiddenField ID="hidIsBtnSave" runat="server" />
                <asp:HiddenField ID="hidStudentId" runat="server" />
                <asp:HiddenField ID="hidStandardId" runat="server" />
                <asp:HiddenField ID="hidIsStudentwiseProgressReport" runat="server" />
                <asp:HiddenField ID="hidstdDivId" runat="server" Value="0" />
                <asp:HiddenField ID="hidIsOldReport" runat="server" Value="N" />
                <asp:HiddenField ID="hidCultureInfo" runat="server" Value="N" />
               <asp:HiddenField ID="hidGradeShouldBeSelectedForCoCurricularSubject" runat="server"  />
                <asp:HiddenField ID="HidbtnPublishText" runat="server"  />
                <asp:HiddenField ID="hidRemarkText" runat="server" Value ="" />
                <asp:HiddenField ID="hidSubRemarkText" runat="server" Value="" />
                <asp:HiddenField ID="hidRemarkLength" runat="server" Value="0" />
            </td>
        </tr>
    </table>
    <style type="text/css">
        .ProgressReportHeader
        {
            font-weight: 700;
            font-size: 10pt;
            color: #333;
            text-decoration: none;
            height: 20px;
            background-color: #c8dffe;
        }
        .StudentDetailsHeader
        {
            font-weight: 700;
            font-size: 10pt;
            color: #333;
            text-decoration: none;
            height: 20px;
            padding-left: 5px;
            background-color: #c8dffe;
        }
    </style>

    <script type="text/javascript" language="javascript">

        _clientcstvalLearningOutcomeGrades = "<%=cstvalLearningOutcomeGrades.ClientID %>";
        _clientcstvalNonXseedSubjectGrades = "<%=cstvalNonXseedSubjectGrades.ClientID %>";
        _clientcstvalCoCurricularSubjectGrades = "<%=cstvalCoCurricularSubjectGrades.ClientID %>";
        _clientcmbAssessment = "<%=cmbAssessment.ClientID %>";
        _clientcstvalObservationsLength = "<%=cstvalObservationsLength.ClientID %>";
        _clienthidIsPublishButtonClick = "<%=hidIsPublishButtonClick.ClientID %>";
        _clienthidIsBtnSave = "<%=hidIsBtnSave.ClientID %>";
        _clientbtnPublish = "<%=btnPublish.ClientID %>";
        _clientbtnSave = "<%=btnSave.ClientID %>";
        _clienthidRemarkText = "<%=this.hidRemarkText.ClientID %>"
        _clienthidSubRemarkText = "<%=this.hidSubRemarkText.ClientID %>"
        _clienthidRemarkLength = "<%=this.hidRemarkLength.ClientID %>"

        var prm = Sys.WebForms.PageRequestManager.getInstance()
        prm.add_endRequest(EndReqHandler)
        prm.add_beginRequest(beginRequestHandler)
        function EndReqHandler(sender, args) {
            var postBackElement = sender._postBackSettings.sourceElement
        }
        function beginRequestHandler(sender, args) {
        }

        function ShowOldProgressReports(queryStrung) {
            window.open(queryStrung, '_new', 'scrollbars=yes,resizable=no,top=0,left=0,width=1050,height=700');
        }

//        function Validate(oSrc, args) {
//            if (document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes') != null)
//            for (i = 0; i < document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows.length; i++) {
//                for (j = 0; j < document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows[i].cells.length; j++) {
//                    var input = []
//                    input = document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows[i].cells[j].getElementsByTagName('TEXTAREA');

//                    for (k = 0; k < input.length; k++) {
//                        var sObservation = document.getElementById(input[k].id).value;
//                        if (sObservation.trim() == "" && !document.getElementById(input[k].id).disabled) {
//                            oSrc.errormessage = "Observation(s) should not be blank.";
//                            document.getElementById(_clientcstvalObservations).innerText = "Observation(s) should not be blank.";
//                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
//                            args.IsValid = false
//                            return true
//                        }
//                    }
//                }
//            }

//            if (document.getElementById('ctl00_MainBody_tblNonXseedProgressReport') != null)
//            for (i = 0; i < document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows.length; i++) {
//                for (j = 0; j < document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows[i].cells.length; j++) {
//                    var input = []
//                    input = document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows[i].cells[j].getElementsByTagName('TEXTAREA');

//                    for (k = 0; k < input.length; k++) {
//                        var sObservation = document.getElementById(input[k].id).value;
//                        if (sObservation.trim() == "" && !document.getElementById(input[k].id).disabled) {
//                            oSrc.errormessage = "Observation(s) should not be blank.";
//                            document.getElementById(_clientcstvalObservations).innerText = "Observation(s) should not be blank.";
//                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
//                            args.IsValid = false
//                            return true
//                        }
//                    }
//                }
//            }

//            if (document.getElementById('ctl00_MainBody_tblCoCurricularSubjects') != null)
//            for (i = 0; i < document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows.length; i++) {
//                for (j = 0; j < document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows[i].cells.length; j++) {
//                    var input = []
//                    input = document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows[i].cells[j].getElementsByTagName('TEXTAREA');

//                    for (k = 0; k < input.length; k++) {
//                        var sObservation = document.getElementById(input[k].id).value;
//                        if (sObservation.trim() == "" && !document.getElementById(input[k].id).disabled) {
//                            oSrc.errormessage = "Observation(s) should not be blank.";
//                            document.getElementById(_clientcstvalObservations).innerText = "Observation(s) should not be blank.";
//                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
//                            args.IsValid = false
//                            return true
//                        }
//                    }
//                }
//            }
//            args.IsValid = true
//            return false
//        }

        function ValidateObservationLength(oSrc, args) {
            if (document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes') != null)
            for (i = 0; i < document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows.length; i++) {
                for (j = 0; j < document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows[i].cells.length; j++) {
                    var input = []
                    input = document.getElementById('ctl00_MainBody_tblXseedLearningOutcomes').rows[i].cells[j].getElementsByTagName('TEXTAREA');

                    for (k = 0; k < input.length; k++) {
                        var sObservation = document.getElementById(input[k].id).value;
                        if (sObservation.trim().length > 500) {
                            oSrc.errormessage = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clientcstvalObservationsLength).innerText = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }

            if (document.getElementById('ctl00_MainBody_tblNonXseedProgressReport') != null)
            for (i = 0; i < document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows.length; i++) {
                for (j = 0; j < document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows[i].cells.length; j++) {
                    var input = []
                    input = document.getElementById('ctl00_MainBody_tblNonXseedProgressReport').rows[i].cells[j].getElementsByTagName('TEXTAREA');

                    for (k = 0; k < input.length; k++) {
                        var sObservation = document.getElementById(input[k].id).value;
                        if (sObservation.trim().length > 500) {
                            oSrc.errormessage = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clientcstvalObservationsLength).innerText = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }

            if (document.getElementById('ctl00_MainBody_tblCoCurricularSubjects') != null)
            for (i = 0; i < document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows.length; i++) {
                for (j = 0; j < document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows[i].cells.length; j++) {
                    var input = []
                    input = document.getElementById('ctl00_MainBody_tblCoCurricularSubjects').rows[i].cells[j].getElementsByTagName('TEXTAREA');

                    for (k = 0; k < input.length; k++) {
                        var sObservation = document.getElementById(input[k].id).value;
                        if (sObservation.trim().length > 500) {
                            oSrc.errormessage = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clientcstvalObservationsLength).innerText = document.getElementById("<%=this.hidObservationShouldNotBeGreaterThanCharacters.ClientID %>").value;
                            document.getElementById(_clienthidIsPublishButtonClick).value = "N";
                            args.IsValid = false
                            return true
                        }
                    }
                }
            }
            args.IsValid = true
            return false
        }

        function SetInitStatus(obj) {
            var bAction = true;
            var btn = obj.value.toLowerCase();
            var bResult = true;
            var isPageValid = true
            if (typeof (Page_ClientValidate) == 'function' && (btn == document.getElementById("<%=this.HidPublish.ClientID %>").value || btn == document.getElementById("<%=this.HidSave.ClientID %>").value)) {
                isPageValid = Page_ClientValidate()
            }

            document.getElementById(_clienthidIsBtnSave).value = (btn == document.getElementById("<%=this.HidSave.ClientID %>").value.toLowerCase()) ? "Y" : "N";

            if (isPageValid && btn == document.getElementById("<%=this.HidPublish.ClientID %>").value.toLowerCase()) {
                
                if (window.confirm(document.getElementById("<%=this.hidValPublishResult.ClientID %>").value)) {
                    bResult = true;
                    document.getElementById(_clienthidIsPublishButtonClick).value = "Y";
                }
                else
                    bResult = false;
            }
            else if (btn == document.getElementById("<%=this.hidViewProgressReport.ClientID %>").value.toLowerCase()) {
            if (window.confirm(document.getElementById("<%=this.hidValRecentlyAddedData.ClientID %>").value)) {
                bResult = true;
                document.getElementById(_clienthidIsPublishButtonClick).value = "Y";
            }
            else
                bResult = false;
            }
            if (btn == document.getElementById("<%=this.HidUnpublish.ClientID %>").value)
                document.getElementById(_clienthidIsPublishButtonClick).value = "Y";
            if (isPageValid && btn == document.getElementById("<%=this.HidSave.ClientID %>").value) {
                document.getElementById(_clienthidIsPublishButtonClick).value = "Y";
            }
            else if (btn == document.getElementById("<%=this.HidShow.ClientID %>").value)
                document.getElementById(_clienthidIsPublishButtonClick).value = "N";
            if (isPageValid && bResult && (btn == document.getElementById("<%=this.HidPublish.ClientID %>").value || btn == document.getElementById("<%=this.HidSave.ClientID %>").value)) {
                document.getElementById(_clientbtnSave).disabled = true;
                document.getElementById(_clientbtnPublish).disabled = true;
                //Calling DisablePage function to show overlay div for disabling page
				DisablePage(obj);
                __doPostBack(btn == document.getElementById("<%=this.HidSave.ClientID %>").value ? document.getElementById(_clientbtnSave).name : document.getElementById(_clientbtnPublish).name, '');
                return false;
            }
            return bResult;
        }

        function EnableDisableObservtionControl(ddl, txt) {
            if (ddl.value == "9" || ddl.value == "10")
                document.getElementById('ctl00_MainBody_' + txt).value = "";
            document.getElementById('ctl00_MainBody_' + txt).disabled = !(ddl.value != "9" && ddl.value != "10");

            // This is for PPS
            document.getElementById('ctl00_MainBody_' + txt).disabled = true;
        }

        function ValidateLearningOutcomeGrades(oSrc, args) {
            var sLearningOutcomeErrorMsg = "";
            var iSubjectSectionId = "0";
            var sRowNo = "";
            var sSubjectSection = "";
            $('select[id*=ddlLearningGrade_]').each(
                function () {
                    if (this.value == "0") {
                        if (iSubjectSectionId != this.id.split("_")[3]) {
                            if (iSubjectSectionId != "0") {
                                sLearningOutcomeErrorMsg += " " + sSubjectSection + " : " + sRowNo.substring(2, sRowNo.length) + ".<br/>";
                                sRowNo = "";
                            }

                            iSubjectSectionId = this.id.split("_")[3];
                            sSubjectSection = this.id.split("_")[7];
                        }

                        sRowNo += ", " + this.id.split("_")[8];
                    }
                }
            );

            if (sRowNo != "") {
                sLearningOutcomeErrorMsg += " " + sSubjectSection + " : " + sRowNo.substring(2, sRowNo.length) + ".";
            }


            if (sLearningOutcomeErrorMsg.length > 0) {
                document.getElementById(_clientcstvalLearningOutcomeGrades).errormessage = document.getElementById("<%=this.hidGradeShouldBeSelectedForLearningOutcome.ClientID %>").value + "<br/>" + sLearningOutcomeErrorMsg;
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function ValidateNonXseedSubjectGrades(oSrc, args) {
            var sNonXseedSuibjectsErrorMsg = "";
            $('select[id*=ddlNonXseedGrade_]').each(
                function () {
                    if (this.value == "0") {
                        sNonXseedSuibjectsErrorMsg += ", " + this.id.split("_")[4];
                    }
                }
            );

                if (sNonXseedSuibjectsErrorMsg.length > 0) {
                    
                    document.getElementById(_clientcstvalNonXseedSubjectGrades).errormessage = document.getElementById("<%=this.hidGradeShouldBeSelectedForNonXseedSubject.ClientID %>").value + sNonXseedSuibjectsErrorMsg.substring(2, sNonXseedSuibjectsErrorMsg.length + ".");
                    args.IsValid = false;
                    return true;
                }
                else {
                    args.IsValid = true;
                    return false;
                }
        }   

        function ValidateCoCurricularSubjectGrades(oSrc, args) {
            var sCoCurricularErrorMsg = "";
            $('select[id*=ddlCoCurricularSubjectsGrade_]').each(
                function () {
                    if (this.value == "0") {
                        sCoCurricularErrorMsg += ", " + this.id.split("_")[4];
                    }
                }
            );

            if (sCoCurricularErrorMsg.length > 0) {
                document.getElementById(_clientcstvalCoCurricularSubjectGrades).errormessage = document.getElementById("<%=this.hidGradeShouldBeSelectedForCoCurricularSubject.ClientID %>").value + sCoCurricularErrorMsg.substring(2, sCoCurricularErrorMsg.length) + ".";
                args.IsValid = false;
                return true;
            }
            else {
                args.IsValid = true;
                return false;
            }
        }

        function SetDefaultGrade(ddlGradeId, defaultGrade) {
            $('select[id*=' + ddlGradeId + '_]').each(
                function () {
                    this.value = defaultGrade;
                }
            );
            }

            function ValidateRemark(oSrc, args) {
                var remark = document.getElementById("ctl00_MainBody_txtRemark").value
                remark = remark.trim()

                var maxLength = parseInt(document.getElementById(_clienthidRemarkLength).value)

                if (remark.length > 0 && remark.length > maxLength) {
                    oSrc.errormessage = 'Remark length should not be greater than ' + maxLength + ' characters.'
                    args.IsValid = false;
                    return true;
                }
                args.IsValid = true;
                return false;
            }

            function UpdateTextLength(txt) {
                var lblCount = document.getElementById("ctl00_MainBody_txtRemarkCountLabel")

                var oldRemark = $("#" + _clienthidRemarkText).val();
                var maxLength = parseInt(document.getElementById(_clienthidRemarkLength).value)
                
                if (txt.value.length > maxLength) {
                    txt.value = oldRemark;
                }
                else {
                    $("#" + _clienthidRemarkText).val(txt.value)
                    lblCount.innerHTML = "(" + (maxLength - txt.value.length) + ")"
                }
            }

            function UpdateRemarkLength(txt) {
                var oldRemark = $("#" + _clienthidSubRemarkText).val();
                var maxLength = parseInt(document.getElementById(_clienthidRemarkLength).value)
                if (txt.value.length > maxLength) {
                    txt.value = oldRemark;
                }
                else {
                    $("#" + _clienthidSubRemarkText).val(txt.value)
                    var nm = "lblSubjectRemark" + txt.id.substring(txt.id.lastIndexOf('_'))
                    $("[id*=" + nm + "]").html("(" + (maxLength - txt.value.length) + ")")
                }
            }
              
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PopupMainBody" runat="Server">
</asp:Content>
