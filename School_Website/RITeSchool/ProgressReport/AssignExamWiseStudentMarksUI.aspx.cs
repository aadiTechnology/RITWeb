/* File Name - AssignExamWiseStudentMarksUI.aspx.cs
 * Created Date - 23-Oct-2011
 * Created by - Vipul
 * Class Description - This class is used for student wise marks assignment.

 * Modified Date - 22-Dec-2011
 * Modified by - Vipul
 * Modification Description - This class will save student's marks as per Out Of Marks configuration.

 * Modified Date - 20-Jan-2012
 * Modified by - Vipul
 * Modification Description - To add late joninig related functionallity.
  
 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using ProgressReportEntities;
using Utility;

public partial class AssignExamWiseStudentMarksUI : StudentProgress
{
    #region "Constants"
    private const string S_SAPARATOR = "_";
    private const string S_DB_COL_TEST_DATE = "Test_Date";
    private const int S_VAL_TESTTYPE_ID = 6;
    private const int S_VAL_TESTWISE_SUBJECT_MARKS_ID = 3;
    private const int S_VAL_SUBJECT_ID = 4;
    private const string S_ELEMENT = "element";
    #endregion "Constants"

    #region "Data Members"
    private int miStudentId;
    private int miClassTacherId;
    private string msTestNames = string.Empty;
    #endregion "Data Members"


    #region "Events"

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {   
			InitializeMemberVariables();         
            base.OnInit(e);
            ReadQueryString();
            ShowProgresSheet();
            SetJavaScriptAttributes();
        }
        catch (MarksNotAvailableForResult oEx)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = oEx.Message;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (NoResultFound)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod(), " StandardDivisionId = " + hidStandardDivisionId.Value + " StudsentId = " + miStudentId);
        }
    }

    /// <summary>
    /// This method is used to set panel in base class.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            miAcademicYearId = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToInt();
            miSchoolId = Session[Constants.S_SESSION_SCHOOL_ID].ToInt();
            miUserId = Session[Constants.S_SESSION_USER_ID].ToInt();
            hidRoundMarksAtSubjectLevel.Value = Settings.RoundMarksAtSubjectLevel ? Constants.S_YES : Constants.S_NO;
            base.SetpanelMember(GridViewScrollContainer);
             // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event used for publishing the selected tests.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            ManageStudentMarks("Publish");
            if (hidConfirmSms.Value == Constants.I_ONE.ToString())
                foreach (string otest in msTestNames.Split(','))
                    SendMessageToStudent(otest);           
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx?" + Request.QueryString);
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save marks of the selected exams.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ManageStudentMarks("Save");            
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx?" + Request.QueryString);
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to view progress report of the selected exams.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId);
            string sTestIds = GenerateTestIds(oHtmlTable);
            int iIndex = (HidBackUrl.Value.IndexOf("&TestId=") == -1) ? HidBackUrl.Value.Length : HidBackUrl.Value.IndexOf("&TestId=");
            string sUrl = "~/RITeSchool/ProgressReport/StudentWiseProgressSheet.aspx?" + CommonUtility.EncryptQuerystring(HidBackUrl.Value.Substring(0, iIndex) + "&TestId=" + sTestIds);
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(sUrl);
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete marks of the selected exams.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            ManageStudentMarks("Delete");
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage("~/RITeSchool/ProgressReport/AssignExamWiseStudentMarksUI.aspx?" + Request.QueryString);
        }
        catch (Exception oEx)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Methods"

    /// <summary>
    /// This method is used to set marks to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamMarks(int aiTestId, DictionaryEntry oHtSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHtSubjectEntry.Value;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Cast<StudentWiseProgressReportMarkAssignment>()
                                                            .Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                      ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                      ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (oMarkAssignmentDetails.Count > 0 && oMarkAssignmentDetails[0].MarksScored >= Constants.I_ZERO)
        {
            // If subject has grade then dont append total marks(i.e 12/100)
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHtSubjectEntry.Key.ToInt()];
            CreateSubjectExamMarkCell(oHtmlTableCell, oHtSubjectEntry, aiRowIndex, oMarkAssignmentDetails[0]);
        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHtSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set grade to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGrade(int aiTestId, DictionaryEntry oHtSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHtSubjectEntry.Value;

        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Cast<StudentWiseProgressReportMarkAssignment>()
                                                            .Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                      ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (!mbStudentwiseProgressReport)
            oMarkAssignmentDetails = oMarkAssignmentDetails.Where(ma => ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan).ToList();
        HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHtSubjectEntry.Key.ToInt()];
        if (oMarkAssignmentDetails.Count > 0)
        {
            if (!oMarkAssignmentDetails[0].Grade.IsNullOrEmpty())
            {
                if (aiTestId != -1)
                {
                    if (oSubjectDetailsForProgressReport.SubjectCellType != Constants.ReportCellType.GradeExamTypeTotal)
                    {
                        SetGradeTotal(ref oHtSubjectEntry, aiRowIndex, ref oSubjectDetailsForProgressReport, oMarkAssignmentDetails, oHtmlTableCell);
                    }
                    else
                    {
                        Label olblGrade = new Label
                                              {
                                                  EnableViewState = true,
                                                  Text = oMarkAssignmentDetails[0].Grade.ToString(),
                                                  ID = CreateIDForControl(
                                                       "lbl", 
                                                       aiRowIndex, 
                                                       oHtSubjectEntry.Key.ToInt(), 
                                                       aiTestId, 
                                                       oSubjectDetailsForProgressReport.SubjectCellColSpan,
                                                       oSubjectDetailsForProgressReport.SubjectId.ToString(), 
                                                       oSubjectDetailsForProgressReport.SubjectCellType)
                                              };
                        oHtmlTableCell.Controls.Add(olblGrade);
                    }
                }
                else
                    SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHtSubjectEntry.Key.ToInt(), null);
            }
            else
            {
                SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHtSubjectEntry.Key.ToInt(), null);
            }
        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHtSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set exam type total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamTypeTotal(int aiTestId, DictionaryEntry oHtSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHtSubjectEntry.Value;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                                            ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId)
                                                                                  .Cast<StudentWiseProgressReportMarkAssignment>()
                                                                                  .ToList();
        if (oMarkAssignmentDetails.Count > 0 && !oMarkAssignmentDetails[0].Marks.IsNullOrEmpty())
        {
            // If subject has grade then dont append total marks(i.e 12/100)                     
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHtSubjectEntry.Key.ToInt()];
            Label olblMarks = new Label();
            Label olblMarksTotal = new Label();
            olblMarks.EnableViewState = true;
            olblMarksTotal.EnableViewState = true;
            olblMarks.Text = oMarkAssignmentDetails[0].TotalMarksScored.ToString("0.#");
            olblMarks.ID = CreateIDForControl(
                           "lblTotal",
                           aiRowIndex,
                           oMarkAssignmentDetails[0].SchoolWiseStudentTestId.ToInt(),
                           oMarkAssignmentDetails[0].TestWiseSubjectId.ToInt(),
                           oSubjectDetailsForProgressReport.SubjectId,
                           oHtSubjectEntry.Key.ToInt() + S_SAPARATOR + oSubjectDetailsForProgressReport.SubjectCellColSpan +
                           S_SAPARATOR + oMarkAssignmentDetails[0].TestOutOfMarks + S_SAPARATOR + oMarkAssignmentDetails[0].TestTypeOutOfMarks + S_SAPARATOR + oMarkAssignmentDetails[0].TestTypeTotalMarks +
                           S_SAPARATOR + oMarkAssignmentDetails[0].ConsiderExamStatus + S_SAPARATOR + oMarkAssignmentDetails[0].TotalConsideration,
                           oSubjectDetailsForProgressReport.SubjectCellType);


            olblMarksTotal.Text = " / " + oMarkAssignmentDetails[0].SubjectTotalMarks;
            oHtmlTableCell.Controls.Add(olblMarks);
            oHtmlTableCell.Controls.Add(olblMarksTotal);

            if (!IsTotalConsiderForProgressReport())
                oHtmlTableCell.Attributes.Add("style", "display:none");

        }
        else
            SetNotApplicableCellValuesForExamTypeTotal(tblProgress.Rows[aiRowIndex], oHtSubjectEntry.Key.ToInt(), string.Empty);
    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGroupTotal(int aiTestId, DictionaryEntry oHtSubjectEntry, int aiRowIndex)
    {
        /*We are overriding this function still we need to call base class's methods beacause in base class version of this mathod 
        there is calls for another methods which are overrided here causes caling base class method but still method of derrived class 
        get called in between calls of overrided methods.*/
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHtSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).OrderByDescending(subject => subject.Id).ToList();

        if (oParentSubjectDetails.Count > 0 && oParentSubjectDetails[0].ParentSubjectId != Constants.I_ZERO)
        {
            // Take a group total of a subject.
            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectTestGroupTotalDetails.Cast<StudentWiseProgressReportSubjectTestGroupTotal>()
                                                                                                 .Where(sgt => sgt.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId && 
                                                                                                               sgt.SchoolWiseTestId == aiTestId).ToList();
            if (oParentSubjectTotalDetails.Count > 0)
                base.FillSubjectExamGroupTotal(aiTestId, oHtSubjectEntry, aiRowIndex);
            else
                base.FillSubjectExamMarks(aiTestId, oHtSubjectEntry, aiRowIndex);
        }
        else
            base.FillSubjectExamMarks(aiTestId, oHtSubjectEntry, aiRowIndex);
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void ReadQueryString()
    {
        HidBackUrl.Value = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString()));
        if (QueryString.Count > 0)
        {
            if (QueryString["ClassTeacherId"] != null)
                miClassTacherId = QueryString["ClassTeacherId"].ToInt();
            if (QueryString["StudentId"] != null)
                miStudentId = QueryString["StudentId"].ToInt();
            if (QueryString["StandardDivisionId"] != null)
                hidStandardDivisionId.Value = Convert.ToString(QueryString["StandardDivisionId"]);
            if (QueryString["ClassTeacher"] != null)
                hidClassTeacher.Value = Convert.ToString(QueryString["ClassTeacher"]);
            if (QueryString["ProgresSheetID"] != null)
                hidRemoveProgressReport.Value = QueryString["ProgresSheetID"].ToInt() != 0 ? Constants.S_YES : Constants.S_NO;

            btnBack.PostBackUrl = btnBack.PostBackUrl + "?" + Request.QueryString;
        }
    }

    /// <summary>
    /// This method is used to show students progress sheet.
    /// </summary>
    private void ShowProgresSheet()
    {
        mbStudentwiseProgressReport = true;
        SetpanelMember(GridViewScrollContainer);
        int iTeacherId = 0;
        if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();
        else if (miClassTacherId != 0)
            iTeacherId = miClassTacherId;
        base.ShowProgressSheet(iTeacherId, miStudentId);
        HidGradeRange.Value = msGradeDetails;

        hidUserID.Value = miUserId.ToString();
        hidIsFailCriteriaNotApplicable.Value = msIsFailCriteriaNotApplicable;
        hidShowTotalAsPerOutOfMarks.Value = Settings.ShowTotalAsPerOutOfMarks ? Constants.S_YES : Constants.S_NO;
        hidExamStatus.Value = msExamStatusDetails;
    }

    /// <summary>
    /// This method is used to set grade total.
    /// </summary>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="oSubjectDetailsForProgressReport"></param>
    /// <param name="oDataRow"></param>
    /// <param name="oHtmlTableCell"></param>
    private void SetGradeTotal(ref DictionaryEntry oHtSubjectEntry, int aiRowIndex, ref SubjectDetailsForProgressReport oSubjectDetailsForProgressReport, List<StudentWiseProgressReportMarkAssignment> aoMarkAssignmentDetails, HtmlTableCell oHtmlTableCell)
    {
        HiddenField ohidTestDate = new HiddenField
        {
            ID = "hidTestDate_" + aiRowIndex + "_" + oHtSubjectEntry.Key.ToInt(),
            Value = aoMarkAssignmentDetails[0].TestDate.ToString()
        };
        oHtmlTableCell.Controls.Add(ohidTestDate);

        DropDownList oddlExamStatus = new DropDownList
        {
            DataTextField = S_DB_COL_DISPLAY_NAME,
            DataValueField = S_DB_COL_SHORT_NAME,
            DataSource = moStudentProgressReport.ExamStatusDetails
        };
        oddlExamStatus.DataBind();
        oddlExamStatus.Items.Insert(0, new ListItem(Constants.S_SELECT, "N"));

        oddlExamStatus.ID = CreateIDForControl(
                            "ddlExamStatus",
                            aiRowIndex,
                            aoMarkAssignmentDetails[0].SchoolWiseStudentTestId.ToInt(),
                            aoMarkAssignmentDetails[0].TestWiseSubjectId.ToInt(),
                            oSubjectDetailsForProgressReport.SubjectId,
                            oHtSubjectEntry.Key.ToInt() + S_SAPARATOR + oSubjectDetailsForProgressReport.SubjectCellColSpan +
                            S_SAPARATOR + "IEX" + (aoMarkAssignmentDetails[0].IsExamStatusApplicable ? Constants.I_ONE: Constants.I_ZERO) + S_SAPARATOR + aoMarkAssignmentDetails[0].IsAbsent +
                            S_SAPARATOR + aoMarkAssignmentDetails[0].ConsiderExamStatus + S_SAPARATOR + aoMarkAssignmentDetails[0].TotalConsideration,
                            oSubjectDetailsForProgressReport.SubjectCellType);
        oddlExamStatus.SelectedValue = aoMarkAssignmentDetails[0].IsAbsent;
        oddlExamStatus.Width = Unit.Pixel(100);
        oHtmlTableCell.Controls.Add(oddlExamStatus);

        DropDownList oDropDownList = new DropDownList();
        FillGradesCombobox(oDropDownList, aoMarkAssignmentDetails[0].IsCoCurricularActivity, aoMarkAssignmentDetails, oSubjectDetailsForProgressReport.SubjectId);
        oDropDownList.ID = CreateIDForControl(
                           "ddl",
                           aiRowIndex,
                           aoMarkAssignmentDetails[0].SchoolWiseStudentTestId.ToInt(),
                           aoMarkAssignmentDetails[0].TestWiseSubjectId.ToInt(),
                           oSubjectDetailsForProgressReport.SubjectId,
                           oHtSubjectEntry.Key.ToInt() + S_SAPARATOR + oSubjectDetailsForProgressReport.SubjectCellColSpan +
                           S_SAPARATOR + aoMarkAssignmentDetails[0].ConsiderExamStatus + S_SAPARATOR + aoMarkAssignmentDetails[0].TotalConsideration,
                           oSubjectDetailsForProgressReport.SubjectCellType);
        ListItem oListItem = oDropDownList.Items.FindByText(aoMarkAssignmentDetails[0].Grade);
        oDropDownList.SelectedValue = oListItem.Value;

        if (aoMarkAssignmentDetails[0].ExamPublishStatus != Constants.S_NO || aoMarkAssignmentDetails[0].StudentWiseTestPublishStatus != Constants.S_NO)
            oddlExamStatus.Enabled = oDropDownList.Enabled = false;
        else oDropDownList.Enabled = aoMarkAssignmentDetails[0].IsAbsent == Constants.S_NO;

        if (aoMarkAssignmentDetails[0].IsAbsent == "J")
            oddlExamStatus.Enabled = oDropDownList.Enabled = false;
        else
            oddlExamStatus.Items.RemoveAt(oddlExamStatus.Items.Count - 1);

        oddlExamStatus.Enabled = oddlExamStatus.Enabled && aoMarkAssignmentDetails[0].IsExamStatusApplicable;

        oHtmlTableCell.Controls.Add(oDropDownList);
        oddlExamStatus.Attributes.Add("onchange", "SetControlAsPerExamStatus(this,'" + oDropDownList.ClientID + "','" + tblProgress.ClientID + "','" + aiRowIndex + "')");
    }


    /// <summary>
    /// Fills combobox for each row if the grades are to be assigned to students.
    /// </summary>
    protected void FillGradesCombobox(DropDownList ddlGrade, bool abIsCoCuricularSubject, List<StudentWiseProgressReportMarkAssignment> aoMarkAssignmentDetails, int iSubjectId)
    {
        if (miSchoolId == Constants.SchoolId.SNS.ToInt())
        {
            ddlGrade.DataTextField = "GradeName";
            ddlGrade.DataValueField = "GradeId";

            if (aoMarkAssignmentDetails.Any(sub => sub.IsActivitySubject))
                ddlGrade.DataSource = moStudentProgressReport.GradeDetails.Cast<StudentWiseProgressReportGrade>().ToList().Where(grade => grade.IsForCoCurricularSubjects == abIsCoCuricularSubject && grade.IsActivitySubject == true).ToList();             
            else            
                ddlGrade.DataSource = moStudentProgressReport.GradeDetails.Cast<StudentWiseProgressReportGrade>().ToList().Where(grade => grade.IsForCoCurricularSubjects == abIsCoCuricularSubject && grade.IsActivitySubject == false).ToList();

            ddlGrade.DataBind();
            ddlGrade.Dispose();
        }
        else
        {
            ddlGrade.DataTextField = "GradeName";
            ddlGrade.DataValueField = "GradeId";
            ddlGrade.DataSource = moStudentProgressReport.GradeDetails.Cast<StudentWiseProgressReportGrade>().ToList().Where(grade => grade.IsForCoCurricularSubjects == abIsCoCuricularSubject).ToList();
            ddlGrade.DataBind();
            ddlGrade.Dispose();
        }
    }

    
    /// <summary>
    /// This method is used to create Id for a textbox added to cell
    /// </summary>
    /// <param name="asPrefix"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aiCellIndex"></param>
    /// <param name="aiTestId"></param>
    /// <param name="aiTestTypeId"></param>
    /// <param name="aiSubjectId"></param>
    /// <param name="aiEnumColType"></param>
    /// <returns></returns>
    private string CreateIDForControl(string asPrefix, int aiRowIndex, int aiCellIndex, int aiTestId, int aiTestTypeId, string aiSubjectId, Constants.ReportCellType aiEnumColType)
    {
        return asPrefix + S_SAPARATOR + aiRowIndex + S_SAPARATOR + aiCellIndex
            + S_SAPARATOR + aiTestId
            + S_SAPARATOR + aiTestTypeId + S_SAPARATOR + aiSubjectId
            + S_SAPARATOR + aiEnumColType.ToInt();
    }

    /// <summary>
    /// This method is used to add controls and set values to cell.
    /// </summary>
    /// <param name="aoHtmlTableCell"></param>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aoMarkAssignment"></param>
    private void CreateSubjectExamMarkCell(HtmlTableCell aoHtmlTableCell, DictionaryEntry oHtSubjectEntry, int aiRowIndex, StudentWiseProgressReportMarkAssignment aoMarkAssignment)
    {
        if (aoMarkAssignment.TestTypeTotalMarks >= Constants.I_ZERO)
        {
            SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHtSubjectEntry.Value;

            Label olblMarksTotal = new Label
                                       {
                                           Text = " / " + aoMarkAssignment.TestTypeTotalMarks
                                       };

            if (aoMarkAssignment.SchoolWiseTestId != -1)
            {
                HiddenField ohidTestDate = new HiddenField
                                               {
                                                   ID =
                                                       "hidTestDate_" + aiRowIndex + "_" +
                                                       oHtSubjectEntry.Key.ToInt(),
                                                   Value = aoMarkAssignment.TestDate.ToString()
                                               };
                aoHtmlTableCell.Controls.Add(ohidTestDate);

                DropDownList oddlExamStatus = new DropDownList
                                                  {
                                                      DataTextField = S_DB_COL_DISPLAY_NAME,
                                                      DataValueField = S_DB_COL_SHORT_NAME,
                                                      DataSource = moStudentProgressReport.ExamStatusDetails
                                                  };
                oddlExamStatus.DataBind();
				oddlExamStatus.Items.Insert(0, new ListItem(Constants.S_SELECT, "N"));
                oddlExamStatus.ID = CreateIDForControl(
                                    "ddlExamStatus",
                                    aiRowIndex,
                                    aoMarkAssignment.SchoolWiseStudentTestId.ToInt(),
                                    aoMarkAssignment.TestWiseSubjectId.ToInt(),
                                    oSubjectDetailsForProgressReport.SubjectId,
                                    oHtSubjectEntry.Key.ToInt() + S_SAPARATOR + oSubjectDetailsForProgressReport.SubjectCellColSpan +
                                    S_SAPARATOR + "IEX" + (aoMarkAssignment.IsExamStatusApplicable ? Constants.I_ONE : Constants.I_ZERO) + S_SAPARATOR + aoMarkAssignment.IsAbsent +
                                    S_SAPARATOR + aoMarkAssignment.ConsiderExamStatus + S_SAPARATOR + aoMarkAssignment.TotalConsideration,
                                    oSubjectDetailsForProgressReport.SubjectCellType);
                oddlExamStatus.SelectedValue = aoMarkAssignment.IsAbsent;
                oddlExamStatus.Width = Unit.Pixel(100);

                TextBox oTextBox = SetScoredTotalMarks(ref oHtSubjectEntry, aiRowIndex, aoMarkAssignment, ref oSubjectDetailsForProgressReport);
                bool bAllowDecimal = aoMarkAssignment.AllowDecimal;
                oTextBox.Attributes.Add("onkeyup", "javascript:extractNumber(this," +(bAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                oTextBox.Attributes.Add("onblur", "javascript:Validate(this," + aoMarkAssignment.TestTypeTotalMarks + ",'" + tblProgress.ClientID + "','" + aiRowIndex + "');extractNumber(this," + (bAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, " + aoMarkAssignment.AllowDecimal.ToString().ToLower() + ", false);");
                aoHtmlTableCell.Align = HorizontalAlign.Left.ToString();
                aoHtmlTableCell.Attributes.Add("class", aoHtmlTableCell.Attributes["class"] + " ClspaddingL");
                oddlExamStatus.Enabled = !(aoMarkAssignment.ExamPublishStatus != Constants.S_NO || aoMarkAssignment.StudentWiseTestPublishStatus != Constants.S_NO);
                oTextBox.Enabled = !(aoMarkAssignment.ExamPublishStatus != Constants.S_NO || aoMarkAssignment.StudentWiseTestPublishStatus != Constants.S_NO) && aoMarkAssignment.IsAbsent == Constants.S_NO;

                oddlExamStatus.Enabled = oddlExamStatus.Enabled && aoMarkAssignment.IsExamStatusApplicable;
                //if (aoMarkAssignment.IsAbsent == "J")
                //    oddlExamStatus.Enabled = oTextBox.Enabled = false;
                //else
                //    oddlExamStatus.Items.RemoveAt(oddlExamStatus.Items.Count - 1);
				aoHtmlTableCell.Controls.Add(oddlExamStatus);
                aoHtmlTableCell.Controls.Add(oTextBox);
                oddlExamStatus.Attributes.Add("onchange", "SetControlAsPerExamStatus(this,'" + oTextBox.ClientID + "','" + tblProgress.ClientID + "','" + aiRowIndex + "')");
            }
            else
            {
                Label olblMarks = new Label { Text = aoMarkAssignment.MarksScored.ToString() };
                aoHtmlTableCell.Controls.Add(olblMarks);
            }

            aoHtmlTableCell.Controls.Add(olblMarksTotal);
        }
    }

    /// <summary>
    /// This method is used to set scored total marks .
    /// </summary>
    /// <param name="oHtSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aoMarkAssignment"></param>
    /// <param name="oSubjectDetailsForProgressReport"></param>
    /// <returns></returns>
    private TextBox SetScoredTotalMarks(ref DictionaryEntry oHtSubjectEntry, int aiRowIndex, StudentWiseProgressReportMarkAssignment aoMarkAssignment, ref SubjectDetailsForProgressReport oSubjectDetailsForProgressReport)
    {
        TextBox oTextBox = new TextBox
        {
            Text = aoMarkAssignment.MarksScored.ToString("0.#"),
            ID = CreateIDForControl(
                 "txtMarks",
                 aiRowIndex,
                 aoMarkAssignment.SchoolWiseStudentTestId,
                 aoMarkAssignment.TestWiseSubjectId,
                 oSubjectDetailsForProgressReport.SubjectId,
                 oHtSubjectEntry.Key.ToInt() + S_SAPARATOR + oSubjectDetailsForProgressReport.SubjectCellColSpan +
                 S_SAPARATOR + aoMarkAssignment.TestOutOfMarks + S_SAPARATOR + aoMarkAssignment.TestTypeOutOfMarks + S_SAPARATOR + aoMarkAssignment.TestTypeTotalMarks +
                 S_SAPARATOR + aoMarkAssignment.ConsiderExamStatus + S_SAPARATOR + aoMarkAssignment.TotalConsideration + S_SAPARATOR +
                 ((aoMarkAssignment.AllowDecimal || !Settings.RoundMarksAtSubjectLevel) ? bool.TrueString.ToLower() : bool.FalseString.ToLower()),
                 oSubjectDetailsForProgressReport.SubjectCellType),
            MaxLength = aoMarkAssignment.AllowDecimal ? Constants.I_FIVE : Constants.I_THREE,
            Width = Unit.Pixel(40),
            CssClass = "ExSmlTxtBoxP",
        };
        
        return oTextBox;
    }

    /// <summary>
    /// This method is used to send message to student.
    /// </summary>
    /// <param name="asTestName"></param>
    private void SendMessageToStudent(string asTestName)
    {
        if (asTestName.Trim() != string.Empty)
        {
            string sMessageBody = string.Empty;
            string sClsTchrName = hidClassTeacher.Value;
            string sClsTeacher = sClsTchrName.Substring(sClsTchrName.IndexOf(':') + 1, ((sClsTchrName.Length - 1) - sClsTchrName.IndexOf(':'))).Trim();
            string sClass = sClsTchrName.Substring(0, sClsTchrName.IndexOf(':')).Trim();
            int iSmsId = Constants.SMSTemplate.ExamPublishSMS.ToInt();
            string sLoginDetailsSmsText = string.Empty;
            string sSmsSubject = string.Empty;

            DataTable oDTSmsTemplate = SmsTemplateBL.GetTemplate(iSmsId, miSchoolId);
            if (oDTSmsTemplate.Rows.Count != 0)
                if (oDTSmsTemplate.Rows[0][2] != DBNull.Value)
                    sLoginDetailsSmsText = Convert.ToString(oDTSmsTemplate.Rows[0][2]);
            sMessageBody = sLoginDetailsSmsText;
            sMessageBody = sMessageBody.Replace("%EXAM%", asTestName);
            sMessageBody = sMessageBody.Replace("%CLASSTEACHER%", sClsTeacher);
            sMessageBody = sMessageBody.Replace("%STUDENTCLASS%", sClass);
            SendMessage(hidUserID.Value, asTestName + " Result", sMessageBody);
        }
    }

    /// <summary>
    /// This method is used to send the message.
    /// </summary>
    private void SendMessage(string asUserId, string asMsgSubject, string asMsgBody)
    {
        Message oMessage = new Message { sMessageBody = asMsgBody, sMessageSubject = asMsgSubject };
        oMessage.SetMessageReceivers(asUserId, Session[Constants.S_SESSION_USER_ID].ToInt());
        oMessage.InsertMessageDetails(Session[Constants.S_SESSION_USER_ID].ToInt(), moUserRole.ToInt(), miAcademicYearId);
    }

    /// <summary>
    /// This method is used to generate comma seperated string of test ids.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    /// <returns></returns>
    private string GenerateTestIds(HtmlTable aoHtmlTable)
    {
        int iRowIndex = 0;
        string sTestIds = string.Empty;
        foreach (HtmlTableRow oHtmlTableRow in aoHtmlTable.Rows)
        {
            sTestIds = (from HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells
                        from Control oControl in oHtmlTableCell.Controls
                        where oHtmlTableRow.FindControl("chkPublish_" + iRowIndex) != null && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Checked
                        select oControl).OfType<HiddenField>().Where(control => control.ID.Contains("hidTestId_")).Aggregate(sTestIds, (current, oControl) => current + ((HiddenField)oHtmlTableRow.FindControl("hidTestId_" + iRowIndex)).Value + ",");
            iRowIndex++;
        }

        return sTestIds;
    }

    /// <summary>
    /// This method is used to generate xml for students marks details.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    /// <returns></returns>
    private string GenerateStudentMarksXml(HtmlTable aoHtmlTable)
    {
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("SchoolWiseStudentTestMarksDetails");
        XmlNode oXmlNode;
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarksDetails", string.Empty);
        int iRowIndex = 0;

        foreach (HtmlTableRow oHtmlTableRow in aoHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                string sIsAbsent = string.Empty;
                string sIsMarksAssigned = string.Empty;
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oHtmlTableRow.FindControl("chkPublish_" + iRowIndex) != null && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Checked && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Enabled)
                    {
                        if (oControl is TextBox && sIsMarksAssigned != Constants.S_NO)
                        {
                            string sCntrlId = oControl.ID;
                            char cSplit = Convert.ToChar(S_SAPARATOR);
                            string[] sIds = sCntrlId.Split(cSplit);
                            oXmlNode = GetNodeForMarksAssigned(ref oDoc, miStudentId, sIds, sIsAbsent, "SchoolWiseStudentTestMarksDetail");

                            string sAtrrName = "TestType_Id";
                            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = sIds[S_VAL_TESTTYPE_ID];
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "TestWise_Subject_Marks_Id";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = sIds[S_VAL_TESTWISE_SUBJECT_MARKS_ID];
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "Marks_Scored";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = ((TextBox)oControl).Text;
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "Assigned_Grade_Id";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = null;
                            oXmlNode.Attributes.Append(attr);

                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                        else if (oControl is DropDownList)
                        {
                            if (!oControl.ID.Contains("ddlExamStatus"))
                            {
                                string sCntrlId = oControl.ID;
                                char cSplit = Convert.ToChar(S_SAPARATOR);
                                string[] sIds = sCntrlId.Split(cSplit);
                                oXmlNode = GetNodeForMarksAssigned(ref oDoc, miStudentId, sIds, sIsAbsent, "SchoolWiseStudentTestMarksDetail");
                                oXmlRootNode.AppendChild(oXmlNode);

                                string sAtrrName = "TestWise_Subject_Marks_Id";
                                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = sIds[S_VAL_TESTWISE_SUBJECT_MARKS_ID];
                                oXmlNode.Attributes.Append(attr);

                                sAtrrName = "Marks_Scored";
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = null;
                                oXmlNode.Attributes.Append(attr);

                                sAtrrName = "Assigned_Grade_Id";
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = ((DropDownList)oControl).SelectedValue;
                                oXmlNode.Attributes.Append(attr);
                            }
                            else
                                sIsAbsent = ((DropDownList)oControl).SelectedValue;
                        }
                        else if (oControl is HiddenField)
                        {
                            if (oControl.ID.Contains("hidIsMarkAssigned_"))
                                sIsMarksAssigned = ((HiddenField)oControl).Value;
                        }
                    }
                    else
                    {
                        if (oControl.ID != null && oControl.ID.Contains("chkIsApplicable") && ((CheckBox)oControl).Checked)
                            sIsAbsent = string.Empty;
                    }
                }
            }

            iRowIndex++;
        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to generate xml for student marks.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    /// <returns></returns>
    private string GenerateStudentXml(HtmlTable aoHtmlTable)
    {
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("SchoolWiseStudentTestMarks");

        XmlNode oXmlNode;
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarks", string.Empty);

        int iRowIndex = 0;
        foreach (HtmlTableRow oHtmlTableRow in aoHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                DateTime oTestDate = DateTime.Now;
                string sIsAbsent = string.Empty;
                string sIsMarksAssigned = string.Empty;
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oHtmlTableRow.FindControl("chkPublish_" + iRowIndex) != null && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Checked && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Enabled)
                    {
                        if (oControl is TextBox && sIsMarksAssigned != Constants.S_NO)
                        {
                            string sCntrlId = oControl.ID;
                            char cSplit = Convert.ToChar(S_SAPARATOR);
                            string[] sIds = sCntrlId.Split(cSplit);
                            oXmlNode = GetNodeForMarksAssigned(ref oDoc, miStudentId, sIds, sIsAbsent, "SchoolWiseStudentTestMark");

                            string sAtrrName = "TestWise_Subject_Marks_Id";
                            XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = sIds[S_VAL_TESTWISE_SUBJECT_MARKS_ID].ToString();
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "Total_Marks_Scored";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = ((TextBox)oControl).Text;
                            int iTestTypeOutOfMarks = sIds[8].ToInt();
                            if (iTestTypeOutOfMarks != 0)
                                attr.Value = Math.Round(((TextBox)oControl).Text.ToDecimal() * iTestTypeOutOfMarks / sIds[9].ToDecimal(), ((sIds[12].ToBool() || !Settings.RoundMarksAtSubjectLevel) ? Constants.I_ONE : Constants.I_ZERO), MidpointRounding.AwayFromZero).ToString();
                            else
                                attr.Value = ((TextBox)oControl).Text;
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = "TestType_Id";
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = sIds[S_VAL_TESTTYPE_ID];
                            oXmlNode.Attributes.Append(attr);

                            sAtrrName = S_DB_COL_TEST_DATE;
                            attr = oDoc.CreateAttribute(sAtrrName);
                            attr.Value = oTestDate.ToString();
                            oXmlNode.Attributes.Append(attr);

                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                        else if (oControl is DropDownList)
                        {
                            if (!oControl.ID.Contains("ddlExamStatus"))
                            {
                                string sCntrlId = oControl.ID;
                                char cSplit = Convert.ToChar(S_SAPARATOR);
                                string[] sIds = sCntrlId.Split(cSplit);
                                oXmlNode = GetNodeForMarksAssigned(ref oDoc, miStudentId, sIds, sIsAbsent, "SchoolWiseStudentTestMark");

                                string sAtrrName = "TestWise_Subject_Marks_Id";
                                XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = sIds[S_VAL_TESTWISE_SUBJECT_MARKS_ID].ToString();
                                oXmlNode.Attributes.Append(attr);

                                sAtrrName = "Assigned_Grade_Id";
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = ((DropDownList)oControl).SelectedValue;
                                oXmlNode.Attributes.Append(attr);

                                sAtrrName = S_DB_COL_TEST_DATE;
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = oTestDate.ToString();
                                oXmlNode.Attributes.Append(attr);

                                sAtrrName = "IsInternalExternalTestApplicable";
                                attr = oDoc.CreateAttribute(sAtrrName);
                                attr.Value = Constants.S_NO;
                                oXmlNode.Attributes.Append(attr);
                                oXmlRootNode.AppendChild(oXmlNode);
                            }
                            else
                                sIsAbsent = ((DropDownList)oControl).SelectedValue;
                        }
                        else if (oControl is HiddenField)
                        {
                            if (oControl.ID.Contains("hidTestDate") && ((HiddenField)oControl).Value != "0" && ((HiddenField)oControl).Value != string.Empty)
                                oTestDate = ((HiddenField)oControl).Value.ToDateTime();
                            if (oControl.ID.Contains("hidIsMarkAssigned_"))
                                sIsMarksAssigned = ((HiddenField)oControl).Value;
                        }
                    }
                    else
                    {
                        if (oControl.ID != null && oControl.ID.Contains("chkIsApplicable") && ((CheckBox)oControl).Checked)
                            sIsAbsent = string.Empty;
                    }
                }
            }

            iRowIndex++;
        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to generate xml for test to be published.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    /// <param name="asMode"></param>
    /// <returns></returns>
    private string GenerateStudentWiseTestPublishStatusXml(HtmlTable aoHtmlTable, string asMode)
    {
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("StudentWiseTestPublishStatus");
        XmlNode oXmlNode;
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "StudentWiseTestPublishStatus", string.Empty);
        msTestNames = string.Empty;
        int iRowIndex = 0;
        if (asMode != "Save")
        {
            foreach (HtmlTableRow oHtmlTableRow in aoHtmlTable.Rows)
            {
                int iTestId = 0;
                if (oHtmlTableRow.FindControl("chkPublish_" + iRowIndex) != null && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Enabled)
                {
                    oXmlNode = oDoc.CreateNode(S_ELEMENT, "StudentWiseTestPublishStatusDetail", string.Empty);
                    string sAtrrName = "StudentId";
                    XmlAttribute attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = miStudentId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    if (oHtmlTableRow.FindControl("hidTestId_" + iRowIndex) != null)
                        iTestId = ((HiddenField)oHtmlTableRow.FindControl("hidTestId_" + iRowIndex)).Value.ToInt();
                    if (oHtmlTableRow.FindControl("hidTestName_" + iRowIndex) != null && ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Checked)
                        msTestNames += ((HiddenField)oHtmlTableRow.FindControl("hidTestName_" + iRowIndex)).Value + ",";

                    sAtrrName = "SchoolWise_Test_Id";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = iTestId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Standard_division_Id";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = hidStandardDivisionId.Value;
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "School_Id";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = miSchoolId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Academic_Year_Id";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = miAcademicYearId.ToString();
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Is_Published";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = ((CheckBox)oHtmlTableRow.FindControl("chkPublish_" + iRowIndex)).Checked ? Constants.S_YES : Constants.S_NO;
                    oXmlNode.Attributes.Append(attr);

                    sAtrrName = "Is_Deleted";
                    attr = oDoc.CreateAttribute(sAtrrName);
                    attr.Value = Constants.S_NO;
                    oXmlNode.Attributes.Append(attr);

                    oXmlRootNode.AppendChild(oXmlNode);
                }

                iRowIndex++;
            }
        }

        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    /// <summary>
    /// This method is used to get node for marks assignment.
    /// </summary>
    /// <param name="aoDoc"></param>
    /// <param name="aiStudentId"></param>
    /// <param name="asIds"></param>
    /// <param name="asIsAbsent"></param>
    /// <param name="asNodeName"></param>
    /// <returns></returns>
    private XmlNode GetNodeForMarksAssigned(ref XmlDocument aoDoc, int aiStudentId, string[] asIds, string asIsAbsent, string asNodeName)
    {
        XmlNode oXmlNode = aoDoc.CreateNode(S_ELEMENT, asNodeName, string.Empty);

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
        attr.Value = aiStudentId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Subject_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = asIds[S_VAL_SUBJECT_ID].ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = S_DB_COL_IS_ABSENT;
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = asIsAbsent;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        btnPublish.Attributes.Add("Onclick", "if(!(ConfirmAction(this,'" + GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId).ClientID + "'))){return false;}");
        btnSave.Attributes.Add("Onclick", "if(!(ConfirmAction(this,'" + GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId).ClientID + "'))){return false;}");
        btnView.Attributes.Add("Onclick", "if(!(ConfirmAction(this,'" + GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId).ClientID + "'))){return false;}");
        btnDelete.Attributes.Add("Onclick", "if(!(ConfirmAction(this,'" + GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId).ClientID + "'))){return false;}");
        ApplyMouseHoverEffect(new List<Button>() { btnBack, btnDelete, btnPublish, btnView, btnSave });
    }

    /// <summary>
    /// This method is used to either save or publish student marks.
    /// </summary>
    /// <param name="asMode"></param>
    private void ManageStudentMarks(string asMode)
    {
        HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + menumResultType + miStudentId);
        StudentSubjectMarksBL oStudentSubjectMarksBL = new StudentSubjectMarksBL
                                                           {
                                                               InsertedBYId = miUserId,
                                                               StudentDetails = GenerateStudentXml(oHtmlTable),
                                                               StudentMarkDetails = GenerateStudentMarksXml(oHtmlTable),
                                                               StudentTestSubmitStatus = GenerateStudentWiseTestPublishStatusXml(oHtmlTable, asMode)
                                                           };
        oStudentSubjectMarksBL.ManageTestWiseStudentMarks(hidRemoveProgressReport.Value, asMode, hidStandardDivisionId.Value.ToInt(), Settings.RoundMarksAtSubjectLevel ? Constants.S_YES : Constants.S_NO);
    }

    #endregion "Methods"
}