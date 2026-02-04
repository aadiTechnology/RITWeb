/* File Name = SubjectSectionConfigurationUI.aspx.cs
 * Created Date - 26 May 2011
 * Created by - Vipul
 * Class Description - This class is defined to manage subject section details.*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using BusinessLogic;
using BusinessLogic.Exceptions;
using MasterEntities;
using Utility;
using XseedReportEntities;

public partial class SubjectSectionConfigurationUI : SchoolBase
{
    #region "Data Members"

    int miSavedRecordCount = 0;
    private List<int> mlstSortOrders = new List<int>();

    #endregion "Data Members"

    #region "Events"

    /// <summary>
    /// This event is used to set default attributes.
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
                SetJavaScriptAttributres();
                if (CheckPreCondition())
                {
                    valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
                    FillStandardComboBox();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                RefreshValue();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save subject section details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            SubjectSectionConfigurationMasterBL oSubjectSectionConfigurationMasterBL = new SubjectSectionConfigurationMasterBL();
            List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = PopulateSubjectSectionDetails();
            string sMessage = CheckDependencies(lstSubjectSectionConfigurationMaster.Where(Sub => Sub.Action == Constants.Action.Delete).ToList(), miAcademicYearId);
            if (string.IsNullOrEmpty(sMessage))
                oSubjectSectionConfigurationMasterBL.Save(GetSubjectSectionXML(lstSubjectSectionConfigurationMaster), Convert.ToInt32(cmbSubjects.SelectedValue), miAcademicYearId, miSchoolId, miUserId);
            else
                throw new ReferenceExceptions(sMessage);
            lblUpdateSucess.Text = Resources.LocalizedResources.SubjectSectionDetailsSavedSuccessfully;
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] == Constants.S_YES;
            if (!bIsConfigured)
                SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration));
            lblErrorMsg.Text = string.Empty;
            hidSaveCount.Value = miSavedRecordCount.ToString();
            FillSubjectSectionDetails();
        }
        catch (ReferenceExceptions ex)
        {
            lblErrorMsg.Text = CommonUtility.ModifyExceptionMessage(ex.Message, "Subject Section", Resources.LocalizedResources.SubjectSection, "can not be removed since it is associated with", Resources.LocalizedResources.CanNoBeRemovedAssociatedWith);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to fill subjectwise subject section details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbSubjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbSubjects.SelectedValue != Constants.I_ZERO.ToString())
            {
                divSubjectSectionDetails.Visible = true;
                btnSave.Visible = true;
                FillSubjectSectionDetails();
                hidSaveCount.Value = miSavedRecordCount.ToString();
            }
            else
            {
                divSubjectSectionDetails.Visible = false;
                btnSave.Visible = false;
            }
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void cmbStandards_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (!cmbStandards.SelectedValue.Equals("0"))
            {
                FillSubjectsCombo();
                hidSaveCount.Value = miSavedRecordCount.ToString();
            }
            else
            {
                cmbSubjects.Items.Clear();
                cmbSubjects.DataBind();
                cmbSubjects.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
            }
            divSubjectSectionDetails.Visible = false;
            btnSave.Visible = false;
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set default listview controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwSubjectSectionDetails_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                Label lblSrNo = oCurrentItem.FindControl("lblRowNo") as Label;
                CheckBox ochkIsSaved = oCurrentItem.FindControl("chkIsSubmitted") as CheckBox;
                DropDownList cmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;

                cmbSortOrder.Items.Add(new ListItem { Text="--Select--", Value = "0" });
                mlstSortOrders.ForEach(so =>
                {
                    cmbSortOrder.Items.Add(new ListItem { Text=so.ToString(), Value = so.ToString() });
                });

                TextBox txtSubjectSectionName = oCurrentItem.FindControl("txtSubjectSectionName") as TextBox;
                txtSubjectSectionName.Attributes.Add("onkeyup", "OnGridKeyUp(this,event);");
                int iIsDeleted = Convert.ToInt32(lstvwSubjectSectionDetails.DataKeys[oCurrentItem.DisplayIndex]["Is_Deleted"]);
                cmbSortOrder.SelectedValue = Convert.ToString(lstvwSubjectSectionDetails.DataKeys[oCurrentItem.DisplayIndex]["SortOrder"]);
                ochkIsSaved.Attributes.Add("onclick", "EnableControls(" + oCurrentItem.DisplayIndex + ")");
                lblSrNo.Text = (oCurrentItem.DisplayIndex + 1).ToString();
                if (iIsDeleted == Constants.I_ZERO)
                {
                    ochkIsSaved.Checked = true;
                    miSavedRecordCount = miSavedRecordCount + 1;
                }
                else
                {
                    cmbSortOrder.Enabled = false;
                    txtSubjectSectionName.Enabled = false;
                }
            }
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
    protected void lstvwSubjectSectionDetails_DataBound(object sender, EventArgs e)
    {
        try
        {
            hidRowCount.Value = lstvwSubjectSectionDetails.Items.Count.ToString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion "Events"

    #region "Private Methods"


    /// <summary>
    /// This method is used to fill standard combobox.
    /// </summary>
    /// <param name="cmbStandard"></param>
    private void FillStandardComboBox()
    {       
        List<StandardMaster> lstStandardMaster = new List<StandardMaster>();

        if (moUserRole == Constants.UserRoles.Admin || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration) == Constants.C_YES)
            lstStandardMaster = StandardMasterBL.GetStandardsAssociatedToAssessments(miSchoolId, miAcademicYearId, 0);
        else if (moUserRole == Constants.UserRoles.Teacher)
        {
            int iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
            lstStandardMaster = StandardMasterBL.GetStandardsAssociatedToAssessments(miSchoolId, miAcademicYearId, iTeacherId);
        }

        cmbStandards.DataSource = lstStandardMaster;
        cmbStandards.DataTextField = "StandardName";
        cmbStandards.DataValueField = "StandardId";
        cmbStandards.DataBind();
        cmbStandards.Items.Insert(0, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
        cmbSubjects.Items.Add(new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This function is used to fill subject combo.
    /// </summary>
    private void FillSubjectsCombo()
    {
        int iTeacherId = 0;
        if (moUserRole == Constants.UserRoles.Admin || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration) == Constants.C_YES)
            iTeacherId = Constants.I_ZERO;
        else if (moUserRole == Constants.UserRoles.Teacher)
            iTeacherId = Convert.ToInt32(Session[Constants.S_SESSION_TEACHER_ID]);
        cmbSubjects.DataSource = TeacherSubjectDetailsBL.GetTeacherAssociatedSubjects(iTeacherId, Convert.ToInt32(cmbStandards.SelectedValue), miAcademicYearId, miSchoolId, false);
        cmbSubjects.DataTextField = "SubjectName";
        cmbSubjects.DataValueField = "StandardwiseSubjectId";
        cmbSubjects.DataBind();
        cmbSubjects.Items.Insert(Constants.I_ZERO, new ListItem(Constants.S_SELECT, Constants.I_ZERO.ToString()));
    }

    /// <summary>
    /// This method is used to fill subject section details.
    /// </summary>
    private void FillSubjectSectionDetails()
    {
        SubjectSectionConfigurationMasterBL oSubjectSectionConfigurationMasterBL = new SubjectSectionConfigurationMasterBL();
        SubjectSectionConfigurationMaster oSubjectSectionConfigurationMaster = new SubjectSectionConfigurationMaster();
        oSubjectSectionConfigurationMaster.SchoolId = miSchoolId;
        oSubjectSectionConfigurationMaster.AcademicYearId = miAcademicYearId;
        oSubjectSectionConfigurationMaster.StandardwiseSubjectId = Convert.ToInt32(cmbSubjects.SelectedValue);
        oSubjectSectionConfigurationMasterBL.SubjectSectionConfigurationMaster = oSubjectSectionConfigurationMaster;
        List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = oSubjectSectionConfigurationMasterBL.GetAll();

        mlstSortOrders.Clear();
        for (int k = 1; k <= lstSubjectSectionConfigurationMaster.Count; k++)
            mlstSortOrders.Add(k);

        lstvwSubjectSectionDetails.DataSource = lstSubjectSectionConfigurationMaster;
        lstvwSubjectSectionDetails.DataBind();
    }

    /// <summary>
    /// This method is used to populate sbject section details.
    /// </summary>
    /// <returns></returns>
    private List<SubjectSectionConfigurationMaster> PopulateSubjectSectionDetails()
    {
        List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster = new List<SubjectSectionConfigurationMaster>();
        SubjectSectionConfigurationMaster oSubjectSectionConfigurationMaster = null;

        foreach (ListViewDataItem oCurrentItem in lstvwSubjectSectionDetails.Items)
        {
            CheckBox oChkSelect = oCurrentItem.FindControl("chkIsSubmitted") as CheckBox;
            if (oChkSelect.Checked == true || Convert.ToInt32(lstvwSubjectSectionDetails.DataKeys[oCurrentItem.DisplayIndex]["SchoolId"]) == 0)
            {
                oSubjectSectionConfigurationMaster = new SubjectSectionConfigurationMaster();
                TextBox otxtSubjectSectionName = oCurrentItem.FindControl("txtSubjectSectionName") as TextBox;
                DropDownList ocmbSortOrder = oCurrentItem.FindControl("cmbSortOrder") as DropDownList;
                oSubjectSectionConfigurationMaster.SubjectSectionConfigurationId = Convert.ToInt32(lstvwSubjectSectionDetails.DataKeys[oCurrentItem.DisplayIndex]["SubjectSectionConfigurationId"].ToString());
                oSubjectSectionConfigurationMaster.SubjectSectionName = otxtSubjectSectionName.Text;
                oSubjectSectionConfigurationMaster.OrginalSubjectSectionId = Convert.ToInt32(lstvwSubjectSectionDetails.DataKeys[oCurrentItem.DisplayIndex]["OrginalSubjectSectionId"].ToString());
                oSubjectSectionConfigurationMaster.SortOrder = Convert.ToInt32(ocmbSortOrder.SelectedValue);
                oSubjectSectionConfigurationMaster.Action = !oChkSelect.Checked ? Constants.Action.Delete : Constants.Action.Insert;
                lstSubjectSectionConfigurationMaster.Add(oSubjectSectionConfigurationMaster);
            }
        }
        return lstSubjectSectionConfigurationMaster;
    }

    /// <summary>
    /// This method is used to check dependencies.
    /// </summary>
    /// <param name="lstSubjectSectionConfigurationMaster"></param>
    /// <param name="aiAcademicYearId"></param>
    /// <returns></returns>
    private string CheckDependencies(List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster, int aiAcademicYearId)
    {
        GenericReferenceList<SubjectSectionConfigurationMaster> objStdRefereces = new GenericReferenceList<SubjectSectionConfigurationMaster>(lstSubjectSectionConfigurationMaster, aiAcademicYearId);
        return objStdRefereces.CheckDependenciesForList("SubjectSectionConfigurationId", "SubjectSectionName", "Action", Constants.ReferenceId.SubjectSectionConfiguration, false);
    }
    
    /// <summary>
    /// This method is used to get subject section XML.
    /// </summary>
    /// <param name="lstSubjectSectionConfigurationMaster"></param>
    /// <returns></returns>
    private string GetSubjectSectionXML(List<SubjectSectionConfigurationMaster> lstSubjectSectionConfigurationMaster)
    {
        StringWriter sw = new StringWriter();
        new XmlSerializer(lstSubjectSectionConfigurationMaster.GetType()).Serialize(sw, lstSubjectSectionConfigurationMaster);
        string sXML = sw.ToString();
        sXML = sXML.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty);
        return sXML;
    }

    /// <summary>
    /// This method is used to set javascript attributes.
    /// </summary>
    private void SetJavaScriptAttributres()
    {
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Xseed_Report_Related));
        btnSave.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
        ApplyMouseHoverEffect(new List<Button> {btnCancel, btnSave});
        lblErrorMsg.Text = string.Empty;
        btnSave.Visible = false;
    }

    /// <summary>
    /// This method checks the preconditons of Configured Subjects for Subject Group criteria.
    /// </summary>
    /// <returns></returns>
    private bool CheckPreCondition()
    {
        bool bReturn = false;
        string sLinks = ReferenceBL.GetPreConditionMsg(Constants.SchoolConfigurations.SubjectwiseSubjectSectionConfiguration);

        if (sLinks.Equals(""))
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
    /// This method is used to set visible or hide properties of controls.
    /// </summary>
    private void VisibleOrHideControls()
    {
        tblSubjectSection.Visible = false;
        divSubjectSectionDetails.Visible = false;
        btnCancel.Visible = true;
        btnCancel.Text = Resources.LocalizedResources.Back;
        btnSave.Visible = false;
    }
    /// <summary>
    /// This method used to value based on Culture
    /// </summary>
    private void RefreshValue()
    {
        hidAreYouSureYouWantToDeleteCurrentlyUncheckedSubjectSection.Value = Resources.LocalizedResources.AreYouSureYouWantToDeleteCurrentlyUncheckedSubjectSection;
        hidSortOrderShouldNotBeDuplicatedForRow.Value = Resources.LocalizedResources.SortOrderShouldNotBeDuplicatedForRow;
        hidSubjectSectionNameShouldNotBeDuplicatedForRow.Value = Resources.LocalizedResources.SubjectSectionNameShouldNotBeDuplicatedForRow;
        hidSubjectSectionShouldNotBeBlankForRow.Value = Resources.LocalizedResources.SubjectSectionShouldNotBeBlankForRow;
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }
    #endregion "Private Methods"

}