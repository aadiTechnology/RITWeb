/* File Name    :   SchoolConfigurationControlPanel.aspx.cs
 * Purpose      :   This class is used to fill menu control and grid view as per user role.
 * File Modified:   6 Jan 2009
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class SchoolConfigurationControlPanel : SchoolBase
{

    #region " Constant "

    const int I_ASSIGN_EXTRA_SCREENS_TO_TEACHER = 87;
    const int I_OTHER_USERS = 89;

    const string S_SCREEN_ID_FEILD = "Screen_Id";
    const string S_CONFIGURARION_NAME_FEILD = "Configure_Name";

    const string S_DATAKEY_CONFIGURE_ID = "Configure_Id";
    const string S_DATAKEY_IS_CONFIGURE = "Is_Configure";
    const string S_DATAKEY_NAVIGATE_URL = "NavigateURL";

    #endregion

    #region " Event "

    /// <summary>
    /// This event is used to load information in menu and display related information to the grid view.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
			this.RemoveSession(Constants.S_SESSION_USER_IMAGE_DATA);      
            if (!IsPostBack)
            {
                ClearTempSessionVariables();
                VisibleOrHideControls();
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }   

    /// <summary>
    /// This event is used to fill menu grid view for user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ConfigurationMenu_Click(object sender, MenuEventArgs e)
    {
        try
        {
            int iParentId = Convert.ToInt32(e.Item.Value);
            hidScreenParentId.Value = iParentId.ToString();
            if (moUserRole == Constants.UserRoles.Admin)
            {
                FillGridViewForAdmin(iParentId);
            }
            else if (moUserRole == Constants.UserRoles.Supervisor || moUserRole == Constants.UserRoles.Teacher)
            {
                FillGridViewForSupervisor(iParentId);
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is handled to set navigate url and status image.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void grdvwConfigurationMenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
                Image oStatus = (Image)e.Row.Cells[0].Controls[Constants.I_ZERO];
                HyperLink oConfigurationName = (HyperLink)e.Row.Cells[1].Controls[Constants.I_ZERO];
                string sQueryString = "Is_Configured=" + grdvwConfigurationMenu.DataKeys[e.Row.RowIndex][S_DATAKEY_IS_CONFIGURE].ToString();
                string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQueryString);
                string sNavigateUrl = Convert.ToString(grdvwConfigurationMenu.DataKeys[e.Row.RowIndex][S_DATAKEY_NAVIGATE_URL]) + "?" + sEncrypt;
                oConfigurationName.NavigateUrl = sNavigateUrl;

                if ((grdvwConfigurationMenu.DataKeys[e.Row.RowIndex]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibrarySettings.ToInt() || grdvwConfigurationMenu.DataKeys[e.Row.RowIndex]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.CategoryManagement.ToInt() || grdvwConfigurationMenu.DataKeys[e.Row.RowIndex]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibraryVendors.ToInt() || grdvwConfigurationMenu.DataKeys[e.Row.RowIndex]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.GenerateBarcode.ToInt())&& Settings.ExternalLibrarySite!=string.Empty)
                    oConfigurationName.Target = "blank";

                if (oConfigurationName.Text == "Supervisor")
                    oConfigurationName.Text = Constants.S_SUPERVISOR_ROLE_NAME;

                if (grdvwConfigurationMenu.DataKeys[e.Row.RowIndex][S_DATAKEY_IS_CONFIGURE].ToString() == Constants.C_YES.ToString())
                {
                    oStatus.ImageUrl = "~/RITeSchool/images/IconGrid_AssignTrue.gif";
                    oStatus.ImageAlign = ImageAlign.Middle;
                }
                else
                {
                    oStatus.ImageUrl = "~/RITeSchool/images/IconGrid_Delete.gif";
                    oStatus.ImageAlign = ImageAlign.Middle;
                }
            }
            //Condition is used to hide "LectureTimingUI" screen from configuration menu.
            if (grdvwConfigurationMenu.Rows.Count == Constants.I_THREE)
            {
                int iRowId = grdvwConfigurationMenu.Rows.Count - 1;
                if (Convert.ToInt32(grdvwConfigurationMenu.DataKeys[2][S_DATAKEY_CONFIGURE_ID]) == Convert.ToInt32(Constants.SchoolConfigurations.LectureTiming))
                    grdvwConfigurationMenu.Rows[iRowId].Visible = false;
            }

            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion

    #region " Private Method "
        
    /// <summary>
    /// This method is used to get query string and set menu item index as per query string value.
    /// </summary>
    private void GetQueryString(DataTable aoDTMenuDetails, List<int> aoScreenIdList)
    {
        if (QueryString.Count != Constants.I_ZERO)
        {
            if (QueryString["MenuId"] != null)
            {
                int iScreenId = QueryString["MenuId"].ToInt();
                if ((iScreenId == I_ASSIGN_EXTRA_SCREENS_TO_TEACHER || iScreenId == I_OTHER_USERS) &&
                    !aoScreenIdList.Contains(iScreenId))
                    iScreenId = aoScreenIdList[0];                
                hidScreenParentId.Value = iScreenId.ToString();
                DataRow[] oDRMenu = aoDTMenuDetails.Select("Screen_Id=" + iScreenId);
                if (oDRMenu.Length > 0)
                {
                    int iIndex = Convert.ToInt32(oDRMenu[0]["RowIndex"].ToString()) - 1;
                    ConfigurationMenu.Items[iIndex].Selected = true;
                }               
            }
        }
        else
            ConfigurationMenu.Items[0].Selected = true;
    }

    /// <summary>
    /// This method is used to redirect control to control panel.
    /// </summary>
    private void RedirectToControlPanel()
    {
        MasterPage oMasterPage = (MasterPage)this.Master;
        oMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
    }

    /// <summary>
    /// This method sets the temparary session variables to null.
    /// </summary>
    private void ClearTempSessionVariables()
    {
        Session[Constants.S_TEMP_SESSION_DS] = null;
    }

    /// <summary>
    /// This method is used to visible or hide controls according to user role. 
    /// </summary>
    private void VisibleOrHideControls()
    {
        int iParentId = 0;
        if (moUserRole == Constants.UserRoles.Admin)
        {
            FillMenuItemsForAdmin();
            iParentId = Convert.ToInt32(hidScreenParentId.Value);
            FillGridViewForAdmin(iParentId);
        }
        else if (moUserRole == Constants.UserRoles.Supervisor ||  moUserRole == Constants.UserRoles.Teacher)
        {
            FillMenuItemsForSupervisor();
            iParentId = Convert.ToInt32(hidScreenParentId.Value);
            FillGridViewForSupervisor(iParentId);
        }
    }
    
    /// <summary>
    /// This method is used to fill menu control for admin.
    /// </summary>
    private void FillMenuItemsForAdmin()
    {
        int iScreenLevel = Convert.ToInt32(Constants.ScreenLevel.SchoolConfiguration);
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDTMenuDetails = oMasterDataCollectionBL.GetMenuItemDetails(iScreenLevel);
        FillMenuDetails(oDTMenuDetails);
    }
    
    /// <summary>
    /// This method is used to fill menu control for supervisor.
    /// </summary>
    private void FillMenuItemsForSupervisor()
    {
        int iScreenLevel = Convert.ToInt32(Constants.ScreenLevel.SchoolConfiguration);        
        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTMenuDetails = oSchoolWiseSupervisorMasterBL.GetScreenAccessDetails(miUserId, iScreenLevel);
        if (oDTMenuDetails != null && oDTMenuDetails.Rows.Count > 0 && oDTMenuDetails.Rows[0][0] != DBNull.Value)
            FillMenuDetails(oDTMenuDetails);        
        else
            RedirectToControlPanel();
    }

    /// <summary>
    /// This method is used to fill menu control details.
    /// </summary>
    private void FillMenuDetails(DataTable aoDTMenuDetails)
    {
        List<int> oScreenIdList = new List<int>(); 
        oScreenIdList.Clear();
        for (int iCount = 0; iCount < aoDTMenuDetails.Rows.Count; iCount++)
        {
            MenuItem oMenuItem = new MenuItem();
            oMenuItem.Text = aoDTMenuDetails.Rows[iCount][S_CONFIGURARION_NAME_FEILD].ToString();
            oMenuItem.Value = aoDTMenuDetails.Rows[iCount][S_SCREEN_ID_FEILD].ToString();
            
                ConfigurationMenu.Items.Add(oMenuItem);
                oScreenIdList.Add(Convert.ToInt32(oMenuItem.Value));          
            
        }

        GetQueryString(aoDTMenuDetails, oScreenIdList);
        if (QueryString.Count == Constants.I_ZERO)
            hidScreenParentId.Value = aoDTMenuDetails.Rows[0][S_SCREEN_ID_FEILD].ToString();
    }

    /// <summary>
    /// This method is used to fill grid view for admin.
    /// </summary>
    private void FillGridViewForAdmin(int iParentId)
    {
        int iScreenLevel = Convert.ToInt32(Constants.ScreenLevel.Configuration);

        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        DataTable oDSUserDetails = oMasterDataCollectionBL.GetConfigurationDetails(miSchoolId, miAcademicYearId, miFinancialYearId,  iParentId, iScreenLevel, miUserId, moUserRole.ToInt());
        if(Settings.ExternalLibrarySite !=string.Empty)
        for (int i = 0; i < oDSUserDetails.Rows.Count; i++)
        {
            if (oDSUserDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibrarySettings.ToInt() || oDSUserDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.CategoryManagement.ToInt() || oDSUserDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibraryVendors.ToInt() || oDSUserDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.GenerateBarcode.ToInt())
                oDSUserDetails.Rows[i]["NavigateURL"] = Settings.ExternalLibrarySite;

        }
            grdvwConfigurationMenu.DataSource = oDSUserDetails.DefaultView;
        grdvwConfigurationMenu.DataBind();
    }

    /// <summary>
    /// This method is used to fill grid view superviosr.
    /// </summary>
    private void FillGridViewForSupervisor(int iParentId)
    {
        SchoolWiseSupervisorMasterBL oSchoolWiseSupervisorMasterBL = new SchoolWiseSupervisorMasterBL();
        DataTable oDTSuperviosrDetails = oSchoolWiseSupervisorMasterBL.GetSuperviosrDetails(miSchoolId, miAcademicYearId, miFinancialYearId, miUserId, iParentId, moUserRole.ToInt());
        if (Settings.ExternalLibrarySite != string.Empty)
            for (int i = 0; i < oDTSuperviosrDetails.Rows.Count; i++)
            {
                if (oDTSuperviosrDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibrarySettings.ToInt() || oDTSuperviosrDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.CategoryManagement.ToInt() || oDTSuperviosrDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.LibraryVendors.ToInt() || oDTSuperviosrDetails.Rows[i]["Configure_Id"].ToInt() == Constants.SchoolConfigurations.GenerateBarcode.ToInt())
                    oDTSuperviosrDetails.Rows[i]["NavigateURL"] = Settings.ExternalLibrarySite;

            }
        grdvwConfigurationMenu.DataSource = oDTSuperviosrDetails.DefaultView;
        grdvwConfigurationMenu.DataBind();
    }

    #endregion

}
