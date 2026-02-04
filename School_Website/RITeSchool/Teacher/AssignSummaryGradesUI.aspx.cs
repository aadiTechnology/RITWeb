using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SchoolEntities.Teacher;
using Utility;

public partial class AssignSummaryGradesUI : SchoolBase
{
    #region Constant(s)

    private const string S_SUBMIT = "Submit";
    private const string S_UNSUBMIT = "UnSubmit";
    private const string S_SAVE_MESSAGE = "Summary Grades saved successfully !!!";
    private const string S_SUBMIT_MESSAGE = "Summary Grades submitted successfully !!!";
    private const string S_UNSUBMIT_MESSAGE = "Summary Grades un-submitted successfully !!!";

    #endregion

    #region Data Member(s)

    private AssignSummaryGradesBL moAssignSummaryGradesBL;
    private const string S_GRADES = "GradeData";

    #endregion

    #region Events

    /// <summary>
    /// This event is used to fill grade and listview .
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            moAssignSummaryGradesBL = new AssignSummaryGradesBL(miSchoolId, miUserId, miAcademicYearId);
            if (!IsPostBack)
            {
                ReadQueryString();
                FillGradeList();
                FillAddSummaryListview();
                SetLegends();
                SetJavaScriptAttribute();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit and unsubmit grade details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            bool aIsSubmit = (btnSubmit.Text == S_SUBMIT);
            moAssignSummaryGradesBL.SubmitSummaryGradeDetails(
                   hidStdDivId.Value.ToInt(),
                   hidSubjectId.Value.ToInt(),
                   hidTestId.Value.ToInt(),
                   aIsSubmit
               );

            if (aIsSubmit)
                base.DisplayMessage(S_SUBMIT_MESSAGE, false, tdMessage);
            else
                base.DisplayMessage(S_UNSUBMIT_MESSAGE, false, tdMessage);

            FillAddSummaryListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to fill listview dropdown
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwAssignSummeryGrades_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                AssignSummaryGradesDetails oAssignGradeSummary = e.Item.DataItem as AssignSummaryGradesDetails;
                if (ViewState[S_GRADES] != null)
                {
                    DataTable dtGrade = (DataTable)ViewState[S_GRADES];
                    DropDownList ddlGrade = e.Item.FindControl("ddlGrade") as DropDownList;
                    ListSource.FillDropDownList(dtGrade, ddlGrade, "Name", "Id", Constants.S_SELECT);

                    ddlGrade.SelectedValue = oAssignGradeSummary.GradeId.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this method is used to save grade details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            base.DisplayMessage(S_SAVE_MESSAGE, false, tdMessage);
            FillAddSummaryListview();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region methods

    /// <summary>
    /// this method is used to read query string
    /// </summary>
    private void ReadQueryString()
    {
        hidTestId.Value = QueryString["TestId"];
        hidStdDivId.Value = QueryString["StandardDivisionId"];
        hidSubjectId.Value = QueryString["SubjectId"];
        hidTeacherId.Value = QueryString["TeacherId"];
        hidIsClassTeacher.Value = QueryString["IsClassTeacher"];
        hidFilterStdDivId.Value = QueryString["FilteredStdDivId"];
    }

    /// <summary>
    /// This method is used to display legends.
    /// </summary>
    private void SetLegends()
    {
        lblExam.Text = moAssignSummaryGradesBL.TestName;
        lblSubject.Text = moAssignSummaryGradesBL.SubjectName;
    }

    /// <summary>
    /// This method is used fill AddSummary listview.
    /// </summary>

    private void FillAddSummaryListview()
    {
        List<AssignSummaryGradesDetails> lstStudentlist = moAssignSummaryGradesBL.GetAll(hidStdDivId.Value.ToInt(), hidSubjectId.Value.ToInt(), hidTestId.Value.ToInt());
        lstvwAssignSummeryGrades.DataSource = lstStudentlist;
        lstvwAssignSummeryGrades.DataBind();
        SetButtonState();

        if (ViewState[S_GRADES] != null)
        {
            HtmlTableCell thGradeAll = lstvwAssignSummeryGrades.FindControl("thGradeAll") as HtmlTableCell;
            if (thGradeAll != null)
            {
                DropDownList ddlGradeAll = thGradeAll.FindControl("ddlGradeAll") as DropDownList;
                if (ddlGradeAll != null)
                {
                    DataTable dtGrade = (DataTable)ViewState[S_GRADES];
                    ListSource.FillDropDownList(dtGrade, ddlGradeAll, "Name", "Id", Constants.S_SELECT);
                    ddlGradeAll.Attributes.Add("onchange", "SetGrades('" + ddlGradeAll.ClientID + "'); return false;");
                }
            }
        }
    }

    /// <summary>
    /// This method is used to set button state.
    /// </summary>
    private void SetButtonState()
    {
        if (moAssignSummaryGradesBL.ButtonStates.IsPublished)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = false;
            lstvwAssignSummeryGrades.Enabled = false;
            btnSubmit.Text = S_UNSUBMIT;
        }
        else if (moAssignSummaryGradesBL.ButtonStates.IsSubmitted)
        {
            btnSave.Enabled = false;
            btnSubmit.Enabled = true;
            lstvwAssignSummeryGrades.Enabled = false;
            btnSubmit.Text = S_UNSUBMIT;
        }
        else if (moAssignSummaryGradesBL.ButtonStates.IsSaved)
        {
            btnSave.Enabled = true;
            btnSubmit.Enabled = true;
            lstvwAssignSummeryGrades.Enabled = true;
            btnSubmit.Text = S_SUBMIT;
        }
        else
        {
            btnSave.Enabled = true;
            btnSubmit.Enabled = false;
            lstvwAssignSummeryGrades.Enabled = true;
            btnSubmit.Text = S_SUBMIT;
        }
    }

    /// <summary>
    /// This method is used to fill grade dropdown.
    /// </summary>
    private void FillGradeList()
    {
        StudentAssessmentBL mostudentAssessmentBL = new StudentAssessmentBL(miSchoolId, miAcademicYearId, miUserId);
        DataTable odtGrade = mostudentAssessmentBL.GetGrades(miAcademicYearId);
        ViewState[S_GRADES] = odtGrade;
    }

    /// <summary>
    /// This method is used to save student grade details.
    /// </summary>
    private void Save()
    {
        List<AssignSummaryGradesDetails> oStudentGradeDetails = Populate();
        string sXml = base.GenerateXml(oStudentGradeDetails);
        moAssignSummaryGradesBL.Save(sXml, hidSubjectId.Value.ToInt(), hidTestId.Value.ToInt());
    }

    /// <summary>
    /// This method is used to populate student assessment details.
    /// </summary>
    /// <returns></returns>
    private List<AssignSummaryGradesDetails> Populate()
    {
        List<AssignSummaryGradesDetails> lstAssignSummeryGrades = new List<AssignSummaryGradesDetails>();

        foreach (ListViewDataItem item in lstvwAssignSummeryGrades.Items)
        {
            DropDownList ddlGrade = item.FindControl("ddlGrade") as DropDownList;
            int iYearwiseStudentId = lstvwAssignSummeryGrades.DataKeys[item.DisplayIndex]["YearwiseStudentId"].ToInt();

            AssignSummaryGradesDetails oStudentGradeDetails = new AssignSummaryGradesDetails
            {
                YearwiseStudentId = iYearwiseStudentId,
                GradeId = ddlGrade.SelectedValue.ToInt(),
            };
            lstAssignSummeryGrades.Add(oStudentGradeDetails);
        }
        return lstAssignSummeryGrades;

    }

    /// <summary>
    /// this metod is used for set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnSave, btnBack, btnSubmit });
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        string sQueryString = CommonUtility.EncryptQuerystring("TeacherId=" + hidTeacherId.Value + "&TestId=" + hidTestId.Value + "&IsClassTeacher=" + hidIsClassTeacher.Value + "&FilteredStdDivId=" + hidFilterStdDivId.Value);
        btnBack.PostBackUrl = "~/RITeSchool/Teacher/AssignGradesUI.aspx?" + sQueryString;
    }

    #endregion
}