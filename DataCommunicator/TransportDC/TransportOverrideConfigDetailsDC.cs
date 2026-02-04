using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using SchoolEntities.Transport;
using Utility;

namespace DataCommunicator.TransportDC
{
   public class TransportOverrideConfigDetailsDC : DataCommunicatorBaseDC
    {
        private int miSchoolId;
        private int miAcademicYearId;
        private int miUpdatedById;

        public TransportOverrideConfigDetailsDC(int aiSchoolId, int aiAcademicYearId, int aiUpdatedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
            this.miUpdatedById = aiUpdatedById;
        }

        public TransportOverrideConfigDetailsDC()
        {
        }

        public void Delete(int aiId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("Id", aiId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miUpdatedById, SqlDbType.Int);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_DeleteTransportOverrideConfigDetails]");
            }
        }

        public List<TransportOverrideConfigDetails> GetAll(int aiSchoolId, int aiAcademicYearId, string asRouteNo, string asRouteName, string asVehicleNo, string asJourneyName, string asName, string asSortExpression, int aiStartIndex, int aiEndIndex)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", aiAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("RouteNo", asRouteNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("RouteName", asRouteName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("VehicleNo", asVehicleNo, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("JourneyName", asJourneyName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Name", asName, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("SortExpression", asSortExpression, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("EndIndex", aiEndIndex, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportOverrideConfigDetails]"))
                {
                    List<TransportOverrideConfigDetails> lstTransportOverrideConfigDetails = new List<TransportOverrideConfigDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstTransportOverrideConfigDetails.Add(new TransportOverrideConfigDetails
                        {
                            Name = Convert.ToString(oSqlDataReader["Name"]),
                            StartDate =  (oSqlDataReader["StartDate"] == DBNull.Value? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["StartDate"])),
                            EndDate = (oSqlDataReader["EndDate"]== DBNull.Value? DateTime.MinValue : Convert.ToDateTime(oSqlDataReader["EndDate"])),
                            SourceRoute = Convert.ToString(oSqlDataReader["SourceRoute"]),
                            SourceVehicle = Convert.ToString(oSqlDataReader["SourceVehicle"]),
                            SourceJourney = Convert.ToString(oSqlDataReader["SourceJourney"]),
                            TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]),
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            WeekdayIds = Convert.ToString(oSqlDataReader["WeekdayIds"])
                        });
                    }
                    return lstTransportOverrideConfigDetails;
                }
            }
        }


    }
}
