// File Name     : RegistrationWizard_Step2.aspx.cs
// Modified By   : Amit 
// Modified Date : 25/09/2009
// Description   : This class is used to save admin user for school.

using System;
using System.Web;
using System.Web.UI;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class RegistrationWizard_Step2 :SchoolBase
{
    #region " Constants "

    const string S_USER_ID = "UserId";
    const string S_DEFAULT_MENU_NAME = "About School";

    #endregion " Constants "

    #region " Events "

    /// <summary>
    /// This event is used to set default properties to page controls.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultProperties();
                SetUserId();
                if (hidUserId.Value != Constants.S_EMPTY_STRING)
                    PopulateUserDetails();
                SetClientSideScriptAttributes();
            }

            if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] == null && Session[Constants.S_SESSION_SCHOOL_ID] != null)
            {
                imgBtnCancel.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used save school admin user details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            // Check the mode of the form. 
            // If user id from the hidden field is blank then add the user in the system.  
            if (hidUserId.Value == "")
            {
                //Registration in progress.
                if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
                {
                    SchoolBL oSchoolDetails = (SchoolBL)Session["O_SCHOOL_DETAILS"];
                    oSchoolDetails.DefaultMenuInfo = CreateAndGetObjectForMenuDetails();
                    oSchoolDetails.SchoolUserInfo = CreateAndGetObjectForUserDetails();
                    int iSchoolId = oSchoolDetails.InsertSchoolDetails();
                    if (iSchoolId == 0)
                    {
                        lblErrorMsg.Text = Constants.S_COMMON_ERROR_MESSAGE;
                        lblErrorMsg.Visible = true;
                    }
                    else
                    {
                        SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                        oSuperAdminMasterPage.RedirectToNextPage("../SuperAdmin/RegistrationWizard_Step3.aspx");
                    }
                }
                else
                {
                    SchoolUserBL oSchoolUserBL = CreateAndGetObjectForUserDetails();
                    oSchoolUserBL.SchoolId = miSchoolId;
                    int iUserId = oSchoolUserBL.InsertSchoolUserDetails();
                    if (iUserId == 0)
                    {
                        lblErrorMsg.Text = Constants.S_COMMON_ERROR_MESSAGE;
                        lblErrorMsg.Visible = true;
                    }
                    else
                    {
                        SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                        oSuperAdminMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
                    }
                }
            }
            else
            {
                //Update user.
                SchoolUserBL oSchoolUserBL = CreateAndGetObjectForUserDetails();
                oSchoolUserBL.UserId = Convert.ToInt32(hidUserId.Value);
                oSchoolUserBL.SchoolId = miSchoolId;
                oSchoolUserBL.UpdateSchoolUser();
                if (moUserRole == Constants.UserRoles.Admin)
                {
                    Session[Constants.S_SESSION_USER_NAME] = oSchoolUserBL.Login;
                }
                SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                oSuperAdminMasterPage.RedirectToNextPage(Constants.S_PAGE_CONTROL_PANEL);
            }
        }
        catch (DuplicateUserException ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }


    /// <summary>
    /// This event is used to move back to dashboard screen. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
            {
                SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                oSuperAdminMasterPage.RedirectToNextPage("../SuperAdmin/ScreensUI.aspx");
            }
            else if (Session[Constants.S_SESSION_SCHOOL_ID] == null)
            {
                SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master; 
                oSuperAdminMasterPage.RedirectToNextPage("Home.aspx");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion " Events "


    #region " Private Methods "

    /// <summary>
    /// This method is used to set default properties to page controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
            Session.Remove(Constants.S_SESSION_SCHOOL_ID);
        cmbSalutation.Focus();
        MasterDataCollectionBL oMasterDataCollectionBL = new MasterDataCollectionBL();
        oMasterDataCollectionBL.FillSalutationComboBox(ref cmbSalutation);
        oMasterDataCollectionBL.FillDesignationCombobox(ref cmbDesignations);
        txtLogin.ToolTip = Constants.S_LOGIN_TOOL_TIP;
        txtPasswd.ToolTip = Constants.S_PASSWORD_RELATED_NOTE;
        txtConfirmPasswd.ToolTip = Constants.S_PASSWORD_RELATED_NOTE;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        if (Session["I_SCHOOL_ID"] == null)
            tdUserLbl.Visible = true;
    }

    /// <summary>
    /// This method is used to set java script properties to page controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        imgBtnSubmit.Attributes.Add("Onclick", "ResetErrorMsgLbl();");
        imgBtnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { imgBtnSubmit, imgBtnCancel });
    }

    /// <summary>
    /// This method is used to set used id as per login user.
    /// </summary>
    private void SetUserId()
    {
        if (Session[Constants.S_SESSION_SCHOOL_ID] != null)
        {
	        if (Request.QueryString.ToString().Equals(Constants.S_EMPTY_STRING))
		        hidUserId.Value = miUserId.ToString();
	        else
	        {
		        int iUserRoleId = QueryString["User_Role_Id"].ToInt();
		        if (QueryString.Count > 0)
		        {
			        if (QueryString["S_USER_ID"] != null)
				        hidUserId.Value = QueryString["S_USER_ID"];
			        else if (iUserRoleId == Constants.UserRoles.Teacher.ToInt())
			        {
				        hidUserId.Value = "";
				        hidUserRoleId.Value = Constants.UserRoles.Teacher.ToString();
			        }
		        }
	        }
        }
    }

    /// <summary>
    /// This method is used to populate SchoolUserBL which is used used to create school admin user.
    /// </summary>
    /// <returns></returns>
    private SchoolUserBL CreateAndGetObjectForUserDetails()
    {
        // Create the user role's object for the available values.
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.FirstName = txtFirstName.Text.Trim();
        if (txtMiddleName.Text != Constants.S_EMPTY_STRING)
            oSchoolUserBL.MiddleName = txtMiddleName.Text.Trim();
        oSchoolUserBL.LastName = txtLastName.Text.Trim();
        oSchoolUserBL.Email = txtEmail.Text.Trim();
        oSchoolUserBL.Login = txtLogin.Text.Trim();
        oSchoolUserBL.Mobile_Number = txtMobileNo.Text.Trim();
        oSchoolUserBL.Password = txtPasswd.Text;
        oSchoolUserBL.UserRoleId = Convert.ToInt32(Constants.UserRoles.Admin);
        oSchoolUserBL.UpdatedBy = Convert.ToString(miUserId);
        oSchoolUserBL.SalutationId = Convert.ToInt32(cmbSalutation.SelectedValue);
        oSchoolUserBL.DesignationId = Convert.ToInt32(cmbDesignations.SelectedValue);
        oSchoolUserBL.CanApproveRequisition = 'N';
        oSchoolUserBL.CanCreateGeneralRequisition = 'N';
        oSchoolUserBL.CanSanctionLeave = 'N';
        return oSchoolUserBL;
    }  

    /// <summary>
    /// This method is used to populate ConfigureMenuBL object used to create menu.
    /// </summary>
    /// <returns></returns>
    private ConfigureMenuBL CreateAndGetObjectForMenuDetails()
    {
        ConfigureMenuBL oConfigureMenuBL = new ConfigureMenuBL();
        oConfigureMenuBL.ConfigureMenuContent = "";
        oConfigureMenuBL.ConfigureMenuName = S_DEFAULT_MENU_NAME;
        oConfigureMenuBL.IsExternal = Constants.C_YES;
        oConfigureMenuBL.IsDefault = Constants.C_YES;
        oConfigureMenuBL.Priority = 10;
        oConfigureMenuBL.ParentMenuId = 0;
        oConfigureMenuBL.IsOnPopUp = 'N';
        oConfigureMenuBL.IsActive = 'Y';        
        return oConfigureMenuBL;
    }   

    /// <summary>
    /// This method is used to fill all page controls.
    /// </summary>
    private void PopulateUserDetails()
    {
        // Display user details for the selected user.
        SchoolUserBL oSchoolUserBL = new SchoolUserBL(Convert.ToInt32(hidUserId.Value));

        if (oSchoolUserBL != null)
        {
            tblUsername.Visible = true;
            txtFirstName.Text = oSchoolUserBL.FirstName;
            txtMiddleName.Text = oSchoolUserBL.MiddleName;
            txtLastName.Text = oSchoolUserBL.LastName;
            txtEmail.Text = oSchoolUserBL.Email;
            txtLogin.Text = oSchoolUserBL.Login;
            cmbSalutation.SelectedValue = Convert.ToString(oSchoolUserBL.SalutationId);
            cmbDesignations.SelectedValue = Convert.ToString(oSchoolUserBL.DesignationId);
        }
        txtPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        txtConfirmPasswd.Attributes.Add("value", oSchoolUserBL.Password);
        hidUserRoleId.Value = Constants.UserRoles.Admin.ToString();
    }

    #endregion " Private Methods "
}

    
   

