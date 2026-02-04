// File Name    : WeekDaysConfiguration.aspx   
// Created By   : Ketan     
// Created Date : 27/11/2007
// Description  : This class is used to configure school working days.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
/// This class provides user interface to configure and save the Weekdays.
/// </summary>
public partial class WeekDaysConfiguration :SchoolBase
{
    #region " Constant "

    const string S_ERR_MSG_SELECT_WEEKDAY = "At least one weekday name should be selected for saving.";
    const Int32 I_CHECKBOX_COLUMN_NUMBER = 0;
    const string S_DATAKEY_WEEKDAYS_ID = "WeekDays_Id";
    const string S_DATAKEY_SCHOOL_ID = "School_Id";
    const string S_DATAKEY_ORG_WEEKDAYS_ID = "Original_WeekDays_Id";  
    const string S_CHECKBOX_WEEKDAY = "ChkAllCheckedWeekDays";
    #endregion  " Constants "
    
    #region " Event "

    /// <summary>
    /// This method is used to fill weekdays grid and set default properties to page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                ReadQuerystring();
                FillWeekdayGrid();
                CheckIfOtherStaffApplicable();
                SetDefaultProperties();
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();

                }
            }
            if (hidCultureInfo.Value != Session[Constants.S_SESSION_LANGUAGE].ToString())
            {
                hidCultureInfo.Value = Session[Constants.S_SESSION_LANGUAGE].ToString();
                btnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdWeekDaysConfiguration.AllowPaging
                               + "','" + Resources.LocalizedResources.WeekDayErrorMessage + "'))){return false;}");
               // FillWeekdayGrid();
            }

        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to check grid checkbox for configured working days of school.  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdWeekDaysConfiguration_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                CheckBox chkIsSelected = (CheckBox)e.Row.Cells[I_CHECKBOX_COLUMN_NUMBER].FindControl(S_CHECKBOX_WEEKDAY);
                if (hidConfigurationFlag.Value == "Y")
                {
                    if (grdWeekDaysConfiguration.DataKeys[e.Row.RowIndex][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                        chkIsSelected.Checked = true;
                }
                ((TextBox)e.Row.FindControl("txtWeekdayShortName")).Text = grdWeekDaysConfiguration.DataKeys[e.Row.RowIndex]["WeekDay_Short_Name"].ToString();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to save weekdays configuration for school. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkIsSelected;
            Boolean chkIsOtherStaffSelected;
            WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();
            WeekEndMasterBL oWeekEndDayMasterBL = new WeekEndMasterBL();
            UserWeekEndAssociationBL oUserWeekEndAssociationBL = new UserWeekEndAssociationBL();
            WeekDaysConfigCollectionBL oWeekDaysConfigCollectionBL = new WeekDaysConfigCollectionBL();
            WeekEndConfigCollectionBL oWeekEndDayConfigCollectionBL = new WeekEndConfigCollectionBL();
            UserWeekEndConfigCollectionBL oUserWeekEndAssociationConfigCollectionBL = new UserWeekEndConfigCollectionBL();
            Collection<WeekDaysMasterBL> oWeekdaysCollection = new Collection<WeekDaysMasterBL>();
            Collection<WeekEndMasterBL> oWeekEndsCollection = new Collection<WeekEndMasterBL>();
            Collection<UserWeekEndAssociationBL> oUserWeekEndAssociationCollection = new Collection<UserWeekEndAssociationBL>();
            TextBox txtWeekDayShortName;
            chkIsOtherStaffSelected = IsOtherStaffApplicable.Checked;

            List<Int32> WeekendsList = oWeekEndDayMasterBL.GetAllWeeknds(miSchoolId, miAcademicYearId);
            for (int igrdRowCount = 0; igrdRowCount < grdWeekDaysConfiguration.Rows.Count; igrdRowCount++)
            {
                chkIsSelected = ((CheckBox)grdWeekDaysConfiguration.Rows[igrdRowCount].FindControl(S_CHECKBOX_WEEKDAY));
               
                Int32 iOrgWeekId = Convert.ToInt32(grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_ORG_WEEKDAYS_ID].ToString());
                string sWeekDayName = grdWeekDaysConfiguration.Rows[igrdRowCount].Cells[1].Text;
                txtWeekDayShortName = ((TextBox)grdWeekDaysConfiguration.Rows[igrdRowCount].FindControl("txtWeekdayShortName"));
                if (chkIsSelected.Checked == true && grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oWeekDaysMasterBL = PopulateWeekDayConfigurationBL(sWeekDayName, iOrgWeekId, 0, txtWeekDayShortName.Text);
                    oWeekDaysMasterBL.ConfigurationAction = Constants.Action.Insert;
                    oWeekdaysCollection.Add(oWeekDaysMasterBL);

                    if (WeekendsList.Contains(iOrgWeekId))
                    {
                        int iWeekendId = Convert.ToInt32(grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_WEEKDAYS_ID]);
                        oWeekEndDayMasterBL = PopulateWeekEndConfigurationBL(sWeekDayName, iOrgWeekId, iWeekendId, txtWeekDayShortName.Text, chkIsOtherStaffSelected);
                        oWeekEndDayMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oWeekEndsCollection.Add(oWeekEndDayMasterBL);

                        oUserWeekEndAssociationBL = PopulateUserWeekEndConfigurationBL(iOrgWeekId, chkIsOtherStaffSelected);
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Delete;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                }
                if (chkIsSelected.Checked == true && grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    oWeekDaysMasterBL = PopulateWeekDayConfigurationBL(sWeekDayName, iOrgWeekId, 0, txtWeekDayShortName.Text);
                    oWeekDaysMasterBL.ConfigurationAction = Constants.Action.Update;
                    oWeekdaysCollection.Add(oWeekDaysMasterBL);

                    if (WeekendsList.Contains(iOrgWeekId))
                    {
                        int iWeekendId = Convert.ToInt32(grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_WEEKDAYS_ID]);
                        oWeekEndDayMasterBL = PopulateWeekEndConfigurationBL(sWeekDayName, iOrgWeekId, iWeekendId, txtWeekDayShortName.Text, chkIsOtherStaffSelected);
                        oWeekEndDayMasterBL.ConfigurationAction = Constants.Action.Delete;
                        oWeekEndsCollection.Add(oWeekEndDayMasterBL);

                        oUserWeekEndAssociationBL = PopulateUserWeekEndConfigurationBL(iOrgWeekId, chkIsOtherStaffSelected);
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Delete;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                }
                else if (chkIsSelected.Checked == false && grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_SCHOOL_ID].ToString() != Constants.S_DEFAUL_SCHOOL_ID)
                {
                    int iWeekDayId = Convert.ToInt32(grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_WEEKDAYS_ID]);
                    oWeekDaysMasterBL = PopulateWeekDayConfigurationBL(sWeekDayName, iOrgWeekId, iWeekDayId, txtWeekDayShortName.Text);
                    oWeekDaysMasterBL.ConfigurationAction = Constants.Action.Delete;
                    oWeekdaysCollection.Add(oWeekDaysMasterBL);

                    oWeekEndDayMasterBL = PopulateWeekEndConfigurationBL(sWeekDayName, iOrgWeekId, iWeekDayId, txtWeekDayShortName.Text, chkIsOtherStaffSelected);
                    oWeekEndsCollection.Add(oWeekEndDayMasterBL);

                    oUserWeekEndAssociationBL = PopulateUserWeekEndConfigurationBL(iOrgWeekId, chkIsOtherStaffSelected);
                    if (chkIsOtherStaffSelected == true)
                    {
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Insert;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                    else
                    {
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Delete;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                }
                else if (chkIsSelected.Checked == false && grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_SCHOOL_ID].ToString() == Constants.S_DEFAUL_SCHOOL_ID)
                {
                    int iWeekendId = Convert.ToInt32(grdWeekDaysConfiguration.DataKeys[igrdRowCount][S_DATAKEY_WEEKDAYS_ID]);
                    oWeekEndDayMasterBL = PopulateWeekEndConfigurationBL(sWeekDayName, iOrgWeekId, iWeekendId, txtWeekDayShortName.Text, chkIsOtherStaffSelected);
                    oWeekEndsCollection.Add(oWeekEndDayMasterBL);

                    oUserWeekEndAssociationBL = PopulateUserWeekEndConfigurationBL(iOrgWeekId, chkIsOtherStaffSelected);
                    if (chkIsOtherStaffSelected == true)
                    {
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Insert;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                    else
                    {
                        oUserWeekEndAssociationBL.ConfigurationAction = Constants.Action.Delete;
                        oUserWeekEndAssociationCollection.Add(oUserWeekEndAssociationBL);
                    }
                }
            }

            if (oWeekdaysCollection.Count > 0)
            {
                oWeekDaysConfigCollectionBL.WeekdaysConfigListBL = oWeekdaysCollection;
                oWeekDaysConfigCollectionBL.UpdateAllWeekDayConfigurationDetails(miAcademicYearId);
            }

            if (oWeekEndsCollection.Count > 0)
            {
                oWeekEndDayConfigCollectionBL.WeekdaysConfigListBL = oWeekEndsCollection;
                oWeekEndDayConfigCollectionBL.UpdateAllWeekEndConfigurationDetails(miAcademicYearId);
            }

            if (oUserWeekEndAssociationCollection.Count > 0)
            {
                oUserWeekEndAssociationConfigCollectionBL.UserWeekendConfigListBL = oUserWeekEndAssociationCollection;
                oUserWeekEndAssociationConfigCollectionBL.UpdateAllUserWeekEndAssociationConfigurationDetails(miAcademicYearId);
            }

            if (hidConfigurationFlag.Value != "Y")
                ConfigureWeekDaysForGivenSchool();

            MasterPage oMasterPage = (MasterPage)this.Master;
            oMasterPage.RedirectToNextPage(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related)));
        }
        catch (BusinessLogic.Exceptions.ReferenceExceptions ex)
        {
            lblErr.Text = CommonUtility.ModifyExceptionMessage(ex.Message, " ", " " , "can not be removed since associated with", Resources.LocalizedResources.valRemoveText);
            FillWeekdayGrid();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "

    #region " Private Methods "

    /// <summary>
    /// This method is used to decrypt encrypted querystring.
    /// </summary>
    private void ReadQuerystring()
    {
        try
        {
	        if (QueryString.Count > 0 && QueryString["Is_Configured"] != null)
		        hidConfigurationFlag.Value = QueryString["Is_Configured"];
        }
        catch (Exception)
        {
            MasterPage oMasterPage = (MasterPage)this.Master; 
            oMasterPage.RedirectToNextPage(Constants.S_PAGE_ERROR);
        }
    }

    /// <summary>
    /// This method is used to set default properties to page controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        valSumErrorMsg.HeaderText = Resources.LocalizedResources.PleaseFixFollowingError;
        System.Web.UI.HtmlControls.HtmlForm oform = (HtmlForm)this.Master.FindControl("form1");
        oform.DefaultButton = btnSave.UniqueID;
        MasterPage oMasterPage = (MasterPage)this.Page.Master;
        oMasterPage.SetParentNodeURL(Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related)));
        btnCancel.PostBackUrl = Convert.ToString(CommonUtility.GetEncryptedQueryStringForConfigMenu(Constants.SchoolConfigMenuId.Attendance_Related));
        btnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { btnSave, btnCancel });
    }

    /// <summary>
    /// This method is used to populate data from gridview & insert into Weekdays_master table.
    /// </summary>
    /// <param name="aiSchoolID"></param>
    /// <param name="aigrdRowCount"></param>
    /// <returns></returns>
    private WeekDaysMasterBL PopulateWeekDayConfigurationBL(string asWeekDayName, int aiOrgWeekId, int aiWeekDayId, string asWeekDayShortName)
    {
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();

        oWeekDaysMasterBL.SchoolId = miSchoolId;
        oWeekDaysMasterBL.AcademicYearId = miAcademicYearId;
        oWeekDaysMasterBL.WeekDayName = asWeekDayName;
        oWeekDaysMasterBL.OriginalWeekDaysId = aiOrgWeekId;
        oWeekDaysMasterBL.WeekDaysId = aiWeekDayId;
        oWeekDaysMasterBL.UpdatedById = miUserId;
        oWeekDaysMasterBL.InsertedByid = miUserId;
        oWeekDaysMasterBL.WeekDayShortName = asWeekDayShortName;
        return oWeekDaysMasterBL;
    }

    /// <summary>
    /// This method is used to populate ConfigurationSchoolMasterBL to configure entry of weekdays for school.
    /// </summary>
    private void ConfigureWeekDaysForGivenSchool()
    {
        ConfigurationSchoolMasterBL oConfiguration = new ConfigurationSchoolMasterBL();
        oConfiguration.SchoolId = miSchoolId;
        oConfiguration.OriginalConfigId = Convert.ToInt32(Constants.SchoolConfigurations.WeekDaysConfiguration);
        oConfiguration.AcademicYearId = miAcademicYearId;
        if (!oConfiguration.IsSchoolConfigured())
        {
            oConfiguration.IsConfigure = Constants.C_YES;
            oConfiguration.InsertedById = miUserId;
            oConfiguration.UpdateById = miUserId;
            oConfiguration.InsertConfigurationSchoolMaster();
        }
    }

    /// <summary>
    /// This method is used to retrieve the Data in Weekdaysgridview.
    /// </summary>
    private void FillWeekdayGrid()
    {
        WeekDaysMasterBL oWeekDaysMasterBL = new WeekDaysMasterBL();
        DataTable oDTWeekDaysConfigDetails = oWeekDaysMasterBL.GetAllWeekDayConfigurationDetalis(miSchoolId, miAcademicYearId);
        grdWeekDaysConfiguration.DataSource = oDTWeekDaysConfigDetails.DefaultView;
        grdWeekDaysConfiguration.DataBind();
        btnSave.Attributes.Add("Onclick", "if(!(ConfirmAction('" + grdWeekDaysConfiguration.AllowPaging
                                + "','" + Resources.LocalizedResources.WeekDayErrorMessage + "'))){return false;}");
    }

    /// <summary>
    /// This method is used to populate data from gridview & insert into WeekEnd master table.
    /// </summary>
    /// <param name="aiSchoolID"></param>
    /// <param name="aigrdRowCount"></param>
    /// <returns></returns>
    private WeekEndMasterBL PopulateWeekEndConfigurationBL(string asWeekEndName, int aiOrgWeekEndId, int aiWeekEndId, string asWeekEndShortName, bool abIsOtherStaffApplicable)
    {
        WeekEndMasterBL oWeekDaysMasterBL = new WeekEndMasterBL();

        oWeekDaysMasterBL.SchoolId = miSchoolId;
        oWeekDaysMasterBL.AcademicYearId = miAcademicYearId;
        oWeekDaysMasterBL.WeekEndName = asWeekEndName;
        oWeekDaysMasterBL.OriginalWeekDaysId = aiOrgWeekEndId;
        oWeekDaysMasterBL.WeekDaysId = aiWeekEndId;
        oWeekDaysMasterBL.UpdatedById = miUserId;
        oWeekDaysMasterBL.InsertedByid = miUserId;
        oWeekDaysMasterBL.IsStaffApplicable = abIsOtherStaffApplicable;
        oWeekDaysMasterBL.WeekDayShortName = asWeekEndShortName;
        return oWeekDaysMasterBL;
    }

    /// <summary>
    /// This method is used to check or uncheck checbox of Is other staff applicable.
    /// </summary>
    private void CheckIfOtherStaffApplicable()
    {
        WeekEndMasterBL oWeekDaysMasterBL = new WeekEndMasterBL();
        IsOtherStaffApplicable.Checked = oWeekDaysMasterBL.ChkIfOtherStaffApplicable(miSchoolId, miAcademicYearId);
    }

    /// <summary>
    /// This method is used to populate data from gridview & insert into WeekEnd master table.
    /// </summary>
    /// <param name="aiSchoolID"></param>
    /// <param name="aigrdRowCount"></param>
    /// <returns></returns>
    private UserWeekEndAssociationBL PopulateUserWeekEndConfigurationBL(int aiOrgWeekEndId, bool chkIsOtherStaffSelected)
    {
        UserWeekEndAssociationBL oUserWeekndAssociationMasterBL = new UserWeekEndAssociationBL();

        oUserWeekndAssociationMasterBL.SchoolId = miSchoolId;
        oUserWeekndAssociationMasterBL.AcademicYearId = miAcademicYearId;
        oUserWeekndAssociationMasterBL.WeekEndId = aiOrgWeekEndId;
        oUserWeekndAssociationMasterBL.UpdatedById = miUserId;
        oUserWeekndAssociationMasterBL.InsertedById = miUserId;
        oUserWeekndAssociationMasterBL.IsOtherStaffApplicable = chkIsOtherStaffSelected;
        return oUserWeekndAssociationMasterBL;
    }


    #endregion
}
