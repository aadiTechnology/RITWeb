using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using Utility;
using LessonPlanEntities;
using PayrollReportingUserEntities;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class LessonPlanReportingUserConfigUI : SchoolBase
{
    #region Data Member(s)

    private LessonPlanReportingConfigBL moLessonPlanReportingConfigBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to handle the page load event for filling up the default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moLessonPlanReportingConfigBL = new LessonPlanReportingConfigBL(miSchoolId, miAcademicYearId,  miUserId);
        if (!IsPostBack)
        {
            SetJavaScriptAttribute();
            FillStaff(cmbStaffName);
            FillStaff(cmbReportingStaffName);
            FillReportingStaffDetails();
            RefreshValue();
        }       
    }

    /// <summary>
    /// This event is used to handle the command events like Edit,Delete of configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguration_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                hidReportingConfigId.Value = e.CommandArgument.ToString();
                HiddenField hidUserIdTemp = e.Item.FindControl("hidUserId") as HiddenField;

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moLessonPlanReportingConfigBL.Delete(0, hidReportingConfigId.Value.ToInt());
                    FillReportingStaffDetails();
                    base.DisplayMessage(Resources.LocalizedResources.MsgReportingConfigUserDeleted, false, tdMessage);
                    ClearFields();
                    FillStaff(cmbReportingStaffName);
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = Resources.LocalizedResources.Update;
                    hidEditedUserId.Value = hidUserIdTemp.Value;
                    LessonPlanReportingConfig oLessonPlanReportingConfig = moLessonPlanReportingConfigBL.Get(cmbStaffName.SelectedValue.ToInt(), hidReportingConfigId.Value.ToInt());
                    if (!oLessonPlanReportingConfig.IsNull())
                    {
                        FillStaff(cmbReportingStaffName);
                        cmbReportingStaffName.SelectedValue = oLessonPlanReportingConfig.ReportingUserId.ToString();
                        chkfinalApprover.Checked = oLessonPlanReportingConfig.IsFinalApprover;
                        txtApprovalSortOrder.Text = oLessonPlanReportingConfig.ApprovalSortOrder.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save the configuration details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Save();
            bool bIsConfigured = QueryString[Constants.S_IS_CONFIGURED] != Constants.S_YES;

            if (bIsConfigured)
                base.SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LessonPlanReportingUserConfig));

            DisplayMessage(hidReportingConfigId.Value == Constants.S_ZERO ? Resources.LocalizedResources.MsgReportingConfigSaved : Resources.LocalizedResources.MsgReportingConfigUpdated, false);
            ClearFields();
            FillStaff(cmbReportingStaffName);
            FillReportingStaffDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to clear all the fields on canceling the operation.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            FillStaff(cmbReportingStaffName);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to delete all the configuration for selected user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            moLessonPlanReportingConfigBL.Delete(cmbStaffName.SelectedValue.ToInt(), 0);
            FillReportingStaffDetails();
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigDeleted, false);
            ClearFields();
            FillStaff(cmbReportingStaffName);
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to submit all the details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moLessonPlanReportingConfigBL.SubmitUnsubmitConfig(cmbStaffName.SelectedValue.ToInt(), true);
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigSubmited, false);
            ClearFields();
            FillStaff(cmbReportingStaffName);
            FillReportingStaffDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to un submit all the configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moLessonPlanReportingConfigBL.SubmitUnsubmitConfig(cmbStaffName.SelectedValue.ToInt(), false);
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigUnSubmited, false);
            ClearFields();
            FillStaff(cmbReportingStaffName);
            FillReportingStaffDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetQueryString()
    {
        string sUserName = cmbStaffName.SelectedItem.Text;
        hidQueryString.Value = CommonUtility.EncryptQuerystring("UserId=" + cmbStaffName.SelectedValue + "&UserName=" + sUserName + "&UserRoleId="+Constants.UserRoles.Teacher.ToInt()+"&IsLessonPlanScreen=1");
    }

    /// <summary>
    /// This is used to handle the role changed event on selecting diff. role from combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cmbStaffName.ClearSelection();
            ClearFields();
            FillStaff(cmbReportingStaffName);
            FillStaff(cmbStaffName);
            FillReportingStaffDetails();
            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    
    /// <summary>
    /// This is used to handle the user changed event on selecting diff. user from combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            FillStaff(cmbReportingStaffName);
            FillReportingStaffDetails();
            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to set attributes on edit and delete button.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwConfiguration_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton imgEdit = e.Item.FindControl("imgEdit") as ImageButton;
                ImageButton imgDelete = e.Item.FindControl("imgDelete") as ImageButton;
                LessonPlanReportingConfig oLessonPlanReportingConfig = e.Item.DataItem as LessonPlanReportingConfig;
                //if (oLessonPlanReportingConfig.IsPublished)
                //{
                //    imgEdit.Visible = false;
                //    imgDelete.Visible = false;
                //}

                bool bIsSubmitted = Convert.ToBoolean(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["IsSubmitted"]);
                if (bIsSubmitted)
                {
                    imgDelete.Visible = imgEdit.Visible = false;
                }

                HiddenField hidIsFinalApprover = e.Item.FindControl("hidIsFinalApprover") as HiddenField;
                hidIsFinalApprover.Value = Constants.S_NO;
                if (oLessonPlanReportingConfig.IsFinalApprover)
                    hidIsFinalApprover.Value = Constants.S_YES;

            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method(s)

    /// <summary>
    /// This method is used to save the reporting configuration for selected user.
    /// </summary>
    private void Save()
    {
        LessonPlanReportingConfig oLessonPlanReportingConfig = new LessonPlanReportingConfig
        {
            UserId = cmbStaffName.SelectedValue.ToInt(),
            ReportingUserId = cmbReportingStaffName.SelectedValue.ToInt(),
            IsFinalApprover = chkfinalApprover.Checked,
            Id = hidReportingConfigId.Value.ToInt(),
            ApprovalSortOrder = txtApprovalSortOrder.Text.ToInt()
        };

        moLessonPlanReportingConfigBL.Save(oLessonPlanReportingConfig);
    }

    /// <summary>
    /// This method is used to display message.
    /// </summary>
    /// <param name="aoItemState"></param>
    /// <param name="abIsErrorMessage"></param>
    private void DisplayMessage(string asName, bool abIsErrorMessage)
    {
        base.DisplayMessage(asName, abIsErrorMessage, tdMessage);
    }

    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit, btnDeleteAll });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.LessonPlanRelated));
        valSumError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
    }

    /// <summary>
    /// This method is used to fill the users.
    /// </summary>
    private void FillStaff(DropDownList aocmbUsers)
    {
        List<ReportingUserConfiguration> lstReportingUserConfiguration = MasterDataCollectionBL.GetReportingUsers(Constants.UserRoles.Teacher.ToInt(), miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstReportingUserConfiguration, aocmbUsers, "UserName", "UserId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill up saved reporting staff in listview according to selected staff.
    /// </summary>
    private void FillReportingStaffDetails()
    {
        List<LessonPlanReportingConfig> lstLessonPlanReportingConfig = moLessonPlanReportingConfigBL.GetAll(cmbStaffName.SelectedValue.ToInt());
        lstvwConfiguration.DataSource = lstLessonPlanReportingConfig;
        lstvwConfiguration.DataBind();

        btnSubmit.Enabled = btnDeleteAll.Enabled = lstLessonPlanReportingConfig.FindAll(prm => !prm.IsSubmitted).Any();
        btnUnSubmit.Enabled = lstLessonPlanReportingConfig.FindAll(prm => prm.IsSubmitted).Any();

        if (lstvwConfiguration.Items.Count == Constants.I_ZERO)
        {
            btnDeleteAll.Enabled = false;
            btnCopyConfig.Enabled = false;
        }
        else
        {
            btnDeleteAll.Enabled = !(lstLessonPlanReportingConfig.FindAll(prm => prm.IsSubmitted).Any());
            btnCopyConfig.Enabled = true;
        }

        if (lstLessonPlanReportingConfig.FindAll(user => user.IsPublished).Any())
        {
            btnDeleteAll.Enabled = false;
            //btnUnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {   
        chkfinalApprover.Checked = false;        
        hidReportingConfigId.Value = Constants.S_ZERO;
        hidEditedUserId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
        txtApprovalSortOrder.Text = string.Empty;
    }

    /// <summary>
    /// This method is used to update hidden fields according to selected culture and resource file value.
    /// </summary>
    private void RefreshValue()
    {   
        lblReportingStaffName.Text = Resources.LocalizedResources.Reporting + " " + Resources.LocalizedResources.StaffName;
        hidStaffNameSelected.Value = Resources.LocalizedResources.valRequiredStaffName;
        hidReporting.Value = Resources.LocalizedResources.Reporting;        
        hidStaffAlreadyexist.Value = Resources.LocalizedResources.MsgStaffNameAlreadyExists;
        hidStaffNameshouldNotSame.Value = Resources.LocalizedResources.valStaffNameshouldNotSame;
    }
    #endregion
}