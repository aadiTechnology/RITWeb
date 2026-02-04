// File Name   : SchoolwiseSubjectList.aspx.cs
// Created By  : Anugandha
// Date        : 29/01/2008
// Description : This form is used to change user's password.
// Modified By : Amit
// Date        : 24/09/2009

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Security;
using BusinessLogic;
using BusinessLogic.Exceptions;
using SuperAdminEntities;
using Utility;
using System.Configuration;
using PushNotificationService;
///<Summary>
///This class is used to change user's password which is already exist.
///</Summary>
public partial class StudentChangePassword : SchoolBase
{
    #region Events

    /// <summary>
    /// This event is used to set master page for that page.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPreInit(EventArgs e)
    {
	    try
	    {
		    base.OnPreInit(e);

			if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null && moUserRole == Constants.UserRoles.Admin && QueryString["IsFromManagementDashboard"] == Constants.S_NO)
				this.Page.MasterPageFile = "../SuperAdmin/SuperAdminMasterPage.master";
			else if (QueryString["IsFromManagementDashboard"] == Constants.S_YES)
			{
				this.Page.MasterPageFile = "../MasterPages/PopupMaster.master";				
			}
	    }
	    catch (Exception ex)
	    {
		    ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
	    }
    }

    ///<Summary>
    ///This event is used to set default properties to page controls..
    ///</Summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            if (!IsPostBack)
            {
                SetDefaultProperties();
                if (miUserId != 0)
                    SetControlsAsPerUser();
                SetClientSideScriptAttributes(); 
            }

            if (hidSuperAdmin.Value == "false")
            {
                imgBtnCancel.Attributes.Add("style", "visibility: hidden !important;");
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    ///<Summary>
    ///This event is used to update user's password.
    ///</Summary>
    protected void imgBtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (IsvalidPassword())
            {
				if (hidSuperAdmin.Value == "false" && QueryString["IsFromManagementDashboard"] == null)
					UpdateUser();
				else
					UpdateSuperAdmin();

                hidOldPassword.Value = txtPasswd.Text.ToString();
                SendPushNotification(miUserId.ToString());
                if (hidUrl.Value.ToLower() == "termsofuse.aspx")
                {
                    SchoolUserBL.AcceptTerms(miUserId, miSchoolId);

                    if (moUserRole == Constants.UserRoles.Teacher && Settings.BetaVersionURL != string.Empty)
                        Response.Redirect(Settings.BetaVersionURL + CommonUtility.EncryptQuerystring("SchoolId=" + miSchoolId + "&UserId=" + miUserId).Replace("+", "%20").Replace("/", "%2F"), false);
                    else
                        Response.Redirect("../Common/ControlPanel.aspx", false);
                }
                else
                {
                    lblUpdateSucess.Visible = true;
                    lblUpdateSucess.ForeColor = System.Drawing.Color.Blue;
                    lblUpdateSucess.Text = "<b>Password updated successfully !!!</b>";
                    
                }
            }
            else
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Incorrect old password.";
            }
        }
        catch (DuplicateUserException ex)
        {
            lblErrorMsg.Text = ex.Message;
            lblErrorMsg.Visible = true;            
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    /// <summary>
    /// This event is used to move to previous page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgBtnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
            if (oSchoolUserBL != null)
                txtLogin.Text = oSchoolUserBL.Login;
            hidOldPassword.Value = oSchoolUserBL.Password.ToString();

            if (hidUrl.Value.ToLower() == "termsofuse.aspx" || (hidOldPassword.Value == txtLogin.Text && hidUrl.Value.ToLower() == "eschoollogin.aspx") || (hidOldPassword.Value == txtLogin.Text && hidUrl.Value == string.Empty))
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Response.Cookies.Set(new HttpCookie("ASP.NET_SessionId", String.Empty));
                Response.Redirect("~/eSchoolLogin.aspx", false);
            }
            else
            {
                if (QueryString["IsFromManagementDashboard"] != null)
                {
                    Response.Write("<Script language='Javascript'> window.close();window.opener.focus(); </Script>");
                }
                else
                {
                    Response.Redirect(Constants.S_PAGE_SUPERADMIN, false);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
        }
    }

    #endregion Events 

    #region Private Methods

    /// <summary>
    /// This method is used to get previous page name.
    /// </summary>
    /// <returns></returns>
    private string GetFromPageUrl()
    {
        string sSourcePageUrl = string.Empty;
        if (Request.UrlReferrer != null)
        {
            sSourcePageUrl = Request.UrlReferrer.AbsolutePath;
            sSourcePageUrl = sSourcePageUrl.Substring(sSourcePageUrl.LastIndexOf("/") + 1);
        }
        return sSourcePageUrl;
    }

    /// <summary>
    /// This method is used to set default properties for page controls.
    /// </summary>
    private void SetDefaultProperties()
    {
        txtOldPasswd.Focus();
        txtLogin.Enabled = false;
        txtOldPasswd.ToolTip = Constants.S_PASSWORD_RELATED_NOTE;
        txtPasswd.ToolTip = Constants.S_PASSWORD_RELATED_NOTE;
        txtConfirmPasswd.ToolTip = Constants.S_PASSWORD_RELATED_NOTE;
        valSumErrorMsg.HeaderText = Constants.S_VALIDATION_SUMMARY_HEADER;
        lblErrorMsg.Visible = false;
        hidSuperAdmin.Value = "false";
        hidUrl.Value = GetFromPageUrl();

        if (hidUrl.Value.ToLower() == "termsofuse.aspx" || ((hidUrl.Value.ToLower() == "eschoollogin.aspx" || hidUrl.Value == string.Empty) && (QueryString["ShowPasswordNote"] == null || QueryString["ShowPasswordNote"].ToString() != "Y")))
            BlockMasterControls();

        if (QueryString["ShowPasswordNote"] != null && QueryString["ShowPasswordNote"].ToString() == "Y")
            trPasswordNote.Visible = true;
    }

    /// <summary>
    /// This function is used to block control panels to those who are loning in first time or his/her user name & password are same.
    /// </summary>
    private void BlockMasterControls()
    {
        trNote.Visible = true;
        MasterPage oMasterPage = (MasterPage)this.Master;        
        var tblSitemap = oMasterPage.FindControl("tblSitemap");        
        
        if (tblSitemap != null)
            tblSitemap.Visible = false;
    }

    /// <summary>
    /// This event is used to set java script properties to page controls.
    /// </summary>
    private void SetClientSideScriptAttributes()
    {
        imgBtnSubmit.Attributes.Add("onclick", "ResetErrLabel()");       
        imgBtnCancel.Attributes["onclick"] = "javascript:DisableButtons()";
        ApplyMouseHoverEffect(new List<Button> { imgBtnCancel, imgBtnSubmit }); 
    }

    /// <summary>
    /// This method is used to update superadmin information.
    /// </summary>
    private void UpdateSuperAdmin()
    {
        int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID]);
        string asLoginName = txtLogin.Text.Trim();
        string asPassword = txtPasswd.Text;
        SuperAdminBL.UpdateSuperAdminDetails(iUserId, asLoginName, asPassword);
    }

    /// <summary>
    /// This method is used to update user information.
    /// </summary>
    private void UpdateUser()
    {
        string IsDemoSite = ConfigurationManager.AppSettings["IsDemoSite"];
        if (IsDemoSite == Constants.S_NO || string.IsNullOrEmpty(IsDemoSite))
        {
            SchoolUserBL oSchoolUserBL = CreateAndGetObjectForUserPassword();
            oSchoolUserBL.UserId = miUserId;
            oSchoolUserBL.SchoolId = miSchoolId;
            oSchoolUserBL.UpdateSchoolUserPassword();
         }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="asUserId"></param>
    /// <param name="aoObject"></param>
    public override void SendPushNotification(string asUserId, object aoObject = null)
    {
        PushNotificationClient pushNotificationClient = null;
        try
        {
            int[] intArrayUserId = new int[1];
            intArrayUserId[0] = Convert.ToInt32(asUserId);

            pushNotificationClient = new PushNotificationClient();
            Dictionary<string, string> dictionaryNotificationParameter = new Dictionary<string, string>();
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_SCHOOLNAME, Convert.ToString(System.Web.HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_NAME]));
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_USERNAME, txtLogin.Text.Trim());
            dictionaryNotificationParameter.Add(Constants.S_NOTIFICATION_PARAMETER_PASSWORD, txtPasswd.Text.ToString());
            pushNotificationClient.SendNotification(NotificationMessageHeadings.ForgotPassword, this.miSchoolId.ToString(), intArrayUserId, dictionaryNotificationParameter);
            pushNotificationClient.Close();
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, System.Reflection.MethodBase.GetCurrentMethod());
        }
        finally
        {
            if (pushNotificationClient.State != System.ServiceModel.CommunicationState.Faulted)
                pushNotificationClient.Close();
        }
    }


    ///<Summary>
    ///This method is used to display current user's login name.
    ///</Summary>
    private void SetControlsAsPerUser()
    {
		if (QueryString["IsFromManagementDashboard"] == Constants.S_YES)
		{

			int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID]);
			SuperAdminBL oSuperAdminBL = new SuperAdminBL();
			txtLogin.Text = oSuperAdminBL.GetLoginName(iUserId);
			hidOldPassword.Value = oSuperAdminBL.SuperAdminPass;
			imgBtnCancel.PostBackUrl = string.Empty;
			imgBtnCancel.Attributes.Add("onclick", "window.close();");
			trTitle.Visible = true;
		}
        else if (moUserRole == Constants.UserRoles.Admin && Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] != null)
        {
            int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID]);
            SuperAdminBL oSuperAdminBL = new SuperAdminBL();
            txtLogin.Text = oSuperAdminBL.GetLoginName(iUserId);
            hidOldPassword.Value = oSuperAdminBL.SuperAdminPass;
            hidSuperAdmin.Value = "true";            
            PassNote.Visible = false;
        }
        else        
        {
            hidSuperAdmin.Value = "false";
            SchoolUserBL oSchoolUserBL = new SchoolUserBL(miUserId);
            if (oSchoolUserBL != null)
                txtLogin.Text = oSchoolUserBL.Login;
            hidOldPassword.Value = oSchoolUserBL.Password.ToString();
        }        
        txtPasswd.Text = Constants.S_EMPTY_STRING;
        txtConfirmPasswd.Text = Constants.S_EMPTY_STRING;
    }

    ///<Summary>
    ///This method is used to populate oSchoolUserBL object to update password.
    ///</Summary>
    private SchoolUserBL CreateAndGetObjectForUserPassword()
    {
        SchoolUserBL oSchoolUserBL = new SchoolUserBL();
        oSchoolUserBL.Login = txtLogin.Text.Trim();
        oSchoolUserBL.Password = txtPasswd.Text;
        oSchoolUserBL.UpdatedBy = miUserId.ToString();
        oSchoolUserBL.UpdatedDate = System.DateTime.Now.ToString("MM/dd/yyyy");
        return oSchoolUserBL;
    }

    ///<Summary>
    ///This method is used to check whether given old password is valid or not.
    ///</Summary>
    private bool IsvalidPassword()
    {
        string sPassword = txtOldPasswd.Text;
        string sLogin = txtLogin.Text;
        bool bValid = false;
		if (hidSuperAdmin.Value == "false" && QueryString["IsFromManagementDashboard"] == null)
		{
			DataSet moDataSet = SchoolUserBL.GetValidUser(miSchoolId, sLogin, sPassword, "");
			if (moDataSet.Tables.Count != 0)
				bValid = true;
		}
		else
		{
			DataTable oDTSuperAdminDetails = SuperAdminBL.GetCompAdmin(sLogin, sPassword, true);
			if (oDTSuperAdminDetails != null && oDTSuperAdminDetails.Rows.Count > 0)
				bValid = true;
		}
        return bValid;
    }

    #endregion

}