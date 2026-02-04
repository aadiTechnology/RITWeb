// File Name  : IssueBookUI.aspx.cs
// Created By : Ashish
// Date       : 17/09/2008
//Description : This class is used to add/edit library settings.
//              If library setting is set first time then set configuration flag after adding library setting in database.
//Modified by: Rohini
//Date:23/12/2011
//Description:code review

using System;
using System.Data;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;
using System.Collections.Generic;

public partial class LibraryConfigurationUI : SchoolBase
{
    #region " Event "

    /// <summary>
    /// This event is used to fill grid view, user role combo box etc.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {			
            if (!IsPostBack)
            {
                FillLibraryConfigGridView();
                FillUserRolesCombo();
                SetDefaultControls();
                SetClientScriptAttributes();
            }
            lblErrorMsg.Visible = false;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to fill library setting in the respected controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void cmbLibraryConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbLibraryConfig.SelectedIndex != Constants.I_ZERO)
            {
                int iUserRoleId = Convert.ToInt32(cmbLibraryConfig.SelectedItem.Value);              
                LibraryConfigurationBL oLibraryConfigurationBL = new LibraryConfigurationBL(iUserRoleId, miSchoolId, miAcademicYearId);
                SetLibraryDetails(oLibraryConfigurationBL);
            }
            else
                ResetControls();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This method is used for going to source page named-ControlPanel.aspx.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            MasterPage oMasterPage =this.Master as MasterPage;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Library_Related)));
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to save library settings.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            LibraryConfigurationBL oLibraryConfigurationBL = PopulateLibraryConfigBL();
            if (hidNewConfig.Value == "true")
            {   
                oLibraryConfigurationBL.AddLibraryConfigurarion();

                if (QueryString["Is_Configured"] != null && QueryString["Is_Configured"] == Constants.S_NO)
					SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LibrarySettings));

                FillLibraryConfigGridView();
                if (grdvwLibraryConfig.Rows.Count == Constants.I_ONE)
                {  
                    ConfigureLibraryForGivenSchool(Convert.ToInt32(Constants.SchoolConfigurations.IssuePeriod));
                    ConfigureLibraryForGivenSchool(Convert.ToInt32(Constants.SchoolConfigurations.RenewAttempts));
                    ConfigureLibraryForGivenSchool(Convert.ToInt32(Constants.SchoolConfigurations.BookPerPerson));
                }
            }
            else if (hidNewConfig.Value == "false")
            {
                if (QueryString["Is_Configured"] != null && QueryString["Is_Configured"] == Constants.S_NO)
                    SaveConfigDetails(Convert.ToInt32(Constants.SchoolConfigurations.LibrarySettings));
                oLibraryConfigurationBL.LibConfigId = Convert.ToInt32(hidLibConfigId.Value);
                oLibraryConfigurationBL.UpdateLibraryConfigurarion();
                FillLibraryConfigGridView();
            }
            ResetControls();
            cmbLibraryConfig.SelectedIndex = Constants.I_ZERO;
        }
        catch (BusinessLogic.Exceptions.DuplicateEntityException ex)
        {
            lblErrorMsg.Visible = true;
            lblErrorMsg.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to kept all feild clear/Blank.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnNew_Click(object sender, EventArgs e)
    {
        try
        {
            ResetControls();
            cmbLibraryConfig.SelectedIndex = Constants.I_ZERO;
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }
    /// <summary>
    /// This event is used to bound default control to the grid view,
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwLibraryConfig_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            const Int32 I_COL_INDEX_ISSUE_PERIOD = 1;
            const Int32 I_COL_INDEX_RENEW_ATTEMPT = 2;
            const Int32 I_COL_INDEX_LATE_FEE_PER_DAY = 4;
            const Int32 I_COL_INDEX_LATE_FEE_EFFECTIVE_FROM = 5;
            const Int32 I_COL_INDEX_RESERVE_BOOK= 6; 

            if (e.Row.RowIndex >= Constants.I_ZERO)
            {
                e.Row.Cells[I_COL_INDEX_ISSUE_PERIOD].Text =e.Row.Cells[I_COL_INDEX_ISSUE_PERIOD].Text!=Constants.S_ZERO?e.Row.Cells[I_COL_INDEX_ISSUE_PERIOD].Text :"N/A";
                e.Row.Cells[I_COL_INDEX_RENEW_ATTEMPT].Text = e.Row.Cells[I_COL_INDEX_RENEW_ATTEMPT].Text != Constants.S_ZERO ? e.Row.Cells[I_COL_INDEX_RENEW_ATTEMPT].Text : "N/A";
                e.Row.Cells[I_COL_INDEX_LATE_FEE_PER_DAY].Text =e.Row.Cells[I_COL_INDEX_LATE_FEE_PER_DAY].Text!=Constants.S_ZERO?e.Row.Cells[I_COL_INDEX_LATE_FEE_PER_DAY].Text:"N/A";
                e.Row.Cells[I_COL_INDEX_LATE_FEE_EFFECTIVE_FROM].Text =e.Row.Cells[I_COL_INDEX_LATE_FEE_EFFECTIVE_FROM].Text !=Constants.S_ZERO? e.Row.Cells[I_COL_INDEX_LATE_FEE_EFFECTIVE_FROM].Text :"N/A";
                e.Row.Cells[I_COL_INDEX_RESERVE_BOOK].Text = e.Row.Cells[I_COL_INDEX_RESERVE_BOOK].Text != Constants.S_ZERO ? e.Row.Cells[I_COL_INDEX_RESERVE_BOOK].Text : "N/A";
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region Private Method
    /// <summary>
    /// This method is used to add school configuration data.
    /// </summary>
    private void AddSchoolConfigurationBL(int aiOriginalConfigId,bool abIsAdd)
    {
        ConfigurationSchoolMasterBL oConfiguration = PopulateSchoolConfigurationBL();
        oConfiguration.OriginalConfigId = aiOriginalConfigId;
        if(abIsAdd)
            oConfiguration.InsertConfigurationSchoolMaster();
        else
            oConfiguration.UpdateConfigurationSchoolMaster();
    }
    /// <summary>
    /// This method is used to populate school configuration data.
    /// </summary>
    private ConfigurationSchoolMasterBL PopulateSchoolConfigurationBL()
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL
        {
            SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]),
            AcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR]),
            IsConfigure = Constants.C_YES,
            InsertedById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]),
            UpdateById = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID])
        };
        return oConfiguration;
    }
    /// <summary>
    /// This method is used to check whether library settings are configured or not.
    /// </summary>
    private void ConfigureLibraryForGivenSchool(int aiOriginalConfigId)
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL
        {
            SchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]),
            OriginalConfigId = aiOriginalConfigId,
            AcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR])
        };
        if (!oConfiguration.IsSchoolConfigured())
            AddSchoolConfigurationBL(aiOriginalConfigId,true);
        else
            AddSchoolConfigurationBL(aiOriginalConfigId, false);
    }
    /// <summary>
    /// This method is used to fill user role combo box.
    /// </summary>
    private void FillUserRolesCombo()
    {   
        LibraryConfigurationBL oLibraryConfigurationBL = new LibraryConfigurationBL();
        DataTable oDTUserRoles = oLibraryConfigurationBL.GetUserRoles();

        ControlUtility.FillDropDownList(oDTUserRoles, ref cmbLibraryConfig,
                                     "User_Role_Id",
                                     "User_Role_Name",
                                     "--Select--");
    }
    /// <summary>
    /// This method is used to set default control on page load.
    /// </summary>
    private void SetDefaultControls()
    {
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        hidNewConfig.Value = "true";
    }
    /// <summary>
    /// This method is used to fill grid view.
    /// </summary>
    private void FillLibraryConfigGridView()
    {
        LibraryConfigurationBL oLibraryConfigurationBL = new LibraryConfigurationBL
                                                             {
                                                                 SchoolId =Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]),
                                                                 AcademicYearId =Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID])
                                                             };

        DataTable oDTLibDetail = oLibraryConfigurationBL.RetriveLibraryConfigurarion();
        grdvwLibraryConfig.DataSource = oDTLibDetail.DefaultView;
        grdvwLibraryConfig.DataBind();
    }
    /// <summary>
    /// This method is used to initilized library setting.
    /// </summary>
    /// <returns></returns>
    private LibraryConfigurationBL PopulateLibraryConfigBL()
    {
        LibraryConfigurationBL oLibraryConfigurationBL = new LibraryConfigurationBL
                                                             {
                                                                 SchoolId =miSchoolId,
                                                                 AcademicYearId =miAcademicYearId,
                                                                 UpdatedById =miUserId,
                                                                 InsertedById =miUserId,
                                                                 UserId =miUserId,
                                                                 UpdatedDate = System.DateTime.Today,
                                                                 UserRoleID =Convert.ToInt32(cmbLibraryConfig.SelectedItem.Value),
                                                                 RenewAttempt = Convert.ToInt32(txtAttemptToRenew.Text),
                                                                 ReturnDays = Convert.ToInt32(txtReturnDay.Text),
                                                                 BookPerPerson = Convert.ToInt32(txtBookPerPerson.Text),
                                                                 LateFeePerDay = Convert.ToInt32(txtLateFee.Text),
                                                                 LateFeeEffectiveDays =Convert.ToInt32(txtEffectiveLateFee.Text),
                                                                 ReserveBooks = Convert.ToInt32(txtReserveBooks.Text)
                                                             };

        return oLibraryConfigurationBL;
    }
    /// <summary>
    /// This method is used to clear all control values.
    /// </summary>
    private void ResetControls()
    {
        txtAttemptToRenew.Text = string.Empty;
        txtBookPerPerson.Text = string.Empty;
        txtEffectiveLateFee.Text = string.Empty;
        txtLateFee.Text = string.Empty;
        txtReturnDay.Text = string.Empty;        
        hidNewConfig.Value = "true";
        hidLibConfigId.Value = "";
        cmbLibraryConfig.Enabled = true;
        txtReserveBooks.Text = string.Empty;
    }
    /// <summary>
    /// This method is used to set javascript attribute on page load event.
    /// </summary>
    private void SetClientScriptAttributes()
    {
        btnNew.Attributes.Add("onclick", "javascript:ClearText();");     
        grdvwLibraryConfig.Attributes.Add("onclick", "javascript:ClearText();");        
		ApplyMouseHoverEffect(new List<Button>(){ btnBack, btnNew, btnSave });
    }
    /// <summary>
    /// This method is used to Assign value to the library setting configuration.
    /// </summary>
    /// <param name="oLibraryConfigurationBL"></param>
    private void SetLibraryDetails(LibraryConfigurationBL aoLibraryConfigurationBL)
    {

        hidNewConfig.Value = "true";
        if (aoLibraryConfigurationBL.LibConfigId != Constants.I_ZERO)
        {
            hidLibConfigId.Value = aoLibraryConfigurationBL.LibConfigId.ToString();
            hidNewConfig.Value = "false";
            txtAttemptToRenew.Text = Convert.ToString(aoLibraryConfigurationBL.RenewAttempt);
            txtBookPerPerson.Text = Convert.ToString(aoLibraryConfigurationBL.BookPerPerson);
            txtEffectiveLateFee.Text = Convert.ToString(aoLibraryConfigurationBL.LateFeeEffectiveDays);
            txtLateFee.Text = Convert.ToString(aoLibraryConfigurationBL.LateFeePerDay);
            txtReturnDay.Text = Convert.ToString(aoLibraryConfigurationBL.ReturnDays);
            cmbLibraryConfig.SelectedValue = Convert.ToString(aoLibraryConfigurationBL.UserRoleID);
            int iIndex = Convert.ToInt32(cmbLibraryConfig.SelectedIndex.ToString());
            cmbLibraryConfig.SelectedIndex = iIndex;
            txtReserveBooks.Text = aoLibraryConfigurationBL.ReserveBooks.ToString();
        }
        else
            ResetControls();
    }
    #endregion

}
