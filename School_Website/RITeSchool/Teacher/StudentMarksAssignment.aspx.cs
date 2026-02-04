/* File Name - StudentMaksAssignment.aspx.cs
 * Created Date - 
 * Created by - 
 * Class Description - This class is used for marks assignment.
 */

/* File Name - StudentMaksAssignment.aspx.cs
 * Modified Date - 20-Dec-2011
 * Modified by - Vipul
 * Modification Description - This class will save marks as per Out Of Marks configuration.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using SchoolEntities;
using System.Web.UI.HtmlControls;

public partial class StudentMarksAssignment : SchoolBase
{
    #region Constant

    
    private const int I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX = 2;
    private const int I_ASSIGNED_GRADE_COLUMN_INDEX = 3;
    private const int I_REMARK_COLUMN_INDEX = 4;
    private const string S_CONSTANT_MARKS = "M";
    private const string S_GRADE = "G";
    private const string S_LATE_JOIN = "J";
    private const string S_LATE_JOIN_DISPLAY_VALUE = "Late Joining";
    private const string S_FALSE = "False";
    private const string S_REMARK_TEMPLATE_KEYWORDS = "sNotes";
    private const string S_SALUTATION = "%MASTER/MISS%";
    private const string S_FULLNAME = "%FULLNAME%";
    private const string S_FIRSTNAME = "%FNAME%";
    private const string S_MIDDLENAME = "%MNAME%";
    private const string S_LASTNAME = "%LNAME%";
    private const string S_ChangedExamDate = "ChangedExamDate";
    public static string mstaticID;
    public static string msIsExamScheduleDate;
    public static string msIsDisableSave;
    
    #endregion

    #region Data Member

    private DataSet modsMarksDetails;

    #endregion

    #region Property(s)
    
    private bool ShowAdditionalGrid
    {
        get
        {
            return (LblDataExam.Text.ToUpper().Contains("FORMATIVE ASSESSMENT") && LblDataExam.Text.ToUpper().Contains("TOOLS"));
        }
    }

    private bool AllowLateJoineeOption
    {
        get
        {
            return (moSchool == Constants.SchoolId.VPMCPS);
        }
    }

    #endregion

    #region Events

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            msIsExamScheduleDate = Constants.S_NO;
            msIsDisableSave = Constants.S_NO;
            SetQueryStringValues();
            DisplayData();
            CreateStudentsGrid();
            if (!IsPostBack)
            {
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                DesignSettingAccordingLanguage();
                FillStudentsGrid();
                if (!Convert.ToBoolean(hidIsExamStatusApplicable.Value))
                    DisableExamStatus();
                base.OnInit(e);
            }

            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                DesignSettingAccordingLanguage();
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to fill student attendance and set todays date.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if(msIsDisableSave == Constants.S_YES)
                    btnSave.Enabled = false;
                else
                    btnSave.Enabled=true;
                InitializeForm();
                FillRemarksCombo();
                FillGradesCombo();
                FillTemplateKeywords();
                if (hidTimerVisibleState.Value == S_CONSTANT_MARKS)
                    timer.Enabled = false;
                else
                    timer.Enabled = true;                
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to configure(save and update) student attendance.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            if (Page.IsValid)
            {   
                if (!CheckIfTestDateIsValid())
                {
                    string oStudentXml = GenerateStudentXml();
                    string oStudentMarksXml = GenerateStudentMarksXml();
                    string oRemarkXml = GenerateRemarkXml();

                    bool bShowRemark = SubjectTestTypeConfigurationCollectionBL.IsTestAndSubjectConfiguredForRemark(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());

                    StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL
                                                                   {
                                                                       InsertedBYId = miUserId,
                                                                       TestWiseSubjectMarksId = Convert.ToInt32(hidSchoolSubjectTestId.Value),
                                                                       StudentDetails = oStudentXml,
                                                                       StudentMarkDetails = oStudentMarksXml,
                                                                       RemarkXml = oRemarkXml,
                                                                       HasRemarks = bShowRemark,
                                                                       TestId = hidTestId.Value.ToInt(),
                                                                       SubjectId = hidSubjectId.Value.ToInt()
                                                                   };
                    oStudentSubjectMarksBL.ManageStudentTestMarks(hidIsTestPublished.Value, miSchoolId, miAcademicYearId);

                    MasterPage oMasterPage = (MasterPage)Master;
                    oMasterPage.RedirectToNextPage(GetEncryptedTestQueryString(Convert.ToInt32(hidTestId.Value)));
                }
            }

            if (!Convert.ToBoolean(hidIsExamStatusApplicable.Value))
                DisableExamStatus();

            if (hidTimerVisibleState.Value == S_CONSTANT_MARKS)
                timer.Enabled = false;
            else
                timer.Enabled = true;
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    private string GenerateRemarkXml()
    {
       const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("RemarkDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "RemarkDetails", string.Empty);

        bool bIsRemarkFound = false;
        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount < grdStudentMarks.Rows.Count; iRowCount++)
        {   
            if (hidMarksOrGrades.Value == S_GRADE)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "RemarkDetails", string.Empty);
                TextBox txtRemark = (TextBox)grdStudentMarks.Rows[iRowCount].FindControl("txtRemark");

                if (txtRemark != null)
                {
                    XmlAttribute attr = oDoc.CreateAttribute("StudentId");
                    attr.Value = grdStudentMarks.DataKeys[iRowCount].Value.ToString();
                    oXmlNode.Attributes.Append(attr);

                    attr = oDoc.CreateAttribute("Remark");
                    attr.Value = txtRemark.Text.Trim();
                    oXmlNode.Attributes.Append(attr);

                    oXmlRootNode.AppendChild(oXmlNode);
                    bIsRemarkFound = true;
                }
            }
        }

        if (bIsRemarkFound)
        {
            // Add the root node to document element. 
            root.AppendChild(oXmlRootNode);
            return root.InnerXml;
        }

        return string.Empty;
    }

    /// <summary>
    /// This method is used to transfer control to SchoolConfigurationControlPanel page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(GetEncryptedTestQueryString(Convert.ToInt32(hidTestId.Value)));
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to do processing after data bound to row of grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudentMarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList oDropDownList = (DropDownList)e.Row.FindControl("ddlExamStatus");                
                ControlUtility.FillDropDownList(
                               modsMarksDetails.Tables[2],
                               ref oDropDownList,
                               "ShortName",
                               "DisplayName",
                               string.Empty);

                oDropDownList.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT, "N"));               
                if (hidMarksOrGrades.Value == S_GRADE)
                    oDropDownList.Attributes.Add("onchange", "DisableRelatedControl(this," + (e.Row.RowIndex + 2) + ",'ddlGrade')");

                TextBox txtRemark = (TextBox)e.Row.FindControl("txtRemark");
                if (txtRemark != null)
                {
                    Label lblRemarkLength = (Label)e.Row.FindControl("lblRemarkLength");
                    txtRemark.Attributes.Add("onkeyup", "alertMsgLength(event, this);");
                    txtRemark.Attributes.Add("onchange", "UpdateLength("+e.Row.RowIndex+");");                    
                }

                Label lblRollNo = (Label)e.Row.FindControl("lblRollNo");
                if (lblRollNo != null)
                    lblRollNo.Text = grdStudentMarks.DataKeys[e.Row.RowIndex]["Roll_No"].ToString();

                // iF "Student Joining Date > ExamDate" - then not able to assign exam marks for those student and default "Late Joining" status should be selected.
                if (DateTime.Parse(grdStudentMarks.DataKeys[e.Row.RowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue())
                {
                    oDropDownList.SelectedValue = S_LATE_JOIN;
                    oDropDownList.Enabled = false;
                }
                else
                {
                    if (!AllowLateJoineeOption)
                        oDropDownList.Items.RemoveAt(oDropDownList.Items.Count - 1);
                }

                if (hidIsCoCurricullarSubject.Value == Constants.S_ONE && !Settings.AllowExamStatusForCoCurricullarSubjects)
                    oDropDownList.Enabled = false;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event method is used to varify the test date with student school left date. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void calTestDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            timer.Enabled = false;
            Page.Validate();
            if (Page.IsValid)
                if (!CheckIfTestDateIsValid())
                    FillStudentsGrid();

            if (!Convert.ToBoolean(hidIsExamStatusApplicable.Value))
                DisableExamStatus();
            timer.Enabled = true;
            ViewState[S_ChangedExamDate] = cTestDate.DateValue.ToString(Constants.S_DATE_FORMAT_MARATHI);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is sued to handle row command event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdStudentMarks_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "OPEN_TEMPLATE_POPUP")
            {
                int iRowIndex = Convert.ToInt32(e.CommandArgument);
                hidSelectedStudentId.Value = grdStudentMarks.DataKeys[iRowIndex]["Student_Id"].ToString();
                hidSalutationId.Value = grdStudentMarks.DataKeys[iRowIndex]["SalutationId"].ToString();

                hidFname.Value = grdStudentMarks.DataKeys[iRowIndex]["FName"].ToString();
                hidMname.Value = grdStudentMarks.DataKeys[iRowIndex]["MName"].ToString();
                hidLname.Value = grdStudentMarks.DataKeys[iRowIndex]["LName"].ToString();
                hidSelectedRowIndex.Value = iRowIndex.ToString();


                DropDownList ddlGrade = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlGrade");

                hidMarksGradesConfigurationDetailsId.Value = ddlGrade.SelectedValue;


                lblStudName.Text = grdStudentMarks.DataKeys[iRowIndex]["Name"].ToString();

                if (hidMarksGradesConfigurationDetailsId.Value != Constants.S_ZERO)
                {
                    cmbGradesOnDiv.SelectedValue = hidMarksGradesConfigurationDetailsId.Value;
                    DisplayRemarkTemplates();
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Total", "SetTotal();", true);
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "OpenP", "OpenPopup();", true);
                }
            else
                lblErrorMsg.Text = "Grade should be selected.";
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// this event is used to call bind data to label
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                Label oLabel = e.Item.FindControl("lblTemplate") as Label;
                oLabel.Text = UpdateTemplateText(oLabel.Text);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to save templates from popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPopupSave_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder oTemplates = new StringBuilder();
            foreach (ListViewDataItem Item in lstvwTemplates.Items)
            {
                CheckBox chkTemplate = Item.FindControl("chkTemplate") as CheckBox;
                if (chkTemplate.Checked)
                    oTemplates.Append(" " + (Item.FindControl("lblTemplate") as Label).Text);
            }

            for (int iRowIndex = 0; iRowIndex < grdStudentMarks.Rows.Count; iRowIndex++)
            {
                int iYearwiseStudentId = Convert.ToInt32(grdStudentMarks.DataKeys[iRowIndex]["Student_Id"]);

                if (hidSelectedStudentId.Value.ToInt() == iYearwiseStudentId)
                {
                    if (oTemplates.Length > 0)
                    {
                        TextBox txtId = grdStudentMarks.Rows[iRowIndex].FindControl("txtRemark") as TextBox;
                        txtId.Text = txtId.Text + " "+ oTemplates.ToString().Substring(1);

                        DropDownList ddlExamStatus = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlExamStatus");
                        if (ddlExamStatus != null)
                        {
                            if (ddlExamStatus.SelectedValue != Constants.S_NO)
                            {
                                DropDownList ddlGrade = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlGrade");
                                if (ddlGrade != null)
                                    ddlGrade.Enabled = false;
                            }
                        }
                    }
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CloseP", "RefreshRemarkLength("+hidSelectedRowIndex.Value.ToInt()+");", true);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to sort the listview
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwTemplates_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortExpression.Value = e.SortExpression;
            SetSortDirection();
            DisplayRemarkTemplates();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method gives you the templates according to selected remaark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRemarksOnDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisplayRemarkTemplates();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method gives you the templates according to selected remaark
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbGradesOnDiv_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisplayRemarkTemplates();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save students remark at specific interval of time.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void timer_Tick(object sender, EventArgs e)
    {
        try
        {
            if (hidIsReadOnly.Value == S_FALSE)
            {
                timer.Enabled = false;

                bool bShowRemark = SubjectTestTypeConfigurationCollectionBL.IsTestAndSubjectConfiguredForRemark(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());

                if (bShowRemark)
                {
                    string oRemarkXml = GenerateRemarkXml();

                    StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL
                    {
                        InsertedBYId = miUserId,
                        RemarkXml = oRemarkXml,                        
                        TestId = hidTestId.Value.ToInt(),
                        SubjectId = hidSubjectId.Value.ToInt()
                    };
                    oStudentSubjectMarksBL.SaveStudentwiseRemarks(miSchoolId, miAcademicYearId);
                }

                timer.Enabled = true;
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set roll no.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblRollNo = (Label)e.Row.FindControl("lblRollNo");
                if (lblRollNo != null)
                    lblRollNo.Text = grdStudentMarks.DataKeys[e.Row.RowIndex]["Roll_No"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method

    /// <summary>
    /// This method is used to initialize screen.
    /// </summary>
    private void InitializeForm()
    {
        calTestDate.Focus();
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
        btnSave.Attributes["onclick"] = "javascript:if(!ValidateAcademicYear())return false;";
        btnSave.Attributes.Add("onclick", "if(!IsMarksAreGreaterThanTotalMarks()){return false;}");
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";

        hidConvertDecimalMarks.Value = Constants.S_ZERO;
        if (miSchoolId == Constants.SchoolId.PPSN.ToInt())
            hidConvertDecimalMarks.Value = Constants.S_ONE;
    }

    /// <summary>
    /// Check that whether entered exam date is valid or not.
    /// </summary>
    private bool CheckIfTestDateIsValid()
    {
        bool bIsValid = true;
        DateTime dtTestDate = cTestDate.DateValue;
        if (dtTestDate == DateTime.MinValue)
            return true;
        string sErrorMessage = CheckIfDateIsValidTestDate();
        if (!sErrorMessage.Equals(string.Empty))
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = sErrorMessage;
        }
        else
            bIsValid = false;
        return bIsValid;
    }

    /// <summary>
    /// This method is used to whether the entered exam date is valid or not with future date
    /// , academic year and exam date.
    /// </summary>
    /// <returns></returns>
    private string CheckIfDateIsValidTestDate()
    {
        DateTime dtTestDate = cTestDate.DateValue;
        string sReturn = string.Empty;
        if (dtTestDate >= DateTime.Now)
            sReturn = Resources.LocalizedResources.ExamDateShouldNotBeFutureDate;
        else
        {
            SchoolwiseStandardExamScheduleMasterBL oTestSchedule = new SchoolwiseStandardExamScheduleMasterBL(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidTestId.Value), hidSubjectId.Value.ToInt());
            if (oTestSchedule.Schoolwise_Standard_Exam_Schedule_Id != Constants.I_ZERO)
                if (dtTestDate.Date > oTestSchedule.Exam_End_Date.Date || dtTestDate < oTestSchedule.Exam_Start_Date.Date)
                    sReturn = Resources.LocalizedResources.ExamDateForThisStandardShouldBeBetween.Replace("%Exam_Start_Date%", oTestSchedule.Exam_Start_Date.ToString(Constants.S_STANDARD_DATE_FORMAT)).Replace("%Exam_End_Date%", oTestSchedule.Exam_End_Date.ToString(Constants.S_STANDARD_DATE_FORMAT));
        }

        return sReturn;
    }

    /// <summary>
    /// This method is used to set query string values.
    /// </summary>
    private void SetQueryStringValues()
    {
        hidStandardDivisionId.Value = QueryString["StandardDivisionId"] ?? Constants.S_ZERO;
        hidSelectedStandardDivisionId.Value = QueryString["SelectedStandardDivisionId"] ?? Constants.S_ZERO;
        hidSubjectId.Value = QueryString["SubjectId"] ?? Constants.S_ZERO;
        hidTestId.Value = QueryString["TestId"] ?? Constants.S_ZERO;
        hidTeacherId.Value = QueryString["TeacherId"] ?? Constants.S_ZERO;
        hidIsReadOnly.Value = QueryString["IsReadOnly"] ?? S_FALSE;

        if (moSchool == Constants.SchoolId.VPMCPS)
            hidShowTotalGrade.Value = Constants.S_NO;
        else
            hidShowTotalGrade.Value = Constants.S_YES;

        if ((QueryString["IsPublish"] != null)
            && (QueryString["IsPublish"] == Constants.S_YES))
        {
            lblSubmitMessage.Text = Resources.LocalizedResources.ResultsForThisExamHasBeenPublished;
            pnlSubmitStatus.Visible = true;
        }

        hidCanOverride.Value = QueryString["CanOverride"] ?? S_FALSE;
        hidStandardId.Value = QueryString["StandardId"] ?? Constants.S_ZERO;

        // Standardwise academic year change.
        if (Convert.ToInt32(hidStandardId.Value) > Constants.I_ZERO)
        {
            DataTable oDT = SchoolWiseAcademicYearMasterBL.GetAcademicDatesForStandard(
                                                           miSchoolId,
                                                           miAcademicYearId,
                                                           Convert.ToInt32(hidStandardId.Value));

            hidAcademicStartDate.Value = oDT.Rows[0]["StartDate"].ToString();
            hidAcademicEndDate.Value = oDT.Rows[0]["EndDate"].ToString();
        }
        else
        {
            hidAcademicStartDate.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_START_DATE]);
            hidAcademicEndDate.Value = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_END_DATE]);
        }

        hidShowGrade.Value = Constants.S_ONE;
        if (miSchoolId == Constants.SchoolId.PPSH.ToInt())
            hidShowGrade.Value = Constants.S_ZERO;
    }

    /// <summary>
    /// /This method is used to get encrypted query string.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <returns></returns>
    private string GetEncryptedTestQueryString(int aiTestId)
    {
        string sQuerystring = "TestId=" + aiTestId;
        sQuerystring = sQuerystring + "&TeacherId=" + hidTeacherId.Value;
		sQuerystring = sQuerystring + "&StandardDivisionId=" + hidStandardDivisionId.Value;
        sQuerystring = sQuerystring + "&SelectedStandardDivisionId=" + hidSelectedStandardDivisionId.Value;
        /*Can override means you are here from exam result screen and 
        /admin is going to override marks submitted my subject teacher.*/
        string sUrl = !hidCanOverride.Value.Equals(bool.TrueString) ? "~/Teacher/TestMarksConfigurationUI.aspx?" : "~/Teacher/ClassTeacherTestMarksUI.aspx?";
        string sEncryptedString = sUrl + CommonUtility.EncryptQuerystring(sQuerystring);

        return sEncryptedString;
    }

    /// <summary>
    /// This method is used to hide or display grade columns of grid.
    /// </summary>
    /// <param name="abDisplay"></param>
    private void HideOrDisplayGradeColumns(bool abDisplay)
    {
        grdStudentMarks.Columns[I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX].Visible = abDisplay;
        grdStudentMarks.Columns[I_ASSIGNED_GRADE_COLUMN_INDEX].Visible = abDisplay;
        grdStudentMarks.Columns[I_REMARK_COLUMN_INDEX].Visible = false;
        bool bShowRemark = SubjectTestTypeConfigurationCollectionBL.IsTestAndSubjectConfiguredForRemark(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());
        if (bShowRemark)
            grdStudentMarks.Columns[I_REMARK_COLUMN_INDEX].Visible = abDisplay;
    }

    /// <summary>
    /// Fills all the students for current academic year for specified standard division.
    /// </summary>
    private void CreateStudentsGrid()
    {
        AddTestTypeColumnsAndDisplayMarks();
        MakegridReadOnly(bool.Parse(hidIsReadOnly.Value));
        MakeFormFieldsReadOnly(bool.Parse(hidIsReadOnly.Value));
    }

    /// <summary>
    /// Fills all the students for current academic year for specified standard division.
    /// </summary>
    private void FillStudentsGrid()
    {
        DataTable oDSStudents = StudentBL.GetAllStudentsForSubject(
                                          miSchoolId,
                                          Convert.ToInt32(hidStandardDivisionId.Value),
                                          miAcademicYearId,
                                          cTestDate.DateValue.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime(),
                                          Convert.ToInt32(hidSubjectId.Value));
        grdStudentMarks.DataSource = oDSStudents.DefaultView;
        grdStudentMarks.DataBind();

        if (ShowAdditionalGrid)
        {
            grdvwStudents.DataSource = oDSStudents.DefaultView;
            grdvwStudents.DataBind();
        }

        calTestDate.Enabled = btnSave.Enabled = grdStudentMarks.Rows.Count > 0;
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(Convert.ToInt32(hidStandardDivisionId.Value));
        int iStandardId = oStandardDivisionMasterBL.StandardId;

        DataTable odtGradeDetails = MarksGradesConfigurationBL.GetAllGradesForStandard(miSchoolId, miAcademicYearId, iStandardId, hidSubjectId.Value.ToInt());
       
        /*Get all grades applicable for this standard and assign to hidden field
        his hidden field value then get utilise to show grades dynamically for each marks entered.*/
        string sGradeSeparator = string.Empty;
        StringBuilder oStringBuilder = new StringBuilder();
        foreach (DataRow oDataRow in odtGradeDetails.Rows)
        {
            oStringBuilder.Append(sGradeSeparator);
            oStringBuilder.Append(oDataRow["Grade_Name"]);
            oStringBuilder.Append(":");
            oStringBuilder.Append(oDataRow["Starting_Marks_Range"]);
            oStringBuilder.Append(":");
            oStringBuilder.Append(oDataRow["Actual_Ending_Marks_Range"]);
            sGradeSeparator = "#";
        }

        HidGradeRange.Value = oStringBuilder.ToString();
        if (hidMarksOrGrades.Value == S_GRADE)
            DisplayGrades(odtGradeDetails);

        // Display already assigned test marks for each student.
        SetExamStatus(modsMarksDetails.Tables[0]);
        DisplayStudentMarks(modsMarksDetails.Tables[1], odtGradeDetails);
        MakegridReadOnly(bool.Parse(hidIsReadOnly.Value));
        DisplayRemark(modsMarksDetails.Tables[4]);
    }

    /// <summary>
    /// This method is used to set exam status in exam status dropdown list.
    /// </summary>
    /// <param name="aoDtMarks"></param>
    private void SetExamStatus(DataTable aoDtMarks)
    {
        for (int iRowIndex = 0; iRowIndex < grdStudentMarks.Rows.Count; iRowIndex++)
        {
            foreach (DataRow oDRStudentMarks in aoDtMarks.Rows)
            {
                string sHeaderText1 = Convert.ToString(oDRStudentMarks["TestType_Name"]) + "/" + Convert.ToString(oDRStudentMarks["TestType_Total_Marks"]) + "IsAbsent";
                DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl(sHeaderText1);
                if (oDropDownList != null)
                {
                    string str = oDropDownList.ClientID.Replace(sHeaderText1.Replace(" ", string.Empty) + "IsAbsent", "txtTotalMarks");
                    oDropDownList.Attributes.Add("onchange", "DisableMarksControl(this,'" + oDropDownList.ClientID.Replace("IsAbsent", "txtMarks") + "','" + str + "')");
                }
            }
        }
    }

    /// <summary>
    /// This method is used to generate column template for all applicable test types dynamically to the grid.
    /// </summary>
    private void AddTestTypeColumnsAndDisplayMarks()
    {
        int iStdDivId = Convert.ToInt32(hidStandardDivisionId.Value);
        int iSubjectId = Convert.ToInt32(hidSubjectId.Value);
        int iTestId = Convert.ToInt32(hidTestId.Value);
        string ShowTotalAsPerOutOfMarks = Settings.ShowTotalAsPerOutOfMarks ? Constants.S_YES : Constants.S_NO;
        modsMarksDetails = SubjectTestTypeConfigurationCollectionBL.GetAllTestTypesForStandardDivisionSubjectTest(iStdDivId, iSubjectId, iTestId, miSchoolId, miAcademicYearId, ShowTotalAsPerOutOfMarks);

        // Set Test date if test marks are configured.
        // If Student marks are save from "Studntwise Progress Report" that time for those student Exam date is save as "Marks save date" even if Exam date is configured or not.
        // In this case if in "modsMarksDetails.Tables[1]" table first row is any of these student then in this case Exam Date is set as "Marks save date" and not as "Exam Date".
        // So that here we just find that get the top most student's exam Date whose Marks are saved from "Assign Exam Marks" and set this date to Date control.
        if (modsMarksDetails.Tables[1].Rows.Count > Constants.I_ZERO && modsMarksDetails.Tables[1].Select("IsSavedForSingleStudent =" + 0).Count() > Constants.I_ZERO)
        {
            DataRow[] oDrDate = modsMarksDetails.Tables[1].Select("IsSavedForSingleStudent =" + 0);
            SchoolwiseStandardExamScheduleMasterBL oTestSchedule = new SchoolwiseStandardExamScheduleMasterBL(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidTestId.Value), Convert.ToInt32(hidSubjectId.Value));
            if (oTestSchedule.Schoolwise_Standard_Exam_Schedule_Id != Constants.I_ZERO)
            {
                if (oTestSchedule.SubjectExamStartDate.Date == Convert.ToDateTime(oDrDate[0]["Test_Date"]))
                {
                    cTestDate.DateValue = Convert.ToDateTime(oDrDate[0]["Test_Date"].ToString());
                    cTestDate.Visible = false;
                    spExamCalendar.Visible = false;
                }
                else
                {
                    cTestDate.DateValue = Convert.ToDateTime(oDrDate[0]["Test_Date"].ToString());
                    //cTestDate.DateValue = Convert.ToDateTime(oTestSchedule.SubjectExamStartDate);
                    cTestDate.Visible = true;
                    spExamCalendar.Visible = true;
                }
            }
            else
            {
                cTestDate.DateValue = Convert.ToDateTime(oDrDate[0]["Test_Date"].ToString());
                cTestDate.To.Date = DateTime.Today.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime();
            }            
        }
        else
        {
            SchoolwiseStandardExamScheduleMasterBL oTestSchedule = new SchoolwiseStandardExamScheduleMasterBL(Convert.ToInt32(hidStandardId.Value), Convert.ToInt32(hidTestId.Value),Convert.ToInt32(hidSubjectId.Value));
            if (oTestSchedule.Schoolwise_Standard_Exam_Schedule_Id != Constants.I_ZERO)
            {                
                cTestDate.DateValue = oTestSchedule.SubjectExamStartDate.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime();                
                msIsExamScheduleDate = "Y";
                cTestDate.Visible = false;
                spExamCalendar.Visible = false;
                if (DateTime.Today < oTestSchedule.SubjectExamStartDate.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime())
                {
                    msIsDisableSave = Constants.S_YES;
                }
                else
                {
                    msIsDisableSave = Constants.S_NO;
                }
            }
            else
            {
                cTestDate.DateValue = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime();
                cTestDate.Visible = true;
                spExamCalendar.Visible = true;
                cTestDate.To.Date = DateTime.Today.ToString(Constants.S_DATE_FORMAT_MARATHI).ToDateTime();
            }
        }

        // if there are test types.
        DataRow[] oMarksDataRow = modsMarksDetails.Tables[3].Select();
        if (oMarksDataRow.Length > Constants.I_ZERO)
        {
            hidTestOutOfMarksAvailable.Value = oMarksDataRow[0]["TestOutOfMarksAvailable"].ToString();
            hidTestTypeOutOfMarksAvailable.Value = oMarksDataRow[0]["TestTypeOutOfMarksAvailable"].ToString();
            hidTestOutOfMarks.Value = oMarksDataRow[0]["TestOutOfMarks"].ToString();
            hidIsCoCurricullarSubject.Value = Convert.ToString(oMarksDataRow[0]["IsCoCurricullar"].ToInt());
            hidShowTotalAsPerOutOfMarks.Value = Settings.ShowTotalAsPerOutOfMarks ? Constants.S_YES : Constants.S_NO;
        }

        if (modsMarksDetails.Tables[0].Rows.Count > Constants.I_ZERO && modsMarksDetails.Tables[0].Rows[0]["Grade_Or_Marks"].ToString() == S_CONSTANT_MARKS)
        {
            TemplateField customField;
            StringBuilder sAllTestTypes = new StringBuilder();

            DataTable dtExamStatus = modsMarksDetails.Tables[2].Copy();
            DataRow oRow = dtExamStatus.NewRow();
            oRow["ShortName"] = "N";
            oRow["DisplayName"] = Constants.S_SELECT;
            dtExamStatus.Rows.InsertAt(oRow, Constants.I_ZERO);
            bool mbIsCoCurricullar = true;
            if (hidIsCoCurricullarSubject.Value == Constants.S_ONE && !Settings.AllowExamStatusForCoCurricullarSubjects)
                mbIsCoCurricullar = false;

            int iRowIndex = 0;
            // Each test type will have two column one for marks and another for Is_Absent
            foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
            {
                string sHeaderText = Resources.LocalizedResources.ExamStatus;
                customField = new TemplateField
                                  {
                                      HeaderTemplate = new GridViewDropDownListTemplate(DataControlRowType.Header, sHeaderText, new DataTable(), true, mbIsCoCurricullar, AllowLateJoineeOption)
                                  };
                sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                customField.ItemTemplate = new GridViewDropDownListTemplate(DataControlRowType.DataRow, sHeaderText, dtExamStatus, Convert.ToBoolean(hidIsExamStatusApplicable.Value), mbIsCoCurricullar, AllowLateJoineeOption);
                customField.ControlStyle.Width = Unit.Pixel(100);
                customField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                grdStudentMarks.Columns.Add(customField);
                sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                bool bTestTypeOutOfMarksApplicable = Convert.ToInt32(oDataRow["TestTypeOutOfMarks"]) != Constants.I_ZERO && Convert.ToInt32(oDataRow["TestType_Total_Marks"]) != Convert.ToInt32(oDataRow["TestTypeOutOfMarks"]);
                customField = new TemplateField
                                  {
                                      ItemTemplate = new GridViewTextBoxTemplate(
                                                         DataControlRowType.DataRow,
                                                         sHeaderText,
                                                         "TxtAlignCenter",
                                                         hidShowTotalAsPerOutOfMarks.Value == Constants.S_YES && bTestTypeOutOfMarksApplicable,
                                                         sHeaderText.Replace(" ", string.Empty) + "txtMarks", hidAllowDecimal.Value.ToBool(), iRowIndex),
                                      HeaderTemplate = new GridViewTextBoxTemplate(
                                                           DataControlRowType.Header,
                                                           sHeaderText,
                                                           "TxtAlignCenter",
                                                           hidShowTotalAsPerOutOfMarks.Value == Constants.S_YES && bTestTypeOutOfMarksApplicable,

                                                           sHeaderText.Replace(" ", string.Empty) + "txtMarks", hidAllowDecimal.Value.ToBool(),
                                                           iRowIndex
                                                           )
                                  };

                if (ShowAdditionalGrid)
                    customField.ControlStyle.Width = Unit.Pixel(150);
                else
                    customField.ControlStyle.Width = Unit.Pixel(50);

                customField.ItemStyle.Wrap = false;
                customField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                customField.HeaderStyle.CssClass = "ClsPaddingL";
                customField.ItemStyle.CssClass = "ClsPaddingL";
                grdStudentMarks.Columns.Add(customField);
                sAllTestTypes.Append(sHeaderText.Replace(" ", string.Empty) + ";" + oDataRow["TestTypeOutOfMarks"]);
                sAllTestTypes.Append("||");
                iRowIndex++;
            }

            string sHeader = Resources.LocalizedResources.Total + ((hidShowTotalAsPerOutOfMarks.Value == Constants.S_YES && modsMarksDetails.Tables[0].Rows.Count > 0) ? " / " + modsMarksDetails.Tables[0].Rows[0]["TotalMarks"].ToString() : string.Empty);
            // Add total column after test types.
            customField = new TemplateField
                              {
                                  ItemTemplate = new GridViewTextBoxTemplate(DataControlRowType.DataRow, sHeader, "TxtAlignCenterBLeftPad", false, "txtTotalMarks", hidAllowDecimal.Value.ToBool(), Constants.I_DEFAULT_MAX_VALUE),
                                  HeaderTemplate = new GridViewTextBoxTemplate(DataControlRowType.Header, sHeader, "TxtAlignCenterBLeftPad", false, "txtTotalMarks", hidAllowDecimal.Value.ToBool(), Constants.I_DEFAULT_MAX_VALUE)
                              };
            customField.ControlStyle.Width = Unit.Pixel(50);
            customField.ItemStyle.Wrap = false;
            customField.HeaderStyle.CssClass = "ClsPaddingL";
            customField.ItemStyle.CssClass = "ClsPaddingL";
            customField.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            grdStudentMarks.Columns.Add(customField);

            sAllTestTypes.Remove(sAllTestTypes.Length - 2, 2);
            hidAllTestTypes.Value = sAllTestTypes.ToString();
        }
    }

    /// <summary>
    /// Fills combo box for each row if the grades are to be assigned to students.
    /// </summary>
    private void FillGradesCombobox(DataTable aoDtGradeDetails)
    {
        DropDownList oddlHeaderGrades = (DropDownList)grdStudentMarks.HeaderRow.FindControl("ddlHeaderGrade");
        oddlHeaderGrades.Attributes.Add("onchange", "SetSelectedGradeForAllRows();");
        oddlHeaderGrades.Bind(aoDtGradeDetails, "Marks_Grades_Configuration_Detail_ID", "Grade_Name", Constants.S_SELECT);

        for (int iRowIndex = 0; iRowIndex < grdStudentMarks.Rows.Count; iRowIndex++)
        {
            DropDownList oddlGrades = (DropDownList)grdStudentMarks.Rows[iRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlGrade");
            oddlGrades.Bind(aoDtGradeDetails, "Marks_Grades_Configuration_Detail_ID", "Grade_Name", Constants.S_SELECT);
            if (iRowIndex == Constants.I_ZERO)
                oddlGrades.Focus();

            /// If "Student Joining Date > ExamDate" - then not able to assign exam marks for those student and default "Late Joining" status should be selected.
            if (DateTime.Parse(grdStudentMarks.DataKeys[iRowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue() && hidIsReadOnly.Value == S_FALSE)
            {
                oddlGrades.SelectedValue = S_LATE_JOIN;
                oddlGrades.Enabled = false;
            }
        }
    }

    /// <summary>
    /// this method is used to display grades.
    /// </summary>
    /// <param name="aoDtGradeDetails"></param>
    private void DisplayGrades(DataTable aoDtGradeDetails)
    {
        HideOrDisplayGradeColumns(true);
        FillGradesCombobox(aoDtGradeDetails);
    }

    /// <summary>
    /// Display data in respective controls.
    /// </summary>
    private void DisplayData()
    {
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL();
        DataSet oDSInfo = oStudentSubjectMarksBL.GetAllRelatedInformation(miSchoolId, miAcademicYearId, Convert.ToInt32(hidSubjectId.Value), Convert.ToInt32(hidTestId.Value), Convert.ToInt32(hidStandardDivisionId.Value));

        // Dataset contains 3 tables - 1. standard-division name, 2. subject name, 3. test relation information.
        hidSchoolSubjectTestId.Value = oDSInfo.Tables[2].Rows[0]["TestWise_Subject_Marks_Id"].ToString();
        hidAllowDecimal.Value = oDSInfo.Tables[2].Rows[0]["AllowDecimal"].ToString();
        trNote.Visible = hidAllowDecimal.Value.ToBool();        

        if (oDSInfo.Tables[2].Rows[0]["Grade_Or_Marks"].ToString() == S_CONSTANT_MARKS)
        {
            hidTimerVisibleState.Value = S_CONSTANT_MARKS;
            timer.Enabled = false;
            lblDataPassingMarks.Text = oDSInfo.Tables[2].Rows[0]["Passing_Total_Marks"].ToDecimal().ToString("0.#");
            lblDataTotalMarks.Text = oDSInfo.Tables[2].Rows[0]["Subject_Total_Marks"].ToString();
            hidMarksOrGrades.Value = S_CONSTANT_MARKS;
            trGrade.Visible = false;
        }
        else
        {
            bool bShowRemark = SubjectTestTypeConfigurationCollectionBL.IsTestAndSubjectConfiguredForRemark(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());
            if (!bShowRemark)
            {
                timer.Enabled = false;
                hidTimerVisibleState.Value = S_CONSTANT_MARKS;
            }
            else
                hidTimerVisibleState.Value = oDSInfo.Tables[2].Rows[0]["Grade_Or_Marks"].ToString();

            lblDataPassingMarks.Visible = false;
            lblDataTotalMarks.Visible = false;
            lblTotalMarks.Visible = false;
            lblPassingMarks.Visible = false;
            trMarks.Visible = false;
            trGrade.Visible = true;
            lblPassingGrade.Text = oDSInfo.Tables[2].Rows[0]["Grade_Name"].ToString();
            hidMarksOrGrades.Value = S_GRADE;
        }
        

        // standard division.
        lblDataStdDiv.Text = oDSInfo.Tables[0].Rows[0]["Standard_Name"] + " - "
                            + oDSInfo.Tables[0].Rows[0]["Division_Name"];

        // subject name
        lblDataSubjectName.Text = oDSInfo.Tables[1].Rows[0]["Subject_Name"].ToString();
        hidStandardId.Value = oDSInfo.Tables[0].Rows[0]["Standard_Id"].ToString();

        // Exam name 
        LblDataExam.Text = oDSInfo.Tables[2].Rows[0]["SchoolWise_Test_Name"].ToString();

        if (ShowAdditionalGrid)
        {
            pnl1.ScrollBars = ScrollBars.Horizontal;
            SetFieldVisibility(false);
        }
        else
        {
            SetFieldVisibility(true);
            divMarks.Style.Add("width","100%");
        }
        
        hidIsExamStatusApplicable.Value = oDSInfo.Tables[0].Rows[0]["IsExamStatusApplicable"].ToString();
    }

    /// <summary>
    /// This method is used to set field visibility,
    /// </summary>
    /// <param name="abShow"></param>
    private void SetFieldVisibility(bool abShow)
    {
        grdStudentMarks.Columns[0].Visible = abShow;
        grdStudentMarks.Columns[1].Visible = abShow;
        tdStudGrid.Visible = !abShow;
    }

    /// <summary>
    /// Display already assigned marks for students.
    /// </summary>
    /// <param name="aoDtMarks"></param>
    /// <param name="aoDtGradeDetails"></param>
    private void DisplayStudentMarks(DataTable aoDtMarks, DataTable aoDtGradeDetails)
    {
        for (int iRowIndex = 0; iRowIndex < grdStudentMarks.Rows.Count; iRowIndex++)
        {
            int iStudentId = Convert.ToInt32(grdStudentMarks.DataKeys[iRowIndex]["Student_Id"]);
            DataRow[] oArrRows = aoDtMarks.Select("Student_Id =" + iStudentId);

            if (oArrRows.Count() < 1)
            {
                foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
                {
                    string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                    DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent");
                    if (oDropDownList == null)
                        oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlExamStatus");
                    if (DateTime.Parse(grdStudentMarks.DataKeys[iRowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue())
                    {
                        oDropDownList.Items.Add(new ListItem(S_LATE_JOIN_DISPLAY_VALUE, S_LATE_JOIN));
                        oDropDownList.SelectedValue = S_LATE_JOIN;
                        if (hidMarksOrGrades.Value == "M")
                        {
                            TextBox otxtMarks = (TextBox)grdStudentMarks.Rows[iRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks");
							if (Settings.AllowMarksEntryForLateJoin)
							{
								oDropDownList.Enabled = true;
								otxtMarks.Enabled = false;
							}
							else

								oDropDownList.Enabled = otxtMarks.Enabled = false;
                        }
                        else
                        {
                            DropDownList oddlGrade = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlGrade");
                            oddlGrade.Enabled = oDropDownList.Enabled = false;
                        }
                    }
                }

                // If not for a single student marks (Grade) is save, in this case only Late Joinied Student have option to set "Late Joining" status.
                if (modsMarksDetails.Tables[0].Rows.Count == Constants.I_ZERO)
                {
                    if (DateTime.Parse(grdStudentMarks.DataKeys[iRowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue())
                    {
                        DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlExamStatus");
                        oDropDownList.Items.Add(new ListItem(S_LATE_JOIN_DISPLAY_VALUE, S_LATE_JOIN));
                    }
                }
            }

            // If datatable contains rows for current roll number then display marks in resp. cells.
            foreach (DataRow oDRStudentMarks in oArrRows)
            {
                if ((hidMarksOrGrades.Value == "M" && oDRStudentMarks["Assigned_Grade_Id"].ToString() == Constants.S_ZERO) || (hidMarksOrGrades.Value == "M" && string.IsNullOrEmpty(oDRStudentMarks["Assigned_Grade_Id"].ToString())))
                {
                    string sHeaderText1 = Convert.ToString(oDRStudentMarks["TestType_Name"]) + "/" + Convert.ToString(oDRStudentMarks["TestType_Total_Marks"]) + "IsAbsent";
                    string sHeaderText = Convert.ToString(oDRStudentMarks["TestType_Name"]) + " / " + Convert.ToString(oDRStudentMarks["TestType_Total_Marks"]);
                    TextBox otxtMarks = (TextBox)grdStudentMarks.Rows[iRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks");

                    DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl(sHeaderText1.Replace(" ", string.Empty));
                    decimal dMarks = oDRStudentMarks["Marks_Scored"].ToDecimal();
                    if (oDropDownList != null)
                    {
                        oDropDownList.SelectedValue = oDRStudentMarks["Is_Absent"].ToString();
                        string str = oDropDownList.ClientID.Replace(sHeaderText1.Replace(" ", string.Empty) + "IsAbsent", "txtTotalMarks");
                        oDropDownList.Attributes.Add("onchange", "DisableMarksControl(this,'" + oDropDownList.ClientID.Replace("IsAbsent", "txtMarks") + "','" + str + "')");
						if (Settings.AllowMarksEntryForLateJoin)
						{
							if (oDRStudentMarks["Is_Absent"].ToString() == S_LATE_JOIN || DateTime.Parse(grdStudentMarks.DataKeys[iRowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue())
							{
								oDropDownList.Items.Add(new ListItem(S_LATE_JOIN_DISPLAY_VALUE, S_LATE_JOIN));
                                if (dMarks == Constants.I_ZERO.ToDecimal())
                                     oDropDownList.SelectedValue = oDRStudentMarks["Is_Absent"].ToString();                                
							}
						}
						else
						{
							if (oDRStudentMarks["Is_Absent"].ToString() == S_LATE_JOIN || DateTime.Parse(grdStudentMarks.DataKeys[iRowIndex]["Joining_Date"].ToString()) > cTestDate.GetDateValue())
							{
								oDropDownList.Items.Add(new ListItem(S_LATE_JOIN_DISPLAY_VALUE, S_LATE_JOIN));
								
									oDropDownList.SelectedValue = S_LATE_JOIN;
								oDropDownList.Enabled = otxtMarks.Enabled = false;
								dMarks = Constants.I_ZERO.ToDecimal();
							}
						}
                    }

                    SetMarksToTextBox(dMarks, oDRStudentMarks["TestType_Total_Marks"].ToString(), iRowIndex, sHeaderText.Replace(" ", string.Empty) + "txtMarks", oDRStudentMarks["Is_Absent"].ToString(), aoDtGradeDetails, Convert.ToInt32(oDRStudentMarks["TestType_Id"]));

                    string sSubjectTotalMarks = oDRStudentMarks["Subject_Total_Marks"].ToString();
                    if (dMarks != Constants.I_ZERO.ToDecimal())
                        dMarks = oDRStudentMarks["Marks_Scored"].ToDecimal();
                    if (hidShowTotalAsPerOutOfMarks.Value == Constants.S_YES && (hidTestOutOfMarksAvailable.Value == Constants.S_YES || hidTestTypeOutOfMarksAvailable.Value == Constants.S_YES))
                    {
                        sSubjectTotalMarks = hidTestOutOfMarks.Value;
                        dMarks = oDRStudentMarks["Total_Marks_Scored"].ToDecimal();
                    }
                    SetMarksToTextBox(dMarks, sSubjectTotalMarks, iRowIndex, "txtTotalMarks", Constants.C_NO.ToString(), aoDtGradeDetails, Convert.ToInt32(oDRStudentMarks["TestType_Id"]));
                }
                else
                {
                    DropDownList ddlExamStatus = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlExamStatus");
                    if (ddlExamStatus != null)
                    {
                        ddlExamStatus.SelectedValue = oDRStudentMarks["Is_Absent"].ToString();
                        if (oDRStudentMarks["Is_Absent"].ToString() == S_LATE_JOIN && oDRStudentMarks["Joining_Date"].ToDateTime() > cTestDate.GetDateValue())
                        {
                            ddlExamStatus.Items.Add(new ListItem(S_LATE_JOIN_DISPLAY_VALUE, S_LATE_JOIN));
                            ddlExamStatus.Enabled = false;
                        }
                    }

                    DropDownList oddlGrades = (DropDownList)grdStudentMarks.Rows[iRowIndex].FindControl("ddlGrade");
                    if (modsMarksDetails.Tables[2].Select("ShortName = '" + oDRStudentMarks["Is_Absent"].ToString() + "'").Count() > Constants.I_ZERO)
                    {
                        if ((oDRStudentMarks["Is_Absent"].ToString() == "J" && oDRStudentMarks["Joining_Date"].ToDateTime() > cTestDate.GetDateValue()) || oDRStudentMarks["Is_Absent"].ToString() != "J")
                        {
                            oddlGrades.Enabled = false;
                            oddlGrades.Text = string.Empty;
                        }
                    }
                    else
                    {
                        ListItem oListItem = oddlGrades.Items.FindByValue(oDRStudentMarks["Assigned_Grade_Id"].ToString());
                        if (oListItem != null)
                            oListItem.Selected = true;
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method is used to display remark.
    /// </summary>
    /// <param name="aodtRemarkDetails"></param>
    private void DisplayRemark(DataTable aodtRemarkDetails)
    {
        bool bShowRemark = SubjectTestTypeConfigurationCollectionBL.IsTestAndSubjectConfiguredForRemark(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());
        if (bShowRemark)
        {
            RemarksConfigurationBL oRemarksConfigurationBL = new RemarksConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            int iRemarkLength = oRemarksConfigurationBL.GetConfiguredMaxRemarkLength(Convert.ToInt32(hidSubjectId.Value), hidTestId.Value.ToInt(), hidStandardId.Value.ToInt());
            iRemarkLength = iRemarkLength == 0 ? Settings.RemarkLength : iRemarkLength;
            hidRemarkLength.Value = iRemarkLength.ToString();
        
            for (int iRowIndex = 0; iRowIndex < grdStudentMarks.Rows.Count; iRowIndex++)
            {
                int iStudentId = Convert.ToInt32(grdStudentMarks.DataKeys[iRowIndex]["Student_Id"]);
                
                TextBox txtRemark = (TextBox)grdStudentMarks.Rows[iRowIndex].FindControl("txtRemark");
                Label lblRemarkLength = (Label)grdStudentMarks.Rows[iRowIndex].FindControl("lblRemarkLength");
                if (aodtRemarkDetails.Rows.Count > 0)
                {
                    DataRow[] dr = aodtRemarkDetails.Select("YearwiseStudentId=" + iStudentId);
                    if (dr.Length > 0)
                    {
                        txtRemark.Text = dr[0]["RemarkDetails"].ToString();
                        lblRemarkLength.Text = "(" + (iRemarkLength - dr[0]["RemarkDetails"].ToString().Length) + ")";
                    }
                    else
                        lblRemarkLength.Text = "(" + iRemarkLength + ")";
                }
                else
                    lblRemarkLength.Text = "(" + iRemarkLength + ")";
            }
        }
    }

    /// <summary>
    /// This method is used to set marks for students grid textboxes
    /// </summary>
    /// <param name="adValue"></param>
    /// <param name="asTotalMarksValue"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="asControlName"></param>
    /// <param name="asIsAbsent"></param>
    /// <param name="aoDtGradeDetails"></param>
    private void SetMarksToTextBox(decimal adValue, string asTotalMarksValue, int aiRowIndex, string asControlName, string asIsAbsent, DataTable aoDtGradeDetails, int aiTestTypeId)
    {
        TextBox aoTextBox = (TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(asControlName);

        // if ShowTotalAsPerOutOfMarks flag in setting file is "N" then we will display total.
        if (hidShowTotalAsPerOutOfMarks.Value == Constants.S_NO)
        {
            if (hidAllowDecimal.Value.ToBool())
                aoTextBox.Text = (aoTextBox.Text == string.Empty ? adValue : aoTextBox.Text.ToDecimal() + adValue).ToDecimal().ToString("0.#");
            else
            {
                //aoTextBox.Text = (aoTextBox.Text == string.Empty ? adValue : aoTextBox.Text.ToDecimal() + adValue).ToString("0.#");
                if (aoTextBox.Text == string.Empty)
                {
                    aoTextBox.Text = adValue.ToString("0.#");
                }
                else
                {
                    adValue = aoTextBox.Text.ToDecimal() + adValue;
                    aoTextBox.Text = adValue.ToString("0.#");
                }
            }
        }
        else
            aoTextBox.Text = adValue.ToString("0.#");

        if (modsMarksDetails.Tables[2].Select("ShortName = '" + asIsAbsent + "'").Count() > Constants.I_ZERO)
        {
            aoTextBox.Enabled = false;
            aoTextBox.Text = string.Empty;
        }
        else
        {
            decimal dcParcentage = aoTextBox.Text.ToDecimal() * 100 / asTotalMarksValue.ToDecimal();
            if (miSchoolId == Constants.SchoolId.DYPV.ToInt())  //////
            {
                dcParcentage = aoTextBox.Text.ToDecimal();
            }

            DataRow[] oDataRow = aoDtGradeDetails.Select(string.Format("{0} >= Starting_Marks_Range AND {0}<= Actual_Ending_Marks_Range", dcParcentage));
            if (oDataRow.Length > Constants.I_ZERO)
            {
                if (miSchoolId != Constants.SchoolId.PPSH.ToInt())
                {
                    Label oLabel = (Label)grdStudentMarks.Rows[aiRowIndex].FindControl(asControlName + "lbl");
                    oLabel.Text = Convert.ToString(oDataRow[0]["Grade_Name"]);
                }
            }

            if (asControlName != "txtTotalMarks")
            {
                oDataRow = modsMarksDetails.Tables[0].Select("TestType_Id = " + aiTestTypeId);
                if (oDataRow.Length > 0)
                {
                    int iTesTypeOutOfMarks = Convert.ToInt32(oDataRow[0]["TestTypeOutOfMarks"]);
                    int iTestTypeTotalMarks = Convert.ToInt32(oDataRow[0]["TestType_Total_Marks"]);
                    if (hidShowTotalAsPerOutOfMarks.Value == Constants.S_YES && iTesTypeOutOfMarks != 0 && iTesTypeOutOfMarks != iTestTypeTotalMarks)
                    {
                        Label oLabel = (Label)grdStudentMarks.Rows[aiRowIndex].FindControl(asControlName + "lblConvertedMarks");
                        oLabel.Text = "(" + Convert.ToString(Math.Round(adValue * iTesTypeOutOfMarks / iTestTypeTotalMarks, hidAllowDecimal.Value.ToBool() ? Constants.I_ONE : Constants.I_ZERO,MidpointRounding.AwayFromZero)) + "/" + iTesTypeOutOfMarks + ")";
                    }
                }
            }
        }
    }

    /// <summary>
    /// Generate XML for the students.
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentXml()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolWiseStudentTestMarks");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarks", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= grdStudentMarks.Rows.Count - 1; iRowCount++)
        {
            // If students marks are entered then only add the node in the XML.
            if (CheckIfDataEntered(iRowCount))
            {
                // Create root xml element.
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMark", string.Empty);

                string sAtrrName = "School_Id";
                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = miSchoolId.ToString();

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Academic_Year_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = miAcademicYearId.ToString();

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Student_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = grdStudentMarks.DataKeys[iRowCount].Value.ToString();

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Subject_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = hidSubjectId.Value;

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "TestWise_Subject_Marks_Id";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = hidSchoolSubjectTestId.Value;

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Test_Date";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = (ViewState[S_ChangedExamDate] == null || ViewState[S_ChangedExamDate].ToString() == string.Empty) ? cTestDate.DateValue.ToString(Constants.S_DATE_FORMAT_MARATHI) : ViewState[S_ChangedExamDate].ToDateTime().ToString(Constants.S_DATE_FORMAT_MARATHI);

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "IsSavedForSingleStudent";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = modsMarksDetails.Tables[1].Rows.Count > Constants.I_ZERO && modsMarksDetails.Tables[1].Select("Student_Id =" + grdStudentMarks.DataKeys[iRowCount]["Student_Id"].ToString()).Count() > 0 ? modsMarksDetails.Tables[1].Select("Student_Id =" + grdStudentMarks.DataKeys[iRowCount]["Student_Id"].ToString()).First()[20].ToString() : "False";

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "Total_Marks_Scored";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = GetTotalMarksForStudent(iRowCount);

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "IsAbsent";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = IsStudentAbsentforAllTestTypes(iRowCount);

                oXmlNode.Attributes.Append(attr);

                sAtrrName = "IsOptional";
                attr = oDoc.CreateAttribute(sAtrrName);
                attr.Value = Constants.C_NO.ToString();

                oXmlNode.Attributes.Append(attr);

                // Add the node to root node.
                oXmlRootNode.AppendChild(oXmlNode);
            }
        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method checks the marking system(grade or mark) of the test
    /// and cheks if the data is entered in given row.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private bool CheckIfDataEntered(int aiRowIndex)
    {
        bool bReturn = grdStudentMarks.Columns[I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX].Visible ? CheckIfGradeIsAssigned(aiRowIndex) : CheckIfStudentsMarksAreEntered(aiRowIndex);
        return bReturn;
    }

    /// <summary>
    /// This method checks if grade is assigned to the student
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private bool CheckIfGradeIsAssigned(int aiRowIndex)
    {
        bool bReturn;

        DropDownList oddlGrades = (DropDownList)grdStudentMarks.Rows[aiRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlGrade");
        DropDownList oddlExamStatus = (DropDownList)grdStudentMarks.Rows[aiRowIndex].FindControl("ddlExamStatus");
        if ((oddlGrades.SelectedValue == Constants.S_ZERO) && (oddlExamStatus.SelectedValue == Constants.C_NO.ToString()))
            bReturn = false;
        else
            bReturn = true;

        return bReturn;
    }

    /// <summary>
    /// This method is used to check whether any od text box have entered marks dor a given grid row of student
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private bool CheckIfStudentsMarksAreEntered(int aiRowIndex)
    {
        if (hidMarksOrGrades.Value == S_CONSTANT_MARKS)
        {
            foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
            {
                string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                if ((((TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text != string.Empty) ||
                (((DropDownList)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent")).SelectedValue != Constants.C_NO.ToString()))
                    return true;
            }
        }
        else
        {
            if ((((DropDownList)grdStudentMarks.Rows[aiRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl("ddlGrade")).SelectedValue != Constants.S_ZERO) ||
            ((DropDownList)grdStudentMarks.Rows[aiRowIndex].Cells[I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX].FindControl("ddlExamStatus")).SelectedValue != Constants.C_NO.ToString())
                return true;
        }

        return false;
    }

    /// <summary>
    /// This method is used to get total marks scored by student.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private string GetTotalMarksForStudent(int aiRowIndex)
    {
        int iTotalMarks = 0;
        decimal dTotalMarksScored = 0;
        if (hidMarksOrGrades.Value == S_CONSTANT_MARKS)
        {
            foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
            {
                string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                if ((((TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text != string.Empty) &&
                (((DropDownList)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent")).SelectedValue == Constants.C_NO.ToString()))
                {
                    decimal dMarks = ((TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text.ToDecimal();
                    int iTestTypeTotalMarks = oDataRow["TestType_Total_Marks"].ToInt();
                    if (hidTestTypeOutOfMarksAvailable.Value != Constants.S_NO)
                        dTotalMarksScored += Math.Round(Convert.ToDouble((dMarks * oDataRow["TestTypeOutOfMarks"].ToInt()) / iTestTypeTotalMarks.ToDecimal()), (hidAllowDecimal.Value.ToBool() || !Settings.RoundMarksAtSubjectLevel) ? Constants.I_ONE : Constants.I_ZERO, MidpointRounding.AwayFromZero).ToDecimal();
                    else
                        dTotalMarksScored += ((TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text.ToDecimal();
                }

                iTotalMarks += oDataRow["TestType_Total_Marks"].ToInt();
            }
        }

        if (hidTestOutOfMarksAvailable.Value != Constants.S_NO && iTotalMarks != Constants.I_ZERO)
        {
            decimal dc = (dTotalMarksScored * hidTestOutOfMarks.Value.ToInt()) / iTotalMarks.ToDecimal();

            if (miSchoolId == Constants.SchoolId.PPSN.ToInt() && hidAllowDecimal.Value.ToBool())
            {
                decimal num = Math.Floor(dc);
                if (dc <= (num + Convert.ToDecimal(0.2)))
                    dc = num;
                else if ((dc > (num + Convert.ToDecimal(0.2))) && (dc <= (num + Convert.ToDecimal(0.5))))
                    dc = (num + Convert.ToDecimal(0.5));
                else if ((dc > (num + Convert.ToDecimal(0.5))) && (dc <= (num + Convert.ToDecimal(0.7))))
                    dc = (num + Convert.ToDecimal(0.5));
                else
                    dc = (Math.Ceiling(dc));
            }

            return Math.Round(dc, (hidAllowDecimal.Value.ToBool() || !Settings.RoundMarksAtSubjectLevel) ? Constants.I_ONE : Constants.I_ZERO, MidpointRounding.AwayFromZero).ToString();

            //return Math.Round((dTotalMarksScored * hidTestOutOfMarks.Value.ToInt()) / iTotalMarks.ToDecimal(), (hidAllowDecimal.Value.ToBool() || !Settings.RoundMarksAtSubjectLevel) ? Constants.I_ONE : Constants.I_ZERO, MidpointRounding.AwayFromZero).ToString();
        }
        return dTotalMarksScored.ToString();
    }

    /// <summary>
    /// This method is used to check whether this student is absent for all test types.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <returns></returns>
    private string IsStudentAbsentforAllTestTypes(int aiRowIndex)
    {
        bool bIsPresent = false;
        string sIsAbsent = Constants.S_NO;
        if (hidMarksOrGrades.Value == S_CONSTANT_MARKS)
        {
            foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
            {
                string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent");
                if ((((TextBox)grdStudentMarks.Rows[aiRowIndex].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text != string.Empty) &&
                (oDropDownList.SelectedValue == Constants.C_NO.ToString()))
                    bIsPresent = true;
                else
                {
                    if (oDataRow["ConsiderExamStatus"].ToString() == Constants.S_YES)
                    {
                        sIsAbsent = oDropDownList.SelectedValue;
                        bIsPresent = false;
                        break;
                    }
                    else bIsPresent = true;
                }
            }
        }
        else if (hidMarksOrGrades.Value == S_GRADE)
        {
            DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[aiRowIndex].Cells[I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX].FindControl("ddlExamStatus");
            if (oDropDownList.SelectedValue == Constants.C_NO.ToString())
                bIsPresent = true;
            else
                sIsAbsent = oDropDownList.SelectedValue;
        }

        if (bIsPresent)
            return Constants.C_NO.ToString();

        return sIsAbsent;
    }

    /// <summary>
    /// This method is used to generate xml for marks entered
    /// </summary>
    /// <returns></returns>
    private string GenerateStudentMarksXml()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();

        // Create a root level element.
        XmlElement root = oDoc.CreateElement("SchoolWiseStudentTestMarksDetails");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarksDetails", string.Empty);

        // Loop through all the grid rows.
        for (int iRowCount = 0; iRowCount <= grdStudentMarks.Rows.Count - 1; iRowCount++)
        {
            // If students marks are entered then only add the node in the XML.
            if (CheckIfStudentsMarksAreEntered(iRowCount))
            {
                XmlNode oXmlNode = null;
                if (hidMarksOrGrades.Value == S_GRADE)
                {
                    // string sIsAbsent = GetValueForIsAbsent(iRowCount, I_IS_ABSENT_FOR_GRADE_COLUMN_INDEX, "chkGrade");
                    DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowCount].FindControl("ddlExamStatus");
                    string sIsAbsent = oDropDownList.SelectedValue;
                    string sAtrrName = "Is_Absent";
                    int iGradeId = 0;
                    if (sIsAbsent.Equals(Constants.C_NO.ToString()))
                        iGradeId = GetAssignedGradeForStudent(iRowCount, "ddlGrade");

                    if (iGradeId != Constants.I_ZERO || modsMarksDetails.Tables[2].Select("ShortName = '" + sIsAbsent + "'").Count() > Constants.I_ZERO)
                    {
                        oXmlNode = GetNodeForMarksAssigned(ref oDoc, iRowCount);
                        XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                        attr.Value = sIsAbsent;

                        oXmlNode.Attributes.Append(attr);

                        sAtrrName = "Assigned_Grade_Id";
                        attr = oDoc.CreateAttribute(sAtrrName);
                        if (sIsAbsent == Constants.C_YES.ToString())
                            attr.Value = "-1";
                        else
                            attr.Value = iGradeId.ToString();
                        oXmlNode.Attributes.Append(attr);

                        AddBlankAttributesExceptGrade(ref oXmlNode, ref oDoc);
                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                }
                else
                {
                    foreach (DataRow oDataRow in modsMarksDetails.Tables[0].Rows)
                    {
                        string sHeaderText = Convert.ToString(oDataRow["TestType_Name"]) + " / " + Convert.ToString(oDataRow["TestType_Total_Marks"]);
                        if ((((TextBox)grdStudentMarks.Rows[iRowCount].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text.Trim() != string.Empty) ||
                        (((DropDownList)grdStudentMarks.Rows[iRowCount].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent")).SelectedValue != Constants.S_NO))
                        {
                            oXmlNode = GetNodeForMarksAssigned(ref oDoc, iRowCount);
                            DropDownList oDropDownList = (DropDownList)grdStudentMarks.Rows[iRowCount].FindControl(sHeaderText.Replace(" ", string.Empty) + "IsAbsent");
                            string sIsAbsent = oDropDownList.SelectedValue;
                            string sAtrrName = "Is_Absent";
                            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = sIsAbsent;

                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "TestType_Id";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = Convert.ToString(oDataRow["TestType_Id"]);

                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "Marks_Scored";
                            attr = oDoc.CreateAttribute(sAtrrName);

                            attr.Value = sIsAbsent == Constants.C_YES.ToString() ? string.Empty : ((TextBox)grdStudentMarks.Rows[iRowCount].FindControl(sHeaderText.Replace(" ", string.Empty) + "txtMarks")).Text;

                            oXmlNode.Attributes.Append(attr);
                            AddBlankAttributesExceptMarks(ref oXmlNode, ref oDoc);
                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                    }
                }

                if (oXmlNode != null)
                {
                    // Add the node to root node.
                    oXmlRootNode.AppendChild(oXmlNode);
                }
            }
        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to add blank attribute 
    /// </summary>
    /// <param name="aoNode"></param>
    /// <param name="aoDoc"></param>
    private void AddBlankAttributesExceptMarks(ref XmlNode aoNode, ref XmlDocument aoDoc)
    {
        const string I_ASSIGNED_GRADE_ID = "Assigned_Grade_Id";
        XmlAttribute attr = aoDoc.CreateAttribute(I_ASSIGNED_GRADE_ID);
        attr.Value = null;
        aoNode.Attributes.Append(attr);
    }

    /// <summary>
    /// This method is used to get assigned grade for student.
    /// </summary>
    /// <param name="aiRowIndex"></param>
    /// <param name="asControlName"></param>
    /// <returns></returns>
    private int GetAssignedGradeForStudent(int aiRowIndex, string asControlName)
    {
        DropDownList oddlGrades = (DropDownList)grdStudentMarks.Rows[aiRowIndex].Cells[I_ASSIGNED_GRADE_COLUMN_INDEX].FindControl(asControlName);
        int iGradeId = Convert.ToInt32(oddlGrades.SelectedValue);
        return iGradeId;
    }

    /// <summary>
    /// This method is used to fe blank attribute excempt grade
    /// </summary>
    /// <param name="aoNode"></param>
    /// <param name="aoDoc"></param>
    private void AddBlankAttributesExceptGrade(ref XmlNode aoNode, ref XmlDocument aoDoc)
    {
        const string I_MARKS_SCORED = "Marks_Scored";
        XmlAttribute attr = aoDoc.CreateAttribute(I_MARKS_SCORED);
        attr.Value = null;
        aoNode.Attributes.Append(attr);
    }

    /// <summary>
    /// This method is used to get xml node for marks entered.
    /// </summary>
    /// <param name="aoDoc"></param>
    /// <param name="aiGridRowIndex"></param>
    /// <returns></returns>
    private XmlNode GetNodeForMarksAssigned(ref XmlDocument aoDoc, int aiGridRowIndex)
    {
        const string S_ELEMENT = "element";
        XmlNode oXmlNode = aoDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarksDetail", string.Empty);

        string sAtrrName = "School_Id";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = miSchoolId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Academic_Year_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = miAcademicYearId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Student_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = grdStudentMarks.DataKeys[aiGridRowIndex].Value.ToString();

        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Subject_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = hidSubjectId.Value;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    /// <summary>
    /// This function is used to set date control read only.
    /// and make save button invisible
    /// </summary>
    /// <param name="abReadOnly"></param>
    /// <returns></returns>
    private void MakeFormFieldsReadOnly(bool abReadOnly)
    {
        btnSave.Visible = !abReadOnly;
        calTestDate.Enabled = !abReadOnly;
        pnlSubmitStatus.Visible = abReadOnly;
    }

    /// <summary>
    /// This function is used to set grid controls read only.
    /// </summary>
    /// <param name="abReadOnly"></param>
    /// <returns></returns>
    private void MakegridReadOnly(bool abReadOnly)
    {
        grdStudentMarks.Enabled = !abReadOnly;
    }

    /// <summary>
    /// This method is used to disable exam status dropdown list.
    /// </summary>
    private void DisableExamStatus()
    {
        foreach (GridViewRow row in grdStudentMarks.Rows)
        {
            DropDownList ddlExamStatus = row.FindControl("ddlExamStatus") as DropDownList;
             ddlExamStatus.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to set design according to the language selected.
    /// </summary>
    private void DesignSettingAccordingLanguage()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidRollnumbers.Value = Resources.LocalizedResources.Rollnumbers;
        hidPleaseSelectGradesForFollowingStudents.Value = Resources.LocalizedResources.PleaseSelectGradesForFollowingStudents;
        hidMarksForFollowingStudentsShouldNotBeBlank.Value = Resources.LocalizedResources.MarksForFollowingStudentsShouldNotBeBlank;
        hidhidPleaseFixFollowingErrors.Value = Resources.LocalizedResources.PleaseFixFollowingError;
        hidExamIsAlreadyPublished.Value = Resources.LocalizedResources.ExamIsAlreadyPublished;
        hidExamDateShouldBeWithinCurrentAcademicYear.Value = Resources.LocalizedResources.ExamDateShouldBeWithinCurrentAcademicYear;
        hidMarksForFollowingStudentsShouldBeLessThan.Value = Resources.LocalizedResources.MarksForFollowingStudentsShouldBeLessThan;
    }

    /// <summary>
    /// This method is sets the sortdirection according to previous derection
    /// </summary>
    private void SetSortDirection()
    {
        if (string.IsNullOrEmpty(hidSortDirection.Value) || hidSortDirection.Value == Constants.S_DESCENDING)
            hidSortDirection.Value = Constants.S_ASCENDING;
        else
            hidSortDirection.Value = Constants.S_DESCENDING;
    }

    /// <summary>
    /// This method is used to add image for sorted column.
    /// </summary>
    private void AddSortImage()
    {

        HtmlTableRow oHtmlTableHeaderRow = lstvwTemplates.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            AddImageToHeader(oHtmlTableHeaderRow, hidSortExpression.Value, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to Add image to the header of column according to sort direction
    /// </summary>
    /// <param name="aoHtmlTableRow"></param>
    /// <param name="asSortExpression"></param>
    /// <param name="asSortDirection"></param>
    private void AddImageToHeader(HtmlTableRow aoHtmlTableRow, string asSortExpression, string asSortDirection)
    {
        if (asSortExpression.Trim().Equals(""))
            return;

        // Create the sorting image based on the sort direction.
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
        sortImage.ID = "sortImage";
        if (asSortDirection == "asc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/up.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else if (asSortDirection == "desc")
        {
            sortImage.ImageUrl = "~/RITeSchool/images/down.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Iterate through the Columns collection to determine the index
        // of the column being sorted.
        foreach (HtmlTableCell oHtmlTableCell in aoHtmlTableRow.Cells)
        {
            asSortExpression = asSortExpression.Replace(" ", "").Replace("asc", "").Replace("desc", "");

            // Iterate through the cells collection to determine the index
            // of the cell being sorted.
            foreach (Control oControl in oHtmlTableCell.Controls)
            {
                LinkButton oLinkButton = oControl as LinkButton;
                if (oLinkButton != null && oLinkButton.CommandArgument == asSortExpression)
                {
                    System.Web.UI.WebControls.Image oImage = (System.Web.UI.WebControls.Image)oHtmlTableCell.FindControl("sortImage");
                    if (oImage == null)
                    {
                        // Add the image to the appropriate header cell.
                        if (sortImage.ImageUrl != "")
                        {
                            oHtmlTableCell.Controls.Add(sortImage);
                            break;
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    ///This method is used to fill the notes with appropriate rows and columns.
    /// </summary>
    private void FillTemplateKeywords()
    {
        List<RemarkTemplateKeyword> olstRemarkTemplateKeywords = RemarksConfigurationBL.GetTemplateNotes();
        ViewState[S_REMARK_TEMPLATE_KEYWORDS] = olstRemarkTemplateKeywords;
    }

    /// <summary>
    /// This function is used to set appropriate values for the keywords.
    /// </summary>
    /// <param name="asNote"></param>
    /// <returns></returns>
    private string UpdateTemplateText(string asNote)
    {
        string sValue = string.Empty;
        string sTemplate = string.Empty;
        int iIndex = Constants.I_ZERO;

        List<RemarkTemplateKeyword> olstRemarkTemplateKeywords = new List<RemarkTemplateKeyword>();
        if (ViewState[S_REMARK_TEMPLATE_KEYWORDS] != null)
            olstRemarkTemplateKeywords = ViewState[S_REMARK_TEMPLATE_KEYWORDS] as List<RemarkTemplateKeyword>;

        olstRemarkTemplateKeywords.ForEach(templateText =>
        {
            sValue = Constants.Salutation.Master.ToInt() == Convert.ToInt32(hidSalutationId.Value) ? templateText.Male : templateText.Female;
            iIndex = asNote.IndexOf(templateText.Keyword);
            if (iIndex != -1 && templateText.Keyword != S_SALUTATION)
            {
                sTemplate = asNote.Substring(Constants.I_ZERO, iIndex).Trim();
                if (!sTemplate.EndsWith(".") && iIndex != Constants.I_ZERO)
                    sValue = sValue.ToLower();
            }

            asNote = asNote.Replace(templateText.Keyword, sValue);
            asNote = asNote.Replace(S_FULLNAME, lblStudName.Text);
            asNote = asNote.Replace(S_FIRSTNAME, hidFname.Value);
            asNote = asNote.Replace(S_MIDDLENAME, hidMname.Value);
            asNote = asNote.Replace(S_LASTNAME, hidLname.Value);
        });
        return asNote.TrimAll();
    }

    /// <summary>
    /// This method is used to fill remark combo
    /// </summary>    
    private void FillRemarksCombo()
    {  
        RemarkTemplateBL oRemarkTemplateBL = new RemarkTemplateBL();
        List<RemarkTypeCategory> lstCategories = oRemarkTemplateBL.GetAllRemarkTypeCategories(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());
        List<RemarksCategory> olstRemarkTemplateConfig = RemarksCategoryBL.GetConfig(miSchoolId, miAcademicYearId);

        olstRemarkTemplateConfig = (from rt in olstRemarkTemplateConfig
                                    join ct in lstCategories
                                    on rt.Id equals ct.CategoryId
                                    select rt).ToList();

        ListSource.FillDropDownList(olstRemarkTemplateConfig, cmbRemarksOnDiv, "Name", "Id", string.Empty);
    }

    /// <summary>
    /// This method is used to fill Grades combo
    /// </summary> 
    private void FillGradesCombo()
    {
        StandardDivisionMasterBL oStandardDivisionMasterBL = new StandardDivisionMasterBL(Convert.ToInt32(hidStandardDivisionId.Value));
        int iStandardId = oStandardDivisionMasterBL.StandardId;
        DataTable odtGradeDetails = MarksGradesConfigurationBL.GetAllGradesForStandard(miSchoolId, miAcademicYearId, iStandardId, hidSubjectId.Value.ToInt());
        cmbGradesOnDiv.Bind(odtGradeDetails, "Marks_Grades_Configuration_Detail_ID", "Grade_Name", Constants.S_ALL);       
    }

    /// <summary>
    /// This method is sued to display remark templates.
    /// </summary>
    private void DisplayRemarkTemplates()
    {
        hidMarksGradesConfigurationDetailsId.Value = cmbGradesOnDiv.SelectedValue;
        RemarkTemplateBL oTemplateConfigurationBL = new RemarkTemplateBL();
        List<RemarkTemplateConfig> lstRemarks = oTemplateConfigurationBL.GetAll(miSchoolId, Convert.ToInt32(cmbRemarksOnDiv.SelectedValue), hidSortExpression.Value, hidSortDirection.Value, string.Empty, miAcademicYearId,hidMarksGradesConfigurationDetailsId.Value.ToInt(), hidStandardId.Value.ToInt());

        RemarkTemplateBL oRemarkTemplateBL = new RemarkTemplateBL();
        List<RemarkTypeCategory> lstCategories = oRemarkTemplateBL.GetAllRemarkTypeCategories(miSchoolId, miAcademicYearId, hidTestId.Value.ToInt(), hidSubjectId.Value.ToInt());

        lstRemarks = (from rmk in lstRemarks
                      join ct in lstCategories
                      on rmk.CategoryId equals ct.CategoryId
                      select rmk).ToList();

        lstvwTemplates.DataSource = lstRemarks;
        lstvwTemplates.DataBind();

        if (lstvwTemplates.Items.Count > 0)
        {
            btnPopupSave.Enabled = true;
            AddSortImage();
        }
        else
            btnPopupSave.Enabled = false;
    }

    #endregion    
}

// Create a template class to represent a dynamic textbox template column.
public class GridViewTextBoxTemplate : ITemplate
{
    private readonly DataControlRowType moTemplateType;
    private readonly bool mbShowTotalAsPerOutOfMarks;
    private readonly string msColumnName;
    private readonly string msCntrlName;
    private readonly string mclassName;
    private readonly bool mbAllowDecimal;
    private readonly bool mbIsMultiLine;
    private int miRowIndex;

    public GridViewTextBoxTemplate(DataControlRowType aoType, string asColname, string asClassName, bool abShowTotalAsPerOutOfMarks, string asControlName, bool abAllowDecimal, int aiRowIndex, bool abIsMultiLine = false)
    {
        moTemplateType = aoType;
        msColumnName = asColname;
        msCntrlName = asControlName;
        mclassName = asClassName;
        mbShowTotalAsPerOutOfMarks = abShowTotalAsPerOutOfMarks;
        mbAllowDecimal = abAllowDecimal;
        mbIsMultiLine = abIsMultiLine;
        miRowIndex = aiRowIndex;
    }

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

                if (miRowIndex != Constants.I_DEFAULT_MAX_VALUE)
                {
                    TextBox txt = new TextBox { MaxLength = mbAllowDecimal ? 5 : 3 };
                    txt.ID = msCntrlName;
                    txt.CssClass = mclassName;
                    if (mbIsMultiLine)
                        txt.TextMode = TextBoxMode.MultiLine;

                    txt.Attributes.Add("onblur", "extractNumber(this, " + (mbAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                    txt.Attributes.Add("onkeyup", "OnGridKeyUp(this, " + (mbAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false,event);");
                    txt.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, " + mbAllowDecimal.ToString().ToLower() + ", false);");
                    txt.Attributes.Add("onpaste", "event.returnValue=false");
                    txt.Attributes.Add("ondrop", "event.returnValue=false");
                    txt.Attributes.Add("onchange", "SetData(this,'" + miRowIndex + "')");

                    txt.Style.Add("width", "50px");
                    txt.Style.Add("margin-left", "5px");
                    aoContainer.Controls.Add(txt);
                }

                break;
            case DataControlRowType.DataRow:
                // Create the controls to put in a data row
                // section and set their properties.
                Label oLabel = new Label();
                Label oLblConvertedMarks = new Label();

                // If decimals are allowed then set max length 5 else 3.
                TextBox oTextBox = new TextBox { MaxLength = mbAllowDecimal ? 5 : 3 };

                // To support data binding, register the event-handling methods
                // to perform the data binding. Each control needs its own event
                // handler.
                oTextBox.DataBinding += Marks_DataBinding;
                oTextBox.ID = msCntrlName;
                oTextBox.CssClass = mclassName + " " + miRowIndex;

                if (mbIsMultiLine)
                    oTextBox.TextMode = TextBoxMode.MultiLine;

                oLabel.ID = msCntrlName + "lbl";
                oLabel.CssClass = "LblGrade";
                oLabel.Style.Add("width", "75px");
                
                oLblConvertedMarks.CssClass = "LblGrade";
                oLblConvertedMarks.ID = msCntrlName + "lblConvertedMarks";

                oLblConvertedMarks.Style.Add("width", mbShowTotalAsPerOutOfMarks ? mbAllowDecimal ? "50px" : "40px" : "0px");
                
                
                if (msColumnName.Contains("Total"))
                {
                    oTextBox.ReadOnly = true;
                    oTextBox.TabIndex = -1;
                }

                oTextBox.Attributes.Add("onblur", "extractNumber(this, " + (mbAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                oTextBox.Attributes.Add("onkeyup", "OnGridKeyUp(this, " + (mbAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false,event);");
                oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers (this, event, " + mbAllowDecimal.ToString().ToLower() + ", false);");
                oTextBox.Attributes.Add("onpaste", "event.returnValue=false");
                oTextBox.Attributes.Add("ondrop", "event.returnValue=false");

                // Add the controls to the Controls collection
                // of the container.
                aoContainer.Controls.Add(oTextBox);
                aoContainer.Controls.Add(oLblConvertedMarks);
                aoContainer.Controls.Add(oLabel);
                break;

            // Insert cases to create the content for the other row types, if desired.
            default:
                // Insert code to handle unexpected values.
                break;
        }
    }

    private void Marks_DataBinding(object sender, EventArgs e)
    {
        // Get the Label control to bind the value. The Label control
        // is contained in the object that raised the DataBinding 
        // event (the sender parameter).
        TextBox oTextBox = (TextBox)sender;

        // Get the GridViewRow object that contains the Label control.
        GridViewRow oRow = (GridViewRow)oTextBox.NamingContainer;
        string sOnchangeAttrbute = string.Format("SetRowTotalMarks('{0}');", oRow.ClientID);
        if (msColumnName.LastIndexOf("/") > Constants.I_ZERO)
        {
            string iMaxMarks = msColumnName.Substring(msColumnName.LastIndexOf("/") + 1);
            sOnchangeAttrbute = string.Format("{0}SetGrade('{1}',{2});", sOnchangeAttrbute, oTextBox.ClientID, iMaxMarks);
        }

        oTextBox.Attributes.Add("onchange", sOnchangeAttrbute);

        // Get the field value from the GridViewRow object and 
        // assign it to the Text property of the Label control.
        oTextBox.Text = DataBinder.Eval(oRow.DataItem, "Marks_Scored").ToString();
    }
}

// Create a template class to represent a dynamic checkbox template column.
public class GridViewCheckBoxTemplate : ITemplate
{
    private readonly DataControlRowType moTemplateType;
    private readonly string msColumnName;

    public GridViewCheckBoxTemplate(DataControlRowType aoType, string asColname)
    {
        moTemplateType = aoType;
        msColumnName = asColname;
    }

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
                CheckBox oCheckBox = new CheckBox();

                // To support data binding, register the event-handling methods
                // to perform the data binding. Each control needs its own event
                // handler.
                oCheckBox.DataBinding += IsAbsent_DataBinding;
                oCheckBox.ID = msColumnName.Replace(" ", string.Empty) + "IsAbsent";
                oCheckBox.TabIndex = -1;

                // Add the controls to the Controls collection
                // of the container.
                aoContainer.Controls.Add(oCheckBox);
                break;

            // Insert cases to create the content for the other 
            // row types, if desired.
            default:
                // Insert code to handle unexpected values.
                break;
        }
    }

    private void IsAbsent_DataBinding(object sender, EventArgs e)
    {
        // Get the Label control to bind the value. The Label control
        // is contained in the object that raised the DataBinding 
        // event (the sender parameter).
        CheckBox oCheckBox = (CheckBox)sender;

        // Get the GridViewRow object that contains the Label control. 
        GridViewRow oRow = (GridViewRow)oCheckBox.NamingContainer;
        oCheckBox.Attributes.Add("onclick", "DisableMarksControl(this,'" + oCheckBox.ClientID.Replace("IsAbsent", "txtMarks") + "','" + oCheckBox.ClientID.Replace(msColumnName.Replace(" ", string.Empty) + "IsAbsent", "txtTotalMarks") + "','" + oRow.ClientID + "')");
        
        // Get the field value from the GridViewRow object and 
        // assign it to the Text property of the Label control.
        oCheckBox.Checked = Convert.ToBoolean(DataBinder.Eval(oRow.DataItem, "is_absent").ToString());
    }
}

public class GridViewDropDownListTemplate : ITemplate
{
    private readonly DataControlRowType moTemplateType;
    private readonly string msColumnName;
    private readonly DataTable mdtExamStatus;
    private readonly bool mbEnabled;
    private readonly bool mbIsCocurricullar;
    private readonly bool mbAllowLateJoineeOption;

    public GridViewDropDownListTemplate(DataControlRowType aoType, string asColname, DataTable aoDtExamStatus, bool abEnabled, bool abIsCocurricullar, bool abAllowLateJoineeOption)
    {
        moTemplateType = aoType;
        msColumnName = asColname;
        mdtExamStatus = aoDtExamStatus;
        mbEnabled = abEnabled;
        mbIsCocurricullar = abIsCocurricullar;
        mbAllowLateJoineeOption = abAllowLateJoineeOption;
    }

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
                DropDownList oDropDownList = new DropDownList
                                                 {
                                                     ID = msColumnName.Replace(" ", string.Empty) + "IsAbsent",
                                                     TabIndex = -1
                                                 };

                // To support data binding, register the event-handling methods
                // to perform the data binding. Each control needs its own event
                // handler.

                oDropDownList.Items.Clear();
                DataTable oDTExamStatus = mdtExamStatus.Copy();

                if (!mbAllowLateJoineeOption)
                {
                    DataRow[] oDrExamStatus = oDTExamStatus.Select("ShortName <> 'J'", "ExamStatusId ASC");
                    oDTExamStatus = oDrExamStatus.CopyToDataTable();
                }

                oDropDownList.DataSource = oDTExamStatus;
                oDropDownList.DataBind();
                oDropDownList.DataTextField = "DisplayName";
                oDropDownList.DataValueField = "ShortName";
                oDropDownList.Enabled = mbEnabled;
                oDropDownList.Enabled = mbIsCocurricullar;

                string str = oDropDownList.ClientID.Replace(msColumnName.Replace(" ", string.Empty) + "IsAbsent", "txtTotalMarks");
                oDropDownList.Attributes.Add("onchange", "DisableMarksControl(this,'" + oDropDownList.ClientID.Replace("IsAbsent", "txtMarks") + "','" + str + "')");
                
                // Add the controls to the Controls collection
                // of the container.
                aoContainer.Controls.Add(oDropDownList);

                break;

            // Insert cases to create the content for the other 
            // row types, if desired.
            default:
                // Insert code to handle unexpected values.
                break;
        }
    }

   
}
