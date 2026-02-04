//----------------------------------------------------------------------------------------------------------
// Class Name       :- AssignXseedGradesUI
// Purpose          :- This class is used to display all the subjects 
//                     assigned with the status of grades of selected assessment and teacher.
// Date Of creation :- 6/07/2011
// Author Name      :- Shobha Patil.
//----------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

public partial class ClassTeacherXseedGradesUI : SchoolBase
{
 
    #region "DATA MEMBERS"

    AssignXseedGradesBL moAssignXseedGradesBL;

    #endregion

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
            moAssignXseedGradesBL = new AssignXseedGradesBL();
            if (!IsPostBack)
            {               
                SetJavaScriptAttributes();
                if (CheckPreCondition())
                {
                    FillTeacherDropDown();
                    FillAssessmentDropDown();
                    ReadQueryString();
                    FillSubjectGradeAssignmentListview();
                }
                RefreshValues();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
            }
          
             // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                btnUnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    RefreshValues();
                }

             if (miSchoolId == Constants.SchoolId.PPS.ToInt())
             {
                 MasterPage oMasterPage = (MasterPage)this.Master;
                 SiteMapPath siteMap = (SiteMapPath)oMasterPage.FindControl("SiteMapPath1");
                 oMasterPage.NodeTitle = "Pre-Primary Results";
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
    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbAssessment.SelectedIndex != 0 && cmbTeachers.SelectedValue != "0")
                FillSubjectGradeAssignmentListview();
            else
                lstvwXseedStatus.Visible = false;
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
            if (cmbAssessment.SelectedIndex != 0 && cmbTeachers.SelectedIndex != 0)
                FillSubjectGradeAssignmentListview();
            else
                lstvwXseedStatus.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set the buttons and ListView controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwXseedStatus_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                int iRowId = Convert.ToInt32(oCurrentItem.DisplayIndex);
                char cEditStatus = Convert.ToChar(lstvwXseedStatus.DataKeys[iRowId]["EditStatus"]);
                ImageButton imgBtnEdit = e.Item.FindControl("imgBtnEdit") as ImageButton;
                Label lblStudentName = e.Item.FindControl("lblStudentName") as Label;
                if (cEditStatus == Constants.C_NO)
                {
                    imgBtnEdit.ImageUrl = string.Empty;
                    imgBtnEdit.AlternateText = "-";
                    imgBtnEdit.Enabled = false;
                    lblStudentName.Enabled = false;
                }
                else
                    imgBtnEdit.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    ///  This event is used to edit assigned Xseed grades.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwXseedStatus_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
            int iRowId = oCurrentItem.DisplayIndex;
            int iSubjectId = Convert.ToInt32(lstvwXseedStatus.DataKeys[iRowId]["SubjectId"]);

            if (e.CommandName == Constants.S_EDIT_MODE)
            {
                string sIsReadOnly = lstvwXseedStatus.DataKeys[iRowId]["IsSubmitted"].ToString();
                string sQueryString = CreateQueryString(Convert.ToInt32(hidStandardDivisionId.Value), iSubjectId, sIsReadOnly);
                string sEncrypt = CommonUtility.EncryptQuerystring(sQueryString);
                char  cIsXeedSubject = Convert.ToChar(lstvwXseedStatus.DataKeys[iRowId]["IsXseedSubject"]);

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
    /// this event is used to publish result.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            moAssignXseedGradesBL.GradeSubmitEntity = PopulateGradeSubmitStatusBL();
            moAssignXseedGradesBL.GradeSubmitEntity.StandardDivisionId = Convert.ToInt32(hidStandardDivisionId.Value);
            moAssignXseedGradesBL.Publish();
            FillSubjectGradeAssignmentListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void lstvwXseedStatus_ItemEditing(object sender, ListViewEditEventArgs e)
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
        ApplyMouseHoverEffect(new List<Button> { btnPublish, btnUnPublish });
        btnPublish.Attributes.Add("Onclick", "if(!(ConfirmAction())){return false;}");
    }

    /// <summary>
    /// This method is used to get the querystring.
    /// </summary>
    private void ReadQueryString()
    {
        cmbTeachers.SelectedValue = QueryString["TeacherId"];
        if (QueryString["AssessmentId"] != null)
            cmbAssessment.SelectedValue = QueryString["AssessmentId"];
        if(QueryString["TestId"] != null)
            cmbAssessment.SelectedValue = QueryString["TestId"];
    }

    /// <summary>
    /// This method is used to fill the subject listview for selected class teacher and assessment
    /// </summary>
    private void FillSubjectGradeAssignmentListview()
    {
        Constants.UserRoles eUserRoles = moUserRole;
        string sUserHasFullAccess = (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedResults) == Constants.C_YES).ToString();
        if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedResults) != Constants.C_YES))
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToString();
        if ((cmbTeachers.SelectedIndex == 0 ||  cmbAssessment.SelectedIndex==0))
            lstvwXseedStatus.Visible = false;
        else
        {
            lstvwXseedStatus.Visible = true;

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

            List<XseedGradesStatus> lstXseedGradesStatus = moAssignXseedGradesBL.GetClassTeacherSubjects(Convert.ToInt32(cmbTeachers.SelectedValue), miSchoolId,
                                                                                                    Convert.ToInt32(cmbAssessment.SelectedValue), miAcademicYearId);
            lstvwXseedStatus.DataSource = lstXseedGradesStatus;
            lstvwXseedStatus.DataBind();
            hidStandardDivisionId.Value = moAssignXseedGradesBL.XseedResultPublishEntity.StandardDivisionId.ToString();
            char cPublishStatus = moAssignXseedGradesBL.XseedResultPublishEntity.PublishStatus;
            char cIsPublished = moAssignXseedGradesBL.XseedResultPublishEntity.IsPublished;
            if (cPublishStatus == 'N')
            {
                btnPublish.Enabled = false;
                btnUnPublish.Enabled = false;
            }
            else if (cPublishStatus == 'Y' && cIsPublished == 'N')
            {
                btnPublish.Enabled = true;
                btnUnPublish.Enabled = false;
            }
            else if (cPublishStatus == 'Y' && cIsPublished == 'Y')
            {
                btnPublish.Enabled = false;
                btnUnPublish.Enabled = true;
            }
        }
        SetUnpublishButtonAttributes();
    }

    /// <summary>
    /// This method is used to unpublish the published result.
    /// </summary>
    private void SetUnpublishButtonAttributes()
    {
        int iTeacherID = 0;
        if (moUserRole == Constants.UserRoles.Admin || moUserRole == Constants.UserRoles.Supervisor || (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedResults) == Constants.C_YES))
            iTeacherID = Convert.ToInt32(cmbTeachers.SelectedValue);
        else if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
		string sQuerystring = "StandardDivisionId=" + hidStandardDivisionId.Value +
                              "&sTeacherName=" + cmbTeachers.SelectedItem.Text +
                              "&TeacherId=" + iTeacherID.ToString() +
                              "&TestId=" + cmbAssessment.SelectedValue +
                              "&From=XseedReport" +
                              "&sTestName=" + cmbAssessment.SelectedItem.Text;

        string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
        btnUnPublish.Attributes.Add("onclick", "window.open('../Admin/TestUnpublishPopUp.aspx?" + sEncrypt
                                                          + "', '_blank','scrollbars=yes,resizable=no,top=0,left=0,width=675,height=370').focus();return false;");
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
		List<ClassTeacherDetails> lstClassTeacher = AssignXseedGradesBL.GetClassTeachers(miSchoolId, miAcademicYearId);
		ListSource.FillDropDownList(lstClassTeacher, cmbTeachers, "TeacherName", "StandardDivisionId", Constants.S_SELECT);
        if (moUserRole == Constants.UserRoles.Teacher && (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.XseedResults) != Constants.C_YES))
        {
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_STDDIV_ID].ToString();
            cmbTeachers.Visible = false;
            tdTeacher.Visible = false;
        }
    }

    /// <summary>
    /// This method is used to set create querystring.
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
        sQuerystring = sQuerystring + "&From=Result";
        return sQuerystring;
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
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.XseedResults);

        if (sLinks.Equals(""))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            btnUnPublish.Visible = false;
            btnPublish.Visible = false;
            tblAssignGrades.Visible = false;
        }
        return bReturn;
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        hidMsgClassTecherXseed.Value = Resources.LocalizedResources.MsgClassTecherXseed;
    }
    #endregion
}
