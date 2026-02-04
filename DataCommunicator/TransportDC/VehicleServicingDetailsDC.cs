using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using SchoolEntities.Transport;
using System.Data.SqlClient;

namespace DataCommunicator.TransportDC
{
    public class VehicleServicingDetailsDC
    {
        #region Data Member(s)

        private int miSchoolId;
        private int miAcademicYearId;
        private int miInsertedById;

        #endregion

        #region Construstor(s)

        public VehicleServicingDetailsDC()
        {             
        }

        public VehicleServicingDetailsDC(int aiSchoolId, int aiAcademicYearId)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearId;
        }

        public VehicleServicingDetailsDC(int aiSchoolId, int aiAcademicYearID, int aiInsertedById)
        {
            this.miSchoolId = aiSchoolId;
            this.miAcademicYearId = aiAcademicYearID;
            this.miInsertedById = aiInsertedById;
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to Save the Vehicle servicing details in database.
        /// </summary>
        /// <param name="aoVehicleServicingDetails"></param>
        public void Save(VehicleServicingDetails aoVehicleServicingDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("AcademicYearId", this.miAcademicYearId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("InsertedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServicingId", aoVehicleServicingDetails.VehicleServicingId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("VehicleId", aoVehicleServicingDetails.VehicalId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServicingDate", aoVehicleServicingDetails.ServicingDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("NextServicingDate", aoVehicleServicingDetails.NextServicingDate, SqlDbType.DateTime);
                oSQLServerDbUtility.AddParameter("NotificaionDays", aoVehicleServicingDetails.NotificationDays, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("DocumentPhotos", aoVehicleServicingDetails.DocumnetPhoto, SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("Note", aoVehicleServicingDetails.ServicingNote, SqlDbType.NVarChar);

                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_SaveVehicleServicingDetails]");
            }
        }

        /// <summary>
        /// This method is used to Get all PUC PUC details for List view.
        /// </summary>
        /// <param name="aiSchoolId"></param>
        /// <param name="asSortExpression"></param>
        /// <param name="aiStartIndex"></param>
        /// <param name="aiEndIndex"></param>
        /// <param name="asFilter"></param>
        /// <returns></returns>
        public List<VehicleServicingDetails> GetAll(int aiSchoolId, bool abShowOldRecord, string asSortExpression, int aiStartIndex, int aiEndIndex, string asFilter)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ShowOldRecord", abShowOldRecord, SqlDbType.Bit);                
                oSQLServerDbUtility.AddParameter("SortExpr", "ORDER BY " + asSortExpression.ToString(), SqlDbType.NVarChar);
                oSQLServerDbUtility.AddParameter("prm_StartIndex", aiStartIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("prm_EndIndex", aiEndIndex, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("Filter", asFilter, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetAllVehicleServicingDetails]"))
                    return FillVehicleServicingDetails(oSqlDataReader);
            }
        }

        /// <summary>
        /// This method is used to get vehicle PUC details for Edit.
        /// </summary>
        /// <param name="aiPUCId"></param>
        /// <returns></returns>
        public VehicleServicingDetails Get(int aiServicingId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                VehicleServicingDetails oVehicleServicingDetails = new VehicleServicingDetails();

                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServicingId", aiServicingId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetVehicleServicingDetails]"))
                    if (oSqlDataReader.Read())
                    {
                        oVehicleServicingDetails.VehicleServicingId = Convert.ToInt32(oSqlDataReader["Id"]);
                        oVehicleServicingDetails.VehicalId = Convert.ToInt32(oSqlDataReader["VehicleId"]);
                        oVehicleServicingDetails.ServicingDate = Convert.ToDateTime(oSqlDataReader["ServicingDate"]);
                        oVehicleServicingDetails.NextServicingDate = Convert.ToDateTime(oSqlDataReader["NextServicingDate"]);
                        oVehicleServicingDetails.NotificationDays = Convert.ToInt32(oSqlDataReader["NotificationDays"]);

                        if (oSqlDataReader["Note"] != DBNull.Value)
                            oVehicleServicingDetails.ServicingNote = Convert.ToString(oSqlDataReader["Note"]);
                    }
                return oVehicleServicingDetails;
            }
        }

        /// <summary>
        /// This method is used to delete PUC details.
        /// </summary>
        /// <param name="aiVehiclePUCId"></param>
        public List<string> Delete(int aiServicingId)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<string> lstFileNames = new List<string>();
                oSQLServerDbUtility.AddParameter("SchoolId", this.miSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("UpdatedById", this.miInsertedById, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("ServicingId", aiServicingId, SqlDbType.Int);

                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_DeleteVehicleServicingDetails]"))
                { 
                    while(oSqlDataReader.Read())
                        lstFileNames.Add(oSqlDataReader["FileName"].ToString());
                }
                return lstFileNames;
            }
        }

