/* ---------------------------------------------------------------------------------------------------------------
 *	Filename	: TrialBalanceReportDC.cs
 *	Author		: Pravin Shinde
 *	Date		: 18-07-2013
 *	Description	: This class is used to get the details to show the details of trial balance report on dashboard.
 * ---------------------------------------------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AccountsEntities;
using Utility;
namespace DataCommunicator
{
    /// <summary>
    /// This class is used to get the details to show the details of trial balance report on dashboard.
    /// </summary>
    public class TrialBalanceReportDC
    {
        #region -- PUBLIC METHOD(s) --
        
        /// <summary>
        /// This method is used to return all the Monthly details of selected ledger for given date range.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiFinancialYearId"></param>
        /// <param name="adtStartDate"></param>
        /// <param name="adtEndDate"></param>
        /// <param name="aiLedgerId"></param>
        /// <returns></returns>
        public static List<MonthlyTrialBalance> GetMonthlyLedgerDetails(int aiSchoolId, int aiFinancialYearId, DateTime adtStartDate, DateTime adtEndDate, int aiLedgerId)
        {
            List<MonthlyTrialBalance> lstMonthlyTrialBalance = new List<MonthlyTrialBalance>();
            using (var oSqlDBUtility = new SQLServerDbUtility())
            {
                oSqlDBUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("FinancialYearId", aiFinancialYearId, SqlDbType.Int);
                oSqlDBUtility.AddParameter("StartDate", adtStartDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("EndDate", adtEndDate, SqlDbType.DateTime);
                oSqlDBUtility.AddParameter("LedgerId", aiLedgerId, SqlDbType.Int);
                using (SqlDataReader oSqlDataReader = oSqlDBUtility.ExecuteStoredProcedureAndGetresult("[Accounts].[usp_GetMonthwiseLedgerDetails]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstMonthlyTrialBalance.Add(new MonthlyTrialBalance
                                                       {
                                                           MonthId = oSqlDataReader["MonthId"].ToInt(),
                                                           MonthName = oSqlDataReader["MonthName"].ToString(),
                                                           StartDate = oSqlDataReader["StartDate"].ToDateTime(),
                                                           EndDate = oSqlDataReader["EndDate"].ToDateTime(),
                                                           oGroup = new Group
                                                                        {
                                                                            Debit = oSqlDataReader["Debit"].ToDecimal(),
                                                                            Credit = oSqlDataReader["Credit"].ToDecimal(),
                                                                            Ledgers = new List<Ledger>() { new Ledger { ClosingBlanace = oSqlDataReader["ClosingBalance"].ToDecimal(), } },
                                                                        }
                                                       });
                    }

                    return lstMonthlyTrialBalance;
                }
            }
        }

        #endregion -- PUBLIC METHOD(s) --
    }
}
