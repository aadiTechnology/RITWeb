/*   Author		 : Vishal Shah
 *   Date		 : 10 Sept 2011
 *	 Description : This is the DataLayer for the Leaving Certificate Report configuration screen.
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{

	public class LeavingCertificateConfigDC
	{

		#region -- PUBLIC METHOD(s) --

		public static List<LeavingCertificateConfig> GetLeavingCertificateConfigList(int aiSchoolId)
		{
			List<LeavingCertificateConfig> olstLeavingCertificateConfig = null;          

            using (SQLServerDbUtility oSqlServerDBUtility = new SQLServerDbUtility())
            {
                oSqlServerDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                using (SqlDataReader oSqlReader = oSqlServerDBUtility.ExecuteStoredProcedureAndGetresult("usp_GetLCConfiguration"))
                {
                    if (oSqlReader.HasRows)
                    {
                        olstLeavingCertificateConfig = new List<LeavingCertificateConfig>();
                        while (oSqlReader.Read())
                        {
                            olstLeavingCertificateConfig.Add(new LeavingCertificateConfig
                            {
                                Id = Convert.ToInt32(oSqlReader["LCDetailsId"]),
                                Name = oSqlReader["LCDetailsName"].ToString(),
                                OriginalId = Convert.ToInt32(oSqlReader["OriginalLCDetailsId"]),
                                OriginalName = oSqlReader["OriginalLCDetailsName"].ToString(),
                                SchoolId = oSqlReader["SchoolId"] == DBNull.Value ? Convert.ToInt32(Constants.S_DEFAUL_SCHOOL_ID) : Convert.ToInt32(oSqlReader["SchoolId"]),
                                SortOrder = oSqlReader["SortOrder"].ToString(),
                                DefaultValue = oSqlReader["DefaultValue"].ToString(),
                                IsDefaultValueApplicable = oSqlReader["IsDefaultValueApplicable"].ToBool()
                            });
                        }
                    }
                }
            }

			return olstLeavingCertificateConfig;
		}

		public static void SaveLeavingCertificateConfig(int aiSchoolId, int aiUserId, string sConfigXML)
		{
			using(SQLServerDbUtility oSqlServerDBUtility = new SQLServerDbUtility())
			{
				oSqlServerDBUtility.AddParameter("SchoolId" , aiSchoolId, SqlDbType.Int);
				oSqlServerDBUtility.AddParameter("UserId"   , aiUserId  , SqlDbType.Int);
				oSqlServerDBUtility.AddParameter("ConfigXML", sConfigXML, SqlDbType.Xml);
				oSqlServerDBUtility.ExecuteStoredProcedureOnServer("usp_InsertLCReportConfigDetails");
			}
		}

		#endregion -- PUBLIC METHOD(s) --

	}

}