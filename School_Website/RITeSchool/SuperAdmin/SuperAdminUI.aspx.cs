// File Name  : SuperAdminUI.aspx.cs
// Created By : Ashish
// Date       : 05/12/2008
// Description: This class is used for Super admin login.

using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BusinessLogic;
using Utility;
using BusinessLogic.Exceptions;
using System.Reflection;

public partial class SuperAdminUI :SchoolBase
{

	#region " Event "

	/// <summary>
    /// This method is used to load Super admin information and initialized session variable.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            Login1.FailureText = "";
            TextBox otxtUserName = (TextBox)this.FindControl("ctl00$MainBody$Login1$UserName");
            otxtUserName.Focus();
            if (!IsPostBack)
            {
                valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
                Button oButton = (Button)this.FindControl("ctl00$MainBody$Login1$LoginButton");
                if (oButton != null)
                {
                    SuperAdminMasterPage oSuperAdminMasterPage = (SuperAdminMasterPage)this.Master;
                    HtmlForm oForm = (HtmlForm)oSuperAdminMasterPage.FindControl("form1");
                    if (oForm != null)
                        oForm.DefaultButton = oButton.UniqueID;
                    Session.Clear();
                    Session.Abandon();
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used for Authenticate user login.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            string sLogin = Login1.UserName.Trim();
            string sPassword = Login1.Password;            
            UserAuthentication oUserAuthentication = new UserAuthentication(0, sLogin, sPassword, string.Empty);
            DataTable oDTSuperAdminDetails = oUserAuthentication.GetSuperAdmin(true);
            if (oDTSuperAdminDetails != null && oDTSuperAdminDetails.Rows.Count > 0)
            {
                DataRow oDR = oDTSuperAdminDetails.Rows[0];
                Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] = Convert.ToInt32(oDR["User_Id"].ToString());
                SetScreenSizeInSession();

                Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] = Convert.ToInt32(oDR["UserRoleId"]);
                 Response.Redirect("ScreensUI.aspx", false);                
            }
            else
            {
                Login1.FailureText = "You are not authenticated user.";
            }            
        }

        catch (Exception ex)
        {
            Login1.FailureText = ex.Message;
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }

    }

    #endregion

    #region " Private Method "

    /// <summary>
    /// This method is used to update session variable and redirect to screenui page.
    /// </summary>
    /// <param name="oDTCompAdmin"></param>
    private void UpdateSessionVariable()
    {
		int iSchoolId = Convert.ToInt32(ConfigurationManager.AppSettings["SchoolID"]);
        string sLoginName = SuperAdminBL.GetSchoolAdminLoginName(iSchoolId);
        UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, Login1.UserName.Trim());
        oUserAuthentication.UpdateSessionForSuperAdmin();
    }

    private void SetScreenSizeInSession()
    {
        int iWidth;
        bool bIsWidth = Int32.TryParse(hidScreenWidth.Value, out iWidth);
        if (bIsWidth)
            Session.Add(Constants.S_SESSION_SCREEN_WIDTH, iWidth + "px !important");
        else
            Session.Add(Constants.S_SESSION_SCREEN_WIDTH, 1024 + "px !important");

    } 

    #endregion

}
