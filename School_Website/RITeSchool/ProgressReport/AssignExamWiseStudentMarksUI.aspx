<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AssignExamWiseStudentMarksUI.aspx.cs"
    Inherits="AssignExamWiseStudentMarksUI" MasterPageFile="../MasterPages/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainBody" runat="Server">
	<asp:Panel ID="pnlErrorMsg" Visible="true" runat="server" Width="100%">
        <table>
            <tr>
                <td align="left">
                    <asp:Label ID="lblErrorMsg" Style="text-align: left" runat="server" ForeColor="blue"
                        Width="100%" CssClass="ClsConfigText" EnableViewState="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:UpdatePanel ID="UPanelStandardt" runat="server" ChildrenAsTriggers="True" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="GridViewScrollContainer" runat="server" Visible="true" Style="overflow: auto;
                width: 842px; left: 0px;">
                <asp:Label ID="lblSuccessfulMsg" Style="text-align: center" runat="server" ForeColor="blue"
                    Width="100%" CssClass="ClsConfigText" EnableViewState="false"></asp:Label><br />
            </asp:Panel>
            <input type="hidden" id="marks" />
            <asp:HiddenField ID="hidEdited" Value="0" runat="server" />
            <asp:HiddenField ID="hidResultGenrted" Value="1" runat="server" />
            <asp:HiddenField ID="HidBackUrl" runat="server" />
            <asp:HiddenField ID="hidIsGraceApplied" Value="0" runat="server" />
            <asp:HiddenField ID="hidRemoveProgressReport" runat="server" Value="N" />
            <asp:HiddenField ID="HidGradeRange" runat="server" />
            <asp:HiddenField ID="hidExamStatus" runat="server" />
            <asp:HiddenField ID="hidStandardDivisionId" runat="server" />
            <asp:HiddenField ID="hidClassTeacher" runat="server" />
            <asp:HiddenField ID="hidUserID" runat="server" />
            <asp:HiddenField ID="hidOptionalSubjectCells" runat="server" Value="" />
            <asp:HiddenField ID="hidIsFailCriteriaNotApplicable" runat="server" />
            <asp:HiddenField ID="hidConfirmSms" runat="server" />
            <asp:HiddenField ID="hidDependentTestNames" runat="server" />
            <asp:HiddenField ID="hidShowTotalAsPerOutOfMarks" runat="server" />
            <asp:HiddenField ID="hidRoundMarksAtSubjectLevel" runat="server" />
            <div style="padding-bottom: 7px; padding-top: 5px">
            </div>
            <asp:Panel runat="server" Visible="true">
                <table id="tblNote" runat="server" width="700px">
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                            
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note1 :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                            
                            <span class="LblSmlV" style="border-width:0px;">To save result, first select exam(s) in the result section and click on 'Save' button.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                            
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note2 :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                           
                            <span class="LblSmlV" style="border-width:0px;">To publish result, first select exam(s) in the result section and click on 'Publish' button.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                            
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note3 :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                            
                            <span class="LblSmlV" style="border-width:0px;">To unpublish result, first remove exam(s) selection from the result section and click on 'Publish' button.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                          
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note4 :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                           
                            <span class="LblSmlV" style="border-width:0px;">To delete marks, first select exam(s) in the result section and click on 'Delete' button.</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" class="ClsBorderlight " style="width: 50px; background-color: #ffffc4;">                           
                                <span class="LblNrmlB" style="font-weight:bold;border-width:0px;">Note5 :</span>
                        </td>
                        <td align="left" class="ClsBorderlight" style="padding-left: 5px;">                          
                            <span class="LblSmlV" style="border-width:0px;">To view progress report, first select exam(s) in the result section and click on 'View Progress Report' button.</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnPublish" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <div>
        <asp:Button ID="btnBack" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
            Text="Back" Visible="True" PostBackUrl="~/RITeSchool/ProgressReport/StudentwiseProgreesReportUI.aspx"
            UseSubmitBehavior="false" />
        <asp:Button ID="btnSave" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml" disable-page="true"
            Text="Save" Visible="True" OnClick="btnSave_Click" />
        <asp:Button ID="btnView" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnLrg"
            Text="View Progress Report" Visible="True" OnClick="btnView_Click" />
        <asp:Button ID="btnPublish" runat="server" BorderStyle="Solid" BorderWidth="1px"
            CssClass="ClsBtnSml" Text="Publish" Visible="True" OnClick="btnPublish_Click" />
        <asp:Button ID="btnDelete" runat="server" BorderStyle="Solid" BorderWidth="1px" CssClass="ClsBtnSml"
            Text="Delete" Visible="True" OnClick="btnDelete_Click" />
    </div>
    <script language="javascript" type="text/javascript">

        _clientIdhidEdited = "<%=hidEdited.ClientID%>";
        _clientHidGradeRange = "<%=HidGradeRange.ClientID %>";
        _clienthidOptionalSubjectCells = "<%=hidOptionalSubjectCells.ClientID %>";
        _clienthidConfirmSms = "<%=hidConfirmSms.ClientID %>"
        _clientbtnPublish = "<%=btnPublish.ClientID %>"
        _clienthidExamStatus = "<%=hidExamStatus.ClientID %>"
        _clienthidIsFailCriteriaNotApplicable = "<%=hidIsFailCriteriaNotApplicable.ClientID %>"
        _clienthidDependentTestNames = "<%=this .hidDependentTestNames.ClientID %>"
        _clienthidShowTotalAsPerOutOfMarks = "<%=this .hidShowTotalAsPerOutOfMarks.ClientID %>"

        function SetControlAsPerExamStatus(oddlExamStatus, oControlId, obj, RowIndex) {
            var oControl = document.getElementById(oControlId);
            if (oddlExamStatus.value == "N") {
                oControl.value = "";
                oControl.value = "0";
                oControl.disabled = false;
            }
            else {
                oControl.value = "";
                oControl.value = "0";
                oControl.disabled = true;
            }
            Validate(oControl, oControl.maxLength, obj, RowIndex);
        }

        function SetValue(textbox) {
            document.getElementById("marks").value = textbox.value;
        }

        function Validate(textbox, MaxVal, obj, aiRowIndex) { 
            var sMarks = textbox.value;
            var iMarks = parseFloat(sMarks);
            if (sMarks.length <= 0)
                textbox.value = "0";
            if (iMarks > MaxVal) {
                textbox.value = "0";
                textbox.focus();
            }
            var i = 0
            var j = 0
            var k = 0
            var Total = 0
            var MarksScoredInSubject = 0
            var ExamTotal = 0
            var ObtainedTotParcentage = 0
            var TestTypeOutOfMarks = false;
            var TestOutOfMarks = false;
            var ExamStatusValid = true;
            var TestTypeCount = 0;
            var SubjectTotalMarks = 0;
            var AllowDecimalIndex = 14;
            var LastTextBoxId = "";
            var AddInTotal = false;
            for (i = 0; i < document.getElementById(obj).rows[aiRowIndex].cells.length; i++) {
                var input = []
                var spans = []
                input = document.getElementById(obj).rows[aiRowIndex].cells[i].getElementsByTagName('input');
                for (j = 0; j < input.length; j++) {
                    var id = input[j].id.split('_');
                    LastTextBoxId = (input[j].id.match("txtMarks")) ? id : "";
                    if (input[j].id.match("txtMarks") && $('select[id*=ddlExamStatus_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + id[7] + ']').length > 0
                            && (($('select[id*=ddlExamStatus_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + id[7] + ']')[0].value == "N" && !input[j].disabled) ||
                                ConsiderInTotal($('select[id*=ddlExamStatus_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + id[7] + ']')[0].value))) {
                        if (IsAdjacentExamStatusValid(id) || id[13] == 'Y') {
                            if (input[j].value != "") {
                                if (input[j].value.trim() == ".")
                                    input[j].value = "0";
                                TestOutOfMarks = id[9];
                                TestTypeOutOfMarks = id[10];
                                AddInTotal = id[13] == "Y";
                                SubjectTotalMarks = SubjectTotalMarks + parseInt((TestTypeOutOfMarks != 0) ? TestTypeOutOfMarks : id[11]);  //TestType_Total_Marks
                                // (TestTypeOutOfMarks != 0) ? Convert marks scored to TestTypeOutOfMarks : MarksScored.
                                MarksScoredInSubject = MarksScoredInSubject + parseFloat((TestTypeOutOfMarks != 0) ? Round(input[j].value * TestTypeOutOfMarks / parseInt(id[11]), Boolean.parse(id[AllowDecimalIndex]) ? 1 : 0) : parseFloat(input[j].value));
                                TestTypeCount++;
                            }
                        }
                        else if (ExamStatusValid)
                            ExamStatusValid = false;
                    }
                    else {
                        TestOutOfMarks = 0;
                        if (input[j].id.indexOf("txt") != -1) {
                            if (id[9] == 'N' && ExamStatusValid && id[12] == 'Y') {
                                ExamStatusValid = false;
                            }
                            if (id[9] == 'Y' && $('input:checkbox[id*=chkIsApplicable_0_' + id[6] + ']').length > 0 && $('input:checkbox[id*=chkIsApplicable_0_' + id[6] + ']')[0].checked) {
                                SubjectTotalMarks = SubjectTotalMarks + parseInt((id[11] != 0) ? id[11] : id[11]);   //TestType_Total_Marks
                                TestTypeCount++;
                            }
                        }
                    }
                }

                if (LastTextBoxId != "" && $('span[id*=lblTotal_' + LastTextBoxId[3] + '_' + LastTextBoxId[4] + '_' + LastTextBoxId[5] + '_' + LastTextBoxId[6] + '_' + ']').length > 0) {
                	$('span[id*=lblTotal_' + LastTextBoxId[3] + '_' + LastTextBoxId[4] + '_' + LastTextBoxId[5] + '_' + LastTextBoxId[6] + '_' + i + '_' + ']').each
                    (
                        function () {
                        	if (TestTypeCount == 1) {
                        		if (TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks)
                        			this.innerHTML = (Boolean.parse(LastTextBoxId[AllowDecimalIndex])) ? Math.round(MarksScoredInSubject * TestOutOfMarks / SubjectTotalMarks, (MarksScoredInSubject % 1 > 0) ? 1 : 0) : parseInt(Math.round(MarksScoredInSubject / TestTypeCount));
                        		else this.innerHTML = MarksScoredInSubject.toFixed(Boolean.parse(LastTextBoxId[AllowDecimalIndex]) ? (MarksScoredInSubject % 1 > 0) ? 1 : 0 : 0);

                        		if (AddInTotal) {
                        			Total = Total + ((TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks) ? (TestTypeCount == 0) ? 0 : Round(MarksScoredInSubject * TestOutOfMarks / SubjectTotalMarks, $("input:hidden[id*=hidRoundMarksAtSubjectLevel]")[0].value == "N" ? 1 : 0) : MarksScoredInSubject);
                        			Total = Round(Total, 1);
                        			ExamTotal = ExamTotal + parseInt((TestOutOfMarks != 0) ? TestOutOfMarks : SubjectTotalMarks);
                        			AddInTotal = false;
                        		}
                        	}
                        	else {
                        		if (TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks)
                        			this.innerHTML = (TestTypeCount == 0) ? 0 : (Boolean.parse(LastTextBoxId[AllowDecimalIndex])) ? Round(MarksScoredInSubject / TestTypeCount, (MarksScoredInSubject % 1 > 0) ? 1 : 0) : parseInt(Math.round(MarksScoredInSubject / TestTypeCount));
                        		else this.innerHTML = MarksScoredInSubject.toFixed(Boolean.parse(LastTextBoxId[AllowDecimalIndex]) ? (MarksScoredInSubject % 1 > 0) ? 1 : 0 : 0);


                        		if (TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks)
                        			this.innerText = (TestTypeCount == 0) ? 0 : (Boolean.parse(LastTextBoxId[AllowDecimalIndex])) ? Round(MarksScoredInSubject / TestTypeCount, (MarksScoredInSubject % 1 > 0) ? 1 : 0) : parseInt(Math.round(MarksScoredInSubject / TestTypeCount));
                        		else this.innerText = MarksScoredInSubject.toFixed(Boolean.parse(LastTextBoxId[AllowDecimalIndex]) ? (MarksScoredInSubject % 1 > 0) ? 1 : 0 : 0);

                        		if (AddInTotal) {
                        			//Total = Total + ((TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks) ? (TestTypeCount == 0) ? 0 : Round(MarksScoredInSubject * TestOutOfMarks / SubjectTotalMarks, $("input:hidden[id*=hidRoundMarksAtSubjectLevel]")[0].value == "N" ? 1 : 0) : MarksScoredInSubject);
                        			Total = Total + parseFloat((TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks) ? (TestTypeCount == 0) ? 0 : parseInt(Math.round(MarksScoredInSubject / TestTypeCount)) : MarksScoredInSubject);
                        			Total = Round(Total, 1);
                        			ExamTotal = ExamTotal + parseInt((TestOutOfMarks != 0) ? TestOutOfMarks : SubjectTotalMarks);
                        			AddInTotal = false;
                        		}
                        	}
	                        MarksScoredInSubject = 0;
                        	TestTypeCount = 0;
                        	SubjectTotalMarks = 0;
                        }
                    );
                }
                else { 
                    if (AddInTotal) {
                        Total = Total + ((TestOutOfMarks != 0 && TestOutOfMarks != SubjectTotalMarks) ? (TestTypeCount == 0) ? 0 : Round(MarksScoredInSubject * TestOutOfMarks / SubjectTotalMarks, $("input:hidden[id*=hidRoundMarksAtSubjectLevel]")[0].value == "N" ? 1 : 0) : MarksScoredInSubject);
                        Total = Round(Total, 1);
                        ExamTotal = ExamTotal + parseInt((TestOutOfMarks != 0) ? TestOutOfMarks : SubjectTotalMarks);
                        AddInTotal = false;
                    }
                    MarksScoredInSubject = 0;
                    TestTypeCount = 0;
                    SubjectTotalMarks = 0;
                }
            }

            if (!ExamStatusValid && document.getElementById(_clienthidShowTotalAsPerOutOfMarks).value == "Y") {
                $('span[id*=lblMarks_' + aiRowIndex + ']')[0].innerHTML = ' - ';
                $('span[id*=lblTotalMarks_' + aiRowIndex + ']')[0].innerHTML = '';
                $('span[id*=lblPercentage_' + aiRowIndex + ']')[0].innerHTML = ' - ';
                $('span[id*=lblGrade_' + aiRowIndex + ']')[0].innerHTML = ' - ';
                $('span[id*=lblGradeRemarks_' + aiRowIndex + ']')[0].innerHTML = '';
            }
            else {
                $('span[id*=lblMarks_' + aiRowIndex + ']')[0].innerHTML = Total;
                $('span[id*=lblTotalMarks_' + aiRowIndex + ']')[0].innerHTML = ExamTotal;
                var Percentage = ((ExamTotal == 0) ? 0 : Math.round((Total * 100 / ExamTotal) * 100) / 100);
                $('span[id*=lblPercentage_' + aiRowIndex + ']')[0].innerHTML = Percentage + "%";
                SetTotalGrade(Percentage, $('span[id*=lblGrade_' + aiRowIndex + ']')[0]);
                $('span[id*=lblMarks_' + aiRowIndex + ']')[0].innerHTML = $('span[id*=lblMarks_' + aiRowIndex + ']')[0].innerHTML + ' / ';
            }            
        }

        function IsAdjacentExamStatusValid(id) {
            var Next = $('input:text[id*=txtMarks_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + (parseInt(id[7]) + 1) + ']')
            var ddlExamStatus = $('input:text[id*=ddlExamStatus_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + (parseInt(id[7]) + 1) + ']');
            if (Next.length > 0 && Next[0].disabled && ddlExamStatus.length > 0 && ddlExamStatus[0].value != "N" && !ConsiderInTotal(ddlExamStatus[0].value))
                return false;
            var Prev = $('input:text[id*=txtMarks_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + (parseInt(id[7]) - 1) + ']')
            ddlExamStatus = $('input:text[id*=ddlExamStatus_' + id[3] + '_' + id[4] + '_' + id[5] + '_' + id[6] + '_' + (parseInt(id[7]) + 1) + ']');
            if (Prev.length > 0 && Prev[0].disabled && ddlExamStatus.length > 0 && ddlExamStatus[0].value != "N" && !ConsiderInTotal(ddlExamStatus[0].value))
                return false;
            return true;
        }

        function ConsiderInTotal(ExamStatus) { 
            var arrExamStatus = document.getElementById(_clienthidExamStatus).value.split("#");
            var iExamStsId;
            for (iExamStatusId = 0; iExamStatusId < arrExamStatus.length; iExamStatusId++) {
                if (arrExamStatus[iExamStatusId].split(":")[0] == ExamStatus && arrExamStatus[iExamStatusId].split(":")[1] == "Y")
                    return true;
            }
            return false;
        }

        function SetTotalGrade(ObtainedTotParcentage, ObtainedTotmarkslbl) { 
            var ObtainedTotRemarkslbl = document.getElementById(ObtainedTotmarkslbl.id.replace("lblGrade_", "lblGradeRemarks_"));
            var arrGrades = document.getElementById(_clientHidGradeRange).value.split("#");
            var iGradeCnt;
            for (iGradeCnt = 0; iGradeCnt < arrGrades.length; iGradeCnt++) {
                var arrRanges = arrGrades[iGradeCnt].split(":");
                var GradeName = arrRanges[0];
                var GradeMinMarks = parseFloat(arrRanges[1]);
                var GradeMaxMarks = parseFloat(arrRanges[2]);
                var GradeRemarks = arrRanges[3];
                //Now calculate grades for total marks.
                if (ObtainedTotParcentage >= GradeMinMarks && ObtainedTotParcentage <= GradeMaxMarks) {
                    ObtainedTotmarkslbl.innerHTML = GradeName;
                    ObtainedTotRemarkslbl.innerHTML = "&nbsp[" + GradeRemarks + "]";
                    break;
                } else {
                    ObtainedTotmarkslbl.innerHTML = "";
                    ObtainedTotRemarkslbl.innerHTML = "";
                }
            }

        }

        function EnableDisableControlsOfRow(chk, obj, RowIndex) { 
            var i = 0
            var j = 0
            //EnableDisableExamDependentControlsOfRow(chk, obj, RowIndex)
            if (document.getElementById(_clienthidDependentTestNames).value == "")
                document.getElementById(_clienthidDependentTestNames).value = document.getElementById('ctl00_MainBody_hidDependentExamName_' + RowIndex).value
            else
                document.getElementById(_clienthidDependentTestNames).value = document.getElementById(_clienthidDependentTestNames).value + "," + document.getElementById('ctl00_MainBody_hidDependentExamName_' + RowIndex).value

            for (i = 0; i < document.getElementById(obj).rows[RowIndex].cells.length; i++) {
                var controls = []
                controls = document.getElementById(obj).rows[RowIndex].cells[i].getElementsByTagName('*');
                var sExamStaus = "N";                
                for (j = 0; j < controls.length; j++) {
                    if (controls[j].id.match("txtMarks") || controls[j].id.match("ddl")) {
                        var bIsExamStatusApplicable = true;
                        if (controls[j].id.match("ddlExamStatus")) {
                            var val = controls[j].id.match(/_IEX(\d)_/);
                            if (val && val.length > 1) {
                                bIsExamStatusApplicable = val[1] == "1";
                            }
                            sExamStaus = controls[j].value;
                        }

                        if (document.getElementById('ctl00_MainBody_hidIsMarkAssigned_' + RowIndex + '_' + i) != null && document.getElementById('ctl00_MainBody_hidIsMarkAssigned_' + RowIndex + '_' + i).value != 'N' && bIsExamStatusApplicable)
                            document.getElementById(controls[j].id).disabled = chk.checked;
                        else if (document.getElementById('ctl00_MainBody_hidIsMarkAssigned_' + RowIndex + '_' + i) == null && bIsExamStatusApplicable)
                            document.getElementById(controls[j].id).disabled = chk.checked;
                        if ((controls[j].id.match("txtMarks") || controls[j].id.match("ddl_")) && sExamStaus != "N")
                            document.getElementById(controls[j].id).disabled = true;
                        if (controls[j].id.split('_')[11] == 'J')
                            document.getElementById(controls[j].id).disabled = true;
                    }
                }
            }
        }


        function EnableDisableControlsForOptionalSubjects(chk, obj, SubjectId, CellIndex) { 
        $('input:text[id*=_' + SubjectId + '_]').each(
            function () {
                this.disabled = !chk.checked
            }
            );
            $('select[id*=_' + SubjectId + '_]').each(
            function () {
                this.disabled = !chk.checked
            }
            );
        }

        function SetConfirmation(btn, tbl) { 

            /////////////////////////////////////////////////////////////////////////////////////////
            document.getElementById(_clienthidDependentTestNames).value = ""
            ///Find all the controls on the form
            var checks = document.forms[0].elements;
            var boxLength = checks.length;
            var iCount = 0;
            var iNo = 0;
            var ExamNames = ''
            //loop thow all controls
            for (j = 0; j < boxLength; j++) {
                //Find the checkbox
                if (checks[j].type == 'checkbox' && checks[j] != null) {
                    if (checks[j].id.match("chkPublish") && document.getElementById(checks[j].id).checked) {
                        var ParentChkId = document.getElementById(checks[j].id)
                        var iRowNo = checks[j].id.substring(Number(checks[j].id.lastIndexOf("_") + 1), checks[j].id.length)

                        ///get Exam Id value
                        var iTestId = document.getElementById('ctl00_MainBody_hidTestId_' + iRowNo).value;
                        var ExmNm = document.getElementById('ctl00_MainBody_hidDependentExamName_' + iRowNo).value
                        ExamNames = ExmNm;
                        document.getElementById(_clienthidDependentTestNames).value = ExmNm;
                        ///Find the dependent exam details
                        var sDependentExamId = document.getElementById('ctl00_MainBody_hidDependentExamId_' + iRowNo)
                        var sDependentExamIdArray = new Array();
                        if (sDependentExamId != null)
                            if (sDependentExamId.value != "") {

                                ///if comma seperated value present
                                if (sDependentExamId.value.indexOf(',') != -1) {
                                    sDependentExamIdArray = sDependentExamId.value.split(",");
                                    iCount = sDependentExamIdArray.length;
                                }
                                else {
                                    sDependentExamIdArray[iNo] = sDependentExamId.value;
                                    iCount = 1;
                                }

                                ///loop for no of dependent exam times
                                while (iNo < iCount) {
                                    //loop thow all controls
                                    for (k = 0; k < boxLength; k++) {
                                        if (checks[k].type == 'hidden' && checks[k] != null) {

                                            ///Match Parent and dependent exam status.
                                            if (checks[k].id.match("hidTestId") && document.getElementById(checks[k].id).value != iTestId && document.getElementById(checks[k].id).value == sDependentExamIdArray[iNo]) {

                                                ///find the Row Number
                                                var RwNo = checks[k].id.substring(Number(checks[k].id.lastIndexOf("_") + 1), checks[k].id.length)

                                                if (document.getElementById('ctl00_MainBody_chkPublish_' + RwNo).checked == ParentChkId.checked) {
                                                    if (document.getElementById(_clienthidDependentTestNames).value == "")
                                                        document.getElementById(_clienthidDependentTestNames).value = document.getElementById('ctl00_MainBody_hidDependentExamName_' + RwNo).value
                                                    else
                                                        document.getElementById(_clienthidDependentTestNames).value = document.getElementById(_clienthidDependentTestNames).value + "," + document.getElementById('ctl00_MainBody_hidDependentExamName_' + RwNo).value
                                                }
                                                else {
                                                    if (document.getElementById('ctl00_MainBody_chkPublish_' + RwNo).checked != ParentChkId.checked) {
                                                        if (ExamNames == "")
                                                            ExamNames = document.getElementById('ctl00_MainBody_hidDependentExamName_' + RwNo).value
                                                        else
                                                            ExamNames = ExamNames + ", " + document.getElementById('ctl00_MainBody_hidDependentExamName_' + RwNo).value
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    iNo++;
                                }
                                if (ExamNames.indexOf(',') != -1 && ExamNames != "") {
                                    ExamNames = ExamNames.replace(ExamNames.substring(Number(ExamNames.lastIndexOf(",")),Number(ExamNames.lastIndexOf(",")) + 1), " and")
                                    alert(ExamNames + " are dependent exams. Hence, to publish, both should be selected.")
                                    return false
                                }
                                else
                                    return true
                            }
                    }
                    
                       
                }

            }
            return true

        }

        var Page_IsValid = true;
        function ConfirmAction(btn, tbl) {
        	Page_IsValid = true;
                var bAction = true;
                var sConfirmMsg = "";
                if (btn.value == "Publish") {

                    if (!SetConfirmation(btn, tbl)) {
                        return false;
                    }
                    else {
                        if (document.getElementById(_clienthidDependentTestNames).value == "") {
                            var sMsg = document.getElementById(_clienthidDependentTestNames).value + " exams will be published. \nAre you sure you want to continue?"
                            if (document.getElementById(_clienthidDependentTestNames).value.indexOf(',') != -1 && !window.confirm(sMsg))
                                return false;
                        }
                        sConfirmMsg = "Saved marks for selected tests will be published. Once you publish the marks it will be visible to parents/student. Are you sure you want to continue?";
                        if (document.getElementById(_clienthidIsFailCriteriaNotApplicable).value == "") {
                            window.alert("Fail Criteria is not configured for this standard.");
                            return false;
                        }
                    }
                }
                else if (btn.value == "Save")
                    sConfirmMsg = "Marks will be saved for selected tests only. Are you sure you want to continue?";
                else if (btn.value == "View Progress Report")
                    sConfirmMsg = "Progress report for the selected tests will be displayed. Also, recently entered data will be lost. Are you sure you want to continue?";
                else if (btn.value == "Delete")
                    sConfirmMsg = "Are you sure you want to delete marks for selected exam(s)?";


                var bResult = false;

                if (window.confirm(sConfirmMsg)) {
                    bResult = true;
                    if (btn.value == "Publish" && window.confirm("Do you want to send message to the student?")) {
                        document.getElementById(_clienthidConfirmSms).value = 1;
                    }
                    else {
                        document.getElementById(_clienthidConfirmSms).value = 0;
                        if (btn.value == "Delete") {
                            for (i = 0; i < document.getElementById(tbl).rows.length; i++) {
                                var DefaultString = "ctl00_MainBody_";
                                if (i >= 10)
                                    str = "ctl0" + i + "_MainBody_";

                                if (document.getElementById(DefaultString + "chkPublish_" + i) != null && !document.getElementById(DefaultString + "chkPublish_" + i).disabled && document.getElementById(DefaultString + "chkPublish_" + i).checked) {
                                    if (document.getElementById(DefaultString + "hidTestPublishStatus_" + i) != null && document.getElementById(DefaultString + "hidTestPublishStatus_" + i).value != "N") {
                                        window.alert("Can not delete marks for published exam(s).");
                                        return false;
                                    }
                                    if (document.getElementById(DefaultString + "hidTestSubmitStatus_" + i) != null && document.getElementById(DefaultString + "hidTestSubmitStatus_" + i).value != "N") {
                                        window.alert("Can not delete marks for submitted exam(s).");
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                	Page_IsValid = false;
                    bResult = false;
                }
                return bResult;
            //}
        }
         
    </script>
</asp:Content>
