using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using System.Data;
using System.Data.SqlClient;
using Utility;
using SchoolEntities.Transport;

namespace DataCommunicator
{
    public class VehicleMaintenanceExpensesDC
    {
        /// <summary>
        /// This method is used to Get Vehicle Numbers details.
        /// </summary>
        /// <returns></returns>
        public static List<VehicleMaintenanceExpenses> GetVehicleNumbers(int aiAcademicYearId)
        {
            List<VehicleMaintenanceExpenses> olstVehicleMaintenanceExpenses = new List<VehicleMaintenanceExpenses>();
            string sSQLStatemente = "SELECT VehicleId, VehicleType, VehicleNumber FROM [Transport].VehicleMaster WHERE Is_Deleted = 0 AND Academic_Year_Id =  " + aiAcademicYearId;

            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSQLStatemente))
                {
                    while (oSqlDataReader.Read())
                    {
                        VehicleMaintenanceExpenses oVehicleMaintenanceExpenses = new VehicleMaintenanceExpenses
                        {
                            VehicleId = oSqlDataReader["VehicleId"].ToInt(),
                            VehicleNumber = oSqlDataReader["VehicleNumber"].ToString(),
                            VehicleType = oSqlDataReader["VehicleType"].ToString()
                        };
                        olstVehicleMaintenanceExpenses.Add(oVehicleMaintenanceExpenses);
                    }
                }
            }
            return olstVehicleMaintenanceExpenses;
        }

        /// <summary>
        /// This method is used to Get Vehicle Maintenance Expenses details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="asSortExpression"></param>
        /// <returns></returns>
        public DataSet GetAllVehicleExpensesDetails(int aiSchoolId, int aiAcademicYearId, string asMainFilter, string asSortExpression)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asMainFilter, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[Transport].[usp_GetAllVehicleMaintenanceExpensesDetails]");
            }
        }

        /// <summary>
        /// This method is used to Get Vehicle Maintenance Expenses Parts Used details of the selected Vehicle Maintenance Expense.
        /// </summary>
        /// <param name="aiVehicleMaintenanceExpensesID"></param>
        /// <returns></returns>
        public DataSet GetAllVehicleExpensesPartsUsedDetails(int aiVehicleMaintenanceExpensesID)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VehicleMaintenanceExpensesId", aiVehicleMaintenanceExpensesID, SqlDbType.Int);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[Transport].[usp_GetAllVehicleMaintenancePartsUsed]");
            }
        }

        /// <summary>
        /// This method is used to save and update Vehicle Maintenance Expenses details and Vehicle Maintenance Expenses Parts Used detail.
        /// </summary>
        /// <param name="asXml"></param>
        /// <param name="xXmlVehiclePartsUsed"></param>
        public static void SaveUpdateVehicleMaintenanceExpenses(string asXml, string xXmlVehiclePartsUsed)
        {
            using (var oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VehicleMaintenanceExpensesXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("VehicleMaintenancePartsUsedXml", xXmlVehiclePartsUsed, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveUpdateVehicleMaintenanceExpenses]");
            }
        }

        /// <summary>
        /// This method is used to delete Vehicle Maintenance Expenses details and Vehicle Maintenance Expenses Parts Used details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiVehicleMaintenanceExpensesId"></param>
        /// <param name="aiUserId"></param>
        public static void Delete(int aiSchoolId, int aiVehicleMaintenanceExpensesId, int aiUserId, int aiAcademicYearId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VehicleMaintenanceExpensesId", aiVehicleMaintenanceExpensesId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteVehicleMaintenanceExpenses]");
            }
        }

        /// <summary>
        /// This method is used to get maintenance type list.
        /// </summary>
        /// <returns></returns>
        public List<Maintanance> GetMaintenanceTypeList()
        {
            List<Maintanance> lstMaintenance = new List<Maintanance>();
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("usp_GetVehicleMaintenanceTypeList"))
                {
                    while (oSqlDataReader.Read())
                    {
                        Maintanance oMaintanance = new Maintanance();
                        oMaintanance.MaintenanceType = oSqlDataReader["MaintenanceType"].ToString();
                        oMaintanance.MaintenanceTypeId = oSqlDataReader["MaintenanceTypeId"].ToInt();

                        lstMaintenance.Add(oMaintanance);
                    }
                }
                return lstMaintenance;
            }
        }
    }
}
