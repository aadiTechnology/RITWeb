// File Name  : ClassTeacherTestMarksUI.aspx.cs
// Created By : 
// Date       : 
// Description: This class is used to publish/unpublish test result.

// Modified By : Vipul
// Date       : 23 Jan 2011
// Description: To create dynamic columns for exma status.

/* Modified By :Rohini
 * Date: 29 Apr 2013
 * Description: changes for pre-primary configuration.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using PushNotificationService;
using PayrollReportingUserEntities;
using System.Linq;
using SchoolEntities.Teacher;

public partial class ClassTeacherTestMarksUI : SchoolBase
{
    #region Constants

    private const int I_STANDARD_DIVISION_ID_DATAKEY_NUMBER = 0;
    private const int I_IS_SUMBITTED_DATAKEY_NUMBER = 1;
    private const int I_COL_INDEX_GENERATERESULT = 2;
    private const int I_TBL_INDEX_EXAM_STATUS = 1;

    private const string S_IMG_FOR_NONE_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryNotStart.gif";
    private const string S_IMG_FOR_PARTIAL_CONFIGURATION = "~/RITeSchool/images/icoGrid_MarkEntryPartDone.gif";
    private const string S_IMG_FOR_COMPLETE_CONFIGURATION = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
    private const string S_TOOLTIP_COMPLETE = "Progress Report entry Completed";
    private const string S_TOOLTIP_PARTIAL = "Progress Report entry partially done";
    private const string S_TOOLTIP_NOT_STARTED = "Progress Report entry not started";
    private const string S_SUBMITTED = "Submitted";
    private const string S_PUBLISHED = "Published";
    private const string S_UNDERSCORE = "-";
    private const string S_PUBLISH_MESSAGE = "Results for this exam has been published.";

    #endregion

    #region Data members

    private int miTeacherId;
    private int miStandardDivisionId;
    private int miTestId;
    private bool mbCanPublish = true;
    private bool mbIsPublish;
    private string msStatus;
    private bool mbToppersGenerated;
    private bool mbIsPrePrimaryClassTeacher;
    private bool mbIsPreprimaryExamConfiguartion;

    private DataSet moDsAllStdandardDivisions;

    #endregion

    #region Events

    /// <summary>
    /// This method is used to fill grid with standard-division and subjects. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            IsXseedApplicable();
            SetInitialControls();
            grdSubjects.Visible = tblStudentRow.Visible = false;
            cmbTests.Focus();
            SetQueryString();
            if (!IsPostBack)
            {
                if (CheckPreCondition())
                {
                    if (CheckUserRolesAndSetDisplay())
                        InitializeForm();
                }

                // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                btnUnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                chkSendMessage.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                SetUnPublistButtonState();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event method is used to process at data bound time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdSubjects_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                char cIsSubmitted = Convert.ToChar(grdSubjects.DataKeys[e.Row.RowIndex][I_IS_SUMBITTED_DATAKEY_NUMBER]);
                if (grdSubjects != null)
                    miStandardDivisionId = Convert.ToChar(grdSubjects.DataKeys[e.Row.RowIndex][I_STANDARD_DIVISION_ID_DATAKEY_NUMBER]);
                HyperLink oHlnkSubject;

                // This flag indicates that marks of this subject is not submitted by its subject teacher.
                // So make this row readonly.
                if (cIsSubmitted != Constants.C_YES)
                {
                    e.Row.CssClass = "GridRowDisable";
                    oHlnkSubject = (HyperLink)e.Row.Cells[0].Controls[0];
                    if (e.Row.Cells[grdSubjects.Columns.Count - 1].Controls.Count > 0)
                    {
                        HyperLink ohlnkSubjectEdit = (HyperLink)e.Row.Cells[grdSubjects.Columns.Count - 1].Controls[0];
                        ohlnkSubjectEdit.Text = S_UNDERSCORE;
                        ohlnkSubjectEdit.Enabled = false;
                    }

                    mbCanPublish = oHlnkSubject.Enabled = false;
                    hlnkToppers.Attributes.Remove("onclick");
                    hlnkToppers.Visible = Settings.ShowTopppers;
                }
                else
                {
                    // Get hyperlink of subject name and encrypt its navigation url.
                    oHlnkSubject = (HyperLink)e.Row.Cells[0].Controls[0];
                    string sUrl = oHlnkSubject.NavigateUrl;
                    StringBuilder sQueryString = new StringBuilder();
                    sQueryString.Append(sUrl.Substring(sUrl.IndexOf("?") + 1) + "&TeacherId=" + miTeacherId);
                    oHlnkSubject.NavigateUrl = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString.ToString());

                    // Add "edit  marks" link image
                    if (grdSubjects != null)
                        oHlnkSubject = (HyperLink)e.Row.Cells[grdSubjects.Columns.Count - 1].Controls[0];
                    oHlnkSubject.ImageUrl = "~/RITeSchool/images/IconGrid_Edit.gif";
                    sUrl = oHlnkSubject.NavigateUrl;
                    sQueryString = new StringBuilder();
                    sQueryString.Append(sUrl.Substring(sUrl.IndexOf("?") + 1) + "&CanOverride=" + bool.TrueString + "&TeacherId=" + miTeacherId);
                    sQueryString.Append(!mbIsPublish ? "&IsReadOnly=False&IsPublish=N" : "&IsReadOnly=True&IsPublish=Y");
                    oHlnkSubject.NavigateUrl = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString.ToString());
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill student details as per the class teacher selection.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
			miStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);            
            chkSendMessage.Checked = false;
            FillTestCombobox();
            SetTeacherDetails();
            SetUnPublistButtonState();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }    

    /// <summary>
    /// This method is used to select test combo.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            grdSubjects.Visible = tblStudentRow.Visible = true;
            tdToppers.Visible = chkSendMessage.Checked = false;
            tdErr.Visible = false;
            if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
                miStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
			else if (moUserRole == Constants.UserRoles.Teacher)
			{
				miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
				miStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
			}
            FillRespectiveGrid();
            miTestId = int.Parse(cmbTests.SelectedValue);
            SetTeacherDetails();
            SetUnPublistButtonState();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event method is used to publish marks 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            miStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
            miTestId = Convert.ToInt32(cmbTests.SelectedValue);

            /* We required classname so we split class name from teacher dropdown with :(Colon) Symbol i.e. Before colon symbol it havinmg classname like StandardName- DivisionName*/
            string asClassName = cmbTeachers.SelectedItem.Text;
            asClassName = asClassName.Substring(0, asClassName.IndexOf(":"));

            // If login user is teacher having extra access or supervisor or admin the take teacher id from selected dropdown.
            if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
                miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            else if (moUserRole == Constants.UserRoles.Teacher)
                miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStandardDivisionId, miTestId);

            if (oSwStdDivTestMasterBL.Standerd_division_Id == 0)
            {
                oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL
                                            {
                                                School_id = miSchoolId,
                                                Acadmic_year_id = miAcademicYearId,
                                                Standerd_division_Id = miStandardDivisionId,
                                                SchoolWise_Test_Id = miTestId,
                                                Is_Published = Constants.C_YES,
                                                Inserted_By_id = miUserId
                                            };

                TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
                if (!oTeacherStandardDetailsBL.IsTeacherPrePrimary(miSchoolId, miAcademicYearId, miTeacherId))
                    oSwStdDivTestMasterBL.CheckGradeConfigurations();
                oSwStdDivTestMasterBL.InsertSchoolWiseStandaredDivisionTestMaster();
                oSwStdDivTestMasterBL.PublishTestMarks();
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                btnPublish.Enabled = trSendMessage.Visible = false;

                SetTopperslinkURL();
                btnViewProgress.Enabled = false;
                SetPublishControls(true);
                grdStudents.DataBind();
                mbIsPublish = true;
                
                /*need to send these pushnotifications only in case of send message flag will selected.*/
                if (chkSendMessage.Checked)
                    SendPushNotification(miStandardDivisionId.ToString(), asClassName);
            }

            FillRespectiveGrid();

            if (chkSendMessage.Checked)
                SendMessageToStudent();

            SetUnPublistButtonState();
        }
        catch (ResultNotAvailableForOtherDiv oEx)
        {
            FillRespectiveGrid();
            tdErr.Visible = true;
            lblError.Visible = true;
            lblError.Text = oEx.Message;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnGenerateToppers_Click(object sender, EventArgs e)
    {
        try
        {
            miStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
            miTestId = Convert.ToInt32(cmbTests.SelectedValue);

            // If login user is teacher having extra access or supervisor or admin the take teacher id from selected dropdown.
            if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
                miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            else if (moUserRole == Constants.UserRoles.Teacher)
                miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL(miSchoolId, miAcademicYearId, miStandardDivisionId, miTestId);

            // If test is not published there is no entry in this table
            if (oSwStdDivTestMasterBL.Standerd_division_Id == 0)
            {
                oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL
                {
                    School_id = miSchoolId,
                    Acadmic_year_id = miAcademicYearId,
                    Standerd_division_Id = miStandardDivisionId,
                    SchoolWise_Test_Id = miTestId,
                    Is_Published = Constants.C_YES,
                    Inserted_By_id = miUserId
                };

                oSwStdDivTestMasterBL.GenerateTestTotalMarks();
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                SetTopperslinkURL();
            }

            FillStandardwiseDivisionsAndSubjectsInGrid();
        }
        catch (ResultNotAvailableForOtherDiv oEx)
        {
            FillRespectiveGrid();
            tdErr.Visible = true;
            lblError.Visible = true;
            lblError.Text = oEx.Message;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle event  of paging changed by dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void PageDropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // Retrieve the pager row.
            GridViewRow oPagerRow = grdStudents.BottomPagerRow;

            // Retrieve the PageDropDownList DropDownList from the bottom pager row.
            DropDownList oPageList = (DropDownList)oPagerRow.Cells[0].FindControl("PageDropDownList");

            // Set the PageIndex property to display that page selected by the user.
            grdStudents.PageIndex = oPageList.SelectedIndex;
            FillStudentGrid();
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to handle event  of record selected by gridview datasource.
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
                    trTotalRec.Visible = e.ReturnValue.ToString() == Constants.S_ZERO ? false : true;

                    if (e.ReturnValue.GetType() != typeof(DataTable))
                    {
                        if (Convert.ToInt32(lblEndIndex.Text) > Convert.ToInt32(lblTotal.Text))
                            lblEndIndex.Text = e.ReturnValue.ToString();
                    }

                    if (lblTotal.Text != string.Empty)
                            trTotalRec.Visible = Convert.ToInt32(lblTotal.Text) <= Constants.I_GRID_PAGE_COUNT ? false : true;
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Grid events

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
    /// This method is used handle sorting of gridview 
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
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  method is used to row data bound 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_RowDatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                HyperLink oHyperLinkField = (HyperLink)e.Row.Cells[I_COL_INDEX_GENERATERESULT].Controls[0];
                string sUrl = Convert.ToBoolean(hidIsMonthConfig.Value) == false ? oHyperLinkField.NavigateUrl : oHyperLinkField.NavigateUrl.Replace("~/RITeSchool/Teacher/PrePrimaryProgressSheetEntry.aspx", "~/RITeSchool/Teacher/StudentProgressReportEntry.aspx");

                StringBuilder sQueryString = new StringBuilder();
				sQueryString.Append(sUrl.Substring(sUrl.IndexOf("?") + 1) + "&TeacherId=" + cmbTeachers.SelectedValue + "&StandardDivisionId="
                                        + hidStandardDivisionId.Value + "&TestId=" + cmbTests.SelectedValue
                                        + "&pIndex=" + grdStudents.PageIndex.ToString()
                                        + "&pSortExp=" + hidSortExpression.Value
                                        + "&pSortDirc=" + hidSortDirection.Value
                                        + "&From=ExamResult");
                sQueryString.Append(!mbIsPublish ? "&IsReadOnly=False&IsPublish=N" : "&IsReadOnly=True&IsPublish=Y");

                oHyperLinkField.NavigateUrl = sUrl.Substring(0, sUrl.IndexOf("?") + 1) + CommonUtility.EncryptQuerystring(sQueryString.ToString());

                if (oHyperLinkField != null)
                {
                    string sStatus = grdStudents.DataKeys[e.Row.RowIndex][1].ToString();
                    switch (sStatus)
                    {
                        case "Not Started":
                            oHyperLinkField.ImageUrl = S_IMG_FOR_NONE_CONFIGURATION;
                            oHyperLinkField.Text = S_TOOLTIP_NOT_STARTED;
                            oHyperLinkField.ToolTip = S_TOOLTIP_NOT_STARTED;
                            break;
                        case "Partial":
                            oHyperLinkField.ImageUrl = S_IMG_FOR_PARTIAL_CONFIGURATION;
                            oHyperLinkField.Text = S_TOOLTIP_PARTIAL;
                            oHyperLinkField.ToolTip = S_TOOLTIP_PARTIAL;
                            break;
                        case "Complete":
                            oHyperLinkField.ImageUrl = S_IMG_FOR_COMPLETE_CONFIGURATION;
                            oHyperLinkField.Text = S_TOOLTIP_COMPLETE;
                            oHyperLinkField.ToolTip = S_TOOLTIP_COMPLETE;
                            break;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Pager)
            {
                GridViewRow oPagerRow = e.Row;

                // Retrieve the DropDownList and Label controls from the row.
                DropDownList oPageList = (DropDownList)oPagerRow.Cells[0].FindControl("PageDropDownList");
                Label oPageLabel = (Label)oPagerRow.Cells[0].FindControl("CurrentPageLabel");

                if (oPageList != null)
                {
                    // Create the values for the DropDownList control based on 
                    // the  total number of pages required to display the data
                    // source.
                    for (int iStudentNo = 0; iStudentNo < grdStudents.PageCount; iStudentNo++)
                    {
                        // Create a ListItem object to represent a page.
                        int iPageNumber = iStudentNo + 1;
                        ListItem oItem = new ListItem(iPageNumber.ToString());

                        // If the ListItem object matches the currently selected
                        // page, flag the ListItem object as being selected. Because
                        // the DropDownList control is recreated each time the pager
                        // row gets created,  will persist the selected item in
                        // the DropDownList control.   
                        if (iStudentNo == grdStudents.PageIndex)
                            oItem.Selected = true;

                        // Add the ListItem object to the Items collection of the 
                        // DropDownList.
                        oPageList.Items.Add(oItem);
                    }
                }

                if (oPageLabel != null)
                {
                    // Calculate the current page number.
                    int iCurrentPage = grdStudents.PageIndex + 1;

                    // Update the Label control with the current page information.
                    oPageLabel.Text = "Page " + iCurrentPage +
                      " of " + grdStudents.PageCount;
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  event is used to create grdview row.
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
                int iSortColumnIndex = CommonUtility.GetSortColumnIndex(sGridviewName, hidSortExpression.Value);

                if (iSortColumnIndex != -1)

                    // Call the AddSortImage helper method to add
                    // a sort direction image to the appropriate
                    // column header. 
                    CommonUtility.AddSortImage(iSortColumnIndex, e.Row, hidSortDirection.Value);
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  method is used to check is  progress sheet published or not before datatabinding.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_DataBinding(object sender, EventArgs e)
    {
        try
        {
            grdStudents.Enabled = true;
            msStatus = StudentBL.getPrePrimaryProgressSheetCompleteStatus(miSchoolId, Convert.ToInt32(hidStandardDivisionId.Value), Convert.ToInt32(cmbTests.SelectedValue), miAcademicYearId);
			if (Convert.ToBoolean(hidIsMonthConfig.Value))
			{
				if (msStatus == S_SUBMITTED)
				{
					mbCanPublish = true;
					btnGenerateToppers.Enabled = !mbToppersGenerated;
					btnViewProgress.Enabled = false;
					PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
					DataTable oDataTable = oPrePrimaryProgressSheetConfigBL.GetIncompleteProgressRollNos(miSchoolId, miAcademicYearId, miTeacherId, Convert.ToInt32(cmbTests.SelectedValue));
					GenerateIncompleteProgressAlert(oDataTable);
				}
				else
				{
					mbCanPublish = false;
					mbIsPublish = true;
					trSendMessage.Visible = false;
					btnViewProgress.Enabled = false;
				}
				
				btnGenerateToppers.Enabled = !mbToppersGenerated;
				btnPublish.Enabled = true;
				btnUnPublish.Enabled = true;
				tdErr.Visible = false;
			}
	        else if (msStatus == S_SUBMITTED)
            {
                mbCanPublish = true;
                SetPublishControls(!mbCanPublish);
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                btnPublish.Enabled = true;
	            btnUnPublish.Enabled = false;
				btnViewProgress.Enabled = false;
                PrePrimaryProgressSheetConfigBL oPrePrimaryProgressSheetConfigBL = new PrePrimaryProgressSheetConfigBL();
                DataTable oDataTable = oPrePrimaryProgressSheetConfigBL.GetIncompleteProgressRollNos(miSchoolId, miAcademicYearId, miTeacherId, Convert.ToInt32(cmbTests.SelectedValue));
                GenerateIncompleteProgressAlert(oDataTable);
				tdErr.Visible = false;
            }
            else if (msStatus == S_PUBLISHED)
            {
                mbCanPublish = false;
                mbIsPublish = true;
                SetPublishControls(!mbCanPublish);
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                btnPublish.Enabled = trSendMessage.Visible = false;
				btnUnPublish.Enabled = true;
				btnViewProgress.Enabled = false;
				tdErr.Visible = true;
				lblError.Text = S_PUBLISH_MESSAGE;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  event is used to set control enability and disabililty.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudents_DataBound(object sender, EventArgs e)
    {
        try
        {
            btnGenerateToppers.Enabled = !mbToppersGenerated;
            if (msStatus != S_SUBMITTED && msStatus != S_PUBLISHED && (!Convert.ToBoolean(hidIsMonthConfig.Value)))
            {
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                btnPublish.Enabled = trSendMessage.Visible = false;
                btnViewProgress.Enabled = false;
                btnUnPublish.Enabled = false;
                if (grdStudents.Rows.Count > 0)
                {
                    mbCanPublish = false;
                    btnGenerateToppers.Enabled = !mbToppersGenerated;
                    btnPublish.Enabled = trSendMessage.Visible = false;
                    btnViewProgress.Enabled = false;
                    tdErr.Visible = true;
                    lblError.Visible = true;
                    lblError.Text = "Progress Report have not yet been submitted.";
                    grdStudents.Enabled = false;
                }
                else
                {
                    tdErr.Visible = false;
                    lblError.Visible = false;
                }
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region private Methods

    /// <summary>
    ///  method is used to send message to student.
    /// </summary>
    private void SendMessageToStudent()
    {
        /////////////////////// SMS Template
        int iSmsId = Convert.ToInt32(Constants.SMSTemplate.ExamPublishSMS);
        string sLoginDetailsSmsText = string.Empty;
        string sSmsSubject = string.Empty;

        DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
        if (oDTSmsTemplate.Rows.Count != 0)
            if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
        ////////////////////////
        string sClsTchrName = cmbTeachers.SelectedItem.Text;
        string sClass = sClsTchrName.Substring(0, sClsTchrName.IndexOf(':')).Trim();
        string sClsTeacher = sClsTchrName.Substring(sClsTchrName.IndexOf(':') + 1, ((sClsTchrName.Length - 1) - sClsTchrName.IndexOf(':'))).Trim();
        string sMessageBody = sLoginDetailsSmsText;
        sMessageBody = hidDependentExamNames.Value == string.Empty ? sMessageBody.Replace("%EXAM%", cmbTests.SelectedItem.Text) : sMessageBody.Replace("%EXAM%", hidDependentExamNames.Value);
        sMessageBody = sMessageBody.Replace("%EXAM%", hidDependentExamNames.Value);
        sMessageBody = sMessageBody.Replace("%STUDENTCLASS%", sClass);
        sMessageBody = sMessageBody.Replace("%CLASSTEACHER%", sClsTeacher);
        using (DataTable oDtUserId = StudentBL.GetAllStudentsByGivenStdDivs(miSchoolId, miAcademicYearId, hidStandardDivisionId.Value,false))
        {
            hidUserID.Value = Constants.S_EMPTY_STRING;
            if (oDtUserId.Rows.Count > 0)
            {
                for (int iCount = 0; iCount < oDtUserId.Rows.Count; iCount++)
                    hidUserID.Value += oDtUserId.Rows[iCount]["ID"] + ";";
                hidUserID.Value = hidUserID.Value.Substring(0, hidUserID.Value.LastIndexOf(";"));
                string sSubject = hidDependentExamNames.Value == string.Empty ? cmbTests.SelectedItem.Text : hidDependentExamNames.Value;
                SendMessage(hidUserID.Value, sSubject + " Result", sMessageBody);
            }
        }
        chkSendMessage.Checked = false;
    }

    /// <summary>
    ///  method is used to send the message.
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="asMsgSubject"></param>
    /// <param name="asMsgBody"></param>
    private void SendMessage(string asUserId, string asMsgSubject, string asMsgBody)
    {
        Message oMessage = new Message {sMessageBody = asMsgBody, sMessageSubject = asMsgSubject};
        oMessage.SetMessageReceivers(asUserId, miUserId);
        oMessage.InsertMessageDetails(
                 miUserId,
                 Convert.ToInt32(moUserRole),
                 miAcademicYearId);
    }

    /// <summary>
    ///  method is used to initialize from.
    /// </summary>
    private void InitializeForm()
    {
        ApplyMouseHoverEffect(new List<Button> { btnPublish, btnUnPublish, btnViewProgress, btnGenerateToppers });
        SetDefaultSortGridArrow();
        grdSubjects.EmptyDataText = Constants.S_BLANK_GRID_MESSAGE;
        FillTeachersComboBox();

        FillTestCombobox();
        cmbTests.SelectedValue = SchoolWiseTestMasterBL.GetLatestExamId(miSchoolId, miAcademicYearId, 0, 0).ToString();

        GetQueryString();

        if (moUserRole == Constants.UserRoles.Teacher)
        {
            if (Request.QueryString.ToString().IsNullOrEmpty())
                FillRespectiveGrid();
            if (Convert.ToBoolean(hidIsMonthConfig.Value))
            {
                btnPublish.Visible = trSendMessage.Visible = true;
                btnUnPublish.Enabled = true;
                btnUnPublish.Visible = true;
                btnViewProgress.Visible = false;
            }
        }

		SetPublishButtonAttribute(Convert.ToBoolean(hidIsMonthConfig.Value));
		if (moUserRole == Constants.UserRoles.Admin
			|| moUserRole == Constants.UserRoles.Supervisor ||
			bool.Parse(hidUserHasFullAccess.Value))
			hyplnkTransferStudentMarks.Visible = true;
    }

	/// <summary>
	/// This method is used to set publish button attribute, when pre-primary configuration is selected button will open new screen other wise confirmation message is displayed.
	/// </summary>
	/// <param name="abIsMonthConfig"></param>
	private void SetPublishButtonAttribute(bool abIsMonthConfig)
	{
		if (!abIsMonthConfig)
			btnPublish.Attributes.Add("Onclick",
			                          "if(!(ConfirmAction('" + hidExamDependencyMsg.ClientID + "','" +
			                          hidDependentExamNames.ClientID + "','" +
			                          (Settings.AllowPartialSubmit ? Constants.S_YES : Constants.S_NO) + "'))){return false;}");
		else
		{
			string sQuerystring = "StandardDivisionId=" + miStandardDivisionId;
			string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
			btnPublish.Attributes.Add("onclick",
			                          "window.open('./SubmitProgreesReportResult.aspx?" + sEncrypt +
			                          "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=800,height=500'); return false;");
		}
	}

	/// <summary>
    /// Generate alert for incmplete progress report.
    /// </summary>
    /// <param name="aoDataTable"></param>
    private void GenerateIncompleteProgressAlert(DataTable aoDataTable)
    {
        string sAlert = string.Empty;
        if (aoDataTable != null)
            foreach (DataRow oDataRow in aoDataTable.Rows)
            {
                sAlert = sAlert + "Progress report entry is " + Convert.ToString(oDataRow["Status"]) + " for\n";
                sAlert = sAlert + "Roll Nos : " + Convert.ToString(oDataRow["RollNos"]) + "\n";
            }

        hidAlert.Value = sAlert;
    }

    /// <summary>
    /// Used to set attribute of ubpublish button to open a popup of reasoning.
    /// </summary>
    private void SetUnpublishButtonAttributes()
    {
        miStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
        miTestId = Convert.ToInt32(cmbTests.SelectedValue);
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor ||
            bool.Parse(hidUserHasFullAccess.Value))
            miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
        else if (moUserRole == Constants.UserRoles.Teacher)
            miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
        if (!Convert.ToBoolean(hidIsMonthConfig.Value))
        {
			string sQuerystring = "StandardDivisionId=" + miStandardDivisionId + "&sTeacherName=" + cmbTeachers.SelectedItem.Text +
                "&TeacherId=" + miTeacherId + "&TestId=" + miTestId + "&sTestName=" + cmbTests.SelectedItem.Text;
            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
            btnUnPublish.Attributes.Add("onclick", "window.open('../Admin/TestUnpublishPopUp.aspx?" + sEncrypt + "','_blank','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370');return false;");
        }
        else
        {
            string sQuerystring = "StandardDivisionId=" + miStandardDivisionId + "&IsUnpublish=true";
            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
            btnUnPublish.Attributes.Add("onclick", "window.open('./SubmitProgreesReportResult.aspx?" + sEncrypt + "' , '_new','scrollbars=no,resizable=no,menubar=no,status=no,titlebar=no,toolbar=no,top=200,left=300,width=800,height=500'); return false;");
        }
    }

    /// <summary>
    /// This method is used to set default sort arrow in grid.
    /// </summary>
    private void SetDefaultSortGridArrow()
    {
        hidSortExpression.Value = grdStudents.Columns[0].SortExpression;
        hidSortDirection.Value = Constants.S_ASCENDING;
        tdToppers.Visible = false;
    }

    /// <summary>
    /// This method fills the combobox for the tests.
    /// </summary>
    private void FillTestCombobox()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);       

        if (cmbTests.SelectedValue != Constants.S_ZERO)
            hidOldExamId.Value = cmbTests.SelectedValue;

        int iStandardDivId = Constants.I_ZERO;
        if (cmbTeachers.SelectedValue.ToInt() == Constants.I_ZERO)
            iStandardDivId = QueryString["StandardDivisionId"].ToInt();
        else
            iStandardDivId = cmbTeachers.SelectedValue.ToInt();

            using (DataTable oDsAllTests = oTestCollectionBL.GetAllTestsForClass(iStandardDivId))
            {
                ControlUtility.FillDropDownList(
                    oDsAllTests,
                    ref cmbTests,
                    Constants.S_TEST_ID_FIELD,
                    Constants.S_TEST_NAME_FIELD,
                   Constants.S_SELECT );
            }
        
        
        ListItem oListItem = cmbTests.Items.FindByValue(hidOldExamId.Value);

        if (oListItem != null)
        {
            oListItem.Selected = true;
            FillRespectiveGrid();
        }
        else
            hidOldExamId.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// This method is used to fill standard- divisions and subjects 
    /// </summary>
    private void FillStandardwiseDivisionsAndSubjectsInGrid()
    {
        miTestId = int.Parse(cmbTests.SelectedValue);
        if (miStandardDivisionId == 0)
            miStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
        TeacherSubjectAssignmentCollectionBL oTeacherSubjectAssignmentCollectionBL = new TeacherSubjectAssignmentCollectionBL(miTeacherId);
        moDsAllStdandardDivisions = oTeacherSubjectAssignmentCollectionBL.RetriveSubjectsDetailsForClassTeacher(miSchoolId, miAcademicYearId, miStandardDivisionId, miTestId);
        AddExamStatusColumns();
        mbIsPublish = oTeacherSubjectAssignmentCollectionBL.IsPublished;
        mbToppersGenerated = oTeacherSubjectAssignmentCollectionBL.ToppersGenerated;
        grdSubjects.DataSource = moDsAllStdandardDivisions.Tables[0];
        grdSubjects.DataBind();
        if (grdSubjects.Rows.Count == 0)
            mbCanPublish = false;
        SetPublishControls(mbIsPublish);
        grdSubjects.Visible = tblStudentRow.Visible = true;
        if (moDsAllStdandardDivisions.Tables[0].Rows.Count == Constants.I_ZERO)
            tblStudentRow.Visible = false;

        FillGrid();
		SetPublishButtonAttribute(Convert.ToBoolean(hidIsMonthConfig.Value));
    }

    /// <summary>
    /// This mwthod is used to fill grid.
    /// </summary>
    private void FillGrid()
    {
        for (int iRowIndex = 0; iRowIndex < grdSubjects.Rows.Count; iRowIndex++)
        {
            int iSubjectId = Convert.ToInt32(grdSubjects.DataKeys[iRowIndex]["Subject_Id"]);
            DataRow[] oDrStudentCount = moDsAllStdandardDivisions.Tables[2].Select("Subject_Id=" + iSubjectId);

            foreach (DataRow oDataRow in oDrStudentCount)
            {
                ((Label)grdSubjects.Rows[iRowIndex].FindControl("lbl" + oDataRow["ExamStatus"].ToString().Trim())).Text = (oDataRow["Is_Submitted"].ToString() != Constants.S_YES) ? S_UNDERSCORE : oDataRow["Count"].ToString();
            }
        }
    }

    /// <summary>
    /// This method is used to add exam status columns.
    /// </summary>
    private void AddExamStatusColumns()
    {
        TemplateField oCustomField;

        // When class is changed twice then remove previously added columns, except first two.
        while (grdSubjects.Columns.Count != 2)
            grdSubjects.Columns.RemoveAt(grdSubjects.Columns.Count - 2);

        foreach (DataRow oDataRow in moDsAllStdandardDivisions.Tables[I_TBL_INDEX_EXAM_STATUS].Rows)
        {
            oCustomField = new TemplateField
            {
                HeaderTemplate =
                    new GridViewLabelTemplate(DataControlRowType.Header, Convert.ToString(oDataRow["ExamStatus"])),

                ItemTemplate =
                    new GridViewLabelTemplate(DataControlRowType.DataRow, Convert.ToString(oDataRow["ExamStatus"]))
            };
            oCustomField.ItemStyle.Width = Unit.Pixel(180);
            oCustomField.ItemStyle.Wrap = false;
            oCustomField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdSubjects.Columns.Insert(grdSubjects.Columns.Count - 1, oCustomField);
        }
    }

    /// <summary>
    /// This method is used to fill respective grid 
    /// </summary>
    private void FillRespectiveGrid()
    {
        int iTeacherId = 0;
            iTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
        TeacherStandardDetailsBL oTeacherStandardDetailsBL = new TeacherStandardDetailsBL();
        // If teacher is preprimary then show class students with their progressheeet entry status.
        mbIsPrePrimaryClassTeacher = oTeacherStandardDetailsBL.IsTeacherPrePrimary(miSchoolId, miAcademicYearId, iTeacherId);
	    mbIsPreprimaryExamConfiguartion = oTeacherStandardDetailsBL.IsPreprimaryExamConfiguration(miSchoolId, miAcademicYearId, iTeacherId, moUserRole.ToString());
	    cmbTests.Enabled = true;
		if (mbIsPreprimaryExamConfiguartion)
        {
            trgrdSubjects.Visible = false;
            trStudentGrid.Visible = true;
            tdToppers.Visible = false;
	        btnGenerateToppers.Visible = false;
            FillStudentGrid();
        }
        else
        {
			hidIsMonthConfig.Value = "false";
            trStudentGrid.Visible = false;
            trgrdSubjects.Visible = true;
            tdToppers.Visible = Settings.ShowTopppers;
            FillStandardwiseDivisionsAndSubjectsInGrid();
        }

        SetUnpublishButtonAttributes();
        CheckPublishExamDependency();
    }

    /// <summary>
    /// This method is used to check publish exam dependency.
    /// </summary>
    private void CheckPublishExamDependency()
    {
        miStandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
        miTestId = Convert.ToInt32(cmbTests.SelectedValue);
        SchoolWiseStanderedDivisionTestMasterBL oSwStdDivTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL
                                                                            {
                                                                                School_id = miSchoolId,
                                                                                Acadmic_year_id = miAcademicYearId,
                                                                                Standerd_division_Id = miStandardDivisionId,
                                                                                SchoolWise_Test_Id = miTestId,
                                                                                Is_Published = Constants.C_YES,
                                                                                Inserted_By_id = miUserId
                                                                            };
        oSwStdDivTestMasterBL.CheckPublishExamDependency();
        hidDependentExamNames.Value = oSwStdDivTestMasterBL.lstPublishExamDependencyMaster[0].DependentExamName.ToString();
        hidExamDependencyMsg.Value = oSwStdDivTestMasterBL.lstPublishExamDependencyMaster[0].ExamDependentMessage.ToString();
    }

    /// <summary>
    /// This method is used to fill student grid
    /// </summary>
    private void FillStudentGrid()
    {
        trTotalRec.Visible = true;
        int iTeacherId = moUserRole == Constants.UserRoles.Teacher && (!bool.Parse(hidUserHasFullAccess.Value)) ? Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]) : Convert.ToInt32(cmbTeachers.SelectedValue);        
		TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
        bool bIsMonthConfig = oTeacherStandardDetailsCollectionBL.IsMonthConfiguration(miStandardDivisionId==Constants.I_ZERO?cmbTeachers.SelectedValue.ToInt():miStandardDivisionId);
        hidIsMonthConfig.Value = bIsMonthConfig.ToString();
		hidStandardDivisionId.Value = miStandardDivisionId == Constants.I_ZERO ? cmbTeachers.SelectedValue : miStandardDivisionId.ToString();
		SetPublishButtonAttribute(bIsMonthConfig);
    }

    /// <summary>
    /// This methos is used to set publish controls
    /// </summary>
    /// <param name="abIsPublish"></param>
    private void SetPublishControls(bool abIsPublish)
    {
		if (mbIsPreprimaryExamConfiguartion)
		{
			if (Convert.ToBoolean(hidIsMonthConfig.Value))
			{
				btnPublish.Enabled = abIsPublish;
				btnUnPublish.Enabled = abIsPublish;
				btnViewProgress.Visible = false;
			}
		}
		else
		{
			hidPublish.Value = abIsPublish.ToString();
			if (grdSubjects.Rows.Count == 0)
				tdToppers.Visible = false;
			hidStandardDivisionId.Value = miStandardDivisionId.ToString();
			if (abIsPublish)
			{
				mbCanPublish = false;
				btnViewProgress.Enabled = true;
				btnUnPublish.Enabled = true;
				tdErr.Visible = true;
				lblError.Visible = true;
				SetTopperslinkURL();
				lblError.Text = S_PUBLISH_MESSAGE;
			}

			if (mbCanPublish && (grdSubjects.Rows.Count > 0 || grdStudents.Rows.Count > 0))
			{
				btnGenerateToppers.Enabled = !mbToppersGenerated;
				btnPublish.Enabled = trSendMessage.Visible = true;
				btnViewProgress.Enabled = true;
				btnUnPublish.Enabled = false;
				tdErr.Visible = false;
				lblError.Text = string.Empty;
				if (mbToppersGenerated)
					SetTopperslinkURL();
				else
					hlnkToppers.Attributes.Remove("onclick");
			}
			else
			{
				if ((!mbCanPublish) && (!abIsPublish))
				{
					tdErr.Visible = true;
					lblError.Visible = true;
					lblError.Text = "Not all results for this exam have been submitted.";
					btnUnPublish.Enabled = false;
				}
				
				btnViewProgress.Enabled = !abIsPublish;

				if (!mbCanPublish)
				{
					trSendMessage.Visible = false;
					btnPublish.Enabled = btnViewProgress.Enabled = btnGenerateToppers.Enabled = false;
				}
				else
				{
					btnGenerateToppers.Enabled = !mbToppersGenerated;
				}

				if (grdSubjects.Rows.Count == 0 && !abIsPublish)
					btnUnPublish.Enabled = false;
			}

			AddPrintAttributs();
			btnViewProgress.Attributes.Add("onclick", "ShowProgressSheet();return false;");
		}
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void SetTopperslinkURL()
    {
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(miStandardDivisionId);
        string sQueryString = "ExamType=0&ToppersType=1&StdDivId=" + miStandardDivisionId + "&TestId="
            + cmbTests.SelectedValue + "&StdId=" + oStandardDivisionMasterBL.StandardId;
        sQueryString = "../Student/ExamToppersUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
        hlnkToppers.Attributes.Add("onclick", "ShowToppers('" + sQueryString + "');return false;");
    }

    /// <summary>
    /// This function is used to Check UserRoles And Set Display
    /// </summary>
    /// <returns></returns>
    private bool CheckUserRolesAndSetDisplay()
    {
        bool bResult = false;
        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
        {
            VisibleHideCombo(true);
            btnUnPublish.Visible = true;
            if (IsPostBack)
                miTeacherId = Convert.ToInt32(cmbTeachers.SelectedValue);
            string sQueryString = CommonUtility.EncryptQuerystring("TeacherId=" + miStandardDivisionId);
            hyplnkTransferStudentMarks.NavigateUrl = hyplnkTransferStudentMarks.NavigateUrl + "?" + sQueryString;
            bResult = true;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.ExamResults).ToString();
            if (bool.Parse(hidUserHasFullAccess.Value))
            {
                VisibleHideCombo(true);
                btnUnPublish.Visible = true;
            }
            else
            {
                btnUnPublish.Visible = true;
            }

            miTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            string sQueryString = CommonUtility.EncryptQuerystring("TeacherId=" + miTeacherId);
            hyplnkTransferStudentMarks.NavigateUrl = hyplnkTransferStudentMarks.NavigateUrl + "?" + sQueryString;
            bResult = true;
        }

        return bResult;
    }

    /// <summary>
    /// This method is used to hide combo box as per the filter selected.
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideCombo(bool abAction)
    {
        cmbTeachers.Visible = abAction;
        tdTeacher.Visible = abAction;
    }

    private void SetUnPublistButtonState()
    {
        if (SchoolBase.Settings.BlockExamPublish == true)
        {
            ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            List<ReportingUserConfiguration> lstUsers = oReportingUserConfigurationBL.GetAll();

            if (!(moUserRole == Constants.UserRoles.Admin || lstUsers.Any(ru => ru.ReportingPrameterId == Constants.ReportingParameters.AllowExamPublishAction.ToInt() && ru.UserId == miUserId)))
            {
                StudentProgress oStudentProgress = new StudentProgress();
                bool FinalPublished = oStudentProgress.IsFinalResultPublished(cmbTeachers.SelectedValue.ToInt());
                bool TermPublished = false;
                if (FinalPublished == false)
                {
                    string sStandardName = string.Empty;
                    TermPublished = oStudentProgress.IsTermExamPublished(cmbTeachers.SelectedValue.ToInt(), out sStandardName);
                }

                if (FinalPublished == true || TermPublished == true)
                    btnUnPublish.Enabled =false;
		    }
        }            
    }

    /// <summary>
    /// This method is used to fill teacher comobox.
    /// </summary>
    private void FillTeachersComboBox()
    {
        // get all class teachers

        DataTable oDt = SchoolWiseStandardDivisionTeacherAssignmentMasterBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);

        if (moUserRole == Constants.UserRoles.Teacher && !bool.Parse(hidUserHasFullAccess.Value))
        {
            DataRow[] oDataRow = oDt.Select("Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID]);
                ControlUtility.FillDropDownList(
                           oDataRow,
                           ref cmbTeachers,
                           Constants.S_STANDARD_DIVISION_ID_FIELD,
                           Constants.S_TEACHER_NAME_FIELD,
                           Constants.S_SELECT);
                if (oDataRow.Length == 1)
                {
                    cmbTeachers.SelectedIndex = 1;
                    cmbTeachers.Enabled = false;
                }
         }

   else   if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.ExamResults) != Constants.C_YES)
        {
            if (moSchool == Constants.SchoolId.PPSN)
            {
                AttendanceDetailsBL oAttendanceDetailsBL = new AttendanceDetailsBL();
                List<CoordinateDetails> lstCoordinatorDetails = oAttendanceDetailsBL.GetCoordinatorDetails(miSchoolId, miAcademicYearId);
                List<int> lstStandardIds = lstCoordinatorDetails.Where(ct => ct.UserId == miUserId).Select(ct => ct.StandardId).ToList();
                DataRow[] dtArray = null;
                    if (lstStandardIds.Count > 0)
                    {
                        dtArray = oDt.Select("(Standard_Id IN (" + string.Join(",", lstStandardIds) + ") OR Teacher_Id=" + Session[Constants.S_SESSION_TEACHER_ID] + ")");
                        if (dtArray.Length > 0)
                        {
                            var sortedRows = dtArray.AsEnumerable()
                            .OrderBy(row => row.Field<int>("Original_Standard_Id"))
                            .ThenBy(row => row.Field<int>("Original_Division_Id"));

                            oDt = sortedRows.CopyToDataTable();
                        }
                    }
                    else
                    {
                        dtArray = oDt.Select("Teacher_Id=" + Convert.ToString(Session[Constants.S_SESSION_TEACHER_ID]));
                        oDt = dtArray.CopyToDataTable();
                    }

                    ControlUtility.FillDropDownList(oDt, ref cmbTeachers,
                                                       Constants.S_STANDARD_DIVISION_ID_FIELD,
                                                       Constants.S_TEACHER_NAME_FIELD,
                                                       string.Empty);
                   
                if (oDt.Rows.Count == Constants.I_ONE)
                        cmbTeachers.Enabled = false;
             }
            ControlUtility.FillDropDownList(oDt, ref cmbTeachers,
                     Constants.S_STANDARD_DIVISION_ID_FIELD,
                       Constants.S_TEACHER_NAME_FIELD,
                     string.Empty);
        }

        else
        {
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                DataRow[] dr = oDt.Select("Is_Preprimary='N'");
                if (dr.Length > 0)
                    oDt = dr.CopyToDataTable();
            }
        
          ControlUtility.FillDropDownList(oDt, ref cmbTeachers,
        Constants.S_STANDARD_DIVISION_ID_FIELD,
          Constants.S_TEACHER_NAME_FIELD,
           Constants.S_SELECT);
          }
    }

    /// <summary>
    /// This method is used to get query string.
    /// </summary>
    private void GetQueryString()
    {
	    if (QueryString.Count <= 0)
		    return;

		if ((QueryString["StandardDivisionId"] != null && !QueryString["StandardDivisionId"].Trim().Equals("0")) || bool.Parse(hidUserHasFullAccess.Value))
	    {
		    miTeacherId = QueryString["TeacherId"].ToInt();
			miStandardDivisionId = QueryString["StandardDivisionId"].ToInt();
			cmbTeachers.SelectedValue = miStandardDivisionId.ToString();
	    }

	    if (QueryString["TestId"] != null)
	    {
            cmbTests.SelectedValue = QueryString["TestId"];
            if (QueryString["TeacherId"] != null && QueryString["TeacherId"] != Constants.S_ZERO)
            {
                cmbTeachers.SelectedValue = QueryString["TeacherId"];
            }
            else
            {                
                if (QueryString["pIndex"] != null)
                    grdStudents.PageIndex = QueryString["pIndex"].ToInt();
                if (QueryString["pSortExp"] != null)
                    hidSortExpression.Value = QueryString["pSortExp"];
                if (QueryString["pSortDirc"] != null)
                    hidSortDirection.Value = QueryString["pSortDirc"];
            }
		    FillRespectiveGrid();

	    }
    }
    
    /// <summary>
    /// This function checks the preconditons of Exams.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.ExamResults);
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }

        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls depends on configuration done or not.
    /// </summary>
    private void VisibleOrHideControls()
    {
        btnPublish.Visible = trSendMessage.Visible = false;
        tblFilter.Visible = false;
        btnViewProgress.Visible = false;
        btnUnPublish.Visible = false;
        HyperLinkProgressRem.Visible = false;
        HyperLinkCaptureHeightWeight.Visible = false;
    }

    /// <summary>
    /// This method is used to add attribute for print button.
    /// </summary>
    private void AddPrintAttributs()
    {
        string sQryStr = "mode=print&IsTeacherView=Y";
		if (moUserRole == Constants.UserRoles.Admin
			|| moUserRole == Constants.UserRoles.Supervisor || bool.Parse(hidUserHasFullAccess.Value))
			sQryStr = string.Format("{0}&iStdDivId={1}&iStudId=0&iTestId={2}", sQryStr, cmbTeachers.SelectedValue, cmbTests.SelectedValue);
		else if (moUserRole == Constants.UserRoles.Teacher)
			sQryStr = string.Format("{0}&iStdDivId={1}&iStudId=0&iTestId={2}", sQryStr, cmbTeachers.SelectedValue, cmbTests.SelectedValue);

        sQryStr = CommonUtility.EncryptQuerystring(sQryStr);
        hidQery.Value = sQryStr;
    }

    /// <summary>
    /// This method is used to sort grid.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to check isXseed applicable.
    /// </summary>
    private void IsXseedApplicable()
    {
        int iStandardId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        int iTeachersStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_STDDIV_ID]);
        XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
        if (oXseedProgressReportBL.IsXseedApplicable(miSchoolId, miAcademicYearId, iStandardId, iTeachersStandardDivisionId))
        {
            MasterPage oMasterPage = Master as MasterPage;
            oMasterPage.RedirectToNextPage("../Xseed/ClassTeacherXseedGradesUI.aspx");
        }
    }

    /// <summary>
    /// This method is used to visible Progress remark link.
    /// </summary>
    private void SetInitialControls()
    {
        SchoolWiseStanderedDivisionTestMasterBL oSchoolWiseStanderedDivisionTestMasterBL = new SchoolWiseStanderedDivisionTestMasterBL();
        HyperLinkProgressRem.Visible = oSchoolWiseStanderedDivisionTestMasterBL.IsPrePrimaryTeacher(miUserId, miSchoolId, miAcademicYearId) == false ? true : false;
        HyperLinkCaptureHeightWeight.Visible = oSchoolWiseStanderedDivisionTestMasterBL.IsPrePrimaryTeacher(miUserId, miSchoolId, miAcademicYearId) == false ? true : false;
        btnGenerateToppers.Visible = Settings.ShowTopppers;
        HyperLinkProgressRem.Attributes.Add("onclick", "if(!OpenStudentRemarks()) return false;");
        HyperLinkCaptureHeightWeight.Attributes.Add("onclick", "OpenTermwiseHeightWeight();return false;");
    }
   
   /// <summary>
   /// This method is used to send notification to the student
   /// </summary>
   /// <param name="asStandardDivisionId"></param>
   /// <param name="asClassName"></param>
    public override void SendPushNotification(string asStandardDivisionId, object asClassName)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {
            List<int> studentList = new List<int>();

            /* This code is used to get current Standard division i.e. Current Class student */
            string sStudentId = string.Empty;
             List<int> lstUserIds = new List<int>();
             using (DataTable oDtUserId = StudentBL.GetAllStudentsByGivenStdDivs(miSchoolId, miAcademicYearId, asStandardDivisionId, false))
            {
                var userIds = oDtUserId.AsEnumerable().Select(r => r["ID"].ToInt());                               
                foreach(var userID in userIds)              
                    lstUserIds.Add(userID);
            }
            
            pushNotificationClient = new PushNotificationClient();
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_CLASSNAME, Convert.ToString(asClassName));
            pushNotificationClient.SendNotification(NotificationMessageHeadings.ProgressReportAvailable, this.miSchoolId.ToString(), lstUserIds.ToArray(), dictionaryNotificationParameter);
            pushNotificationClient.Close();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                pushNotificationClient.Close();
        }
    }

    /// <summary>
    /// This method is used to set the Query String.
    /// </summary>
    private void SetQueryString()
    {
        string sQueryString = "StdDivId=" + cmbTeachers.SelectedValue + "&ExamId=" + cmbTests.SelectedValue + "&IsPrimary=" + Constants.I_ZERO;
        hidQueryString.Value = CommonUtility.EncryptQuerystring(sQueryString);
    }

    private void SetTeacherDetails()
    {
        if (cmbTeachers.SelectedValue != Constants.S_ZERO && cmbTests.SelectedValue != Constants.S_ZERO)
        {
            if (miStandardDivisionId != Constants.I_ZERO)
            {
                grdSubjects.Visible = tblStudentRow.Visible = true;
                trTotalRec.Visible = true;
                FillRespectiveGrid();
                if (mbIsPublish)
                {
                    SetTopperslinkURL();
                    btnPublish.Enabled = false;
                    btnUnPublish.Enabled = true;
                }
                else
                {
                    btnPublish.Enabled = mbCanPublish;
                    btnUnPublish.Enabled = false;
                }
                grdStudents.PageIndex = Constants.I_ZERO;
            }
            else
            {
                grdSubjects.Visible = tblStudentRow.Visible = false;
                trTotalRec.Visible = false;
                tdErr.Visible = false;
                lblError.Text = string.Empty;
                hlnkToppers.Attributes.Remove("onclick");
                btnUnPublish.Enabled = false;
                btnViewProgress.Enabled = false;
                btnGenerateToppers.Enabled = !mbToppersGenerated;
                btnPublish.Enabled = trSendMessage.Visible = false;
            }
            hlnkToppers.Visible = Settings.ShowTopppers;
            SetQueryString();
        }
    }

}

    #endregion

    #region    public methods

