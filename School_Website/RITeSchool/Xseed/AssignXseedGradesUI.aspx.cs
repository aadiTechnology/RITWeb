//-------------------------------------------------------------------------------------------------------------------------------------
// Class Name       :- AssignXseedGradesUI
// Purpose          :- This class is used to display all the subjects 
//                     assigned with the status of grades of selected assessment and teacher.
// Date Of creation :- 6/01/2011
// Author Name      :- Shobha Patil.
//-------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

public partial class AssignXseedGradesUI : SchoolBase
{
   #region "EVENTS"

    /// <summary>
    /// This event is used to initialise the controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                RefreshValues();
                SetJavaScriptAttributes();
                if (CheckPreCondition())
                {
                    FillTeacherDropDown();
                    FillAssessmentDropDown();
                    ReadQueryString();
                    FillSubjectGradeAssignmentListview();
                }               
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
                if (lstvwXseedSubjects.Items.Count > 0)
                {
                    List<XseedGradesStatus> lstXseedGradesStatus = AssignXseedGradesBL.GetTeacherSubjectDetails(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId, miAcademicYearId, Convert.ToInt32(cmbAssessment.SelectedValue));
                    lstvwXseedSubjects.DataSource = lstXseedGradesStatus;
                    lstvwXseedSubjects.DataBind();
                }
            }
            // Is School is PPS then change the name of Sitemap.
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                MasterPage oMasterPage = (MasterPage)this.Master;
                SiteMapPath siteMap = (SiteMapPath)oMasterPage.FindControl("SiteMapPath1");
                oMasterPage.NodeTitle = "Assign Pre-Primary Grades";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the listview for the selected assessment and teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeachers.SelectedIndex == 0 || cmbAssessment.SelectedIndex == 0)
                lstvwXseedSubjects.Visible = false;
            else
                FillSubjectGradeAssignmentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    /// <summary>
    /// This event is used to fill the listview for the selected assessment and teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbTeachers.SelectedIndex == 0 || cmbAssessment.SelectedIndex == 0)
                lstvwXseedSubjects.Visible = false;
            else
                FillSubjectGradeAssignmentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to assign Xseed grades and to submit assigned grades to class teacher.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwXseedSubjects_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iListIndex = oCurrentItem.DisplayIndex;
            int iStandardDivisionID = Convert.ToInt32(lstvwXseedSubjects.DataKeys[iListIndex]["StandardDivisionID"]);
            int iSubjectId = Convert.ToInt32(lstvwXseedSubjects.DataKeys[iListIndex]["SubjectId"]);
            AssignXseedGradesBL oAssignXseedGradesBL = new AssignXseedGradesBL();
            if (e.CommandName == Constants.S_SUBMIT)
            {
                oAssignXseedGradesBL.GradeSubmitEntity = PopulateGradeSubmitStatusBL();
                oAssignXseedGradesBL.GradeSubmitEntity.StandardDivisionId = iStandardDivisionID;
                oAssignXseedGradesBL.GradeSubmitEntity.SubjectId = iSubjectId;
                oAssignXseedGradesBL.Submit();
                FillSubjectGradeAssignmentListview();
            }
            else if (e.CommandName == Constants.S_EDIT_MODE)
            {
                string sIsReadOnly = lstvwXseedSubjects.DataKeys[iListIndex]["SubmitStatus"].ToString();
                string sQueryString = CreateQueryString(iStandardDivisionID, iSubjectId, sIsReadOnly);
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                char cIsXeedSubject =Convert.ToChar(lstvwXseedSubjects.DataKeys[iListIndex]["IsXseedSubject"]);

                if (cIsXeedSubject == Constants.C_NO)
                    sEncrypt = Constants.S_PAGE_NON_XSEED_GRADE_ASSIGNMENT + "?" + sEncrypt;
                else
                    sEncrypt = Constants.S_PAGE_XSEED_GRADE_ASSIGNMENT + "?" + sEncrypt;

                MasterPage oMasterPage = (MasterPage)this.Master;
                oMasterPage.RedirectToNextPage(sEncrypt);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill the subject listview with assigned grade status.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwXseedSubjects_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                ImageButton imgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                ImageButton imgBtnSubmit = e.Item.FindControl("imgBtnSubmit") as ImageButton;
                Label lblStatus = (Label)e.Item.FindControl("lblStatus");
                int iCanEdit = Convert.ToInt32(lstvwXseedSubjects.DataKeys[iRowId]["EditStatus"]);
                int iSubmitStatus = Convert.ToChar(lstvwXseedSubjects.DataKeys[iRowId]["SubmitStatus"]);
				XseedGradesStatus oXseedGradesStatus = oCurrentItem.DataItem as XseedGradesStatus;
                imgBtnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmitAction('" + (oXseedGradesStatus.IncompleteRollNo.IsNullOrEmpty() ? string.Empty : oXseedGradesStatus.IncompleteRollNo) + "')) {return false;}");
                SetEditStatus(iCanEdit,imgBtnEdit);
                SetSubmitStatus(iSubmitStatus, imgBtnSubmit, lblStatus);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetEditStatus(int iCanEdit, ImageButton imgBtnEdit)
    {
        switch ((Constants.MarkAssignmentStatus)iCanEdit)
        {
            case Constants.MarkAssignmentStatus.NotAssigned : imgBtnEdit.ForeColor = System.Drawing.Color.Black;
                imgBtnEdit.Font.Bold = true;
                imgBtnEdit.BackColor = System.Drawing.Color.FromArgb(253, 252, 178);
                imgBtnEdit.ImageUrl = Constants.S_IMG_FOR_NONE_CONFIGURATION;
                imgBtnEdit.ToolTip = Resources.LocalizedResources.MarksEntryNotStarted;
                break;
            case Constants.MarkAssignmentStatus.PartiallyAssigned: imgBtnEdit.ImageUrl = Constants.S_IMG_FOR_PARTIAL_CONFIGURATION;
                imgBtnEdit.ToolTip = Resources.LocalizedResources.MarksEntryPartiallyDone; 
                break;
            case Constants.MarkAssignmentStatus.Assigned : imgBtnEdit.ImageUrl = Constants.S_IMG_FOR_COMPLETE_CONFIGURATION;
                imgBtnEdit.ToolTip = Resources.LocalizedResources.MarksEntryCompleted; 
                break;
        }
    }

    private void SetSubmitStatus(int iSubmitStatus, ImageButton imgBtnSubmit, Label lblStatus)
    {
        switch ((Constants.MarkSubmitStatus)iSubmitStatus)
        {
            case Constants.MarkSubmitStatus.SubmitDenied: imgBtnSubmit.Visible = false;
                lblStatus.Visible = true;
                lblStatus.Text = Resources.LocalizedResources.MarksNotSubmitted; 
                lblStatus.ToolTip = Resources.LocalizedResources.MarksNotSubmitted;
                break;
            case Constants.MarkSubmitStatus.Submit: imgBtnSubmit.ImageUrl = Constants.S_IMG_FOR_SUBMIT_EXAM_MARKS;
                imgBtnSubmit.ToolTip = Resources.LocalizedResources.SubmitMarksToClass;
                imgBtnSubmit.CssClass = "IconSpacing CursorHand";
                break;
            case Constants.MarkSubmitStatus.Submitted: imgBtnSubmit.Visible = false;
                lblStatus.Visible = true;
                lblStatus.Text = Resources.LocalizedResources.MarksAlreadySubmitted; 
                lblStatus.ToolTip = Resources.LocalizedResources.MarksAlreadySubmitted; 
                break;
        }
    }

    protected void lstvwXseedSubjects_ItemEditing(object sender, ListViewEditEventArgs e)
    {
    }
    #endregion

    #region "PRIVATE METHODS"

    /// <summary>
    /// This method is used to set the default attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        cmbAssessment.Focus();
       
    }

    /// <summary>
    /// This method is used to fill the Assessments DropDownList.
    /// </summary>
    private void FillAssessmentDropDown()
    {
        List<AssessmentMaster> lstAssessmentMaster = AssignXseedGradesBL.GetAssessments(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstAssessmentMaster, cmbAssessment, "Name", "AssessmentId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill the Teachers DropDownList.
    /// </summary>
    private void FillTeacherDropDown()
    {
        List<ClassTeacher> lstClassTeachers = AssignXseedGradesBL.GetTeachers(miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstClassTeachers, cmbTeachers, "TeacherName", "TeacherId", Constants.S_SELECT);
        if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AssignXseedGrades) != Constants.C_YES))
        {
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_ID].ToString();
            cmbTeachers.Visible = false;
            tdTeacher.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void ReadQueryString()
    {
        if (QueryString["TeacherId"] != null)
            cmbTeachers.SelectedValue = QueryString["TeacherId"];
        if (QueryString["AssessmentId"] != null)
            cmbAssessment.SelectedValue = QueryString["AssessmentId"];
      
    }

    /// <summary>
    ///  This method is used to display all the subjects assigned with the status of grades of selected assessment and teacher.
    /// </summary>
    private void FillSubjectGradeAssignmentListview()
    {
        Constants.UserRoles eUserRoles = moUserRole;
        string sUserHasFullAccess = (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AssignXseedGrades) == Constants.C_YES).ToString();
        if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AssignXseedGrades) != Constants.C_YES))
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_ID].ToString();
        if ((cmbTeachers.SelectedIndex == 0 || cmbAssessment.SelectedIndex==0) )
            lstvwXseedSubjects.Visible = false;
        else
            lstvwXseedSubjects.Visible = true;

        if (eUserRoles == Constants.UserRoles.Teacher && (!Boolean.Parse(sUserHasFullAccess)))
        {
            cmbTeachers.Visible = false;
            tdTeacher.Visible = false;
        }
        //if admin or supervisor
        else if (eUserRoles == Constants.UserRoles.Admin || eUserRoles == Constants.UserRoles.Supervisor || (Boolean.Parse(sUserHasFullAccess)))
        {
            cmbTeachers.Visible = true;
            tdTeacher.Visible = true;
        }
        int iAsseessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        List<XseedGradesStatus> lstXseedGradesStatus = AssignXseedGradesBL.GetTeacherSubjectDetails(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId, miAcademicYearId, iAsseessmentId);
        lstvwXseedSubjects.DataSource = lstXseedGradesStatus;
        lstvwXseedSubjects.DataBind();
    }

    /// <summary>
    /// This method is used to populate AssignXseedGradesBL objects.
    /// </summary>
    /// <returns></returns>
    private GradeSubmitStatus PopulateGradeSubmitStatusBL()
    {
        GradeSubmitStatus oGradeSubmitStatus = new GradeSubmitStatus
        {
            AssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue),
            AcademicYearId = miAcademicYearId,
            SchoolId = miSchoolId,
            InsertedById = miUserId,
        };
        return oGradeSubmitStatus;
    }

    /// <summary>
    /// This method is used to create QueryString.
    /// </summary>
    /// <param name="aiStandardDivisionID"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="IsReadOnly"></param>
    /// <returns></returns>
    private string CreateQueryString(int aiStandardDivisionID, int aiSubjectId, string IsReadOnly)
    {
        string sQuerystring = string.Empty;
        sQuerystring = "StandardDivisionId=" + aiStandardDivisionID.ToString();
        sQuerystring = sQuerystring + "&SubjectId=" + aiSubjectId.ToString();
        sQuerystring = sQuerystring + "&AssessmentId=" + cmbAssessment.SelectedValue;
        sQuerystring = sQuerystring + "&TeacherId=" + cmbTeachers.SelectedValue;
        sQuerystring = sQuerystring + "&IsReadOnly=" + IsReadOnly;
       
        return sQuerystring;
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.AssignXseedGrades);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            tblAssignXseedGrades.Visible = false;
        }
        return bReturn;
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidAreYouSureYouWantToContinue.Value = Resources.LocalizedResources.AreYouSureYouWantToContinue;
        hidRollNos.Value = Resources.LocalizedResources.RollNos;
        hidValGradeSumbit.Value = Resources.LocalizedResources.ValGradeSumbit;
        hidGradesnotenteredfor.Value = Resources.LocalizedResources.Gradesnotenteredfor;       
    }

    #endregion
}
