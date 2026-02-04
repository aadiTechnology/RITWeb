using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
    public class HomeworkDailyLogDC
    {
        #region "Data Member"

		private int miSchoolId = 0;
		private int miAcademicYearId = 0;
		private int miUserId = 0;

		#endregion

		#region "Constructor"

        public HomeworkDailyLogDC()
        {
        }

        public HomeworkDailyLogDC(int aiSchoolId, int aiAcademicYearId, int aiUserId)
		{
			miSchoolId = aiSchoolId;
			miAcademicYearId = aiAcademicYearId;
			miUserId = aiUserId;
		}

		#endregion

        #region "Public Method"

        public bool ValidateHomeworkDailyLog(int aiSchoolId, int aiAcademicYearId, string aidate, int aiStdDivId, int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", aidate, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                SqlParameter oSqlParameter = oSQLServerDbUtility.AddParameter("IsValid", false, SqlDbType.Bit, ParameterDirection.Output);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_ValidateHomeworkDailyLog");
                return oSqlParameter.Value.ToBool();
            }
        }

        public void Save(HomeworkDailyLog aoHomeworkLog, string fname, int aiStdDivId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("HomeWorkLogId", aoHomeworkLog.Id, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("StdDivId", aiStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Date", aoHomeworkLog.Date, SqlDbType.Date);
                oSQLServerDbUtility.AddParameter("AttachmentName", fname, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[usp_SaveHomeworkDailyLog]");
            }
        }
        /// <summary>
        /// This method is used to delete homework.
        /// </summary>
        /// <param name="aiHomeworkLogId"></param>
        /// <param name="asReason"></param>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <returns></returns>
        public void Delete(int aiHomeworkLogId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiHomeworkLogId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_DeleteHomeworkDailyLog");
            }
        }

        public string Publish(string asHomeworkLogId, bool abIsPublish)
        {
            string sUserIds = string.Empty; 

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("LogId", asHomeworkLogId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", miUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("IsPublished", abIsPublish, SqlDbType.Bit);

                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_PublishUnpublishHomeworkDailylog"))
                {
                    if (oSqlReader.HasRows)
                    {
                        oSqlReader.Read();

                        if (oSqlReader["UserIds"] != DBNull.Value)
                        {
                            sUserIds = oSqlReader["UserIds"].ToString();  
                        }
                    }
                }
            }
            return sUserIds; 
        }


        public List<HomeworkDailyLog> GetAll(int aiSchoolId,int aiuserroleid, string asFilter, string asStdDivId, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StdDivId", asStdDivId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UserId", aiuserroleid, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllHomeworkDailyLogs"))
                {
                    List<HomeworkDailyLog> lstHomeworkDailyLog = new List<HomeworkDailyLog>();
                    while (oSqlDataReader.Read())
                        lstHomeworkDailyLog.Add(Sethomework(oSqlDataReader));
                    return lstHomeworkDailyLog;
                }
            }
        }

        private HomeworkDailyLog Sethomework(SqlDataReader aoSqlDataReader)
        {
            return new  HomeworkDailyLog
            {
                Id = Convert.ToInt32(aoSqlDataReader["Id"]),
                Date = Convert.ToDateTime(aoSqlDataReader["Date"]),
                AttachmentsName = Convert.ToString(aoSqlDataReader["AttchmentName"]),
                IsPublished=Convert.ToBoolean(aoSqlDataReader["IsPublished"]),
               TotalRows = aoSqlDataReader["TotalRows"].ToInt()
            };
        } 
        public HomeworkDailyLog Get(int aiHomeWorkLogId)
        {
            HomeworkDailyLog HomeworkDailyLog = null;
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Id", aiHomeWorkLogId, SqlDbType.Int);
                using (SqlDataReader oReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetHomeworkDailyLog"))
                {
                    if (oReader.HasRows)
                    {
                        oReader.Read();
                        HomeworkDailyLog = ReadObjectFromReader(oReader);
                    }


                }
                return HomeworkDailyLog;
            }
        }

        #endregion

        #region "Private Method"

        private HomeworkDailyLog ReadObjectFromReader(SqlDataReader aoReader)
        {
            return new HomeworkDailyLog()
            {
                AttachmentsName = aoReader["AttchmentName"].ToString(),
                Date = Convert.ToDateTime(aoReader["Date"]),
            };
        }

        #endregion
    }
}
