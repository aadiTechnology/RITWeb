/* File Name - AssignGradesUI.aspx.cs
 * Created By - Sachin
 * Created Date - 18-Sept-2015
 * Description - This class is used to display class-subject list view.
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using BusinessLogic;
using System.Data;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Web.UI.HtmlControls;

public partial class AssignGradesUI : SchoolBase
{
    #region Constant(s)

    private const string S_PUBLISH_MESSAGE = "Observation Exam Published Successfully !!!";
    private const string S_UNPUBLISH_MESSAGE = "Observation Exam Un-Published Successfully !!!";
    private const string S_PUBLISH_TEXT = "Publish";
    private const string S_UNPUBLISH_TEXT = "Un-Publish";

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to fill teacher and exam combo box, subject list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                FillTeachersComboBox();
                GetQueryString();
                FillTestCombobox();                
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill class-subject list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbTeachers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {   
            FillClasses();
            //FillExamMarksStatusGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void FillClasses()
    {
        SubjectTestConfigurationCollectionBL obj = new SubjectTestConfigurationCollectionBL(miSchoolId,miAcademicYearId);
        DataTable oDT = obj.GetAllClasses(cmbTeachers.SelectedValue.ToInt());
        ListSource.FillDropDownList(oDT, cmbClass, "className", "SchoolWise_Standard_Division_Id", Constants.S_SELECT);

        if (oDT.Rows.Count == 1)
        {
            cmbClass.SelectedIndex = 1;
            cmbClass_SelectedIndexChanged(cmbClass, null);
        }
        else
        {
            lstvwSubjects.DataSource = null;
            lstvwSubjects.DataBind();
        }
    }

    /// <summary>
    /// This event is used to open observation assignmentUI.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSubjects_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "SELECT")
            {
                int iStdDivId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Standard_Division_Id"].ToInt();
                int iSubjectId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Subject_Id"].ToInt();
                string sQuerystring = "StandardDivisionId=" + iStdDivId + "&SubjectId=" + iSubjectId + "&TestId=" + cmbExams.SelectedValue + "&TeacherId=" + cmbTeachers.SelectedValue + "&IsClassTeacher=" + hidIsClassTeacher.Value + "&FilteredStdDivId=" + cmbClass.SelectedValue + "&IsSummaryMode=N";
                sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
                Response.Redirect("ObservationGradeAssignmentUI.aspx?" + sQuerystring, false);
            }
            else 
            {
                if (moSchool == Constants.SchoolId.PPSH)
                {
                    int iStdDivId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Standard_Division_Id"].ToInt();
                    int iSubjectId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Subject_Id"].ToInt();
                    string sQuerystring = "StandardDivisionId=" + iStdDivId + "&SubjectId=" + iSubjectId + "&TestId=" + cmbExams.SelectedValue + "&TeacherId=" + cmbTeachers.SelectedValue + "&IsClassTeacher=" + hidIsClassTeacher.Value + "&FilteredStdDivId=" + cmbClass.SelectedValue;
                    sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
                    Response.Redirect("AssignSummaryGradesUI.aspx?" + sQuerystring, false);
                }
                else if (moSchool == Constants.SchoolId.SNS)
                {
                    int iStdDivId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Standard_Division_Id"].ToInt();
                    int iSubjectId = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["Subject_Id"].ToInt();
                    string sQuerystring = "StandardDivisionId=" + iStdDivId + "&SubjectId=" + iSubjectId + "&TestId=" + cmbExams.SelectedValue + "&TeacherId=" + cmbTeachers.SelectedValue + "&IsClassTeacher=" + hidIsClassTeacher.Value + "&FilteredStdDivId=" + cmbClass.SelectedValue+"&IsSummaryMode=Y";
                    sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
                    Response.Redirect("ObservationGradeAssignmentUI.aspx?" + sQuerystring, false);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwSubjects_SelectedIndexChanging(object sender, ListViewSelectEventArgs e)
    {
    }

    /// <summary>
    /// This event is used to fill class-subject list view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbExams_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExamMarksStatusGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to Publish exam.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            SubjectTestConfigurationCollectionBL oTestConfigCollection = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);

            if (btnPublish.Text == S_PUBLISH_TEXT)
            {
                oTestConfigCollection.PublishObservationTest(cmbExams.SelectedValue.ToInt(), hidStdDivId.Value.ToInt(), miUserId, true);
                btnPublish.Text = S_UNPUBLISH_TEXT;
                base.DisplayMessage(S_PUBLISH_MESSAGE, false, tdMessage);
            }
            else
            {
                oTestConfigCollection.PublishObservationTest(cmbExams.SelectedValue.ToInt(), hidStdDivId.Value.ToInt(), miUserId, false);
                btnPublish.Text = S_PUBLISH_TEXT;
                base.DisplayMessage(S_UNPUBLISH_MESSAGE, false, tdMessage);
            }


        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwSubjects_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lstvwSubjects = sender as ListView;

                bool bIsSubmited = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["IsSubmitted"].ToBool();
                bool bIsSubjectTeacher = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["IsSubjectTeacher"].ToBool();
                bool bIsCoCurricularSubject = lstvwSubjects.DataKeys[e.Item.DisplayIndex]["IsCoCurricularSubject"].ToBool();

                ImageButton btnSelect = e.Item.FindControl("btnSelect") as ImageButton;
                ImageButton btnAddSummary = e.Item.FindControl("btnAddSummary") as ImageButton;

                Label lblDash = e.Item.FindControl("lblDash") as Label;

                if (hidIsClassTeacher.Value == "Y")
                {
                    if (!bIsSubmited)
                    {
                        lblDash.Visible = true;
                        btnSelect.Visible = false;
                    }
                    else
                        lblDash.Visible = false;
                }
                else
                    lblDash.Visible = false;

                if (moSchool == Constants.SchoolId.PPSH)
                {
                    HtmlTableCell tdSummary = e.Item.FindControl("tdSummary") as HtmlTableCell;
                    if (tdSummary != null)
                        tdSummary.Visible = true;
                }
                else if (moSchool == Constants.SchoolId.SNS)
                {
                    if (miAcademicYearId >= 11)
                    {
                        HtmlTableCell tdSummary = e.Item.FindControl("tdSummary") as HtmlTableCell;
                        if (tdSummary != null)
                        {
                            if (cmbExams.SelectedItem.Text == "Term II")
                            {
                                tdSummary.Visible = true;
                                if (bIsCoCurricularSubject)
                                    btnAddSummary.Visible = false;
                                else
                                    btnAddSummary.Visible = true;
                            }
                            else
                                tdSummary.Visible = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillExamMarksStatusGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to read query string.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count <= 0)
            return;

        if (QueryString["TestId"] != null)
            cmbExams.SelectedValue = QueryString["TestId"];

        if (QueryString["FilteredStdDivId"] != null)
            cmbClass.SelectedValue = QueryString["FilteredStdDivId"];
        
        if (QueryString["TeacherId"] != null && QueryString["TeacherId"].Trim() != Constants.S_ZERO)
        {
            cmbTeachers.SelectedValue = QueryString["TeacherId"].ToString();
            cmbTeachers_SelectedIndexChanged(cmbTeachers, null);
        }

        if (QueryString["IsClassTeacher"] != null)
            hidIsClassTeacher.Value = QueryString["IsClassTeacher"];

        //if (hidIsClassTeacher.Value == Constants.S_YES)
        //{   
        //    btnPublish.Visible = true;
        //}
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

        if (moUserRole == Constants.UserRoles.Teacher && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.AssignGrades) != Constants.C_YES)
        {
            cmbTeachers.SelectedValue = Session[Constants.S_SESSION_TEACHER_ID].ToString();
            cmbTeachers.Enabled = false;

            cmbTeachers_SelectedIndexChanged(cmbTeachers, null);
        }
        else
            cmbClass.Items.Add(new ListItem { Text = Constants.S_SELECT, Value = Constants.S_ZERO });
    }

    /// <summary>
    /// This method  is used to fill test combo box.
    /// </summary>
    private void FillTestCombobox()
    {
        TestCollectionBL oTestCollectionBL = new TestCollectionBL(miSchoolId, miAcademicYearId);
        DataTable oDtAllTests = oTestCollectionBL.GetAllTestsForSchool();

        DataTable dt = oDtAllTests.Clone();
        DataRow[] drArr;

        DataRow[] dreval1;
        DataRow[] dreval2;
        DataRow[] dreval3;
            
        if(miSchoolId == Constants.SchoolId.PPSH.ToInt())
        {            
            if (miAcademicYearId >= 14)
            {
                dreval1 = oDtAllTests.Select("schoolwise_test_name = 'Evaluation I'");
                dreval1[0]["schoolwise_test_name"] = "Term-I";

                dreval2 = oDtAllTests.Select("schoolwise_test_name = 'Evaluation 2'");
                dreval2[0]["schoolwise_test_name"] = "Term-II";

                dreval3 = oDtAllTests.Select("schoolwise_test_name = 'Evaluation 3'");
                dreval3[0].Delete();        
       
                oDtAllTests.AcceptChanges();

                drArr = oDtAllTests.Select("schoolwise_test_name IN ('Term-I','Term-II')");
             }
            else
                drArr = oDtAllTests.Select("schoolwise_test_name LIKE '%Evaluation%' ");
       }

        else if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            if(miAcademicYearId <=6)
                drArr = oDtAllTests.Select("schoolwise_test_name LIKE '%Annual Exam%' ");
            else
                drArr = oDtAllTests.Select("schoolwise_test_name LIKE '%Term%' ");
        }
        else
            drArr = oDtAllTests.Select("IsFinalExam=1");

        if (drArr.Length > 0)
        {
            dt = drArr.CopyToDataTable();
            DataView dv = dt.DefaultView;
            dv.Sort = "original_schoolwise_test_id asc";
        }

        ControlUtility.FillDropDownList(
                       dt,
                       ref cmbExams,
                       Constants.S_TEST_ID_FIELD,
                       Constants.S_TEST_NAME_FIELD,
                       string.Empty);

        cmbExams.SelectedValue = SchoolWiseTestMasterBL.GetLatestExamId(miSchoolId, miAcademicYearId, 0, 0).ToString();
        cmbExams_SelectedIndexChanged(cmbExams, null);
    }

    /// <summary>
    /// This method is used to fill the standard- divisions and subjects grid.
    /// </summary>
    private void FillExamMarksStatusGrid()
    {
        SubjectTestConfigurationCollectionBL oTestConfigCollection = new SubjectTestConfigurationCollectionBL(miSchoolId, miAcademicYearId);
        bool bIsClasssTeacher = false;

        if (Session[Constants.S_SESSION_IS_CLASS_TEACHER]!= null && Session[Constants.S_SESSION_IS_CLASS_TEACHER].ToString() == Constants.S_YES)
            bIsClasssTeacher = true;

        DataTable oDtAllStdandardDivisions = oTestConfigCollection.GetSubjectTeachers(cmbTeachers.SelectedValue.ToInt(), Convert.ToInt32(cmbExams.SelectedValue), bIsClasssTeacher, cmbClass.SelectedValue.ToInt());

        bool bIsPublished = false;
        hidStdDivId.Value = Constants.S_ZERO;
        
        DataTable dtClasses = null;

        if (oDtAllStdandardDivisions.Rows.Count > 0)
        {
            bIsPublished = oDtAllStdandardDivisions.Rows[0]["IsPublished"].ToBool();
            hidStdDivId.Value = oDtAllStdandardDivisions.Rows[0]["Standard_Division_Id"].ToString();

            if (miSchoolId == Constants.SchoolId.SNS.ToInt() && miAcademicYearId < 10)
                dtClasses = oDtAllStdandardDivisions.Select("Subject_Name='Behaviour'").CopyToDataTable();
            else if (miSchoolId == Constants.SchoolId.SNS.ToInt() && miAcademicYearId >= 10)
                dtClasses = oDtAllStdandardDivisions.Select("Subject_Name<>'Feedback'").CopyToDataTable();
            else
                dtClasses = oDtAllStdandardDivisions;

            dtClasses.DefaultView.Sort = "OrgStdId ASC, OrgDivId ASC, Sort_Order ASC";

            if (hidIsClassTeacher.Value == Constants.S_YES)
            {
                if (bIsPublished)
                {
                    btnPublish.Visible = true;
                    btnPublish.Enabled = true;
                    btnPublish.Text = S_UNPUBLISH_TEXT;
                }
                else if (bIsClasssTeacher)
                {
                    btnPublish.Visible = true;

                    if (dtClasses.Rows.Count > 0 && dtClasses.Select("IsSubmitted=1").Length == dtClasses.Rows.Count)
                        btnPublish.Enabled = true;
                    else
                        btnPublish.Enabled = false;

                    btnPublish.Text = S_PUBLISH_TEXT;
                }
            }
        }
        else
            btnPublish.Visible = false;

        lstvwSubjects.DataSource = dtClasses;
        lstvwSubjects.DataBind();

        if (moSchool == Constants.SchoolId.PPSH)
        {
            HtmlTableCell thGradeAll = lstvwSubjects.FindControl("thSummary") as HtmlTableCell;
            if (thGradeAll != null)
                thGradeAll.Visible = true;
        }
        else if (moSchool == Constants.SchoolId.SNS)
        {
            if (miAcademicYearId >= 11)
            {
                HtmlTableCell thGradeAll = lstvwSubjects.FindControl("thSummary") as HtmlTableCell;
                if (thGradeAll != null)
                {
                    if (cmbExams.SelectedItem.Text == "Term II")
                        thGradeAll.Visible = true;
                    else
                        thGradeAll.Visible = false;
                }
            }
        }

        
    } 

    #endregion
}