/* File Name ;- TestMarksConfigurationUI.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 26-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- 
 *  This class diisplays the status (and appropriate links) of marks entry for students in teacher's  class.
 *  Admin user can select a teacher and the status for selected exam is diplayed.
 *  Teacher user can view status for selected exam is diplayed.
 *  Status-links are as follows;
 *  1.No student in class
 *  2.Marks entry not started
 *  3.Marks entry partially done
 *  4.Marks entry Completed
 *  5.If exam is not yet configured.
 *  The admin user is given the link for test configuration
 *  Teacher just gets the status message of "exam not configured".
*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TestMarksConfigurationUI : SchoolBase
{
    #region Constants

    private const string S_LNK_S_NOT_CONFIGURE = "Not Configured";

    private const string S_IMG_FOR_NONE_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif";
    private const string S_IMG_FOR_PARTIAL_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif";
    private const string S_IMG_FOR_COMPLETE_CONFIGURATION = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
    private const string S_IMG_FOR_EXAM_DATE_CONFIGURATION = "~/RITeSchool/images/GridIcon_ExamDateNC.gif";
    private const string S_CSS_CLASS_NOT_APPLICABLE = "ClsGridNA";
    private const string S_DEFAULT = "0";
    private const string S_DATAKEY_STATUS = "Status";
    private const string S_DATAKEY_SUBMIT = "Is_Submitted";
    private const string S_DATAKEY_STANDARD_DIVISION = "Standard_Division_Id";
    private const string S_DATAKEY_STANDARD_ID = "Standard_Id";
    private const string S_DATAKEY_SUBJECT_ID = "Subject_Id";
    private const string S_ALLOW_PARTIAL_SUBMIT = "AllowPartialSubmit";
    private const string S_CLASS_DATA = "CLASSDATA";

    #endregion

    #region Data Members

    private int miTeacherId;
    private int miStandardDivisionId;

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill test and teacher combobox by checking user role. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (CheckUserRolesAndSetDisplay())
            {
                if (!IsPostBack)
                {
                    FillTeachersComboBox();
                    GetQueryString();
                    FillTeachersClassComboBox();
                    FillTestCombobox();                    
                    grdSubjects.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
                    grdMyClassSubjects.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
                    if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    {
                        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    }
                    SetOwnClassVisibility(true);
                    RefreshValues();                   
                }

                FillExamMarksStatusGrid();
                
                if (IsPostBack)
                    SetOwnClassVisibility(false);

                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString(); 
                    RefreshValues();
                }
            }
            else
                HideControls();
            cmbExams.Focus();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// Through this event handler assigns appropriate links to the "Edit" and "Submit" column of the grid.
    /// different situations and resp. links
    /// 1.If exam is not configured : test configuration link 
    /// 2.If 1 is false, and exam dates are not configured : exam schedule link
    /// 3.If exam structure and dates are configured, 
    /// 4.if any of above all conditions is not satified    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                int iRowIndex = e.Row.RowIndex;
                const int I_CELL_INDEX = 2;
                int iStandardDivisionId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex][S_DATAKEY_STANDARD_DIVISION].ToString());
                int iStandardId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex][S_DATAKEY_STANDARD_ID].ToString());
                int iSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBJECT_ID].ToString());
                bool bIsMonthConfig = Convert.ToBoolean(grdSubjects.DataKeys[iRowIndex]["Is_MonthConfig"].ToString());
                int iTestId = Convert.ToInt32(cmbExams.SelectedValue);
                string sAllowPartialSubmit = grdSubjects.DataKeys[iRowIndex][S_ALLOW_PARTIAL_SUBMIT].ToString();

                string sStatus = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_STATUS].ToString();
                string sSubmitTooltip = Resources.LocalizedResources.MarksNotSubmitted;
                string sQueryString = CreateQueryString(iStandardDivisionId, iSubjectId, iStandardId, bIsMonthConfig);
                bool bIsSubmitted = true;

                if (grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals("N"))
                    bIsSubmitted = false;

                switch (sStatus)
                {
                    case "Not Started":
                        AddLinkForNoConfiguration(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdSubjects.DataKeys[e.Row.RowIndex][3].ToString()) == -1)
                        {
                            sSubmitTooltip = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_YES.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadySubmitted : Resources.LocalizedResources.ProgressReportEntryNotStarted;
                        }

                        break;
                    case "Partial":
                        AddLinkForPartialConfiguration(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdSubjects.DataKeys[e.Row.RowIndex][3].ToString()) == -1)
                        {
                            sSubmitTooltip = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_YES.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadySubmitted : Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
                        }

                        break;
                    case "Complete":
                    case "Submitted":
                        AddLinkForCompleteConfiguration(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdSubjects.DataKeys[e.Row.RowIndex][3].ToString()) != -1)
                        {
                            sSubmitTooltip = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.SubmitMarksToClass : Resources.LocalizedResources.MarksAlreadySubmitted;
                        }
                        else
                        {
                            sSubmitTooltip = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.SubmitProgressReportToClassTeacher : Resources.LocalizedResources.ProgressReportAlreadySubmitted;
                        }

                        break;
                    case "Published":
                        AddLinkForPublishedConfiguration(e.Row, I_CELL_INDEX, sQueryString);
                        sSubmitTooltip = grdSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadyPublished : Resources.LocalizedResources.ProgressReportAlreadyPublished;
                        break;
                    case "Test dates":
                        AddLinkForTestDateConfiguration(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        break;
                    case "N/A":
                        e.Row.Cells[2].CssClass = S_CSS_CLASS_NOT_APPLICABLE;
                        e.Row.Cells[2].Text = "N/A";
                        break;
                    case "No Student":
                        e.Row.Cells[2].CssClass = S_CSS_CLASS_NOT_APPLICABLE;
                        break;
                }

                AddSubmitMarksToClassTeacherLink(e.Row, I_CELL_INDEX + 1, iStandardDivisionId, iSubjectId, iTestId, sSubmitTooltip, sAllowPartialSubmit, sQueryString, bIsMonthConfig);
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// Through this event handler assigns appropriate links to the "Edit" and "Submit" column of the grid.
    /// different situations and resp. links
    /// 1.If exam is not configured : test configuration link 
    /// 2.If 1 is false, and exam dates are not configured : exam schedule link
    /// 3.If exam structure and dates are configured, 
    /// 4.if any of above all conditions is not satified    
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdMyClassSubjects_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                int iRowIndex = e.Row.RowIndex;
                const int I_CELL_INDEX = 2;
                int iStandardDivisionId = Convert.ToInt32(grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_STANDARD_DIVISION].ToString());
                int iStandardId = Convert.ToInt32(grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_STANDARD_ID].ToString());
                int iSubjectId = Convert.ToInt32(grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBJECT_ID].ToString());
                bool bIsMonthConfig = Convert.ToBoolean(grdMyClassSubjects.DataKeys[iRowIndex]["Is_MonthConfig"].ToString());
                int iTestId = Convert.ToInt32(cmbExams.SelectedValue);
                string sAllowPartialSubmit = grdMyClassSubjects.DataKeys[iRowIndex][S_ALLOW_PARTIAL_SUBMIT].ToString();

                string sStatus = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_STATUS].ToString();
                string sSubmitTooltip = Resources.LocalizedResources.MarksNotSubmitted;
                string sQueryString = CreateQueryString(iStandardDivisionId, iSubjectId, iStandardId, bIsMonthConfig);
                bool bIsSubmitted = true;

                if (grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals("N"))
                    bIsSubmitted = false;

                switch (sStatus)
                {
                    case "Not Started":
                        AddLinkForNoConfigurationForClassTeacher(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[e.Row.RowIndex][3].ToString()) == -1)
                        {
                            sSubmitTooltip = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_YES.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadySubmitted : Resources.LocalizedResources.ProgressReportEntryNotStarted;
                        }
                        break;
                    case "Partial":
                        AddLinkForPartialConfigurationForClassTeacher(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[e.Row.RowIndex][3].ToString()) == -1)
                        {
                            sSubmitTooltip = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_YES.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadySubmitted : Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
                        }
                        break;
                    case "Complete":
                    case "Submitted":
                        AddLinkForCompleteConfigurationForClassTeacher(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[e.Row.RowIndex][3].ToString()) != -1)
                        {
                            sSubmitTooltip = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.SubmitMarksToClass : Resources.LocalizedResources.MarksAlreadySubmitted;
                        }
                        else
                        {
                            sSubmitTooltip = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.SubmitProgressReportToClassTeacher : Resources.LocalizedResources.ProgressReportAlreadySubmitted;
                        }

                        break;
                    case "Published":
                        AddLinkForPublishedConfiguration(e.Row, I_CELL_INDEX, sQueryString);
                        sSubmitTooltip = grdMyClassSubjects.DataKeys[iRowIndex][S_DATAKEY_SUBMIT].ToString().Equals(Constants.C_NO.ToString()) ? Resources.LocalizedResources.ProgressReportAlreadyPublished : Resources.LocalizedResources.ProgressReportAlreadyPublished;
                        break;
                    case "Test dates":
                        AddLinkForTestDateConfiguration(e.Row, I_CELL_INDEX, bIsSubmitted, sQueryString);
                        break;
                    case "N/A":
                        e.Row.Cells[2].CssClass = S_CSS_CLASS_NOT_APPLICABLE;
                        e.Row.Cells[2].Text = "N/A";
                        break;
                    case "No Student":
                        e.Row.Cells[2].CssClass = S_CSS_CLASS_NOT_APPLICABLE;
                        break;
                }

                AddSubmitMarksToClassTeacherLink(e.Row, I_CELL_INDEX + 1, iStandardDivisionId, iSubjectId, iTestId, sSubmitTooltip, sAllowPartialSubmit, sQueryString, bIsMonthConfig);
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to hide grid if teacher is not selected.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            FillTeachersClassComboBox();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired while class combo box selection changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            miStandardDivisionId = cmbClass.SelectedValue.ToInt();
            FillTestCombobox();
            FillExamMarksStatusGrid();            
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event will fired while Exam combo box selection changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbExams_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExamMarksStatusGrid();
            //SetOwnClassVisibility(false);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method  is used to fill test combobox.
    /// </summary>
    private void FillTestCombobox()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtAllTests = oTestCollectionBL.GetAllTestsForClass(cmbClass.SelectedValue.ToInt());

        if (cmbExams.SelectedValue != Constants.S_ZERO)
            hidOldExamId.Value = cmbExams.SelectedValue;

        ControlUtility.FillDropDownList(
                       oDtAllTests,
                       ref cmbExams,
                       Constants.S_TEST_ID_FIELD,
                       Constants.S_TEST_NAME_FIELD,
                       string.Empty);

        ListItem oListItem = cmbExams.Items.FindByValue(hidOldExamId.Value);

        if (oListItem != null)
        {
            oListItem.Selected = true;
            FillExamMarksStatusGrid();
            //SetOwnClassVisibility(false);
        }
        else
            hidOldExamId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to fill the standard- divisions and subjects grid.
    /// </summary>
    private void FillExamMarksStatusGrid()
    {
        if (CheckPreCondition())
        {

            grdSubjects.Visible = true;
            SubjectTestConfigurationCollectionBL oTestConfigCollection = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
            DataTable oDtAllStdandardDivisions = oTestConfigCollection.FetchTestsConfigurationForMySubjects(miTeacherId, Convert.ToInt32(cmbExams.SelectedValue), Settings.AllowPartialSubmit ? Constants.S_YES : Constants.S_NO, miStandardDivisionId == 0 ? Convert.ToInt32(cmbClass.SelectedValue) : miStandardDivisionId);
            grdSubjects.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;

            PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
            DataTable oDataTable = oPrePrimaryProgressSheetConfigBL.GetIncompleteProgressRollNos(miSchoolId, miAcademicYearId, miTeacherId, Convert.ToInt32(cmbExams.SelectedValue));
            GenerateIncompleteProgressAlert(oDataTable);

            if (cmbTeachers.SelectedIndex == 0 && (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor))
            {
                spnMySubject.Visible = false;
                grdSubjects.Visible = false;
            }
            else
            {
                spnMySubject.Visible = true;
                grdSubjects.Visible = true;
            }

            grdSubjects.DataSource = oDtAllStdandardDivisions;
            grdSubjects.DataBind();

            if (Settings.AllowUnsubmitExamMarks && grdSubjects.Rows.Count > 0)
            {
                grdSubjects.Columns[3].HeaderText = "Submit / Un-Submit";
            }
        }
    }

    /// <summary>
    /// This method is used to fill Class Teachers Subject Listview.
    /// </summary>
    private void FillMyClassExamStatusGrid()
    {
        if (CheckPreCondition())
        {
            if (miTeacherId != 0 && cmbClass.SelectedValue != Constants.S_ZERO)
            {
                DataTable oDtTeacherSubjects;
                if (ViewState[S_CLASS_DATA] == null)
                {
                    TeacherSubjectAssignmentBL oTeacherSubjectAssignmentBL = new TeacherSubjectAssignmentBL();
                    oDtTeacherSubjects = oTeacherSubjectAssignmentBL.GetTeacherSubjectDetails(miSchoolId, miAcademicYearId);
                    DataTable oDT = oDtTeacherSubjects.Select("Is_ClassTeacher = 'Y'").CopyToDataTable();
                    ViewState[S_CLASS_DATA] = oDT;
                }
                else
                    oDtTeacherSubjects = ViewState[S_CLASS_DATA] as DataTable;

                DataRow[] drArray = oDtTeacherSubjects.Select("Is_ClassTeacher = 'Y' AND Teacher_Id=" + miTeacherId + "AND Standard_Division_Id=" + cmbClass.SelectedValue.ToInt());
                if (drArray.Length > 0)
                    miStandardDivisionId = drArray[0]["Standard_Division_Id"].ToInt();
                else
                    miStandardDivisionId = 0;

                if (miStandardDivisionId != 0)
                {
                    spnMyClassSubjects.Visible = true;
                    grdMyClassSubjects.Visible = true;

                    SubjectTestConfigurationCollectionBL oTestConfigCollection = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
                    DataTable oDtAllStdandardDivisions = oTestConfigCollection.FetchTestsConfigurationForMyClass(miTeacherId, Convert.ToInt32(cmbExams.SelectedValue), Settings.AllowPartialSubmit ? Constants.S_YES : Constants.S_NO, miStandardDivisionId == 0 ? Convert.ToInt32(cmbClass.SelectedValue) : miStandardDivisionId);

                    PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                    DataTable oDataTable = oPrePrimaryProgressSheetConfigBL.GetIncompleteProgressRollNos(miSchoolId, miAcademicYearId, miTeacherId, Convert.ToInt32(cmbExams.SelectedValue));
                    GenerateIncompleteProgressAlert(oDataTable);

                    if (oDtAllStdandardDivisions.Rows.Count > 0)
                    {
                        grdMyClassSubjects.DataSource = oDtAllStdandardDivisions;
                        grdMyClassSubjects.DataBind();
                    }
                    else
                    {
                        grdMyClassSubjects.EmptyDataText = Resources.LocalizedResources.NoRecordsFound;
                    }
                }
                else
                {
                    spnMyClassSubjects.Visible = false;
                    grdMyClassSubjects.Visible = false;
                }
            }
        }
    }
    
    /// <summary>
    /// This method is used to generate incompleate progress alert.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private void GenerateIncompleteProgressAlert(DataTable aoDataTable)
    {
        string sAlert = string.Empty;
        if (aoDataTable != null)
        {
            foreach (DataRow oDataRow in aoDataTable.Rows)
            {
                sAlert = sAlert + Resources.LocalizedResources.ProgressReportEntryForRollNo.Replace("%replace%", Convert.ToString(oDataRow["Status"])) + Convert.ToString(oDataRow["RollNos"]) + " \n";
               // sAlert = sAlert + "Progress report entry is " + Convert.ToString(oDataRow["Status"]) + " for\n";
               // sAlert = sAlert + "Roll Nos : " + Convert.ToString(oDataRow["RollNos"]) + " \n";
            }
        }

        hidAlert.Value = sAlert;
    }


    /// <summary>
    /// This function is used to create query string.
    /// </summary>
    private string CreateQueryString(int aiStandardDivisionId, int aiSubjectId, int aiStandardId, bool abIsMonthConfig)
    {
        string sQuerystring = "StandardDivisionId=" + aiStandardDivisionId;
        sQuerystring = sQuerystring + "&SubjectId=" + aiSubjectId;
        sQuerystring = sQuerystring + "&TestId=" + cmbExams.SelectedValue;
        sQuerystring = sQuerystring + "&TeacherId=" + cmbTeachers.SelectedValue;
        sQuerystring = sQuerystring + "&StandardId=" + aiStandardId;
        sQuerystring = sQuerystring + "&IsMonthConfig=" + abIsMonthConfig;
        sQuerystring = sQuerystring + "&SelectedStandardDivisionId=" + cmbClass.SelectedValue;
        return sQuerystring;
    }

    #region Add link functions

    /// <summary>
    /// This method is used to add hyperlink to the table cell where the testdate configuration 
    /// is not done
    /// </summary>
    private void AddLinkForTestDateConfiguration(GridViewRow aaoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor)
        {
            HyperLink oHyperLink = new HyperLink
                                       {
                                           Text = S_LNK_S_NOT_CONFIGURE,
                                           ForeColor = System.Drawing.Color.Black
                                       };
            oHyperLink.Font.Bold = true;
            oHyperLink.BackColor = System.Drawing.Color.FromArgb(253, 252, 178);
            oHyperLink.ImageUrl = S_IMG_FOR_EXAM_DATE_CONFIGURATION;

            if (abIsSubmitted)
                asQuerystring = asQuerystring + "&IsReadOnly=True";
            else
                asQuerystring = asQuerystring + "&IsReadOnly=False";

            string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);
            oHyperLink.NavigateUrl = "~/RITeSchool/Admin/StandardExamScheduleConfigurationUI.aspx" + "?" + sEncrypt;
            oHyperLink.Text = Resources.LocalizedResources.ExamDatesNotConfigured;
            oHyperLink.ToolTip = Resources.LocalizedResources.ExamDatesNotConfigured;
            aaoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
        }
        else
            aaoGridViewRow.Cells[aiCellIndex].Text = Resources.LocalizedResources.ExamDatesNotConfigured;
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell where we have to
    /// assign teacher.
    /// </summary>
    private void AddLinkForNoConfiguration(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink { Text = S_LNK_S_NOT_CONFIGURE, ForeColor = System.Drawing.Color.Black };
        oHyperLink.Font.Bold = true;
        oHyperLink.BackColor = System.Drawing.Color.FromArgb(253, 252, 178);
        oHyperLink.ImageUrl = S_IMG_FOR_NONE_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);
        if (Convert.ToInt32(grdSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryNotStarted;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryNotStarted;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryNotStarted;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryNotStarted;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryNotStarted;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to add hyperlink to the table cell where we have to
    /// assign teacher.
    /// </summary>
    private void AddLinkForNoConfigurationForClassTeacher(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink { Text = S_LNK_S_NOT_CONFIGURE, ForeColor = System.Drawing.Color.Black };
        oHyperLink.Font.Bold = true;
        oHyperLink.BackColor = System.Drawing.Color.FromArgb(253, 252, 178);
        oHyperLink.ImageUrl = S_IMG_FOR_NONE_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);
        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryNotStarted;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryNotStarted;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryNotStarted;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryNotStarted;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryNotStarted;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher
    /// or add new teacher.
    /// </summary>
    private void AddLinkForPartialConfiguration(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink { ForeColor = System.Drawing.Color.White };
        oHyperLink.Font.Bold = true;
        oHyperLink.ImageUrl = S_IMG_FOR_PARTIAL_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);

        if (Convert.ToInt32(grdSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryPartiallyDone;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryPartiallyDone;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher
    /// or add new teacher.
    /// </summary>
    private void AddLinkForPartialConfigurationForClassTeacher(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink { ForeColor = System.Drawing.Color.White };
        oHyperLink.Font.Bold = true;
        oHyperLink.ImageUrl = S_IMG_FOR_PARTIAL_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);

        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryPartiallyDone;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryPartiallyDone;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryPartiallyDone;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher
    /// or add new teacher.
    /// </summary>    
    private void AddLinkForCompleteConfigurationForClassTeacher(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink();
        oHyperLink.ForeColor = System.Drawing.Color.White;
        oHyperLink.Font.Bold = true;
        oHyperLink.ImageUrl = S_IMG_FOR_COMPLETE_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);

        if (Convert.ToInt32(grdMyClassSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryCompleted;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryCompleted;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryCompleted;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryCompleted;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to provide link to remove assignment of teacher
    /// or add new teacher.
    /// </summary>    
    private void AddLinkForCompleteConfiguration(GridViewRow aoGridViewRow, int aiCellIndex, bool abIsSubmitted, string asQuerystring)
    {
        HyperLink oHyperLink = new HyperLink();
        oHyperLink.ForeColor = System.Drawing.Color.White;
        oHyperLink.Font.Bold = true;
        oHyperLink.ImageUrl = S_IMG_FOR_COMPLETE_CONFIGURATION;

        if (abIsSubmitted)
            asQuerystring = asQuerystring + "&IsReadOnly=True";
        else
            asQuerystring = asQuerystring + "&IsReadOnly=False";

        string sEncrypt = CommonUtility.EncryptQuerystring(asQuerystring);

        if (Convert.ToInt32(grdSubjects.DataKeys[aoGridViewRow.RowIndex][3].ToString()) == -1)
        {
            oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.ProgressReportEntryCompleted;
            oHyperLink.Text = Resources.LocalizedResources.ProgressReportEntryCompleted;
            oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportEntryCompleted;
        }
        else
        {
            oHyperLink.NavigateUrl = Constants.S_PAGE_SUBJECT_MARK_ASSIGNMENT + "?" + sEncrypt;
            aoGridViewRow.Cells[aiCellIndex].ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
            oHyperLink.Text = Resources.LocalizedResources.MarksEntryCompleted;
            oHyperLink.ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
        }

        aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }

    /// <summary>
    /// This method is used to add links for published configuration.
    /// </summary>
    /// <param name="gridViewRow"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="asQueryString"></param>
    private void AddLinkForPublishedConfiguration(GridViewRow gridViewRow, int aiCellIndex, string asQueryString)
    {
        HyperLink oHyperLink = new HyperLink { ForeColor = System.Drawing.Color.White };
        oHyperLink.Font.Bold = true;
        oHyperLink.ImageUrl = S_IMG_FOR_COMPLETE_CONFIGURATION;
        oHyperLink.Text = Resources.LocalizedResources.ProgressReportAlreadyPublished;
        oHyperLink.ToolTip = Resources.LocalizedResources.ProgressReportAlreadyPublished;
        asQueryString = asQueryString + "&IsReadOnly=True";
        oHyperLink.NavigateUrl = "PrePrimaryStudentProgressList.aspx?" + CommonUtility.EncryptQuerystring(asQueryString);
        gridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
    }


    /// <summary>
    /// To add submit link to the subject.
    /// </summary>
    /// <param name="aoGridViewRow"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="aiStandardDivisionId"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="aiTestId"></param>
    /// <param name="asSubmitToolTip"></param>
    /// <param name="asAllowPartialSubmit"></param>
    /// <param name="asQueryString"></param>
    /// <param name="abIsMonthConfig"></param>
    private void AddSubmitMarksToClassTeacherLink(GridViewRow aoGridViewRow, int aiCellIndex, int aiStandardDivisionId, int aiSubjectId, int aiTestId, string asSubmitToolTip, string asAllowPartialSubmit, string asQueryString, bool abIsMonthConfig)
    {
        Image oHyperLink = new Image();
        aoGridViewRow.Cells[aiCellIndex].ToolTip = asSubmitToolTip;
        aoGridViewRow.Cells[aiCellIndex].Text = asSubmitToolTip;
        aoGridViewRow.Cells[aiCellIndex].Style.Add(HtmlTextWriterStyle.Color, "#014f4f");
        if (asSubmitToolTip.Equals(Resources.LocalizedResources.SubmitMarksToClass) || asSubmitToolTip.Equals(Resources.LocalizedResources.SubmitProgressReportToClassTeacher)
            || asSubmitToolTip.Equals(Resources.LocalizedResources.ProgressReportEntryPartiallyDone) || asSubmitToolTip.Equals(Resources.LocalizedResources.ProgressReportEntryNotStarted)
            || (asAllowPartialSubmit == Constants.S_YES && !asSubmitToolTip.Equals(Resources.LocalizedResources.MarksAlreadySubmitted)))
        {
            string sIncompleteRollNos = string.Empty;
            if (asAllowPartialSubmit == Constants.S_YES)
            {
                DataRowView oDrView = (DataRowView)aoGridViewRow.DataItem;

                // Retrieve the state value for the current row. 
                sIncompleteRollNos = oDrView["IncompleteRollNos"].ToString();
            }

            string sEncrypt = CommonUtility.EncryptQuerystring(asQueryString);
            if (abIsMonthConfig)
                oHyperLink.Attributes.Add("onclick", "window.open('./SubmitProgreesReportResult.aspx?" + sEncrypt + "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=600,height=500').focus(); return false;");
            else
                oHyperLink.Attributes.Add("onclick", "SubmitMarksToClassTeacher(" + aiStandardDivisionId + "," + aiSubjectId + "," + aiTestId + "," + miAcademicYearId + "," + miSchoolId + "," + miUserId + ",'" + sIncompleteRollNos + "','Y')");
            oHyperLink.ImageUrl = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
            oHyperLink.CssClass = "IconSpacing CursorHand";
            oHyperLink.ToolTip = (asAllowPartialSubmit != Constants.S_YES) ? asSubmitToolTip : "Submit Marks.";
            aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
        }

        if (asSubmitToolTip.Equals("Marks already submitted") && Settings.AllowUnsubmitExamMarks)
        {
            Image ohunsubmit = new Image();
            ohunsubmit.Attributes.Add("onclick", "SubmitMarksToClassTeacher(" + aiStandardDivisionId + "," + aiSubjectId + "," + aiTestId + "," + miAcademicYearId + "," + miSchoolId + "," + miUserId + ",'" + "" + "','N')");
            ohunsubmit.ImageUrl = "~/riteschool/images/unsubmit.jpg";
            ohunsubmit.CssClass = "iconspacing cursorhand";
            ohunsubmit.ToolTip = "unsubmit marks.";
            aoGridViewRow.Cells[aiCellIndex].Controls.Add(ohunsubmit);
        }
    }


    /// <summary>
    /// To add submit link to the subject.
    /// </summary>
    /// <param name="aoGridViewRow"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="aiStandardDivisionId"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="aiTestId"></param>
    /// <param name="asSubmitToolTip"></param>
    /// <param name="asAllowPartialSubmit"></param>
    /// <param name="asQueryString"></param>
    /// <param name="abIsMonthConfig"></param>
    private void AddSubmitMarksToClassTeacherLinkForMyClass(GridViewRow aoGridViewRow, int aiCellIndex, int aiStandardDivisionId, int aiSubjectId, int aiTestId, string asSubmitToolTip, string asAllowPartialSubmit, string asQueryString, bool abIsMonthConfig)
    {
        Image oHyperLink = new Image();
        aoGridViewRow.Cells[aiCellIndex].ToolTip = asSubmitToolTip;
        aoGridViewRow.Cells[aiCellIndex].Text = asSubmitToolTip;
        aoGridViewRow.Cells[aiCellIndex].Style.Add(HtmlTextWriterStyle.Color, "#014f4f");
        if (asSubmitToolTip.Equals(Resources.LocalizedResources.SubmitMarksToClass) || asSubmitToolTip.Equals(Resources.LocalizedResources.SubmitProgressReportToClassTeacher)
            || asSubmitToolTip.Equals(Resources.LocalizedResources.ProgressReportEntryPartiallyDone) || asSubmitToolTip.Equals(Resources.LocalizedResources.ProgressReportEntryNotStarted)
            || (asAllowPartialSubmit == Constants.S_YES && !asSubmitToolTip.Equals(Resources.LocalizedResources.MarksAlreadySubmitted)))
        {
            string sIncompleteRollNos = string.Empty;
            if (asAllowPartialSubmit == Constants.S_YES)
            {
                DataRowView oDrView = (DataRowView)aoGridViewRow.DataItem;

                // Retrieve the state value for the current row. 
                sIncompleteRollNos = oDrView["IncompleteRollNos"].ToString();
            }

            string sEncrypt = CommonUtility.EncryptQuerystring(asQueryString);
            if (abIsMonthConfig)
                oHyperLink.Attributes.Add("onclick", "window.open('./SubmitProgreesReportResult.aspx?" + sEncrypt + "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=600,height=500').focus(); return false;");
            else
                oHyperLink.Attributes.Add("onclick", "SubmitMarksToClassTeacher(" + aiStandardDivisionId + "," + aiSubjectId + "," + aiTestId + "," + miAcademicYearId + "," + miSchoolId + "," + miUserId + ",'" + sIncompleteRollNos + "','Y')");
            oHyperLink.ImageUrl = "~/RITeSchool/images/icoGrid_SubmitExamMarks.gif";
            oHyperLink.CssClass = "IconSpacing CursorHand";
            oHyperLink.ToolTip = (asAllowPartialSubmit != Constants.S_YES) ? asSubmitToolTip : "Submit Marks.";
            aoGridViewRow.Cells[aiCellIndex].Controls.Add(oHyperLink);
        }

        if (asSubmitToolTip.Equals("Marks already submitted") && Settings.AllowUnsubmitExamMarks)
        {
            Image ohunsubmit = new Image();
            ohunsubmit.Attributes.Add("onclick", "SubmitMarksToClassTeacher(" + aiStandardDivisionId + "," + aiSubjectId + "," + aiTestId + "," + miAcademicYearId + "," + miSchoolId + "," + miUserId + ",'','N')");
            ohunsubmit.ImageUrl = "~/riteschool/images/iconGridSml_ViewGE.gif";
            ohunsubmit.CssClass = "iconspacing cursorhand";
            ohunsubmit.ToolTip = "unsubmit marks.";
            aoGridViewRow.Cells[aiCellIndex].Controls.Add(ohunsubmit);
        }
    }



    #endregion

    /// <summary>
    /// This function checks the preconditons of Teacher timetable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.ExamMarks);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            HideControls();
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls as per requirement.
    /// </summary>
    private void HideControls()
    {
        divGridView.Visible = false;
        LegendTable.Visible = false;
        lblLegend.Visible = false;
        pnlFields.Visible = false;
    }

    /// <summary>
    /// This function checks for the user roles and displays the fields accordingly.
    /// For Admin: The combobox for teachers is diplayed.
    /// For Teacher: The combobox for teachers is not diplayed.
    /// Other roles(student) are invalid.
    /// </summary>
    /// <returns>
    /// true : for user roles of admin and teacher.
    /// false: otherwise.
    /// </returns>
    private bool CheckUserRolesAndSetDisplay()
    {
        bool bReturn = true;
        Constants.UserRoles oUserRoles = moUserRole;
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.AssignExamMarks).ToString();

        // if teacher        
        if (oUserRoles == Constants.UserRoles.Teacher && !bool.Parse(hidUserHasFullAccess.Value))
        {
            HideCombo(false);
            miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
        }
        else if (oUserRoles == Constants.UserRoles.Admin || oUserRoles == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
        {
            // if admin or supervisor
            HideCombo(true);
            if (IsPostBack)
                miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
        }
        else
            bReturn = false;
        return bReturn;
    }

    /// <summary>
    /// This method is used to show/hide teacher's combobox.
    /// </summary>
    /// <param name="abAction"></param>
    private void HideCombo(bool abAction)
    {
        cmbTeachers.Visible = abAction;
        lblTeacher.Visible = abAction;
        tdTeacher.Visible = abAction;
    }

    /// <summary>
    /// This method fills the combo box for teachers.
    /// </summary>
    private void FillTeachersComboBox()
    {
        // Get all class teachers
        TeacherSubjectAssignmentCollectionBL oSubjectTeacherBL = new TeacherSubjectAssignmentCollectionBL();
        DataTable oDt = oSubjectTeacherBL.RetriveSubjectTeachers(miAcademicYearId);
        ControlUtility.FillDropDownList(
                       oDt,
                       ref cmbTeachers,
                       Constants.S_TEACHER_ID_FIELD,
                       Constants.S_TEACHER_NAME_FIELD,
                       Constants.S_SELECT);

        if (moUserRole == Constants.UserRoles.Teacher)
        {
            miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            cmbTeachers.SelectedValue = miTeacherId.ToString();
        }
        
    }

    /// <summary>
    /// This method is used to fill  class comobox.
    /// 
    /// </summary>
    private void FillTeachersClassComboBox()
    {
        // get all class teachers
        TeacherSubjectAssignmentCollectionBL oSubjectTeacherBL = new TeacherSubjectAssignmentCollectionBL();
        DataTable oDtSubjectTeachersClass = oSubjectTeacherBL.RetriveSubjectTeacherClass(miSchoolId, miAcademicYearId, miTeacherId);
        ControlUtility.FillDropDownList(oDtSubjectTeachersClass, ref cmbClass, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_STANDARD_DIVISION_NAME_FIELD, Constants.S_ALL);
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count <= 0)
			return;
        
		if (QueryString["TeacherId"] != null && !QueryString["TeacherId"].Trim().Equals(S_DEFAULT))
        {
            miTeacherId = QueryString["TeacherId"].ToInt();
            miStandardDivisionId = QueryString["SelectedStandardDivisionId"].ToInt();
            cmbTeachers.SelectedValue = miTeacherId.ToString();
            cmbClass.SelectedValue = miStandardDivisionId.ToString();
        }

        if (QueryString["TestId"] == null)
			return;
        
		string sTestId = QueryString["TestId"];
        cmbExams.SelectedValue = sTestId;
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidValRegenerateMsg.Value = Resources.LocalizedResources.ValRegenerateMsg;
        hidValTestMarksConfiguration.Value = Resources.LocalizedResources.ValTestMarksConfiguration;
        hidRollNos.Value = Resources.LocalizedResources.RollNos;
    }

    /// <summary>
    /// this method is decide to visibility of my subject listview
    /// </summary>
    /// <param name="abFlag"></param>
    private void SetOwnClassVisibility(bool abIsAtLoad)
    {
        if (Settings.EnableAssignExamMarksToAllSubjectOfClass == true)
        {
            trMyClass.Visible = true;

            if (!abIsAtLoad || (QueryString["TeacherId"] != null && QueryString["TeacherId"].ToString() != string.Empty && QueryString["TeacherId"].ToString() != Constants.S_ZERO))
                FillMyClassExamStatusGrid();
        }
        else
            trMyClass.Visible = false;

    }    

    #endregion
   
}