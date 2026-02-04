/* ----------------------------------------------------------------------------
 *	FileName	: BaseMasterPage.cs
 *	Author		: Vishal B. Shah
 *	Date		: 3-May-2012
 * ---------------------------------------------------------------------------- 
 */

using System;
using System.Reflection;
using System.Web.UI;
using BusinessLogic;
using BusinessLogic.Exceptions;
using Utility;

/// <summary>
///		Serves as the base class for all Master Pages in the website.
///		It is used to record all page requests and db activity.
/// </summary>
public class BaseMasterPage : MasterPage
{
	#region -- MEMBER(s) --

	private string msPageUrl = String.Empty;

	#endregion -- MEMBER(s) --

	#region -- EVENT HANDLER(s) --

	/// <summary>
	/// Records page request related information and stores it in the session for later use.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnInit(EventArgs e)
	{
		try
		{
			base.OnInit(e);

			msPageUrl = Request.AppRelativeCurrentExecutionFilePath;
			
			if (Constants.B_ACTIVITY_LOGGING)
				ActivityLoggingBL.RecordPageRequest();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
	}

	/// <summary>
	/// Logs all page request & db activty related info to the db.
	/// </summary>
	/// <param name="e"></param>
	protected override void OnUnload(EventArgs e)
	{
		try
		{
			base.OnUnload(e);

			if (Constants.B_ACTIVITY_LOGGING)
				ActivityLoggingBL.LogActivity();
		}
		catch (Exception ex)
		{
			ExceptionHandler.WriteExceptionToErrorLog(ex, MethodBase.GetCurrentMethod(), msPageUrl);
		}
		finally
		{
			if (Constants.B_ACTIVITY_LOGGING)
				ActivityLoggingBL.ClearActivityLog();
		}
	}

	#endregion -- EVENT HANDLER(s) --
}