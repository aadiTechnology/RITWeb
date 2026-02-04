/* -----------------------------------------------------------------------
 *  FileName	: AccountsDC.cs
 *  Author		: Vishal B. Shah
 *  Date		: 10-March-2012
 *  Description : DAL for performing some basic accounts related functions.
 * -----------------------------------------------------------------------
 */

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using AccountsEntities;
using Utility;
using SchoolEntities.Dashboard;
using System;

namespace DataCommunicator
{

	/// <summary>
	/// DAL for performing some basic accounts related functions.
	/// </summary>
	public class AccountsDC
	{

		#region -- PUBLIC METHOD(s) --

		/// <summary>
		/// Returns all the Financial Years in the system.
		/// </summary>
		/// <returns>A List of FinancialYearMaster entity objects.</returns>
		public static List<FinancialYear> GetFinancialYears(int aiSchoolId = 0, bool abIsServiceCall = false)
		{
			var lstFinancialYears = new List<FinancialYear>();

            using (var oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
			using (var oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetFinancialYears"))
			{
				if (oSqlDataReader.HasRows)
				{
					var oGenericClass = new GenericClass<FinancialYear>();
					lstFinancialYears = oGenericClass.GetFilledObjectList(oSqlDataReader);
				}
			}

			return lstFinancialYears;
		}

		/// <summary>
		/// Gets user permissions for the accounts module.
		/// </summary>
		/// <returns></returns>
		public static List<UserPermissions> GetUserPermissions()
		{
			string sSqlStatement = "SELECT User_Id, CanApproveVoucher, CanCreateVoucher, CanSelfApprove, CanDeleteVoucher, CanEditOldFinancialYear, CASE Is_Locked WHEN N'Y' THEN 1 ELSE 0 END AS IsLocked FROM User_Master WHERE Is_Deleted = N'N'";
			
			using (var oSQLServerDbUtility = new SQLServerDbUtility())
			using (var oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSqlStatement))
			{
				var oGenricClass = new GenericClass<UserPermissions>();
				return oGenricClass.GetFilledObjectList(oSqlDataReader);
			}
		}

		/// <summary>
		/// This method is used to create next financial year.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <returns></returns>
		public static bool CreateFinancialYear(int aiSchoolId, int aiUserId, bool abMarkAsCurrent)
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
		public static bool UpdateFinancialYear(string asXml)
		{
			bool bResult = false;
			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			{
				oSqlServerDbUtility.AddParameter("FinancialYearDet", asXml, SqlDbType.Xml);
				using (SqlDataReader oReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_UpdateFinancialYearDetails"))
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
		/// Returns a List of Group entity objects to be used on the MIS Report.
		/// </summary>
		/// <param name="aiSchoolId"></param>
		/// <param name="aiFinancialYearId"></param>
		/// <returns></returns>
		public static List<MISReportGroup> GetGroupsForMISReport(int aiSchoolId, int aiFinancialYearId)
		{
			var lstGroups = new List<MISReportGroup>();

			using (var oSqlServerDbUtility = new SQLServerDbUtility())
			{
				oSqlServerDbUtility.AddParameter("SchoolId"		  , aiSchoolId		 , SqlDbType.Int);
				oSqlServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
				
				using (SqlDataReader oReader = oSqlServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetMISReport"))
				{
					var lstLedgers = new List<MISReportLedger>();
					while (oReader.Read())
						lstLedgers.Add(new MISReportLedger
										{
											Id			   = oReader["Id"].ToInt(),
											Name		   = oReader["Name"].ToString(),
											Group		   = new Group { Id = oReader["GroupId"].ToInt() },
											Budget		   = oReader["Budget"].ToDecimal(),
											MonthlyTotals  = new LedgerTotals
																{
																	January   = oReader["January"].ToDecimal(),
																	February  = oReader["February"].ToDecimal(),
																	March	  = oReader["March"].ToDecimal(),
																	April	  = oReader["April"].ToDecimal(),
																	May		  = oReader["May"].ToDecimal(),
																	June	  = oReader["June"].ToDecimal(),
																	July	  = oReader["July"].ToDecimal(),
																	August	  = oReader["August"].ToDecimal(),
																	September = oReader["September"].ToDecimal(),
																	October   = oReader["October"].ToDecimal(),
																	November  = oReader["November"].ToDecimal(),
																	December  = oReader["December"].ToDecimal()
																}
										});

					if (oReader.NextResult())
						while (oReader.Read())
						{
							int iGroupdId = oReader["Id"].ToInt();
							var ledgers = lstLedgers.Where(ledger => ledger.Group.Id == iGroupdId).ToList();
							lstGroups.Add(new MISReportGroup
											{
												Id				 = oReader["Id"].ToInt(),
												Name			 = oReader["Name"].ToString(),
												OriginalGroup	 = new Group { Id = oReader["OriginalId"].ToInt() },
												GroupNature		 = new GroupNature { Id = oReader["GroupNatureId"].ToInt() },
												MISReportLedgers = ledgers,
												MonthlyTotals	 = new LedgerTotals
																	{
													                    January	  = ledgers.Select(ledger => ledger.MonthlyTotals.January).Sum(),
													                    February  = ledgers.Select(ledger => ledger.MonthlyTotals.February).Sum(),
													                    March	  = ledgers.Select(ledger => ledger.MonthlyTotals.March).Sum(),
													                    April	  = ledgers.Select(ledger => ledger.MonthlyTotals.April).Sum(),
													                    May		  = ledgers.Select(ledger => ledger.MonthlyTotals.May).Sum(),
													                    June	  = ledgers.Select(ledger => ledger.MonthlyTotals.June).Sum(),
													                    July	  = ledgers.Select(ledger => ledger.MonthlyTotals.July).Sum(),
													                    August	  = ledgers.Select(ledger => ledger.MonthlyTotals.August).Sum(),
													                    September = ledgers.Select(ledger => ledger.MonthlyTotals.September).Sum(),
													                    October	  = ledgers.Select(ledger => ledger.MonthlyTotals.October).Sum(),
													                    November  = ledgers.Select(ledger => ledger.MonthlyTotals.November).Sum(),
													                    December  = ledgers.Select(ledger => ledger.MonthlyTotals.December).Sum()
																	}
											});
						}
				}
			}

			return lstGroups;
		}



        /// <summary>
        /// this method is used to get account widget related data
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <returns></returns>
        public static InflowOutflowSummary GetAccountInflowOutflowSummary(int aiSchoolId, int aiFinancialYearId, bool abIsServiceCall = false)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility(aiSchoolId, Constants.I_ZERO, Constants.I_ZERO, abIsServiceCall))
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("Accounts.usp_GetAccountsSummary"))
                {
                    InflowOutflowSummary oAccountInflowOutflowDetails = new InflowOutflowSummary();
                    while (oSqlDataReader.Read())
                    {
                        // Group nature id 1 = Inflow & 2 = outflow
                        if (oSqlDataReader["GroupNatureId"].ToString() == Constants.S_ONE)
                            oAccountInflowOutflowDetails.MonthwiseInflowAmount = GetMonthwiseInflowOutflowAmount(oSqlDataReader);
                        else if (oSqlDataReader["GroupNatureId"].ToString() == Constants.S_TWO)
                            oAccountInflowOutflowDetails.MonthwiseOutflowAmount = GetMonthwiseInflowOutflowAmount(oSqlDataReader);
                    }

                    return oAccountInflowOutflowDetails;
                }
            }
        }

        /// <summary>
        /// This function is used to get in flow out flow amount.
        /// </summary>
        /// <param name="oSqlDataReader">oSqlDataReader</param>
        /// <returns>double[]</returns>
        private static double[] GetMonthwiseInflowOutflowAmount(SqlDataReader oSqlDataReader)
        {
            int iFieldCount = oSqlDataReader.FieldCount;
            double[] arrayFlowAmount = new double[iFieldCount];

            arrayFlowAmount[0] = Convert.ToDouble(oSqlDataReader["April"]);
            arrayFlowAmount[1] = Convert.ToDouble(oSqlDataReader["May"]);
            arrayFlowAmount[2] = Convert.ToDouble(oSqlDataReader["June"]);
            arrayFlowAmount[3] = Convert.ToDouble(oSqlDataReader["July"]);
            arrayFlowAmount[4] = Convert.ToDouble(oSqlDataReader["August"]);
            arrayFlowAmount[5] = Convert.ToDouble(oSqlDataReader["September"]);
            arrayFlowAmount[6] = Convert.ToDouble(oSqlDataReader["October"]);
            arrayFlowAmount[7] = Convert.ToDouble(oSqlDataReader["November"]);
            arrayFlowAmount[8] = Convert.ToDouble(oSqlDataReader["December"]);
            arrayFlowAmount[9] = Convert.ToDouble(oSqlDataReader["January"]);
            arrayFlowAmount[10] = Convert.ToDouble(oSqlDataReader["February"]);
            arrayFlowAmount[11] = Convert.ToDouble(oSqlDataReader["March"]);

            return arrayFlowAmount;
        }
		
		#endregion -- PUBLIC METHOD(s) --

	}
}