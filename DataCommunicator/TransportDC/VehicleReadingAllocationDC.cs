using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchoolEntities;
using SchoolEntities.Transport;
using System.Data.SqlClient;
using System.Data;

//using TransportEntities;


namespace DataCommunicator.TransportDC
{
    public class VehicleReadingAllocationDC
    {
        /// <summary>
        /// this method is used to save  transport reading allocation details.
        /// </summary>
        /// <param name="oTransportReadingAllocationDetails"></param>
        /// <returns></returns>
        public static void SaveTransportReadingAllocationDetails(string asXml)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VehicleReadingAllocationXml", asXml, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveVehicleReadingAllocationDetails]");
            }
        }

       /// <summary>
       ///  this method is used to get all vehicle numbers.
       /// </summary>
       /// <returns></returns>

        public List<TransportReadingAllocationDetails> GetAllVehicleNumbers(int aiAcademicYearId)
        {
            List<TransportReadingAllocationDetails> lstVehicleNumberDetails = new List<TransportReadingAllocationDetails>();

            string sSelectStatement = "SELECT VehicleId, VehicleNumber FROM Transport.VehicleMaster WHERE Is_Deleted = 0 AND Academic_Year_Id =  " + aiAcademicYearId;
            using (var oSQLServerDbUtility = new SQLServerDbUtility())

            using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteSqlStatementAndGetResults(sSelectStatement))
            {
                while (oSqlDataReader.Read())
                    lstVehicleNumberDetails.Add(new TransportReadingAllocationDetails
                    {
                        VehicleId = Convert.ToInt32(oSqlDataReader["VehicleId"]),
                        VehicleNumber = oSqlDataReader["VehicleNumber"].ToString()
                    });
            }

            return lstVehicleNumberDetails;
        }

        /// <summary>
        ///  this method is used to get vehicle reading allocation details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiAcademicYearId"></param>
        /// <param name="sSortExpression"></param>
        /// <param name="iEndIndex"></param>
        /// <param name="startRowIndex"></param>
        /// <returns></returns>
        public DataSet GetAllVehicleReadingAllocationDetails(int aiSchoolId, int aiAcademicYearId, string asSortExpression, string asFilter, string asFilterDate)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SortExp", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                if (asFilterDate != null && asFilterDate != string.Empty)
                    oSQLServerDbUtility.AddParameter("FilterDate", asFilterDate, SqlDbType.DateTime);
                return oSQLServerDbUtility.ExecuteStoredProcedureAndGetDataSet("[Transport].[usp_GetVehicleReadingAllocationDetails]");
            }

        }
      
        /// <summary>
        /// this method is used to delete vehicle reading allocation details.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="aiNoticeId"></param>
        /// <param name="aiUserId"></param>

        public static void DeleteVehicleAllocationDetails(int aiVehicleReadingAllocationId, int aiSchoolId, int aiAcademicYearId,int aiUpdatedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("VehicleReadingAllocationId", aiVehicleReadingAllocationId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", aiUpdatedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteVehicleAllocationDetails]");
            }
        }

        public void InsertAllocationDetails(string asAllocationDetails, int aiSchoolId, int aiAcademicYearId, int aiUserId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AllocationDetailsXML", asAllocationDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("UserId", aiUserId, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_ImportAllocationDetails]");
            }
        }

        public void InsertMaintenanceDetails(string asMaintenanceDetails, int aiSchoolId, int aiAcademicYearId, int aiInsertedById)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("MaintenanceDetailsXML", asMaintenanceDetails, SqlDbType.Xml);
                oSQLServerDbUtility.AddParameter("InsertedById", aiInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_ImportMaintenanceDetails]");
            }
        }

        public List<string> GetVehicleNumbers(int aiSchoolId, int aiAcademicYearId)
        {
            List<string> lstVehicleNo = new List<string>();

            using (var oSqlDbUtility = new SQLServerDbUtility())
            {
                oSqlDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSqlDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);

                using (SqlDataReader oReader = oSqlDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehicleNumbers]"))
                    if (oReader.HasRows)
                        while (oReader.Read())
                            lstVehicleNo.Add(oReader["VehicleNumber"].ToString());
            }
            return lstVehicleNo;
        }
    }
}