/// <summary>
/// 
/// </summary>
public class GridViewLabelTemplate : ITemplate
{
    private readonly DataControlRowType moTemplateType;
    private readonly string msColumnName;

    public GridViewLabelTemplate(DataControlRowType aoType, string asColname)
    {
        moTemplateType = aoType;
        msColumnName = asColname;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="aoContainer"></param>
    public void InstantiateIn(Control aoContainer)
    {
        // Create the content for the different row types.
        switch (moTemplateType)
        {
            case DataControlRowType.Header:
                // Create the controls to put in the header
                // section and set their properties.
                Literal oLc = new Literal { Text = "<b>" + msColumnName + "</b>" };

                // Add the controls to the Controls collection
                // of the container.
                aoContainer.Controls.Add(oLc);
                break;
            case DataControlRowType.DataRow:
                // Create the controls to put in a data row
                // section and set their properties.
                Label olblExamStatusCount = new Label {ID = "lbl" + msColumnName.Trim(), TabIndex = -1};
                
                // To support data binding, register the event-handling methods
                // to perform the data binding. Each control needs its own event
                // handler.

                // Add the controls to the Controls collection
                // of the container.
                aoContainer.Controls.Add(olblExamStatusCount);
                break;

            // Insert cases to create the content for the other 
            // row types, if desired.
            default:
                // Insert code to handle unexpected values.
                break;
        }
    }
}
#endregion