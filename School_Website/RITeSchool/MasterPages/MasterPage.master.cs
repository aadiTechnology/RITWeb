
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLogic;
using BusinessLogic.Exceptions;
using StudentEntities;
using Utility;
using System.Web.Script.Serialization;

public partial class MasterPage : BaseMasterPage
{

	#region -- MEMBER(s) --

	//Static variable is used to store value in memory which is used when master page is initialized.
	private static string msMenuName = "";
	private static string msParentURL = "";
	private static string msNodeTitle = "";

	private string msPageUrl = String.Empty;
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

	#endregion -- MEMBER(s) --

	#region -- PROPERTIES --

	public string NodeTitle
	{
		get { return msNodeTitle; }
		set { msNodeTitle = value; }
	}

	#endregion -- PROPERTIES --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Collects information abou the Page Request.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
    {
		try
		{
            if (Session["Location"] != null && Session["Location"].ToString().Length > 0)
                spanLocation.Visible = true;
            else
                spanLocation.Visible = false;
			msPageUrl = Request.AppRelativeCurrentExecutionFilePath;

			// If the Session is lost, we redirect the user to error page.
			if (!msPageUrl.ToUpper().Equals("~/RITESCHOOL/COMMON/ERROR.ASPX"))
			{
				// When SchoolId is null
				if (Session["I_SCHOOL_ID"] == null ||
					// When currently logged in user is from the Management Role
					(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null && Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.SuperAdminRoles.ManagementUser.ToInt()))
                     Response.Redirect("~/RITeSchool/Common/Error.aspx", true);
			}
			
			// Check if Session is shared with another user. If it is, redirect the user to the error page.
			if (!Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]).IsNull() && Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]) != Constants.S_ZERO && Convert.ToString(Request.Params[hidSessionUserId.ClientID.Replace("_", "$")]) != Session[Constants.S_SESSION_USER_ID].ToString())
                Response.Redirect("~/RITeSchool/Common/Error.aspx?" + CommonUtility.EncryptQuerystring("Is_Session_Shared=Y"), true);

			base.OnInit(e);
		}
		catch (ThreadAbortException)
		{
			// Do nothing. ASP.NET is redirecting.
			// Always comment this so other developers know why the exception 
			// is being swallowed.
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);
            if (Session["Location"] != null && Session["Location"].ToString().Length > 0)
                spanLocation.Visible = true;
            else
                spanLocation.Visible = false;
			if (!Session[Constants.S_SESSION_USER_ID].IsNull())
                hidSessionUserId.Value = Session[Constants.S_SESSION_USER_ID].ToString();
			string sPageRequest = Request.AppRelativeCurrentExecutionFilePath;
			sPageRequest = sPageRequest.Substring(sPageRequest.LastIndexOf("/") + 1);

            if (Session[Constants.S_SESSION_LANGUAGE] == null)
                Session[Constants.S_SESSION_LANGUAGE] = "en";

			if (!sPageRequest.ToUpper().Equals("ERROR.ASPX") &&
					((Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID] != null && (Constants.SuperAdminRoles)Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID].ToInt() == Constants.SuperAdminRoles.ManagementUser) || !sPageRequest.ToUpper().Equals("DISPLAYMENUCONTENTS.ASPX")))
                SetSiteMapProviderAccordingToUserRole(sPageRequest);

            if (!Session[Constants.S_SESSION_SCHOOL_ID].IsNull())
                hidSchoolId.Value = Session[Constants.S_SESSION_SCHOOL_ID].ToString();

            if (!Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].IsNull())
                hidAcademicYearId.Value = Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToString();
            
            if (!Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].IsNull())
                hidSessionUserRoleId.Value = (Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID])).ToString();

            if (Session[Utility.Constants.S_SESSION_USER_FULLNAME] != null && Session[Utility.Constants.S_SESSION_USER_FULLNAME].ToString() == "Software Coordinator")
                hidUserFullName.Value = "Software Coordinator";
			
           hidUserHasFullAccess.Value = CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.AnnualEventPlanner).ToString();
            
			Response.Buffer = true;
			Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
			Response.Expires = -1;
			Response.Cache.AppendCacheExtension("max-age=0, no-store, must-revalidate");

			// The title is not set in !IsPostBack because for the pages like annual planner
			// where AJAX is not implemented, after changing the month the title is reset to its URL.
			Page.Title = Constants.S_TITLE_FOR_PAGE;
			SiteMap.SiteMapResolve += ExpandForumPaths;

            // This is used for showing/hiding Email Icon on dashboard.
            hlnkEmail.Visible = SchoolBase.Settings.ShowEmailIcon;

            // This is used for showing/hiding Themes Icon on dashboard.
            //tdThemes.Visible = SchoolBase.Settings.ShowThemes;
            SetScreenTooltip();
            SetSchoolLogoAndProfileImage();

            
			if (!IsPostBack)
			{
                if (SchoolBase.Settings.SupportURL.Trim() == string.Empty || (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt()))
                    hidSupportURL.Value =  "";
                else
                    hidSupportURL.Value = SchoolBase.Settings.SupportURL.ToString();
                    
                // Session will be null in case of registration pages.
                    if (Session["I_SCHOOL_ID"] != null)
                    {
                        FillMenuControl();
                        SetAcademicYearMessage();
                    }
                if (Session[Constants.S_SESSION_LANGUAGE] != null)
                {
                    ddlLanguage.SelectedValue = Session[Constants.S_SESSION_LANGUAGE].ToString();
                }
                hlnkEmail.NavigateUrl = ConfigurationManager.AppSettings["ExternalMailServerURL"];

                /* Configure RITeStore Settings*/
                SetRITeStoreDisplaySettings();

                if (ConfigurationManager.AppSettings["TrustName"] != null)
                    spnTrust.InnerText = ConfigurationManager.AppSettings["TrustName"];
                else
                    spnTrust.InnerText = string.Empty;
			}

                
            else
                FillMenuControl();

			if (!SchoolBase.Settings.ShowAds)
			{
				tdTopAd.Visible = false;
				tdTopNoAd.Visible = true;
				hidShowAds.Value = "N";
			}   
			else
			{
				tdTopNoAd.Visible = false;
				tdTopAd.Visible = true;
				hidShowAds.Value = "Y";
			}
            hidRightsReserved.Value = Resources.LocalizedResources.RightsReserved;
            hidRIT.Value = Resources.LocalizedResources.RIT;
			hidMiniSite.Value = SchoolBase.Settings.IsMiniSite.ToString();
            hidServerDate.Value = Convert.ToString(DateTime.Now.Date.Year);
            hidServerFullDate.Value = DateTime.Now.ToString(Constants.S_DATE_FORMAT+ " hh:mm tt");

			hlnkReadMe.Attributes.Add("onclick", "window.open('" + hlnkReadMe.NavigateUrl + "', '_blank','scrollbars=yes,resizable=yes,height=800,width=1000');return false;");
			
            if (Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]) != Constants.SchoolId.RITeSchool.ToInt())
            {
                hlnkParentGuide.NavigateUrl = SchoolBase.Settings.UserGuideLocation;
                hlnkTeacherGuide.NavigateUrl = SchoolBase.Settings.UserGuideLocation;
                hlnkParentGuide.Attributes.Add("onclick", "window.open('" + hlnkParentGuide.NavigateUrl + "', '_blank','scrollbars=yes,resizable=yes,height=800,width=1000');return false;");
                hlnkTeacherGuide.Attributes.Add("onclick", "window.open('" + hlnkTeacherGuide.NavigateUrl + "', '_blank','scrollbars=yes,resizable=yes,height=800,width=1000');return false;");

                hlnkKnowledgebase.Attributes.Add("onclick", "window.open('" + "https://www.riteschool.com/knowledge-base" + "', '_new','scrollbars=yes,resizable=yes,height=800,width=1000');return false;");
                //hlnkKnowledgebase.Attributes.Add("onclick", "window.open('" + "http://riteschool.com/KnowledgeBase.aspx" + "', '_new','scrollbars=yes,resizable=yes,height=800,width=1000');return false;");
            }

            CheckSibling();
            CheckStaff();

            if ((Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt()))
                lnkMyProfile.Visible = true;
            else

                lnkMyProfile.Visible = false;
          
            
            //set doctype
            docType.Text = @"<!DOCTYPE html>" + Environment.NewLine + @"<html>";
            /////new add  for hide support option
            if (SchoolBase.Settings.IsAaryanSchool == true)
            {
                hlnkSupport.Visible = false;
            }

            if ((Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt()))
            {
                if (Session[Constants.S_SESSION_ARE_MANDATORY_FIELD_SUBMITTED_BY_STUDENT] != null && Session[Constants.S_SESSION_ARE_MANDATORY_FIELD_SUBMITTED_BY_STUDENT].ToString() != Constants.S_YES)
                {
                    string currentPage = Request.Path.ToLower();
                    if (!currentPage.ToUpper().Contains("STUDENTMANDATORYDETAILSUI.ASPX") && !currentPage.ToUpper().Contains("CONTROLPANEL.ASPX"))
                    {
                        RedirectToNextPage("~/RITeSchool/Student/StudentMandatoryDetailsUI.aspx");
                    }
                }
            }
		}
		catch (ThreadAbortException)
		{
			// Do nothing. ASP.NET is redirecting.
			// Always comment this so other developers know why the exception 
			// is being swallowed.
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

    private void CheckSibling()
    {
        if ((Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt()) && 
            (Session[Constants.S_SESSION_HAS_SIBLING].ToString().Trim().ToUpper() == "TRUE"))
        {
            lnkSiblingLogin.Visible = true;
            lnkSiblingLoginList.Visible = true;
        }
        else {
            lnkSiblingLogin.Visible = false;
            lnkSiblingLoginList.Visible = false;
        }
    }

    private void CheckStaff()
    {
        if (Session[Constants.S_SESSION_HAS_PARENT_STAFF] != null && Session[Constants.S_SESSION_HAS_PARENT_STAFF].ToString().Trim().ToUpper() == "TRUE") 
        {
            lnkParentLogin.Visible = true;

            Label lblName = lnkParentLogin.FindControl("lblName") as Label;
            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt())
                lblName.Text = "Login to Staff";
            else
                lblName.Text = "Login to Child";
        }
        else
        {
            lnkParentLogin.Visible = false;
        }
   }

	protected void Menu_Configure_MenuItemClick(object sender, MenuEventArgs e)
	{
		try
		{
			//This function is used to fetch the content of the selectecd menu item details.
			var oMenu = sender as Menu;
			RedirectToNextPage(GetEncryptedMenuQueryString(oMenu.SelectedValue, oMenu.SelectedItem.Text));
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	protected void lnkLogout_Click(object sender, EventArgs e)
	{
		try
		{
			FormsAuthentication.SignOut();
			Session.Abandon();
            Session[Constants.S_SESSION_LANGUAGE] = Constants.S_MARATHI_LANGUAGE;
			Response.Cookies.Set(new HttpCookie("ASP.NET_SessionId", String.Empty));
            // false - endResponce- continue execution of current page after executing page which is passed as URl.
            string sLogoutPage = ConfigurationManager.AppSettings["LogoutPage"];
           int aiUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);
           SchoolUserBL moSchoolUserBL = new SchoolUserBL();
           moSchoolUserBL.UpdateLogOutDate(aiUserId);
            if (sLogoutPage != string.Empty)
                Response.Redirect(sLogoutPage, false);
            if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME] != null)
            {
                if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME].ToString() == Constants.DemoCompanyName.Marpha.ToString())
                    Response.Redirect("~/Login.aspx", false);
                else if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME].ToString() == Constants.DemoCompanyName.GMore.ToString())
                    Response.Redirect("~/SchoolLogin.aspx", false);
            }
            else
                Response.Redirect(sLogoutPage, false);            
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	
    }

    //protected void lnkMyProfile_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        this.RedirectToNextPage("~/Common/MyProfile.aspx?");
    //    }
    //    catch (Exception ex)
    //    {
    //        ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
    //    }
    //}

	protected void lnkFeedback_Click(object sender, EventArgs e)
	{
		try
		{
            string sQuerString = "UserMode=1";
            string sEncrypt = Utility.CommonUtility.EncryptQuerystring(sQuerString);
            RedirectToNextPage("~/Common/FeedbackUI.aspx?" + sEncrypt);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}
    /// <summary>
    /// Themes Implementation - This event set the selected theme for the web site.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlThemes_SelectedIndexChanged(object sender, EventArgs e)
    {
		try
		{
			HidTheme.Value = ddlThemes.SelectedValue;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
    }


    /// <summary>
    /// Language Implementation - This event set the selected language for the web site.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session[Constants.S_SESSION_LANGUAGE] = ddlLanguage.SelectedValue;           
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Session[Constants.S_SESSION_LANGUAGE].ToString());
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Session[Constants.S_SESSION_LANGUAGE].ToString());
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
        }
    }
	/// <summary>
	/// This event set the default theme for the web site.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			string sCurrentTheme = HidTheme.Value;
			if (sCurrentTheme == "Default")
			{
				Response.Cookies["UserTheme"].Value = string.Empty;
				Response.Cookies["UserTheme"].Expires = DateTime.Now.AddDays(360);
			}
			else
			{
				Response.Cookies["UserTheme"].Value = sCurrentTheme;
				Response.Cookies["UserTheme"].Expires = DateTime.Now.AddDays(360);
			}
			this.RedirectToNextPage(this.Page.Request.Url.ToString());
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	/// <summary>
	/// This event is used to redirect same page..
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnClose_Click(object sender, EventArgs e)
	{
		try
		{
			this.RedirectToNextPage(this.Page.Request.Url.ToString());
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

    /// <summary>
    /// This event is used to open student sibling screen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkSiblingLogin_Click(object sender, EventArgs e)
    {
        try
        {
            if (((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Constants.UserRoles.Student)
            {
                int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
                int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
                int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
                string sUserName = Convert.ToString(Session[Constants.S_SESSION_USER_NAME]);
              
                int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);                
                List<StudentInfo> lstStudents = StudentSiblingDetailsBL.GetSiblingDetailsForLogin(iSchoolId, iAcademicYearId, iStudentId, iUserId, true);

                var oStudentInfo = lstStudents.FirstOrDefault();
                if (lstStudents.Count == 1)
                {
                    UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, oStudentInfo.UserName, string.Empty, string.Empty);
                    string sUrl = "../Common/ControlPanel.aspx";                 
                        
                    if (oUserAuthentication.TermAccepted)
                        oUserAuthentication.UpdateSession();                            
                    else
                    {
                        string sPassword = CommonUtility.GetDecryptedPassword(oStudentInfo.UserName.ToLower(), oStudentInfo.Password);
                        string sQuerystring = "login=true&sLogin=" + oStudentInfo.UserName + "&sPassword=" + sPassword + "&iSchoolId=" + iSchoolId;
                        string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
                        sUrl = "../../TermsOfUse.aspx?" + sEncrypt;
                    }

                    RedirectToNextPage(sUrl);
                }
                else if (lstStudents.Count > 0)
                {
                    string pQueryString = CommonUtility.EncryptQuerystring("IsFromSiblingScreen=Y");
                    RedirectToNextPage("~/Student/SiblingLoginUI.aspx?" + pQueryString);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
        }
    }

    protected void lnkParentLogin_Click(object sender, EventArgs e)
    {
        try
        {            
            int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
            int iAcademicYearId = Convert.ToInt32(Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID]);
            int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
            string sUserName = Convert.ToString(Session[Constants.S_SESSION_USER_NAME]);
            int iUserId = Convert.ToInt32(Session[Constants.S_SESSION_USER_ID]);

            List<StudentInfo> lstStudents = StudentSiblingDetailsBL.GetParentDetailsForLogin(iSchoolId, iAcademicYearId, iStudentId, iUserId, false);

            var oStudentInfo = lstStudents.FirstOrDefault();
            if (lstStudents.Count == 1)
            {
                UserAuthentication oUserAuthentication = new UserAuthentication(iSchoolId, oStudentInfo.UserName, string.Empty, string.Empty);
                string sUrl = "../Common/ControlPanel.aspx";

                if (oUserAuthentication.TermAccepted)
                    oUserAuthentication.UpdateSession();
                else
                {
                    string sPassword = CommonUtility.GetDecryptedPassword(oStudentInfo.UserName.ToLower(), oStudentInfo.Password);
                    string sQuerystring = "login=true&sLogin=" + oStudentInfo.UserName + "&sPassword=" + sPassword + "&iSchoolId=" + iSchoolId;
                    string sEncrypt = CommonUtility.EncryptQuerystring(sQuerystring);
                    sUrl = "../../TermsOfUse.aspx?" + sEncrypt;
                }

                RedirectToNextPage(sUrl);
            }
            else if (lstStudents.Count > 0)
            {
                string pQueryString = CommonUtility.EncryptQuerystring("IsFromSiblingScreen=N");
                RedirectToNextPage("~/Student/SiblingLoginUI.aspx?" + pQueryString);
            }
        }
        catch (Exception ex)
        {

            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
        }
    }
    
     protected void lnkAskMe_Click(object sender, EventArgs e)
    {
        try
        {
            this.RedirectToNextPage("~/RITeSchool/AskMe/PublishedQueriesUI.aspx");
        }
        catch (Exception ex)
        {
            ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
        }
    }

	#endregion -- EVENT HANDLER(s) --

	#region -- PUBLIC METHOD(s) --

	/// <summary>
	/// This method is used to set site map provider according to the user role id.
	/// </summary>
	public void SetSiteMapProviderAccordingToUserRole(string asPageName)
	{
		string sUserRole = Convert.ToString((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
        if (!sUserRole.Equals(string.Empty) && sUserRole.Equals("Student") && (asPageName.ToUpper().Equals("STUDENTHOMEWORKUI.ASPX") || asPageName.ToUpper().Equals("VIEWHOMEWORKUI.ASPX")))
        {
            SiteMapPath1.Provider = SiteMap.Providers[sUserRole];
        }
        else if (sUserRole.Equals("Student"))
        {
            SiteMapPath1.Provider = SiteMap.Providers[sUserRole];
        }
        else if ((Constants.SuperAdminRoles)Session[Constants.S_SESSION_SUPERADMIN_ROLE_ID].ToInt() == Constants.SuperAdminRoles.ManagementUser)
			SiteMapPath1.Provider = SiteMap.Providers["Management"];

        if (SiteMapPath1.Provider.CurrentNode == null)
            Response.Redirect("~/RITeSchool/Common/Error.aspx?" + CommonUtility.EncryptQuerystring("AccessRestriction=Y"), true);
	}

	public void SetParentNodeURL(string asText)
	{
		msParentURL = asText;
	}

	public SiteMapNode SetCurrentNodeText(string asText, int aiCurrentUserRoleId, object aoSchoolId)
	{
		msMenuName = asText;
		// The current node represents a Post page in a bulletin board forum.
		// Clone the current node and all of its relevant parents. This
		// returns a site map node that a developer can then
		// walk, modifying each node.Url property in turn.
		// Since the cloned nodes are separate from the underlying
		// site navigation structure, the fixups that are made do not
		// effect the overall site navigation structure.
		SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);
		SiteMapNode tempNode = currentNode;
		SiteMapPath1.NodeStyle.CssClass = "SMPrvNode";
		SiteMapPath1.CurrentNodeStyle.CssClass = "SMCurentNode";
		if (tempNode.ParentNode != null && tempNode.ParentNode.Url == "~/Admin/SchoolConfigurationControlPanel.aspx" && msParentURL != "")
		{
			tempNode.ParentNode.Url = msParentURL;
			tempNode.ParentNode.Title = msNodeTitle;
		}
		// Obtain the recent IDs.
		if (aoSchoolId != null)
		{
			if (tempNode != null)
			{
                if (tempNode.Title == "Menu" || tempNode.Title == "Add Override Details")
					tempNode.Title = asText;
				else if (tempNode.Title == "Add/Edit Supervisor")
				{
					tempNode.ParentNode.Title = Constants.S_SUPERVISOR_ROLE_NAME;
					tempNode.ParentNode.Url = "~/RITeSchool/Admin/SupervisorUserListUI.aspx";
					tempNode.ParentNode.ParentNode.Title = "School Configuration";
					tempNode.ParentNode.ParentNode.Url = "~/RITeSchool/Admin/SchoolConfigurationControlPanel.aspx";
					tempNode.Title = msMenuName;
				}
				else if (tempNode.Title == "Manage And Issue Books" &&
					(Constants.UserRoles)aiCurrentUserRoleId == Constants.UserRoles.Supervisor)
					tempNode.Title = asText;
                else if (tempNode.Title != null && (tempNode.Title == "Association" || tempNode.Title == "Transport Committee" || tempNode.Title == "Parent Teacher Association"))
                {   
                    tempNode.ParentNode.Title = "Dashboard";
                    tempNode.ParentNode.Url = "~/RITeSchool/Common/ControlPanel.aspx";
                    tempNode.Title = asText;
                }
			}
		}

        if ((tempNode != null) && ((tempNode.Title == "SMS Center") || (tempNode.Title == "Sent SMS") || (tempNode.Title == "Scheduled SMS")))
		{
			tempNode = SetNoteForSMSPages(ref tempNode, aiCurrentUserRoleId, msMenuName);
		}

		return currentNode;
	}

	public void RedirectToNextPage(string asPageName)
	{
		if (asPageName.IndexOf("RITeSchool/") == -1)
			asPageName = asPageName.Replace("~/", "~/RITeSchool/");
		Response.Redirect(asPageName, false);
		ClearTempararySession();
	}

	#endregion -- PUBLIC METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	private void SetAcademicYearMessage()
	{
		string sMessage = Convert.ToString(Session[Constants.S_SESSION_ACADEMIC_YEAR_STATUS]);

		if (sMessage != "")
		{
			trYearStatus.Visible = true;
			lblYearStatus.Text = sMessage;
		}
		else
		{
			trYearStatus.Visible = false;
			lblYearStatus.Text = "";
		}
	}

	private SiteMapNode ExpandForumPaths(Object sender, SiteMapResolveEventArgs e)
	{
		SiteMapNode currentNode = SiteMap.CurrentNode.Clone(true);        
		try
		{
			// The current node represents a Post page in a bulletin board forum.
			// Clone the current node and all of its relevant parents. This
			// returns a site map node that a developer can then
			// walk, modifying each node.Url property in turn.
			// Since the cloned nodes are separate from the underlying
			// site navigation structure, the fixups that are made do not
			// effect the overall site navigation structure.

			SiteMapNode tempNode = currentNode;
			SiteMapPath1.NodeStyle.CssClass = "SMPrvNode";
			SiteMapPath1.CurrentNodeStyle.CssClass = "SMCurentNode";

			if ((tempNode != null) && (tempNode.ParentNode != null && tempNode.ParentNode.Url.EndsWith("~/Admin/SchoolConfigurationControlPanel.aspx") && msParentURL != ""))
			{
				tempNode.ParentNode.Url = msParentURL;
			}
			if (tempNode != null && msNodeTitle != string.Empty)
				tempNode.Title = msNodeTitle;

			if (e.Context.Session[Constants.S_SESSION_SCHOOL_ID] != null)
			{
				if (tempNode != null)
				{
					if (tempNode.Title == "Menu")
						tempNode.Title = msMenuName;
				}
			}

            if ((tempNode != null) && ((tempNode.Title == "SMS Center") || (tempNode.Title == "Sent SMS") || (tempNode.Title == "Received SMS") || (tempNode.Title == "Scheduled SMS")))
			{
				tempNode = SetNoteForSMSPages(ref tempNode, Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]), msMenuName);
			}
			else if (tempNode != null && tempNode.Title == "Holidays")
			{
				tempNode = SetNodeForTeacherPages(ref tempNode, msMenuName, Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]), Constants.SchoolConfigurations.HolidaysManagement);
			}
			else if (tempNode != null && tempNode.Title == "Students")
			{
				if (Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Convert.ToInt32(Constants.UserRoles.Teacher) ||
                    Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Convert.ToInt32(Constants.UserRoles.Supervisor))
					if (CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.Student))
						tempNode = SetNodeForTeacherPages(ref tempNode, msMenuName, Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]), Constants.SchoolConfigurations.Student);
			}
			else if (tempNode != null && tempNode.Title == "Exam Schedule")
			{

				if (Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) != Convert.ToInt32(Constants.UserRoles.Student))
					if (Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Convert.ToInt32(Constants.UserRoles.Teacher))
						if (CommonUtility.IsUserHasScreenAccess(Constants.SchoolConfigurations.StandardwiseExamScheduleConfig))
							tempNode = SetNodeForTeacherPages(ref tempNode, msMenuName, Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]), Constants.SchoolConfigurations.StandardwiseExamScheduleConfig);
			}
			else if (tempNode != null && tempNode.Title == "Add/Edit Supervisor")
			{
				tempNode.ParentNode.Title = Constants.S_SUPERVISOR_ROLE_NAME;
				tempNode.ParentNode.Url = "~/RITeSchool/Admin/SupervisorUserListUI.aspx";
				tempNode.ParentNode.ParentNode.Title = "School Configuration";
				tempNode.ParentNode.ParentNode.Url = "~/RITeSchool/Admin/SchoolConfigurationControlPanel.aspx";
				tempNode.Title = msMenuName;
			}
			else if (tempNode != null && tempNode.Url.Contains("/Teacher/StudentMarksAssignment.aspx"))
			{
				if (HttpContext.Current.Request.UrlReferrer.AbsolutePath.Contains("/Teacher/ClassTeacherTestMarksUI.aspx"))
				{
					tempNode.ParentNode.Url = tempNode.ParentNode.Url.Replace("/Teacher/TestMarksConfigurationUI.aspx", "/Teacher/ClassTeacherTestMarksUI.aspx");
					tempNode.ParentNode.Title = "Exam Results";
				}
			}
			else if (tempNode != null && ((tempNode.Url.Contains("/Teacher/PrePrimaryProgressSheetEntry.aspx")) || tempNode.Url.Contains("/Teacher/StudentProgressReportEntry.aspx")))
			{
				if (HttpContext.Current.Request.UrlReferrer != null)
				{
					if (HttpContext.Current.Request.UrlReferrer.AbsolutePath.Contains("/Teacher/ClassTeacherTestMarksUI.aspx"))
					{
						SiteMapNode oSiteMapNode = tempNode.ParentNode.ParentNode.Clone();
						oSiteMapNode.Url = "~/RITeSchool/Teacher/ClassTeacherTestMarksUI.aspx";
						oSiteMapNode.Title = "Exam Results";
						tempNode.ParentNode = oSiteMapNode;
					}
					if (HttpContext.Current.Request.UrlReferrer.AbsolutePath.Contains("/ProgressReport/StudentwiseProgreesReportUI.aspx") || HttpContext.Current.Request.UrlReferrer.AbsolutePath.Contains("/Teacher/StudentProgressReportEntry.aspx"))
					{
						SiteMapNode oSiteMapNode = tempNode.ParentNode.ParentNode.Clone();
						oSiteMapNode.Url = "~/RITeSchool/ProgressReport/StudentwiseProgreesReportUI.aspx";
						oSiteMapNode.Title = "Student Wise Progress Report";
						tempNode.ParentNode = oSiteMapNode;
					}
				}
			}
			else if (tempNode != null && tempNode.Url.Contains("/Admin/PrePrimaryProgressReportConfigList.aspx"))
			{
				if (HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.AbsolutePath.Contains("/Admin/PrePrimaryProgressReportConfigList.aspx"))
				{
					string sTestDecrypt = Server.UrlDecode(HttpContext.Current.Request.QueryString.ToString());
					if (sTestDecrypt.Length > 0)
					{
						string msQueryString = Utility.CommonUtility.DecryptQuerystring(sTestDecrypt);
						HttpRequest moHttpRequest = new HttpRequest(HttpContext.Current.Request.FilePath.ToString(),
														HttpContext.Current.Request.Url.ToString(),
														msQueryString);
						if (moHttpRequest.QueryString["ParentHeading_Id"] != null)
						{
							SiteMapNode oSiteMapNode = tempNode.Clone();
							oSiteMapNode.ParentNode = tempNode.ParentNode;
							tempNode.ParentNode = oSiteMapNode;
							tempNode.Title = "Skills";
							;

						}
					}
				}
			}
			else if (tempNode != null && tempNode.Title == "Manage And Issue Books" &&
				(Constants.UserRoles)Convert.ToInt32(e.Context.Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) == Constants.UserRoles.Supervisor)
			{
				tempNode.Title = msMenuName;
			}
            else if (tempNode != null && (tempNode.Title == "Association" || tempNode.Title == "Transport Committee" || tempNode.Title == "Parent Teacher Association"))
            {
                tempNode.ParentNode.Title = "Dashboard";
                tempNode.ParentNode.Url = "~/RITeSchool/Common/ControlPanel.aspx";
                tempNode.Title = msMenuName;
            }

			msNodeTitle = string.Empty;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
		return currentNode;
	}

	private SiteMapNode SetNodeForTeacherPages(ref SiteMapNode tempNode, string msMenuName, int aiUserLoginId, Constants.SchoolConfigurations oSchoolConfigurations)
	{
		if (aiUserLoginId == Convert.ToInt32(Constants.UserRoles.Teacher)
			|| aiUserLoginId == Convert.ToInt32(Constants.UserRoles.Student)
            || aiUserLoginId == Convert.ToInt32(Constants.UserRoles.Supervisor))
		{

			SiteMapPath1.NodeStyle.CssClass = "SMPrvNodeNoPadding";
			SiteMapPath1.CurrentNodeStyle.CssClass = "SMCurentNode";

			if (HttpContext.Current.Request.UrlReferrer != null)
			{
				string sUrl = HttpContext.Current.Request.UrlReferrer.AbsolutePath;
				sUrl = sUrl.Substring(sUrl.LastIndexOf("/") + 1);
				if (sUrl == "ControlPanel.aspx")
				{
					tempNode.ParentNode = tempNode.ParentNode.ParentNode;
				}
			}
		}
		return tempNode;
	}

	/// <summary>
	/// This method is used to swap parent and childs for a flow changes for other users
	/// </summary>
	private SiteMapNode SetNoteForSMSPages(ref SiteMapNode tempNode, int aiCurrentUserRoleId, string msMnuName)
	{
		
		if (aiCurrentUserRoleId != Convert.ToInt32(Constants.UserRoles.Admin) && CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter) == Constants.C_NO)
		{
			if (tempNode.ChildNodes.Count >= 1)
			{
				SiteMapNode oSiteMapNode = tempNode.ChildNodes[0].Clone();
				oSiteMapNode.ParentNode = tempNode.ParentNode;
				tempNode.ParentNode = oSiteMapNode;
				oSiteMapNode.Title = "Received SMS";
				tempNode.Title = "View SMS";
			}
			else
			{
				tempNode.ParentNode = tempNode.ParentNode.ParentNode;
				tempNode.Title = "Received SMS";
			}

			SiteMapPath1.NodeStyle.CssClass = "SMPrvNodeNoPadding";
			SiteMapPath1.CurrentNodeStyle.CssClass = "SMCurentNode";
		}

        if (aiCurrentUserRoleId == Convert.ToInt32(Constants.UserRoles.Admin) || CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.SMSCenter) == Constants.C_YES)
        {
            if (tempNode.Title == "SMS Center" && HttpContext.Current.Request.QueryString.Count > 0)
            {
                string sDecryptedQueryString = CommonUtility.DecryptQuerystring(Server.UrlDecode(HttpContext.Current.Request.QueryString.ToString()));
                NameValueCollection oQueryString = HttpUtility.ParseQueryString(sDecryptedQueryString);
               
                SiteMapNode oSiteMapNode = tempNode.ChildNodes[0].Clone();

                if (!oQueryString["Access"].IsNull() && oQueryString["Access"] == Constants.S_ZERO)                
                    oSiteMapNode.Title = "Received SMS";
                else if (!oQueryString["Access"].IsNull() && oQueryString["Access"] == "3")
                    oSiteMapNode.Title = "Scheduled SMS";
                else 
                    oSiteMapNode.Title = "Sent SMS";

                if(!oQueryString["Access"].IsNull())
                    Session["Access"] = oQueryString["Access"];
                tempNode.ParentNode = oSiteMapNode;                
                tempNode.Title = "View SMS";
            }
        }

		return tempNode;
	}

	private void SetScreenTooltip()
	{
        string sPageRequest = GetPageRequest();
        sp1.InnerText = oResourceManager.GetString(sPageRequest.ToLower());
        if (string.IsNullOrEmpty(sp1.InnerText.ToString()))
            sp1.InnerText = Resources.SchoolResource.ResourceManager.GetString(sPageRequest.ToLower());
        if (sPageRequest.StartsWith("DisplayMenuContents"))
            tdTooltip.Visible = false;
        else if (sPageRequest.StartsWith("ParentTeacherAssociationUI"))
            SetTeansportCommitteeTooltip();
	}

    private string GetPageRequest()
    {
        string sPageRequest = Request.AppRelativeCurrentExecutionFilePath;
        sPageRequest = sPageRequest.Remove(sPageRequest.LastIndexOf("."));
        sPageRequest = sPageRequest.Substring(sPageRequest.LastIndexOf("/") + 1);
        string sCheckEdit = GetQuerstringFlag();
        if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == (Constants.UserRoles.Supervisor))
            sPageRequest = sPageRequest + "_" + Convert.ToInt32(Constants.UserRoles.Admin) + sCheckEdit;
        else
            sPageRequest = sPageRequest + "_" + Convert.ToInt32((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]).ToString() + sCheckEdit;
        
        return sPageRequest;
    }

    private void SetTeansportCommitteeTooltip()
    {
        string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
        string sQueryString = CommonUtility.DecryptQuerystring(sTestDecrypt);
        HttpRequest oHttprequest = new HttpRequest(Page.Request.FilePath, Page.Request.Url.ToString(), sQueryString);
        string sTransportCommitteeFullAccessText = "Click on 'Search' button to search Parent or Teacher or Admin Staff details. And after searching Add/Edit/Delete Parent, Teacher and Admin Staff  in transport committee.";
        string sPTAFullAccessText = "Click on 'Search' button to search Parent or Teacher or Admin Staff details. And after searching Add/Edit/Delete Parent, Teacher and Admin Staff  in PTA.";
        if (Convert.ToInt32(Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]) != Constants.UserRoles.Admin.ToInt())
        {
            if (Convert.ToInt32(oHttprequest.QueryString["SchoolCommitteeId"]) == Constants.SchoolCommittees.Transport.ToInt())
            {
                if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.TransportCommittee) != Constants.C_YES)
                    sp1.InnerText = "Displays members available in transport committee.";
                else
                    sp1.InnerText = sTransportCommitteeFullAccessText;
            }
            else if (Convert.ToInt32(oHttprequest.QueryString["SchoolCommitteeId"]) == Constants.SchoolCommittees.PTA.ToInt())
            {
                if (CommonUtility.IsUserHasEditAccess(Constants.SchoolConfigurations.ParentTeacherAssociation) != Constants.C_YES)
                    sp1.InnerText = "Displays members available in PTA.";
                else
                    sp1.InnerText = sPTAFullAccessText;
            }
        }
        else
        {
            if (Convert.ToInt32(oHttprequest.QueryString["SchoolCommitteeId"]) == Constants.SchoolCommittees.Transport.ToInt())
                sp1.InnerText = sTransportCommitteeFullAccessText;
            else if (Convert.ToInt32(oHttprequest.QueryString["SchoolCommitteeId"]) == Constants.SchoolCommittees.PTA.ToInt())
                sp1.InnerText = sPTAFullAccessText;
        }
    }

    /// <summary>
    /// This method returns querrystring values.
    /// </summary>
    /// <returns></returns>
    private string GetQuerstringFlag()
    {
        string sIsEdit = string.Empty;
        string sTestDecrypt = Server.UrlDecode(Request.QueryString.ToString());
        string sQueryString = CommonUtility.DecryptQuerystring(sTestDecrypt);
        HttpRequest oHttprequest = new HttpRequest(Page.Request.FilePath, Page.Request.Url.ToString(), sQueryString);
        if (oHttprequest.QueryString["Access"] != null)
        {
            sIsEdit = oHttprequest.QueryString["Access"];
            Session["Access"] = oHttprequest.QueryString["Access"].ToString();
            //if (sIsEdit == Constants.S_ZERO)
            //    sIsEdit = Constants.S_ONE;
            //else if (sIsEdit == Constants.S_ONE)
            //    sIsEdit = Constants.S_ONE;               
            //else
            //   sIsEdit = "3";
        }
        return sIsEdit;
    }

    public bool IsNewMenu(string sConfigId)
	{
		bool bFlag = false;
		bool bIsOnlyParentMenu = false;
		DateTime odtTodayDate = DateTime.Today;
		DataTable oDataSet = (DataTable)Session[Constants.S_SESSION_SCHOOL_MENUS];
		DataRow[] oArrParentMenus = oDataSet.Select("Parent_Menu_Id = 0");
		string sEnd_Date;
		if (oArrParentMenus.Length > 0)
		{
			foreach (DataRow oDRParent in oArrParentMenus)
			{
				sEnd_Date = oDRParent["End_Date"].ToString();
				string sText = oDRParent["ConfigureMenuName"].ToString();
				string sValue = oDRParent["ConfigureMenuId"].ToString();
				if (sConfigId == sValue)
				{
					bIsOnlyParentMenu = true;
					DataRow[] oArrRows = oDataSet.Select("Parent_Menu_Id =" + sValue);
					foreach (DataRow oDR in oArrRows)
					{
						bIsOnlyParentMenu = false;
						sEnd_Date = oDR["End_Date"].ToString();
						sText = oDR["ConfigureMenuName"].ToString();
						sValue = oDR["ConfigureMenuId"].ToString();

                        if (sEnd_Date != string.Empty)
                        {
                            DateTime odtEndDate = Convert.ToDateTime(oDR["End_Date"]);
                            if (odtEndDate > odtTodayDate)
                                bFlag = true;
                        }

                        DataRow[] oSubMenuRows = oDataSet.Select("Parent_Menu_Id =" + sValue);
                        foreach (DataRow oDRSubMenu in oSubMenuRows)
                        {
                            bIsOnlyParentMenu = false;
                            sEnd_Date = oDR["End_Date"].ToString();
                            sText = oDR["ConfigureMenuName"].ToString();
                            sValue = oDR["ConfigureMenuId"].ToString();

                            if (sEnd_Date != string.Empty)
                            {
                                DateTime odtEndDate = Convert.ToDateTime(oDR["End_Date"]);
                                if (odtEndDate > odtTodayDate)
                                    bFlag = true;
                            }
                        }   
					}

                    if (bIsOnlyParentMenu && sEnd_Date != string.Empty)
                    {
                        DateTime sParentEnd_Date = Convert.ToDateTime(sEnd_Date);
                        if(sParentEnd_Date > odtTodayDate)
                            bFlag = true ;
                    }
				}
			}
		}
		return bFlag;
	}

    /// <summary>
    /// Function is used for fill menu control.
    /// </summary>
    public void FillMenuControl()
    {
        // This  function is used to fill the menu control with the current menu items.
        DateTime odtTodayDate = DateTime.Today;
        ulMenus.InnerHtml = string.Empty;
        if (Session[Constants.S_SESSION_SCHOOL_MENUS] != null)
        {
            DataTable oDataSet = (DataTable)Session[Constants.S_SESSION_SCHOOL_MENUS];

            DataRow[] oArrParentMenus = oDataSet.Select("Parent_Menu_Id = 0");
            if (oArrParentMenus.Length > 0)
            {
                string strLiHtml = "";
                foreach (DataRow oDRParent in oArrParentMenus)
                {
                    string strNewImageHtml = string.Empty;
                    string strMenuEndDate = oDRParent["End_Date"].ToString();
                    string strMenuText = oDRParent["ConfigureMenuName"].ToString();
                    string strMenuValue = oDRParent["ConfigureMenuId"].ToString();

                    if (IsNewMenu(strMenuValue))
                        strNewImageHtml = "<span>&nbsp;&nbsp;<img src='/RITeSchool/images/new.png'/>&nbsp;&nbsp;</span>";
                    else
                        strNewImageHtml = "<span><label>&nbsp;</label></span>";

                    // Add menus which are sub menus.
                    DataRow[] oArrRows = oDataSet.Select("Parent_Menu_Id =" + strMenuValue);
                    string strSubMenuHtml = string.Empty;

                    if (oArrRows.Count() > 0)
                    {
                        DataView dv = oArrRows.CopyToDataTable().DefaultView;
                        dv.Sort = "Priority asc";
                        DataTable sortedDT = dv.ToTable();

                        string strSubMenuText = string.Empty;
                        string strSubMenuValue = string.Empty;
                        string strSubmenuNewImageHtml = string.Empty;
                        foreach (DataRow oDR in sortedDT.Rows)
                        {
                            strMenuEndDate = oDR["End_Date"].ToString();
                            strSubMenuText = oDR["ConfigureMenuName"].ToString();
                            strSubMenuValue = oDR["ConfigureMenuId"].ToString();

                            if (strMenuEndDate != string.Empty)
                            {
                                DateTime odtEndDate = Convert.ToDateTime(oDR["End_Date"]);
                                if (odtEndDate > odtTodayDate)
                                    strSubmenuNewImageHtml = "<img src='/RITeSchool/images/new.png'/>&nbsp;&nbsp;";
                                else
                                    strSubmenuNewImageHtml = string.Empty;
                            }
							// Add menus which are child menus.
                            DataRow[] oArrSubMenu = oDataSet.Select("Parent_Menu_Id = " + strSubMenuValue);
							string strSubMenuItemHtml = string.Empty;
                            strSubmenuNewImageHtml = string.Empty;
                            if (oArrSubMenu.Count() > 0)
                            {
                                foreach (DataRow oDRSubMenu in oArrSubMenu)
                                {
                                    string strSubMenuEndDate = oDRSubMenu["End_Date"].ToString();
                                    string strSubSubMenuText = oDRSubMenu["ConfigureMenuName"].ToString();
                                    string strSubSubMenuValue = oDRSubMenu["ConfigureMenuId"].ToString();

                                    if (strSubMenuEndDate != string.Empty)
                                    {
                                        DateTime odtEndDate = Convert.ToDateTime(oDRSubMenu["End_Date"]);
                                        if (odtEndDate > odtTodayDate)
                                            strSubmenuNewImageHtml = "<img src='/RITeSchool/images/new.png'/>&nbsp;&nbsp;";
                                        else
                                            strSubmenuNewImageHtml = string.Empty;
                                    }
                                    strSubMenuItemHtml += "<li class='dropdown-hover'><a href='/RITeSchool" + GetEncryptedMenuQueryString(strSubSubMenuValue, strSubSubMenuText).Remove(0, 1) + "'>" + strSubmenuNewImageHtml + strSubSubMenuText + "</a>";

                                    DataRow[] oSubMenu = oDataSet.Select("Parent_Menu_Id = " + strSubSubMenuValue);
                                    string strChildSubMenuEndDate = string.Empty;
                                    string strChildSubMenuMenuText = string.Empty;
                                    string strChildSubMenuValue = string.Empty;
                                    string strChildSubMenuItemHtml = string.Empty;
                                    if (oSubMenu.Count() > 0)
                                    {
                                        foreach (DataRow oChildSubMenu in oSubMenu)
                                        {
                                            strChildSubMenuEndDate = oChildSubMenu["End_Date"].ToString();
                                            strChildSubMenuMenuText = oChildSubMenu["ConfigureMenuName"].ToString();
                                            strChildSubMenuValue = oChildSubMenu["ConfigureMenuId"].ToString();

                                            if (strChildSubMenuEndDate != string.Empty)
                                            {
                                                DateTime odtEndDate = Convert.ToDateTime(oChildSubMenu["End_Date"]);
                                                if (odtEndDate > odtTodayDate)
                                                    strSubmenuNewImageHtml = "<img src='/RITeSchool/images/new.png'/>&nbsp;&nbsp;";
                                                else
                                                    strSubmenuNewImageHtml = string.Empty;
                                            }
                                            strChildSubMenuItemHtml += "<li><a href='/RITeSchool" + GetEncryptedMenuQueryString(strChildSubMenuValue, strChildSubMenuMenuText).Remove(0, 1) + "'>" + strSubmenuNewImageHtml + strChildSubMenuMenuText + "</a></li>";
                                        }
                                        strSubMenuItemHtml += "<ul class='dropdown-menu dropdown-danger' style='margin-left:100px; top:0px;'>" + strChildSubMenuItemHtml + "</ul></li>";                                       
                                    }
                                }
                                strSubMenuItemHtml = "<ul class='dropdown-menu subMenu'>" + strSubMenuItemHtml + "</ul>";

                            strSubMenuHtml += "<li class='dropdown-hover'><a href='/RITeSchool" + GetEncryptedMenuQueryString(strSubMenuValue, strSubMenuText).Remove(0, 1) + "'>" + strSubmenuNewImageHtml + strSubMenuText + "</a> " + strSubMenuItemHtml + "</li>";
                            }
                            else
                                strSubMenuHtml += "<li><a href='/RITeSchool" + GetEncryptedMenuQueryString(strSubMenuValue, strSubMenuText).Remove(0, 1) + "'>" + strSubmenuNewImageHtml + strSubMenuText + "</a></li>";
                        }
                        strSubMenuHtml = "<ul class='dropdown-menu dropdown-danger'>" + strSubMenuHtml + "</ul>";
                    }

                    strLiHtml += "<li class='dropdown-hover'><a href='/RITeSchool" + GetEncryptedMenuQueryString(strMenuValue, strMenuText).Remove(0, 1) + "'>" + strNewImageHtml + strMenuText + "</a>" + strSubMenuHtml + "</li>";
                }

                ulMenus.InnerHtml = strLiHtml;
            }
            else
                trMenu.Visible = false;
        }
    }


	private string GetEncryptedMenuQueryString(string asMenuId, string asManuName)
	{
		string sQuerystring = string.Empty;
		asManuName = asManuName.Replace("&", " sps ");
		sQuerystring = "MenuId=" + asMenuId + "&MenuName=" + asManuName;
		string sEncryptedString = "~/Common/DisplayMenuContents.aspx?" + Utility.CommonUtility.EncryptQuerystring(sQuerystring);

		return sEncryptedString;
	}

	private void ClearTempararySession()
	{
		Session.Remove(Constants.S_TEMP_SESSION_DS);
	}


    /// <summary>
    /// This method is used to set profile image of logged in user.
    /// </summary>
    private void SetSchoolLogoAndProfileImage()
    {
        // Set school logo from physical path    
        imgSchoolLogo.Src = Constants.S_SCHOOL_LOGO_FILE_PATH;
       
        if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME] != null)
        {
            if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME].ToString() == Constants.DemoCompanyName.Marpha.ToString())
            {
                imgRITeschoolSchoolLogo.ImageUrl = "/RITeSchool/images/Logos/School_Login_Logo.PNG";
                imgSchoolLogo.Visible = false;
            }
            else if (Session[Constants.S_SESSION_DEMO_COMPANY_NAME].ToString() == Constants.DemoCompanyName.GMore.ToString())
                imgRITeschoolSchoolLogo.ImageUrl = "/RITeSchool/images/Logos/School_Login.PNG";
            
        }
        else
            imgRITeschoolSchoolLogo.ImageUrl = "../images/NewLogo.jpg";  
       
        profilePicContainer.Attributes.Add("title", Convert.ToString(Session[Constants.S_SESSION_USER_LAST_LOGIN]));
    }

    /// <summary>
    /// This method is used to display settings for RITeStore.
    /// </summary>
    private void SetRITeStoreDisplaySettings()
    {
        hlnkStore.Visible = false;
        if (SchoolBase.Settings.EnableRITeStoreModule && !string.IsNullOrEmpty(SchoolBase.Settings.ExternalRITeStoreURL))
        {
            if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID].ToInt() == Constants.UserRoles.Student.ToInt() 
                && Session[Constants.S_SESSION_STUDENT_ID] != null)
            {
                hlnkStore.Visible = true;
                hlnkStore.NavigateUrl = GetRITeStoreURL();
            }
        }
    }

    /// <summary>
    /// This method is used to get RITeStore URL for respective school.
    /// </summary>
    /// <returns></returns>
    private string GetRITeStoreURL()
    {
        int iSchoolId = Convert.ToInt32(Session[Constants.S_SESSION_SCHOOL_ID]);
        int iStudentId = Convert.ToInt32(Session[Constants.S_SESSION_STUDENT_ID]);
        StudentBL oStudentBL = new StudentBL(iStudentId);

        SchoolEntities.RITeStoreUser storeUser = new SchoolEntities.RITeStoreUser();
        storeUser.SchoolId = iSchoolId;
        storeUser.StudentId = oStudentBL.StudentId;
        storeUser.FirstName = (oStudentBL.FirstName != null ? oStudentBL.FirstName : string.Empty);
        storeUser.LastName = (oStudentBL.LastName != null ? oStudentBL.LastName : string.Empty);
        storeUser.Email = (oStudentBL.Email != null ? oStudentBL.Email : string.Empty);
        storeUser.Address = (oStudentBL.Address != null ? oStudentBL.Address : string.Empty);
        storeUser.MobilePhoneNo = (oStudentBL.MobilePhoneNo != null ? oStudentBL.MobilePhoneNo : string.Empty);

        string storeUserDetails = new JavaScriptSerializer().Serialize(storeUser);
        string sEncrypt = CommonUtility.EncryptQuerystring(storeUserDetails);

        return SchoolBase.Settings.ExternalRITeStoreURL + sEncrypt;
    }

    #endregion -- PRIVATE METHOD(s) --
}
