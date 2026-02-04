// File Name      : SuperAdminMasterPage.master.cs
// Modified  By   : Amit
// Modified Date  : 26 Sept 2009
// Description    : This class is used to create master page for superadmin.

using System;
using System.Reflection;
using System.Threading;
using System.Web.Security;
using BusinessLogic.Exceptions;
using Utility;

public partial class SuperAdminMasterPage : BaseMasterPage
{

	#region -- MEMBER(s) --

	private string msPageUrl = String.Empty;

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

			// If the Session is lost, we redirect the user to superadmin login page.
			if (!msPageUrl.ToUpper().Equals("~/RITESCHOOL/SUPERADMIN/SUPERADMINUI.ASPX"))
			{
				if (Session[Constants.S_SESSION_SUPER_ADMIN_USER_ID] == null)
					Response.Redirect("~/RITeSchool/SuperAdmin/SuperAdminUI.aspx", true);
			}

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
	/// This event is used to set default properties for page controls.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnLoad(EventArgs e)
	{
		try
		{
			base.OnLoad(e);

			string sPageRequest = Request.AppRelativeCurrentExecutionFilePath;
			sPageRequest = sPageRequest.Substring(sPageRequest.LastIndexOf("/") + 1);

			if (sPageRequest.ToUpper().Equals("SUPERADMINUI.ASPX"))
				tblLinkLogout.Visible = false;

			Response.Buffer = true;
			Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
			Response.Expires = -1;
			Response.Cache.AppendCacheExtension("max-age=0, no-store, must-revalidate");

			// The title is not set in !IsPostBack because for the pages like annual planner
			// where AJAX is not implemented, after changing the month the title is reset to its URL.
			Page.Title = Constants.S_TITLE_FOR_PAGE;
			hidServerDate.Value = Convert.ToString(DateTime.Now.Date.Year);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	/// <summary>
	/// This event is used for logout the super admin authentication.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void lnkSuperAdminLogout_Click(object sender, EventArgs e)
	{
		try
		{
			FormsAuthentication.SignOut();
			Session.Abandon();
			Response.Redirect("~/RITeSchool/SuperAdmin/SuperAdminUI.aspx", false);
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	#endregion -- EVENT HANDLER(s) --

	#region -- PUBLIC METHOD(s) --

	/// <summary>
	/// This method is used to postback url ans clears session variable.  
	/// </summary>
	/// <param name="asPageName"></param>
	public void RedirectToNextPage(string asPageName)
	{
		if (asPageName.IndexOf("RITeSchool/") == -1)
			asPageName = asPageName.Replace("~/", "~/RITeSchool/");
		Response.Redirect(asPageName, false);
		ClearTempararySession();
	}

	/// <summary>
	/// Hides the dashboard link.
	/// </summary>
	public void HideLink()
	{
		lnkDashboard.Visible = false;
	}

	#endregion -- PUBLIC METHOD(s) --

	#region -- PRIVATE METHOD(s) --

	/// <summary>
	/// This method is used to clear session variable.
	/// </summary>
	private void ClearTempararySession()
	{
		Session.Remove(Constants.S_TEMP_SESSION_DS);
	}

	#endregion -- PRIVATE METHOD(s) --

}
