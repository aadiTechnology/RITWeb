using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using System.Data;
using System.Drawing;
using BookEntities;
using PayrollReportingUserEntities;
using MasterEntities;
using Utility;
using System.Data.SqlClient;
using System.Linq;

public partial class ReportingUserConfigurationUI : SchoolBase
{
    #region -- CONSTANT(s) --

    private const string S_SAVE_MESSAGE = "Reporting User configuration for selected user is %OPERATION% successfully!!!";
    private const string S_USER_EXISTS = "Reporting User configuration for selected user is already exist.";

    #endregion -- CONSTANT(s) --

    #region Data Member(s)

    ReportingUserConfigurationBL oReportingUserConfigurationBL = null;

    #endregion

    #region --Events(s)--
    /// <summary>
    /// This method is used to load data in listview firsttime.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, hidConfigId.Value.ToInt());
            oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId, miUserId);
            if (!IsPostBack)
            {
                RefreshValue();
                SetJavaScriptAttributes();
                FillReportingParameterCombo();
                FillRoleCombo();
                FillReportingConfigListview();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    Refresh();
                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                Refresh();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is called upon changing the role.As per the selected role users gets added.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbRole.SelectedValue != Constants.S_ZERO)
            {
                FillUsers(cmbRole.SelectedValue.ToInt());
                cmbUsers.Enabled = true;
            }
            else
            {
                cmbUsers.ClearSelection();
                cmbUsers.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to handle the edit and delete commands of listview.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstvwReportingParameter_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListViewDataItem oCurrentItem = e.Item as ListViewDataItem;
                hidConfigId.Value = (oCurrentItem.FindControl("ReportingId") as HiddenField).Value;
                switch (e.CommandName)
                {
                    case Constants.S_COMMAND_UPDATE:
                        int iRoleId = lstvwReportingParameter.DataKeys[oCurrentItem.DisplayIndex]["RoleId"].ToInt();
                        btnSave.Text = Constants.ButtonText.Update.ToString();
                        if (iRoleId != 0)
                        {
                            FillUsers(iRoleId);
                            SetReportingUserConfigDetails(hidConfigId.Value.ToInt());
                            cmbUsers.Enabled = true;
                        }
                        break;

                    //Following case is used to delete the Reporting related configuration.
                    case Constants.S_COMMAND_REMOVE:
                        int iUserId = Convert.ToInt32(lstvwReportingParameter.DataKeys[oCurrentItem.DisplayIndex]["UserId"]);
                        int iReportingParameterTypeId = Convert.ToInt32(lstvwReportingParameter.DataKeys[oCurrentItem.DisplayIndex]["ReportingPrameterId"]);
                        oReportingUserConfigurationBL.Delete(hidConfigId.Value.ToInt(), iReportingParameterTypeId);
                        hidConfigId.Value = string.Empty;
                        FillReportingConfigListview();
                        SetMessage(Resources.LocalizedResources.Deleted, false);
                        cmbRole.Enabled = true;
                        ClearFields();
                        break;
                }
            }
            lblErrorMsg.Text = string.Empty;
        }
        catch (SqlException ex)
        {
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is called to save configuration.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                ReportingUserConfiguration oReportingUserConfiguration = new ReportingUserConfiguration
                {
                    UserId = cmbUsers.SelectedValue.ToInt(),
                    ReportingTypeId = Convert.ToInt16(cmbReportingParameter.SelectedValue.ToInt()),
                    InsertedById = miUserId
                };  
                oReportingUserConfigurationBL.Save(oReportingUserConfiguration, hidConfigId.Value.ToInt());
                if (hidConfigId.Value.ToInt() == Constants.I_ZERO)
                {
                    SetMessage(Resources.LocalizedResources.Added, false);
                    ClearFields();
                }
                else if (hidConfigId.Value.ToInt() > Constants.I_ZERO)
                {
                    SetMessage(Resources.LocalizedResources.Updated, false);
                    ClearFields();
                }
                else
                    lblErrorMsg.Text = S_USER_EXISTS;

                FillReportingConfigListview();

                cmbRole.Enabled = true;
                if (QueryString["Is_Configured"] != Constants.S_YES)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ReportingUserConfiguration));
                cmbUsers.Enabled = false;
            }
            else
                lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    protected void lstvwReportingParameter_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ImageButton oimgbtnDelete = e.Item.FindControl("imgBtnDelete") as ImageButton;
                oimgbtnDelete.Attributes.Add("onclick", "if(!ConfirmDelete()) {return false;}");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This is called to clear all the fields and hidden variables.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            ClearFields();
            cmbRole.Enabled = true;
            cmbUsers.Enabled = false;
            cmbReportingParameter.Enabled = true;
            lblErrorMsg.Text = string.Empty;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion --Events(s)--

    #region --Method(s)--

    /// <summary>
    /// This method is used to fill the users.
    /// </summary>
    private void FillUsers(int aiRoleId)
    {
        List<ReportingUserConfiguration> olstReportingUserConfiguration = MasterDataCollectionBL.GetReportingUsers(aiRoleId, miSchoolId, miAcademicYearId);
        ListSource.FillDropDownList(olstReportingUserConfiguration, cmbUsers, "UserName", "UserId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is called to fill roles in combo.
    /// </summary>
    private void FillRoleCombo()
    {
        List<UserRoles> olstRoles = MasterDataCollectionBL.GetUserRoles();
        olstRoles = olstRoles.Where(rl => rl.User_Role_Id != Convert.ToInt32(Constants.UserRoles.ExAdmin)).ToList();
        ListSource.FillDropDownList(olstRoles, cmbRole, "User_Role_Name", "User_Role_Id", Constants.S_SELECT);
    }

    /// <summary>
    /// This method is used to fill Reporting Parameter Names.
    /// </summary>
    private void FillReportingParameterCombo()
    {
        List<ReportingParameter> olstParameters = oReportingUserConfigurationBL.GetAllReportingParameters();
        ListSource.FillDropDownList(olstParameters, cmbReportingParameter, "ReportingParameterName", "ReportingPrameterId", Constants.S_SELECT);
    }
    /// <summary>
    /// This method is called to set appropriate messages.
    /// </summary>
    /// <param name="asMessage"></param>
    /// <param name="abIsErrorMessage"></param>
    private void SetMessage(string asOperation, bool abIsErrorMessage)
    {
        lblErrorMsg.Text = string.Empty;
        lblUpdateMessage.Text = S_SAVE_MESSAGE.Replace("%OPERATION%", asOperation.ToLower());
        lblUpdateMessage.Font.Bold = true;
        if (abIsErrorMessage)
            lblUpdateMessage.ForeColor = Color.Red;
        else
            lblUpdateMessage.ForeColor = Color.Blue;
    }

    /// <summary>
    /// This method is called to set the javascript attributes on pageload.
    /// </summary>
    private void SetJavaScriptAttributes()
    {
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel, btnBack });
        btnBack.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Basic_Configuration));
        cmbReportingParameter.Focus();
        btnSave.Text = Constants.ButtonText.Save.ToString();
        cmbUsers.Items.Insert(Constants.I_ZERO, Constants.S_SELECT);
    }

    /// <summary>
    /// This method is called to clear the fields.
    /// </summary>
    private void ClearFields()
    {
        btnSave.Text = Constants.ButtonText.Save.ToString();
        hidConfigId.Value = Constants.S_ZERO;
        cmbRole.ClearSelection();
        cmbUsers.ClearSelection();
        cmbReportingParameter.ClearSelection();
    }

    /// <summary>
    /// This method is called to fill the listview.
    /// </summary>
    private void FillReportingConfigListview()
    {
        ReportingUserConfigurationBL oReportingUserConfigurationBL = new ReportingUserConfigurationBL(miSchoolId, miAcademicYearId);
        lstvwReportingParameter.DataSource = oReportingUserConfigurationBL.GetAll();
        lstvwReportingParameter.DataBind();
        if (lstvwReportingParameter.Items.Count == 0)
            DeleteConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.ReportingUserConfiguration));
    }

    private void Refresh()
    {
        hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
        RefreshValue();
    }

    /// <summary>
    /// This is used to getting the listview details.
    /// </summary>
    /// <param name="aiConfigId"></param>
    private void SetReportingUserConfigDetails(int aiConfigId)
    {
        ReportingUserConfiguration oReportingUserConfiguration = oReportingUserConfigurationBL.Get(aiConfigId);
        cmbRole.SelectedValue = oReportingUserConfiguration.RoleId.ToString();
        cmbUsers.SelectedValue = oReportingUserConfiguration.UserId.ToString();
        cmbReportingParameter.SelectedValue = oReportingUserConfiguration.ReportingPrameterId.ToString();
    }

    private void RefreshValue()
    {
        hidValBlankTimeSpan.Value = Resources.LocalizedResources.ValBlankTimeSpan;
        hidAlertDeleteUser.Value = Resources.LocalizedResources.AlertDeleteUser;
        hidValTimeSpan.Value = Resources.LocalizedResources.ValTimeSpan;
    }
    #endregion --Method(s)--

}