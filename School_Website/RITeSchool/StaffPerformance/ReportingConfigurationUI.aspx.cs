/*File Name - ReportingConfigurationDC.cs
 * Created By - Pravin Shinde
 * Created Date - 25 Sept 2013
 * Description - This class is used to manage performance reporting configuration.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BookEntities;
using BusinessLogic;
using BusinessLogic.Exceptions;
using PayrollReportingUserEntities;
using Utility;
using StaffPerformanceEntity;
using System.Resources;
using SchoolEntities;

public partial class ReportingConfigurationUI : SchoolBase
{
    #region Data Member(s)

    private ReportingConfigurationBL moReportingConfigurationBL;

    #endregion

    #region Event(s)

    /// <summary>
    /// This event is used to handle the page load event for filling up the default values.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        moReportingConfigurationBL = new ReportingConfigurationBL(miSchoolId, miUserId);
        if (!IsPostBack)
        {
            if (Session[Constants.S_SESSION_LANGUAGE] != null)
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
            }

            RefreshValue();
            SetJavaScriptAttribute();
            FillYearCombobox();
            FillUserRoles();
            FillStaff(cmbRole.SelectedValue.ToInt(), cmbStaffName);
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
            FillReportingStaffDetails();
        }
        if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
        {
            hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
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
                HiddenField hidUserIdTemp  = e.Item.FindControl("hidUserId") as HiddenField;

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moReportingConfigurationBL.Delete(cmbYear.SelectedValue.ToInt(), 0, hidReportingConfigId.Value.ToInt());
                    FillReportingStaffDetails();
                    base.DisplayMessage(Resources.LocalizedResources.MsgReportingConfigUserDeleted, false, tdMessage);
                    ClearFields();
                    FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = Resources.LocalizedResources.Update;
                    hidEditedUserId.Value = hidUserIdTemp.Value;
                    PerformanceReportingConfig oPerformanceReportingConfig = moReportingConfigurationBL.Get(cmbStaffName.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), hidReportingConfigId.Value.ToInt());
                    if (!oPerformanceReportingConfig.IsNull())
                    {
                        cmbReportingRole.SelectedValue = oPerformanceReportingConfig.RoleId.ToString();
                        FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
                        cmbReportingStaffName.SelectedValue = oPerformanceReportingConfig.ReportingUserId.ToString();                        
                        chkfinalApprover.Checked = oPerformanceReportingConfig.IsFinalApprover;
                        chkIsSupervisor.Checked = oPerformanceReportingConfig.IsSupervisor;
                        txtApprovalSortOrder.Text = oPerformanceReportingConfig.ApprovalSortOrder.ToString();
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
                base.SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ReportingConfiguration));

            DisplayMessage(hidReportingConfigId.Value == Constants.S_ZERO ? Resources.LocalizedResources.MsgReportingConfigSaved : Resources.LocalizedResources.MsgReportingConfigUpdated, false);
            ClearFields();
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
            FillReportingStaffDetails();
            //FillUserDetails();
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
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
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
            moReportingConfigurationBL.Delete(cmbYear.SelectedValue.ToInt(), cmbStaffName.SelectedValue.ToInt(), 0);
            FillReportingStaffDetails();
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigDeleted, false);
            ClearFields();
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
            //FillUserDetails();
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
            moReportingConfigurationBL.SubmitUnsubmitConfig(cmbStaffName.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), true);
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigSubmited, false);
            ClearFields();
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
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
            moReportingConfigurationBL.SubmitUnsubmitConfig(cmbStaffName.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt(), false);
            DisplayMessage(Resources.LocalizedResources.MsgReportingConfigUnSubmited, false);
            ClearFields();
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
            FillReportingStaffDetails();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }        
    }

    /// <summary>
    /// This is used to handle the year changed event on selecting diff. year from combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
            FillReportingStaffDetails();
            SetQueryString();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    private void SetQueryString()
    {
        string sUserName = cmbStaffName.SelectedItem.Text;
        string sUserRole = cmbRole.SelectedItem.Text;
        hidQueryString.Value = CommonUtility.EncryptQuerystring("UserId=" + cmbStaffName.SelectedValue + "&UserRoleId=" + cmbRole.SelectedValue + "&Year=" + cmbYear.SelectedValue + "&UserName=" + sUserName + "&UserRole=" + sUserRole);
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
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);        
            FillStaff(cmbRole.SelectedValue.ToInt(), cmbStaffName);
            FillReportingStaffDetails();
            SetQueryString();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is used to handle the reporting role changed event on selecting diff. role from combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbReportingRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbReportingRole.SelectedValue != Constants.S_ZERO)
                cmbReportingStaffName.ClearSelection();

            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);                          
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
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);                    
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
                PerformanceReportingConfig oPerformanceReportingConfig = e.Item.DataItem as PerformanceReportingConfig;
                if (oPerformanceReportingConfig.IsPublished)
                {
                    imgEdit.Visible = false;
                    imgDelete.Visible = false;
                }

                bool bIsSubmitted = Convert.ToBoolean(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["IsSubmitted"]);
                if (bIsSubmitted)
                    imgDelete.Visible = imgEdit.Visible = false;

                HiddenField hidIsFinalApprover = e.Item.FindControl("hidIsFinalApprover") as HiddenField;
                hidIsFinalApprover.Value = Constants.S_NO;
                if (oPerformanceReportingConfig.IsFinalApprover)
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
        PerformanceReportingConfig oPerformanceReportingConfig = new PerformanceReportingConfig
        {
            UserId = cmbStaffName.SelectedValue.ToInt(),
            ReportingUserId = cmbReportingStaffName.SelectedValue.ToInt(),
            Year = cmbYear.SelectedValue.ToInt(),
            IsFinalApprover = chkfinalApprover.Checked,
        //    IsSupervisor = chkIsSupervisor.Checked,
            Id = hidReportingConfigId.Value.ToInt(),
            ApprovalSortOrder = txtApprovalSortOrder.Text.ToInt()
        };

        moReportingConfigurationBL.Save(oPerformanceReportingConfig);
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
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit ,btnDeleteAll});
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated));
        valSumError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;         
        cmbYear.Focus();
    }

    /// <summary>
    /// This method is used to fill up year combo box.
    /// </summary>
    private void FillYearCombobox()
    {
        List<AcademicYear> lstYears = SchoolWiseAcademicYearMasterBL.GetAllYears(miSchoolId);
        ListSource.FillDropDownList(lstYears, cmbYear, "Year", "Id", string.Empty);
        cmbYear.SelectedValue = miAcademicYearId.ToString();
    }

    /// <summary>
    /// Following method is used to fill the roles checkbox into user role comboboxes.    
    /// </summary>
    private void FillUserRoles()
    {
        List<UserRoles> lstUserRoles = MasterDataCollectionBL.GetAllRoles();        
        lstUserRoles = lstUserRoles.Where(a => (a.User_Role_Id != Constants.UserRoles.Student.ToInt() && a.User_Role_Id != Constants.UserRoles.TransportStaff.ToInt() && a.User_Role_Id != Constants.UserRoles.Parent.ToInt())).ToList();
        ListSource.FillDropDownList(lstUserRoles, cmbRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
        ListSource.FillDropDownList(lstUserRoles, cmbReportingRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill the users.
    /// </summary>
    private void FillStaff(int aiRoleId, DropDownList aocmbUsers)
    {
        List<ReportingUserConfiguration> lstReportingUserConfiguration = MasterDataCollectionBL.GetReportingUsers(aiRoleId, miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(lstReportingUserConfiguration, aocmbUsers, "UserName", "UserId", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill up saved reporting staff in listview according to selected staff.
    /// </summary>
    private void FillReportingStaffDetails()
    {
        List<PerformanceReportingConfig> lstPerformanceReportingConfig = moReportingConfigurationBL.GetAll(cmbStaffName.SelectedValue.ToInt(), cmbYear.SelectedValue.ToInt());
        lstvwConfiguration.DataSource = lstPerformanceReportingConfig;
        lstvwConfiguration.DataBind();

        //btnSubmit.Enabled = btnDeleteAll.Enabled = lstPerformanceReportingConfig.FindAll(prm => !prm.IsSubmitted).Where(p=>p.IsSupervisor).Any();
        btnSubmit.Enabled = btnDeleteAll.Enabled = lstPerformanceReportingConfig.FindAll(prm => !prm.IsSubmitted).Any();
        //btnUnSubmit.Enabled = lstPerformanceReportingConfig.FindAll(prm => prm.IsSubmitted).Where(p => p.IsSupervisor).Any();
        btnUnSubmit.Enabled = lstPerformanceReportingConfig.FindAll(prm => prm.IsSubmitted).Any();

        if (lstvwConfiguration.Items.Count == Constants.I_ZERO)
        {
            btnDeleteAll.Enabled = false;
            btnCopyConfig.Enabled = false;
        }
        else
        {
            btnDeleteAll.Enabled = !(lstPerformanceReportingConfig.FindAll(prm => prm.IsSubmitted).Any());
            btnCopyConfig.Enabled = true;
        }

        if (lstPerformanceReportingConfig.FindAll(user => user.IsPublished).Any())
        {
            btnDeleteAll.Enabled = false;
            btnUnSubmit.Enabled = false;
        }
    }

    /// <summary>
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbReportingRole.ClearSelection();             
        chkfinalApprover.Checked = false;
        chkIsSupervisor.Checked = false;
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
        lblReportingRole.Text = Resources.LocalizedResources.Reporting + " " + Resources.LocalizedResources.UserRole;
        lblReportingStaffName.Text = Resources.LocalizedResources.Reporting + " " + Resources.LocalizedResources.StaffName;
        hidStaffNameSelected.Value = Resources.LocalizedResources.valRequiredStaffName;
        hidReporting.Value = Resources.LocalizedResources.Reporting;
        hidAlertProgressReportUser.Value = Resources.LocalizedResources.AlertProgressReportUser;
        hidAlertAllProgressReportUser.Value = Resources.LocalizedResources.AlertAllProgressReportUser;
        hidStaffAlreadyexist.Value = Resources.LocalizedResources.MsgStaffNameAlreadyExists;
        hidStaffNameshouldNotSame.Value = Resources.LocalizedResources.valStaffNameshouldNotSame;
    }
    #endregion
}