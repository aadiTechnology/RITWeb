using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
using System.Web.UI.WebControls;

public partial class XseedProgressReportPrint : SchoolBase
{
    #region Data Members

    XseedProgressReportBL moXseedProgressReportBL;
    HtmlTable moTblStudentdetails;
    HtmlTable moTblGrades;
    HtmlTable moTblXseedLearningOutcomes;
    HtmlTable moTblNonXseedProgressReport;
    HtmlTable moTblCoCurricularSubjects;
    HtmlTable moNoteTable;
    HtmlTable moRemarkTable;

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
	    try
	    {		    
		    if (!IsPostBack)
		    {
			    ReadQueryString();
			    DisplayProgressReport();
		    }
	    }
	    catch (Exception ex)
	    {
		    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	    }
    }

    #endregion

    #region Methods
 
    /// <summary>
    /// This method is used to decrypt queystring.
    /// </summary>
    private void ReadQueryString()
    {
	    if (QueryString.Count <= 0)
		    return;
	    
		hidAssessment.Value = QueryString["AssessmentId"];
	    hidstdDivId.Value = QueryString["StandardDivisionId"];
	    hidStudentId.Value = QueryString["StudentId"];
	    miAcademicYearId = QueryString["AcademicYearId"].ToInt();
    } 

    private void CreateTables()
    {
        moTblStudentdetails = new HtmlTable();

        moTblGrades = new HtmlTable();
        moTblXseedLearningOutcomes = new HtmlTable();
        moTblNonXseedProgressReport = new HtmlTable();
        moTblCoCurricularSubjects = new HtmlTable();
        moNoteTable = new HtmlTable();
        moRemarkTable = new HtmlTable();

        AddTable(moTblStudentdetails);
        AddTable(moTblGrades);
        AddTable(moTblXseedLearningOutcomes);
        AddTable(moTblNonXseedProgressReport);
        AddTable(moTblCoCurricularSubjects);
        AddTable(moRemarkTable);
        AddNoteTable(moNoteTable);

        CreateGradeHeaders();
        CreateXseedOutcomesHeader();
        CreateNonXseedOutcomesHeader();
        CreateCoCurricularSubjects();
        CreateNoteTable();
    }

    private void CreateNoteTable()
    {
        //AddEmptyRow(moNoteTable);
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Note + ":", Align = "left" };        
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Attributes.Add("class", "ClsLabel");
        moNoteTable.Rows.Add(oHtmlTableRow);
    }

    private void CreateCoCurricularSubjects()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();

        oHtmlTableRow.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = Resources.LocalizedResources.CoCurricularSubjects, Align = "Center" });
        oHtmlTableRow.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblCoCurricularSubjects.Rows.Add(oHtmlTableRow);

        oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.Subject, Align = "left", Width = "240px" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade, Width = "50px", Align = "Center" });

        oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation, Width = "550px", Align = "Left" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblCoCurricularSubjects.Rows.Add(oHtmlTableRow);
    }

    private void CreateNonXseedOutcomesHeader()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oHtmlTableRow.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = Resources.LocalizedResources.NonXseedCurricularSubjects, Align = "Center" });
        oHtmlTableRow.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblNonXseedProgressReport.Rows.Add(oHtmlTableRow);

        oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.Subject, Align = "left", Width = "400px" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade, Width = "50px", Align = "Center" });

        oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation, Width = "550px", Align = "Left" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblNonXseedProgressReport.Rows.Add(oHtmlTableRow);
    }

    private void CreateXseedOutcomesHeader()
    {
        string sHeader = Resources.LocalizedResources.XseedCurricularSubjects;

        if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            sHeader = "Pre-Primary Curricular Subjects";

        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oHtmlTableRow.Cells.Add(new HtmlTableCell { ColSpan = 4, InnerHtml = sHeader, Align = "Center" });
        oHtmlTableRow.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblXseedLearningOutcomes.Rows.Add(oHtmlTableRow);

        oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell;
        oHtmlTableRow.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.SrNo, Align = "Center", Width = "60px" });

        oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.LearningOutcome, Align = "Left", Width = "380px" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow.Cells.Add(new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade, Width = "50px", Align = "Center" });

        oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.FacilitatorsObservation, Align = "Left" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblXseedLearningOutcomes.Rows.Add(oHtmlTableRow);
    }

    private void CreateGradeHeaders()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        oHtmlTableRow.Cells.Add(new HtmlTableCell { ColSpan = 2, InnerHtml = Resources.LocalizedResources.KeyToCurricularAndCoCurricular, Align = "Center" });
        oHtmlTableRow.Attributes.Add("Class", "HeadTxtBWOPadding");
        moTblGrades.Rows.Add(oHtmlTableRow);

        oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Grade, Align = "left" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell { InnerHtml = Resources.LocalizedResources.Description, Align = "left", Width = "80%" };
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow.Attributes.Add("class", "ClsProgressGridTestHeader");
        moTblGrades.Rows.Add(oHtmlTableRow);
    }

    private void AddTable(HtmlTable oHtmlTable)
    {
        HtmlTableRow trStudentdetails = new HtmlTableRow();
        HtmlTableCell tdStudentdetails = new HtmlTableCell();
        tdStudentdetails.Width = "90%";
        tdStudentdetails.Controls.Add(oHtmlTable);
        trStudentdetails.Cells.Add(tdStudentdetails);
        tdStudentdetails.Align = "Center";
        oHtmlTable.Width = "90%";
        oHtmlTable.Border = 1;
        tblMainProgressReport.Rows.Add(trStudentdetails);
    }

    private void AddNoteTable(HtmlTable oHtmlTable)
    {
        HtmlTableRow trStudentdetails = new HtmlTableRow();
        HtmlTableCell tdStudentdetails = new HtmlTableCell();
        tdStudentdetails.Width = "90%";
        tdStudentdetails.Controls.Add(oHtmlTable);
        trStudentdetails.Cells.Add(tdStudentdetails);
        tdStudentdetails.Align = "Center";
        oHtmlTable.Width = "90%";        
        tblMainProgressReport.Rows.Add(trStudentdetails);
    }

    private void AddEmptyRow(HtmlTable oHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Width = "90%";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Height = "10px";
        oHtmlTable.Rows.Add(oHtmlTableRow);
    }

    private void DisplayProgressReport()
    {
        int iStudentId;
        int iAssessmentId;
        int iStandDivisionId;

        iStudentId = Convert.ToInt32(hidStudentId.Value);
        iAssessmentId = Convert.ToInt32(hidAssessment.Value);
        iStandDivisionId = Convert.ToInt32(hidstdDivId.Value);

        moXseedProgressReportBL = new XseedProgressReportBL
        {
            ExamResult = new ExamResult
            {
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                AssessmentId = iAssessmentId,
                YearwiseStudentId = iStudentId,
                StandardDivisionId = iStandDivisionId
            }
        };

        moXseedProgressReportBL.GetXseedProgressReport();


        bool bisReportAvailable = false;

        if (moXseedProgressReportBL.YearwiseStudentMaster.Count > 0 && (moXseedProgressReportBL.AssessmentPublishStatus || moXseedProgressReportBL.StudentWiseAssessmentPublishStatus))
        {
            trErrorMessage.Visible = false;
            moXseedProgressReportBL.YearwiseStudentMaster.ForEach
             (
                student =>
                {
                    List<StudentsLearningOutcome> outcomes = moXseedProgressReportBL.StudentsLearningOutcomes.Where(stud => stud.YearwiseStudentId == student.YearwiseStudentId).ToList();
                    if (outcomes.Count > 0)
                    {
                        CreateTables();
                        moTblStudentdetails.Rows.Add(GetSchoolDetailsCell("SocietyName", moXseedProgressReportBL.SchoolEntity.OrganizationName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetailsCell("ActualSchoolName", moXseedProgressReportBL.SchoolEntity.SchoolName));
                        moTblStudentdetails.Rows.Add(GetSchoolDetailsCell("ClsReportHead", Resources.LocalizedResources.ProgressReport));
                        FillStudentDetails(student.YearwiseStudentId);
                        FillGrades();
                        FillAssessmentAndGradeDetails(student.YearwiseStudentId);
                        FillLearingOutcomeDetails(student.YearwiseStudentId);
                        FillRemark(student.YearwiseStudentId);
                        FillNotes();
                        AddSeperater();
                        bisReportAvailable = true;
                    }
                }
            );
        }

        if (!bisReportAvailable)
        {
            trErrorMessage.Visible = true;
        }
        else
            tblMainProgressReport.Rows.RemoveAt(tblMainProgressReport.Rows.Count - 1);
    }

    /// <summary>
    /// This method is used to display remark.
    /// </summary>
    /// <param name="aiYearwiseStudentId"></param>
    private void FillRemark(int aiYearwiseStudentId)
    {
        string sRemark = string.Empty;

        if (moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Any())
            sRemark = moXseedProgressReportBL.XseedRemarks.Where(rmk => rmk.YearwiseStudentId == aiYearwiseStudentId).Select(rmk => rmk.Remark).FirstOrDefault();

        if (sRemark.Trim() != string.Empty)
        {
            HtmlTableRow trRemark = new HtmlTableRow();
            HtmlTableCell tdRemark = new HtmlTableCell();
            Label lblRemark = new Label { Text = "Remark ", CssClass = "ClsLabel" };
            tdRemark.Width = "100px";
            tdRemark.Attributes.Add("class", "ClsBorderLight");
            tdRemark.Controls.Add(lblRemark);
            trRemark.Cells.Add(tdRemark);

            tdRemark = new HtmlTableCell();

            Label lblComment = new Label();
            lblComment.CssClass = "ClsLabel";
            lblComment.Width = Unit.Percentage(100);

            lblComment.Text = sRemark;

            tdRemark.Controls.Add(lblComment);
            tdRemark.Align = "Justified";
            tdRemark.Attributes.Add("Padding-Left", "5px");
            tdRemark.Attributes.Add("Padding-Top", "5px");
            tdRemark.Attributes.Add("Class", "ClsBorderLight");

            trRemark.Cells.Add(tdRemark);
            moRemarkTable.Rows.Add(trRemark);
        }
    }

    private void FillNotes()
    {
        moXseedProgressReportBL.GradeMaster.Where(grade => grade.ConsideredAsAbsent || grade.ConsideredAsExempted).OrderBy(grade => grade.SortOrder).ToList()
            .ForEach
            (
                grade =>
                {
                    HtmlTableRow oHtmlTableRow = new HtmlTableRow();
                    HtmlTableCell oHtmlTableCell = new HtmlTableCell { InnerHtml = grade.GradeName + " - " + grade.Description, Align = "Left" };
                    oHtmlTableCell.Style.Add("Padding-left", "5px");
                    oHtmlTableRow.Cells.Add(oHtmlTableCell);
                    oHtmlTableRow.Attributes.Add("class", "ClsLabel");
                    moNoteTable.Rows.Add(oHtmlTableRow);
                }
            );
    }

    private void AddSeperater()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Width = "100%";
        oHtmlTableCell.InnerHtml = "--------------------------------------------------------------------------------------------------------------------------------------------";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Height = "10px";
        tblMainProgressReport.Rows.Add(oHtmlTableRow);
    }

    private void FillAssessmentAndGradeDetails(int aiStudentId)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Height = "17px";
        oHtmlTableCell.InnerHtml = Resources.LocalizedResources.Assessment;
        oHtmlTableCell.Align = "Center";
        oHtmlTableCell.Width = "110px";
        oHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.ColSpan = 3;
        oHtmlTableCell.Align = "Left";
        oHtmlTableCell.Height = "17px";
        oHtmlTableCell.Width = Unit.Pixel(324).ToString();
        oHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
        oHtmlTableCell.InnerHtml = moXseedProgressReportBL.YearwiseStudentMaster.Where(student => student.YearwiseStudentId == aiStudentId).FirstOrDefault().Assessment;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = Resources.LocalizedResources.Attendance;
        oHtmlTableCell.ColSpan = 2;
        oHtmlTableCell.Align = "Right";
        oHtmlTableCell.Height = "17px";
        oHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
        oHtmlTableCell.Style.Add("Padding-Right", "5px");
        oHtmlTableCell.Width = Unit.Pixel(191).ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        string sPresentDays = string.Empty;
        List<StudentAttendance> lstStudentAttendance = moXseedProgressReportBL.StudentAttendance.Where(attendance => attendance.YearwiseStudentId == aiStudentId).ToList();
        if (lstStudentAttendance.Count > 0)
            sPresentDays = lstStudentAttendance.Where(student => student.IsPresent).Count() + " Out Of " + lstStudentAttendance.Count;

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.ColSpan = 2;
        oHtmlTableCell.InnerHtml = sPresentDays;
        oHtmlTableCell.Align = "Left";
        oHtmlTableCell.Height = "17px";
        oHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        moTblStudentdetails.Rows.Add(oHtmlTableRow);
    }

    private void FillGrades()
    {
        moXseedProgressReportBL.GradeMaster.Where(grade => !grade.ConsideredAsAbsent && !grade.ConsideredAsExempted).ToList().ForEach(grade => CreateGradeRow(grade));
    }

    private void CreateGradeRow(GradeMaster grade)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = "Left";
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableCell.InnerHtml = grade.GradeName;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = "Left";
        oHtmlTableCell.Style.Add("Padding-Left", "5px");
        oHtmlTableCell.InnerHtml = grade.Description == string.Empty ? "-" : grade.Description;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow.Attributes.Add("class", "ClsLabel ClsMarksCell");
        moTblGrades.Rows.Add(oHtmlTableRow);
    }

    private void FillLearingOutcomeDetails(int iStudentId)
    {
        HtmlTableRow oSubjectSection = null;
        List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = moXseedProgressReportBL.SubjectSections
                               .Join(moXseedProgressReportBL.LearningOutcomesObservations, subjectSection => subjectSection.SubjectSectionConfigurationId, outcome => outcome.SubjectSectionConfigurationId, (subjectSection, outcome) => new { subjectSection = subjectSection, LearningOutcome = outcome })
                               .Where(student => student.LearningOutcome.YearwiseStudentId == iStudentId)
                               .Select(subjetSection => subjetSection.subjectSection)
                               .Distinct()
                               .ToList();

        if (moXseedProgressReportBL.StudentsLearningOutcomes.Count > 0)
        {
            trErrorMessage.Visible = false;
            lstSubjectSectionConfigurationMaster.ForEach
                                        (
                                            subjectSection =>
                                            {
                                                oSubjectSection = GetCell(subjectSection.SubjectSectionName);
                                                moTblXseedLearningOutcomes.Rows.Add(oSubjectSection);
                                                FillLearningOutcomes(subjectSection.SubjectSectionConfigurationId, iStudentId);
                                            }
                                        );

            moTblXseedLearningOutcomes.Visible = lstSubjectSectionConfigurationMaster.Count > 0;
            FillNonXseedSubjectDetails(iStudentId);
        }
        else
        {
            trErrorMessage.Visible = true;
        }
    }

    private void FillStudentDetails(int aiStudentId)
    {
        YearwiseStudentMaster oYearwiseStudentMaster = moXseedProgressReportBL.YearwiseStudentMaster
                               .Where(student => student.YearwiseStudentId == aiStudentId)
                               .FirstOrDefault();

        if (oYearwiseStudentMaster != null)
        {
            HtmlTableRow OHtmlTableRow;
            HtmlTableCell OHtmlTableCell;

            OHtmlTableRow = new HtmlTableRow();
            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Center";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = Resources.LocalizedResources.RollNo;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
            OHtmlTableCell.Width = Unit.Pixel(100).ToString();

            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Center";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
            OHtmlTableCell.InnerHtml = oYearwiseStudentMaster.RollNo.ToString();
            OHtmlTableCell.Width = Unit.Pixel(10).ToString();
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Center";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = Resources.LocalizedResources.Name;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Left";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = oYearwiseStudentMaster.StudentName;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
            OHtmlTableCell.Width = Unit.Pixel(270).ToString();
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Center";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = Resources.LocalizedResources.Class;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
            OHtmlTableCell.Width = Unit.Pixel(50).ToString();
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Left";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = oYearwiseStudentMaster.Class;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
            OHtmlTableCell.Width = Unit.Pixel(100).ToString();
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Center";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = Resources.LocalizedResources.Year;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight");
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            OHtmlTableCell = new HtmlTableCell();
            OHtmlTableCell.Align = "Left";
            OHtmlTableCell.Height = "17px";
            OHtmlTableCell.InnerHtml = oYearwiseStudentMaster.AcademicYear;
            OHtmlTableCell.Attributes.Add("Class", "ClsLabel ClsBGWhite ClsBorderlight ClsHilightTextB");
            OHtmlTableRow.Cells.Add(OHtmlTableCell);

            moTblStudentdetails.Rows.Add(OHtmlTableRow);
        }
    }

    private HtmlTableRow GetSchoolDetailsCell(string asClass, string asName)
    {
        HtmlTableRow OHtmlTableRow = new HtmlTableRow();
        HtmlTableCell OHtmlTableCell = new HtmlTableCell();
        OHtmlTableCell.ColSpan = 8;
        OHtmlTableCell.Align = "Center";
        OHtmlTableCell.InnerHtml = asName;
        OHtmlTableRow.Cells.Add(OHtmlTableCell);
        OHtmlTableRow.Attributes.Add("class", asClass);
        moTblStudentdetails.Rows.Add(OHtmlTableRow);
        return OHtmlTableRow;
    }

    private void FillNonXseedSubjectDetails(int aiStudentId)
    {
        List<NonXseedSubjectGrades> lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && !grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(grade => moTblNonXseedProgressReport.Rows.Add(GetSubjectGradeCell(grade)));
        moTblNonXseedProgressReport.Visible = lstNonXseedSubjectGardes.Count > 0;

        lstNonXseedSubjectGardes = moXseedProgressReportBL.NonXseedSubjectGrades.Where(grade => grade.YearwiseStudentId == aiStudentId && grade.IsCoCurricularActivity).ToList();
        lstNonXseedSubjectGardes.ForEach(grade => moTblCoCurricularSubjects.Rows.Add(GetSubjectGradeCell(grade)));
        moTblCoCurricularSubjects.Visible = lstNonXseedSubjectGardes.Count > 0;
    }

    private HtmlTableRow GetSubjectGradeCell(NonXseedSubjectGrades grade)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = null;

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = grade.SubjectName;
        oHtmlTableCell.ColSpan = 2;
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Style.Add("padding-left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = grade.ShortName;
        oHtmlTableCell.Align = "Center";
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = grade.Observation;
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Style.Add("padding-left", "5px");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableRow.Attributes.Add("class", "ClsMarksCell");

        return oHtmlTableRow;
    }

    private void FillLearningOutcomes(int aiSubjectSectionConfigurationId, int aiStudentId)
    {
        string sObservation = string.Empty;
        sObservation = moXseedProgressReportBL.LearningOutcomesObservations
                                               .Where(observation => observation.SubjectSectionConfigurationId == aiSubjectSectionConfigurationId && observation.YearwiseStudentId == aiStudentId)
                                               .Select(observation => observation.Observation)
                                               .FirstOrDefault();

        List<StudentsLearningOutcome> StudentsLearningOutcomes = moXseedProgressReportBL.StudentsLearningOutcomes
                                .Where(outcome => outcome.SubjectSectionConfigId == aiSubjectSectionConfigurationId && outcome.YearwiseStudentId == aiStudentId).OrderBy(sortorder => sortorder.LearningOutcomeSortOrder)
                                .ToList();

        HtmlTableRow oHtmlTableRow = null;
        bool bIsFirstRow = false;
        HtmlTableRow oHtmlTableFirstRow = new HtmlTableRow();
        int iRowIndex = 1;
        StudentsLearningOutcomes.ForEach
            (
                outcome =>
                {
                    oHtmlTableRow = GetLearningOutcomeCell(outcome.LearningOutcome, outcome.ShortName, iRowIndex++);
                    if (!bIsFirstRow)
                    {
                        oHtmlTableFirstRow = oHtmlTableRow;
                        bIsFirstRow = true;
                    }
                    moTblXseedLearningOutcomes.Rows.Add(oHtmlTableRow);
                }
            );

        HtmlTableCell oHtmlTableCell = CreateObservationCell(sObservation, StudentsLearningOutcomes.Count);
        oHtmlTableCell.RowSpan = StudentsLearningOutcomes.Count;
        oHtmlTableFirstRow.Cells.Add(oHtmlTableCell);
    }

    private HtmlTableCell CreateObservationCell(string asObservation, int aiRowSpan)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();        
        oHtmlTableCell.InnerHtml = asObservation;
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Style.Add("padding-left", "5px");
        oHtmlTableCell.RowSpan = aiRowSpan;
        return oHtmlTableCell;
    }

    private HtmlTableRow GetCell(string asSubjectSection)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.ColSpan = 4;
        oHtmlTableCell.InnerHtml = asSubjectSection;
        oHtmlTableCell.Align = "center";
        oHtmlTableCell.Attributes.Add("class", "ProgressReportHeader");
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        return oHtmlTableRow;
    }

    private HtmlTableRow GetLearningOutcomeCell(string asLearningOutcome, string asGradeName, int aiRowIndex)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = null;

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Align = "Center";
        oHtmlTableCell.InnerHtml = aiRowIndex.ToString();
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = asLearningOutcome;
        oHtmlTableCell.Align = "left";
        oHtmlTableCell.Style.Add("padding-left", "5px");        
        oHtmlTableRow.Cells.Add(oHtmlTableCell);

        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = asGradeName;
        oHtmlTableCell.Align = "Center";        
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableRow.Attributes.Add("class", "ClsMarksCell");

        return oHtmlTableRow;
    }

    #endregion
}