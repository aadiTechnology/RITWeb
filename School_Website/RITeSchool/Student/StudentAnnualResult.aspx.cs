/*
* This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 28 Jan 2008
 * Date of modification: 2 Feb 2008
 */
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Linq;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using System.Collections.Generic;
using BusinessLogic.Exceptions;
using XseedReportEntities;

public partial class StudentProgressSheet : StudentResult 
{
    #region Constant

    const string S_ERROR_MSG = "Class teacher is not associated yet.";
    const String S_ERR_MSG_TESTS = "All configured exams are not published";

    #endregion

    #region Events

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    /// <summary>
    /// This method event is used to render student's progress report while first time page load
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
			InitializeMemberVariables();
            base.SetpanelMember(GridViewScrollContainer);
            if (!IsPostBack)
            {
                tdhlnkToppers.Visible = false;
                FillTeachersComboBox();
                GetQueryString();
                hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.FinalResult).ToString();
                if (moUserRole == Constants.UserRoles.Admin||moUserRole == Constants.UserRoles.Supervisor|| Convert.ToBoolean(hidUserHasFullAccess.Value))
                {
                    VisibleHideTeacherCombo(true);
                    VisibleHideGenerateButton(true);
                }
                else if (moUserRole == Constants.UserRoles.Teacher)
                {
                    TeacherStandardDetailsCollectionBL oTeacherStandardDetailsCollectionBL = new TeacherStandardDetailsCollectionBL(miSchoolId, miAcademicYearId);
                    int iTeacherID = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
					int iStandardDivisionId = cmbTeachers.SelectedValue.ToInt();
                    if (isTestPublishedForStdDivId(iStandardDivisionId))
                    {
                        VisibleHideGenerateButton(true);
                        DataTable oDTStudents = GetStudentDatset(iTeacherID, true);
                        FillStudentsComboBox(oDTStudents);
                        setTopperslinkURL();
                    }
                    else
                    {
                        VisibleHideStudentCombo(false);
                        VisibleHideGenerateButton(false);
                        btnShow.Visible = false;
                        throw new BusinessLogic.Exceptions.NoResultFound("No exam of this class has been published for the current academic year.");
                    }
                }
                else if (moUserRole == Constants.UserRoles.Student)
                {
                    VisibleHideTeacherCombo(false);
                    VisibleHideStudentCombo(false);
                    btnShow.Visible = false;
                    btnPrint.Visible = false;
                    CheckIsResultPublished();
                    int iStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
                    if (isTestPublishedForStdDivId(iStandardDivisionId))
                    {
                        VisibleHideGenerateButton(true);
                        int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
                        hidStudId.Value = iStudentId.ToString();
                        FillProgressReport(iStudentId);
                        tdhlnkToppers.Visible = Settings.ShowTopppers;
                        tdhlnkToppers.Width = "100%";
                        tdhlnkToppers.Align = HorizontalAlign.Right.ToString();
                        hlnkToppers.Enabled = true;
                        setTopperslinkURL();
                    }
                    else
                    {
                        throw new BusinessLogic.Exceptions.NoResultFound("No exam of this class has been published for the current academic year.");
                    }
                }
                btnPrint.Attributes.Add("onclick", "GeneratePrint();return false;");
                AddPrintAttributs();
                btnShow.Attributes["onclick"] = "javascript:DisableButtons()";

