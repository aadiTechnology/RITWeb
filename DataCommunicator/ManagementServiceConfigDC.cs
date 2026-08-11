// -----------------------------------------------------------------------
//	FileName	: ManagementServiceConfigDC.cs
//	Author		: Vishal Shah
//	Date		: 8-Nov-2012
//	Description	: Provides DAL methods to fetch details from the database.
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Management.Entities;
using Utility;
using System.Data;
using System.Configuration;

namespace DataCommunicator
{

	/// <summary>
	///		DataAccessLayer for the management service.
	/// </summary>
	public class ManagementServiceConfigDC
	{
		#region -- MEMBER(s) --

		private int miSchoolId;

		#endregion -- MEMBER(s) --

		#region -- CONSTRUCTOR(s) --

		/// <summary>
		///		Initializes member variables.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		public ManagementServiceConfigDC(int aiSchoolId)
		{
			miSchoolId = aiSchoolId;
		}

		#endregion -- CONSTRUCTOR(s) --

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		///		Gets the token for the given school.
		/// </summary>
		/// <returns></returns>
		public string GetToken()
		{
			string sSqlStatement = String.Format("SELECT Token from ManagementServiceConfiguration WHERE SchoolId = {0} AND IsDeleted = 0", miSchoolId);

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
				return oSqlServerDbUtility.PerformStringQueryOnSqlServer(sSqlStatement);
		}

		/// <summary>
		///		Returns all associated/integrated schools.
		/// </summary>
		/// <returns></returns>
		public List<SchoolMISDetails> GetAssociatedSchools()
		{
			List<SchoolMISDetails> lstMgtConfig = null;

			string sSqlStatement = String.Format("SELECT Id, SchoolId, SchoolName, SchoolShortName, ServiceURL, Token, IsDefault FROM dbo.ManagementServiceConfiguration WHERE IsDeleted = 0");

			using (var oSqlServerDbUtlity = new SQLServerDbUtility())
			using (SqlDataReader oReader = oSqlServerDbUtlity.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				if (oReader.HasRows)
				{
					lstMgtConfig = new List<SchoolMISDetails>();
					while (oReader.Read())
						lstMgtConfig.Add(new SchoolMISDetails
							{
								Id				= oReader["Id"].ToInt(),
								SchoolId		= oReader["SchoolId"].ToInt(),
								SchoolName		= oReader["SchoolName"].ToString(),
								SchoolShortName = oReader["SchoolShortName"].ToString(),
								ServiceURL		= oReader["ServiceURL"].ToString(),
								Token			= oReader["Token"].ToString(),
								IsDefault		= oReader["IsDefault"].ToBool()
							});
				}
			}

			return lstMgtConfig;
		}

		#endregion -- PUBLIC METHOD(s) --

        public static DataTable GetManagementUserInfo(int aiSchoolId, int aiUserId)
        {
            string connectionString = "Data Source= " + ConfigurationManager.AppSettings["SchoolLocationsDataSource"] + "; Database=" + ConfigurationManager.AppSettings["SchoolLocationsDataBaseName"]
                           + "; User ID=" + ConfigurationManager.AppSettings["SchoolLocationsUserId"] + "; Password=" + ConfigurationManager.AppSettings["SchoolLocationsPassword"];

            string sSchoolDBName = string.Empty;
            using (SqlConnection oSqlConnection = new SqlConnection(connectionString))
            {
                string command = string.Empty;
                if (aiSchoolId != 0)                
                    command = "SELECT * FROM BiometricSchools WHERE SchoolID = " + aiSchoolId + " AND IsDeleted = 0";

                SqlCommand oSqlCommand = new SqlCommand(command, oSqlConnection);
                oSqlConnection.Open();

                string sConnectionString = string.Empty;
                using (SqlDataReader oSqlDataReader = oSqlCommand.ExecuteReader())
                {
                    if (oSqlDataReader.Read())
                    {
                        sSchoolDBName = oSqlDataReader["SchoolDatabaseName"].ToString();
                    }
                }
            }

            if (sSchoolDBName != string.Empty)
            {
                string sNewConnectionString = Constants.S_CONNECTION_STRING.Replace(ConfigurationManager.AppSettings["reportdatabasename"], sSchoolDBName);
                using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(sNewConnectionString))
                {
                    oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                    oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                    return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataTable("usp_GetManagementUserInfo");
                }
            }
            else
                return null;
        }
    }
}
