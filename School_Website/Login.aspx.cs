using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using Utility;
using System.Data;
using BusinessLogic.Exceptions;
using System.Reflection;
using BusinessLogic;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SetDefaultButton();
            txtUserName.Focus();
            valSum.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        }
    }

    private void SetDefaultButton()
    {
        var oform = this.FindControl("form1") as HtmlForm;
        oform.DefaultButton = btnLogin.UniqueID;
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            Context.Session.Add("ReturnURL", "Login.aspx");
            Char cIsLoginAllowd = Convert.ToChar(ConfigurationManager.AppSettings["IsLoginAllowd"]);
            if (cIsLoginAllowd.Equals(Constants.C_YES))
            {
                string sLogin = txtUserName.Text.Trim();
                string sPassword = txtPassword.Text;
                int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
                string sIPAddress = Request.UserHostAddress;

                var oUserAuthentication = new UserAuthentication(iSchoolId, sLogin, sPassword, sIPAddress);

                if (oUserAuthentication.ValidUser)
                {
                    if (!oUserAuthentication.Locked)
                    {
                        if (!oUserAuthentication.TermAccepted)
                        {
                            sLogin = sLogin.Replace("&", "%USN%");
                            sPassword = sPassword.Replace("&", "%PWD%");
                            string sQuerystring = "login=true&sLogin=" + sLogin + "&sPassword=" + sPassword + "&iSchoolId=" + iSchoolId;
                            string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
                            System.Web.HttpContext.Current.Response.Redirect("~/TermsOfUse.aspx?" + sEncrypt, false);
                        }
                        else
                        {
                            oUserAuthentication.UpdateSession();
                            SetScreenSizeInSession();
                            string returnUrl = Request.QueryString["ReturnUrl"];
                            if (string.IsNullOrEmpty(returnUrl) || returnUrl.Equals("/") || !returnUrl.Contains(".aspx"))
                            {
                                if (sLogin == sPassword)
                                    Response.Redirect("RITeSchool/Common/StudentChangePassword.aspx", false);
                                else
                                    Response.Redirect("RITeSchool/Common/ControlPanel.aspx", false);
                            }
                            else
                                Response.Redirect(returnUrl, false);
                        }
                    }
                    else if (oUserAuthentication.Locked)
                        lblMessage.Text = "Your account is locked. Please contact school administrator.";
                }
                else
                {
                    // Check if the user is from the management user role group.
                    DataTable oDTSuperAdminDetails = oUserAuthentication.GetSuperAdmin(false);

                    if (oDTSuperAdminDetails != null && oDTSuperAdminDetails.Rows.Count > 0)
                    {
                        DataRow oDR = oDTSuperAdminDetails.Rows[0];
                        Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] = oDR["User_Id"].ToInt();
                        Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] = oDR["UserRoleId"].ToInt();
                        SetScreenSizeInSession();
                        Response.Redirect("RITeSchool/Management/ManagementDashboardUI.aspx?" + CommonUtility.EncryptQuerystring("SuperAdminDetailsId=" + oDR["SuperAdminDetailsId"] + "&UserId=" + oDR["User_Id"]), false);
                        UpdateSessionVariable(oDR["Name"].ToString());
                    }
                    else
                        lblMessage.Text = "You are not authenticated user.";
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Web site is under maintenance or upgrade in progress.";

            try
            {
                ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
            }
            catch (Exception exception)
            {
                string asCallingFunctionName = MethodBase.GetCurrentMethod().DeclaringType.FullName + '.' + MethodBase.GetCurrentMethod().Name;
                string sReplaceSingleQuoteString = asCallingFunctionName + " : " + exception.Message
                                                            + Constants.S_TRACE + exception.StackTrace;
                string sSchoolId = ConfigurationManager.AppSettings["SchoolID"];
                int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);

                ExceptionHandler.NotifyErrorLog(sReplaceSingleQuoteString, iUserId, sSchoolId);
            }
        }
    }

    /// <summary>
    ///		Sets the client screen size in session.
    /// </summary>
    private void SetScreenSizeInSession()
    {
        int iWidth;
        bool bIsWidth = Int32.TryParse(hidScreenWidth.Value, out iWidth);
        Session.Add(Constants.S_SESSION_SCREEN_WIDTH, bIsWidth ? iWidth : 1024);
    }

    /// <summary>
    ///		Updates the Session for management user.
    /// </summary>
    /// <param name="asUserName"></param>
    private void UpdateSessionVariable(string asUserName)
    {
        int iSchoolId = ConfigurationManager.AppSettings["SchoolID"].ToInt();
        string sLoginName = SuperAdminBL.GetSchoolAdminLoginName(iSchoolId);
        var oUserAuthentication = new UserAuthentication(iSchoolId, sLoginName, string.Empty, String.Empty);
        oUserAuthentication.UpdateSession();
        Session[Constants.S_SESSION_USER_NAME] = asUserName;
        //	InitializeMemberVariables();
    }
}