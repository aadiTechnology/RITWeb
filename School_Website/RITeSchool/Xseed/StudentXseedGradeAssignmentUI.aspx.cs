/*
 * File Name - StudentXseedGradeAssignmentUI.aspx.xs
 * Creadted By - Sachin
 * Created Date - 31-May-2011
 * Description - This class is used to set grades for learning outcomes.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using XseedReportEntities;

/// <summary>
/// This class is used to set grades for learning outcomes.
/// </summary>
public partial class StudentXseedGradeAssignmentUI : SchoolBase
{
    #region Data Members

    StudentXseedGradeAssignmentBL moStudentXseedGradeAssignmentBL;

    #endregion

    #region Constants

    const string S_XSEED_GRADES = "XseedGrades";

    #endregion

    #region Events

    /// <summary>
    /// this event is used to fill student and subject section combobox.
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

                RefreshValue();
                ReadQuerystring();
                FillStudentAndSubjectCombo();
                SetJavascriptAttributes();                
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
            if (miSchoolId == Constants.SchoolId.PPS.ToInt())
            {
                MasterPage oMasterPage = (MasterPage)this.Master;
                SiteMapPath siteMap = (SiteMapPath)oMasterPage.FindControl("SiteMapPath1");
                oMasterPage.NodeTitle = "Pre-Primary Subject Grades";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill learning outcomes into listview according to seleced student and subject section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSubjectSections_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetLearningOutcomes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill learning outcomes into listview according to seleced student and subject section.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            SetLearningOutcomes();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill grade comboboxes of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLearningOutcome_ItemDataBound(object sender, ListViewItemEventArgs e)
    { 
        ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
        LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = oCurrentItem.DataItem as LearningOutcomeConfigMaster;
        DropDownList cmbGrades = oCurrentItem.FindControl("cmbGrades") as DropDownList;     
        FillGradeCombobox(cmbGrades);
        cmbGrades.SelectedValue = oLearningOutcomeConfigMaster.GradeId.ToString();
    }

    /// <summary>
    /// This event is used to save grades respective to learning outcomes and student.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moStudentXseedGradeAssignmentBL = new StudentXseedGradeAssignmentBL
            {
                LearningOutcomesGradeDetails = new LearningOutcomesGrade
                {
                    SchoolId = miSchoolId,
                    AcademicYearId = miAcademicYearId,
                    InsertedById = miUserId,
                    YearwiseStudentId = Convert.ToInt32(cmbStudent.SelectedValue),
                    LearningOutcomeXml = GetLearningOutcomeXML()
                },
                LearningOutcomesObservation = new LearningOutcomesObservation
                {
                    LearningOutcomesObservationId = Convert.ToInt32(hidLearningOutcomesObservationId.Value),
                    SubjectSectionConfigurationId = Convert.ToInt32(cmbSubjectSections.SelectedValue),
                    Observation = txtObservation.Text.Trim(),
                    AssessmentId = Convert.ToInt32(hidAssessmentId.Value),
                    SubjectRemark = txtSubjectRemark.Text.Trim()
                }
            };
            moStudentXseedGradeAssignmentBL.Save(hidSubjectId.Value.ToInt());
            lblMessage.Text = Resources.LocalizedResources.ValGradesSavedSuccessfully;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    } 

    #endregion

    #region Methods

    /// <summary>
    /// This method is used to set learning outcomes.
    /// </summary>
    private void SetLearningOutcomes()
    {
        if (cmbStudent.SelectedValue != "0" && cmbSubjectSections.SelectedValue != "0")
            FillLearningOutcomes();
        else
            ResetFields();
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavascriptAttributes()
    {
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        ApplyMouseHoverEffect(new List<Button> {btnBack, btnSave});
        if (hidFrom.Value != "0")
            btnBack.PostBackUrl = "../Xseed/ClassTeacherXseedGradesUI.aspx?" + CommonUtility.EncryptQuerystring("AssessmentId=" + hidAssessmentId.Value + "&TeacherId=" + hidTeacherId.Value);
        else
            btnBack.PostBackUrl = "../Xseed/AssignXseedGradesUI.aspx?" + CommonUtility.EncryptQuerystring("AssessmentId=" + hidAssessmentId.Value + "&TeacherId=" + hidTeacherId.Value);

        if (moSchool == Constants.SchoolId.PPS)
            hidRemarkLength.Value = "400";
        else
            hidRemarkLength.Value = "300";
    }

    /// <summary>
    /// This method is used todecrypt query string.
    /// </summary>
    private void ReadQuerystring()
    {
        hidStdDivId.Value = QueryString["StandardDivisionId"];
        hidAssessmentId.Value = QueryString["AssessmentId"];
        hidSubjectId.Value = QueryString["SubjectId"];
        hidTeacherId.Value = QueryString["TeacherId"];
        hidIsReadOnly.Value = QueryString["IsReadOnly"];
        hidFrom.Value = QueryString["From"] ?? Constants.S_ZERO;        
        if (hidIsReadOnly.Value == "Y" || hidIsReadOnly.Value == "3")
        {
            pnlSubmitStatus.Visible = true;
            if (hidFrom.Value == "0")
                lblSubmitMessage.Visible = true;
            else
            {
                lblSubmitMessage.Visible = true;
                lblSubmitMessage.Text = Resources.LocalizedResources.ValResultsPubliched;
            }
        }
        else
        {
            pnlSubmitStatus.Visible = false;
            btnSave.Visible = false;
            lblSubmitMessage.Visible = false;
        }
    }
     

    /// <summary>
    /// This method is used to fill student and subject section combobox.
    /// </summary>
    private void FillStudentAndSubjectCombo()
    {
        moStudentXseedGradeAssignmentBL = new StudentXseedGradeAssignmentBL
        {
            GradeSubmitStatus = new GradeSubmitStatus
            {
                StandardDivisionId = Convert.ToInt32(hidStdDivId.Value),
                AssessmentId = Convert.ToInt32(hidAssessmentId.Value),
                SubjectId = Convert.ToInt32(hidSubjectId.Value),
                SchoolId = miSchoolId,
                AcademicYearId = miAcademicYearId
            }
        };
        moStudentXseedGradeAssignmentBL.GetStudentsForStdDiv();
        moStudentXseedGradeAssignmentBL.FillStudentAndSubjectComboboxes(cmbStudent, cmbSubjectSections);
        SetBasicDetails();
        ViewState[S_XSEED_GRADES] = moStudentXseedGradeAssignmentBL.GradeDetailsList;
        List<GradeMaster> lstGradeMaster = moStudentXseedGradeAssignmentBL.GradeDetailsList;
        ViewState[S_XSEED_GRADES] = lstGradeMaster;
        hidIsAbsent.Value = lstGradeMaster.Where(grade => grade.ConsideredAsAbsent).Select(grade => grade.GradeId).FirstOrDefault().ToString();
        hidIsExempted.Value = lstGradeMaster.Where(grade => grade.ConsideredAsExempted).Select(grade => grade.GradeId).FirstOrDefault().ToString();
    }

    /// <summary>
    /// This method is used to set basic details.
    /// </summary>
    private void SetBasicDetails()
    {
        lblClass.Text = moStudentXseedGradeAssignmentBL.ClassName;
        lblAssessment.Text = moStudentXseedGradeAssignmentBL.AssessmentDetails.Name;
        lblSubject.Text = moStudentXseedGradeAssignmentBL.SubjectName;
    }

    /// <summary>
    /// This method is used to fill learning outcomes into listview.
    /// </summary>
    private void FillLearningOutcomes()
    {
        StudentXseedGradeAssignmentBL oStudentXseedGradeAssignmentBL = new StudentXseedGradeAssignmentBL
         {
             LearningOutcomesObservation = new LearningOutcomesObservation
             {
                 SchoolId = miSchoolId,
                 AcademicYearId = miAcademicYearId,
                 AssessmentId = Convert.ToInt32(hidAssessmentId.Value),
                 SubjectSectionConfigurationId = Convert.ToInt32(cmbSubjectSections.SelectedValue),
                 YearwiseStudentId = Convert.ToInt32(cmbStudent.SelectedValue)
             }
         };

        lstvwLearningOutcome.DataSource = oStudentXseedGradeAssignmentBL.GetLearningOutcomesForStdDiv(hidSubjectId.Value.ToInt());
        lstvwLearningOutcome.DataBind();
        FillHeaderCombobox();
        if (oStudentXseedGradeAssignmentBL.LearningOutcomesObservation != null)
        {
            txtObservation.Text = oStudentXseedGradeAssignmentBL.LearningOutcomesObservation.Observation;

            if (oStudentXseedGradeAssignmentBL.LearningOutcomesObservation.ShowSubjectRemark)
            {
                trSubjectRemark.Visible = true;
                txtSubjectRemark.Text = oStudentXseedGradeAssignmentBL.LearningOutcomesObservation.SubjectRemark;
            }
            else
            {
                txtSubjectRemark.Text = string.Empty;
                trSubjectRemark.Visible = false;
            }

            hidLearningOutcomesObservationId.Value = oStudentXseedGradeAssignmentBL.LearningOutcomesObservation.LearningOutcomesObservationId.ToString();
        }
        HideFields(lstvwLearningOutcome.Items.Count > 0);
        MakeListReadOnly();
        CheckExamPublishStatus(oStudentXseedGradeAssignmentBL.IsExamPublished);
    }

    /// <summary>
    /// This method is used to check exam publish status.
    /// </summary>
    /// <param name="abIsExamPublished"></param>
    private void CheckExamPublishStatus(bool abIsExamPublished)
    {
        if (abIsExamPublished)
        {
            lstvwLearningOutcome.Enabled = false;
            btnSave.Visible = false;
            txtObservation.Enabled = false;
            txtSubjectRemark.Enabled = false;
        }
    }

    private void MakeListReadOnly()
    {
        if (hidIsReadOnly.Value == "Y" || hidIsReadOnly.Value=="3")
        {
            lstvwLearningOutcome.Enabled = false;
            btnSave.Visible = false;
            txtObservation.Enabled=false;
            txtSubjectRemark.Enabled = false;
        }
        else
        {
            txtObservation.Enabled = true;
            txtSubjectRemark.Enabled = true;
            lstvwLearningOutcome.Enabled = true;
            btnSave.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to hide fields according to listview item count.
    /// </summary>
    /// <param name="abAreItemExists"></param>
    private void HideFields(bool abAreItemExists)
    {
        if(miSchoolId == Constants.SchoolId.PPS.ToInt())
            tblObservation.Visible = false;
        else
            tblObservation.Visible = abAreItemExists;

        divContainer.Visible = abAreItemExists;
        btnSave.Visible = abAreItemExists;
        trNoRecordFoundMessage.Visible = !abAreItemExists;
    }

    /// <summary>
    /// This method is used to reset fields.
    /// </summary>
    private void ResetFields()
    {
        lstvwLearningOutcome.DataSource = null;
        lstvwLearningOutcome.DataBind();
        tblObservation.Visible = false;
        divContainer.Visible = false;
        btnSave.Visible = false;
        trNoRecordFoundMessage.Visible = false;
    }

    /// <summary>
    /// This method is used to fill listview header combobox.
    /// </summary>
    /// <param name="lstXseedGrades"></param>
    private void FillHeaderCombobox()
    {
        HtmlTableRow oHtmlTableRow = (HtmlTableRow)lstvwLearningOutcome.FindControl("trHeaderContol");
        if (oHtmlTableRow != null)
        {
            DropDownList cmbAllGrades = (DropDownList)oHtmlTableRow.FindControl("cmbAllGrades");
            FillGradeCombobox(cmbAllGrades);
            cmbAllGrades.Attributes.Add("onclick", "SelectAll(this)");
        }
    }

    /// <summary>
    /// This method is used to fill grade combobox.
    /// </summary>
    /// <param name="cmbGrades"></param>
    private void FillGradeCombobox(DropDownList cmbGrades)
    {
        List<GradeMaster> lstXseedGrades = new List<GradeMaster>();
        if (ViewState[S_XSEED_GRADES] != null)
            lstXseedGrades = ViewState[S_XSEED_GRADES] as List<GradeMaster>;

        cmbGrades.DataSource = lstXseedGrades;
        cmbGrades.DataTextField = "GradeName";
        cmbGrades.DataValueField = "GradeId";
        cmbGrades.DataBind();
        cmbGrades.Items.Insert(0, new ListItem("-- Select --", "0"));
    }

    /// <summary>
    /// This method is used to return learning outcome XML;
    /// </summary>
    /// <returns>Learning outcome Xml</returns>
    private string GetLearningOutcomeXML()
    {
        const string S_ELEMENT = "element";
        XmlDocument oDoc = new XmlDocument();
        XmlElement oRoot = oDoc.CreateElement("LearningOutcomes");
        XmlNode oXmlRootNode = oDoc.CreateNode(S_ELEMENT, "LearningOutcomes", "");

        foreach (ListViewDataItem oCurrentItem in lstvwLearningOutcome.Items)
        {
            if (oCurrentItem.ItemType == ListViewItemType.DataItem)
            {
                XmlNode oXmlNode = oDoc.CreateNode(S_ELEMENT, "LearningOutcomes", "");

                XmlAttribute attr = oDoc.CreateAttribute("LearningOutcomeGradeId");
                attr.Value = Convert.ToString(lstvwLearningOutcome.DataKeys[oCurrentItem.DisplayIndex]["LearningOutcomeGradeId"]);
                oXmlNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("LearningOutcomeConfigId");
                attr.Value = Convert.ToString(lstvwLearningOutcome.DataKeys[oCurrentItem.DisplayIndex]["LearningOutcomeConfigId"]);
                oXmlNode.Attributes.Append(attr);

                attr = oDoc.CreateAttribute("GradeId");
                attr.Value = ((DropDownList)oCurrentItem.FindControl("cmbGrades")).SelectedValue;
                oXmlNode.Attributes.Append(attr);

                oXmlRootNode.AppendChild(oXmlNode);
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
        HidObservationLengthShouldBeLess.Value = Resources.LocalizedResources.ObservationLengthShouldBeLess;
        HidObservationShouldNotBeBlank.Value = Resources.LocalizedResources.ObservationShouldNotBeBlank;
        hidSchoolId.Value = miSchoolId.ToString();
        valSum.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }
    #endregion
}