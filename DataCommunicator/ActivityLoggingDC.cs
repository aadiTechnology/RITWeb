/* ------------------------------------------------------------------------------------------------
 *	Author		: Vishal Shah
 *	Date		: 20-Sept-2011
 *	Description : This is the Data Access Layer for logging Page request details and database
 *				  activity details.
 * ------------------------------------------------------------------------------------------------
 */

using System.Data;
using SchoolEntities;
using System.Collections.Generic;
using Utility;

namespace DataCommunicator
{
    public class ActivityLoggingDC
    {
        static List<PageRequestLog> mlstPageRequestLogs = new List<PageRequestLog>();
        static object pageRequestLogCacheLock = new object();

        /// <summary>
        ///		Logs the given PageRequest details and activity log to the database.
        /// </summary>
        /// <param name="aoPageRequestLog">A PageRequestLog object representing the page request details.</param>
        /// <param name="asPageRequestData">A string representing any request data to be logged. It can be null.</param>
        /// <param name="asActivityLogXML">Database activity details in xml represented as a string.</param>
        public static void LogActivity(PageRequestLog aoPageRequestLog, string asPageRequestData, string asActivityLogXML, bool abIsServiceCall = false)
        {
            //If current call is service call then add request log into cache and insert activity in bulk.
            if (abIsServiceCall)
            {
                lock (pageRequestLogCacheLock)
                {
                    //Add current request in the list
                    mlstPageRequestLogs.Add(aoPageRequestLog);

                    //If count of page requests is greater than or equal to maximum cache count then need to insert into database.
                    if (mlstPageRequestLogs.Count >= Constants.I_ACTIVITY_LOG_CACHE_COUNT)
                    {
                        /* Bulk insert*/
                        string sPageRequestLogXML = CommonUtility.GetXMLForList(mlstPageRequestLogs);
                        using (var oSqlDbUtility = new SQLServerDbUtility())
                        {
                            oSqlDbUtility.AddParameter("PageRequestLogXML", sPageRequestLogXML, SqlDbType.Xml);
                            oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_InsertPageRequestLogs");
                        }
                        mlstPageRequestLogs.Clear();
                    }                    
                }
            }
            else
            {
                using (var oSqlDbUtility = new SQLServerDbUtility())
                {
                    oSqlDbUtility.AddParameter("SchoolId", aoPageRequestLog.SchoolId, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("AcademicYrId", aoPageRequestLog.AcademicYearId, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("UserId", aoPageRequestLog.UserId, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("SessionId", aoPageRequestLog.SessionId, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("IPAddress", aoPageRequestLog.IPAddress, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("Browser", aoPageRequestLog.Browser, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("BrowserVersion", aoPageRequestLog.BrowserVersion, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("Page", aoPageRequestLog.Page, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("QueryString", aoPageRequestLog.QueryString, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("IsPostBack", aoPageRequestLog.IsPostBack, SqlDbType.Bit);
                    oSqlDbUtility.AddParameter("RequestData", asPageRequestData, SqlDbType.NVarChar);
                    oSqlDbUtility.AddParameter("ExecutionTime", aoPageRequestLog.ExecutionTime, SqlDbType.Int);
                    oSqlDbUtility.AddParameter("InsertDate", aoPageRequestLog.InsertDate, SqlDbType.DateTime);
                    oSqlDbUtility.AddParameter("ActivityXML", asActivityLogXML, SqlDbType.Xml);

                    oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_InsertActivityLog");
                }
            }
        }

        /// <summary>
        /// This method is used to Clear ActivityLog data and PagerequestLog data.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void ClearLogs(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_ClearLogData");
            }
        }

        /// <summary>
        /// This method is used to Clear Session log data.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        public static void ClearSession(int aiSchoolId)
        {
            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.ExecuteStoredProcedureOnServer("usp_ClearSessionLogData");
            }
        }

    }
}