        #endregion

        #region Public Method(s)

        /// <summary>
        /// This method is used to fill the Vehicle details.
        /// </summary>
        /// <param name="oSqlDataReader"></param>
        /// <returns></returns>
        private List<VehicleServicingDetails> FillVehicleServicingDetails(SqlDataReader oSqlDataReader)
        {
            List<VehicleServicingDetails> lstVehicleServicingDetails = new List<VehicleServicingDetails>();
            while (oSqlDataReader.Read())
            {
                VehicleServicingDetails oVehicleServicingDetails = new VehicleServicingDetails();
                oVehicleServicingDetails.VehicalId = Convert.ToInt32(oSqlDataReader["VehicleId"]);
                oVehicleServicingDetails.VehicleServicingId = Convert.ToInt32(oSqlDataReader["Id"]);
                oVehicleServicingDetails.VehicalNumber = Convert.ToString(oSqlDataReader["VehicleNumber"]);
                oVehicleServicingDetails.ServicingDate = Convert.ToDateTime(oSqlDataReader["ServicingDate"]);
                oVehicleServicingDetails.NextServicingDate = Convert.ToDateTime(oSqlDataReader["NextServicingDate"]);
                oVehicleServicingDetails.NotificationDays = Convert.ToInt32(oSqlDataReader["NotificationDays"]);
                oVehicleServicingDetails.TotalRows = Convert.ToInt32(oSqlDataReader["TotalRows"]);
                oVehicleServicingDetails.IsFileExists = Convert.ToBoolean(oSqlDataReader["IsFileExists"]);
                oVehicleServicingDetails.IsLocked = Convert.ToBoolean(oSqlDataReader["IsLocked"]);
                oVehicleServicingDetails.IsOldRecord = Convert.ToBoolean(oSqlDataReader["IsOldRecord"]);

                lstVehicleServicingDetails.Add(oVehicleServicingDetails);
            }
            return lstVehicleServicingDetails;
        }


        #endregion

        #region Call from Transport Schedular

        public static void UpdateNotificationDetails(int aiSchoolId, string asNotificationDetails)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NotificationDetails", asNotificationDetails, SqlDbType.Xml);
                oSQLServerDbUtility.ExecuteStoredProcedureOnServer("[Transport].[usp_UpdateNotificationDetails]");
            }
        }

        public static List<TransportNotificationDetails> GetTransportDetails(int aiSchoolId, string asNotificationUserIds)
        {
            using (SQLServerDbUtility oSQLServerDbUtility = new SQLServerDbUtility())
            {
                List<TransportNotificationDetails> lstTransportDetails = new List<TransportNotificationDetails>();
                oSQLServerDbUtility.AddParameter("SchoolId", aiSchoolId, SqlDbType.Int);
                oSQLServerDbUtility.AddParameter("NotificationUserIds", asNotificationUserIds, SqlDbType.NVarChar);
                using (SqlDataReader oSqlDataReader = oSQLServerDbUtility.ExecuteStoredProcedureAndGetresult("[Transport].[usp_GetTransportNotificationDetails]"))
                {
                    while (oSqlDataReader.Read())
                    {
                        lstTransportDetails.Add(new TransportNotificationDetails
                        {
                            CategoryId = Convert.ToInt32(oSqlDataReader["CategoryId"]),
                            Id = Convert.ToInt32(oSqlDataReader["Id"]),
                            ExpiryDate = Convert.ToDateTime(oSqlDataReader["ExpiryDate"]),
                            MobileNos = oSqlDataReader["MobileNos"].ToString(),
                            UserNames = oSqlDataReader["UserNames"].ToString(),
                            AcademicYearId = Convert.ToInt32(oSqlDataReader["AcademicYearId"])
                        });
                    }

                    return lstTransportDetails;
                }
            }
        } 

        #endregion
    }
}
