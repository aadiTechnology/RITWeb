// File Name   : PopupMaster.aspx.cs
// Modified by : Amit
// Date        : 29 Sept 2009
// Descrption  : This class is used to create master page for popup screens.

using System;
using System.Reflection;
using System.Threading;
using BusinessLogic.Exceptions;
using Utility;
using System.Resources;
public partial class BasicMaster : BaseMasterPage
{

	#region -- MEMBER(s) --

    private string msPageUrl = String.Empty;
    ResourceManager oResourceManager = new ResourceManager(typeof(Resources.LocalizedResources));

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Collects information abou the Page Request.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
	{
		try
		{
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

	/// <summary>
	/// This event is used to set properties of page controls. 
	/// </summary>
	/// <param name="e"></param> 
	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);

            if (Session[Constants.S_SESSION_LANGUAGE] == null)
                Session[Constants.S_SESSION_LANGUAGE] = "en";

			if (!Session[Constants.S_SESSION_USER_ID].IsNull())
                hidSessionUserId.Value = Session[Constants.S_SESSION_USER_ID].ToString();
			Page.Title = Constants.S_TITLE_FOR_PAGE;
			SetScreenTooltip();
            hidRightsReserved.Value = Resources.LocalizedResources.RightsReserved;
            hidRIT.Value = Resources.LocalizedResources.RIT;
            hidServerDate.Value = Convert.ToString(DateTime.Now.Date.Year);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to set tooltip for screen.
	/// </summary>
	private void SetScreenTooltip()
	{
        //string sPageRequest = GetPageRequest();
        //sp1.InnerText = oResourceManager.GetString(sPageRequest.ToLower());
        //if (string.IsNullOrEmpty(sp1.InnerText.ToString()))
        //    sp1.InnerText = Resources.SchoolResource.ResourceManager.GetString(sPageRequest.ToLower());
        //if (sPageRequest.ToUpper().Equals("SMSTEMPLATEUI_1") && Request.QueryString.Count > 0)
        //    sp1.InnerText = "Select required template to create the SMS.";

	}

	/// <summary>
	/// This method is used get name of file from request is come.
	/// </summary>
	/// <returns></returns>
	public string GetPageRequest()
	{
		string sPageRequest = Request.AppRelativeCurrentExecutionFilePath;
		sPageRequest = sPageRequest.Remove(sPageRequest.LastIndexOf("."));
		sPageRequest = sPageRequest.Substring(sPageRequest.LastIndexOf("/") + 1);

		if (Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] != null)
		{
			if ((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID] == Constants.UserRoles.Supervisor)
			    sPageRequest = sPageRequest + "_" + Convert.ToInt32(Constants.UserRoles.Admin);
			else
			    sPageRequest = sPageRequest + "_" + Convert.ToInt32((Constants.UserRoles)Session[Constants.S_SESSION_USER_LOGIN_ROLE_ID]);
		}

		sPageRequest = sPageRequest.ToLower();
		return sPageRequest;
	}

	/// <summary>
	/// This method is used to move to previous page.
	/// </summary>
	/// <param name="asPageName"></param>
	public void RedirectToNextPage(string asPageName)
	{
		if (asPageName.IndexOf("RITeSchool/") == -1)
			asPageName = asPageName.Replace("~/", "~/RITeSchool/");
		Response.Redirect(asPageName, false);
	}

	#endregion -- PRIVATE METHOD(s) --

}
