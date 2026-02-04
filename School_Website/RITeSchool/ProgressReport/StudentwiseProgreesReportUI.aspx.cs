/* File Name - StudentwiseProgreesReportUI.aspx.cs
 * Created Date - 22-Oct-2011
 * Created by - Vipul
 * Class Description - This class is used for selecting student for student wise marks assignment.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Linq;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
using System.Data;
using SchoolEntities;

public partial class StudentwiseProgreesReportUI : SchoolBase
{
    const string S_DEFAULT_SORT_EXP = "Roll_No";  
    private StudentBL moStudentBL;
    private Constants.MarkAssignmentStatus ohyp;

    StudentBL StudentBL
    {
        get
        {
            if (moStudentBL == null)
                moStudentBL = new StudentBL();
            return moStudentBL;
        }
        set { moStudentBL = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetViewAsPerAccess();
                FillAssessmentDropDown();
                FillTeachersComboBox();
                ApplyMouseHoverEffect(new List<Button> { });
                GetQueryString();
                SetJavaScriptAttributes();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
                if (lstvwStudentDetails.Items.Count > 0)
                    ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudentDetails, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Publish Xseed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
            string sPublish = (btnPublish.Text == "Publish" ? "Publish" : "UnPublish");
            oXseedProgressReportBL.PublishXseedResult(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), Convert.ToInt32(CmbAssessment.SelectedValue), sPublish, miUserId);
            CmbAssessment_SelectedIndexChanged(CmbAssessment, null);

            if (sPublish == "Publish")
                lblMessage.Text = "Pre-Primary progress report grade details published successfully!!!";
            else
                lblMessage.Text = "Pre-Primary progress report grade details unpublished successfully!!!";

        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
    private void SetViewAsPerAccess()
    {
        if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentWiseProgressReport) == Constants.C_NO)
        {
            tdcmbTeachers.Visible = tdClassTeacherLable.Visible = false;
            hidTeacherId.Value = Session[Constants.S_SESSION_TEACHER_ID].ToString();
        }
    }

    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckPreCondition())
            {
                hidTeacherId.Value = cmbTeachers.SelectedValue;
                DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
                lstvwStudentDetails.DataSourceID = ObjDSStudentDetails.ID;
                lstvwStudentDetails.DataBind();
                SetPublishButtonState();

               
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    protected void CmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hidAssessmentId.Value = CmbAssessment.SelectedValue;
            DtPgCount.SetPageProperties(Constants.I_ZERO, Constants.I_GRID_PAGE_COUNT, false);
            lstvwStudentDetails.DataSourceID = ObjDSStudentDetails.ID;
            lstvwStudentDetails.DataBind();
            SetPublishButtonState();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    protected void lstvwStudentDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iListIndex = oCurrentItem.DisplayIndex;
            int iYearwiseStudentId = Convert.ToInt32(lstvwStudentDetails.DataKeys[iListIndex]["YearwiseStudentId"]);

            if (e.CommandName == Constants.S_COMMAND_REMOVE)
            {
                StudentBL moStudentBL = new StudentBL();
                moStudentBL.Delete(iYearwiseStudentId, miSchoolId, miAcademicYearId, CmbAssessment.SelectedValue.ToInt(), miUserId);
                lblMessage.Text = "Pre-Primary progress report grades deleted successfully!!!";
                RefreshValues();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void lstvwStudentDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
       
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string sQueryString = string.Empty;
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iListIndex = oCurrentItem.DisplayIndex;

                HyperLink ohyplnkEdit = ((HyperLink)(oCurrentItem.FindControl("hyplnkEdit")));
                HtmlTableRow tr = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
                HtmlTableCell thdelete = tr.FindControl("thdelete") as HtmlTableCell;
                HtmlTableCell tdSelect = e.Item.FindControl("tddelete") as HtmlTableCell;
                ohyplnkEdit.ImageUrl = "../images/IconGrid_Edit.GIF";

                string ishowprogressreport = (lstvwStudentDetails.DataKeys[iListIndex]["ShowProgressReport"]).ToString();
                ImageButton btn = e.Item.FindControl("btndelete") as ImageButton;
                int ieditStatus = Convert.ToInt32(lstvwStudentDetails.DataKeys[iListIndex]["EditStatus"]);

                if (ieditStatus == 1 || ishowprogressreport == "Y")
                {
                    btn.Visible = false;
                }
                else
                {
                    btn.Visible = true;
                    btn.Attributes.Add("onclick", "if(!ConfirmDelete()) return false;");
                }

                if (ieditStatus == 1 || ishowprogressreport == "Y")
                {
                    btn.Enabled = false;
                }
                if (ishowprogressreport == "Y")
                {
                    btndeleteGrade.Enabled = false;
                }

                string sType = Convert.ToString(lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["ProgressReportType"]).Trim();

                if (sType == "Xseed" || sType == "PrePrimary")
                {
                    thdelete.Visible = true;
                    tdSelect.Visible = true;
                }
                else
                {
                    thdelete.Visible = false;
                    tdSelect.Visible = false;
                }

                if (Convert.ToString(lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["ProgressReportType"]).Trim() == string.Empty)
                {
                    sQueryString = "&ClassTeacherId=" + cmbTeachers.SelectedValue +
                                   "&StudentId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString() +
                                   "&ProgresSheetID=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["ProgresSheetID"].ToString() +
                                   "&StandardDivisionId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["Standard_Division_Id"].ToString() +
                                   "&ClassTeacher=" + cmbTeachers.SelectedItem.Text;

                    ohyplnkEdit.NavigateUrl = ohyplnkEdit.NavigateUrl + "?" + CommonUtility.EncryptQuerystring(sQueryString);
                }
                else
                {
                    if (Convert.ToString(lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["ProgressReportType"]).Trim() == "Xseed")
                    {
                        SetEditStatus(lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["EditStatus"].ToInt(), ohyplnkEdit);
                        sQueryString = "&ClassTeacherId=" + cmbTeachers.SelectedValue +
                                       "&StudentId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString() +
                                       "&StandardId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString() +
                                       "&StandardDivisionId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["Standard_Division_Id"].ToString() +
                                       "&EditMode=Y" +
                                       "&AssessmentId=" + CmbAssessment.SelectedValue;

                        ohyplnkEdit.NavigateUrl = "~/RITeSchool/ProgressReport/XssedStudentwiseProgressReportUI.aspx?" + CommonUtility.EncryptQuerystring(sQueryString);
                    }
                    else if (Convert.ToString(lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["ProgressReportType"]).Trim() == "MonthConfig")
                    {
                        sQueryString = "&ClassTeacherId=" + cmbTeachers.SelectedValue +
                          "&StudentId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString() +
                          "&StandardId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString() +
                          "&StandardDivisionId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["Standard_Division_Id"].ToString();

                        ohyplnkEdit.NavigateUrl = "~/RITeSchool/Teacher/StudentProgressReportEntry.aspx?" + CommonUtility.EncryptQuerystring(sQueryString + "&From=StudentWiseProgressReport");
                    }
                    else
                    {
                        sQueryString = "&TeacherId=" + cmbTeachers.SelectedValue +
                                        "&StudentId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["YearwiseStudentId"].ToString() +
                                        "&StandardId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["StandardId"].ToString() +
                                        "&StandardDivisionId=" + lstvwStudentDetails.DataKeys[oCurrentItem.DisplayIndex]["Standard_Division_Id"].ToString() +
                                        "&IsReadOnly=False";
                        ohyplnkEdit.NavigateUrl = "~/RITeSchool/Teacher/PrePrimaryProgressSheetEntry.aspx?" + CommonUtility.EncryptQuerystring(sQueryString + "&From=StudentWiseProgressReport");

                    }

                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetEditStatus(int iCanEdit, HyperLink hyplnkEdit)
    {
        switch ((Constants.MarkAssignmentStatus)iCanEdit)
        {
            case Constants.MarkAssignmentStatus.NotAssigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_NONE_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryNotStarted;
                break;
            case Constants.MarkAssignmentStatus.PartiallyAssigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_PARTIAL_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryPartiallyDone;
                break;
            case Constants.MarkAssignmentStatus.Assigned:
                hyplnkEdit.ImageUrl = Constants.S_IMG_FOR_COMPLETE_CONFIGURATION;
                hyplnkEdit.ToolTip = Resources.LocalizedResources.MarksEntryCompleted;
                break;
        }
    }

    protected void lstvwStudentDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (lstvwStudentDetails.Items.Count > 0)
            {
                lstvwStudentDetails.Items.Clear();
                ControlUtility.FillListViewPagerFooterWithCulture(lstvwStudentDetails, DtPgCount, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);

                if (lstvwStudentDetails.DataKeys[0]["ProgressReportType"].ToString() == "Xseed")
                    LegendTable.Visible = tdCmbAssessment.Visible = tdAssessmentLable.Visible = true;
                else
                    LegendTable.Visible = tdCmbAssessment.Visible = tdAssessmentLable.Visible = false;

                if (lstvwStudentDetails.DataKeys[0]["ShowDeleteButton"].ToBool())
                    btndeleteGrade.Visible = true;
                else
                    btndeleteGrade.Visible = false;
                    
            }
            else
            {
                DtPgCount.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view pagewise menu files.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbPageCnt_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            ControlUtility.SetDataPagerAccordingToPageNoAndCulture(lstvwStudentDetails, Resources.LocalizedResources.PageNo, Resources.LocalizedResources.Of, Resources.LocalizedResources.OutOflst);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortVariables();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void ObjDSStudentDetails_ObjectDisposing(object sender, ObjectDataSourceDisposingEventArgs e)
    {
        try
        {
            e.Cancel = true;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void ObjDSStudentDetails_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
    {
        try
        {
            e.ObjectInstance = StudentBL;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwStudentDetails_PreRender(object sender, EventArgs e)
    {
        try
        {
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btndeleteGrade_Click(object sender, EventArgs e)
    {
        try
        {
            StudentBL moStudentbl = new StudentBL();
            moStudentbl.DeleteAllStudent(cmbTeachers.SelectedValue.ToInt(), miSchoolId, miAcademicYearId, miUserId, CmbAssessment.SelectedValue.ToInt());
            lblMessage.Text = "Pre-Primary progress report grades of all students of this class deleted successfully!!!";
            RefreshValues();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillTeachersComboBox()
    {
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDtClassTeacher = oMasterDataCollectionBL.GetAllClassTeachers(miSchoolId, miAcademicYearId);
        if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.StudentWiseProgressReport) == Constants.C_NO)
        {

            DataRow[] drArray = oDtClassTeacher.Select("Teacher_Id = " + hidTeacherId.Value);
            if (drArray.Length > 0)
            {
                DataTable oDtClasses = drArray.CopyToDataTable();
                tdcmbTeachers.Visible = true;
                ListSource.FillDropDownList(oDtClasses, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, string.Empty);
                cmbTeachers_SelectedIndexChanged(cmbTeachers, null);
            }
        }
        else
            ListSource.FillDropDownList(oDtClassTeacher, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, Constants.S_STANDARD_DIVISION_ID_FIELD, Constants.S_SELECT);

    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        hidSortDirection.Value = (lstvwStudentDetails.SortDirection.ToString() == "Ascending" || lstvwStudentDetails.SortDirection.ToString() == string.Empty) ? Constants.S_ASCENDING : Constants.S_DESCENDING;
        hidSortExpression.Value = (lstvwStudentDetails.SortExpression != string.Empty) ? lstvwStudentDetails.SortExpression.ToString() : S_DEFAULT_SORT_EXP;
        HtmlTableRow oHtmlTableHeaderRow = lstvwStudentDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }


    /// <summary>
    /// This method is used set sort variables.
    /// </summary>
    private void SetSortVariables()
    {
        hidSortDirection.Value = hidSortDirection.Value == Constants.S_DESCENDING ? Constants.S_ASCENDING : Constants.S_DESCENDING;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 0 && QueryString["StandardDivisionId"] != null)
            hidTeacherId.Value = cmbTeachers.SelectedValue = QueryString["StandardDivisionId"];

        if (QueryString.Count > 0 && QueryString["AssessmentId"] != null)
            hidAssessmentId.Value = CmbAssessment.SelectedValue = QueryString["AssessmentId"];

        lstvwStudentDetails.DataSourceID = ObjDSStudentDetails.ID;

    }

    /// <summary>
    /// This method is used to fill the Assessments DropDownList.
    /// </summary>
    private void FillAssessmentDropDown()
    {
        List<AssessmentMaster> lstAssessmentMaster = AssignXseedGradesBL.GetAssessments(miSchoolId, miAcademicYearId);
        if (lstAssessmentMaster.Count > 0)
        {
            ListSource.FillDropDownList(lstAssessmentMaster, CmbAssessment, "Name", "AssessmentId", string.Empty);
            hidAssessmentId.Value = CmbAssessment.SelectedValue = lstAssessmentMaster.First().AssessmentId.ToString();
        }
    }
    /// <summary>
    /// This method is used to Set Publish Button
    /// </summary>
    private void SetPublishButtonState()
    {
        btnPublish.Visible = false;
        XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL();
        PublishStatus oPublishStatus = oXseedProgressReportBL.GetPublishStatus(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue), Convert.ToInt32(CmbAssessment.SelectedValue));
        if (oPublishStatus != null)
        {
            if (oPublishStatus.AllowPublish || oPublishStatus.AllowUnpublish)
            {
                btnPublish.Visible = true;
                if (oPublishStatus.AllowUnpublish)
                    btnPublish.Text = "Un Publish";
                else
                    btnPublish.Text = "Publish";
            }
        }
    }

    /// <summary>
    /// This function checks the preconditons of Teacher timetable.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = string.Empty;
        if (cmbTeachers.SelectedValue != string.Empty)
            sLinks = ReferenceBL.GetPreConditionMsgForStudentWiseProgressReport(miSchoolId, miAcademicYearId, Convert.ToInt32(cmbTeachers.SelectedValue));
        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
            HideControls(true);
        }
        else
        {
            divErr.Visible = true;
            divErr.InnerHtml = sLinks;
            HideControls(false);

        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to visible or hide controls as per requirement.
    /// </summary>
    private void HideControls(bool abFlag)
    {
        trPrecondition.Visible = !abFlag;
        trStudentDetails.Visible = abFlag;
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        lstvwStudentDetails.DataSourceID = ObjDSStudentDetails.ID;
        lstvwStudentDetails.DataBind();
    }

    private void SetJavaScriptAttributes()
    {
        btndeleteGrade.Attributes.Add("onclick", "if(!ConfirmDeleteAll()) return false;");
    }

}