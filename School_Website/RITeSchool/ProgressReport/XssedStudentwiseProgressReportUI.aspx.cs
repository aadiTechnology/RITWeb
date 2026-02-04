/* File Name - XseedStudentwiseProgressReportUI.aspx.cs
 * Created Date - 3-Nov-2011
 * Created by - Vipul
 * Class Description - This class is used to display studentwise xseed progress report.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;
using System.Resources;

public partial class XssedStudentwiseProgressReportUI : XseedProgressReport
{
    #region Constants

    const string S_CLASS_TEACHER_ID = "ClassTeacherId";
    const string S_STUDENT_ID = "StudentId";
    const string S_SUBJECT_ID = "SubjectId";
    const string S_STANDARD_ID = "StandardId";
    const string S_STANDARD_DIVISION_ID = "StandardDivisionId";
    const string S_EDIT_MODE = "EditMode";
    const string S_ASSESSMENT_ID = "AssessmentId";
    const string S_LEARNING_OUTCOMES = "LearningOutcomes";
    const string S_OBSERVATION = "Observation";
    const string S_GRADE_ID = "GradeId";
    const string S_NON_XSEED_SUBJECT_GRADES = "NonXseedSubjectGrades";
    string S_PUBLISH = "Publish";
    string S_UNPUBLISH = "Unpublish";
    const string S_TXT_OBSERVATION = "txtObservation_";
    const string S_DDL_COCURRICULAR_SUBJECTS_GRADE = "ddlCoCurricularSubjectsGrade_";
    const string S_DDL_NON_XSEED_SUBJECTS_GRADE = "ddlNonXseedGrade_";
    const string S_URL_XSEED_PROGRESS_REPORT = "~/RITeSchool/ProgressReport/XssedStudentwiseProgressReportUI.aspx?";

    #endregion Constants

    #region Member Variables

    ResourceManager oResourceManager=new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region Events

    /// <summary>
    /// This event is used to set base class details.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnInit(EventArgs e)
    {
        try
        {
            base.OnInit(e);
            if (Convert.ToString(Request.Params[hidIsPublishButtonClick.ClientID.Replace("_", "$")]) == Constants.S_YES || Convert.ToString(Request.Params[hidIsBtnSave.ClientID.Replace("_", "$")]) == Constants.S_YES)
                DisplayProgressReport(false);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill class teacher and assessment combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SetRemarkLength();
                if (CheckPreCondition())
                {
                    if (Session[Constants.S_SESSION_LANGUAGE] != null)
                    {
                        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                    }
                    GetQueryString();
                    FillAssessmentCombo();
                }
                else
                    btnBack.Visible = false;
                SetJavascriptAttributes();
                RefreshValue();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
                if (cmbAssessment.SelectedValue != "0")
                {
                    DisplayProgressReport(true);
                    // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
                    SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                    btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
                }

            }

            hidIsBtnSave.Value = "N";
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    private void SetRemarkLength()
    {
        if (moSchool == Constants.SchoolId.PPS)
            hidRemarkLength.Value = "400";
        else
            hidRemarkLength.Value = "300";
    }

    /// <summary>
    /// This event is used to display progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbAssessment.SelectedValue != "0")
            {
                DisplayProgressReport(true);
                // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
            }
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save Xseed grades and observations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            XseedProgressReportBL oXseedProgressReportBL = new XseedProgressReportBL
                                                               {
                                                                   LearningOutcomeXML = GetLearningOutcomeXml((HtmlTable)tblMainProgressReport.FindControl("tblXseedLearningOutcomes")),
                                                                   XseedGradesXML = GetXseedGradesXml(),
                                                                   Remark = GetRemark(),
                                                                   SubjectRemark = GetSubjectRemarks()
                                                               };
            oXseedProgressReportBL.ManageStudentWiseAssessmentGrades(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentId.Value), Convert.ToInt32(hidstdDivId.Value), Convert.ToInt32(cmbAssessment.SelectedValue), miUserId, "Save");
            
            MasterPage oMasterPage = (MasterPage)Master;
            string sQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString()));
            if (sQueryString.IndexOf("&AssessmentId=") != -1)
                sQueryString = sQueryString.Substring(0, sQueryString.IndexOf("&AssessmentId="));
            oMasterPage.RedirectToNextPage(S_URL_XSEED_PROGRESS_REPORT + CommonUtility.EncryptQuerystring(sQueryString + "&AssessmentId=" + cmbAssessment.SelectedValue+"&StatusId=1"));
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }
      

    /// <summary>
    /// This event is used to publish Xseed grades and observations.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPublish_Click(object sender, EventArgs e)
    {
        try
        {
            var oXseedProgressReportBL = new XseedProgressReportBL
                                        {
                                            LearningOutcomeXML = GetLearningOutcomeXml((HtmlTable)tblMainProgressReport.FindControl("tblXseedLearningOutcomes")),
                                            XseedGradesXML = GetXseedGradesXml(),
                                            Remark = GetRemark()
                                        };
            string sPublish = btnPublish.Text == Resources.LocalizedResources.Publish ? S_PUBLISH : S_UNPUBLISH;
            oXseedProgressReportBL.ManageStudentWiseAssessmentGrades(miSchoolId, miAcademicYearId, Convert.ToInt32(hidStudentId.Value), Convert.ToInt32(hidstdDivId.Value), Convert.ToInt32(cmbAssessment.SelectedValue), miUserId, sPublish);
            MasterPage oMasterPage = (MasterPage)Master;
            string sQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString()));
            if (sQueryString.IndexOf("&AssessmentId=") != -1)
                sQueryString = sQueryString.Substring(0, sQueryString.IndexOf("&AssessmentId="));
            oMasterPage.RedirectToNextPage(S_URL_XSEED_PROGRESS_REPORT + CommonUtility.EncryptQuerystring(sQueryString + "&AssessmentId=" + cmbAssessment.SelectedValue + "&StatusId=" + (sPublish == S_PUBLISH ? 2 : 3)));
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

   

    /// <summary>
    /// This event is used to view Xseed progress report.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            string sQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString())).Replace("&EditMode=Y", "&EditMode=N");
            if (sQueryString.IndexOf("&AssessmentId=") != -1)
                sQueryString = sQueryString.Substring(0, sQueryString.IndexOf("&AssessmentId="));
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(S_URL_XSEED_PROGRESS_REPORT + CommonUtility.EncryptQuerystring(sQueryString + "&AssessmentId=" + cmbAssessment.SelectedValue));
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to go back to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            string sUrl = string.Empty;
            if (hidIsStudentwiseProgressReport.Value == Constants.S_YES && hidEditMode.Value == Constants.S_YES)
                sUrl = "~/RITeSchool/ProgressReport/StudentwiseProgreesReportUI.aspx?" + Request.QueryString;
            else if (hidIsStudentwiseProgressReport.Value == Constants.S_YES && hidEditMode.Value == Constants.S_NO)
                sUrl = S_URL_XSEED_PROGRESS_REPORT + CommonUtility.EncryptQuerystring(CommonUtility.DecryptQuerystring(Server.UrlDecode(Request.QueryString.ToString())).Replace("&EditMode=N", "&EditMode=Y"));
            MasterPage oMasterPage = (MasterPage)Master;
            oMasterPage.RedirectToNextPage(sUrl);
        }
        catch (Exception oEx)
        {
            ExceptionHandler.WriteExceptionToErrorLog(oEx, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to return subject remarks.
    /// </summary>
    /// <returns></returns>
    private string GetSubjectRemarks()
    {
        HtmlTableRow trSubjectRemark = (HtmlTableRow)tblMainProgressReport.FindControl("trSubjectRemark");
        string sRemark = string.Empty;
        List<SubjectRemark> lstSubjectRemarks = new List<SubjectRemark>();
        if (trSubjectRemark != null)
        {
            HtmlTable tblSubjectRemark = (HtmlTable)trSubjectRemark.FindControl("tblSubjectRemark");
            if (tblSubjectRemark != null)
            {
                if (ViewState[Constants.S_SUBJECT_REMARK] != null)
                {
                    List<int> lstSubjectIds = ViewState[Constants.S_SUBJECT_REMARK] as List<int>;

                    lstSubjectIds.ForEach(
                        id =>
                        {
                            TextBox txtremark = (TextBox)tblSubjectRemark.FindControl("txtSubjectRemark_" + id);
                            if (txtremark != null)
                            {
                                lstSubjectRemarks.Add(new SubjectRemark { SubjectId = id, Remark = txtremark.Text.Trim() });
                            }
                        }
                    );
                }
            }
        }

        if (lstSubjectRemarks.Count > 0)
            sRemark = base.GenerateXml(lstSubjectRemarks);

        return sRemark;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnBack, btnPublish, btnSave, btnView });
        valSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnPublish.Attributes.Add("onclick", "if(!(SetInitStatus(this))){return false;}");
        btnSave.Attributes.Add("onclick", "if(!(SetInitStatus(this))){return false;}");
        btnView.Attributes.Add("onclick", "if(!(SetInitStatus(this))){return false;}");
        //cmbAssessment.Attributes.Add("onchange", "if(!(SetInitStatus(this))){return false;}");
    }

    /// <summary>
    /// This method is used to display progress report.
    /// </summary>
    private void DisplayProgressReport(bool abSetGrade)
    {
        miStudentId = (Request.Params[hidStudentId.ClientID.Replace("_", "$")] ?? hidStudentId.Value).ToInt();
        miAssessmentId = (Request.Params[cmbAssessment.ClientID.Replace("_", "$")] ?? cmbAssessment.SelectedValue).ToInt();
        miStandDivisionId = (Request.Params[hidstdDivId.ClientID.Replace("_", "$")] ?? hidstdDivId.Value).ToInt();
        mbIsEditMode = Convert.ToString(Request.Params[hidEditMode.ClientID.Replace("_", "$")] ?? hidEditMode.Value) != Constants.S_NO;
        SetProgressReportTableAndErrorRow(tblMainProgressReport, trErrorMessage);
        moXseedProgressReportBL = new XseedProgressReportBL
        {
            ExamResult = new ExamResult
            {
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId,
                AssessmentId = miAssessmentId,
                YearwiseStudentId = miStudentId,
                StandardDivisionId = miStandDivisionId
            }
        };
        moXseedProgressReportBL.GetXseedProgressReport();

        if (moXseedProgressReportBL.StudentsLearningOutcomes.Any(slo => slo.LearningOutcomeGradeId != 0))
        {
            btnPublish.Enabled = !moXseedProgressReportBL.AssessmentPublishStatus;
            btnView.Enabled = moXseedProgressReportBL.StudentWiseAssessmentPublishStatus;
        }
        else
        {
            btnPublish.Enabled = false;
            btnView.Enabled = false;
        }

       // btnPublish.Text = moXseedProgressReportBL.StudentWiseAssessmentPublishStatus ? S_UNPUBLISH : S_PUBLISH;
        btnPublish.Text = moXseedProgressReportBL.StudentWiseAssessmentPublishStatus ? Resources.LocalizedResources.Unpublish : Resources.LocalizedResources.Publish;
        HidbtnPublishText.Value = moXseedProgressReportBL.StudentWiseAssessmentPublishStatus ? Resources.LocalizedResources.Unpublish : Resources.LocalizedResources.Publish;
        btnSave.Enabled = !(moXseedProgressReportBL.StudentWiseAssessmentPublishStatus || moXseedProgressReportBL.AssessmentPublishStatus);

        if (Convert.ToString(Request.Params[hidEditMode.ClientID.Replace("_", "$")] ?? hidEditMode.Value) == Constants.S_NO && !moXseedProgressReportBL.AssessmentPublishStatus && !moXseedProgressReportBL.StudentWiseAssessmentPublishStatus)
        {
            trErrorMessage.Visible = true;
        }
        else
        {
            mbIsStudentWiseProgressReport = true;
            mbSetGrade = abSetGrade;
            SetStudentwiseProgressReport(hidRemarkLength.Value.ToInt());
            btnSave.Visible = hidEditMode.Value == Constants.S_YES && !trErrorMessage.Visible;
            btnPublish.Visible = hidEditMode.Value == Constants.S_YES && !trErrorMessage.Visible;
            btnView.Visible = hidEditMode.Value == Constants.S_YES && !trErrorMessage.Visible;
            hidIsPublishButtonClick.Value = Constants.S_NO;
        }
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.XseedResults);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            tblProgressReportDetails.Visible = false;
            trErrorMessage.Visible = false;
            trFilterSelection.Visible = false;
        }

        return bReturn;
    }

    /// <summary>
    /// This function sets the form fields according to the query string values.
    /// </summary>
    private void GetQueryString()
    {
        if (QueryString.Count > 0)
        {
            hidAssessmentId.Value = string.Empty;
            if (QueryString[S_CLASS_TEACHER_ID] != null)
                hidClassTacherID.Value = QueryString[S_CLASS_TEACHER_ID];
            if (QueryString[S_STUDENT_ID] != null)
                hidStudentId.Value = QueryString[S_STUDENT_ID];
            if (QueryString[S_STANDARD_ID] != null)
                hidStandardId.Value = QueryString[S_STANDARD_ID];
            if (QueryString[S_STANDARD_DIVISION_ID] != null)
                hidstdDivId.Value = QueryString[S_STANDARD_DIVISION_ID];
            if (QueryString[S_EDIT_MODE] != null)
                hidEditMode.Value = QueryString[S_EDIT_MODE];
            if (QueryString[S_ASSESSMENT_ID] != null)
                hidAssessmentId.Value = QueryString[S_ASSESSMENT_ID];
            hidIsStudentwiseProgressReport.Value = (hidEditMode.Value == string.Empty) ? Constants.S_NO : Constants.S_YES;

            if (QueryString["StatusId"] != null && QueryString["StatusId"].ToString() != string.Empty)
            {
                if (Convert.ToInt32(QueryString["StatusId"]) == 1)
                    lblSuccessfulMsg.Text = "Pre-Primary grades saved successfully!!!";
                else if (Convert.ToInt32(QueryString["StatusId"]) == 2)
                    lblSuccessfulMsg.Text = "Pre-Primary grades published successfully!!!";
                else if (Convert.ToInt32(QueryString["StatusId"]) == 3)
                    lblSuccessfulMsg.Text = "Pre-Primary grades unpublished successfully!!!";
            }
        }
    }

    /// <summary>
    /// This method is used to fill assessment combobox.
    /// </summary>
    private void FillAssessmentCombo()
    {
        StandardwiseAssessmentMasterBL oStandardwiseAssessmentMasterBL = new StandardwiseAssessmentMasterBL(miSchoolId, miAcademicYearId);
        oStandardwiseAssessmentMasterBL.GetStandardwiseAssessmentDetails(Convert.ToInt32(hidStandardId.Value));
        ListSource.FillDropDownList(oStandardwiseAssessmentMasterBL.lstStandardwiseAssessmentDetails.Where(assessment => assessment.StandardwiseAssessmentId != 0).ToList(), cmbAssessment, "AssessmentName", "AssessmentId", string.Empty);

        if (!string.IsNullOrEmpty(hidAssessmentId.Value))
        {
            cmbAssessment.SelectedValue = hidAssessmentId.Value;
            DisplayProgressReport(true);
            // This is for Check Publish or Unpublish Exam Right and based on that we hide Those particular buttons
            if (hidEditMode.Value == Constants.S_YES)
            {
                SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
                btnPublish.Visible = oSchoolUserBL.CanPublishUnpublishExam;
            }
        }
    }

    /// <summary>
    /// This method is used to get learnig outcomes XML.
    /// </summary>
    /// <param name="aoHtmlTable"></param>
    /// <returns></returns>
    private string GetLearningOutcomeXml(HtmlTable aoHtmlTable)
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement(S_LEARNING_OUTCOMES);
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_LEARNING_OUTCOMES, string.Empty);
        string sLearningOutcomeObservation = string.Empty;
        bool bIsDisabled = true;
        string sSubjectSectionId = string.Empty;
        if (aoHtmlTable != null)
            foreach (HtmlTableRow oHtmlTableRow in aoHtmlTable.Rows)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_LEARNING_OUTCOMES, string.Empty);
                string sGradeId = string.Empty;
                string sLearningOutcomeGradeId = string.Empty;
                string sSubjectSectionConfigurationId = string.Empty;
                string sLearningOutcomeConfigId = string.Empty;
                string sLearningOutcomesObservationId = string.Empty;
                foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
                {
                    foreach (Control oControl in oHtmlTableCell.Controls)
                    {
                        if (oControl is TextBox && oControl.ID.Contains("txtLearningObservation_"))
                        {
                            bIsDisabled = ((TextBox)oControl).Enabled;
                            sLearningOutcomeObservation = ((TextBox)oControl).Text;
                            oXmlNode = AddNodeToXML(oDoc, oXmlNode, sLearningOutcomeGradeId, sLearningOutcomeConfigId, sSubjectSectionConfigurationId, sLearningOutcomesObservationId, sLearningOutcomeObservation, sGradeId);
                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                        else if (oControl is DropDownList && oControl.ID.Contains("ddlLearningGrade_"))
                        {
                            string sCntrlId = oControl.ID;
                            char cSplit = Convert.ToChar("_");
                            string[] sIds = sCntrlId.Split(cSplit);
                            sSubjectSectionConfigurationId = sIds[1];
                            if (sSubjectSectionId.IsNullOrEmpty() || sSubjectSectionId != sSubjectSectionConfigurationId)
                            {
                                sSubjectSectionId = sSubjectSectionConfigurationId;
                                sLearningOutcomeObservation = string.Empty;
                                bIsDisabled = true;
                            }

                            sLearningOutcomeConfigId = sIds[2];
                            sLearningOutcomeGradeId = sIds[3];
                            sLearningOutcomesObservationId = sIds[4];
                            sGradeId = ((DropDownList)oControl).SelectedValue;
                            //if (!sLearningOutcomeObservation.IsNullOrEmpty() || !bIsDisabled)
                            //if (!bIsDisabled)
                            //{
                            oXmlNode = AddNodeToXML(oDoc, oXmlNode, sLearningOutcomeGradeId, sLearningOutcomeConfigId, sSubjectSectionConfigurationId, sLearningOutcomesObservationId, sLearningOutcomeObservation, sGradeId);
                            oXmlRootNode.AppendChild(oXmlNode);
                            //}
                        }
                    }
                }
            }

        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }

    /// <summary>
    /// This method is used to Create XML Node
    /// </summary>
    /// <param name="aoDoc"></param>
    /// <param name="aoXmlNode"></param>
    /// <param name="asLearningOutcomeGradeId"></param>
    /// <param name="asLearningOutcomeConfigId"></param>
    /// <param name="asSubjectSectionConfigurationId"></param>
    /// <param name="asLearningOutcomesObservationId"></param>
    /// <param name="asObservation"></param>
    /// <param name="asGradeId"></param>
    /// <returns></returns>
    private XmlNode AddNodeToXML(XmlDocument aoDoc, XmlNode aoXmlNode, string asLearningOutcomeGradeId, string asLearningOutcomeConfigId, string asSubjectSectionConfigurationId, string asLearningOutcomesObservationId, string asObservation, string asGradeId)
    {
        XmlAttribute oAttr = aoDoc.CreateAttribute("LearningOutcomeGradeId");
        oAttr.Value = asLearningOutcomeGradeId;
        aoXmlNode.Attributes.Append(oAttr);

        oAttr = aoDoc.CreateAttribute("LearningOutcomeConfigId");
        oAttr.Value = asLearningOutcomeConfigId;
        aoXmlNode.Attributes.Append(oAttr);

        oAttr = aoDoc.CreateAttribute("SubjectSectionConfigurationId");
        oAttr.Value = asSubjectSectionConfigurationId;
        aoXmlNode.Attributes.Append(oAttr);

        oAttr = aoDoc.CreateAttribute("LearningOutcomesObservationId");
        oAttr.Value = asLearningOutcomesObservationId;
        aoXmlNode.Attributes.Append(oAttr);

        oAttr = aoDoc.CreateAttribute(S_OBSERVATION);
        oAttr.Value = asObservation;
        aoXmlNode.Attributes.Append(oAttr);

        oAttr = aoDoc.CreateAttribute(S_GRADE_ID);
        oAttr.Value = asGradeId;
        aoXmlNode.Attributes.Append(oAttr);

        return aoXmlNode;
    }

    /// <summary>
    /// This method is used to get Xseed grades XML.
    /// </summary>
    /// <returns></returns>
    private string GetXseedGradesXml()
    {
        HtmlTable oHtmlTable = (HtmlTable)tblMainProgressReport.FindControl("tblCoCurricularSubjects");
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement(S_NON_XSEED_SUBJECT_GRADES);
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, S_NON_XSEED_SUBJECT_GRADES, string.Empty);
        if (oHtmlTable != null)
            foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_NON_XSEED_SUBJECT_GRADES, string.Empty);
                string sGradeId = string.Empty;
                foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
                {
                    foreach (Control oControl in oHtmlTableCell.Controls)
                    {
                        if (oControl is TextBox && oControl.ID.Contains(S_TXT_OBSERVATION))
                        {
                            string sCntrlId = oControl.ID;
                            char cSplit = Convert.ToChar("_");
                            string[] sIds = sCntrlId.Split(cSplit);

                            XmlAttribute oAttr = oDoc.CreateAttribute(S_SUBJECT_ID);
                            oAttr.Value = sIds[1];
                            oXmlNode.Attributes.Append(oAttr);

                            oAttr = oDoc.CreateAttribute(S_OBSERVATION);
                            oAttr.Value = ((TextBox)oControl).Text;
                            oXmlNode.Attributes.Append(oAttr);

                            oAttr = oDoc.CreateAttribute(S_GRADE_ID);
                            oAttr.Value = sGradeId;
                            oXmlNode.Attributes.Append(oAttr);

                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                        else if (oControl is DropDownList && oControl.ID.Contains(S_DDL_COCURRICULAR_SUBJECTS_GRADE))
                            sGradeId = ((DropDownList)oControl).SelectedValue;
                        else if (oControl is DropDownList && oControl.ID.Contains(S_DDL_NON_XSEED_SUBJECTS_GRADE))
                            sGradeId = ((DropDownList)oControl).SelectedValue;
                    }
                }
            }

        oHtmlTable = (HtmlTable)tblMainProgressReport.FindControl("tblNonXseedProgressReport");

        if (oHtmlTable != null)
            foreach (HtmlTableRow oHtmlTableRow in oHtmlTable.Rows)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, S_NON_XSEED_SUBJECT_GRADES, string.Empty);
                string sGradeId = string.Empty;
                foreach (HtmlTableCell oHtmlTableCell in oHtmlTableRow.Cells)
                {
                    foreach (Control oControl in oHtmlTableCell.Controls)
                    {
                        if (oControl is TextBox && oControl.ID.Contains(S_TXT_OBSERVATION))
                        {
                            string sCntrlId = oControl.ID;
                            char cSplit = Convert.ToChar("_");
                            string[] sIds = sCntrlId.Split(cSplit);

                            XmlAttribute oAttr = oDoc.CreateAttribute(S_SUBJECT_ID);
                            oAttr.Value = sIds[1];
                            oXmlNode.Attributes.Append(oAttr);

                            oAttr = oDoc.CreateAttribute(S_OBSERVATION);
                            oAttr.Value = ((TextBox)oControl).Text;
                            oXmlNode.Attributes.Append(oAttr);

                            oAttr = oDoc.CreateAttribute(S_GRADE_ID);
                            oAttr.Value = sGradeId;
                            oXmlNode.Attributes.Append(oAttr);

                            oXmlRootNode.AppendChild(oXmlNode);

                            oXmlRootNode.AppendChild(oXmlNode);
                        }
                        else if (oControl is DropDownList && oControl.ID.Contains(S_DDL_COCURRICULAR_SUBJECTS_GRADE))
                            sGradeId = ((DropDownList)oControl).SelectedValue;
                        else if (oControl is DropDownList && oControl.ID.Contains(S_DDL_NON_XSEED_SUBJECTS_GRADE))
                            sGradeId = ((DropDownList)oControl).SelectedValue;
                    }
                }
            }

        oRoot.AppendChild(oXmlRootNode);
        return oRoot.InnerXml;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidObservationShouldNotBeGreaterThanCharacters.Value = Resources.LocalizedResources.ObservationShouldNotBeGreaterThanCharacters;
        hidValPublishResult.Value = Resources.LocalizedResources.ValPublishResult;
        hidGradeShouldBeSelectedForCoCurricularSubject.Value = Resources.LocalizedResources.GradeShouldBeSelectedForCoCurricularSubject;
        hidValRecentlyAddedData.Value = Resources.LocalizedResources.ValRecentlyAddedData;
        hidGradeShouldBeSelectedForLearningOutcome.Value = Resources.LocalizedResources.GradeShouldBeSelectedForLearningOutcome;
        hidGradeShouldBeSelectedForNonXseedSubject.Value = Resources.LocalizedResources.GradeShouldBeSelectedForNonXseedSubject;
        hidViewProgressReport.Value = Resources.LocalizedResources.ViewProgressReport;
        HidSave.Value = Resources.LocalizedResources.Save;
        HidUnpublish.Value = Resources.LocalizedResources.Unpublish;
        HidPublish.Value = Resources.LocalizedResources.Publish;
        //btnPublish.Text=oResourceManager.GetString(HidbtnPublishText.Value.Replace(" ",string.Empty));
        hidViewProgressReport.Value = Resources.LocalizedResources.ViewProgressReport;
        hidValRecentlyAddedData.Value = Resources.LocalizedResources.ValRecentlyAddedData;
        valSummary.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSummary1.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to set remark to textbox.
    /// </summary>
    /// <returns></returns>
    private string GetRemark()
    {
        HtmlTable tblRemark = (HtmlTable)tblMainProgressReport.FindControl("tblRemark");
        string sRemark = string.Empty;

        if (tblRemark != null)
        {           
           TextBox txtremark = tblRemark.Rows[0].Cells[1].Controls[0] as TextBox;
           if (txtremark != null)
               sRemark = txtremark.Text.Trim();
        }

        return sRemark;
    }

    #endregion
}


