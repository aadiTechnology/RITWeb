/* ----------------------------------------------------------------------------
 *	Author		: Vishal Shah
 *  Date		: 20-Sept-2011
 *  Description : This is the Business Logic layer for Logging Database activity to the database
 * ----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DataCommunicator;
using SchoolEntities;
using Utility;

namespace BusinessLogic
{
	public class ActivityLoggingBL
	{
		#region -- PROPERTIES --

		/// <summary>
		///		Returns true if the System.Web.HttpContext.Current.Session is null.
		/// </summary>
		private static bool IsSessionNull
		{
			get { return HttpContext.Current == null || HttpContext.Current.Session == null; }
		}
		
		#endregion -- PROPERTIES --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Creates a PageRequestLog entity object populated with details regarding the current PageRequest and stores it in the session.
		/// </summary>
		/// <returns></returns>
		public static void RecordPageRequest()
		{
			if (IsSessionNull)
				return;
			
			// Fetch the query string in decrypted format.
			string sQueryString = String.Empty;
			if (HttpContext.Current.Request.QueryString.Count > 0)
			{
				string sTempQueryString = HttpContext.Current.Request.QueryString.ToString();
				try
				{
					// We decrypt the query string only if it contains a trailing q character.
					// There are some pages in the School application, which do not encrypt the query string, such as PaymentStusUI & SchoolReportUI.
					// For such pages, the query string will not contain a trailing q character, so we do not need to decrypt it.
					// Also, the reason for performing decryption in a try block is to record the original query string incase something goes wrong.
					sQueryString = sTempQueryString.EndsWith("q") ? CommonUtility.DecryptQuerystring(HttpContext.Current.Server.UrlDecode(sTempQueryString)) : sTempQueryString;
				}
				catch (Exception)
				{
					sQueryString = sTempQueryString;
				}
			}

			// Get UserId, SchoolId  AcademicYearId.
			int iUserId = 0, iSchoolId = 0, iAcademicYearId = 0;
			if (!HttpContext.Current.Session[Constants.S_SESSION_USER_ID].IsNull())
				Int32.TryParse(HttpContext.Current.Session[Constants.S_SESSION_USER_ID].ToString(), out iUserId);
			if (!HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID].IsNull())
				Int32.TryParse(HttpContext.Current.Session[Constants.S_SESSION_SCHOOL_ID].ToString(), out iSchoolId);
			if (!HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].IsNull())
				Int32.TryParse(HttpContext.Current.Session[Constants.S_SESSION_CURRENT_ACADEMIC_YEAR_ID].ToString(), out iAcademicYearId);

			// Determine if the current request is a postback.
			bool bIsPostBack = false;

			var currentPage = HttpContext.Current.Handler as Page;
			
			if (currentPage != null)
				bIsPostBack = currentPage.IsPostBack;
			
			// Create and save a PageRequestLog object in session for later use.
			HttpContext.Current.Session[Constants.S_SESSION_PAGE_REQUEST] = new PageRequestLog
																				{
																					SessionId		= HttpContext.Current.Session.SessionID,
																					UserId			= iUserId,
																					IPAddress		= HttpContext.Current.Request.UserHostAddress,
																					Browser			= HttpContext.Current.Request.Browser.Browser,
																					BrowserVersion	= HttpContext.Current.Request.Browser.Version,
																					Page			= HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath,
																					QueryString		= sQueryString,
																					IsPostBack		= bIsPostBack,
																					RequestData		= new List<KeyValuePair<string, string>>(),
																					InsertDate		= DateTime.Now,
																					SchoolId		= iSchoolId,
																					AcademicYearId	= iAcademicYearId,
																					ActivityLog		= new List<ActivityLog>()
																				};
		}

		/// <summary>
		///		This function is used to log all the ActivityLog items present in the ActivityLogColletion item, to the database.
		/// </summary>
		public static void LogActivity()
		{
			if (IsSessionNull)
				return;
			
			var pageRequestLog = HttpContext.Current.Session[Constants.S_SESSION_PAGE_REQUEST] as PageRequestLog;

			if (pageRequestLog == null)
				return;

			string sRequestData = ParseRequestData(pageRequestLog.RequestData);
			string sActivityLogXML = pageRequestLog.ActivityLog.Count > 0 ? CommonUtility.GetXMLForList(pageRequestLog.ActivityLog) : String.Empty;
			
			ActivityLoggingDC.LogActivity(pageRequestLog, sRequestData, sActivityLogXML);
		}

		/// <summary>
		///		Removes the PageRequestLog object from Session.
		/// </summary>
		public static void ClearActivityLog()
		{
			if (!IsSessionNull)
				HttpContext.Current.Session.Remove(Constants.S_SESSION_PAGE_REQUEST);
		}

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		///		
		/// </summary>
		/// <param name="alstRequestData"></param>
		/// <returns></returns>
		private static string ParseRequestData(List<KeyValuePair<string, string>> alstRequestData)
		{
			if (alstRequestData == null || alstRequestData.Count <= 0)
				return null;
		
			return alstRequestData.Select(a => String.Format("{0} = {1}.", a.Key, a.Value ?? "NULL"))
								  .Aggregate((a, b) => String.Format("{0} {1}.", a, b));
		}

		#endregion -- PRIVATE METHOD(s) --
	}
}
