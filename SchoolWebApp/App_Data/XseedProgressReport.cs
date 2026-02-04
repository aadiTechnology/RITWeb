using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using XseedReportEntities;
using Utility;


/// <summary>
/// Summary description for XseedProgressReport
/// </summary>
public class XseedProgressReport : SchoolBase
{
    #region "Data Mambers"

    protected XseedProgressReportBL moXseedProgressReportBL;
    HtmlTable motblMainProgressReport;
    HtmlTable moTblStudentdetails;
    HtmlTable moTblGrades;
    HtmlTable moTblXseedLearningOutcomes;
    HtmlTable moTblNonXseedProgressReport;
    HtmlTable moTblCoCurricularSubjects;
    HtmlTable moNoteTable;
    HtmlTable moRemarkTable;    
    HtmlTableRow motrErrorMessage;
    protected int miStudentId;
    protected int miAssessmentId;
    protected int miStandDivisionId;
    protected bool mbIsEditMode;
    protected bool mbSetGrade;
    protected bool mbIsStudentWiseProgressReport;

    #endregion "Data Mambers"

    #region "Constructors"

    public XseedProgressReport()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public XseedProgressReport(int aiStandDivisionId, int aiStudentId, int aiAssessmentId, bool abIsEditMode)
    {
        miStandDivisionId = aiStandDivisionId;
        miStudentId = aiStudentId;
        miAssessmentId = aiAssessmentId;
        mbIsEditMode = abIsEditMode;
    }

    #endregion "Constructors"

    /// <summary>
    /// This method is used to set base table of the progress report.
    /// </summary>
    /// <param name="aotblMainProgressReport"></param>
    /// <param name="aotrErrorMessage"></param>
    protected void SetProgressReportTableAndErrorRow(HtmlTable aotblMainProgressReport, HtmlTableRow aotrErrorMessage)
    {
        motblMainProgressReport = aotblMainProgressReport;
        motrErrorMessage = aotrErrorMessage;
    }

