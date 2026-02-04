/* File Name = LearnigOutcomeConfigurationUI.aspx.cs
 * Created Date - 
 * Modified Date  - 24 May 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage larning outcome details.*/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;
using XseedReportEntities;
using System.Resources;

public partial class LearnigOutcomeConfigurationUI : SchoolBase
{
    #region "Constants"

    const string S_DEFAULT_SORT_EXP = "LearningOutcome";
    const string S_COMMAND_REMOVE = "RemoveLearningOutcome";
    const string S_COMMAND_UPDATE = "UpdateLearningOutcome";
    const string S_LEARNING_OUTCOME_CONFIG_ID = "LearningOutcomeConfigId";
    const string S_LEARNING_OUTCOME = "LearningOutcome";

    #endregion "Constants"

    #region "Data Member"

    LearningOutcomeConfigMasterBL moLearningOutcomeConfigMasterBL;

    private ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

    #endregion

    #region "Events"

    /// <summary>
    /// This event is used to set default controls and fill comboboxes details.
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
                hidSubmitText.Value = "Submit";
                SetJavaScriptAttributes();
                if (CheckPreCondition())
                {
                    FillStandardComboBox(cmbStandards);
                    AddDefaultFliterComboBoxItems();
                    SetDefaultFieldValues();
                    HideSubmitCopyControls(false);
                }
                RefreshValues();
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValues();
            }
        }
        catch(Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save learning outcomes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
            moLearningOutcomeConfigMasterBL.LearningOutcomeConfigMaster = PopulateLearningOutcomeConfigMaster();
            lblErrorMsg.Text = string.Empty;
            if (hidMode.Value != Constants.Action.Update.ToString())
            {
                moLearningOutcomeConfigMasterBL.Insert();
                lblUpdateSucess.Text = Resources.LocalizedResources.MsgLearningOutcomeSuccess;
            }
            else
            {
                moLearningOutcomeConfigMasterBL.Update();
                lblUpdateSucess.Text = Resources.LocalizedResources.MsgLearningOutcomeUpdate;
            }
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (!bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LearningOutcomeConfiguration));
            FillLearningOutcomeDetails();
            SetDefaultFieldValues();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit learning outcomes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
            moLearningOutcomeConfigMasterBL.LearningOutcomesSubmitStatus = PopulateLearningOutcomesSubmitStatus();
            if (btnSubmit.Text != Resources.LocalizedResources.UnSubmit)
            {
                moLearningOutcomeConfigMasterBL.LearningOutcomesSubmitStatus.IsSubmitted = true;
                moLearningOutcomeConfigMasterBL.SaveLearningOutcomesSubmitStatus();
                hidSubmitText.Value = "Un Submit";
                btnSubmit.Text = Resources.LocalizedResources.UnSubmit;
                lblUpdateSucess.Text = Resources.LocalizedResources.MsgLearningOutcome + " '" + cmbSubjects.SelectedItem.Text + "' " + Resources.LocalizedResources.SubmittedSuccessfully;
                tblCopy.Visible = false;
            }
            else
            {
                moLearningOutcomeConfigMasterBL.LearningOutcomesSubmitStatus.IsSubmitted = false;
                moLearningOutcomeConfigMasterBL.SaveLearningOutcomesSubmitStatus();
                hidSubmitText.Value = "Submit";
                btnSubmit.Text = Resources.LocalizedResources.Submit;
                hidIsSubmitted.Value = "false";
                tblCopy.Visible = true;
            }
            FillLearningOutcomeDetails();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to copy learning outcomes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        try
        {
            moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
            LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
            oLearningOutcomeConfigMaster.SchoolId = miSchoolId;
            oLearningOutcomeConfigMaster.AcademicYearId = miAcademicYearId;
            oLearningOutcomeConfigMaster.InsertedById = miUserId;
            oLearningOutcomeConfigMaster.StandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
            oLearningOutcomeConfigMaster.SubjectSectionConfigId = Convert.ToInt32(cmbSubjectSection.SelectedValue);
            moLearningOutcomeConfigMasterBL.LearningOutcomeConfigMaster = oLearningOutcomeConfigMaster;
            moLearningOutcomeConfigMasterBL.Copy(Convert.ToInt32(cmbCopyAssessment.SelectedValue), Convert.ToInt32(cmbCopySubjectSection.SelectedValue));
            lblUpdateSucess.Text = Resources.LocalizedResources.MsgLearningOutcomeCopy;
            lblErrorMsg.Visible = false;
            cmbCopySubjects.SelectedIndex = 0;
            cmbCopySubjectSection.SelectedIndex = 0;
            cmbCopyStandards.SelectedIndex = 0;
            cmbCopyAssessment.SelectedIndex = 0;
            cmbStandards.Focus();
            AddSortImage();
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = string.Empty;
            lblErrorMsg.Text = ex.Message.Replace("Configuration can not be copied since grades are already assigned to students.",Resources.LocalizedResources.ValLearningOutcome1);
            AddSortImage();
            cmbStandards.Focus();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This method is used to set default controls and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SetDefaultFieldValues();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill assessment and standard comboboxes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!cmbStandards.SelectedValue.Equals("0"))
        {
            FillSubjectComboBox(cmbSubjects);
            FillAssessmentComboBox(cmbAssessment);
        }
        else
        {
            btnSave.Enabled = true;
            AddDefaultComboBoxItem(cmbSubjects);
            AddDefaultComboBoxItem(cmbAssessment);
        }
        HideSubmitCopyControls(false);
        AddDefaultComboBoxItem(cmbSubjectSection);
        SetDefaultFieldValues();
    }

    /// <summary>
    /// This event is used to fill learning outcome details and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbAssessment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbAssessment.SelectedValue != Constants.I_ZERO.ToString() && cmbSubjectSection.SelectedValue != Constants.I_ZERO.ToString())
            {
                FillLearningOutcomeDetails();
                FillDefaultComboBoxes();
                AddSortImage();
                SetDefaultFieldValues();

            }
            else
            {
                btnSave.Enabled = true;
                HideSubmitCopyControls(false);
                SetDefaultFieldValues();
                btnSave.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill subject section combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!cmbSubjects.SelectedValue.Equals("0"))
        {
            FillSubjectSectionComboBox(cmbSubjectSection);
            HideSubmitCopyControls(false);
            SetDefaultFieldValues();
        }
        else
        {
            btnSave.Enabled = true;
            AddDefaultComboBoxItem(cmbSubjectSection);
            SetDefaultFieldValues();
        }
    }

    /// <summary>
    /// This event is used to fill learning outcome and add sort image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSubjectSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbAssessment.SelectedValue != Constants.I_ZERO.ToString() && cmbSubjectSection.SelectedValue != Constants.I_ZERO.ToString())
            {
                FillLearningOutcomeDetails();
                FillDefaultComboBoxes();
                AddSortImage();
                SetDefaultFieldValues();
            }
            else
            {
                btnSave.Enabled = true;
                HideSubmitCopyControls(false);
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used fill copy to assessments.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbCopyStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!cmbStandards.SelectedValue.Equals("0"))
            {
                FillSubjectComboBox(cmbCopySubjects);
                FillAssessmentComboBox(cmbCopyAssessment);
            }
            else
            {
                AddDefaultComboBoxItem(cmbCopySubjects);
                AddDefaultComboBoxItem(cmbCopyAssessment);
            }
            AddSortImage();
            AddDefaultComboBoxItem(cmbCopySubjectSection);
            SetDefaultFieldValues();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill subject section combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbCopySubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!cmbSubjects.SelectedValue.Equals("0"))
                FillSubjectSectionComboBox(cmbCopySubjectSection);
            else
                AddDefaultComboBoxItem(cmbCopySubjectSection);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set sorting variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLearningOutcomeDetails_Sorting(object sender, ListViewSortEventArgs e)
    {
        try
        {
            hidSortDirection.Value = (hidSortDirection.Value == Constants.S_DESCENDING) ? Constants.S_ASCENDING
                                                : Constants.S_DESCENDING;
            FillLearningOutcomeDetails();
            AddSortImage();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set list view total records count.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLearningOutcomeDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwLearningOutcomeDetails.Items.Count.ToString();

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default list view controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLearningOutcomeDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                ImageButton oimgbtnDelete = oCurrentItem.FindControl("imgBtnDelete") as ImageButton;
                ImageButton oimgBtnEdit = oCurrentItem.FindControl("imgBtnEdit") as ImageButton;
                Image oimgIsConsidered = oCurrentItem.FindControl("imgIsConsidered") as Image;
                Label lblSrNo = oCurrentItem.FindControl("lblRowNo") as Label;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete('" + Resources.LocalizedResources.AreYouSureYouWantToDeleteThisRecords + "')) {return false;}");
                oimgIsConsidered.Visible = Convert.ToBoolean(lstvwLearningOutcomeDetails.DataKeys[oCurrentItem.DisplayIndex]["IsConsidered"]);
                lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();
                if (Convert.ToBoolean(hidIsSubmitted.Value))
                {
                    oimgbtnDelete.Visible = false;
                    oimgBtnEdit.Visible = false;
                }
                else
                {
                    oimgbtnDelete.Visible = true;
                    oimgBtnEdit.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to edit or delete learning outcome details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwLearningOutcomeDetails_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName != "Sort")
            {
                lblErrorMsg.Text = string.Empty;
                lblUpdateSucess.Text = string.Empty;
                ListViewDataItem oCurrentItem = (ListViewDataItem)e.Item;
                int iListIndex = oCurrentItem.DisplayIndex;
                hidRowNo.Value = ((Label)oCurrentItem.FindControl("lblRowNo")).Text;
                hidLearningOutcomeConfigId.Value = lstvwLearningOutcomeDetails.DataKeys[iListIndex][S_LEARNING_OUTCOME_CONFIG_ID].ToString();
                if (e.CommandName == S_COMMAND_REMOVE)
                {
                    SetDefaultFieldValues();
                    if (!LearningOutcomeConfigMasterBL.Dependent(Convert.ToInt32(hidLearningOutcomeConfigId.Value),miSchoolId, miAcademicYearId))
                        DeleteLearningOutcomeDetails();
                    else
                    {
                        lblErrorMsg.Visible = true;
                        lblErrorMsg.Text = Resources.LocalizedResources.ValLearningOutcomeGrade;
                    }
                }
                else if (e.CommandName == S_COMMAND_UPDATE)
                    LoadLearningOutcomeDetails();
                FillLearningOutcomeDetails();
                AddSortImage();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.LearningOutcomeConfiguration);

        if (sLinks.Equals(string.Empty))
        {
            divErr.Visible = false;
            bReturn = true;
        }
        else
        {
            divErr.InnerHtml = sLinks;
            VisibleOrHideControls();
        }
        return bReturn;
    }

    /// <summary>
    /// This method is used to hide submit copy controls.
    /// </summary>
    private void HideSubmitCopyControls(bool abStatus)
    {
        if (!abStatus)
        {
            lstvwLearningOutcomeDetails.DataSource = null;
            lstvwLearningOutcomeDetails.DataBind();
            hidRowCount.Value = "0";
        }
        divLearningOutcomeDetails.Visible = abStatus;
        tblCopy.Visible = abStatus;
        btnSubmit.Visible = abStatus;
    }

    /// <summary>
    /// This method is used to set visible or hide properties of controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tblLearningOutcome.Visible = false;
        HideSubmitCopyControls(false);
    }

    /// <summary>
    /// This method is used to set javascript atteibutes.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumErrorMsg2.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave,btnBack,btnSubmit,btnCopy});
        btnCopy.Attributes.Add("onclick", "if(!ConfirmCopy()) {return false;}");
        btnSubmit.Attributes.Add("onclick", "if(!ConfirmSubmit()) {return false;}");
    }

    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>
    /// <param name="cmbStandard"></param>
    private void FillStandardComboBox(DropDownList cmbStandard)
    {
        Constants.UserRoles oUserRole = moUserRole;
        List<StandardMaster> lstStandardMaster = new List<StandardMaster>();
        if (moUserRole == Constants.UserRoles.Admin || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration) == Constants.C_YES)
            lstStandardMaster = StandardMasterBL.GetStandardsAssociatedToAssessments(miSchoolId, miAcademicYearId, 0);
        else if (oUserRole == Constants.UserRoles.Teacher)
        {
            int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            lstStandardMaster = StandardMasterBL.GetStandardsAssociatedToAssessments(miSchoolId, miAcademicYearId, iTeacherId);
        }
        cmbStandard.DataSource = lstStandardMaster;
        cmbStandard.DataTextField = "StandardName";
        cmbStandard.DataValueField = "StandardId";
        cmbStandard.DataBind();
        cmbStandard.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill default comboboxes.
    /// </summary>
    private void FillDefaultComboBoxes()
    {
        FillStandardComboBox(cmbCopyStandards);
        AddCopyDefaultComboBoxItems();
    }

    /// <summary>
    /// This method is used to fill subject combobox.
    /// </summary>
    private void FillSubjectComboBox(DropDownList cmbSubject)
    {
        
        int iStandardId = cmbSubject.ID == cmbCopySubjects.ID ? Convert.ToInt32(cmbCopyStandards.SelectedValue) : Convert.ToInt32(cmbStandards.SelectedValue);
        if (moUserRole == Constants.UserRoles.Admin || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration) == Constants.C_YES)
        {
            cmbSubject.DataSource = TeacherSubjectDetailsBL.GetTeacherAssociatedSubjects(Constants.I_ZERO, iStandardId, miAcademicYearId, miSchoolId, true);
        }
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            cmbSubject.DataSource = TeacherSubjectDetailsBL.GetTeacherAssociatedSubjects(iTeacherId, iStandardId, miAcademicYearId, miSchoolId, true);
        }
        cmbSubject.DataTextField = "SubjectName";
        cmbSubject.DataValueField = "StandardwiseSubjectId";
        cmbSubject.DataBind();
        cmbSubject.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill assessment combobox.
    /// </summary>
    /// <param name="cmbAssessments"></param>
    private void FillAssessmentComboBox(DropDownList cmbAssessments)
    {
        StandardwiseAssessmentMasterBL oStandardwiseAssessmentMasterBL = new StandardwiseAssessmentMasterBL();
        int iStandardId = cmbAssessments.ID == cmbCopyAssessment.ID ? Convert.ToInt32(cmbCopyStandards.SelectedValue) : Convert.ToInt32(cmbStandards.SelectedValue);
        cmbAssessments.DataSource = oStandardwiseAssessmentMasterBL.GetStandardwiseAssementDetailsList(iStandardId);
        ListItem oSelect = new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString());
        cmbAssessments.DataValueField = "StandardwiseAssessmentId";
        cmbAssessments.DataTextField = "AssessmentName";
        cmbAssessments.DataBind();
        cmbAssessments.Items.Insert(Constants.I_ZERO, oSelect);


    }

    /// <summary>
    /// This method is used to set default combo box item.
    /// </summary>
    /// <param name="cmb"></param>
    private void AddDefaultComboBoxItem(DropDownList cmb)
    {
        cmb.Items.Clear();
        cmb.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill subject section combobox.
    /// </summary>
    private void FillSubjectSectionComboBox(DropDownList cmbSubjectSection)
    {
        int iSubjectId = cmbSubjectSection.ID == cmbCopySubjectSection.ID ? Convert.ToInt32(cmbCopySubjects.SelectedValue) : Convert.ToInt32(cmbSubjects.SelectedValue);
        SubjectSectionConfigurationMasterBL oSubjectSectionConfigurationMasterBL = new SubjectSectionConfigurationMasterBL();
        cmbSubjectSection.DataSource = oSubjectSectionConfigurationMasterBL.GetSubjectwiseSubjectSectionList(iSubjectId);
        cmbSubjectSection.DataValueField = "SubjectSectionConfigurationId";
        cmbSubjectSection.DataTextField = "SubjectSectionName";
        cmbSubjectSection.DataBind();
        cmbSubjectSection.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used delete config details.
    /// </summary>
    private void DeleteConfigDetails()
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();
        oConfiguration.OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.AssessmentConfiguration);
        oConfiguration.SchoolId = miSchoolId;
        oConfiguration.AcademicYearId = miAcademicYearId;
        oConfiguration.IsConfigure = Constants.C_YES;
        oConfiguration.InsertedById = miUserId;
        oConfiguration.UpdateById = miUserId;
        oConfiguration.DeleteConfigurationSchoolMaster();
    }

    /// <summary>
    /// This method is used to set default controls.
    /// </summary>
    private void SetDefaultFieldValues()
    {
        hidbtnSave.Value = "Save";
        hidMode.Value = Constants.Action.Insert.ToString();
        btnSave.Text = oResourceManager.GetString(hidbtnSave.Value.Replace(" ", string.Empty));
        lblErrorMsg.Text = string.Empty;
        txtLearningOutcome.Text = string.Empty;
        txtSortOrder.Text = string.Empty;
        chkIsconsidered.Checked = false;
        hidRowNo.Value = "0";
    }

    /// <summary>
    /// This method is used populate calss "LearningOutcomeConfigMaster".
    /// </summary>
    /// <returns></returns>
    private LearningOutcomeConfigMaster PopulateLearningOutcomeConfigMaster()
    {
        LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
        oLearningOutcomeConfigMaster.LearningOutCome = txtLearningOutcome.Text;
        oLearningOutcomeConfigMaster.StandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        oLearningOutcomeConfigMaster.SubjectSectionConfigId = Convert.ToInt32(cmbSubjectSection.SelectedValue);
        oLearningOutcomeConfigMaster.SortOrder = Convert.ToInt32(txtSortOrder.Text);
        oLearningOutcomeConfigMaster.IsConsidered = chkIsconsidered.Checked;
        oLearningOutcomeConfigMaster.SchoolId = miSchoolId;
        oLearningOutcomeConfigMaster.AcademicYearId =miAcademicYearId;
        if (hidMode.Value != Constants.Action.Insert.ToString())
        {
            oLearningOutcomeConfigMaster.LearningOutcomeConfigId = Convert.ToInt32(hidLearningOutcomeConfigId.Value);
            oLearningOutcomeConfigMaster.UpdatedById = miUserId;
            oLearningOutcomeConfigMaster.UpdateDate = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
            hidbtnSave.Value = "Save";
        }
        else
        {
            oLearningOutcomeConfigMaster.InsertedById = miUserId;
            oLearningOutcomeConfigMaster.InsertDate = DateTime.Now.ToString(Constants.S_DATE_FORMAT_MARATHI);
        }
        return oLearningOutcomeConfigMaster;
    }

    /// <summary>
    /// This method is used to fill learning outcomes list view.
    /// </summary>
    public void FillLearningOutcomeDetails()
    {
        moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
        LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
        oLearningOutcomeConfigMaster.AcademicYearId = miAcademicYearId;
        oLearningOutcomeConfigMaster.SchoolId = miSchoolId;
        oLearningOutcomeConfigMaster.StandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        oLearningOutcomeConfigMaster.SubjectSectionConfigId = Convert.ToInt32(cmbSubjectSection.SelectedValue);
        moLearningOutcomeConfigMasterBL.LearningOutcomeConfigMaster = oLearningOutcomeConfigMaster;
        List<LearningOutcomeConfigMaster> olstLearningOutcomeConfigMaster = moLearningOutcomeConfigMasterBL.GetAll(hidSortDirection.Value);
        hidIsSubmitted.Value = moLearningOutcomeConfigMasterBL.IsSubmitted.ToString();
        lstvwLearningOutcomeDetails.DataSource = olstLearningOutcomeConfigMaster;
        lstvwLearningOutcomeDetails.DataBind();
        if (lstvwLearningOutcomeDetails.Items.Count > 0)
        {
            HideSubmitCopyControls(true);
            if (Convert.ToBoolean(hidIsSubmitted.Value))
            {
                btnSave.Enabled = false;
                btnSubmit.Text = Resources.LocalizedResources.UnSubmit;
                hidSubmitText.Value = "Un Submit";
                btnSubmit.Enabled = !moLearningOutcomeConfigMasterBL.GradeSubmitStatus;
                tblCopy.Visible = false;
            }
            else
            {
                btnSubmit.Enabled = true;
                btnSave.Enabled = true;
                btnSubmit.Text = Resources.LocalizedResources.Submit;
                hidSubmitText.Value = "Submit";
                tblCopy.Visible = true;
            }
            trNoRecordMsg.Visible = false;
        }
        else
        {
            btnSave.Enabled = true;
            HideSubmitCopyControls(false);
            trNoRecordMsg.Visible = true;
        }
    }

    /// <summary>
    /// This method is used to load learning outcome details.
    /// </summary>
    private void LoadLearningOutcomeDetails()
    {
        moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
        LearningOutcomeConfigMaster oLearningOutcomeConfigMaster = new LearningOutcomeConfigMaster();
        oLearningOutcomeConfigMaster.AcademicYearId = miAcademicYearId;
        oLearningOutcomeConfigMaster.SchoolId =miSchoolId;
        oLearningOutcomeConfigMaster.StandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        oLearningOutcomeConfigMaster.SubjectSectionConfigId = Convert.ToInt32(cmbSubjectSection.SelectedValue);
        moLearningOutcomeConfigMasterBL.LearningOutcomeConfigMaster = oLearningOutcomeConfigMaster;

        moLearningOutcomeConfigMasterBL.Load(Convert.ToInt32(hidLearningOutcomeConfigId.Value));
        oLearningOutcomeConfigMaster = moLearningOutcomeConfigMasterBL.LearningOutcomeConfigMaster;
        txtLearningOutcome.Text = oLearningOutcomeConfigMaster.LearningOutCome;
        txtSortOrder.Text = Convert.ToString(oLearningOutcomeConfigMaster.SortOrder);
        chkIsconsidered.Checked = oLearningOutcomeConfigMaster.IsSubmitted;
        btnSave.Text = Resources.LocalizedResources.Update;
        hidbtnSave.Value = Constants.Action.Update.ToString();
        hidMode.Value = Constants.Action.Update.ToString();
    }
   

    /// <summary>
    /// This method is used to delete learning outcome details.
    /// </summary>
    private void DeleteLearningOutcomeDetails()
    {
        moLearningOutcomeConfigMasterBL = new LearningOutcomeConfigMasterBL();
        moLearningOutcomeConfigMasterBL.Delete(Convert.ToInt32(hidLearningOutcomeConfigId.Value),miUserId);
        SetDefaultFieldValues();
    }

    /// <summary>
    /// This method is used to set sorting image to list view headers.
    /// </summary>
    private void AddSortImage()
    {
        if (hidSortDirection.Value == string.Empty)
            hidSortDirection.Value = Constants.S_ASCENDING;
        HtmlTableRow oHtmlTableHeaderRow = lstvwLearningOutcomeDetails.FindControl("trHeader") as HtmlTableRow;
        if (oHtmlTableHeaderRow != null)
            CommonUtility.AddSortImage(oHtmlTableHeaderRow, S_LEARNING_OUTCOME, hidSortDirection.Value);
    }

    /// <summary>
    /// This method is used to populate class "LearningOutcomesSubmitStatus".
    /// </summary>
    /// <returns></returns>
    private LearningOutcomesSubmitStatus PopulateLearningOutcomesSubmitStatus()
    {
        LearningOutcomesSubmitStatus oLearningOutcomesSubmitStatus = new LearningOutcomesSubmitStatus();
        oLearningOutcomesSubmitStatus.StandardwiseAssessmentId = Convert.ToInt32(cmbAssessment.SelectedValue);
        oLearningOutcomesSubmitStatus.SubjectId = Convert.ToInt32(cmbSubjects.SelectedValue);
        oLearningOutcomesSubmitStatus.SchoolId = miSchoolId;
        oLearningOutcomesSubmitStatus.AcademicYearId = miAcademicYearId;
        if (hidMode.Value != Constants.Action.Insert.ToString())
        {
            oLearningOutcomesSubmitStatus.UpdatedById = miUserId;
            oLearningOutcomesSubmitStatus.UpdateDate = DateTime.Now.ToString();
        }
        else
        {
            oLearningOutcomesSubmitStatus.InsertedById = miUserId;
            oLearningOutcomesSubmitStatus.InsertDate = DateTime.Now.ToString();
        }

        return oLearningOutcomesSubmitStatus;
    }

    /// <summary>
    /// This method is used to add default items.
    /// </summary>
    private void AddDefaultFliterComboBoxItems()
    {
        AddDefaultComboBoxItem(cmbAssessment);
        AddDefaultComboBoxItem(cmbSubjects);
        AddDefaultComboBoxItem(cmbSubjectSection);
    }

    /// <summary>
    /// This method is used to add default items in copy combo boxes.
    /// </summary>
    private void AddCopyDefaultComboBoxItems()
    {
        AddDefaultComboBoxItem(cmbCopyAssessment);
        AddDefaultComboBoxItem(cmbCopySubjects);
        AddDefaultComboBoxItem(cmbCopySubjectSection);
    }

    /// <summary>
    /// This Method used to change value of messgae according to culture
    /// </summary>
    private void RefreshValues()
    {
        btnSubmit.Text = oResourceManager.GetString(hidSubmitText.Value.Replace(" ", string.Empty));
        btnSave.Text = oResourceManager.GetString(hidbtnSave.Value.Replace(" ", string.Empty));
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        valSumErrorMsg2.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        hidValStandardSubjectAssessment.Value = Resources.LocalizedResources.ValStandardSubjectAssessment;
        hidConfirmUnLearningOutcome.Value = Resources.LocalizedResources.ConfirmUnLearningOutcome;
        hidDupLearningOutcome.Value = Resources.LocalizedResources.DupLearningOutcome;
        hidConfirmLearningOutcome.Value = Resources.LocalizedResources.ConfirmLearningOutcome;
        hidDupSortOder.Value = Resources.LocalizedResources.DupSortOder;
        hidValSelectedCopyTo.Value = Resources.LocalizedResources.ValSelectedCopyTo;
        hidSubmit.Value = Resources.LocalizedResources.Submit;
        lblErrorMsg.Text = string.Empty;
    }
    #endregion "Private Methods".
}