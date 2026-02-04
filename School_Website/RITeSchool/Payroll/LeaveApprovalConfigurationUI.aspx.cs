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
using System.Data;
using PayrollEntities;
using BusinessLogic.PayrollBL;
using SchoolEntities.Payroll;
//using BusinessLogic.PayrollBL

public partial class LeaveApprovalConfigurationUI : SchoolBase
{
    private LeaveApprovalConfigurationBL moReportingConfigurationBL;

    protected void Page_Load(object sender, EventArgs e)
    {
        moReportingConfigurationBL = new LeaveApprovalConfigurationBL(miSchoolId, miAcademicYearId, miUserId);

        if (!IsPostBack)
        {
            SetJavaScriptAttribute();
            FillUserRoles();
            FillStaff(cmbRole.SelectedValue.ToInt(), cmbStaffName);
            FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
            FillReportingStaffDetails();
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
                HiddenField hidUserIdTemp = e.Item.FindControl("hidUserId") as HiddenField;
                int iParameterId = lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["Id"].ToInt();
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                hidReportingConfigId.Value = iParameterId.ToString();

                if (e.CommandName == Constants.S_COMMAND_REMOVE)
                {
                    moReportingConfigurationBL.Delete(iParameterId);
                    FillReportingStaffDetails();
                    ClearFields();
                    base.DisplayMessage(Resources.LocalizedResources.MsgReportingConfigUserDeleted, false, tdMessage);
                }
                else if (e.CommandName == Constants.S_COMMAND_UPDATE)
                {
                    btnSave.Text = Resources.LocalizedResources.Update;
                    hidEditedUserId.Value = hidUserIdTemp.Value;
                    LeaveApprovalConfiguration oPerformanceReportingConfig = moReportingConfigurationBL.Get(iParameterId);
                    if (!oPerformanceReportingConfig.IsNull())
                    {
                        txtApprovalSortOrder.Text = oPerformanceReportingConfig.ApproverSortOrder.ToString();
                        chkfinalApprover.Checked = oPerformanceReportingConfig.IsFinalApprover;

                        cmbReportingRole.SelectedValue = oPerformanceReportingConfig.UserRoleId.ToString();
                        FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
                        cmbReportingStaffName.SelectedValue = oPerformanceReportingConfig.ReportingUserId.ToString();
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
                Image imgIsFinalApprover = e.Item.FindControl("imgIsFinalApprover") as Image;

                LeaveApprovalConfiguration oPerformanceReportingConfig = e.Item.DataItem as LeaveApprovalConfiguration;

                bool bIsSubmitted = Convert.ToBoolean(lstvwConfiguration.DataKeys[e.Item.DisplayIndex]["IsSubmitted"]);
                if (bIsSubmitted)
                    imgDelete.Visible = imgEdit.Visible = false;

                HiddenField hidIsFinalApprover = e.Item.FindControl("hidIsFinalApprover") as HiddenField;
                hidIsFinalApprover.Value = Constants.S_NO;
                if (oPerformanceReportingConfig.IsFinalApprover)
                {
                    hidIsFinalApprover.Value = Constants.S_YES;
                    imgIsFinalApprover.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moReportingConfigurationBL.Submit(cmbStaffName.SelectedValue.ToInt(), true);
            ClearFields();
            DisplayMessage("Leave approval configuration submitted successfully!!!", false, tdMessage);
            FillReportingStaffDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    protected void btnUnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            moReportingConfigurationBL.Submit(cmbStaffName.SelectedValue.ToInt(), false);
            ClearFields();
            DisplayMessage("Leave approval configuration un-submitted successfully!!!", false, tdMessage);

            FillReportingStaffDetails();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used to set java script attributes.
    /// </summary>
    private void SetJavaScriptAttribute()
    {
        base.ApplyMouseHoverEffect(new List<Button> { btnCancel, btnSave, btnBack, btnSubmit, btnUnSubmit });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.StaffPerformanceRelated));
        valSumError.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        cmbRole.Focus();
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
    /// This method is used to clear fields.
    /// </summary>
    private void ClearFields()
    {
        cmbReportingStaffName.ClearSelection();
        cmbReportingRole.ClearSelection();
        chkfinalApprover.Checked = false;
        hidReportingConfigId.Value = Constants.S_ZERO;
        hidEditedUserId.Value = Constants.S_ZERO;
        btnSave.Text = Resources.LocalizedResources.Save;
        txtApprovalSortOrder.Text = string.Empty;
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
        List<LeaveApprovalConfiguration> lstPerformanceReportingConfig = moReportingConfigurationBL.GetAll(cmbStaffName.SelectedValue.ToInt());
        lstvwConfiguration.DataSource = lstPerformanceReportingConfig;
        lstvwConfiguration.DataBind();

        btnSubmit.Enabled = lstPerformanceReportingConfig.FindAll(prm => !prm.IsSubmitted).Any();
        btnUnSubmit.Enabled = lstPerformanceReportingConfig.FindAll(prm => prm.IsSubmitted).Any();

        if (lstvwConfiguration.Items.Count == Constants.I_ZERO)
            btnCopyConfig.Enabled = false;
        else
            btnCopyConfig.Enabled = true;
    }

    private void SetQueryString()
    {
        string sUserName = cmbStaffName.SelectedItem.Text;
        string sUserRole = cmbRole.SelectedItem.Text;
        hidQueryString.Value = CommonUtility.EncryptQuerystring("UserId=" + cmbStaffName.SelectedValue + "&UserRoleId=" + cmbRole.SelectedValue + "&UserName=" + sUserName + "&UserRole=" + sUserRole);
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

            DisplayMessage(hidReportingConfigId.Value == Constants.S_ZERO ? "Leave approval configuration saved successfully!!!" : "Leave approval configuration updated successfully!!!", false, tdMessage);
            ClearFields();            
            FillReportingStaffDetails();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used to save the reporting configuration for selected user.
    /// </summary>
    private void Save()
    {
        LeaveApprovalConfiguration oLeaveApprovalConfig = new LeaveApprovalConfiguration
         {
             UserId = cmbStaffName.SelectedValue.ToInt(),
             ReportingUserId = cmbReportingStaffName.SelectedValue.ToInt(),

             IsFinalApprover = chkfinalApprover.Checked,

             ApproverSortOrder = txtApprovalSortOrder.Text.ToInt(),
             Id = hidReportingConfigId.Value.ToInt()
         };

        moReportingConfigurationBL.Save(oLeaveApprovalConfig);
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
            //ClearFields();
            //FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
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
    /// This is used to handle the user changed event on selecting diff. user from combobox.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbStaffName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //ClearFields();
            //FillStaff(cmbReportingRole.SelectedValue.ToInt(), cmbReportingStaffName);
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
}