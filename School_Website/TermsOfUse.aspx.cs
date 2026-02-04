/* File Name :- TermsOfUse.aspx.cs
 * Modified By :- Sachin
 * Modified Date :- 18-Sept-2009
 * Purpose :- Code Review.
 * Class Description :- This class is used to display terms of use.
*/

using System;
using System.Reflection;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

public partial class TermsOfUse : SchoolBase
{

	#region Events

	/// <summary>
	/// This event is used to decrypt query string and hide buttons.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		this.Page.Title = Constants.S_TITLE_FOR_PAGE;
		try
		{
			if (!IsPostBack)
				SetJavascriptAttributes();

			if (QueryString.Count == Constants.I_ZERO || QueryString["login"] == false.ToString())
			{
				trBtns.Visible = false;
				trline.Visible = false;
			}

            divOnline.Visible = Settings.EnabledOnlineFee;
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}

	/// <summary>
	/// This event is used to redirect towards control panel.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void btnContinue_Click(object sender, EventArgs e)
	{
		try
		{
			string sLogin = QueryString["sLogin"];
			string sPassword = QueryString["sPassword"];
           
			int iSchoolId = QueryString["iSchoolId"].ToInt();
			string sIPAddress = Request.UserHostAddress;
            sLogin = sLogin.Replace("%USN%", "&");
            sPassword = sPassword.Replace("%PWD%", "&");
			var oUserAuthentication = new UserAuthentication(iSchoolId, sLogin, sPassword, sIPAddress);
			if (oUserAuthentication.ValidUser)
			{	
			oUserAuthentication.UpdateSession();			
            Response.Redirect("RITeSchool/Common/StudentChangePassword.aspx", false);
			}
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod());
		}
	}
	#endregion

	#region Method

	/// <summary>
	/// This method is used to set java script attributes.
	/// </summary>
	private void SetJavascriptAttributes()
	{
		rdoAccept.Attributes.Add("onclick", "return enabledisablecontrols('" + rdoAccept.ClientID + "');");
		rdoNoAccept.Attributes.Add("onclick", "return enabledisablecontrols('" + rdoNoAccept.ClientID + "');");
	}

	#endregion
}