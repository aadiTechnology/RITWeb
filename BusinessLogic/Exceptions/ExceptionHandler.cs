using System;
using System.Reflection;
using System.Web;
using DataCommunicator;
using Utility;
using System.IO;

namespace BusinessLogic.Exceptions
{
    /// <summary>
    ///		Provides an API for error logging.
    /// </summary>
	public class ExceptionHandler
    {
        #region -- PUBLIC METHOD(s) --

        /// <summary>
        ///		Logs the given error to the database.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="method">The method which caught the exception.</param>
		public static void WriteExceptionToErrorLog(Exception ex, MethodBase method)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session["RITMiniSite"] != null && HttpContext.Current.Session["RITMiniSite"].ToString() == Constants.S_YES)
				ErrorLog(ex, method);
			else
				ErrorLogDC.WriteExceptionToErrorLog(ex, method);
        }

        /// <summary>
        ///		Logs the given error to the database.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="method">The method which caught the exception.</param>
        /// <param name="asMessage">An extra message to be logged in the database along with the error.</param>
        public static void WriteExceptionToErrorLog(Exception ex, MethodBase method, string asMessage, bool abIsServiceCall = false)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session["RITMiniSite"] != null && HttpContext.Current.Session["RITMiniSite"].ToString() == Constants.S_YES)
				ErrorLog(ex, method);
			else
                ErrorLogDC.WriteExceptionToErrorLog(ex, method, asMessage, abIsServiceCall);
        }

		/// <summary>
		///		Logs the given error to the database.
		/// </summary>
		/// <param name="asMessage">The error message to be logged.</param>
		/// <param name="asCalingFunctionName">The name of the function which caught the error that is being logged.</param>
		/// <param name="aiUserId">The UserId of the current user.</param>
		public static void WriteExceptionToErrorLog(string asMessage, string asCalingFunctionName, int aiUserId)
        {
            ErrorLogDC.WriteExceptionToErrorLog(asMessage, asCalingFunctionName, aiUserId);
        }

		public static void ErrorLog(Exception ex, MethodBase method)
		{
			
				string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
				string sPathName = @"C:\MiniRIT\ErrorLogs\logfile.txt";
				//Directory.CreateDirectory(@"C:\MiniRIT\");
				Directory.CreateDirectory(@"C:\MiniRIT\ErrorLogs\"); 
				string sYear = DateTime.Now.Year.ToString();
				string sMonth = DateTime.Now.Month.ToString();
				string sDay = DateTime.Now.Day.ToString();

				string sErrorTime = sDay + "-" + sMonth + "-" + sYear;
                using (StreamWriter sw = new StreamWriter(sPathName, true))
                {
                    sw.WriteLine(HttpContext.Current.Session[Constants.S_SESSION_USER_ID] + Environment.NewLine + sLogFormat + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace);
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
		}

        public static void NotifyErrorLog(string asReplaceSingleQuoteString, int aiUserId, string asSchoolId)
        {
            ErrorLogDC.NotifyErrorLog(asReplaceSingleQuoteString,aiUserId,asSchoolId);
        }

		
        #endregion -- PUBLIC METHOD(s) --
    }
}
