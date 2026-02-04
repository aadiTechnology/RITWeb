using System.Data;
using System.Data.SqlClient;
using SchoolEntities;
using Utility;

namespace DataCommunicator
{
	/// <summary>
	/// This class is used to create and update financial year.
	/// </summary>
	public class FinancialYearDC
	{
		/// <summary>
		/// This method is used to create next financial year.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public bool CreateFinancialYear(int aiSchoolId, int aiUserId, bool abMarkAsCurrent)
		{
			bool bResult = false;
			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			{
				oSqlServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
				oSqlServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
				oSqlServerDbUtility.AddParameter("MarkAsCurrentYear", abMarkAsCurrent, SqlDbType.Bit);

                using (SqlDataReader oReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_CreateFinancialYear"))
				{
					if (oReader.Read())
					{
						if (oReader["Result"].ToInt() == 1)
							bResult = true;
					}
				}
			}

			return bResult;
		}

		/// <summary>
		/// This method is used to update financial year details.
		/// </summary>
		/// <param name="asXml"></param>
		/// <returns></returns>
		public void UpdateFinancialYearDetails(string asXml, int aiUserId)
		{	
			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			{
				oSqlServerDbUtility.AddParameter("FinancialYearDet", asXml, SqlDbType.Xml);
				oSqlServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_UpdateFinancialYearDetails");				
			}			
		}

	}
}
