/*
* This Class is used to show student Result report 
 * rendered HTMLTable to show this Result.
 * Author: Shankar Gurav.
 * Date of creation: 5 March 2008
 * Date of modification: 5 March 2008

 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ. 
 */
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using ProgressReportEntities;
using Utility;

/// <summary>
/// Summary description for StudentResult
/// </summary>
public class StudentResult : StudentProgress
{

    #region Constant

    // Database tables indexces constants
    protected int S_DB_TABLE_GRACENOTE_INDEX = 5;

    #endregion Constant

    #region Data Member

    Panel GridViewScrollContainer;
    bool mbShowMarks = false;
    #endregion

    #region Custructor

    public StudentResult()
    {
        // TODO: Add constructor logic here
    }

    public StudentResult(Panel oPanel)
    {
        GridViewScrollContainer = oPanel;
        SetpanelMember(GridViewScrollContainer);
        menumResultType = enumResultType.Annual;
    }

    public StudentResult(Panel oPanel, bool bShowMarks)
    {
        GridViewScrollContainer = oPanel;
        SetpanelMember(GridViewScrollContainer);
        menumResultType = enumResultType.Annual;
        mbShowMarks = bShowMarks;
    }

    #endregion Custructor

    #region Public method

    public void SetRenderMode(Constants.PageMode aoenumPageMode)
    {
        menmPagemode = aoenumPageMode;
        switch (aoenumPageMode)
        {
            case Constants.PageMode.Print: S_CSS_PRINT_PREFIX = "P";
                break;
        }
    }

    /// <summary>
    /// This method is used to show student's progress sheets depending upon login role.
    /// </summary>
    public override Int32 ShowProgressSheet(int aiTeacherId, int aiStudentId)
    {
        if (aiStudentId != 0)
        {
            FillProgressReport(aiStudentId);
            return 1;
        }
        else
        {
            DataTable oDtStudents = GetStudentDatset(aiTeacherId, true);
            GenaratePrograssSheets(oDtStudents);
            return oDtStudents.Rows.Count;
        }
    }

    #endregion Public method

    #region Protected method

    /// <summary>
    /// This function is used to setclass member panel
    /// </summary>
    /// <param name="aoPanel"></param>
    protected override void SetpanelMember(Panel aoPanel)
    {
        base.SetpanelMember(aoPanel);
        S_CSS_PRINT_PREFIX = "AnR";
        GridViewScrollContainer = aoPanel;

        S_DB_TABLE_SUBJECT_LIST_INDEX = 1;
        S_DB_TABLE_TESTS_LIST_INDEX = 3;
        S_DB_TABLE_MARKS_LIST_INDEX = 1;
        S_DB_TABLE_TEST_TOTAL_INDEX = 2;
        S_DB_TABLE_GROUP_TOTAL_INDEX = 3;
        base.S_DB_TABLE_GRADE_INDEX = 4;
        base.S_DB_TABLE_SUBJECT_TEST_TYPE_INDEX = 1;
        base.miTotalCellColSpan = 2;
    }

    /// <summary>
    /// This Function is used to  genearate progress sheets for a all students of a class for a selected class teacher.
    /// </summary>
    protected override void GenaratePrograssSheets(DataTable oDTStudents)
    {
        foreach (DataRow oDRStudent in oDTStudents.Rows)
        {
            try
            {
                int iStudentId = oDRStudent["Student_Id"].ToInt();
                FillProgressReport(iStudentId);
                CreateSaparatorBlankTable();
            }
            catch (Exception ex)
            {
                ErrorCreateProgressReport(ex.Message);
                CreateSaparatorBlankTable();
            }
        }
    }
    
    /// <summary>
    /// This method is used to create table header.
    /// </summary>    
    protected override void CreateTableHeaderRow()
    {
        HtmlTableRow oHtmlTableRow;
        HtmlTableCell oHtmlTableCell;
        // Create first header row.
        oHtmlTableRow = CreateHeaderRowForSubjects();
        // Add top left cell with image(Exam\Subject)
        oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + "ClsMarksGridHeader");

        Label oLabel = new Label();
        oHtmlTableCell.Width = "12%";
        oLabel.Text = "Subjects";
        oLabel.CssClass = S_CSS_PRINT_PREFIX + " lblAnnualTtl";
        oHtmlTableCell.Controls.Add(oLabel);

