/*
* This Class is used to show student progress report 
 * rendered HTMLTable to show this progress report including subject group and test types.
 * Author: Shankar Gurav.
 * Date of creation: 28 Jan 2008
 * Date of modification: 2 Feb 2008

 * Modified Date - 11-Feb-2013
 * Modified by - Vipul
 * Modification Description - Code review changes - Use of entity classes and LINQ. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
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

public partial class StudentProgressSheetEdit : StudentProgress
{

    #region constants

    static string S_SAPARATOR = "_";
    Boolean isUpdateMode = false;

    #endregion constants

    #region Class Members

    Int32 miStudentId = 0;
    Int32 miClassTacherID = 0;
	int miStdDivId = 0;

    #endregion Class Members

    #region Events

    /// <summary>
    /// Overidded method for page initialization.
    /// </summary>
    /// <param name="e"></param>
    override protected void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            GetQueryString();
            if (menmPagemode == Constants.PageMode.Edit)
            {
                string ctrlname = Page.Request.Params.Get("__EVENTTARGET");
                if (!ctrlname.IsNullOrEmpty())
                {
                    if (btnResult.Equals(this.Page.FindControl(ctrlname)))
                        isUpdateMode = true;
                }
                ShowProgresSheet();
                if (!IsPostBack)
                {
                    GenerateResult(miStudentId, true);
                    Boolean isGraceApplied = IsGraceAppliedForTheStudent(miStudentId);
                    if (isGraceApplied)
                    {
                        btnResult.Attributes.Add("onclick", "if(!(ShowGraceWarning())){return false;}");
                        hidIsGraceApplied.Value = Constants.I_ONE.ToString();
                    }
                    else
                        btnResult.Attributes.Remove("onclick");
                }
                else
                    ResultContainer.Visible = false;
            }
            else
            {
                GenerateResult(miStudentId, false);
                btnResult.Visible = false;
                GridViewScrollContainer.Visible = false;                
            }            
        }
        catch (MarksNotAvailableForResult Ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            btnResult.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (NoResultFound)
        {
            ResultContainer.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (SqlException Ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = Ex.Message;
            btnResult.Visible = false;
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
        }
        catch (Exception ex)
        {
            hidResultGenrted.Value = Constants.I_ZERO.ToString();
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This method is used to intialize the page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            base.SetpanelMember(GridViewScrollContainer);
            if (!IsPostBack)
            {
                if (moUserRole != Constants.UserRoles.Student)
                    btnBack.Attributes.Add("onclick", "window.open('" + "../Teacher/StudentResultList.aspx?" + HidBackUrl.Value + "' , '_self').focus(); return false;");
            }
	        ApplyMouseHoverEffect(new List<Button> { btnBack, btnResult });

            if (moUserRole == Constants.UserRoles.Student)
                btnBack.Visible = false;
        }
        catch (ResultNotPublished ex)
        {
            pnlErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to generate result and show into 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnResult_Click(object sender, EventArgs e)
    {
        try
        {
            ResultContainer.Visible = true;
            if (hidEdited.Value == Constants.I_ONE.ToString() ||
                    hidResultGenrted.Value == Constants.I_ZERO.ToString() || hidIsGraceApplied.Value == Constants.I_ONE.ToString())
            {
                UpdateStudentMarks(miStudentId);
                btnResult.Attributes.Remove("onclick");
            }
            hidEdited.Value = Constants.I_ZERO.ToString();
            GenerateResult(miStudentId,true);
        }
        catch (BusinessLogic.Exceptions.NoResultFound)
        {
            ResultContainer.Visible = false;            
        }
        catch (Exception ex)
        {
	        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to navigate to back page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage = (MasterPage)this.Master; oMasterPage.RedirectToNextPage("../Teacher/StudentResultList.aspx?" + HidBackUrl.Value);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events

    #region Override Method

    /// <summary>
    /// This method is used to create required Html rows For tests and add it to progress table
    /// </summary>    
    protected override void CreateExamsAndTotalBlankRows()
    {
        Exam oExamDetails = CreateExamBlankRows(moStudentProgressReport.ExamDetails);
        String strTemp = S_CSS_PRINT_PREFIX;
        S_CSS_PRINT_PREFIX = "T";
        CreateAndAddBlankRows(oExamDetails, true);
        S_CSS_PRINT_PREFIX = strTemp;
    }

    /// <summary>
    /// This method is used to set marks to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamMarks(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && 
                                                                                               ma.TestTypeId == oSubjectDetailsForProgressReport.SubjectCellColSpan && 
                                                                                               ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (oMarkAssignmentDetails.Count > 0 && !oMarkAssignmentDetails[0].Marks.IsNullOrEmpty())
        {
            //If subject has grade then dont append total marks(i.e 12/100)                     
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];

            if (oMarkAssignmentDetails[0].IsAbsent == "N")
            {
                SetValuesToCell(oHtmlTableCell, oHTSubjectEntry, aiRowIndex, oMarkAssignmentDetails[0]);
            }
            else
			{
                ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oMarkAssignmentDetails[0].IsAbsent);
                string sExamStatus = oExamStatus.DisplayName;
                string sColor = oExamStatus.ForeColor;
				oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
			}

        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// This method is used to set grade to subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGrade(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        string sSelectFilter = string.Empty;
        var oMarkAssignmentDetails = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];        
        if (oMarkAssignmentDetails.Count > 0)
        {
            if (!oMarkAssignmentDetails[0].Grade.IsNullOrEmpty() && oMarkAssignmentDetails[0].IsAbsent == Constants.S_NO)
            {
                if (aiTestId != -1)
                {
                    // If subject has grade then dont append total marks(i.e 12/100)  
                    DropDownList oDropDownList = new DropDownList();
                    FillGradesCombobox(oDropDownList, oMarkAssignmentDetails[0].IsCoCurricularActivity);
                    oDropDownList.ID = CreateIDForCntrl("ddl", aiRowIndex, oHTSubjectEntry.Key.ToInt()
                                           , oMarkAssignmentDetails[0].SchoolWiseStudentTestId
                                           , oSubjectDetailsForProgressReport.SubjectCellRowSpan
                                           , oMarkAssignmentDetails[0].TestWiseSubjectId
                                           , oSubjectDetailsForProgressReport.SubjectCellType);
                    ListItem oListItem = oDropDownList.Items.FindByText(oMarkAssignmentDetails[0].Grade);
                    if (!isUpdateMode)
                        oDropDownList.SelectedValue = oListItem.Value;
                    oHtmlTableCell.Controls.Add(oDropDownList);
                }
                else
                    SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
            }
            else
            {
                if (aiTestId != -1)
				{
                    string sExamStatus = oMarkAssignmentDetails[0].Marks.Trim();
                    ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.DisplayName == sExamStatus);
                    string sColor = oExamStatus.ForeColor;
					oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
				}
                else
                    SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
            }
        }
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), null);
    }

    /// <summary>
    /// Fills combobox for each row if the grades are to be assigned to students.
    /// </summary>
    protected void FillGradesCombobox(DropDownList ddlGrade, bool abIsCoCuricularSubject)
    {
        ddlGrade.DataTextField = "GradeName";
        ddlGrade.DataValueField = "GradeId";
        ddlGrade.DataSource = moStudentProgressReport.GradeDetails.ToList().Where(grade => grade.IsForCoCurricularSubjects == abIsCoCuricularSubject).ToList();
        ddlGrade.DataBind();
        ddlGrade.Dispose();
    }

    /// <summary>
    /// This method is used to set exam type total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamTypeTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {

        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oMarksAssignment = moStudentProgressReport.MarkAssignmentDetails.Where(ma => ma.SchoolWiseTestId == aiTestId && ma.SubjectId == oSubjectDetailsForProgressReport.SubjectId).ToList();
        if (oMarksAssignment.Count > 0 && !oMarksAssignment[0].Marks.IsNullOrEmpty())
        {
            //If subject has grade then dont append total marks(i.e 12/100)
            HtmlTableCell oHtmlTableCell = tblProgress.Rows[aiRowIndex].Cells[oHTSubjectEntry.Key.ToInt()];
            if (oMarksAssignment[0].IsAbsent.Trim() == Constants.S_NO)
            {
                Label olblMarks = new Label();
                Label olblMarksTotal = new Label();
                olblMarks.EnableViewState = true;
                olblMarksTotal.EnableViewState = true;
                olblMarks.Text = oMarksAssignment[0].TotalMarksScored.ToString("0.#");
                olblMarks.ID = CreateIDForCntrl("lbl", aiRowIndex, oHTSubjectEntry.Key.ToInt(), aiTestId, oSubjectDetailsForProgressReport.SubjectCellColSpan, oSubjectDetailsForProgressReport.SubjectId, oSubjectDetailsForProgressReport.SubjectCellType);

                olblMarksTotal.Text = " / " + oMarksAssignment[0].SubjectTotalMarks;
                oHtmlTableCell.Controls.Add(olblMarks);

                oHtmlTableCell.Controls.Add(olblMarksTotal);
            }
            else
            {
                string sExamStatus = oMarksAssignment[0].Marks.Trim();
                ExamStatus oExamStatus = mlstExamStatusDetails.FirstOrDefault(esd => esd.ShortName == oMarksAssignment[0].IsAbsent);
                sExamStatus = oExamStatus.DisplayName;
                string sColor = oExamStatus.ForeColor;
                oHtmlTableCell.InnerHtml = "<B><font size='2pt' Type='verdana' color='" + sColor + "'>" + sExamStatus + "</font></B>";
            }
            if (!IsTotalConsiderForProgressReport())
                oHtmlTableCell.Attributes.Add("style", "display:none");
        }
        else if (!IsTotalConsiderForProgressReport())
            SetNotApplicableCellValuesForExamTypeTotal(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), string.Empty);
        else
            SetNotApplicableCellValues(tblProgress.Rows[aiRowIndex], oHTSubjectEntry.Key.ToInt(), string.Empty);

    }

    /// <summary>
    /// This method is used to set exam group total of a subject cell for a given test Id.
    /// </summary>
    /// <param name="aiTestId"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    protected override void FillSubjectExamGroupTotal(int aiTestId, DictionaryEntry oHTSubjectEntry, int aiRowIndex)
    {
        //We are overriding this function still we need to call base class's methods beacause in base class version of this mathod 
        //there is calls for another methods which are overrided here causes caling base class method but still method of derrived class 
        //get called in between calls of overrided methods.
        SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;
        var oParentSubjectDetails = moStudentProgressReport.SubjectDetails.Where(subject => subject.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId).OrderByDescending(oSubjectDetails => oSubjectDetails.Id).ToList<Subject>();

        if (oParentSubjectDetails.Count > 0 && !oParentSubjectDetails[0].SubjectName.IsNullOrEmpty())// && oParentSubjectDetails[0].SubjectId == oSubjectDetailsForProgressReport.iSubjectId)
        {
            //Take a group total of a subject.
            var oParentSubjectTotalDetails = moStudentProgressReport.SubjectTestGroupTotalDetails.Cast<StudentWiseProgressReportSubjectTestGroupTotal>()
                                                                    .Where(sgt => sgt.ParentSubjectId == oSubjectDetailsForProgressReport.ParentSubjectId && 
                                                                                  sgt.SchoolWiseTestId == aiTestId)
                                                                    .ToList<StudentWiseProgressReportSubjectTestGroupTotal>();
            if (oParentSubjectTotalDetails.Count > 0)
                base.FillSubjectExamGroupTotal(aiTestId, oHTSubjectEntry, aiRowIndex);
            else
                base.FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
        }
        else
            base.FillSubjectExamMarks(aiTestId, oHTSubjectEntry, aiRowIndex);
    }

    #endregion Override Method

    #region Private Methods

    /// <summary>
    /// This method is used to fill tests result to a progress table
    /// </summary>    
    protected override void FillExamsMarks()
    {
        Exam oExam = FillExamWiseSubjectMarks();
        FillSubjectTotal(oExam, moStudentProgressReport.ExamDetails.Count + 2);
    }

    /// <summary>
    /// This method is used to add controls and set values to cell.
    /// </summary>
    /// <param name="oHtmlTableCell"></param>
    /// <param name="oHTSubjectEntry"></param>
    /// <param name="aiRowIndex"></param>
    /// <param name="aoMarkAssignment"></param>
    private void SetValuesToCell(HtmlTableCell oHtmlTableCell, DictionaryEntry oHTSubjectEntry, int aiRowIndex, MarkAssignment aoMarkAssignment)
    {
        if (aoMarkAssignment.TestTypeTotalMarks >= Constants.I_ZERO)
        {
            SubjectDetailsForProgressReport oSubjectDetailsForProgressReport = (SubjectDetailsForProgressReport)oHTSubjectEntry.Value;

            Label olblMarksTotal = new Label();
            olblMarksTotal.Text = " / " + aoMarkAssignment.TestTypeTotalMarks;

            if (aoMarkAssignment.SchoolWiseTestId != -1)
            {
                TextBox oTextBox = new TextBox();
                if (!isUpdateMode)
                    oTextBox.Text = aoMarkAssignment.MarksScored.ToString("0.#");
                oTextBox.ID = CreateIDForCntrl("txt", aiRowIndex, oHTSubjectEntry.Key.ToInt()
                                    , aoMarkAssignment.SchoolWiseStudentTestId
                                    , oSubjectDetailsForProgressReport.SubjectCellColSpan
                                    , aoMarkAssignment.TestWiseSubjectId
                                    , oSubjectDetailsForProgressReport.SubjectCellType);

                bool bAllowDecimal = aoMarkAssignment.AllowDecimal;
                oTextBox.MaxLength = bAllowDecimal ? Constants.I_FIVE : Constants.I_THREE;
                oTextBox.Width = Unit.Pixel(40);
                oTextBox.CssClass = "ExSmlTxtBoxP";
                oTextBox.Attributes.Add("onkeyup", "javascript:extractNumber(this," + (bAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                oTextBox.Attributes.Add("onblur", "javascript:Validate(this," + aoMarkAssignment.TestTypeTotalMarks + ");extractNumber(this," + (bAllowDecimal ? Constants.I_ONE : Constants.I_ZERO) + ",false);");
                oTextBox.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, " + aoMarkAssignment.AllowDecimal.ToString().ToLower() + ", false);");
                oTextBox.Attributes.Add("onfocus", "javascript:SetValue(this);");
                oHtmlTableCell.Controls.Add(oTextBox);
            }
            else
            {
                Label olblMarks = new Label();
                olblMarks.Text = aoMarkAssignment.MarksScored.ToString("0.#");
                oHtmlTableCell.Controls.Add(olblMarks);
            }
            oHtmlTableCell.Controls.Add(olblMarksTotal);
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
    private String CreateIDForCntrl(String asPrefix, int aiRowIndex, int aiCellIndex, int aiTestId, int aiTestTypeId, int aiSubjectId, Constants.ReportCellType aiEnumColType)
    {
        return asPrefix + S_SAPARATOR + aiRowIndex + S_SAPARATOR + aiCellIndex
            + S_SAPARATOR + aiTestId
            + S_SAPARATOR + aiTestTypeId + S_SAPARATOR + aiSubjectId
            + S_SAPARATOR + aiEnumColType.ToInt();
    }

    /// <summary>
    /// This method is used to update students marks
    /// </summary>
    /// <param name="aiStudentId"></param>
    private void UpdateStudentMarks(int aiStudentId)
    {
        HtmlTable oHtmlTable = (HtmlTable)GridViewScrollContainer.FindControl("tbl_" + menumResultType + aiStudentId);
        string xmlStr = getMarksUpdateXML(aiStudentId, oHtmlTable);
        char cUseAvarageFinalResult = Settings.UseAvarageFinalResult ? Constants.C_YES : Constants.C_NO;
        StudentSubjectMarksBL.UpdateStudentTestMarks(miUserId, xmlStr, cUseAvarageFinalResult);
        GridViewScrollContainer.Controls.Clear();
        isUpdateMode = false;
        ShowProgresSheet();
    }

    /// <summary>
    /// This method is used to show progress
    /// </summary>
    private void ShowProgresSheet()
    {
        SetpanelMember(GridViewScrollContainer);
        int iTeacherId = 0;
        if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Session[Constants.S_SESSION_TEACHER_ID].ToInt();
        else if (miClassTacherID != 0)
            iTeacherId = miClassTacherID;
        base.mbFinalResult = true;
		base.ShowProgressSheet(miStdDivId, miStudentId);
    }

    /// <summary>
    /// This method is used to create an result of a student
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <param name="abShowMarks"></param>
    private void GenerateResult(int aiStudentId, bool abShowMarks)
    {
        StudentResult oStudentResult = new StudentResult(ResultContainer, abShowMarks);
        oStudentResult.FillProgressReport(aiStudentId);
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 1)
        {
	        if (QueryString["TeacherId"] != null)
		        miClassTacherID = QueryString["TeacherId"].ToInt();
			if (QueryString["StandardDivisionId"] != null)
				miStdDivId = QueryString["StandardDivisionId"].ToInt();
	        if (QueryString["StudentId"] != null)
		        miStudentId = QueryString["StudentId"].ToInt();
	        if (QueryString["Mode"] != null && QueryString["Mode"] == "Edit")
		        menmPagemode = Constants.PageMode.Edit;
            HidBackUrl.Value = Request.QueryString.ToString();
        }

        if (moUserRole == Constants.UserRoles.Student)
        {
            miStudentId = Session[Constants.S_SESSION_STUDENT_ID].ToInt();
            CheckIsResultPublished();
        }
    }

    /// <summary>
    /// This method is used to check that is Result is published or not
    /// </summary>
    private void CheckIsResultPublished()
    {
        int iStandardDivisionId = Session[Constants.S_SESSION_STUDENT_STANDERED_DIVISION_ID].ToInt();
        SchoolWiseAnnualResultPublishBL oSchoolWisdeAnnualResultPublishBL = new SchoolWiseAnnualResultPublishBL(miSchoolId, miAcademicYearId, iStandardDivisionId);
        if (oSchoolWisdeAnnualResultPublishBL.AnnualResult_publish_Id == 0)
            throw new ResultNotPublished("Result not published for this class.");
    }

    /// <summary>
    /// This method is used to check that is the grace applicable for any student or not.
    /// </summary>
    /// <param name="aiStudentId"></param>
    /// <returns></returns>
    private bool IsGraceAppliedForTheStudent(int aiStudentId)
    {
        SchoolWiseAnnualResultPublishBL oSWStdDivResultPublishBL = new SchoolWiseAnnualResultPublishBL();
        return oSWStdDivResultPublishBL.IsGraceAppliedForTheStudent(aiStudentId);
    }

    #endregion Private Methods

    #region XML functions

    private string getMarksUpdateXML(int aiStudentId, HtmlTable oHtmlTable)
    {

        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement root = oDoc.CreateElement("SchoolWiseStudentTestMarksDetails");
        XmlNode oXmlNode = null;
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarksDetails", "");
        foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
        {
            foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
            {
                foreach (Control oControl in oHtmlTableCell.Controls)
                {
                    if (oControl is TextBox)
                    {
                        String sCntrlId = oControl.ID;
                        Char cSplit = Convert.ToChar(S_SAPARATOR);
                        String[] sIds = sCntrlId.Split(cSplit);
                        oXmlNode = GetNodeTestMarks(aiStudentId, (TextBox)oControl, sIds, ref oDoc);
                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                    else if (oControl is DropDownList)
                    {
                        String sCntrlId = oControl.ID;
                        Char cSplit = Convert.ToChar(S_SAPARATOR);
                        String[] sIds = sCntrlId.Split(cSplit);
                        oXmlNode = GetNodeTestGrade(aiStudentId, (DropDownList)oControl, sIds, ref oDoc);
                        oXmlRootNode.AppendChild(oXmlNode);
                    }
                }
            }
        }
        // Add the root node to document element. 
        root.AppendChild(oXmlRootNode);

        // return the string generated.
        return root.InnerXml;
    }

    private XmlNode GetNodeTestGrade(int aiStudentId, DropDownList oddlGrade, String[] sIds, ref XmlDocument aoDoc)
    {
        XmlNode oXmlNode = GetNodeForMarksAssigned(ref aoDoc, aiStudentId, sIds);

        string sAtrrName = "Marks_Scored";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Assigned_Grade_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = oddlGrade.SelectedValue;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    private XmlNode GetNodeTestMarks(int aiStudentId, TextBox oTxtMarks, String[] sIds, ref XmlDocument aoDoc)
    {
        XmlNode oXmlNode = GetNodeForMarksAssigned(ref aoDoc, aiStudentId, sIds);

        string sAtrrName = "Marks_Scored";
        XmlAttribute attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = oTxtMarks.Text;
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "Assigned_Grade_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = null;
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    private XmlNode GetNodeForMarksAssigned(ref XmlDocument aoDoc, int aiStudentId, String[] sIds)
    {
        int iTestMarksId = sIds[3].ToInt();
        int iTestTypeID = sIds[4].ToInt();
        int iSubjectMarksId = sIds[5].ToInt();

        const string S_ELEMENT = "element";
        XmlNode oXmlNode = aoDoc.CreateNode(S_ELEMENT, "SchoolWiseStudentTestMarksDetail", "");

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

        sAtrrName = "TestWise_Subject_Marks_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = iSubjectMarksId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "SchoolWise_Student_Test_Marks_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = iTestMarksId.ToString();
        oXmlNode.Attributes.Append(attr);

        sAtrrName = "TestType_Id";
        attr = aoDoc.CreateAttribute(sAtrrName);
        attr.Value = iTestTypeID.ToString();
        oXmlNode.Attributes.Append(attr);

        return oXmlNode;
    }

    #endregion XML functions

}