using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data.SqlClient;
using Utility;
using System.Data;

namespace DataCommunicator
{
    public class RiteSchoolUsageDC
    {   
        public static List<ExecutionDate> GetAllDates()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<ExecutionDate> lstDates = new List<ExecutionDate>();
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetAllExecutionDates"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstDates.Add
                            (
                                new ExecutionDate
                                {
                                    Date = Convert.ToDateTime(oSqlDataReader["Date"])
                                }
                            );
                    }
                }
                return lstDates;
            }
        }

        public static List<UsageDetails> GetRitUsageDetails(int aiStartIndex, int aiEndIndex, string asSortExpression, string asDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<UsageDetails> lstUsagees = new List<UsageDetails>();
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Date", asDate, SqlDbType.NVarChar);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetRitUsageData"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstUsagees.Add
                            (
                                new UsageDetails
                                {
                                    QueryName = Convert.ToString(oSqlDataReader["QueryName"]),
                                    Legend = Convert.ToString(oSqlDataReader["Legend"]),
                                    TotalRows = Convert.ToInt32(oSqlDataReader["RecordCount"])
                                }
                            );
                    }
                }
                return lstUsagees;
            }
        }

        public static void GenerateReport()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("usp_GenerateRITUsageReport");
            }
        }
    }
}
