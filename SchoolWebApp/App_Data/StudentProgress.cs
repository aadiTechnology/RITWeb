/*
 * This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 27 Feb 2008
 * Date of modification: 4 Sept 2009
 
 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;

public class StudentProgress : ProgressSheetBase
{
    #region Constant
    
    // Database column constants   
    protected const string S_DB_COL_TEST_NAME = "Test_Name";
    protected const string S_DB_COL_TEST_ID = "Test_Id";
    protected const string S_DB_COL_SUBJECT_ID = "Subject_Id";
    protected const string S_DB_COL_SUBJECT_NAME = "Subject_Name";
    protected const string S_DB_COL_TOTAL_CONSIDERATION = "Total_Consideration";
    protected const string S_DB_COL_PARENTSUBJECTID = "Parent_Subject_Id";
    protected const string S_DB_COL_PARENT_SUBJECT_NAME = "Parent_Subject_Name";
    protected const string S_DB_COL_SCHOOLWISE_TEST_ID = "SchoolWise_Test_Id";
    protected const string S_DB_COL_GRADE_OR_MARKS = "Grade_Or_Marks";
    protected const string S_DB_COL_TOTAL_MARKS_SCORED = "Total_Marks_Scored";
    protected const string S_DB_COL_SUBJECT_TOTAL_MARKS = "Subject_Total_Marks";
    protected const string S_DB_COL_CHILDSUBJECT_MARKS_TOTAL = "ChildSubject_Marks_Total";
    protected const string S_DB_COL_PERCENTAGE = "Percentage";
    protected const string S_DB_COL_MARKS = "Marks";
    protected const string S_DB_COL_MARKS_SCORED = "Marks_Scored";
    protected const string S_DB_COL_GRADE = "Grade";
    protected const string S_DB_COL_GRADETOTAL = "TotalGrade";
    protected const string S_DB_COL_TESTTYPE_ID = "TestType_Id";
    protected const string S_COL_TOTAL = "Total";    
    protected const string S_DB_COL_DISPLAY_NAME = "DisplayName";
    protected const string S_DB_COL_SHORT_NAME = "ShortName";
    protected const string S_DB_COL_IS_ABSENT = "Is_Absent";

    // Css classes constants
    protected const string S_CSS_CLSMARKSCELL = "ClsMarksCell";
    protected const string S_CSS_CLSMARKSGRIDHEADER = "ClsMarksGridHeader";
    protected const string S_CSS_CLSMARKSGRIDROW = "ClsMarksGridRow";
    protected const string S_CSS_CLSMARKSGRIDALTROW = "ClsMarksGridAltRow";
    protected const string S_CSS_CLSPADDING = " Clspadding";
    protected const string S_CSS_PADDINGL = " PaddingL";
    protected const string S_CSS_CLSPADDINGL = " ClspaddingL";

    #endregion Constant

    #region Data Member

    protected int miTotalCellCount;
    protected int miTotalCellColSpan = 3;
    protected Hashtable moHTSubject = new Hashtable();
    protected ArrayList moGroupSubjectList = new ArrayList();
    protected HtmlTable tblProgress;
    protected StudentProgressReport moStudentProgressReport;
    protected List<ExamStatus> mlstExamStatusDetails;
    protected string msGradeDetails;
    protected string msExamStatusDetails;
    //protected int miUserId = 0;
    protected bool mbStudentwiseProgressReport = false;
    protected bool mbFinalResult = false;
    protected bool bShowOnlyGradesInProgressSheet = false;
	protected bool mbDisplayGrade;
    protected bool bIsGradesStandard = false;
    protected enumResultType menumResultType = enumResultType.Progress;
    protected bool mbIsFailCriteriaNotApplicable;
    protected string msIsFailCriteriaNotApplicable;
    protected string S_CSS_PRINT_PREFIX = " ";

    // Database tables indexes constants
    protected int S_DB_TABLE_STUDENT_INFO_INDEX = 0;
    protected int S_DB_TABLE_SUBJECT_LIST_INDEX = 1;
    protected int S_DB_TABLE_TESTS_LIST_INDEX = 2;
    protected int S_DB_TABLE_MARKS_LIST_INDEX = 3;
    protected int S_DB_TABLE_TEST_TOTAL_INDEX = 4;
    protected int S_DB_TABLE_GROUP_TOTAL_INDEX = 5;
    protected int S_DB_TABLE_TEST_TYPE_GROUP_TOTAL_INDEX = 6;
    protected int S_DB_TABLE_SUBJECT_TEST_TYPE_INDEX = 7;
    protected int S_DB_TABLE_TEST_TYPE_INDEX = 8;
    protected int S_DB_TABLE_GRADE_INDEX = 9;
    protected int S_DB_TABLE_EXAM_STATUS = 10;

    private const string S_CSS_CLSTESTHEADER = "ClsTestHeader";
    private const string S_CSS_CLSGPTESTTYPEHEADER = "ClsGpTestTypeHdr";
    private const string S_CSS_CLSTOTALMARKSCELL = "TotalType PaddingL";
    private const string S_NON_APPLICABLE = "-";

    private Panel grdvwScrollContainer;
    private int miPageCount = 5;
    private int miPageStartIndex = 1;
    private bool mbShowPublishButton;
    private bool bShortPrintEnabled = true;
    public bool mbIsApplicable;
    #endregion

    #region Cunstructor

    public StudentProgress()
    {
        InitializeMemberVariables();
        // TODO: Add constructor logic here
    }

    /// <summary>
    /// This overloaded costructor is defined to set panel for rendering
    /// </summary>
    /// <param name="oPanel"></param>
    public StudentProgress(Panel oPanel)
    {
        InitializeMemberVariables();
        grdvwScrollContainer = oPanel;
    }

    #endregion Cunstructor

    #region Enum

    public enum enumResultType
    {
        Progress = 0,
        Annual = 1,
        TeacherModeProgress = 2,
    }

    #endregion Enum

    #region Prperties

    /// <summary>
    /// Used to get set Result Type.
    /// </summary>
    public enumResultType ResultType
    {
        get { return menumResultType; }
        set { menumResultType = value; }
    }

    /// <summary>
    /// Used to get set Result Type.
    /// </summary>
    public string PrintPrefix
    {
        get { return S_CSS_PRINT_PREFIX; }
        set { S_CSS_PRINT_PREFIX = value; }
    }

    /// <summary>
    /// This function is used to page start index for a paging.
    /// </summary>
    protected virtual int PageStartIndex
    {
        get { return miPageStartIndex; }
        set { miPageStartIndex = value; }
    }

    protected bool ShowPublishButton
    {
        get { return mbShowPublishButton; }
        set { mbShowPublishButton = value; }
    }

    /// <summary>
    /// This function is used to page count for a paging.
    /// </summary>
    protected virtual int PageCount
    {
        get { return miPageCount; }
        set { miPageCount = value; }
    }

    #endregion Prperties

    #region Public Method

    /// <summary>
    /// This method is used to show student's progress sheets depending upon login role.
    /// </summary>
    public override int ShowProgressSheet(int aiTeacherId, int aiStudentId)
    {
        if (aiStudentId != 0)
        {
            FillProgressReport(aiStudentId);
            return 1;
        }
        else
        {
            DataSet odsStudents = GetAllStudentsProgressSheet(aiTeacherId);
            FillExamStatusList(odsStudents.Tables[1]);
            GenaratePrograssSheets(odsStudents.Tables[0]);
            odsStudents.Dispose();
            return odsStudents.Tables[0].Rows.Count;
        }
    }

    /// <summary>
    /// This methoid is used to create and fill progress sheet
    /// </summary>
    public override void ShowProgressSheet(int aiStudentId)
    {
        FillProgressReport(aiStudentId);
    }

    /// <summary>
    /// This methoid is used to create and fill progress sheet
    /// </summary>
    public void FillProgressReport(int aiStudentId)
    {
        SetStudentProgressDataSet(aiStudentId);
        CreateProgressReport();
        FillExamsMarks();
        ResetControls();
    }

    /// <summary>
    /// This methoid is used to create and fill progress sheet
    /// </summary>
    public void FillProgressReport(DataRow oDataRow)
    {
        CreateStudentProgressDataSet(oDataRow);
        CreateProgressReport();
        FillExamsMarks();
        ResetControls();
    }

    /// <summary>
    /// This method is used to show progress sheet note.
    /// </summary>
    public void ShowProgressSheetNote()
    {
        int iAcademicYrId;
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] == null || Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString() == "0")
            iAcademicYrId = miAcademicYearId;
        else
            iAcademicYrId = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();
        string sShowProgressSheetNote = AllSettings[iAcademicYrId].ShowProgressSheetNote ? Constants.S_YES : Constants.S_NO;
        if (Convert.ToChar(sShowProgressSheetNote) == Constants.C_YES)
        {
            string sProgressSheetNote = AllSettings[iAcademicYrId].ProgressSheetNote;
            HtmlTable oHeaderHtmlTable = new HtmlTable();
            oHeaderHtmlTable.EnableViewState = false;
            oHeaderHtmlTable.Width = "100%";
            oHeaderHtmlTable.Border = 0;
            oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
            oHeaderHtmlTable.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
            HtmlTableRow oHtmlTableRow = new HtmlTableRow();
            HtmlTableCell oHtmlTableCell = new HtmlTableCell();
            Label oLabel = new Label();
            oLabel.Text = sProgressSheetNote;
            oLabel.CssClass = "LblSmlGray";
            oHtmlTableCell.Controls.Add(oLabel);
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            oHeaderHtmlTable.Rows.Add(oHtmlTableRow);
            oHtmlTableRow = new HtmlTableRow();
            oHtmlTableCell = new HtmlTableCell();
            oHtmlTableCell.InnerHtml = "&nbsp;";
            oHtmlTableRow.Cells.Add(oHtmlTableCell);
            oHeaderHtmlTable.Rows.Add(oHtmlTableRow);
            LiteralControl oLiteralControl = new LiteralControl("<br />");
            grdvwScrollContainer.Controls.Add(oLiteralControl);
            grdvwScrollContainer.Controls.Add(oHeaderHtmlTable);
        }
    }

    /// <summary>
    /// This function is used to get student dataset  for a given teacher ID
    /// </summary>
    /// <returns></returns>
    public DataTable GetStudentDatset(int aiTeacherId, bool bConsiderLeftStudent)
    {
        StudentCollectionBL oStudentCollectionBL = new StudentCollectionBL(miSchoolId, miAcademicYearId, bConsiderLeftStudent);
        DataTable oDSStudents = oStudentCollectionBL.GetStudentListOfGivenClassTeacher(aiTeacherId);
        return oDSStudents;
    }

    /// <summary>
    /// This method is used to fill exam total details.
    /// </summary>
    /// <param name="aoExamWisePercentageDetails"></param>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiTestId"></param>
    public void FillExamTotalDetails(ExamWisePercentage aoExamWisePercentageDetails, HtmlTableRow aoHtmlTableRow, int aiRowIndex, int aiTestId)
    {
        int iCellIndex = moHTSubject.Count + 1;
        HtmlTableCell oHtmlTableCell = null;

        DependentExam oDependentExam = new DependentExam();
        if (mbStudentwiseProgressReport && moStudentProgressReport.DependentExamDetails.Count > 0)
            oDependentExam = moStudentProgressReport.DependentExamDetails.FirstOrDefault(ded => ded.ParentExamId == aiTestId);

        bool bShowResult = aoExamWisePercentageDetails.TotalMarksScored >= 0;
        if (!aoExamWisePercentageDetails.Grade.IsNullOrEmpty())
        {
            if (!bShowOnlyGradesInProgressSheet)
            {
                if (aoExamWisePercentageDetails.TotalMarksScored >= Constants.I_ZERO)
                {
                    oHtmlTableCell = aoHtmlTableRow.Cells[iCellIndex];
                    if (menmPagemode != Constants.PageMode.Print)
                    {
                        oHtmlTableCell.Align = HorizontalAlign.Left.ToString();
                        if (!mbStudentwiseProgressReport)
                        {
                            if (bShowResult)
                            {
                                if (!moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && !sd.IsThirdLanguage))
                                    oHtmlTableCell.InnerHtml = "<B>" + aoExamWisePercentageDetails.TotalMarksScored.ToDecimal().ToString("0.#") + "</B>" + " / " + aoExamWisePercentageDetails.SubjectTotalMarks;
                                else
                                    oHtmlTableCell.InnerHtml = "<B> - </B>";
                            }
                            else
                                oHtmlTableCell.InnerHtml = "<B> - </B>";
                        }
                        else
                        {
                            Label olblTotal = new Label();
                            Label olblTotalMarks = new Label();
                            Label olbl = new Label();
                            olblTotal.ID = "lblMarks_" + aiRowIndex;
                            olblTotalMarks.ID = "lblTotalMarks_" + aiRowIndex;
                            olblTotal.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                            olblTotalMarks.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                            olbl.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                            if (bShowResult)
                            {
                                olblTotal.Text = aoExamWisePercentageDetails.TotalMarksScored.ToString("0.#") + " / ";
                                olblTotalMarks.Text = aoExamWisePercentageDetails.SubjectTotalMarks.ToString();
                            }
                            else
                            {
                                olblTotal.Text = " - ";
                                olblTotalMarks.Style.Add(HtmlTextWriterStyle.TextAlign, "Center");
                                oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
                            }
                            oHtmlTableCell.InnerHtml = string.Empty;
                            oHtmlTableCell.InnerText = string.Empty;
                            oHtmlTableCell.Controls.Add(olblTotal);
                            oHtmlTableCell.Controls.Add(olbl);
                            oHtmlTableCell.Controls.Add(olblTotalMarks);
                        }
                    }
                    else if (bShowResult)
                        oHtmlTableCell.InnerHtml = "<B>" + aoExamWisePercentageDetails.TotalMarksScored.ToString("0.#") + "</B>" + "/" + aoExamWisePercentageDetails.SubjectTotalMarks;
                    else
                    {
                        oHtmlTableCell.InnerHtml = "<B> - </B>";
                        oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
                    }
                    
                    if (!IsTotalConsiderForProgressReport())
                        oHtmlTableCell.Attributes.Add("style", "display:none");

                    CheckDSKStatus(oHtmlTableCell);
                }
                else if (menmPagemode == Constants.PageMode.Print)
                      ShowTotalOnPrintPreview(aoHtmlTableRow, iCellIndex);
          
                
                iCellIndex++;
                if (aoExamWisePercentageDetails.Percentage >= Constants.I_ZERO)
                {
                    oHtmlTableCell = aoHtmlTableRow.Cells[iCellIndex];
                    if (menmPagemode != Constants.PageMode.Print)
                        oHtmlTableCell.Align = HorizontalAlign.Left.ToString();
                    if (!mbStudentwiseProgressReport)
                    {
                        if (bShowResult)
                        {
                            if (!moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && !sd.IsThirdLanguage))
                                oHtmlTableCell.InnerHtml = "<B>" + aoExamWisePercentageDetails.Percentage + "% </B>";
                            else
                            {
                                oHtmlTableCell.InnerHtml = "<B> - </B>";
                                oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
                            }
                        }
                        else
                            oHtmlTableCell.InnerHtml = "<B> - </B>";
                    }
                    else
                    {
                        Label olblPercentage = new Label();
                        olblPercentage.ID = "lblPercentage_" + aiRowIndex;
                        olblPercentage.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                        if (bShowResult)
                            olblPercentage.Text = aoExamWisePercentageDetails.Percentage + "%";
                        else
                        {
                            olblPercentage.Text = " - ";
                            olblPercentage.Style.Add(HtmlTextWriterStyle.TextAlign, "Center");
                            oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
                        }

                        oHtmlTableCell.InnerHtml = string.Empty;
                        oHtmlTableCell.InnerText = string.Empty;
                        oHtmlTableCell.Controls.Add(olblPercentage);
                    }
                    if (!IsTotalConsiderForProgressReport())
                        oHtmlTableCell.Attributes.Add("style", "display:none");
                    
                    CheckDSKStatus(oHtmlTableCell);
                }
                else if (menmPagemode == Constants.PageMode.Print)
                    ShowTotalOnPrintPreview(aoHtmlTableRow, iCellIndex);

                iCellIndex++;
            }

            if (!aoExamWisePercentageDetails.Grade.IsNullOrEmpty())
            {
                oHtmlTableCell = aoHtmlTableRow.Cells[iCellIndex];

                Label olblGrade = new Label();
                Label olblGradeRemarks = new Label();
                olblGrade.ID = "lblGrade_" + aiRowIndex;
                olblGradeRemarks.ID = "lblGradeRemarks_" + aiRowIndex;
                olblGrade.Style.Add(HtmlTextWriterStyle.FontWeight, "Bold");
                olblGradeRemarks.Style.Add(HtmlTextWriterStyle.Color, "#000000");
                olblGradeRemarks.Style.Add(HtmlTextWriterStyle.FontSize, "10px");
                olblGradeRemarks.Style.Add(HtmlTextWriterStyle.FontWeight, "normal");
                olblGradeRemarks.Style.Add(HtmlTextWriterStyle.FontFamily, "Verdana");


                if (bShowResult)
                {
                    var oRemarksDetails = moStudentProgressReport.GradeDetails.Where(grd => grd.GradeId == aoExamWisePercentageDetails.GradeId).ToList<Grade>();
                    if (!mbStudentwiseProgressReport)
                    {
                        if (menmPagemode != Constants.PageMode.Print)
                            oHtmlTableCell.Align = HorizontalAlign.Left.ToString();

                        if (!moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && !sd.IsThirdLanguage))
                        {
                            if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
                                oHtmlTableCell.InnerHtml = "<B>" + "-" + "</B>";
                            else
                                oHtmlTableCell.InnerHtml = "<B>" + aoExamWisePercentageDetails.Grade + "</B> <font color='#000000' size='1' face='Verdana' style='font-weight:normal'>[" + oRemarksDetails[0].Remarks + "]</font>";
                        }
                        else
                        {
                            oHtmlTableCell.InnerHtml = "<B>" + "-" + "</B>";
                            if (moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && !sd.IsThirdLanguage))
                                oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
                        }
                    }
                    else
                    {
                        olblGrade.Text = aoExamWisePercentageDetails.Grade;
                        olblGradeRemarks.Text = " [" + oRemarksDetails[0].Remarks + "]";
                    }
                }
                else
                {
                    oHtmlTableCell.InnerHtml = "<B> - </B>";
                    oHtmlTableCell.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                }

                if (mbStudentwiseProgressReport)
                {
                    oHtmlTableCell.InnerHtml = string.Empty;
                    oHtmlTableCell.InnerText = string.Empty;
                    if (!bShowResult)
                        olblGrade.Text = "-";
                    oHtmlTableCell.Controls.Add(olblGrade);
                    oHtmlTableCell.Controls.Add(olblGradeRemarks);
                }
                if (!IsTotalConsiderForProgressReport())
                    oHtmlTableCell.Attributes.Add("style", "display:none");
                CheckDSKStatus(oHtmlTableCell);
            }

            if (mbIsFailCriteriaNotApplicable)
            {
                iCellIndex++;
                if (!aoExamWisePercentageDetails.Result.IsNullOrEmpty())
                {
                    oHtmlTableCell = aoHtmlTableRow.Cells[iCellIndex];
                    if (menmPagemode != Constants.PageMode.Print)
                    {
                        if (bShowResult)
                        {
                            if (aoExamWisePercentageDetails.Result.Trim().ToUpper().Equals("FAIL"))
                                oHtmlTableCell.InnerHtml = string.Format("<b><font color='red'>{0}</font></b>", aoExamWisePercentageDetails.Result);
                            else if (aoExamWisePercentageDetails.Result.Trim().ToUpper().Equals("PASS"))
                                oHtmlTableCell.InnerHtml = string.Format("<b><font color='green'>{0}</font></b>", aoExamWisePercentageDetails.Result);
                            else
                                oHtmlTableCell.InnerHtml = string.Format("<b><font color='Orange'>{0}</font></b>", aoExamWisePercentageDetails.Result);
                        }
                        else
                            oHtmlTableCell.InnerHtml = "<B> - </B>";
                    }
                    else if (bShowResult)
                    {
                        oHtmlTableCell.InnerHtml = string.Format("<b>{0}</b>", aoExamWisePercentageDetails.Result);
                    }
                    else
                        oHtmlTableCell.InnerHtml = "<B> - </B>";
                    if (!IsTotalConsiderForProgressReport())
                        oHtmlTableCell.Attributes.Add("style", "display:none");
                    //CheckDSKStatus(oHtmlTableCell);
                }

            }

            if (!bShowOnlyGradesInProgressSheet)
                if (mbIsApplicable)
                {
                    iCellIndex++;
                    if (ShouldShowRankColumn()
                        && aoExamWisePercentageDetails.Rank != Constants.I_ZERO
                        && aoExamWisePercentageDetails.Rank > 0
                        && aoExamWisePercentageDetails.Rank <= Settings.ToppersCount)
                    {
                        oHtmlTableCell = aoHtmlTableRow.Cells[iCellIndex];
                        if (bShowResult)
                            if (menmPagemode != Constants.PageMode.Print)
                            {
                                if (!moStudentProgressReport.SubjectDetails.Any(sd => sd.IsAbsent && !sd.IsThirdLanguage))
                                    oHtmlTableCell.InnerHtml = string.Format("<b><font color='green'>{0}</font></b>", aoExamWisePercentageDetails.Rank);
                                else
                                    oHtmlTableCell.InnerHtml = "<B> - </B>";
                            }
                            else
                                oHtmlTableCell.InnerHtml = string.Format("<b>{0}</b>", aoExamWisePercentageDetails.Rank);
                        else
                            oHtmlTableCell.InnerHtml = "<B> - </B>";
                        if (!IsTotalConsiderForProgressReport())
                            oHtmlTableCell.Attributes.Add("style", "display:none");

                        //CheckDSKStatus(oHtmlTableCell);
                    }
                    else if (menmPagemode == Constants.PageMode.Print)
                    {
                        ShowTotalOnPrintPreview(aoHtmlTableRow, iCellIndex);
                        if (!Settings.ShowRankColumn)
                            HideRankColumnCell(aoHtmlTableRow.Cells[iCellIndex]);
                    }
                    else if (!Settings.ShowRankColumn)
                        HideRankColumnCell(aoHtmlTableRow.Cells[iCellIndex]);
                }
        }

        if (mbStudentwiseProgressReport)
        {
            StudentWiseProgressReportExamWisePercentage oStudentWiseProgressReportExamWisePercentage = ((StudentWiseProgressReportExamWisePercentage)aoExamWisePercentageDetails);
            iCellIndex++;
            CheckBox ochkPublish = new CheckBox();
            ochkPublish.ID = "chkPublish_" + aiRowIndex;
            ochkPublish.Checked = oStudentWiseProgressReportExamWisePercentage.StudentWiseTestPublishStatus == Constants.S_YES || oStudentWiseProgressReportExamWisePercentage.ExamPublishStatus == Constants.S_YES;
            ochkPublish.Enabled = oStudentWiseProgressReportExamWisePercentage.ExamPublishStatus != Constants.S_YES;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(ochkPublish);
            ochkPublish.Attributes.Add("onclick", "EnableDisableControlsOfRow(this,'" + tblProgress.ClientID + "','" + aiRowIndex + "')");
            HiddenField oHiddenField = new HiddenField();
            oHiddenField.ID = "hidTestId_" + aiRowIndex;
            oHiddenField.Value = aiTestId.ToString();
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);
            oHiddenField = new HiddenField();
            oHiddenField.ID = "hidTestName_" + aiRowIndex;
            oHiddenField.Value = oStudentWiseProgressReportExamWisePercentage.SchoolWiseTestName;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);
            oHiddenField = new HiddenField();
            oHiddenField.ID = "hidTestPublishStatus_" + aiRowIndex;
            oHiddenField.Value = (oStudentWiseProgressReportExamWisePercentage.StudentWiseTestPublishStatus == Constants.S_YES || oStudentWiseProgressReportExamWisePercentage.ExamPublishStatus == Constants.S_YES) ? Constants.S_YES : Constants.S_NO;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);
            oHiddenField = new HiddenField();
            oHiddenField.ID = "hidTestSubmitStatus_" + aiRowIndex;
            oHiddenField.Value = oStudentWiseProgressReportExamWisePercentage.ExamSubmitStatus;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);

            oHiddenField = new HiddenField();
            oHiddenField.ID = "hidDependentExamId_" + aiRowIndex;
            oHiddenField.Value = !oDependentExam.ExamName.IsNullOrEmpty() ? oDependentExam.DependentExamId.ToString() : string.Empty;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);

            oHiddenField = new HiddenField();
            oHiddenField.ID = "hidDependentExamName_" + aiRowIndex;
            oHiddenField.Value = !oDependentExam.ExamName.IsNullOrEmpty() ? oDependentExam.ExamName : string.Empty;
            aoHtmlTableRow.Cells[iCellIndex].Controls.Add(oHiddenField);
        }

        if (oHtmlTableCell != null)
            oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to create object of subject details.
    /// </summary>
    /// <param name="aiCellIndex"></param>
    /// <param name="asSubjectname"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="aiSubjectCellColSpan"></param>
    /// <param name="asTestTypeName"></param>
    /// <param name="aiParentSubjectId"></param>
    /// <returns></returns>
    public SubjectDetailsForProgressReport FillSubjectDetails(int aiCellIndex, string asSubjectname, int aiSubjectId, int aiSubjectCellColSpan, string asTestTypeName, int aiParentSubjectId, Constants.ReportCellType aenumSubjectColType)
    {
        return new SubjectDetailsForProgressReport()
        {
            CellIndex = aiCellIndex,
            Subjectname = asSubjectname,
            SubjectId = aiSubjectId,
            SubjectCellColSpan = aiSubjectCellColSpan,
            TestTypeName = asTestTypeName,
            ParentSubjectId = aiParentSubjectId,
            SubjectCellType = aenumSubjectColType,
        };
    }

    #endregion

    #region Protected method

    /// <summary>
    ///	Used to read the QueryString.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
        try
        {            
            base.OnPreInit(e);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), "Page : " + Request.AppRelativeCurrentExecutionFilePath);
        }
    }

    /// <summary>
    /// This function is used to set class member panel
    /// </summary>
    /// <param name="aoPanel"></param>
    protected virtual void SetpanelMember(Panel aoPanel)
    {
        grdvwScrollContainer = aoPanel;
    }

    /// <summary>
    /// This method is used to get all sutdents data.
    /// </summary>
    /// <param name="aiTeacherId"></param>
    /// <returns></returns>
	protected DataSet GetAllStudentsProgressSheet(int aiStdDivIdId)
    {
        DataSet oDataSet;

        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        PageCount = 100;
        if (menumResultType == enumResultType.TeacherModeProgress)
            oDataSet = oStudentSubjectMarksBL.GetAllStudentProgressResult(miSchoolId, miAcademicYearId, aiStdDivIdId, PageStartIndex, PageCount, miTestId);
        else
			oDataSet = oStudentSubjectMarksBL.GetAllStudentProgressResult(miSchoolId, miAcademicYearId, aiStdDivIdId);
        return oDataSet;
    }

    /// <summary>
    /// This method is used to set Student Progress dataSet.
    /// </summary>
    /// <param name="aiStudentId"></param>
    protected virtual void SetStudentProgressDataSet(int aiStudentId)
    {
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        int iAcademicYrID = 0;
        if (Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID] == null || Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToString() == "0")
            iAcademicYrID = miAcademicYearId;
        else
            iAcademicYrID = Session[Constants.S_SESSION_SELECTED_ACADEMIC_YEAR_ID].ToInt();

        if (!mbStudentwiseProgressReport && !mbViewStudnetwiseProgressReport)
        {
            moStudentProgressReport = oStudentSubjectMarksBL.GetStudentProgressResult(miSchoolId, iAcademicYrID, aiStudentId, miUserId);
            if (mlstExamStatusDetails == null || mlstExamStatusDetails.Count <= 0)
                mlstExamStatusDetails = moStudentProgressReport.ExamStatusDetails;
        }
        else if (!mbViewStudnetwiseProgressReport)
        {
            moStudentProgressReport = oStudentSubjectMarksBL.GetMarksDetailsForExamwiseStudentMarksAssignment(miSchoolId, iAcademicYrID, aiStudentId, miUserId);
            SetGrades();
            SetExamStatusConsiderationInTotal();

            if (!moStudentProgressReport.StudentDetails.StudentName.IsNullOrEmpty())
                miUserId = ((StudentWiseProgressReportStudentDetails)moStudentProgressReport.StudentDetails).UserId;
        }
        else
        {
            oStudentSubjectMarksBL.TestIds = msTestId;
            moStudentProgressReport = oStudentSubjectMarksBL.GetStudentTestProgressResult(miSchoolId, iAcademicYrID, aiStudentId, 0);
            mlstExamStatusDetails = moStudentProgressReport.ExamStatusDetails;
        }

        if (!moStudentProgressReport.StudentDetails.StudentName.IsNullOrEmpty() && !moStudentProgressReport.StudentDetails.IsFailCriteriaNotApplicable.IsNullOrEmpty())
        {
            mbIsFailCriteriaNotApplicable = moStudentProgressReport.StudentDetails.IsFailCriteriaNotApplicable == Constants.S_NO;
            msIsFailCriteriaNotApplicable = moStudentProgressReport.StudentDetails.IsFailCriteriaNotApplicable;
        }
        else
            mbIsFailCriteriaNotApplicable = true;
    }
        
    /// <summary>
    /// This method is used to get test type count for a given subject
    /// </summary>
    /// <param name="aiSubjectId"></param>
    /// <returns></returns>
    protected virtual int GetExamTypeCount(int aiSubjectId)
    {
        int iCellExamTypeCount;
        int iCount = moStudentProgressReport.SubjectTestTypeDetails.Count(tt => tt.SubjectId == aiSubjectId);
        if (iCount > 1)
        {
            if (IsTotalConsiderForProgressReport())
                iCellExamTypeCount = iCount + 1;
            else
                iCellExamTypeCount = iCount;
        }
        else
            iCellExamTypeCount = 1;
        if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
            iCellExamTypeCount = 1;

        return iCellExamTypeCount;
    }

    /// <summary>
    /// This Function is used to  generate progress sheets for a all students of a class for a selected class teacher.
    /// </summary>
    protected virtual void GenaratePrograssSheets(DataTable oDTStudents)
    {
        bool bresult = false;
        int iCount = 1;
        foreach (DataRow oDRStudent in oDTStudents.Rows)
        {
            // This method is used to set dataset depending upon the mode.
            // if mode is not teacher preview then set the dataset schema for the first student
            // and then fill the generate dataset from the xml.
            if (menumResultType != enumResultType.TeacherModeProgress && !bresult)
            {
                SetStudentProgressDataSet(oDRStudent[0].ToInt());
                bresult = true;
            }
            else if (menumResultType == enumResultType.TeacherModeProgress && !bresult)
            {
                // if mode is teacher preview then set the dataset schema specific to selected test only for the first student
                // and then fill the generate dataset from the xml.
                SetStudentTestProgressDataSet(oDRStudent[0].ToInt());
                bresult = true;
            }

            FillProgressReport(oDRStudent);
            if (oDTStudents.Rows.Count > 1)
                CreateSaparatorBlankTable();
            iCount++;
        }
    }

    /// <summary>
    /// This method is used to create student's Header information.
    /// </summary>    
    protected virtual void CreateStudentInfo()
    {
        HtmlTable oHeaderHtmlTable = CreateHdTable();
        CreateHdSchoolName(oHeaderHtmlTable);
        CreateHdProgressCard(oHeaderHtmlTable);
        CreateHdStudentName(oHeaderHtmlTable);
        bool bShowConsideredLegd = IsNotConsideredSubContains();
        if (bShowConsideredLegd)
            CreateHdNotApplLegend(oHeaderHtmlTable);
        oHeaderHtmlTable.Dispose();
    }

    /// <summary>
    /// This method is used to show the legends if they are appplicable.
    /// </summary>
    /// <returns></returns>
    protected bool IsNotConsideredSubContains()
    {
        return moStudentProgressReport.SubjectDetails.Count(subject => subject.TotalConsideration == Constants.S_NO) > Constants.I_ZERO;
    }

    /// <summary>
    /// This method is used to create not applicable legends.
    /// </summary>
    protected virtual void CreateHdNotApplLegend(HtmlTable aoHeaderHtmlTable)
    {
        bool bShowConsideredLegd = IsNotConsideredSubContains();
        if ((menmPagemode == Constants.PageMode.Print) || bShowConsideredLegd)
        {
            HtmlTableRow otrLegend = new HtmlTableRow();
            HtmlTableCell otdLegend = new HtmlTableCell();

            otrLegend.EnableViewState = false;
            if (menmPagemode != Constants.PageMode.Print)
                otdLegend.Align = "left";
            otdLegend.Attributes.Add("class", "ClsBGWhite ");

            if (menmPagemode == Constants.PageMode.Print)
                otdLegend.NoWrap = false;
            else
                otdLegend.NoWrap = true;

            otdLegend.ColSpan = 7;
            AddStudentInfo(otrLegend, "Legend ", string.Empty);
            Label olblLegend = new Label();
            otrLegend.Cells.Add(otdLegend);
            aoHeaderHtmlTable.Rows.Add(otrLegend);

            if (bShowConsideredLegd)
            {
                if (menmPagemode == Constants.PageMode.Print)
                    olblLegend.Text = "* :  Subject marks not considered in total marks.";
                else
                    olblLegend.Text = "<font color='red'>*</font> :  Subject marks not considered in total marks.";
                olblLegend.CssClass = "ClsLabel";
                otdLegend.Controls.Add(olblLegend);
            }

            otrLegend.Dispose();
            otdLegend.Dispose();
            olblLegend.Dispose();
        }
    }

    /// <summary>
    /// This method is used to create not Student name.
    /// </summary>
    protected void CreateHdStudentName(HtmlTable aoHeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        if (menmPagemode == Constants.PageMode.Print)
            oHtmlTableRow.Height = "35px";
        AddStudentInfo(oHtmlTableRow, "Roll No. ", moStudentProgressReport.StudentDetails.RollNo.ToString());
        AddStudentInfo(oHtmlTableRow, "Name ", moStudentProgressReport.StudentDetails.StudentName);
        AddStudentInfo(oHtmlTableRow, "Class ", moStudentProgressReport.StudentDetails.StandardDivisionDetails.StandardName + " - " + moStudentProgressReport.StudentDetails.StandardDivisionDetails.DivisionName);
        AddStudentInfo(oHtmlTableRow, "Year ", moStudentProgressReport.StudentDetails.AcademicYear);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to create not School Name header.
    /// </summary>
    protected void CreateHdSchoolName(HtmlTable aoHeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, moStudentProgressReport.StudentDetails.OrganizationName, S_CSS_PRINT_PREFIX + "SocietyName", 1, 8);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, moStudentProgressReport.StudentDetails.SchoolName, S_CSS_PRINT_PREFIX + "ActualSchoolName", 1, 8);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to create not applicable legend.
    /// </summary>
    protected HtmlTable CreateHdTable()
    {
        HtmlTable otblHeader = new HtmlTable();
        if (mbStudentwiseProgressReport)
            otblHeader.ID = "tblProgressHeader";
        otblHeader.EnableViewState = false;
        //otblHeader.Width = "842px";
        otblHeader.Width = "100%";
        otblHeader.CellPadding = 0;
        otblHeader.CellSpacing = 1;

        if (menmPagemode == Constants.PageMode.Print)
            otblHeader.Border = 1;
        else
        {
            otblHeader.Attributes.Add("class", S_CSS_PRINT_PREFIX + "ReportOuter");
            otblHeader.Border = 0;
        }

        otblHeader.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        otblHeader.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        grdvwScrollContainer.Controls.Add(otblHeader);
        otblHeader.Dispose();
        return otblHeader;
    }

    /// <summary>
    /// This method is used to student info pair to html row.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="asLblText"></param>
    /// <param name="asLblVal"></param>
    protected void AddStudentInfo(HtmlTableRow aoHtmlTableRow, string asLblText, string asLblVal)
    {
        Label oLabel = new Label();
        oLabel.Text = asLblText;
        oLabel.CssClass = "LblRht ClspaddingR";

        HtmlTableCell otdStudentInfo = new HtmlTableCell();
        otdStudentInfo.Controls.Add(oLabel);
        otdStudentInfo.Align = "right";
        otdStudentInfo.Attributes.Add("class", "ClsBGWhite");

        if (menmPagemode == Constants.PageMode.Print)
            otdStudentInfo.NoWrap = false;
        else
            otdStudentInfo.NoWrap = true;
        aoHtmlTableRow.Cells.Add(otdStudentInfo);
        if (asLblVal != string.Empty)
        {
            oLabel = new Label();
            oLabel.Text = asLblVal;
            oLabel.CssClass = S_CSS_PRINT_PREFIX + "ClsHilightTextB ClspaddingR";

            otdStudentInfo = new HtmlTableCell();
            otdStudentInfo.Controls.Add(oLabel);
            if (menmPagemode == Constants.PageMode.Print)
            {
                otdStudentInfo.Align = "center";
                otdStudentInfo.Style.Add(HtmlTextWriterStyle.FontSize, "10pt");
            }
            else
                otdStudentInfo.Align = "left";
            otdStudentInfo.Attributes.Add("class", "ClsBGWhite ");
            if (menmPagemode == Constants.PageMode.Print)
                otdStudentInfo.NoWrap = false;
            else
                otdStudentInfo.NoWrap = true;
            aoHtmlTableRow.Cells.Add(otdStudentInfo);
        }
    }

    /// <summary>
    /// This method is used to create blank table which can be placed between two progress sheets.
    /// </summary>
    protected void CreateSaparatorBlankTable()
    {
        HtmlTable otblSaparetor = new HtmlTable();
        otblSaparetor.EnableViewState = false;
        otblSaparetor.Height = "30px";
        otblSaparetor.Width = "100%";
        otblSaparetor.Border = 0;
        otblSaparetor.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        otblSaparetor.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());

        HtmlTableRow otrsaparetor = new HtmlTableRow();
        HtmlTableCell otdsaparetor = new HtmlTableCell();
        otdsaparetor.InnerHtml = "&nbsp;";
        otrsaparetor.Cells.Add(otdsaparetor);
        otblSaparetor.Rows.Add(otrsaparetor);
        otdsaparetor.Attributes.Add("class", "Dottedhr");
        otdsaparetor.Attributes.Add("page-break-after", "always");

        otrsaparetor = new HtmlTableRow();
        otdsaparetor = new HtmlTableCell();
        otdsaparetor.InnerHtml = "&nbsp;";
        otrsaparetor.Cells.Add(otdsaparetor);
        otblSaparetor.Rows.Add(otrsaparetor);

        grdvwScrollContainer.Controls.Add(new LiteralControl("<br />"));
        grdvwScrollContainer.Controls.Add(otblSaparetor);
    }

    /// <summary>
    /// This method is used to create table header.
    /// </summary>    
    protected virtual void CreateTableHeaderRow()
    {
        HtmlTableCell otdImage = new HtmlTableCell();

        // Add top left cell with image(Exam\Subject)
        otdImage.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER);
        Image oImage = new Image();
        otdImage.Width = "15%";
        if (menmPagemode == Constants.PageMode.Print)
            oImage.ImageUrl = "../images/GridHead_Sub_AnrPrint.gif";
        else
            oImage.ImageUrl = "../images/GrrdMarkHead_SubTest.gif";
        oImage.Style.Add("left", "0");
        otdImage.Controls.Add(oImage);
        if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
            otdImage.Attributes.Add("rowspan", "1");
        else
            otdImage.Attributes.Add("rowspan", "3");

        // Create first header row.
        HtmlTableRow otrSubjectHeader = CreateHeaderRowForSubjects();
        otrSubjectHeader.Cells.Insert(0, otdImage);
        otdImage.VAlign = "top";
        otdImage.Align = "left";
        tblProgress.Rows.Add(otrSubjectHeader);

        // Create and add another row with given child subject collection to a table header.
        otrSubjectHeader = CreateGroupSubjectsRow(moGroupSubjectList);
        tblProgress.Rows.Add(otrSubjectHeader);

        // Create Exam type header
        otrSubjectHeader = CreateSubjectExamTypeHeader();
        tblProgress.Rows.Add(otrSubjectHeader);

        // dispose everything 
        otrSubjectHeader.Dispose();
        otdImage.Dispose();
        oImage.Dispose();
    }

    /// <summary>
    /// This method is used to create Html Header Row with required sublects cells.
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    protected HtmlTableRow CreateHeaderRowForSubjects()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        int iCellIndex = 1;
        int iCellExamTypeCount;

        // iterate through subjects collection
        moStudentProgressReport.SubjectDetails
        .ForEach(
            subject =>
            {
                iCellExamTypeCount = 1;
                if (!subject.SubjectName.IsNullOrEmpty())
                {
                    iCellExamTypeCount = GetExamTypeCount(subject.SubjectId);

                    // Take the only leaf subjects.                

                    if (moStudentProgressReport.SubjectTestTypeGroupTotalDetails == null || (moStudentProgressReport.SubjectTestTypeGroupTotalDetails != null && moStudentProgressReport.SubjectTestTypeGroupTotalDetails.Count(sttgt => sttgt.ParentSubjectId == subject.SubjectId) <= 0))
                    {
                        // Put this subject into Hashtable with its id and cell index. we will use this collection to fill marks into rows.                        
                        moHTSubject.Add(iCellIndex, subject.SubjectId);

                        // Check that is this subject in a group
                        if (subject.ParentSubjectId.ToString() != "0")
                            iCellIndex = CreateSubjectGroupCell(oHtmlTableRow, subject, iCellExamTypeCount, iCellIndex);
                        else
                        {
                            // If this is not from any subject group then create plane html header cell.
                            int iRowSpan = 1;
                            if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
                                iRowSpan = 1;
                            else
                                iRowSpan = 2;
                            if (!subject.TotalConsideration.IsNullOrEmpty() && subject.TotalConsideration == Constants.S_NO)
                            {
                                if (menmPagemode == Constants.PageMode.Print)
                                    CreateHtmlCell(oHtmlTableRow, subject.SubjectName + "<font size='2' type='Verdana'>&nbsp;*</font>", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING, iRowSpan, iCellExamTypeCount);
                                else
                                    CreateHtmlCell(oHtmlTableRow, subject.SubjectName + "<font color='red' size='2' type='Verdana'>&nbsp;*</font>", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING, iRowSpan, iCellExamTypeCount);
                            }
                            else
                            {
                                string sInnerHTML = string.Empty;
                                sInnerHTML = subject.SubjectName;
                                CreateHtmlCell(oHtmlTableRow, sInnerHTML, S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING, iRowSpan, iCellExamTypeCount);
                            }
                        }

                        iCellIndex = iCellExamTypeCount + iCellIndex;
                    }
                }
            }
        );

        // Create the summary totals for that test.
        CreateRowTotalHeaderCells(oHtmlTableRow);

        return oHtmlTableRow;
    }

    /// <summary>
    /// This function is used to create subject group cells and add it into row.
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="oCurrentSubject"></param>
    /// <param name="iCellExamTypeCount"></param>
    /// <param name="iCellIndex"></param>
    /// <returns></returns>
    protected int CreateSubjectGroupCell(HtmlTableRow oHtmlTableRow, Subject oCurrentSubject, int iCellExamTypeCount, int iCellIndex)
    {
        
        // Take the first subject from this group from all subjects of that group. 
        // And check that is the current loop subject is the first subject of that group
        var oSubjectsInCurrentGroup = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == oCurrentSubject.ParentSubjectId).OrderBy(subject => subject.Id).ToList<Subject>();
        if ((oSubjectsInCurrentGroup.Count > 0 && oSubjectsInCurrentGroup[0].SubjectId != Constants.I_ZERO)
            && oSubjectsInCurrentGroup[0].SubjectId == oCurrentSubject.SubjectId)
        {
            // if this is the first subject of that group then take group parent subject name 
            // and create its cell with colspan equal to number of subject of that group + coloumn for total cell.
            var oParentTotal = moStudentProgressReport.SubjectTestGroupTotalDetails.Where(stgt => stgt.ParentSubjectId == oCurrentSubject.ParentSubjectId).ToList<SubjectTestGroupTotal>();

            if (oSubjectsInCurrentGroup.Count > 0)
            {
                if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
                {
                    int iExmCnt = GetTotalExamTypeCountOfAllChildSubjects(oSubjectsInCurrentGroup) + GetMaxExamTypeCountOfAllChildSubjects(oSubjectsInCurrentGroup);
                    if (IsTotalConsiderForProgressReport())
                        iExmCnt += 1;
                    CreateHtmlCell(oHtmlTableRow, oParentTotal[0].ParentSubjectName, S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING, 1, iExmCnt);
                }
                else if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
                {
                    CreateHtmlCell(oHtmlTableRow, oParentTotal[0].ParentSubjectName, S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING, 1, 1);
                    miTotalCellCount++;
                }
            }
        }

        if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
        {
            // we need this is child subject name and subject id so that we can put it on next header row bellow this parent subject.
            SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCellIndex, oCurrentSubject.SubjectName, oSubjectsInCurrentGroup[0].SubjectId, iCellExamTypeCount, string.Empty, Constants.I_ZERO, Constants.ReportCellType.Name);
            oGroupSubjectdetails.SubjectCellRowSpan = 1;
            oGroupSubjectdetails.IsConsideredInTotal = !oSubjectsInCurrentGroup[0].TotalConsideration.IsNullOrEmpty() && oSubjectsInCurrentGroup[0].TotalConsideration == Constants.S_NO;
            moGroupSubjectList.Add(oGroupSubjectdetails);

            // If this is the last subject of group then add test type totals.
            if ((oSubjectsInCurrentGroup.Count > 0
                && !oSubjectsInCurrentGroup[0].SubjectName.IsNullOrEmpty())
                && oSubjectsInCurrentGroup[oSubjectsInCurrentGroup.Count - 1].SubjectId == oCurrentSubject.SubjectId)
            {
                SubjectDetailsForProgressReport oGroupSubjectTotaldetails;
                if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
                {
                    // get unique test type for current subject group and then add them before group subject group total.
                    
                    List<int> oTestTypesList = new List<int>();
                    var oSubExamTypes = moStudentProgressReport.SubjectTestTypeGroupTotalDetails.Where(sttgt => sttgt.ParentSubjectId == oCurrentSubject.ParentSubjectId && sttgt.SchoolWiseTestId != -1)
                                                                                                .OrderBy(sttgt => sttgt.TestTypeSortOrder)
                                                                                                .ToList<SubjectTestTypeGroupTotal>();
                    oSubExamTypes.ForEach(
                        oSubExamType =>
                        {
                            // Check if it is added already and if not then add it.
                            if (!oTestTypesList.Contains(oSubExamType.TestTypeId))
                            {
                                string sShortenTestTypeName = moStudentProgressReport.TestTypeDetails.Where(tt => tt.TestTypeId == oSubExamType.TestTypeId).ToList()[0].ShortenTestTypeName;
                                oTestTypesList.Add(oSubExamType.TestTypeId);
                                iCellIndex++;
                                oGroupSubjectTotaldetails = FillSubjectDetails(iCellIndex,
                                                            S_COL_TOTAL + " " + sShortenTestTypeName,
                                                            oSubjectsInCurrentGroup[0].ParentSubjectId,
                                                            Constants.I_ZERO,
                                                            string.Empty,
                                                            Constants.I_ZERO,
                                                            Constants.ReportCellType.ExamTypeGroupTotal);
                                oGroupSubjectTotaldetails.SubjectCellRowSpan = miTotalCellColSpan - 1;
                                oGroupSubjectTotaldetails.IsConsideredInTotal = false;
                                moGroupSubjectList.Add(oGroupSubjectTotaldetails);
                                miTotalCellCount++;
                            }
                        }
                    );
                }

                // if this is the last subject put total column for that group into child subjects collection                                
                    string sHeader = S_COL_TOTAL;
                    if (miSchoolId == Constants.SchoolId.DSK.ToInt())
                        sHeader = "Total&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;Average";

                    iCellIndex++;
                    oGroupSubjectTotaldetails = FillSubjectDetails(iCellIndex, sHeader, oSubjectsInCurrentGroup[0].ParentSubjectId, Constants.I_ONE, string.Empty, Constants.I_ZERO, Constants.ReportCellType.GroupTotal);
                    oGroupSubjectTotaldetails.SubjectCellRowSpan = miTotalCellColSpan - 1;
                    oGroupSubjectTotaldetails.IsConsideredInTotal = false;
                    moGroupSubjectList.Add(oGroupSubjectTotaldetails);

                    miTotalCellCount++;
            }
        }

        return iCellIndex;
    }

    /// <summary>
    /// This method is used to create row having cells of given values
    /// </summary>
    /// <param name="aoArrGroupSubjectList"></param>
    /// <returns></returns>
    protected HtmlTableRow CreateGroupSubjectsRow(ArrayList aoArrGroupSubjectList)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        foreach (SubjectDetailsForProgressReport oGroupSubjectdetails in aoArrGroupSubjectList)
        {
            string sClass = oGroupSubjectdetails.SubjectCellType == Constants.ReportCellType.ExamTypeGroupTotal ? S_CSS_PRINT_PREFIX + S_CSS_CLSGPTESTTYPEHEADER : S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDHEADER + " " + S_CSS_CLSPADDING;
            if (oGroupSubjectdetails.IsConsideredInTotal)
            {
                if (menmPagemode == Constants.PageMode.Print)
                    CreateHtmlCell(oHtmlTableRow, oGroupSubjectdetails.Subjectname + "*", sClass, oGroupSubjectdetails.SubjectCellRowSpan, oGroupSubjectdetails.SubjectCellColSpan);
                else
                    CreateHtmlCell(oHtmlTableRow, oGroupSubjectdetails.Subjectname + "<font color='red'>*</font>", sClass, oGroupSubjectdetails.SubjectCellRowSpan, oGroupSubjectdetails.SubjectCellColSpan);
            }
            else
               CreateHtmlCell(oHtmlTableRow, oGroupSubjectdetails.Subjectname, sClass, oGroupSubjectdetails.SubjectCellRowSpan, oGroupSubjectdetails.SubjectCellColSpan);
        }

        return oHtmlTableRow;
    }

    /// <summary>
    /// This method is used to create 
    /// </summary>
    protected virtual HtmlTableRow CreateSubjectExamTypeHeader()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        moHTSubject.Clear();
        int iCurrentIndex = 1;

        // iterate through subjects collection
        moStudentProgressReport.SubjectDetails.ForEach(
            cursub =>
            {
                string sSubjectName = cursub.SubjectName;
                int iSubjectId = cursub.SubjectId;
                int iParentSubjectId = cursub.ParentSubjectId;
                int iSubjectCellColSpan;
                string sShortenTestTypeName;

                // Take only leaf subjects
                if (moStudentProgressReport.SubjectTestGroupTotalDetails.Count(stgt => stgt.ParentSubjectId == iSubjectId) <= 0)
                {
                    // Take exam types for a current subject
                    string sFilter = S_DB_COL_SUBJECT_ID + "=" + iSubjectId.ToString();
                    var oSubExamTypes = moStudentProgressReport.SubjectTestTypeDetails.Where(stt => stt.SubjectId == iSubjectId).OrderBy(stt => stt.TestTypeSortOrder).ToList<SubjectTestType>();
                    int isubExamTypeCount = 0;

                    // If subject have exam types then render its exam type
                    if (oSubExamTypes.Count > 0)
                    {
                    
                        if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
                        {
                            // render all exam type and put it's subject structure into hash table.
                            for (; isubExamTypeCount < oSubExamTypes.Count && (oSubExamTypes.Count != Constants.I_ONE || mbStudentwiseProgressReport || mbFinalResult); isubExamTypeCount++)
                            {
                                if (menmPagemode == Constants.PageMode.Print)
                                    CreateHtmlCell(oHtmlTableRow, oSubExamTypes[isubExamTypeCount].ShortenTestTypeName.Substring(0, 1), S_CSS_PRINT_PREFIX + S_CSS_CLSTESTHEADER, 1, 1);
                                else
                                    CreateHtmlCell(oHtmlTableRow, oSubExamTypes[isubExamTypeCount].ShortenTestTypeName, S_CSS_PRINT_PREFIX + S_CSS_CLSTESTHEADER, 1, 1);

                                iSubjectCellColSpan = oSubExamTypes[isubExamTypeCount].TestTypeId;
                                sShortenTestTypeName = oSubExamTypes[isubExamTypeCount].ShortenTestTypeName;
                                SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, iSubjectCellColSpan, sShortenTestTypeName, iParentSubjectId, bShowOnlyGradesInProgressSheet ? Constants.ReportCellType.Grade : Constants.ReportCellType.ExamType);
                                moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                                miTotalCellCount++;
                                iCurrentIndex++;
                            }

                            if (oSubExamTypes.Count == Constants.I_ONE && !mbStudentwiseProgressReport && !mbFinalResult)
                            {
                                String sExamType=null;
                                String sCollumnType=null;
                                if (IsTotalConsiderForProgressReport())
                                {
                                    sExamType = Constants.ReportCellType.ExamTypeTotal.ToString();
                                    sCollumnType = S_COL_TOTAL;
                                }
                                else
                                {
                                    sExamType = Constants.ReportCellType.ExamType.ToString();
                                    sCollumnType = oSubExamTypes[isubExamTypeCount].ShortenTestTypeName;
                                }
                                CreateHtmlCell(oHtmlTableRow, sCollumnType, S_CSS_PRINT_PREFIX + S_CSS_CLSTESTHEADER, 1, 1);

                                iSubjectCellColSpan = oSubExamTypes[oSubExamTypes.Count - 1].TestTypeId;
                                sShortenTestTypeName = oSubExamTypes[oSubExamTypes.Count - 1].ShortenTestTypeName;
                                SubjectDetailsForProgressReport oGroupSubjectdetails =IsTotalConsiderForProgressReport() ? FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, iSubjectCellColSpan, sShortenTestTypeName, iParentSubjectId, bShowOnlyGradesInProgressSheet ? Constants.ReportCellType.GradeExamTypeTotal : Constants.ReportCellType.ExamTypeTotal):
                                                                                                                                             FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, iSubjectCellColSpan, sShortenTestTypeName, iParentSubjectId, bShowOnlyGradesInProgressSheet ? Constants.ReportCellType.Grade : Constants.ReportCellType.ExamType);
                                moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                                iCurrentIndex++;
                                miTotalCellCount++;   
                            }
                             
                            // If exam types for this subject is greater than 1 then place total column for subject total and put this total subject structure into hash table.
                            if (isubExamTypeCount > 1)
                            {
                                CreateHtmlCell(oHtmlTableRow, S_COL_TOTAL, S_CSS_PRINT_PREFIX + S_CSS_CLSTESTHEADER, 1, 1);
                                iSubjectCellColSpan = oSubExamTypes[oSubExamTypes.Count - 1].TestTypeId;
                                sShortenTestTypeName = oSubExamTypes[oSubExamTypes.Count - 1].ShortenTestTypeName;
                                SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, iSubjectCellColSpan, sShortenTestTypeName, iParentSubjectId, bShowOnlyGradesInProgressSheet ? Constants.ReportCellType.GradeExamTypeTotal : Constants.ReportCellType.ExamTypeTotal);
                                moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                                iCurrentIndex++;
                                miTotalCellCount++;
                            }
                        }
                        else if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
                        {
                            if (iParentSubjectId.ToString() == "0")
                            {
                                iSubjectCellColSpan = oSubExamTypes[oSubExamTypes.Count - 1].TestTypeId;
                                sShortenTestTypeName = oSubExamTypes[oSubExamTypes.Count - 1].ShortenTestTypeName;
                                SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, iSubjectCellColSpan, sShortenTestTypeName, iParentSubjectId, bShowOnlyGradesInProgressSheet ? Constants.ReportCellType.GradeExamTypeTotal : Constants.ReportCellType.ExamTypeTotal);
                                                                                                
                                moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                                iCurrentIndex++;
                                miTotalCellCount++;
                            }
                        }
                    }
                    else
                    {
                        // If subject do not have exam types that means it have grade
                        if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
                            CreateHtmlCell(oHtmlTableRow, "Grade", S_CSS_PRINT_PREFIX + S_CSS_CLSTESTHEADER, 1, 1);
                        SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iSubjectId, Constants.I_ZERO, string.Empty, Constants.I_ZERO, Constants.ReportCellType.Grade);
                        oGroupSubjectdetails.SubjectCellColSpan = 0;
                        oGroupSubjectdetails.SubjectCellRowSpan = 1;
                        moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                        miTotalCellCount++;
                        iCurrentIndex++;
                    }

                   //  check if current subject is a member of subject group.
                    if (iParentSubjectId.ToString() != "0")
                    {
                        // Take the first subject from this group from all subjects of that group. 
                        // And check that is the current loop subject is the last subject of that group
                        var oSubjectsInCurrentGroup = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == iParentSubjectId).OrderBy(subject => subject.Id).ToList<Subject>();

                        if ((oSubjectsInCurrentGroup.Count > 0
                            && !oSubjectsInCurrentGroup[oSubjectsInCurrentGroup.Count - 1].SubjectName.IsNullOrEmpty())
                            && oSubjectsInCurrentGroup[oSubjectsInCurrentGroup.Count - 1].SubjectId == iSubjectId)
                        {
                            var oSubExamTypeDetails = moStudentProgressReport.SubjectTestTypeGroupTotalDetails.Where(sttgt => sttgt.ParentSubjectId == iParentSubjectId && sttgt.SchoolWiseTestId != -1)
                                                                                                              .OrderBy(subject => subject.TestTypeSortOrder)
                                                                                                              .ToList<SubjectTestTypeGroupTotal>();
                                
                            if (!bShortPrintEnabled || menmPagemode != Constants.PageMode.Print)
                            {
                                List<int> oTestTypesList = new List<int>();

                                // Add exam type group total for each exam type of each subject
                                for (int iExmCnt = 0; iExmCnt < oSubExamTypeDetails.Count; iExmCnt++)
                                {
                                    if (!oTestTypesList.Contains(oSubExamTypeDetails[iExmCnt].TestTypeId))
                                    {
                                        oTestTypesList.Add(oSubExamTypeDetails[iExmCnt].TestTypeId);
                                        iSubjectCellColSpan = oSubExamTypeDetails[iExmCnt].TestTypeId;
                                        SubjectDetailsForProgressReport oGroupExamTypeSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iParentSubjectId, iSubjectCellColSpan, string.Empty, iParentSubjectId, Constants.ReportCellType.ExamTypeGroupTotal);                                    
                                        moHTSubject[iCurrentIndex] = oGroupExamTypeSubjectdetails;
                                        iCurrentIndex++;
                                    }
                                }
                            }

                            // then put a subject structure into hashtable for a group total
                            SubjectDetailsForProgressReport oGroupSubjectdetails = FillSubjectDetails(iCurrentIndex, sSubjectName, iParentSubjectId, Constants.I_ONE, string.Empty, iParentSubjectId, Constants.ReportCellType.GroupTotal);
                                                                                                                                       // : FillSubjectDetails(iCurrentIndex, sSubjectName, iParentSubjectId, Constants.I_ONE, string.Empty, iParentSubjectId, Constants.ReportCellType.ExamType);
                            moHTSubject[iCurrentIndex] = oGroupSubjectdetails;
                            iCurrentIndex++;
                        }
                    }
                }
            }
        );

        return oHtmlTableRow;
    }

    /// <summary>
    /// This method is used to create HTAML table row with reqd coloumn.
    /// </summary>
    /// <param name="bIsHeader"></param>
    /// <returns></returns>
    protected HtmlTableRow CreateBlankRow()
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();

        // iterate through subjects collection
        for (int i = 0; i < miTotalCellCount; i++)
        {
            CreateHtmlCell(oHtmlTableRow, " ", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSCELL + " ClspaddingL", 1, 1);
        }

        // Create the summary totals for that test.
        CreateRowTotalCells(oHtmlTableRow);
        return oHtmlTableRow;
    }

    /// <summary>
    /// This method is used to create cell
    /// </summary>
    /// <param name="sInnerText"></param>
    /// <param name="sClassName"></param>
    /// <param name="iRowSpan"></param>
    /// <param name="iColSpan"></param>
    protected void CreateHtmlCell(HtmlTableRow oHtmlTableRow, string sInnerText, string sClassName, int iRowSpan, int iColSpan)
    {
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Align = "center";
        
        if (menmPagemode == Constants.PageMode.Print)
        {
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderColor, "black");
            oHtmlTableCell.NoWrap = false;

            if (moStudentProgressReport.SubjectDetails.Count(subject => subject.SubjectName == sInnerText) > 0)
                oHtmlTableCell.Width = Unit.Pixel(200).ToString();
        }
        else
            oHtmlTableCell.NoWrap = true;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);


        if ((!IsTotalConsiderForProgressReport()) && (sInnerText.Equals("Total") || sInnerText.Equals("%") || sInnerText.Equals("Result") || sInnerText.Equals("-") || (sInnerText.Equals("Rank") && ShouldShowRankColumn())))
            oHtmlTableCell.Attributes.Add("style", "display:none");

        //CheckDSKStatus(oHtmlTableCell);

        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// This method is used to create required Html rows For tests and add it to progress table
    /// </summary>    
    protected virtual void CreateExamsAndTotalBlankRows()
    {
        CreateExamBlankRows(moStudentProgressReport.ExamDetails);
    }

    /// <summary>
    /// This method is used to create required Html rows For tests and add it to progress table
    /// </summary>    
    protected Exam CreateExamBlankRows(List<Exam> aolstExamDetails)
    {
        bool bAltRow = true;
        Exam oTotalExam = aolstExamDetails[0];

        // iterate through tests collection.
        aolstExamDetails.ForEach(
            exam =>
            {
                if (exam.SchoolWiseTestId != -1)
                {
                    CreateAndAddBlankRows(exam, bAltRow);
                    bAltRow = !bAltRow;
                }
                else
                    oTotalExam = exam;
            }
        );

        return oTotalExam;
    }

    /// <summary>
    /// This method is used to create and blank rows for a exam
    /// </summary>
    protected void CreateAndAddBlankRows(Exam aoExam, bool abAltRow)
    {
        HtmlTableRow oHtmlTableRow = CreateBlankRow();

        // Crete row for that test containing required subject's cells
        if (!aoExam.SchoolWiseTestName.IsNullOrEmpty())
        {
            // Create the row header cell with the test name and css class for alternet rows
            HtmlTableCell oBlankCell = new HtmlTableCell { InnerText = aoExam.SchoolWiseTestName };
            if (abAltRow)
                oBlankCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDROW);
            else
                oBlankCell.Attributes.Add("class", S_CSS_PRINT_PREFIX + S_CSS_CLSMARKSGRIDALTROW);
            oHtmlTableRow.Cells.Insert(0, oBlankCell);
            // Add this row to the table.
            tblProgress.Rows.Add(oHtmlTableRow);
            oBlankCell.Dispose();
        }

        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to fill tests result to a progress table
    /// </summary>    
    protected virtual void FillExamsMarks()
    {
        FillExamWiseSubjectMarks();
    }

    protected Exam FillExamWiseSubjectMarks()
    {
        HtmlTableRow oHtmlTableRow;
        Exam oExamDetails = moStudentProgressReport.ExamDetails[0];

        // Skip col headers
        int iRowIndex = 3;
        moStudentProgressReport.ExamDetails.ForEach(
            exam =>
            {
                if (exam.SchoolWiseTestId != -1)
                {
                    oHtmlTableRow = tblProgress.Rows[iRowIndex];

                    // Skip row header
                    foreach (DictionaryEntry oHTSubjectEntry in moHTSubject)
                    {
                        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
                        Constants.ReportCellType enumSubjectColType = oSubjectDetailsForProgressReport.SubjectCellType;
                        var oMarks = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId && ma.SchoolWiseTestId == exam.SchoolWiseTestId).ToList<MarkAssignment>();
                        mbDisplayGrade = oMarks.Count > 0 && oMarks[0].ShowOnlyGrades;

                        bool bTestApplicable = oMarks.Count > 0 && oMarks.Any(oMark => oMark.ShortenTestTypeName == oSubjectDetailsForProgressReport.TestTypeName);

                        // Update Constants.ReportCellType to Grade, if we have to diplay grade instead of marks.
                        // We don't do this in case this is being called from the StudentWiseProgressReport screen.
                        if (!mbStudentwiseProgressReport && mbDisplayGrade && bTestApplicable && !mbFinalResult)
                            enumSubjectColType = Constants.ReportCellType.Grade;

                        if (mbStudentwiseProgressReport)
                        {
                            if (!mbStudentwiseProgressReport)
                                oMarks = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId &&
                                                                                                   ma.SchoolWiseTestId == exam.SchoolWiseTestId && 
                                                                                                   ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan
                                                                                            ).ToList<MarkAssignment>();
                            if (oMarks.Count > 0)
                            {
                                if (enumSubjectColType == Constants.ReportCellType.Grade && oMarks[0].GradeOrMarks == "M")
                                    enumSubjectColType = Constants.ReportCellType.ExamType;

                                if (enumSubjectColType == Constants.ReportCellType.ExamType && oMarks[0].GradeOrMarks == "G")
                                    enumSubjectColType = Constants.ReportCellType.Grade;

                                if (enumSubjectColType == Constants.ReportCellType.GradeExamTypeTotal && oMarks[0].GradeOrMarks == "M")
                                    enumSubjectColType = Constants.ReportCellType.ExamTypeTotal;

                                if (enumSubjectColType == Constants.ReportCellType.ExamTypeTotal && oMarks[0].GradeOrMarks == "G")
                                    enumSubjectColType = Constants.ReportCellType.GradeExamTypeTotal;
                            }
                        }

                        CreateCell(enumSubjectColType, exam.SchoolWiseTestId, oHTSubjectEntry, iRowIndex, oHtmlTableRow);
                    }
                    // Fill the totals summary for that test row.
                    FillExamTotals(oHtmlTableRow, iRowIndex, exam.SchoolWiseTestId);
                    iRowIndex++;
                    oHtmlTableRow.Dispose();
                }
                else
                    oExamDetails = exam;
            }
        );

        return oExamDetails;
    }

    /// <summary>
    /// This method is used to Fill subject total.
    /// </summary>
    /// <param name="aoExam"></param>
    /// <param name="iRowIndex"></param>
    protected virtual void FillSubjectTotal(Exam aoExam, int aiRowIndex)
    {
        HtmlTableRow oHtmlTableRow;
        oHtmlTableRow = tblProgress.Rows[aiRowIndex];

        // Skip row header
        foreach (DictionaryEntry oHTSubjectEntry in moHTSubject)
        {
            SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
            CreateCell(oSubjectDetailsForProgressReport.SubjectCellType, aoExam.SchoolWiseTestId, oHTSubjectEntry, aiRowIndex, oHtmlTableRow);
        }

        aiRowIndex++;
    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected virtual void FillSubjectExamGroupTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Where(sd => sd.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).OrderByDescending(sd => sd.Id).ToList<Subject>();
        if (oParentSubjectDetails.Count > 0 && !oParentSubjectDetails[0].SubjectName.IsNullOrEmpty())
        {
            // Take a group total of a subject.
            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectTestGroupTotalDetails.Cast<StudentWiseProgressReportSubjectTestGroupTotal>()
                                                                                                  .Where(sgt => sgt.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId && 
                                                                                                                sgt.SchoolWiseTestId == aiTestId).ToList();
            if (oParentSubjectTotalDetails.Count > 0)
            {
                List<int> olstSubjectIds = new List<int>();
                oParentSubjectDetails.ForEach(ps => olstSubjectIds.Add(ps.SubjectId));
                
                HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];

                var oSubjectGroupRecords = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && olstSubjectIds.Contains(ma.SubjectId)).ToList<MarkAssignment>();
                int iAbsentChildCount = 0;
                if (oSubjectGroupRecords.Count > 0)
                    iAbsentChildCount = oSubjectGroupRecords.Count(sg => sg.IsAbsent != Constants.S_NO);

                if (!mbStudentwiseProgressReport && iAbsentChildCount == oSubjectGroupRecords.Count)
                {
                    ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oSubjectGroupRecords[0].IsAbsent);
                    string sExamStatus = oExamStatus.DisplayName;
                    string sColor = oExamStatus.ForeColor;
                    if (menmPagemode != Constants.PageMode.Print)
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
                    else
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
                }
                else
                {
                    if (bShowOnlyGradesInProgressSheet)
                        oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].Grade + "</B>";
                    else
                    {
                        if (oParentSubjectTotalDetails[0].AverageMarks == 0)
                        {
                            if (menmPagemode == Constants.PageMode.Print)
                                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].TotalMarksScored.ToDecimal().ToString("0.#") + "</B>" + "/" + oParentSubjectTotalDetails[0].ChildSubjectMarksTotal;
                            else
                                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].TotalMarksScored.ToDecimal().ToString("0.#") + "</B>" + " / " + oParentSubjectTotalDetails[0].ChildSubjectMarksTotal;
                        }
                        else
                        {
                            HtmlTable tbl = new HtmlTable();
                            HtmlTableRow tr = new HtmlTableRow();
                            HtmlTableCell td = new HtmlTableCell();
                            
                            tbl.Width = "100%";

                            if (menmPagemode == Constants.PageMode.Print)
                                oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].AverageMarks.ToDecimal().ToString("0.#") + "</B>" + "/" + oParentSubjectTotalDetails[0].OutOfMarks.ToString("0");
                            else
                            {
                                td.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].TotalMarksScored.ToDecimal().ToString("0.#") + "</B>" + " / " + oParentSubjectTotalDetails[0].ChildSubjectMarksTotal;
                                td.Width = "45%%";
                                tr.Cells.Add(td);

                                td = new HtmlTableCell();
                                td.Width = "10%";
                                td.InnerHtml = "|";
                                td.Style.Add("padding-left", "5px");
                                td.Style.Add("padding-right", "5px");
                                tr.Cells.Add(td);

                                td = new HtmlTableCell();
                                td.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].AverageMarks.ToDecimal().ToString("0.#") + "</B>" + " / " + oParentSubjectTotalDetails[0].OutOfMarks.ToInt();
                                tr.Cells.Add(td);
                                tbl.Rows.Add(tr);
                                oHtmlTableCell.Controls.Add(tbl);
                                //oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].AverageMarks.ToDecimal().ToString("0.#") + "</B>" + " / " + oParentSubjectTotalDetails[0].OutOfMarks.ToInt();
                            }
                        }

                    }

                    oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];

                }
                if (!IsTotalConsiderForProgressReport() && menmPagemode != Constants.PageMode.Print)
                    oHtmlTableCell.Attributes.Add("style", "display:none");

                //CheckDSKStatus(oHtmlTableCell);
            }
            else
                FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
        }
        else
            FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected virtual void FillGroupSubjectExamTypeTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).OrderByDescending(subject => subject.Id).ToList<Subject>();
        if (oParentSubjectDetails.Count > 0 && !oParentSubjectDetails[0].SubjectName.IsNullOrEmpty())
        {
            // Take a group total of a subject.

            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectTestTypeGroupTotalDetails.Where(sttgt => sttgt.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId && 
                                                                                                                     sttgt.SchoolWiseTestId == aiTestId && 
                                                                                                                     sttgt.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan).ToList();
            if (oParentSubjectTotalDetails.Count > 0)
            {
                List<int> olstSubjectIds = new List<int>();
                oParentSubjectDetails.ForEach(ps => olstSubjectIds.Add(ps.SubjectId));

                HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];

                var oSubjectGroupRecords = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                                     ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                                     olstSubjectIds.Contains(ma.SubjectId)).ToList<MarkAssignment>();
                int iAbsentChildCount = 0;
                if (oSubjectGroupRecords.Count > 0)
                    iAbsentChildCount = oSubjectGroupRecords.Count(sg => sg.IsAbsent != Constants.S_NO);

                if (!mbStudentwiseProgressReport && iAbsentChildCount == oSubjectGroupRecords.Count)
                {
                    ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oSubjectGroupRecords[0].IsAbsent);
                    string sExamStatus = oExamStatus.DisplayName;
                    string sColor = oExamStatus.ForeColor;
                    if (menmPagemode != Constants.PageMode.Print)
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
                    else
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
                }
                else
                {
                    if (bShowOnlyGradesInProgressSheet)
                        oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].Grade + "</B>";
                    else
                        oHtmlTableCell.InnerHtml = "<B>" + oParentSubjectTotalDetails[0].TestTypeTotalMarksScored.ToDecimal().ToString("0.#") + "</B>" + (menmPagemode == Constants.PageMode.Print ? "/" : " / ") + oParentSubjectTotalDetails[0].TestTypeTotalMarks.ToDecimal().ToString("0.#");

                    oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
 
                }
            }
            else
                FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
        }
        else
            FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
    }

    /// <summary>
    /// This method is used to set exam type total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected virtual void FillSubjectExamTypeTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                               ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId && 
                                                                                               ma.ConsiderExamStatus == Constants.S_YES).ToList<MarkAssignment>();
        if (oMarkAssignmentDetails.Count > 0 && !oMarkAssignmentDetails[0].Marks.IsNullOrEmpty() )
        {
            // If subject has grade then don't append total marks (i.e 12/100)
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];

            int iRowIndex = 0;
            int iCount = oMarkAssignmentDetails.Count(ma => ma.IsAbsent != Constants.S_NO);
            if (iCount > Constants.I_ZERO)
                foreach (MarkAssignment oMarkAssignment in oMarkAssignmentDetails)
                    if (oMarkAssignment.IsAbsent.Trim() == Constants.S_NO)
                        iRowIndex++;
                    else break;

            if (miSchoolId == Constants.SchoolId.PPSH.ToInt() || miSchoolId == Constants.SchoolId.MNS.ToInt())
            {
                int iIndex = 0;
                //if (iCount > Constants.I_ZERO && oMarkAssignmentDetails.Any(nm => nm.ShortenTestTypeName == "Practical" && nm.IsAbsent == Constants.S_YES) && !oMarkAssignmentDetails.Any(nm => nm.ShortenTestTypeName != "Practical" && nm.IsAbsent == Constants.S_YES))

                if (iCount > Constants.I_ZERO && oMarkAssignmentDetails.Count > 1)
                {
                    foreach (MarkAssignment oMarkAssignment in oMarkAssignmentDetails)
                        if (oMarkAssignment.IsAbsent.Trim() == Constants.S_YES)
                            iIndex++;
                        else
                            break;

                    if (iIndex < oMarkAssignmentDetails.Count)
                    {
                        if (oMarkAssignmentDetails[iIndex].Marks == "Absent")
                            oMarkAssignmentDetails[iIndex].Marks = oMarkAssignmentDetails[iIndex].MarksScored.ToString();

                        iRowIndex = iIndex;
                    }
                }                
            }

            if (oMarkAssignmentDetails[iRowIndex].IsAbsent.Trim() == Constants.S_NO && oMarkAssignmentDetails[iRowIndex].ConsiderInResult.Trim() == Constants.S_YES)
            {
                if (oMarkAssignmentDetails[iRowIndex].GradeOrMarks.Trim() == "M")
                {
                    if (menmPagemode == Constants.PageMode.Print)
                        oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[iRowIndex].Marks.ToDecimal().ToString("0.#") + "</B>" + "/" + oMarkAssignmentDetails[iRowIndex].SubjectTotalMarks;
                    else
                        oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[iRowIndex].Marks.ToDecimal().ToString("0.#") + "</B>" + " / " + oMarkAssignmentDetails[iRowIndex].SubjectTotalMarks;
                }
                else
                    oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[iRowIndex].Marks + "</B>";
            }
            else
            {
                ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oMarkAssignmentDetails[iRowIndex].IsAbsent);
                string sExamStatus = oExamStatus.DisplayName;
                string sColor = oExamStatus.ForeColor;
                if (menmPagemode != Constants.PageMode.Print)
                    oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
                else
                    oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
            }

            oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"];
            if (!IsTotalConsiderForProgressReport() && menmPagemode != Constants.PageMode.Print)
                oHtmlTableCell.Attributes.Add("style", "display:none");
            //CheckDSKStatus(oHtmlTableCell);
        }
        else
             SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), string.Empty);
    }


    public void SetNotApplicableCellValuesForExamTypeTotal(HtmlTableRow oHtmlTableRow, int aiCellIndex, string sCssClass)
    {
        HtmlTableCell oHtmlTableCell;
         // That means this is not applicable cell
        oHtmlTableCell = oHtmlTableRow.Cells[aiCellIndex];
        oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"].Replace(S_CSS_CLSPADDINGL, string.Empty).Replace(S_CSS_PADDINGL, string.Empty);
        oHtmlTableCell.Align = "center";

         // some times we r getting additional group total coloumn into this section so this is the addtional checking
         // that set not applicable only if this cell is filled with value.
        if (oHtmlTableCell.InnerText.Trim() == string.Empty)
              oHtmlTableCell.InnerText = S_NON_APPLICABLE;
        oHtmlTableCell.Dispose();
      
        if (!IsTotalConsiderForProgressReport())
             oHtmlTableCell.Attributes.Add("style", "display:none");

        CheckDSKStatus(oHtmlTableCell);
    }
    /// <summary>
    /// This method is used to set grade to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected virtual void FillSubjectExamGrade(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        Constants.ReportCellType enumSubjectColType = oSubjectDetailsForProgressReport.SubjectCellType;

        // Update Constants.ReportCellType to Grade, if we have to diplay grade instead of marks.
		if (mbDisplayGrade)
            enumSubjectColType = (oSubjectDetailsForProgressReport.SubjectCellType == Constants.ReportCellType.ExamTypeTotal) ? Constants.ReportCellType.GradeExamTypeTotal : Constants.ReportCellType.Grade;

        List<MarkAssignment> oMarkAssignmentDetails;
        if (mbDisplayGrade && oSubjectDetailsForProgressReport.SubjectCellColSpan != 0 || bShowOnlyGradesInProgressSheet && oSubjectDetailsForProgressReport.SubjectCellColSpan != 0)
        {
            oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                               ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                               ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList<MarkAssignment>();
        }
        else
        {
            oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList<MarkAssignment>();
        }

        if (menmPagemode == Constants.PageMode.Print || enumSubjectColType == Constants.ReportCellType.GradeExamTypeTotal)
            oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList<MarkAssignment>();
        
        HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
        if (oMarkAssignmentDetails.Count > 0 && !oMarkAssignmentDetails[0].Grade.IsNullOrEmpty())
        {
            string sExamStatus = string.Empty;
            string sColor = string.Empty;
            if (oMarkAssignmentDetails[0].IsAbsent != Constants.S_NO)
            {
                ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oMarkAssignmentDetails[0].IsAbsent);
                sExamStatus = oExamStatus.DisplayName;
                sColor = oExamStatus.ForeColor;
            }

            if (!oMarkAssignmentDetails[0].GradeOrMarks.IsNullOrEmpty()
                && oMarkAssignmentDetails[0].IsAbsent == "N" && enumSubjectColType == Constants.ReportCellType.Grade)
            {
                // If subject has grade then dont append total marks(i.e 12/100)  
                if (enumSubjectColType == Constants.ReportCellType.Grade)
                    oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[0].Grade + "</B>";
            }
            else if (enumSubjectColType == Constants.ReportCellType.GradeExamTypeTotal)
            {
                var oMarkAssignment = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                          ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                          ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList<MarkAssignment>();

                int iRowIndex = 0;
                int iCount = oMarkAssignmentDetails.Count(ma => ma.IsAbsent != Constants.S_NO);
                if (iCount > Constants.I_ZERO)
                    foreach (MarkAssignment ma in oMarkAssignmentDetails)
                    {
                        if (ma.IsAbsent.Trim() == Constants.S_NO)
                            iRowIndex++;
                        else break;
                    }

                if (oMarkAssignment.Count > Constants.I_ZERO && iRowIndex <= oMarkAssignment.Count - 1 && oMarkAssignment[iRowIndex].IsAbsent != Constants.S_NO)
                {
                    ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oMarkAssignment[iRowIndex].IsAbsent);
                    sColor = oExamStatus.ForeColor;
                    sExamStatus = oExamStatus.DisplayName;
                }

                if (iRowIndex > oMarkAssignment.Count - 1)
                    iRowIndex = 0;

                if (oMarkAssignmentDetails[iRowIndex].IsAbsent == "N")
                {
                    ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.DisplayName == oMarkAssignmentDetails[iRowIndex].TotalGrade);
                    if (oExamStatus != null && !oExamStatus.DisplayName.IsNullOrEmpty())
                    {
                        if (menmPagemode != Constants.PageMode.Print)
                            oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + oExamStatus.ForeColor + "'>" + oExamStatus.DisplayName + "</font></B>";
                        else
                            oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + oExamStatus.DisplayName + "</font></B>";
                    }
                    else
                        oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[iRowIndex].TotalGrade + "</B>";
                }
                else
                {
                    if (menmPagemode != Constants.PageMode.Print)
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
                    else
                        oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
                }
            }
            else if (menmPagemode != Constants.PageMode.Print)
                oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
            else
                oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
            if ((!IsTotalConsiderForProgressReport()) && (enumSubjectColType.ToString() == "GradeExamTypeTotal") && menmPagemode != Constants.PageMode.Print)
                oHtmlTableCell.Attributes.Add("style", "display:none");

            CheckDSKStatus(oHtmlTableCell);
        }
        else if ((!IsTotalConsiderForProgressReport()) && enumSubjectColType.ToString() == "GradeExamTypeTotal" && menmPagemode != Constants.PageMode.Print)
            SetNotApplicableCellValuesForExamTypeTotal(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set marks to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected virtual void FillSubjectExamMarks(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                               ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                               ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList<MarkAssignment>();
        if (oMarkAssignmentDetails.Count > 0 && !oMarkAssignmentDetails[0].Marks.IsNullOrEmpty())
        {
            // If subject has grade then dont append total marks(i.e 12/100)                     
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
            if (oMarkAssignmentDetails[0].IsAbsent == "N")
            {
                if (bShowOnlyGradesInProgressSheet)
                    oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[0].Grade + "</B>";
                else
                {
                    if (menmPagemode == Constants.PageMode.Print)
                        oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[0].MarksScored.ToDecimal().ToString("0.#") + "</B>" + "/" + oMarkAssignmentDetails[0].TestTypeTotalMarks;
                    else
                        oHtmlTableCell.InnerHtml = "<B>" + oMarkAssignmentDetails[0].MarksScored.ToDecimal().ToString("0.#") + "</B>" + " / " + oMarkAssignmentDetails[0].TestTypeTotalMarks;
                }
            }
            else
            {
                ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(marks => marks.ShortName == oMarkAssignmentDetails[0].IsAbsent);
                string sExamStatus = oExamStatus.DisplayName;
                if (menmPagemode != Constants.PageMode.Print)
                    oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + oExamStatus.ForeColor + "'>" + sExamStatus + "</font></B>";
                else
                    oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' >" + sExamStatus + "</font></B>";
            }

         
            oHtmlTableCell.Dispose();
        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set not applicable to the cell
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="aiCellIndex"></param>
    protected void SetNotApplicableCellValues(HtmlTableRow oHtmlTableRow, int aiCellIndex, string sCssClass)
    {
        HtmlTableCell oHtmlTableCell;

        // That means this is not applicable cell
        oHtmlTableCell = oHtmlTableRow.Cells[aiCellIndex];
        oHtmlTableCell.Attributes["class"] = oHtmlTableCell.Attributes["class"].Replace(S_CSS_CLSPADDINGL, string.Empty).Replace(S_CSS_PADDINGL, string.Empty);
        oHtmlTableCell.Align = "center";

        // some times we r getting additional group total coloumn into this section so this is the addtional checking
        // that set not applicable only if this cell is filled with value.
        if (oHtmlTableCell.InnerText.Trim() == string.Empty)
            oHtmlTableCell.InnerText = S_NON_APPLICABLE;

        oHtmlTableCell.Dispose();
    }

    /// <summary>
    /// Fill the totals summary for that test row.
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiTestId"></param>
    protected virtual void FillExamTotals(HtmlTableRow aoHtmlTableRow, int aiRowIndex, int aiTestId)
    {
        FillExamTotalDetails(GetExamTotal(aiTestId), aoHtmlTableRow, aiRowIndex, aiTestId);
    }

    /// <summary>
    /// This method is used to get exam totals
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <returns></returns>
    protected virtual ExamWisePercentage GetExamTotal(int aiTestId)
    {
        return moStudentProgressReport.ExamWisePercentageDetails.FirstOrDefault(ewp => ewp.SchoolWiseTestId == aiTestId);        
    }

    /// <summary>
    /// Fills combobox for each row if the grades are to be assigned to students.
    /// </summary>
    protected void FillGradesCombobox(DropDownList ddlGrade)
    {
        ddlGrade.DataTextField = "GradeName";
        ddlGrade.DataValueField = "GradeId";
        ddlGrade.DataSource = moStudentProgressReport.GradeDetails;
        ddlGrade.DataBind();
        ddlGrade.Dispose();
    }

    /// <summary>
    /// This method is used to reset member variable and controls for next rendering.
    /// </summary>
    protected void ResetControls()
    {
        try
        {
            if (!mbStudentwiseProgressReport)
                moStudentProgressReport = new StudentProgressReport();
            miTotalCellCount = 0;
            moGroupSubjectList.Clear();
            moHTSubject.Clear();
            moHTSubject = null;
            moGroupSubjectList = null;
            GC.Collect();
            moHTSubject = new Hashtable();
            moGroupSubjectList = new ArrayList();
            mbIsApplicable = false;
        }
        catch (System.Web.HttpException oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Protected method

    #region Private Method

    /// <summary>
    /// This method is used to create cell as per column type.
    /// </summary>
    /// <param name="aenumSubjectColType"></param>
    /// <param name="iTestId"></param>
    /// <param name="aoHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aoHtmlTableRow"></param>
    private void CreateCell(Constants.ReportCellType aenumSubjectColType, int iTestId, DictionaryEntry aoHTSubjectEntry, int aiRowIndex, HtmlTableRow aoHtmlTableRow)
    {
        switch (aenumSubjectColType)
        {
            case Constants.ReportCellType.ExamType:
                FillSubjectExamMarks(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;

            case Constants.ReportCellType.Grade:
                FillSubjectExamGrade(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;

            case Constants.ReportCellType.ExamTypeTotal:
                FillSubjectExamTypeTotal(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;

            case Constants.ReportCellType.GradeExamTypeTotal:
                FillSubjectExamGrade(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;

            case Constants.ReportCellType.GroupTotal:
                FillSubjectExamGroupTotal(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;
            case Constants.ReportCellType.ExamTypeGroupTotal:
                FillGroupSubjectExamTypeTotal(iTestId, aoHTSubjectEntry, aiRowIndex);
                break;

            default:
                SetNotApplicableCellValues(aoHtmlTableRow, aoHTSubjectEntry.Key.ToInt(), null);
                break;
        }
    }

    /// <summary>
    /// This method is used to set Student Progress dataSet.
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void SetStudentTestProgressDataSet(int aiStudentId)
    {
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        moStudentProgressReport = oStudentSubjectMarksBL.GetStudentTestProgressResult(miSchoolId, miAcademicYearId, aiStudentId, miTestId);
    }
    /// <summary>
    /// This method use to set total value for progress report in print preview mode
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="aiCellIndesx"></param>
    private void ShowTotalOnPrintPreview(HtmlTableRow aoHtmlTableRow, int aiCellIndesx)
    {
        HtmlTableCell oHtmlTableCell = null;
        oHtmlTableCell = aoHtmlTableRow.Cells[aiCellIndesx];
        oHtmlTableCell.InnerHtml = "<B> - </B>";
        oHtmlTableCell.Align = HorizontalAlign.Center.ToString();
        if (!IsTotalConsiderForProgressReport())
            oHtmlTableCell.Attributes.Add("style", "display:none");

        CheckDSKStatus(oHtmlTableCell);
    }
    /// <summary>
    /// This method is used to creat dataset from a xml datarow
    /// </summary>
    /// <param name="oDataRow"></param>
    private void CreateStudentProgressDataSet(DataRow oDataRow)
    {
        moStudentProgressReport = new StudentProgressReport();
        if (oDataRow[1] != DBNull.Value)
            FillStudentDetailsFromXml(oDataRow[1].ToString());
        if (oDataRow[2] != DBNull.Value)
            FillSubjectDetailsFromXml(oDataRow[2].ToString());
        if (oDataRow[3] != DBNull.Value)
            FillExamDetailsFromXml(oDataRow[3].ToString());
        if (oDataRow[4] != DBNull.Value)
            FillMarksAssignmentDetailsFromXML(oDataRow[4].ToString());
        if (oDataRow[5] != DBNull.Value)
            FillExamWisePercentageDetailsFromXML(oDataRow[5].ToString());
        if (oDataRow[6] != DBNull.Value)
            FillSubjectTestGroupTotalDetailsFromXML(oDataRow[6].ToString());
        if (oDataRow[7] != DBNull.Value)
            FillSubjectTestTypeGroupTotalDetailsFromXML(oDataRow[7].ToString());
        if (oDataRow[8] != DBNull.Value)
            FillSubjectTestTypeDetailsFromXML(oDataRow[8].ToString());
        if (oDataRow[9] != DBNull.Value)
            FillTestTypeDetailsFromXML(oDataRow[9].ToString());
        if (oDataRow[10] != DBNull.Value)
            FillGradeDetailsFromXML(oDataRow[10].ToString());
    }

    /// <summary>
    /// This method is used to fill student details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillStudentDetailsFromXml(string asXmlData)
    {
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table").ToList().ForEach(
            student => {
                StudentDetails oStudentDetails = new StudentDetails
                {
                    YearWiseStudentId = student.Element("YearWise_Student_Id").Value.ToInt(),
                    StudentName = student.Element("Student_Name").Value.ToString(),
                    StandardDivisionDetails = new MasterEntities.StandardDivisionMaster()
                    {
                        StandardName = student.Element("Standard_Name").Value.ToString(),
                        DivisionName = student.Element("Division_Name").Value.ToString(),
                        StandardDivisionId = student.Element("Standard_Division_Id").Value.ToInt(),
                        StandardId = student.Element("Standard_Id").Value.ToInt(),
                    },
                    AcademicYear = student.Element("Academic_Year").Value.ToString(),
                    RollNo = student.Element("Roll_No").Value.ToInt(),
                    EnrolmentNumber = student.Element("Enrolment_Number").Value.ToString(),
                    SchoolName = student.Element("School_Name").Value.ToString(),
                    OrganizationName = student.Element("School_Orgn_Name").Value.ToString(),
                    ShowOnlyGrades = student.Element("ShowOnlyGrades").Value.ToBool(),
                    IsFailCriteriaNotApplicable = student.Element("IsFailCriteriaNotApplicable").Value.ToString(),
                };

                moStudentProgressReport.StudentDetails = oStudentDetails;
            }
        );
    }

    private void CheckDSKStatus(HtmlTableCell aoHtmlTableCell)
    {
        //if(miSchoolId == Constants.SchoolId.DSK.ToInt())
        //{
        //    if (moStudentProgressReport.StudentDetails.StandardDivisionDetails.StandardId == 1017 || moStudentProgressReport.StudentDetails.StandardDivisionDetails.StandardId == 1018 || moStudentProgressReport.StudentDetails.StandardDivisionDetails.StandardId == 1019)
        //        aoHtmlTableCell.InnerText = "-";
        //}
    }

    /// <summary>
    /// This method is used to fill subject details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillSubjectDetailsFromXml(string asXmlData)
    {
        XDocument xmldoc = XDocument.Parse(asXmlData);
        moStudentProgressReport.SubjectDetails = new List<Subject>();
        xmldoc.Elements("NewDataSet").Elements("Table1").ToList()
        .ForEach(
            subject => 
            {
                ProgressReportSubujectDetails oProgressReportSubujectDetails = new ProgressReportSubujectDetails
                {
                    Id = subject.Element("ID_Num").Value.ToInt(),
                    SubjectName = subject.Element("Subject_Name").Value.ToString(),
                    SubjectId = subject.Element("Subject_Id").Value.ToInt(),
                    ParentSubjectId = subject.Element("Parent_Subject_Id").Value.ToInt(),
                    TotalConsideration = subject.Element("Total_Consideration").Value.ToString(),
                    SortOrder = subject.Element("Sort_Order").Value.ToInt(),
                };
                moStudentProgressReport.SubjectDetails.Add(oProgressReportSubujectDetails);
            }
        );
    }
    
    /// <summary>
    /// This method is used to fill exam details from XML.
    /// </summary>
    /// <param name="axmlData"></param>
    private void FillExamDetailsFromXml(string axmlData)
    {
        moStudentProgressReport.ExamDetails = new List<Exam>();
        XDocument xmldoc = XDocument.Parse(axmlData);
        xmldoc.Elements("NewDataSet").Elements("Table2").ToList().ForEach(
            exam =>
            {
                Exam oExam = new Exam
                {
                    SchoolWiseTestName = exam.Element("Test_Name").Value.ToString(),
                    SchoolWiseTestId = exam.Element("Test_Id").Value.ToInt(),
                    OriginalShcoolWiseTestId = exam.Element("Original_SchoolWise_Test_Id").Value.ToInt(),
                };
                moStudentProgressReport.ExamDetails.Add(oExam);
            }
        );
    }

    /// <summary>
    /// This method is used to fill marks assignment details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillMarksAssignmentDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.MarkAssignmentDetails = new List<MarkAssignment>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table3").ToList().ForEach(
            marks =>
            {
                MarkAssignment oMarkAssignment = new MarkAssignment();
                
                oMarkAssignment.SubjectId = marks.Element("Subject_Id").Value.ToInt();
                if (!marks.Element("Marks").IsNull())
                    oMarkAssignment.Marks = marks.Element("Marks").Value.ToString();
                if (!marks.Element("SchoolWise_Test_Id").IsNull())
                    oMarkAssignment.SchoolWiseTestId = marks.Element("SchoolWise_Test_Id").Value.ToInt();
                if (!marks.Element("Original_SchoolWise_Test_Id").IsNull())
                    oMarkAssignment.OriginalShcoolWiseTestId = marks.Element("Original_SchoolWise_Test_Id").Value.ToInt();
                if (!marks.Element("SchoolWise_Test_Name").IsNull())
                    oMarkAssignment.SchoolWiseTestName = marks.Element("SchoolWise_Test_Name").Value.ToString();
                if (!marks.Element("Subject_Name").IsNull())
                    oMarkAssignment.SubjectName = marks.Element("Subject_Name").Value.ToString();
                if (!marks.Element("Total_Marks_Scored").IsNull())
                    oMarkAssignment.TotalMarksScored = marks.Element("Total_Marks_Scored").Value.ToDecimal();
                if (!marks.Element("Subject_Total_Marks").IsNull())
                    oMarkAssignment.SubjectTotalMarks = marks.Element("Subject_Total_Marks").Value.ToInt();
                if (!marks.Element("Passing_Total_Marks").IsNull())
                    oMarkAssignment.PassingTotalMarks = marks.Element("Passing_Total_Marks").Value.ToDecimal();
                if (!marks.Element("Subject_Total").IsNull())
                    oMarkAssignment.SubjectTotal = marks.Element("Subject_Total").Value.ToString();
                if (!marks.Element("Grade_Or_Marks").IsNull())
                    oMarkAssignment.GradeOrMarks = marks.Element("Grade_Or_Marks").Value.ToString();
                if (!marks.Element("TestType_Id").IsNull())
                    oMarkAssignment.TestTypeId = marks.Element("TestType_Id").Value.ToInt();
                if (!marks.Element("Marks_Scored").IsNull())
                    oMarkAssignment.MarksScored = marks.Element("Marks_Scored").Value.ToDecimal();
                if (!marks.Element("TestType_Name").IsNull())
                    oMarkAssignment.TestTypeName = marks.Element("TestType_Name").Value.ToString();
                if (!marks.Element("ShortenTestType_Name").IsNull())
                    oMarkAssignment.ShortenTestTypeName = marks.Element("ShortenTestType_Name").Value.ToString();
                if (!marks.Element("TestType_Total_Marks").IsNull()) 
                    oMarkAssignment.TestTypeTotalMarks = marks.Element("TestType_Total_Marks").Value.ToInt();
                if (!marks.Element("TestType_Passing_Marks").IsNull())
                    oMarkAssignment.TestTypePassingMarks = marks.Element("TestType_Passing_Marks").Value.ToDecimal();
                if (!marks.Element("Is_Absent").IsNull())
                    oMarkAssignment.IsAbsent = marks.Element("Is_Absent").Value.ToString();
                if (!marks.Element("SchoolWise_Student_Test_Marks_Id").IsNull())
                    oMarkAssignment.SchoolWiseStudentTestId = marks.Element("SchoolWise_Student_Test_Marks_Id").Value.ToInt();
                if (!marks.Element("TestWise_Subject_Marks_Id").IsNull())
                    oMarkAssignment.TestWiseSubjectId = marks.Element("TestWise_Subject_Marks_Id").Value.ToInt();
                if (!marks.Element("ConsiderExamStatus").IsNull())
                    oMarkAssignment.ConsiderExamStatus = marks.Element("ConsiderExamStatus").Value.ToString();
                if (!marks.Element("ConsiderInResult").IsNull())
                    oMarkAssignment.ConsiderInResult = marks.Element("ConsiderInResult").Value.ToString();
                if (!marks.Element("ShowOnlyGrades").IsNull())
                    oMarkAssignment.ShowOnlyGrades = (marks.Element("ShowOnlyGrades").Value == Constants.S_ZERO || marks.Element("ShowOnlyGrades").Value == Constants.S_ONE) ? marks.Element("ShowOnlyGrades").Value == Constants.S_ONE : marks.Element("ShowOnlyGrades").Value.ToBool();
                if (!marks.Element("AllowDecimal").IsNull())
                    oMarkAssignment.AllowDecimal = marks.Element("AllowDecimal").Value.ToInt() == Constants.I_ZERO;
                if (!marks.Element("Grade").IsNull())
                    oMarkAssignment.Grade = marks.Element("Grade").Value.ToString();
                if (!marks.Element("TotalGrade").IsNull())
                    oMarkAssignment.TotalGrade = marks.Element("TotalGrade").Value.ToString();
                moStudentProgressReport.MarkAssignmentDetails.Add(oMarkAssignment);
            }
        );
    }

    /// <summary>
    /// This method is used to fill exam wise percentage details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillExamWisePercentageDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.ExamWisePercentageDetails = new List<ExamWisePercentage>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table4").ToList().ForEach(
            ewp =>
            {
                StudentWiseProgressReportExamWisePercentage oExamWisePercentage = new StudentWiseProgressReportExamWisePercentage();

                if (!ewp.Element("SchoolWise_Test_Id").IsNull())
                    oExamWisePercentage.SchoolWiseTestId = ewp.Element("SchoolWise_Test_Id").Value.ToInt();
                if (!ewp.Element("Total_Marks_Scored").IsNull())
                    oExamWisePercentage.TotalMarksScored = ewp.Element("Total_Marks_Scored").Value.ToDecimal();
                if (!ewp.Element("Subjects_Total_Marks").IsNull())
                    oExamWisePercentage.SubjectTotalMarks = ewp.Element("Subjects_Total_Marks").Value.ToInt();
                if (!ewp.Element("Percentage").IsNull())
                    oExamWisePercentage.Percentage = ewp.Element("Percentage").Value.ToDecimal();
                if (!ewp.Element("Grade_Name").IsNull())
                    oExamWisePercentage.Grade = ewp.Element("Grade_Name").Value.ToString();
                if (!ewp.Element("Grade_id").IsNull())
                    oExamWisePercentage.GradeId = ewp.Element("Grade_id").Value.ToInt();
                if (!ewp.Element("Result").IsNull())
                    oExamWisePercentage.Result = ewp.Element("Result").Value.ToString();
                if (!ewp.Element("rank").IsNull())
                    oExamWisePercentage.Rank = ewp.Element("rank").Value.ToInt();
                

                moStudentProgressReport.ExamWisePercentageDetails.Add(oExamWisePercentage);
            }
        );
    }

    /// <summary>
    /// This method is used to fill test wise subject group total details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillSubjectTestGroupTotalDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.SubjectTestGroupTotalDetails = new List<SubjectTestGroupTotal>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table5").ToList().ForEach(
            stgtd =>
            {
                StudentWiseProgressReportSubjectTestGroupTotal oSubjectTestGroupTotal = new StudentWiseProgressReportSubjectTestGroupTotal();
                if (!stgtd.Element("Test_Id").IsNull())
                    oSubjectTestGroupTotal.SchoolWiseTestId = stgtd.Element("Test_Id").Value.ToInt();
                if (!stgtd.Element("Total_Marks_Scored").IsNull())
                    oSubjectTestGroupTotal.TotalMarksScored = stgtd.Element("Total_Marks_Scored").Value.ToDecimal();
                if (!stgtd.Element("Parent_Subject_Id").IsNull())
                    oSubjectTestGroupTotal.ParentSubjectId = stgtd.Element("Parent_Subject_Id").Value.ToInt();
                if (!stgtd.Element("Parent_Subject_Name").IsNull())
                    oSubjectTestGroupTotal.ParentSubjectName = stgtd.Element("Parent_Subject_Name").Value.ToString();
                if (!stgtd.Element("ChildSubject_Marks_Total").IsNull())
                    oSubjectTestGroupTotal.ChildSubjectMarksTotal = stgtd.Element("ChildSubject_Marks_Total").Value.ToDecimal();
                if (!stgtd.Element("Grade").IsNull())
                    oSubjectTestGroupTotal.Grade = stgtd.Element("Grade").Value.ToString();
                if (!stgtd.Element("AverageMarks").IsNull())
                    oSubjectTestGroupTotal.AverageMarks = stgtd.Element("AverageMarks").Value.ToDecimal();
                if (!stgtd.Element("OutOfMarks").IsNull())
                    oSubjectTestGroupTotal.OutOfMarks = stgtd.Element("OutOfMarks").Value.ToDecimal();
                moStudentProgressReport.SubjectTestGroupTotalDetails.Add(oSubjectTestGroupTotal);
            }
        );
    }

    /// <summary>
    /// This method is used to fill test type wise subject group total details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillSubjectTestTypeGroupTotalDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.SubjectTestTypeGroupTotalDetails = new List<SubjectTestTypeGroupTotal>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table6").ToList().ForEach(
            sttgt =>
            {
                SubjectTestTypeGroupTotal oSubjectTestTypeGroupTotal = new SubjectTestTypeGroupTotal();
                if (!sttgt.Element("Test_Id").IsNull())
                    oSubjectTestTypeGroupTotal.SchoolWiseTestId = sttgt.Element("Test_Id").Value.ToInt();
                if (!sttgt.Element("TestType_Id").IsNull())
                    oSubjectTestTypeGroupTotal.TestTypeId = sttgt.Element("TestType_Id").Value.ToInt();
                if (!sttgt.Element("TestTypeSort_Order").IsNull())
                    oSubjectTestTypeGroupTotal.TestTypeSortOrder = sttgt.Element("TestTypeSort_Order").Value.ToInt();
                if (!sttgt.Element("Parent_Subject_Id").IsNull())
                    oSubjectTestTypeGroupTotal.ParentSubjectId = sttgt.Element("Parent_Subject_Id").Value.ToInt();
                if (!sttgt.Element("TestType_Total_Marks_Scored").IsNull())
                    oSubjectTestTypeGroupTotal.TestTypeTotalMarksScored = sttgt.Element("TestType_Total_Marks_Scored").Value.ToDecimal();
                if (!sttgt.Element("TestType_Total_Marks").IsNull())
                    oSubjectTestTypeGroupTotal.TestTypeTotalMarks = sttgt.Element("TestType_Total_Marks").Value.ToDecimal();
                if (!sttgt.Element("Grade").IsNull())
                    oSubjectTestTypeGroupTotal.Grade = sttgt.Element("Grade").Value.ToString();

                moStudentProgressReport.SubjectTestTypeGroupTotalDetails.Add(oSubjectTestTypeGroupTotal);
            }
        );
    }

    /// <summary>
    /// This method is used to fill subject wise test type details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillSubjectTestTypeDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.SubjectTestTypeDetails = new List<SubjectTestType>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table7").ToList().ForEach(
            sttd =>
            {
                SubjectTestType oSubjectTestType = new SubjectTestType();
                if (!sttd.Element("Subject_Id").IsNull())
                    oSubjectTestType.SubjectId = sttd.Element("Subject_Id").Value.ToInt();
                if (!sttd.Element("TestType_Id").IsNull())
                    oSubjectTestType.TestTypeId = sttd.Element("TestType_Id").Value.ToInt();
                if (!sttd.Element("ShortenTestType_Name").IsNull())
                    oSubjectTestType.ShortenTestTypeName = sttd.Element("ShortenTestType_Name").Value.ToString();
                if (!sttd.Element("Total_Marks_Scored").IsNull())
                    oSubjectTestType.TotalMarksScored = sttd.Element("Total_Marks_Scored").Value.ToDecimal();
                if (!sttd.Element("TestTypeSort_Order").IsNull())
                    oSubjectTestType.TestTypeSortOrder = sttd.Element("TestTypeSort_Order").Value.ToInt();

                moStudentProgressReport.SubjectTestTypeDetails.Add(oSubjectTestType);
            }
        );
    }

    /// <summary>
    /// This method is used to fill test type details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillTestTypeDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.TestTypeDetails = new List<TestType>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table8").ToList().ForEach(
            tt =>
            {
                TestType oTestType = new TestType();
                if (!tt.Element("TestType_Name").IsNull())
                    oTestType.TestTypeName = tt.Element("TestType_Name").Value.ToString();
                if (!tt.Element("TestType_Id").IsNull())
                    oTestType.TestTypeId = tt.Element("TestType_Id").Value.ToInt();
                if (!tt.Element("ShortenTestType_Name").IsNull())
                    oTestType.ShortenTestTypeName = tt.Element("ShortenTestType_Name").Value.ToString();
                if (!tt.Element("TestTypeSort_Order").IsNull())
                    oTestType.TestTypeSortOrder = tt.Element("TestTypeSort_Order").Value.ToInt();

                moStudentProgressReport.TestTypeDetails.Add(oTestType);
            }
        );
    }

    /// <summary>
    /// This method is used to fill grde details from XML.
    /// </summary>
    /// <param name="asXmlData"></param>
    private void FillGradeDetailsFromXML(string asXmlData)
    {
        moStudentProgressReport.GradeDetails = new List<Grade>();
        XDocument xmldoc = XDocument.Parse(asXmlData);
        xmldoc.Elements("NewDataSet").Elements("Table9").ToList().ForEach(
            grd =>
            {
                Grade oGrade = new Grade();
                if (!grd.Element("Grade_Name").IsNull())
                    oGrade.GradeName = grd.Element("Grade_Name").Value.ToString();
                if (!grd.Element("Marks_Grades_Configuration_Detail_ID").IsNull())
                    oGrade.GradeId = grd.Element("Marks_Grades_Configuration_Detail_ID").Value.ToInt();
                if (!grd.Element("Remarks").IsNull())
                    oGrade.Remarks = grd.Element("Remarks").Value.ToString();
                moStudentProgressReport.GradeDetails.Add(oGrade);
            }
        );
    }

    /// <summary>
    /// This methoid is used to create progress sheet
    /// </summary>
    private void CreateProgressReport()
    {
        mbIsApplicable = CheckThatIsRankApplicable();
        if (menmPagemode != Constants.PageMode.Edit)
        {
            bShowOnlyGradesInProgressSheet = moStudentProgressReport.StudentDetails.ShowOnlyGrades;
            if (mbStudentwiseProgressReport)
                bIsGradesStandard = ((StudentWiseProgressReportStudentDetails)moStudentProgressReport.StudentDetails).IsGradesStandard;
        }

        CreateStudentInfo();
        tblProgress = new HtmlTable();
        tblProgress.ID = "tbl_" + menumResultType + moStudentProgressReport.StudentDetails.YearWiseStudentId;
        tblProgress.EnableViewState = false;
        tblProgress.CellPadding = 0;
        tblProgress.CellSpacing = 1;
        tblProgress.Style.Add(HtmlTextWriterStyle.VerticalAlign, HtmlTextWriterStyle.Top.ToString());
        tblProgress.Style.Add(HtmlTextWriterStyle.Left, Unit.Pixel(0).ToString());
        if (menmPagemode != Constants.PageMode.Print)
        {
            tblProgress.Border = 0;
            tblProgress.Width = "100%";
            tblProgress.Attributes.Add("class", S_CSS_PRINT_PREFIX + "ReportOuter");
            Panel oPanel = new Panel();
            if (mbStudentwiseProgressReport)
                oPanel.ID = "pnlOuter";
            oPanel.ScrollBars = ScrollBars.Horizontal;
            //oPanel.Width = Unit.Pixel(842);
            oPanel.Width = Unit.Percentage(100);
            oPanel.Controls.Add(tblProgress);
            grdvwScrollContainer.Controls.Add(oPanel);
            oPanel.Dispose();
        }
        else
        {
            grdvwScrollContainer.Controls.Add(tblProgress);
            tblProgress.Width = "100%";
            tblProgress.Border = 1;
        }

        CreateTableHeaderRow();
        CreateExamsAndTotalBlankRows();
    }

    /// <summary>
    /// This method is used to create progress report header
    /// </summary>
    /// <param name="aoHeaderHtmlTable"></param>
    private void CreateHdProgressCard(HtmlTable aoHeaderHtmlTable)
    {
        HtmlTableRow oHtmlTableRow = new HtmlTableRow();
        CreateHtmlCell(oHtmlTableRow, "Progress Report", S_CSS_PRINT_PREFIX + "ClsReportHead", 1, 8);
        aoHeaderHtmlTable.Rows.Add(oHtmlTableRow);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// Get total exam count of all child subject of group subject.
    /// </summary>
    /// <param name="aolstSubject"></param>
    /// <returns></returns>
    private int GetTotalExamTypeCountOfAllChildSubjects(List<Subject> aolstSubject)
    {
        int iTotalCnt = 0;
        int iexmCnt = 0;

        aolstSubject.ForEach(
            subject =>
            {
                iexmCnt = GetExamTypeCount(subject.SubjectId);
                iTotalCnt += iexmCnt;
            }
        );

        return iTotalCnt;
    }

    /// <summary>
    /// Get count of unique test types all childs of given group subject.
    /// </summary>
    /// <param name="aolstSubject"></param>
    /// <returns></returns>
    private int GetMaxExamTypeCountOfAllChildSubjects(List<Subject> aolstSubject)
    {
        List<int> oTestTypesList = new List<int>();
        aolstSubject.ForEach(
            subject =>
            {
                moStudentProgressReport.SubjectTestTypeDetails.Where(sttd => sttd.SubjectId == subject.SubjectId).ToList().ForEach(
                    sttd =>
                        {
                            if (!oTestTypesList.Contains(sttd.TestTypeId))
                                oTestTypesList.Add(sttd.TestTypeId);
                        }
                );
            }
        );

        return oTestTypesList.Count;
    }

    /// <summary>
    /// This method is used to create row cells for subject group totals header.
    /// </summary>
    /// <param name="otrHeader"></param>
    private void CreateRowTotalHeaderCells(HtmlTableRow otrHeader)
    {
        int colspan = 0;

        // If this the header then create header HTML cells
        if (bShortPrintEnabled && menmPagemode == Constants.PageMode.Print)
            colspan = 1;
        else
            colspan = miTotalCellColSpan;

        if (!bShowOnlyGradesInProgressSheet)
        {
            CreateHtmlCell(otrHeader, S_COL_TOTAL, " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);
            CreateHtmlCell(otrHeader, "%", S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);
        }
        if (!IsTotalConsiderForProgressReport())
            CreateHtmlCellForTotalInHaderForGrade(otrHeader, S_DB_COL_GRADE, S_CSS_CLSPADDING + " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);
        else
            CreateHtmlCell(otrHeader, S_DB_COL_GRADE, S_CSS_CLSPADDING + " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);

        if (mbIsFailCriteriaNotApplicable)
            CreateHtmlCell(otrHeader, "Result", S_CSS_CLSPADDING + " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);

        // If rank is applicable for this sudent then show rank column.
        if (!bShowOnlyGradesInProgressSheet && !bIsGradesStandard)
            if (mbIsApplicable)
            {
                CreateHtmlCell(otrHeader, "Rank", S_CSS_CLSPADDING + " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);
                HideRankColumnCell(otrHeader.Cells[otrHeader.Cells.Count - 1]);
            }
        if (mbStudentwiseProgressReport)
            CreateHtmlCell(otrHeader, "Select", S_CSS_CLSPADDING + " " + S_CSS_PRINT_PREFIX + "TotalHead", colspan, 1);
        otrHeader.Dispose();
    }

    /// <summary>
    /// This method use to hide grade in final total if istotal consider is false
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    /// <param name="sInnerText"></param>
    /// <param name="sClassName"></param>
    /// <param name="iRowSpan"></param>
    /// <param name="iColSpan"></param>
 
    protected void CreateHtmlCellForTotalInHaderForGrade(HtmlTableRow oHtmlTableRow, string sInnerText, string sClassName, int iRowSpan, int iColSpan)
    { 
        HtmlTableCell oHtmlTableCell = new HtmlTableCell();
        oHtmlTableCell.InnerHtml = sInnerText;
        oHtmlTableCell.Attributes.Add("rowspan", iRowSpan.ToString());
        oHtmlTableCell.Attributes.Add("colspan", iColSpan.ToString());
        oHtmlTableCell.Attributes.Add("class", sClassName);
        oHtmlTableCell.Align = "center";
        
        if (menmPagemode == Constants.PageMode.Print)
        {
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderWidth, "1px");
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderStyle, "solid");
            oHtmlTableCell.Style.Add(HtmlTextWriterStyle.BorderColor, "black");
            oHtmlTableCell.NoWrap = false;

            if (moStudentProgressReport.SubjectDetails.Count(subject => subject.SubjectName == sInnerText) > 0)
                oHtmlTableCell.Width = Unit.Pixel(200).ToString();
        }
        else
            oHtmlTableCell.NoWrap = true;
        oHtmlTableRow.Cells.Add(oHtmlTableCell);
        oHtmlTableCell.Attributes.Add("style", "display:none");

        CheckDSKStatus(oHtmlTableCell);

        oHtmlTableCell.Dispose();
    }
   
    
    /// <summary>
    /// Hides Rank column text while preserving cell layout and CellSpacing grid lines.
    /// Only inner content is hidden; td styles/classes must not be changed.
    /// </summary>
    /// <param name="aoHtmlTableCell"></param>
    private void HideRankColumnCell(HtmlTableCell aoHtmlTableCell)
    {
        if (Settings.ShowRankColumn || aoHtmlTableCell == null)
            return;

        aoHtmlTableCell.Attributes.Add("style", "display:none");
    }

    /// <summary>
    /// Returns true when Rank column should be rendered with values.
    /// </summary>
    private bool ShouldShowRankColumn()
    {
        return Settings.ShowRankColumn && mbIsApplicable;
    }

    /// <summary>
    /// Check if rank is applicable for progress report or not.
    /// </summary>
    /// <returns></returns>
    private bool CheckThatIsRankApplicable()
    {
        return moStudentProgressReport.ExamWisePercentageDetails.Count(ewpd => ewpd.Rank != Constants.I_ZERO && ewpd.Rank <= Settings.ToppersCount && ewpd.Percentage != -99) > 0;
    }

    /// <summary>
    /// This method is used to create row cells for subject group marks totals .
    /// </summary>
    /// <param name="oHtmlTableRow"></param>
    private void CreateRowTotalCells(HtmlTableRow oHtmlTableRow)
    {
        string sTempValue = S_NON_APPLICABLE;
        if (menmPagemode == Constants.PageMode.Print)
            sTempValue = " ";

        // If this the header then create plane HTML cells        
        if (!bShowOnlyGradesInProgressSheet)
        {
            CreateHtmlCell(oHtmlTableRow, sTempValue, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
            CreateHtmlCell(oHtmlTableRow, sTempValue, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
        }

        CreateHtmlCell(oHtmlTableRow, sTempValue, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
        if (mbIsFailCriteriaNotApplicable)
            CreateHtmlCell(oHtmlTableRow, sTempValue, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
        if (!bShowOnlyGradesInProgressSheet)
            if (mbIsApplicable)
            {
                CreateHtmlCell(oHtmlTableRow, sTempValue, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
                HideRankColumnCell(oHtmlTableRow.Cells[oHtmlTableRow.Cells.Count - 1]);
            }
        if (mbStudentwiseProgressReport)
            CreateHtmlCell(oHtmlTableRow, string.Empty, S_CSS_PRINT_PREFIX + S_CSS_CLSTOTALMARKSCELL, 1, 1);
        oHtmlTableRow.Dispose();
    }

    /// <summary>
    /// This method is used to SetGrades
    /// </summary>
    private void SetGrades()
    {
        string sGradeSeparator = string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        moStudentProgressReport.GradeDetails.Cast<StudentWiseProgressReportGrade>().ToList().ForEach(
            grd =>
            {
                oStringBuilder.Append(sGradeSeparator);
                oStringBuilder.Append(grd.GradeName);
                oStringBuilder.Append(":");
                oStringBuilder.Append(grd.StartingMarksRange);
                oStringBuilder.Append(":");
                oStringBuilder.Append(grd.ActualEndingMarksRange);
                oStringBuilder.Append(":");
                oStringBuilder.Append(grd.Remarks);
                sGradeSeparator = "#";
            }
        );

        msGradeDetails = oStringBuilder.ToString();
    }

    /// <summary>
    /// This method is used to set exam status consideration details in string.
    /// </summary>
    private void SetExamStatusConsiderationInTotal()
    {
        string sSeparator = string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        moStudentProgressReport.ExamStatusDetails.ForEach(
            esd =>
            {
                oStringBuilder.Append(sSeparator);
                oStringBuilder.Append(esd.ShortName);
                oStringBuilder.Append(":");
                oStringBuilder.Append(esd.ConsiderInTotal);
                sSeparator = "#";
            }
        );

        msExamStatusDetails = oStringBuilder.ToString();
    }

    /// <summary>
    /// This method is used to fill exam status details.
    /// </summary>
    /// <param name="aoDTExamStatus"></param>
    private void FillExamStatusList(DataTable aoDTExamStatus)
    {
        mlstExamStatusDetails = new List<ExamStatus>();
        foreach (DataRow oDRExamStatus in aoDTExamStatus.Rows)
        {
            ExamStatus oExamStatus = new ExamStatus();
            if (oDRExamStatus["DisplayName"] != DBNull.Value)
                oExamStatus.DisplayName = oDRExamStatus["DisplayName"].ToString();
            if (oDRExamStatus["DisplayValue"] != DBNull.Value)
                oExamStatus.DisplayValue = oDRExamStatus["DisplayValue"].ToString();
            if (oDRExamStatus["ShortName"] != DBNull.Value)
                oExamStatus.ShortName = oDRExamStatus["ShortName"].ToString();
            if (oDRExamStatus["ForeColor"] != DBNull.Value)
                oExamStatus.ForeColor = oDRExamStatus["ForeColor"].ToString();
            if (oDRExamStatus["BackColor"] != DBNull.Value)
                oExamStatus.BackColor = oDRExamStatus["BackColor"].ToString();
            mlstExamStatusDetails.Add(oExamStatus);
        }
    }

    protected bool IsTotalConsiderForProgressReport()
    {
        bool bShowTotal = SchoolBase.Settings.IsTotalConsiderForProgressReport;
        if (bShowTotal)
        {
            if (miSchoolId == Constants.SchoolId.JOS.ToInt() && moStudentProgressReport.StudentDetails.StandardDivisionDetails.IsPreprimaryStandard)
                bShowTotal = false;
        }
        return bShowTotal;
    }

    #endregion
}