    /// <summary>
    /// This method is used to set standardwise progress report.
    /// </summary>
    protected void SetStudentwiseProgressReport(int aiRemarkLength = 300)
    {
        bool bIsReportAvailable = false;
        if (moXseedProgressReportBL.YearwiseStudentMaster.Count > 0)
        {
            motrErrorMessage.Visible = false;
            moXseedProgressReportBL.YearwiseStudentMaster.ForEach
             (
                student =>
                {
                    List<StudentsLearningOutcome> lstStudentsLearningOutcomesomes = moXseedProgressReportBL.StudentsLearningOutcomes.Where(StudentsLearningOutcome => StudentsLearningOutcome.YearwiseStudentId == student.YearwiseStudentId).ToList();
                    if (lstStudentsLearningOutcomesomes.Count > 0)
                    {
                        CreateTables();
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("SocietyName", moXseedProgressReportBL.SchoolEntity.OrganizationName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("ActualSchoolName", moXseedProgressReportBL.SchoolEntity.SchoolName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetails("ClsReportHead", "Progress Report"));
                        FillStudentDetails(student.YearwiseStudentId);
                        FillGrades();
                        FillAssessmentAndAttendanceDetails(student.YearwiseStudentId);
                        FillLearingOutcomeDetails(student.YearwiseStudentId, aiRemarkLength);
                        FillNotes();
                        FillRemark(student.YearwiseStudentId, aiRemarkLength);
                        AddSeperater();
                        bIsReportAvailable = true;
                    }
                }
            );
        }

        if (!bIsReportAvailable)
            motrErrorMessage.Visible = true;
        else
            motblMainProgressReport.Rows.RemoveAt(motblMainProgressReport.Rows.Count - 1);
    }

    private void FillRemark(int aiYearwiseStudentId, int aiRemarkLength)
    {
        HtmlTableRow trRemark = new HtmlTableRow();
        HtmlTableCell tdRemark = new HtmlTableCell();
        Label lblRemark = new Label { Text="Remark ", CssClass="ClsLabel" };
        tdRemark.Width = "100px";
        tdRemark.Attributes.Add("class", "ClsBorderLight");
        tdRemark.Controls.Add(lblRemark);
        trRemark.Cells.Add(tdRemark);

        tdRemark = new HtmlTableCell();
        TextBox txtComment = new TextBox();
        txtComment.CssClass = "ExLrgTxtBox";
        txtComment.Width = Unit.Percentage(100);
        txtComment.ID = "txtRemark";
        txtComment.TextMode = TextBoxMode.MultiLine;

        if (moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Any())
            txtComment.Text = moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Select(rmk => rmk.Remark).FirstOrDefault();

        txtComment.Attributes.Add("onkeyup", "UpdateTextLength(this);");
        txtComment.Attributes.Add("onpaste", "UpdateTextLength(this);");

        txtComment.Enabled = !(moXseedProgressReportBL.StudentWiseAssessmentPublishStatus || moXseedProgressReportBL.AssessmentPublishStatus);

        tdRemark.Controls.Add(txtComment);
        
        trRemark.Cells.Add(tdRemark);

        HtmlTableCell tdRemarkCount = new HtmlTableCell();
        Label lblCount = new Label();
        lblCount.ID = "txtRemarkCountLabel";
        lblCount.CssClass = "clsLabel";
        lblCount.Text = "(" + (txtComment.Text.Length == 0 ? aiRemarkLength : aiRemarkLength - txtComment.Text.Length) + ")";
        tdRemarkCount.Width = "50px";
        tdRemarkCount.Controls.Add(lblCount);
        trRemark.Cells.Add(tdRemarkCount);
        

        moRemarkTable.Rows.Add(trRemark);
    }

    /// <summary>
    /// This method is used to fill note details.
    /// </summary>
    private void FillNotes()
    {
        HtmlTableRow trNote;
        HtmlTableCell tdNote;
        moXseedProgressReportBL.GradeMaster.Where(grade => grade.ConsideredAsAbsent || grade.ConsideredAsExempted).OrderBy(grade => grade.SortOrder).ToList()
            .ForEach
            (
                grade =>
                {
                    trNote = new HtmlTableRow();
                    tdNote = new HtmlTableCell { InnerHtml = grade.GradeName + " - " + grade.Description, Align = "Left" };
                    tdNote.Style.Add("Padding-left", "5px");
                    tdNote.Attributes.Add("class", "clsLabel");
                    trNote.Cells.Add(tdNote);
                    moNoteTable.Rows.Add(trNote);
                }
            );
    }


    /// <summary>
    /// This method is used to create tables for progress report.
    /// </summary>
    private void CreateTables()
    {
        moTblStudentdetails = new HtmlTable();

        moTblGrades = new HtmlTable();
        moTblXseedLearningOutcomes = new HtmlTable();
        moTblNonXseedProgressReport = new HtmlTable();
        moTblCoCurricularSubjects = new HtmlTable();
        moNoteTable = new HtmlTable();
        moTblXseedLearningOutcomes.ID = "tblXseedLearningOutcomes";
        moTblXseedLearningOutcomes.EnableViewState = false;
        moRemarkTable = new HtmlTable();

        moTblNonXseedProgressReport.ID = "tblNonXseedProgressReport";
        moTblNonXseedProgressReport.EnableViewState = false;

        moTblCoCurricularSubjects.ID = "tblCoCurricularSubjects";
        moTblCoCurricularSubjects.EnableViewState = false;
        AddTable(moTblStudentdetails);
        AddTable(moTblGrades);
        AddTable(moTblXseedLearningOutcomes);
        AddTable(moTblNonXseedProgressReport);
        AddTable(moTblCoCurricularSubjects);
        AddTable(moRemarkTable);
        moRemarkTable.ID = "tblRemark";
        AddTable(moNoteTable);       

        CreateGradeHeaders();
        CreateLearningOutcomeHeader();
        CreateNonXseedOutcomesHeader();
        CreateCoCurricularSubjectsHeaders();
        CreateNoteTable();
    }

    /// <summary>
    /// This method is used to fill student details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillStudentDetails(int aiStudentId)
    {
        YearwiseStudentMaster oYearwiseStudentMaster = moXseedProgressReportBL.YearwiseStudentMaster
                               .Where(student => student.YearwiseStudentId == aiStudentId)
                               .FirstOrDefault();

        if (oYearwiseStudentMaster != null)
        {
            HtmlTableRow trStudentDetails = new HtmlTableRow();
            HtmlTableCell tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = "Roll No." };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");

            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell {Align = "Center"};
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            tdStudentDetails.InnerHtml = oYearwiseStudentMaster.RollNo.ToString();
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = "Name" };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell {Align = "Left", InnerHtml = oYearwiseStudentMaster.StudentName};
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = "Class" };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell {Align = "Left", InnerHtml = oYearwiseStudentMaster.Class};
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell { Align = "Center", InnerHtml = "Year" };
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
            trStudentDetails.Cells.Add(tdStudentDetails);

            tdStudentDetails = new HtmlTableCell {Align = "Left", InnerHtml = oYearwiseStudentMaster.AcademicYear};
            tdStudentDetails.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
            trStudentDetails.Cells.Add(tdStudentDetails);

