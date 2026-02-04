using System;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Web;
using Utility;
using System.Data;
using System.Data.SqlClient;

namespace DataCommunicator
{
	/// <summary>
	///		Provides an API for error logging.
	/// </summary>
	public static class ErrorLogDC
	{
		#region -- CONSTANT(s) --

		private const string S_DEFAULT_BROWSER = "Unknown 0.0";

		#endregion -- CONSTANT(s) --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Logs the given error to the database.
		/// </summary>
		/// <param name="ex">The exception to be logged.</param>
		/// <param name="method">The method which caught the exception.</param>
		public static void WriteExceptionToErrorLog(Exception ex, MethodBase method)
		{
			WriteExceptionToErrorLog(ex, method, null);
		}

		/// <summary>
		///		Logs the given error to the database.
		/// </summary>
		/// <param name="ex">The exception to be logged.</param>
		/// <param name="method">The method which caught the exception.</param>
		/// <param name="asMessage">An extra message to be logged in the database along with the error.</param>
        public static void WriteExceptionToErrorLog(Exception ex, MethodBase method, String asMessage, bool abIsServiceCall = false)
		{
			int iUserId = 0;
			string sUserName = string.Empty;
			string sSchoolName = string.Empty;
			if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Constants.S_SESSION_USER_ID] != null)
				iUserId = HttpContext.Current.Session[Constants.S_SESSION_USER_ID].ToInt();
			if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Utility.Constants.S_SESSION_USER_NAME] != null)
			 sUserName = HttpContext.Current.Session[Utility.Constants.S_SESSION_USER_NAME].ToString();
			if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session[Utility.Constants.S_SESSION_SCHOOL_NAME] != null)
			 sSchoolName = HttpContext.Current.Session[Utility.Constants.S_SESSION_SCHOOL_NAME].ToString();

            WriteExceptionToErrorLog(ex, method, iUserId, asMessage, sUserName, sSchoolName, abIsServiceCall); 
		}

        /// <summary>
        ///		Logs the given error to the database.
        /// </summary>
        /// <param name="asMessage">The error log message and stack trace.</param>
        /// <param name="asFunction">The function which caught the exception.</param>
        /// <param name="aiUserId">The UserId of the current user.</param>
        public static void WriteExceptionToErrorLog(string asMessage, string asFunction, int aiUserId)
        {
            WriteExceptionToErrorLog(asMessage, asFunction, null, aiUserId, Convert.ToInt32(Constants.S_SCHOOLID));
        }

		#endregion -- PUBLIC METHOD(s) --

		#region -- PRIVATE METHOD(s) --

		/// <summary>
		///		Logs the given error to the database.
		/// </summary>
		/// <param name="ex">The exception to be logged.</param>
		/// <param name="method">The method which caught the exception.</param>
		/// <param name="aiUserId">The UserId of the current user.</param>
		/// <param name="asMessage">An extra message to be logged in the database along with the error.</param>
        private static void WriteExceptionToErrorLog(Exception ex, MethodBase method, int aiUserId, string asMessage, string asUserName, string asSchoolName, bool abIsServiceCall = false)
		{
            bool bIsErrorLogExist = IsErrorLogExist(aiUserId, method.DeclaringType.FullName, method.Name, (asMessage == null ? String.Empty : String.Format("Message - {0}. ", StringUtility.ReplaceSingleQuoteInString(asMessage, true))), StringUtility.ReplaceSingleQuoteInString(ex.Message, true));
            if (!bIsErrorLogExist && aiUserId != 0)
            {
                string sSqlStatement = String.Format("INSERT Error_Log(User_Id, Description) VALUES ({0}, '{1}.{2} : {3}{4} Trace : {5}')",
                                                      aiUserId,
                                                      method.DeclaringType.FullName,
                                                      method.Name,
                                                      asMessage == null ? String.Empty : String.Format("Message - {0}. ", StringUtility.ReplaceSingleQuoteInString(asMessage, true)),
                                                      StringUtility.ReplaceSingleQuoteInString(ex.Message, true),
                                                      StringUtility.ReplaceSingleQuoteInString(ex.StackTrace, true));

                int iErrorLogId = LogError(Convert.ToInt32(Constants.S_SCHOOLID), aiUserId, sSqlStatement, abIsServiceCall);

                string sBrowserInfo = null;
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    sBrowserInfo = String.Format("{0} {1}", HttpContext.Current.Request.Browser.Browser, HttpContext.Current.Request.Browser.Version);

                var sbContent = new StringBuilder();
                sbContent.Append("<pre>");
                sbContent.AppendFormat("This is an auto generated mail from the School Error Log.{0}", Environment.NewLine);
                sbContent.AppendFormat("An error log entry is added into the school database as follows.{0}", Environment.NewLine);
                sbContent.AppendFormat("Please take necessary action to resolve it on priority.{0}{0}", Environment.NewLine);
                sbContent.AppendFormat("ErrorLog Id  : {0}{1}", iErrorLogId, Environment.NewLine);
                sbContent.AppendFormat("School Id    : {0}{1}", Constants.S_SCHOOLID, Environment.NewLine);
                sbContent.AppendFormat("School Name  : {0}{1}", asSchoolName, Environment.NewLine);
                sbContent.AppendFormat("User Id      : {0}{1}", aiUserId, Environment.NewLine);
                sbContent.AppendFormat("User Name    : {0}{1}", asUserName, Environment.NewLine);
                sbContent.AppendFormat("DateTime     : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
                sbContent.AppendFormat("Browser      : {0}{1}", sBrowserInfo ?? S_DEFAULT_BROWSER, Environment.NewLine);
                sbContent.AppendFormat("Origin       : {0}.{1}{2}", method.DeclaringType.FullName, method.Name, Environment.NewLine);
                sbContent.AppendFormat("Error        : {0}{1}", HttpUtility.HtmlEncode(ex.Message), Environment.NewLine);
                if (asMessage != null)
                    sbContent.AppendFormat("Message      : {0}{1}", HttpUtility.HtmlEncode(asMessage), Environment.NewLine);
                sbContent.AppendFormat("Stack Trace  : {0}{1}", Environment.NewLine, HttpUtility.HtmlEncode(ex.StackTrace));
                sbContent.Append("</pre>");

                sbContent.Replace(Environment.NewLine, "<br />");

                NotifyErrorLog(sbContent.ToString());
            }
		}

        /// <summary>
        /// To check whether there exist same error log for the day.
        /// </summary>
        /// <param name="aiUserId"></param>
        /// <param name="asClassName"></param>
        /// <param name="asMethodName"></param>
        /// <param name="asExtraMessage"></param>
        /// <param name="asErrorMessage"></param>
        /// <returns></returns>
        private static bool IsErrorLogExist(int aiUserId, string asClassName, string asMethodName, string asExtraMessage, string asErrorMessage)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                string sMessage = asClassName + "." + asMethodName + " : " + asExtraMessage + asErrorMessage;
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Message", sMessage, SqlDbType.NVarChar);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsExist", 0, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_IsErrorLogPresent");
                return oSqlParameter.Value.ToBool();
            }
        }

        /// <summary>
        /// Logs the given error to the database.
        /// </summary>
        /// <param name="asMessage">The error message to be logged.</param>
        /// <param name="asFunction">The name of the function which caught the error that is being logged.</param>
        /// <param name="asBrowserInfo">Browser related information.</param>
        /// <param name="aiCurrentUserId">The UserId of the current user.</param>
        /// <param name="aiSchoolId">The current school id, In case of Existing code it will be 
        /// read from constant as parameter it will be default. In case of school mobile service it will pass as parmeter</param>
        public static void WriteExceptionToErrorLog(string asMessage, string asFunction, string asBrowserInfo, int aiCurrentUserId, int aiSchoolId = 0, bool abIsServiceCall = false)
        {
            string sErrorLog = StringUtility.ReplaceSingleQuoteInString(asFunction + " : " + asMessage, false);

            string sSqlStatement = String.Format("INSERT Error_Log(User_Id, Description) VALUES ({0}, '{1}')", aiCurrentUserId, sErrorLog);

            int iErrorLogId = LogError(aiSchoolId, aiCurrentUserId, sSqlStatement, abIsServiceCall);

            var sbContent = new StringBuilder();
            sbContent.Append("<pre>");
            sbContent.AppendFormat("This is an auto generated mail from the School Error Log.{0}", Environment.NewLine);
            sbContent.AppendFormat("An error log entry is added into the school database as follows.{0}", Environment.NewLine);
            sbContent.AppendFormat("Please take necessary action to resolve it on priority.{0}{0}", Environment.NewLine);
            sbContent.AppendFormat("ErrorLog Id : {0}{1}", iErrorLogId, Environment.NewLine);
            sbContent.AppendFormat("School Id   : {0}{1}", aiSchoolId, Environment.NewLine);
            sbContent.AppendFormat("User Id     : {0}{1}", aiCurrentUserId, Environment.NewLine);
            sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            sbContent.AppendFormat("Browser     : {0}{1}", asBrowserInfo ?? S_DEFAULT_BROWSER, Environment.NewLine);
            sbContent.AppendFormat("Origin      : {0}{1}", asFunction, Environment.NewLine);
            sbContent.Append(HttpUtility.HtmlEncode(asMessage));
            sbContent.Append("</pre>");

            sbContent.Replace(Environment.NewLine, "<br />");

            NotifyErrorLog(sbContent.ToString());
        }

		/// <summary>
		///		Logs the error to database.
		/// </summary>
		/// <param name="asSqlStatement">The SQLStatement to be executed.</param>
		/// <returns>The Id of the row inserted.</returns>
        private static int LogError(int aiSchoolId, int aiCurrentUserId, string asSqlStatement, bool abIsServiceCall)
		{
            using (var oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, aiCurrentUserId, abIsServiceCall))
                return oSQLServerDbUtility.ExecuteTransaction(asSqlStatement);
		}

		/// <summary>
		///		Notifies the respective authorities of the error log via email.
		/// </summary>
		/// <param name="sContent">The contents of the email.</param>
		private static void NotifyErrorLog(string sContent)
		{
			string sToMailAddress = ConfigurationManager.AppSettings["ErrorLogEmailAddresses"];
			if (sToMailAddress.IsNullOrEmpty())
				sToMailAddress = "admin@regulusit.net";

			string sFromMailAddress = Constants.S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN;
			string sSubject = "School Error Log Notification";

			string sIsDemoSite = ConfigurationManager.AppSettings["IsDemoSite"];
			if (!sIsDemoSite.IsNullOrEmpty() && sIsDemoSite == Constants.S_YES)
				sSubject = "RITeSchool Error Log Notification";

			CommonUtility.SendE_Mail(sToMailAddress, sFromMailAddress, sSubject, sContent);
		}

        /// <summary>
        ///		Notifies the respective authorities of the error log via email.
        /// </summary>
        /// <param name="sContent">The contents of the email.</param>
        public static void NotifyErrorLog(string asReplaceSingleQuoteString, int aiUserId, string asSchoolId)
        {
            string sToMailAddress = ConfigurationManager.AppSettings["ErrorLogEmailAddresses"];
            if (sToMailAddress.IsNullOrEmpty())
                sToMailAddress = "admin@regulusit.net";
            string asFromMailAddress = Constants.S_FROM_EMAIL_ADDRESS_OF_SITE_ADMIN;             
            string asSubject = "School Error Log Notification ";            
            var sbContent = new StringBuilder();
            sbContent.Append("<pre>");
            sbContent.AppendFormat("This is an auto generated mail from the School Error Log.{0}", Environment.NewLine);
            sbContent.AppendFormat("An error log entry is added into the school database as follows.{0}", Environment.NewLine);
            sbContent.AppendFormat("Please take necessary action to resolve it on priority.{0}{0}", Environment.NewLine);
            sbContent.AppendFormat("ErrorLog Id : {0}{1}", -999999, Environment.NewLine);
            sbContent.AppendFormat("School Id   : {0}{1}", asSchoolId, Environment.NewLine);
            sbContent.AppendFormat("User Id     : {0}{1}", aiUserId, Environment.NewLine);
            sbContent.AppendFormat("DateTime    : {0}{1}", DateTime.Now.ToString(), Environment.NewLine);
            
            sbContent.Append(HttpUtility.HtmlEncode(asReplaceSingleQuoteString));
            sbContent.Append("</pre>");

            sbContent.Replace(Environment.NewLine, "<br />");

            CommonUtility.SendE_Mail(sToMailAddress, asFromMailAddress, asSubject, sbContent.ToString());
        }

		#endregion -- PRIVATE METHOD(s) --
	}
}