        oHtmlTableCell.Attributes.Add("rowspan", "2");
        oHtmlTableRow.Cells.Insert(0, oHtmlTableCell);
        oHtmlTableCell.VAlign = "bottom";
        oHtmlTableCell.Align = "left";
        tblProgress.Rows.Add(oHtmlTableRow);
        // Creant and add another row with given child subject collection to a table header.
        oHtmlTableRow = CreateGroupSubjectsRow(moGroupSubjectList);
        tblProgress.Rows.Add(oHtmlTableRow);
        CreateSubjectExamTypeHeader();
    }

    /// <summary>
    /// This method is used to set Student Progress dataSet.
    /// </summary>
    /// <param name="aiStudentId"></param>
    protected override void SetStudentProgressDataSet(int aiStudentId)
    {
        int iAcademicYrID;
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] == null || Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString() == "0")
            iAcademicYrID = miAcademicYearId;
        else
            iAcademicYrID = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
        moStudentProgressReport = oStudentSubjectMarksBL.GetStudentResult(miSchoolId, iAcademicYrID, aiStudentId);
        if (!moStudentProgressReport.StudentDetails.StudentName.IsNullOrEmpty() && !moStudentProgressReport.StudentDetails.IsFailCriteriaNotApplicable.IsNullOrEmpty())
            mbIsFailCriteriaNotApplicable = moStudentProgressReport.StudentDetails.IsFailCriteriaNotApplicable == Constants.S_NO;
        else
            mbIsFailCriteriaNotApplicable = true;
    }

    /// <summary>
    /// This method is used to set marks to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamMarks(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oSubjectDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(subject => subject.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (oSubjectDetails.Count > 0 && !oSubjectDetails[0].SubjectName.IsNullOrEmpty())
        {
            HtmlTableCell oHtmlTableCell;
            if (!bShowOnlyGradesInProgressSheet || mbShowMarks)
            {
                // If subject has grade then dont append total marks(i.e 12/100)                     
                oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
                oHtmlTableCell.Align = "center";
                oHtmlTableCell.Attributes.Remove("class");
                oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
                if (menmPagemode == Constants.PageMode.Print)
                    oHtmlTableCell.InnerHtml = "<B>" + oSubjectDetails[0].MarksScored.ToString("0.#") + "</B>" + "/" + oSubjectDetails[0].SubjectTotalMarks;
                else
                {
                    if (moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && oSubjectDetails[0].SubjectId == sd.SubjectId))
                        oHtmlTableCell.InnerHtml = "-";
                    else
                        oHtmlTableCell.InnerHtml = "<B>" + oSubjectDetails[0].MarksScored.ToString("0.#") + "</B>" + " / " + oSubjectDetails[0].SubjectTotalMarks;
                }
                oHtmlTableCell = tblProgress.Rows[aiRowIndex + 1].Cells[oHTSubjectEntry.Key.ToInt()];
            }
            else
                oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
            oHtmlTableCell.Align = "center";
            oHtmlTableCell.Attributes.Remove("class");
            oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
            oHtmlTableCell.InnerHtml = oSubjectDetails[0].Grade;

            if (moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && oSubjectDetails[0].SubjectId == sd.SubjectId))
                oHtmlTableCell.InnerHtml = "-";

        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set grade to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGrade(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oSubjectDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(subject => subject.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
        if (oSubjectDetails.Count > 0)
        {
            oHtmlTableCell.Align = "center";
            oHtmlTableCell.Attributes.Remove("class");
            oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);

            // If subject has grade then dont append total marks(i.e 12/100)                                 
            oHtmlTableCell.InnerHtml = "<B>" + oSubjectDetails[0].Grade + "</B>";

        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGroupTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>()
                                                           .Where(subject => subject.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId)
                                                           .OrderByDescending(subject => subject.Id).ToList();
        if (oParentSubjectDetails.Count > 0 && !oParentSubjectDetails[0].SubjectName.IsNullOrEmpty())
        {
            // Take a group total of a subject.
            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>().Where(subject => subject.SubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).ToList();

            if (oParentSubjectTotalDetails.Count > 0)
            {
                HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
                oHtmlTableCell.Align = "center";
                oHtmlTableCell.Attributes.Remove("class");
                oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL);
                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].MarksScored + "</B>" + " / " + oParentSubjectTotalDetails[0].SubjectTotalMarks;
                oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
                oHtmlTableCell = tblProgress.Rows[aiRowIndex + 1].Cells[oHTSubjectEntry.Key.ToInt()];
                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].Grade;
                oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
            }
            else
                FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
        }
        else
            FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
    }

    /// <summary>
    /// This method is used to get test type count for a given subject
    /// </summary>
    /// <param name="aiSubjectId"></param>
    /// <returns></returns>
    protected override int GetExamTypeCount(int aiSubjectId)
    {
        return 1;
    }

    /// <summary>
    /// This method is used to create required Html rows For tests and add it to progress table
    /// </summary>    
    protected override void CreateExamsAndTotalBlankRows()
    {
        HtmlTableRow oHtmlTableRow;
        HtmlTableCell oHtmlTableCell;
        bool bAltRow = true;
        // Create row for that test containing required subject's cells
        String[] sRowHeader = GetRowHeaders();
        for (int i = 0; i < sRowHeader.Length; i++)
        {
            oHtmlTableRow = CreateBlankRow();
            // Create the row header cell with the test name and css class for alternet rows
            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.InnerText = sRowHeader[i];
            if (bAltRow)
                oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDROW);
            else
                oHtmlTableCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDALTROW);
            bAltRow = !bAltRow;
            if (menmPagemode == Constants.PageMode.Print)
            {
                oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
                oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
                oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderColor, "black");
                oHtmlTableCell.NoWrap = false;
            }
            oHtmlTableRow.Cells.Insert(0, oHtmlTableCell);
            // Add this row to the table.
            tblProgress.Rows.Add(oHtmlTableRow);
        }

        ShowGraceNote();
    }

    /// <summary>
    /// This method is used to get the row header.
    /// </summary>
    /// <returns></returns>
    protected virtual string[] GetRowHeaders()
    {
        if (bShowOnlyGradesInProgressSheet && !mbShowMarks)
        {
            String[] sGradeRowHeader = new String[1];
            sGradeRowHeader[0] = "Subject Grade";
            return sGradeRowHeader;
        }

        String[] sRowHeader = new String[2];
        sRowHeader[0] = "Marks";
        sRowHeader[1] = "Subject Grade";
        return sRowHeader;
    }

    /// <summary>
    /// This method is used to create 
    /// </summary>
    protected override HtmlTableRow CreateSubjectExamTypeHeader()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        moHTSubject.Clear();
        int iCurrentIndex = 1;
        // itarate through subjects collection
        foreach (FinalResultSubjectDetails oCurrentSubject in moStudentProgressReport.SubjectDetails.Cast<FinalResultSubjectDetails>())
        {
            if (oCurrentSubject.ParentSubjectId != -1)
            {
                // If subject have exam types then render its exam type
                if (oCurrentSubject.GradeOrMarks == "M")
                {
                    SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, oCurrentSubject.SubjectName, oCurrentSubject.SubjectId, Constants.I_ONE, string.Empty, oCurrentSubject.ParentSubjectId, Constants.ReportCellType.ExamType);
                    moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                    miTotalCellCount++;
                    iCurrentIndex++;
                }
                else // If subject do not have exam types that means it have grade
                {
                    SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, oCurrentSubject.SubjectName, oCurrentSubject.SubjectId, Constants.I_ONE, string.Empty, Constants.I_ZERO, Constants.ReportCellType.Grade);
                    oGroupSubjectdetails.SubjectCellRowSpan = 1;
                    moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                    miTotalCellCount++;
                    iCurrentIndex++;
                }

                // check if current subject is a member of subject group.
                if (oCurrentSubject.ParentSubjectId.ToInt() != 0)
                {
                    // Take the first subject from this group from all subjects of that group. 
                    // And check that is the current loop subject is the last subject of that group
                    var oSubjectsInCurrentGroupDetails = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == oCurrentSubject.ParentSubjectId).OrderBy(subject => subject.Id).ToList<Subject>();
                    if ((oSubjectsInCurrentGroupDetails.Count > 0
                        && !oSubjectsInCurrentGroupDetails[oSubjectsInCurrentGroupDetails.Count - 1].SubjectName.IsNullOrEmpty())
                        && oSubjectsInCurrentGroupDetails[oSubjectsInCurrentGroupDetails.Count - 1].SubjectId == oCurrentSubject.SubjectId)
                    {
                        // then put a subject structure into hashtable for a group total
                        SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, oCurrentSubject.SubjectName, oCurrentSubject.SubjectId, Constants.I_ONE, string.Empty, oCurrentSubject.ParentSubjectId, Constants.ReportCellType.GroupTotal);
                        moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                        iCurrentIndex++;
                    }
                }
            }
        }

        return oHtmlTableRow;
    }

    /// <summary>
    /// This method is used to fill tests result to a progress table
    /// </summary>    
    protected override void FillExamsMarks()
    {
        HtmlTableRow oHtmlTableRow;
        // Skip col headers
        int iRowIndex = 2;

        oHtmlTableRow = tblProgress.Rows[iRowIndex];

        // Skip row header
        foreach (DictionaryEntry oHTSubjectEntry in moHTSubject)
        {
            SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
            switch (oSubjectDetailsForProgressReport.SubjectCellType)
            {
                case Constants.ReportCellType.ExamType:
                    FillSubjectExamMarks(1, oHTSubjectEntry, iRowIndex);
                    break;

                case Constants.ReportCellType.Grade:
                    FillSubjectExamGrade(1, oHTSubjectEntry, iRowIndex);
                    break;

                case Constants.ReportCellType.ExamTypeTotal:
                    FillSubjectExamTypeTotal(1, oHTSubjectEntry, iRowIndex);
                    break;

                case Constants.ReportCellType.GroupTotal:
                    FillSubjectExamGroupTotal(1, oHTSubjectEntry, iRowIndex);
                    break;

                default:
                    SetNotApplicableCellValues(oHtmlTableRow, oHTSubjectEntry.Key.ToInt(), null);
                    break;
            }
        }

        // Fill the totals summary for that test row.
        FillExamTotals(oHtmlTableRow, iRowIndex, 1);
        if (!bShowOnlyGradesInProgressSheet)
        {
            iRowIndex++;
            oHtmlTableRow = tblProgress.Rows[iRowIndex];
            SetGraceMark(oHtmlTableRow);
        }
    }

    /// <summary>
    /// Fill the totals summary for that test row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiTestId"></param>
    protected override void FillExamTotals(HtmlTableRow aoHtmlTableRow, int aiRowIndex, int aiTestId)
    {
        FillExamTotalDetails(GetExamTotal(aiTestId), aoHtmlTableRow, aiRowIndex, aiTestId);
    }

    /// <summary>
    /// This method is used to get exam totals
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    protected override ExamWisePercentage GetExamTotal(int aiRowIndex)
    {
        return moStudentProgressReport.ExamWisePercentageDetails.FirstOrDefault();
    }

    /// <summary>
    /// This method is overided to add Final result header.
    /// </summary>
    protected override void CreateStudentInfo()
    {
        HtmlTable HeaderHtmlTable = CreateHdTable();
        CreateHdSchoolName(HeaderHtmlTable);
        CreateHdAnnualResult(HeaderHtmlTable);
        CreateHdStudentName(HeaderHtmlTable);
        bool bShowConsideredLegd = IsNotConsideredSubContains();
        if (bShowConsideredLegd)
            CreateHdNotApplLegend(HeaderHtmlTable);
    }

    /// <summary>
    /// This methos is used to create not applicable ledgend.
    /// </summary>
    protected override void CreateHdNotApplLegend(HtmlTable aoHeaderHtmlTable)
    {
        bool bShowConsideredLegd = IsNotConsideredSubContains();
        if ((menmPagemode == Constants.PageMode.Print) || bShowConsideredLegd)
        {
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            HtmlTableCell oHtmlTableCell = new HtmlTableCell();
            oHtmlTableRow.EnableViewState = false;
            if (menmPagemode != Constants.PageMode.Print)
                oHtmlTableCell.Align = "left";
            oHtmlTableCell.Attributes.Add("class", "ClsBGWhite ");
            if (menmPagemode == Constants.PageMode.Print)
                oHtmlTableCell.NoWrap = false;
            else
                oHtmlTableCell.NoWrap = true;
            oHtmlTableCell.ColSpan = 7;
            AddStudentInfo(oHtmlTableRow, "Legend ", string.Empty);
            Label oLabel = new Label();
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
            if (bShowConsideredLegd)
            {
                if (menmPagemode == Constants.PageMode.Print)
                    oLabel.Text = "* : Subject marks not considered in total marks.";
                else
                    oLabel.Text = "<font color='red'>*</font> : Subject marks not considered in total marks.";
                oLabel.CssClass = "ClsLabel";
                oHtmlTableCell.Controls.Add(oLabel);

            }
            
            oHtmlTableRow.Dispose();
            oHtmlTableCell.Dispose();
            oLabel.Dispose();
        }
    }

    /// <summary>
    /// This method is used to show grace note.
    /// </summary>
    protected virtual void ShowGraceNote()
    {
    }

    /// <summary>
    /// This method is used to set grace mark not if student is promoted
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    protected virtual void SetGraceMark(HtmlTableRow oHtmlTableRow)
    {
        if (moStudentProgressReport.GraceMarks != Constants.I_ZERO)
        {
            oHtmlTableRow.Cells[miTotalCellCount + 3].InnerText = "Total Grace Marks - " + moStudentProgressReport.GraceMarks;
            oHtmlTableRow.Cells[miTotalCellCount + 3].ColSpan = 2;
            oHtmlTableRow.Cells[miTotalCellCount + 3].Attributes["class"] = "AnRGraceNote";
            oHtmlTableRow.Cells.RemoveAt(miTotalCellCount + 4);
        }
    }

    #endregion Protected method

    #region Private method

    private void ErrorCreateProgressReport(string sMessage)
    {
        tblProgress = new HtmlTable();
        tblProgress.EnableViewState = false;
        tblProgress.CellPadding = 0;
        tblProgress.CellSpacing = 1;
        tblProgress.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        tblProgress.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        Label oLbl = new Label();
        oLbl.Text = sMessage;
        oLbl.CssClass = "LblNoRecord";
        oHtmlTableCell.Controls.Add(oLbl);
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        tblProgress.Rows.Add(oHtmlTableRow);
        Panel oPanel = new Panel();
        oPanel.Width = Unit.Pixel(842);
        oPanel.Controls.Add(tblProgress);
        GridViewScrollContainer.Controls.Add(oPanel);
        oPanel.Dispose();
    }

    /// <summary>
    /// This methos is used to create not Schooll Name header.
    /// </summary>
    private void CreateHdAnnualResult(HtmlTable aoHeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Final Result", S_CSS_PRINT_PREFIX + "ClsReportHead", 1, 8);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
    }

    /// <summary>
    /// This method is used to show grade details table.
    /// </summary>
    private void ShowGradeInfo()
    {
        HtmlTable HeaderHtmlTable = new HtmlTable();
        HeaderHtmlTable.EnableViewState = false;
        HeaderHtmlTable.CellPadding = 3;
        HeaderHtmlTable.CellSpacing = 1;
        HeaderHtmlTable.Border = 0;
        if (menmPagemode != Constants.PageMode.Print)
            HeaderHtmlTable.Align = "left";
        HeaderHtmlTable.BgColor = "Black";
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        HeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        GridViewScrollContainer.Controls.Add(HeaderHtmlTable);

        if (moStudentProgressReport.ExamStatusDetails.Count > 0)
        {
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            CreateHtmlCell(oHtmlTableRow, "Range", "Lbl10ptB ConfigHeadBG", 1, 1);
            moStudentProgressReport.GradeDetails.Cast<FinalResultGrade>().ToList().ForEach(grade => CreateHtmlCell(oHtmlTableRow, grade.Range, "LblSmlV ClsBGWhite", 1, 1));
            HeaderHtmlTable.Rows.Add(oHtmlTableRow);

            oHtmlTableRow = new HtmlTableRow();
            CreateHtmlCell(oHtmlTableRow, "Grade", "LblSmlVB ConfigHeadBG", 1, 1);
            moStudentProgressReport.GradeDetails.ForEach(grade => CreateHtmlCell(oHtmlTableRow, grade.GradeName, "LblSmlV ClsBGWhite", 1, 1));
            HeaderHtmlTable.Rows.Add(oHtmlTableRow);

            oHtmlTableRow = new HtmlTableRow();
            CreateHtmlCell(oHtmlTableRow, "Remarks", "LblSmlVB ConfigHeadBG", 1, 1);
            moStudentProgressReport.GradeDetails.ForEach(grade => CreateHtmlCell(oHtmlTableRow, grade.Remarks, "LblSmlV ClsBGWhite", 1, 1));
            HeaderHtmlTable.Rows.Add(oHtmlTableRow);
        }
    }

    #endregion Private method
}
