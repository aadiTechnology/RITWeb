/*
 * This Class is used to show list of student for Anual result.
 * User can generate and view the result 
 * Author: Shankar Gurav.
 * Date of creation: 8 March 2008
 * Date of modification: 8 March 2008
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

public partial class StudentResultList : SchoolBase
{
    #region constants

    private const int I_COL_INDEX_DATAKEY_ISGENERATED = 1;
    private const int I_COL_INDEX_RESULT = 5;
    private const int I_COL_INDEX_GENERATERESULT = 6;
    private const int I_COL_INDEX_VIEW = 7;
    private const int I_COL_INDEX_GRACE = 8;
    private const int I_CONFIRM = 1; 

    #endregion

    private int miTeacherId;
    private int miStdDivId;
    private bool miIsPublished;

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
			hlnkToppers.Visible = Settings.ShowTopppers;
            btnPublishAll.CausesValidation = false;            
            if (!IsPostBack)
            {
                InitializeFields();
                tblErrorMsg.Visible = false;
                btnViewAll.Enabled = false;
                btnPublishAll.Visible = false;
                SetDefaultSortGridArrow();
				btnPublishAll.Attributes.Add("Onclick", "if(!(ConfirmAction('" + !Settings.IsMiniSite + "'))){return false;}");
				btnPublish.Attributes.Add("Onclick", "if(!(ConfirmAction('" + !Settings.IsMiniSite+ "'))){return false;}");
                btnShow.Attributes["onclick"] = "javascript:DisableButtons()";

                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                if (CheckPreCondition())
                {
                    FillTeachersComboBox();
                    GetQueryString();
                    if (moUserRole == Constants.UserRoles.Admin
                        || bool.Parse(hidUserHasFullAccess.Value))
                    {
                        VisibleHideTeacherCombo(true);
                        VisibleHideGenerateButton(true);
                    }
                    else if (moUserRole == Constants.UserRoles.Teacher)
                    {
                        btnUnPublish.Visible = false;
                        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                        miTeacherId = (bool.Parse(hidUserHasFullAccess.Value)) ? cmbTeachers.SelectedValue.ToInt() : Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
                        miStdDivId = oTeacherStandardDetailsCollectionBL.GetStdDivIdOfClassTeacher(miTeacherId);
                        StudentProgress oStudentProgress = new StudentProgress();
                        if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId))
                        {
                            FillStudentGrid();
                            if (IsAllResultsGeneratedForStdDiv())
                            {
                                SetTopperslinkUrl();
								if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(miStdDivId, 0) == Constants.C_YES)
                                    hlnkToppers.Attributes.Remove("onclick");
                                grdStudents.Visible = true;
                            }
                            else
                                hlnkToppers.Attributes.Remove("onclick");

                            if (!IsAllTestPublishedForStdDivId(miStdDivId))
                            {
                                string sErrorMesage = AllUnpublishedTestForStdDivId(miStdDivId);
								tblErrorMsg.Visible=true;
                                throw new NoResultFound(Resources.LocalizedResources.AllConfiguredExamsAreNotPublished + (sErrorMesage == string.Empty ? string.Empty : " - " + sErrorMesage));
                            }
                        }
                        else
                        {
                            MakeDisableAllCntrl();
                            throw new NoResultFound(Resources.LocalizedResources.NoExamOfThisClassHasBeenPublished);
                        }
                    }
                }
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
            
            // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
            btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
            btnUnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
            cmbTeachers.Focus();
            ApplyMouseHoverEffect(new List<Button> { btnPublish, btnPublishAll, btnShow, btnViewAll });
        }
        catch (NoResultFound oEx)
        {
            SetErrorMessage(oEx.Message);
        }
        catch (ApplicationException oEx)
        {
            SetErrorMessage(oEx.Message);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
            {
                e.Arguments.SortExpression = hidSortExpression.Value + " " + hidSortDirection.Value;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
			miStdDivId= Convert.ToInt32(cmbTeachers.SelectedValue);
            int iUserId = miUserId;
            TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);            
            StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
            char cUseAvarageFinalResult = Settings.UseAvarageFinalResult ? Constants.C_YES : Constants.C_NO;
            oStudentSubjectMarksBL.GenerateAllStudentsResult(miSchoolId, miAcademicYearId, miStdDivId, iUserId, cUseAvarageFinalResult);
            FillStudentGrid();
            StudentProgress oStudentProgress = new StudentProgress();
            if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId))
            {
                FillStudentGrid();
				if (IsAtleastOneResultGeneratedForStdDiv(miStdDivId))
                {
                    SetTopperslinkUrl();
					if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(miStdDivId, 0) == Constants.C_YES)
                        hlnkToppers.Attributes.Remove("onclick");
                    grdStudents.Visible = true;
                }
                else
                {
                    hlnkToppers.Attributes.Remove("onclick");
                }

                if (!IsAllTestPublishedForStdDivId(miStdDivId))
                {
                    string sErrorMesage = AllUnpublishedTestForStdDivId(miStdDivId);
                    throw new NoResultFound(Resources.LocalizedResources.AllConfiguredExamsAreNotPublished + (sErrorMesage == string.Empty ? string.Empty : " - " + sErrorMesage));
                }
            }
            else
            {
                MakeDisableAllCntrl();
                throw new NoResultFound(Resources.LocalizedResources.NoExamOfThisClassHasBeenPublished);
            }
        }
        catch (NoResultFound oEx)
        {
            SetErrorMessage(oEx.Message);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
            tblErrorMsg.Visible = true;
            trTotalRec.Visible = false;
            btnViewAll.Enabled = false;
			miStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
			if (miStdDivId != 0)
            {
                StudentProgress oStudentProgress = new StudentProgress();
                if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId))
                {
                    FillStudentGrid();
					if (IsAtleastOneResultGeneratedForStdDiv(miStdDivId))
                    {
                        SetTopperslinkUrl();
                        grdStudents.Visible = true;
                    }
					else
					   hlnkToppers.Attributes.Remove("onclick");
                }
                else
                {
                    MakeDisableAllCntrl();
                    trTotalRec.Visible = false;
                    throw new NoResultFound(Resources.LocalizedResources.NoExamOfThisClassHasBeenPublished);
                }
            }
            else
            {
                hlnkToppers.Attributes.Remove("onclick");
                grdStudents.Visible = false;
                btnShow.Enabled = false;
                btnPublish.Enabled = false;
                btnViewAll.Enabled = false;
                btnUnPublish.Enabled = false;
                tblErrorMsg.Visible = false;

            }

            if (grdStudents.PageCount > 0)
                grdStudents.PageIndex = 0;

            if (!IsAllTestPublishedForStdDivId(miStdDivId))
            {
                string sErrorMesage = AllUnpublishedTestForStdDivId(miStdDivId);
				tblErrorMsg.Visible = true;
                throw new NoResultFound(Resources.LocalizedResources.AllConfiguredExamsAreNotPublished + (sErrorMesage == string.Empty ? string.Empty : " - " + sErrorMesage));
            }
            else
            {
                lblErrorMsg.Text = string.Empty;
                lblErrorMsg.Visible = false;
				tblErrorMsg.Visible=false;
            }
        }
        catch (NoResultFound oEx)
        {
            SetErrorMessage(oEx.Message);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
            int iTeacherId = (Session[Constants.S_SESSION_TEACHER_ID] != null && !Convert.ToBoolean(hidUserHasFullAccess.Value)) ? Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]) : Convert.ToInt32(cmbTeachers.SelectedValue);
            TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
            			
			miStdDivId = cmbTeachers.SelectedValue.ToInt();

            SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, miStdDivId);
            if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == 0)
            {
                oSchoolWisdeAnnualResultPublishBL.School_Id = miSchoolId;
                oSchoolWisdeAnnualResultPublishBL.Academic_Year_Id = miAcademicYearId;
                oSchoolWisdeAnnualResultPublishBL.Standard_Division_Id = miStdDivId;
                oSchoolWisdeAnnualResultPublishBL.Inserted_By_id = miUserId;
                oSchoolWisdeAnnualResultPublishBL.InsertSchoolWiseAnnualResultPublish();
				
                if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(miStdDivId, 0) == Constants.C_YES)
                    hlnkToppers.Attributes.Remove("onclick");
				SetTopperslinkUrl();
            }

            if (!IsAllTestPublishedForStdDivId(miStdDivId))
            {
                string sErrorMesage = AllUnpublishedTestForStdDivId(miStdDivId);
                SetErrorMessage(Resources.LocalizedResources.AllConfiguredExamsAreNotPublished + (sErrorMesage == string.Empty ? string.Empty : " - " + sErrorMesage));
            }
            else
            {
                lblErrorMsg.Text = string.Empty;
                lblErrorMsg.Visible = false;
				tblErrorMsg.Visible=false;
            }

            FillStudentGrid();
            if (!string.IsNullOrEmpty(hidConfirmSms.Value) && Convert.ToInt32(hidConfirmSms.Value) == I_CONFIRM) 
            SendMessageToStudent();
        }
        catch (Exception oEx)
        {
            SetErrorMessage(oEx.Message);
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This for Unpublish Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnPublish_Click(object sender, EventArgs e)
    {
        try
        {
			miStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
            SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, miStdDivId);
            if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id != 0)
                oSchoolWisdeAnnualResultPublishBL.DeleteSchoolWiseAnnualResultPublish();
            FillStudentGrid();
        }
        catch (Exception oEx)
        {
            SetErrorMessage(oEx.Message);
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
            cmbTeachers.SelectedIndex = 0;
            MakeDisableAllCntrl();
            SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
            oSWStdDivTestMasterBL.isAllResultsGenerated(miSchoolId, miAcademicYearId);
            SchoolWiseAnnualResultPublishCollectionBL.PublishAllSchoolResults(miSchoolId, miAcademicYearId);

            SetErrorMessage(Resources.LocalizedResources.AllSchoolResultsArePublishedSuccessfully);
        }
        catch (Exception oEx)
        {
            SetErrorMessage(oEx.Message);
            grdStudents.Visible = false;

            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to view all result
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnViewAll_Click(object sender, EventArgs e)
    {
        try
        {
			string sQueryString = "StandardDivisionId=" + Convert.ToInt32(cmbTeachers.SelectedValue);
            sQueryString = CommonUtility.EncryptQuerystring(sQueryString);
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage("~/Student/StudentAnnualResult.aspx?" + sQueryString);
        }
        catch (Exception oEx)
        {
            SetErrorMessage(oEx.Message);
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is method is row command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Type t = e.CommandArgument.GetType();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This Method use For Sorting Student Record.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            SetSortVariables(e.SortExpression);
            e.SortExpression = hidSortExpression.Value;
            FillStudentGrid();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
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
                HyperLink oHyperLinkField;
                if (e.Row.Cells[I_COL_INDEX_RESULT].Text.Trim() == "Pass" && Settings.IsGraceApplicable)
                {
                    e.Row.Cells[I_COL_INDEX_RESULT].ForeColor = System.Drawing.Color.Green;
                    oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GRACE].Controls[0];
                    oHyperLinkField.Visible = false;
                }
                else if (e.Row.Cells[I_COL_INDEX_RESULT].Text.Trim() == "Fail")
                {
                    e.Row.Cells[I_COL_INDEX_RESULT].ForeColor = System.Drawing.Color.Red;
                }
                else if (e.Row.Cells[I_COL_INDEX_RESULT].Text.Trim() == "Promoted")
                {
                    e.Row.Cells[I_COL_INDEX_RESULT].ForeColor = System.Drawing.Color.Orange;
                }

                int iTeacherId = (Session[Constants.S_SESSION_TEACHER_ID] != null && !bool.Parse(hidUserHasFullAccess.Value))
                                     ? Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID])
                                     : Convert.ToInt32(cmbTeachers.SelectedValue);
				int iStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
                string strUrl;
                string sQueryString;
                oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GENERATERESULT].Controls[0];
                if (moUserRole == Constants.UserRoles.Admin
                    || moUserRole == Constants.UserRoles.Supervisor
                    || moUserRole == Constants.UserRoles.Teacher)
                {
                    strUrl = oHyperLinkField.NavigateUrl;
					sQueryString = strUrl.Substring(strUrl.IndexOf("?") + 1) + "&StandardDivisionId=" + iStdDivId
                                            + "&pIndex=" + grdStudents.PageIndex
                                            + "&pSortExp=" + hidSortExpression.Value
                                            + "&pSortDirc=" + hidSortDirection.Value;
                    oHyperLinkField.NavigateUrl = strUrl.Substring(0, strUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                    oHyperLinkField.ImageUrl = "~/RITeSchool/images/final_Result.png";
                }
                else
                    oHyperLinkField.Visible = false;

                if ((grdStudents.DataKeys[e.Row.RowIndex][I_COL_INDEX_DATAKEY_ISGENERATED] != DBNull.Value) &&
                        (Convert.ToChar(grdStudents.DataKeys[e.Row.RowIndex][I_COL_INDEX_DATAKEY_ISGENERATED]) == 'Y'))
                {
                    oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_VIEW].Controls[0];

                    strUrl = oHyperLinkField.NavigateUrl;
					sQueryString = strUrl.Substring(strUrl.IndexOf("?") + 1) + "&StandardDivisionId=" + iStdDivId
                                            + "&pIndex=" + grdStudents.PageIndex
                                            + "&pSortExp=" + hidSortExpression.Value
                                            + "&pSortDirc=" + hidSortDirection.Value;
                    oHyperLinkField.NavigateUrl = strUrl.Substring(0, strUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                    oHyperLinkField.Enabled = true;
                    oHyperLinkField.ImageUrl = "~/RITeSchool/images/view.png";

                    if (Settings.IsGraceApplicable)
                    {
                        oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GRACE].Controls[0];
                        strUrl = oHyperLinkField.NavigateUrl;
						sQueryString = strUrl.Substring(strUrl.IndexOf("?") + 1) + "&StandardDivisionId=" + iStdDivId
                                                + "&StdDivId=" + miStdDivId
                                                + "&pIndex=" + grdStudents.PageIndex
                                                + "&pSortExp=" + hidSortExpression.Value
                                                + "&pSortDirc=" + hidSortDirection.Value;
                        oHyperLinkField.NavigateUrl = strUrl.Substring(0, strUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString);
                        oHyperLinkField.Enabled = true;
                        oHyperLinkField.ImageUrl = "~/RITeSchool/images/Add_Grace.png";
                    }

                    btnViewAll.Enabled = true;
                }
                else
                {
                    oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_VIEW].Controls[0];
                    oHyperLinkField.Visible = false;
                    if (Settings.IsGraceApplicable)
                    {
                        oHyperLinkField = (HyperLink)(e.Row.Cells[I_COL_INDEX_GRACE].Controls[0]);
                        oHyperLinkField.Visible = false;
                    }
                }

                if (miIsPublished)
                {
                    oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GENERATERESULT].Controls[0];
                    oHyperLinkField.Visible = false;
                    if (Settings.IsGraceApplicable)
                    {
                        oHyperLinkField = (HyperLink)(e.Row.Cells[I_COL_INDEX_GRACE].Controls[0]);
                        oHyperLinkField.Visible = false;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow pagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");
                Label pageLabel = (Label)pagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (pageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int i = 0; i < grdStudents.PageCount; i++)
                    {
                        // Create a ListItem object to represent a page.
                        int pageNumber = i + 1;
                        ListItem item = new ListItem(pageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created, this will persist the selected item in
                        // the DropDownList control.   
                        if (i == grdStudents.PageIndex)
                        {
                            item.Selected = true;
                        }

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        pageList.Items.Add(item);
                    }
                }

                if (pageLabel != null)
                {
                    // Calculate the current page number.
                    int currentPage = grdStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    pageLabel.Text = Resources.LocalizedResources.PageNo + " " + currentPage + " " +
                      Resources.LocalizedResources.Of + " " + grdStudents.PageCount + " " + Resources.LocalizedResources.Records;
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This for grdStudents RowCreate
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView sGridviewName = (GridView)sender;

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
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    /// <summary>
    /// This for Page Drop down Selected Index change
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow pagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList pageList = (DropDownList)pagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = pageList.SelectedIndex;
            FillStudentGrid();
            grdStudents.DataSourceID = GrdODStudent.ID;
      
        }
      
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this is for GrdODStudent_Selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GrdODStudent_Selected(object sender, ObjectDataSourceStatusEventArgs e)
    {
        try
        {
            if (e.ReturnValue != null && e.ReturnValue.ToString() != string.Empty)
            {
                lblStartIndex.Text = Convert.ToString((grdStudents.PageSize * grdStudents.PageIndex) + 1);
                lblEndIndex.Text = Convert.ToString((Convert.ToInt32(lblStartIndex.Text) + grdStudents.PageSize) - 1);
                if (e.ReturnValue.ToString() != string.Empty)
                {
                    lblTotal.Text = e.ReturnValue.ToString();
                    trTotalRec.Visible = e.ReturnValue.ToString() != "0";

                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }

                    if (lblTotal.Text != string.Empty)
                    {
                        trTotalRec.Visible = Convert.ToInt32(lblTotal.Text) > Constants.I_GRID_PAGE_COUNT;
                    }
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Private Method

    /// <summary>
    /// This method is used to check that is the grace applicable for any student or not.
    /// </summary>
    /// <param name="aiStdDivId"></param>
    /// <returns></returns>
    private static bool IsGraceAppliedForAnyStudentOfStdDivId(int aiStdDivId)
    {
        SchoolWiseAnnualResultPublishBL oSWStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL();
        bool bResult = oSWStdDivResultPublishBL.IsGraceAppliedForAnyStudentOfStdDivId(aiStdDivId);
        return bResult;
    }

    /// <summary>
    /// This method is used to send message to student when result is published.
    /// </summary>
    private void SendMessageToStudent()
    {
        string sMessageBody = Resources.LocalizedResources.FinalResultExamMessage;

        switch ((Constants.SchoolId)ConfigurationManager.AppSettings["SchoolID"].ToInt())
        {
            case Constants.SchoolId.SS: sMessageBody = sMessageBody.Replace("%SCHOOL_NAME%", "SS Pune"); break;
            case Constants.SchoolId.PPS: sMessageBody = sMessageBody.Replace("%SCHOOL_NAME%", "PPS Pune"); break;
        }

        string sClsTchrName = cmbTeachers.SelectedItem.Text;
        string sClass = sClsTchrName.Substring(0, sClsTchrName.IndexOf(':')).Trim();
        string sClsTeacher = sClsTchrName.Substring(sClsTchrName.IndexOf(':') + 1, ((sClsTchrName.Length - 1) - sClsTchrName.IndexOf(':'))).Trim();
        sMessageBody = sMessageBody.Replace("%studentclass%", sClass);
        sMessageBody = sMessageBody.Replace("%Classteacher%", sClsTeacher);
        using (DataTable oDtUserId = StudentBL.GetAllStudentsByGivenStdDivs(miSchoolId, miAcademicYearId, hidStdDivId.Value, false))
        {
            hidUserID.Value = Constants.S_EMPTY_STRING;
            if (oDtUserId.Rows.Count > 0)
            {
                for (int iCount = 0; iCount < oDtUserId.Rows.Count; iCount++)
                    hidUserID.Value += oDtUserId.Rows[iCount]["ID"] + ";";
                hidUserID.Value = hidUserID.Value.Substring(0, hidUserID.Value.LastIndexOf(";"));
                SendMessage(hidUserID.Value, Resources.LocalizedResources.FinalExamResult , sMessageBody);
            }
        }
    }

    /// <summary>
    /// This for send message
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="sMsgSubject"></param>
    /// <param name="sMsgBody"></param>
    private void SendMessage(string asUserId, string sMsgSubject, string sMsgBody)
    {
        Message oMessage = new Message { sMessageBody = sMsgBody, sMessageSubject = sMsgSubject };
        oMessage.SetMessageReceivers(asUserId, miUserId);
        oMessage.InsertMessageDetails(
                 miUserId,
                 moUserRole.ToInt(),
                 miAcademicYearId);
    }

    /// <summary>
    /// This method is used to set default values to controls.
    /// </summary> 
    private void InitializeFields()
    {
        trTotalRec.Visible = false;
        hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.FinalResult).ToString();
    }

    /// <summary>
    /// This is method to check all test publish
    /// </summary>
    /// <returns></returns>
    private bool IsAllResultsGeneratedForStdDiv()
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStdDivId);
        return oSWStdDivTestMasterBL.IsAllResultsGeneratedForStdDiv();
    }

    /// <summary>
    /// This method is used to disable all contro;l of screen
    /// </summary>
    private void MakeDisableAllCntrl()
    {
        btnPublish.Enabled = false;
        btnShow.Enabled = false;
        btnViewAll.Enabled = false;
        grdStudents.Visible = false;
        hlnkToppers.Visible = Settings.ShowTopppers;
        hlnkToppers.Attributes.Remove("onclick");
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
            pnlFilter.Visible = false;
            trValidation.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to set default sort arrow in grid.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        hidSortExpression.Value = grdStudents.Columns[0].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
    }

    /// <summary>
    /// This method is used to fill student grid
    /// </summary>
    private void FillStudentGrid()
    {
        trTotalRec.Visible = true;
		miStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
		TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
		hidStdDivId.Value = miStdDivId.ToString();
        miIsPublished = IsResultPublishedForStdDivId(miStdDivId);
        btnUnPublish.Enabled = miIsPublished;
        if (miIsPublished || !IsAtleastOneResultGeneratedForStdDiv(miStdDivId))
            btnPublish.Enabled = false;
        else
            btnPublish.Enabled = true;
        btnShow.Enabled = !miIsPublished;

        grdStudents.DataSourceID = GrdODStudent.ID;
        grdStudents.Visible = true;

        if (!Settings.IsGraceApplicable)
        {
            grdStudents.Columns[I_COL_INDEX_GRACE].Visible = false;
        }
        else if (btnShow.Enabled)
        {
            bool isGraceApplied = IsGraceAppliedForAnyStudentOfStdDivId(miStdDivId);
            if (isGraceApplied)
                btnShow.Attributes["onclick"] = "if(!(ShowGraceWarning())){return false;}";
            else
                btnShow.Attributes.Remove("onclick");
        }

        SetUnpublishButtonAttributes();
    }

    /// <summary>
    /// This for Check one result is Generated for Standed division
    /// </summary>
    /// <param name="aiStdDivId"></param>
    /// <returns></returns>
    private bool IsAtleastOneResultGeneratedForStdDiv(int aiStdDivId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, aiStdDivId);
        DataTable oDtResultDetails = oSWStdDivTestMasterBL.IsAtleastOneResultGeneratedForStdDiv();
        if (oDtResultDetails.IsNonEmpty())
        {
            DataRow oDataRow = oDtResultDetails.Rows[0];
            hidAbsentStudentCount.Value = Convert.ToString(oDataRow["TotalAbsentStudents"]);
            return Convert.ToBoolean(oDataRow["AllowPublish"]);
        }

        return false;
    }
    
    /// <summary>
    /// Used to set attribute of ubpublish button to open a popup of reasoning.
    /// </summary>
    private void SetUnpublishButtonAttributes()
    {
        int iStandardDivisionId = Convert.ToInt32(hidStdDivId.Value);
        int iTeacherId = 0;
        if (moUserRole == Constants.UserRoles.Admin
            || bool.Parse(hidUserHasFullAccess.Value))
            iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
        else if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);

		string sQuerystring = "StandardDivisionId=" + iStandardDivisionId + "&sTeacherName=" + cmbTeachers.SelectedItem.Text + "&TeacherId=" + iTeacherId + "&TestId=-9999" + "&sTestName=Final result";
        string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
        btnUnPublish.Attributes.Add("onclick", "window.open('../Admin/TestUnpublishPopUp.aspx?" + sEncrypt + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370').focus();return false;");
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
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideGenerateButton(bool abAction)
    {
        tdbtnShow.Visible = abAction;
        btnShow.Visible = abAction;
    }

    /// <summary>
    /// This function is used to fill teacher combo
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
		List<ClassTeacherDetails> lstTeachers = oMasterDataCollectionBL.GetClassTeachersForExamResult(miSchoolId, miAcademicYearId);

        string sHasEditAccess = CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.FinalResult).ToString();

        if (moUserRole == Constants.UserRoles.Teacher && sHasEditAccess == Constants.S_NO && bool.Parse(hidUserHasFullAccess.Value))
        {
            List<ClassTeacherDetails> lstClassTeachers = lstTeachers.Where(Teacher => Teacher.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();
            ListSource.FillDropDownList(lstClassTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", Constants.S_SELECT);
            if (lstClassTeachers.Count == Constants.I_ONE)
            {
                cmbTeachers.SelectedIndex = Constants.I_ONE;
                cmbTeachers.Enabled = false;
                cmbTeachers_SelectedIndexChanged(cmbTeachers, new EventArgs());
            }
            else
                ListSource.FillDropDownList(lstTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", Constants.S_SELECT);
        }
        else
        {
            if (moUserRole == Constants.UserRoles.Teacher && !bool.Parse(hidUserHasFullAccess.Value))
            {
                List<ClassTeacherDetails> lstClassTeachers = lstTeachers.Where(Teacher => Teacher.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();
                ListSource.FillDropDownList(lstClassTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", Constants.S_SELECT);
                if (lstClassTeachers.Count == Constants.I_ONE)
                {
                    cmbTeachers.SelectedIndex = Constants.I_ONE;
                    cmbTeachers.Enabled = false;
                    cmbTeachers_SelectedIndexChanged(cmbTeachers, new EventArgs());
                }
            }
            else
                ListSource.FillDropDownList(lstTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", Constants.S_SELECT);
        }

		if (cmbTeachers.Items.Count == 1)
		{
			lblErrorMsg.Visible = true;
			HyperLink1.Visible = true;
            lblErrorMsg.Text = Resources.LocalizedResources.AllConfiguredExamsAreNotPublished;
			pnlFilter.Visible = false;
		}
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetTopperslinkUrl()
    {
        hlnkToppers.Enabled = true;
        if (Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] != null)
            miStdDivId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(miStdDivId);
		//string sQueryString = "ExamType=0&ToppersType=1&StandardDivisionId=" + miStdDivId + "&StdId=" + oStandardDivisionMasterBL.StandardId;
        string sQueryString = "ExamType=0&ToppersType=1&StdDivId=" + hidStdDivId.Value + "&StdId=" + oStandardDivisionMasterBL.StandardId;
        sQueryString = "../Student/ExamToppersUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        hlnkToppers.Attributes.Add("onclick", "ShowToppers('" + sQueryString + "');return false;");
    }

    /// <summary>
    /// This method is used to sort grid
    /// </summary>
    private void SetSortVariables(string sSortExpression)
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING
                                     ? Constants.S_ASCENDING
                                     : Constants.S_DESCENDING;
        hidSortExpression.Value = sSortExpression;
        hidSortExpression.Value = hidSortExpression.Value + " " + hidSortDirection.Value;
    }

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private bool IsAllTestPublishedForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        return oSWStdDivTestMasterBL.isAllTestPublished();
    }

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private string AllUnpublishedTestForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseStanderedDivisionTestMasterBL oSWStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        return oSWStdDivTestMasterBL.AllUnpublishedTestForStdDivId();
    }

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private bool IsResultPublishedForStdDivId(int aiStandardDivisionId)
    {
        SchoolWiseAnnualResultPublishBL oSWStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, aiStandardDivisionId);
        return oSWStdDivResultPublishBL.AnnualResult_publish_Id != 0;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 0)
        {
			if (QueryString["StandardDivisionId"] != null)
            {
                if (QueryString["pIndex"] != null)
                    grdStudents.PageIndex = QueryString["pIndex"].ToInt();
                if (QueryString["pSortExp"] != null)
                    hidSortExpression.Value = QueryString["pSortExp"];
                if (QueryString["pSortDirc"] != null)
                    hidSortDirection.Value = QueryString["pSortDirc"];

				TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
				miStdDivId = QueryString["StandardDivisionId"].ToInt();
				if (IsAtleastOneResultGeneratedForStdDiv(miStdDivId))
                {
                    SetTopperslinkUrl();
					if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(miStdDivId, 0) == Constants.C_YES)
                        hlnkToppers.Attributes.Remove("onclick");
                    grdStudents.Visible = true;
                }
                else
                    hlnkToppers.Attributes.Remove("onclick");

					cmbTeachers.SelectedValue = miStdDivId.ToString();
                    cmbTeachers_SelectedIndexChanged(cmbTeachers, new EventArgs());
                    FillPageContent();
            }
        }
    }

    /// <summary>
    /// This for Fill Page Content
    /// </summary>
    private void FillPageContent()
    {
        TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
        StudentProgress oStudentProgress = new StudentProgress();
        if (oStudentProgress.isTestPublishedForStdDivId(miStdDivId))
        {
            FillStudentGrid();
			if (IsAtleastOneResultGeneratedForStdDiv(miStdDivId))
            {
                SetTopperslinkUrl();
				if (oTeacherStandardDetailsCollectionBL.CheckIfStandardHasOnlyGradeSystem(miStdDivId, 0) == Constants.C_YES)
                    hlnkToppers.Attributes.Remove("onclick");
				grdStudents.Visible = true;
            }
            else
            {
                hlnkToppers.Attributes.Remove("onclick");
                trTotalRec.Visible = false;
            }

            if (!IsAllTestPublishedForStdDivId(miStdDivId))
            {
                string sErrorMesage = AllUnpublishedTestForStdDivId(miStdDivId);
                throw new NoResultFound(Resources.LocalizedResources.AllConfiguredExamsAreNotPublished + (sErrorMesage == string.Empty ? string.Empty : " - " + sErrorMesage));
            }
        }
        else
        {
            MakeDisableAllCntrl();
            throw new ApplicationException(Resources.LocalizedResources.NoExamOfThisClassHasBeenPublished);
        }
    }

    /// <summary>
    /// This method is used to set error message.
    /// </summary>
    /// <param name="asMessage"></param>
    private void SetErrorMessage(string asMessage)
    {
        lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
        lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
        lblErrorMsg.CssClass = "LblNoRecord";
        lblErrorMsg.Visible = true;
        lblErrorMsg.Text = asMessage;
    }
    /// <summary>
    /// This method is used to set design a
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        hidResultOfStudentsNotGeneratedOnceYouPublish.Value = Resources.LocalizedResources.ResultOfStudentsNotGeneratedOnceYouPublish;
        hidOnceYouPublishTheResultItWillBeVisible.Value = Resources.LocalizedResources.OnceYouPublishTheResultItWillBeVisible;
        hidDoYouWantToSendMessageToTheStudents.Value = Resources.LocalizedResources.DoYouWantToSendMessageToTheStudents;
        hidThisActionWillOverwriteTheGraceMarksApplied.Value = Resources.LocalizedResources.ThisActionWillOverwriteTheGraceMarksApplied;
    }
    #endregion
}