            moTblStudentdetails.Rows.Add(trStudentDetails);
        }
    }

    /// <summary>
    /// This method is used to return school details.
    /// </summary>
    /// <param name="asClass"></param>
    /// <param name="asName"></param>
    /// <returns></returns>
    private HtmlTableRow GetSchoolDetails(string asClass, string asName)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell {ColSpan = 8, Align = "Center", InnerHtml = asName};
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Attributes.Add("class", asClass);
        moTblStudentdetails.Rows.Add(oHtmlTableRow);
        return oHtmlTableRow;
    }


    /// <summary>
    /// This method is used to fill grades.
    /// </summary>
    private void FillGrades()
    {
        moXseedProgressReportBL.GradeMaster.Where(grade => !grade.ConsideredAsAbsent && !grade.ConsideredAsExempted).ToList().ForEach
            (
                grade =>
                {
                    HtmlTableRow trGradeDetails = new HtmlTableRow();
                    HtmlTableCell tdGradeDetails = new HtmlTableCell();
                    tdGradeDetails.Style.Add("Padding-Left", "5px");
                    tdGradeDetails.InnerHtml = grade.GradeName;
                    trGradeDetails.Cells.Add(tdGradeDetails);

                    tdGradeDetails = new HtmlTableCell();
                    tdGradeDetails.Style.Add("Padding-Left", "5px");
                    tdGradeDetails.InnerHtml = grade.Description == string.Empty ? "-" : grade.Description;
                    trGradeDetails.Cells.Add(tdGradeDetails);

                    trGradeDetails.Attributes.Add("class", "ClsMarksCell");
                    moTblGrades.Rows.Add(trGradeDetails);
                }
            );
    }

    /// <summary>
    /// This method is used to fill assessment and attendance details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillAssessmentAndAttendanceDetails(int aiStudentId)
    {
        HtmlTableRow trAssessmentsAndAttendance = new HtmlTableRow();
        HtmlTableCell tdAssessmentsAndAttendance = new HtmlTableCell
                                                       {
                                                           InnerHtml = "Assessment",
                                                           Align = "Center",
                                                           Width = "110px"
                                                       };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        tdAssessmentsAndAttendance = new HtmlTableCell {ColSpan = 3};
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
        tdAssessmentsAndAttendance.InnerHtml = moXseedProgressReportBL.YearwiseStudentMaster.Where(student => student.YearwiseStudentId == aiStudentId).FirstOrDefault().Assessment;
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        tdAssessmentsAndAttendance = new HtmlTableCell { InnerHtml = "Attendance", ColSpan = 2, Align = "Right" };
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight");
        tdAssessmentsAndAttendance.Style.Add("Padding-Right", "5px");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        string sPresentDays = string.Empty;
        List<StudentAttendance> lstStudentAttendance = moXseedProgressReportBL.StudentAttendance.Where(attendance => attendance.YearwiseStudentId == aiStudentId).ToList();
        if (lstStudentAttendance.Count > 0)
            sPresentDays = lstStudentAttendance.Where(student => student.IsPresent).Count() + " Out Of " + lstStudentAttendance.Count;

        tdAssessmentsAndAttendance = new HtmlTableCell {ColSpan = 2, InnerHtml = sPresentDays};
        tdAssessmentsAndAttendance.Attributes.Add("Class", "ClsBGWhite ClsBorderlight ClsHilightTextB");
        trAssessmentsAndAttendance.Cells.Add(tdAssessmentsAndAttendance);

        moTblStudentdetails.Rows.Add(trAssessmentsAndAttendance);
    }

    /// <summary>
    /// This method is used to fill learning outcomes into table.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillLearingOutcomeDetails(int aiStudentId, int aiRemarkLength)
    {
        List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = moXseedProgressReportBL.SubjectSections
                               .Join(moXseedProgressReportBL.LearningOutcomesObservations, subjectSection => subjectSection.SubjectSectionConfigurationId, learningOutcome => learningOutcome.SubjectSectionConfigurationId, (subjectSection, outcome) => new { subjectSection, LearningOutcome = outcome })
                               .Where(student => student.LearningOutcome.YearwiseStudentId == aiStudentId)
                               .Select(subjetSection => subjetSection.subjectSection)
                               .Distinct()
                               .ToList();

        if (moXseedProgressReportBL.StudentsLearningOutcomes.Count > 0)
        {
            motrErrorMessage.Visible = false;
            List<int> lstOrders = new List<int>();
            lstSubjectSectionConfigurationMaster.Select(ssc => new { ssc.SortOrder, ssc.ShowSubjectRemarks, ssc.SubjectId}).Distinct().OrderBy(s => s.SortOrder).ToList().ForEach
                (
                order =>
                {
                    lstSubjectSectionConfigurationMaster.Where(sc => sc.SortOrder == order.SortOrder).ToList().ForEach
                                                (
                                                    subjectSection =>
                                                    {
                                                        HtmlTableRow trSubjectSection = GetSubjectSectionCell(subjectSection.SubjectSectionName);
                                                        moTblXseedLearningOutcomes.Rows.Add(trSubjectSection);
                                                        FillLearningOutcomes(subjectSection.SubjectSectionConfigurationId, subjectSection.SubjectSectionName, aiStudentId);
                                                    }
                                                );

                    if (order.ShowSubjectRemarks)
                    {
                        moTblXseedLearningOutcomes.Rows.Add(AddBlankRow("2px"));

                        HtmlTableRow trRmk = new HtmlTableRow();
                        trRmk.ID = "trSubjectRemark";

                        AddSubjectRemarkHeader(trRmk);

                        HtmlTableCell tdRmk = new HtmlTableCell
                        {   
                            InnerHtml = string.Empty,
                            Align = "left",
                            ColSpan=3,
                            VAlign="middle"
                        };

                        HtmlTable tbl = new HtmlTable { Width = "100%",ID="tblSubjectRemark" };
                        HtmlTableRow trsub = new HtmlTableRow { ID = "trRemark" };
                        HtmlTableCell tdSub = new HtmlTableCell
                        {
                            InnerHtml = string.Empty,
                            Align = "left",
                            VAlign = "middle"
                        };

                        TextBox txtRemark = new TextBox
                        {
                            ID = "txtSubjectRemark_" + order.SubjectId,
                            Text = string.Empty,
                            Width = Unit.Percentage(100),
                            TextMode = TextBoxMode.MultiLine,
                            Enabled =
                                !moXseedProgressReportBL.AssessmentPublishStatus &&
                                !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus                            
                        };

                        var oSubjectRemarks = moXseedProgressReportBL.SubjectRemarks.Where(rm => rm.SubjectId == order.SubjectId).FirstOrDefault();
                        if (oSubjectRemarks != null)
                            txtRemark.Text = oSubjectRemarks.Remark;

                        txtRemark.Attributes.Add("onkeyup", "UpdateRemarkLength(this)");
                        txtRemark.Attributes.Add("onpaste", "UpdateRemarkLength(this)");

                        Label lblCount = new Label();
                        lblCount.CssClass = "clsLabel";
                        lblCount.ID = "lblSubjectRemark_" + order.SubjectId;
                        lblCount.Text = "(" + (txtRemark.Text.Length == 0 ? aiRemarkLength : aiRemarkLength - txtRemark.Text.Length) + ")";
                        
                        tdSub.Controls.Add(txtRemark);
                        trsub.Cells.Add(tdSub);
                        
                        HtmlTableCell tdRemarkCount = new HtmlTableCell();                        
                        tdRemarkCount.Width = "50px";
                        tdRemarkCount.Controls.Add(lblCount);
                        trsub.Cells.Add(tdRemarkCount);
                        tbl.Rows.Add(trsub);
                        
                        tdRmk.Controls.Add(tbl);
                        trRmk.Cells.Add(tdRmk);
                        moTblXseedLearningOutcomes.Rows.Add(trRmk);

                        moTblXseedLearningOutcomes.Rows.Add(AddBlankRow("15px"));

                        if (!lstOrders.Any(s => s == order.SubjectId))
                            lstOrders.Add(order.SubjectId);                        
                    }

                    ViewState[Constants.S_SUBJECT_REMARK] = lstOrders;
                }
            );

            moTblXseedLearningOutcomes.Visible = lstSubjectSectionConfigurationMaster.Count > 0;
            FillNonXseedSubjectDetails(aiStudentId);
        }
        else
            motrErrorMessage.Visible = true;
    }

    /// <summary>
    /// This method is used to add subject remark header.
    /// </summary>
    /// <param name="trRmk"></param>
    private static void AddSubjectRemarkHeader(HtmlTableRow trRmk)
    {
        HtmlTableCell tdRmkHeader = new HtmlTableCell
        {
            InnerHtml = "Subject Remark",
            Align = "center",
            ColSpan = 1
        };

        trRmk.Attributes.Add("class", "Lbl10pt ClsMarksCell");

        trRmk.Cells.Add(tdRmkHeader);
    }

    /// <summary>
    /// This method is used to add blank row.
    /// </summary>
    /// <param name="asHeight"></param>
    /// <returns></returns>
    private static HtmlTableRow AddBlankRow(string asHeight)
    {
        HtmlTableRow tr = new HtmlTableRow();
        HtmlTableCell tdBreak = new HtmlTableCell
        {
            InnerHtml = string.Empty,
            Align = "center",
            ColSpan = 4,
            Height = asHeight
        };

        tr.Cells.Add(tdBreak);
        return tr;
    }

    /// <summary>
    /// This method is used to add seperator.
    /// </summary>
    private void AddSeperater()
    {
        HtmlTableRow trSeperator = new HtmlTableRow();
        HtmlTableCell tdSeperator = new HtmlTableCell
                                        {
                                            Width = "100%",
                                            InnerHtml =
                                                "------------------------------------------------------------------------------------------------------------------------------------------------------------------------"
                                        };
        trSeperator.Cells.Add(tdSeperator);
        trSeperator.Height = "10px";
        motblMainProgressReport.Rows.Add(trSeperator);
    }

    /// <summary>
    /// This method is used to fill non xseed subject details.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void FillNonXseedSubjectDetails(int aiStudentId)
    {
        List<NonXseedSubjectGrades> lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && !grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(
            grade => {
                int iRowIndex = 0;
                moTblNonXseedProgressReport.Rows.Add(GetSubjectGradeCell("ddlNonXseedGrade", grade, ++iRowIndex));
            }
        );
        moTblNonXseedProgressReport.Visible = lstNonXseedSubjectGardes.Count > 0;

        lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(grade =>
            {
                int iRowIndex = 0;
                moTblCoCurricularSubjects.Rows.Add(GetSubjectGradeCell("ddlCoCurricularSubjectsGrade", grade, ++iRowIndex));
            }
        );
        moTblCoCurricularSubjects.Visible = lstNonXseedSubjectGardes.Count > 0;
    }

    /// <summary>
    /// This method is used to returnsubject grade cell.
    /// </summary>
    /// <param name="aoGrade"></param>
    /// <returns></returns>
    private HtmlTableRow GetSubjectGradeCell(string asGradeName, NonXseedSubjectGrades aoGrade, int aiRowIndex)
    {
        HtmlTableRow trGradeDetails = new HtmlTableRow();
        HtmlTableCell tdGradeDetails = new HtmlTableCell
                                           {
                                               InnerHtml = aoGrade.SubjectName,
                                               ColSpan = 2,
                                               Align = "left"
                                           };

        tdGradeDetails.Style.Add("padding-left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell();
        if (!mbIsEditMode)
            tdGradeDetails.InnerHtml = aoGrade.ShortName;
        else
        {
            DropDownList oddlGrade = new DropDownList
                                         {
                                             ID = asGradeName + "_" + aoGrade.SubjectId + "_" + aoGrade.SubjectName + "_" + aiRowIndex,
                                             DataSource = moXseedProgressReportBL.GradeMaster,
                                             DataTextField = "GradeName",
                                             DataValueField = "GradeId",
                                             Enabled =
                                                 !moXseedProgressReportBL.AssessmentPublishStatus &&
                                                 !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus
                                         };
            oddlGrade.Attributes.Add("onchange", "EnableDisableObservtionControl(this,'txtObservation_" + aoGrade.SubjectId + "')");
            oddlGrade.DataBind();
            oddlGrade.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            oddlGrade.SelectedValue = aoGrade.GradeId.ToString();
            tdGradeDetails.Controls.Add(oddlGrade);
        }
        tdGradeDetails.Align = "Center";
        trGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell();
        if (!mbIsEditMode)
            tdGradeDetails.InnerHtml = aoGrade.Observation;
        else
        {
            TextBox otxtObservation = new TextBox
                                          {
                                              ID = "txtObservation_" + aoGrade.SubjectId,
                                              Text = aoGrade.Observation,
                                              Width = Unit.Pixel(300),
                                              TextMode = TextBoxMode.MultiLine,
                                              Enabled =
                                                  !moXseedProgressReportBL.AssessmentPublishStatus &&
                                                  !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus &&
                                                  aoGrade.GradeId != 9 && aoGrade.GradeId != 10,
                                              Rows = 2
                                          };
            //if (miSchoolId != Constants.SchoolId.BFS.ToInt())
            otxtObservation.Enabled = false;

            //otxtObservation.Attributes.Add("onblur", "Validate(this,500,'" + motblMainProgressReport.ClientID + "')");
            tdGradeDetails.Controls.Add(otxtObservation);
        }
        tdGradeDetails.Align = "left";
        tdGradeDetails.Style.Add("padding-left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        trGradeDetails.Attributes.Add("class", " ClsMarksCell");

        return trGradeDetails;
    }

    /// <summary>
    /// This method is used to fill learning outcomes.
    /// </summary>
    /// <param name="aiSubjectSectionConfigurationId"></param>
    /// <param name="aiStudentId"></param>
    private void FillLearningOutcomes(int aiSubjectSectionConfigurationId, string asSubjectSectionName, int aiStudentId)
    {
        string sObservation = moXseedProgressReportBL.LearningOutcomesObservations
            .Where(observation => observation.SubjectSectionConfigurationId == aiSubjectSectionConfigurationId && observation.YearwiseStudentId == aiStudentId)
            .Select(observation => observation.Observation)
            .FirstOrDefault();

        List<StudentsLearningOutcome> lstStudentsLearningOutcomes = moXseedProgressReportBL.StudentsLearningOutcomes
                                .Where(outcome => outcome.SubjectSectionConfigId == aiSubjectSectionConfigurationId && outcome.YearwiseStudentId == aiStudentId).OrderBy(sortorder => sortorder.LearningOutcomeSortOrder)
                                .ToList();

        HtmlTableRow trFirstRow;
        bool bIsFirstRow = false;
        HtmlTableRow oHtmlTableFirstRow = new HtmlTableRow();
        int iRowIndex = 1;
        int iGradeId = 0;
        lstStudentsLearningOutcomes.ForEach
            (
                learningOutcome =>
                {
                    trFirstRow = GetLearningOutcomeCell(learningOutcome.LearningOutcome, learningOutcome.ShortName, iRowIndex++, aiSubjectSectionConfigurationId, asSubjectSectionName, learningOutcome.LearningOutcomeConfigId, learningOutcome.LearningOutcomeGradeId);
                    if (!bIsFirstRow)
                    {
                        oHtmlTableFirstRow = trFirstRow;
                        bIsFirstRow = true;
                    }
                    iGradeId = learningOutcome.GradeId;
                    moTblXseedLearningOutcomes.Rows.Add(trFirstRow);
                }
            );

        HtmlTableCell tdObservation;
        //if(miSchoolId == Constants.SchoolId.BFS.ToInt())
        //    tdObservation = CreateObservationCell(sObservation, lstStudentsLearningOutcomes.Count, aiSubjectSectionConfigurationId, true);
        //else
            tdObservation = CreateObservationCell(sObservation, lstStudentsLearningOutcomes.Count, aiSubjectSectionConfigurationId, (iGradeId != 9 && iGradeId != 10));
        
        tdObservation.RowSpan = lstStudentsLearningOutcomes.Count;
        oHtmlTableFirstRow.Cells.Add(tdObservation);
    }

    /// <summary>
    /// This method is used to create observation cell.
    /// </summary>
    /// <param name="asObservation"></param>
    /// <param name="aiRowSpan"></param>
    /// <param name="aiSubjectSectionConfigurationId"></param>
    /// <returns></returns>
    private HtmlTableCell CreateObservationCell(string asObservation, int aiRowSpan, int aiSubjectSectionConfigurationId, bool abEnable)
    {
        HtmlTableCell tdObservation = new HtmlTableCell();
        if (!mbIsEditMode)
            tdObservation.InnerHtml = asObservation;
        else
        {
            TextBox otxtObservation = new TextBox
                                          {
                                              ID = "txtLearningObservation_" + aiSubjectSectionConfigurationId,
                                              Text = asObservation,
                                              Width = Unit.Pixel(300),
                                              TextMode = TextBoxMode.MultiLine,
                                              Enabled =
                                                  !moXseedProgressReportBL.AssessmentPublishStatus &&
                                                  !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus && abEnable,
                                              Rows = 2
                                          };
            
           // if (miSchoolId != Constants.SchoolId.BFS.ToInt())
            otxtObservation.Enabled = false;
            tdObservation.Controls.Add(otxtObservation);
        }
        tdObservation.Align = "left";
        tdObservation.Style.Add("padding-left", "5px");
        tdObservation.RowSpan = aiRowSpan;
        return tdObservation;
    }

    /// <summary>
    /// This method is used to return subject section cell.
    /// </summary>
    /// <param name="asSubjectSection"></param>
    /// <returns></returns>
    private HtmlTableRow GetSubjectSectionCell(string asSubjectSection)
    {
        HtmlTableRow trSubjectSection = new HtmlTableRow();
        HtmlTableCell tdSubjectSection = new HtmlTableCell
                                             {
                                                 ColSpan = 4,
                                                 InnerHtml = asSubjectSection,
                                                 Align = "center"
                                             };
        tdSubjectSection.Attributes.Add("class", "ProgressReportHeader");
        trSubjectSection.Cells.Add(tdSubjectSection);
        return trSubjectSection;
    }

    /// <summary>
    /// This method is used to return learning outcome cell.
    /// </summary>
    /// <param name="asLearningOutcome"></param>
    /// <param name="asGradeName"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiSubjectSectionConfigurationId"></param>
    /// <param name="aiLearningOutcomeConfigId"></param>
    /// <param name="aiLearningOutcomeGradeId"></param>
    /// <returns></returns>
    private HtmlTableRow GetLearningOutcomeCell(string asLearningOutcome, string asGradeName, int aiRowIndex, int aiSubjectSectionConfigurationId, string asSubjectSectionName, int aiLearningOutcomeConfigId, int aiLearningOutcomeGradeId)
    {
        HtmlTableRow trLearningOutcome = new HtmlTableRow();

        HtmlTableCell tdLearningOutcome = new HtmlTableCell {Align = "Center", InnerHtml = aiRowIndex.ToString()};
        trLearningOutcome.Cells.Add(tdLearningOutcome);

        tdLearningOutcome = new HtmlTableCell {InnerHtml = asLearningOutcome, Align = "left"};
        tdLearningOutcome.Style.Add("padding-left", "5px");
        trLearningOutcome.Cells.Add(tdLearningOutcome);

        tdLearningOutcome = new HtmlTableCell();
        if (!mbIsEditMode)
            tdLearningOutcome.InnerHtml = asGradeName;
        else
        {
            int iLearningOutcomesObservationId = 0;
            moXseedProgressReportBL.LearningOutcomesObservations.Where(observation => observation.SubjectSectionConfigurationId == aiSubjectSectionConfigurationId && observation.YearwiseStudentId == miStudentId).ToList()
                                                     .ForEach
                                                     (
                                                         observation =>
                                                             iLearningOutcomesObservationId = observation.LearningOutcomesObservationId
                                                     );
            DropDownList oddlGrade = new DropDownList
                                         {
                                             DataSource = moXseedProgressReportBL.GradeMaster,
                                             DataTextField = "GradeName",
                                             ID =
                                                 "ddlLearningGrade_" + aiSubjectSectionConfigurationId + "_" +
                                                 aiLearningOutcomeConfigId + "_" + aiLearningOutcomeGradeId + "_" +
                                                 iLearningOutcomesObservationId + "_" + asSubjectSectionName + "_" + aiRowIndex,
                                             DataValueField = "GradeId",
                                             Enabled =
                                                 !moXseedProgressReportBL.AssessmentPublishStatus &&
                                                 !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus
                                         };
            oddlGrade.DataBind();
            oddlGrade.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            oddlGrade.Attributes.Add("onchange", "EnableDisableObservtionControl(this,'txtLearningObservation_" + aiSubjectSectionConfigurationId + "')");
            if (mbSetGrade)
                moXseedProgressReportBL.GradeMaster.Where(grade => grade.GradeName == asGradeName.Trim()).ToList().ForEach(grade => oddlGrade.SelectedValue = grade.GradeId.ToString());
            tdLearningOutcome.Controls.Add(oddlGrade);
        }
        tdLearningOutcome.Align = "Center";
        trLearningOutcome.Cells.Add(tdLearningOutcome);
        trLearningOutcome.Attributes.Add("class", "Lbl10pt ClsMarksCell");

        return trLearningOutcome;
    }

    /// <summary>
    /// This method is used to create progress report note table.
    /// </summary>
    private void CreateNoteTable()
    {
        AddEmptyRow(moNoteTable);
        HtmlTableRow trProgressReportNote = new HtmlTableRow();
        HtmlTableCell tdProgressReportNote = new HtmlTableCell { InnerHtml = "Note" + " :", Align = "left" };
        tdProgressReportNote.Style.Add("Padding-Left", "5px");
        tdProgressReportNote.Attributes.Add("class", "clsLabel");
        trProgressReportNote.Cells.Add(tdProgressReportNote);
        moNoteTable.Rows.Add(trProgressReportNote);
    }

    /// <summary>
    /// This method is used to createco-curricular subjects.
    /// </summary>
    private void CreateCoCurricularSubjectsHeaders()
    {
        HtmlTableRow trCoCurricularSubjectsHeaders = new HtmlTableRow();

        trCoCurricularSubjectsHeaders.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = "Co-Curricular Subjects", Align = "Center" });
        trCoCurricularSubjectsHeaders.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblCoCurricularSubjects.Rows.Add(trCoCurricularSubjectsHeaders);

        trCoCurricularSubjectsHeaders = new HtmlTableRow();
        HtmlTableCell tdCoCurricularSubjectsHeaders = new HtmlTableCell { ColSpan = 2, InnerHtml = "Subject", Align = "Left", Width = "240px" };
        tdCoCurricularSubjectsHeaders.Style.Add("Padding-Left", "5px");
        trCoCurricularSubjectsHeaders.Cells.Add(tdCoCurricularSubjectsHeaders);
        if (mbIsStudentWiseProgressReport && mbIsEditMode)
        {
            HtmlTableCell tdGrade = new HtmlTableCell();
            
            DropDownList oddlGrade = new DropDownList
            {
                ID = "ddlDefaultCoCurricularGrade",
                Enabled =
                    !moXseedProgressReportBL.AssessmentPublishStatus &&
                    !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus,
                DataSource = moXseedProgressReportBL.GradeMaster,
                DataTextField = "GradeName",
                DataValueField = "GradeId",
            };
            
            oddlGrade.DataBind();
            oddlGrade.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            oddlGrade.Attributes.Add("onchange", "SetDefaultGrade('ddlCoCurricularSubjectsGrade',this.value)"); 
            tdGrade.Controls.Add(oddlGrade);
            trCoCurricularSubjectsHeaders.Cells.Add(tdGrade);
        }
        else
            trCoCurricularSubjectsHeaders.Cells.Add(new HtmlTableCell { InnerHtml = "Grade", Width = "50px", Align = "Center" });

        tdCoCurricularSubjectsHeaders = new HtmlTableCell { InnerHtml = "Facilitator's Observation", Width = "550px", Align = "Left" };
        tdCoCurricularSubjectsHeaders.Style.Add("Padding-Left", "5px");
        trCoCurricularSubjectsHeaders.Cells.Add(tdCoCurricularSubjectsHeaders);

        trCoCurricularSubjectsHeaders.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblCoCurricularSubjects.Rows.Add(trCoCurricularSubjectsHeaders);
    }

    /// <summary>
    /// This method is used to create non xseed learning outcome header.
    /// </summary>
    private void CreateNonXseedOutcomesHeader()
    {
        HtmlTableRow trNonXseedProgressReport = new HtmlTableRow();

        string sFirstCellWidth = "400px";
        string sLastCellWidth = "550px";
        if (miSchoolId != Constants.SchoolId.JPS.ToInt())
        {
            trNonXseedProgressReport.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = "Curricular Subjects", Align = "Center" });
            trNonXseedProgressReport.Attributes.Add("Class", "HeadTxtBWOPadding");
            moTblNonXseedProgressReport.Rows.Add(trNonXseedProgressReport);
        }
        else
        {
            sFirstCellWidth = "440px";
            sLastCellWidth = "";
        }

        trNonXseedProgressReport = new HtmlTableRow();
        HtmlTableCell tdNonXseedProgressReport = new HtmlTableCell { ColSpan = 2, InnerHtml = "Subject", Align = "Left", Width = sFirstCellWidth };
        tdNonXseedProgressReport.Style.Add("Padding-Left", "5px");
        trNonXseedProgressReport.Cells.Add(tdNonXseedProgressReport);

        if (mbIsStudentWiseProgressReport && mbIsEditMode)
        {
            HtmlTableCell tdGrade = new HtmlTableCell();

            DropDownList oddlGrade = new DropDownList
            {
                ID = "ddlDefaultNonXseedGrade",
                Enabled =
                    !moXseedProgressReportBL.AssessmentPublishStatus &&
                    !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus,
                DataSource = moXseedProgressReportBL.GradeMaster,
                DataTextField = "GradeName",
                DataValueField = "GradeId",
            };

            oddlGrade.DataBind();
            oddlGrade.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            oddlGrade.Attributes.Add("onchange", "SetDefaultGrade('ddlNonXseedGrade',this.value)");
            tdGrade.Controls.Add(oddlGrade);
            trNonXseedProgressReport.Cells.Add(tdGrade);
        }
        else
            trNonXseedProgressReport.Cells.Add(new HtmlTableCell { InnerHtml = "Grade", Width = "50px", Align = "Center" });

        tdNonXseedProgressReport = new HtmlTableCell { InnerHtml = "Facilitator's Observation", Width = sLastCellWidth, Align = "Left" };
        tdNonXseedProgressReport.Style.Add("Padding-Left", "5px");
        trNonXseedProgressReport.Cells.Add(tdNonXseedProgressReport);
        trNonXseedProgressReport.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblNonXseedProgressReport.Rows.Add(trNonXseedProgressReport);
    }

    /// <summary>
    /// This method is use to create learning outcome table header,
    /// </summary>
    private void CreateLearningOutcomeHeader()
    {
        HtmlTableRow trLearningOutcome = new HtmlTableRow();

        if (miSchoolId != Constants.SchoolId.JPS.ToInt())
        {
            trLearningOutcome.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = "Pre-Primary Curricular Subjects", Align = "Center" });
            trLearningOutcome.Attributes.Add("Class", "HeadTxtBWOPadding");
            moTblXseedLearningOutcomes.Rows.Add(trLearningOutcome);
        }

        trLearningOutcome = new HtmlTableRow();
        trLearningOutcome.Cells.Add(new HtmlTableCell { InnerHtml = "Sr. No.", Align = "Center", Width = "60px" });

        HtmlTableCell tdLearningOutcome = new HtmlTableCell { InnerHtml = "Learning Outcome", Align = "Left", Width = "380px" };
        tdLearningOutcome.Style.Add("Padding-Left", "5px");
        trLearningOutcome.Cells.Add(tdLearningOutcome);
        if (mbIsStudentWiseProgressReport && mbIsEditMode)
        {
            HtmlTableCell tdGrade = new HtmlTableCell();

            DropDownList oddlGrade = new DropDownList
            {
                ID = "ddlDefaultLearningOutcomeGrade",
                Enabled =
                    !moXseedProgressReportBL.AssessmentPublishStatus &&
                    !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus,
                DataSource = moXseedProgressReportBL.GradeMaster,
                DataTextField = "GradeName",
                DataValueField = "GradeId",
            };

            oddlGrade.DataBind();
            oddlGrade.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.S_ZERO));
            oddlGrade.Attributes.Add("onchange", "SetDefaultGrade('ddlLearningGrade',this.value)");
            tdGrade.Controls.Add(oddlGrade);
            trLearningOutcome.Cells.Add(tdGrade);
        }
        else
            trLearningOutcome.Cells.Add(new HtmlTableCell { InnerHtml = "Grade", Width = "50px", Align = "Center" });

        tdLearningOutcome = new HtmlTableCell { InnerHtml = "Facilitator's Observation", Align = "Left" };
        tdLearningOutcome.Style.Add("Padding-Left", "5px");
        trLearningOutcome.Cells.Add(tdLearningOutcome);
        trLearningOutcome.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblXseedLearningOutcomes.Rows.Add(trLearningOutcome);
    }

    /// <summary>
    /// This method is used to create grade header.
    /// </summary>
    private void CreateGradeHeaders()
    {
        HtmlTableRow trGradeDetails = new HtmlTableRow();
        trGradeDetails.Cells.Add(new HtmlTableCell { ColSpan = 2, InnerHtml = "Key to Curricular and Co-Curricular", Align = "Center" });
        trGradeDetails.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblGrades.Rows.Add(trGradeDetails);

        trGradeDetails = new HtmlTableRow();
        HtmlTableCell tdGradeDetails = new HtmlTableCell { InnerHtml = "Grade", Align = "left" };
        tdGradeDetails.Style.Add("Padding-Left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        tdGradeDetails = new HtmlTableCell { InnerHtml = "Description", Align = "left", Width = "80%" };
        tdGradeDetails.Style.Add("Padding-Left", "5px");
        trGradeDetails.Cells.Add(tdGradeDetails);

        trGradeDetails.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblGrades.Rows.Add(trGradeDetails);
    }

    /// <summary>
    /// This method is used to add sub tables into main table.
    /// </summary>
    /// <param name="ahtmlTable"></param>
    private void AddTable(HtmlTable ahtmlTable)
    {
        HtmlTableRow trSubTable = new HtmlTableRow();
        HtmlTableCell tdSubTable = new HtmlTableCell {Width = "90%"};
        tdSubTable.Controls.Add(ahtmlTable);
        trSubTable.Cells.Add(tdSubTable);
        tdSubTable.Align = "Center";
        ahtmlTable.Width = "90%";
        motblMainProgressReport.Rows.Add(trSubTable);
    }

    /// <summary>
    /// This method is used to add empty ow in given table.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    private void AddEmptyRow(HtmlTable aoHtmlTable)
    {
        HtmlTableRow trEmpty = new HtmlTableRow();
        HtmlTableCell tdEmpty = new HtmlTableCell {Width = "90%"};
        trEmpty.Cells.Add(tdEmpty);
        trEmpty.Height = "10px";
        aoHtmlTable.Rows.Add(trEmpty);
    }
}