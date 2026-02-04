/* Filename		:- ManagementFileSharingDC.cs
 * Author		:- Vishal Shah
 * Created On	:- 17-August-2011
 * Description	:- This is the Data Access Layer Class for the File Sharing feature for SuperAdmins (Management Role Group)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data.SqlClient;
using System.Data;
using Utility;

namespace DataCommunicator
{

	public class ManagementFileSharingDC
	{

		#region -- MEMBER(s) --

		public ManagementFileUploadDetails moFileUploadDetails;

		#endregion -- MEMBER(s) --


		#region -- CONSTRUCTOR(s) --

		public ManagementFileSharingDC()
		{
			moFileUploadDetails = new ManagementFileUploadDetails();
		}

		public ManagementFileSharingDC(int aiFileUploadId)
		{
			moFileUploadDetails = new ManagementFileUploadDetails();
			LoadFileDetails(aiFileUploadId);
		}

		#endregion -- CONSTRUCTOR(s) --


		#region -- PUBLIC METHOD(s) --

		public List<ManagementFileUploadDetails> GetAllFiles(int aiSchoolId, int aiAcademicYearId, int aiUserId, string sortExpression, int aiStartIndex, int aiEndIndex)
		{
			List<ManagementFileUploadDetails> lstFileUploadDetails = new List<ManagementFileUploadDetails>();
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{

				oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("SortExp", String.IsNullOrEmpty(sortExpression) ? String.Empty : "ORDER BY " + sortExpression, SqlDbType.NVarChar);
				oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
				oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

				using(SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetPagedFileUploadDetails"))
                {
                    if (oSqlDataReader != null && oSqlDataReader.HasRows)
                    {
                        while (oSqlDataReader.Read())
                        {
                            lstFileUploadDetails.Add(new ManagementFileUploadDetails
                            {
                                UploadId = Convert.ToInt32(oSqlDataReader["UploadId"]),
                                Title = oSqlDataReader["Title"].ToString(),
                                Description = oSqlDataReader["Description"].ToString(),
                                FilePath = oSqlDataReader["FilePath"].ToString(),
                                UploadedById = Convert.ToInt32(oSqlDataReader["UploadedById"]),
                                UploadedBy = oSqlDataReader["UploadedBy"].ToString(),
                                UploadDate = Convert.ToDateTime(oSqlDataReader["UploadDate"]),
                                UpdatedDate = oSqlDataReader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["UpdateDate"]),
                                IsRead = (aiUserId > 0) ? Convert.ToBoolean(oSqlDataReader["IsRead"]) : false
                            });
                        }
                    }
				}
			}
			return lstFileUploadDetails;
		}

		public int GetCount(int aiSchoolId, int aiAcademicYearId, int aiUserId)
		{
			int iCount = 0;

			string sSelectStatement = "SELECT COUNT(DISTINCT f.UploadId) FROM FileUploadDetails f INNER JOIN FileUploadUserDetails fu ON f.UploadId = fu.UploadId WHERE f.Is_Deleted = 0 AND fu.IsDeleted = 0 AND School_Id = " + aiSchoolId.ToString();
			if(aiAcademicYearId > 0) sSelectStatement += " AND Academic_Year_Id = " + aiAcademicYearId.ToString();
			if(aiUserId > 0) sSelectStatement += " AND fu.UserId = " + aiUserId.ToString();

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				iCount = oSQLServerDbUtility.ExecuteTransaction(sSelectStatement);

			return iCount;
		}

		public bool InsertFile(ManagementFileUploadDetails aoFileDetails)
		{
			bool bResult = false;

			using(SQLServerDbUtility oSQLServerDBUtility = new SQLServerDbUtility())
			{
				oSQLServerDBUtility.AddParameter("FileUploadId",	 aoFileDetails.UploadId,		SqlDbType.Int);
				oSQLServerDBUtility.AddParameter("Title",			 aoFileDetails.Title,			SqlDbType.NVarChar);
				oSQLServerDBUtility.AddParameter("Description",		 aoFileDetails.Description,		SqlDbType.NVarChar);
				oSQLServerDBUtility.AddParameter("FilePath",		 aoFileDetails.FilePath,		SqlDbType.NVarChar);
				oSQLServerDBUtility.AddParameter("UploadedById",	 aoFileDetails.UploadedById,	SqlDbType.Int);
				oSQLServerDBUtility.AddParameter("SelectedUserIds",  aoFileDetails.SelectedUserIds, SqlDbType.NVarChar);
				oSQLServerDBUtility.AddParameter("School_Id",		 aoFileDetails.SchoolId,		SqlDbType.Int);
				oSQLServerDBUtility.AddParameter("Academic_Year_Id", aoFileDetails.AcademicYearId,	SqlDbType.Int);

                using (SqlDataReader oSqlReader = oSQLServerDBUtility.ExecuteStoredProcedureAndGetresult("usp_InsertManagementFileUploadDetails"))
				{
					if(oSqlReader.HasRows && oSqlReader.Read()) {
						bResult = Convert.ToBoolean(oSqlReader["result"]);
					}
				}
			}

			return bResult;
		}

		public bool DeleteFile(int aiFileId)
		{
			bool bSuccess = false;

			string sSqlStatement = "UPDATE FileUploadDetails SET Is_Deleted = 1 WHERE UploadId = " + aiFileId.ToString();

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
				bSuccess = oSQLServerDbUtility.ExecuteTransaction(sSqlStatement) > 0;
			}

			return bSuccess;
		}

		public static void MarkAsRead(int aiFileUploadId, int aiUserId)
		{
			string sSqlStatement = String.Format("UPDATE FileUploadUserDetails SET IsRead = 1 WHERE UploadId = {0} AND UserId = {1}", aiFileUploadId, aiUserId);

			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
				oSQLServerDbUtility.ExecuteTransaction(sSqlStatement);
		}

		#endregion -- PUBLIC METHOD(s) --


		#region -- PRIVATE METHOD(s)--

		private void LoadFileDetails(int aiFileUploadId)
		{
			string sSqlStatement = "SELECT		 UploadId " +
												",Title " +
												",Description " +
												",FilePath " +
												",UserName AS UploadedBy " +
												",UploadDate " +
												",UpdateDate " +
									"FROM		FileUploadDetails f " +
												"INNER JOIN vw_user " +
												"ON UploadedById = User_Id " +
									"WHERE		f.Is_Deleted = 0 " +
												"AND f.UploadId = " + aiFileUploadId.ToString();
			using(SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
			{
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
                {
                    if (oSqlReader.Read())
                    {
                        moFileUploadDetails.UploadId = Convert.ToInt32(oSqlReader["UploadId"]);
                        moFileUploadDetails.Title = oSqlReader["Title"].ToString();
                        moFileUploadDetails.Description = oSqlReader["Description"].ToString();
                        moFileUploadDetails.FilePath = oSqlReader["FilePath"].ToString();
                        moFileUploadDetails.UploadedBy = oSqlReader["UploadedBy"].ToString();
                        moFileUploadDetails.UploadDate = Convert.ToDateTime(oSqlReader["UploadDate"]);
                        moFileUploadDetails.UpdatedDate = oSqlReader["UpdateDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(oSqlReader["UpdateDate"]);
                    }

                }

				List<string> oIdList = new List<string>();

				sSqlStatement = "SELECT UserId FROM FileUploadUserDetails WHERE IsDeleted = 0 AND UploadId = " + aiFileUploadId.ToString();
                using (SqlDataReader oSqlReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
                {
                    while (oSqlReader.Read())
                    {
                        oIdList.Add(oSqlReader["UserId"].ToString());
                    }
                    moFileUploadDetails.SelectedUserIds = String.Join(",", oIdList.ToArray());
                }
			}
		}

		#endregion -- PRIVATE METHOD(s) --

	}
}