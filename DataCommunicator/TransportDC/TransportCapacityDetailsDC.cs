using System;
using System.Collections.Generic;
using SchoolEntities.Transport;
using System.Data.SqlClient;
using System.Data;

namespace DataCommunicator.TransportDC
{
    public class TransportCapacityDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private List<StandardwiseCapacityDetails> mlstStandards;

        #endregion

        #region Constructor(s)

        public TransportCapacityDetailsDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public TransportCapacityDetailsDC()
        {
        }

        #endregion

        public List<StandardwiseCapacityDetails> StandardwiseCount
        {
            get { return this.mlstStandards; }
        }
        
        
        #region Public Method(s)

        /// <summary>
        /// This method is used to get transport capacity details.
        /// </summary>
        /// <returns></returns>
        public List<TransportCapacityDetails> GetTransportCapacityDetails()
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", miAcademicYearId, SqlDbType.Int);
                
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportCapacityDetails]"))
                {
                    List<TransportCapacityDetails> lstTransportCapacity = new List<TransportCapacityDetails>();
                    while (oSqlDataReader.Read())
                    {
                        lstTransportCapacity.Add(new TransportCapacityDetails
                        {
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            RouteId = Convert.ToInt32(oSqlDataReader["RouteId"]),
                            RouteNo = Convert.ToInt32(oSqlDataReader["RouteNo"]),
                            RouteName = Convert.ToString(oSqlDataReader["RouteName"]),
                            VehicleNumber = Convert.ToString(oSqlDataReader["VehicleNumber"]),
                            VehicleCapacity = Convert.ToString(oSqlDataReader["VehicleCapacity"]),
                            PickUpCount_A = Convert.ToInt32(oSqlDataReader["PickUpCount_A"]),
                            PickUpCount_B = Convert.ToInt32(oSqlDataReader["PickUpCount_B"]),
                            PickUpCount_C = Convert.ToInt32(oSqlDataReader["PickUpCount_C"]),
                            DropCount_A = Convert.ToInt32(oSqlDataReader["DropCount_A"]),
                            DropCount_B = Convert.ToInt32(oSqlDataReader["DropCount_B"]),
                            DropCount_C = Convert.ToInt32(oSqlDataReader["DropCount_C"]),
                        });
                    }

                    oSqlDataReader.NextResult();
                    FillStandardwiseCount(oSqlDataReader);

                    return lstTransportCapacity;
                }
            }
        }

        #endregion

        private void FillStandardwiseCount(SqlDataReader aoSqlDataReader)
        {
            mlstStandards = new List<StandardwiseCapacityDetails>();
            while (aoSqlDataReader.Read())
            {
                mlstStandards.Add
                    (
                        new StandardwiseCapacityDetails
                        {
                            VehicleNumber = Convert.ToString(aoSqlDataReader["VehicleNumber"]),
                            JourneyTypeId = Convert.ToInt32(aoSqlDataReader["TravelerTypeId"]),
                            JourneyName = Convert.ToString(aoSqlDataReader["JourneyCategory"]),
                            StandardName = Convert.ToString(aoSqlDataReader["Standard_Name"]),
                            Count = Convert.ToInt32(aoSqlDataReader["UserCount"]),
                        }

                    );
            }
        }
    }
}
