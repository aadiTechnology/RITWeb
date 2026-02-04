using System;
using System.Collections.Generic;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities;
using Utility;
using System.Web.UI.WebControls;
using System.Data;

public partial class StudentExamWiseSubjectMarksDetailsUI : SchoolBase
{
    #region Data Member(s)

    private StudentExamWiseSubjectMarksDetailsBL moStudentExamWiseSubjectMarksDetailsBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// These event is used to fill exam dropdown.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moStudentExamWiseSubjectMarksDetailsBL = new StudentExamWiseSubjectMarksDetailsBL(miSchoolId, miUserId, miAcademicYearId);
        try
        {
            if (!IsPostBack)
            {
                FillExamDropdown();
                SetUrlToLinkButton();
                SetJavascriptAttributes();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwExamwiseSubjectMarkDetails_ItemDataBound(object sender, System.Web.UI.WebControls.ListViewItemEventArgs e)
    {
        if (e.Item.ItemType == System.Web.UI.WebControls.ListViewItemType.DataItem)
        {
            Label lblMarks = e.Item.FindControl("lblMarks") as Label;
            Label lblGrades = e.Item.FindControl("lblGrades") as Label;
            Label lblOutOfMarks = e.Item.FindControl("lblOutOfMarks") as Label;

            StudentExamWiseSubjectMarksDetails obj = e.Item.DataItem as StudentExamWiseSubjectMarksDetails;

            if (obj.IsGradingSubject)
            {
                lblMarks.Text = "-";
                lblOutOfMarks.Text = "-";

                if (obj.IsAbsentGrade != string.Empty)
                {
                    lblGrades.Text = obj.IsAbsentGrade;
                }
            }
            else
            {
                if (obj.IsAbsentGrade != string.Empty)
                {
                    lblMarks.Text = obj.IsAbsentGrade;
                    lblGrades.Text = obj.IsAbsentGrade;
                }
            }
        }
    }

    /// <summary>
    /// This event is used to fill up ExamwiseSubjectMarkDetails listview according to selected exam.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillMarkListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Method(s)

    /// <summary>
    /// This method is used to fill ExamwiseSubjectMarkDetails list view.
    /// </summary>
    private void FillMarkListview()
    {
        int iTestId = Convert.ToInt32(cmbExam.SelectedValue);
        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        List<StudentExamWiseSubjectMarksDetails> lstSubject = moStudentExamWiseSubjectMarksDetailsBL.GetAllSubjects(iTestId, iStudentId);

        lstvwExamwiseSubjectMarkDetails.DataSource = lstSubject;
        lstvwExamwiseSubjectMarkDetails.DataBind();
    }

    /// <summary>
    /// This method is used to fill up Exam DropDown .
    /// </summary>
    private void FillExamDropdown()
    {
        int iStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
        List<StudentExamWiseSubjectMarksDetails> lstExams = moStudentExamWiseSubjectMarksDetailsBL.GetExams(iStudentId);
        ListSource.FillDropDownList(lstExams, cmbExam, "TestName", "TestId", Constants.S_SELECT);
    }

    private void SetJavascriptAttributes()
    {
        lnkbtnGradeConfigurationDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetUrlToLinkButton()
    {
        lnkbtnGradeConfigurationDetails.Attributes.Add("onclick", "OpenPopup(); return false;");
        lnkbtnGradeConfigurationDetails.Visible = true;
        FillMarkGradeListViewSubject();
        FillMarkGradeListViewCurricularSubject();
    }

    /// <summary>
    /// This method for FillMarkGradeListViews
    /// </summary>
    /// <param name="abIsCoCurricularSubjects"></param>
    private void FillMarkGradeListViewSubject()
    {
        MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
        oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;
        oMarksGradesConfigurationBL.School_Id = miSchoolId;
        oMarksGradesConfigurationBL.Standard_Id = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        oMarksGradesConfigurationBL.IsCoCurricularSubjects = false;
        DataSet oDsMarkGradesSubject = oMarksGradesConfigurationBL.FetchMarksGradesConfigurationDetails();
        DataRow[] drArr = oDsMarkGradesSubject.Tables[1].Select("Standard_Id IS NOT NULL");
        if (drArr.Length > 0)
        {
            lstvwGradeConfigurationDetailsSubject.DataSource = drArr.CopyToDataTable();
            lstvwGradeConfigurationDetailsSubject.DataBind();
        }
        else
        {
            lstvwGradeConfigurationDetailsSubject.DataSource = null;
            lstvwGradeConfigurationDetailsSubject.DataBind();
        }
    }

    private void FillMarkGradeListViewCurricularSubject()
    {
        MarksGradesConfigurationBL oMarksGradesConfigurationBL = new MarksGradesConfigurationBL();
        oMarksGradesConfigurationBL.Academic_Year_Id = miAcademicYearId;
        oMarksGradesConfigurationBL.School_Id = miSchoolId;
        oMarksGradesConfigurationBL.Standard_Id = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_ID]);
        oMarksGradesConfigurationBL.IsCoCurricularSubjects = true;
        DataSet oDsMarkGrades = oMarksGradesConfigurationBL.FetchMarksGradesConfigurationDetails();
        DataRow[] drArr = oDsMarkGrades.Tables[1].Select("Standard_Id IS NOT NULL");
        if (drArr.Length > 0)
        {
            lstvwGradingConfigurationDetailsCurricularSubject.DataSource = drArr.CopyToDataTable();
            lstvwGradingConfigurationDetailsCurricularSubject.DataBind();
        }
        else
        {
            lstvwGradingConfigurationDetailsCurricularSubject.DataSource = null;
            lstvwGradingConfigurationDetailsCurricularSubject.DataBind();
        }
    }

    #endregion    
}