                if (moUserRole == Constants.UserRoles.Student)
                    btnCancel.Visible = false;
            }

			ApplyMouseHoverEffect(new List<Button>() { btnPrint, btnShow, btnCancel, btnCancelUp });
            btnCancelUp.Visible = false;
        }
        catch (BusinessLogic.Exceptions.NoResultFound ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (BusinessLogic.Exceptions.ResultNotPublished ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            if (moUserRole != Constants.UserRoles.Student)
            {
                string sEncrypt = GetEncryptedTestQueryString();
                MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("~/Teacher/StudentResultList.aspx?" + sEncrypt);
            }
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
            int iStudentId = Convert.ToInt32(cmbStudents.SelectedValue);
			int iStandardDivisionId = Convert.ToInt32(cmbTeachers.SelectedValue);
            if (isTestPublishedForStdDivId(iStandardDivisionId))
            {
				int iResult = base.ShowProgressSheet(iStandardDivisionId, iStudentId);
                if (iResult > 1)
                    btnCancelUp.Visible = true;

                if (!isAllTestPublishedForStdDivId(iStandardDivisionId))
                {
                    string sErrorMesage = AllUnpublishedTestForStdDivId(iStandardDivisionId);
                    throw new BusinessLogic.Exceptions.NoResultFound(S_ERR_MSG_TESTS + (sErrorMesage == string.Empty ? "" : " - " + sErrorMesage));
                }
            }
            else
            {
                throw new BusinessLogic.Exceptions.NoResultFound("No exam of this class has been published for the current academic year.");
            }
        }
        catch (BusinessLogic.Exceptions.MarksNotAvailableForResult ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }            
        catch (BusinessLogic.Exceptions.NoResultFound ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Height = Unit.Pixel(25);
            lblErrorMsg.Width = Unit.Percentage(95);
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (BusinessLogic.Exceptions.ResultNotPublished ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
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
			int iStdDivId = Convert.ToInt32(cmbTeachers.SelectedValue);
            StudentProgress oStudentProgress = new StudentProgress();
            if (oStudentProgress.isTestPublishedForStdDivId(iStdDivId))
            {
                SetTeacherAndToppers(iStdDivId);
                AddPrintAttributs();
                if (!isAllTestPublishedForStdDivId(iStdDivId))
                {
                    string sErrorMesage = AllUnpublishedTestForStdDivId(iStdDivId);
                    throw new BusinessLogic.Exceptions.NoResultFound(S_ERR_MSG_TESTS + (sErrorMesage == string.Empty ? "" : " - " + sErrorMesage));
                }
            }
            else
            {
                throw new BusinessLogic.Exceptions.NoResultFound("No exam of this class has been published for the current academic year.");
            }
        }
        catch (BusinessLogic.Exceptions.NoResultFound ex)
        {
            pnlErrorMsg.Visible = true;
            trErr.Visible = true;
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
            lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
            lblErrorMsg.CssClass = "LblNoRecord";
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to add event for button on student combo changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            AddPrintAttributs();
        }
        catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }


    #endregion Events

    #region Private Method

    /// <summary>
    /// This methos is used to check is the result for that test is published or not
    /// </summary>
    private Boolean isAllTestPublishedForStdDivId(int aiStandardDivisionId)
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
    /// This method is used to add attribute for print button.
    /// </summary>
    private void AddPrintAttributs()
    {
        string sQryStr = "mode=print";

        if (moUserRole == Constants.UserRoles.Admin
            || moUserRole == Constants.UserRoles.Supervisor
            || Convert.ToBoolean(hidUserHasFullAccess.Value))
        {
			sQryStr = sQryStr + "&StandardDivisionId=" + cmbTeachers.SelectedValue;
            sQryStr = sQryStr + "&iStudId=" + cmbStudents.SelectedValue;
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
			sQryStr = sQryStr + "&StandardDivisionId=" + Convert.ToString(cmbTeachers.SelectedValue);
            sQryStr = sQryStr + "&iStudId=" + cmbStudents.SelectedValue;
        }
        else
        {
			sQryStr = sQryStr + "&StandardDivisionId=0";
            sQryStr = sQryStr + "&iStudId=" + Convert.ToString(Session[Constants.S_SESSION_STUDENT_ID]);
        }

        sQryStr = Utility.CommonUtility.EncryptQuerystring(sQryStr);
        hidQery.Value = sQryStr;
    }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideTeacherCombo(bool abAction)
    {
        tdcmbTeachers.Visible = abAction;
        tdlblTeacher.Visible = abAction;
    }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideStudentCombo(bool abAction)
    {
        tdUPanelStudent.Attributes.Remove("class");
        cmbStudents.Visible = abAction;
        tdlblStudent.Visible = abAction;
    }

    /// <summary>
    /// This function is used to make visible and unvisible controls
    /// </summary>
    /// <param name="abAction"></param>
    private void VisibleHideGenerateButton(bool abAction)
    {
        tdbtnPrint.Visible = abAction;
        btnPrint.Visible = abAction;
    }

    /// <summary>
    /// This function is used to fill teacher combo
    /// </summary>
    private void FillTeachersComboBox()
    {
        //get all class teachers
		MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();

		List<ClassTeacherDetails> lstTeachers = oMasterDataCollectionBL.GetClassTeachersForExamResult(miSchoolId, miAcademicYearId);

		if (moUserRole == Constants.UserRoles.Teacher)
		{
			List<ClassTeacherDetails> lstClassTeacher = lstTeachers.Where(Teacher => Teacher.TeacherId == Session[Constants.S_SESSION_TEACHER_ID].ToInt()).ToList();
			ListSource.FillDropDownList(lstClassTeacher, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", string.Empty);
			if (lstTeachers.Count == Constants.I_ONE)
			{
				cmbTeachers.Enabled = false;
				cmbTeachers.SelectedIndex = Constants.I_ONE;
			}
		}
		else
			ListSource.FillDropDownList(lstTeachers, cmbTeachers, Constants.S_TEACHER_NAME_FIELD, "StandardDivisionId", Constants.S_SELECT);

        if (cmbTeachers.Items.Count == 1)
        {
            trErr.Visible = true;
            lblErrorMsg.Visible = true;
            if (moUserRole == Constants.UserRoles.Admin
                || moUserRole == Constants.UserRoles.Supervisor)
            {
                HyperLink1.Visible = true;
                lblErrorMsg.Text = Constants.S_ERROR_MSG_FOR_ALL_CONFIGURATION;
                pnlFilter.Visible = false;
                GridViewScrollContainer.Visible = false;
            }
            else
            {
                trErr.Visible = true;
                lblErrorMsg.Style.Add(HtmlTextWriterStyle.TextAlign, "center");
                lblErrorMsg.Style.Add(HtmlTextWriterStyle.BorderStyle, "None");
                lblErrorMsg.Text = S_ERROR_MSG;
            }
        }
    }

    /// <summary>
    /// This function is used to fill student's combo
    /// </summary>
    private void FillStudentsComboBox(DataTable aoDtStudent)
    {
        //get all class teachers
        ControlUtility.FillDropDownList(aoDtStudent, ref cmbStudents,
                                                 "Student_Id",
                                                 "Student_Name",
                                                 Constants.S_SELECT_ALL);
    }

    /// <summary>
    /// This method is used to set decrypted URL to toppres link
    /// </summary>
    private void setTopperslinkURL()
    {
        int istdDivid = 0;
        if (Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID] != null)
            istdDivid = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(istdDivid);
        String QueryString = "ExamType=1&ToppersType=0&StdDivId=" + istdDivid.ToString() + "&StdId=" + oStandardDivisionMasterBL.StandardId.ToString();
        QueryString = "../Student/ExamToppersUI.aspx?" + CommonUtility.EncryptQuerystring(QueryString);
        hlnkToppers.Attributes.Add("onclick", "ShowToppers('" + QueryString + "');return false;");
        hlnkToppers.Visible = Settings.ShowTopppers;
    }

    /// <summary>
    /// This method is used to check that is Result is published or not
    /// </summary>
    private void CheckIsResultPublished()
    {
        int iStandardDivisionId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID]);
        SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, iStandardDivisionId);
        if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == 0)
            throw new BusinessLogic.Exceptions.ResultNotPublished("Result not published for this class.");
    }

    /// <summary>
    /// This method is used to set combo of techer for a given teacher id
    /// </summary>
    /// <param name="aiTecherId"></param>
    private void SetTeacherAndToppers(int aiTeacherId)
    {
        DataTable oDtStudents = GetStudentDatset(aiTeacherId, true);
        FillStudentsComboBox(oDtStudents);
        if (aiTeacherId != 0)
        {
            setTopperslinkURL();
        }
        else
        {
            hlnkToppers.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to get encrypted query string.
    /// </summary>
    /// <returns></returns>
    private string GetEncryptedTestQueryString()
    {
        string sQuerystring = "";
        if (Convert.ToInt32(cmbTeachers.SelectedValue) != 0)
        {
			sQuerystring = "&StandardDivisionId=" + cmbTeachers.SelectedValue;
            sQuerystring = CommonUtility.EncryptQuerystring(sQuerystring);
        }
        return sQuerystring;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
		try
		{
			if (QueryString != null)
			{
				if (QueryString["StandardDivisionId"] != null)
				{
					cmbTeachers.SelectedValue = QueryString["StandardDivisionId"];
					SetTeacherAndToppers(Convert.ToInt32(cmbTeachers.SelectedValue));
				}
			}
		}
		catch (Exception)
		{
			MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
		}
    }
    #endregion

}