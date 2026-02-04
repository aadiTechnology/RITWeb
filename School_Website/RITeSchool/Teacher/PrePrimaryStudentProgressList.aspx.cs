/*
 * This Class is used to show list of student for Anual result.
 * User can generate and view the result 
 * Author: Shankar Gurav.
 * Date of creation: 8 March 2008
 * Date of modification: 8 March 2008
 * 
 * Modified By: Rohini
 * Date: 29-Apr-2013
 * Description: Added post back url to back button.
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

public partial class PrePrimaryStudentProgressList : SchoolBase
{
    #region constants

    private const int I_COL_INDEX_ROLL_NO = 0;
    private const int I_COL_INDEX_NAME = 1;
    private const int I_COL_INDEX_DATAKEY_STUDENTID = 0;
    private const int I_COL_INDEX_DATAKEY_ISGENERATED = 1;
    private const int I_COL_INDEX_GENERATERESULT = 2;
    private const int I_COL_INDEX_VIEW = 7;
    private const int I_COL_INDEX_GRACE = 8;
    private const int I_COL_ZERO = 0;

    private const string S_IMG_FOR_NONE_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif";
    private const string S_IMG_FOR_PARTIAL_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif";
    private const string S_IMG_FOR_COMPLETE_CONFIGURATION = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
    private const string S_CSS_CLASS_NOT_APPLICABLE = "ClsGridNA";
    private const string S_TOOLTIP_COMPLETE = "Progress Report entry Completed";
    private const string S_TOOLTIP_PARTIAL = "Progress Report entry partially done";
    private const string S_TOOLTIP_NOT_STARTED = "Progress Report entry not started";
    private const string S_COMPLETED = "Complete";
    private const string S_PUBLISHED = "Published";
    private const string S_NOT_STARTED = "Not Started";
    private const string S_PARTIAL = "Partial";
    private const string S_CENTER = "center";
    private const string S_NONE = "None";
    private const string S_LBL_NO_RECORD = "LblNoRecord";
    private const string S_RESULT_PUBLISH_MSG = "All school's results are published successfully.";
    private const string S_ERR_MSG_TESTS = "All configured exams are not published";
    private const string S_NO_EXAM_PUBLISH_MSG = "No exam of this class has been published for the current academic year.";

    #endregion

    #region Members

    private int miTeacherID = 0;
    private int miStdDivId = 0;
    
    #endregion 

    #region Events

    /// <summary>
    /// This method event is used to render student's progress report while first time page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            btnPublishAll.CausesValidation = false;
            if (!IsPostBack)
            {
                InitializeFields();
                btnPublishAll.Visible = false;
                SetDefaultSortGridArrow();
                SetJavascriptAttributes();
                if (CheckPreCondition())
                {
                    FillTeachersComboBox();
                    GetQueryString();
                    if (moUserRole == Constants.UserRoles.Admin
                        || bool.Parse(hidUserHasFullAccess.Value))
                    {
                        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                        miTeacherID = Convert.ToInt32(cmbTeachers.SelectedValue);
                        miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
                        if (String.IsNullOrEmpty(Request.QueryString.ToString())) 
                            FillTestCombobox();
                        VisibleHideTeacherCombo(true);
                    }
                    else if (moUserRole == Constants.UserRoles.Teacher)
                    {
                        VisibleHideTeacherCombo(false);
                        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                        miTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
                        miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
                        if (String.IsNullOrEmpty(Request.QueryString.ToString()))
                            FillTestCombobox();
                    }
                }
            }

            cmbTeachers.Focus();
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// In this event we can change the value of the objectdatasource parameters.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdODStudent_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hidSortExpression.Value))
                e.Arguments.SortExpression = hidSortExpression.Value + " " + hidSortDirection.Value;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method event is used to navigate to control panel when user press cancel button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage("TestMarksConfigurationUI.aspx?" + hidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show the student progress sheet list for test.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTests_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdStudents.PageIndex = Constants.I_ZERO;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event method is used to show progress sheet.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            ShowList();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to show student of class for selected class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            trTotalRec.Visible = false;
            SetDefaultSortGridArrow();
            miTeacherID = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (miTeacherID != Constants.I_ZERO)
            {
                TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
                hidStdDivId.Value = miStdDivId.ToString();
                FillTestCombobox();
                grdStudents.Visible = btnPublish.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle list the studet list of given class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        try
        {
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to publish the result
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            int iTestID = Convert.ToInt32(cmbTests.SelectedValue);
            if (moUserRole == Constants.UserRoles.Admin
                || bool.Parse(hidUserHasFullAccess.Value))
                miTeacherID = Convert.ToInt32(cmbTeachers.SelectedValue);
            else if (moUserRole == Constants.UserRoles.Teacher)
                miTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);

            TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
            miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
            hidStdDivId.Value = miStdDivId.ToString();
            SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStdDivId, iTestID);

            if (oSwStdDivTestMasterBL.Standerd_division_Id == Constants.I_ZERO)
            {
                oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
                oSwStdDivTestMasterBL.School_id = miSchoolId;
                oSwStdDivTestMasterBL.Acadmic_year_id = miAcademicYearId;
                oSwStdDivTestMasterBL.Standerd_division_Id = miStdDivId;
                oSwStdDivTestMasterBL.SchoolWise_Test_Id = iTestID;
                oSwStdDivTestMasterBL.Is_Published = Constants.C_YES;
                oSwStdDivTestMasterBL.Inserted_By_id = miUserId;
                oSwStdDivTestMasterBL.InsertSchoolWiseStanderedDivisionTestMaster();
                btnPublish.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message; 
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This method is used to publish the result of all school
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublishAll_Click(object sender, EventArgs e)
    {
        try
        {
            cmbTeachers.SelectedIndex = Constants.I_ZERO;
            MakeDisableAllCntrl();
            SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
            oSwStdDivTestMasterBL.isAllResultsGenerated(miSchoolId, miAcademicYearId);
            SchoolWiseAnnualResultPublishCollectionBL.PublishAllSchoolResults(miSchoolId, miAcademicYearId);

            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = S_RESULT_PUBLISH_MSG;
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            grdStudents.Visible = false;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #region grid events

    /// <summary>
    /// This method is used to page index changing
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStudents.PageIndex = e.NewPageIndex;
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sorting status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
            FillStudentGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to row data bound 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                int iTeacherID = Constants.I_ZERO;
                iTeacherID = Session[Constants.S_SESSION_TEACHER_ID] != null && !bool.Parse(hidUserHasFullAccess.Value) ? Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]) : Convert.ToInt32(cmbTeachers.SelectedValue);
                string strUrl;
                string sQueryString;
                HyperLink oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GENERATERESULT].Controls[I_COL_ZERO];

                strUrl = Convert.ToBoolean(hidIsMonthConfig.Value) == false ? oHyperLinkField.NavigateUrl : oHyperLinkField.NavigateUrl.Replace("~/RITeSchool/Teacher/PrePrimaryProgressSheetEntry.aspx", "~/RITeSchool/Teacher/StudentProgressReportEntry.aspx");
                sQueryString = strUrl.Substring(strUrl.IndexOf("?") + Constants.I_ONE) + "&TeacherId=" + iTeacherID.ToString() + "&StdDiv=" + miStdDivId + "&TestId=" + cmbTests.SelectedValue
                                        + "&pIndex=" + grdStudents.PageIndex.ToString()
                                        + "&pSortExp=" + hidSortExpression.Value
                                        + "&pSortDirc=" + hidSortDirection.Value
                                        + "&IsReadOnly=" + hidIsReadOnly.Value
                                        + "&IsMonthConfig=" + hidIsMonthConfig.Value;

                oHyperLinkField.NavigateUrl = strUrl.Substring(Constants.I_ZERO, strUrl.IndexOf("?") + Constants.I_ONE) + CommonUtility.EncryptQuerystring(sQueryString);

                string sStatus = grdStudents.DataKeys[e.Row.RowIndex][Constants.I_ONE].ToString();
                switch (sStatus)                    
                {
                    case S_NOT_STARTED:
                        oHyperLinkField.ImageUrl = S_IMG_FOR_NONE_CONFIGURATION;
                        oHyperLinkField.Text = S_TOOLTIP_NOT_STARTED;
                        oHyperLinkField.ToolTip = S_TOOLTIP_NOT_STARTED;
                        break;
                    case S_PARTIAL:
                        oHyperLinkField.ImageUrl = S_IMG_FOR_PARTIAL_CONFIGURATION;
                        oHyperLinkField.Text = S_TOOLTIP_PARTIAL;
                        oHyperLinkField.ToolTip = S_TOOLTIP_PARTIAL;
                        break;
                    case S_COMPLETED:
                        oHyperLinkField.ImageUrl = S_IMG_FOR_COMPLETE_CONFIGURATION;
                        oHyperLinkField.Text = S_TOOLTIP_COMPLETE;
                        oHyperLinkField.ToolTip = S_TOOLTIP_COMPLETE;
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;               
                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[I_COL_ZERO].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[I_COL_ZERO].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    int pageNumber;
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int iPageIndex = Constants.I_ZERO; iPageIndex < grdStudents.PageCount; iPageIndex++)
                    {
                        // Create a ListItem object to represent a page.
                        pageNumber = iPageIndex + Constants.I_ONE;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (iPageIndex == grdStudents.PageIndex)
                            item.Selected = true;

                        // Add the ListItem object to the Items collection of the DropDownList.
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + Constants.I_ONE;

                    // Update the Label control with the current page information.
                    pageLabel.Text = "Page " + currentPage.ToString() +
                      "of " + grdStudents.PageCount.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to created gridview rows.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = (System.Web.UI.WebControls.GridView)sender;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                // Call the GetSortColumnIndex helper method to determine
                // the index of the column being sorted.
                int sortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (sortColumnIndex != -1)
                {
                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(sortColumnIndex, e.Row, hidSortDirection.Value);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    /// <summary>
    /// This event is used to change gridview page as per the pages selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow grdPagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList ddlPageList = (DropDownList)grdPagerRow.Cells[I_COL_ZERO].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = ddlPageList.SelectedIndex;
            FillStudentGrid();
            grdStudents.DataSourceID = GrdODStudent.ID;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set pager start and end value. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdODStudent_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + Constants.I_ONE);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStudents.PageSize) - Constants.I_ONE);
                lblTotal.Text = e.ReturnValue.ToString();
                trTotalRec.Visible = e.ReturnValue.ToString() == Constants.S_ZERO ? false : true;

                if (e.ReturnValue.GetType() != typeof(DataTable))
                    if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                        lblEndIndex.Text = e.ReturnValue.ToString();
                if (lblTotal.Text != string.Empty)
                    trTotalRec.Visible = Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT ? false : true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to check is this progress sheet published or not before datatabinding.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_DataBinding(object sender, EventArgs e)
    {
        try
        {
            string sStatus = StudentBL.getPrePrimaryProgressSheetCompleteStatus(miSchoolId, miStdDivId, Convert.ToInt32(cmbTests.SelectedValue), miAcademicYearId);
            if (sStatus == S_COMPLETED)
                btnPublish.Enabled = true;
            else if (sStatus == S_PUBLISHED)
                btnPublish.Enabled = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private Method

    /// <summary>
    /// This method is used to fill student grid.
    /// </summary>
    private void ShowList()
    {
        try
        {
            if (moUserRole == Constants.UserRoles.Admin
                || bool.Parse(hidUserHasFullAccess.Value))
                miTeacherID = Convert.ToInt32(cmbTeachers.SelectedValue);
            else if (moUserRole == Constants.UserRoles.Teacher)
                miTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);

            TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
            miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
            StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
            char cUseAvarageFinalResult = Settings.UseAvarageFinalResult ? Constants.C_YES : Constants.C_NO;
            oStudentSubjectMarksBL.GenerateAllStudentsResult(miSchoolId, miAcademicYearId, miStdDivId, miUserId, cUseAvarageFinalResult);
            FillStudentGrid();
            StudentProgress oStudentProgress = new StudentProgress();

            if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId))
            {
                if (!IsAllTestPublishedForStdDivId(miStdDivId))
                {
                    MakeDisableAllCntrl();
                    throw new Exception(S_ERR_MSG_TESTS);
                }

                FillStudentGrid();
                if (IsAllResultsGeneratedForStdDiv(miStdDivId))
                {
                    SetToppersLinkURL();
                    grdStudents.Visible = true;
                }
            }
            else
            {
                MakeDisableAllCntrl();
                throw new Exception(S_NO_EXAM_PUBLISH_MSG);
            }
        }
        catch (Exception ex)
        {
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, S_CENTER);
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, S_NONE);
            lblErrorMsg.CssClass = S_LBL_NO_RECORD;
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        trTotalRec.Visible = false;
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.AssignExamMarks).ToString();
    }

    /// <summary>
    /// This mwthod is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnCancel, btnPublish, btnPublishAll, btnViewAll });
        btnPublishAll.Attributes.Add("Onclick", "if(!(ConfirmAction())){return false;}");
        btnPublish.Attributes.Add("Onclick", "if(!(ConfirmAction())){return false;}");
    }

    /// <summary>
    /// This is method to check all test publish
    /// </summary>
    /// <param name="aiStandardDivisionId"></param>
    /// <returns></returns>
    private bool IsAllResultsGeneratedForStdDiv(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStdDivId);
        return oSWStdDivTestMasterBL.IsAllResultsGeneratedForStdDiv();
    }

    /// <summary>
    /// This is method to check all test publish
    /// </summary>
    /// <returns></returns>
    private bool IsAllSchoolResultsPublished()
    {
        SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        return oSwStdDivTestMasterBL.isAllSchoolResultsPublished(miSchoolId, miAcademicYearId);
    }

    /// <summary>
    /// This method is used to disable all contro;l of screen
    /// </summary>
    private void MakeDisableAllCntrl()
    {
        btnPublish.Enabled = grdStudents.Visible = false;
    }

    /// <summary>
    /// This function checks the preconditons of Teachertimetable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AnnualResult);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            trPrecondition.Visible = true;
            divErr.InnerHtml = sLinks;
            pnlFilter.Visible = trValidation.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to set default sort arrow in grid.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        hidSortExpression.Value = grdStudents.Columns[I_COL_ZERO].SortExpression;
        hidSortDirection.Value = Utility.Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to fill student grid
    /// </summary>
    private void FillStudentGrid()
    {
        trTotalRec.Visible = true;
        miTeacherID = moUserRole == Constants.UserRoles.Teacher && !bool.Parse(hidUserHasFullAccess.Value) ? Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]) : Convert.ToInt32(cmbTeachers.SelectedValue);
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
        miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
        hidStdDivId.Value = miStdDivId.ToString();
        grdStudents.Visible = true;
        grdStudents.DataSourceID = GrdODStudent.ID;
        grdStudents.DataBind();
    }

    /// <summary>
    /// This method is used to check that is the grace applicable for any student or not.
    /// </summary>
    /// <param name="aiStdDivId"></param>
    /// <returns></returns>
    private bool IsGraceAppliedForAnyStudentOfStdDivId(int aiStdDivId)
    {
        SchoolWiseAnnualResultPublishBL oSWStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL();
        bool bResult = oSWStdDivResultPublishBL.IsGraceAppliedForAnyStudentOfStdDivId(aiStdDivId);
        return bResult;
    }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideTeacherCombo(bool abAction)
    {
        tdlblTeacher.Visible = abAction;
    }

    /// <summary>
    /// This function is used to fill teacher combo
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillPrePrimaryClassTeachersComboBox(miSchoolId, miAcademicYearId, ref cmbTeachers, string.Empty);
        if (cmbTeachers.Items.Count == Constants.I_ZERO)
        {
            lblErrorMsg.Visible = HyperLink1.Visible = true;
            lblErrorMsg.Text = Constants.S_ERROR_MSG_FOR_ALL_CONFIGURATION;
            pnlFilter.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetToppersLinkURL()
    {
        if (Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] != null)
            miStdDivId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(miStdDivId);
        string sQueryString = "ExamType=0&ToppersType=0&StdDivId=" + miStdDivId.ToString() + "&StdId=" + oStandardDivisionMasterBL.StandardId.ToString();
        sQueryString = "../Student/ExamToppersUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
    }

    /// <summary>
    /// This method is used to sort grid
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private bool IsAllTestPublishedForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        return oSwStdDivTestMasterBL.isAllTestPublished();
    }

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private bool IsResultPublishedForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseAnnualResultPublishBL oSwStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        return oSwStdDivResultPublishBL.AnnualResult_publish_Id != Constants.I_ZERO;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 0)
        {
            if (QueryString["TeacherId"] != null)
            {
                if (QueryString["pIndex"] != null)
                    grdStudents.PageIndex = QueryString["pIndex"].ToInt();
                if (QueryString["pSortExp"] != null)
                    hidSortExpression.Value = QueryString["pSortExp"];
                if (QueryString["pSortDirc"] != null)
                    hidSortDirection.Value = QueryString["pSortDirc"];
                if (QueryString["IsReadOnly"] != null)
                    hidIsReadOnly.Value = QueryString["IsReadOnly"];

                miTeacherID = QueryString["TeacherId"].ToInt();
	            hidBackUrl.Value = "TeacherId=" + miTeacherID;
                if (moUserRole == Constants.UserRoles.Admin
                || bool.Parse(hidUserHasFullAccess.Value))
                {
                 
                    cmbTeachers.SelectedValue = miTeacherID.ToString();
                    FillPageContent();
                    cmbTests.SelectedValue = QueryString["IsMonthConfig"].ToBool() == false ? QueryString["TestId"] : Constants.S_ZERO;

                    hidIsMonthConfig.Value = QueryString["IsMonthConfig"].ToString();
                    FillStudentGrid();
                    tblHeading.Visible = true;
                    lblTeacherHeading.Text = cmbTeachers.SelectedItem.Text;
                }
                else if (moUserRole == Constants.UserRoles.Teacher)
                {
                    FillPageContent();
                    cmbTests.SelectedValue = QueryString["IsMonthConfig"].ToBool() == false ? QueryString["TestId"] : Constants.S_ZERO;

                    hidIsMonthConfig.Value = QueryString["IsMonthConfig"].ToString();
                    FillStudentGrid();
                }
            }
        }
        else
        {
            grdStudents.Visible = true;
            grdStudents.DataSourceID = GrdODStudent.ID;
            grdStudents.DataBind();
        }
    }

    /// <summary>
    /// This method is used to fill page content.
    /// </summary>
    private void FillPageContent()
    {
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
        miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherID);
        hidStdDivId.Value = miStdDivId.ToString();
        StudentProgress oStudentProgress = new StudentProgress();
        FillTestCombobox();
    }

    /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillTestCombobox()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(miStdDivId);

        DataTable oDSAllTests = oTestCollectionBL.GetAllTestsForStandard(oStandardDivisionMasterBL.StandardId);
        ControlUtility.FillDropDownList(oDSAllTests, ref cmbTests,
                                       Constants.S_TEST_ID_FIELD,
                                       Constants.S_TEST_NAME_FIELD,
                                       Constants.S_SELECT);
    }

    #endregion
}